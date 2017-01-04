/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The e-mobility provider is not only the main contract party of the EV driver,
    /// the e-mobility provider also takes care of the EV driver master data,
    /// the authentication and autorisation process before charging and for the
    /// billing process after charging.
    /// The e-mobility provider provides the EV drivere one or multiple methods for
    /// authentication (e.g. based on RFID cards, login/passwords, client certificates).
    /// The e-mobility provider takes care that none of the provided authentication
    /// methods can be misused by any entity in the ev charging process to track the
    /// ev driver or its behaviour.
    /// </summary>
    public class NavigationProvider : AEMobilityEntity<NavigationProvider_Id>,
                                      IRemoteNavigationProvider,
                                      IEquatable <NavigationProvider>,
                                      IComparable<NavigationProvider>,
                                      IComparable
    {

        #region Data

        /// <summary>
        /// The default max size of the admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        /// <summary>
        /// The default max size of the status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        #endregion

        #region Properties

        //public Authorizator_Id AuthorizatorId { get; }

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the EVSE Operator.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {
                return _Name;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_Name != value)
                    SetProperty(ref _Name, value);

            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of the EVSE Operator.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {
                return _Description;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_Description != value)
                    SetProperty(ref _Description, value);

            }

        }

        #endregion

        #region Logo

        private String _Logo;

        /// <summary>
        /// The logo of this evse operator.
        /// </summary>
        [Optional]
        public String Logo
        {

            get
            {
                return _Logo;
            }

            set
            {
                if (_Logo != value)
                    SetProperty(ref _Logo, value);
            }

        }

        #endregion

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of the operators headquarter.
        /// </summary>
        [Optional]
        public Address Address
        {

            get
            {
                return _Address;
            }

            set
            {

                if (value == null)
                    value = new Address();

                if (_Address != value)
                    SetProperty(ref _Address, value);

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this operator.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {
                return _GeoLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(new Latitude(0), new Longitude(0));

                if (_GeoLocation != value)
                    SetProperty(ref _GeoLocation, value);

            }

        }

        #endregion

        #region Telephone

        private String _Telephone;

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        [Optional]
        public String Telephone
        {

            get
            {
                return _Telephone;
            }

            set
            {
                if (_Telephone != value)
                    SetProperty(ref _Telephone, value);
            }

        }

        #endregion

        #region EMailAddress

        private String _EMailAddress;

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        [Optional]
        public String EMailAddress
        {

            get
            {
                return _EMailAddress;
            }

            set
            {
                if (_EMailAddress != value)
                    SetProperty(ref _EMailAddress, value);
            }

        }

        #endregion

        #region Homepage

        private String _Homepage;

        /// <summary>
        /// The homepage of this evse operator.
        /// </summary>
        [Optional]
        public String Homepage
        {

            get
            {
                return _Homepage;
            }

            set
            {
                if (_Homepage != value)
                    SetProperty(ref _Homepage, value);
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        [Optional]
        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {
                if (_HotlinePhoneNumber != value)
                    SetProperty(ref _HotlinePhoneNumber, value);
            }

        }

        #endregion


        #region DataLicense

        private List<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public IEnumerable<DataLicense> DataLicenses
            => _DataLicenses;

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<NavigationProviderAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<NavigationProviderAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<NavigationProviderAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<NavigationProviderStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<NavigationProviderStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<NavigationProviderStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion


        public NavigationProviderPriority Priority { get; set; }

        #endregion

        #region Links

        /// <summary>
        /// The remote e-mobility provider.
        /// </summary>
        public IRemoteNavigationProvider  RemoteNavigationProvider    { get; }


        /// <summary>
        /// The parent roaming network.
        /// </summary>
        public RoamingNetwork            RoamingNetwork             { get; }

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE data will be send upstream.
        ///// </summary>
        //public event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE data had been sent upstream.
        ///// </summary>
        //public event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE status will be send upstream.
        ///// </summary>
        //public event OnPushEVSEStatusRequestDelegate OnPushEVSEStatusRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE status had been sent upstream.
        ///// </summary>
        //public event OnPushEVSEStatusResponseDelegate OnPushEVSEStatusResponse;

        #endregion


        #region OnReserve... / OnReserved...

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnReserveEVSERequestDelegate              OnReserveEVSE;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnReserveEVSEResponseDelegate             OnEVSEReserved;

        #endregion

        #region OnRemote...Start / OnRemote...Started

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartEVSERequestDelegate               OnRemoteEVSEStart;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartEVSEResponseDelegate             OnRemoteEVSEStarted;

        #endregion

        #region OnRemote...Stop / OnRemote...Stopped

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        public event OnRemoteStopEVSERequestDelegate                OnRemoteEVSEStop;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        public event OnRemoteStopEVSEResponseDelegate             OnRemoteEVSEStopped;

        #endregion

        // CancelReservation

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility (service) provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="Id">The unique e-mobility provider identification.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal NavigationProvider(NavigationProvider_Id                    Id,
                                   RoamingNetwork                          RoamingNetwork,
                                   Action<NavigationProvider>               Configurator                    = null,
                                   RemoteNavigationProviderCreatorDelegate  RemoteNavigationProviderCreator  = null,
                                   I18NString                              Name                            = null,
                                   I18NString                              Description                     = null,
                                   NavigationProviderPriority               Priority                        = null,
                                   NavigationProviderAdminStatusType        AdminStatus                     = NavigationProviderAdminStatusType.Available,
                                   NavigationProviderStatusType             Status                          = NavigationProviderStatusType.Available,
                                   UInt16                                  MaxAdminStatusListSize          = DefaultMaxAdminStatusListSize,
                                   UInt16                                  MaxStatusListSize               = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(NavigationProvider),  "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this.RoamingNetwork              = RoamingNetwork;

            this._Name                        = Name        ?? new I18NString();
            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new List<DataLicense>();

            this.Priority                     = Priority    ?? new NavigationProviderPriority(0);

            this._AdminStatusSchedule         = new StatusSchedule<NavigationProviderAdminStatusType>();
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule              = new StatusSchedule<NavigationProviderStatusType>();
            this._StatusSchedule.Insert(Status);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteNavigationProvider = RemoteNavigationProviderCreator?.Invoke(this);

        }

        #endregion



        #region Incoming requests from the roaming network

        //#region Receive incoming EVSEData

        //#region UpdateEVSEData                   (EVSE,             ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateEVSEData(EVSE                 EVSE,
        //                                    ActionType           ActionType,

        //                                    DateTime?            Timestamp          = null,
        //                                    CancellationToken?   CancellationToken  = null,
        //                                    EventTracking_Id     EventTrackingId    = null,
        //                                    TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (EVSE == null)
        //        throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateEVSEData                   (EVSEs,            ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given enumeration of EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateEVSEData(IEnumerable<EVSE>    EVSEs,
        //                                    ActionType           ActionType,

        //                                    DateTime?            Timestamp          = null,
        //                                    CancellationToken?   CancellationToken  = null,
        //                                    EventTracking_Id     EventTrackingId    = null,
        //                                    TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (EVSEs == null)
        //        throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateChargingStationData        (ChargingStation,  ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given charging station.
        ///// </summary>
        ///// <param name="ChargingStation">A charging station.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateChargingStationData(ChargingStation      ChargingStation,
        //                                              ActionType           ActionType,

        //                                              DateTime?            Timestamp          = null,
        //                                              CancellationToken?   CancellationToken  = null,
        //                                              EventTracking_Id     EventTrackingId    = null,
        //                                              TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStation == null)
        //        throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateChargingStationData        (ChargingStations, ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given charging stations.
        ///// </summary>
        ///// <param name="ChargingStations">An enumeration of charging stations.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateChargingStationData(IEnumerable<ChargingStation>  ChargingStations,
        //                                              ActionType                    ActionType,

        //                                              DateTime?                     Timestamp          = null,
        //                                              CancellationToken?            CancellationToken  = null,
        //                                              EventTracking_Id              EventTrackingId    = null,
        //                                              TimeSpan?                     RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStations == null)
        //        throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateChargingPoolData           (ChargingPool,     ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given charging pool.
        ///// </summary>
        ///// <param name="ChargingPool">A charging pool.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateChargingPoolData(ChargingPool         ChargingPool,
        //                                            ActionType           ActionType,

        //                                            DateTime?            Timestamp          = null,
        //                                            CancellationToken?   CancellationToken  = null,
        //                                            EventTracking_Id     EventTrackingId    = null,
        //                                            TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingPool == null)
        //        throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateChargingPoolData           (ChargingPools,    ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given charging pools.
        ///// </summary>
        ///// <param name="ChargingPools">An enumeration of charging pools.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateChargingPoolData(IEnumerable<ChargingPool>  ChargingPools,
        //                                            ActionType                 ActionType,

        //                                            DateTime?                  Timestamp          = null,
        //                                            CancellationToken?         CancellationToken  = null,
        //                                            EventTracking_Id           EventTrackingId    = null,
        //                                            TimeSpan?                  RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingPools == null)
        //        throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateChargingStationOperatorData(EVSEOperator,     ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given Charging Station Operator.
        ///// </summary>
        ///// <param name="ChargingStationOperator">An Charging Station Operator.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateChargingStationOperatorData(ChargingStationOperator  ChargingStationOperator,
        //                                                       ActionType               ActionType,

        //                                                       DateTime?                Timestamp          = null,
        //                                                       CancellationToken?       CancellationToken  = null,
        //                                                       EventTracking_Id         EventTrackingId    = null,
        //                                                       TimeSpan?                RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStationOperator == null)
        //        throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateChargingStationOperatorData(EVSEOperators,    ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given Charging Station Operators.
        ///// </summary>
        ///// <param name="ChargingStationOperators">An enumeration of Charging Station Operators.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateChargingStationOperatorData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,
        //                                                       ActionType                            ActionType,

        //                                                       DateTime?                             Timestamp          = null,
        //                                                       CancellationToken?                    CancellationToken  = null,
        //                                                       EventTracking_Id                      EventTrackingId    = null,
        //                                                       TimeSpan?                             RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStationOperators == null)
        //        throw new ArgumentNullException(nameof(ChargingStationOperators),  "The given enumeration of charging station operators must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region UpdateRoamingNetworkData         (RoamingNetwork,   ActionType, ...)

        ///// <summary>
        ///// Upload the EVSE data of the given roaming network.
        ///// </summary>
        ///// <param name="RoamingNetwork">A roaming network.</param>
        ///// <param name="ActionType">The server-side data management operation.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //Task<Acknowledgement>

        //    IRemotePushData.UpdateRoamingNetworkData(RoamingNetwork       RoamingNetwork,
        //                                              ActionType           ActionType,

        //                                              DateTime?            Timestamp          = null,
        //                                              CancellationToken?   CancellationToken  = null,
        //                                              EventTracking_Id     EventTrackingId    = null,
        //                                              TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (RoamingNetwork == null)
        //        throw new ArgumentNullException(nameof(SmartCityStub), "The given roaming network must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion


        //public void RemoveChargingStations(DateTime                      Timestamp,
        //                                   IEnumerable<ChargingStation>  ChargingStations)
        //{

        //    foreach (var _ChargingStation in ChargingStations)
        //        Console.WriteLine(DateTime.Now + " LocalEMobilityService says: " + _ChargingStation.Id + " was removed!");

        //}

        //#endregion

        //#region Receive incoming EVSEStatus

        //private IRemotePushStatus AsIPushStatus2Remote => this;

        //#region UpdateEVSEStatus(EVSEStatus, ...)

        ///// <summary>
        ///// Upload the given EVSE status.
        ///// </summary>
        ///// <param name="EVSEStatus">An EVSE status.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //async Task<Acknowledgement>

        //    IRemotePushStatus.UpdateEVSEStatus(EVSEStatus          EVSEStatus,

        //                                        DateTime?           Timestamp,
        //                                        CancellationToken?  CancellationToken,
        //                                        EventTracking_Id    EventTrackingId,
        //                                        TimeSpan?           RequestTimeout)

        //{

        //    #region Initial checks

        //    if (EVSEStatus == null)
        //        throw new ArgumentNullException(nameof(EVSEStatus), "The given EVSE status must not be null!");


        //    Acknowledgement result;

        //    #endregion

        //    #region Send OnUpdateEVSEStatusRequest event

        //    //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
        //    //                                   Timestamp.Value,
        //    //                                   this,
        //    //                                   this.Id.ToString(),
        //    //                                   EventTrackingId,
        //    //                                   this.RoamingNetwork.Id,
        //    //                                   ActionType,
        //    //                                   GroupedEVSEStatus,
        //    //                                   (UInt32) _NumberOfEVSEStatus,
        //    //                                   RequestTimeout);

        //    #endregion


        //    if (RemoteNavigationProvider != null)
        //        result = await RemoteNavigationProvider.UpdateEVSEStatus(EVSEStatus,

        //                                                        Timestamp,
        //                                                        CancellationToken,
        //                                                        EventTrackingId,
        //                                                        RequestTimeout);

        //    else
        //        result = new Acknowledgement(ResultType.NoOperation);


        //    #region Send OnUpdateEVSEStatusResponse event

        //    // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
        //    //                                    Timestamp.Value,
        //    //                                    this,
        //    //                                    this.Id.ToString(),
        //    //                                    EventTrackingId,
        //    //                                    this.RoamingNetwork.Id,
        //    //                                    ActionType,
        //    //                                    GroupedEVSEStatus,
        //    //                                    (UInt32) _NumberOfEVSEStatus,
        //    //                                    RequestTimeout,
        //    //                                    result,
        //    //                                    DateTime.Now - Timestamp.Value);

        //    #endregion

        //    return result;

        //}

        //#endregion

        //#region UpdateEVSEStatus(EVSEStatus, ...)

        ///// <summary>
        ///// Upload the given enumeration of EVSE status.
        ///// </summary>
        ///// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //async Task<Acknowledgement>

        //    IRemotePushStatus.UpdateEVSEStatus(IEnumerable<EVSEStatus>  EVSEStatus,

        //                                        DateTime?                Timestamp,
        //                                        CancellationToken?       CancellationToken,
        //                                        EventTracking_Id         EventTrackingId,
        //                                        TimeSpan?                RequestTimeout)

        //{

        //    #region Initial checks

        //    if (EVSEStatus == null)
        //        throw new ArgumentNullException(nameof(EVSEStatus),  "The given enumeration of EVSE status must not be null!");


        //    Acknowledgement result;

        //    #endregion

        //    #region Send OnUpdateEVSEStatusRequest event

        //    //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
        //    //                                   Timestamp.Value,
        //    //                                   this,
        //    //                                   this.Id.ToString(),
        //    //                                   EventTrackingId,
        //    //                                   this.RoamingNetwork.Id,
        //    //                                   ActionType,
        //    //                                   GroupedEVSEStatus,
        //    //                                   (UInt32) _NumberOfEVSEStatus,
        //    //                                   RequestTimeout);

        //    #endregion


        //    if (RemoteNavigationProvider != null)
        //        result = await RemoteNavigationProvider.UpdateEVSEStatus(EVSEStatus,

        //                                                        Timestamp,
        //                                                        CancellationToken,
        //                                                        EventTrackingId,
        //                                                        RequestTimeout);

        //    else
        //        result = new Acknowledgement(ResultType.NoOperation);


        //    #region Send OnUpdateEVSEStatusResponse event

        //    // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
        //    //                                    Timestamp.Value,
        //    //                                    this,
        //    //                                    this.Id.ToString(),
        //    //                                    EventTrackingId,
        //    //                                    this.RoamingNetwork.Id,
        //    //                                    ActionType,
        //    //                                    GroupedEVSEStatus,
        //    //                                    (UInt32) _NumberOfEVSEStatus,
        //    //                                    RequestTimeout,
        //    //                                    result,
        //    //                                    DateTime.Now - Timestamp.Value);

        //    #endregion

        //    return result;

        //}

        //#endregion

        //#region UpdateEVSEStatus(EVSE, ...)

        ///// <summary>
        ///// Upload the EVSE status of the given EVSE.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //async Task<Acknowledgement>

        //    IRemotePushStatus.UpdateEVSEStatus(EVSE                 EVSE,

        //                                        DateTime?            Timestamp,
        //                                        CancellationToken?   CancellationToken,
        //                                        EventTracking_Id     EventTrackingId,
        //                                        TimeSpan?            RequestTimeout)

        //{

        //    #region Initial checks

        //    if (EVSE == null)
        //        throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");


        //    Acknowledgement result;

        //    #endregion

        //    #region Send OnUpdateEVSEStatusRequest event

        //    //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
        //    //                                   Timestamp.Value,
        //    //                                   this,
        //    //                                   this.Id.ToString(),
        //    //                                   EventTrackingId,
        //    //                                   this.RoamingNetwork.Id,
        //    //                                   ActionType,
        //    //                                   GroupedEVSEStatus,
        //    //                                   (UInt32) _NumberOfEVSEStatus,
        //    //                                   RequestTimeout);

        //    #endregion


        //    if (RemoteNavigationProvider != null)
        //        result = await RemoteNavigationProvider.UpdateEVSEStatus(EVSE,

        //                                                        Timestamp,
        //                                                        CancellationToken,
        //                                                        EventTrackingId,
        //                                                        RequestTimeout);

        //    else
        //        result = new Acknowledgement(ResultType.NoOperation);


        //    #region Send OnUpdateEVSEStatusResponse event

        //    // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
        //    //                                    Timestamp.Value,
        //    //                                    this,
        //    //                                    this.Id.ToString(),
        //    //                                    EventTrackingId,
        //    //                                    this.RoamingNetwork.Id,
        //    //                                    ActionType,
        //    //                                    GroupedEVSEStatus,
        //    //                                    (UInt32) _NumberOfEVSEStatus,
        //    //                                    RequestTimeout,
        //    //                                    result,
        //    //                                    DateTime.Now - Timestamp.Value);

        //    #endregion

        //    return result;

        //}

        //#endregion

        //#region UpdateEVSEStatus(EVSEs, ...)

        ///// <summary>
        ///// Upload all EVSE status of the given enumeration of EVSEs.
        ///// </summary>
        ///// <param name="EVSEs">An enumeration of EVSEs.</param>
        ///// 
        ///// <param name="Timestamp">The optional timestamp of the request.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        //async Task<Acknowledgement>

        //    IRemotePushStatus.UpdateEVSEStatus(IEnumerable<EVSE>    EVSEs,

        //                                        DateTime?            Timestamp,
        //                                        CancellationToken?   CancellationToken,
        //                                        EventTracking_Id     EventTrackingId,
        //                                        TimeSpan?            RequestTimeout)

        //{

        //    #region Initial checks

        //    if (EVSEs == null)
        //        throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");


        //    Acknowledgement result;

        //    #endregion

        //    #region Send OnUpdateEVSEStatusRequest event

        //    //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
        //    //                                   Timestamp.Value,
        //    //                                   this,
        //    //                                   this.Id.ToString(),
        //    //                                   EventTrackingId,
        //    //                                   this.RoamingNetwork.Id,
        //    //                                   ActionType,
        //    //                                   GroupedEVSEStatus,
        //    //                                   (UInt32) _NumberOfEVSEStatus,
        //    //                                   RequestTimeout);

        //    #endregion


        //    if (RemoteNavigationProvider != null)
        //        result = await RemoteNavigationProvider.UpdateEVSEStatus(EVSEs,

        //                                                        Timestamp,
        //                                                        CancellationToken,
        //                                                        EventTrackingId,
        //                                                        RequestTimeout);

        //    else
        //        result = new Acknowledgement(ResultType.NoOperation);


        //    #region Send OnUpdateEVSEStatusResponse event

        //    // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
        //    //                                    Timestamp.Value,
        //    //                                    this,
        //    //                                    this.Id.ToString(),
        //    //                                    EventTrackingId,
        //    //                                    this.RoamingNetwork.Id,
        //    //                                    ActionType,
        //    //                                    GroupedEVSEStatus,
        //    //                                    (UInt32) _NumberOfEVSEStatus,
        //    //                                    RequestTimeout,
        //    //                                    result,
        //    //                                    DateTime.Now - Timestamp.Value);

        //    #endregion

        //    return result;

        //}

        //#endregion

        //#endregion

        #endregion



        #region IComparable<EVSE_Operator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as NavigationProvider;
            if ((Object) EVSE_Operator == null)
                throw new ArgumentException("The given object is not an EVSE_Operator!");

            return CompareTo(EVSE_Operator);

        }

        #endregion

        #region CompareTo(EVSE_Operator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Operator">An EVSE_Operator object to compare with.</param>
        public Int32 CompareTo(NavigationProvider EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                throw new ArgumentNullException("The given EVSE_Operator must not be null!");

            return Id.CompareTo(EVSE_Operator.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSE_Operator> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as NavigationProvider;
            if ((Object) EVSE_Operator == null)
                return false;

            return this.Equals(EVSE_Operator);

        }

        #endregion

        #region Equals(EVSE_Operator)

        /// <summary>
        /// Compares two EVSE_Operator for equality.
        /// </summary>
        /// <param name="EVSE_Operator">An EVSE_Operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(NavigationProvider EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                return false;

            return Id.Equals(EVSE_Operator.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
