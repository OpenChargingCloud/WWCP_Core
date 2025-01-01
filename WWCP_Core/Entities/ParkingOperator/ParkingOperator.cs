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

using System.Collections;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this ParkingOperator, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this ParkingOperator  ParkingOperator,
                                     Boolean               Embedded                        = false,
                                     Boolean               ExpandChargingRoamingNetworkId  = false,
                                     Boolean               ExpandChargingPoolIds           = false,
                                     Boolean               ExpandChargingStationIds        = false,
                                     Boolean               ExpandEVSEIds                   = false)

            => ParkingOperator != null
                   ? JSONObject.Create(

                         new JProperty("id",                        ParkingOperator.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      ParkingOperator.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    ParkingOperator.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  ParkingOperator.Name.       ToJSON()),
                         new JProperty("description",           ParkingOperator.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         ParkingOperator.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          ParkingOperator.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         ParkingOperator.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            ParkingOperator.Homepage)
                             : null,

                         ParkingOperator.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             ParkingOperator.HotlinePhoneNumber)
                             : null,

                         ParkingOperator.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(ParkingOperator.DataLicenses.Select(license => license.ToJSON())))
                             : null

                         //new JProperty("chargingPools",         ExpandChargingPoolIds
                         //                                           ? new JArray(ParkingOperator.ChargingPools.     ToJSON(Embedded: true))
                         //                                           : new JArray(ParkingOperator.ChargingPoolIds.   Select(id => id.ToString()))),

                         //new JProperty("chargingStations",      ExpandChargingStationIds
                         //                                           ? new JArray(ParkingOperator.ChargingStations.  ToJSON(Embedded: true))
                         //                                           : new JArray(ParkingOperator.ChargingStationIds.Select(id => id.ToString()))),

                         //new JProperty("evses",                 ExpandEVSEIds
                         //                                           ? new JArray(ParkingOperator.EVSEs.             ToJSON(Embedded: true))
                         //                                           : new JArray(ParkingOperator.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this ParkingOperator, JPropertyKey)

        public static JProperty ToJSON(this ParkingOperator ParkingOperator, String JPropertyKey)

            => ParkingOperator != null
                   ? new JProperty(JPropertyKey, ParkingOperator.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this ParkingOperators, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="ParkingOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<ParkingOperator>  ParkingOperators,
                                    UInt64?                            Skip                            = null,
                                    UInt64?                            Take                            = null,
                                    Boolean                            Embedded                        = false,
                                    Boolean                            ExpandChargingRoamingNetworkId  = false,
                                    Boolean                            ExpandChargingPoolIds           = false,
                                    Boolean                            ExpandChargingStationIds        = false,
                                    Boolean                            ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (ParkingOperators == null)
                return new JArray();

            #endregion

            return new JArray(ParkingOperators.
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

        #region ToJSON(this ParkingOperators, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ParkingOperator> ParkingOperators, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ParkingOperators != null
                       ? new JProperty(JPropertyKey, ParkingOperators.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this ParkingOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<ParkingOperatorAdminStatusTypes>>  ParkingOperatorAdminStatus,
                                     UInt64?                                                        Skip         = null,
                                     UInt64?                                                        Take         = null,
                                     UInt64?                                                        HistorySize  = 1)

        {

            if (ParkingOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorAdminStatus.
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
                // e.g. when a Stack behind ParkingOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this ParkingOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusTypes>>>>  ParkingOperatorAdminStatus,
                                     UInt64?                                                                                                       Skip         = null,
                                     UInt64?                                                                                                       Take         = null,
                                     UInt64?                                                                                                       HistorySize  = 1)

        {

            if (ParkingOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorAdminStatus.
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
                // e.g. when a Stack behind ParkingOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this ParkingOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<ParkingOperatorStatusTypes>>  ParkingOperatorStatus,
                                     UInt64?                                                   Skip         = null,
                                     UInt64?                                                   Take         = null,
                                     UInt64?                                                   HistorySize  = 1)

        {

            if (ParkingOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorStatus.
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
                // e.g. when a Stack behind ParkingOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this ParkingOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusTypes>>>>  ParkingOperatorStatus,
                                     UInt64?                                                                                                  Skip         = null,
                                     UInt64?                                                                                                  Take         = null,
                                     UInt64?                                                                                                  HistorySize  = 1)

        {

            if (ParkingOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(ParkingOperatorStatus.
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
                // e.g. when a Stack behind ParkingOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }


    /// <summary>
    /// The parking operator is responsible for operating parking spaces.
    /// </summary>
    public class ParkingOperator : ACryptoEMobilityEntity<ParkingOperator_Id,
                                                          ParkingOperatorAdminStatusTypes,
                                                          ParkingOperatorStatusTypes>,
                                   IEquatable<ParkingOperator>, IComparable<ParkingOperator>, IComparable,
                                   IEnumerable<ParkingGarage>
    {

        #region Data

        /// <summary>
        /// The default max size of the admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusScheduleSize   = 15;

        /// <summary>
        /// The default max size of the status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusScheduleSize        = 15;

        #endregion

        #region Properties

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of the ParkingSpace Operator.
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

        private List<OpenDataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public IEnumerable<OpenDataLicense> DataLicenses
            => _DataLicenses;

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote charging station operator.
        /// </summary>
        [Optional]
        public IRemoteParkingOperator  RemoteParkingOperator    { get; }

        #endregion

        #region Events

        #region OnInvalidParkingSpaceIdAdded

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated ParkingSpaces changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnInvalidParkingSpaceIdAddedDelegate(DateTime Timestamp, ParkingOperator ParkingOperator, ParkingSpace_Id ParkingSpaceId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated ParkingSpaces changed.
        /// </summary>
        public event OnInvalidParkingSpaceIdAddedDelegate OnInvalidParkingSpaceIdAdded;

        #endregion

        #region OnInvalidParkingSpaceIdRemoved

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated ParkingSpaces changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public delegate void OnInvalidParkingSpaceIdRemovedDelegate(DateTime Timestamp, ParkingOperator ParkingOperator, ParkingSpace_Id ParkingSpaceId);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated ParkingSpaces changed.
        /// </summary>
        public event OnInvalidParkingSpaceIdRemovedDelegate OnInvalidParkingSpaceIdRemoved;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator (CSO) having the given
        /// charging station operator identification (CSO Id).
        /// </summary>
        /// <param name="Id">The unique identification of the Charging Station Operator.</param>
        /// <param name="Name">The offical (multi-language) name of the ParkingSpace Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the ParkingSpace Operator.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal ParkingOperator(ParkingOperator_Id                     Id,
                                 RoamingNetwork                         RoamingNetwork,
                                 I18NString?                            Name                           = null,
                                 I18NString?                            Description                    = null,
                                 Action<ParkingOperator>?               Configurator                   = null,
                                 RemoteParkingOperatorCreatorDelegate?  RemoteParkingOperatorCreator   = null,
                                 ParkingOperatorAdminStatusTypes?       InitialAdminStatus             = ParkingOperatorAdminStatusTypes.Operational,
                                 ParkingOperatorStatusTypes?            InitialStatus                  = ParkingOperatorStatusTypes.Available,
                                 UInt16?                                MaxAdminStatusScheduleSize     = DefaultMaxAdminStatusScheduleSize,
                                 UInt16?                                MaxStatusScheduleSize          = DefaultMaxStatusScheduleSize,

                                 String?                                DataSource                     = null,
                                 DateTime?                              LastChange                     = null,

                                 JObject?                               CustomData                     = null,
                                 UserDefinedDictionary?                 InternalData                   = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus         ?? ParkingOperatorAdminStatusTypes.Operational,
                   InitialStatus              ?? ParkingOperatorStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new List<OpenDataLicense>();

            #region InvalidParkingSpaceIds

            this.InvalidParkingSpaceIds            = new ReactiveSet<ParkingSpace_Id>();

            InvalidParkingSpaceIds.OnItemAdded += (Timestamp, Set, ParkingSpaceId) =>
            {
                var OnInvalidParkingSpaceIdAddedLocal = OnInvalidParkingSpaceIdAdded;
                if (OnInvalidParkingSpaceIdAddedLocal != null)
                    OnInvalidParkingSpaceIdAddedLocal(Timestamp, this, ParkingSpaceId);
            };

            InvalidParkingSpaceIds.OnItemRemoved += (Timestamp, Set, ParkingSpaceId) =>
            {
                var OnInvalidParkingSpaceIdRemovedLocal = OnInvalidParkingSpaceIdRemoved;
                if (OnInvalidParkingSpaceIdRemovedLocal != null)
                    OnInvalidParkingSpaceIdRemovedLocal(Timestamp, this, ParkingSpaceId);
            };

            #endregion

            this._ParkingGarages               = new EntityHashSet<ParkingOperator, ParkingGarage_Id,         ParkingGarage>(this);
            //this._ParkingGarageGroups         = new SpecialHashSet<ParkingOperator, ParkingGarageGroup_Id, ParkingGarageGroup>(this);
            //this._ParkingReservations        = new ConcurrentDictionary<ChargingReservation_Id, ParkingGarage>();
            //this._ChargingSessions            = new ConcurrentDictionary<ChargingSession_Id,     ParkingGarage>();

            #endregion

            #region Init events

            // Parking garage events
            this.ParkingGarageAddition         = new VotingNotificator<DateTime, ParkingOperator, ParkingGarage, Boolean>(() => new VetoVote(), true);
            this.ParkingGarageRemoval          = new VotingNotificator<DateTime, ParkingOperator, ParkingGarage, Boolean>(() => new VetoVote(), true);

            // Parking space events
            this.ParkingSpaceAddition          = new VotingNotificator<DateTime, ParkingGarage,   ParkingSpace,  Boolean>(() => new VetoVote(), true);
            this.ParkingSpaceRemoval           = new VotingNotificator<DateTime, ParkingGarage,   ParkingSpace,  Boolean>(() => new VetoVote(), true);

            // Parking sensor events
            this.ParkingSensorAddition         = new VotingNotificator<DateTime, ParkingSpace,    ParkingSensor, Boolean>(() => new VetoVote(), true);
            this.ParkingSensorRemoval          = new VotingNotificator<DateTime, ParkingSpace,    ParkingSensor, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            // ParkingOperator events
            //this.OnParkingGarageAddition.   OnVoting       += (timestamp, ParkingOperator, pool, vote) => RoamingNetwork.ParkingGarageAddition.   SendVoting      (timestamp, ParkingOperator, pool, vote);
            //this.OnParkingGarageAddition.   OnNotification += (timestamp, ParkingOperator, pool)       => RoamingNetwork.ParkingGarageAddition.   SendNotification(timestamp, ParkingOperator, pool);

            //this.OnParkingGarageRemoval.    OnVoting       += (timestamp, ParkingOperator, pool, vote) => RoamingNetwork.ParkingGarageRemoval.    SendVoting      (timestamp, ParkingOperator, pool, vote);
            //this.OnParkingGarageRemoval.    OnNotification += (timestamp, ParkingOperator, pool)       => RoamingNetwork.ParkingGarageRemoval.    SendNotification(timestamp, ParkingOperator, pool);

            //// ParkingGarage events
            //this.OnParkingGarageAddition.OnVoting       += (timestamp, pool, station, vote)      => RoamingNetwork.ParkingGarageAddition.SendVoting      (timestamp, pool, station, vote);
            //this.OnParkingGarageAddition.OnNotification += (timestamp, pool, station)            => RoamingNetwork.ParkingGarageAddition.SendNotification(timestamp, pool, station);

            ////this.OnParkingGarageRemoval. OnVoting       += (timestamp, pool, station, vote)      => RoamingNetwork.ParkingGarageRemoval. SendVoting      (timestamp, pool, station, vote);
            //this.OnParkingGarageRemoval. OnNotification += (timestamp, pool, station)            => RoamingNetwork.ParkingGarageRemoval. SendNotification(timestamp,       station);

            //// ParkingGarage events
            //this.OnParkingSpaceAddition.           OnVoting       += (timestamp, station, evse, vote)      => RoamingNetwork.ParkingSpaceAddition.           SendVoting      (timestamp, station, evse, vote);
            //this.OnParkingSpaceAddition.           OnNotification += (timestamp, station, evse)            => RoamingNetwork.ParkingSpaceAddition.           SendNotification(timestamp, station, evse);

            //this.OnParkingSpaceRemoval.            OnVoting       += (timestamp, station, evse, vote)      => RoamingNetwork.ParkingSpaceRemoval.            SendVoting      (timestamp, station, evse, vote);
            //this.OnParkingSpaceRemoval.            OnNotification += (timestamp, station, evse)            => RoamingNetwork.ParkingSpaceRemoval.            SendNotification(timestamp, station, evse);

            //// ParkingSpace events
            //this.ParkingSensorAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => RoamingNetwork.ParkingSensorAddition.   SendVoting      (timestamp, evse, outlet, vote);
            //this.ParkingSensorAddition.     OnNotification += (timestamp, evse, outlet)             => RoamingNetwork.ParkingSensorAddition.   SendNotification(timestamp, evse, outlet);

            //this.ParkingSensorRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => RoamingNetwork.ParkingSensorRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            //this.ParkingSensorRemoval.      OnNotification += (timestamp, evse, outlet)             => RoamingNetwork.ParkingSensorRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

            LocalParkingSpaceIds = new ReactiveSet<ParkingSpace_Id>();

            Configurator?.Invoke(this);

            this.RemoteParkingOperator = RemoteParkingOperatorCreator?.Invoke(this);

        }

        #endregion


        #region  Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnParkingOperatorDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnParkingOperatorAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnParkingOperatorStatusChangedDelegate       OnStatusChanged;

        #endregion


        #region (internal) UpdateData(Timestamp, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="Sender">The changed Charging Station Operator.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime  Timestamp,
                                       Object    Sender,
                                       String    PropertyName,
                                       Object    OldValue,
                                       Object    NewValue)
        {

            var onDataChanged = OnDataChanged;
            if (onDataChanged != null)
                await onDataChanged(Timestamp, Sender as ParkingOperator, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old ParkingSpace status.</param>
        /// <param name="NewStatus">The new ParkingSpace status.</param>
        internal async Task UpdateStatus(DateTime                             Timestamp,
                                         Timestamped<ParkingOperatorStatusTypes>  OldStatus,
                                         Timestamped<ParkingOperatorStatusTypes>  NewStatus)
        {

            var onStatusChanged = OnStatusChanged;
            if (onStatusChanged != null)
                await onStatusChanged(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                  Timestamp,
                                              Timestamped<ParkingOperatorAdminStatusTypes>  OldStatus,
                                              Timestamped<ParkingOperatorAdminStatusTypes>  NewStatus)
        {

            var onAdminStatusChanged = OnAdminStatusChanged;
            if (onAdminStatusChanged != null)
                await onAdminStatusChanged(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region Charging pools

        #region ParkingGarageAddition

        internal readonly IVotingNotificator<DateTime, ParkingOperator, ParkingGarage, Boolean> ParkingGarageAddition;

        /// <summary>
        /// Called whenever an charging pool will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ParkingOperator, ParkingGarage, Boolean> OnParkingGarageAddition

            => ParkingGarageAddition;

        #endregion

        #region ParkingGarageRemoval

        //internal readonly IVotingNotificator<DateTime, ParkingOperator, ParkingGarage, Boolean> ParkingGarageRemoval;

        ///// <summary>
        ///// Called whenever an charging pool will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, ParkingOperator, ParkingGarage, Boolean> OnParkingGarageRemoval

        //    => ParkingGarageRemoval;

        #endregion


        #region ParkingGarages

        private EntityHashSet<ParkingOperator, ParkingGarage_Id, ParkingGarage> _ParkingGarages;

        public IEnumerable<ParkingGarage> ParkingGarages

            => _ParkingGarages;

        #endregion

        #region ParkingGarageAdminStatus(IncludePool = null)

        //public IEnumerable<KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>> ParkingGarageAdminStatus(Func<ParkingGarage, Boolean> IncludePool = null)

        //    => _ParkingGarages.
        //           Where  (pool => IncludePool == null || IncludePool(pool)).
        //           OrderBy(pool => pool.Id).
        //           Select (pool => new KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>(pool.Id, pool.AdminStatus.Value));

        #endregion


        #region CreateNewParkingGarage(ParkingGarageId = null, Configurator = null, OnSuccess = null, OnError = null)

        ///// <summary>
        ///// Create and register a new charging pool having the given
        ///// unique charging pool identification.
        ///// </summary>
        ///// <param name="ParkingGarageId">The unique identification of the new charging pool.</param>
        ///// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        ///// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        ///// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        //public ParkingGarage CreateNewParkingGarage(ParkingGarage_Id                                   ParkingGarageId             = null,
        //                                          Action<ParkingGarage>                              Configurator               = null,
        //                                          RemoteParkingGarageCreatorDelegate                 RemoteParkingGarageCreator  = null,
        //                                          ParkingGarageAdminStatusType                       AdminStatus                = ParkingGarageAdminStatusType.Operational,
        //                                          ParkingGarageStatusType                            Status                     = ParkingGarageStatusType.Available,
        //                                          Action<ParkingGarage>                              OnSuccess                  = null,
        //                                          Action<ParkingOperator, ParkingGarage_Id>  OnError                    = null)

        //{

        //    #region Initial checks

        //    if (ParkingGarageId == null)
        //        ParkingGarageId = ParkingGarage_Id.Random(this.Id);

        //    // Do not throw an exception when an OnError delegate was given!
        //    if (_ParkingGarages.Any(pool => pool.Id == ParkingGarageId))
        //    {
        //        if (OnError == null)
        //            throw new ParkingGarageAlreadyExists(ParkingGarageId, this.Id);
        //        else
        //            OnError?.Invoke(this, ParkingGarageId);
        //    }

        //    #endregion

        //    var _ParkingGarage = new ParkingGarage(ParkingGarageId,
        //                                         this,
        //                                         Configurator,
        //                                         RemoteParkingGarageCreator,
        //                                         AdminStatus,
        //                                         Status);


        //    if (ParkingGarageAddition.SendVoting(Timestamp.Now, this, _ParkingGarage))
        //    {
        //        if (_ParkingGarages.TryAdd(_ParkingGarage))
        //        {

        //            _ParkingGarage.OnParkingSpaceDataChanged                    += UpdateParkingSpaceData;
        //            _ParkingGarage.OnParkingSpaceStatusChanged                  += UpdateParkingSpaceStatus;
        //            _ParkingGarage.OnParkingSpaceAdminStatusChanged             += UpdateParkingSpaceAdminStatus;

        //            _ParkingGarage.OnParkingGarageDataChanged         += UpdateParkingGarageData;
        //            _ParkingGarage.OnParkingGarageStatusChanged       += UpdateParkingGarageStatus;
        //            _ParkingGarage.OnParkingGarageAdminStatusChanged  += UpdateParkingGarageAdminStatus;

        //            _ParkingGarage.OnDataChanged                        += UpdateParkingGarageData;
        //            _ParkingGarage.OnStatusChanged                      += UpdateParkingGarageStatus;
        //            _ParkingGarage.OnAdminStatusChanged                 += UpdateParkingGarageAdminStatus;

        //            _ParkingGarage.OnNewReservation                     += SendNewReservation;
        //            _ParkingGarage.OnCancelReservationResponse               += SendOnCancelReservationResponse;
        //            _ParkingGarage.OnNewChargingSession                 += SendNewChargingSession;
        //            _ParkingGarage.OnNewChargeDetailRecord              += SendNewChargeDetailRecord;


        //            OnSuccess?.Invoke(_ParkingGarage);
        //            ParkingGarageAddition.SendNotification(Timestamp.Now, this, _ParkingGarage);

        //            return _ParkingGarage;

        //        }
        //    }

        //    return null;

        //}

        #endregion


        #region ContainsParkingGarage(ParkingGarage)

        /// <summary>
        /// Check if the given ParkingGarage is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ParkingGarage">A charging pool.</param>
        public Boolean ContainsParkingGarage(ParkingGarage ParkingGarage)

            => _ParkingGarages.Contains(ParkingGarage);

        #endregion

        #region ContainsParkingGarage(ParkingGarageId)

        /// <summary>
        /// Check if the given ParkingGarage identification is already present within the Charging Station Operator.
        /// </summary>
        /// <param name="ParkingGarageId">The unique identification of the charging pool.</param>
        public Boolean ContainsParkingGarage(ParkingGarage_Id ParkingGarageId)

            => _ParkingGarages.ContainsId(ParkingGarageId);

        #endregion

        #region GetParkingGaragebyId(ParkingGarageId)

        public ParkingGarage GetParkingGaragebyId(ParkingGarage_Id ParkingGarageId)

            => _ParkingGarages.GetById(ParkingGarageId);

        #endregion

        #region TryGetParkingGaragebyId(ParkingGarageId, out ParkingGarage)

        public Boolean TryGetParkingGaragebyId(ParkingGarage_Id ParkingGarageId, out ParkingGarage ParkingGarage)

            => _ParkingGarages.TryGet(ParkingGarageId, out ParkingGarage);

        #endregion

        #region RemoveParkingGarage(ParkingGarageId)

        //public ParkingGarage RemoveParkingGarage(ParkingGarage_Id ParkingGarageId)
        //{

        //    ParkingGarage _ParkingGarage = null;

        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //    {

        //        if (ParkingGarageRemoval.SendVoting(Timestamp.Now, this, _ParkingGarage))
        //        {

        //            if (_ParkingGarages.TryRemove(ParkingGarageId, out _ParkingGarage))
        //            {

        //                ParkingGarageRemoval.SendNotification(Timestamp.Now, this, _ParkingGarage);

        //                return _ParkingGarage;

        //            }

        //        }

        //    }

        //    return null;

        //}

        #endregion

        #region TryRemoveParkingGarage(ParkingGarageId, out ParkingGarage)

        public Boolean TryRemoveParkingGarage(ParkingGarage_Id ParkingGarageId, out ParkingGarage ParkingGarage)
        {

            if (TryGetParkingGaragebyId(ParkingGarageId, out ParkingGarage))
            {

                if (ParkingGarageRemoval.SendVoting(EventTracking_Id.New, Timestamp.Now, this, ParkingGarage))
                {

                    if (_ParkingGarages.TryRemove(ParkingGarageId,
                                                  out ParkingGarage,
                                                  EventTracking_Id.New,
                                                  null))
                    {

                        ParkingGarageRemoval.SendNotification(EventTracking_Id.New, Timestamp.Now, this, ParkingGarage);

                        return true;

                    }

                }

                return false;

            }

            return true;

        }

        #endregion

        #region SetParkingGarageAdminStatus(ParkingGarageId, NewStatus)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id                           ParkingGarageId,
        //                                       Timestamped<ParkingGarageAdminStatusType>  NewStatus,
        //                                       Boolean                                   SendUpstream = false)
        //{

        //    ParkingGarage _ParkingGarage = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(NewStatus);

        //}

        #endregion

        #region SetParkingGarageAdminStatus(ParkingGarageId, NewStatus, Timestamp)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id              ParkingGarageId,
        //                                       ParkingGarageAdminStatusType  NewStatus,
        //                                       DateTime                     Timestamp)
        //{

        //    ParkingGarage _ParkingGarage  = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(NewStatus, Timestamp);

        //}

        #endregion

        #region SetParkingGarageAdminStatus(ParkingGarageId, StatusList, ChangeMethod = ChangeMethods.Replace)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id                                        ParkingGarageId,
        //                                       IEnumerable<Timestamped<ParkingGarageAdminStatusType>>  StatusList,
        //                                       ChangeMethods                                          ChangeMethod  = ChangeMethods.Replace)
        //{

        //    ParkingGarage _ParkingGarage  = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(StatusList, ChangeMethod);

        //    //if (SendUpstream)
        //    //{
        //    //
        //    //    RoamingNetwork.
        //    //        SendParkingGarageAdminStatusDiff(new ParkingGarageAdminStatusDiff(Timestamp.Now,
        //    //                                               ParkingOperatorId:    Id,
        //    //                                               ParkingOperatorName:  Name,
        //    //                                               NewStatus:         new List<KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>>(),
        //    //                                               ChangedStatus:     new List<KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>>() {
        //    //                                                                          new KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>(ParkingGarageId, NewStatus.Value)
        //    //                                                                      },
        //    //                                               RemovedIds:        new List<ParkingGarage_Id>()));
        //    //
        //    //}

        //}

        #endregion


        #region OnParkingGarageData/(Admin)StatusChanged

        ///// <summary>
        ///// An event fired whenever the static data of any subordinated charging pool changed.
        ///// </summary>
        //public event OnParkingGarageDataChangedDelegate         OnParkingGarageDataChanged;

        ///// <summary>
        ///// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        ///// </summary>
        //public event OnParkingGarageStatusChangedDelegate       OnParkingGarageStatusChanged;

        ///// <summary>
        ///// An event fired whenever the aggregated dynamic status of any subordinated charging pool changed.
        ///// </summary>
        //public event OnParkingGarageAdminStatusChangedDelegate  OnParkingGarageAdminStatusChanged;

        #endregion

        #region ParkingGarageAddition

        //internal readonly IVotingNotificator<DateTime, ParkingGarage, ParkingGarage, Boolean> ParkingGarageAddition;

        ///// <summary>
        ///// Called whenever a charging station will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, ParkingOperator, ParkingGarage, Boolean> OnParkingGarageAddition

        //    => ParkingGarageAddition;

        #endregion

        #region ParkingGarageRemoval

        internal readonly IVotingNotificator<DateTime, ParkingOperator, ParkingGarage, Boolean> ParkingGarageRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ParkingOperator, ParkingGarage, Boolean> OnParkingGarageRemoval

            => ParkingGarageRemoval;

        #endregion


        #region (internal) UpdateParkingGarageData(Timestamp, ParkingGarage, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the data of an charging pool.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingGarage">The changed charging pool.</param>
        ///// <param name="PropertyName">The name of the changed property.</param>
        ///// <param name="OldValue">The old value of the changed property.</param>
        ///// <param name="NewValue">The new value of the changed property.</param>
        //internal async Task UpdateParkingGarageData(DateTime      Timestamp,
        //                                           ParkingGarage  ParkingGarage,
        //                                           String        PropertyName,
        //                                           Object        OldValue,
        //                                           Object        NewValue)
        //{

        //    var OnParkingGarageDataChangedLocal = OnParkingGarageDataChanged;
        //    if (OnParkingGarageDataChangedLocal != null)
        //        await OnParkingGarageDataChangedLocal(Timestamp, ParkingGarage, PropertyName, OldValue, NewValue);

        //}

        #endregion

        #region (internal) UpdateParkingGarageStatus(Timestamp, ParkingGarage, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the current charging pool status.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingGarage">The updated charging pool.</param>
        ///// <param name="OldStatus">The old aggreagted charging station status.</param>
        ///// <param name="NewStatus">The new aggreagted charging station status.</param>
        //internal async Task UpdateParkingGarageStatus(DateTime                             Timestamp,
        //                                             ParkingGarage                         ParkingGarage,
        //                                             Timestamped<ParkingGarageStatusType>  OldStatus,
        //                                             Timestamped<ParkingGarageStatusType>  NewStatus)
        //{

        //    var OnParkingGarageStatusChangedLocal = OnParkingGarageStatusChanged;
        //    if (OnParkingGarageStatusChangedLocal != null)
        //        await OnParkingGarageStatusChangedLocal(Timestamp, ParkingGarage, OldStatus, NewStatus);

        //    //if (StatusAggregationDelegate != null)
        //    //    _StatusSchedule.Insert(StatusAggregationDelegate(new ParkingGarageStatusReport(_ParkingGarages.Values)),
        //    //                           Timestamp);

        //}

        #endregion

        #region (internal) UpdateParkingGarageAdminStatus(Timestamp, ParkingGarage, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the current charging pool admin status.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingGarage">The updated charging pool.</param>
        ///// <param name="OldStatus">The old aggreagted charging station status.</param>
        ///// <param name="NewStatus">The new aggreagted charging station status.</param>
        //internal async Task UpdateParkingGarageAdminStatus(DateTime                                  Timestamp,
        //                                                  ParkingGarage                              ParkingGarage,
        //                                                  Timestamped<ParkingGarageAdminStatusType>  OldStatus,
        //                                                  Timestamped<ParkingGarageAdminStatusType>  NewStatus)
        //{

        //    var OnParkingGarageAdminStatusChangedLocal = OnParkingGarageAdminStatusChanged;
        //    if (OnParkingGarageAdminStatusChangedLocal != null)
        //        await OnParkingGarageAdminStatusChangedLocal(Timestamp, ParkingGarage, OldStatus, NewStatus);

        //}

        #endregion


        #region IEnumerable<ParkingGarage> Members

        IEnumerator IEnumerable.GetEnumerator()

            => _ParkingGarages.GetEnumerator();

        public IEnumerator<ParkingGarage> GetEnumerator()

            => _ParkingGarages.GetEnumerator();

        #endregion

        #endregion

        #region Charging stations

        #region ParkingGarages

        //public IEnumerable<ParkingGarage> ParkingGarages

        //    => _ParkingGarages.
        //           SelectMany(pool => pool.ParkingGarages);

        #endregion

        #region ParkingGarageIds

        //public IEnumerable<ParkingGarage_Id> ParkingGarageIds

        //    => _ParkingGarages.
        //           SelectMany(pool    => pool.   ParkingGarages).
        //           Select    (station => station.Id);

        #endregion

        #region ParkingGarageAdminStatus(IncludeStation = null)

        //public IEnumerable<KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusTypes>> ParkingGarageAdminStatus(Func<ParkingGarage, Boolean> IncludeStation = null)

        //    => _ParkingGarages.
        //           SelectMany(pool    => pool.ParkingGarages).
        //           Where     (station => IncludeStation == null || IncludeStation(station)).
        //           OrderBy   (station => station.Id).
        //           Select    (station => new KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusTypes>(station.Id, station.AdminStatus.Value));

        #endregion


        #region ContainsParkingGarage(ParkingGarage)

        ///// <summary>
        ///// Check if the given ParkingGarage is already present within the Charging Station Operator.
        ///// </summary>
        ///// <param name="ParkingGarage">A charging station.</param>
        //public Boolean ContainsParkingGarage(ParkingGarage ParkingGarage)

        //    => _ParkingGarages.Any(pool => pool.ContainsParkingGarage(ParkingGarage.Id));

        #endregion

        #region ContainsParkingGarage(ParkingGarageId)

        ///// <summary>
        ///// Check if the given ParkingGarage identification is already present within the Charging Station Operator.
        ///// </summary>
        ///// <param name="ParkingGarageId">The unique identification of the charging station.</param>
        //public Boolean ContainsParkingGarage(ParkingGarage_Id ParkingGarageId)

        //    => _ParkingGarages.Any(pool => pool.ContainsParkingGarage(ParkingGarageId));

        #endregion

        #region GetParkingGaragebyId(ParkingGarageId)

        //public ParkingGarage GetParkingGaragebyId(ParkingGarage_Id ParkingGarageId)

        //    => _ParkingGarages.
        //           SelectMany    (pool    => pool.ParkingGarages).
        //           FirstOrDefault(station => station.Id == ParkingGarageId);

        #endregion

        #region TryGetParkingGaragebyId(ParkingGarageId, out ParkingGarage ParkingGarage)

        //public Boolean TryGetParkingGaragebyId(ParkingGarage_Id ParkingGarageId, out ParkingGarage ParkingGarage)
        //{

        //    ParkingGarage = _ParkingGarages.
        //                          SelectMany    (pool    => pool.ParkingGarages).
        //                          FirstOrDefault(station => station.Id == ParkingGarageId);

        //    return ParkingGarage != null;

        //}

        #endregion


        #region SetParkingGarageStatus(ParkingGarageId, NewStatus)

        //public void SetParkingGarageStatus(ParkingGarage_Id         ParkingGarageId,
        //                                     ParkingGarageStatusTypes  NewStatus)
        //{

        //    ParkingGarage _ParkingGarage  = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetStatus(NewStatus);

        //}

        #endregion

        #region SetParkingGarageStatus(ParkingGarageId, NewTimestampedStatus)

        //public void SetParkingGarageStatus(ParkingGarage_Id                      ParkingGarageId,
        //                                     Timestamped<ParkingGarageStatusTypes>  NewTimestampedStatus)
        //{

        //    ParkingGarage _ParkingGarage = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetStatus(NewTimestampedStatus);

        //}

        #endregion


        #region SetParkingGarageAdminStatus(ParkingGarageId, NewStatus)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id              ParkingGarageId,
        //                                          ParkingGarageAdminStatusTypes  NewStatus)
        //{

        //    ParkingGarage _ParkingGarage  = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(NewStatus);

        //}

        #endregion

        #region SetParkingGarageAdminStatus(ParkingGarageId, NewTimestampedStatus)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id                           ParkingGarageId,
        //                                          Timestamped<ParkingGarageAdminStatusTypes>  NewTimestampedStatus)
        //{

        //    ParkingGarage _ParkingGarage = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(NewTimestampedStatus);

        //}

        #endregion

        #region SetParkingGarageAdminStatus(ParkingGarageId, NewStatus, Timestamp)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id              ParkingGarageId,
        //                                          ParkingGarageAdminStatusTypes  NewStatus,
        //                                          DateTime                        Timestamp)
        //{

        //    ParkingGarage _ParkingGarage  = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(NewStatus, Timestamp);

        //}

        #endregion

        #region SetParkingGarageAdminStatus(ParkingGarageId, StatusList, ChangeMethod = ChangeMethods.Replace)

        //public void SetParkingGarageAdminStatus(ParkingGarage_Id                                        ParkingGarageId,
        //                                          IEnumerable<Timestamped<ParkingGarageAdminStatusTypes>>  StatusList,
        //                                          ChangeMethods                                             ChangeMethod  = ChangeMethods.Replace)
        //{

        //    ParkingGarage _ParkingGarage  = null;
        //    if (TryGetParkingGaragebyId(ParkingGarageId, out _ParkingGarage))
        //        _ParkingGarage.SetAdminStatus(StatusList, ChangeMethod);

        //    //if (SendUpstream)
        //    //{
        //    //
        //    //    RoamingNetwork.
        //    //        SendParkingGarageAdminStatusDiff(new ParkingGarageAdminStatusDiff(Timestamp.Now,
        //    //                                               ParkingOperatorId:    Id,
        //    //                                               ParkingOperatorName:  Name,
        //    //                                               NewStatus:         new List<KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>>(),
        //    //                                               ChangedStatus:     new List<KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>>() {
        //    //                                                                          new KeyValuePair<ParkingGarage_Id, ParkingGarageAdminStatusType>(ParkingGarageId, NewStatus.Value)
        //    //                                                                      },
        //    //                                               RemovedIds:        new List<ParkingGarage_Id>()));
        //    //
        //    //}

        //}

        #endregion


        #region OnParkingGarageData/(Admin)StatusChanged

        ///// <summary>
        ///// An event fired whenever the static data of any subordinated charging station changed.
        ///// </summary>
        //public event OnParkingGarageDataChangedDelegate         OnParkingGarageDataChanged;

        ///// <summary>
        ///// An event fired whenever the aggregated dynamic status of any subordinated charging station changed.
        ///// </summary>
        //public event OnParkingGarageStatusChangedDelegate       OnParkingGarageStatusChanged;

        ///// <summary>
        ///// An event fired whenever the aggregated admin status of any subordinated charging station changed.
        ///// </summary>
        //public event OnParkingGarageAdminStatusChangedDelegate  OnParkingGarageAdminStatusChanged;

        #endregion

        #region ParkingSpaceAddition

        internal readonly IVotingNotificator<DateTime, ParkingGarage, ParkingSpace, Boolean> ParkingSpaceAddition;

        /// <summary>
        /// Called whenever an ParkingSpace will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ParkingGarage, ParkingSpace, Boolean> OnParkingSpaceAddition

            => ParkingSpaceAddition;

        #endregion

        #region ParkingSpaceRemoval

        internal readonly IVotingNotificator<DateTime, ParkingGarage, ParkingSpace, Boolean> ParkingSpaceRemoval;

        /// <summary>
        /// Called whenever an ParkingSpace will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ParkingGarage, ParkingSpace, Boolean> OnParkingSpaceRemoval

            => ParkingSpaceRemoval;

        #endregion


        #region (internal) UpdateParkingGarageData(Timestamp, ParkingGarage, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the data of a charging station.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingGarage">The changed charging station.</param>
        ///// <param name="PropertyName">The name of the changed property.</param>
        ///// <param name="OldValue">The old value of the changed property.</param>
        ///// <param name="NewValue">The new value of the changed property.</param>
        //internal async Task UpdateParkingGarageData(DateTime         Timestamp,
        //                                              ParkingGarage  ParkingGarage,
        //                                              String           PropertyName,
        //                                              Object           OldValue,
        //                                              Object           NewValue)
        //{

        //    var OnParkingGarageDataChangedLocal = OnParkingGarageDataChanged;
        //    if (OnParkingGarageDataChangedLocal != null)
        //        await OnParkingGarageDataChangedLocal(Timestamp, ParkingGarage, PropertyName, OldValue, NewValue);

        //}

        #endregion

        #region (internal) UpdateParkingGarageStatus(Timestamp, ParkingGarage, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the current aggregated charging station status.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingGarage">The updated charging station.</param>
        ///// <param name="OldStatus">The old aggreagted charging station status.</param>
        ///// <param name="NewStatus">The new aggreagted charging station status.</param>
        //internal async Task UpdateParkingGarageStatus(DateTime                                Timestamp,
        //                                                ParkingGarage                         ParkingGarage,
        //                                                Timestamped<ParkingGarageStatusTypes>  OldStatus,
        //                                                Timestamped<ParkingGarageStatusTypes>  NewStatus)
        //{

        //    var OnParkingGarageStatusChangedLocal = OnParkingGarageStatusChanged;
        //    if (OnParkingGarageStatusChangedLocal != null)
        //        await OnParkingGarageStatusChangedLocal(Timestamp, ParkingGarage, OldStatus, NewStatus);

        //}

        #endregion

        #region (internal) UpdateParkingGarageAdminStatus(Timestamp, ParkingGarage, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the current aggregated charging station admin status.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingGarage">The updated charging station.</param>
        ///// <param name="OldStatus">The old aggreagted charging station admin status.</param>
        ///// <param name="NewStatus">The new aggreagted charging station admin status.</param>
        //internal async Task UpdateParkingGarageAdminStatus(DateTime                                     Timestamp,
        //                                                     ParkingGarage                              ParkingGarage,
        //                                                     Timestamped<ParkingGarageAdminStatusTypes>  OldStatus,
        //                                                     Timestamped<ParkingGarageAdminStatusTypes>  NewStatus)
        //{

        //    var OnParkingGarageAdminStatusChangedLocal = OnParkingGarageAdminStatusChanged;
        //    if (OnParkingGarageAdminStatusChangedLocal != null)
        //        await OnParkingGarageAdminStatusChangedLocal(Timestamp, ParkingGarage, OldStatus, NewStatus);

        //}

        #endregion

        #endregion

        #region Charging station groups

        #region ParkingGarageGroupAddition

        //internal readonly IVotingNotificator<DateTime, ParkingOperator, ParkingGarageGroup, Boolean> ParkingGarageGroupAddition;

        ///// <summary>
        ///// Called whenever a charging station group will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, ParkingOperator, ParkingGarageGroup, Boolean> OnParkingGarageGroupAddition

        //    => ParkingGarageGroupAddition;

        #endregion

        #region ParkingGarageGroupRemoval

        //internal readonly IVotingNotificator<DateTime, ParkingOperator, ParkingGarageGroup, Boolean> ParkingGarageGroupRemoval;

        ///// <summary>
        ///// Called whenever an charging station group will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, ParkingOperator, ParkingGarageGroup, Boolean> OnParkingGarageGroupRemoval

        //    => ParkingGarageGroupRemoval;

        #endregion


        #region ParkingGarageGroups

        //private readonly SpecialHashSet<ParkingOperator, ParkingGarageGroup_Id, ParkingGarageGroup> _ParkingGarageGroups;

        ///// <summary>
        ///// All charging station groups registered within this Charging Station Operator.
        ///// </summary>
        //public IEnumerable<ParkingGarageGroup> ParkingGarageGroups

        //    => _ParkingGarageGroups;

        #endregion

        #region CreateNewParkingGarageGroup(ParkingGarageGroupId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        ///// Create and register a new charging group having the given
        ///// unique charging group identification.
        ///// </summary>
        ///// <param name="ParkingGarageGroupId">The unique identification of the new charging group.</param>
        ///// <param name="Configurator">An optional delegate to configure the new charging group before its successful creation.</param>
        ///// <param name="OnSuccess">An optional delegate to configure the new charging group after its successful creation.</param>
        ///// <param name="OnError">An optional delegate to be called whenever the creation of the charging group failed.</param>
        //public ParkingGarageGroup CreateNewParkingGarageGroup(ParkingGarageGroup_Id                                   ParkingGarageGroupId  = null,
        //                                                          Action<ParkingGarageGroup>                              Configurator            = null,
        //                                                          Action<ParkingGarageGroup>                              OnSuccess               = null,
        //                                                          Action<ParkingOperator, ParkingGarageGroup_Id>  OnError                 = null)
        //{

        //    #region Initial checks

        //    if (ParkingGarageGroupId == null)
        //        ParkingGarageGroupId = ParkingGarageGroup_Id.Random(this.Id);

        //    // Do not throw an exception when an OnError delegate was given!
        //    if (_ParkingGarageGroups.Contains(ParkingGarageGroupId))
        //    {
        //        if (OnError == null)
        //            throw new ParkingGarageGroupAlreadyExists(ParkingGarageGroupId, this.Id);
        //        else
        //            OnError?.Invoke(this, ParkingGarageGroupId);
        //    }

        //    #endregion

        //    var _ParkingGarageGroup = new ParkingGarageGroup(ParkingGarageGroupId, this);

        //    if (Configurator != null)
        //        Configurator(_ParkingGarageGroup);

        //    if (ParkingGarageGroupAddition.SendVoting(Timestamp.Now, this, _ParkingGarageGroup))
        //    {
        //        if (_ParkingGarageGroups.TryAdd(_ParkingGarageGroup))
        //        {

        //            _ParkingGarageGroup.OnParkingSpaceDataChanged                             += UpdateParkingSpaceData;
        //            _ParkingGarageGroup.OnParkingSpaceStatusChanged                           += UpdateParkingSpaceStatus;
        //            _ParkingGarageGroup.OnParkingSpaceAdminStatusChanged                      += UpdateParkingSpaceAdminStatus;

        //            _ParkingGarageGroup.OnParkingGarageDataChanged                  += UpdateParkingGarageData;
        //            _ParkingGarageGroup.OnParkingGarageStatusChanged                += UpdateParkingGarageStatus;
        //            _ParkingGarageGroup.OnParkingGarageAdminStatusChanged           += UpdateParkingGarageAdminStatus;

        //            //_ParkingGarageGroup.OnDataChanged                                 += UpdateParkingGarageGroupData;
        //            //_ParkingGarageGroup.OnAdminStatusChanged                          += UpdateParkingGarageGroupAdminStatus;

        //            OnSuccess?.Invoke(_ParkingGarageGroup);
        //            ParkingGarageGroupAddition.SendNotification(Timestamp.Now, this, _ParkingGarageGroup);
        //            return _ParkingGarageGroup;

        //        }
        //    }

        //    return null;

        //}

        #endregion

        #region GetOrCreateParkingGarageGroup(...)

        //public ParkingGarageGroup GetOrCreateParkingGarageGroup(ParkingGarageGroup_Id                        ParkingGarageGroupId,
        //                                                            Action<ParkingGarageGroup>                   Configurator            = null,
        //                                                            Action<ParkingGarageGroup>                   OnSuccess               = null,
        //                                                            Action<ParkingOperator, ParkingGarageGroup_Id>  OnError                 = null)
        //{

        //    ParkingGarageGroup _ParkingGarageGroup = null;

        //    if (_ParkingGarageGroups.TryGet(ParkingGarageGroupId, out _ParkingGarageGroup))
        //        return _ParkingGarageGroup;

        //    return CreateNewParkingGarageGroup(ParkingGarageGroupId,
        //                                         Configurator,
        //                                         OnSuccess,
        //                                         OnError);

        //}

        #endregion

        #region TryGetParkingGarageGroup

        //public Boolean TryGetParkingGarageGroup(ParkingGarageGroup_Id   ParkingGarageGroupId,
        //                                          out ParkingGarageGroup  ParkingGarageGroup)

        //    => _ParkingGarageGroups.TryGet(ParkingGarageGroupId, out ParkingGarageGroup);

        #endregion

        #endregion

        #region ParkingSpaces

        #region ParkingSpaces

        //public IEnumerable<ParkingSpace> ParkingSpaces

        //    => _ParkingGarages.
        //           SelectMany(v => v.ParkingGarages).
        //           SelectMany(v => v.ParkingSpaces);

        #endregion

        #region ParkingSpaceIds

        //public IEnumerable<ParkingSpace_Id> ParkingSpaceIds

        //    => _ParkingGarages.
        //           SelectMany(v => v.ParkingGarages).
        //           SelectMany(v => v.ParkingSpaces).
        //           Select    (v => v.Id);

        #endregion

        #region AllParkingSpaceStatus(IncludeParkingSpace = null)

        //public IEnumerable<KeyValuePair<ParkingSpace_Id, ParkingSpaceStatusType>> AllParkingSpaceStatus(Func<ParkingSpace, Boolean>  IncludeParkingSpace = null)

        //    => _ParkingGarages.
        //           SelectMany(pool    => pool.ParkingGarages).
        //           SelectMany(station => station.ParkingSpaces).
        //           Where     (evse    => IncludeParkingSpace == null || IncludeParkingSpace(evse)).
        //           OrderBy   (evse    => evse.Id).
        //           Select    (evse    => new KeyValuePair<ParkingSpace_Id, ParkingSpaceStatusType>(evse.Id, evse.Status.Value));

        #endregion


        #region ContainsParkingSpace(ParkingSpace)

        ///// <summary>
        ///// Check if the given ParkingSpace is already present within the Charging Station Operator.
        ///// </summary>
        ///// <param name="ParkingSpace">An ParkingSpace.</param>
        //public Boolean ContainsParkingSpace(ParkingSpace ParkingSpace)

        //    => _ParkingGarages.Any(pool => pool.ContainsParkingSpace(ParkingSpace.Id));

        #endregion

        #region ContainsParkingSpace(ParkingSpaceId)

        ///// <summary>
        ///// Check if the given ParkingSpace identification is already present within the Charging Station Operator.
        ///// </summary>
        ///// <param name="ParkingSpaceId">The unique identification of an ParkingSpace.</param>
        //public Boolean ContainsParkingSpace(ParkingSpace_Id ParkingSpaceId)

        //    => _ParkingGarages.Any(pool => pool.ContainsParkingSpace(ParkingSpaceId));

        #endregion

        #region GetParkingSpacebyId(ParkingSpaceId)

        //public ParkingSpace GetParkingSpacebyId(ParkingSpace_Id ParkingSpaceId)

        //    => _ParkingGarages.
        //           SelectMany    (pool    => pool.   ParkingGarages).
        //           SelectMany    (station => station.ParkingSpaces).
        //           FirstOrDefault(evse    => evse.Id == ParkingSpaceId);

        #endregion

        #region TryGetParkingSpacebyId(ParkingSpaceId, out ParkingSpace)

        //public Boolean TryGetParkingSpacebyId(ParkingSpace_Id ParkingSpaceId, out ParkingSpace ParkingSpace)
        //{

        //    ParkingSpace = _ParkingGarages.
        //               SelectMany    (pool    => pool.   ParkingGarages).
        //               SelectMany    (station => station.ParkingSpaces).
        //               FirstOrDefault(evse    => evse.Id == ParkingSpaceId);

        //    return ParkingSpace != null;

        //}

        #endregion


        #region ValidParkingSpaceIds

        //private readonly ReactiveSet<ParkingSpace_Id> _ValidParkingSpaceIds;

        ///// <summary>
        ///// A list of valid ParkingSpace Ids. All others will be filtered.
        ///// </summary>
        //public ReactiveSet<ParkingSpace_Id> ValidParkingSpaceIds
        //{
        //    get
        //    {
        //        return _ValidParkingSpaceIds;
        //    }
        //}

        #endregion

        #region InvalidParkingSpaceIds

        /// <summary>
        /// A list of invalid ParkingSpace Ids.
        /// </summary>
        public ReactiveSet<ParkingSpace_Id> InvalidParkingSpaceIds { get; }

        #endregion

        #region LocalParkingSpaceIds

        /// <summary>
        /// A list of manual ParkingSpace Ids which will not be touched automagically.
        /// </summary>
        public ReactiveSet<ParkingSpace_Id> LocalParkingSpaceIds { get; }

        #endregion


        #region SetParkingSpaceStatus(ParkingSpaceId, NewStatus)

        //public void SetParkingSpaceStatus(ParkingSpace_Id         ParkingSpaceId,
        //                          ParkingSpaceStatusType  NewStatus)
        //{

        //    ParkingSpace _ParkingSpace = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetStatus(NewStatus);

        //}

        #endregion

        #region SetParkingSpaceStatus(ParkingSpaceId, NewTimestampedStatus)

        //public void SetParkingSpaceStatus(ParkingSpace_Id                      ParkingSpaceId,
        //                          Timestamped<ParkingSpaceStatusType>  NewTimestampedStatus)
        //{

        //    ParkingSpace _ParkingSpace = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetStatus(NewTimestampedStatus);

        //}

        #endregion

        #region SetParkingSpaceStatus(ParkingSpaceId, NewStatus, Timestamp)

        //public void SetParkingSpaceStatus(ParkingSpace_Id         ParkingSpaceId,
        //                          ParkingSpaceStatusType  NewStatus,
        //                          DateTime        Timestamp)
        //{

        //    ParkingSpace _ParkingSpace = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetStatus(NewStatus, Timestamp);

        //}

        #endregion

        #region SetParkingSpaceStatus(ParkingSpaceId, StatusList, ChangeMethod = ChangeMethods.Replace)

        //public void SetParkingSpaceStatus(ParkingSpace_Id                                   ParkingSpaceId,
        //                          IEnumerable<Timestamped<ParkingSpaceStatusType>>  StatusList,
        //                          ChangeMethods                             ChangeMethod  = ChangeMethods.Replace)
        //{

        //    if (InvalidParkingSpaceIds.Contains(ParkingSpaceId))
        //        return;

        //    ParkingSpace _ParkingSpace  = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetStatus(StatusList, ChangeMethod);

        //}

        #endregion

        #region CalcParkingSpaceStatusDiff(ParkingSpaceStatus, IncludeParkingSpace = null)

        //public ParkingSpaceStatusDiff CalcParkingSpaceStatusDiff(Dictionary<ParkingSpace_Id, ParkingSpaceStatusType>  ParkingSpaceStatus,
        //                                         Func<ParkingSpace, Boolean>                  IncludeParkingSpace  = null)
        //{

        //    if (ParkingSpaceStatus == null || ParkingSpaceStatus.Count == 0)
        //        return new ParkingSpaceStatusDiff(Timestamp.Now, Id, Name);

        //    #region Get data...

        //    var ParkingSpaceStatusDiff     = new ParkingSpaceStatusDiff(Timestamp.Now, Id, Name);

        //    // Only ValidParkingSpaceIds!
        //    // Do nothing with manual ParkingSpace Ids!
        //    var CurrentParkingSpaceStates  = AllParkingSpaceStatus(IncludeParkingSpace).
        //                                 //Where(KVP => ValidParkingSpaceIds. Contains(KVP.Key) &&
        //                                 //            !ManualParkingSpaceIds.Contains(KVP.Key)).
        //                                 ToDictionary(v => v.Key, v => v.Value);

        //    var OldParkingSpaceIds         = new List<ParkingSpace_Id>(CurrentParkingSpaceStates.Keys);

        //    #endregion

        //    try
        //    {

        //        #region Find new and changed ParkingSpace states

        //        // Only for ValidParkingSpaceIds!
        //        // Do nothing with manual ParkingSpace Ids!
        //        foreach (var NewParkingSpaceStatus in ParkingSpaceStatus)
        //                                          //Where(KVP => ValidParkingSpaceIds. Contains(KVP.Key) &&
        //                                          //            !ManualParkingSpaceIds.Contains(KVP.Key)))
        //        {

        //            // Add to NewParkingSpaceStates, if new ParkingSpace was found!
        //            if (!CurrentParkingSpaceStates.ContainsKey(NewParkingSpaceStatus.Key))
        //                ParkingSpaceStatusDiff.AddNewStatus(NewParkingSpaceStatus);

        //            else
        //            {

        //                // Add to CHANGED, if state of known ParkingSpace changed!
        //                if (CurrentParkingSpaceStates[NewParkingSpaceStatus.Key] != NewParkingSpaceStatus.Value)
        //                    ParkingSpaceStatusDiff.AddChangedStatus(NewParkingSpaceStatus);

        //                // Remove ParkingSpaceId, as it was processed...
        //                OldParkingSpaceIds.Remove(NewParkingSpaceStatus.Key);

        //            }

        //        }

        //        #endregion

        //        #region Delete what is left in OldParkingSpaceIds!

        //        ParkingSpaceStatusDiff.AddRemovedId(OldParkingSpaceIds);

        //        #endregion

        //        return ParkingSpaceStatusDiff;

        //    }

        //    catch (Exception e)
        //    {

        //        while (e.InnerException != null)
        //            e = e.InnerException;

        //        DebugX.Log("GetParkingSpaceStatusDiff led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

        //    }

        //    // empty!
        //    return new ParkingSpaceStatusDiff(Timestamp.Now, Id, Name);

        //}

        #endregion

        #region ApplyParkingSpaceStatusDiff(ParkingSpaceStatusDiff)

        //public ParkingSpaceStatusDiff ApplyParkingSpaceStatusDiff(ParkingSpaceStatusDiff ParkingSpaceStatusDiff)
        //{

        //    #region Initial checks

        //    if (ParkingSpaceStatusDiff == null)
        //        throw new ArgumentNullException(nameof(ParkingSpaceStatusDiff),  "The given ParkingSpace status diff must not be null!");

        //    #endregion

        //    foreach (var status in ParkingSpaceStatusDiff.NewStatus)
        //        SetParkingSpaceStatus(status.Key, status.Value);

        //    foreach (var status in ParkingSpaceStatusDiff.ChangedStatus)
        //        SetParkingSpaceStatus(status.Key, status.Value);

        //    return ParkingSpaceStatusDiff;

        //}

        #endregion


        #region SetParkingSpaceAdminStatus(ParkingSpaceId, NewAdminStatus)

        //public void SetParkingSpaceAdminStatus(ParkingSpace_Id              ParkingSpaceId,
        //                               ParkingSpaceAdminStatusType  NewAdminStatus)
        //{

        //    ParkingSpace _ParkingSpace = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetAdminStatus(NewAdminStatus);

        //}

        #endregion

        #region SetParkingSpaceAdminStatus(ParkingSpaceId, NewTimestampedAdminStatus)

        //public void SetParkingSpaceAdminStatus(ParkingSpace_Id                           ParkingSpaceId,
        //                               Timestamped<ParkingSpaceAdminStatusType>  NewTimestampedAdminStatus)
        //{

        //    ParkingSpace _ParkingSpace = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetAdminStatus(NewTimestampedAdminStatus);

        //}

        #endregion

        #region SetParkingSpaceAdminStatus(ParkingSpaceId, NewAdminStatus, Timestamp)

        //public void SetParkingSpaceAdminStatus(ParkingSpace_Id              ParkingSpaceId,
        //                               ParkingSpaceAdminStatusType  NewAdminStatus,
        //                               DateTime             Timestamp)
        //{

        //    ParkingSpace _ParkingSpace = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetAdminStatus(NewAdminStatus, Timestamp);

        //}

        #endregion

        #region SetParkingSpaceAdminStatus(ParkingSpaceId, AdminStatusList, ChangeMethod = ChangeMethods.Replace)

        //public void SetParkingSpaceAdminStatus(ParkingSpace_Id                                        ParkingSpaceId,
        //                               IEnumerable<Timestamped<ParkingSpaceAdminStatusType>>  AdminStatusList,
        //                               ChangeMethods                                  ChangeMethod  = ChangeMethods.Replace)
        //{

        //    if (InvalidParkingSpaceIds.Contains(ParkingSpaceId))
        //        return;

        //    ParkingSpace _ParkingSpace  = null;
        //    if (TryGetParkingSpacebyId(ParkingSpaceId, out _ParkingSpace))
        //        _ParkingSpace.SetAdminStatus(AdminStatusList, ChangeMethod);

        //}

        #endregion

        #region ApplyParkingSpaceAdminStatusDiff(ParkingSpaceAdminStatusDiff)

        //public ParkingSpaceAdminStatusDiff ApplyParkingSpaceAdminStatusDiff(ParkingSpaceAdminStatusDiff ParkingSpaceAdminStatusDiff)
        //{

        //    #region Initial checks

        //    if (ParkingSpaceAdminStatusDiff == null)
        //        throw new ArgumentNullException(nameof(ParkingSpaceAdminStatusDiff),  "The given ParkingSpace admin status diff must not be null!");

        //    #endregion

        //    foreach (var status in ParkingSpaceAdminStatusDiff.NewStatus)
        //        SetParkingSpaceAdminStatus(status.Key, status.Value);

        //    foreach (var status in ParkingSpaceAdminStatusDiff.ChangedStatus)
        //        SetParkingSpaceAdminStatus(status.Key, status.Value);

        //    return ParkingSpaceAdminStatusDiff;

        //}

        #endregion


        #region OnParkingSpaceData/(Admin)StatusChanged

        ///// <summary>
        ///// An event fired whenever the static data of any subordinated ParkingSpace changed.
        ///// </summary>
        //public event OnParkingSpaceDataChangedDelegate         OnParkingSpaceDataChanged;

        ///// <summary>
        ///// An event fired whenever the dynamic status of any subordinated ParkingSpace changed.
        ///// </summary>
        //public event OnParkingSpaceStatusChangedDelegate       OnParkingSpaceStatusChanged;

        ///// <summary>
        ///// An event fired whenever the admin status of any subordinated ParkingSpace changed.
        ///// </summary>
        //public event OnParkingSpaceAdminStatusChangedDelegate  OnParkingSpaceAdminStatusChanged;

        #endregion

        #region ParkingSensorAddition

        internal readonly IVotingNotificator<DateTime, ParkingSpace, ParkingSensor, Boolean> ParkingSensorAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ParkingSpace, ParkingSensor, Boolean> OnParkingSensorAddition

            => ParkingSensorAddition;

        #endregion

        #region ParkingSensorRemoval

        internal readonly IVotingNotificator<DateTime, ParkingSpace, ParkingSensor, Boolean> ParkingSensorRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ParkingSpace, ParkingSensor, Boolean> OnParkingSensorRemoval
        {
            get
            {
                return ParkingSensorRemoval;
            }
        }

        #endregion


        #region (internal) UpdateParkingSpaceData(Timestamp, ParkingSpace, OldStatus, NewStatus)

        ///// <summary>
        ///// Update the data of an ParkingSpace.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="ParkingSpace">The changed ParkingSpace.</param>
        ///// <param name="PropertyName">The name of the changed property.</param>
        ///// <param name="OldValue">The old value of the changed property.</param>
        ///// <param name="NewValue">The new value of the changed property.</param>
        //internal async Task UpdateParkingSpaceData(DateTime  Timestamp,
        //                                   ParkingSpace      ParkingSpace,
        //                                   String    PropertyName,
        //                                   Object    OldValue,
        //                                   Object    NewValue)
        //{

        //    var OnParkingSpaceDataChangedLocal = OnParkingSpaceDataChanged;
        //    if (OnParkingSpaceDataChangedLocal != null)
        //        await OnParkingSpaceDataChangedLocal(Timestamp, ParkingSpace, PropertyName, OldValue, NewValue);

        //}

        #endregion

        #region (internal) UpdateParkingSpaceAdminStatus(Timestamp, EventTrackingId, ParkingSpace, OldStatus, NewStatus)

        ///// <summary>
        ///// Update an ParkingSpace admin status.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        ///// <param name="ParkingSpace">The updated ParkingSpace.</param>
        ///// <param name="OldStatus">The old ParkingSpace status.</param>
        ///// <param name="NewStatus">The new ParkingSpace status.</param>
        //internal async Task UpdateParkingSpaceAdminStatus(DateTime                          Timestamp,
        //                                          EventTracking_Id                  EventTrackingId,
        //                                          ParkingSpace                              ParkingSpace,
        //                                          Timestamped<ParkingSpaceAdminStatusType>  OldStatus,
        //                                          Timestamped<ParkingSpaceAdminStatusType>  NewStatus)
        //{

        //    var OnParkingSpaceAdminStatusChangedLocal = OnParkingSpaceAdminStatusChanged;
        //    if (OnParkingSpaceAdminStatusChangedLocal != null)
        //        await OnParkingSpaceAdminStatusChangedLocal(Timestamp,
        //                                            EventTrackingId,
        //                                            ParkingSpace,
        //                                            OldStatus,
        //                                            NewStatus);

        //}

        #endregion

        #region (internal) UpdateParkingSpaceStatus     (Timestamp, EventTrackingId, ParkingSpace, OldStatus, NewStatus)

        ///// <summary>
        ///// Update an ParkingSpace status.
        ///// </summary>
        ///// <param name="Timestamp">The timestamp when this change was detected.</param>
        ///// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        ///// <param name="ParkingSpace">The updated ParkingSpace.</param>
        ///// <param name="OldStatus">The old ParkingSpace status.</param>
        ///// <param name="NewStatus">The new ParkingSpace status.</param>
        //internal async Task UpdateParkingSpaceStatus(DateTime                     Timestamp,
        //                                     EventTracking_Id             EventTrackingId,
        //                                     ParkingSpace                         ParkingSpace,
        //                                     Timestamped<ParkingSpaceStatusType>  OldStatus,
        //                                     Timestamped<ParkingSpaceStatusType>  NewStatus)
        //{

        //    var OnParkingSpaceStatusChangedLocal = OnParkingSpaceStatusChanged;
        //    if (OnParkingSpaceStatusChangedLocal != null)
        //        await OnParkingSpaceStatusChangedLocal(Timestamp,
        //                                       EventTrackingId,
        //                                       ParkingSpace,
        //                                       OldStatus,
        //                                       NewStatus);

        //}

        #endregion

        #endregion


        #region IComparable<ParkingOperator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ParkingSpace_Operator.
            var ParkingSpace_Operator = Object as ParkingOperator;
            if ((Object) ParkingSpace_Operator == null)
                throw new ArgumentException("The given object is not an ParkingSpace_Operator!");

            return CompareTo(ParkingSpace_Operator);

        }

        #endregion

        #region CompareTo(Operator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Operator">An Charging Station Operator object to compare with.</param>
        public Int32 CompareTo(ParkingOperator Operator)
        {

            if ((Object) Operator == null)
                throw new ArgumentNullException("The given Charging Station Operator must not be null!");

            return Id.CompareTo(Operator.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ParkingOperator> Members

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

            // Check if the given object is an ParkingOperator.
            var ParkingSpace_Operator = Object as ParkingOperator;
            if ((Object) ParkingSpace_Operator == null)
                return false;

            return this.Equals(ParkingSpace_Operator);

        }

        #endregion

        #region Equals(Operator)

        /// <summary>
        /// Compares two Charging Station Operators for equality.
        /// </summary>
        /// <param name="Operator">An Charging Station Operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingOperator Operator)
        {

            if ((Object) Operator == null)
                return false;

            return Id.Equals(Operator.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

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
