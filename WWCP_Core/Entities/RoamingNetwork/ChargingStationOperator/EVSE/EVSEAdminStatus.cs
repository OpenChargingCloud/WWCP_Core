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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current admin status of an EVSE.
    /// </summary>
    public struct EVSEAdminStatus : IEquatable <EVSEAdminStatus>,
                                    IComparable<EVSEAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id              Id            { get; }

        /// <summary>
        /// The current admin status of the EVSE.
        /// </summary>
        public EVSEAdminStatusType  AdminStatus   { get; }

        /// <summary>
        /// The timestamp of the current admin status of the EVSE.
        /// </summary>
        public DateTime             Timestamp     { get; }

        /// <summary>
        /// The timestamped admin status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusType> Combined
            => new Timestamped<EVSEAdminStatusType>(Timestamp, AdminStatus);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The current admin status of the EVSE.</param>
        /// <param name="Timestamp">The timestamp of the current admin status of the EVSE.</param>
        public EVSEAdminStatus(EVSE_Id              Id,
                               EVSEAdminStatusType  Status,
                               DateTime             Timestamp)

        {

            this.Id           = Id;
            this.AdminStatus  = Status;
            this.Timestamp    = Timestamp;

        }

        #endregion


        #region (static) Snapshot(EVSE)

        /// <summary>
        /// Take a snapshot of the current EVSE admin status.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public static EVSEAdminStatus Snapshot(EVSE EVSE)

            => new EVSEAdminStatus(EVSE.Id,
                                   EVSE.AdminStatus.Value,
                                   EVSE.AdminStatus.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEAdminStatus1, EVSEAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEAdminStatus1 == null) || ((Object) EVSEAdminStatus2 == null))
                return false;

            return EVSEAdminStatus1.Equals(EVSEAdminStatus2);

        }

        #endregion

        #region Operator != (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
            => !(EVSEAdminStatus1 == EVSEAdminStatus2);

        #endregion

        #region Operator <  (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
        {

            if ((Object) EVSEAdminStatus1 == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatus1), "The given EVSEAdminStatus1 must not be null!");

            return EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
            => !(EVSEAdminStatus1 > EVSEAdminStatus2);

        #endregion

        #region Operator >  (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
        {

            if ((Object) EVSEAdminStatus1 == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatus1), "The given EVSEAdminStatus1 must not be null!");

            return EVSEAdminStatus1.CompareTo(EVSEAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (EVSEAdminStatus1, EVSEAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus1">An EVSE admin status.</param>
        /// <param name="EVSEAdminStatus2">Another EVSE admin status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEAdminStatus EVSEAdminStatus1, EVSEAdminStatus EVSEAdminStatus2)
            => !(EVSEAdminStatus1 < EVSEAdminStatus2);

        #endregion

        #endregion

        #region IComparable<EVSEAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEAdminStatus))
                throw new ArgumentException("The given object is not a EVSEAdminStatus!",
                                            nameof(Object));

            return CompareTo((EVSEAdminStatus) Object);

        }

        #endregion

        #region CompareTo(EVSEAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(EVSEAdminStatus EVSEAdminStatus)
        {

            if ((Object) EVSEAdminStatus == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatus), "The given EVSEAdminStatus must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(EVSEAdminStatus.Id);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = AdminStatus.CompareTo(EVSEAdminStatus.AdminStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEAdminStatus> Members

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

            if (!(Object is EVSEAdminStatus))
                return false;

            return this.Equals((EVSEAdminStatus) Object);

        }

        #endregion

        #region Equals(EVSEAdminStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEAdminStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEAdminStatus EVSEAdminStatus)
        {

            if ((Object) EVSEAdminStatus == null)
                return false;

            return Id.         Equals(EVSEAdminStatus.Id)          &&
                   AdminStatus.Equals(EVSEAdminStatus.AdminStatus) &&
                   Timestamp.  Equals(EVSEAdminStatus.Timestamp);

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

                return Id.         GetHashCode() * 7 ^
                       AdminStatus.GetHashCode() * 5 ^
                       Timestamp.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             AdminStatus,
                             " since ",
                             Timestamp.ToIso8601());

        #endregion

    }

}
