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
    /// The current admin status of a charging pool.
    /// </summary>
    public class ChargingPoolAdminStatus : AInternalData,
                                           IEquatable <ChargingPoolAdminStatus>,
                                           IComparable<ChargingPoolAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                            Id       { get; }

        /// <summary>
        /// The current admin status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolAdminStatusTypes>  Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current timestamped admin status of the charging pool.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingPoolAdminStatus(ChargingPool_Id                            Id,
                                       Timestamped<ChargingPoolAdminStatusTypes>  Status,
                                       IReadOnlyDictionary<String, Object>        CustomData  = null)

            : base(null,
                   CustomData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region (static) Snapshot(ChargingPool)

        /// <summary>
        /// Take a snapshot of the current charging pool admin status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingPoolAdminStatus Snapshot(ChargingPool ChargingPool)

            => new ChargingPoolAdminStatus(ChargingPool.Id,
                                           ChargingPool.AdminStatus);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolAdminStatus ChargingPoolAdminStatus1, ChargingPoolAdminStatus ChargingPoolAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolAdminStatus1, ChargingPoolAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolAdminStatus1 == null) || ((Object) ChargingPoolAdminStatus2 == null))
                return false;

            return ChargingPoolAdminStatus1.Equals(ChargingPoolAdminStatus2);

        }

        #endregion

        #region Operator != (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolAdminStatus ChargingPoolAdminStatus1, ChargingPoolAdminStatus ChargingPoolAdminStatus2)
            => !(ChargingPoolAdminStatus1 == ChargingPoolAdminStatus2);

        #endregion

        #region Operator <  (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolAdminStatus ChargingPoolAdminStatus1, ChargingPoolAdminStatus ChargingPoolAdminStatus2)
        {

            if ((Object) ChargingPoolAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatus1), "The given ChargingPoolAdminStatus1 must not be null!");

            return ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolAdminStatus ChargingPoolAdminStatus1, ChargingPoolAdminStatus ChargingPoolAdminStatus2)
            => !(ChargingPoolAdminStatus1 > ChargingPoolAdminStatus2);

        #endregion

        #region Operator >  (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolAdminStatus ChargingPoolAdminStatus1, ChargingPoolAdminStatus ChargingPoolAdminStatus2)
        {

            if ((Object) ChargingPoolAdminStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatus1), "The given ChargingPoolAdminStatus1 must not be null!");

            return ChargingPoolAdminStatus1.CompareTo(ChargingPoolAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolAdminStatus1, ChargingPoolAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus1">A charging pool admin status.</param>
        /// <param name="ChargingPoolAdminStatus2">Another charging pool admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolAdminStatus ChargingPoolAdminStatus1, ChargingPoolAdminStatus ChargingPoolAdminStatus2)
            => !(ChargingPoolAdminStatus1 < ChargingPoolAdminStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPoolAdminStatus))
                throw new ArgumentException("The given object is not a ChargingPoolAdminStatus!",
                                            nameof(Object));

            return CompareTo((ChargingPoolAdminStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingPoolAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolAdminStatus ChargingPoolAdminStatus)
        {

            if ((Object) ChargingPoolAdminStatus == null)
                throw new ArgumentNullException(nameof(ChargingPoolAdminStatus), "The given ChargingPoolAdminStatus must not be null!");

            // Compare ChargingPool Ids
            var _Result = Id.CompareTo(ChargingPoolAdminStatus.Id);

            // If equal: Compare ChargingPool status
            if (_Result == 0)
                _Result = Status.CompareTo(ChargingPoolAdminStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolAdminStatus> Members

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

            if (!(Object is ChargingPoolAdminStatus))
                return false;

            return Equals((ChargingPoolAdminStatus) Object);

        }

        #endregion

        #region Equals(ChargingPoolAdminStatus)

        /// <summary>
        /// Compares two ChargingPool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolAdminStatus">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolAdminStatus ChargingPoolAdminStatus)
        {

            if ((Object) ChargingPoolAdminStatus == null)
                return false;

            return Id.    Equals(ChargingPoolAdminStatus.Id) &&
                   Status.Equals(ChargingPoolAdminStatus.Status);

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
