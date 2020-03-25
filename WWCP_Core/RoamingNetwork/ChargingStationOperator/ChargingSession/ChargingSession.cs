/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extention methods for the charging sessions.
    /// </summary>
    public static class ChargingSessionExtentions
    {

        #region ToJSON(this ChargingSession, JPropertyKey)

        public static JProperty ToJSON(this ChargingSession ChargingSession, String JPropertyKey)
        {

            #region Initial checks

            if (ChargingSession == null)
                throw new ArgumentNullException(nameof(ChargingSession),  "The given charging session must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),     "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargingSession.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingSessions)

        public static JArray ToJSON(this IEnumerable<ChargingSession>  ChargingSessions,
                                    UInt64?                            Skip       = null,
                                    UInt64?                            Take       = null,
                                    Boolean                            Embedded   = false)

            => ChargingSessions == null || !ChargingSessions.Any()

                   ? new JArray()

                   : new JArray(ChargingSessions.
                                    Where         (session => session != null).
                                    OrderBy       (session => session.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (session => session.ToJSON(Embedded)));

        #endregion

        #region ToJSON(this ChargingSessions, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingSession> ChargingSessions, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingSessions != null
                       ? new JProperty(JPropertyKey, ChargingSessions.ToJSON())
                       : new JProperty(JPropertyKey, new JArray());

        }

        #endregion

    }

    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging session
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingSession : AEMobilityEntity<ChargingSession_Id>,
                                   IEquatable<ChargingSession>,
                                   IComparable<ChargingSession>,
                                   IComparable
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/chargingSession";

        #endregion

        #region Properties

        public EventTracking_Id EventTrackingId { get; set; }

        public System_Id?  SystemIdStart    { get; set; }
        public System_Id?  SystemIdStop     { get; set; }

        public System_Id?  SystemIdCDR      { get; set; }


        #region RoamingNetwork

        /// <summary>
        /// The unqiue identification of the roaming network serving this session.
        /// </summary>
        public RoamingNetwork_Id? RoamingNetworkId { get; set; }


        private IRoamingNetwork _RoamingNetwork;

        /// <summary>
        /// The roaming network serving this session.
        /// </summary>
        public IRoamingNetwork RoamingNetwork
        {

            get
            {
                return _RoamingNetwork;
            }

            set
            {

                _RoamingNetwork = value;

                if (value != null)
                    RoamingNetworkId = value.Id;

            }

        }

        #endregion

        #region ChargingStationOperator

        /// <summary>
        /// The unqiue identification of the charging station operator serving this session.
        /// </summary>
        public ChargingStationOperator_Id? ChargingStationOperatorId { get; set; }


        private ChargingStationOperator _ChargingStationOperator;

        /// <summary>
        /// The charging station operator serving this session.
        /// </summary>
        public ChargingStationOperator ChargingStationOperator

        {

            get
            {
                return _ChargingStationOperator;
            }

            set
            {

                _ChargingStationOperator = value;

                if (value != null)
                {
                    ChargingStationOperatorId  = value.Id;
                    RoamingNetwork             = value.RoamingNetwork;
                }

            }

        }

        #endregion

        #region ChargingPool

        /// <summary>
        /// The unqiue identification of the charging pool serving this session.
        /// </summary>
        public ChargingPool_Id? ChargingPoolId { get; set; }


        private ChargingPool _ChargingPool;

        /// <summary>
        /// The charging pool serving this session.
        /// </summary>
        public ChargingPool ChargingPool
        {

            get
            {
                return _ChargingPool;
            }

            set
            {

                _ChargingPool = value;

                if (value != null)
                {
                    ChargingPoolId           = value.Id;
                    ChargingStationOperator  = value.Operator;
                }

            }

        }

        #endregion

        #region ChargingStation

        /// <summary>
        /// The unqiue identification of the charging station serving this session.
        /// </summary>
        public ChargingStation_Id? ChargingStationId { get; set; }


        private ChargingStation _ChargingStation;

        /// <summary>
        /// The charging station serving this session.
        /// </summary>
        public ChargingStation ChargingStation
        {

            get
            {
                return _ChargingStation;
            }

            set
            {

                _ChargingStation = value;

                if (value != null)
                {
                    ChargingStationId  = value.Id;
                    ChargingPool       = value.ChargingPool;
                }

            }

        }

        #endregion

        #region EVSE

        /// <summary>
        /// The unqiue identification of the EVSE serving this session.
        /// </summary>
        public EVSE_Id? EVSEId { get; set; }


        private EVSE _EVSE;

        /// <summary>
        /// The EVSE serving this session.
        /// </summary>
        [Optional]
        public EVSE EVSE
        {

            get
            {
                return _EVSE;
            }

            set
            {

                _EVSE  = value;

                if (value != null)
                {
                    EVSEId           = value.Id;
                    ChargingStation  = value.ChargingStation;
                }

            }

        }

        #endregion

        #region ChargingProduct

        /// <summary>
        /// The charging product selected for this charging session.
        /// </summary>
        [Optional]
        public ChargingProduct  ChargingProduct   { get; set; }

        #endregion

        #region EnergyMeterId

        /// <summary>
        /// An optional unique identification of the energy meter.
        /// </summary>
        [Optional]
        public EnergyMeter_Id? EnergyMeterId { get; set; }

        #endregion

        #region EnergyMeterValues

        private readonly List<Timestamped<Decimal>> _EnergyMeterValues;

        /// <summary>
        /// An optional enumeration of intermediate energy meter values.
        /// This values indicate the consumed energy between the current
        /// and the last timestamp in watt-hours [Wh].
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<Decimal>> EnergyMeteringValues
            => _EnergyMeterValues;

        #endregion

        #region SignedMeteringValues

        private readonly List<SignedMeteringValue<Decimal>> _SignedMeteringValues;

        /// <summary>
        /// An optional enumeration of signed intermediate energy meter values.
        /// This values indicate the consumed energy between the current
        /// and the last timestamp in watt-hours [Wh].
        /// </summary>
        [Optional]
        public IEnumerable<SignedMeteringValue<Decimal>> SignedMeteringValues
            => _SignedMeteringValues;

        #endregion

        #region ConsumedEnergy

        /// <summary>
        /// The current amount of energy consumed while charging in [kWh].
        /// </summary>
        [Mandatory]
        public Decimal ConsumedEnergy
        {
            get
            {

                return EnergyMeteringValues.
                           Select(metervalue => metervalue.Value).
                           Sum() / 1000;

            }
        }

        #endregion

        #region ParkingTime

        /// <summary>
        /// Optional timestamps when the parking started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime ParkingTime { get; set; }

        #endregion

        #region SessionTime

        /// <summary>
        /// Optional timestamps when the charging session started and ended.
        /// </summary>
        [Mandatory]
        public StartEndDateTime SessionTime { get; set; }

        #endregion

        #region Duration

        public TimeSpan Duration
            => (SessionTime.EndTime ?? DateTime.UtcNow) - SessionTime.StartTime;

        #endregion



        #region Reservation

        private ChargingReservation _Reservation;

        /// <summary>
        /// An optional charging reservation for this charging session.
        /// </summary>
        [Optional]
        public ChargingReservation Reservation
        {

            get
            {
                return _Reservation;
            }

            set
            {

                _Reservation = value;

                if (value != null)
                    _ReservationId = value.Id;

            }

        }

        #endregion

        #region ReservationId

        private ChargingReservation_Id? _ReservationId;

        /// <summary>
        /// An optional charging reservation for this charging session.
        /// </summary>
        [Optional]
        public ChargingReservation_Id? ReservationId
        {

            get
            {
                return _ReservationId;
            }

            set
            {

                _ReservationId = value;

                if (_Reservation != null && _Reservation.Id != value)
                    _Reservation = null;

            }

        }

        #endregion


        #region ProviderStart

        /// <summary>
        /// The identification of the e-mobility provider used for starting this charging process.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id?            ProviderIdStart        { get; set; }

        private eMobilityProvider _ProviderStart;

        public eMobilityProvider ProviderStart
        {

            get
            {
                return _ProviderStart;
            }

            set
            {

                _ProviderStart = value;

                if (value != null)
                    ProviderIdStart = value.Id;

            }

        }

        #endregion

        #region ProviderStop

        /// <summary>
        /// The identification of the e-mobility provider used for stopping this charging process.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id?            ProviderIdStop         { get; set; }

        private eMobilityProvider _ProviderStop;

        public eMobilityProvider ProviderStop
        {

            get
            {
                return _ProviderStop;
            }

            set
            {

                _ProviderStop = value;

                if (value != null)
                    ProviderIdStop = value.Id;

            }

        }

        #endregion

        #region AuthenticationStart/-Stop

        /// <summary>
        /// The authentication used for starting this charging process.
        /// </summary>
        [Optional]
        public AAuthentication                  AuthenticationStart    { get; set; }

        /// <summary>
        /// The authentication used for stopping this charging process.
        /// </summary>
        [Optional]
        public AAuthentication                  AuthenticationStop     { get; set; }

        #endregion


        #region EMPRoamingProviderStart

        public EMPRoamingProvider_Id? EMPRoamingProviderIdStart { get; set; }


        private IEMPRoamingProvider _EMPRoamingProviderStart;

        public IEMPRoamingProvider EMPRoamingProviderStart
        {

            get
            {
                return _EMPRoamingProviderStart;
            }

            set
            {

                _EMPRoamingProviderStart = value;

                if (value != null)
                    EMPRoamingProviderIdStart = value.Id;

            }

        }

        #endregion

        #region EMPRoamingProviderStop

        public EMPRoamingProvider_Id? EMPRoamingProviderIdStop { get; set; }


        private IEMPRoamingProvider _EMPRoamingProviderStop;

        public IEMPRoamingProvider EMPRoamingProviderStop
        {

            get
            {
                return _EMPRoamingProviderStop;
            }

            set
            {

                _EMPRoamingProviderStop = value;

                if (value != null)
                    EMPRoamingProviderIdStop = value.Id;

            }

        }

        #endregion


        #region CSORoamingProviderStart

        public CSORoamingProvider_Id?     CSORoamingProviderIdStart          { get; set; }


        private ICSORoamingProvider _CSORoamingProviderStart;

        public ICSORoamingProvider        CSORoamingProviderStart
        {

            get
            {
                return _CSORoamingProviderStart;
            }

            set
            {

                _CSORoamingProviderStart = value;

                if (value != null)
                    CSORoamingProviderIdStart = value.Id;

            }

        }

        #endregion

        #region CSORoamingProviderStop

        public CSORoamingProvider_Id?     CSORoamingProviderIdStop          { get; set; }


        private ICSORoamingProvider _CSORoamingProviderStop;

        public ICSORoamingProvider        CSORoamingProviderStop
        {

            get
            {
                return _CSORoamingProviderStop;
            }

            set
            {

                _CSORoamingProviderStop = value;

                if (value != null)
                    CSORoamingProviderIdStop = value.Id;

            }

        }

        #endregion



        public ChargeDetailRecord         CDR                           { get; set; }

        public DateTime?                  CDRReceived                  { get; set; }

        public DateTime?                  CDRForwarded                  { get; set; }

        public SendCDRResult              CDRResult                     { get; set; }


        public Boolean                    RemoveMe                      { get; set; }


        private readonly HashSet<String> _Signatures;

        public IEnumerable<String> Signatures
                   => _Signatures;

        #endregion

        #region Events

        /// <summary>
        /// An event send whenever a new energy meter value was received.
        /// </summary>
        /// <param name="Timestamp">The current timestamp.</param>
        /// <param name="ChargingSession">The unique charging session identification.</param>
        /// <param name="EnergyMeterValue">A timestamped energy meter value.</param>
        public delegate void OnNewEnergyMeterValueDelegate(DateTime Timestamp, ChargingSession ChargingSession, Timestamped<Double> EnergyMeterValue);

        /// <summary>
        /// An event send whenever a new energy meter value was received.
        /// </summary>
        public event OnNewEnergyMeterValueDelegate OnNewEnergyMeterValue;


        /// <summary>
        /// An event send whenever a new energy meter value was received.
        /// </summary>
        /// <param name="Timestamp">The current timestamp.</param>
        /// <param name="ChargingSession">The unique charging session identification.</param>
        /// <param name="EnergyMeterValue">A timestamped energy meter value.</param>
        public delegate void OnNewSignedEnergyMeterValueDelegate(DateTime Timestamp, ChargingSession ChargingSession, SignedMeteringValue<Decimal> EnergyMeterValue);

        /// <summary>
        /// An event send whenever a new energy meter value was received.
        /// </summary>
        public event OnNewSignedEnergyMeterValueDelegate OnNewSignedEnergyMeterValue;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="Timestamp">The timestamp of the session creation.</param>
        public ChargingSession(ChargingSession_Id  Id,
                               DateTime?           Timestamp = null)

            : base(Id)

        {

            this.SessionTime         = new StartEndDateTime(Timestamp ?? DateTime.UtcNow);
            this._EnergyMeterValues  = new List<Timestamped<Decimal>>();

        }

        #endregion


        public void AddEnergyMeterValue(Timestamped<Decimal> Value)
        {
            _EnergyMeterValues.Add(Value);
        }


        //public ChargingSession AddUserData(String Key, Object Value)
        //{
        //    this._UserDefined
        //}



        public JObject ToJSON(Boolean Embedded = false)

            => JSONObject.Create(

                   Id.ToJSON("@id"),

                   Embedded
                       ? null
                       : new JProperty("@context",                    JSONLDContext),

                   RoamingNetworkId.HasValue
                       ? new JProperty("roamingNetworkId",            RoamingNetworkId.ToString())
                       : null,


                   Reservation != null
                       ? new JProperty("reservation", new JObject(
                                                          new JProperty("reservationId",  Reservation.Id.ToString()),
                                                          new JProperty("start",          Reservation.StartTime.ToIso8601()),
                                                          new JProperty("duration",       Reservation.Duration.TotalSeconds)
                                                          )
                                                      )
                       : ReservationId != null
                             ? new JProperty("reservationId",         ReservationId.ToString())
                             : null,


                   SessionTime != null
                       ? new JProperty("start", JSONObject.Create(

                             new JProperty("timestamp",               SessionTime.StartTime.ToIso8601()),

                             SystemIdStart.HasValue
                                 ? new JProperty("systemId",              SystemIdStart.            ToString())
                                 : null,

                             EMPRoamingProviderIdStart.HasValue
                                 ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderIdStart.ToString())
                                 : null,

                             CSORoamingProviderIdStart.HasValue
                                 ? new JProperty("CSORoamingProviderId",  CSORoamingProviderIdStart.ToString())
                                 : null,

                             ProviderIdStart != null
                                 ? new JProperty("providerId",            ProviderIdStart.          ToString())
                                 : null,

                             AuthenticationStart.IsDefined()
                                 ? new JProperty("authentication",        AuthenticationStart.      ToJSON())
                                 : null

                         ))
                       : null,


                   SessionTime != null
                       ? new JProperty("duration",                    Duration.TotalSeconds)
                       : null,


                   SessionTime.EndTime.HasValue
                       ? new JProperty("stop", JSONObject.Create(

                             SessionTime.EndTime.HasValue
                                 ? new JProperty("timestamp",             SessionTime.EndTime.Value.ToIso8601())
                                 : null,

                             SystemIdStop.HasValue
                                 ? new JProperty("systemId",              SystemIdStop.             ToString())
                                 : null,

                             EMPRoamingProviderIdStop.HasValue
                                 ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderIdStop. ToString())
                                 : null,

                             CSORoamingProviderIdStop.HasValue
                                 ? new JProperty("CSORoamingProviderId",  CSORoamingProviderIdStop. ToString())
                                 : null,

                             ProviderIdStop != null
                                 ? new JProperty("providerId",            ProviderIdStop.           ToString())
                                 : null,

                             AuthenticationStop.IsDefined()
                                 ? new JProperty("authentication",        AuthenticationStop.       ToJSON())
                                 : null

                         ))
                       : null,


                   CDRReceived.HasValue
                       ? new JProperty("cdr", JSONObject.Create(

                             CDRReceived.HasValue
                                 ? new JProperty("timestamp",             CDRReceived.Value. ToIso8601())
                                 : null,

                             SystemIdCDR.HasValue
                                 ? new JProperty("systemId",              SystemIdCDR.       ToString())
                                 : null,

                             CDR != null
                                 ? new JProperty("cdr",                   CDR.               ToJSON(Embedded: true))
                                 : null,


                             CDRForwarded.HasValue
                                 ? new JProperty("forwarded",             CDRForwarded.Value.ToIso8601())
                                 : null,

                             CDRResult != null
                                 ? new JProperty("result",                CDRResult.         ToJSON())
                                 : null

                         ))
                       : null,


                   ChargingStationOperatorId.HasValue
                       ? new JProperty("chargingStationOperatorId",   ChargingStationOperatorId.ToString())
                       : null,

                   ChargingPoolId.HasValue
                       ? new JProperty("chargingPoolId",              ChargingPoolId.           ToString())
                       : null,

                   ChargingStationId.HasValue
                       ? new JProperty("chargingStationId",           ChargingStationId.        ToString())
                       : null,

                   EVSEId.HasValue
                       ? new JProperty("EVSEId",                      EVSEId.                   ToString())
                       : null,

                   ChargingProduct != null
                       ? new JProperty("chargingProduct",             ChargingProduct.          ToJSON())
                       : null,

                   EnergyMeterId.HasValue
                       ? new JProperty("energyMeterId",               EnergyMeterId.            ToString())
                       : null,

                   EnergyMeteringValues.Any()
                       ? new JProperty("energyMeterValues",           JSONArray.Create(
                                                                          EnergyMeteringValues.
                                                                          Select(meterValue => JSONObject.Create(
                                                                                                   new JProperty("timestamp", meterValue.Timestamp.ToIso8601()),
                                                                                                   new JProperty("value",     meterValue.Value)
                                                                                               ))
                                                                      ))
                       : null

            );


        public static ChargingSession Parse(JObject JSON)
        {

            // {
            //     "@id":                           "7f0d7978-ab29-462c-908f-b31aa9c9326e",
            //     "@context":                      "https://open.charging.cloud/contexts/wwcp+json/chargingSession",
            //     "roamingNetworkId":              "Prod",
            //     "start": {
            //         "timestamp":                 "2020-03-24T07:44:39.476Z",
            //         "systemId":                  "de-bd-gw-02",
            //         "CSORoamingProviderId":      "HubjectProd",
            //         "providerId":                "DE-MEG",
            //         "authentication": {
            //           "remoteIdentification":    "DE-MEG-C00171852-1"
            //         }
            //     },
            //     "duration":                      13309.27226,
            //     "stop": {
            //         "timestamp":                 "2020-03-24T11:26:28.748Z",
            //         "systemId":                  "de-bd-gw-02"
            //     },
            //     "cdr": {
            //         "timestamp":                 "2020-03-24T11:26:28.748Z",
            //         "systemId":                  "de-bd-gw-02",
            //         "cdr": {
            //             "@id":                   "7f0d7978-ab29-462c-908f-b31aa9c9326e",
            //             "sessionTime": {
            //                 "start":             "2020-03-24T07:44:29.575Z",
            //                 "end":               "2020-03-24T11:26:21.096Z"
            //             },
            //             "meterValues": [
            //                {
            //                    "timestamp":      "2020-03-24T07:44:29.575Z",
            //                    "value":            0.0
            //                },
            //                {
            //                    "timestamp":      "2020-03-24T11:26:21.096Z",
            //                    "value":           33.1
            //                }
            //             ],
            //             "evseId":                "DE*LVF*E970993236*1"
            //         }
            //     },
            //     "chargingStationOperatorId":     "DE*LVF",
            //     "chargingPoolId":                "DE*LVF*P555F437E4ECB6F6",
            //     "chargingStationId":             "DE*LVF*S970993236",
            //     "EVSEId":                        "DE*LVF*E970993236*1"
            // }

            var session = new ChargingSession(ChargingSession_Id.Parse(JSON["@id"]?.Value<String>())) {

                RoamingNetworkId           = JSON["roamingNetworkId"]          != null ? RoamingNetwork_Id.         Parse(JSON["roamingNetworkId"]?.         Value<String>()) : new RoamingNetwork_Id?(),
                CSORoamingProviderIdStart  = JSON["CSORoamingProviderId"]      != null ? CSORoamingProvider_Id.     Parse(JSON["CSORoamingProviderId"]?.     Value<String>()) : new CSORoamingProvider_Id?(),
                EMPRoamingProviderIdStart  = JSON["EMPRoamingProviderId"]      != null ? EMPRoamingProvider_Id.     Parse(JSON["EMPRoamingProviderId"]?.     Value<String>()) : new EMPRoamingProvider_Id?(),
                ChargingStationOperatorId  = JSON["chargingStationOperatorId"] != null ? ChargingStationOperator_Id.Parse(JSON["chargingStationOperatorId"]?.Value<String>()) : new ChargingStationOperator_Id?(),
                ChargingPoolId             = JSON["chargingPoolId"]            != null ? ChargingPool_Id.           Parse(JSON["chargingPoolId"]?.           Value<String>()) : new ChargingPool_Id?(),
                ChargingStationId          = JSON["chargingStationId"]         != null ? ChargingStation_Id.        Parse(JSON["chargingStationId"]?.        Value<String>()) : new ChargingStation_Id?(),
                EVSEId                     = JSON["EVSEId"]                    != null ? EVSE_Id.                   Parse(JSON["EVSEId"]?.                   Value<String>()) : new EVSE_Id?()

            };


            if (JSON["start"] is JObject sessionStartJSON)
            {

                var startTime = sessionStartJSON["timestamp"]?.Value<DateTime>();

                if (startTime != null)
                {

                    session.SessionTime                = new StartEndDateTime(startTime.Value);

                    session.SystemIdStart              = sessionStartJSON["systemId"]             != null                  ? System_Id.            Parse(sessionStartJSON["systemId"]?.            Value<String>()) : new System_Id?();
                    session.EMPRoamingProviderIdStart  = sessionStartJSON["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(sessionStartJSON["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                    session.CSORoamingProviderIdStart  = sessionStartJSON["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(sessionStartJSON["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                    session.ProviderIdStart            = sessionStartJSON["providerId"]           != null                  ? eMobilityProvider_Id. Parse(sessionStartJSON["providerId"]?.Value<String>())           : new eMobilityProvider_Id?();
                    session.AuthenticationStart        = sessionStartJSON["authentication"] is JObject authenticationStart ? RemoteAuthentication. Parse(authenticationStart)                                       : null;

                }

                if (JSON["stop"] is JObject sessionStopJSON)
                {

                    var stopTime = sessionStopJSON["timestamp"]?.Value<DateTime>();

                    if (stopTime != null)
                    {

                        session.SessionTime                = new StartEndDateTime(startTime.Value, stopTime);

                        session.SystemIdStop               = sessionStopJSON["systemId"]             != null                  ? System_Id.            Parse(sessionStopJSON["systemId"]?.            Value<String>()) : new System_Id?();
                        session.EMPRoamingProviderIdStop   = sessionStopJSON["EMPRoamingProviderId"] != null                  ? EMPRoamingProvider_Id.Parse(sessionStopJSON["EMPRoamingProviderId"]?.Value<String>()) : new EMPRoamingProvider_Id?();
                        session.CSORoamingProviderIdStop   = sessionStopJSON["CSORoamingProviderId"] != null                  ? CSORoamingProvider_Id.Parse(sessionStopJSON["CSORoamingProviderId"]?.Value<String>()) : new CSORoamingProvider_Id?();
                        session.ProviderIdStop             = sessionStopJSON["providerId"]           != null                  ? eMobilityProvider_Id. Parse(sessionStopJSON["providerId"]?.Value<String>())           : new eMobilityProvider_Id?();
                        session.AuthenticationStop         = sessionStopJSON["authentication"] is JObject authenticationStop  ? LocalAuthentication.  Parse(authenticationStop)                                       : null;

                    }

                }

                if (JSON["cdr"] is JObject sessionCDRJSON)
                {

                    var cdrTime = sessionCDRJSON["timestamp"]?.Value<DateTime>();

                    if (cdrTime != null)
                    {
                        session.CDRReceived   = cdrTime;
                        session.SystemIdCDR   = sessionCDRJSON["systemId"] != null ? System_Id.Parse(sessionCDRJSON["systemId"]?.Value<String>()) : new System_Id?();
                        session.CDRForwarded  = sessionCDRJSON["forwarded"]?.Value<DateTime>();
                    }

                }

            }

            return session;

        }



        #region IComparable<ChargingSession> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging session.
            var ChargingSession = Object as ChargingSession;
            if ((Object) ChargingSession == null)
                throw new ArgumentException("The given object is not a charging session!");

            return CompareTo(ChargingSession);

        }

        #endregion

        #region CompareTo(ChargingSession)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSession">A charging session object to compare with.</param>
        public Int32 CompareTo(ChargingSession ChargingSession)
        {

            if ((Object) ChargingSession == null)
                throw new ArgumentNullException("The given charging session must not be null!");

            return Id.CompareTo(ChargingSession.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingSession> Members

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

            // Check if the given object is a charging session.
            var ChargingSession = Object as ChargingSession;
            if ((Object) ChargingSession == null)
                return false;

            return this.Equals(ChargingSession);

        }

        #endregion

        #region Equals(ChargingSession)

        /// <summary>
        /// Compares two charging sessions for equality.
        /// </summary>
        /// <param name="ChargingSession">A charging session to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingSession ChargingSession)
        {

            if ((Object) ChargingSession == null)
                return false;

            return Id.Equals(ChargingSession.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}
