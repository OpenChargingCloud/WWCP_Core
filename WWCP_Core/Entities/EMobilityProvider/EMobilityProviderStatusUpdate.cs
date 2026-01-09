/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A e-mobility provider status update.
    /// </summary>
    public readonly struct EMobilityProviderStatusUpdate : IEquatable<EMobilityProviderStatusUpdate>,
                                                           IComparable<EMobilityProviderStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public EMobilityProvider_Id                       Id           { get; }

        /// <summary>
        /// The old timestamped status of the e-mobility provider.
        /// </summary>
        public Timestamped<EMobilityProviderStatusTypes>  OldStatus    { get; }

        /// <summary>
        /// The new timestamped status of the e-mobility provider.
        /// </summary>
        public Timestamped<EMobilityProviderStatusTypes>  NewStatus    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility provider status update.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility provider.</param>
        /// <param name="OldStatus">The old timestamped status of the e-mobility provider.</param>
        /// <param name="NewStatus">The new timestamped status of the e-mobility provider.</param>
        public EMobilityProviderStatusUpdate(EMobilityProvider_Id                       Id,
                                                   Timestamped<EMobilityProviderStatusTypes>  OldStatus,
                                                   Timestamped<EMobilityProviderStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(EMobilityProvider)

        /// <summary>
        /// Take a snapshot of the current e-mobility provider status.
        /// </summary>
        /// <param name="EMobilityProvider">A e-mobility provider.</param>
        public static EMobilityProviderStatusUpdate Snapshot(IEMobilityProvider EMobilityProvider)

            => new (EMobilityProvider.Id,
                    EMobilityProvider.Status,
                    EMobilityProvider.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (EMobilityProviderStatusUpdate1, EMobilityProviderStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate1">A e-mobility provider status update.</param>
        /// <param name="EMobilityProviderStatusUpdate2">Another e-mobility provider status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate1,
                                           EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate2)

            => EMobilityProviderStatusUpdate1.Equals(EMobilityProviderStatusUpdate2);

        #endregion

        #region Operator != (EMobilityProviderStatusUpdate1, EMobilityProviderStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate1">A e-mobility provider status update.</param>
        /// <param name="EMobilityProviderStatusUpdate2">Another e-mobility provider status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate1,
                                           EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate2)

            => !EMobilityProviderStatusUpdate1.Equals(EMobilityProviderStatusUpdate2);

        #endregion

        #region Operator <  (EMobilityProviderStatusUpdate1, EMobilityProviderStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate1">A e-mobility provider status update.</param>
        /// <param name="EMobilityProviderStatusUpdate2">Another e-mobility provider status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate1,
                                          EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate2)

            => EMobilityProviderStatusUpdate1.CompareTo(EMobilityProviderStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EMobilityProviderStatusUpdate1, EMobilityProviderStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate1">A e-mobility provider status update.</param>
        /// <param name="EMobilityProviderStatusUpdate2">Another e-mobility provider status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate1,
                                           EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate2)

            => EMobilityProviderStatusUpdate1.CompareTo(EMobilityProviderStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EMobilityProviderStatusUpdate1, EMobilityProviderStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate1">A e-mobility provider status update.</param>
        /// <param name="EMobilityProviderStatusUpdate2">Another e-mobility provider status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate1,
                                          EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate2)

            => EMobilityProviderStatusUpdate1.CompareTo(EMobilityProviderStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EMobilityProviderStatusUpdate1, EMobilityProviderStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate1">A e-mobility provider status update.</param>
        /// <param name="EMobilityProviderStatusUpdate2">Another e-mobility provider status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate1,
                                           EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate2)

            => EMobilityProviderStatusUpdate1.CompareTo(EMobilityProviderStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EMobilityProviderStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two e-mobility provider status updates.
        /// </summary>
        /// <param name="Object">A e-mobility provider status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is EMobilityProviderStatusUpdate chargingStationOperatorStatusUpdate
                   ? CompareTo(chargingStationOperatorStatusUpdate)
                   : throw new ArgumentException("The given object is not a e-mobility provider status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMobilityProviderStatusUpdate)

        /// <summary>
        /// Compares two e-mobility provider status updates.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate">A e-mobility provider status update to compare with.</param>
        public Int32 CompareTo(EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate)
        {

            var c = Id.       CompareTo(EMobilityProviderStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.CompareTo(EMobilityProviderStatusUpdate.NewStatus);

            if (c == 0)
                c = OldStatus.CompareTo(EMobilityProviderStatusUpdate.OldStatus);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EMobilityProviderStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two e-mobility provider status updates for equality.
        /// </summary>
        /// <param name="Object">A e-mobility provider status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityProviderStatusUpdate chargingStationOperatorStatusUpdate &&
                   Equals(chargingStationOperatorStatusUpdate);

        #endregion

        #region Equals(EMobilityProviderStatusUpdate)

        /// <summary>
        /// Compares two e-mobility provider status updates for equality.
        /// </summary>
        /// <param name="EMobilityProviderStatusUpdate">A e-mobility provider status update to compare with.</param>
        public Boolean Equals(EMobilityProviderStatusUpdate EMobilityProviderStatusUpdate)

            => Id.       Equals(EMobilityProviderStatusUpdate.Id)        &&
               OldStatus.Equals(EMobilityProviderStatusUpdate.OldStatus) &&
               NewStatus.Equals(EMobilityProviderStatusUpdate.NewStatus);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
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
