﻿/*
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the charging sessions.
    /// </summary>
    public static class ChargingSessionExtensions
    {

        #region ToJSON(this ChargeDetailRecords, Embedded = false, ...)

        public static JArray ToJSON(this IEnumerable<ChargingSession>                     ChargingSessions,
                                    Boolean                                               Embedded                             = false,
                                    CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                                    CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer        = null,
                                    CustomJObjectSerializerDelegate<ChargingSession>?     CustomChargingSessionSerializer      = null,
                                    UInt64?                                               Skip                                 = null,
                                    UInt64?                                               Take                                 = null)
        {

            #region Initial checks

            if (ChargingSessions is null || !ChargingSessions.Any())
                return new JArray();

            #endregion

            return new JArray(ChargingSessions.
                                  SkipTakeFilter(Skip, Take).
                                  Select        (chargingSession => chargingSession.ToJSON(Embedded,
                                                                                           CustomChargeDetailRecordSerializer,
                                                                                           CustomSendCDRResultSerializer,
                                                                                           CustomChargingSessionSerializer)));

        }

        #endregion

    }


    /// <summary>
    /// A pool of electric vehicle charging stations.
    /// The geo locations of these charging stations will be close together and the charging session
    /// might provide a shared network access to aggregate and optimize communication
    /// with the EVSE Operator backend.
    /// </summary>
    public class ChargingSession : AInternalData,
                                   IHasId<ChargingSession_Id>,
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

        /// <summary>
        /// The unique charging session identification.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id  Id                 { get; }

        /// <summary>
        /// The event tracking identification causing the creation of this charging session.
        /// </summary>
        public EventTracking_Id    EventTrackingId    { get; }

        #region SystemId

        public System_Id?        SystemIdStart      { get; set; }
        public System_Id?        SystemIdStop       { get; set; }

        public System_Id?        SystemIdCDR        { get; set; }

        #endregion


        #region RoamingNetwork

        /// <summary>
        /// The unqiue identification of the roaming network serving this session.
        /// </summary>
        public RoamingNetwork_Id? RoamingNetworkId { get; set; }


        private IRoamingNetwork? roamingNetwork;

        /// <summary>
        /// The roaming network serving this session.
        /// </summary>
        public IRoamingNetwork? RoamingNetwork
        {

            get
            {
                return roamingNetwork;
            }

            set
            {
                roamingNetwork    = value;
                RoamingNetworkId  = value?.Id;
            }

        }

        #endregion

        #region ChargingStationOperator

        private ChargingStationOperator_Id? chargingStationOperatorId;

        /// <summary>
        /// The unqiue identification of the charging station operator serving this session.
        /// </summary>
        public ChargingStationOperator_Id? ChargingStationOperatorId
        {

            get
            {
                return chargingStationOperatorId;
            }

            set
            {

                chargingStationOperatorId  = value;
                chargingStationOperator    = value.HasValue ? RoamingNetwork?.GetChargingStationOperatorById(value.Value) : null;
                roamingNetwork           ??= chargingStationOperator?.RoamingNetwork;

            }

        }

        private IChargingStationOperator? chargingStationOperator;

        /// <summary>
        /// The charging station operator serving this session.
        /// </summary>
        public IChargingStationOperator? ChargingStationOperator

        {

            get
            {
                return chargingStationOperator;
            }

            set
            {

                chargingStationOperator      = value;
                chargingStationOperatorId    = value?.Id;
                RoamingNetwork             ??= value?.RoamingNetwork;

            }

        }

        #endregion

        #region ChargingPool

        private ChargingPool_Id? chargingPoolId;

        /// <summary>
        /// The unqiue identification of the charging pool serving this session.
        /// </summary>
        public ChargingPool_Id? ChargingPoolId
        {

            get
            {
                return chargingPoolId;
            }

            set
            {
                chargingPoolId             = value;
                chargingPool               = value.HasValue ? RoamingNetwork?.GetChargingPoolById(value.Value) : null;
                ChargingStationOperator  ??= chargingPool?.Operator;
            }

        }


        private IChargingPool? chargingPool;

        /// <summary>
        /// The charging pool serving this session.
        /// </summary>
        public IChargingPool? ChargingPool
        {

            get
            {
                return chargingPool;
            }

            set
            {
                chargingPool               = value;
                chargingPoolId             = value?.Id;
                ChargingStationOperator  ??= value?.Operator;
            }

        }

        #endregion

        #region ChargingStation

        private ChargingStation_Id? chargingStationId;

        /// <summary>
        /// The unqiue identification of the charging station serving this session.
        /// </summary>
        public ChargingStation_Id? ChargingStationId
        {

            get
            {
                return chargingStationId;
            }

            set
            {
                chargingStationId    = value;
                chargingStation      = value.HasValue ? RoamingNetwork?.GetChargingStationById(value.Value) : null;
                ChargingPool       ??= chargingStation?.ChargingPool;
            }

        }


        private IChargingStation? chargingStation;

        /// <summary>
        /// The charging station serving this session.
        /// </summary>
        public IChargingStation? ChargingStation
        {

            get
            {
                return chargingStation;
            }

            set
            {
                chargingStation      = value;
                chargingStationId    = value?.Id;
                ChargingPool       ??= value?.ChargingPool;
            }

        }

        #endregion

        #region EVSE

        private EVSE_Id? evseId;

        /// <summary>
        /// The unqiue identification of the EVSE serving this session.
        /// </summary>
        public EVSE_Id? EVSEId
        {

            get
            {
                return evseId;
            }

            set
            {
                evseId             = value;
                evse               = value.HasValue ? RoamingNetwork?.GetEVSEById(value.Value) : null;
                ChargingStation  ??= evse?.ChargingStation;
            }

        }


        private IEVSE? evse;

        /// <summary>
        /// The EVSE serving this session.
        /// </summary>
        [Optional]
        public IEVSE? EVSE
        {

            get
            {
                return evse;
            }

            set
            {
                evse               = value;
                evseId             = value?.Id;
                ChargingStation  ??= value?.ChargingStation;
            }

        }

        #endregion

        #region ChargingProduct

        /// <summary>
        /// The charging product selected for this charging session.
        /// </summary>
        [Optional]
        public ChargingProduct?  ChargingProduct   { get; set; }

        #endregion

        #region Reservation

        private ChargingReservation_Id? reservationId;

        /// <summary>
        /// An optional charging reservation for this charging session.
        /// </summary>
        [Optional]
        public ChargingReservation_Id? ReservationId
        {

            get
            {
                return reservationId;
            }

            set
            {
                reservationId  = value;
                reservation    = value.HasValue ? RoamingNetwork?.GetChargingReservationById(value.Value) : null;
            }

        }


        private ChargingReservation? reservation;

        /// <summary>
        /// An optional charging reservation for this charging session.
        /// </summary>
        [Optional]
        public ChargingReservation? Reservation
        {

            get
            {
                return reservation;
            }

            set
            {
                reservation    = value;
                reservationId  = value?.Id;
            }

        }

        #endregion


        #region EnergyMeterId

        /// <summary>
        /// An optional unique identification of the energy meter.
        /// </summary>
        [Optional]
        public EnergyMeter_Id? EnergyMeterId { get; set; }

        #endregion

        #region EnergyMeterValues

        private readonly List<EnergyMeteringValue> energyMeterValues;

        /// <summary>
        /// An optional enumeration of intermediate energy meter values.
        /// This values indicate the consumed energy between the current
        /// and the last timestamp in watt-hours [Wh].
        /// </summary>
        [Optional]
        public IEnumerable<EnergyMeteringValue> EnergyMeteringValues
            => energyMeterValues;

        #endregion

        #region ConsumedEnergy

        /// <summary>
        /// The current amount of energy consumed while charging in [kWh].
        /// </summary>
        [Mandatory]
        public Decimal ConsumedEnergy

            => EnergyMeteringValues.
                   Select(metervalue => metervalue.Value).
                   Sum() / 1000;

        #endregion


        #region ParkingTime

        /// <summary>
        /// Optional timestamps when the parking started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime? ParkingTime { get; set; }

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
            => (SessionTime.EndTime ?? Timestamp.Now) - SessionTime.StartTime;

        #endregion


        #region ProviderStart

        private EMobilityProvider_Id? providerIdStart;

        /// <summary>
        /// The identification of the e-mobility provider used for starting this charging process.
        /// </summary>
        [Optional]
        public EMobilityProvider_Id? ProviderIdStart
        {

            get
            {
                return providerIdStart;
            }

            set
            {
                providerIdStart  = value;
                providerStart    = value.HasValue ? RoamingNetwork?.GetEMobilityProviderById(value.Value) : null;
            }

        }

        private IEMobilityProvider? providerStart;

        public IEMobilityProvider? ProviderStart
        {

            get
            {
                return providerStart;
            }

            set
            {
                providerStart    = value;
                providerIdStart  = value?.Id;
            }

        }

        #endregion

        #region ProviderStop

        private EMobilityProvider_Id? providerIdStop;

        /// <summary>
        /// The identification of the e-mobility provider used for stopping this charging process.
        /// </summary>
        [Optional]
        public EMobilityProvider_Id? ProviderIdStop
        {

            get
            {
                return providerIdStop;
            }

            set
            {
                providerIdStop  = value;
                providerStop    = value.HasValue ? RoamingNetwork?.GetEMobilityProviderById(value.Value) : null;
            }

        }

        private IEMobilityProvider? providerStop;

        public IEMobilityProvider? ProviderStop
        {

            get
            {
                return providerStop;
            }

            set
            {

                providerStop    = value;
                providerIdStop  = value?.Id;

            }

        }

        #endregion

        #region AuthenticationStart/-Stop

        /// <summary>
        /// The authentication used for starting this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?  AuthenticationStart    { get; set; }

        /// <summary>
        /// The authentication used for stopping this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?  AuthenticationStop     { get; set; }

        #endregion


        #region CSORoamingProviderStart

        private CSORoamingProvider_Id? csoRoamingProviderIdStart;

        public CSORoamingProvider_Id? CSORoamingProviderIdStart
        {

            get
            {
                return csoRoamingProviderIdStart;
            }

            set
            {
                csoRoamingProviderIdStart  = value;
                csoRoamingProviderStart    = value.HasValue ? RoamingNetwork?.GetCSORoamingProviderById(value.Value) : null;
            }

        }

        private ICSORoamingProvider? csoRoamingProviderStart;

        public ICSORoamingProvider? CSORoamingProviderStart
        {

            get
            {
                return csoRoamingProviderStart;
            }

            set
            {
                csoRoamingProviderStart    = value;
                CSORoamingProviderIdStart  = value?.Id;
            }

        }

        #endregion

        #region CSORoamingProviderStop

        private CSORoamingProvider_Id? csoRoamingProviderIdStop;

        public CSORoamingProvider_Id? CSORoamingProviderIdStop
        {

            get
            {
                return csoRoamingProviderIdStop;
            }

            set
            {
                csoRoamingProviderIdStop  = value;
                csoRoamingProviderStop    = value.HasValue ? RoamingNetwork?.GetCSORoamingProviderById(value.Value) : null;
            }

        }

        private ICSORoamingProvider? csoRoamingProviderStop;

        public ICSORoamingProvider? CSORoamingProviderStop
        {

            get
            {
                return csoRoamingProviderStop;
            }

            set
            {
                csoRoamingProviderStop    = value;
                CSORoamingProviderIdStop  = value?.Id;
            }

        }

        #endregion


        #region EMPRoamingProviderStart

        private EMPRoamingProvider_Id? empRoamingProviderIdStart;

        public EMPRoamingProvider_Id? EMPRoamingProviderIdStart
        {

            get
            {
                return empRoamingProviderIdStart;
            }

            set
            {
                empRoamingProviderIdStart  = value;
                empRoamingProviderStart    = value.HasValue ? RoamingNetwork?.GetEMPRoamingProviderById(value.Value) : null;
            }

        }

        private IEMPRoamingProvider? empRoamingProviderStart;

        public IEMPRoamingProvider? EMPRoamingProviderStart
        {

            get
            {
                return empRoamingProviderStart;
            }

            set
            {
                empRoamingProviderStart    = value;
                EMPRoamingProviderIdStart  = value?.Id;
            }

        }

        #endregion

        #region EMPRoamingProviderStop

        private EMPRoamingProvider_Id? empRoamingProviderIdStop;

        public EMPRoamingProvider_Id? EMPRoamingProviderIdStop
        {

            get
            {
                return empRoamingProviderIdStop;
            }

            set
            {
                empRoamingProviderIdStop  = value;
                empRoamingProviderStop    = value.HasValue ? RoamingNetwork?.GetEMPRoamingProviderById(value.Value) : null;
            }

        }

        private IEMPRoamingProvider? empRoamingProviderStop;

        public IEMPRoamingProvider? EMPRoamingProviderStop
        {

            get
            {
                return empRoamingProviderStop;
            }

            set
            {
                empRoamingProviderStop    = value;
                EMPRoamingProviderIdStop  = value?.Id;
            }

        }

        #endregion



        public ChargeDetailRecord?           CDR                           { get; set; }

        public DateTime?                     CDRReceived                   { get; set; }

        public SendCDRResult?                CDRResult                     { get; set; }


        #region Runtime

        public TimeSpan?                  RuntimeStart                  { get; set; }

        public TimeSpan?                  RuntimeStop                   { get; set; }

        public TimeSpan?                  RuntimeCDR                    { get; set; }

        #endregion



        private readonly List<SessionStopRequest> stopRequests;
        public IEnumerable<SessionStopRequest> StopRequests
            => stopRequests;



        public Boolean                       RemoveMe                      { get; set; }


        private readonly HashSet<String> signatures;

        public IEnumerable<String> Signatures
                   => signatures;

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a new energy meter value was received.
        /// </summary>
        /// <param name="Timestamp">The current timestamp.</param>
        /// <param name="ChargingSession">The unique charging session identification.</param>
        /// <param name="EnergyMeterValue">A timestamped energy meter value.</param>
        public delegate void OnNewEnergyMeterValueDelegate(DateTime Timestamp, ChargingSession ChargingSession, EnergyMeteringValue EnergyMeterValue);

        /// <summary>
        /// An event sent whenever a new energy meter value was received.
        /// </summary>
        public event OnNewEnergyMeterValueDelegate? OnNewEnergyMeterValue;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new group/pool of charging stations having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing pool.</param>
        /// <param name="Timestamp">The timestamp of the session creation.</param>
        public ChargingSession(ChargingSession_Id      Id,
                               IRoamingNetwork         RoamingNetwork,
                               EventTracking_Id        EventTrackingId,

                               DateTime?               Timestamp      = null,
                               JObject?                CustomData     = null,
                               UserDefinedDictionary?  InternalData   = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id                 = Id;
            this.RoamingNetwork     = RoamingNetwork;
            this.EventTrackingId    = EventTrackingId;
            this.SessionTime        = new StartEndDateTime(Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now);
            this.energyMeterValues  = [];
            this.stopRequests       = [];
            this.signatures         = [];

        }

        #endregion


        public void AddEnergyMeterValue(EnergyMeteringValue Value)
        {
            energyMeterValues.Add(Value);
        }

        public void AddStopRequest(SessionStopRequest Value)
        {
            stopRequests.Add(Value);
        }


        //public ChargingSession AddUserData(String Key, Object Value)
        //{
        //    this._UserDefined
        //}


        public static Boolean TryParse(JObject               JSON,
                                       out ChargingSession?  ChargingSession,
                                       out String?           ErrorResponse)
        {

            ErrorResponse    = null;
            ChargingSession  = null;

            return false;

        }


        #region ToJSON(Embedded, ...)

        public JObject ToJSON(Boolean                                               Embedded                             = false,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                              CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer        = null,
                              CustomJObjectSerializerDelegate<ChargingSession>?     CustomChargingSessionSerializer      = null)

        {

            var json = JSONObject.Create(

                                 new JProperty("@id",                         Id.              ToString()),
                                 new JProperty("eventTrackingId",             EventTrackingId. ToString()),

                           Embedded
                               ? null
                               : new JProperty("@context",                    JSONLDContext),

                           RoamingNetworkId.HasValue
                               ? new JProperty("roamingNetworkId",            RoamingNetworkId.ToString())
                               : null,


                           Reservation is not null
                               ? new JProperty("reservation", new JObject(
                                                                  new JProperty("reservationId",  Reservation.Id.ToString()),
                                                                  new JProperty("start",          Reservation.StartTime.ToIso8601()),
                                                                  new JProperty("duration",       Reservation.Duration.TotalSeconds))
                                                              )
                               : ReservationId is not null
                                     ? new JProperty("reservationId",         ReservationId.ToString())
                                     : null,


                           SessionTime is not null
                               ? new JProperty("start", JSONObject.Create(

                                           new JProperty("timestamp",              SessionTime.StartTime.    ToIso8601()),

                                     SystemIdStart.HasValue
                                         ? new JProperty("systemId",               SystemIdStart.            ToString())
                                         : null,

                                     CSORoamingProviderIdStart.HasValue
                                         ? new JProperty("CSORoamingProviderId",   CSORoamingProviderIdStart.ToString())
                                         : null,

                                     EMPRoamingProviderIdStart.HasValue
                                         ? new JProperty("EMPRoamingProviderId",   EMPRoamingProviderIdStart.ToString())
                                         : null,

                                     ProviderIdStart is not null
                                         ? new JProperty("providerId",             ProviderIdStart.          ToString())
                                         : null,

                                     AuthenticationStart?.IsDefined() == true
                                         ? new JProperty("authentication",         AuthenticationStart.      ToJSON())
                                         : null

                                 ))
                               : null,


                           SessionTime is not null
                               ? new JProperty("duration",                    Duration.TotalSeconds)
                               : null,


                           SessionTime is not null && SessionTime.EndTime.HasValue
                               ? new JProperty("stop", JSONObject.Create(

                                     SessionTime.EndTime.HasValue
                                         ? new JProperty("timestamp",             SessionTime.EndTime.Value.ToIso8601())
                                         : null,

                                     SystemIdStop.HasValue
                                         ? new JProperty("systemId",              SystemIdStop.             ToString())
                                         : null,

                                     CSORoamingProviderIdStop.HasValue
                                         ? new JProperty("CSORoamingProviderId",  CSORoamingProviderIdStop. ToString())
                                         : null,

                                     EMPRoamingProviderIdStop.HasValue
                                         ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderIdStop. ToString())
                                         : null,

                                     ProviderIdStop is not null
                                         ? new JProperty("providerId",            ProviderIdStop.           ToString())
                                         : null,

                                     AuthenticationStop?.IsDefined() == true
                                         ? new JProperty("authentication",        AuthenticationStop.       ToJSON())
                                         : null

                                 ))
                               : null,


                           CDRReceived.HasValue
                               ? new JProperty("CDRReceived", JSONObject.Create(

                                     CDRReceived.HasValue
                                         ? new JProperty("timestamp",             CDRReceived.Value. ToIso8601())
                                         : null,

                                     SystemIdCDR.HasValue
                                         ? new JProperty("systemId",              SystemIdCDR.       ToString())
                                         : null,

                                     CDR is not null
                                         ? new JProperty("cdr",                   CDR.               ToJSON(Embedded:                           true,
                                                                                                            CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer))
                                         : null,

                                     CDRResult is not null
                                         ? new JProperty("result",                CDRResult.         ToJSON(Embedded:                           true,
                                                                                                            CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer,
                                                                                                            CustomSendCDRResultSerializer:      CustomSendCDRResultSerializer))
                                         : null

                                 ))
                               : null,



                           StopRequests.Any()
                               ? new JProperty("stopRequests",                new JArray(StopRequests.Select(stopRequest => stopRequest.ToJSON(Embedded: false,
                                                                                                                                               CustomChargeDetailRecordSerializer))))
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

                           ChargingProduct is not null
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

                           //_UserDefined.Any()
                           //    ? new JProperty("userDefined",    new JObject(_UserDefined.Where(kkvp => kkvp.Value is JObject).
                           //                                                               Select(kkvp => new JProperty(kkvp.Key, kkvp.Value as JObject))))
                           //    : null

                );

            return CustomChargingSessionSerializer is not null
                       ? CustomChargingSessionSerializer(this, json)
                       : json;

        }

        #endregion


        public static ChargingSession Parse(String Text, IRoamingNetwork RoamingNetwork)
            => Parse(JObject.Parse(Text), RoamingNetwork);

        public static ChargingSession Parse(JObject JSON, IRoamingNetwork RoamingNetwork)
        {

            // {
            //     "@id":                           "7f0d7978-ab29-462c-908f-b31aa9c9326e",
            //     "@context":                      "https://open.charging.cloud/contexts/wwcp+json/chargingSession",
            //     "roamingNetworkId":              "Prod",
            //     "start": {
            //         "timestamp":                 "2020-03-24T07:44:39.476Z",
            //         "systemId":                  "occ3a",
            //         "CSORoamingProviderId":      "HubjectProd",
            //         "providerId":                "DE-GDF",
            //         "authentication": {
            //           "remoteIdentification":    "DE-GDF-C00171852-1"
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
            //             "evseId":                "DE*GEF*E970993236*1"
            //         }
            //     },
            //
            //     "stopRequests":                  [ ... ],
            //
            //     "chargingStationOperatorId":     "DE*GEF",
            //     "chargingPoolId":                "DE*GEF*P555F437E4ECB6F6",
            //     "chargingStationId":             "DE*GEF*S970993236",
            //     "EVSEId":                        "DE*GEF*E970993236*1"
            // }

            var sessionId  = (JSON["@id"]?.Value<String>())
                                 ?? throw new Exception("The session identification must not be null!");

            var session    = new ChargingSession(
                                 ChargingSession_Id.Parse(sessionId),
                                 RoamingNetwork,
                                 JSON["eventTrackingId"]?.Value<String>() is String eventTrackingId
                                     ? EventTracking_Id.Parse(eventTrackingId)
                                     : EventTracking_Id.New
                             ) {

                                 RoamingNetwork             = RoamingNetwork,
                                 ChargingStationOperatorId  = JSON["chargingStationOperatorId"]?.Value<String>() is String chargingStationOperatorId ? ChargingStationOperator_Id.Parse(chargingStationOperatorId) : null,
                                 ChargingPoolId             = JSON["chargingPoolId"]?.           Value<String>() is String chargingPoolId            ? ChargingPool_Id.           Parse(chargingPoolId)            : null,
                                 ChargingStationId          = JSON["chargingStationId"]?.        Value<String>() is String chargingStationId         ? ChargingStation_Id.        Parse(chargingStationId)         : null,
                                 EVSEId                     = JSON["EVSEId"]?.                   Value<String>() is String EVSEId                    ? EVSE_Id.                   Parse(EVSEId)                    : null,
                             
                             };


            if (JSON["start"]        is JObject sessionStartJSON)
            {

                var startTime = sessionStartJSON["timestamp"]?.Value<DateTime>();

                if (startTime is not null)
                {

                    session.SessionTime                = new StartEndDateTime(startTime.Value);

                    session.SystemIdStart              = sessionStartJSON["systemId"]?.            Value<String>() is String  systemId             ? System_Id.            Parse(systemId)             : null;
                    session.EMPRoamingProviderIdStart  = sessionStartJSON["EMPRoamingProviderId"]?.Value<String>() is String  empRoamingProviderId ? EMPRoamingProvider_Id.Parse(empRoamingProviderId) : null;
                    session.CSORoamingProviderIdStart  = sessionStartJSON["CSORoamingProviderId"]?.Value<String>() is String  csoRoamingProviderId ? CSORoamingProvider_Id.Parse(csoRoamingProviderId) : null;
                    session.ProviderIdStart            = sessionStartJSON["providerId"]?.          Value<String>() is String  providerId           ? EMobilityProvider_Id. Parse(providerId)           : null;
                    session.AuthenticationStart        = sessionStartJSON["authentication"]                        is JObject authenticationStart  ? RemoteAuthentication. Parse(authenticationStart)  : null;

                }

                if (JSON["stop"] is JObject sessionStopJSON)
                {

                    var stopTime = sessionStopJSON["timestamp"]?.Value<DateTime>();

                    if (startTime is not null && stopTime is not null)
                    {

                        session.SessionTime                = new StartEndDateTime(startTime.Value, stopTime);

                        session.SystemIdStop               = sessionStopJSON["systemId"]?.            Value<String>() is String  systemId             ? System_Id.            Parse(systemId)             : null;
                        session.EMPRoamingProviderIdStop   = sessionStopJSON["EMPRoamingProviderId"]?.Value<String>() is String  EMPRoamingProviderId ? EMPRoamingProvider_Id.Parse(EMPRoamingProviderId) : null;
                        session.CSORoamingProviderIdStop   = sessionStopJSON["CSORoamingProviderId"]?.Value<String>() is String  CSORoamingProviderId ? CSORoamingProvider_Id.Parse(CSORoamingProviderId) : null;
                        session.ProviderIdStop             = sessionStopJSON["providerId"]?.          Value<String>() is String  providerId           ? EMobilityProvider_Id. Parse(providerId)           : null;
                        session.AuthenticationStop         = sessionStopJSON["authentication"]                        is JObject authenticationStop   ? LocalAuthentication.  Parse(authenticationStop)   : null;

                    }

                }

                if (JSON["CDRReceived"] is JObject sessionCDRReceivedJSON)
                {

                    var cdrReceivedTime = sessionCDRReceivedJSON["timestamp"]?.Value<DateTime>();

                    if (cdrReceivedTime is not null)
                    {
                        session.CDRReceived   = cdrReceivedTime;
                        session.SystemIdCDR   = sessionCDRReceivedJSON["systemId"]?.Value<String>() is String systemId ? System_Id.Parse(systemId) : null;
                    }

                    if (sessionCDRReceivedJSON["CDRResult"] is JObject CDRResultJSON)
                    {
                        if (SendCDRResult.TryParse(CDRResultJSON, out var sendCDRResult, out var errorResponse))
                            session.CDRResult     = sendCDRResult;
                    }

                }

            }

            if (JSON["stopRequests"] is JArray  stopRequestsJSON)
            {

                foreach (var stopRequestJSON in stopRequestsJSON)
                {
                    if (stopRequestJSON is JObject stopRequestJObject)
                    {
                        session.AddStopRequest(SessionStopRequest.Parse(stopRequestJObject));
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
        public Int32 CompareTo(Object? Object)

            => Object is ChargingSession chargingSession
                   ? CompareTo(chargingSession)
                   : throw new ArgumentException("The given object is not a charging session!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingSession)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSession">A charging session object to compare with.</param>
        public Int32 CompareTo(ChargingSession? ChargingSession)
        {

            if (ChargingSession is null)
                throw new ArgumentNullException(nameof(ChargingSession), "The given charging session must not be null!");

            var c = Id.CompareTo(ChargingSession.Id);

            //if (c == 0)
            //    c = SessionId.CompareTo(ChargingSession.SessionId);

            return c;

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
        public override Boolean Equals(Object? Object)

            => Object is ChargingSession chargingSession &&
                   Equals(chargingSession);

        #endregion

        #region Equals(ChargingSession)

        /// <summary>
        /// Compares two charging sessions for equality.
        /// </summary>
        /// <param name="ChargingSession">A charging session to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingSession? ChargingSession)

            => ChargingSession is not null &&
               Id.Equals(ChargingSession.Id);

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
