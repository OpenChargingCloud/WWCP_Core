/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
                                                    IComparable<EVSEEnergyStatusUpdate>,
                                                    IComparable
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                   Id               { get; }

        /// <summary>
        /// The new timestamped energy information of the EVSE.
        /// </summary>
        public Timestamped<EnergyInfo>   NewEnergyInfo    { get; }

        /// <summary>
        /// The optional old timestamped energy information of the EVSE.
        /// </summary>
        public Timestamped<EnergyInfo>?  OldEnergyInfo    { get; }

        /// <summary>
        /// An optional data source or context for this EVSE energy information update.
        /// </summary>
        public String?                   Context          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE energy status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="OldEnergyInfo">The old timestamped energy information of the EVSE.</param>
        /// <param name="NewEnergyInfo">The new timestamped energy information of the EVSE.</param>
        /// <param name="Context">An optional data source or context for the EVSE energy information update.</param>
        public EVSEEnergyStatusUpdate(EVSE_Id                   Id,
                                      Timestamped<EnergyInfo>   NewEnergyInfo,
                                      Timestamped<EnergyInfo>?  OldEnergyInfo   = null,
                                      String?                   Context         = null)

        {

            this.Id             = Id;
            this.NewEnergyInfo  = NewEnergyInfo;
            this.OldEnergyInfo  = OldEnergyInfo;
            this.Context        = Context;

            unchecked
            {

                hashCode = Id.            GetHashCode()       * 7 ^
                           NewEnergyInfo. GetHashCode()       * 5 ^
                          (OldEnergyInfo?.GetHashCode() ?? 0) * 3 ^
                          (Context?.      GetHashCode() ?? 0);

            }

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
        public Int32 CompareTo(Object? Object)

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

            var c = Id.                 CompareTo(EVSEEnergyStatusUpdate.Id);

            if (c == 0)
                c = NewEnergyInfo.      CompareTo(EVSEEnergyStatusUpdate.NewEnergyInfo);

            if (c == 0 && OldEnergyInfo.HasValue && EVSEEnergyStatusUpdate.OldEnergyInfo.HasValue)
                c = OldEnergyInfo.Value.CompareTo(EVSEEnergyStatusUpdate.OldEnergyInfo.Value);

            if (c == 0 && Context is not null && EVSEEnergyStatusUpdate.Context is not null)
                c = Context.            CompareTo(EVSEEnergyStatusUpdate.Context);

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

            => Id.           Equals(EVSEEnergyStatusUpdate.Id)            &&
               NewEnergyInfo.Equals(EVSEEnergyStatusUpdate.NewEnergyInfo) &&

            ((!OldEnergyInfo.HasValue && !EVSEEnergyStatusUpdate.OldEnergyInfo.HasValue) ||
              (OldEnergyInfo.HasValue &&  EVSEEnergyStatusUpdate.OldEnergyInfo.HasValue && OldEnergyInfo.Value.Equals(EVSEEnergyStatusUpdate.OldEnergyInfo.Value))) &&

            (( Context is null        &&  EVSEEnergyStatusUpdate.Context is null) ||
              (Context is not null    &&  EVSEEnergyStatusUpdate.Context is not null && Context.               Equals(EVSEEnergyStatusUpdate.Context)));

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

            => $"{Id}: {(OldEnergyInfo.HasValue ? $"'{OldEnergyInfo.Value}' -> " : "")}'{NewEnergyInfo}'{(Context is not null ? $" ({Context})" : "")}";

        #endregion

    }

}
