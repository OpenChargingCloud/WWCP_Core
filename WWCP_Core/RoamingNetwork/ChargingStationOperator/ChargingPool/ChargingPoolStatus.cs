/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current status of a charging pool.
    /// </summary>
    public class ChargingPoolStatus : ACustomData,
                                      IEquatable <ChargingPoolStatus>,
                                      IComparable<ChargingPoolStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                       Id       { get; }

        /// <summary>
        /// The current status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>  Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="Status">The current status of the charging pool.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingPoolStatus(ChargingPool_Id                       Id,
                                  Timestamped<ChargingPoolStatusTypes>  Status,
                                  IReadOnlyDictionary<String, Object>   CustomData  = null)

            : base(CustomData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region (static) Snapshot(ChargingPool)

        /// <summary>
        /// Take a snapshot of the current charging pool status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingPoolStatus Snapshot(ChargingPool ChargingPool)

            => new ChargingPoolStatus(ChargingPool.Id,
                                      ChargingPool.Status);

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolStatus ChargingPoolStatus1, ChargingPoolStatus ChargingPoolStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingPoolStatus1, ChargingPoolStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolStatus1 == null) || ((Object) ChargingPoolStatus2 == null))
                return false;

            return ChargingPoolStatus1.Equals(ChargingPoolStatus2);

        }

        #endregion

        #region Operator != (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolStatus ChargingPoolStatus1, ChargingPoolStatus ChargingPoolStatus2)
            => !(ChargingPoolStatus1 == ChargingPoolStatus2);

        #endregion

        #region Operator <  (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolStatus ChargingPoolStatus1, ChargingPoolStatus ChargingPoolStatus2)
        {

            if ((Object) ChargingPoolStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolStatus1), "The given ChargingPoolStatus1 must not be null!");

            return ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolStatus ChargingPoolStatus1, ChargingPoolStatus ChargingPoolStatus2)
            => !(ChargingPoolStatus1 > ChargingPoolStatus2);

        #endregion

        #region Operator >  (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolStatus ChargingPoolStatus1, ChargingPoolStatus ChargingPoolStatus2)
        {

            if ((Object) ChargingPoolStatus1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolStatus1), "The given ChargingPoolStatus1 must not be null!");

            return ChargingPoolStatus1.CompareTo(ChargingPoolStatus2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolStatus1, ChargingPoolStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus1">A charging pool status.</param>
        /// <param name="ChargingPoolStatus2">Another charging pool status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolStatus ChargingPoolStatus1, ChargingPoolStatus ChargingPoolStatus2)
            => !(ChargingPoolStatus1 < ChargingPoolStatus2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPoolStatus))
                throw new ArgumentException("The given object is not a ChargingPoolStatus!",
                                            nameof(Object));

            return CompareTo((ChargingPoolStatus) Object);

        }

        #endregion

        #region CompareTo(ChargingPoolStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatus">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatus ChargingPoolStatus)
        {

            if ((Object) ChargingPoolStatus == null)
                throw new ArgumentNullException(nameof(ChargingPoolStatus), "The given ChargingPoolStatus must not be null!");

            // Compare ChargingPool Ids
            var _Result = Id.CompareTo(ChargingPoolStatus.Id);

            // If equal: Compare ChargingPool status
            if (_Result == 0)
                _Result = Status.CompareTo(ChargingPoolStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatus> Members

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

            if (!(Object is ChargingPoolStatus))
                return false;

            return Equals((ChargingPoolStatus) Object);

        }

        #endregion

        #region Equals(ChargingPoolStatus)

        /// <summary>
        /// Compares two ChargingPool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolStatus">A charging pool identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolStatus ChargingPoolStatus)
        {

            if ((Object) ChargingPoolStatus == null)
                return false;

            return Id.    Equals(ChargingPoolStatus.Id) &&
                   Status.Equals(ChargingPoolStatus.Status);

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
