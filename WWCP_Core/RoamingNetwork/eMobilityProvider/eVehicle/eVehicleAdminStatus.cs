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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current admin status of an electric vehicle.
    /// </summary>
    public class eVehicleAdminStatus : IEquatable<eVehicleAdminStatus>,
                                       IComparable<eVehicleAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of a e-mobility station.
        /// </summary>
        public eVehicle_Id              Id          { get; }

        /// <summary>
        /// The current status of a e-mobility station.
        /// </summary>
        public eVehicleAdminStatusType  Status      { get; }

        /// <summary>
        /// The timestamp of the current status of the e-mobility station.
        /// </summary>
        public DateTime                         Timestamp   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility station status.
        /// </summary>
        /// <param name="Id">The unique identification of a e-mobility station.</param>
        /// <param name="Status">The current status of a e-mobility station.</param>
        /// <param name="Timestamp">The timestamp of the current status of the e-mobility station.</param>
        public eVehicleAdminStatus(eVehicle_Id              Id,
                                           eVehicleAdminStatusType  Status,
                                           DateTime                         Timestamp)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id), "The given unique identification of a e-mobility station must not be null!");

            #endregion

            this.Id         = Id;
            this.Status     = Status;
            this.Timestamp  = Timestamp;

        }

        #endregion


        #region (static) Snapshot(eVehicle)

        /// <summary>
        /// Take a snapshot of the current e-mobility station admin status.
        /// </summary>
        /// <param name="eVehicle">An e-mobility station.</param>

        public static eVehicleAdminStatus Snapshot(eVehicle eVehicle)

            => new eVehicleAdminStatus(eVehicle.Id,
                                               eVehicle.AdminStatus.Value,
                                               eVehicle.AdminStatus.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (eVehicleAdminStatus1, eVehicleAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus1">A eVehicleAdminStatus.</param>
        /// <param name="eVehicleAdminStatus2">Another eVehicleAdminStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (eVehicleAdminStatus eVehicleAdminStatus1, eVehicleAdminStatus eVehicleAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(eVehicleAdminStatus1, eVehicleAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eVehicleAdminStatus1 == null) || ((Object) eVehicleAdminStatus2 == null))
                return false;

            return eVehicleAdminStatus1.Equals(eVehicleAdminStatus2);

        }

        #endregion

        #region Operator != (eVehicleAdminStatus1, eVehicleAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus1">A eVehicleAdminStatus.</param>
        /// <param name="eVehicleAdminStatus2">Another eVehicleAdminStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eVehicleAdminStatus eVehicleAdminStatus1, eVehicleAdminStatus eVehicleAdminStatus2)
        {
            return !(eVehicleAdminStatus1 == eVehicleAdminStatus2);
        }

        #endregion

        #region Operator <  (eVehicleAdminStatus1, eVehicleAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus1">A eVehicleAdminStatus.</param>
        /// <param name="eVehicleAdminStatus2">Another eVehicleAdminStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eVehicleAdminStatus eVehicleAdminStatus1, eVehicleAdminStatus eVehicleAdminStatus2)
        {

            if ((Object) eVehicleAdminStatus1 == null)
                throw new ArgumentNullException("The given eVehicleAdminStatus1 must not be null!");

            return eVehicleAdminStatus1.CompareTo(eVehicleAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (eVehicleAdminStatus1, eVehicleAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus1">A eVehicleAdminStatus.</param>
        /// <param name="eVehicleAdminStatus2">Another eVehicleAdminStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eVehicleAdminStatus eVehicleAdminStatus1, eVehicleAdminStatus eVehicleAdminStatus2)
        {
            return !(eVehicleAdminStatus1 > eVehicleAdminStatus2);
        }

        #endregion

        #region Operator >  (eVehicleAdminStatus1, eVehicleAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus1">A eVehicleAdminStatus.</param>
        /// <param name="eVehicleAdminStatus2">Another eVehicleAdminStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eVehicleAdminStatus eVehicleAdminStatus1, eVehicleAdminStatus eVehicleAdminStatus2)
        {

            if ((Object) eVehicleAdminStatus1 == null)
                throw new ArgumentNullException("The given eVehicleAdminStatus1 must not be null!");

            return eVehicleAdminStatus1.CompareTo(eVehicleAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (eVehicleAdminStatus1, eVehicleAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus1">A eVehicleAdminStatus.</param>
        /// <param name="eVehicleAdminStatus2">Another eVehicleAdminStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eVehicleAdminStatus eVehicleAdminStatus1, eVehicleAdminStatus eVehicleAdminStatus2)
        {
            return !(eVehicleAdminStatus1 < eVehicleAdminStatus2);
        }

        #endregion

        #endregion

        #region IComparable<eVehicleAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eVehicleAdminStatus.
            var eVehicleAdminStatus = Object as eVehicleAdminStatus;
            if ((Object) eVehicleAdminStatus == null)
                throw new ArgumentException("The given object is not a eVehicleAdminStatus!");

            return CompareTo(eVehicleAdminStatus);

        }

        #endregion

        #region CompareTo(eVehicleAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(eVehicleAdminStatus eVehicleAdminStatus)
        {

            if ((Object) eVehicleAdminStatus == null)
                throw new ArgumentNullException("The given eVehicleAdminStatus must not be null!");

            // Compare eVehicle Ids
            var _Result = Id.CompareTo(eVehicleAdminStatus.Id);

            // If equal: Compare eVehicle status
            if (_Result == 0)
                _Result = Status.CompareTo(eVehicleAdminStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eVehicleAdminStatus> Members

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

            // Check if the given object is an eVehicleAdminStatus.
            var eVehicleAdminStatus = Object as eVehicleAdminStatus;
            if ((Object) eVehicleAdminStatus == null)
                return false;

            return this.Equals(eVehicleAdminStatus);

        }

        #endregion

        #region Equals(eVehicleAdminStatus)

        /// <summary>
        /// Compares two eVehicle identifications for equality.
        /// </summary>
        /// <param name="eVehicleAdminStatus">An eVehicle identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eVehicleAdminStatus eVehicleAdminStatus)
        {

            if ((Object) eVehicleAdminStatus == null)
                return false;

            return Id.    Equals(eVehicleAdminStatus.Id) &&
                   Status.Equals(eVehicleAdminStatus.Status);

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
                return Id.GetHashCode() * 17 ^ Status.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(Id, " -> ", Status.ToString());

        #endregion

    }

}
