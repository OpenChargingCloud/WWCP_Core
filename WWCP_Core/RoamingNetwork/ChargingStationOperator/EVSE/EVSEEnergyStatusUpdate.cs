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
    /// An EVSE energy status update.
    /// </summary>
    public readonly struct EVSEEnergyStatusUpdate : IEquatable<EVSEEnergyStatusUpdate>,
                                                    IComparable<EVSEEnergyStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id              Id           { get; }

        /// <summary>
        /// The old timestamped energy usage of the EVSE.
        /// </summary>
        public Timestamped<Double>  OldEnergyUsage        { get; }

        /// <summary>
        /// The new timestamped energy usage of the EVSE.
        /// </summary>
        public Timestamped<Double>  NewEnergyUsage        { get; }

        /// <summary>
        /// The old timestamped available energy of the EVSE.
        /// </summary>
        public Timestamped<Double>  OldAvailableEnergy    { get; }

        /// <summary>
        /// The new timestamped available energy of the EVSE.
        /// </summary>
        public Timestamped<Double>  NewAvailableEnergy    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE energy status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="OldEnergyUsage">The old timestamped energy usage of the EVSE.</param>
        /// <param name="NewEnergyUsage">The new timestamped energy usage of the EVSE.</param>
        /// <param name="OldAvailableEnergy">The old timestamped available energy of the EVSE.</param>
        /// <param name="NewAvailableEnergy">The new timestamped available energy of the EVSE.</param>
        public EVSEEnergyStatusUpdate(EVSE_Id              Id,
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

        #region Operator == (EVSEEnergyStatusUpdate1, EVSEEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEEnergyStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate1,
                                           EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate2)

            => EVSEEnergyStatusUpdate1.Equals(EVSEEnergyStatusUpdate2);

        #endregion

        #region Operator != (EVSEEnergyStatusUpdate1, EVSEEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEEnergyStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate1,
                                           EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate2)

            => !EVSEEnergyStatusUpdate1.Equals(EVSEEnergyStatusUpdate2);

        #endregion

        #region Operator <  (EVSEEnergyStatusUpdate1, EVSEEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEEnergyStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate1,
                                          EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate2)

            => EVSEEnergyStatusUpdate1.CompareTo(EVSEEnergyStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EVSEEnergyStatusUpdate1, EVSEEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEEnergyStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate1,
                                           EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate2)

            => EVSEEnergyStatusUpdate1.CompareTo(EVSEEnergyStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EVSEEnergyStatusUpdate1, EVSEEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEEnergyStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate1,
                                          EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate2)

            => EVSEEnergyStatusUpdate1.CompareTo(EVSEEnergyStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EVSEEnergyStatusUpdate1, EVSEEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEEnergyStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate1,
                                           EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate2)

            => EVSEEnergyStatusUpdate1.CompareTo(EVSEEnergyStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEEnergyStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE energy status updates.
        /// </summary>
        /// <param name="Object">An EVSE status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is EVSEEnergyStatusUpdate evseEnergyStatusUpdate
                   ? CompareTo(evseEnergyStatusUpdate)
                   : throw new ArgumentException("The given object is not an EVSE energy status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEEnergyStatusUpdate)

        /// <summary>
        /// Compares two EVSE energy status updates.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate">An EVSE energy status update to compare with.</param>
        public Int32 CompareTo(EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate)
        {

            var c = Id.                CompareTo(EVSEEnergyStatusUpdate.Id);

            if (c == 0)
                c = NewEnergyUsage.    CompareTo(EVSEEnergyStatusUpdate.NewEnergyUsage);

            if (c == 0)
                c = OldEnergyUsage.    CompareTo(EVSEEnergyStatusUpdate.OldEnergyUsage);

            if (c == 0)
                c = OldAvailableEnergy.CompareTo(EVSEEnergyStatusUpdate.OldAvailableEnergy);

            if (c == 0)
                c = NewAvailableEnergy.CompareTo(EVSEEnergyStatusUpdate.NewAvailableEnergy);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEEnergyStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE energy status updates for equality.
        /// </summary>
        /// <param name="Object">An EVSE energy status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEEnergyStatusUpdate evseEnergyStatusUpdate &&
                   Equals(evseEnergyStatusUpdate);

        #endregion

        #region Equals(EVSEEnergyStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="EVSEEnergyStatusUpdate">An EVSE status update to compare with.</param>
        public Boolean Equals(EVSEEnergyStatusUpdate EVSEEnergyStatusUpdate)

            => Id.                Equals(EVSEEnergyStatusUpdate.Id)                 &&
               OldEnergyUsage.    Equals(EVSEEnergyStatusUpdate.OldEnergyUsage)     &&
               NewEnergyUsage.    Equals(EVSEEnergyStatusUpdate.NewEnergyUsage)     &&
               OldAvailableEnergy.Equals(EVSEEnergyStatusUpdate.OldAvailableEnergy) &&
               NewAvailableEnergy.Equals(EVSEEnergyStatusUpdate.NewAvailableEnergy);

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
