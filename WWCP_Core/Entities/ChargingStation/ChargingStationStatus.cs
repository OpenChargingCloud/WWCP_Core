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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current status of a charging station.
    /// </summary>
    public class ChargingStationStatus : IEquatable<ChargingStationStatus>,
                                         IComparable<ChargingStationStatus>
    {

        #region Properties

        #region Id

        private readonly ChargingStation_Id _Id;

        /// <summary>
        /// The unique identification of a charging station.
        /// </summary>
        public ChargingStation_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Status

        private readonly ChargingStationStatusType _Status;

        /// <summary>
        /// The current status of a charging station.
        /// </summary>
        public ChargingStationStatusType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #region Timestamp

        private readonly DateTime _Timestamp;

        /// <summary>
        /// The timestamp of the current status of the charging station.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return _Timestamp;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status.
        /// </summary>
        /// <param name="Id">The unique identification of a charging station.</param>
        /// <param name="Status">The current status of a charging station.</param>
        /// <param name="Timestamp">The timestamp of the current status of the charging station.</param>
        public ChargingStationStatus(ChargingStation_Id         Id,
                                     ChargingStationStatusType  Status,
                                     DateTime                   Timestamp)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id), "The given unique identification of a charging station must not be null!");

            #endregion

            this._Id         = Id;
            this._Status     = Status;
            this._Timestamp  = Timestamp;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station status.
        /// </summary>
        /// <param name="ChargingStation">An charging station.</param>
        public static ChargingStationStatus Snapshot(ChargingStation ChargingStation)
        {

            return new ChargingStationStatus(ChargingStation.Id,
                                             ChargingStation.Status.Value,
                                             ChargingStation.Status.Timestamp);

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A ChargingStationStatus.</param>
        /// <param name="ChargingStationStatus2">Another ChargingStationStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationStatus1, ChargingStationStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationStatus1 == null) || ((Object) ChargingStationStatus2 == null))
                return false;

            return ChargingStationStatus1.Equals(ChargingStationStatus2);

        }

        #endregion

        #region Operator != (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A ChargingStationStatus.</param>
        /// <param name="ChargingStationStatus2">Another ChargingStationStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {
            return !(ChargingStationStatus1 == ChargingStationStatus2);
        }

        #endregion

        #region Operator <  (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A ChargingStationStatus.</param>
        /// <param name="ChargingStationStatus2">Another ChargingStationStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {

            if ((Object) ChargingStationStatus1 == null)
                throw new ArgumentNullException("The given ChargingStationStatus1 must not be null!");

            return ChargingStationStatus1.CompareTo(ChargingStationStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A ChargingStationStatus.</param>
        /// <param name="ChargingStationStatus2">Another ChargingStationStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {
            return !(ChargingStationStatus1 > ChargingStationStatus2);
        }

        #endregion

        #region Operator >  (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A ChargingStationStatus.</param>
        /// <param name="ChargingStationStatus2">Another ChargingStationStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {

            if ((Object) ChargingStationStatus1 == null)
                throw new ArgumentNullException("The given ChargingStationStatus1 must not be null!");

            return ChargingStationStatus1.CompareTo(ChargingStationStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationStatus1, ChargingStationStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus1">A ChargingStationStatus.</param>
        /// <param name="ChargingStationStatus2">Another ChargingStationStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationStatus ChargingStationStatus1, ChargingStationStatus ChargingStationStatus2)
        {
            return !(ChargingStationStatus1 < ChargingStationStatus2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStationStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a charging stationStatus.
            var ChargingStationStatus = Object as ChargingStationStatus;
            if ((Object) ChargingStationStatus == null)
                throw new ArgumentException("The given object is not a ChargingStationStatus!");

            return CompareTo(ChargingStationStatus);

        }

        #endregion

        #region CompareTo(ChargingStationStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationStatus ChargingStationStatus)
        {

            if ((Object) ChargingStationStatus == null)
                throw new ArgumentNullException("The given ChargingStationStatus must not be null!");

            // Compare ChargingStation Ids
            var _Result = _Id.CompareTo(ChargingStationStatus._Id);

            // If equal: Compare ChargingStation status
            if (_Result == 0)
                _Result = _Status.CompareTo(ChargingStationStatus._Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationStatus> Members

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

            // Check if the given object is a charging stationStatus.
            var ChargingStationStatus = Object as ChargingStationStatus;
            if ((Object) ChargingStationStatus == null)
                return false;

            return this.Equals(ChargingStationStatus);

        }

        #endregion

        #region Equals(ChargingStationStatus)

        /// <summary>
        /// Compares two ChargingStation identifications for equality.
        /// </summary>
        /// <param name="ChargingStationStatus">An ChargingStation identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationStatus ChargingStationStatus)
        {

            if ((Object) ChargingStationStatus == null)
                return false;

            return _Id.    Equals(ChargingStationStatus._Id) &&
                   _Status.Equals(ChargingStationStatus._Status);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return _Id.GetHashCode() * 17 ^ _Status.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            return String.Concat(_Id, " -> ", _Status.ToString());

        }

        #endregion

    }

}
