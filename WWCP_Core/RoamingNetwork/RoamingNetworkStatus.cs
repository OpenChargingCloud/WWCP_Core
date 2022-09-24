/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The current status of a roaming network.
    /// </summary>
    public class RoamingNetworkStatus : AInternalData,
                                        IEquatable <RoamingNetworkStatus>,
                                        IComparable<RoamingNetworkStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id                       Id       { get; }

        /// <summary>
        /// The current timestamped status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkStatusTypes>  Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current timestamped status of the roaming network.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public RoamingNetworkStatus(RoamingNetwork_Id                       Id,
                                    Timestamped<RoamingNetworkStatusTypes>  Status,
                                    IReadOnlyDictionary<String, Object>     CustomData  = null)

            : base(null,
                   CustomData)

        {

            this.Id         = Id;
            this.Status     = Status;

        }

        #endregion


        #region (static) Snapshot(RoamingNetwork)

        /// <summary>
        /// Take a snapshot of the current roaming network status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkStatus Snapshot(RoamingNetwork RoamingNetwork)

            => new RoamingNetworkStatus(RoamingNetwork.Id,
                                        RoamingNetwork.Status.Value);

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkStatus RoamingNetworkStatus1, RoamingNetworkStatus RoamingNetworkStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingNetworkStatus1, RoamingNetworkStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetworkStatus1 == null) || ((Object) RoamingNetworkStatus2 == null))
                return false;

            return RoamingNetworkStatus1.Equals(RoamingNetworkStatus2);

        }

        #endregion

        #region Operator != (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkStatus RoamingNetworkStatus1, RoamingNetworkStatus RoamingNetworkStatus2)
            => !(RoamingNetworkStatus1 == RoamingNetworkStatus2);

        #endregion

        #region Operator <  (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkStatus RoamingNetworkStatus1, RoamingNetworkStatus RoamingNetworkStatus2)
        {

            if ((Object) RoamingNetworkStatus1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatus1), "The given RoamingNetworkStatus1 must not be null!");

            return RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkStatus RoamingNetworkStatus1, RoamingNetworkStatus RoamingNetworkStatus2)
            => !(RoamingNetworkStatus1 > RoamingNetworkStatus2);

        #endregion

        #region Operator >  (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkStatus RoamingNetworkStatus1, RoamingNetworkStatus RoamingNetworkStatus2)
        {

            if ((Object) RoamingNetworkStatus1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatus1), "The given RoamingNetworkStatus1 must not be null!");

            return RoamingNetworkStatus1.CompareTo(RoamingNetworkStatus2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetworkStatus1, RoamingNetworkStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus1">A roaming network status.</param>
        /// <param name="RoamingNetworkStatus2">Another roaming network status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkStatus RoamingNetworkStatus1, RoamingNetworkStatus RoamingNetworkStatus2)
            => !(RoamingNetworkStatus1 < RoamingNetworkStatus2);

        #endregion

        #endregion

        #region IComparable<RoamingNetworkStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RoamingNetworkStatus))
                throw new ArgumentException("The given object is not a RoamingNetworkStatus!",
                                            nameof(Object));

            return CompareTo((RoamingNetworkStatus) Object);

        }

        #endregion

        #region CompareTo(RoamingNetworkStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkStatus">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkStatus RoamingNetworkStatus)
        {

            if ((Object) RoamingNetworkStatus == null)
                throw new ArgumentNullException(nameof(RoamingNetworkStatus), "The given RoamingNetworkStatus must not be null!");

            // Compare RoamingNetwork Ids
            var _Result = Id.CompareTo(RoamingNetworkStatus.Id);

            // If equal: Compare RoamingNetwork status
            if (_Result == 0)
                _Result = Status.CompareTo(RoamingNetworkStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkStatus> Members

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

            if (!(Object is RoamingNetworkStatus))
                return false;

            return Equals((RoamingNetworkStatus) Object);

        }

        #endregion

        #region Equals(RoamingNetworkStatus)

        /// <summary>
        /// Compares two RoamingNetwork identifications for equality.
        /// </summary>
        /// <param name="RoamingNetworkStatus">A roaming network identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkStatus RoamingNetworkStatus)
        {

            if ((Object) RoamingNetworkStatus == null)
                return false;

            return Id.       Equals(RoamingNetworkStatus.Id) &&
                   Status.   Equals(RoamingNetworkStatus.Status);

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

                return Id.    GetHashCode() * 5 ^
                       Status.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status.Value,
                             " since ",
                             Status.Timestamp.ToIso8601());

        #endregion

    }

}
