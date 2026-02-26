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

using System.Diagnostics.CodeAnalysis;

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

        #region ToJSON(this ChargingSessions, Embedded = false, ...)

        public static JArray ToJSON(this IEnumerable<ChargingSession>                     ChargingSessions,
                                    Boolean                                               Embedded                             = false,
                                    Boolean                                               OnlineInfos                          = true,
                                    CustomJObjectSerializerDelegate<ChargingSession>?     CustomChargingSessionSerializer      = null,
                                    CustomJObjectSerializerDelegate<ReceivedCDRInfo>?     CustomCDRReceivedInfoSerializer      = null,
                                    CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                                    CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer        = null,
                                    UInt64?                                               Skip                                 = null,
                                    UInt64?                                               Take                                 = null)
        {

            #region Initial checks

            if (ChargingSessions is null || !ChargingSessions.Any())
                return [];

            #endregion

            return new JArray(ChargingSessions.
                                  SkipTakeFilter(Skip, Take).
                                  Select        (chargingSession => chargingSession.ToJSON(Embedded,
                                                                                           OnlineInfos,
                                                                                           CustomChargingSessionSerializer,
                                                                                           CustomCDRReceivedInfoSerializer,
                                                                                           CustomChargeDetailRecordSerializer,
                                                                                           CustomSendCDRResultSerializer)));

        }

        #endregion

    }

    public class ReceivedCDRInfo
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/cdrReceivedInfo";

        #endregion

        #region Properties

        public DateTimeOffset      Timestamp             { get; }
        public System_Id           SystemId              { get; }
        public EventTracking_Id    EventTrackingId       { get; }
        public ChargeDetailRecord  ChargeDetailRecord    { get; }

        #endregion

        #region Constructor(s)

        public ReceivedCDRInfo(DateTimeOffset      Timestamp,
                               System_Id           SystemId,
                               EventTracking_Id    EventTrackingId,
                               ChargeDetailRecord  ChargeDetailRecord)
        {

            this.Timestamp           = Timestamp;
            this.SystemId            = SystemId;
            this.EventTrackingId     = EventTrackingId;
            this.ChargeDetailRecord  = ChargeDetailRecord;

        }

        #endregion



        #region ToJSON(...)

        public JObject ToJSON(Boolean                                               Embedded                             = true,
                              CustomJObjectSerializerDelegate<ReceivedCDRInfo>?     CustomCDRReceivedInfoSerializer      = null,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                              CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer        = null,
                              CustomJObjectSerializerDelegate<Warning>?             CustomWarningSerializer              = null)

        {

            var json = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",          JSONLDContext)
                               : null,

                                 new JProperty("timestamp",         Timestamp.         ToISO8601()),
                                 new JProperty("systemId",          SystemId.          ToString()),
                                 new JProperty("eventTrackingId",   EventTrackingId.   ToString()),

                                 new JProperty("cdr",               ChargeDetailRecord.ToJSON(Embedded:                             true,
                                                                                              CustomChargeDetailRecordSerializer:   CustomChargeDetailRecordSerializer))

                                 //new JProperty("result",            Result.         ToJSON(Embedded:                             true,
                                 //                                                          IncludeCDR:                           false,
                                 //                                                          CustomChargeDetailRecordSerializer:   CustomChargeDetailRecordSerializer,
                                 //                                                          CustomSendCDRResultSerializer:        CustomSendCDRResultSerializer,
                                 //                                                          CustomWarningSerializer:              CustomWarningSerializer))

                       );

            return CustomCDRReceivedInfoSerializer is not null
                        ? CustomCDRReceivedInfoSerializer(this, json)
                        : json;

        }

        #endregion

        #region TryParse()

        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out ReceivedCDRInfo?  ReceivedCDRInfo,
                                       [NotNullWhen(false)] out String?           ErrorResponse)
        {

            ReceivedCDRInfo = null;

            try
            {

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Timestamp             [mandatory]

                if (!JSON.ParseMandatory("timestamp",
                                         "timestamp",
                                         out DateTime Timestamp,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SystemId              [mandatory]

                if (!JSON.ParseMandatory("systemId",
                                         "system identification",
                                         System_Id.TryParse,
                                         out System_Id SystemId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EventTrackingId       [mandatory]

                if (!JSON.ParseMandatory("eventTrackingId",
                                        "event tracking identification",
                                        EventTracking_Id.TryParse,
                                        out EventTracking_Id? EventTrackingId,
                                        out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargeDetailRecord    [mandatory]

                if (!JSON.ParseMandatoryJSON("cdr",
                                             "charge detail record",
                                             WWCP.ChargeDetailRecord.TryParse,
                                             out ChargeDetailRecord? ChargeDetailRecord,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SendCDRResult         [optional]

                //if (JSON.ParseOptionalJSON("sendCDRResult",
                //                           "send charge detail record result",
                //                           WWCP.SendCDRResult.TryParse,
                //                           out SendCDRResult? SendCDRResult,
                //                           out ErrorResponse))
                //{
                //    if (ErrorResponse is not null)
                //        return false;
                //}

                #endregion


                ReceivedCDRInfo = new ReceivedCDRInfo(
                                      Timestamp,
                                      SystemId,
                                      EventTrackingId,
                                      ChargeDetailRecord
                                      //SendCDRResult ?? SendCDRResult.Success(
                                      //                     Timestamp,
                                      //                     SystemId,
                                      //                     ChargeDetailRecord
                                      //                 )
                                  );

                return true;

            }
            catch (Exception e)
            {
                ReceivedCDRInfo  = null;
                ErrorResponse    = "The given JSON representation of a charge detail record info is invalid: " + e.Message;
            }

            return false;

        }

        #endregion


    }



    //public class CDRForwardedInfos(DateTime            Timestamp,
    //                               SendCDRResult       Result)
    //{

    //    public DateTime            Timestamp    { get; } = Timestamp;
    //    public ChargeDetailRecord  CDR          { get; } = CDR;

    //}



    /// <summary>
    /// A charging session.
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

        #region SystemIds

        public System_Id?          SystemIdStart      { get; set; }
        public System_Id?          SystemIdStop       { get; set; }

        #endregion


        #region RoamingNetwork(Id)

        /// <summary>
        /// The unique identification of the roaming network serving this session.
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

        #region ChargingStationOperator(Id)

        private ChargingStationOperator_Id? chargingStationOperatorId;

        /// <summary>
        /// The unique identification of the charging station operator serving this session.
        /// </summary>
        public ChargingStationOperator_Id? ChargingStationOperatorId
        {

            get
            {

                if (!chargingStationOperatorId.HasValue && chargingStationOperator is not null)
                    chargingStationOperatorId = chargingStationOperator?.Id;

                return chargingStationOperatorId;

            }

            set
            {

                chargingStationOperatorId  = value;

                if (chargingStationOperator is null && chargingStationOperatorId.HasValue)
                    chargingStationOperator = RoamingNetwork?.GetChargingStationOperatorById(chargingStationOperatorId.Value);

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

                if (chargingStationOperator is null && chargingStationOperatorId.HasValue)
                    chargingStationOperator = RoamingNetwork?.GetChargingStationOperatorById(chargingStationOperatorId.Value);

                return chargingStationOperator;

            }

            set
            {

                chargingStationOperator     = value;
                chargingStationOperatorId   = value?.Id;
                RoamingNetwork            ??= value?.RoamingNetwork;

            }

        }

        #endregion

        #region ChargingPool(Id)

        private ChargingPool_Id? chargingPoolId;

        /// <summary>
        /// The unique identification of the charging pool serving this session.
        /// </summary>
        public ChargingPool_Id? ChargingPoolId
        {

            get
            {

                if (chargingPool is null && chargingPoolId.HasValue)
                    chargingPool = RoamingNetwork?.GetChargingPoolById(chargingPoolId.Value);

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

        #region ChargingStation(Id)

        private ChargingStation_Id? chargingStationId;

        /// <summary>
        /// The unique identification of the charging station serving this session.
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

                if (chargingStation is null && chargingStationId.HasValue)
                    chargingStation = RoamingNetwork?.GetChargingStationById(chargingStationId.Value);

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

        #region EVSE(Id)

        private EVSE_Id? evseId;

        /// <summary>
        /// The unique identification of the EVSE serving this session.
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

                if (evse is null && evseId.HasValue)
                    evse = RoamingNetwork?.GetEVSEById(evseId.Value);

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

        #region EnergyMeter(Id)

        private EnergyMeter_Id? energyMeterId;

        /// <summary>
        /// An optional unique identification of the energy meter.
        /// </summary>
        [Optional]
        /// <summary>
        /// The unique identification of the EVSE serving this session.
        /// </summary>
        public EnergyMeter_Id? EnergyMeterId
        {

            get
            {
                return energyMeterId;
            }

            set
            {
                energyMeterId  = value;
                energyMeter    = value.HasValue ? RoamingNetwork?.GetEnergyMeterById(value.Value) : null;
            }

        }


        private IEnergyMeter? energyMeter;

        /// <summary>
        /// The EVSE serving this session.
        /// </summary>
        [Optional]
        public IEnergyMeter? EnergyMeter
        {

            get
            {
                return energyMeter;
            }

            set
            {
                energyMeter    = value;
                energyMeterId  = value?.Id;
            }

        }

        #endregion

        #region Reservation(Id)

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


        #region ChargingProduct

        /// <summary>
        /// The charging product selected for this charging session.
        /// </summary>
        [Optional]
        public ChargingProduct? ChargingProduct { get; set; }

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
        public WattHour ConsumedEnergy

            => EnergyMeteringValues.
                   Select(metervalue => metervalue.WattHours).
                   Sum();

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


        public IId? AuthorizatorIdStart { get; set; }
        public IId? AuthorizatorIdStop  { get; set; }

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



        #region AuthenticationStart/-Stop (can be local or remote)

        /// <summary>
        /// The authentication used for starting this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?  AuthenticationStart    { get; set; }

        // AuthenticationStartTimestamp

        // AuthenticationStartResponseTimestamp
        // AuthenticationStartResponse


        /// <summary>
        /// The authentication used for stopping this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?  AuthenticationStop     { get; set; }

        // AuthenticationStopTimestamp

        // AuthenticationStopResponseTimestamp
        // AuthenticationStopResponse

        #endregion



        private readonly List<SessionStopRequest> stopRequests;
        public IEnumerable<SessionStopRequest> StopRequests
            => stopRequests;



        //public DateTimeOffset?                CDRReceivedTimestamp          { get; set; }
        //public ChargeDetailRecord?            CDR                           { get; set; }




        private readonly List<ReceivedCDRInfo> receivedCDRInfos = [];

        public IEnumerable<ReceivedCDRInfo>   ReceivedCDRInfos
            => receivedCDRInfos;




        private readonly List<SendCDRResult> sendCDRResults = [];

        public IEnumerable<SendCDRResult>     SendCDRResults
            => sendCDRResults;


        #region Runtime

        public TimeSpan?                  RuntimeStart                  { get; set; }

        public TimeSpan?                  RuntimeStop                   { get; set; }

        public TimeSpan?                  RuntimeCDR                    { get; set; }

        #endregion


        public DateTimeOffset?                NoAutoDeletionBefore          { get; set; }
        public Boolean                        RemoveMe                      { get; set; }


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
        /// Create a new charging session.
        /// </summary>
        /// <param name="Id">The unique identification of the charging session.</param>
        /// <param name="Timestamp">The timestamp of the charging session creation.</param>
        public ChargingSession(ChargingSession_Id      Id,
                               EventTracking_Id        EventTrackingId,

                               IRoamingNetwork?        RoamingNetwork            = null,
                               ICSORoamingProvider?    CSORoamingProviderStart   = null,
                               IEMPRoamingProvider?    EMPRoamingProviderStart   = null,

                               DateTimeOffset?         Timestamp                 = null,
                               JObject?                CustomData                = null,
                               UserDefinedDictionary?  InternalData              = null)

            : base(CustomData,
                   InternalData,
                   org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)

        {

            this.Id                       = Id;
            this.RoamingNetwork           = RoamingNetwork;
            this.EventTrackingId          = EventTrackingId;

            this.CSORoamingProviderStart  = CSORoamingProviderStart;
            this.EMPRoamingProviderStart  = EMPRoamingProviderStart;

            this.SessionTime              = new StartEndDateTime(Timestamp ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now);
            this.energyMeterValues        = [];
            this.stopRequests             = [];
            this.signatures               = [];

        }

        #endregion


        public void AddAdditionalSessionInfos(JObject? AdditionalSessionInfos)
        {
            if (AdditionalSessionInfos is not null)
            {
                foreach (var property in AdditionalSessionInfos.Properties())
                {
                    this.CustomData[property.Name] = property.Value;
                }
            }
        }

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


        #region Parse(JSON)

        public static ChargingSession Parse(JObject JSON)
        {

            if (TryParse(JSON,
                         out var chargingSession,
                         out var errorResponse))
            {
                return chargingSession;
            }

            throw new ArgumentException("The given JSON representation of a charging session is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region TryParse(JSON, out ChargingSession, ErrorResponse)


        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out ChargingSession?  ChargingSession,
                                       [NotNullWhen(false)] out String?           ErrorResponse)
        {

            ErrorResponse    = null;
            ChargingSession  = null;

            // {
            //     "@id":                           "7f0d7978-ab29-462c-908f-b31aa9c9326e",
            //     "@context":                      "https://open.charging.cloud/contexts/wwcp+json/chargingSession",
            //     "roamingNetworkId":              "Prod",
            //     "noAutoDeletionBefore":          "2020-06-24T07:44:39.476Z",
            //
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
            //     "sendCDRResults":                [ ... ],
            //
            //     "stopRequests":                  [ ... ],
            //
            //     "chargingStationOperatorId":     "DE*GEF",
            //     "chargingPoolId":                "DE*GEF*P555F437E4ECB6F6",
            //     "chargingStationId":             "DE*GEF*S970993236",
            //     "EVSEId":                        "DE*GEF*E970993236*1"
            // }




            // {
            //   "@id":                     "b0a4af6a-3a11-4296-b1e4-7221fd318d36",
            //   "@context":                "https://open.charging.cloud/contexts/wwcp+json/chargingSession",
            //   "roamingNetworkId":        "Prod",
            //   "start": {
            //       "timestamp":                   "2023-12-31T18:45:43.636Z",
            //       "systemId":                    "de-bd-gw-02",
            //       "CSORoamingProviderId":        "HubjectProd",
            //       "providerId":                  "DE*DCS",
            //       "authentication": {
            //           "authToken":                       "042C797A846B85"
            //       }
            //   },
            //   "duration": 19119.0457514,
            //   "stop": {
            //       "timestamp":                   "2024-01-01T00:04:22.681Z",
            //       "systemId":                    "de-bd-gw-02"
            //   },
            //   "CDRReceived": {
            //       "timestamp":                   "2024-01-01T00:04:22.681Z",
            //       "systemId":                    "de-bd-gw-02",
            //       "cdr": {
            //           "@id":                             "b0a4af6a-3a11-4296-b1e4-7221fd318d36",
            //           "sessionId":                       "b0a4af6a-3a11-4296-b1e4-7221fd318d36",
            //           "sessionTime": {
            //               "start":                           "2023-12-31T18:45:42.799Z",
            //               "end":                             "2024-01-01T00:04:21.285Z"
            //           },
            //           "duration":                        19118.486,
            //           "providerIdStart":                 "DE*DCS",
            //           "energyMeteringValues": [
            //               {
            //                   "timestamp":                       "2023-12-31T18:45:42.799Z",
            //                   "value":                            14788.891
            //               },
            //               {
            //                   "timestamp":                       "2024-01-01T00:04:21.285Z",
            //                   "value":                            14820.773
            //               }
            //           ],
            //           "chargingStationOperatorId":       "DE*SLB",
            //           "chargingPoolId":                  "DE*SLB*P7787BF6CE4B4732",
            //           "chargingStationId":               "DE*SLB*S611556753",
            //           "evseId":                          "DE*SLB*E611556753*1",
            //           "created":                         "2024-01-01T00:04:22.681Z",
            //           "lastChange":                      "2024-01-01T00:04:22.681Z"
            //       }
            //   },
            //   "chargingPoolId":          "DE*SLB*P7787BF6CE4B4732",
            //   "chargingStationId":       "DE*SLB*S611556753",
            //   "EVSEId":                  "DE*SLB*E611556753*1"
            // }






            try
            {

                var sessionId  = (JSON["@id"]?.Value<String>())
                                     ?? throw new Exception("The session identification must not be null!");

                ChargingSession  = new ChargingSession(
                                       ChargingSession_Id.Parse(sessionId),
                                       EventTrackingId:  JSON["eventTrackingId"]?.Value<String>() is String eventTrackingId
                                                             ? EventTracking_Id.Parse(eventTrackingId)
                                                             : EventTracking_Id.New,
                                       CustomData:       JSON["customData"] as JObject
                                   ) {

                                       EVSEId                     = JSON["EVSEId"]?.                   Value<String>() is String EVSEId                    ? EVSE_Id.                   Parse(EVSEId)                    : null,
                                       ChargingStationId          = JSON["chargingStationId"]?.        Value<String>() is String chargingStationId         ? ChargingStation_Id.        Parse(chargingStationId)         : null,
                                       ChargingPoolId             = JSON["chargingPoolId"]?.           Value<String>() is String chargingPoolId            ? ChargingPool_Id.           Parse(chargingPoolId)            : null,
                                       ChargingStationOperatorId  = JSON["chargingStationOperatorId"]?.Value<String>() is String chargingStationOperatorId ? ChargingStationOperator_Id.Parse(chargingStationOperatorId) : null,
                                       RoamingNetworkId           = JSON["roamingNetworkId"]?.         Value<String>() is String roamingNetworkId          ? RoamingNetwork_Id.         Parse(roamingNetworkId)          : null

                                   };


                var noAutoDeletionBefore = JSON["noAutoDeletionBefore"]?.Value<DateTime>();
                if (noAutoDeletionBefore.HasValue)
                    ChargingSession.NoAutoDeletionBefore = noAutoDeletionBefore.Value;


                if (JSON["start"]        is JObject sessionStartJSON)
                {

                    var startTime = sessionStartJSON["timestamp"]?.Value<DateTime>();

                    if (startTime is not null)
                    {

                        ChargingSession.SessionTime                = new StartEndDateTime(startTime.Value);

                        ChargingSession.SystemIdStart              = sessionStartJSON["systemId"]?.            Value<String>() is String  systemId             ? System_Id.            Parse(systemId)             : null;
                        ChargingSession.EMPRoamingProviderIdStart  = sessionStartJSON["EMPRoamingProviderId"]?.Value<String>() is String  empRoamingProviderId ? EMPRoamingProvider_Id.Parse(empRoamingProviderId) : null;
                        ChargingSession.CSORoamingProviderIdStart  = sessionStartJSON["CSORoamingProviderId"]?.Value<String>() is String  csoRoamingProviderId ? CSORoamingProvider_Id.Parse(csoRoamingProviderId) : null;
                        ChargingSession.ProviderIdStart            = sessionStartJSON["providerId"]?.          Value<String>() is String  providerId           ? EMobilityProvider_Id. Parse(providerId)           : null;
                        ChargingSession.AuthenticationStart        = sessionStartJSON["authentication"]                        is JObject authenticationStart  ? AAuthentication.      Parse(authenticationStart)  : null;

                    }

                    if (JSON["stop"] is JObject sessionStopJSON)
                    {

                        var stopTime = sessionStopJSON["timestamp"]?.Value<DateTime>();

                        if (startTime is not null && stopTime is not null)
                        {

                            ChargingSession.SessionTime                = new StartEndDateTime(startTime.Value, stopTime);

                            ChargingSession.SystemIdStop               = sessionStopJSON["systemId"]?.            Value<String>() is String  systemId             ? System_Id.            Parse(systemId)             : null;
                            ChargingSession.EMPRoamingProviderIdStop   = sessionStopJSON["EMPRoamingProviderId"]?.Value<String>() is String  EMPRoamingProviderId ? EMPRoamingProvider_Id.Parse(EMPRoamingProviderId) : null;
                            ChargingSession.CSORoamingProviderIdStop   = sessionStopJSON["CSORoamingProviderId"]?.Value<String>() is String  CSORoamingProviderId ? CSORoamingProvider_Id.Parse(CSORoamingProviderId) : null;
                            ChargingSession.ProviderIdStop             = sessionStopJSON["providerId"]?.          Value<String>() is String  providerId           ? EMobilityProvider_Id. Parse(providerId)           : null;
                            ChargingSession.AuthenticationStop         = sessionStopJSON["authentication"]                        is JObject authenticationStop   ? LocalAuthentication.  Parse(authenticationStop)   : null;

                        }

                    }

                    if (JSON["receivedCDRInfos"] is JArray receivedCDRInfosJSON)
                    {
                        foreach (var receivedCDRInfoJSON in receivedCDRInfosJSON.Cast<JObject>())
                        {
                            if (ReceivedCDRInfo.TryParse(receivedCDRInfoJSON, out var receivedCDRInfo, out var err1))
                            {

                                // Temporary fix for missing AuthenticationStart in received CDRs!
                                if (receivedCDRInfo.ChargeDetailRecord is not null &&
                                    receivedCDRInfo.ChargeDetailRecord.AuthenticationStart is null &&
                                    ChargingSession.AuthenticationStart is not null)
                                {
                                    receivedCDRInfo.ChargeDetailRecord.AuthenticationStart = ChargingSession.AuthenticationStart;
                                }

                                ChargingSession.receivedCDRInfos.Add(receivedCDRInfo);

                            }
                            else
                                DebugX.Log(nameof(ChargingSession) + ".ReceivedCDRInfo.TryParse(...) failed: " + err1);
                        }
                    }

                    if (JSON["sendCDRResults"] is JArray sendCDRResultsJSON)
                    {
                        foreach (var sendCDRResultJSON in sendCDRResultsJSON.Cast<JObject>())
                        {
                            if (SendCDRResult.TryParse(sendCDRResultJSON, out var sendCDRResult, out var err2))
                                ChargingSession.sendCDRResults.Add(sendCDRResult);
                            else
                                DebugX.Log(nameof(ChargingSession) + ".ReceivedCDRInfo.TryParse(...) failed: " + err2);
                        }
                    }

                }


                return true;

            }
            catch (Exception e)
            {
                DebugX.LogException(e, $"{nameof(ChargingSession)}.TryParse(...)");
                ErrorResponse = e.Message;
            }

            return false;

        }

        #endregion

        #region ToJSON(Embedded, ...)

        public JObject ToJSON(Boolean                                               Embedded                             = false,
                              Boolean                                               OnlineInfos                          = true,
                              CustomJObjectSerializerDelegate<ChargingSession>?     CustomChargingSessionSerializer      = null,
                              CustomJObjectSerializerDelegate<ReceivedCDRInfo>?     CustomCDRReceivedInfoSerializer      = null,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                              CustomJObjectSerializerDelegate<SendCDRResult>?       CustomSendCDRResultSerializer        = null,
                              CustomJObjectSerializerDelegate<Warning>?             CustomWarningSerializer              = null)

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

                           NoAutoDeletionBefore.HasValue
                               ? new JProperty("noAutoDeletionBefore",        NoAutoDeletionBefore.Value.ToISO8601())
                               : null,


                           Reservation is not null
                               ? new JProperty("reservation", new JObject(
                                                                  new JProperty("reservationId",  Reservation.Id.ToString()),
                                                                  new JProperty("start",          Reservation.StartTime.ToISO8601()),
                                                                  new JProperty("duration",       Reservation.Duration.TotalSeconds))
                                                              )
                               : ReservationId is not null
                                     ? new JProperty("reservationId",         ReservationId.ToString())
                                     : null,


                           SessionTime is not null
                               ? new JProperty("start", JSONObject.Create(

                                           new JProperty("timestamp",              SessionTime.StartTime.    ToISO8601()),

                                     SystemIdStart.HasValue
                                         ? new JProperty("systemId",               SystemIdStart.            ToString())
                                         : null,

                                     CSORoamingProviderIdStart.HasValue
                                         ? new JProperty("CSORoamingProviderId",   CSORoamingProviderIdStart.ToString())
                                         : null,

                                     EMPRoamingProviderIdStart.HasValue
                                         ? new JProperty("EMPRoamingProviderId",   EMPRoamingProviderIdStart.ToString())
                                         : null,

                                     AuthorizatorIdStart is not null
                                         ? new JProperty("authorizatorId",         AuthorizatorIdStart.      ToString())
                                         : null,

                                     ProviderIdStart     is not null
                                         ? new JProperty("providerId",             ProviderIdStart.          ToString())
                                         : null,

                                     AuthenticationStart?.IsDefined() == true
                                         ? new JProperty("authentication",         AuthenticationStart.      ToJSON())
                                         : null

                                 ))
                               : null,


                           OnlineInfos && SessionTime is not null
                               ? new JProperty("duration",                    Duration.TotalSeconds)
                               : null,


                           SessionTime is not null && SessionTime.EndTime.HasValue
                               ? new JProperty("stop", JSONObject.Create(

                                     SessionTime.EndTime.HasValue
                                         ? new JProperty("timestamp",             SessionTime.EndTime.Value.ToISO8601())
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

                                     AuthorizatorIdStop is not null
                                         ? new JProperty("authorizatorId",        AuthorizatorIdStop.       ToString())
                                         : null,

                                     ProviderIdStop     is not null
                                         ? new JProperty("providerId",            ProviderIdStop.           ToString())
                                         : null,

                                     AuthenticationStop?.IsDefined() == true
                                         ? new JProperty("authentication",        AuthenticationStop.       ToJSON())
                                         : null

                                 ))
                               : null,


                           StopRequests.Any()
                               ? new JProperty("stopRequests",                new JArray(StopRequests.Select(stopRequest => stopRequest.ToJSON(Embedded: false,
                                                                                                                                               CustomChargeDetailRecordSerializer))))
                               : null,



                           //CDRReceivedTimestamp.HasValue
                           //    ? new JProperty("CDRReceived", JSONObject.Create(

                           //          CDRReceivedTimestamp.HasValue
                           //              ? new JProperty("timestamp",             CDRReceivedTimestamp.Value. ToISO8601())
                           //              : null,

                           //          SystemIdCDR.HasValue
                           //              ? new JProperty("systemId",              SystemIdCDR.       ToString())
                           //              : null,

                           //          CDR is not null
                           //              ? new JProperty("cdr",                   CDR.               ToJSON(Embedded:                           true,
                           //                                                                                 CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer))
                           //              : null,

                           //          CDRResult is not null
                           //              ? new JProperty("result",                CDRResult.         ToJSON(Embedded:                           true,
                           //                                                                                 CustomChargeDetailRecordSerializer: CustomChargeDetailRecordSerializer,
                           //                                                                                 CustomSendCDRResultSerializer:      CustomSendCDRResultSerializer))
                           //              : null

                           //      ))
                           //    : null,


                           receivedCDRInfos.Count != 0
                               ? new JProperty("receivedCDRInfos",            new JArray(receivedCDRInfos.Select(cdrReceivedInfo   => cdrReceivedInfo.ToJSON(Embedded:                            true,
                                                                                                                                                             CustomCDRReceivedInfoSerializer:     CustomCDRReceivedInfoSerializer,
                                                                                                                                                             CustomChargeDetailRecordSerializer:  CustomChargeDetailRecordSerializer,
                                                                                                                                                             CustomSendCDRResultSerializer:       CustomSendCDRResultSerializer,
                                                                                                                                                             CustomWarningSerializer:             CustomWarningSerializer))))
                               : null,


                           sendCDRResults.Count != 0
                               ? new JProperty("sendCDRResults",              new JArray(sendCDRResults.  Select(CDRResult         => CDRResult.      ToJSON(Embedded:                            true,
                                                                                                                                                             CustomChargeDetailRecordSerializer:  CustomChargeDetailRecordSerializer,
                                                                                                                                                             CustomSendCDRResultSerializer:       CustomSendCDRResultSerializer,
                                                                                                                                                             CustomWarningSerializer:             CustomWarningSerializer))))
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
                                                                                                           new JProperty("timestamp",  meterValue.Timestamp.ToISO8601()),
                                                                                                           new JProperty("value",      meterValue.WattHours)
                                                                                                       ))
                                                                              ))
                               : null,

                           //_UserDefined.Any()
                           //    ? new JProperty("userDefined",    new JObject(_UserDefined.Where(kkvp => kkvp.Value is JObject).
                           //                                                               Select(kkvp => new JProperty(kkvp.Key, kkvp.Value as JObject))))
                           //    : null

                                 new JProperty("customData",                  CustomData)

                );

            return CustomChargingSessionSerializer is not null
                       ? CustomChargingSessionSerializer(this, json)
                       : json;

        }

        #endregion



        public void AddCDRReceivedInfo(ReceivedCDRInfo CDRReceivedInfo)
        {

            // Temporary fix for missing AuthenticationStart in received CDRs!
            if (CDRReceivedInfo.ChargeDetailRecord is not null &&
                CDRReceivedInfo.ChargeDetailRecord.AuthenticationStart is null &&
                AuthenticationStart is not null)
            {
                CDRReceivedInfo.ChargeDetailRecord.AuthenticationStart = AuthenticationStart;
            }

            receivedCDRInfos.Add(CDRReceivedInfo);

        }

        public void AddCDRResult(SendCDRResult SendCDRResult)
        {
            sendCDRResults.Add(SendCDRResult);
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
        /// <returns>True if both match; False otherwise.</returns>
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
