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
    /// A charging station energy status update.
    /// </summary>
    public readonly struct ChargingStationEnergyStatusUpdate : IEquatable<ChargingStationEnergyStatusUpdate>,
                                                               IComparable<ChargingStationEnergyStatusUpdate>,
                                                               IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id              Id           { get; }

        /// <summary>
        /// The old timestamped energy usage of the charging station.
        /// </summary>
        public Timestamped<Double>  OldEnergyUsage        { get; }

        /// <summary>
        /// The new timestamped energy usage of the charging station.
        /// </summary>
        public Timestamped<Double>  NewEnergyUsage        { get; }

        /// <summary>
        /// The old timestamped available energy of the charging station.
        /// </summary>
        public Timestamped<Double>  OldAvailableEnergy    { get; }

        /// <summary>
        /// The new timestamped available energy of the charging station.
        /// </summary>
        public Timestamped<Double>  NewAvailableEnergy    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station energy status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldEnergyUsage">The old timestamped energy usage of the charging station.</param>
        /// <param name="NewEnergyUsage">The new timestamped energy usage of the charging station.</param>
        /// <param name="OldAvailableEnergy">The old timestamped available energy of the charging station.</param>
        /// <param name="NewAvailableEnergy">The new timestamped available energy of the charging station.</param>
        public ChargingStationEnergyStatusUpdate(ChargingStation_Id   Id,
                                                 Timestamped<Double>  OldEnergyUsage,
                                                 Timestamped<Double>  NewEnergyUsage,
                                                 Timestamped<Double>  OldAvailableEnergy,
                                                 Timestamped<Double>  NewAvailableEnergy)

        {

            this.Id                  = Id;
            this.OldEnergyUsage      = OldEnergyUsage;
            this.NewEnergyUsage      = NewEnergyUsage;
            this.OldAvailableEnergy  = OldAvailableEnergy;
            this.NewAvailableEnergy  = NewAvailableEnergy;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationEnergyStatusUpdate1, ChargingStationEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate1">A charging station energy status update.</param>
        /// <param name="ChargingStationEnergyStatusUpdate2">Another charging station energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate1,
                                           ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate2)

            => ChargingStationEnergyStatusUpdate1.Equals(ChargingStationEnergyStatusUpdate2);

        #endregion

        #region Operator != (ChargingStationEnergyStatusUpdate1, ChargingStationEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate1">A charging station energy status update.</param>
        /// <param name="ChargingStationEnergyStatusUpdate2">Another charging station energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate1,
                                           ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate2)

            => !ChargingStationEnergyStatusUpdate1.Equals(ChargingStationEnergyStatusUpdate2);

        #endregion

        #region Operator <  (ChargingStationEnergyStatusUpdate1, ChargingStationEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate1">A charging station energy status update.</param>
        /// <param name="ChargingStationEnergyStatusUpdate2">Another charging station energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate1,
                                          ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate2)

            => ChargingStationEnergyStatusUpdate1.CompareTo(ChargingStationEnergyStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingStationEnergyStatusUpdate1, ChargingStationEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate1">A charging station energy status update.</param>
        /// <param name="ChargingStationEnergyStatusUpdate2">Another charging station energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate1,
                                           ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate2)

            => ChargingStationEnergyStatusUpdate1.CompareTo(ChargingStationEnergyStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingStationEnergyStatusUpdate1, ChargingStationEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate1">A charging station energy status update.</param>
        /// <param name="ChargingStationEnergyStatusUpdate2">Another charging station energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate1,
                                          ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate2)

            => ChargingStationEnergyStatusUpdate1.CompareTo(ChargingStationEnergyStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingStationEnergyStatusUpdate1, ChargingStationEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate1">A charging station energy status update.</param>
        /// <param name="ChargingStationEnergyStatusUpdate2">Another charging station energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate1,
                                           ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate2)

            => ChargingStationEnergyStatusUpdate1.CompareTo(ChargingStationEnergyStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationEnergyStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station energy status updates.
        /// </summary>
        /// <param name="Object">A charging station energy status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationEnergyStatusUpdate chargingStationEnergyStatusUpdate
                   ? CompareTo(chargingStationEnergyStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging station energy status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationEnergyStatusUpdate)

        /// <summary>
        /// Compares two charging station energy status updates.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate">A charging station energy status update to compare with.</param>
        public Int32 CompareTo(ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate)
        {

            var c = Id.                CompareTo(ChargingStationEnergyStatusUpdate.Id);

            if (c == 0)
                c = NewEnergyUsage.    CompareTo(ChargingStationEnergyStatusUpdate.NewEnergyUsage);

            if (c == 0)
                c = OldEnergyUsage.    CompareTo(ChargingStationEnergyStatusUpdate.OldEnergyUsage);

            if (c == 0)
                c = OldAvailableEnergy.CompareTo(ChargingStationEnergyStatusUpdate.OldAvailableEnergy);

            if (c == 0)
                c = NewAvailableEnergy.CompareTo(ChargingStationEnergyStatusUpdate.NewAvailableEnergy);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationEnergyStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station energy status updates for equality.
        /// </summary>
        /// <param name="Object">A charging station energy status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationEnergyStatusUpdate chargingStationEnergyStatusUpdate &&
                   Equals(chargingStationEnergyStatusUpdate);

        #endregion

        #region Equals(ChargingStationEnergyStatusUpdate)

        /// <summary>
        /// Compares two charging station energy status updates for equality.
        /// </summary>
        /// <param name="ChargingStationEnergyStatusUpdate">A charging station energy status update to compare with.</param>
        public Boolean Equals(ChargingStationEnergyStatusUpdate ChargingStationEnergyStatusUpdate)

            => Id.                Equals(ChargingStationEnergyStatusUpdate.Id)                 &&
               OldEnergyUsage.    Equals(ChargingStationEnergyStatusUpdate.OldEnergyUsage)     &&
               NewEnergyUsage.    Equals(ChargingStationEnergyStatusUpdate.NewEnergyUsage)     &&
               OldAvailableEnergy.Equals(ChargingStationEnergyStatusUpdate.OldAvailableEnergy) &&
               NewAvailableEnergy.Equals(ChargingStationEnergyStatusUpdate.NewAvailableEnergy);

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

                return Id.                GetHashCode() * 11 ^
                       OldEnergyUsage.    GetHashCode() *  7 ^
                       NewEnergyUsage.    GetHashCode() *  5 ^
                       OldAvailableEnergy.GetHashCode() *  3 ^
                       NewAvailableEnergy.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(
                   Id, ": ",
                   OldEnergyUsage,
                   " -> ",
                   NewEnergyUsage,
                   ", ",
                   OldAvailableEnergy,
                   " -> ",
                   NewAvailableEnergy
               );

        #endregion

    }

}
