/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this SmartCity, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this SmartCityProxy  SmartCity,
                                     Boolean                       Embedded                        = false,
                                     Boolean                       ExpandChargingRoamingNetworkId  = false,
                                     Boolean                       ExpandChargingPoolIds           = false,
                                     Boolean                       ExpandChargingStationIds        = false,
                                     Boolean                       ExpandEVSEIds                   = false)

            => SmartCity != null
                   ? JSONObject.Create(

                         new JProperty("id",                        SmartCity.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      SmartCity.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    SmartCity.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  SmartCity.Name.       ToJSON()),
                         new JProperty("description",           SmartCity.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         SmartCity.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          SmartCity.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         SmartCity.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            SmartCity.Homepage)
                             : null,

                         SmartCity.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             SmartCity.HotlinePhoneNumber)
                             : null,

                         SmartCity.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(SmartCity.DataLicenses.Select(license => license.ToJSON())))
                             : null

                         //new JProperty("chargingPools",         ExpandChargingPoolIds
                         //                                           ? new JArray(SmartCity.ChargingPools.     ToJSON(Embedded: true))
                         //                                           : new JArray(SmartCity.ChargingPoolIds.   Select(id => id.ToString()))),

                         //new JProperty("chargingStations",      ExpandChargingStationIds
                         //                                           ? new JArray(SmartCity.ChargingStations.  ToJSON(Embedded: true))
                         //                                           : new JArray(SmartCity.ChargingStationIds.Select(id => id.ToString()))),

                         //new JProperty("evses",                 ExpandEVSEIds
                         //                                           ? new JArray(SmartCity.EVSEs.             ToJSON(Embedded: true))
                         //                                           : new JArray(SmartCity.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this SmartCity, JPropertyKey)

        public static JProperty ToJSON(this SmartCityProxy SmartCity, String JPropertyKey)

            => SmartCity != null
                   ? new JProperty(JPropertyKey, SmartCity.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this SmartCities, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="SmartCities">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<SmartCityProxy>  SmartCities,
                                    UInt64?                           Skip                            = null,
                                    UInt64?                           Take                            = null,
                                    Boolean                           Embedded                        = false,
                                    Boolean                           ExpandChargingRoamingNetworkId  = false,
                                    Boolean                           ExpandChargingPoolIds           = false,
                                    Boolean                           ExpandChargingStationIds        = false,
                                    Boolean                           ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (SmartCities == null)
                return new JArray();

            #endregion

            return new JArray(SmartCities.
                                  Where     (cso => cso != null).
                                  OrderBy   (cso => cso.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(cso => cso.ToJSON(Embedded,
                                                               ExpandChargingRoamingNetworkId,
                                                               ExpandChargingPoolIds,
                                                               ExpandChargingStationIds,
                                                               ExpandEVSEIds)));

        }

        #endregion

        #region ToJSON(this SmartCities, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<SmartCityProxy> SmartCities, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return SmartCities != null
                       ? new JProperty(JPropertyKey, SmartCities.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this SmartCityAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<SmartCityAdminStatusTypes>>  SmartCityAdminStatus,
                                     UInt64?                                                  Skip         = null,
                                     UInt64?                                                  Take         = null,
                                     UInt64?                                                  HistorySize  = 1)

        {

            if (SmartCityAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityAdminStatus.
                                       SkipTakeFilter(Skip, Take).

                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch
            {
                // e.g. when a Stack behind SmartCityAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this SmartCityAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusTypes>>>>  SmartCityAdminStatus,
                                     UInt64?                                                                                           Skip         = null,
                                     UInt64?                                                                                           Take         = null,
                                     UInt64?                                                                                           HistorySize  = 1)

        {

            if (SmartCityAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityAdminStatus.
                                       SkipTakeFilter(Skip, Take).
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

                                                                                // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                                GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                                Select           (group => group.First()).

                                                                                OrderByDescending(tsv   => tsv.Timestamp).
                                                                                Take             (HistorySize).
                                                                                Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                         tsv.Value.    ToString())))

                                                          )));

            }
            catch
            {
                // e.g. when a Stack behind SmartCityAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this SmartCityStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<SmartCityStatusTypes>>  SmartCityStatus,
                                     UInt64?                                             Skip         = null,
                                     UInt64?                                             Take         = null,
                                     UInt64?                                             HistorySize  = 1)

        {

            if (SmartCityStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityStatus.
                                       SkipTakeFilter(Skip, Take).

                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch
            {
                // e.g. when a Stack behind SmartCityStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this SmartCityStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusTypes>>>>  SmartCityStatus,
                                     UInt64?                                                                                      Skip         = null,
                                     UInt64?                                                                                      Take         = null,
                                     UInt64?                                                                                      HistorySize  = 1)

        {

            if (SmartCityStatus == null)
                return new JObject();

            try
            {

                return new JObject(SmartCityStatus.
                                       SkipTakeFilter(Skip, Take).
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

                                                                                // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                                GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                                Select           (group => group.First()).

                                                                                OrderByDescending(tsv   => tsv.Timestamp).
                                                                                Take             (HistorySize).
                                                                                Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                         tsv.Value.    ToString())))

                                                                )));

            }
            catch
            {
                // e.g. when a Stack behind SmartCityStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }


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
    public class SmartCityProxy : ACryptoEMobilityEntity<SmartCity_Id,
                                                         SmartCityAdminStatusTypes,
                                                         SmartCityStatusTypes>,
                                  ISend2RemoteSmartCity,
                                  IEquatable <SmartCityProxy>,
                                  IComparable<SmartCityProxy>,
                                  IComparable
    {

        #region Data

        #endregion

        #region Properties

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


        #region DataLicense

        private List<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public IEnumerable<DataLicense> DataLicenses
            => _DataLicenses;

        #endregion

        public SmartCityPriority Priority { get; set; }


        //#region AllTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> AllTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.AllTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        //#region AuthorizedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> AuthorizedTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.AuthorizedTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        //#region NotAuthorizedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> NotAuthorizedTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.NotAuthorizedTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        //#region BlockedTokens

        //public IEnumerable<KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>> BlockedTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.BlockedTokens
        //           : new KeyValuePair<AuthenticationToken, TokenAuthorizationResultType>[0];

        //#endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote smart city.
        /// </summary>
        public IRemoteSmartCity          RemoteSmartCity    { get; }

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
        public event OnReserveRequestDelegate?             OnReserveRequest;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnReserveResponseDelegate?            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate?             OnNewReservation;

        /// <summary>
        /// An event fired whenever a charging reservation was deleted.
        /// </summary>
        public event OnCancelReservationResponseDelegate?  OnCancelReservationResponse;

        #endregion

        #region OnRemote...Start / OnRemote...Started

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate?   OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate?  OnRemoteStartResponse;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate?    OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate?   OnRemoteStopResponse;

        #endregion

        // CancelReservation

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility (service) provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="Id">The unique smart city identification.</param>
        /// <param name="Name">The official (multi-language) name of the smart city.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal SmartCityProxy(SmartCity_Id                     Id,
                                RoamingNetwork                   RoamingNetwork,
                                I18NString?                      Name                         = null,
                                I18NString?                      Description                  = null,
                                Action<SmartCityProxy>?          Configurator                 = null,
                                RemoteSmartCityCreatorDelegate?  RemoteSmartCityCreator       = null,
                                SmartCityPriority?               Priority                     = null,
                                SmartCityAdminStatusTypes?       InitialAdminStatus           = null,
                                SmartCityStatusTypes?            InitialStatus                = null,
                                UInt16?                          MaxAdminStatusScheduleSize   = null,
                                UInt16?                          MaxStatusScheduleSize        = null,

                                String?                          DataSource                   = null,
                                DateTime?                        Created                      = null,
                                DateTime?                        LastChange                   = null,

                                JObject?                         CustomData                   = null,
                                UserDefinedDictionary?           InternalData                 = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus         ?? SmartCityAdminStatusTypes.Available,
                   InitialStatus              ?? SmartCityStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   Created,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Initial checks

            if (IEnumerableExtensions.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name), "The given smart city name must not be null or empty!");

            #endregion

            #region Init data and properties

            this._DataLicenses                = new List<DataLicense>();

            this.Priority                     = Priority    ?? new SmartCityPriority(0);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteSmartCity = RemoteSmartCityCreator?.Invoke(this);

        }

        #endregion


        #region PushEVSEData/-Status directly...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushData            { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableSendAdminStatus     { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableSendStatus          { get; set; }

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableSendEnergyStatus    { get; set; }


        #region (Set/Add/Update/Delete) Roaming network(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                          DisableSendRoamingNetworkData    { get; set; }

        /// <summary>
        /// Only include roaming network identifications matching the given delegate.
        /// </summary>
        public IncludeRoamingNetworkIdDelegate  IncludeRoamingNetworkIds         { get; }

        /// <summary>
        /// Only include roaming network matching the given delegate.
        /// </summary>
        public IncludeRoamingNetworkDelegate    IncludeRoamingNetworks           { get; }


        #region AddRoamingNetwork           (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddRoamingNetworkResult>

            ISendRoamingNetworkData.AddRoamingNetwork(IRoamingNetwork     RoamingNetwork,
                                                      TransmissionTypes   TransmissionType,

                                                      DateTime?           Timestamp,
                                                      EventTracking_Id?   EventTrackingId,
                                                      TimeSpan?           RequestTimeout,
                                                      CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddRoamingNetworkIfNotExists(RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddRoamingNetworkResult>

            ISendRoamingNetworkData.AddRoamingNetworkIfNotExists(IRoamingNetwork     RoamingNetwork,
                                                                 TransmissionTypes   TransmissionType,

                                                                 DateTime?           Timestamp,
                                                                 EventTracking_Id?   EventTrackingId,
                                                                 TimeSpan?           RequestTimeout,
                                                                 CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddOrUpdateRoamingNetwork   (RoamingNetwork, ...)

        /// <summary>
        /// Set the EVSE data of the given roaming network as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddOrUpdateRoamingNetworkResult>

            ISendRoamingNetworkData.AddOrUpdateRoamingNetwork(IRoamingNetwork     RoamingNetwork,
                                                              TransmissionTypes   TransmissionType,

                                                              DateTime?           Timestamp,
                                                              EventTracking_Id?   EventTrackingId,
                                                              TimeSpan?           RequestTimeout,
                                                              CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddOrUpdateRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region UpdateRoamingNetwork        (RoamingNetwork, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<UpdateRoamingNetworkResult>

            ISendRoamingNetworkData.UpdateRoamingNetwork(IRoamingNetwork     RoamingNetwork,
                                                         String              PropertyName,
                                                         Object?             NewValue,
                                                         Object?             OldValue,
                                                         Context?            DataSource,
                                                         TransmissionTypes   TransmissionType,

                                                         DateTime?           Timestamp,
                                                         EventTracking_Id?   EventTrackingId,
                                                         TimeSpan?           RequestTimeout,
                                                         CancellationToken   CancellationToken)


                => Task.FromResult(
                       UpdateRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region DeleteRoamingNetwork        (RoamingNetwork, ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<DeleteRoamingNetworkResult>

            ISendRoamingNetworkData.DeleteRoamingNetwork(IRoamingNetwork     RoamingNetwork,
                                                         TransmissionTypes   TransmissionType,

                                                         DateTime?           Timestamp,
                                                         EventTracking_Id?   EventTrackingId,
                                                         TimeSpan?           RequestTimeout,
                                                         CancellationToken   CancellationToken)


                => Task.FromResult(
                       DeleteRoamingNetworkResult.NoOperation(
                           RoamingNetwork:   RoamingNetwork,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion


        #region AddRoamingNetworks          (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddRoamingNetworksResult>

            AddRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                               TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?                     Timestamp           = null,
                               EventTracking_Id?             EventTrackingId     = null,
                               TimeSpan?                     RequestTimeout      = null,
                               CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       AddRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region AddRoamingNetworksIfNotExist(RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of roaming networks, if they do not already exist.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddRoamingNetworksResult>

            AddRoamingNetworksIfNotExist(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                         TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                         DateTime?                     Timestamp           = null,
                                         EventTracking_Id?             EventTrackingId     = null,
                                         TimeSpan?                     RequestTimeout      = null,
                                         CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       AddRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateRoamingNetworks  (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to add or update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateRoamingNetworksResult>

            AddOrUpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                       TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                       DateTime?                     Timestamp           = null,
                                       EventTracking_Id?             EventTrackingId     = null,
                                       TimeSpan?                     RequestTimeout      = null,
                                       CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region UpdateRoamingNetworks       (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to update.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateRoamingNetworksResult>

            UpdateRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                  TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTime?                     Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       UpdateRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion

        #region DeleteRoamingNetworks       (RoamingNetworks, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks to delete.</param>
        /// <param name="TransmissionType">Whether to send the roaming network directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteRoamingNetworksResult>

            DeleteRoamingNetworks(IEnumerable<IRoamingNetwork>  RoamingNetworks,
                                  TransmissionTypes             TransmissionType    = TransmissionTypes.Enqueue,

                                  DateTime?                     Timestamp           = null,
                                  EventTracking_Id?             EventTrackingId     = null,
                                  TimeSpan?                     RequestTimeout      = null,
                                  CancellationToken             CancellationToken   = default)


                => Task.FromResult(
                       DeleteRoamingNetworksResult.NoOperation(
                           RejectedRoamingNetworks:  RoamingNetworks,
                           SenderId:                 Id,
                           Sender:                   this,
                           EventTrackingId:          EventTrackingId
                       )
                   );

        #endregion


        #region UpdateRoamingNetworkAdminStatus(RoamingNetworkAdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushRoamingNetworkAdminStatusResult>

            ISendAdminStatus.UpdateRoamingNetworkAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  RoamingNetworkAdminStatusUpdates,
                                                             TransmissionTypes                             TransmissionType,

                                                             DateTime?                                     Timestamp,
                                                             EventTracking_Id?                             EventTrackingId,
                                                             TimeSpan?                                     RequestTimeout,
                                                             CancellationToken                             CancellationToken)

        {

            return PushRoamingNetworkAdminStatusResult.NoOperation(Id, this);

        }

        #endregion

        #region UpdateRoamingNetworkStatus     (RoamingNetworkStatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdates">An enumeration of roaming network status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushRoamingNetworkStatusResult>

            ISendStatus.UpdateRoamingNetworkStatus(IEnumerable<RoamingNetworkStatusUpdate>  RoamingNetworkStatusUpdates,
                                                   TransmissionTypes                        TransmissionType,

                                                   DateTime?                                Timestamp,
                                                   EventTracking_Id?                        EventTrackingId,
                                                   TimeSpan?                                RequestTimeout,
                                                   CancellationToken                        CancellationToken)

        {

            return PushRoamingNetworkStatusResult.NoOperation(Id, this);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                                   DisableSendChargingStationOperatorData    { get; set; }

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        public IncludeChargingStationOperatorIdDelegate  IncludeChargingStationOperatorIds         { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        public IncludeChargingStationOperatorDelegate    IncludeChargingStationOperators           { get; }


        #region AddChargingStationOperator           (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorResult>

            ISendChargingStationOperatorData.AddChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                                                        TransmissionTypes         TransmissionType,

                                                                        DateTime?                 Timestamp,
                                                                        EventTracking_Id?         EventTrackingId,
                                                                        TimeSpan?                 RequestTimeout,
                                                                        CancellationToken         CancellationToken)


                => Task.FromResult(
                       AddChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:              this
                       )
                   );

        #endregion

        #region AddChargingStationOperatorIfNotExists(ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorResult>

            ISendChargingStationOperatorData.AddChargingStationOperatorIfNotExists(IChargingStationOperator  ChargingStationOperator,
                                                                                   TransmissionTypes         TransmissionType,

                                                                                   DateTime?                 Timestamp,
                                                                                   EventTracking_Id?         EventTrackingId,
                                                                                   TimeSpan?                 RequestTimeout,
                                                                                   CancellationToken         CancellationToken)


                => Task.FromResult(
                       AddChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:              this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStationOperator   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationOperatorResult>

            ISendChargingStationOperatorData.AddOrUpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                                                                TransmissionTypes         TransmissionType,

                                                                                DateTime?                 Timestamp,
                                                                                EventTracking_Id?         EventTrackingId,
                                                                                TimeSpan?                 RequestTimeout,
                                                                                CancellationToken         CancellationToken)


                => Task.FromResult(
                       AddOrUpdateChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:              this
                       )
                   );

        #endregion

        #region UpdateChargingStationOperator        (ChargingStationOperator, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationOperatorResult>

            ISendChargingStationOperatorData.UpdateChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                                                           String                    PropertyName,
                                                                           Object?                   NewValue,
                                                                           Object?                   OldValue,
                                                                           Context?                  DataSource,
                                                                           TransmissionTypes         TransmissionType,

                                                                           DateTime?                 Timestamp,
                                                                           EventTracking_Id?         EventTrackingId,
                                                                           TimeSpan?                 RequestTimeout,
                                                                           CancellationToken         CancellationToken)


                => Task.FromResult(
                       UpdateChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:              this
                       )
                   );

        #endregion

        #region DeleteChargingStationOperator        (ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationOperatorResult>

            ISendChargingStationOperatorData.DeleteChargingStationOperator(IChargingStationOperator  ChargingStationOperator,
                                                                           TransmissionTypes         TransmissionType,

                                                                           DateTime?                 Timestamp,
                                                                           EventTracking_Id?         EventTrackingId,
                                                                           TimeSpan?                 RequestTimeout,
                                                                           CancellationToken         CancellationToken)


                => Task.FromResult(
                       DeleteChargingStationOperatorResult.NoOperation(
                           ChargingStationOperator:  ChargingStationOperator,
                           EventTrackingId:          EventTrackingId,
                           SenderId:                 Id,
                           Sender:              this
                       )
                   );

        #endregion


        #region AddChargingStationOperators          (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorsResult>

            ISendChargingStationOperatorData.AddChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                         TransmissionTypes                      TransmissionType,

                                                                         DateTime?                              Timestamp,
                                                                         EventTracking_Id?                      EventTrackingId,
                                                                         TimeSpan?                              RequestTimeout,
                                                                         CancellationToken                      CancellationToken)


                => Task.FromResult(
                       AddChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                       this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingStationOperatorsIfNotExist(ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationOperatorsResult>

            ISendChargingStationOperatorData.AddChargingStationOperatorsIfNotExist(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                                   TransmissionTypes                      TransmissionType,

                                                                                   DateTime?                              Timestamp,
                                                                                   EventTracking_Id?                      EventTrackingId,
                                                                                   TimeSpan?                              RequestTimeout,
                                                                                   CancellationToken                      CancellationToken)


                => Task.FromResult(
                       AddChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                       this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStationOperators  (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationOperatorsResult>

            ISendChargingStationOperatorData.AddOrUpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                                 TransmissionTypes                      TransmissionType,

                                                                                 DateTime?                              Timestamp,
                                                                                 EventTracking_Id?                      EventTrackingId,
                                                                                 TimeSpan?                              RequestTimeout,
                                                                                 CancellationToken                      CancellationToken)


                => Task.FromResult(
                       AddOrUpdateChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                       this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingStationOperators       (ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationOperatorsResult>

            ISendChargingStationOperatorData.UpdateChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                            TransmissionTypes                      TransmissionType,

                                                                            DateTime?                              Timestamp,
                                                                            EventTracking_Id?                      EventTrackingId,
                                                                            TimeSpan?                              RequestTimeout,
                                                                            CancellationToken                      CancellationToken)


                => Task.FromResult(
                       UpdateChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                       this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingStationOperators       (ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationOperatorsResult>

            ISendChargingStationOperatorData.DeleteChargingStationOperators(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                                                            TransmissionTypes                      TransmissionType,

                                                                            DateTime?                              Timestamp,
                                                                            EventTracking_Id?                      EventTrackingId,
                                                                            TimeSpan?                              RequestTimeout,
                                                                            CancellationToken                      CancellationToken)


                => Task.FromResult(
                       DeleteChargingStationOperatorsResult.NoOperation(
                           RejectedChargingStationOperators:  ChargingStationOperators,
                           SenderId:                          Id,
                           Sender:                       this,
                           EventTrackingId:                   EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingStationOperatorAdminStatus(ChargingStationOperatorAdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="ChargingStationOperatorAdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushChargingStationOperatorAdminStatusResult>

            ISendAdminStatus.UpdateChargingStationOperatorAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  ChargingStationOperatorAdminStatusUpdates,
                                                                      TransmissionTypes                                      TransmissionType,

                                                                      DateTime?                                              Timestamp,
                                                                      EventTracking_Id?                                      EventTrackingId,
                                                                      TimeSpan?                                              RequestTimeout,
                                                                      CancellationToken                                      CancellationToken)

        {

            return PushChargingStationOperatorAdminStatusResult.NoOperation(Id, this);

        }

        #endregion

        #region UpdateChargingStationOperatorStatus     (ChargingStationOperatorStatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="ChargingStationOperatorStatusUpdates">An enumeration of charging station operator status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        async Task<PushChargingStationOperatorStatusResult>

            ISendStatus.UpdateChargingStationOperatorStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  ChargingStationOperatorStatusUpdates,
                                                            TransmissionTypes                                 TransmissionType,

                                                            DateTime?                                         Timestamp,
                                                            EventTracking_Id?                                 EventTrackingId,
                                                            TimeSpan?                                         RequestTimeout,
                                                            CancellationToken                                 CancellationToken)

        {

            return PushChargingStationOperatorStatusResult.NoOperation(Id, this);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                        DisableSendChargingPoolData    { get; set; }

        /// <summary>
        /// Only include charging pool identifications matching the given delegate.
        /// </summary>
        public IncludeChargingPoolIdDelegate  IncludeChargingPoolIds         { get; }

        /// <summary>
        /// Only include charging pools matching the given delegate.
        /// </summary>
        public IncludeChargingPoolDelegate    IncludeChargingPools           { get; }


        #region AddChargingPool           (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolResult>

            ISendChargingPoolData.AddChargingPool(IChargingPool       ChargingPool,
                                                  TransmissionTypes   TransmissionType,

                                                  DateTime?           Timestamp,
                                                  EventTracking_Id?   EventTrackingId,
                                                  TimeSpan?           RequestTimeout,
                                                  CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddChargingPoolIfNotExists(ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolResult>

            ISendChargingPoolData.AddChargingPoolIfNotExists(IChargingPool       ChargingPool,
                                                             TransmissionTypes   TransmissionType,

                                                             DateTime?           Timestamp,
                                                             EventTracking_Id?   EventTrackingId,
                                                             TimeSpan?           RequestTimeout,
                                                             CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingPool   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingPoolResult>

            ISendChargingPoolData.AddOrUpdateChargingPool(IChargingPool       ChargingPool,
                                                          TransmissionTypes   TransmissionType,

                                                          DateTime?           Timestamp,
                                                          EventTracking_Id?   EventTrackingId,
                                                          TimeSpan?           RequestTimeout,
                                                          CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddOrUpdateChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region UpdateChargingPool        (ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="OldValue">The old value of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingPoolResult>

            ISendChargingPoolData.UpdateChargingPool(IChargingPool       ChargingPool,
                                                     String              PropertyName,
                                                     Object?             NewValue,
                                                     Object?             OldValue,
                                                     Context?            DataSource,
                                                     TransmissionTypes   TransmissionType,

                                                     DateTime?           Timestamp,
                                                     EventTracking_Id?   EventTrackingId,
                                                     TimeSpan?           RequestTimeout,
                                                     CancellationToken   CancellationToken)


                => Task.FromResult(
                       UpdateChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region DeleteChargingPool        (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingPoolResult>

            ISendChargingPoolData.DeleteChargingPool(IChargingPool       ChargingPool,
                                                     TransmissionTypes   TransmissionType,

                                                     DateTime?           Timestamp,
                                                     EventTracking_Id?   EventTrackingId,
                                                     TimeSpan?           RequestTimeout,
                                                     CancellationToken   CancellationToken)


                => Task.FromResult(
                       DeleteChargingPoolResult.NoOperation(
                           ChargingPool:     ChargingPool,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion


        #region AddChargingPools          (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolsResult>

            ISendChargingPoolData.AddChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                                   TransmissionTypes           TransmissionType,

                                                   DateTime?                   Timestamp,
                                                   EventTracking_Id?           EventTrackingId,
                                                   TimeSpan?                   RequestTimeout,
                                                   CancellationToken           CancellationToken)


                => Task.FromResult(
                       AddChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingPoolsIfNotExist(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingPoolsResult>

            ISendChargingPoolData.AddChargingPoolsIfNotExist(IEnumerable<IChargingPool>  ChargingPools,
                                                             TransmissionTypes           TransmissionType,

                                                             DateTime?                   Timestamp,
                                                             EventTracking_Id?           EventTrackingId,
                                                             TimeSpan?                   RequestTimeout,
                                                             CancellationToken           CancellationToken)


                => Task.FromResult(
                       AddChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingPools  (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingPoolsResult>

            ISendChargingPoolData.AddOrUpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                                           TransmissionTypes           TransmissionType,

                                                           DateTime?                   Timestamp,
                                                           EventTracking_Id?           EventTrackingId,
                                                           TimeSpan?                   RequestTimeout,
                                                           CancellationToken           CancellationToken)


                => Task.FromResult(
                       AddOrUpdateChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingPools       (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingPoolsResult>

            ISendChargingPoolData.UpdateChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                                      TransmissionTypes           TransmissionType,

                                                      DateTime?                   Timestamp,
                                                      EventTracking_Id?           EventTrackingId,
                                                      TimeSpan?                   RequestTimeout,
                                                      CancellationToken           CancellationToken)


                => Task.FromResult(
                       UpdateChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region ReplaceChargingPools      (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of charging pools.
        /// Charging pools not included will be deleted.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging stations to replace.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<ReplaceChargingPoolsResult>

            ISendChargingPoolData.ReplaceChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                                       TransmissionTypes           TransmissionType,

                                                       DateTime?                   Timestamp,
                                                       EventTracking_Id?           EventTrackingId,
                                                       TimeSpan?                   RequestTimeout,
                                                       CancellationToken           CancellationToken)


                => Task.FromResult(
                       ReplaceChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingPools       (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingPoolsResult>

            ISendChargingPoolData.DeleteChargingPools(IEnumerable<IChargingPool>  ChargingPools,
                                                      TransmissionTypes           TransmissionType,

                                                      DateTime?                   Timestamp,
                                                      EventTracking_Id?           EventTrackingId,
                                                      TimeSpan?                   RequestTimeout,
                                                      CancellationToken           CancellationToken)


                => Task.FromResult(
                       DeleteChargingPoolsResult.NoOperation(
                           RejectedChargingPools:  ChargingPools,
                           SenderId:               Id,
                           Sender:            this,
                           EventTrackingId:        EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingPoolAdminStatus (ChargingPoolAdminStatusUpdates,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="ChargingPoolAdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolAdminStatusResult>

            ISendAdminStatus.UpdateChargingPoolAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  ChargingPoolAdminStatusUpdates,
                                                           TransmissionTypes                           TransmissionType,

                                                           DateTime?                                   Timestamp,
                                                           EventTracking_Id?                           EventTrackingId,
                                                           TimeSpan?                                   RequestTimeout,
                                                           CancellationToken                           CancellationToken)
        {

            return Task.FromResult(PushChargingPoolAdminStatusResult.OutOfService(Id,
                                                                                  this,
                                                                                  ChargingPoolAdminStatusUpdates));

        }

        #endregion

        #region UpdateChargingPoolStatus      (ChargingPoolStatusUpdates,       TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolStatusResult>

            ISendStatus.UpdateChargingPoolStatus(IEnumerable<ChargingPoolStatusUpdate>  ChargingPoolStatusUpdates,
                                                 TransmissionTypes                      TransmissionType,

                                                 DateTime?                              Timestamp,
                                                 EventTracking_Id?                      EventTrackingId,
                                                 TimeSpan?                              RequestTimeout,
                                                 CancellationToken                      CancellationToken)


                => Task.FromResult(PushChargingPoolStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateChargingPoolEnergyStatus(ChargingPoolEnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingPoolEnergyStatusResult>

            ISendEnergyStatus.UpdateChargingPoolEnergyStatus(IEnumerable<ChargingPoolEnergyStatusUpdate>  ChargingPoolEnergyStatusUpdates,
                                                             TransmissionTypes                            TransmissionType,

                                                             DateTime?                                    Timestamp,
                                                             EventTracking_Id?                            EventTrackingId,
                                                             TimeSpan?                                    RequestTimeout,
                                                             CancellationToken                            CancellationToken)
        {

            return Task.FromResult(PushChargingPoolEnergyStatusResult.OutOfService(Id,
                                                                                   this,
                                                                                   ChargingPoolEnergyStatusUpdates));

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                           DisableSendChargingStationData    { get; set; }

        /// <summary>
        /// Only include charging station identifications matching the given delegate.
        /// </summary>
        public IncludeChargingStationIdDelegate  IncludeChargingStationIds         { get; }

        /// <summary>
        /// Only include charging stations matching the given delegate.
        /// </summary>
        public IncludeChargingStationDelegate    IncludeChargingStations           { get; }


        #region AddChargingStation           (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationResult>

            ISendChargingStationData.AddChargingStation(IChargingStation    ChargingStation,
                                                        TransmissionTypes   TransmissionType,

                                                        DateTime?           Timestamp,
                                                        EventTracking_Id?   EventTrackingId,
                                                        TimeSpan?           RequestTimeout,
                                                        CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddChargingStationIfNotExists(ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationResult>

            ISendChargingStationData.AddChargingStationIfNotExists(IChargingStation    ChargingStation,
                                                                   TransmissionTypes   TransmissionType,

                                                                   DateTime?           Timestamp,
                                                                   EventTracking_Id?   EventTrackingId,
                                                                   TimeSpan?           RequestTimeout,
                                                                   CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStation   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationResult>

            ISendChargingStationData.AddOrUpdateChargingStation(IChargingStation    ChargingStation,
                                                                TransmissionTypes   TransmissionType,

                                                                DateTime?           Timestamp,
                                                                EventTracking_Id?   EventTrackingId,
                                                                TimeSpan?           RequestTimeout,
                                                                CancellationToken   CancellationToken)


                => Task.FromResult(
                       AddOrUpdateChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region UpdateChargingStation        (ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="OldValue">The old value of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationResult>

            ISendChargingStationData.UpdateChargingStation(IChargingStation    ChargingStation,
                                                           String              PropertyName,
                                                           Object?             NewValue,
                                                           Object?             OldValue,
                                                           Context?            DataSource,
                                                           TransmissionTypes   TransmissionType,

                                                           DateTime?           Timestamp,
                                                           EventTracking_Id?   EventTrackingId,
                                                           TimeSpan?           RequestTimeout,
                                                           CancellationToken   CancellationToken)


                => Task.FromResult(
                       UpdateChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region DeleteChargingStation        (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationResult>

            ISendChargingStationData.DeleteChargingStation(IChargingStation    ChargingStation,
                                                           TransmissionTypes   TransmissionType,

                                                           DateTime?           Timestamp,
                                                           EventTracking_Id?   EventTrackingId,
                                                           TimeSpan?           RequestTimeout,
                                                           CancellationToken   CancellationToken)


                => Task.FromResult(
                       DeleteChargingStationResult.NoOperation(
                           ChargingStation:  ChargingStation,
                           EventTrackingId:  EventTrackingId,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion


        #region AddChargingStations          (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationsResult>

            ISendChargingStationData.AddChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                                         TransmissionTypes              TransmissionType,


                                                         DateTime?                      Timestamp,
                                                         EventTracking_Id?              EventTrackingId,
                                                         TimeSpan?                      RequestTimeout,
                                                         CancellationToken              CancellationToken)


                => Task.FromResult(
                       AddChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:               this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region AddChargingStationsIfNotExist(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddChargingStationsResult>

            ISendChargingStationData.AddChargingStationsIfNotExist(IEnumerable<IChargingStation>  ChargingStations,
                                                                   TransmissionTypes              TransmissionType,


                                                                   DateTime?                      Timestamp,
                                                                   EventTracking_Id?              EventTrackingId,
                                                                   TimeSpan?                      RequestTimeout,
                                                                   CancellationToken              CancellationToken)


                => Task.FromResult(
                       AddChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:               this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateChargingStations  (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateChargingStationsResult>

            ISendChargingStationData.AddOrUpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                                                 TransmissionTypes              TransmissionType,

                                                                 DateTime?                      Timestamp,
                                                                 EventTracking_Id?              EventTrackingId,
                                                                 TimeSpan?                      RequestTimeout,
                                                                 CancellationToken              CancellationToken)


                => Task.FromResult(
                       AddOrUpdateChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:               this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region UpdateChargingStations       (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateChargingStationsResult>

            ISendChargingStationData.UpdateChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                                            TransmissionTypes              TransmissionType,

                                                            DateTime?                      Timestamp,
                                                            EventTracking_Id?              EventTrackingId,
                                                            TimeSpan?                      RequestTimeout,
                                                            CancellationToken              CancellationToken)


                => Task.FromResult(
                       UpdateChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:               this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region ReplaceChargingStations      (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of charging stations.
        /// Charging stations not included will be deleted.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations to replace.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<ReplaceChargingStationsResult>

            ISendChargingStationData.ReplaceChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                                             TransmissionTypes              TransmissionType,

                                                             DateTime?                      Timestamp,
                                                             EventTracking_Id?              EventTrackingId,
                                                             TimeSpan?                      RequestTimeout,
                                                             CancellationToken              CancellationToken)


                => Task.FromResult(
                       ReplaceChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:               this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion

        #region DeleteChargingStations       (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteChargingStationsResult>

            ISendChargingStationData.DeleteChargingStations(IEnumerable<IChargingStation>  ChargingStations,
                                                            TransmissionTypes              TransmissionType,

                                                            DateTime?                      Timestamp,
                                                            EventTracking_Id?              EventTrackingId,
                                                            TimeSpan?                      RequestTimeout,
                                                            CancellationToken              CancellationToken)


                => Task.FromResult(
                       DeleteChargingStationsResult.NoOperation(
                           RejectedChargingStations:  ChargingStations,
                           SenderId:                  Id,
                           Sender:               this,
                           EventTrackingId:           EventTrackingId
                       )
                   );

        #endregion


        #region UpdateChargingStationAdminStatus (ChargingStationAdminStatusUpdates,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationAdminStatusResult>

            ISendAdminStatus.UpdateChargingStationAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  ChargingStationAdminStatusUpdates,
                                                              TransmissionTypes                              TransmissionType,

                                                              DateTime?                                      Timestamp,
                                                              EventTracking_Id?                              EventTrackingId,
                                                              TimeSpan?                                      RequestTimeout,
                                                              CancellationToken                              CancellationToken)


        {

            return Task.FromResult(PushChargingStationAdminStatusResult.OutOfService(Id,
                                                                                     this,
                                                                                     ChargingStationAdminStatusUpdates));

        }

        #endregion

        #region UpdateChargingStationStatus      (ChargingStationStatusUpdates,       TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="ChargingStationStatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationStatusResult>

            ISendStatus.UpdateChargingStationStatus(IEnumerable<ChargingStationStatusUpdate>  ChargingStationStatusUpdates,
                                                    TransmissionTypes                         TransmissionType,

                                                    DateTime?                                 Timestamp,
                                                    EventTracking_Id                          EventTrackingId,
                                                    TimeSpan?                                 RequestTimeout,
                                                    CancellationToken                         CancellationToken)


                => Task.FromResult(PushChargingStationStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateChargingStationEnergyStatus(ChargingStationEnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushChargingStationEnergyStatusResult>

            ISendEnergyStatus.UpdateChargingStationEnergyStatus(IEnumerable<ChargingStationEnergyStatusUpdate>  ChargingStationEnergyStatusUpdates,
                                                               TransmissionTypes                                TransmissionType,

                                                               DateTime?                                        Timestamp,
                                                               EventTracking_Id?                                EventTrackingId,
                                                               TimeSpan?                                        RequestTimeout,
                                                               CancellationToken                                CancellationToken)


        {

            return Task.FromResult(PushChargingStationEnergyStatusResult.OutOfService(Id,
                                                                                      this,
                                                                                      ChargingStationEnergyStatusUpdates));

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) EVSE(s)...

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean                DisableSendEVSEData    { get; set; }

        /// <summary>
        /// Only include EVSE identifications matching the given delegate.
        /// </summary>
        public IncludeEVSEIdDelegate  IncludeEVSEIds         { get; }

        /// <summary>
        /// Only include EVSEs matching the given delegate.
        /// </summary>
        public IncludeEVSEDelegate    IncludeEVSEs           { get; }


        #region AddEVSE           (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEResult>

            AddEVSE(IEVSE              EVSE,
                    TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                    DateTime?          Timestamp           = null,
                    EventTracking_Id?  EventTrackingId     = null,
                    TimeSpan?          RequestTimeout      = null,
                    CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTracking_Id.New,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddEVSEIfNotExists(EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE, if it does not already exist.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEResult>

            AddEVSEIfNotExists(IEVSE              EVSE,
                               TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTracking_Id.New,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region AddOrUpdateEVSE   (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add or update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateEVSEResult>

            AddOrUpdateEVSE(IEVSE              EVSE,
                            TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                            DateTime?          Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTracking_Id.New,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region UpdateEVSE        (EVSE,  PropertyName, NewValue, OldValue = null, DataSource = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE property update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateEVSEResult>

            UpdateEVSE(IEVSE              EVSE,
                       String             PropertyName,
                       Object?            NewValue,
                       Object?            OldValue            = null,
                       Context?           DataSource          = null,
                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                       DateTime?          Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       UpdateEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTracking_Id.New,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion

        #region DeleteEVSE        (EVSE,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteEVSEResult>

            DeleteEVSE(IEVSE              EVSE,
                       TransmissionTypes  TransmissionType    = TransmissionTypes.Enqueue,

                       DateTime?          Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       CancellationToken  CancellationToken   = default)


                => Task.FromResult(
                       DeleteEVSEResult.NoOperation(
                           EVSE:             EVSE,
                           EventTrackingId:  EventTracking_Id.New,
                           SenderId:         Id,
                           Sender:      this
                       )
                   );

        #endregion


        #region AddEVSEs          (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEsResult>

            AddEVSEs(IEnumerable<IEVSE>  EVSEs,
                     TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                     DateTime?           Timestamp           = null,
                     EventTracking_Id?   EventTrackingId     = null,
                     TimeSpan?           RequestTimeout      = null,
                     CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:      this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region AddEVSEsIfNotExist(EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs, if they do not already exist.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add, if they do not already exist.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddEVSEsResult>

            AddEVSEsIfNotExist(IEnumerable<IEVSE>  EVSEs,
                               TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                               DateTime?           Timestamp           = null,
                               EventTracking_Id?   EventTrackingId     = null,
                               TimeSpan?           RequestTimeout      = null,
                               CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       AddEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:      this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region AddOrUpdateEVSEs  (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add or update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add or update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<AddOrUpdateEVSEsResult>

            AddOrUpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTime?           Timestamp           = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null,
                             CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       AddOrUpdateEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:      this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region UpdateEVSEs       (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<UpdateEVSEsResult>

            UpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       UpdateEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:      this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region ReplaceEVSEs      (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Replace the given enumeration of EVSEs.
        /// EVSEs not included will be deleted.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to replace.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<ReplaceEVSEsResult>

            ReplaceEVSEs(IEnumerable<IEVSE>  EVSEs,
                         TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                         DateTime?           Timestamp           = null,
                         EventTracking_Id?   EventTrackingId     = null,
                         TimeSpan?           RequestTimeout      = null,
                         CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       ReplaceEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:      this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion

        #region DeleteEVSEs       (EVSEs, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public virtual Task<DeleteEVSEsResult>

            DeleteEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default)


                => Task.FromResult(
                       DeleteEVSEsResult.NoOperation(
                           RejectedEVSEs:    EVSEs,
                           SenderId:         Id,
                           Sender:      this,
                           EventTrackingId:  EventTrackingId
                       )
                   );

        #endregion


        #region UpdateEVSEAdminStatus (EVSEAdminStatusUpdates,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEAdminStatusResult>

            ISendAdminStatus.UpdateEVSEAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  EVSEAdminStatusUpdates,
                                                   TransmissionTypes                   TransmissionType,

                                                   DateTime?                           Timestamp,
                                                   EventTracking_Id?                   EventTrackingId,
                                                   TimeSpan?                           RequestTimeout,
                                                   CancellationToken                   CancellationToken)


        {

            return Task.FromResult(PushEVSEAdminStatusResult.OutOfService(Id,
                                                                          this,
                                                                          EVSEAdminStatusUpdates));

        }

        #endregion

        #region UpdateEVSEStatus      (EVSEStatusUpdates,       TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEStatusResult>

            ISendStatus.UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,
                                         TransmissionTypes              TransmissionType,

                                         DateTime?                      Timestamp,
                                         EventTracking_Id?              EventTrackingId,
                                         TimeSpan?                      RequestTimeout,
                                         CancellationToken              CancellationToken)

        {

            #region Initial checks

            if (EVSEStatusUpdates == null || !EVSEStatusUpdates.Any())
                return Task.FromResult(PushEVSEStatusResult.NoOperation(Id, this));

            #endregion

            return Task.FromResult(PushEVSEStatusResult.NoOperation(Id, this));

        }

        #endregion

        #region UpdateEVSEEnergyStatus(EVSEEnergyStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<PushEVSEEnergyStatusResult>

            ISendEnergyStatus.UpdateEVSEEnergyStatus(IEnumerable<EVSEEnergyStatusUpdate>  EVSEEnergyStatusUpdates,
                                                     TransmissionTypes                    TransmissionType,

                                                     DateTime?                            Timestamp,
                                                     EventTracking_Id?                    EventTrackingId,
                                                     TimeSpan?                            RequestTimeout,
                                                     CancellationToken                    CancellationToken)


        {

            return Task.FromResult(PushEVSEEnergyStatusResult.OutOfService(Id,
                                                                           this,
                                                                           EVSEEnergyStatusUpdates));

        }

        #endregion

        #endregion

        #endregion


        #region Delayed upstream methods...

        #region EnqueueEVSEStatusUpdate(EVSE, OldStatus, NewStatus)

        ///// <summary>
        ///// Enqueue the given EVSE status for a delayed upload.
        ///// </summary>
        ///// <param name="EVSE">An EVSE.</param>
        ///// <param name="OldStatus">The old status of the EVSE.</param>
        ///// <param name="NewStatus">The new status of the EVSE.</param>
        //public Task<PushEVSEStatusResult>

        //    EnqueueEVSEStatusUpdate(EVSE                          EVSE,
        //                            Timestamped<EVSEStatusType>  OldStatus,
        //                            Timestamped<EVSEStatusType>  NewStatus)

        //{

        //    return Task.FromResult(PushEVSEStatusResult.NoOperation(Id, this, null));

        //}

        #endregion


        #region EnqueueChargingPoolDataUpdate(ChargingPool, PropertyName, OldValue, NewValue)

        ///// <summary>
        ///// Enqueue the given EVSE data for a delayed upload.
        ///// </summary>
        ///// <param name="ChargingPool">A charging station.</param>
        //public Task<PushEVSEDataResult>

        //    EnqueueChargingPoolDataUpdate(ChargingPool  ChargingPool,
        //                                  String        PropertyName,
        //                                  Object        OldValue,
        //                                  Object        NewValue)

        //{

        //    #region Initial checks

        //    if (ChargingPool == null)
        //        throw new ArgumentNullException(nameof(ChargingPool), "The given charging station must not be null!");

        //    #endregion

        //    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, null));

        //}

        #endregion

        #region EnqueueChargingStationDataUpdate(ChargingStation, PropertyName, OldValue, NewValue)

        ///// <summary>
        ///// Enqueue the given EVSE data for a delayed upload.
        ///// </summary>
        ///// <param name="ChargingStation">A charging station.</param>
        //public Task<PushEVSEDataResult>

        //    EnqueueChargingStationDataUpdate(ChargingStation  ChargingStation,
        //                                     String           PropertyName,
        //                                     Object           OldValue,
        //                                     Object           NewValue)

        //{

        //    #region Initial checks

        //    if (ChargingStation == null)
        //        throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

        //    #endregion

        //    return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, null));

        //}

        #endregion

        #endregion



        #region Operator overloading

        #region Operator == (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two smart citys for equality.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SmartCityProxy SmartCity1, SmartCityProxy SmartCity2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SmartCity1, SmartCity2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SmartCity1 == null) || ((Object) SmartCity2 == null))
                return false;

            return SmartCity1.Equals(SmartCity2);

        }

        #endregion

        #region Operator != (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two smart citys for inequality.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SmartCityProxy SmartCity1, SmartCityProxy SmartCity2)

            => !(SmartCity1 == SmartCity2);

        #endregion

        #region Operator <  (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (SmartCityProxy  SmartCity1,
                                          SmartCityProxy  SmartCity2)
        {

            if ((Object) SmartCity1 == null)
                throw new ArgumentNullException(nameof(SmartCity1),  "The given smart city must not be null!");

            return SmartCity1.CompareTo(SmartCity2) < 0;

        }

        #endregion

        #region Operator <= (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (SmartCityProxy SmartCity1,
                                           SmartCityProxy SmartCity2)

            => !(SmartCity1 > SmartCity2);

        #endregion

        #region Operator >  (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (SmartCityProxy SmartCity1,
                                          SmartCityProxy SmartCity2)
        {

            if ((Object) SmartCity1 == null)
                throw new ArgumentNullException(nameof(SmartCity1),  "The given smart city must not be null!");

            return SmartCity1.CompareTo(SmartCity2) > 0;

        }

        #endregion

        #region Operator >= (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (SmartCityProxy SmartCity1,
                                           SmartCityProxy SmartCity2)

            => !(SmartCity1 < SmartCity2);

        #endregion

        #endregion

        #region IComparable<SmartCityStub> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var SmartCityStub = Object as SmartCityProxy;
            if ((Object) SmartCityStub == null)
                throw new ArgumentException("The given object is not an SmartCityStub!", nameof(Object));

            return CompareTo(SmartCityStub);

        }

        #endregion

        #region CompareTo(SmartCityStub)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityStub">An SmartCityStub object to compare with.</param>
        public Int32 CompareTo(SmartCityProxy SmartCityStub)
        {

            if ((Object) SmartCityStub == null)
                throw new ArgumentNullException(nameof(SmartCityStub), "The given SmartCityStub must not be null!");

            return Id.CompareTo(SmartCityStub.Id);

        }

        #endregion

        #endregion

        #region IEquatable<SmartCityStub> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            var SmartCityStub = Object as SmartCityProxy;
            if ((Object) SmartCityStub == null)
                return false;

            return this.Equals(SmartCityStub);

        }

        #endregion

        #region Equals(SmartCityStub)

        /// <summary>
        /// Compares two SmartCityStub for equality.
        /// </summary>
        /// <param name="SmartCityStub">An SmartCityStub to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SmartCityProxy SmartCityStub)
        {

            if ((Object) SmartCityStub == null)
                return false;

            return Id.Equals(SmartCityStub.Id);

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

            => "Smart City " + Id;

        #endregion


    }

}
