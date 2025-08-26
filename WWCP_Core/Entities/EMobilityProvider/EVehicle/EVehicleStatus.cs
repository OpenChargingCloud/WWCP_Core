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

using System;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The current status of an electric vehicle.
    /// </summary>
    public class EVehicleStatus : IEquatable<EVehicleStatus>,
                                  IComparable<EVehicleStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of a e-mobility station.
        /// </summary>
        public EVehicle_Id          Id          { get; }

        /// <summary>
        /// The current status of a e-mobility station.
        /// </summary>
        public eVehicleStatusTypes  Status      { get; }

        /// <summary>
        /// The timestamp of the current status of the e-mobility station.
        /// </summary>
        public DateTimeOffset       Timestamp   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility station status.
        /// </summary>
        /// <param name="Id">The unique identification of a e-mobility station.</param>
        /// <param name="Status">The current status of a e-mobility station.</param>
        /// <param name="Timestamp">The timestamp of the current status of the e-mobility station.</param>
        public EVehicleStatus(EVehicle_Id          Id,
                              eVehicleStatusTypes  Status,
                              DateTimeOffset       Timestamp)

        {

            #region Initial checks

            if (Id is null)
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

        public static EVehicleStatus Snapshot(EVehicle eVehicle)

            => new (eVehicle.Id,
                    eVehicle.Status.Value,
                    eVehicle.Status.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (eVehicleStatus1, eVehicleStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus1">A eVehicleStatus.</param>
        /// <param name="eVehicleStatus2">Another eVehicleStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVehicleStatus eVehicleStatus1, EVehicleStatus eVehicleStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(eVehicleStatus1, eVehicleStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eVehicleStatus1 is null) || ((Object) eVehicleStatus2 is null))
                return false;

            return eVehicleStatus1.Equals(eVehicleStatus2);

        }

        #endregion

        #region Operator != (eVehicleStatus1, eVehicleStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus1">A eVehicleStatus.</param>
        /// <param name="eVehicleStatus2">Another eVehicleStatus.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVehicleStatus eVehicleStatus1, EVehicleStatus eVehicleStatus2)
        {
            return !(eVehicleStatus1 == eVehicleStatus2);
        }

        #endregion

        #region Operator <  (eVehicleStatus1, eVehicleStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus1">A eVehicleStatus.</param>
        /// <param name="eVehicleStatus2">Another eVehicleStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EVehicleStatus eVehicleStatus1, EVehicleStatus eVehicleStatus2)
        {

            if ((Object) eVehicleStatus1 is null)
                throw new ArgumentNullException("The given eVehicleStatus1 must not be null!");

            return eVehicleStatus1.CompareTo(eVehicleStatus2) < 0;

        }

        #endregion

        #region Operator <= (eVehicleStatus1, eVehicleStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus1">A eVehicleStatus.</param>
        /// <param name="eVehicleStatus2">Another eVehicleStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EVehicleStatus eVehicleStatus1, EVehicleStatus eVehicleStatus2)
        {
            return !(eVehicleStatus1 > eVehicleStatus2);
        }

        #endregion

        #region Operator >  (eVehicleStatus1, eVehicleStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus1">A eVehicleStatus.</param>
        /// <param name="eVehicleStatus2">Another eVehicleStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EVehicleStatus eVehicleStatus1, EVehicleStatus eVehicleStatus2)
        {

            if ((Object) eVehicleStatus1 is null)
                throw new ArgumentNullException("The given eVehicleStatus1 must not be null!");

            return eVehicleStatus1.CompareTo(eVehicleStatus2) > 0;

        }

        #endregion

        #region Operator >= (eVehicleStatus1, eVehicleStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus1">A eVehicleStatus.</param>
        /// <param name="eVehicleStatus2">Another eVehicleStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EVehicleStatus eVehicleStatus1, EVehicleStatus eVehicleStatus2)
        {
            return !(eVehicleStatus1 < eVehicleStatus2);
        }

        #endregion

        #endregion

        #region IComparable<eVehicleStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eVehicleStatus.
            var eVehicleStatus = Object as EVehicleStatus;
            if ((Object) eVehicleStatus is null)
                throw new ArgumentException("The given object is not a eVehicleStatus!");

            return CompareTo(eVehicleStatus);

        }

        #endregion

        #region CompareTo(eVehicleStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleStatus">An object to compare with.</param>
        public Int32 CompareTo(EVehicleStatus eVehicleStatus)
        {

            if ((Object) eVehicleStatus is null)
                throw new ArgumentNullException("The given eVehicleStatus must not be null!");

            // Compare eVehicle Ids
            var _Result = Id.CompareTo(eVehicleStatus.Id);

            // If equal: Compare eVehicle status
            if (_Result == 0)
                _Result = Status.CompareTo(eVehicleStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eVehicleStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            // Check if the given object is an eVehicleStatus.
            var eVehicleStatus = Object as EVehicleStatus;
            if ((Object) eVehicleStatus is null)
                return false;

            return this.Equals(eVehicleStatus);

        }

        #endregion

        #region Equals(eVehicleStatus)

        /// <summary>
        /// Compares two eVehicle identifications for equality.
        /// </summary>
        /// <param name="eVehicleStatus">An eVehicle identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVehicleStatus eVehicleStatus)
        {

            if ((Object) eVehicleStatus is null)
                return false;

            return Id.    Equals(eVehicleStatus.Id) &&
                   Status.Equals(eVehicleStatus.Status);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
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
