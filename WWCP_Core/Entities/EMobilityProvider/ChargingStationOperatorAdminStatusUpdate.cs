/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A e-mobility provider admin status update.
    /// </summary>
    public readonly struct EMobilityProviderAdminStatusUpdate : IEquatable<EMobilityProviderAdminStatusUpdate>,
                                                                IComparable<EMobilityProviderAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public EMobilityProvider_Id                            Id           { get; }

        /// <summary>
        /// The old timestamped admin status of the e-mobility provider.
        /// </summary>
        public Timestamped<EMobilityProviderAdminStatusTypes>  OldStatus    { get; }

        /// <summary>
        /// The new timestamped admin status of the e-mobility provider.
        /// </summary>
        public Timestamped<EMobilityProviderAdminStatusTypes>  NewStatus    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility provider admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the e-mobility provider.</param>
        /// <param name="OldStatus">The old timestamped admin status of the e-mobility provider.</param>
        /// <param name="NewStatus">The new timestamped admin status of the e-mobility provider.</param>
        public EMobilityProviderAdminStatusUpdate(EMobilityProvider_Id                            Id,
                                                        Timestamped<EMobilityProviderAdminStatusTypes>  OldStatus,
                                                        Timestamped<EMobilityProviderAdminStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(EMobilityProvider)

        /// <summary>
        /// Take a snapshot of the current e-mobility provider admin status.
        /// </summary>
        /// <param name="EMobilityProvider">A e-mobility provider.</param>
        public static EMobilityProviderAdminStatusUpdate Snapshot(IEMobilityProvider EMobilityProvider)

            => new (EMobilityProvider.Id,
                    EMobilityProvider.AdminStatus,
                    EMobilityProvider.AdminStatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (EMobilityProviderAdminStatusUpdate1, EMobilityProviderAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate1">A e-mobility provider admin status update.</param>
        /// <param name="EMobilityProviderAdminStatusUpdate2">Another e-mobility provider admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate1,
                                           EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate2)

            => EMobilityProviderAdminStatusUpdate1.Equals(EMobilityProviderAdminStatusUpdate2);

        #endregion

        #region Operator != (EMobilityProviderAdminStatusUpdate1, EMobilityProviderAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate1">A e-mobility provider admin status update.</param>
        /// <param name="EMobilityProviderAdminStatusUpdate2">Another e-mobility provider admin status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate1,
                                           EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate2)

            => !EMobilityProviderAdminStatusUpdate1.Equals(EMobilityProviderAdminStatusUpdate2);

        #endregion

        #region Operator <  (EMobilityProviderAdminStatusUpdate1, EMobilityProviderAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate1">A e-mobility provider admin status update.</param>
        /// <param name="EMobilityProviderAdminStatusUpdate2">Another e-mobility provider admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate1,
                                          EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate2)

            => EMobilityProviderAdminStatusUpdate1.CompareTo(EMobilityProviderAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EMobilityProviderAdminStatusUpdate1, EMobilityProviderAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate1">A e-mobility provider admin status update.</param>
        /// <param name="EMobilityProviderAdminStatusUpdate2">Another e-mobility provider admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate1,
                                           EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate2)

            => EMobilityProviderAdminStatusUpdate1.CompareTo(EMobilityProviderAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EMobilityProviderAdminStatusUpdate1, EMobilityProviderAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate1">A e-mobility provider admin status update.</param>
        /// <param name="EMobilityProviderAdminStatusUpdate2">Another e-mobility provider admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate1,
                                          EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate2)

            => EMobilityProviderAdminStatusUpdate1.CompareTo(EMobilityProviderAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EMobilityProviderAdminStatusUpdate1, EMobilityProviderAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate1">A e-mobility provider admin status update.</param>
        /// <param name="EMobilityProviderAdminStatusUpdate2">Another e-mobility provider admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate1,
                                           EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate2)

            => EMobilityProviderAdminStatusUpdate1.CompareTo(EMobilityProviderAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EMobilityProviderAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two e-mobility provider admin status updates.
        /// </summary>
        /// <param name="Object">A e-mobility provider admin status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is EMobilityProviderAdminStatusUpdate chargingStationOperatorAdminStatusUpdate
                   ? CompareTo(chargingStationOperatorAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not a e-mobility provider admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMobilityProviderAdminStatusUpdate)

        /// <summary>
        /// Compares two e-mobility provider admin status updates.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate">A e-mobility provider admin status update to compare with.</param>
        public Int32 CompareTo(EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate)
        {

            var c = Id.       CompareTo(EMobilityProviderAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.CompareTo(EMobilityProviderAdminStatusUpdate.NewStatus);

            if (c == 0)
                c = OldStatus.CompareTo(EMobilityProviderAdminStatusUpdate.OldStatus);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EMobilityProviderAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two e-mobility provider admin status updates for equality.
        /// </summary>
        /// <param name="Object">A e-mobility provider admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMobilityProviderAdminStatusUpdate chargingStationOperatorAdminStatusUpdate &&
                   Equals(chargingStationOperatorAdminStatusUpdate);

        #endregion

        #region Equals(EMobilityProviderAdminStatusUpdate)

        /// <summary>
        /// Compares two e-mobility provider admin status updates for equality.
        /// </summary>
        /// <param name="EMobilityProviderAdminStatusUpdate">A e-mobility provider admin status update to compare with.</param>
        public Boolean Equals(EMobilityProviderAdminStatusUpdate EMobilityProviderAdminStatusUpdate)

            => Id.       Equals(EMobilityProviderAdminStatusUpdate.Id)        &&
               OldStatus.Equals(EMobilityProviderAdminStatusUpdate.OldStatus) &&
               NewStatus.Equals(EMobilityProviderAdminStatusUpdate.NewStatus);

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
