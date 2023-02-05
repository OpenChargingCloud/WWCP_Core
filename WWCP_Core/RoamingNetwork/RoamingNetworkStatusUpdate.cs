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
    /// A roaming network status update.
    /// </summary>
    public readonly struct RoamingNetworkStatusUpdate : IEquatable<RoamingNetworkStatusUpdate>,
                                                        IComparable<RoamingNetworkStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id                       Id          { get; }

        /// <summary>
        /// The old timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network status update.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="OldStatus">The old timestamped status of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped status of the roaming network.</param>
        public RoamingNetworkStatusUpdate(RoamingNetwork_Id                       Id,
                                          Timestamped<RoamingNetworkStatusTypes>  OldStatus,
                                          Timestamped<RoamingNetworkStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion


        #region (static) Snapshot(RoamingNetwork)

        /// <summary>
        /// Take a snapshot of the current roaming network status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkStatusUpdate Snapshot(IRoamingNetwork RoamingNetwork)

            => new (RoamingNetwork.Id,
                    RoamingNetwork.Status,
                    RoamingNetwork.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
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
        /// <returns>true|false</returns>
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
        /// <returns>true|false</returns>
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
        /// <returns>true|false</returns>
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
        /// <returns>true|false</returns>
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
        /// <returns>true|false</returns>
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

            var c = Id.       CompareTo(RoamingNetworkStatusUpdate.Id);

            if (c == 0)
                c = NewStatus.CompareTo(RoamingNetworkStatusUpdate.NewStatus);

            if (c == 0)
                c = OldStatus.CompareTo(RoamingNetworkStatusUpdate.OldStatus);

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
               OldStatus.Equals(RoamingNetworkStatusUpdate.OldStatus) &&
               NewStatus.Equals(RoamingNetworkStatusUpdate.NewStatus);

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
