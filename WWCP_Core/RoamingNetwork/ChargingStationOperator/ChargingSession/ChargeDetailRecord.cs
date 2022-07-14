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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using System.IO;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Parameters;
using org.GraphDefined.Vanaheimr.Hermod.JSON;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate for filtering charge detail records.
    /// </summary>
    /// <param name="ChargeDetailRecord">A charge detail record to filter.</param>
    public delegate ChargeDetailRecordFilters ChargeDetailRecordFilterDelegate(ChargeDetailRecord ChargeDetailRecord);


    /// <summary>
    /// A charge detail record for a charging session.
    /// </summary>
    public class ChargeDetailRecord : AEMobilityEntity<ChargeDetailRecord_Id>,
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
        /// The duration of the charging session, whenever it is more
        /// than the time span between its start- and endtime, e.g.
        /// caused by a tariff granularity of 15 minutes.
        /// </summary>
        [Optional]
        public TimeSpan?                Duration        { get; }

        #endregion

        #region Location / Operator

        /// <summary>
        /// The EVSE used for charging.
        /// </summary>
        [Optional]
        public EVSE?                        EVSE                         { get; }

        /// <summary>
        /// The identification of the EVSE used for charging.
        /// </summary>
        [Optional]
        public EVSE_Id?                     EVSEId                       { get; }

        /// <summary>
        /// The charging station of the charging station used for charging.
        /// </summary>
        [Optional]
        public ChargingStation?             ChargingStation              { get; }

        /// <summary>
        /// The identification of the charging station used for charging.
        /// </summary>
        [Optional]
        public ChargingStation_Id?          ChargingStationId            { get; }

        /// <summary>
        /// The charging pool of the charging pool used for charging.
        /// </summary>
        [Optional]
        public ChargingPool?                ChargingPool                 { get; }

        /// <summary>
        /// The identification of the charging pool used for charging.
        /// </summary>
        [Optional]
        public ChargingPool_Id?             ChargingPoolId               { get; }

        /// <summary>
        /// The charging station operator used for charging.
        /// </summary>
        [Optional]
        public ChargingStationOperator?     ChargingStationOperator      { get; }

        /// <summary>
        /// The identification of the charging station operator used for charging.
        /// </summary>
        [Optional]
        public ChargingStationOperator_Id?  ChargingStationOperatorId    { get; }

        #endregion

        #region Charging product/tariff/price

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
        public Decimal?                     ChargingPrice                { get; }

        #endregion

        #region Authentication

        /// <summary>
        /// The authentication used for starting this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?                 AuthenticationStart    { get; }

        /// <summary>
        /// The authentication used for stopping this charging process.
        /// </summary>
        [Optional]
        public AAuthentication?                 AuthenticationStop     { get; }

        /// <summary>
        /// The identification of the e-mobility provider used for starting this charging process.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id?            ProviderIdStart        { get; }

        /// <summary>
        /// The identification of the e-mobility provider used for stopping this charging process.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id?            ProviderIdStop         { get; }

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

        #endregion

        #region Parking

        /// <summary>
        /// The optional identification of the parkging space.
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
        public Decimal?           ParkingFee        { get; }

        #endregion

        #region Energy

        /// <summary>
        /// An optional unique identification of the energy meter.
        /// </summary>
        [Optional]
        public EnergyMeter_Id?                              EnergyMeterId           { get; }

        /// <summary>
        /// An optional enumeration of energy meter values.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<Decimal>>?           EnergyMeteringValues    { get; }

        /// <summary>
        /// An optional enumeration of energy metering values with digital signatures per measurement.
        /// </summary>
        [Optional]
        public IEnumerable<SignedMeteringValue<Decimal>>?   SignedMeteringValues    { get; }

        /// <summary>
        /// The consumed energy in kWh.
        /// </summary>
        public Decimal?                                     ConsumedEnergy          { get; }

        #endregion


        public ECPublicKeyParameters? PublicKey { get; }

        private readonly HashSet<String> _Signatures;

        public IEnumerable<String> Signatures
                   => _Signatures;

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
        /// <param name="EVSE">The EVSE used for charging.</param>
        /// <param name="EVSEId">The identification of the EVSE used for charging.</param>
        /// <param name="ChargingStation">The charging station of the charging station used for charging.</param>
        /// <param name="ChargingStationId">The identification of the charging station used for charging.</param>
        /// <param name="ChargingPool">The charging pool of the charging pool used for charging.</param>
        /// <param name="ChargingPoolId">The identification of the charging pool used for charging.</param>
        /// <param name="ChargingStationOperator">The charging station operator used for charging.</param>
        /// <param name="ChargingStationOperatorId">The identification of the charging station operator used for charging.</param>
        /// <param name="ChargingProduct">The consumed charging product.</param>
        /// <param name="ChargingPrice">The charging price.</param>
        /// 
        /// <param name="AuthenticationStart">The authentication used for starting this charging process.</param>
        /// <param name="AuthenticationStop">The authentication used for stopping this charging process.</param>
        /// <param name="ProviderIdStart">The identification of the e-mobility provider used for starting this charging process.</param>
        /// <param name="ProviderIdStop">The identification of the e-mobility provider used for stopping this charging process.</param>
        /// 
        /// <param name="Reservation">The optional charging reservation used before charging.</param>
        /// <param name="ReservationId">The optional charging reservation identification used before charging.</param>
        /// <param name="ReservationTime">Optional timestamps when the reservation started and ended.</param>
        /// 
        /// <param name="ParkingSpaceId">The optional identification of the parkging space.</param>
        /// <param name="ParkingTime">Optional timestamps when the parking started and ended.</param>
        /// <param name="ParkingFee">The optional fee for parking.</param>
        /// 
        /// <param name="EnergyMeterId">An optional unique identification of the energy meter.</param>
        /// <param name="EnergyMeteringValues">An optional enumeration of intermediate energy metering values.</param>
        /// <param name="ConsumedEnergy">The consumed energy, whenever it is more than the energy difference between the first and last energy meter value, e.g. caused by a tariff granularity of 1 kWh.</param>
        /// 
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargeDetailRecord(ChargeDetailRecord_Id                       Id,
                                  ChargingSession_Id                          SessionId,
                                  StartEndDateTime                            SessionTime,
                                  TimeSpan?                                   Duration                    = null,

                                  EVSE?                                       EVSE                        = null,
                                  EVSE_Id?                                    EVSEId                      = null,
                                  ChargingStation?                            ChargingStation             = null,
                                  ChargingStation_Id?                         ChargingStationId           = null,
                                  ChargingPool?                               ChargingPool                = null,
                                  ChargingPool_Id?                            ChargingPoolId              = null,
                                  ChargingStationOperator?                    ChargingStationOperator     = null,
                                  ChargingStationOperator_Id?                 ChargingStationOperatorId   = null,
                                  ChargingProduct?                            ChargingProduct             = null,
                                  Decimal?                                    ChargingPrice               = null,

                                  AAuthentication?                            AuthenticationStart         = null,
                                  AAuthentication?                            AuthenticationStop          = null,
                                  eMobilityProvider_Id?                       ProviderIdStart             = null,
                                  eMobilityProvider_Id?                       ProviderIdStop              = null,

                                  ChargingReservation?                        Reservation                 = null,
                                  ChargingReservation_Id?                     ReservationId               = null,
                                  StartEndDateTime?                           ReservationTime             = null,

                                  ParkingSpace_Id?                            ParkingSpaceId              = null,
                                  StartEndDateTime?                           ParkingTime                 = null,
                                  Decimal?                                    ParkingFee                  = null,

                                  EnergyMeter_Id?                             EnergyMeterId               = null,
                                  IEnumerable<Timestamped<Decimal>>?          EnergyMeteringValues        = null,
                                  IEnumerable<SignedMeteringValue<Decimal>>?  SignedMeteringValues        = null,
                                  Decimal?                                    ConsumedEnergy              = null,

                                  IReadOnlyDictionary<String, Object>?        CustomData                  = null,

                                  ECPublicKeyParameters?                      PublicKey                   = null,
                                  IEnumerable<String>?                        Signatures                  = null)

            : base(Id,
                   CustomData)

        {

            this.SessionId                   = SessionId;
            this.SessionTime                 = SessionTime;
            this.Duration                    = Duration;

            this.EVSE                        = EVSE;
            this.EVSEId                      = EVSEId                    ?? EVSE?.Id;
            this.ChargingStation             = ChargingStation;
            this.ChargingStationId           = ChargingStationId         ?? ChargingStation?.Id;
            this.ChargingPool                = ChargingPool;
            this.ChargingPoolId              = ChargingPoolId            ?? ChargingPool?.Id;
            this.ChargingStationOperator     = ChargingStationOperator;
            this.ChargingStationOperatorId   = ChargingStationOperatorId ?? ChargingStationOperator?.Id;
            this.ChargingProduct             = ChargingProduct;
            this.ChargingPrice               = ChargingPrice;

            this.AuthenticationStart         = AuthenticationStart;
            this.AuthenticationStop          = AuthenticationStop;
            this.ProviderIdStart             = ProviderIdStart;
            this.ProviderIdStop              = ProviderIdStop;

            this.Reservation                 = Reservation;
            this.ReservationId               = ReservationId             ?? Reservation?.Id;
            this.ReservationTime             = ReservationTime;

            this.ParkingSpaceId              = ParkingSpaceId;
            this.ParkingTime                 = ParkingTime;
            this.ParkingFee                  = ParkingFee;

            this.EnergyMeterId               = EnergyMeterId;
            this.EnergyMeteringValues        = EnergyMeteringValues != null ? EnergyMeteringValues.OrderBy(mv  => mv. Timestamp).ToArray() : Array.Empty<Timestamped<Decimal>>();
            this.SignedMeteringValues        = SignedMeteringValues != null ? SignedMeteringValues.OrderBy(smv => smv.Timestamp).ToArray() : Array.Empty<SignedMeteringValue<Decimal>>();
            if (SignedMeteringValues.SafeAny() && !EnergyMeteringValues.SafeAny())
                this.EnergyMeteringValues    = SignedMeteringValues is not null
                                                   ? SignedMeteringValues.Select(svalue => new Timestamped<Decimal>(svalue.Timestamp,
                                                                                                                    svalue.MeterValue))
                                                   : Array.Empty<Timestamped<Decimal>>();

            this.ConsumedEnergy              = ConsumedEnergy;
            if (this.ConsumedEnergy == null && this.EnergyMeteringValues.SafeAny())
                this.ConsumedEnergy          = this.EnergyMeteringValues.Last().Value - this.EnergyMeteringValues.First().Value; // kWh

            this.PublicKey                   = PublicKey;
            this._Signatures                 = Signatures is not null && Signatures.Any()
                                                   ? new HashSet<String>(Signatures)
                                                   : new HashSet<String>();

        }

        #endregion


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given charge detail record.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure.</param>
        /// <param name="CustomChargeDetailRecordSerializer">A custom charge detail record serializer.</param>
        public JObject ToJSON(Boolean                                           Embedded                             = false,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null)
        {

            var JSON = JSONObject.Create(

                           SessionId.ToJSON("@id"),

                           Embedded
                               ? null
                               : new JProperty("@context",                    JSONLDContext),

                           SessionTime != null
                               ? new JProperty("sessionTime",                 JSONObject.Create(
                                     new JProperty("start",                   SessionTime.StartTime.ToIso8601()),
                                     SessionTime.EndTime.HasValue
                                         ? new JProperty("end",               SessionTime.EndTime.Value.ToIso8601())
                                         : null
                                 ))
                               : null,

                           Duration.HasValue
                               ? new JProperty("duration",                    Duration.Value.TotalSeconds)
                               : null,


                           //new JProperty("meterId",                           MeterId.ToString()),

                           EnergyMeteringValues.SafeAny()
                               ? new JProperty("meterValues", JSONArray.Create(
                                       EnergyMeteringValues.Select(meterValue => JSONObject.Create(
                                           new JProperty("timestamp",  meterValue.Timestamp.ToIso8601()),
                                           new JProperty("value",      meterValue.Value)
                                       ))
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
                               ? new JProperty("evseId",                      EVSEId.                   ToString())
                               : null,


                           ChargingProduct != null
                               ? new JProperty("chargingProduct",             ChargingProduct.ToJSON())
                               : null


                       //new JProperty("userId",         UserId),
                       //new JProperty("publicKey",      PublicKey.KeyId),
                       //new JProperty("lastSignature",  lastSignature),
                       //new JProperty("signature",      Signature)
                       );

            return CustomChargeDetailRecordSerializer != null
                       ? CustomChargeDetailRecordSerializer(this, JSON)
                       : JSON;

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
            _Signatures.Add(Signature);

            JSON["signature"] = Signature;

            return JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargeDetailRecord1 == null) || ((Object) ChargeDetailRecord2 == null))
                return false;

            return ChargeDetailRecord1.Equals(ChargeDetailRecord2);

        }

        #endregion

        #region Operator != (ChargeDetailRecord1, ChargeDetailRecord2)

        /// <summary>
        /// Compares two charge detail records for inequality.
        /// </summary>
        /// <param name="ChargeDetailRecord1">An charge detail record.</param>
        /// <param name="ChargeDetailRecord2">Another charge detail record.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargeDetailRecord ChargeDetailRecord1, ChargeDetailRecord ChargeDetailRecord2)

            => !(ChargeDetailRecord1 == ChargeDetailRecord2);

        #endregion

        #endregion

        #region IComparable<ChargeDetailRecord> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charge detail record.
            var ChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentException("The given object is not a charge detail record!");

            return CompareTo(ChargeDetailRecord);

        }

        #endregion

        #region CompareTo(ChargeDetailRecord)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record object to compare with.</param>
        public Int32 CompareTo(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                throw new ArgumentNullException("The given charge detail record must not be null!");

            return SessionId.CompareTo(ChargeDetailRecord.SessionId);

        }

        #endregion

        #endregion

        #region IEquatable<ChargeDetailRecord> Members

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

            // Check if the given object is a charge detail record.
            var ChargeDetailRecord = Object as ChargeDetailRecord;
            if ((Object) ChargeDetailRecord == null)
                return false;

            return this.Equals(ChargeDetailRecord);

        }

        #endregion

        #region Equals(ChargeDetailRecord)

        /// <summary>
        /// Compares two charge detail records for equality.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargeDetailRecord ChargeDetailRecord)
        {

            if ((Object) ChargeDetailRecord == null)
                return false;

            return SessionId.Equals(ChargeDetailRecord.SessionId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => SessionId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => SessionId.ToString();

        #endregion

    }

}
