/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        [Mandatory]
        public ChargingReservation_Id    Id                         { get; }

        [Mandatory]
        public DateTime                  Timestamp                  { get; }

        [Mandatory]
        public DateTime                  StartTime                  { get; }

        [Mandatory]
        public TimeSpan                  Duration                   { get; }

        #region TimeLeft

        public TimeSpan TimeLeft
        {
            get
            {

                var _TimeLeft = EndTime - DateTime.UtcNow;// _StartTime + _Duration - DateTime.UtcNow;

                return ChargingSession == null
                           ? _TimeLeft.TotalSeconds > 0 ? _TimeLeft : TimeSpan.FromSeconds(0)
                           : TimeSpan.FromSeconds(0);

            }
        }

        #endregion

        [Mandatory]
        public DateTime                  EndTime                    { get; set; }

        [Mandatory]
        public TimeSpan                  ConsumedReservationTime    { get; private set; }

        [Mandatory]
        public ChargingReservationLevel  ReservationLevel           { get; }

        [Optional]
        public eMobilityProvider_Id?     ProviderId                 { get; }

        public EMPRoamingProvider_Id?    EMPRoamingProviderId       { get; internal set; }

        public CSORoamingProvider_Id?    CSORoamingProviderId       { get; internal set; }

        [Optional]
        public AAuthentication           StartAuthentication        { get; internal set; }

        [Optional]
        public AAuthentication           StopAuthentication         { get; internal set; }

        [Optional]
        public RoamingNetwork_Id?        RoamingNetworkId           { get; internal set; }

        public ChargingStationOperator_Id? ChargingStationOperatorId{ get; internal set; }

        [Optional]
        public ChargingPool_Id?          ChargingPoolId             { get; internal set; }

        [Optional]
        public ChargingStation_Id?       ChargingStationId          { get; internal set; }

        [Optional]
        public EVSE_Id?                  EVSEId                     { get; internal set; }

        [Optional]
        public ChargingProduct           ChargingProduct            { get; }

        [Mandatory]
        public ChargingSession           ChargingSession            { get; set; }


        #region AuthTokens

        private readonly HashSet<Auth_Token> _AuthTokens;

        [Optional]
        public IEnumerable<Auth_Token> AuthTokens
            => _AuthTokens;

        #endregion

        #region eMAIds

        private readonly HashSet<eMobilityAccount_Id> _eMAIds;

        [Optional]
        public IEnumerable<eMobilityAccount_Id> eMAIds
            => _eMAIds;

        #endregion

        #region PINs

        private readonly HashSet<UInt32> _PINs;

        [Optional]
        public IEnumerable<UInt32> PINs
            => _PINs;

        #endregion


        public ChargingReservation       ParentReservation          { get; internal set; }


        private readonly HashSet<ChargingReservation> _SubReservations;

        public IEnumerable<ChargingReservation> SubReservations
            => _SubReservations;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a charging reservation.
        /// </summary>
        public ChargingReservation(ChargingReservation_Id            Id,
                                   DateTime                          Timestamp,
                                   DateTime                          StartTime,
                                   TimeSpan                          Duration,
                                   DateTime                          EndTime,
                                   TimeSpan                          ConsumedReservationTime,
                                   ChargingReservationLevel          ReservationLevel,

                                   eMobilityProvider_Id?             ProviderId                  = null,
                                   RemoteAuthentication              StartAuthentication         = null,

                                   RoamingNetwork_Id?                RoamingNetworkId            = null,
                                   ChargingStationOperator_Id?       ChargingStationOperatorId   = null,
                                   ChargingPool_Id?                  ChargingPoolId              = null,
                                   ChargingStation_Id?               ChargingStationId           = null,
                                   EVSE_Id?                          EVSEId                      = null,
                                   ChargingProduct                   ChargingProduct             = null,

                                   IEnumerable<Auth_Token>           AuthTokens                  = null,
                                   IEnumerable<eMobilityAccount_Id>  eMAIds                      = null,
                                   IEnumerable<UInt32>               PINs                        = null,

                                   IEnumerable<ChargingReservation>  SubReservations             = null)

        {

            this.Id                         = Id;
            this.Timestamp                  = Timestamp.ToUniversalTime();
            this.StartTime                  = StartTime.ToUniversalTime();
            this.Duration                   = Duration;
            this.EndTime                    = StartTime.ToUniversalTime() + Duration;
            this.ConsumedReservationTime    = ConsumedReservationTime;
            this.ReservationLevel           = ReservationLevel;

            this.ProviderId                 = ProviderId;
            this.StartAuthentication        = StartAuthentication;

            this.RoamingNetworkId           = RoamingNetworkId;
            this.ChargingStationOperatorId  = ChargingStationOperatorId;
            this.ChargingPoolId             = ChargingPoolId;
            this.ChargingStationId          = ChargingStationId;
            this.EVSEId                     = EVSEId;
            this.ChargingProduct            = ChargingProduct;

            this._AuthTokens                = AuthTokens      != null ? new HashSet<Auth_Token>         (AuthTokens)      : new HashSet<Auth_Token>();
            this._eMAIds                    = eMAIds          != null ? new HashSet<eMobilityAccount_Id>(eMAIds)          : new HashSet<eMobilityAccount_Id>();
            this._PINs                      = PINs            != null ? new HashSet<UInt32>             (PINs)            : new HashSet<UInt32>();

            this._SubReservations           = SubReservations != null ? new HashSet<ChargingReservation>(SubReservations) : new HashSet<ChargingReservation>();

        }

        #endregion


        #region IsExpired()

        /// <summary>
        /// Returns true if the reservation is expired.
        /// </summary>
        public Boolean IsExpired()

            => ChargingSession == null
                   ? DateTime.UtcNow > EndTime
                   : false;

        #endregion

        #region IsExpired(ReservationSelfCancelAfter)

        /// <summary>
        /// Returns true if the reservation is expired.
        /// </summary>
        public Boolean IsExpired(TimeSpan ReservationSelfCancelAfter)

            => DateTime.UtcNow > (EndTime + ReservationSelfCancelAfter);

        #endregion


        #region AddToConsumedReservationTime(Time)

        public void AddToConsumedReservationTime(TimeSpan Time)
        {
            ConsumedReservationTime = ConsumedReservationTime.Add(Time);
        }

        #endregion


        public JObject ToJSON()

            => JSONObject.Create(

                   new JProperty("@id",                        Id.                     ToString()),
                   new JProperty("timestamp",                  Timestamp.              ToIso8601()),
                   new JProperty("startTime",                  StartTime.              ToIso8601()),
                   new JProperty("duration",                   Duration.               TotalMinutes),
                   new JProperty("endTime",                    EndTime.                ToIso8601()),

                   new JProperty("consumedReservationTime",    ConsumedReservationTime.TotalMinutes),
                   new JProperty("reservationLevel",           ReservationLevel.       ToString()),

                   ProviderId.HasValue
                       ? new JProperty("providerId",           ProviderId.             ToString())
                       : null,

                   StartAuthentication != null
                       ? new JProperty("authentication",       EndTime.                ToIso8601())
                       : null,

                   RoamingNetworkId.HasValue
                       ? new JProperty("roamingNetworkId",     RoamingNetworkId.       ToString())
                       : null,

                   ChargingPoolId.HasValue
                       ? new JProperty("chargingPoolId",       ChargingPoolId.         ToString())
                       : null,

                   ChargingStationId.HasValue
                       ? new JProperty("chargingStationId",    ChargingStationId.      ToString())
                       : null,

                   EVSEId.HasValue
                       ? new JProperty("EVSEId",               EVSEId.                 ToString())
                       : null,

                   ChargingProduct != null
                       ? new JProperty("chargingProduct",      ChargingProduct.        ToJSON())
                       : null,

                   ChargingSession != null
                       ? new JProperty("chargingSessionId",    ChargingSession.Id.     ToString())
                       : null

               );



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

            return Id.CompareTo(ChargingReservation.Id);

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

            return Id.Equals(ChargingReservation.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

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
