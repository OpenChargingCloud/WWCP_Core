/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

        #region IsExpired

        private Boolean _IsExpired;

        /// <summary>
        /// returns true if the charging reservation is expired.
        /// </summary>
        [InternalUseOnly]
        public Boolean IsExpired
        {

            get
            {
                return _IsExpired;
            }

            set
            {
                if (_IsExpired == false && value == true)
                {
                    _IsExpired = true;
                }
            }

        }

        #endregion

        #region ProviderId

        private readonly EVSP_Id _ProviderId;

        [Optional]
        public EVSP_Id ProviderId
        {
            get
            {
                return _ProviderId;
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

        #region RoamingNetwork

        private readonly RoamingNetwork _RoamingNetwork;

        [Optional]
        public RoamingNetwork RoamingNetwork
        {
            get
            {
                return _RoamingNetwork;
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

        #region ChargingProductId

        private readonly ChargingProduct_Id _ChargingProductId;

        [Optional]
        public ChargingProduct_Id ChargingProductId
        {
            get
            {
                return _ChargingProductId;
            }
        }

        #endregion


        #region AuthTokens

        private readonly IEnumerable<Auth_Token> _AuthTokens;

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

        private readonly IEnumerable<eMA_Id> _eMAIds;

        [Optional]
        public IEnumerable<eMA_Id> eMAIds
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

        #region ChargingReservation(...)

        /// <summary>
        /// Create a charging reservation.
        /// </summary>
        public ChargingReservation(DateTime                  Timestamp,
                                   DateTime                  StartTime,
                                   TimeSpan                  Duration,
                                   EVSP_Id                   ProviderId,

                                   ChargingReservationLevel  ChargingReservationLevel,
                                   RoamingNetwork            RoamingNetwork,
                                   ChargingPool_Id           ChargingPoolId     = null,
                                   ChargingStation_Id        ChargingStationId  = null,
                                   EVSE_Id                   EVSEId             = null,
                                   ChargingProduct_Id        ChargingProductId  = null,

                                   IEnumerable<Auth_Token>   AuthTokens            = null,
                                   IEnumerable<eMA_Id>       eMAIds             = null,
                                   IEnumerable<UInt32>       PINs               = null)

            : this(ChargingReservation_Id.New,
                   Timestamp,
                   StartTime,
                   Duration,
                   ProviderId,

                   ChargingReservationLevel,
                   RoamingNetwork,
                   ChargingPoolId,
                   ChargingStationId,
                   EVSEId,
                   ChargingProductId,

                   AuthTokens,
                   eMAIds,
                   PINs)

        { }

        #endregion

        #region ChargingReservation(ReservationId, ...)

        /// <summary>
        /// Create a charging reservation.
        /// </summary>
        public ChargingReservation(ChargingReservation_Id    ReservationId,
                                   DateTime                  Timestamp,
                                   DateTime                  StartTime,
                                   TimeSpan                  Duration,
                                   EVSP_Id                   ProviderId,

                                   ChargingReservationLevel  ReservationLevel,
                                   RoamingNetwork            RoamingNetwork,
                                   ChargingPool_Id           ChargingPoolId     = null,
                                   ChargingStation_Id        ChargingStationId  = null,
                                   EVSE_Id                   EVSEId             = null,
                                   ChargingProduct_Id        ChargingProductId  = null,

                                   IEnumerable<Auth_Token>   AuthTokens            = null,
                                   IEnumerable<eMA_Id>       eMAIds             = null,
                                   IEnumerable<UInt32>       PINs               = null)

        {

            this._Timestamp          = Timestamp;
            this._ReservationId      = ReservationId;
            this._StartTime          = StartTime;
            this._Duration           = Duration;

            this._ReservationLevel   = ReservationLevel;
            this._RoamingNetwork     = RoamingNetwork;
            this._ChargingPoolId     = ChargingPoolId;
            this._ChargingStationId  = ChargingStationId;
            this._EVSEId             = EVSEId;
            this._ChargingProductId  = ChargingProductId;

            this._AuthTokens            = AuthTokens != null ? AuthTokens : new Auth_Token[0];
            this._eMAIds             = eMAIds  != null ? eMAIds  : new eMA_Id[0];
            this._PINs               = PINs    != null ? PINs    : new UInt32[0];

        }

        #endregion

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

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _ReservationId.ToString();
        }

        #endregion

    }

}
