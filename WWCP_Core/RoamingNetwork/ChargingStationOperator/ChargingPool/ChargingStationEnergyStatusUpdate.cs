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
    /// A charging pool energy status update.
    /// </summary>
    public readonly struct ChargingPoolEnergyStatusUpdate : IEquatable<ChargingPoolEnergyStatusUpdate>,
                                                               IComparable<ChargingPoolEnergyStatusUpdate>,
                                                               IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging pool.
        /// </summary>
        public ChargingPool_Id      Id                    { get; }

        /// <summary>
        /// The old timestamped energy usage of the charging pool.
        /// </summary>
        public Timestamped<Double>  OldEnergyUsage        { get; }

        /// <summary>
        /// The new timestamped energy usage of the charging pool.
        /// </summary>
        public Timestamped<Double>  NewEnergyUsage        { get; }

        /// <summary>
        /// The old timestamped available energy of the charging pool.
        /// </summary>
        public Timestamped<Double>  OldAvailableEnergy    { get; }

        /// <summary>
        /// The new timestamped available energy of the charging pool.
        /// </summary>
        public Timestamped<Double>  NewAvailableEnergy    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool energy status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="OldEnergyUsage">The old timestamped energy usage of the charging pool.</param>
        /// <param name="NewEnergyUsage">The new timestamped energy usage of the charging pool.</param>
        /// <param name="OldAvailableEnergy">The old timestamped available energy of the charging pool.</param>
        /// <param name="NewAvailableEnergy">The new timestamped available energy of the charging pool.</param>
        public ChargingPoolEnergyStatusUpdate(ChargingPool_Id      Id,
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

        #region Operator == (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate1,
                                           ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate2)

            => ChargingPoolEnergyStatusUpdate1.Equals(ChargingPoolEnergyStatusUpdate2);

        #endregion

        #region Operator != (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate1,
                                           ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate2)

            => !ChargingPoolEnergyStatusUpdate1.Equals(ChargingPoolEnergyStatusUpdate2);

        #endregion

        #region Operator <  (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate1,
                                          ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate2)

            => ChargingPoolEnergyStatusUpdate1.CompareTo(ChargingPoolEnergyStatusUpdate2) < 0;

        #endregion

        #region Operator <= (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate1,
                                           ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate2)

            => ChargingPoolEnergyStatusUpdate1.CompareTo(ChargingPoolEnergyStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate1,
                                          ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate2)

            => ChargingPoolEnergyStatusUpdate1.CompareTo(ChargingPoolEnergyStatusUpdate2) > 0;

        #endregion

        #region Operator >= (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate1,
                                           ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate2)

            => ChargingPoolEnergyStatusUpdate1.CompareTo(ChargingPoolEnergyStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolEnergyStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool energy status updates.
        /// </summary>
        /// <param name="Object">A charging pool energy status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolEnergyStatusUpdate chargingPoolEnergyStatusUpdate
                   ? CompareTo(chargingPoolEnergyStatusUpdate)
                   : throw new ArgumentException("The given object is not a charging pool energy status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolEnergyStatusUpdate)

        /// <summary>
        /// Compares two charging pool energy status updates.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate">A charging pool energy status update to compare with.</param>
        public Int32 CompareTo(ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate)
        {

            var c = Id.                CompareTo(ChargingPoolEnergyStatusUpdate.Id);

            if (c == 0)
                c = NewEnergyUsage.    CompareTo(ChargingPoolEnergyStatusUpdate.NewEnergyUsage);

            if (c == 0)
                c = OldEnergyUsage.    CompareTo(ChargingPoolEnergyStatusUpdate.OldEnergyUsage);

            if (c == 0)
                c = OldAvailableEnergy.CompareTo(ChargingPoolEnergyStatusUpdate.OldAvailableEnergy);

            if (c == 0)
                c = NewAvailableEnergy.CompareTo(ChargingPoolEnergyStatusUpdate.NewAvailableEnergy);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolEnergyStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool energy status updates for equality.
        /// </summary>
        /// <param name="Object">A charging pool energy status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolEnergyStatusUpdate chargingPoolEnergyStatusUpdate &&
                   Equals(chargingPoolEnergyStatusUpdate);

        #endregion

        #region Equals(ChargingPoolEnergyStatusUpdate)

        /// <summary>
        /// Compares two charging pool energy status updates for equality.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate">A charging pool energy status update to compare with.</param>
        public Boolean Equals(ChargingPoolEnergyStatusUpdate ChargingPoolEnergyStatusUpdate)

            => Id.                Equals(ChargingPoolEnergyStatusUpdate.Id)                 &&
               OldEnergyUsage.    Equals(ChargingPoolEnergyStatusUpdate.OldEnergyUsage)     &&
               NewEnergyUsage.    Equals(ChargingPoolEnergyStatusUpdate.NewEnergyUsage)     &&
               OldAvailableEnergy.Equals(ChargingPoolEnergyStatusUpdate.OldAvailableEnergy) &&
               NewAvailableEnergy.Equals(ChargingPoolEnergyStatusUpdate.NewAvailableEnergy);

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
