/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charge detail record for a charging session.
    /// </summary>
    public class ChargeDetailRecord : ACustomData,
                                      IEquatable <ChargeDetailRecord>,
                                      IComparable<ChargeDetailRecord>,
                                      IComparable
    {

        #region Properties

        #region Session

        /// <summary>
        /// The unique charging session identification.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id  SessionId     { get; }

        /// <summary>
        /// The timestamps when the charging session started and ended.
        /// </summary>
        [Mandatory]
        public StartEndDateTime?   SessionTime   { get; }


        /// <summary>
        /// The duration of the charging session, whenever it is more
        /// than the time span between its start- and endtime, e.g.
        /// caused by a tariff granularity of 15 minutes.
        /// </summary>
        [Optional]
        public TimeSpan?           Duration      { get; }

        #endregion

        #region Location / Product

        /// <summary>
        /// The EVSE used for charging.
        /// </summary>
        [Optional]
        public EVSE                         EVSE                         { get; }

        /// <summary>
        /// The identification of the EVSE used for charging.
        /// </summary>
        [Optional]
        public EVSE_Id?                     EVSEId                       { get; }

        /// <summary>
        /// The charging station of the charging station used for charging.
        /// </summary>
        [Optional]
        public ChargingStation              ChargingStation              { get; }

        /// <summary>
        /// The identification of the charging station used for charging.
        /// </summary>
        [Optional]
        public ChargingStation_Id?          ChargingStationId            { get; }

        /// <summary>
        /// The charging pool of the charging pool used for charging.
        /// </summary>
        [Optional]
        public ChargingPool                 ChargingPool                 { get; }

        /// <summary>
        /// The identification of the charging pool used for charging.
        /// </summary>
        [Optional]
        public ChargingPool_Id?             ChargingPoolId               { get; }

        /// <summary>
        /// The charging station operator used for charging.
        /// </summary>
        [Optional]
        public ChargingStationOperator      ChargingStationOperator      { get; }

        /// <summary>
        /// The identification of the charging station operator used for charging.
        /// </summary>
        [Optional]
        public ChargingStationOperator_Id?  ChargingStationOperatorId    { get; }

        /// <summary>
        /// The consumed charging product.
        /// </summary>
        [Optional]
        public ChargingProduct              ChargingProduct              { get; }

        #endregion

        #region Identification

        /// <summary>
        /// The identification used for starting this charging process.
        /// </summary>
        [Optional]
        public AuthIdentification               IdentificationStart    { get; }

        /// <summary>
        /// The identification used for stopping this charging process.
        /// </summary>
        [Optional]
        public AuthIdentification               IdentificationStop     { get; }

        /// <summary>
        /// The identification of the e-mobility provider used for starting this charging process.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id?  ProviderIdStart        { get; }

        /// <summary>
        /// The identification of the e-mobility provider used for stopping this charging process.
        /// </summary>
        [Optional]
        public eMobilityProvider_Id?  ProviderIdStop         { get; }

        #endregion

        #region Reservation

        /// <summary>
        /// An optional charging reservation used before charging.
        /// </summary>
        [Optional]
        public ChargingReservation      Reservation        { get; }

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
        public EnergyMeter_Id?                   EnergyMeterId          { get; }

        /// <summary>
        /// An optional enumeration of intermediate energy meter values.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<Single>>  EnergyMeteringValues   { get; }

        /// <summary>
        /// The optional sum of the consumed energy based on the meter values.
        /// </summary>
        [Optional]
        public Single ConsumedEnergy
        {
            get
            {

                if (EnergyMeteringValues == null ||
                    EnergyMeteringValues.Count() < 1)
                    return 0;

                return EnergyMeteringValues.Last().Value - EnergyMeteringValues.First().Value;

            }
        }

        /// <summary>
        /// An optional signature of the metering values.
        /// </summary>
        [Optional]
        public String                            MeteringSignature      { get; }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charge detail record for the given charging session (identification).
        /// </summary>
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
        /// 
        /// <param name="IdentificationStart">The identification used for starting this charging process.</param>
        /// <param name="IdentificationStop">The identification used for stopping this charging process.</param>
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
        /// <param name="MeteringSignature">An optional signature of the metering values.</param>
        /// 
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargeDetailRecord(ChargingSession_Id                   SessionId,
                                  StartEndDateTime?                    SessionTime,
                                  TimeSpan?                            Duration                    = null,

                                  EVSE                                 EVSE                        = null,
                                  EVSE_Id?                             EVSEId                      = null,
                                  ChargingStation                      ChargingStation             = null,
                                  ChargingStation_Id?                  ChargingStationId           = null,
                                  ChargingPool                         ChargingPool                = null,
                                  ChargingPool_Id?                     ChargingPoolId              = null,
                                  ChargingStationOperator              ChargingStationOperator     = null,
                                  ChargingStationOperator_Id?          ChargingStationOperatorId   = null,
                                  ChargingProduct                      ChargingProduct             = null,

                                  AuthIdentification                   IdentificationStart         = null,
                                  AuthIdentification                   IdentificationStop          = null,
                                  eMobilityProvider_Id?                ProviderIdStart             = null,
                                  eMobilityProvider_Id?                ProviderIdStop              = null,

                                  ChargingReservation                  Reservation                 = null,
                                  ChargingReservation_Id?              ReservationId               = null,
                                  StartEndDateTime?                    ReservationTime             = null,

                                  ParkingSpace_Id?                     ParkingSpaceId              = null,
                                  StartEndDateTime?                    ParkingTime                 = null,
                                  Decimal?                             ParkingFee                  = null,

                                  EnergyMeter_Id?                      EnergyMeterId               = null,
                                  IEnumerable<Timestamped<Single>>     EnergyMeteringValues        = null,
                                  String                               MeteringSignature           = null,

                                  IReadOnlyDictionary<String, Object>  CustomData                  = null)

            : base(CustomData)

        {

            this.SessionId                   = SessionId;
            this.SessionTime                 = SessionTime;
            this.Duration                    = Duration;

            this.EVSE                        = EVSE;
            this.EVSEId                      = EVSE                     != null ? EVSE.Id                    : EVSEId;
            this.ChargingStation             = ChargingStation;
            this.ChargingStationId           = ChargingStation          != null ? ChargingStation.Id         : ChargingStationId;
            this.ChargingPool                = ChargingPool;
            this.ChargingPoolId              = ChargingPool             != null ? ChargingPool.Id            : ChargingPoolId;
            this.ChargingStationOperator     = ChargingStationOperator;
            this.ChargingStationOperatorId   = ChargingStationOperator  != null ? ChargingStationOperator.Id : ChargingStationOperatorId;
            this.ChargingProduct             = ChargingProduct;

            this.IdentificationStart         = IdentificationStart;
            this.IdentificationStop          = IdentificationStop;
            this.ProviderIdStart             = ProviderIdStart;
            this.ProviderIdStop              = ProviderIdStop;

            this.Reservation                 = Reservation;
            this.ReservationId               = Reservation              != null ? Reservation.Id             : ReservationId;
            this.ReservationTime             = ReservationTime;

            this.ParkingSpaceId              = ParkingSpaceId;
            this.ParkingTime                 = ParkingTime;
            this.ParkingFee                  = ParkingFee;

            this.EnergyMeterId               = EnergyMeterId;
            this.EnergyMeteringValues        = EnergyMeteringValues ?? new Timestamped<Single>[0];
            this.MeteringSignature           = MeteringSignature;

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
            if (Object.ReferenceEquals(ChargeDetailRecord1, ChargeDetailRecord2))
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
        public Int32 CompareTo(Object Object)
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
