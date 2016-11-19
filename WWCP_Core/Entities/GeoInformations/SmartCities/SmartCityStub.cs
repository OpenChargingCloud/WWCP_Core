/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The smart city is not only the main contract party of the EV driver,
    /// the smart city also takes care of the EV driver master data,
    /// the authentication and autorisation process before charging and for the
    /// billing process after charging.
    /// The smart city provides the EV drivere one or multiple methods for
    /// authentication (e.g. based on RFID cards, login/passwords, client certificates).
    /// The smart city takes care that none of the provided authentication
    /// methods can be misused by any entity in the ev charging process to track the
    /// ev driver or its behaviour.
    /// </summary>
    public class SmartCityStub : ACryptoEMobilityEntity<SmartCity_Id>,
                                 IRemoteSmartCity,
                                 IEquatable <SmartCityStub>,
                                 IComparable<SmartCityStub>,
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

        public Authorizator_Id AuthorizatorId { get; }

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
        public Timestamped<SmartCityAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<SmartCityAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<SmartCityAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<SmartCityStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<SmartCityStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<SmartCityStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion


        public SmartCityPriority Priority { get; set; }


        #region AllTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens

            => RemoteSmartCity != null
                   ? RemoteSmartCity.AllTokens
                   : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        #endregion

        #region AuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens

            => RemoteSmartCity != null
                   ? RemoteSmartCity.AuthorizedTokens
                   : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        #endregion

        #region NotAuthorizedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens

            => RemoteSmartCity != null
                   ? RemoteSmartCity.NotAuthorizedTokens
                   : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        #endregion

        #region BlockedTokens

        public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens

            => RemoteSmartCity != null
                   ? RemoteSmartCity.BlockedTokens
                   : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote smart city.
        /// </summary>
        public IRemoteSmartCity          RemoteSmartCity    { get; }


        /// <summary>
        /// The parent roaming network.
        /// </summary>
        public RoamingNetwork            RoamingNetwork             { get; }

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest;

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnPushEVSEStatusRequestDelegate OnPushEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public event OnPushEVSEStatusResponseDelegate OnPushEVSEStatusResponse;

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
        /// <param name="Id">The unique smart city identification.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal SmartCityStub(SmartCity_Id                    Id,
                           RoamingNetwork                  RoamingNetwork,
                           Action<SmartCityStub>               Configurator            = null,
                           RemoteSmartCityCreatorDelegate  RemoteSmartCityCreator  = null,
                           I18NString                      Name                    = null,
                           I18NString                      Description             = null,
                           SmartCityPriority               Priority                = null,
                           SmartCityAdminStatusType        AdminStatus             = SmartCityAdminStatusType.Available,
                           SmartCityStatusType             Status                  = SmartCityStatusType.Available,
                           UInt16                          MaxAdminStatusListSize  = DefaultMaxAdminStatusListSize,
                           UInt16                          MaxStatusListSize       = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(SmartCityStub),  "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this.RoamingNetwork              = RoamingNetwork;

            this._Name                        = Name        ?? new I18NString();
            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new List<DataLicense>();

            this.Priority                     = Priority    ?? new SmartCityPriority(0);

            this._AdminStatusSchedule         = new StatusSchedule<SmartCityAdminStatusType>();
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule              = new StatusSchedule<SmartCityStatusType>();
            this._StatusSchedule.Insert(Status);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteSmartCity = RemoteSmartCityCreator?.Invoke(this);

        }

        #endregion



        #region Incoming requests from the roaming network

        #region Receive incoming EVSEData

        #region UpdateEVSEData                   (EVSE,             ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateEVSEData(EVSE                 EVSE,
                                            ActionType           ActionType,

                                            DateTime?            Timestamp          = null,
                                            CancellationToken?   CancellationToken  = null,
                                            EventTracking_Id     EventTrackingId    = null,
                                            TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateEVSEData                   (EVSEs,            ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateEVSEData(IEnumerable<EVSE>    EVSEs,
                                            ActionType           ActionType,

                                            DateTime?            Timestamp          = null,
                                            CancellationToken?   CancellationToken  = null,
                                            EventTracking_Id     EventTrackingId    = null,
                                            TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateChargingStationData        (ChargingStation,  ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateChargingStationData(ChargingStation      ChargingStation,
                                                      ActionType           ActionType,

                                                      DateTime?            Timestamp          = null,
                                                      CancellationToken?   CancellationToken  = null,
                                                      EventTracking_Id     EventTrackingId    = null,
                                                      TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateChargingStationData        (ChargingStations, ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging stations.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateChargingStationData(IEnumerable<ChargingStation>  ChargingStations,
                                                      ActionType                    ActionType,

                                                      DateTime?                     Timestamp          = null,
                                                      CancellationToken?            CancellationToken  = null,
                                                      EventTracking_Id              EventTrackingId    = null,
                                                      TimeSpan?                     RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateChargingPoolData           (ChargingPool,     ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateChargingPoolData(ChargingPool         ChargingPool,
                                                    ActionType           ActionType,

                                                    DateTime?            Timestamp          = null,
                                                    CancellationToken?   CancellationToken  = null,
                                                    EventTracking_Id     EventTrackingId    = null,
                                                    TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateChargingPoolData           (ChargingPools,    ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given charging pools.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateChargingPoolData(IEnumerable<ChargingPool>  ChargingPools,
                                                    ActionType                 ActionType,

                                                    DateTime?                  Timestamp          = null,
                                                    CancellationToken?         CancellationToken  = null,
                                                    EventTracking_Id           EventTrackingId    = null,
                                                    TimeSpan?                  RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateChargingStationOperatorData(EVSEOperator,     ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given Charging Station Operator.
        /// </summary>
        /// <param name="ChargingStationOperator">An Charging Station Operator.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateChargingStationOperatorData(ChargingStationOperator  ChargingStationOperator,
                                                               ActionType               ActionType,

                                                               DateTime?                Timestamp          = null,
                                                               CancellationToken?       CancellationToken  = null,
                                                               EventTracking_Id         EventTrackingId    = null,
                                                               TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateChargingStationOperatorData(EVSEOperators,    ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given Charging Station Operators.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateChargingStationOperatorData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,
                                                               ActionType                            ActionType,

                                                               DateTime?                             Timestamp          = null,
                                                               CancellationToken?                    CancellationToken  = null,
                                                               EventTracking_Id                      EventTrackingId    = null,
                                                               TimeSpan?                             RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators),  "The given enumeration of charging station operators must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion

        #region UpdateRoamingNetworkData         (RoamingNetwork,   ActionType, ...)

        /// <summary>
        /// Upload the EVSE data of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="ActionType">The server-side data management operation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            IRemotePushData.UpdateRoamingNetworkData(RoamingNetwork       RoamingNetwork,
                                                      ActionType           ActionType,

                                                      DateTime?            Timestamp          = null,
                                                      CancellationToken?   CancellationToken  = null,
                                                      EventTracking_Id     EventTrackingId    = null,
                                                      TimeSpan?            RequestTimeout     = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(SmartCityStub), "The given roaming network must not be null!");

            #endregion

            return Task.FromResult(new Acknowledgement(ResultType.True));

        }

        #endregion


        public void RemoveChargingStations(DateTime                      Timestamp,
                                           IEnumerable<ChargingStation>  ChargingStations)
        {

            foreach (var _ChargingStation in ChargingStations)
                Console.WriteLine(DateTime.Now + " LocalEMobilityService says: " + _ChargingStation.Id + " was removed!");

        }

        #endregion

        #region Receive incoming EVSEStatus

        private IRemotePushStatus AsIPushStatus2Remote => this;

        #region UpdateEVSEStatus(EVSEStatus, ...)

        /// <summary>
        /// Upload the given EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            IRemotePushStatus.UpdateEVSEStatus(EVSEStatus          EVSEStatus,

                                                DateTime?           Timestamp,
                                                CancellationToken?  CancellationToken,
                                                EventTracking_Id    EventTrackingId,
                                                TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus), "The given EVSE status must not be null!");


            Acknowledgement result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
            //                                   Timestamp.Value,
            //                                   this,
            //                                   this.Id.ToString(),
            //                                   EventTrackingId,
            //                                   this.RoamingNetwork.Id,
            //                                   ActionType,
            //                                   GroupedEVSEStatus,
            //                                   (UInt32) _NumberOfEVSEStatus,
            //                                   RequestTimeout);

            #endregion


            if (RemoteSmartCity != null)
                result = await RemoteSmartCity.UpdateEVSEStatus(EVSEStatus,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

            else
                result = new Acknowledgement(ResultType.NoOperation);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    this.Id.ToString(),
            //                                    EventTrackingId,
            //                                    this.RoamingNetwork.Id,
            //                                    ActionType,
            //                                    GroupedEVSEStatus,
            //                                    (UInt32) _NumberOfEVSEStatus,
            //                                    RequestTimeout,
            //                                    result,
            //                                    DateTime.Now - Timestamp.Value);

            #endregion

            return result;

        }

        #endregion

        #region UpdateEVSEStatus(EVSEStatus, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE status.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            IRemotePushStatus.UpdateEVSEStatus(IEnumerable<EVSEStatus>  EVSEStatus,

                                                DateTime?                Timestamp,
                                                CancellationToken?       CancellationToken,
                                                EventTracking_Id         EventTrackingId,
                                                TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus),  "The given enumeration of EVSE status must not be null!");


            Acknowledgement result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
            //                                   Timestamp.Value,
            //                                   this,
            //                                   this.Id.ToString(),
            //                                   EventTrackingId,
            //                                   this.RoamingNetwork.Id,
            //                                   ActionType,
            //                                   GroupedEVSEStatus,
            //                                   (UInt32) _NumberOfEVSEStatus,
            //                                   RequestTimeout);

            #endregion


            if (RemoteSmartCity != null)
                result = await RemoteSmartCity.UpdateEVSEStatus(EVSEStatus,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

            else
                result = new Acknowledgement(ResultType.NoOperation);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    this.Id.ToString(),
            //                                    EventTrackingId,
            //                                    this.RoamingNetwork.Id,
            //                                    ActionType,
            //                                    GroupedEVSEStatus,
            //                                    (UInt32) _NumberOfEVSEStatus,
            //                                    RequestTimeout,
            //                                    result,
            //                                    DateTime.Now - Timestamp.Value);

            #endregion

            return result;

        }

        #endregion

        #region UpdateEVSEStatus(EVSE, ...)

        /// <summary>
        /// Upload the EVSE status of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            IRemotePushStatus.UpdateEVSEStatus(EVSE                 EVSE,

                                                DateTime?            Timestamp,
                                                CancellationToken?   CancellationToken,
                                                EventTracking_Id     EventTrackingId,
                                                TimeSpan?            RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");


            Acknowledgement result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
            //                                   Timestamp.Value,
            //                                   this,
            //                                   this.Id.ToString(),
            //                                   EventTrackingId,
            //                                   this.RoamingNetwork.Id,
            //                                   ActionType,
            //                                   GroupedEVSEStatus,
            //                                   (UInt32) _NumberOfEVSEStatus,
            //                                   RequestTimeout);

            #endregion


            if (RemoteSmartCity != null)
                result = await RemoteSmartCity.UpdateEVSEStatus(EVSE,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

            else
                result = new Acknowledgement(ResultType.NoOperation);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    this.Id.ToString(),
            //                                    EventTrackingId,
            //                                    this.RoamingNetwork.Id,
            //                                    ActionType,
            //                                    GroupedEVSEStatus,
            //                                    (UInt32) _NumberOfEVSEStatus,
            //                                    RequestTimeout,
            //                                    result,
            //                                    DateTime.Now - Timestamp.Value);

            #endregion

            return result;

        }

        #endregion

        #region UpdateEVSEStatus(EVSEs, ...)

        /// <summary>
        /// Upload all EVSE status of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            IRemotePushStatus.UpdateEVSEStatus(IEnumerable<EVSE>    EVSEs,

                                                DateTime?            Timestamp,
                                                CancellationToken?   CancellationToken,
                                                EventTracking_Id     EventTrackingId,
                                                TimeSpan?            RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");


            Acknowledgement result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            //   OnPushEVSEStatusRequest?.Invoke(DateTime.Now,
            //                                   Timestamp.Value,
            //                                   this,
            //                                   this.Id.ToString(),
            //                                   EventTrackingId,
            //                                   this.RoamingNetwork.Id,
            //                                   ActionType,
            //                                   GroupedEVSEStatus,
            //                                   (UInt32) _NumberOfEVSEStatus,
            //                                   RequestTimeout);

            #endregion


            if (RemoteSmartCity != null)
                result = await RemoteSmartCity.UpdateEVSEStatus(EVSEs,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

            else
                result = new Acknowledgement(ResultType.NoOperation);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.Now,
            //                                    Timestamp.Value,
            //                                    this,
            //                                    this.Id.ToString(),
            //                                    EventTrackingId,
            //                                    this.RoamingNetwork.Id,
            //                                    ActionType,
            //                                    GroupedEVSEStatus,
            //                                    (UInt32) _NumberOfEVSEStatus,
            //                                    RequestTimeout,
            //                                    result,
            //                                    DateTime.Now - Timestamp.Value);

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region Receive incoming AuthStart/-Stop

        #region AuthorizeStart(ChargingStationOperatorId, AuthToken, ChargingProductId, SessionId, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An Charging Station Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(ChargingStationOperator_Id  ChargingStationOperatorId,
                           Auth_Token                  AuthToken,
                           ChargingProduct_Id?         ChargingProductId,
                           ChargingSession_Id?         SessionId,

                           DateTime?                   Timestamp,
                           CancellationToken?          CancellationToken,
                           EventTracking_Id            EventTrackingId,
                           TimeSpan?                   RequestTimeout)

        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.AuthorizeStart(ChargingStationOperatorId,
                                                                     AuthToken,
                                                                     ChargingProductId,
                                                                     SessionId,

                                                                     Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     RequestTimeout);



            return AuthStartResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #region AuthorizeStart(ChargingStationOperatorId, AuthToken, EVSEId, ChargingProductId, SessionId, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An Charging Station Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(ChargingStationOperator_Id  ChargingStationOperatorId,
                           Auth_Token                  AuthToken,
                           EVSE_Id                     EVSEId,
                           ChargingProduct_Id?         ChargingProductId,
                           ChargingSession_Id?         SessionId,

                           DateTime?                   Timestamp,
                           CancellationToken?          CancellationToken,
                           EventTracking_Id            EventTrackingId,
                           TimeSpan?                   RequestTimeout)

        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.AuthorizeStart(ChargingStationOperatorId,
                                                                     AuthToken,
                                                                     EVSEId,
                                                                     ChargingProductId,
                                                                     SessionId,

                                                                     Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     RequestTimeout);



            return AuthStartEVSEResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #region AuthorizeStart(ChargingStationOperatorId, AuthToken, ChargingStationId, ChargingProductId, SessionId, ...)

        /// <summary>
        /// Create an AuthorizeStart request at the given charging station.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An Charging Station Operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(ChargingStationOperator_Id  ChargingStationOperatorId,
                           Auth_Token                  AuthToken,
                           ChargingStation_Id          ChargingStationId,
                           ChargingProduct_Id?         ChargingProductId,
                           ChargingSession_Id?         SessionId,

                           DateTime?                   Timestamp,
                           CancellationToken?          CancellationToken,
                           EventTracking_Id            EventTrackingId,
                           TimeSpan?                   RequestTimeout)

        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.AuthorizeStart(ChargingStationOperatorId,
                                                                     AuthToken,
                                                                     ChargingStationId,
                                                                     ChargingProductId,
                                                                     SessionId,

                                                                     Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     RequestTimeout);



            return AuthStartChargingStationResult.OutOfService(AuthorizatorId);

        }

        #endregion


        #region AuthorizeStop(ChargingStationOperatorId, SessionId, AuthToken, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An Charging Station Operator identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingStationOperator_Id  ChargingStationOperatorId,
                          ChargingSession_Id          SessionId,
                          Auth_Token                  AuthToken,

                          DateTime?                   Timestamp,
                          CancellationToken?          CancellationToken,
                          EventTracking_Id            EventTrackingId,
                          TimeSpan?                   RequestTimeout)

        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.AuthorizeStop(ChargingStationOperatorId,
                                                                    SessionId,
                                                                    AuthToken,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);



            return AuthStopResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #region AuthorizeStop(ChargingStationOperatorId, EVSEId, SessionId, AuthToken, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An Charging Station Operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingStationOperator_Id  ChargingStationOperatorId,
                          EVSE_Id                     EVSEId,
                          ChargingSession_Id          SessionId,
                          Auth_Token                  AuthToken,

                          DateTime?                   Timestamp,
                          CancellationToken?          CancellationToken,
                          EventTracking_Id            EventTrackingId,
                          TimeSpan?                   RequestTimeout)

        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.AuthorizeStop(ChargingStationOperatorId,
                                                                    EVSEId,
                                                                    SessionId,
                                                                    AuthToken,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);



            return AuthStopEVSEResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #region AuthorizeStop(ChargingStationOperatorId, ChargingStationId, SessionId, AuthToken, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="ChargingStationOperatorId">An Charging Station Operator identification.</param>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingStationOperator_Id  ChargingStationOperatorId,
                          ChargingStation_Id          ChargingStationId,
                          ChargingSession_Id          SessionId,
                          Auth_Token                  AuthToken,

                          DateTime?                   Timestamp,
                          CancellationToken?          CancellationToken,
                          EventTracking_Id            EventTrackingId,
                          TimeSpan?                   RequestTimeout)

        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.AuthorizeStop(ChargingStationOperatorId,
                                                                    ChargingStationId,
                                                                    SessionId,
                                                                    AuthToken,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);



            return AuthStopChargingStationResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #endregion

        #region Receive incoming ChargeDetailRecords

        #region SendChargeDetailRecord(ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,

                                   DateTime?           Timestamp,
                                   CancellationToken?  CancellationToken,
                                   EventTracking_Id    EventTrackingId,
                                   TimeSpan?           RequestTimeout)
        {

            if (RemoteSmartCity != null)
                return await RemoteSmartCity.SendChargeDetailRecord(ChargeDetailRecord,

                                                                            Timestamp,
                                                                            CancellationToken,
                                                                            EventTrackingId,
                                                                            RequestTimeout);

            return SendCDRResult.OutOfService(AuthorizatorId);

        }

        #endregion

        #endregion

        #endregion


        #region Outgoing requests towards the roaming network

        //ToDo: Send Tokens!
        //ToDo: Download CDRs!

        #region Reserve(...EVSEId, StartTime, Duration, ReservationId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(EVSE_Id                           EVSEId,
                    DateTime?                         StartTime           = null,
                    TimeSpan?                         Duration            = null,
                    ChargingReservation_Id?           ReservationId       = null,
                    eMobilityAccount_Id?              eMAId               = null,
                    ChargingProduct_Id?               ChargingProductId   = null,
                    IEnumerable<Auth_Token>           AuthTokens          = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                    IEnumerable<UInt32>               PINs                = null,

                    DateTime?                         Timestamp           = null,
                    CancellationToken?                CancellationToken   = null,
                    EventTracking_Id                  EventTrackingId     = null,
                    TimeSpan?                         RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSE event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveEVSE?.Invoke(DateTime.Now,
                                      Timestamp.Value,
                                      this,
                                      EventTrackingId,
                                      RoamingNetwork.Id,
                                      ReservationId,
                                      EVSEId,
                                      StartTime,
                                      Duration,
                                      null,
                                      eMAId,
                                      ChargingProductId,
                                      AuthTokens,
                                      eMAIds,
                                      PINs,
                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(SmartCityStub) + "." + nameof(OnReserveEVSE));
            }

            #endregion


            var response = await RoamingNetwork.Reserve(EVSEId,
                                                        StartTime,
                                                        Duration,
                                                        ReservationId,
                                                        null,
                                                        eMAId,
                                                        ChargingProductId,
                                                        AuthTokens,
                                                        eMAIds,
                                                        PINs,

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout);


            #region Send OnEVSEReserved event

            Runtime.Stop();

            try
            {

                OnEVSEReserved?.Invoke(DateTime.Now,
                                       Timestamp.Value,
                                       this,
                                       EventTrackingId,
                                       RoamingNetwork.Id,
                                       ReservationId,
                                       EVSEId,
                                       StartTime,
                                       Duration,
                                       null,
                                       eMAId,
                                       ChargingProductId,
                                       AuthTokens,
                                       eMAIds,
                                       PINs,
                                       response,
                                       Runtime.Elapsed,
                                       RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(SmartCityStub) + "." + nameof(OnEVSEReserved));
            }

            #endregion

            return response;

        }

        #endregion

        #region CancelReservation(...ReservationId, Reason, EVSEId = null, ...)

        /// <summary>
        /// Cancel the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="EVSEId">An optional identification of the EVSE.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              EVSE_Id?                               EVSEId              = null,

                              DateTime?                              Timestamp           = null,
                              CancellationToken?                     CancellationToken   = null,
                              EventTracking_Id                       EventTrackingId     = null,
                              TimeSpan?                              RequestTimeout      = null)

        {

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            var response = await RoamingNetwork.CancelReservation(ReservationId,
                                                                  Reason,
                                                                  null,
                                                                  EVSEId,

                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout);


            //var OnReservationCancelledLocal = OnReservationCancelled;
            //if (OnReservationCancelledLocal != null)
            //    OnReservationCancelledLocal(DateTime.Now,
            //                                this,
            //                                EventTracking_Id.New,
            //                                ReservationId,
            //                                Reason);

            return response;

        }

        #endregion


        #region RemoteStart(...EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult>

            RemoteStart(EVSE_Id                  EVSEId,
                        ChargingProduct_Id?      ChargingProductId  = null,
                        ChargingReservation_Id?  ReservationId      = null,
                        ChargingSession_Id?      SessionId          = null,
                        eMobilityAccount_Id?     eMAId              = null,

                        DateTime?                Timestamp          = null,
                        CancellationToken?       CancellationToken  = null,
                        EventTracking_Id         EventTrackingId    = null,
                        TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStart event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStart?.Invoke(DateTime.Now,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          EVSEId,
                                          ChargingProductId,
                                          ReservationId,
                                          SessionId,
                                          null,
                                          eMAId,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(SmartCityStub) + "." + nameof(OnRemoteEVSEStart));
            }

            #endregion


            var response = await RoamingNetwork.RemoteStart(EVSEId,
                                                            ChargingProductId,
                                                            ReservationId,
                                                            SessionId,
                                                            null,
                                                            eMAId,

                                                            Timestamp,
                                                            CancellationToken,
                                                            EventTrackingId,
                                                            RequestTimeout);


            #region Send OnRemoteEVSEStarted event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStarted?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            EVSEId,
                                            ChargingProductId,
                                            ReservationId,
                                            SessionId,
                                            null,
                                            eMAId,
                                            RequestTimeout,
                                            response,
                                            Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(SmartCityStub) + "." + nameof(OnRemoteEVSEStarted));
            }

            #endregion

            return response;

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(EVSE_Id               EVSEId,
                       ChargingSession_Id    SessionId,
                       ReservationHandling   ReservationHandling,
                       eMobilityAccount_Id?  eMAId               = null,

                       DateTime?             Timestamp           = null,
                       CancellationToken?    CancellationToken   = null,
                       EventTracking_Id      EventTrackingId     = null,
                       TimeSpan?             RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),     "The given EVSE identification must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),  "The given charging session identification must not be null!");

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStop?.Invoke(DateTime.Now,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         EVSEId,
                                         SessionId,
                                         ReservationHandling,
                                         null,
                                         eMAId,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(SmartCityStub) + "." + nameof(OnRemoteEVSEStop));
            }

            #endregion


            var response = await RoamingNetwork.RemoteStop(EVSEId,
                                                           SessionId,
                                                           ReservationHandling,
                                                           null,
                                                           eMAId,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


            #region Send OnRemoteEVSEStopped event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStopped?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            EVSEId,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            eMAId,
                                            RequestTimeout,
                                            response,
                                            Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(SmartCityStub) + "." + nameof(OnRemoteEVSEStopped));
            }

            #endregion

            return response;

        }

        #endregion

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
            var EVSE_Operator = Object as SmartCityStub;
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
        public Int32 CompareTo(SmartCityStub EVSE_Operator)
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
            var EVSE_Operator = Object as SmartCityStub;
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
        public Boolean Equals(SmartCityStub EVSE_Operator)
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
