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
    /// A roaming network status update.
    /// </summary>
    public readonly struct RoamingNetworkStatusUpdate : IEquatable<RoamingNetworkStatusUpdate>,
                                                        IComparable<RoamingNetworkStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id                        Id            { get; }

        /// <summary>
        /// The new timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The optional old timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this roaming network status update.
        /// </summary>
        public String?                                  DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network status update.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped admin status of the roaming network.</param>
        /// <param name="OldStatus">The optional old timestamped admin status of the roaming network.</param>
        /// <param name="DataSource">An optional data source or context for the roaming network status update.</param>
        public RoamingNetworkStatusUpdate(RoamingNetwork_Id                        Id,
                                          Timestamped<RoamingNetworkStatusTypes>   NewStatus,
                                          Timestamped<RoamingNetworkStatusTypes>?  OldStatus    = null,
                                          String?                                  DataSource   = null)

        {

            this.Id          = Id;
            this.NewStatus   = NewStatus;
            this.OldStatus   = OldStatus;
            this.DataSource  = DataSource;

            unchecked
            {

                hashCode = Id.         GetHashCode()       * 7 ^
                           NewStatus.  GetHashCode()       * 5 ^
                          (OldStatus?. GetHashCode() ?? 0) * 3 ^
                          (DataSource?.GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Snapshot(RoamingNetwork, DataSource = null)

        /// <summary>
        /// Take a snapshot of the current roaming network status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="DataSource">An optional data source or context for the roaming network status update.</param>
        public static RoamingNetworkStatusUpdate Snapshot(IRoamingNetwork  RoamingNetwork,
                                                          String?          DataSource   = null)

            => new (RoamingNetwork.Id,
                    RoamingNetwork.Status,
                    RoamingNetwork.StatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1,
                                           RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)

            => RoamingNetworkStatusUpdate1.Equals(RoamingNetworkStatusUpdate2);

        #endregion

        #region Operator != (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1,
                                           RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)

            => !RoamingNetworkStatusUpdate1.Equals(RoamingNetworkStatusUpdate2);

        #endregion

        #region Operator <  (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1,
                                          RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)

            => RoamingNetworkStatusUpdate1.CompareTo(RoamingNetworkStatusUpdate2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1,
                                           RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)

            => RoamingNetworkStatusUpdate1.CompareTo(RoamingNetworkStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1,
                                          RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)

            => RoamingNetworkStatusUpdate1.CompareTo(RoamingNetworkStatusUpdate2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1,
                                           RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)

            => RoamingNetworkStatusUpdate1.CompareTo(RoamingNetworkStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network status updates.
        /// </summary>
        /// <param name="Object">A roaming network status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is RoamingNetworkStatusUpdate roamingNetworkStatusUpdate
                   ? CompareTo(roamingNetworkStatusUpdate)
                   : throw new ArgumentException("The given object is not a roaming network status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkStatusUpdate)

        /// <summary>
        /// Compares two roaming network status updates.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate">A roaming network status update to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate)
        {

            var c = Id.             CompareTo(RoamingNetworkStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(RoamingNetworkStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && RoamingNetworkStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(RoamingNetworkStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && RoamingNetworkStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(RoamingNetworkStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network status updates for equality.
        /// </summary>
        /// <param name="Object">A roaming network status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkStatusUpdate roamingNetworkStatusUpdate &&
                   Equals(roamingNetworkStatusUpdate);

        #endregion

        #region Equals(RoamingNetworkStatusUpdate)

        /// <summary>
        /// Compares two roaming network status updates for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate">A roaming network status update to compare with.</param>
        public Boolean Equals(RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate)

            => Id.       Equals(RoamingNetworkStatusUpdate.Id)        &&
               NewStatus.Equals(RoamingNetworkStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !RoamingNetworkStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  RoamingNetworkStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(RoamingNetworkStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  RoamingNetworkStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  RoamingNetworkStatusUpdate.DataSource is not null && DataSource.     Equals(RoamingNetworkStatusUpdate.DataSource)));

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

            => $"{Id}: {(OldStatus.HasValue ? $"'{OldStatus.Value}' -> " : "")}'{NewStatus}'{(DataSource is not null ? $" ({DataSource})" : "")}";

        #endregion

    }

}
