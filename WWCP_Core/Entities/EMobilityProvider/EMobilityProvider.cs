/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this eMobilityProvider,                      Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given e-mobility provider.
        /// </summary>
        /// <param name="eMobilityProvider">An e-mobility provider.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a roaming network.</param>
        public static JObject? ToJSON(this IEMobilityProvider  eMobilityProvider,
                                      Boolean                  Embedded                 = false,
                                      InfoStatus               ExpandRoamingNetworkId   = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandBrandIds           = InfoStatus.ShowIdOnly,
                                      InfoStatus               ExpandDataLicenses       = InfoStatus.ShowIdOnly)


            => eMobilityProvider is null

                   ? null

                   : JSONObject.Create(

                         new JProperty("@id",  eMobilityProvider.Id.ToString()),

                         Embedded
                             ? new JProperty("@context",  "https://open.charging.cloud/contexts/wwcp+json/eMobilityProvider")
                             : null,

                         new JProperty("name",  eMobilityProvider.Name.ToJSON()),

                         eMobilityProvider.Description.IsNotNullOrEmpty()
                             ? new JProperty("description", eMobilityProvider.Description.ToJSON())
                             : null,

                         //eMobilityProvider.DataSource.  ToJSON("DataSource"),

                         ExpandDataLicenses.Switch(
                             () => new JProperty("dataLicenseIds",  new JArray(eMobilityProvider.DataLicenses.SafeSelect(license => license.Id.ToString()))),
                             () => new JProperty("dataLicenses",    eMobilityProvider.DataLicenses.ToJSON())),

                         #region Embedded means it is served as a substructure of e.g. a charging station operator

                         Embedded
                             ? null
                             : ExpandRoamingNetworkId.Switch(
                                   () => new JProperty("roamingNetworkId",   eMobilityProvider.RoamingNetwork.Id. ToString()),
                                   () => new JProperty("roamingNetwork",     eMobilityProvider.RoamingNetwork.    ToJSON(Embedded:                   true,
                                                                                                                         ExpandEMobilityProviderId:  InfoStatus.Hidden,
                                                                                                                         ExpandChargingPoolIds:      InfoStatus.Hidden,
                                                                                                                         ExpandChargingStationIds:   InfoStatus.Hidden,
                                                                                                                         ExpandEVSEIds:              InfoStatus.Hidden,
                                                                                                                         ExpandBrandIds:             InfoStatus.Hidden,
                                                                                                                         ExpandDataLicenses:         InfoStatus.Hidden))),

                         #endregion

                         eMobilityProvider.Address is not null
                             ? new JProperty("address",             eMobilityProvider.Address.ToJSON())
                             : null,

                         // LogoURI
                         // API
                         // MainKeys
                         // RobotKeys
                         // Endpoints
                         // DNS SRV

                         eMobilityProvider.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          eMobilityProvider.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         eMobilityProvider.Homepage.HasValue
                             ? new JProperty("homepage",            eMobilityProvider.Homepage.ToString())
                             : null,

                         eMobilityProvider.HotlinePhoneNumber.HasValue
                             ? new JProperty("hotline",             eMobilityProvider.HotlinePhoneNumber.ToString())
                             : null,

                         eMobilityProvider.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(eMobilityProvider.DataLicenses.Select(license => license.ToJSON())))
                             : null

                     );

        #endregion

        #region ToJSON(this eMobilityProviders, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of e-mobility providers.
        /// </summary>
        /// <param name="eMobilityProviders">An enumeration of e-mobility providers.</param>
        /// <param name="Skip">The optional number of e-mobility providers to skip.</param>
        /// <param name="Take">The optional number of e-mobility providers to return.</param>
        public static JArray ToJSON(this IEnumerable<IEMobilityProvider>  eMobilityProviders,
                                    UInt64?                               Skip                     = null,
                                    UInt64?                               Take                     = null,
                                    Boolean                               Embedded                 = false,
                                    InfoStatus                            ExpandRoamingNetworkId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandBrandIds           = InfoStatus.ShowIdOnly,
                                    InfoStatus                            ExpandDataLicenses       = InfoStatus.ShowIdOnly)


            => eMobilityProviders is null

                   ? new JArray()

                   : new JArray(eMobilityProviders.
                                    Where     (emp => emp is not null).
                                    OrderBy   (emp => emp.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect(emp => emp.ToJSON(Embedded,
                                                                 ExpandRoamingNetworkId,
                                                                 ExpandBrandIds,
                                                                 ExpandDataLicenses)));

        #endregion


        #region ToJSON(this eMobilityProviderAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<EMobilityProvider_Id, IEnumerable<Timestamped<EMobilityProviderAdminStatusTypes>>>>  eMobilityProviderAdminStatus,
                                     UInt64?                                                                                                            Skip         = null,
                                     UInt64?                                                                                                            Take         = null,
                                     UInt64                                                                                                             HistorySize  = 1)

        {

            #region Initial checks

            if (eMobilityProviderAdminStatus is null || !eMobilityProviderAdminStatus.Any())
                return new JObject();

            var _eMobilityProviderAdminStatus = new Dictionary<EMobilityProvider_Id, IEnumerable<Timestamped<EMobilityProviderAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate eMobilityProvider identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take.HasValue ? eMobilityProviderAdminStatus.Skip(Skip).Take(Take)
                                                    : eMobilityProviderAdminStatus.Skip(Skip))
            {

                if (!_eMobilityProviderAdminStatus.ContainsKey(csostatus.Key))
                    _eMobilityProviderAdminStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _eMobilityProviderAdminStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _eMobilityProviderAdminStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _eMobilityProviderAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_eMobilityProviderAdminStatus.
                                     SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                  new JObject(statuslist.Value.

                                                                              // Will filter multiple cso status having the exact same ISO 8601 timestamp!
                                                                              GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                                                              Select           (group => group.First()).

                                                                              OrderByDescending(tsv   => tsv.Timestamp).
                                                                              Take             (HistorySize).
                                                                              Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                                                       tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


        #region ToJSON(this eMobilityProviderStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<EMobilityProvider_Id, IEnumerable<Timestamped<EMobilityProviderStatusTypes>>>>  eMobilityProviderStatus,
                                     UInt64?                                                                                                       Skip         = null,
                                     UInt64?                                                                                                       Take         = null,
                                     UInt64?                                                                                                       HistorySize  = 1)

        {

            #region Initial checks

            if (eMobilityProviderStatus is null || !eMobilityProviderStatus.Any())
                return new JObject();

            var _eMobilityProviderStatus = new Dictionary<EMobilityProvider_Id, IEnumerable<Timestamped<EMobilityProviderStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate eMobilityProvider identifications in the enumeration... take the newest one!

            foreach (var csostatus in Take.HasValue ? eMobilityProviderStatus.Skip(Skip).Take(Take)
                                                    : eMobilityProviderStatus.Skip(Skip))
            {

                if (!_eMobilityProviderStatus.ContainsKey(csostatus.Key))
                    _eMobilityProviderStatus.Add(csostatus.Key, csostatus.Value);

                else if (csostatus.Value.FirstOrDefault().Timestamp > _eMobilityProviderStatus[csostatus.Key].FirstOrDefault().Timestamp)
                    _eMobilityProviderStatus[csostatus.Key] = csostatus.Value;

            }

            #endregion

            return _eMobilityProviderStatus.Count == 0

                   ? new JObject()

                   : new JObject(_eMobilityProviderStatus.
                                     SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                  new JObject(statuslist.Value.

                                                                              // Will filter multiple cso status having the exact same ISO 8601 timestamp!
                                                                              GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                                                              Select           (group => group.First()).

                                                                              OrderByDescending(tsv   => tsv.Timestamp).
                                                                              Take             (HistorySize).
                                                                              Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                                                       tsv.Value.    ToString())))

                                                              )));

        }

        #endregion


    }


    /// <summary>
    /// An E-Mobility Provider for lookups which allows to connect
    /// an optional remote E-Mobility Provider.
    /// </summary>
    public class EMobilityProvider : ACryptoEMobilityEntity<EMobilityProvider_Id,
                                                            EMobilityProviderAdminStatusTypes,
                                                            EMobilityProviderStatusTypes>,
                                     IEMobilityProvider
    {

        #region Data

        #endregion

        #region Properties

        IId IAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.SendChargeDetailRecordsId
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

                return _DataLicenses is not null && _DataLicenses.Any()
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

                        if (_DataLicenses is null)
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

                if (value is null)
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

                //if (value is null)
                //    value = new GeoCoordinate(Latitude.Parse(0), Longitude.Parse(0));

                if (_GeoLocation != value)
                    SetProperty(ref _GeoLocation, value);

            }

        }

        #endregion

        #region Telephone

        private PhoneNumber? _Telephone;

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        [Optional]
        public PhoneNumber? Telephone
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

        private SimpleEMailAddress? _EMailAddress;

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        [Optional]
        public SimpleEMailAddress? EMailAddress
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

        private URL? _Homepage;

        /// <summary>
        /// The homepage of this evse operator.
        /// </summary>
        [Optional]
        public URL? Homepage
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

        private PhoneNumber? _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        [Optional]
        public PhoneNumber? HotlinePhoneNumber
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


        public EMobilityProviderPriority Priority { get; set; }

        public Boolean DisableSendAdminStatus { get; set; }

        public Boolean DisableSendStatus { get; set; }

        public Boolean DisableSendChargeDetailRecords { get; set; }



        //#region AllTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> AllTokens

        //    => RemoteEMobilityProvider is not null
        //           ? RemoteEMobilityProvider.AllTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        //#region AuthorizedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> AuthorizedTokens

        //    => RemoteEMobilityProvider is not null
        //           ? RemoteEMobilityProvider.AuthorizedTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        //#region NotAuthorizedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> NotAuthorizedTokens

        //    => RemoteEMobilityProvider is not null
        //           ? RemoteEMobilityProvider.NotAuthorizedTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        //#region BlockedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> BlockedTokens

        //    => RemoteEMobilityProvider is not null
        //           ? RemoteEMobilityProvider.BlockedTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        /// <summary>
        /// A delegate for filtering charge detail records.
        /// </summary>
        public ChargeDetailRecordFilterDelegate ChargeDetailRecordFilter { get; }

        #endregion

        #region Links

        /// <summary>
        /// The remote e-mobility provider.
        /// </summary>
        public IRemoteEMobilityProvider RemoteEMobilityProvider { get; }

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
        public event OnAuthorizeStartRequestDelegate OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate OnAuthorizeStopResponse;

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
        internal EMobilityProvider(EMobilityProvider_Id                     Id,
                                   IRoamingNetwork                          RoamingNetwork,

                                   I18NString?                              Name                             = null,
                                   I18NString?                              Description                      = null,
                                   Action<EMobilityProvider>?               Configurator                     = null,
                                   RemoteEMobilityProviderCreatorDelegate?  RemoteEMobilityProviderCreator   = null,
                                   EMobilityProviderPriority?               Priority                         = null,
                                   EMobilityProviderAdminStatusTypes?       InitialAdminStatus               = null,
                                   EMobilityProviderStatusTypes?            InitialStatus                    = null,
                                   UInt16?                                  MaxAdminStatusScheduleSize       = null,
                                   UInt16?                                  MaxStatusScheduleSize            = null,

                                   String?                                  DataSource                       = null,
                                   DateTimeOffset?                          Created                          = null,
                                   DateTimeOffset?                          LastChange                       = null,

                                   JObject?                                 CustomData                       = null,
                                   UserDefinedDictionary?                   InternalData                     = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus         ?? EMobilityProviderAdminStatusTypes.Operational,
                   InitialStatus              ?? EMobilityProviderStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   Created,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Initial checks


            #endregion

            #region Init data and properties

            this._DataLicenses = new ReactiveSet<DataLicense>();

            this.Priority = Priority ?? new EMobilityProviderPriority(0);

            this.ChargeDetailRecordFilter = ChargeDetailRecordFilter ?? (cdr => ChargeDetailRecordFilters.forward);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteEMobilityProvider = RemoteEMobilityProviderCreator?.Invoke(this);

        }

        #endregion


        #region eMobilityStations

        #region eMobilityStationAddition

        internal readonly IVotingNotificator<DateTimeOffset, EMobilityProvider, eMobilityStation, Boolean> eMobilityStationAddition;

        /// <summary>
        /// Called whenever an e-mobility station will be or was added.
        /// </summary>
        public IVotingSender<DateTimeOffset, EMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationAddition

            => eMobilityStationAddition;

        #endregion

        #region eMobilityStationRemoval

        internal readonly IVotingNotificator<DateTimeOffset, EMobilityProvider, eMobilityStation, Boolean> eMobilityStationRemoval;

        /// <summary>
        /// Called whenever an e-mobility station will be or was removed.
        /// </summary>
        public IVotingSender<DateTimeOffset, EMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationRemoval

            => eMobilityStationRemoval;

        #endregion


        #region eMobilityStations

        private EntityHashSet<ChargingStationOperator, eMobilityStation_Id, eMobilityStation> eMobilityStations;

        public IEnumerable<eMobilityStation> EMobilityStations

            => eMobilityStations;

        #endregion

        #region eMobilityStationAdminStatus

        public IEnumerable<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusTypes>> EMobilityStationAdminStatus

            => eMobilityStations.
                   OrderBy(vehicle => vehicle.Id).
                   Select(vehicle => new KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusTypes>(vehicle.Id, vehicle.AdminStatus.Value));

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
        public eMobilityStation CreateNeweMobilityStation(eMobilityStation_Id eMobilityStationId = null,
                                                          Action<eMobilityStation> Configurator = null,
                                                          RemoteEMobilityStationCreatorDelegate RemoteeMobilityStationCreator = null,
                                                          eMobilityStationAdminStatusTypes AdminStatus = eMobilityStationAdminStatusTypes.Operational,
                                                          Action<eMobilityStation> OnSuccess = null,
                                                          Action<EMobilityProvider, eMobilityStation_Id> OnError = null)

        {

            #region Initial checks

            if (eMobilityStationId is null)
                eMobilityStationId = eMobilityStation_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (eMobilityStations.Any(pool => pool.Id == eMobilityStationId))
            {
                if (OnError is null)
                    throw new eMobilityStationAlreadyExists(this, eMobilityStationId);
                else
                    OnError?.Invoke(this, eMobilityStationId);
            }

            #endregion

            var eMobilityStation = new eMobilityStation(eMobilityStationId,
                                                        this,
                                                        Configurator,
                                                        RemoteeMobilityStationCreator,
                                                        AdminStatus);


            if (eMobilityStations.TryAdd(eMobilityStation,
                                         EventTracking_Id.New,
                                         null).Result == CommandResult.Success)
            {

                eMobilityStation.OnDataChanged        += UpdateeMobilityStationData;
                eMobilityStation.OnAdminStatusChanged += UpdateeMobilityStationAdminStatus;

                //_eMobilityStation.OnNewReservation                     += SendNewReservation;
                //_eMobilityStation.OnCancelReservationResponse               += SendOnCancelReservationResponse;
                //_eMobilityStation.OnNewChargingSession                 += SendNewChargingSession;
                //_eMobilityStation.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


                OnSuccess?.Invoke(eMobilityStation);

                return eMobilityStation;

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

            => eMobilityStations.Contains(eMobilityStation);

        #endregion

        #region ContainseMobilityStation(eMobilityStationId)

        /// <summary>
        /// Check if the given eMobilityStation identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eMobilityStationId">The unique identification of the eMobilityStation.</param>
        public Boolean ContainseMobilityStation(eMobilityStation_Id eMobilityStationId)

            => eMobilityStations.ContainsId(eMobilityStationId);

        #endregion

        #region GeteMobilityStationById(eMobilityStationId)

        public eMobilityStation GeteMobilityStationById(eMobilityStation_Id eMobilityStationId)

            => eMobilityStations.GetById(eMobilityStationId);

        #endregion

        #region TryGeteMobilityStationById(eMobilityStationId, out eMobilityStation)

        public Boolean TryGeteMobilityStationById(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation)

            => eMobilityStations.TryGet(eMobilityStationId, out eMobilityStation);

        #endregion

        #region RemoveeMobilityStation(eMobilityStationId)

        public eMobilityStation RemoveeMobilityStation(eMobilityStation_Id eMobilityStationId)
        {

            eMobilityStation _eMobilityStation = null;

            if (TryGeteMobilityStationById(eMobilityStationId, out _eMobilityStation))
            {

                if (eMobilityStationRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now, this, _eMobilityStation))
                {

                    if (eMobilityStations.TryRemove(eMobilityStationId,
                                                     out _eMobilityStation,
                                                     EventTracking_Id.New,
                                                     null))
                    {

                        eMobilityStationRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now, this, _eMobilityStation);

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

                if (eMobilityStationRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now, this, eMobilityStation))
                {

                    if (eMobilityStations.TryRemove(eMobilityStationId,
                                                     out eMobilityStation,
                                                     EventTracking_Id.New,
                                                     null))
                    {

                        eMobilityStationRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now, this, eMobilityStation);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion


        #region SetEMobilityStationAdminStatus(eMobilityStationId, NewStatus)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId,
                                                   Timestamped<eMobilityStationAdminStatusTypes> NewStatus,
                                                   Boolean SendUpstream = false)
        {

            if (TryGeteMobilityStationById(eMobilityStationId, out var eMobilityStation) &&
                eMobilityStation is not null)
            {
                eMobilityStation.AdminStatus = NewStatus;
            }

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, NewStatus, Timestamp)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId,
                                                   eMobilityStationAdminStatusTypes NewStatus,
                                                   DateTimeOffset Timestamp)
        {

            if (TryGeteMobilityStationById(eMobilityStationId, out var eMobilityStation) &&
                eMobilityStation is not null)
            {
                eMobilityStation.AdminStatus = new Timestamped<eMobilityStationAdminStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetEMobilityStationAdminStatus(eMobilityStationId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId,
                                                   IEnumerable<Timestamped<eMobilityStationAdminStatusTypes>> StatusList,
                                                   ChangeMethods ChangeMethod = ChangeMethods.Replace)
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


        #region OnEMobilityStationData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated eMobilityStation changed.
        /// </summary>
        public event OnEMobilityStationDataChangedDelegate?         OnEMobilityStationDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eMobilityStation changed.
        /// </summary>
        public event OnEMobilityStationAdminStatusChangedDelegate?  OnEMobilityStationAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eMobilityStation changed.
        /// </summary>
        public event OnEMobilityStationStatusChangedDelegate?       OnEMobilityStationStatusChanged;

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
        internal async Task UpdateeMobilityStationData(DateTimeOffset    Timestamp,
                                                       EventTracking_Id  EventTrackingId,
                                                       eMobilityStation  eMobilityStation,
                                                       String            PropertyName,
                                                       Object?           NewValue,
                                                       Object?           OldValue     = null,
                                                       Context?          DataSource   = null)
        {

            var onEMobilityStationDataChanged = OnEMobilityStationDataChanged;
            if (onEMobilityStationDataChanged is not null)
                await onEMobilityStationDataChanged(Timestamp,
                                                    EventTrackingId,
                                                    eMobilityStation,
                                                    PropertyName,
                                                    OldValue,
                                                    NewValue,
                                                    DataSource);

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
        internal async Task UpdateeMobilityStationAdminStatus(DateTimeOffset                                  Timestamp,
                                                              EventTracking_Id                                EventTrackingId,
                                                              eMobilityStation                                eMobilityStation,
                                                              Timestamped<eMobilityStationAdminStatusTypes>   NewStatus,
                                                              Timestamped<eMobilityStationAdminStatusTypes>?  OldStatus    = null,
                                                              Context?                                        DataSource   = null)
        {

            var OnEMobilityStationAdminStatusChangedLocal = OnEMobilityStationAdminStatusChanged;
            if (OnEMobilityStationAdminStatusChangedLocal is not null)
                await OnEMobilityStationAdminStatusChangedLocal(Timestamp,
                                                                EventTrackingId,
                                                                eMobilityStation,
                                                                NewStatus,
                                                                OldStatus,
                                                                DataSource);

        }

        #endregion

        #endregion

        #region eVehicles

        #region eVehicleAddition

        internal readonly IVotingNotificator<DateTimeOffset, EMobilityProvider, EVehicle, Boolean> eVehicleAddition;

        /// <summary>
        /// Called whenever an electric vehicle will be or was added.
        /// </summary>
        public IVotingSender<DateTimeOffset, EMobilityProvider, EVehicle, Boolean> OnEVehicleAddition

            => eVehicleAddition;

        #endregion

        #region eVehicleRemoval

        internal readonly IVotingNotificator<DateTimeOffset, EMobilityProvider, EVehicle, Boolean> eVehicleRemoval;

        /// <summary>
        /// Called whenever an electric vehicle will be or was removed.
        /// </summary>
        public IVotingSender<DateTimeOffset, EMobilityProvider, EVehicle, Boolean> OnEVehicleRemoval

            => eVehicleRemoval;

        #endregion


        #region eVehicles

        private readonly EntityHashSet<ChargingStationOperator, EVehicle_Id, EVehicle> eVehicles;

        public IEnumerable<EVehicle> EVehicles

            => eVehicles;

        #endregion

        #region eVehicleAdminStatus

        public IEnumerable<KeyValuePair<EVehicle_Id, eVehicleAdminStatusTypes>> EVehicleAdminStatus

            => eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select(vehicle => new KeyValuePair<EVehicle_Id, eVehicleAdminStatusTypes>(vehicle.Id, vehicle.AdminStatus.Value));

        #endregion

        #region eVehicleStatus

        public IEnumerable<KeyValuePair<EVehicle_Id, eVehicleStatusTypes>> EVehicleStatus

            => eVehicles.
                   OrderBy(vehicle => vehicle.Id).
                   Select(vehicle => new KeyValuePair<EVehicle_Id, eVehicleStatusTypes>(vehicle.Id, vehicle.Status.Value));

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
        public EVehicle CreateNeweVehicle(EVehicle_Id eVehicleId = null,
                                          Action<EVehicle> Configurator = null,
                                          RemoteEVehicleCreatorDelegate RemoteeVehicleCreator = null,
                                          eVehicleAdminStatusTypes AdminStatus = eVehicleAdminStatusTypes.Operational,
                                          eVehicleStatusTypes Status = eVehicleStatusTypes.Available,
                                          Action<EVehicle> OnSuccess = null,
                                          Action<EMobilityProvider, EVehicle_Id> OnError = null)

        {

            #region Initial checks

            if (eVehicleId is null)
                eVehicleId = EVehicle_Id.Random(this.Id);

            // Do not throw an exception when an OnError delegate was given!
            if (eVehicles.Any(pool => pool.Id == eVehicleId))
            {
                if (OnError is null)
                    throw new eVehicleAlreadyExists(this, eVehicleId);
                else
                    OnError?.Invoke(this, eVehicleId);
            }

            #endregion

            var eVehicle = new EVehicle(eVehicleId,
                                        this,
                                        Configurator,
                                        RemoteeVehicleCreator,
                                        AdminStatus,
                                        Status);


            if (eVehicles.TryAdd(eVehicle,
                                 EventTracking_Id.New,
                                 null).Result == CommandResult.Success)
            {

                eVehicle.OnDataChanged         += UpdateEVehicleData;
                eVehicle.OnStatusChanged       += UpdateEVehicleStatus;
                eVehicle.OnAdminStatusChanged  += UpdateEVehicleAdminStatus;

                //_eVehicle.OnNewReservation                     += SendNewReservation;
                //_eVehicle.OnCancelReservationResponse               += SendOnCancelReservationResponse;
                //_eVehicle.OnNewChargingSession                 += SendNewChargingSession;
                //_eVehicle.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(eVehicle);

                return eVehicle;

            }

            return null;

        }

        #endregion


        #region ContainseVehicle(eVehicle)

        /// <summary>
        /// Check if the given eVehicle is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eVehicle">A eVehicle.</param>
        public Boolean ContainseVehicle(EVehicle eVehicle)

            => eVehicles.Contains(eVehicle);

        #endregion

        #region ContainseVehicle(eVehicleId)

        /// <summary>
        /// Check if the given eVehicle identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="eVehicleId">The unique identification of the eVehicle.</param>
        public Boolean ContainseVehicle(EVehicle_Id eVehicleId)

            => eVehicles.ContainsId(eVehicleId);

        #endregion

        #region GetEVehicleById(eVehicleId)

        public EVehicle GetEVehicleById(EVehicle_Id eVehicleId)

            => eVehicles.GetById(eVehicleId);

        #endregion

        #region TryGetEVehicleById(eVehicleId, out eVehicle)

        public Boolean TryGetEVehicleById(EVehicle_Id eVehicleId, out EVehicle eVehicle)

            => eVehicles.TryGet(eVehicleId, out eVehicle);

        #endregion

        #region RemoveEVehicle(eVehicleId)

        public EVehicle RemoveEVehicle(EVehicle_Id eVehicleId)
        {

            EVehicle _eVehicle = null;

            if (TryGetEVehicleById(eVehicleId, out _eVehicle))
            {

                if (eVehicleRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now, this, _eVehicle))
                {

                    if (eVehicles.TryRemove(eVehicleId,
                                             out _eVehicle,
                                             EventTracking_Id.New,
                                             null))
                    {

                        eVehicleRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now, this, _eVehicle);

                        return _eVehicle;

                    }

                }

            }

            return null;

        }

        #endregion

        #region TryRemoveEVehicle(eVehicleId, out eVehicle)

        public Boolean TryRemoveEVehicle(EVehicle_Id eVehicleId, out EVehicle eVehicle)
        {

            if (TryGetEVehicleById(eVehicleId, out eVehicle))
            {

                if (eVehicleRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now, this, eVehicle))
                {

                    if (eVehicles.TryRemove(eVehicleId,
                                             out eVehicle,
                                             EventTracking_Id.New,
                                             null))
                    {

                        eVehicleRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now, this, eVehicle);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SeteVehicleAdminStatus(eVehicleId, NewStatus)

        public void SeteVehicleAdminStatus(EVehicle_Id eVehicleId,
                                           Timestamped<eVehicleAdminStatusTypes> NewStatus,
                                           Boolean SendUpstream = false)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.AdminStatus = NewStatus;
            }

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, NewStatus, Timestamp)

        public void SetEVehicleAdminStatus(EVehicle_Id               eVehicleId,
                                           eVehicleAdminStatusTypes  NewStatus,
                                           DateTimeOffset            Timestamp)
        {

            if (TryGetEVehicleById(eVehicleId, out var eVehicle) &&
                eVehicle is not null)
            {
                eVehicle.AdminStatus = new Timestamped<eVehicleAdminStatusTypes>(Timestamp, NewStatus);
            }

        }

        #endregion

        #region SetEVehicleAdminStatus(eVehicleId, StatusList, ChangeMethod = ChangeMethods.Replace)

        public void SetEVehicleAdminStatus(EVehicle_Id eVehicleId,
                                           IEnumerable<Timestamped<eVehicleAdminStatusTypes>> StatusList,
                                           ChangeMethods ChangeMethod = ChangeMethods.Replace)
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
        public event OnEVehicleDataChangedDelegate OnEVehicleDataChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleStatusChangedDelegate OnEVehicleStatusChanged;

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of any subordinated eVehicle changed.
        /// </summary>
        public event OnEVehicleAdminStatusChangedDelegate OnEVehicleAdminStatusChanged;

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
        internal async Task UpdateEVehicleData(DateTimeOffset Timestamp,
                                               EventTracking_Id? EventTrackingId,
                                               EVehicle eVehicle,
                                               String PropertyName,
                                               Object? NewValue,
                                               Object? OldValue = null,
                                               Context? DataSource = null)
        {

            var onEVehicleDataChanged = OnEVehicleDataChanged;
            if (onEVehicleDataChanged is not null)
                await onEVehicleDataChanged(Timestamp, EventTrackingId, eVehicle, PropertyName, NewValue, OldValue, DataSource);

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
        internal async Task UpdateEVehicleAdminStatus(DateTimeOffset Timestamp,
                                                      EventTracking_Id? EventTrackingId,
                                                      EVehicle eVehicle,
                                                      Timestamped<eVehicleAdminStatusTypes>   NewStatus,
                                                      Timestamped<eVehicleAdminStatusTypes>?  OldStatus    = null,
                                                      Context?                                DataSource   = null)
        {

            var onEVehicleAdminStatusChanged = OnEVehicleAdminStatusChanged;
            if (onEVehicleAdminStatusChanged is not null)
                await onEVehicleAdminStatusChanged(Timestamp, EventTrackingId, eVehicle, NewStatus, OldStatus, DataSource);

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
        internal async Task UpdateEVehicleStatus(DateTimeOffset Timestamp,
                                                 EventTracking_Id? EventTrackingId,
                                                 EVehicle eVehicle,
                                                 Timestamped<eVehicleStatusTypes>   NewStatus,
                                                 Timestamped<eVehicleStatusTypes>?  OldStatus    = null,
                                                 Context?                            DataSource   = null)
        {

            var onEVehicleStatusChanged = OnEVehicleStatusChanged;
            if (onEVehicleStatusChanged is not null)
                await onEVehicleStatusChanged(Timestamp, EventTrackingId, eVehicle, NewStatus, OldStatus, DataSource);

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
        internal async Task UpdateEVehicleGeoLocation(DateTimeOffset Timestamp,
                                                      EventTracking_Id? EventTrackingId,
                                                      EVehicle eVehicle,
                                                      Timestamped<GeoCoordinate>   NewGeoCoordinate,
                                                      Timestamped<GeoCoordinate>?  OldGeoCoordinate   = null,
                                                      Context?                      DataSource         = null)
        {

            var onEVehicleGeoLocationChanged = OnEVehicleGeoLocationChanged;
            if (onEVehicleGeoLocationChanged is not null)
                await onEVehicleGeoLocationChanged(Timestamp, EventTrackingId, eVehicle, NewGeoCoordinate, OldGeoCoordinate, DataSource);

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

        //    if (EVSE is null)
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

        //    if (EVSEs is null)
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

        //    if (ChargingStation is null)
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

        //    if (ChargingStations is null)
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

        //    if (ChargingPool is null)
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

        //    if (ChargingPools is null)
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

        //    if (ChargingStationOperator is null)
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

        //    if (ChargingStationOperators is null)
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

        //    if (RoamingNetwork is null)
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

        #region UpdateEVSEAdminStatus(EVSEAdminStatusUpdates, ...)

        /// <summary>
        /// Receive EVSE admin status updates.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushEVSEAdminStatusResult>

            ISendAdminStatus.UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  EVSEAdminStatusUpdates,
                                                   TransmissionTypes                   TransmissionType,

                                                   DateTimeOffset?                     Timestamp,
                                                   EventTracking_Id?                   EventTrackingId,
                                                   TimeSpan?                           RequestTimeout,
                                                   User_Id?                            CurrentUserId,
                                                   CancellationToken                   CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateEVSEAdminStatus(EVSEAdminStatusUpdates,

                                                                             Timestamp,
                                                                             EventTrackingId,
                                                                             RequestTimeout,
                                                                             CancellationToken);

            else
                result = PushEVSEAdminStatusResult.OutOfService(Id,
                                                                this,
                                                                EVSEAdminStatusUpdates);


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
        /// Receive charging station admin status updates.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationAdminStatusResult>

            ISendAdminStatus.UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  ChargingStationAdminStatusUpdates,
                                                              TransmissionTypes                              TransmissionType,

                                                              DateTimeOffset?                                Timestamp,
                                                              EventTracking_Id?                              EventTrackingId,
                                                              TimeSpan?                                      RequestTimeout,
                                                              User_Id?                                       CurrentUserId,
                                                              CancellationToken                              CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateChargingStationAdminStatus(ChargingStationAdminStatusUpdates,

                                                                                        Timestamp,
                                                                                        EventTrackingId,
                                                                                        RequestTimeout,
                                                                                        CancellationToken);

            else
                result = PushChargingStationAdminStatusResult.OutOfService(Id,
                                                                           this,
                                                                           ChargingStationAdminStatusUpdates);


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
        /// <param name="ChargingPoolAdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolAdminStatusResult>

            ISendAdminStatus.UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  ChargingPoolAdminStatusUpdates,
                                                           TransmissionTypes                           TransmissionType,

                                                           DateTimeOffset?                             Timestamp,
                                                           EventTracking_Id?                           EventTrackingId,
                                                           TimeSpan?                                   RequestTimeout,
                                                           User_Id?                                    CurrentUserId,
                                                           CancellationToken                           CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateChargingPoolAdminStatus(ChargingPoolAdminStatusUpdates,

                                                                                     Timestamp,
                                                                                     EventTrackingId,
                                                                                     RequestTimeout,
                                                                                     CancellationToken);

            else
                result = PushChargingPoolAdminStatusResult.OutOfService(Id,
                                                                        this,
                                                                        ChargingPoolAdminStatusUpdates);


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
        /// <param name="ChargingStationOperatorAdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationOperatorAdminStatusResult>

            ISendAdminStatus.UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  ChargingStationOperatorAdminStatusUpdates,
                                                                      TransmissionTypes                                      TransmissionType,

                                                                      DateTimeOffset?                                        Timestamp,
                                                                      EventTracking_Id?                                      EventTrackingId,
                                                                      TimeSpan?                                              RequestTimeout,
                                                                      User_Id?                                               CurrentUserId,
                                                                      CancellationToken                                      CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateChargingStationOperatorAdminStatus(ChargingStationOperatorAdminStatusUpdates,

                                                                                                Timestamp,
                                                                                                EventTrackingId,
                                                                                                RequestTimeout,
                                                                                                CancellationToken);

            else
                result = PushChargingStationOperatorAdminStatusResult.OutOfService(Id,
                                                                                   this,
                                                                                   ChargingStationOperatorAdminStatusUpdates);


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
        /// <param name="RoamingNetworkAdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushRoamingNetworkAdminStatusResult>

            ISendAdminStatus.UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  RoamingNetworkAdminStatusUpdates,
                                                             TransmissionTypes                             TransmissionType,

                                                             DateTimeOffset?                               Timestamp,
                                                             EventTracking_Id?                             EventTrackingId,
                                                             TimeSpan?                                     RequestTimeout,
                                                             User_Id?                                      CurrentUserId,
                                                             CancellationToken                             CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateRoamingNetworkAdminStatus(RoamingNetworkAdminStatusUpdates,

                                                                                       Timestamp,
                                                                                       EventTrackingId,
                                                                                       RequestTimeout,
                                                                                       CancellationToken);

            else
                result = PushRoamingNetworkAdminStatusResult.OutOfService(Id,
                                                                          this,
                                                                          RoamingNetworkAdminStatusUpdates);


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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushEVSEStatusResult>

            ISendStatus.UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                         TransmissionTypes              TransmissionType,

                                         DateTimeOffset?                Timestamp,
                                         EventTracking_Id?              EventTrackingId,
                                         TimeSpan?                      RequestTimeout,
                                         User_Id?                       CurrentUserId,
                                         CancellationToken              CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateEVSEStatus(StatusUpdates,

                                                                        Timestamp,
                                                                        EventTrackingId,
                                                                        RequestTimeout,
                                                                        CancellationToken);

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
        /// <param name="ChargingStationStatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushChargingStationStatusResult>

            ISendStatus.UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  ChargingStationStatusUpdates,
                                                    TransmissionTypes                         TransmissionType,

                                                    DateTimeOffset?                           Timestamp,
                                                    EventTracking_Id?                         EventTrackingId,
                                                    TimeSpan?                                 RequestTimeout,
                                                    User_Id?                                  CurrentUserId,
                                                    CancellationToken                         CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateChargingStationStatus(ChargingStationStatusUpdates,

                                                                                   Timestamp,
                                                                                   EventTrackingId,
                                                                                   RequestTimeout,
                                                                                   CancellationToken);

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
        /// <param name="ChargingPoolStatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushChargingPoolStatusResult>

            ISendStatus.UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  ChargingPoolStatusUpdates,
                                                 TransmissionTypes                      TransmissionType,

                                                 DateTimeOffset?                        Timestamp,
                                                 EventTracking_Id?                      EventTrackingId,
                                                 TimeSpan?                              RequestTimeout,
                                                 User_Id?                               CurrentUserId,
                                                 CancellationToken                      CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateChargingPoolStatus(ChargingPoolStatusUpdates,

                                                                                Timestamp,
                                                                                EventTrackingId,
                                                                                RequestTimeout,
                                                                                CancellationToken);

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
        /// <param name="ChargingStationOperatorStatus">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushChargingStationOperatorStatusResult>

            ISendStatus.UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  ChargingStationOperatorStatus,
                                                            TransmissionTypes                                 TransmissionType,

                                                            DateTimeOffset?                                   Timestamp,
                                                            EventTracking_Id?                                 EventTrackingId,
                                                            TimeSpan?                                         RequestTimeout,
                                                            User_Id?                                          CurrentUserId,
                                                            CancellationToken                                 CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateChargingStationOperatorStatus(ChargingStationOperatorStatus,

                                                                                           Timestamp,
                                                                                           EventTrackingId,
                                                                                           RequestTimeout,
                                                                                           CancellationToken);

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
        /// <param name="RoamingNetworkStatus">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushRoamingNetworkStatusResult>

            ISendStatus.UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  RoamingNetworkStatus,
                                                   TransmissionTypes                        TransmissionType,

                                                   DateTimeOffset?                          Timestamp,
                                                   EventTracking_Id?                        EventTrackingId,
                                                   TimeSpan?                                RequestTimeout,
                                                   User_Id?                                 CurrentUserId,
                                                   CancellationToken                        CancellationToken)

        {

            #region Initial checks

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


            if (!DisableSendStatus && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.UpdateRoamingNetworkStatus(RoamingNetworkStatus,

                                                                                  Timestamp,
                                                                                  EventTrackingId,
                                                                                  RequestTimeout,
                                                                                  CancellationToken);

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
        public Boolean DisableAuthorization { get; set; }
        public TimeSpan MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<ChargingReservation> ChargingReservations => throw new NotImplementedException();

        public IEnumerable<ChargingSession> ChargingSessions => throw new NotImplementedException();

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
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingLocation?            ChargingLocation      = null,
                           ChargingProduct?             ChargingProduct       = null,
                           ChargingSession_Id?          SessionId             = null,
                           ChargingSession_Id?          CPOPartnerSessionId   = null,
                           ChargingStationOperator_Id?  OperatorId            = null,

                           DateTimeOffset?              RequestTimestamp      = null,
                           EventTracking_Id?            EventTrackingId       = null,
                           TimeSpan?                    RequestTimeout        = null,
                           CancellationToken            CancellationToken     = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;
            RequestTimeout   ??= this.RequestTimeout;

            AuthStartResult? result = null;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
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
                          [],
                          RequestTimeout
                      )
                  );

            #endregion


            if (!DisableAuthorization && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.AuthorizeStart(
                                   LocalAuthentication,
                                   ChargingLocation,
                                   ChargingProduct,
                                   SessionId,
                                   CPOPartnerSessionId,
                                   OperatorId,

                                   RequestTimestamp,
                                   EventTrackingId,
                                   RequestTimeout,
                                   CancellationToken
                               );

            else
                result = AuthStartResult.OutOfService(
                             Id,
                             this,
                             SessionId:  SessionId,
                             Runtime:    TimeSpan.Zero
                         );


            #region Send OnAuthorizeStartResponse event

            var endtime = Timestamp.Now;

            await LogEvent(
                      OnAuthorizeStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endtime,
                          RequestTimestamp.Value,
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
                          [],
                          RequestTimeout,
                          result,
                          endtime - startTime
                      )
                  );

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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingLocation?            ChargingLocation      = null,
                          ChargingSession_Id?          CPOPartnerSessionId   = null,
                          ChargingStationOperator_Id?  OperatorId            = null,

                          DateTimeOffset?              Timestamp             = null,
                          EventTracking_Id?            EventTrackingId       = null,
                          TimeSpan?                    RequestTimeout        = null,
                          CancellationToken            CancellationToken     = default)
        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;

            AuthStopResult? result = null;

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
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            if (!DisableAuthorization && RemoteEMobilityProvider is not null)
                result = await RemoteEMobilityProvider.AuthorizeStop(SessionId,
                                                                     LocalAuthentication,
                                                                     ChargingLocation,
                                                                     CPOPartnerSessionId,
                                                                     OperatorId,

                                                                     Timestamp,
                                                                     EventTrackingId,
                                                                     RequestTimeout,
                                                                     CancellationToken);

            else
                result = AuthStopResult.OutOfService(Id,
                                                     this,
                                                     SessionId:  SessionId,
                                                     Runtime:    TimeSpan.Zero);

            var Endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var Runtime = Endtime - StartTime;


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
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion


        #region SendChargeDetailRecord (ChargeDetailRecord,  ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes   TransmissionType,

                                   DateTimeOffset?     Timestamp           = null,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null,
                                   CancellationToken   CancellationToken   = default)
        {

            if (!DisableSendChargeDetailRecords && RemoteEMobilityProvider is not null)
                return (await RemoteEMobilityProvider.ReceiveChargeDetailRecords(
                                  [ ChargeDetailRecord ],

                                  Timestamp,
                                  EventTrackingId,
                                  RequestTimeout,
                                  CancellationToken
                              )).First();

            return SendCDRResult.OutOfService(
                       org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                       Id,
                       ChargeDetailRecord
                   );

        }

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType,

                                    DateTimeOffset?                  Timestamp           = null,
                                    EventTracking_Id?                EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null,
                                    CancellationToken                CancellationToken   = default)

        {

            if (!DisableSendChargeDetailRecords && RemoteEMobilityProvider is not null)
                return await RemoteEMobilityProvider.ReceiveChargeDetailRecords(
                                 ChargeDetailRecords,
                                 Timestamp,
                                 EventTrackingId,
                                 RequestTimeout,
                                 CancellationToken
                             );

            return SendCDRsResult.OutOfService(
                       org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                       Id,
                       this,
                       ChargeDetailRecords
                   );

        }

        #endregion

        #endregion


        //ToDo: Send Tokens!
        //ToDo: Download CDRs!

        #region Reservations

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
        public event OnReserveRequestDelegate? OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate? OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate? OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate? OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate? OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate? OnReservationCanceled;

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
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        public async Task<ReservationResult>

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTimeOffset?                    ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    Auth_Path?                         AuthenticationPath     = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?  AuthTokens             = null,
                    IEnumerable<EMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,
                    ICSORoamingProvider?               CSORoamingProvider     = null,

                    DateTimeOffset?                    Timestamp              = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null,
                    CancellationToken                  CancellationToken      = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ReservationResult? result = null;

            #endregion

            #region Send OnReserveRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(startTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ReservationId,
                                         LinkedReservationId,
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
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                var response = await RoamingNetwork.Reserve(
                                         ChargingLocation,
                                         ReservationLevel,
                                         ReservationStartTime,
                                         Duration,
                                         ReservationId,
                                         LinkedReservationId,
                                         Id,
                                         RemoteAuthentication,
                                         AuthenticationPath,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         CSORoamingProvider,

                                         Timestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     );


            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }

            result ??= ReservationResult.Error();


            #region Send OnReserveResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(endTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ReservationId,
                                          LinkedReservationId,
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
                                          endTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnReserveResponse));
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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              ICSORoamingProvider?                   CSORoamingProvider   = null,

                              DateTimeOffset?                        Timestamp            = null,
                              EventTracking_Id?                      EventTrackingId      = null,
                              TimeSpan?                              RequestTimeout       = null,
                              CancellationToken                      CancellationToken    = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            ChargingReservation?     canceledReservation  = null;
            CancelReservationResult? result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(startTime,
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
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                var response = await RoamingNetwork.CancelReservation(
                                         ReservationId,
                                         Reason,
                                         CSORoamingProvider,

                                         Timestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     );


                //var OnCancelReservationResponseLocal = OnCancelReservationResponse;
                //if (OnCancelReservationResponseLocal is not null)
                //    OnCancelReservationResponseLocal(Timestamp.Now,
                //                                this,
                //                                EventTracking_Id.New,
                //                                ReservationId,
                //                                Reason);


            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(
                             ReservationId,
                             Reason,
                             e.Message
                         );
            }

            result ??= CancelReservationResult.Error(
                           ReservationId,
                           Reason
                       );


            #region Send OnCancelReservationResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(endTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    endTime - startTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? ChargingReservation)
        {
            throw new NotImplementedException();
        }

        public Boolean TryGetChargingReservationsById(ChargingReservation_Id ReservationId, out ChargingReservationCollection? ChargingReservations)
        {
            throw new NotImplementedException();
        }

        public ChargingReservation? GetChargingReservationById(ChargingReservation_Id ReservationId)
        {
            throw new NotImplementedException();
        }

        public ChargingReservationCollection? GetChargingReservationsById(ChargingReservation_Id ReservationId)
        {
            throw new NotImplementedException();
        }


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTimeOffset       Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTimeOffset                         Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop

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
        public event OnRemoteStartRequestDelegate OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The chosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="RequestTimestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct          = null,
                        ChargingReservation_Id?  ReservationId            = null,
                        ChargingSession_Id?      SessionId                = null,
                        EMobilityProvider_Id?    ProviderId               = null, // Will be ignored!
                        RemoteAuthentication?    RemoteAuthentication     = null,
                        JObject?                 AdditionalSessionInfos   = null,
                        Auth_Path?               AuthenticationPath       = null,
                        ICSORoamingProvider?     CSORoamingProvider       = null,

                        DateTimeOffset?          RequestTimestamp         = null,
                        EventTracking_Id?        EventTrackingId          = null,
                        TimeSpan?                RequestTimeout           = null,
                        CancellationToken        CancellationToken        = default)

        {

            #region Initial checks

            RequestTimestamp ??= Timestamp.Now;
            EventTrackingId  ??= EventTracking_Id.New;

            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          startTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          Id,
                          RequestTimeout
                      )
                  );

            #endregion


            try
            {

                result = await RoamingNetwork.RemoteStart(
                                   ChargingLocation,
                                   ChargingProduct,
                                   ReservationId,
                                   SessionId,
                                   Id,
                                   RemoteAuthentication,
                                   AdditionalSessionInfos,
                                   AuthenticationPath,
                                   CSORoamingProvider,

                                   RequestTimestamp,
                                   EventTrackingId,
                                   RequestTimeout,
                                   CancellationToken
                               );

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(
                             e.Message,
                             System_Id.Local
                         );
            }


            #region Send OnRemoteStartResponse event

            var endTime = Timestamp.Now;

            await LogEvent(
                      OnRemoteStartResponse,
                      loggingDelegate => loggingDelegate.Invoke(
                          endTime,
                          RequestTimestamp.Value,
                          this,
                          EventTrackingId,
                          RoamingNetwork.Id,
                          ChargingLocation,
                          RemoteAuthentication,
                          SessionId,
                          ReservationId,
                          ChargingProduct,
                          null,
                          null,
                          ProviderId,
                          RequestTimeout,
                          result,
                          endTime - startTime
                      )
                  );

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
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling      = null,
                       EMobilityProvider_Id?  ProviderId               = null, // Will be ignored!
                       RemoteAuthentication?  RemoteAuthentication     = null,
                       JObject?               AdditionalSessionInfos   = null,
                       Auth_Path?             AuthenticationPath       = null,
                       ICSORoamingProvider?   CSORoamingProvider       = null,

                       DateTimeOffset?        Timestamp                = null,
                       EventTracking_Id?      EventTrackingId          = null,
                       TimeSpan?              RequestTimeout           = null,
                       CancellationToken      CancellationToken        = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;

            RemoteStopResult? result = null;

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
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                result = await RoamingNetwork.RemoteStop(
                                   SessionId,
                                   ReservationHandling,
                                   Id,
                                   RemoteAuthentication,
                                   AdditionalSessionInfos,
                                   AuthenticationPath,
                                   CSORoamingProvider,

                                   Timestamp,
                                   EventTrackingId,
                                   RequestTimeout,
                                   CancellationToken
                               );

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(
                             SessionId,
                             System_Id.Local,
                             e.Message
                         );
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
                DebugX.LogException(e, nameof(EMobilityProvider) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region ChargingSessions

        public Boolean ContainsChargingSessionId(ChargingSession_Id ChargingSessionId)
        {
            return false;
        }

        public ChargingSession? GetChargingSessionById(ChargingSession_Id ChargingSessionId)
        {
            return null;
        }

        public Boolean TryGetChargingSessionById(ChargingSession_Id                        ChargingSessionId,
                                                 [NotNullWhen(true)] out ChargingSession?  ChargingSession)
        {
            ChargingSession = null;
            return false;
        }

        #endregion


        #region (private) LogEvent(Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                         [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

                => LogEvent(
                       nameof(EMobilityProvider),
                       Logger,
                       LogHandler,
                       EventName,
                       Command
                   );


        private async Task LogEvent<TDelegate>(String                                             WWCPIO,
                                               TDelegate?                                         Logger,
                                               Func<TDelegate, Task>                              LogHandler,
                                               [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                               [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

        {
            if (Logger is not null)
            {
                try
                {

                    await Task.WhenAll(
                              Logger.GetInvocationList().
                                     OfType<TDelegate>().
                                     Select(LogHandler)
                          );

                }
                catch (Exception e)
                {
                    await HandleErrors(WWCPIO, $"{Command}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ExceptionOccurred)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccurred)
        {

            return Task.CompletedTask;

        }

        #endregion


        #region IComparable<eMobilityProvider> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object? Object)
        {

            if (Object is null)
                throw new ArgumentNullException("The given object must not be null!");

            if (!(Object is EMobilityProvider eMobilityProvider))
                throw new ArgumentException("The given object is not an eMobilityProvider!");

            return CompareTo(eMobilityProvider);

        }

        #endregion

        #region CompareTo(eMobilityProvider)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityProvider">An EVSE_Operator object to compare with.</param>
        public Int32 CompareTo(IEMobilityProvider? eMobilityProvider)
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
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)
        {

            if (Object is null)
                return false;

            if (!(Object is EMobilityProvider eMobilityProvider))
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
        public Boolean Equals(IEMobilityProvider? eMobilityProvider)
        {

            if (eMobilityProvider is null)
                return false;

            return Id.Equals(eMobilityProvider.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hash code of this object.
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
