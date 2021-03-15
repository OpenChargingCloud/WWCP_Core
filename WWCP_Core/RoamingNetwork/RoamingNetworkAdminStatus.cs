﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current admin status of a roaming network.
    /// </summary>
    public class RoamingNetworkAdminStatus : AInternalData,
                                             IEquatable <RoamingNetworkAdminStatus>,
                                             IComparable<RoamingNetworkAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming network.
        /// </summary>
        public RoamingNetwork_Id                            Id        { get; }

        /// <summary>
        /// The current timestamped admin status of the roaming network.
        /// </summary>
        public Timestamped<RoamingNetworkAdminStatusTypes>  Status    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new roaming network admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming network.</param>
        /// <param name="Status">The current timestamped admin status of the roaming network.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public RoamingNetworkAdminStatus(RoamingNetwork_Id                            Id,
                                         Timestamped<RoamingNetworkAdminStatusTypes>  Status,
                                         IReadOnlyDictionary<String, Object>          CustomData  = null)

            : base(CustomData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion


        #region (static) Snapshot(RoamingNetwork)

        /// <summary>
        /// Take a snapshot of the current roaming network admin status.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static RoamingNetworkAdminStatus Snapshot(RoamingNetwork RoamingNetwork)

            => new RoamingNetworkAdminStatus(RoamingNetwork.Id,
                                             RoamingNetwork.AdminStatus.Value);

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetworkAdminStatus1 == null) || ((Object) RoamingNetworkAdminStatus2 == null))
                return false;

            return RoamingNetworkAdminStatus1.Equals(RoamingNetworkAdminStatus2);

        }

        #endregion

        #region Operator != (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)
            => !(RoamingNetworkAdminStatus1 == RoamingNetworkAdminStatus2);

        #endregion

        #region Operator <  (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)
        {

            if ((Object) RoamingNetworkAdminStatus1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatus1), "The given RoamingNetworkAdminStatus1 must not be null!");

            return RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)
            => !(RoamingNetworkAdminStatus1 > RoamingNetworkAdminStatus2);

        #endregion

        #region Operator >  (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)
        {

            if ((Object) RoamingNetworkAdminStatus1 == null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatus1), "The given RoamingNetworkAdminStatus1 must not be null!");

            return RoamingNetworkAdminStatus1.CompareTo(RoamingNetworkAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus1">A roaming network admin status.</param>
        /// <param name="RoamingNetworkAdminStatus2">Another roaming network admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetworkAdminStatus RoamingNetworkAdminStatus1, RoamingNetworkAdminStatus RoamingNetworkAdminStatus2)
            => !(RoamingNetworkAdminStatus1 < RoamingNetworkAdminStatus2);

        #endregion

        #endregion

        #region IComparable<RoamingNetworkAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is RoamingNetworkAdminStatus))
                throw new ArgumentException("The given object is not a RoamingNetworkAdminStatus!",
                                            nameof(Object));

            return CompareTo((RoamingNetworkAdminStatus) Object);

        }

        #endregion

        #region CompareTo(RoamingNetworkAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetworkAdminStatus RoamingNetworkAdminStatus)
        {

            if ((Object) RoamingNetworkAdminStatus == null)
                throw new ArgumentNullException(nameof(RoamingNetworkAdminStatus), "The given RoamingNetworkAdminStatus must not be null!");

            // Compare RoamingNetwork Ids
            var _Result = Id.CompareTo(RoamingNetworkAdminStatus.Id);

            // If equal: Compare RoamingNetwork status
            if (_Result == 0)
                _Result = Status.CompareTo(RoamingNetworkAdminStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetworkAdminStatus> Members

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

            if (!(Object is RoamingNetworkAdminStatus))
                return false;

            return Equals((RoamingNetworkAdminStatus) Object);

        }

        #endregion

        #region Equals(RoamingNetworkAdminStatus)

        /// <summary>
        /// Compares two RoamingNetwork identifications for equality.
        /// </summary>
        /// <param name="RoamingNetworkAdminStatus">A roaming network identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetworkAdminStatus RoamingNetworkAdminStatus)
        {

            if ((Object) RoamingNetworkAdminStatus == null)
                return false;

            return Id.    Equals(RoamingNetworkAdminStatus.Id) &&
                   Status.Equals(RoamingNetworkAdminStatus.Status);

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
