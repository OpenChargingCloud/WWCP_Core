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
    /// A roaming network admin status update.
    /// </summary>
    public readonly struct RoamingNetworkAdminStatusUpdate : IEquatable<RoamingNetworkAdminStatusUpdate>,
                                                             IComparable<RoamingNetworkAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id                             Id            { get; }

        /// <summary>
        /// The new timestamped admin status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkAdminStatusTypes>   NewStatus     { get; }

        /// <summary>
        /// The optional old timestamped admin status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkAdminStatusTypes>?  OldStatus     { get; }

        /// <summary>
        /// An optional data source or context for this roaming network admin status update.
        /// </summary>
        public String?                                       DataSource    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped admin status of the roaming network.</param>
        /// <param name="OldStatus">The old timestamped admin status of the roaming network.</param>
        /// <param name="DataSource">An optional data source or context for the roaming network admin status update.</param>
        public RoamingNetworkAdminStatusUpdate(RoamingNetwork_Id                             Id,
                                               Timestamped<RoamingNetworkAdminStatusTypes>   NewStatus,
                                               Timestamped<RoamingNetworkAdminStatusTypes>?  OldStatus    = null,
                                               String?                                       DataSource   = null)

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
        /// Take a snapshot of the current roaming network admin status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="DataSource">An optional data source or context for the roaming network admin status update.</param>
        public static RoamingNetworkAdminStatusUpdate Snapshot(IRoamingNetwork  RoamingNetwork,
                                                               String?          DataSource   = null)

            => new (RoamingNetwork.Id,
                    RoamingNetwork.AdminStatus,
                    RoamingNetwork.AdminStatusSchedule().Skip(1).FirstOrDefault(),
                    DataSource);

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network admin status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1,
                                           RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)

            => RoamingNetworkAdminStatusUpdate1.Equals(RoamingNetworkAdminStatusUpdate2);

        #endregion

        #region Operator != (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network admin status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network admin status update.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1,
                                           RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)

            => !RoamingNetworkAdminStatusUpdate1.Equals(RoamingNetworkAdminStatusUpdate2);

        #endregion

        #region Operator <  (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network admin status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1,
                                          RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)

            => RoamingNetworkAdminStatusUpdate1.CompareTo(RoamingNetworkAdminStatusUpdate2) < 0;

        #endregion

        #region Operator <= (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network admin status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1,
                                           RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)

            => RoamingNetworkAdminStatusUpdate1.CompareTo(RoamingNetworkAdminStatusUpdate2) <= 0;

        #endregion

        #region Operator >  (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network admin status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1,
                                          RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)

            => RoamingNetworkAdminStatusUpdate1.CompareTo(RoamingNetworkAdminStatusUpdate2) > 0;

        #endregion

        #region Operator >= (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network admin status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network admin status update.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1,
                                           RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)

            => RoamingNetworkAdminStatusUpdate1.CompareTo(RoamingNetworkAdminStatusUpdate2) >= 0;

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two roaming network admin status updates.
        /// </summary>
        /// <param name="Object">A roaming network admin status update to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is RoamingNetworkAdminStatusUpdate roamingNetworkStatusUpdate
                   ? CompareTo(roamingNetworkStatusUpdate)
                   : throw new ArgumentException("The given object is not a roaming network admin status update!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RoamingNetworkAdminStatusUpdate)

        /// <summary>
        /// Compares two roaming network admin status updates.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate">A roaming network admin status update to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate)
        {

            var c = Id.             CompareTo(RoamingNetworkAdminStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.      CompareTo(RoamingNetworkAdminStatusUpdate.NewStatus);

            if (c == 0 && OldStatus.HasValue && RoamingNetworkAdminStatusUpdate.OldStatus.HasValue)
                c = OldStatus.Value.CompareTo(RoamingNetworkAdminStatusUpdate.OldStatus.Value);

            if (c == 0 && DataSource is not null && RoamingNetworkAdminStatusUpdate.DataSource is not null)
                c = DataSource.     CompareTo(RoamingNetworkAdminStatusUpdate.DataSource);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two roaming network admin status updates for equality.
        /// </summary>
        /// <param name="Object">A roaming network admin status update to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RoamingNetworkAdminStatusUpdate roamingNetworkStatusUpdate &&
                   Equals(roamingNetworkStatusUpdate);

        #endregion

        #region Equals(RoamingNetworkAdminStatusUpdate)

        /// <summary>
        /// Compares two roaming network admin status updates for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate">A roaming network admin status update to compare with.</param>
        public Boolean Equals(RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate)

            => Id.       Equals(RoamingNetworkAdminStatusUpdate.Id)        &&
               NewStatus.Equals(RoamingNetworkAdminStatusUpdate.NewStatus) &&

            ((!OldStatus.HasValue     && !RoamingNetworkAdminStatusUpdate.OldStatus.HasValue) ||
              (OldStatus.HasValue     &&  RoamingNetworkAdminStatusUpdate.OldStatus.HasValue     && OldStatus.Value.Equals(RoamingNetworkAdminStatusUpdate.OldStatus.Value))) &&

            (( DataSource is null     &&  RoamingNetworkAdminStatusUpdate.DataSource is null) ||
              (DataSource is not null &&  RoamingNetworkAdminStatusUpdate.DataSource is not null && DataSource.     Equals(RoamingNetworkAdminStatusUpdate.DataSource)));

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

            => $"{Id}: {(OldStatus.HasValue ? $"'{OldStatus.Value}' -> " : "")}'{NewStatus}'{(DataSource is not null ? $" ({DataSource})" : "")}";

        #endregion

    }

}
