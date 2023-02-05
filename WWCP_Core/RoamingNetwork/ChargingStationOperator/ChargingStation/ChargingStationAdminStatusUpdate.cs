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
    /// A charging station admin status update.
    /// </summary>
    public readonly struct ChargingStationAdminStatusUpdate : IEquatable<ChargingStationAdminStatusUpdate>,
                                                              IComparable<ChargingStationAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                            Id          { get; }

        /// <summary>
        /// The old timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped admin status of the charging station.
        /// </summary>
        public Timestamped<ChargingStationAdminStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldStatus">The old timestamped admin status of the charging station.</param>
        /// <param name="NewStatus">The new timestamped admin status of the charging station.</param>
        public ChargingStationAdminStatusUpdate(ChargingStation_Id                            Id,
                                                Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                                Timestamped<ChargingStationAdminStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(ChargingStation)

        /// <summary>
        /// Take a snapshot of the current charging station admin status.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public static ChargingStationAdminStatusUpdate Snapshot(IChargingStation ChargingStation)

            => new (ChargingStation.Id,
                    ChargingStation.AdminStatus,
                    ChargingStation.AdminStatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.Equals(ChargingStationAdminStatusUpdate2);

        #endregion

        #region Operator != (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => !ChargingStationAdminStatusUpdate1.Equals(ChargingStationAdminStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                          ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                          ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingStationAdminStatusUpdate1, ChargingStationAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate1">A charging station admin status update.</param>
        /// <param name="ChargingStationAdminStatusUpdate2">Another charging station admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate1,
                                           ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate2)

            => ChargingStationAdminStatusUpdate1.CompareTo(ChargingStationAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station admin status updates.
        /// </summary>
        /// <param name="Object">A charging station admin status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is ChargingStationAdminStatusUpdate chargingStationAdminStatusUpdate
                   ? CompareTo(chargingStationAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging station admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationAdminStatusUpdate)

        /// <summary>
        /// Compares two charging station admin status updates.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate">A charging station admin status update to compare with.</param>
        public Int32 CompareTo(ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate)
        {

            var c = Id.       CompareTo(ChargingStationAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.CompareTo(ChargingStationAdminStatusUpdate.NewStatus);

            if (c == 0)
                c = OldStatus.CompareTo(ChargingStationAdminStatusUpdate.OldStatus);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station admin status updates for equality.
        /// </summary>
        /// <param name="Object">A charging station admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationAdminStatusUpdate chargingStationAdminStatusUpdate &&
                   Equals(chargingStationAdminStatusUpdate);

        #endregion

        #region Equals(ChargingStationAdminStatusUpdate)

        /// <summary>
        /// Compares two charging station admin status updates for equality.
        /// </summary>
        /// <param name="ChargingStationAdminStatusUpdate">A charging station admin status update to compare with.</param>
        public Boolean Equals(ChargingStationAdminStatusUpdate ChargingStationAdminStatusUpdate)

            => Id.       Equals(ChargingStationAdminStatusUpdate.Id)        &&
               OldStatus.Equals(ChargingStationAdminStatusUpdate.OldStatus) &&
               NewStatus.Equals(ChargingStationAdminStatusUpdate.NewStatus);

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
