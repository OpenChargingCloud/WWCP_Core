/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this GridOperator, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        public static JObject ToJSON(this GridOperator  GridOperator,
                                     Boolean                       Embedded                        = false,
                                     Boolean                       ExpandChargingRoamingNetworkId  = false,
                                     Boolean                       ExpandChargingPoolIds           = false,
                                     Boolean                       ExpandChargingStationIds        = false,
                                     Boolean                       ExpandEVSEIds                   = false)

            => GridOperator != null
                   ? JSONObject.Create(

                         new JProperty("id",                        GridOperator.Id.ToString()),

                         Embedded
                             ? null
                             : ExpandChargingRoamingNetworkId
                                   ? new JProperty("roamingNetwork",      GridOperator.RoamingNetwork.ToJSON())
                                   : new JProperty("roamingNetworkId",    GridOperator.RoamingNetwork.Id.ToString()),

                         new JProperty("name",                  GridOperator.Name.       ToJSON()),
                         new JProperty("description",           GridOperator.Description.ToJSON()),

                         // Address
                         // LogoURI
                         // API - RobotKeys, Endpoints, DNS SRV
                         // MainKeys

                         GridOperator.Logo.IsNotNullOrEmpty()
                             ? new JProperty("logos",               JSONArray.Create(
                                                                        JSONObject.Create(
                                                                            new JProperty("uri",          GridOperator.Logo),
                                                                            new JProperty("description",  I18NString.Empty.ToJSON())
                                                                        )
                                                                    ))
                             : null,

                         GridOperator.Homepage.IsNotNullOrEmpty()
                             ? new JProperty("homepage",            GridOperator.Homepage)
                             : null,

                         GridOperator.HotlinePhoneNumber.IsNotNullOrEmpty()
                             ? new JProperty("hotline",             GridOperator.HotlinePhoneNumber)
                             : null,

                         GridOperator.DataLicenses.Any()
                             ? new JProperty("dataLicenses",        new JArray(GridOperator.DataLicenses.Select(license => license.ToJSON())))
                             : null

                         //new JProperty("chargingPools",         ExpandChargingPoolIds
                         //                                           ? new JArray(GridOperator.ChargingPools.     ToJSON(Embedded: true))
                         //                                           : new JArray(GridOperator.ChargingPoolIds.   Select(id => id.ToString()))),

                         //new JProperty("chargingStations",      ExpandChargingStationIds
                         //                                           ? new JArray(GridOperator.ChargingStations.  ToJSON(Embedded: true))
                         //                                           : new JArray(GridOperator.ChargingStationIds.Select(id => id.ToString()))),

                         //new JProperty("evses",                 ExpandEVSEIds
                         //                                           ? new JArray(GridOperator.EVSEs.             ToJSON(Embedded: true))
                         //                                           : new JArray(GridOperator.EVSEIds.           Select(id => id.ToString())))

                     )
                   : null;

        #endregion

        #region ToJSON(this GridOperator, JPropertyKey)

        public static JProperty ToJSON(this GridOperator GridOperator, String JPropertyKey)

            => GridOperator != null
                   ? new JProperty(JPropertyKey, GridOperator.ToJSON())
                   : null;

        #endregion

        #region ToJSON(this GridOperators, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false, ExpandChargingStationIds = false, ExpandChargingStationIds = false, ExpandEVSEIds = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="GridOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<GridOperator>  GridOperators,
                                    UInt64?                         Skip                            = null,
                                    UInt64?                         Take                            = null,
                                    Boolean                         Embedded                        = false,
                                    Boolean                         ExpandChargingRoamingNetworkId  = false,
                                    Boolean                         ExpandChargingPoolIds           = false,
                                    Boolean                         ExpandChargingStationIds        = false,
                                    Boolean                         ExpandEVSEIds                   = false)
        {

            #region Initial checks

            if (GridOperators == null)
                return new JArray();

            #endregion

            return new JArray(GridOperators.
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

        #region ToJSON(this GridOperators, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<GridOperator> GridOperators, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return GridOperators != null
                       ? new JProperty(JPropertyKey, GridOperators.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<GridOperatorAdminStatusType>>  GridOperatorAdminStatus,
                                     UInt64?                                                     Skip         = null,
                                     UInt64?                                                     Take         = null,
                                     UInt64?                                                     HistorySize  = 1)

        {

            if (GridOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorAdminStatus.
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
                // e.g. when a Stack behind GridOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>>  GridOperatorAdminStatus,
                                     UInt64?                                                                                                 Skip         = null,
                                     UInt64?                                                                                                 Take         = null,
                                     UInt64?                                                                                                 HistorySize  = 1)

        {

            if (GridOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorAdminStatus.
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
                // e.g. when a Stack behind GridOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<GridOperatorStatusType>>  GridOperatorStatus,
                                     UInt64?                                                Skip         = null,
                                     UInt64?                                                Take         = null,
                                     UInt64?                                                HistorySize  = 1)

        {

            if (GridOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorStatus.
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
                // e.g. when a Stack behind GridOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>>  GridOperatorStatus,
                                     UInt64?                                                                                            Skip         = null,
                                     UInt64?                                                                                            Take         = null,
                                     UInt64?                                                                                            HistorySize  = 1)

        {

            if (GridOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorStatus.
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
                // e.g. when a Stack behind GridOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }


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
    public class GridOperator : ACryptoEMobilityEntity<GridOperator_Id,
                                                       GridOperatorAdminStatusType,
                                                       GridOperatorStatusType>,
                                IRemoteGridOperator,
                                IEquatable <GridOperator>,
                                IComparable<GridOperator>,
                                IComparable
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

        //public Authorizator_Id AuthorizatorId { get; }

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


        public GridOperatorPriority Priority { get; set; }

        #endregion

        #region Links

        /// <summary>
        /// The remote e-mobility provider.
        /// </summary>
        public IRemoteGridOperator  RemoteGridOperator    { get; }

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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility (service) provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="Id">The unique e-mobility provider identification.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal GridOperator(GridOperator_Id                     Id,
                              RoamingNetwork                      RoamingNetwork,
                              Action<GridOperator>?               Configurator                 = null,
                              RemoteGridOperatorCreatorDelegate?  RemoteGridOperatorCreator    = null,
                              I18NString?                         Name                         = null,
                              I18NString?                         Description                  = null,
                              GridOperatorPriority?               Priority                     = null,
                              GridOperatorAdminStatusType?        InitialAdminStatus           = null,
                              GridOperatorStatusType?             InitialStatus                = null,
                              UInt16?                             MaxAdminStatusScheduleSize   = DefaultMaxAdminStatusScheduleSize,
                              UInt16?                             MaxStatusScheduleSize        = DefaultMaxStatusScheduleSize,

                              String?                             DataSource                   = null,
                              DateTime?                           LastChange                   = null,

                              JObject?                            CustomData                   = null,
                              UserDefinedDictionary?              InternalData                 = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   null,
                   null,
                   null,
                   InitialAdminStatus         ?? GridOperatorAdminStatusType.Available,
                   InitialStatus              ?? GridOperatorStatusType.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this._DataLicenses                = new List<OpenDataLicense>();

            this.Priority                     = Priority    ?? new GridOperatorPriority(0);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteGridOperator = RemoteGridOperatorCreator?.Invoke(this);

        }

        #endregion




        #region IComparable<EVSE_Operator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as GridOperator;
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
        public Int32 CompareTo(GridOperator EVSE_Operator)
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
            var EVSE_Operator = Object as GridOperator;
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
        public Boolean Equals(GridOperator EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                return false;

            return Id.Equals(EVSE_Operator.Id);

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
