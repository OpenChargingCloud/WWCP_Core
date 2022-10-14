/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
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
    public class eMobilityProvider : ACryptoEMobilityEntity<eMobilityProvider_Id,
                                                            eMobilityProviderAdminStatusTypes,
                                                            eMobilityProviderStatusTypes>,
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

        IId IAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

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

        #region DataLicense

        private ReactiveSet<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public ReactiveSet<DataLicense> DataLicenses
        {

            get
            {

                return _DataLicenses != null && _DataLicenses.Any()
                           ? _DataLicenses
                           : RoamingNetwork?.DataLicenses;

            }

            set
            {

                if (value != _DataLicenses && value != RoamingNetwork?.DataLicenses)
                {

                    if (value.IsNullOrEmpty())
                        DeleteProperty(ref _DataLicenses);

                    else
                    {

                        if (_DataLicenses == null)
                            SetProperty(ref _DataLicenses, value);

                        else
                            SetProperty(ref _DataLicenses, _DataLicenses.Set(value));

                    }

                }

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
                    _Address = value;

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
                    value = new GeoCoordinate(Latitude.Parse(0), Longitude.Parse(0));

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

        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        public ChargeDetailRecordFilterDelegate  ChargeDetailRecordFilter    { get; }

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
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion


        /// <summary>
        /// An event fired whenever a new charge detail record was sent.
        /// </summary>
        public event OnSendCDRsResponseDelegate OnSendCDRsResponse;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility (service) provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="Id">The unique e-mobility provider identification.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal eMobilityProvider(eMobilityProvider_Id                     Id,
                                   RoamingNetwork                           RoamingNetwork,

                                   I18NString?                              Name                             = null,
                                   I18NString?                              Description                      = null,
                                   Action<eMobilityProvider>?               Configurator                     = null,
                                   RemoteEMobilityProviderCreatorDelegate?  RemoteEMobilityProviderCreator   = null,
                                   eMobilityProviderPriority?               Priority                         = null,
                                   eMobilityProviderAdminStatusTypes?       InitialAdminStatus               = null,
                                   eMobilityProviderStatusTypes?            InitialStatus                    = null,
                                   UInt16?                                  MaxAdminStatusScheduleSize       = null,
                                   UInt16?                                  MaxStatusScheduleSize            = null,

                                   String?                                  DataSource                       = null,
                                   DateTime?                                LastChange                       = null,

                                   JObject?                                 CustomData                       = null,
                                   UserDefinedDictionary?                   InternalData                     = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus         ?? eMobilityProviderAdminStatusTypes.Operational,
                   InitialStatus              ?? eMobilityProviderStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Initial checks


            #endregion

            #region Init data and properties

            this._DataLicenses                = new ReactiveSet<DataLicense>();

            this.Priority                     = Priority    ?? new eMobilityProviderPriority(0);

            this.ChargeDetailRecordFilter     = ChargeDetailRecordFilter ?? (cdr => ChargeDetailRecordFilters.forward);

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

        #region eMobilityStationAdminStatus

        public IEnumerable<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusTypes>> eMobilityStationAdminStatus

            => _eMobilityStations.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusTypes>(vehicle.Id, vehicle.AdminStatus.Value));

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
                                                          eMobilityStationAdminStatusTypes                 AdminStatus                    = eMobilityStationAdminStatusTypes.Operational,
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


            if (eMobilityStationAddition.SendVoting(Timestamp.Now, this, _eMobilityStation))
            {
                if (_eMobilityStations.TryAdd(_eMobilityStation))
                {

                    _eMobilityStation.OnDataChanged                        += UpdateeMobilityStationData;
                    _eMobilityStation.OnAdminStatusChanged                 += UpdateeMobilityStationAdminStatus;

                    //_eMobilityStation.OnNewReservation                     += SendNewReservation;
                    //_eMobilityStation.OnCancelReservationResponse               += SendOnCancelReservationResponse;
                    //_eMobilityStation.OnNewChargingSession                 += SendNewChargingSession;
                    //_eMobilityStation.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


                    OnSuccess?.Invoke(_eMobilityStation);
                    eMobilityStationAddition.SendNotification(Timestamp.Now, this, _eMobilityStation);

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

                if (eMobilityStationRemoval.SendVoting(Timestamp.Now, this, _eMobilityStation))
                {

                    if (_eMobilityStations.TryRemove(eMobilityStationId, out _eMobilityStation))
                    {

                        eMobilityStationRemoval.SendNotification(Timestamp.Now, this, _eMobilityStation);

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

                if (eMobilityStationRemoval.SendVoting(Timestamp.Now, this, eMobilityStation))
                {

                    if (_eMobilityStations.TryRemove(eMobilityStationId, out eMobilityStation))
                    {

                        eMobilityStationRemoval.SendNotification(Timestamp.Now, this, eMobilityStation);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, NewStatus)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id                            eMobilityStationId,
                                                   Timestamped<eMobilityStationAdminStatusTypes>  NewStatus,
                                                   Boolean                                        SendUpstream = false)
        {

            if (TryGeteMobilityStationById(eMobilityStationId, out var eMobilityStation) &&
                eMobilityStation is not null)
            {
                eMobilityStation.AdminStatus = NewStatus;
            }

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, NewStatus, Timestamp)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id               eMobilityStationId,
                                                   eMobilityStationAdminStatusTypes  NewStatus,
                                                   DateTime                          Timestamp)
        {

            if (TryGeteMobilityStationById(eMobilityStationId, out var eMobilityStation) &&
                eMobilityStation is not null)
            {
                eMobilityStation.AdminStatus = new Timestamped<eMobilityStationAdminStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id                                        eMobilityStationId,
                                                   IEnumerable<Timestamped<eMobilityStationAdminStatusTypes>>  StatusList,
                                                   ChangeMethods                                              ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGeteMobilityStationById(eMobilityStationId, out var eMobilityStation) &&
                eMobilityStation is not null)
            {
                eMobilityStation.SetAdminStatus(StatusList, ChangeMethod);
            }

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendeMobilityStationAdminStatusDiff(new eMobilityStationAdminStatusDiff(Timestamp.Now,
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
                                                              Timestamped<eMobilityStationAdminStatusTypes>  OldStatus,
                                                              Timestamped<eMobilityStationAdminStatusTypes>  NewStatus)
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

        private readonly EntityHashSet<ChargingStationOperator, eVehicle_Id, eVehicle> _eVehicles;

        public IEnumerable<eVehicle> eVehicles

            => _eVehicles;

        #endregion

        #region eVehicleAdminStatus

        public IEnumerable<KeyValuePair<eVehicle_Id, eVehicleAdminStatusTypes>> eVehicleAdminStatus

            => _eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<eVehicle_Id, eVehicleAdminStatusTypes>(vehicle.Id, vehicle.AdminStatus.Value));

        #endregion

        #region eVehicleStatus

        public IEnumerable<KeyValuePair<eVehicle_Id, eVehicleStatusTypes>> eVehicleStatus

            => _eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select (vehicle => new KeyValuePair<eVehicle_Id, eVehicleStatusTypes>(vehicle.Id, vehicle.Status.Value));

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
                                          eVehicleAdminStatusTypes                 AdminStatus            = eVehicleAdminStatusTypes.Operational,
                                          eVehicleStatusTypes                      Status                 = eVehicleStatusTypes.Available,
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


            if (eVehicleAddition.SendVoting(Timestamp.Now, this, _eVehicle))
            {
                if (_eVehicles.TryAdd(_eVehicle))
                {

                    _eVehicle.OnDataChanged                        += UpdateEVehicleData;
                    _eVehicle.OnStatusChanged                      += UpdateEVehicleStatus;
                    _eVehicle.OnAdminStatusChanged                 += UpdateEVehicleAdminStatus;

                    //_eVehicle.OnNewReservation                     += SendNewReservation;
                    //_eVehicle.OnCancelReservationResponse               += SendOnCancelReservationResponse;
                    //_eVehicle.OnNewChargingSession                 += SendNewChargingSession;
                    //_eVehicle.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


                    OnSuccess?.Invoke(_eVehicle);
                    eVehicleAddition.SendNotification(Timestamp.Now, this, _eVehicle);

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

                if (eVehicleRemoval.SendVoting(Timestamp.Now, this, _eVehicle))
                {

                    if (_eVehicles.TryRemove(eVehicleId, out _eVehicle))
                    {

                        eVehicleRemoval.SendNotification(Timestamp.Now, this, _eVehicle);

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

                if (eVehicleRemoval.SendVoting(Timestamp.Now, this, eVehicle))
                {

                    if (_eVehicles.TryRemove(eVehicleId, out eVehicle))
                    {

                        eVehicleRemoval.SendNotification(Timestamp.Now, this, eVehicle);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SeteVehicleAdminStatus(eVehicleId, NewStatus)

        public void SeteVehicleAdminStatus(eVehicle_Id                            eVehicleId,
                                           Timestamped<eVehicleAdminStatusTypes>  NewStatus,
                                           Boolean                                SendUpstream = false)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.AdminStatus = NewStatus;
            }

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, NewStatus, Timestamp)

        public void SetEVehicleAdminStatus(eVehicle_Id               eVehicleId,
                                           eVehicleAdminStatusTypes  NewStatus,
                                           DateTime                  Timestamp)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.AdminStatus = new Timestamped<eVehicleAdminStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVehicleAdminStatus(eVehicle_Id                                        eVehicleId,
                                           IEnumerable<Timestamped<eVehicleAdminStatusTypes>>  StatusList,
                                           ChangeMethods                                      ChangeMethod  = ChangeMethods.Replace)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.SetAdminStatus(StatusList, ChangeMethod);
            }

            //if (SendUpstream)
            //{
            //
            //    RoamingNetwork.
            //        SendeVehicleAdminStatusDiff(new eVehicleAdminStatusDiff(Timestamp.Now,
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
                                                          Timestamped<eVehicleAdminStatusTypes>  OldStatus,
                                                          Timestamped<eVehicleAdminStatusTypes>  NewStatus)
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
                                                     Timestamped<eVehicleStatusTypes>  OldStatus,
                                                     Timestamped<eVehicleStatusTypes>  NewStatus)
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
        //        Console.WriteLine(Timestamp.Now + " LocalEMobilityService says: " + _ChargingStation.Id + " was removed!");

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
        async Task<PushEVSEAdminStatusResult>

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


            PushEVSEAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushEVSEAdminStatusResult.OutOfService(Id,
                                                            this,
                                                            AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushChargingStationAdminStatusResult>

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


            PushChargingStationAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushChargingStationAdminStatusResult.OutOfService(Id,
                                                                           this,
                                                                           AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushChargingPoolAdminStatusResult>

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


            PushChargingPoolAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushChargingPoolAdminStatusResult.OutOfService(Id,
                                                                        this,
                                                                        AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushChargingStationOperatorAdminStatusResult>

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


            PushChargingStationOperatorAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushChargingStationOperatorAdminStatusResult.OutOfService(Id,
                                                                                   this,
                                                                                   AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushRoamingNetworkAdminStatusResult>

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


            PushRoamingNetworkAdminStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateAdminStatus(AdminStatusUpdates,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

            else
                result = PushRoamingNetworkAdminStatusResult.OutOfService(Id,
                                                                          this,
                                                                          AdminStatusUpdates);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushEVSEStatusResult>

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


            PushEVSEStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushEVSEStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushChargingStationStatusResult>

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


            PushChargingStationStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushChargingStationStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushChargingPoolStatusResult>

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


            PushChargingPoolStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushChargingPoolStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushChargingStationOperatorStatusResult>

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


            PushChargingStationOperatorStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushChargingStationOperatorStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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
        async Task<PushRoamingNetworkStatusResult>

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


            PushRoamingNetworkStatusResult result;

            #endregion

            #region Send OnUpdateEVSEStatusRequest event

            // OnUpdateEVSEStatusRequest?.Invoke(StartTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

            #endregion


            if (!DisablePushStatus && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.UpdateStatus(StatusUpdates,

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

            else
                result = PushRoamingNetworkStatusResult.NoOperation(Id, this);


            #region Send OnUpdateEVSEStatusResponse event

            // OnUpdateEVSEStatusResponse?.Invoke(EndTime,
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
            //                                    Timestamp.Now - Timestamp.Value);

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

        #region AuthorizeStart(LocalAuthentication, ChargingLocation = null,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation             ChargingLocation      = null,
                           ChargingProduct              ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTime?                    Timestamp             = null,
                           CancellationToken?           CancellationToken     = null,
                           EventTracking_Id             EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            AuthStartResult result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                null,
                                                null,
                                                OperatorId,
                                                LocalAuthentication,
                                                ChargingLocation,
                                                ChargingProduct,
                                                SessionId,
                                                CPOPartnerSessionId,
                                                new ISendAuthorizeStartStop[0],
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            if (!DisableAuthentication && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.AuthorizeStart(LocalAuthentication,
                                                                      ChargingLocation,
                                                                      ChargingProduct,
                                                                      SessionId,
                                                                      CPOPartnerSessionId,
                                                                      OperatorId,

                                                                      Timestamp,
                                                                      CancellationToken,
                                                                      EventTrackingId,
                                                                      RequestTimeout);

            else
                result = AuthStartResult.OutOfService(Id,
                                                      this,
                                                      SessionId,
                                                      Runtime: TimeSpan.Zero);

            var Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
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
                                                 null,
                                                 null,
                                                 OperatorId,
                                                 LocalAuthentication,
                                                 ChargingLocation,
                                                 ChargingProduct,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 new ISendAuthorizeStartStop[0],
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingLocation = null,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation             ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTime?                    Timestamp             = null,
                          CancellationToken?           CancellationToken     = null,
                          EventTracking_Id             EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null)
        {

            #region Initial checks

            if (LocalAuthentication  == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            AuthStopResult result = null;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                               Timestamp.Value,
                                               this,
                                               Id.ToString(),
                                               EventTrackingId,
                                               RoamingNetwork.Id,
                                               null,
                                               null,
                                               OperatorId,
                                               ChargingLocation,
                                               SessionId,
                                               CPOPartnerSessionId,
                                               LocalAuthentication,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            if (!DisableAuthentication && RemoteEMobilityProvider != null)
                result = await RemoteEMobilityProvider.AuthorizeStop(SessionId,
                                                                     LocalAuthentication,
                                                                     ChargingLocation,
                                                                     CPOPartnerSessionId,
                                                                     OperatorId,

                                                                     Timestamp,
                                                                     CancellationToken,
                                                                     EventTrackingId,
                                                                     RequestTimeout);

            else
                result = AuthStopResult.OutOfService(Id,
                                                     this,
                                                     SessionId,
                                                     Runtime: TimeSpan.Zero);

            var Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
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
                                                null,
                                                null,
                                                OperatorId,
                                                ChargingLocation,
                                                SessionId,
                                                CPOPartnerSessionId,
                                                LocalAuthentication,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnAuthorizeStopResponse));
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

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id                 EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null)
        {

            if (!DisableSendChargeDetailRecords && RemoteEMobilityProvider != null)
                return await RemoteEMobilityProvider.SendChargeDetailRecords(ChargeDetailRecords,

                                                                             Timestamp,
                                                                             CancellationToken,
                                                                             EventTrackingId,
                                                                             RequestTimeout);

            return SendCDRsResult.OutOfService(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                               Id,
                                               this,
                                               ChargeDetailRecords);

        }

        #endregion

        #endregion


        //ToDo: Send Tokens!
        //ToDo: Download CDRs!

        #region Reservations...

        #region Data

        //private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

        ///// <summary>
        ///// All current charging reservations.
        ///// </summary>
        //public IEnumerable<ChargingReservation> Reservations
        //    => _Reservations.Select(_ => _.Value);

        //#region TryGetReservationById(ReservationId, out Reservation)

        ///// <summary>
        ///// Return the charging reservation specified by the given identification.
        ///// </summary>
        ///// <param name="ReservationId">The charging reservation identification.</param>
        ///// <param name="Reservation">The charging reservation.</param>
        //public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
        //    => _Reservations.TryGetValue(ReservationId, out Reservation);

        //#endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate        OnReservationCanceled;

        #endregion

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
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

            Reserve(ChargingLocation                  ChargingLocation,
                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                         ReservationStartTime   = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ReservationResult result = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(StartTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
                                         Duration,
                                         ProviderId,
                                         RemoteAuthentication,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                var response = await RoamingNetwork.
                                         Reserve(ChargingLocation,
                                                 ReservationLevel,
                                                 ReservationStartTime,
                                                 Duration,
                                                 ReservationId,
                                                 Id,
                                                 RemoteAuthentication,
                                                 ChargingProduct,
                                                 AuthTokens,
                                                 eMAIds,
                                                 PINs,

                                                 Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout);


            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          EndTime - StartTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ProviderId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              eMobilityProvider_Id?                  ProviderId         = null,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result                = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                var response = await RoamingNetwork.
                                         CancelReservation(ReservationId,
                                                           Reason,

                                                           Timestamp,
                                                           CancellationToken,
                                                           EventTrackingId,
                                                           RequestTimeout);


                //var OnCancelReservationResponseLocal = OnCancelReservationResponse;
                //if (OnCancelReservationResponseLocal != null)
                //    OnCancelReservationResponseLocal(Timestamp.Now,
                //                                this,
                //                                EventTracking_Id.New,
                //                                ReservationId,
                //                                Reason);


            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region Data

        //private readonly Dictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

        //public IEnumerable<ChargingSession> ChargingSessions
        //    => _ChargingSessions.Select(_ => _.Value);

        //#region TryGetChargingSessionById(SessionId, out ChargingSession)

        ///// <summary>
        ///// Return the charging session specified by the given identification.
        ///// </summary>
        ///// <param name="SessionId">The charging session identification.</param>
        ///// <param name="ChargingSession">The charging session.</param>
        //public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
        //    => _ChargingSessions.TryGetValue(SessionId, out ChargingSession);

        //#endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStartResult result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(StartTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             null,
                                             null,
                                             Id,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                result = await RoamingNetwork.
                                   RemoteStart(ChargingLocation,
                                               ChargingProduct,
                                               ReservationId,
                                               SessionId,
                                               Id,
                                               RemoteAuthentication,
                                               //this,

                                               Timestamp,
                                               CancellationToken,
                                               EventTrackingId,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


            #region Send OnRemoteStartResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(EndTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              null,
                                              null,
                                              Id,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id    SessionId,
                       ReservationHandling?  ReservationHandling    = null,
                       RemoteAuthentication  RemoteAuthentication   = null,

                       DateTime?             Timestamp              = null,
                       CancellationToken?    CancellationToken      = null,
                       EventTracking_Id      EventTrackingId        = null,
                       TimeSpan?             RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            null,
                                            Id,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                result = await RoamingNetwork.
                                   RemoteStop(SessionId,
                                              ReservationHandling,
                                              Id,
                                              RemoteAuthentication,

                                              Timestamp,
                                              CancellationToken,
                                              EventTrackingId,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message);
            }


            #region Send OnRemoteStopResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             SessionId,
                                             ReservationHandling,
                                             null,
                                             null,
                                             Id,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(eMobilityProvider) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion



        #region IComparable<eMobilityProvider> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException("The given object must not be null!");

            if (!(Object is eMobilityProvider eMobilityProvider))
                throw new ArgumentException("The given object is not an eMobilityProvider!");

            return CompareTo(eMobilityProvider);

        }

        #endregion

        #region CompareTo(eMobilityProvider)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProvider">An EVSE_Operator object to compare with.</param>
        public Int32 CompareTo(eMobilityProvider eMobilityProvider)
        {

            if (eMobilityProvider is null)
                throw new ArgumentNullException("The given EVSE_Operator must not be null!");

            return Id.CompareTo(eMobilityProvider.Id);

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityProvider> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is eMobilityProvider eMobilityProvider))
                return false;

            return Equals(eMobilityProvider);

        }

        #endregion

        #region Equals(EVSE_Operator)

        /// <summary>
        /// Compares two eMobilityProviders for equality.
        /// </summary>
        /// <param name="eMobilityProvider">An eMobilityProvider to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityProvider eMobilityProvider)
        {

            if (eMobilityProvider is null)
                return false;

            return Id.Equals(eMobilityProvider.Id);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
