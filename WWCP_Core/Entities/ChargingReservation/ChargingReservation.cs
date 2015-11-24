/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging reservation
    /// </summary>
    public class ChargingReservation : IEquatable <ChargingReservation>,
                                       IComparable<ChargingReservation>,
                                       IComparable
    {

        #region Data

        /// <summary>
        /// The default max size of the charging station (aggregated EVSE) status history.
        /// </summary>
        public const UInt16 DefaultStationStatusHistorySize = 50;

        #endregion

        #region Properties

        #region ReservationId

        private readonly ChargingReservation_Id _ReservationId;

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        [Mandatory]
        public ChargingReservation_Id Id
        {
            get
            {
                return _ReservationId;
            }
        }

        #endregion

        #region StartTime

        private readonly DateTime _StartTime;

        [Optional]
        public DateTime StartTime
        {
            get
            {
                return _StartTime;
            }
        }

        #endregion

        #region Duration

        private readonly TimeSpan _Duration;

        [Optional]
        public TimeSpan Duration
        {
            get
            {
                return _Duration;
            }
        }

        #endregion





        #region ChargingPoolId

        private readonly ChargingPool_Id _ChargingPoolId;

        [Optional]
        public ChargingPool_Id ChargingPoolId
        {
            get
            {
                return _ChargingPoolId;
            }
        }

        #endregion

        #region ChargingStationId

        private readonly ChargingStation_Id _ChargingStationId;

        [Optional]
        public ChargingStation_Id ChargingStationId
        {
            get
            {
                return _ChargingStationId;
            }
        }

        #endregion

        #region EVSEId

        private readonly EVSE_Id _EVSEId;

        [Optional]
        public EVSE_Id EVSEId
        {
            get
            {
                return _EVSEId;
            }
        }

        #endregion

        #region Identification

        private readonly AuthInfo _Identification;

        [Optional]
        public AuthInfo Identification
        {
            get
            {
                return _Identification;
            }
        }

        #endregion

        #region ParkingTime

        private readonly StartEndTime? _ParkingTime;

        [Mandatory]
        public StartEndTime? ParkingTime
        {
            get
            {
                return _ParkingTime;
            }
        }

        #endregion

        #region ChargingTime

        private readonly StartEndTime? _ChargingTime;

        [Mandatory]
        public StartEndTime? ChargingTime
        {
            get
            {
                return _ChargingTime;
            }
        }

        #endregion

        #region ConsumedEnergy

        private readonly Double? _ConsumedEnergy;

        [Optional]
        public Double? ConsumedEnergy
        {
            get
            {
                return _ConsumedEnergy;
            }
        }

        #endregion

        #region MeterValues

        private readonly IEnumerable<Timestamped<Double>> _MeterValues;

        [Optional]
        public IEnumerable<Timestamped<Double>> MeterValues
        {
            get
            {
                return _MeterValues;
            }
        }

        #endregion

        #region MeteringSignature

        private readonly String _MeteringSignature;

        [Optional]
        public String MeteringSignature
        {
            get
            {
                return _MeteringSignature;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charging reservation.
        /// </summary>
        public ChargingReservation(DateTime                Timestamp,
                                   ChargingReservation_Id  ReservationId,
                                   EVSP_Id                 ProviderId,
                                   DateTime?               StartTime          = null,
                                   TimeSpan?               Duration           = null,
                                   ChargingPool_Id         ChargingPoolId     = null,
                                   ChargingStation_Id      ChargingStationId  = null,
                                   EVSE_Id                 EVSEId             = null,
                                   ChargingProduct_Id      ChargingProductId  = null)

        {

            #region Initial checks

            if (ReservationId == null)
                throw new ArgumentNullException("ReservationId", "The charging reservation identification must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId", "The provider identification must not be null!");

            #endregion

            this._ReservationId     = ReservationId;
            this._StartTime         = StartTime.HasValue ? StartTime.Value : DateTime.Now;
            this._Duration          = Duration. HasValue ? Duration. Value : TimeSpan.FromMinutes(15);

        }

        #endregion


        #region IComparable<ChargingReservation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging reservation.
            var ChargingReservation = Object as ChargingReservation;
            if ((Object) ChargingReservation == null)
                throw new ArgumentException("The given object is not a charging reservation!");

            return CompareTo(ChargingReservation);

        }

        #endregion

        #region CompareTo(ChargingReservation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservation">A charging reservation object to compare with.</param>
        public Int32 CompareTo(ChargingReservation ChargingReservation)
        {

            if ((Object) ChargingReservation == null)
                throw new ArgumentNullException("The given charging reservation must not be null!");

            return _ReservationId.CompareTo(ChargingReservation._ReservationId);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingReservation> Members

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

            // Check if the given object is a charging reservation.
            var ChargingReservation = Object as ChargingReservation;
            if ((Object) ChargingReservation == null)
                return false;

            return this.Equals(ChargingReservation);

        }

        #endregion

        #region Equals(ChargingReservation)

        /// <summary>
        /// Compares two charging reservations for equality.
        /// </summary>
        /// <param name="ChargingReservation">A charging reservation to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingReservation ChargingReservation)
        {

            if ((Object) ChargingReservation == null)
                return false;

            return _ReservationId.Equals(ChargingReservation._ReservationId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return _ReservationId.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _ReservationId.ToString();
        }

        #endregion

    }

}
