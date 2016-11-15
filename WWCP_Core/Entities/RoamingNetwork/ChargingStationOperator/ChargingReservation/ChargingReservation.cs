/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region Properties

        #region Timestamp

        private readonly DateTime _Timestamp;

        [Mandatory]
        public DateTime Timestamp
        {
            get
            {
                return _Timestamp;
            }
        }

        #endregion

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

        [Mandatory]
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

        [Mandatory]
        public TimeSpan Duration
        {
            get
            {
                return _Duration;
            }
        }

        #endregion

        #region TimeLeft

        public TimeSpan TimeLeft
        {
            get
            {

                var _TimeLeft = _EndTime - DateTime.Now.ToUniversalTime();// _StartTime + _Duration - DateTime.Now;

                return _ChargingSession == null
                           ? _TimeLeft.TotalSeconds > 0 ? _TimeLeft : TimeSpan.FromSeconds(0)
                           : TimeSpan.FromSeconds(0);

            }
        }

        #endregion

        #region EndTime

        private DateTime _EndTime;

        [Mandatory]
        public DateTime EndTime
        {

            get
            {
                return _EndTime;
            }

            set
            {
                _EndTime = value;
            }

        }

        #endregion

        #region ConsumedReservationTime

        private TimeSpan _ConsumedReservationTime;

        [Mandatory]
        public TimeSpan ConsumedReservationTime
        {
            get
            {
                return _ConsumedReservationTime;
            }
        }

        #endregion

        #region ReservationLevel

        private readonly ChargingReservationLevel _ReservationLevel;

        [Mandatory]
        public ChargingReservationLevel ReservationLevel
        {
            get
            {
                return _ReservationLevel;
            }
        }

        #endregion


        [Optional]
        public eMobilityProvider_Id?  ProviderId          { get; }

        [Optional]
        public eMobilityAccount_Id?   eMAId               { get; }

        [Optional]
        public RoamingNetwork         RoamingNetwork      { get; }

        [Optional]
        public ChargingPool_Id        ChargingPoolId      { get; }

        [Optional]
        public ChargingStation_Id     ChargingStationId   { get; }

        [Optional]
        public EVSE_Id?               EVSEId              { get; }

        [Optional]
        public ChargingProduct_Id     ChargingProductId   { get; }


        #region ChargingSession

        private ChargingSession _ChargingSession;

        [Mandatory]
        public ChargingSession ChargingSession
        {

            get
            {
                return _ChargingSession;
            }

            set
            {
                _ChargingSession = value;
            }

        }

        #endregion


        #region AuthTokens

        private readonly HashSet<Auth_Token> _AuthTokens;

        [Optional]
        public IEnumerable<Auth_Token> AuthTokens
        {
            get
            {
                return _AuthTokens;
            }
        }

        #endregion

        #region eMAIds

        private readonly HashSet<eMobilityAccount_Id> _eMAIds;

        [Optional]
        public IEnumerable<eMobilityAccount_Id> eMAIds
        {
            get
            {
                return _eMAIds;
            }
        }

        #endregion

        #region PINs

        private readonly IEnumerable<UInt32> _PINs;

        [Optional]
        public IEnumerable<UInt32> PINs
        {
            get
            {
                return _PINs;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charging reservation.
        /// </summary>
        public ChargingReservation(ChargingReservation_Id            ReservationId,
                                   DateTime                          Timestamp,
                                   DateTime                          StartTime,
                                   TimeSpan                          Duration,
                                   DateTime                          EndTime,
                                   TimeSpan                          ConsumedReservationTime,
                                   ChargingReservationLevel          ReservationLevel,

                                   eMobilityProvider_Id?             ProviderId          = null,
                                   eMobilityAccount_Id?              eMAId               = null,

                                   RoamingNetwork                    RoamingNetwork      = null,
                                   ChargingPool_Id                   ChargingPoolId      = null,
                                   ChargingStation_Id                ChargingStationId   = null,
                                   EVSE_Id?                          EVSEId              = null,
                                   ChargingProduct_Id                ChargingProductId   = null,

                                   IEnumerable<Auth_Token>           AuthTokens          = null,
                                   IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                                   IEnumerable<UInt32>               PINs                = null)

        {

            #region Initial checks

            if (ReservationId == null)
                throw new ArgumentNullException(nameof(ReservationId), "The given charging reservation identification must not be null!");

            #endregion

            this._ReservationId            = ReservationId;
            this._Timestamp                = Timestamp.ToUniversalTime();
            this._StartTime                = StartTime.ToUniversalTime();
            this._Duration                 = Duration;
            this._EndTime                  = StartTime.ToUniversalTime() + Duration;
            this._ConsumedReservationTime  = ConsumedReservationTime;
            this._ReservationLevel         = ReservationLevel;

            this.ProviderId                = ProviderId;
            this.eMAId                     = eMAId;

            this.RoamingNetwork            = RoamingNetwork;
            this.ChargingPoolId            = ChargingPoolId;
            this.ChargingStationId         = ChargingStationId;
            this.EVSEId                    = EVSEId;
            this.ChargingProductId         = ChargingProductId;

            this._AuthTokens               = AuthTokens != null ? new HashSet<Auth_Token>(AuthTokens) : new HashSet<Auth_Token>();
            this._eMAIds                   = eMAIds     != null ? new HashSet<eMobilityAccount_Id>    (eMAIds)     : new HashSet<eMobilityAccount_Id>();
            this._PINs                     = PINs       != null ? new HashSet<UInt32>    (PINs)       : new HashSet<UInt32>();

        }

        #endregion


        #region IsExpired()

        /// <summary>
        /// Returns true if the reservation is expired.
        /// </summary>
        public Boolean IsExpired()
        {

            return _ChargingSession == null
                       ? DateTime.Now.ToUniversalTime() > _EndTime
                       : false;

        }

        #endregion

        #region IsExpired(ReservationSelfCancelAfter)

        /// <summary>
        /// Returns true if the reservation is expired.
        /// </summary>
        public Boolean IsExpired(TimeSpan ReservationSelfCancelAfter)
        {
            return DateTime.Now.ToUniversalTime() > (_EndTime + ReservationSelfCancelAfter);
        }

        #endregion


        #region AddToConsumedReservationTime(Time)

        public void AddToConsumedReservationTime(TimeSpan Time)
        {
            _ConsumedReservationTime = _ConsumedReservationTime.Add(Time);
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

            => _ReservationId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => _ReservationId.ToString();

        #endregion

    }

}
