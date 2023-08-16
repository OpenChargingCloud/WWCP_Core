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
        public ChargingStation_Id        Id               { get; }

        /// <summary>
        /// The new timestamped energy information of the charging station.
        /// </summary>
        public Timestamped<EnergyInfo>   NewEnergyInfo    { get; }

        /// <summary>
        /// The optional old timestamped energy information of the charging station.
        /// </summary>
        public Timestamped<EnergyInfo>?  OldEnergyInfo    { get; }

        /// <summary>
        /// An optional data source or context for this charging station energy information update.
        /// </summary>
        public String?                   DataSource       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station energy status update.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="OldEnergyInfo">The old timestamped energy information of the EVSE.</param>
        /// <param name="NewEnergyInfo">The new timestamped energy information of the EVSE.</param>
        /// <param name="DataSource">An optional data source or context for the EVSE energy information update.</param>
        public ChargingStationEnergyStatusUpdate(ChargingStation_Id        Id,
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

            var c = Id.                 CompareTo(ChargingStationEnergyStatusUpdate.Id);

            if (c == 0)
                c = NewEnergyInfo.      CompareTo(ChargingStationEnergyStatusUpdate.NewEnergyInfo);

            if (c == 0 && OldEnergyInfo.HasValue && ChargingStationEnergyStatusUpdate.OldEnergyInfo.HasValue)
                c = OldEnergyInfo.Value.CompareTo(ChargingStationEnergyStatusUpdate.OldEnergyInfo.Value);

            if (c == 0 && DataSource is not null && ChargingStationEnergyStatusUpdate.DataSource is not null)
                c = DataSource.         CompareTo(ChargingStationEnergyStatusUpdate.DataSource);

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

            => Id.           Equals(ChargingStationEnergyStatusUpdate.Id)            &&
               NewEnergyInfo.Equals(ChargingStationEnergyStatusUpdate.NewEnergyInfo) &&

            ((!OldEnergyInfo.HasValue && !ChargingStationEnergyStatusUpdate.OldEnergyInfo.HasValue) ||
              (OldEnergyInfo.HasValue &&  ChargingStationEnergyStatusUpdate.OldEnergyInfo.HasValue && OldEnergyInfo.Value.Equals(ChargingStationEnergyStatusUpdate.OldEnergyInfo.Value))) &&

            (( DataSource is null     &&  ChargingStationEnergyStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  ChargingStationEnergyStatusUpdate.DataSource is not null && DataSource.         Equals(ChargingStationEnergyStatusUpdate.DataSource)));

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
