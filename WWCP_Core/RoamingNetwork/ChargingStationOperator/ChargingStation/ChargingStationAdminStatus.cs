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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The current admin status of a charging station.
    /// </summary>
    public class ChargingStationAdminStatus : AInternalData,
                                              IEquatable <ChargingStationAdminStatus>,
                                              IComparable<ChargingStationAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                            Id       { get; }

        /// <summary>
        /// The current timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>  Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="Status">The current timestamped admin status of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationAdminStatus(ChargingStation_Id                            Id,
                                          Timestamped<ChargingStationAdminStatusTypes>  Status,
                                          IReadOnlyDictionary<String, Object>           CustomData  = null)

            : base(null,
                   CustomData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station admin status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationAdminStatus Snapshot(ChargingStation ChargingStation)

            => new ChargingStationAdminStatus(ChargingStation.Id,
                                              ChargingStation.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationAdminStatus1, ChargingStationAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationAdminStatus1 == null) || ((Object) ChargingStationAdminStatus2 == null))
                return false;

            return ChargingStationAdminStatus1.Equals(ChargingStationAdminStatus2);

        }

        #endregion

        #region Operator != (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
            => !(ChargingStationAdminStatus1 == ChargingStationAdminStatus2);

        #endregion

        #region Operator <  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            if ((Object) ChargingStationAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus1), "The given ChargingStationAdminStatus1 must not be null!");

            return ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
            => !(ChargingStationAdminStatus1 > ChargingStationAdminStatus2);

        #endregion

        #region Operator >  (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
        {

            if ((Object) ChargingStationAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus1), "The given ChargingStationAdminStatus1 must not be null!");

            return ChargingStationAdminStatus1.CompareTo(ChargingStationAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationAdminStatus1, ChargingStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus1">A charging station admin status.</param>
        /// <param name="ChargingStationAdminStatus2">Another charging station admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationAdminStatus ChargingStationAdminStatus1, ChargingStationAdminStatus ChargingStationAdminStatus2)
            => !(ChargingStationAdminStatus1 < ChargingStationAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationAdminStatus))
                throw new ArgumentException("The given object is not a ChargingStationAdminStatus!",
                                            nameof(Object));

            return CompareTo((ChargingStationAdminStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatus ChargingStationAdminStatus)
        {

            if ((Object) ChargingStationAdminStatus == null)
                throw new ArgumentNullException(nameof(ChargingStationAdminStatus), "The given ChargingStationAdminStatus must not be null!");

            // Compare ChargingStation Ids
            var _Result = Id.CompareTo(ChargingStationAdminStatus.Id);

            // If equal: Compare ChargingStation status
            if (_Result == 0)
                _Result = Status.CompareTo(ChargingStationAdminStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatus> Members

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

            if (!(Object is ChargingStationAdminStatus))
                return false;

            return Equals((ChargingStationAdminStatus) Object);

        }

        #endregion

        #region Equals(ChargingStationAdminStatus)

        /// <summary>
        /// Compares two ChargingStation identifications for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatus">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationAdminStatus ChargingStationAdminStatus)
        {

            if ((Object) ChargingStationAdminStatus == null)
                return false;

            return Id.    Equals(ChargingStationAdminStatus.Id) &&
                   Status.Equals(ChargingStationAdminStatus.Status);

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

                return Id.    GetHashCode() * 5 ^
                       Status.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status.Value,
                             " since ",
                             Status.Timestamp.ToIso8601());

        #endregion

    }

}
