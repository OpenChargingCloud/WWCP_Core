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
using org.GraphDefined.Vanaheimr.Styx.Arrows;

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
    public class eMobilityProvider : ABaseEMobilityEntity<eMobilityProvider_Id>,
                                          //IRemoteAuthorizeStartStop,
                                          ISend2RemoteEMobilityProvider,
                                          IEquatable <eMobilityProvider>,
                                          IComparable<eMobilityProvider>,
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

        IId ISendAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

        IEnumerable<IId> ISendChargeDetailRecords.Ids
            => Ids.Cast<IId>();

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
        public Timestamped<eMobilityProviderAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<eMobilityProviderAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<eMobilityProviderAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<eMobilityProviderStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<eMobilityProviderStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<eMobilityProviderStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion


        public TimeSpan? RequestTimeout { get; }


        public eMobilityProviderPriority Priority { get; set; }

        public Boolean DisablePushAdminStatus { get; set; }

        public Boolean DisablePushStatus { get; set; }

        public Boolean DisableSendChargeDetailRecords { get; set; }



        //#region AllTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens

        //    => RemoteEMobilityProvider != null
        //           ? RemoteEMobilityProvider.AllTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        //#region AuthorizedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens

        //    => RemoteEMobilityProvider != null
        //           ? RemoteEMobilityProvider.AuthorizedTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        //#region NotAuthorizedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens

        //    => RemoteEMobilityProvider != null
        //           ? RemoteEMobilityProvider.NotAuthorizedTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        //#region BlockedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens

        //    => RemoteEMobilityProvider != null
        //           ? RemoteEMobilityProvider.BlockedTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote e-mobility provider.
        /// </summary>
        public IRemoteEMobilityProvider  RemoteEMobilityProvider    { get; }

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


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate                  OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate                 OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartRequestDelegate              OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartRequestDelegate      OnAuthorizeChargingPoolStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartResponseDelegate     OnAuthorizeChargingPoolStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate                  OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopRequestDelegate              OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopRequestDelegate      OnAuthorizeChargingPoolStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopResponseDelegate     OnAuthorizeChargingPoolStopResponse;

        #endregion


        #region OnReserve... / OnReserved...

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnReserveEVSERequestDelegate              OnReserveEVSERequest;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnReserveEVSEResponseDelegate             OnReserveEVSEResponse;

        #endregion

        #region OnRemote...Start / OnRemote...Started

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartEVSERequestDelegate               OnRemoteEVSEStartRequest;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartEVSEResponseDelegate             OnRemoteEVSEStartResponse;

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
        internal eMobilityProvider(eMobilityProvider_Id                    Id,
                                        RoamingNetwork                          RoamingNetwork,
                                        Action<eMobilityProvider>          Configurator                    = null,
                                        RemoteEMobilityProviderCreatorDelegate  RemoteEMobilityProviderCreator  = null,
                                        I18NString                              Name                            = null,
                                        I18NString                              Description                     = null,
                                        eMobilityProviderPriority               Priority                        = null,
                                        eMobilityProviderAdminStatusType        AdminStatus                     = eMobilityProviderAdminStatusType.Operational,
                                        eMobilityProviderStatusType             Status                          = eMobilityProviderStatusType.Available,
                                        UInt16                                  MaxAdminStatusListSize          = DefaultMaxAdminStatusListSize,
                                        UInt16                                  MaxStatusListSize               = DefaultMaxStatusListSize)

            : base(Id,
                   RoamingNetwork)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(eMobilityProvider),  "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this._Name                        = Name        ?? new I18NString();
            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new List<DataLicense>();

            this.Priority                     = Priority    ?? new eMobilityProviderPriority(0);

            this._AdminStatusSchedule         = new StatusSchedule<eMobilityProviderAdminStatusType>();
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule              = new StatusSchedule<eMobilityProviderStatusType>();
            this._StatusSchedule.Insert(Status);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteEMobilityProvider = RemoteEMobilityProviderCreator?.Invoke(this);

        }

        #endregion



        #region eMobilityStations

        #region eMobilityStationAddition

        internal readonly IVotingNotificator<DateTime, eMobilityProvider, eMobilityStation, Boolean> eMobilityStationAddition;

        /// <summary>
        /// Called whenever an e-mobility station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, eMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationAddition

            => eMobilityStationAddition;

        #endregion

        #region eMobilityStationRemoval

        internal readonly IVotingNotificator<DateTime, eMobilityProvider, eMobilityStation, Boolean> eMobilityStationRemoval;

        /// <summary>
        /// Called whenever an e-mobility station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, eMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationRemoval

            => eMobilityStationRemoval;

        #endregion


        #region eMobilityStations

        private EntityHashSet<ChargingStationOperator, eMobilityStation_Id, eMobilityStation> _eMobilityStations;

        public IEnumerable<eMobilityStation> eMobilityStations

            => _eMobilityStations;

        #endregion

        #region eMobilityStationIds

        public IEnumerable<eMobilityStation_Id> eMobilityStationIds

            => _eMobilityStations.Ids;

        #endregion

        #region eMobilityStationAdminStatus

        public IEnumerable<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusType>> eMobilityStationAdminStatus

            => _eMobilityStations.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusType>(vehicle.Id, vehicle.AdminStatus.Value));

        #endregion


        #region CreateNeweMobilityStation(eMobilityStationId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new eMobilityStation having the given
        /// unique eMobilityStation identification.
        /// </summary>
        /// <param name="eMobilityStationId">The unique identification of the new eMobilityStation.</param>
        /// <param name="Configurator">An optional delegate to configure the new eMobilityStation before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new eMobilityStation after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the eMobilityStation failed.</param>
        public eMobilityStation CreateNeweMobilityStation(eMobilityStation_Id                             eMobilityStationId             = null,
                                                          Action<eMobilityStation>                        Configurator                   = null,
                                                          RemoteEMobilityStationCreatorDelegate           RemoteeMobilityStationCreator  = null,
                                                          eMobilityStationAdminStatusType                 AdminStatus                    = eMobilityStationAdminStatusType.Operational,
                                                          Action<eMobilityStation>                        OnSuccess                      = null,
                                                          Action<eMobilityProvider, eMobilityStation_Id>  OnError                        = null)

        {

            #region Initial checks

            if (eMobilityStationId == null)
                eMobilityStationId = eMobilityStation_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_eMobilityStations.Any(pool => pool.Id == eMobilityStationId))
            {
                if (OnError == null)
                    throw new eMobilityStationAlreadyExists(this, eMobilityStationId);
                else
                    OnError?.Invoke(this, eMobilityStationId);
            }

            #endregion

            var _eMobilityStation = new eMobilityStation(eMobilityStationId,
                                                         this,
                                                         Configurator,
                                                         RemoteeMobilityStationCreator,
                                                         AdminStatus);


            if (eMobilityStationAddition.SendVoting(DateTime.UtcNow, this, _eMobilityStation))
            {
                if (_eMobilityStations.TryAdd(_eMobilityStation))
                {

                    _eMobilityStation.OnDataChanged                        += UpdateeMobilityStationData;
                    _eMobilityStation.OnAdminStatusChanged                 += UpdateeMobilityStationAdminStatus;

                    //_eMobilityStation.OnNewReservation                     += SendNewReservation;
                    //_eMobilityStation.OnReservationCancelled               += SendOnReservationCancelled;
                    //_eMobilityStation.OnNewChargingSession                 += SendNewChargingSession;
                    //_eMobilityStation.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


                    OnSuccess?.Invoke(_eMobilityStation);
                    eMobilityStationAddition.SendNotification(DateTime.UtcNow, this, _eMobilityStation);

                    return _eMobilityStation;

                }
            }

            return null;

        }

        #endregion


        #region ContainseMobilityStation(eMobilityStation)

        /// <summary>
        /// Check if the given eMobilityStation is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eMobilityStation">A eMobilityStation.</param>
        public Boolean ContainseMobilityStation(eMobilityStation eMobilityStation)

            => _eMobilityStations.Contains(eMobilityStation);

        #endregion

        #region ContainseMobilityStation(eMobilityStationId)

        /// <summary>
        /// Check if the given eMobilityStation identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eMobilityStationId">The unique identification of the eMobilityStation.</param>
        public Boolean ContainseMobilityStation(eMobilityStation_Id eMobilityStationId)

            => _eMobilityStations.ContainsId(eMobilityStationId);

        #endregion

        #region GeteMobilityStationById(eMobilityStationId)

        public eMobilityStation GeteMobilityStationById(eMobilityStation_Id eMobilityStationId)

            => _eMobilityStations.GetById(eMobilityStationId);

        #endregion

        #region TryGeteMobilityStationById(eMobilityStationId, out eMobilityStation)

        public Boolean TryGeteMobilityStationById(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation)

            => _eMobilityStations.TryGet(eMobilityStationId, out eMobilityStation);

        #endregion

        #region RemoveeMobilityStation(eMobilityStationId)

        public eMobilityStation RemoveeMobilityStation(eMobilityStation_Id eMobilityStationId)
        {

            eMobilityStation _eMobilityStation = null;

            if (TryGeteMobilityStationById(eMobilityStationId, out _eMobilityStation))
            {

                if (eMobilityStationRemoval.SendVoting(DateTime.UtcNow, this, _eMobilityStation))
                {

                    if (_eMobilityStations.TryRemove(eMobilityStationId, out _eMobilityStation))
                    {

                        eMobilityStationRemoval.SendNotification(DateTime.UtcNow, this, _eMobilityStation);

                        return _eMobilityStation;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveeMobilityStation(eMobilityStationId, out eMobilityStation)

        public Boolean TryRemoveeMobilityStation(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation)
        {

            if (TryGeteMobilityStationById(eMobilityStationId, out eMobilityStation))
            {

                if (eMobilityStationRemoval.SendVoting(DateTime.UtcNow, this, eMobilityStation))
                {

                    if (_eMobilityStations.TryRemove(eMobilityStationId, out eMobilityStation))
                    {

                        eMobilityStationRemoval.SendNotification(DateTime.UtcNow, this, eMobilityStation);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, NewStatus)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id                           eMobilityStationId,
                                                   Timestamped<eMobilityStationAdminStatusType>  NewStatus,
                                                   Boolean                                       SendUpstream = false)
        {

            eMobilityStation _eMobilityStation = null;
            if (TryGeteMobilityStationById(eMobilityStationId, out _eMobilityStation))
                _eMobilityStation.SetAdminStatus(NewStatus);

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, NewStatus, Timestamp)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id              eMobilityStationId,
                                                   eMobilityStationAdminStatusType  NewStatus,
                                                   DateTime                         Timestamp)
        {

            eMobilityStation _eMobilityStation  = null;
            if (TryGeteMobilityStationById(eMobilityStationId, out _eMobilityStation))
                _eMobilityStation.SetAdminStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id                                        eMobilityStationId,
                                                   IEnumerable<Timestamped<eMobilityStationAdminStatusType>>  StatusList,
                                                   ChangeMethods                                              ChangeMethod  = ChangeMethods.Replace)
        {

            eMobilityStation _eMobilityStation  = null;
            if (TryGeteMobilityStationById(eMobilityStationId, out _eMobilityStation))
                _eMobilityStation.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendeMobilityStationAdminStatusDiff(new eMobilityStationAdminStatusDiff(DateTime.UtcNow,
            //                                               ChargingStationOperatorId:    Id,
            //                                               ChargingStationOperatorName:  Name,
            //                                               NewStatus:         new List<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusType>>(),
            //                                               ChangedStatus:     new List<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusType>>() {
            //                                                                          new KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusType>(eMobilityStationId, NewStatus.Value)
            //                                                                      },
            //                                               RemovedIds:        new List<eMobilityStation_Id>()));
            //
            //}

        }

        #endregion


        #region OnEMobilityStationData/AdminStatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated eMobilityStation changed.
        /// </summary>
        public event OnEMobilityStationDataChangedDelegate         OnEMobilityStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eMobilityStation changed.
        /// </summary>
        public event OnEMobilityStationAdminStatusChangedDelegate  OnEMobilityStationAdminStatusChanged;

        #endregion


        #region (internal) UpdateeMobilityStationData       (Timestamp, EventTrackingId, eMobilityStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an eMobilityStation.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="eMobilityStation">The changed eMobilityStation.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateeMobilityStationData(DateTime          Timestamp,
                                                       EventTracking_Id  EventTrackingId,
                                                       eMobilityStation  eMobilityStation,
                                                       String            PropertyName,
                                                       Object            OldValue,
                                                       Object            NewValue)
        {

            var OnEMobilityStationDataChangedLocal = OnEMobilityStationDataChanged;
            if (OnEMobilityStationDataChangedLocal != null)
                await OnEMobilityStationDataChangedLocal(Timestamp,
                                                         EventTrackingId,
                                                         eMobilityStation,
                                                         PropertyName,
                                                         OldValue,
                                                         NewValue);

        }

        #endregion

        #region (internal) UpdateeMobilityStationAdminStatus(Timestamp, EventTrackingId, eMobilityStation, OldStatus, NewStatus)

        /// <summary>
        /// Update the current eMobilityStation admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="eMobilityStation">The updated eMobilityStation.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateeMobilityStationAdminStatus(DateTime                                      Timestamp,
                                                              EventTracking_Id                              EventTrackingId,
                                                              eMobilityStation                              eMobilityStation,
                                                              Timestamped<eMobilityStationAdminStatusType>  OldStatus,
                                                              Timestamped<eMobilityStationAdminStatusType>  NewStatus)
        {

            var OnEMobilityStationAdminStatusChangedLocal = OnEMobilityStationAdminStatusChanged;
            if (OnEMobilityStationAdminStatusChangedLocal != null)
                await OnEMobilityStationAdminStatusChangedLocal(Timestamp,
                                                                EventTrackingId,
                                                                eMobilityStation,
                                                                OldStatus,
                                                                NewStatus);

        }

        #endregion

        #endregion

        #region eVehicles

        #region eVehicleAddition

        internal readonly IVotingNotificator<DateTime, eMobilityProvider, eVehicle, Boolean> eVehicleAddition;

        /// <summary>
        /// Called whenever an electric vehicle will be or was added.
        /// </summary>
        public IVotingSender<DateTime, eMobilityProvider, eVehicle, Boolean> OnEVehicleAddition

            => eVehicleAddition;

        #endregion

        #region eVehicleRemoval

        internal readonly IVotingNotificator<DateTime, eMobilityProvider, eVehicle, Boolean> eVehicleRemoval;

        /// <summary>
        /// Called whenever an electric vehicle will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, eMobilityProvider, eVehicle, Boolean> OnEVehicleRemoval

            => eVehicleRemoval;

        #endregion


        #region eVehicles

        private EntityHashSet<ChargingStationOperator, eVehicle_Id, eVehicle> _eVehicles;

        public IEnumerable<eVehicle> eVehicles

            => _eVehicles;

        #endregion

        #region eVehicleIds

        public IEnumerable<eVehicle_Id> eVehicleIds

            => _eVehicles.Ids;

        #endregion

        #region eVehicleAdminStatus

        public IEnumerable<KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>> eVehicleAdminStatus

            => _eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>(vehicle.Id, vehicle.AdminStatus.Value));

        #endregion

        #region eVehicleStatus

        public IEnumerable<KeyValuePair<eVehicle_Id, eVehicleStatusType>> eVehicleStatus

            => _eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<eVehicle_Id, eVehicleStatusType>(vehicle.Id, vehicle.Status.Value));

        #endregion


        #region CreateNeweVehicle(eVehicleId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new eVehicle having the given
        /// unique eVehicle identification.
        /// </summary>
        /// <param name="eVehicleId">The unique identification of the new eVehicle.</param>
        /// <param name="Configurator">An optional delegate to configure the new eVehicle before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new eVehicle after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the eVehicle failed.</param>
        public eVehicle CreateNeweVehicle(eVehicle_Id                             eVehicleId             = null,
                                          Action<eVehicle>                        Configurator           = null,
                                          RemoteEVehicleCreatorDelegate           RemoteeVehicleCreator  = null,
                                          eVehicleAdminStatusType                 AdminStatus            = eVehicleAdminStatusType.Operational,
                                          eVehicleStatusType                      Status                 = eVehicleStatusType.Available,
                                          Action<eVehicle>                        OnSuccess              = null,
                                          Action<eMobilityProvider, eVehicle_Id>  OnError                = null)

        {

            #region Initial checks

            if (eVehicleId == null)
                eVehicleId = eVehicle_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (_eVehicles.Any(pool => pool.Id == eVehicleId))
            {
                if (OnError == null)
                    throw new eVehicleAlreadyExists(this, eVehicleId);
                else
                    OnError?.Invoke(this, eVehicleId);
            }

            #endregion

            var _eVehicle = new eVehicle(eVehicleId,
                                                 this,
                                                 Configurator,
                                                 RemoteeVehicleCreator,
                                                 AdminStatus,
                                                 Status);


            if (eVehicleAddition.SendVoting(DateTime.UtcNow, this, _eVehicle))
            {
                if (_eVehicles.TryAdd(_eVehicle))
                {

                    _eVehicle.OnDataChanged                        += UpdateEVehicleData;
                    _eVehicle.OnStatusChanged                      += UpdateEVehicleStatus;
                    _eVehicle.OnAdminStatusChanged                 += UpdateEVehicleAdminStatus;

                    //_eVehicle.OnNewReservation                     += SendNewReservation;
                    //_eVehicle.OnReservationCancelled               += SendOnReservationCancelled;
                    //_eVehicle.OnNewChargingSession                 += SendNewChargingSession;
                    //_eVehicle.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


                    OnSuccess?.Invoke(_eVehicle);
                    eVehicleAddition.SendNotification(DateTime.UtcNow, this, _eVehicle);

                    return _eVehicle;

                }
            }

            return null;

        }

        #endregion


        #region ContainseVehicle(eVehicle)

        /// <summary>
        /// Check if the given eVehicle is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eVehicle">A eVehicle.</param>
        public Boolean ContainseVehicle(eVehicle eVehicle)

            => _eVehicles.Contains(eVehicle);

        #endregion

        #region ContainseVehicle(eVehicleId)

        /// <summary>
        /// Check if the given eVehicle identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eVehicleId">The unique identification of the eVehicle.</param>
        public Boolean ContainseVehicle(eVehicle_Id eVehicleId)

            => _eVehicles.ContainsId(eVehicleId);

        #endregion

        #region GetEVehicleById(eVehicleId)

        public eVehicle GetEVehicleById(eVehicle_Id eVehicleId)

            => _eVehicles.GetById(eVehicleId);

        #endregion

        #region TryGetEVehicleById(eVehicleId, out eVehicle)

        public Boolean TryGetEVehicleById(eVehicle_Id eVehicleId, out eVehicle eVehicle)

            => _eVehicles.TryGet(eVehicleId, out eVehicle);

        #endregion

        #region RemoveEVehicle(eVehicleId)

        public eVehicle RemoveEVehicle(eVehicle_Id eVehicleId)
        {

            eVehicle _eVehicle = null;

            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
            {

                if (eVehicleRemoval.SendVoting(DateTime.UtcNow, this, _eVehicle))
                {

                    if (_eVehicles.TryRemove(eVehicleId, out _eVehicle))
                    {

                        eVehicleRemoval.SendNotification(DateTime.UtcNow, this, _eVehicle);

                        return _eVehicle;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveEVehicle(eVehicleId, out eVehicle)

        public Boolean TryRemoveEVehicle(eVehicle_Id eVehicleId, out eVehicle eVehicle)
        {

            if (TryGetEVehicleById(eVehicleId, out eVehicle))
            {

                if (eVehicleRemoval.SendVoting(DateTime.UtcNow, this, eVehicle))
                {

                    if (_eVehicles.TryRemove(eVehicleId, out eVehicle))
                    {

                        eVehicleRemoval.SendNotification(DateTime.UtcNow, this, eVehicle);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SeteVehicleAdminStatus(eVehicleId, NewStatus)

        public void SeteVehicleAdminStatus(eVehicle_Id                           eVehicleId,
                                               Timestamped<eVehicleAdminStatusType>  NewStatus,
                                               Boolean                                   SendUpstream = false)
        {

            eVehicle _eVehicle = null;
            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
                _eVehicle.SetAdminStatus(NewStatus);

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, NewStatus, Timestamp)

        public void SetEVehicleAdminStatus(eVehicle_Id              eVehicleId,
                                           eVehicleAdminStatusType  NewStatus,
                                           DateTime                     Timestamp)
        {

            eVehicle _eVehicle  = null;
            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
                _eVehicle.SetAdminStatus(NewStatus, Timestamp);

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVehicleAdminStatus(eVehicle_Id                                        eVehicleId,
                                           IEnumerable<Timestamped<eVehicleAdminStatusType>>  StatusList,
                                           ChangeMethods                                      ChangeMethod  = ChangeMethods.Replace)
        {

            eVehicle _eVehicle  = null;
            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
                _eVehicle.SetAdminStatus(StatusList, ChangeMethod);

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendeVehicleAdminStatusDiff(new eVehicleAdminStatusDiff(DateTime.UtcNow,
            //                                               ChargingStationOperatorId:    Id,
            //                                               ChargingStationOperatorName:  Name,
            //                                               NewStatus:         new List<KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>>(),
            //                                               ChangedStatus:     new List<KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>>() {
            //                                                                          new KeyValuePair<eVehicle_Id, eVehicleAdminStatusType>(eVehicleId, NewStatus.Value)
            //                                                                      },
            //                                               RemovedIds:        new List<eVehicle_Id>()));
            //
            //}

        }

        #endregion


        #region OnEVehicleData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleDataChangedDelegate         OnEVehicleDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleStatusChangedDelegate       OnEVehicleStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleAdminStatusChangedDelegate  OnEVehicleAdminStatusChanged;

        #endregion

        #region OnEVehicleGeoLocationChanged

        /// <summary>
        /// An event fired whenever the geo coordinate changed.
        /// </summary>
        public event OnEVehicleGeoLocationChangedDelegate OnEVehicleGeoLocationChanged;

        #endregion


        #region (internal) UpdateEVehicleData(Timestamp, eVehicle, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of an eVehicle.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="eVehicle">The changed eVehicle.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateEVehicleData(DateTime  Timestamp,
                                               EventTracking_Id  EventTrackingId,
                                               eVehicle  eVehicle,
                                               String    PropertyName,
                                               Object    OldValue,
                                               Object    NewValue)
        {

            var OnEVehicleDataChangedLocal = OnEVehicleDataChanged;
            if (OnEVehicleDataChangedLocal != null)
                await OnEVehicleDataChangedLocal(Timestamp, EventTrackingId, eVehicle, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateEVehicleAdminStatus(Timestamp, eVehicle, OldStatus, NewStatus)

        /// <summary>
        /// Update the current eVehicle admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="eVehicle">The updated eVehicle.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateEVehicleAdminStatus(DateTime                                  Timestamp,
            EventTracking_Id  EventTrackingId,
                                                          eVehicle                              eVehicle,
                                                          Timestamped<eVehicleAdminStatusType>  OldStatus,
                                                          Timestamped<eVehicleAdminStatusType>  NewStatus)
        {

            var OnEVehicleAdminStatusChangedLocal = OnEVehicleAdminStatusChanged;
            if (OnEVehicleAdminStatusChangedLocal != null)
                await OnEVehicleAdminStatusChangedLocal(Timestamp, EventTrackingId, eVehicle, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateEVehicleStatus(Timestamp, eVehicle, OldStatus, NewStatus)

        /// <summary>
        /// Update the current eVehicle status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="eVehicle">The updated eVehicle.</param>
        /// <param name="OldStatus">The old aggreagted charging station status.</param>
        /// <param name="NewStatus">The new aggreagted charging station status.</param>
        internal async Task UpdateEVehicleStatus(DateTime                             Timestamp,
            EventTracking_Id  EventTrackingId,
                                                     eVehicle                         eVehicle,
                                                     Timestamped<eVehicleStatusType>  OldStatus,
                                                     Timestamped<eVehicleStatusType>  NewStatus)
        {

            var OnEVehicleStatusChangedLocal = OnEVehicleStatusChanged;
            if (OnEVehicleStatusChangedLocal != null)
                await OnEVehicleStatusChangedLocal(Timestamp, EventTrackingId,eVehicle, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateEVehicleGeoLocation(Timestamp, eVehicle, OldGeoCoordinate, NewGeoCoordinate)

        /// <summary>
        /// Update the current electric vehicle geo location.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="eVehicle">The updated eVehicle.</param>
        /// <param name="OldGeoCoordinate">The old aggreagted charging station status.</param>
        /// <param name="NewGeoCoordinate">The new aggreagted charging station status.</param>
        internal async Task UpdateEVehicleGeoLocation(DateTime                    Timestamp,
            EventTracking_Id  EventTrackingId,
                                                      eVehicle                    eVehicle,
                                                      Timestamped<GeoCoordinate>  OldGeoCoordinate,
                                                      Timestamped<GeoCoordinate>  NewGeoCoordinate)
        {

            var OnEVehicleGeoLocationChangedLocal = OnEVehicleGeoLocationChanged;
            if (OnEVehicleGeoLocationChangedLocal != null)
                await OnEVehicleGeoLocationChangedLocal(Timestamp, EventTrackingId,eVehicle, OldGeoCoordinate, NewGeoCoordinate);

        }

        #endregion

        #endregion


        #region Incoming requests from the roaming network

        //#region Receive incoming EVSEData

        //#region PushEVSEData(EVSE,             ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateEVSEData(EVSE                 EVSE,
        //                 ActionType           ActionType,

        //                 DateTime?            Timestamp          = null,
        //                 CancellationToken?   CancellationToken  = null,
        //                 EventTracking_Id     EventTrackingId    = null,
        //                 TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (EVSE == null)
        //        throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(EVSEs,            ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateEVSEData(IEnumerable<EVSE>    EVSEs,
        //                 ActionType           ActionType,

        //                 DateTime?            Timestamp          = null,
        //                 CancellationToken?   CancellationToken  = null,
        //                 EventTracking_Id     EventTrackingId    = null,
        //                 TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (EVSEs == null)
        //        throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(ChargingStation,  ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateChargingStationData(ChargingStation      ChargingStation,
        //                 ActionType           ActionType,

        //                 DateTime?            Timestamp          = null,
        //                 CancellationToken?   CancellationToken  = null,
        //                 EventTracking_Id     EventTrackingId    = null,
        //                 TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStation == null)
        //        throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(ChargingStations, ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateChargingStationData(IEnumerable<ChargingStation>  ChargingStations,
        //                 ActionType                    ActionType,

        //                 DateTime?                     Timestamp          = null,
        //                 CancellationToken?            CancellationToken  = null,
        //                 EventTracking_Id              EventTrackingId    = null,
        //                 TimeSpan?                     RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStations == null)
        //        throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(ChargingPool,     ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateChargingPoolData(ChargingPool         ChargingPool,
        //                 ActionType           ActionType,

        //                 DateTime?            Timestamp          = null,
        //                 CancellationToken?   CancellationToken  = null,
        //                 EventTracking_Id     EventTrackingId    = null,
        //                 TimeSpan?            RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingPool == null)
        //        throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(ChargingPools,    ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateChargingPoolData(IEnumerable<ChargingPool>  ChargingPools,
        //                 ActionType                 ActionType,

        //                 DateTime?                  Timestamp          = null,
        //                 CancellationToken?         CancellationToken  = null,
        //                 EventTracking_Id           EventTrackingId    = null,
        //                 TimeSpan?                  RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingPools == null)
        //        throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(EVSEOperator,     ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateChargingStationOperatorData(ChargingStationOperator  ChargingStationOperator,
        //                 ActionType               ActionType,

        //                 DateTime?                Timestamp          = null,
        //                 CancellationToken?       CancellationToken  = null,
        //                 EventTracking_Id         EventTrackingId    = null,
        //                 TimeSpan?                RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStationOperator == null)
        //        throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(EVSEOperators,    ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateChargingStationOperatorData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,
        //                 ActionType                            ActionType,

        //                 DateTime?                             Timestamp          = null,
        //                 CancellationToken?                    CancellationToken  = null,
        //                 EventTracking_Id                      EventTrackingId    = null,
        //                 TimeSpan?                             RequestTimeout     = null)

        //{

        //    #region Initial checks

        //    if (ChargingStationOperators == null)
        //        throw new ArgumentNullException(nameof(ChargingStationOperators),  "The given enumeration of charging station operators must not be null!");

        //    #endregion

        //    return Task.FromResult(new Acknowledgement(ResultType.True));

        //}

        //#endregion

        //#region PushEVSEData(RoamingNetwork,   ActionType, ...)

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
        //public Task<Acknowledgement>

        //    UpdateRoamingNetworkData(RoamingNetwork       RoamingNetwork,
        //                 ActionType           ActionType,

        //                 DateTime?            Timestamp          = null,
        //                 CancellationToken?   CancellationToken  = null,
        //                 EventTracking_Id     EventTrackingId    = null,
        //                 TimeSpan?            RequestTimeout     = null)

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
        //        Console.WriteLine(DateTime.UtcNow + " LocalEMobilityService says: " + _ChargingStation.Id + " was removed!");

        //}

        //#endregion

        #region Receive incoming EVSEStatus

        //private IRemotePushStatus AsIPushStatus2Remote  => this;

        #region UpdateAdminStatus(AdminStatusUpdates, ...)

        /// <summary>
        /// Receive EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                                 TransmissionTypes                   TransmissionType,

                                                 DateTime?                           Timestamp,
                                                 CancellationToken?                  CancellationToken,
                                                 EventTracking_Id                    EventTrackingId,
                                                 TimeSpan?                           RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null)
                throw new ArgumentNullException(nameof(AdminStatusUpdates), "The given enumeration of EVSE admin status updates must not be null!");


            PushAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushAdminStatusResult.OutOfService(Id,
                                                            this,
                                                            AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                                 TransmissionTypes                              TransmissionType,

                                                 DateTime?                                      Timestamp,
                                                 CancellationToken?                             CancellationToken,
                                                 EventTracking_Id                               EventTrackingId,
                                                 TimeSpan?                                      RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null)
                throw new ArgumentNullException(nameof(AdminStatusUpdates), "The given enumeration of charging station admin status updates must not be null!");


            PushAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                        Timestamp,
                                                                        CancellationToken,
                                                                        EventTrackingId,
                                                                        RequestTimeout);

            else
                result = PushAdminStatusResult.OutOfService(Id,
                                                            this,
                                                            AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                                 TransmissionTypes                           TransmissionType,

                                                 DateTime?                                   Timestamp,
                                                 CancellationToken?                          CancellationToken,
                                                 EventTracking_Id                            EventTrackingId,
                                                 TimeSpan?                                   RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null)
                throw new ArgumentNullException(nameof(AdminStatusUpdates), "The given enumeration of charging pool admin status updates must not be null!");


            PushAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushAdminStatusResult.OutOfService(Id,
                                                            this,
                                                            AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive charging station operator admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                                 TransmissionTypes                                      TransmissionType,

                                                 DateTime?                                              Timestamp,
                                                 CancellationToken?                                     CancellationToken,
                                                 EventTracking_Id                                       EventTrackingId,
                                                 TimeSpan?                                              RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null)
                throw new ArgumentNullException(nameof(AdminStatusUpdates), "The given enumeration of charging station operator admin status updates must not be null!");


            PushAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushAdminStatusResult.OutOfService(Id,
                                                            this,
                                                            AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                                 TransmissionTypes                             TransmissionType,

                                                 DateTime?                                     Timestamp,
                                                 CancellationToken?                            CancellationToken,
                                                 EventTracking_Id                              EventTrackingId,
                                                 TimeSpan?                                     RequestTimeout)

        {

            #region Initial checks

            if (AdminStatusUpdates == null)
                throw new ArgumentNullException(nameof(AdminStatusUpdates), "The given enumeration of roaming network admin status updates must not be null!");


            PushAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushAdminStatusResult.OutOfService(Id,
                                                            this,
                                                            AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }

        #endregion

        #region UpdateStatus(StatusUpdates, ...)

        /// <summary>
        /// Receive EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                            TransmissionTypes              TransmissionType,

                                            DateTime?                      Timestamp,
                                            CancellationToken?             CancellationToken,
                                            EventTracking_Id               EventTrackingId,
                                            TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null)
                throw new ArgumentNullException(nameof(StatusUpdates), "The given enumeration of EVSE status updates must not be null!");


            PushStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                            TransmissionTypes                         TransmissionType,

                                            DateTime?                                 Timestamp,
                                            CancellationToken?                        CancellationToken,
                                            EventTracking_Id                          EventTrackingId,
                                            TimeSpan?                                 RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null)
                throw new ArgumentNullException(nameof(StatusUpdates), "The given enumeration of charging station status updates must not be null!");


            PushStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                            TransmissionTypes                      TransmissionType,

                                            DateTime?                              Timestamp,
                                            CancellationToken?                     CancellationToken,
                                            EventTracking_Id                       EventTrackingId,
                                            TimeSpan?                              RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null)
                throw new ArgumentNullException(nameof(StatusUpdates), "The given enumeration of charging pool status updates must not be null!");


            PushStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                            TransmissionTypes                                 TransmissionType,

                                            DateTime?                                         Timestamp,
                                            CancellationToken?                                CancellationToken,
                                            EventTracking_Id                                  EventTrackingId,
                                            TimeSpan?                                         RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null)
                throw new ArgumentNullException(nameof(StatusUpdates), "The given enumeration of charging station operator status updates must not be null!");


            PushStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }


        /// <summary>
        /// Receive roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                            TransmissionTypes                        TransmissionType,

                                            DateTime?                                Timestamp,
                                            CancellationToken?                       CancellationToken,
                                            EventTracking_Id                         EventTrackingId,
                                            TimeSpan?                                RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null)
                throw new ArgumentNullException(nameof(StatusUpdates), "The given enumeration of roaming network status updates must not be null!");


            PushStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(DateTime.UtcNow,
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
            //                                    DateTime.UtcNow - Timestamp.Value);

            #endregion

            return result;

        }

        #endregion



        #endregion

        #region Receive incoming AuthStart/-Stop

        #region DisableAuthentication

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableAuthentication            { get; set; }

        #endregion


        #region AuthorizeStart(AuthIdentification,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            AuthStartResult result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                AuthIdentification,
                                                ChargingProduct,
                                                SessionId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            if (!DisableAuthentication && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.AuthorizeStart(AuthIdentification,
                                                                      null,
                                                                      null,
                                                                      null,

                                                                      Timestamp,
                                                                      CancellationToken,
                                                                      EventTrackingId,
                                                                      RequestTimeout);

            else
                result = AuthStartResult.OutOfService(Id,
                                                      this,
                                                      SessionId,
                                                      TimeSpan.FromSeconds(0));

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 OperatorId,
                                                 AuthIdentification,
                                                 ChargingProduct,
                                                 SessionId,
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           EVSE_Id                      EVSEId,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            AuthStartEVSEResult result = null;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    AuthIdentification,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeEVSEStartRequest));
            }

            #endregion


            if (!DisableAuthentication && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.AuthorizeStart(AuthIdentification,
                                                                      EVSEId,
                                                                      null,
                                                                      null,
                                                                      null,

                                                                      Timestamp,
                                                                      CancellationToken,
                                                                      EventTrackingId,
                                                                      RequestTimeout);

            else
                result = AuthStartEVSEResult.OutOfService(Id,
                                                          this,
                                                          SessionId,
                                                          TimeSpan.FromSeconds(0));

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeEVSEStartResponse event

            try
            {

                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id.ToString(),
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     OperatorId,
                                                     AuthIdentification,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           ChargingStation_Id           ChargingStationId,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               AuthIdentification,
                                                               ChargingStationId,
                                                               ChargingProduct,
                                                               SessionId,
                                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingStationStartRequest));
            }

            #endregion


            var result   = AuthStartChargingStationResult.OutOfService(Id,
                                                                       this,
                                                                       SessionId,
                                                                       TimeSpan.FromSeconds(0));

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingStationStartResponse event

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
                                                                Timestamp.Value,
                                                                this,
                                                                Id.ToString(),
                                                                EventTrackingId,
                                                                RoamingNetwork.Id,
                                                                OperatorId,
                                                                AuthIdentification,
                                                                ChargingStationId,
                                                                ChargingProduct,
                                                                SessionId,
                                                                RequestTimeout,
                                                                result,
                                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(AuthIdentification, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging pool.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification charging pool.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingPoolResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           ChargingPool_Id              ChargingPoolId,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            AuthIdentification,
                                                            ChargingPoolId,
                                                            ChargingProduct,
                                                            SessionId,
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingPoolStartRequest));
            }

            #endregion


            var result   = AuthStartChargingPoolResult.OutOfService(Id,
                                                                    this,
                                                                    SessionId,
                                                                    TimeSpan.FromSeconds(0));

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingPoolStartResponse event

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id.ToString(),
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             OperatorId,
                                                             AuthIdentification,
                                                             ChargingPoolId,
                                                             ChargingProduct,
                                                             SessionId,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingPoolStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        #region AuthorizeStop(SessionId, AuthIdentification,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            AuthStopResult result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               Id.ToString(),
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               OperatorId,
                                               SessionId,
                                               AuthIdentification,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            if (!DisableAuthentication && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.AuthorizeStop(SessionId,
                                                                     AuthIdentification,
                                                                     null,

                                                                     Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     RequestTimeout);

            else
                result = AuthStopResult.OutOfService(Id,
                                                     this,
                                                     SessionId,
                                                     TimeSpan.FromSeconds(0));

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                SessionId,
                                                AuthIdentification,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          EVSE_Id                      EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification  == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            AuthStopEVSEResult result = null;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id.ToString(),
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   AuthIdentification,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeEVSEStopRequest));
            }

            #endregion


            if (!DisableAuthentication && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.AuthorizeStop(SessionId,
                                                                     AuthIdentification,
                                                                     EVSEId,
                                                                     null,

                                                                     Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     RequestTimeout);

            else
                result = AuthStopEVSEResult.OutOfService(Id,
                                                         this,
                                                         SessionId,
                                                         TimeSpan.FromSeconds(0));

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeEVSEStopResponse event

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    AuthIdentification,
                                                    RequestTimeout,
                                                    result,
                                                    Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingStation_Id           ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              Id.ToString(),
                                                              EventTrackingId,
                                                              RoamingNetwork.Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              AuthIdentification,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            var result   = AuthStopChargingStationResult.OutOfService(
                               Id,
                               this,
                               SessionId,
                               TimeSpan.FromSeconds(0)
                           );

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               AuthIdentification,
                                                               RequestTimeout,
                                                               result,
                                                               Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingStationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging pool.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingPool_Id              ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
                                                           Timestamp.Value,
                                                           this,
                                                           Id.ToString(),
                                                           EventTrackingId,
                                                           RoamingNetwork.Id,
                                                           OperatorId,
                                                           ChargingPoolId,
                                                           SessionId,
                                                           AuthIdentification,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            var result   = AuthStopChargingPoolResult.OutOfService(
                               Id,
                               this,
                               SessionId,
                               TimeSpan.FromSeconds(0)
                           );

            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;


            #region Send OnAuthorizeChargingPoolStopResponse event

            try
            {

                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            ChargingPoolId,
                                                            SessionId,
                                                            AuthIdentification,
                                                            RequestTimeout,
                                                            result,
                                                            Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region SendChargeDetailRecord(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType,

                                    DateTime?                        Timestamp,
                                    CancellationToken?               CancellationToken,
                                    EventTracking_Id                 EventTrackingId,
                                    TimeSpan?                        RequestTimeout)
        {

            if (!DisableSendChargeDetailRecords && RemoteEMobilityProvider != null)
                return await RemoteEMobilityProvider.SendChargeDetailRecords(ChargeDetailRecords,

                                                                             Timestamp,
                                                                             CancellationToken,
                                                                             EventTrackingId,
                                                                             RequestTimeout);

            return SendCDRsResult.OutOfService(Id, this, ChargeDetailRecords);

        }

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
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
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
                    ChargingProduct                   ChargingProduct     = null,
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
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnReserveEVSERequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnReserveEVSERequest?.Invoke(DateTime.UtcNow,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ReservationId,
                                             EVSEId,
                                             StartTime,
                                             Duration,
                                             Id,
                                             eMAId,
                                             ChargingProduct,
                                             AuthTokens,
                                             eMAIds,
                                             PINs,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnReserveEVSERequest));
            }

            #endregion


            var response = await RoamingNetwork.
                                     Reserve(EVSEId,
                                             StartTime,
                                             Duration,
                                             ReservationId,
                                             Id,
                                             eMAId,
                                             ChargingProduct,
                                             AuthTokens,
                                             eMAIds,
                                             PINs,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout);


            #region Send OnReserveEVSEResponse event

            Runtime.Stop();

            try
            {

                OnReserveEVSEResponse?.Invoke(DateTime.UtcNow,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ReservationId,
                                              EVSEId,
                                              StartTime,
                                              Duration,
                                              Id,
                                              eMAId,
                                              ChargingProduct,
                                              AuthTokens,
                                              eMAIds,
                                              PINs,
                                              response,
                                              Runtime.Elapsed,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnReserveEVSEResponse));
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
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            var response = await RoamingNetwork.CancelReservation(ReservationId,
                                                                  Reason,
                                                                  Id,
                                                                  EVSEId ?? new EVSE_Id?(),

                                                                  Timestamp,
                                                                  CancellationToken,
                                                                  EventTrackingId,
                                                                  RequestTimeout);


            //var OnReservationCancelledLocal = OnReservationCancelled;
            //if (OnReservationCancelledLocal != null)
            //    OnReservationCancelledLocal(DateTime.UtcNow,
            //                                this,
            //                                EventTracking_Id.New,
            //                                ReservationId,
            //                                Reason);

            return response;

        }

        #endregion


        #region RemoteStart(...EVSEId, ChargingProduct = null, ReservationId = null, SessionId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
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
                        ChargingProduct          ChargingProduct     = null,
                        ChargingReservation_Id?  ReservationId       = null,
                        ChargingSession_Id?      SessionId           = null,
                        eMobilityAccount_Id?     eMAId               = null,

                        DateTime?                Timestamp           = null,
                        CancellationToken?       CancellationToken   = null,
                        EventTracking_Id         EventTrackingId     = null,
                        TimeSpan?                RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given EVSE identification must not be null!");

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStartRequest event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStartRequest?.Invoke(DateTime.UtcNow,
                                                 Timestamp.Value,
                                                 this,
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 EVSEId,
                                                 ChargingProduct,
                                                 ReservationId,
                                                 SessionId,
                                                 Id,
                                                 eMAId,
                                                 RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnRemoteEVSEStartRequest));
            }

            #endregion


            var response = await RoamingNetwork.
                                     RemoteStart(EVSEId,
                                                 ChargingProduct,
                                                 ReservationId,
                                                 SessionId,
                                                 Id,
                                                 eMAId,

                                                 Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout);


            #region Send OnRemoteEVSEStartResponse event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStartResponse?.Invoke(DateTime.UtcNow,
                                                  Timestamp.Value,
                                                  this,
                                                  EventTrackingId,
                                                  RoamingNetwork.Id,
                                                  EVSEId,
                                                  ChargingProduct,
                                                  ReservationId,
                                                  SessionId,
                                                  Id,
                                                  eMAId,
                                                  RequestTimeout,
                                                  response,
                                                  Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnRemoteEVSEStartResponse));
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
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
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
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            #endregion

            #region Send OnRemoteEVSEStop event

            var Runtime = Stopwatch.StartNew();

            try
            {

                OnRemoteEVSEStop?.Invoke(DateTime.UtcNow,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         EVSEId,
                                         SessionId,
                                         ReservationHandling,
                                         Id,
                                         eMAId,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnRemoteEVSEStop));
            }

            #endregion


            var response = await RoamingNetwork.RemoteStop(EVSEId,
                                                           SessionId,
                                                           ReservationHandling,
                                                           Id,
                                                           eMAId,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


            #region Send OnRemoteEVSEStopped event

            Runtime.Stop();

            try
            {

                OnRemoteEVSEStopped?.Invoke(DateTime.UtcNow,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            EVSEId,
                                            SessionId,
                                            ReservationHandling,
                                            Id,
                                            eMAId,
                                            RequestTimeout,
                                            response,
                                            Runtime.Elapsed);

            }
            catch (Exception e)
            {
                e.Log(nameof(eMobilityProvider) + "." + nameof(OnRemoteEVSEStopped));
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
            var EVSE_Operator = Object as eMobilityProvider;
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
        public Int32 CompareTo(eMobilityProvider EVSE_Operator)
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
            var EVSE_Operator = Object as eMobilityProvider;
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
        public Boolean Equals(eMobilityProvider EVSE_Operator)
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
