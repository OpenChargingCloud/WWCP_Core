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
        public ChargingPool_Id           Id               { get; }

        /// <summary>
        /// The new timestamped energy information of the charging pool.
        /// </summary>
        public Timestamped<EnergyInfo>   NewEnergyInfo    { get; }

        /// <summary>
        /// The optional old timestamped energy information of the charging pool.
        /// </summary>
        public Timestamped<EnergyInfo>?  OldEnergyInfo    { get; }

        /// <summary>
        /// An optional data source or context for this charging pool energy information update.
        /// </summary>
        public String?                   DataSource       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool energy status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging pool.</param>
        /// <param name="OldEnergyInfo">The old timestamped energy information of the EVSE.</param>
        /// <param name="NewEnergyInfo">The new timestamped energy information of the EVSE.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE energy information update.</param>
        public ChargingPoolEnergyStatusUpdate(ChargingPool_Id           Id,
                                              Timestamped<EnergyInfo>   NewEnergyInfo,
                                              Timestamped<EnergyInfo>?  OldEnergyInfo   = null,
                                              String?                   DataSource      = null)

        {

            this.Id             = Id;
            this.NewEnergyInfo  = NewEnergyInfo;
            this.OldEnergyInfo  = OldEnergyInfo;
            this.DataSource     = DataSource;

            unchecked
            {

                hashCode = Id.            GetHashCode()       * 7 ^
                           NewEnergyInfo. GetHashCode()       * 5 ^
                          (OldEnergyInfo?.GetHashCode() ?? 0) * 3 ^
                          (DataSource?.   GetHashCode() ?? 0);

            }

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolEnergyStatusUpdate1, ChargingPoolEnergyStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolEnergyStatusUpdate1">A charging pool energy status update.</param>
        /// <param name="ChargingPoolEnergyStatusUpdate2">Another charging pool energy status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>False if both match; True otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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
        /// <returns>True if both match; False otherwise.</returns>
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

            var c = Id.                 CompareTo(ChargingPoolEnergyStatusUpdate.Id);

            if (c == 0)
                c = NewEnergyInfo.      CompareTo(ChargingPoolEnergyStatusUpdate.NewEnergyInfo);

            if (c == 0 && OldEnergyInfo.HasValue && ChargingPoolEnergyStatusUpdate.OldEnergyInfo.HasValue)
                c = OldEnergyInfo.Value.CompareTo(ChargingPoolEnergyStatusUpdate.OldEnergyInfo.Value);

            if (c == 0 && DataSource is not null && ChargingPoolEnergyStatusUpdate.DataSource is not null)
                c = DataSource.         CompareTo(ChargingPoolEnergyStatusUpdate.DataSource);

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

            => Id.           Equals(ChargingPoolEnergyStatusUpdate.Id)            &&
               NewEnergyInfo.Equals(ChargingPoolEnergyStatusUpdate.NewEnergyInfo) &&

            ((!OldEnergyInfo.HasValue && !ChargingPoolEnergyStatusUpdate.OldEnergyInfo.HasValue) ||
              (OldEnergyInfo.HasValue &&  ChargingPoolEnergyStatusUpdate.OldEnergyInfo.HasValue && OldEnergyInfo.Value.Equals(ChargingPoolEnergyStatusUpdate.OldEnergyInfo.Value))) &&

            (( DataSource is null     &&  ChargingPoolEnergyStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingPoolEnergyStatusUpdate.DataSource is not null && DataSource.         Equals(ChargingPoolEnergyStatusUpdate.DataSource)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Id}: {(OldEnergyInfo.HasValue ? $"'{OldEnergyInfo.Value}' -> " : "")}'{NewEnergyInfo}'{(DataSource is not null ? $" ({DataSource})" : "")}";

        #endregion

    }

}
