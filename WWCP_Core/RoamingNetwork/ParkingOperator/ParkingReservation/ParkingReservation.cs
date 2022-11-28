/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>ParkingReservation
 * This file is part of WWCP Core <https://github.com/OpenParkingCloud/WWCP_Core>
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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A parking reservation
    /// </summary>
    public class ParkingReservation : IEquatable <ParkingReservation>,
                                      IComparable<ParkingReservation>,
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

        private readonly ParkingReservation_Id _ReservationId;

        /// <summary>
        /// The parking reservation identification.
        /// </summary>
        [Mandatory]
        public ParkingReservation_Id Id
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

                var _TimeLeft = _EndTime - org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;// _StartTime + _Duration - Timestamp.Now;

                return _ParkingSession == null
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

        private readonly ParkingReservationLevel _ReservationLevel;

        [Mandatory]
        public ParkingReservationLevel ReservationLevel
        {
            get
            {
                return _ReservationLevel;
            }
        }

        #endregion


        [Optional]
        public EMobilityProvider_Id?  ProviderId          { get; }

        [Optional]
        public eMobilityAccount_Id?   eMAId               { get; }

        [Optional]
        public RoamingNetwork         RoamingNetwork      { get; }

        [Optional]
        public ChargingPool_Id?       ParkingPoolId      { get; }

        [Optional]
        public ChargingStation_Id?    ParkingStationId   { get; }

        [Optional]
        public EVSE_Id?               EVSEId              { get; }

        [Optional]
        public ParkingProduct_Id?    ParkingProductId   { get; }


        #region ChargingSession

        private ChargingSession _ParkingSession;

        [Mandatory]
        public ChargingSession ParkingSession
        {

            get
            {
                return _ParkingSession;
            }

            set
            {
                _ParkingSession = value;
            }

        }

        #endregion


        #region AuthTokens

        private readonly HashSet<AuthenticationToken> _AuthTokens;

        [Optional]
        public IEnumerable<AuthenticationToken> AuthTokens
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
        /// Create a parking reservation.
        /// </summary>
        public ParkingReservation(ParkingReservation_Id             ReservationId,
                                  DateTime                          Timestamp,
                                  DateTime                          StartTime,
                                  TimeSpan                          Duration,
                                  DateTime                          EndTime,
                                  TimeSpan                          ConsumedReservationTime,
                                  ParkingReservationLevel           ReservationLevel,

                                  EMobilityProvider_Id?             ProviderId          = null,
                                  eMobilityAccount_Id?              eMAId               = null,

                                  RoamingNetwork                    RoamingNetwork      = null,
                                  ChargingPool_Id?                  ParkingPoolId      = null,
                                  ChargingStation_Id?               ParkingStationId   = null,
                                  EVSE_Id?                          EVSEId              = null,
                                  ParkingProduct_Id?                ParkingProductId   = null,

                                  IEnumerable<AuthenticationToken>           AuthTokens          = null,
                                  IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
                                  IEnumerable<UInt32>               PINs                = null)

        {

            #region Initial checks

            if (ReservationId == null)
                throw new ArgumentNullException(nameof(ReservationId), "The given parking reservation identification must not be null!");

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
            this.ParkingPoolId            = ParkingPoolId;
            this.ParkingStationId         = ParkingStationId;
            this.EVSEId                    = EVSEId;
            this.ParkingProductId         = ParkingProductId;

            this._AuthTokens               = AuthTokens != null ? new HashSet<AuthenticationToken>(AuthTokens) : new HashSet<AuthenticationToken>();
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

            return _ParkingSession == null
                       ? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now > _EndTime
                       : false;

        }

        #endregion

        #region IsExpired(ReservationSelfCancelAfter)

        /// <summary>
        /// Returns true if the reservation is expired.
        /// </summary>
        public Boolean IsExpired(TimeSpan ReservationSelfCancelAfter)
        {
            return org.GraphDefined.Vanaheimr.Illias.Timestamp.Now > (_EndTime + ReservationSelfCancelAfter);
        }

        #endregion


        #region AddToConsumedReservationTime(Time)

        public void AddToConsumedReservationTime(TimeSpan Time)
        {
            _ConsumedReservationTime = _ConsumedReservationTime.Add(Time);
        }

        #endregion


        #region IComparable<ParkingReservation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a parking reservation.
            var ParkingReservation = Object as ParkingReservation;
            if ((Object) ParkingReservation == null)
                throw new ArgumentException("The given object is not a parking reservation!");

            return CompareTo(ParkingReservation);

        }

        #endregion

        #region CompareTo(ParkingReservation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingReservation">A parking reservation object to compare with.</param>
        public Int32 CompareTo(ParkingReservation ParkingReservation)
        {

            if ((Object) ParkingReservation == null)
                throw new ArgumentNullException("The given parking reservation must not be null!");

            return _ReservationId.CompareTo(ParkingReservation._ReservationId);

        }

        #endregion

        #endregion

        #region IEquatable<ParkingReservation> Members

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

            // Check if the given object is a parking reservation.
            var ParkingReservation = Object as ParkingReservation;
            if ((Object) ParkingReservation == null)
                return false;

            return this.Equals(ParkingReservation);

        }

        #endregion

        #region Equals(ParkingReservation)

        /// <summary>
        /// Compares two parking reservations for equality.
        /// </summary>
        /// <param name="ParkingReservation">A parking reservation to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingReservation ParkingReservation)
        {

            if ((Object) ParkingReservation == null)
                return false;

            return _ReservationId.Equals(ParkingReservation._ReservationId);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => _ReservationId.ToString();

        #endregion

    }

}
