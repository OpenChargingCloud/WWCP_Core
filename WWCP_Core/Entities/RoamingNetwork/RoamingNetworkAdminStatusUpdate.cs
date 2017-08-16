/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A roaming network admin status update.
    /// </summary>
    public struct RoamingNetworkAdminStatusUpdate : IEquatable <RoamingNetworkAdminStatusUpdate>,
                                                    IComparable<RoamingNetworkAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id                           Id          { get; }

        /// <summary>
        /// The old timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkAdminStatusTypes>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkAdminStatusTypes>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        #region RoamingNetworkAdminStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new roaming network admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="OldStatus">The old timestamped admin status of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped admin status of the roaming network.</param>
        public RoamingNetworkAdminStatusUpdate(RoamingNetwork_Id                           Id,
                                               Timestamped<RoamingNetworkAdminStatusTypes>  OldStatus,
                                               Timestamped<RoamingNetworkAdminStatusTypes>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion

        #region RoamingNetworkAdminStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new roaming network admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="OldStatus">The old timestamped admin status of the roaming network.</param>
        /// <param name="NewStatus">The new timestamped admin status of the roaming network.</param>
        public RoamingNetworkAdminStatusUpdate(RoamingNetwork_Id          Id,
                                               RoamingNetworkAdminStatus  OldStatus,
                                               RoamingNetworkAdminStatus  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus.Combined;
            this.NewStatus  = NewStatus.Combined;

        }

        #endregion

        #endregion


        #region (static) Snapshot(RoamingNetwork)

        /// <summary>
        /// Take a snapshot of the current roaming network status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkAdminStatusUpdate Snapshot(RoamingNetwork RoamingNetwork)

            => new RoamingNetworkAdminStatusUpdate(RoamingNetwork.Id,
                                                   RoamingNetwork.AdminStatus,
                                                   RoamingNetwork.AdminStatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetworkAdminStatusUpdate1 == null) || ((Object) RoamingNetworkAdminStatusUpdate2 == null))
                return false;

            return RoamingNetworkAdminStatusUpdate1.Equals(RoamingNetworkAdminStatusUpdate2);

        }

        #endregion

        #region Operator != (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)
            => !(RoamingNetworkAdminStatusUpdate1 == RoamingNetworkAdminStatusUpdate2);

        #endregion

        #region Operator <  (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)
        {

            if ((Object) RoamingNetworkAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatusUpdate1), "The given RoamingNetworkAdminStatusUpdate1 must not be null!");

            return RoamingNetworkAdminStatusUpdate1.CompareTo(RoamingNetworkAdminStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)
            => !(RoamingNetworkAdminStatusUpdate1 > RoamingNetworkAdminStatusUpdate2);

        #endregion

        #region Operator >  (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)
        {

            if ((Object) RoamingNetworkAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatusUpdate1), "The given RoamingNetworkAdminStatusUpdate1 must not be null!");

            return RoamingNetworkAdminStatusUpdate1.CompareTo(RoamingNetworkAdminStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkAdminStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate1, RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate2)
            => !(RoamingNetworkAdminStatusUpdate1 < RoamingNetworkAdminStatusUpdate2);

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RoamingNetworkAdminStatusUpdate))
                throw new ArgumentException("The given object is not a RoamingNetworkStatus!",
                                            nameof(Object));

            return CompareTo((RoamingNetworkAdminStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(RoamingNetworkAdminStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate)
        {

            if ((Object) RoamingNetworkAdminStatusUpdate == null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatusUpdate), "The given RoamingNetwork status update must not be null!");

            // Compare RoamingNetwork Ids
            var _Result = Id.CompareTo(RoamingNetworkAdminStatusUpdate.Id);

            // If equal: Compare the new roaming network status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(RoamingNetworkAdminStatusUpdate.NewStatus);

            // If equal: Compare the old RoamingNetwork status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(RoamingNetworkAdminStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatusUpdate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is RoamingNetworkAdminStatusUpdate))
                return false;

            return this.Equals((RoamingNetworkAdminStatusUpdate) Object);

        }

        #endregion

        #region Equals(RoamingNetworkAdminStatusUpdate)

        /// <summary>
        /// Compares two RoamingNetwork status updates for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatusUpdate">A roaming network status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkAdminStatusUpdate RoamingNetworkAdminStatusUpdate)
        {

            if ((Object) RoamingNetworkAdminStatusUpdate == null)
                return false;

            return Id.       Equals(RoamingNetworkAdminStatusUpdate.Id)        &&
                   OldStatus.Equals(RoamingNetworkAdminStatusUpdate.OldStatus) &&
                   NewStatus.Equals(RoamingNetworkAdminStatusUpdate.NewStatus);

        }

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

                return Id.       GetHashCode() * 7 ^
                       OldStatus.GetHashCode() * 5 ^
                       NewStatus.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, ": ",
                             OldStatus,
                             " -> ",
                             NewStatus);

        #endregion

    }

}
