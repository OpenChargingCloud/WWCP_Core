/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging pool status update.
    /// </summary>
    public struct ChargingPoolStatusUpdate : IEquatable <ChargingPoolStatusUpdate>,
                                             IComparable<ChargingPoolStatusUpdate>,
                                             IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id                       Id           { get; }

        /// <summary>
        /// The old timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>  OldStatus    { get; }

        /// <summary>
        /// The new timestamped status of the charging pool.
        /// </summary>
        public Timestamped<ChargingPoolStatusTypes>  NewStatus    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="OldStatus">The old timestamped status of the charging pool.</param>
        /// <param name="NewStatus">The new timestamped status of the charging pool.</param>
        public ChargingPoolStatusUpdate(ChargingPool_Id                       Id,
                                        Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                        Timestamped<ChargingPoolStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(ChargingPool)

        /// <summary>
        /// Take a snapshot of the current charging pool status.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        public static ChargingPoolStatusUpdate Snapshot(IChargingPool ChargingPool)

            => new (ChargingPool.Id,
                    ChargingPool.Status,
                    ChargingPool.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.Equals(ChargingPoolStatusUpdate2);

        #endregion

        #region Operator != (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => !ChargingPoolStatusUpdate1.Equals(ChargingPoolStatusUpdate2);

        #endregion

        #region Operator <  (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                          ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                          ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingPoolStatusUpdate1, ChargingPoolStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate1">A charging pool status update.</param>
        /// <param name="ChargingPoolStatusUpdate2">Another charging pool status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolStatusUpdate ChargingPoolStatusUpdate1,
                                           ChargingPoolStatusUpdate ChargingPoolStatusUpdate2)

            => ChargingPoolStatusUpdate1.CompareTo(ChargingPoolStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool status updates.
        /// </summary>
        /// <param name="Object">A charging pool status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolStatusUpdate chargingPoolStatusUpdate
                   ? CompareTo(chargingPoolStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging pool status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolStatusUpdate)

        /// <summary>
        /// Compares two charging pool status updates.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate">A charging pool status update to compare with.</param>
        public Int32 CompareTo(ChargingPoolStatusUpdate ChargingPoolStatusUpdate)
        {

            var c = Id.       CompareTo(ChargingPoolStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.CompareTo(ChargingPoolStatusUpdate.NewStatus);

            if (c == 0)
                c = OldStatus.CompareTo(ChargingPoolStatusUpdate.OldStatus);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool status updates for equality.
        /// </summary>
        /// <param name="Object">A charging pool status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolStatusUpdate chargingPoolStatusUpdate &&
                   Equals(chargingPoolStatusUpdate);

        #endregion

        #region Equals(ChargingPoolStatusUpdate)

        /// <summary>
        /// Compares two charging pool status updates for equality.
        /// </summary>
        /// <param name="ChargingPoolStatusUpdate">A charging pool status update to compare with.</param>
        public Boolean Equals(ChargingPoolStatusUpdate ChargingPoolStatusUpdate)

            => Id.       Equals(ChargingPoolStatusUpdate.Id)        &&
               OldStatus.Equals(ChargingPoolStatusUpdate.OldStatus) &&
               NewStatus.Equals(ChargingPoolStatusUpdate.NewStatus);

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

                return Id.       GetHashCode() * 5 ^
                       OldStatus.GetHashCode() * 3 ^
                       NewStatus.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, ": ",
                             OldStatus,
                             " -> ",
                             NewStatus);

        #endregion

    }

}
