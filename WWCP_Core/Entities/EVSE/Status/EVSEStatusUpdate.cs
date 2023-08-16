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
    /// An EVSE status update.
    /// </summary>
    public readonly struct EVSEStatusUpdate : IEquatable<EVSEStatusUpdate>,
                                              IComparable<EVSEStatusUpdate>,
                                              IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                        Id           { get; }

        /// <summary>
        /// The new timestamped status of the EVSE.
        /// </summary>
        public Timestamped<EVSEStatusTypes>   NewStatus    { get; }

        /// <summary>
        /// The optional old timestamped status of the EVSE.
        /// </summary>
        public Timestamped<EVSEStatusTypes>?  OldStatus    { get; }

        /// <summary>
        /// An optional data source or context for this EVSE status update.
        /// </summary>
        public Context?                       Context      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE.</param>
        /// <param name="OldStatus">The optional old timestamped status of the EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE status update.</param>
        public EVSEStatusUpdate(EVSE_Id                        Id,
                                Timestamped<EVSEStatusTypes>   NewStatus,
                                Timestamped<EVSEStatusTypes>?  OldStatus   = null,
                                Context?                       Context     = null)

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
        /// Take a snapshot of the current EVSE status.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE status update.</param>
        public static EVSEStatusUpdate Snapshot(IEVSE     EVSE,
                                                Context?  Context   = null)

            => new (EVSE.Id,
                    EVSE.Status,
                    EVSE.StatusSchedule().Skip(1).FirstOrDefault(),
                    Context);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.Equals(EVSEStatusUpdate2);

        #endregion

        #region Operator != (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => !EVSEStatusUpdate1.Equals(EVSEStatusUpdate2);

        #endregion

        #region Operator <  (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatusUpdate EVSEStatusUpdate1,
                                          EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) < 0;

        #endregion

        #region Operator <= (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatusUpdate EVSEStatusUpdate1,
                                          EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) > 0;

        #endregion

        #region Operator >= (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">An EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatusUpdate EVSEStatusUpdate1,
                                           EVSEStatusUpdate EVSEStatusUpdate2)

            => EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE status updates.
        /// </summary>
        /// <param name="Object">An EVSE status update to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEStatusUpdate evseStatusUpdate
                   ? CompareTo(evseStatusUpdate)
                   : throw new ArgumentException("The given object is not an EVSE status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates.
        /// </summary>
        /// <param name="EVSEStatusUpdate">An EVSE status update to compare with.</param>
        public Int32 CompareTo(EVSEStatusUpdate EVSEStatusUpdate)
        {

            var c = Id.             CompareTo(EVSEStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(EVSEStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && EVSEStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(EVSEStatusUpdate.OldStatus.Value);

            if (c == 0 && Context is not null && EVSEStatusUpdate.Context is not null)
                c = Context.        CompareTo(EVSEStatusUpdate.Context);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="Object">An EVSE status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEStatusUpdate evseStatusUpdate &&
                   Equals(evseStatusUpdate);

        #endregion

        #region Equals(EVSEStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="EVSEStatusUpdate">An EVSE status update to compare with.</param>
        public Boolean Equals(EVSEStatusUpdate EVSEStatusUpdate)

            => Id.       Equals(EVSEStatusUpdate.Id)        &&
               NewStatus.Equals(EVSEStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue  && !EVSEStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue  &&  EVSEStatusUpdate.OldStatus.HasValue  && OldStatus.Value.Equals(EVSEStatusUpdate.OldStatus.Value))) &&

            (( Context is null     &&  EVSEStatusUpdate.Context is null) ||
              (Context is not null &&  EVSEStatusUpdate.Context is not null && Context.        Equals(EVSEStatusUpdate.Context)));

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
