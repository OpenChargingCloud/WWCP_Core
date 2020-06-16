/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// A roaming network status update.
    /// </summary>
    public struct RoamingNetworkStatusUpdate : IEquatable <RoamingNetworkStatusUpdate>,
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
        public static RoamingNetworkStatusUpdate Snapshot(RoamingNetwork RoamingNetwork)

            => new RoamingNetworkStatusUpdate(RoamingNetwork.Id,
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
        public static Boolean operator == (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetworkStatusUpdate1 == null) || ((Object) RoamingNetworkStatusUpdate2 == null))
                return false;

            return RoamingNetworkStatusUpdate1.Equals(RoamingNetworkStatusUpdate2);

        }

        #endregion

        #region Operator != (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)
        {
            return !(RoamingNetworkStatusUpdate1 == RoamingNetworkStatusUpdate2);
        }

        #endregion

        #region Operator <  (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)
        {

            if ((Object) RoamingNetworkStatusUpdate1 == null)
                throw new ArgumentNullException("The given RoamingNetworkStatusUpdate1 must not be null!");

            return RoamingNetworkStatusUpdate1.CompareTo(RoamingNetworkStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)
        {
            return !(RoamingNetworkStatusUpdate1 > RoamingNetworkStatusUpdate2);
        }

        #endregion

        #region Operator >  (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)
        {

            if ((Object) RoamingNetworkStatusUpdate1 == null)
                throw new ArgumentNullException("The given RoamingNetworkStatusUpdate1 must not be null!");

            return RoamingNetworkStatusUpdate1.CompareTo(RoamingNetworkStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate1">A roaming network status update.</param>
        /// <param name="RoamingNetworkStatusUpdate2">Another roaming network status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate1, RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate2)
        {
            return !(RoamingNetworkStatusUpdate1 < RoamingNetworkStatusUpdate2);
        }

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RoamingNetworkStatusUpdate))
                throw new ArgumentException("The given object is not a RoamingNetworkStatus!",
                                            nameof(Object));

            return CompareTo((RoamingNetworkStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(RoamingNetworkStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate)
        {

            if ((Object) RoamingNetworkStatusUpdate == null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatusUpdate), "The given RoamingNetwork status update must not be null!");

            // Compare RoamingNetwork Ids
            var _Result = Id.CompareTo(RoamingNetworkStatusUpdate.Id);

            // If equal: Compare the new roaming network status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(RoamingNetworkStatusUpdate.NewStatus);

            // If equal: Compare the old RoamingNetwork status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(RoamingNetworkStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatusUpdate> Members

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

            if (!(Object is RoamingNetworkStatusUpdate))
                return false;

            return this.Equals((RoamingNetworkStatusUpdate) Object);

        }

        #endregion

        #region Equals(RoamingNetworkStatusUpdate)

        /// <summary>
        /// Compares two RoamingNetwork status updates for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatusUpdate">A roaming network status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkStatusUpdate RoamingNetworkStatusUpdate)
        {

            if ((Object) RoamingNetworkStatusUpdate == null)
                return false;

            return Id.       Equals(RoamingNetworkStatusUpdate.Id)        &&
                   OldStatus.Equals(RoamingNetworkStatusUpdate.OldStatus) &&
                   NewStatus.Equals(RoamingNetworkStatusUpdate.NewStatus);

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
