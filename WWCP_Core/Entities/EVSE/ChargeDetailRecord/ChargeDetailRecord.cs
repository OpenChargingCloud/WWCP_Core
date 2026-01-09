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

using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charge detail records.
    /// </summary>
    /// <param name="ChargeDetailRecord">A charge detail record to filter.</param>
    public delegate ChargeDetailRecordFilters ChargeDetailRecordFilterDelegate(ChargeDetailRecord ChargeDetailRecord);


    /// <summary>
    /// Extension methods for charge detail records.
    /// </summary>
    public static partial class ChargeDetailRecordExtensions
    {

        #region ToJSON(this ChargeDetailRecords, Embedded = false, ...)

        public static JArray ToJSON(this IEnumerable<ChargeDetailRecord>                  ChargeDetailRecords,
                                    Boolean                                               Embedded                             = false,
                                    CustomJObjectSerializerDelegate<ChargeDetailRecord>?  CustomChargeDetailRecordSerializer   = null,
                                    UInt64?                                               Skip                                 = null,
                                    UInt64?                                               Take                                 = null)
        {

            #region Initial checks

            if (ChargeDetailRecords is null || !ChargeDetailRecords.Any())
                return [];

            #endregion

            return new JArray(ChargeDetailRecords.
                                  SkipTakeFilter(Skip, Take).
                                  Select        (chargeDetailRecord => chargeDetailRecord.ToJSON(Embedded,
                                                                                                 CustomChargeDetailRecordSerializer)));

        }

        #endregion

    }


    /// <summary>
    /// A charge detail record for a charging session.
    /// </summary>
    public class ChargeDetailRecord : AInternalData,
                                      IHasId<ChargeDetailRecord_Id>,
                                      IEquatable<ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/chargeDetailRecord";

        #endregion

        #region Properties

        /// <summary>
        /// The unique charge detail record identification.
        /// </summary>
        [Mandatory]
        public ChargeDetailRecord_Id    Id              { get; }

        #region Session

        /// <summary>
        /// The unique charging session identification.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id       SessionId       { get; }

        /// <summary>
        /// The timestamps when the charging session started and ended.
        /// </summary>
        [Mandatory]
        public StartEndDateTime         SessionTime     { get; }

        /// <summary>
        /// The optional duration of the charging session, whenever it is more
        /// than the time span between its start- and endtime, e.g.
        /// caused by a tariff granularity of 15 minutes.
        /// </summary>
        [Optional]
        public TimeSpan?                Duration        { get; }

        #endregion

        #region Location / Operator

        /// <summary>
        /// The charging station operator used for charging.
        /// </summary>
        [Optional]
        public IChargingStationOperator?    ChargingStationOperator      { get; }

        /// <summary>
        /// The identification of the charging station operator used for charging.
        /// </summary>
        [Optional]
        public ChargingStationOperator_Id?  ChargingStationOperatorId    { get; }

        /// <summary>
        /// The charging pool of the charging pool used for charging.
        /// </summary>
        [Optional]
        public IChargingPool?               ChargingPool                 { get; }

        /// <summary>
        /// The identification of the charging pool used for charging.
        /// </summary>
        [Optional]
        public ChargingPool_Id?             ChargingPoolId               { get; }

        /// <summary>
        /// The charging station of the charging station used for charging.
        /// </summary>
        [Optional]
        public IChargingStation?            ChargingStation              { get; }

        /// <summary>
        /// The identification of the charging station used for charging.
        /// </summary>
        [Optional]
        public ChargingStation_Id?          ChargingStationId            { get; }

        /// <summary>
        /// The EVSE used for charging.
        /// </summary>
        [Optional]
        public IEVSE?                       EVSE                         { get; }

        /// <summary>
        /// The identification of the EVSE used for charging.
        /// </summary>
        [Optional]
        public EVSE_Id?                     EVSEId                       { get; }

        /// <summary>
        /// The connector used for charging.
        /// </summary>
        [Optional]
        public IChargingConnector?          ChargingConnector            { get; }

        /// <summary>
        /// The identification of the connector used for charging.
        /// </summary>
        [Optional]
        public ChargingConnector_Id?        ChargingConnectorId          { get; }

        #endregion

        #region Charging product/tariff/price

        /// <summary>
        /// The unique identification of the consumed charging product.
        /// </summary>
        public ChargingProduct_Id?          ChargingProductId            { get; }

        /// <summary>
        /// The consumed charging product.
        /// </summary>
        [Optional]
        public ChargingProduct?             ChargingProduct              { get; }

        /// <summary>
        /// The charging tariff.
        /// </summary>
        public ChargingTariff?              ChargingTariff               { get; }

        /// <summary>
        /// The charging price.
        /// </summary>
        [Optional]
        public Price?                       ChargingPrice                { get; }

        #endregion

        #region Authentication(Start/Stop)

        /// <summary>
        /// The authentication used for starting this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?                 AuthenticationStart    { get; internal set; }

        /// <summary>
        /// The authentication used for stopping this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?                 AuthenticationStop     { get; }

        #endregion

        #region Provider(Id)(Start/Stop)

        /// <summary>
        /// The e-mobility provider used for starting this charging process.
        /// </summary>
        [Optional]
        public IEMobilityProvider?              ProviderStart          { get; internal set; }

        /// <summary>
        /// The identification of the e-mobility provider used for starting this charging process.
        /// </summary>
        [Optional]
        public EMobilityProvider_Id?            ProviderIdStart        { get; internal set; }

        /// <summary>
        /// The identification of the e-mobility provider used for stopping this charging process.
        /// </summary>
        [Optional]
        public IEMobilityProvider?              ProviderStop           { get; internal set; }

        /// <summary>
        /// The e-mobility provider used for stopping this charging process.
        /// </summary>
        [Optional]
        public EMobilityProvider_Id?            ProviderIdStop         { get; internal set; }

        #endregion

        #region RoamingProvider(Id)(Start/Stop)

        /// <summary>
        /// An optional EV roaming provider, e.g. when you want to force the transmission of a CDR via a given roaming network.
        /// </summary>
        public ICSORoamingProvider?             RoamingProviderStart      { get; internal set; }

        /// <summary>
        /// An optional EV roaming provider identification, e.g. when you want to force the transmission of a CDR via a given roaming network.
        /// </summary>
        public CSORoamingProvider_Id?           RoamingProviderIdStart    { get; internal set; }

        /// <summary>
        /// An optional EV roaming provider, e.g. when you want to force the transmission of a CDR via a given roaming network.
        /// </summary>
        public ICSORoamingProvider?             RoamingProviderStop       { get; internal set; }

        /// <summary>
        /// An optional EV roaming provider identification, e.g. when you want to force the transmission of a CDR via a given roaming network.
        /// </summary>
        public CSORoamingProvider_Id?           RoamingProviderIdStop     { get; internal set; }


        ///// <summary>
        ///// An optional EV roaming provider, e.g. when you want to force the transmission of a CDR via a given roaming network.
        ///// </summary>
        //public IEMPRoamingProvider?             EMPRoamingProviderStart      { get; internal set; }

        ///// <summary>
        ///// An optional EV roaming provider identification, e.g. when you want to force the transmission of a CDR via a given roaming network.
        ///// </summary>
        //public EMPRoamingProvider_Id?           EMPRoamingProviderIdStart    { get; internal set; }

        ///// <summary>
        ///// An optional EV roaming provider, e.g. when you want to force the transmission of a CDR via a given roaming network.
        ///// </summary>
        //public IEMPRoamingProvider?             EMPRoamingProviderStop       { get; internal set; }

        ///// <summary>
        ///// An optional EV roaming provider identification, e.g. when you want to force the transmission of a CDR via a given roaming network.
        ///// </summary>
        //public EMPRoamingProvider_Id?           EMPRoamingProviderIdStop     { get; internal set; }

        #endregion

        #region Reservation

        /// <summary>
        /// An optional charging reservation used before charging.
        /// </summary>
        [Optional]
        public ChargingReservation?     Reservation        { get; }

        /// <summary>
        /// An optional charging reservation identification used before charging.
        /// </summary>
        [Optional]
        public ChargingReservation_Id?  ReservationId      { get; }

        /// <summary>
        /// Optional timestamps when the reservation started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime?        ReservationTime    { get; }

        /// <summary>
        /// The optional reservation fee.
        /// </summary>
        [Optional]
        public Price?                   ReservationFee     { get; }

        #endregion

        #region Parking

        /// <summary>
        /// The optional identification of the parking space.
        /// </summary>
        [Optional]
        public ParkingSpace_Id?   ParkingSpaceId    { get; }

        /// <summary>
        /// Optional timestamps when the parking started and ended.
        /// </summary>
        [Optional]
        public StartEndDateTime?  ParkingTime       { get; }

        /// <summary>
        /// The optional fee for parking.
        /// </summary>
        [Optional]
        public Price?             ParkingFee        { get; }

        #endregion

        #region Energy

        /// <summary>
        /// An optional unique identification of the energy meter.
        /// </summary>
        [Optional]
        public EnergyMeter_Id?                              EnergyMeterId           { get; }

        /// <summary>
        /// The optional energy meter.
        /// </summary>
        public EnergyMeter?                                 EnergyMeter             { get; }

        /// <summary>
        /// An optional enumeration of (signed) energy meter values.
        /// </summary>
        [Optional]
        public IEnumerable<EnergyMeteringValue>             EnergyMeteringValues    { get; }

        /// <summary>
        /// The consumed energy in kWh.
        /// </summary>
        public WattHour?                                    ConsumedEnergy          { get; }

        /// <summary>
        /// The optional fee for the consumed energy.
        /// </summary>
        [Optional]
        public Price?                                       ConsumedEnergyFee       { get; }

        #endregion

        #region PublicKey/Signatures

        /// <summary>
        /// The public key used to sign this charge detail record.
        /// Normally this is the public key of the energy meter or charging station.
        /// </summary>
        public ECPublicKeyParameters? PublicKey { get; }

        private readonly HashSet<String> signatures;

        public IEnumerable<String> Signatures
                   => signatures;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charge detail record for the given charging session (identification).
        /// </summary>
        /// <param name="Id">The unique charge detail record identification.</param>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="SessionTime">The timestamps when the charging session started and ended.</param>
        /// <param name="Duration">The duration of the charging session, whenever it is more than the time span between its start- and endtime, e.g. caused by a tariff granularity of 15 minutes.</param>
        /// 
        /// <param name="ChargingConnector">The connector used for charging.</param>
        /// <param name="ChargingConnectorId">The identification of the connector used for charging.</param>
        /// <param name="EVSE">The EVSE used for charging.</param>
        /// <param name="EVSEId">The identification of the EVSE used for charging.</param>
        /// <param name="ChargingStation">The charging station of the charging station used for charging.</param>
        /// <param name="ChargingStationId">The identification of the charging station used for charging.</param>
        /// <param name="ChargingPool">The charging pool of the charging pool used for charging.</param>
        /// <param name="ChargingPoolId">The identification of the charging pool used for charging.</param>
        /// <param name="ChargingStationOperator">The charging station operator used for charging.</param>
        /// <param name="ChargingStationOperatorId">The identification of the charging station operator used for charging.</param>
        /// 
        /// <param name="ChargingProductId">The unique identification of the consumed charging product.</param>
        /// <param name="ChargingProduct">The consumed charging product.</param>
        /// <param name="ChargingPrice">The charging price.</param>
        /// 
        /// <param name="AuthenticationStart">The authentication used for starting this charging process.</param>
        /// <param name="AuthenticationStop">The authentication used for stopping this charging process.</param>
        /// 
        /// <param name="ProviderStart">An optional e-mobility provider used for starting this charging process.</param>
        /// <param name="ProviderIdStart">An optional identification of the e-mobility provider used for starting this charging process.</param>
        /// <param name="ProviderStop">An optional e-mobility provider used for stopping this charging process.</param>
        /// <param name="ProviderIdStop">An optional identification of the e-mobility provider used for stopping this charging process.</param>
        /// 
        /// <param name="CSORoamingProviderStart">An optional EV roaming provider, e.g. when you want to force the transmission of a CDR via a given roaming network.</param>
        /// <param name="CSORoamingProviderIdStart">An optional EV roaming provider identification, e.g. when you want to force the transmission of a CDR via a given roaming network.</param>
        /// <param name="CSORoamingProviderStop">An optional EV roaming provider identification, e.g. when you want to force the transmission of a CDR via a given roaming network.</param>
        /// <param name="CSORoamingProviderIdStop">An optional EV roaming provider, e.g. when you want to force the transmission of a CDR via a given roaming network.</param>
        /// 
        /// <param name="Reservation">An optional charging reservation used before charging.</param>
        /// <param name="ReservationId">An optional charging reservation identification used before charging.</param>
        /// <param name="ReservationTime">Optional timestamps when the reservation started and ended.</param>
        /// <param name="ReservationFee">An optional reservation fee.</param>
        /// 
        /// <param name="ParkingSpaceId">An optional identification of the parking space.</param>
        /// <param name="ParkingTime">Optional timestamps when the parking started and ended.</param>
        /// <param name="ParkingFee">An optional fee for parking.</param>
        /// 
        /// <param name="EnergyMeterId">An optional unique identification of the energy meter.</param>
        /// <param name="EnergyMeter">An optional energy meter.</param>
        /// <param name="EnergyMeteringValues">An optional enumeration of intermediate energy metering values.</param>
        /// <param name="ConsumedEnergy">The consumed energy, whenever it is more than the energy difference between the first and last energy meter value, e.g. caused by a tariff granularity of 1 kWh.</param>
        /// <param name="ConsumedEnergyFee">An optional fee for the consumed energy.</param>
        /// 
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        /// <param name="InternalData">An optional dictionary of internal data.</param>
        /// 
        /// <param name="PublicKey">An optional public key used to sign this charge detail record. Normally this is the public key of the energy meter or charging station.</param>
        /// <param name="Signatures">An optional enumeration of digital signatures.</param>
        /// 
        /// <param name="Created">The creation timestamp of the charge detail record.</param>
        /// <param name="LastChange">The last change timestamp of the charge detail record.</param>
        public ChargeDetailRecord(ChargeDetailRecord_Id              Id,
                                  ChargingSession_Id                 SessionId,
                                  StartEndDateTime                   SessionTime,
                                  TimeSpan?                          Duration                    = null,

                                  IChargingConnector?                ChargingConnector           = null,
                                  ChargingConnector_Id?              ChargingConnectorId         = null,
                                  IEVSE?                             EVSE                        = null,
                                  EVSE_Id?                           EVSEId                      = null,
                                  IChargingStation?                  ChargingStation             = null,
                                  ChargingStation_Id?                ChargingStationId           = null,
                                  IChargingPool?                     ChargingPool                = null,
                                  ChargingPool_Id?                   ChargingPoolId              = null,
                                  IChargingStationOperator?          ChargingStationOperator     = null,
                                  ChargingStationOperator_Id?        ChargingStationOperatorId   = null,

                                  ChargingProduct_Id?                ChargingProductId           = null,
                                  ChargingProduct?                   ChargingProduct             = null,
                                  Price?                             ChargingPrice               = null,

                                  AAuthentication?                   AuthenticationStart         = null,
                                  AAuthentication?                   AuthenticationStop          = null,

                                  IEMobilityProvider?                ProviderStart               = null,
                                  EMobilityProvider_Id?              ProviderIdStart             = null,
                                  IEMobilityProvider?                ProviderStop                = null,
                                  EMobilityProvider_Id?              ProviderIdStop              = null,

                                  ICSORoamingProvider?               CSORoamingProviderStart     = null,
                                  CSORoamingProvider_Id?             CSORoamingProviderIdStart   = null,
                                  ICSORoamingProvider?               CSORoamingProviderStop      = null,
                                  CSORoamingProvider_Id?             CSORoamingProviderIdStop    = null,

                                  ChargingReservation?               Reservation                 = null,
                                  ChargingReservation_Id?            ReservationId               = null,
                                  StartEndDateTime?                  ReservationTime             = null,
                                  Price?                             ReservationFee              = null,

                                  ParkingSpace_Id?                   ParkingSpaceId              = null,
                                  StartEndDateTime?                  ParkingTime                 = null,
                                  Price?                             ParkingFee                  = null,

                                  EnergyMeter_Id?                    EnergyMeterId               = null,
                                  EnergyMeter?                       EnergyMeter                 = null,
                                  IEnumerable<EnergyMeteringValue>?  EnergyMeteringValues        = null,
                                  WattHour?                          ConsumedEnergy              = null,
                                  Price?                             ConsumedEnergyFee           = null,

                                  JObject?                           CustomData                  = null,
                                  UserDefinedDictionary?             InternalData                = null,

                                  ECPublicKeyParameters?             PublicKey                   = null,
                                  IEnumerable<String>?               Signatures                  = null,

                                  DateTime?                          Created                     = null,
                                  DateTime?                          LastChange                  = null)

            : base(CustomData,
                   InternalData,
                   LastChange ?? Timestamp.Now)

        {

            this.Id                          = Id;
            this.SessionId                   = SessionId;
            this.SessionTime                 = SessionTime;
            this.Duration                    = Duration                  ?? SessionTime.Duration;

            this.ChargingConnector           = ChargingConnector;
            this.ChargingConnectorId         = ChargingConnectorId       ?? this.ChargingConnector?.      Id;
            this.EVSE                        = EVSE                      ?? this.ChargingConnector?.      EVSE;
            this.EVSEId                      = EVSEId                    ?? this.EVSE?.                   Id;
            this.ChargingStation             = ChargingStation           ?? this.EVSE?.                   ChargingStation;
            this.ChargingStationId           = ChargingStationId         ?? this.ChargingStation?.        Id;
            this.ChargingPool                = ChargingPool              ?? this.ChargingStation?.        ChargingPool;
            this.ChargingPoolId              = ChargingPoolId            ?? this.ChargingPool?.           Id;
            this.ChargingStationOperator     = ChargingStationOperator   ?? this.ChargingPool?.           Operator;
            this.ChargingStationOperatorId   = ChargingStationOperatorId ?? this.ChargingStationOperator?.Id;

            this.ChargingProductId           = ChargingProductId;
            this.ChargingProduct             = ChargingProduct;
            this.ChargingPrice               = ChargingPrice;

            this.AuthenticationStart         = AuthenticationStart;
            this.AuthenticationStop          = AuthenticationStop;

            this.ProviderStart               = ProviderStart;
            this.ProviderIdStart             = ProviderIdStart;
            this.ProviderStop                = ProviderStop;
            this.ProviderIdStop              = ProviderIdStop;

            this.RoamingProviderStart     = CSORoamingProviderStart;
            this.RoamingProviderIdStart   = CSORoamingProviderIdStart;
            this.RoamingProviderStop      = CSORoamingProviderStop;
            this.RoamingProviderIdStop    = CSORoamingProviderIdStop;

            this.Reservation                 = Reservation;
            this.ReservationId               = ReservationId             ?? Reservation?.Id;
            this.ReservationTime             = ReservationTime;
            this.ReservationFee              = ReservationFee;

            this.ParkingSpaceId              = ParkingSpaceId;
            this.ParkingTime                 = ParkingTime;
            this.ParkingFee                  = ParkingFee;

            this.EnergyMeterId               = EnergyMeterId             ?? EnergyMeter?.Id;
            this.EnergyMeter                 = EnergyMeter;
            this.EnergyMeteringValues        = EnergyMeteringValues is not null
                                                   ? EnergyMeteringValues.OrderBy(energyMeteringValue => energyMeteringValue.Timestamp).ToArray()
                                                   : [];

            this.ConsumedEnergy              = ConsumedEnergy;
            if (this.ConsumedEnergy is null && this.EnergyMeteringValues.Any())
                this.ConsumedEnergy          = this.EnergyMeteringValues.Last().WattHours - this.EnergyMeteringValues.First().WattHours;
            this.ConsumedEnergyFee           = ConsumedEnergyFee;

            this.PublicKey                   = PublicKey;
            this.signatures                  = Signatures is not null && Signatures.Any()
                                                   ? [.. Signatures]
                                                   : [];

            this.Created                     = Created                  ?? Timestamp.Now;

        }

        #endregion


        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out ChargeDetailRecord?  ChargeDetailRecord,
                                       [NotNullWhen(false)] out String?              ErrorResponse)
        {

            ChargeDetailRecord  = null;

            try
            {

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Id                            [mandatory]

                if (!JSON.ParseMandatory("@id",
                                         "timestamp",
                                         ChargeDetailRecord_Id.TryParse,
                                         out ChargeDetailRecord_Id id,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Context                       [mandatory]

                var context = JSON["@context"]?.Value<String>();

                #endregion

                #region Parse SessionId                     [mandatory]

                if (!JSON.ParseMandatory("sessionId",
                                         "session identification",
                                         ChargingSession_Id.TryParse,
                                         out ChargingSession_Id sessionId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse SessionTime                   [mandatory]

                if (!JSON.ParseMandatoryJSON("sessionTime",
                                             "timestamp",
                                             StartEndDateTime.TryParse,
                                             out StartEndDateTime? sessionTime,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Duration                      [mandatory]

                if (!JSON.ParseMandatory("duration",
                                         "session identification",
                                         out TimeSpan duration,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                // reservation
                // reservationId


                #region Parse ChargingStationOperatorId     [optional]

                if (JSON.ParseOptional("chargingStationOperatorId",
                                       "charging station operator identification",
                                       ChargingStationOperator_Id.TryParse,
                                       out ChargingStationOperator_Id? chargingStationOperatorId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingPoolId                [optional]

                if (JSON.ParseOptional("chargingPoolId",
                                       "charging station identification",
                                       ChargingPool_Id.TryParse,
                                       out ChargingPool_Id? chargingPoolId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingStationId             [optional]

                if (JSON.ParseOptional("chargingStationId",
                                       "charging station identification",
                                       ChargingStation_Id.TryParse,
                                       out ChargingStation_Id? chargingStationId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EVSEId                        [optional]

                if (JSON.ParseOptional("evseId",
                                       "EVSE identification",
                                       EVSE_Id.TryParse,
                                       out EVSE_Id? evseId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingConnectorId           [optional]

                if (JSON.ParseOptional("chargingConnectorId",
                                       "charging connector identification",
                                       ChargingConnector_Id.TryParse,
                                       out ChargingConnector_Id? chargingConnectorId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse ChargingProductId             [optional]

                if (JSON.ParseOptional("chargingProductId",
                                       "charging product identification",
                                       ChargingProduct_Id.TryParse,
                                       out ChargingProduct_Id? chargingProductId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingProduct               [optional]

                if (JSON.ParseOptionalJSON("chargingProduct",
                                           "charging product",
                                           WWCP.ChargingProduct.TryParse,
                                           out ChargingProduct? chargingProduct,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingPrice                 [optional]

                if (JSON.ParseOptionalJSON("chargingPrice",
                                           "charging price",
                                           Price.TryParse,
                                           out Price chargingPrice,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse AuthenticationStart           [optional]

                if (JSON.ParseOptionalJSON("authenticationStart",
                                           "authentication start",
                                           AAuthentication.TryParse,
                                           out AAuthentication? authenticationStart,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AuthenticationStop            [optional]

                if (JSON.ParseOptionalJSON("authenticationStop",
                                           "authentication stop",
                                           AAuthentication.TryParse,
                                           out AAuthentication? authenticationStop,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse ProviderIdStart               [optional]

                if (JSON.ParseOptional("providerIdStart",
                                       "provider identification start",
                                       EMobilityProvider_Id.TryParse,
                                       out EMobilityProvider_Id? providerIdStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ProviderIdStop                [optional]

                if (JSON.ParseOptional("providerIdStop",
                                       "provider identification stop",
                                       EMobilityProvider_Id.TryParse,
                                       out EMobilityProvider_Id? providerIdStop,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse RoamingProviderIdStart        [optional]

                if (JSON.ParseOptional("roamingProviderIdStart",
                                       "roaming provider start identification",
                                       CSORoamingProvider_Id.TryParse,
                                       out CSORoamingProvider_Id? roamingProviderIdStart,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse RoamingProviderIdStop         [optional]

                if (JSON.ParseOptional("roamingProviderIdStop",
                                       "roaming provider stop identification",
                                       CSORoamingProvider_Id.TryParse,
                                       out CSORoamingProvider_Id? roamingProviderIdStop,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse EnergyMeterId                 [optional]

                if (JSON.ParseOptional("energyMeterId",
                                       "energy meter identification",
                                       EnergyMeter_Id.TryParse,
                                       out EnergyMeter_Id? energyMeterId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EnergyMeter                   [optional]

                if (JSON.ParseOptionalJSON("energyMeter",
                                           "energy meter",
                                           WWCP.EnergyMeter.TryParse,
                                           out EnergyMeter? energyMeter,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EnergyMeteringValues          [optional]

                if (JSON.ParseOptionalJSON("energyMeteringValues",
                                           "energy metering values",
                                           EnergyMeteringValue.TryParse,
                                           out IEnumerable<EnergyMeteringValue>? energyMeteringValues,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse Created                       [optional]

                if (JSON.ParseOptional("created",
                                       "created timestamp",
                                       out DateTime? created,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse LastChange                    [optional]

                if (JSON.ParseOptional("lastChange",
                                       "last change timestamp",
                                       out DateTime? lastChange,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ChargeDetailRecord = new ChargeDetailRecord(

                                         id,
                                         sessionId,
                                         sessionTime,
                                         duration,

                                         null, //chargingConnector,
                                         chargingConnectorId,
                                         null, //evse,
                                         evseId,
                                         null, //chargingStation,
                                         chargingStationId,
                                         null, //chargingPool,
                                         chargingPoolId,
                                         null, //chargingStationOperator,
                                         chargingStationOperatorId,

                                         chargingProductId,
                                         chargingProduct,
                                         chargingPrice,

                                         authenticationStart,
                                         authenticationStop,

                                         null, //providerStart,
                                         providerIdStart,
                                         null, //providerStop,
                                         providerIdStop,

                                         null, //roamingProviderStart,
                                         roamingProviderIdStart,
                                         null, //roamingProviderStop,
                                         roamingProviderIdStop,

                                         null, //reservation,
                                         null, //reservationId,
                                         null, //reservationTime,
                                         null, //reservationFee,

                                         null, //parkingSpaceId,
                                         null, //parkingTime,
                                         null, //parkingFee,

                                         energyMeterId,
                                         energyMeter,
                                         energyMeteringValues,
                                         null, //consumedEnergy,
                                         null, //consumedEnergyFee,

                                         null, //customData,
                                         null, //internalData,

                                         null, //publicKey,
                                         null, //signatures,

                                         created,
                                         lastChange

                                     );

                return true;

            }
            catch (Exception e)
            {
                ChargeDetailRecord  = null;
                ErrorResponse       = "The given JSON representation of a charge detail record is invalid: " + e.Message;
            }

            return false;

        }

        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charge detail record.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A custom charge detail record serializer.</param>
        /// <param name="CustomAAuthenticationSerializer">A custom AAuthentication JSON serializer.</param>
        /// <param name="CustomChargingProductSerializer">A custom ChargingProduct JSON serializer.</param>
        /// <param name="CustomPriceSerializer">A custom Price JSON serializer.</param>
        /// <param name="CustomEnergyMeterSerializer">A custom energy meter JSON serializer.</param>
        /// <param name="CustomTransparencySoftwareStatusSerializer">A custom transparency software status JSON serializer.</param>
        /// <param name="CustomTransparencySoftwareSerializer">A custom transparency software JSON serializer.</param>
        /// <param name="CustomEnergyMeteringValueSerializer">A custom energy metering value serializer.</param>
        public JObject ToJSON(Boolean                                                       Embedded                                     = false,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?          CustomChargeDetailRecordSerializer           = null,
                              CustomJObjectSerializerDelegate<AAuthentication>?             CustomAAuthenticationSerializer              = null,
                              CustomJObjectSerializerDelegate<ChargingProduct>?             CustomChargingProductSerializer              = null,
                              CustomJObjectSerializerDelegate<Price>?                       CustomPriceSerializer                        = null,
                              CustomJObjectSerializerDelegate<IEnergyMeter>?                CustomEnergyMeterSerializer                  = null,
                              CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?  CustomTransparencySoftwareStatusSerializer   = null,
                              CustomJObjectSerializerDelegate<TransparencySoftware>?        CustomTransparencySoftwareSerializer         = null,
                              CustomJObjectSerializerDelegate<EnergyMeteringValue>?         CustomEnergyMeteringValueSerializer          = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("@id",                         Id.                             ToString()),
                                 new JProperty("sessionId",                   SessionId.                      ToString()),

                           Embedded
                               ? null
                               : new JProperty("@context",                    JSONLDContext),

                           SessionTime is not null
                               ? new JProperty("sessionTime",                 SessionTime.                    ToJSON(Embedded: true))
                               : null,

                           Duration.HasValue
                               ? new JProperty("duration",                    Duration.                 Value.TotalSeconds)
                               : null,


                           Reservation is not null
                               ? new JProperty("reservation",                 JSONObject.Create(
                                                                                  new JProperty("reservationId",  Reservation.Id.       ToString()),
                                                                                  new JProperty("startTime",      Reservation.StartTime.ToISO8601()),
                                                                                  new JProperty("duration",       Reservation.ConsumedReservationTime.TotalSeconds)
                                                                              ))
                               : ReservationId.HasValue
                                     ? new JProperty("reservationId",         ReservationId.            Value.ToString())
                                     : null,


                           //ReservationTime
                           //ReservationFee


                           AuthenticationStart is not null
                               ? new JProperty("authenticationStart",         AuthenticationStart.            ToJSON(CustomAAuthenticationSerializer))
                               : null,

                           AuthenticationStop is not null
                               ? new JProperty("authenticationStop",          AuthenticationStop.             ToJSON(CustomAAuthenticationSerializer))
                               : null,


                           //RoamingProviderStart
                           RoamingProviderIdStart.   HasValue
                               ? new JProperty("roamingProviderIdStart",      RoamingProviderIdStart.   Value.ToString())
                               : null,

                           //RoamingProviderStop
                           RoamingProviderIdStop.    HasValue
                               ? new JProperty("roamingProviderIdStop",       RoamingProviderIdStop.    Value.ToString())
                               : null,


                           //ProviderStart
                           ProviderIdStart.          HasValue
                               ? new JProperty("providerIdStart",             ProviderIdStart.          Value.ToString())
                               : null,

                           //ProviderStop
                           ProviderIdStop.           HasValue
                               ? new JProperty("providerIdStop",              ProviderIdStop.           Value.ToString())
                               : null,



                           //ChargingStationOperator
                           ChargingStationOperatorId.HasValue
                               ? new JProperty("chargingStationOperatorId",   ChargingStationOperatorId.Value.ToString())
                               : null,

                           //ChargingPool
                           ChargingPoolId.           HasValue
                               ? new JProperty("chargingPoolId",              ChargingPoolId.           Value.ToString())
                               : null,

                           //ChargingStation
                           ChargingStationId.        HasValue
                               ? new JProperty("chargingStationId",           ChargingStationId.        Value.ToString())
                               : null,

                           //EVSE
                           EVSEId.                   HasValue
                               ? new JProperty("evseId",                      EVSEId.                   Value.ToString())
                               : null,

                           //ChargingConnector
                           ChargingConnectorId.      HasValue
                               ? new JProperty("chargingConnectorId",         ChargingConnectorId.      Value.ToString())
                               : null,



                           ChargingProductId.        HasValue
                               ? new JProperty("chargingProductId",           ChargingProductId.        Value.ToString())
                               : null,

                           ChargingProduct is not null
                               ? new JProperty("chargingProduct",             ChargingProduct.                ToJSON(Embedded: true,
                                                                                                                     CustomChargingProductSerializer))
                               : null,

                           ChargingPrice.            HasValue
                               ? new JProperty("chargingPrice",               ChargingPrice.Value.            ToJSON(CustomPriceSerializer))
                               : null,



                           //ParkingSpaceId
                           //ParkingTime
                           //ParkingFee



                           EnergyMeterId.HasValue && EnergyMeter is null
                               ? new JProperty("energyMeterId",               EnergyMeterId.                      ToString())
                               : null,

                           EnergyMeter is not null
                               ? new JProperty("energyMeter",                 EnergyMeter.                        ToJSON(Embedded: true,
                                                                                                                         CustomEnergyMeterSerializer,
                                                                                                                         CustomTransparencySoftwareStatusSerializer,
                                                                                                                         CustomTransparencySoftwareSerializer))
                               : null,

                           EnergyMeteringValues is not null && EnergyMeteringValues.Any()
                               ? new JProperty("energyMeteringValues",        new JArray(
                                                                                  EnergyMeteringValues.Select(energyMeteringValue => energyMeteringValue.ToJSON(
                                                                                                                                         Embedded: true,
                                                                                                                                         CustomEnergyMeteringValueSerializer
                                                                                                                                     ))
                                                                              ))
                               : null,


                           //ConsumedEnergy
                           //ConsumedEnergyFee

                           //CustomData
                           //InternalData

                           //PublicKey
                           //Signatures

                           new JProperty("created",                           Created.                            ToISO8601()),
                           new JProperty("lastChange",                        LastChangeDate.                     ToISO8601())

                       );

            return CustomChargeDetailRecordSerializer is not null
                       ? CustomChargeDetailRecordSerializer(this, json)
                       : json;

        }

        #endregion

        #region ToSignedJSON(SecretKey, Passphrase)

        public JObject ToSignedJSON(PgpSecretKey SecretKey, String Passphrase)
        {

            var SignatureGenerator = new PgpSignatureGenerator(SecretKey.PublicKey.Algorithm,
                                                               HashAlgorithmTag.Sha512);

            SignatureGenerator.InitSign(PgpSignature.BinaryDocument,
                                        SecretKey.ExtractPrivateKey(Passphrase.ToCharArray()));

            var JSON             = ToJSON(false);
            var JSONBlob         = JSON.ToString(Newtonsoft.Json.Formatting.None).ToUTF8Bytes();

            SignatureGenerator.Update(JSONBlob, 0, JSONBlob.Length);

            var SignatureGen     = SignatureGenerator.Generate();
            var OutputStream     = new MemoryStream();
            var SignatureStream  = new BcpgOutputStream(new ArmoredOutputStream(OutputStream));

            SignatureGen.Encode(SignatureStream);

            SignatureStream.Flush();
            SignatureStream.Close();

            OutputStream.Flush();
            OutputStream.Close();

            var Signature        = OutputStream.ToArray().ToHexString();
            signatures.Add(Signature);

            JSON["signature"] = Signature;

            return JSON;

        }

        #endregion




        #region Operator overloading

        #region Operator == (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)
        {

            if (ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
                return true;

            if (ChargeDetailRecord1 is null || ChargeDetailRecord2 is null)
                return false;

            return ChargeDetailRecord1.Equals(ChargeDetailRecord2);

        }

        #endregion

        #region Operator != (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 == ChargeDetailRecord2);

        #endregion

        #region Operator <  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargeDetailRecord ChargeDetailRecord1,
                                          ChargeDetailRecord ChargeDetailRecord2)
        {

            if (ChargeDetailRecord1 is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given charge detail record must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) < 0;

        }

        #endregion

        #region Operator <= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 > ChargeDetailRecord2);

        #endregion

        #region Operator >  (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargeDetailRecord ChargeDetailRecord1,
                                          ChargeDetailRecord ChargeDetailRecord2)
        {

            if (ChargeDetailRecord1 is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord1), "The given charge detail record must not be null!");

            return ChargeDetailRecord1.CompareTo(ChargeDetailRecord2) > 0;

        }

        #endregion

        #region Operator >= (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord1">A charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargeDetailRecord ChargeDetailRecord1,
                                           ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 < ChargeDetailRecord2);

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargeDetailRecord chargeDetailRecord
                   ? CompareTo(chargeDetailRecord)
                   : throw new ArgumentException("The given object is not a charge detail record!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord? ChargeDetailRecord)
        {

            if (ChargeDetailRecord is null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord), "The given charge detail record must not be null!");

            var c = Id.       CompareTo(ChargeDetailRecord.Id);

            if (c == 0)
                c = SessionId.CompareTo(ChargeDetailRecord.SessionId);

            //if (c == 0)
            //    c = EVSEId.        CompareTo(ChargeDetailRecord.EVSEId);

            //if (c == 0)
            //    c = Identification.CompareTo(ChargeDetailRecord.Identification);

            //if (c == 0)
            //    c = SessionStart.  CompareTo(ChargeDetailRecord.SessionStart);

            //if (c == 0)
            //    c = SessionEnd.    CompareTo(ChargeDetailRecord.SessionEnd);

            //if (c == 0)
            //    c = ChargingStart. CompareTo(ChargeDetailRecord.ChargingStart);

            //if (c == 0)
            //    c = ChargingEnd.   CompareTo(ChargeDetailRecord.ChargingEnd);

            //if (c == 0)
            //    c = ConsumedEnergy.CompareTo(ChargeDetailRecord.ConsumedEnergy);


            //if (c == 0 && PartnerProductId.   HasValue && ChargeDetailRecord.PartnerProductId.   HasValue)
            //    c = PartnerProductId.   Value.CompareTo(ChargeDetailRecord.PartnerProductId.   Value);

            //if (c == 0 && CPOPartnerSessionId.HasValue && ChargeDetailRecord.CPOPartnerSessionId.HasValue)
            //    c = CPOPartnerSessionId.Value.CompareTo(ChargeDetailRecord.CPOPartnerSessionId.Value);

            //if (c == 0 && EMPPartnerSessionId.HasValue && ChargeDetailRecord.EMPPartnerSessionId.HasValue)
            //    c = EMPPartnerSessionId.Value.CompareTo(ChargeDetailRecord.EMPPartnerSessionId.Value);

            //if (c == 0 && MeterValueStart.    HasValue && ChargeDetailRecord.MeterValueStart.    HasValue)
            //    c = MeterValueStart.    Value.CompareTo(ChargeDetailRecord.MeterValueStart.    Value);

            //if (c == 0 && MeterValueEnd.      HasValue && ChargeDetailRecord.MeterValueEnd.      HasValue)
            //    c = MeterValueEnd.      Value.CompareTo(ChargeDetailRecord.MeterValueEnd.      Value);

            //// MeterValuesInBetween

            //// SignedMeteringValues

            //if (c == 0 && CalibrationLawVerificationInfo is not null && ChargeDetailRecord.CalibrationLawVerificationInfo is not null)
            //    c = CalibrationLawVerificationInfo.CompareTo(ChargeDetailRecord.CalibrationLawVerificationInfo);

            //if (c == 0 && HubOperatorId.HasValue && ChargeDetailRecord.HubOperatorId.HasValue)
            //    c = HubOperatorId.Value.CompareTo(ChargeDetailRecord.HubOperatorId.Value);

            //if (c == 0 && HubProviderId.HasValue && ChargeDetailRecord.HubProviderId.HasValue)
            //    c = HubProviderId.Value.CompareTo(ChargeDetailRecord.HubProviderId.Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ChargeDetailRecord chargeDetailRecord &&
                   Equals(chargeDetailRecord);

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord? ChargeDetailRecord)

            => ChargeDetailRecord is not null &&

               Id.            Equals(ChargeDetailRecord.Id)        &&
               SessionId.     Equals(ChargeDetailRecord.SessionId) &&
               SessionTime.   Equals(ChargeDetailRecord.SessionTime);
             //  EVSEId.        Equals(ChargeDetailRecord.EVSEId)         &&
             //  Identification.Equals(ChargeDetailRecord.Identification) &&
             //  SessionStart.  Equals(ChargeDetailRecord.SessionStart)   &&
             //  SessionEnd.    Equals(ChargeDetailRecord.SessionEnd)     &&
             //  ChargingStart. Equals(ChargeDetailRecord.ChargingStart)  &&
             //  ChargingEnd.   Equals(ChargeDetailRecord.ChargingEnd)    &&
             //  ConsumedEnergy.Equals(ChargeDetailRecord.ConsumedEnergy) &&

             //((!PartnerProductId.   HasValue && !ChargeDetailRecord.PartnerProductId.   HasValue) ||
             //  (PartnerProductId.   HasValue &&  ChargeDetailRecord.PartnerProductId.   HasValue && PartnerProductId.   Value.Equals(ChargeDetailRecord.PartnerProductId.   Value))) &&

             //((!CPOPartnerSessionId.HasValue && !ChargeDetailRecord.CPOPartnerSessionId.HasValue) ||
             //  (CPOPartnerSessionId.HasValue &&  ChargeDetailRecord.CPOPartnerSessionId.HasValue && CPOPartnerSessionId.Value.Equals(ChargeDetailRecord.CPOPartnerSessionId.Value))) &&

             //((!EMPPartnerSessionId.HasValue && !ChargeDetailRecord.EMPPartnerSessionId.HasValue) ||
             //  (EMPPartnerSessionId.HasValue &&  ChargeDetailRecord.EMPPartnerSessionId.HasValue && EMPPartnerSessionId.Value.Equals(ChargeDetailRecord.EMPPartnerSessionId.Value))) &&

             //((!MeterValueStart.    HasValue && !ChargeDetailRecord.MeterValueStart.    HasValue) ||
             //  (MeterValueStart.    HasValue &&  ChargeDetailRecord.MeterValueStart.    HasValue && MeterValueStart.    Value.Equals(ChargeDetailRecord.MeterValueStart.    Value))) &&

             //((!MeterValueEnd.      HasValue && !ChargeDetailRecord.MeterValueEnd.      HasValue) ||
             //  (MeterValueEnd.      HasValue &&  ChargeDetailRecord.MeterValueEnd.      HasValue && MeterValueEnd.      Value.Equals(ChargeDetailRecord.MeterValueEnd.      Value))) &&

             // ((MeterValuesInBetween is     null && ChargeDetailRecord.MeterValuesInBetween is     null) ||
             //  (MeterValuesInBetween is not null && ChargeDetailRecord.MeterValuesInBetween is not null &&
             //   MeterValuesInBetween.Count().Equals(ChargeDetailRecord.MeterValuesInBetween.Count()) &&
             //   MeterValuesInBetween.All(meterValue => ChargeDetailRecord.MeterValuesInBetween.Contains(meterValue)))) &&

             // ((SignedMeteringValues is     null  &&  ChargeDetailRecord.SignedMeteringValues is     null) ||
             //  (SignedMeteringValues is not null  &&  ChargeDetailRecord.SignedMeteringValues is not null  && SignedMeteringValues.    Equals(ChargeDetailRecord.SignedMeteringValues))) &&

             // ((CalibrationLawVerificationInfo is     null && ChargeDetailRecord.CalibrationLawVerificationInfo is     null) ||
             //  (CalibrationLawVerificationInfo is not null && ChargeDetailRecord.CalibrationLawVerificationInfo is not null &&
             //   CalibrationLawVerificationInfo.CompareTo(ChargeDetailRecord.CalibrationLawVerificationInfo) != 0)) &&

             //((!HubOperatorId.      HasValue && !ChargeDetailRecord.HubOperatorId.      HasValue) ||
             //  (HubOperatorId.      HasValue &&  ChargeDetailRecord.HubOperatorId.      HasValue && HubOperatorId.      Value.Equals(ChargeDetailRecord.HubOperatorId.      Value))) &&

             //((!HubProviderId.      HasValue && !ChargeDetailRecord.HubProviderId.      HasValue) ||
             //  (HubProviderId.      HasValue &&  ChargeDetailRecord.HubProviderId.      HasValue && HubProviderId.      Value.Equals(ChargeDetailRecord.HubProviderId.      Value)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Id.         GetHashCode() * 5 ^
                       SessionId.  GetHashCode() * 3 ^
                       SessionTime.GetHashCode();

            }
        }

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
