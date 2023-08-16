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
    /// An EVSE admin status update.
    /// </summary>
    public readonly struct EVSEAdminStatusUpdate : IEquatable<EVSEAdminStatusUpdate>,
                                                   IComparable<EVSEAdminStatusUpdate>,
                                                   IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                             Id           { get; }

        /// <summary>
        /// The new timestamped admin status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusTypes>   NewStatus    { get; }

        /// <summary>
        /// The optional old timestamped admin status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusTypes>?  OldStatus    { get; }

        /// <summary>
        /// An optional data source or context for this EVSE admin status update.
        /// </summary>
        public Context?                            Context      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped admin status of the EVSE.</param>
        /// <param name="OldStatus">The optional old timestamped admin status of the EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE admin status update.</param>
        public EVSEAdminStatusUpdate(EVSE_Id                             Id,
                                     Timestamped<EVSEAdminStatusTypes>   NewStatus,
                                     Timestamped<EVSEAdminStatusTypes>?  OldStatus   = null,
                                     Context?                            Context     = null)

        {

            this.Id         = Id;
            this.NewStatus  = NewStatus;
            this.OldStatus  = OldStatus;
            this.Context    = Context;

            unchecked
            {

                hashCode = Id.        GetHashCode()       * 7 ^
                           NewStatus. GetHashCode()       * 5 ^
                          (OldStatus?.GetHashCode() ?? 0) * 3 ^
                          (Context?.  GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Snapshot(EVSE, Context = null)

        /// <summary>
        /// Take a snapshot of the current EVSE admin status.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE admin status update.</param>
        public static EVSEAdminStatusUpdate Snapshot(IEVSE     EVSE,
                                                     Context?  Context   = null)

            => new (EVSE.Id,
                    EVSE.AdminStatus,
                    EVSE.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                    Context);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">An EVSE admin status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1,
                                           EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)

            => EVSEAdminStatusUpdate1.Equals(EVSEAdminStatusUpdate2);

        #endregion

        #region Operator != (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">An EVSE admin status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1,
                                           EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)

            => !EVSEAdminStatusUpdate1.Equals(EVSEAdminStatusUpdate2);

        #endregion

        #region Operator <  (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">An EVSE admin status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1,
                                          EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)

            => EVSEAdminStatusUpdate1.CompareTo(EVSEAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">An EVSE admin status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1,
                                           EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)

            => EVSEAdminStatusUpdate1.CompareTo(EVSEAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">An EVSE admin status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1,
                                          EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)

            => EVSEAdminStatusUpdate1.CompareTo(EVSEAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">An EVSE admin status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE admin status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1,
                                           EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)

            => EVSEAdminStatusUpdate1.CompareTo(EVSEAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE admin status updates.
        /// </summary>
        /// <param name="Object">An EVSE admin status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEAdminStatusUpdate evseAdminStatusUpdate
                   ? CompareTo(evseAdminStatusUpdate)
                   : throw new ArgumentException("The given object is not an EVSE admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEAdminStatusUpdate)

        /// <summary>
        /// Compares two EVSE admin status updates.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate">An EVSE admin status update to compare with.</param>
        public Int32 CompareTo(EVSEAdminStatusUpdate EVSEAdminStatusUpdate)
        {

            var c = Id.             CompareTo(EVSEAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(EVSEAdminStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && EVSEAdminStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(EVSEAdminStatusUpdate.OldStatus.Value);

            if (c == 0 && Context is not null && EVSEAdminStatusUpdate.Context is not null)
                c = Context.        CompareTo(EVSEAdminStatusUpdate.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE admin status updates for equality.
        /// </summary>
        /// <param name="Object">An EVSE admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEAdminStatusUpdate evseAdminStatusUpdate &&
                   Equals(evseAdminStatusUpdate);

        #endregion

        #region Equals(EVSEAdminStatusUpdate)

        /// <summary>
        /// Compares two EVSE admin status updates for equality.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate">An EVSE admin status update to compare with.</param>
        public Boolean Equals(EVSEAdminStatusUpdate EVSEAdminStatusUpdate)

            => Id.       Equals(EVSEAdminStatusUpdate.Id)        &&
               NewStatus.Equals(EVSEAdminStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue  && !EVSEAdminStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue  &&  EVSEAdminStatusUpdate.OldStatus.HasValue  && OldStatus.Value.Equals(EVSEAdminStatusUpdate.OldStatus.Value))) &&

            (( Context is null     &&  EVSEAdminStatusUpdate.Context is null) ||
              (Context is not null &&  EVSEAdminStatusUpdate.Context is not null && Context.        Equals(EVSEAdminStatusUpdate.Context)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Id}: {(OldStatus.HasValue ? $"'{OldStatus.Value}' -> " : "")}'{NewStatus}'{(Context is not null ? $" ({Context})" : "")}";

        #endregion

    }

}
