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
    /// The current admin status of an e-mobility station.
    /// </summary>
    public class eMobilityStationAdminStatus : IEquatable<eMobilityStationAdminStatus>,
                                               IComparable<eMobilityStationAdminStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of a e-mobility station.
        /// </summary>
        public eMobilityStation_Id              Id          { get; }

        /// <summary>
        /// The current status of a e-mobility station.
        /// </summary>
        public eMobilityStationAdminStatusTypes  Status      { get; }

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
        public eMobilityStationAdminStatus(eMobilityStation_Id              Id,
                                           eMobilityStationAdminStatusTypes  Status,
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


        #region (static) Snapshot(eMobilityStation)

        /// <summary>
        /// Take a snapshot of the current e-mobility station admin status.
        /// </summary>
        /// <param name="eMobilityStation">An e-mobility station.</param>

        public static eMobilityStationAdminStatus Snapshot(eMobilityStation eMobilityStation)

            => new eMobilityStationAdminStatus(eMobilityStation.Id,
                                               eMobilityStation.AdminStatus.Value,
                                               eMobilityStation.AdminStatus.Timestamp);

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityStationAdminStatus1, eMobilityStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus1">A eMobilityStationAdminStatus.</param>
        /// <param name="eMobilityStationAdminStatus2">Another eMobilityStationAdminStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (eMobilityStationAdminStatus eMobilityStationAdminStatus1, eMobilityStationAdminStatus eMobilityStationAdminStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(eMobilityStationAdminStatus1, eMobilityStationAdminStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMobilityStationAdminStatus1 == null) || ((Object) eMobilityStationAdminStatus2 == null))
                return false;

            return eMobilityStationAdminStatus1.Equals(eMobilityStationAdminStatus2);

        }

        #endregion

        #region Operator != (eMobilityStationAdminStatus1, eMobilityStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus1">A eMobilityStationAdminStatus.</param>
        /// <param name="eMobilityStationAdminStatus2">Another eMobilityStationAdminStatus.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (eMobilityStationAdminStatus eMobilityStationAdminStatus1, eMobilityStationAdminStatus eMobilityStationAdminStatus2)
        {
            return !(eMobilityStationAdminStatus1 == eMobilityStationAdminStatus2);
        }

        #endregion

        #region Operator <  (eMobilityStationAdminStatus1, eMobilityStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus1">A eMobilityStationAdminStatus.</param>
        /// <param name="eMobilityStationAdminStatus2">Another eMobilityStationAdminStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (eMobilityStationAdminStatus eMobilityStationAdminStatus1, eMobilityStationAdminStatus eMobilityStationAdminStatus2)
        {

            if ((Object) eMobilityStationAdminStatus1 == null)
                throw new ArgumentNullException("The given eMobilityStationAdminStatus1 must not be null!");

            return eMobilityStationAdminStatus1.CompareTo(eMobilityStationAdminStatus2) < 0;

        }

        #endregion

        #region Operator <= (eMobilityStationAdminStatus1, eMobilityStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus1">A eMobilityStationAdminStatus.</param>
        /// <param name="eMobilityStationAdminStatus2">Another eMobilityStationAdminStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (eMobilityStationAdminStatus eMobilityStationAdminStatus1, eMobilityStationAdminStatus eMobilityStationAdminStatus2)
        {
            return !(eMobilityStationAdminStatus1 > eMobilityStationAdminStatus2);
        }

        #endregion

        #region Operator >  (eMobilityStationAdminStatus1, eMobilityStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus1">A eMobilityStationAdminStatus.</param>
        /// <param name="eMobilityStationAdminStatus2">Another eMobilityStationAdminStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (eMobilityStationAdminStatus eMobilityStationAdminStatus1, eMobilityStationAdminStatus eMobilityStationAdminStatus2)
        {

            if ((Object) eMobilityStationAdminStatus1 == null)
                throw new ArgumentNullException("The given eMobilityStationAdminStatus1 must not be null!");

            return eMobilityStationAdminStatus1.CompareTo(eMobilityStationAdminStatus2) > 0;

        }

        #endregion

        #region Operator >= (eMobilityStationAdminStatus1, eMobilityStationAdminStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus1">A eMobilityStationAdminStatus.</param>
        /// <param name="eMobilityStationAdminStatus2">Another eMobilityStationAdminStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (eMobilityStationAdminStatus eMobilityStationAdminStatus1, eMobilityStationAdminStatus eMobilityStationAdminStatus2)
        {
            return !(eMobilityStationAdminStatus1 < eMobilityStationAdminStatus2);
        }

        #endregion

        #endregion

        #region IComparable<eMobilityStationAdminStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eMobilityStationAdminStatus.
            var eMobilityStationAdminStatus = Object as eMobilityStationAdminStatus;
            if ((Object) eMobilityStationAdminStatus == null)
                throw new ArgumentException("The given object is not a eMobilityStationAdminStatus!");

            return CompareTo(eMobilityStationAdminStatus);

        }

        #endregion

        #region CompareTo(eMobilityStationAdminStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus">An object to compare with.</param>
        public Int32 CompareTo(eMobilityStationAdminStatus eMobilityStationAdminStatus)
        {

            if ((Object) eMobilityStationAdminStatus == null)
                throw new ArgumentNullException("The given eMobilityStationAdminStatus must not be null!");

            // Compare eMobilityStation Ids
            var _Result = Id.CompareTo(eMobilityStationAdminStatus.Id);

            // If equal: Compare eMobilityStation status
            if (_Result == 0)
                _Result = Status.CompareTo(eMobilityStationAdminStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityStationAdminStatus> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an eMobilityStationAdminStatus.
            var eMobilityStationAdminStatus = Object as eMobilityStationAdminStatus;
            if ((Object) eMobilityStationAdminStatus == null)
                return false;

            return this.Equals(eMobilityStationAdminStatus);

        }

        #endregion

        #region Equals(eMobilityStationAdminStatus)

        /// <summary>
        /// Compares two eMobilityStation identifications for equality.
        /// </summary>
        /// <param name="eMobilityStationAdminStatus">An eMobilityStation identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityStationAdminStatus eMobilityStationAdminStatus)
        {

            if ((Object) eMobilityStationAdminStatus == null)
                return false;

            return Id.    Equals(eMobilityStationAdminStatus.Id) &&
                   Status.Equals(eMobilityStationAdminStatus.Status);

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
