/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The current status of an eMAId.
    /// </summary>
    public class eMAIdStatus : IEquatable<eMAIdStatus>,
                               IComparable<eMAIdStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of an eMAId.
        /// </summary>
        public EMobilityAccount_Id  Id       { get; }

        /// <summary>
        /// The current status of an eMAId.
        /// </summary>
        public eMAIdStatusType      Status   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new eMAId status.
        /// </summary>
        /// <param name="eMAId">The unique identification of an eMAId.</param>
        /// <param name="Status">The current status of an eMAId.</param>
        public eMAIdStatus(EMobilityAccount_Id  eMAId,
                           eMAIdStatusType      Status)

        {

            #region Initial checks

            if (eMAId == null)
                throw new ArgumentNullException(nameof(eMAId), "The given unique identification of an eMAId must not be null!");

            #endregion

            this.Id      = eMAId;
            this.Status  = Status;

        }

        #endregion


        #region Operator overloading

        #region Operator == (eMAIdStatus1, eMAIdStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus1">A eMAIdStatus.</param>
        /// <param name="eMAIdStatus2">Another eMAIdStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator ==(eMAIdStatus eMAIdStatus1, eMAIdStatus eMAIdStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(eMAIdStatus1, eMAIdStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object)eMAIdStatus1 == null) || ((Object)eMAIdStatus2 == null))
                return false;

            return eMAIdStatus1.Equals(eMAIdStatus2);

        }

        #endregion

        #region Operator != (eMAIdStatus1, eMAIdStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus1">A eMAIdStatus.</param>
        /// <param name="eMAIdStatus2">Another eMAIdStatus.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator !=(eMAIdStatus eMAIdStatus1, eMAIdStatus eMAIdStatus2)
        {
            return !(eMAIdStatus1 == eMAIdStatus2);
        }

        #endregion

        #region Operator <  (eMAIdStatus1, eMAIdStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus1">A eMAIdStatus.</param>
        /// <param name="eMAIdStatus2">Another eMAIdStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <(eMAIdStatus eMAIdStatus1, eMAIdStatus eMAIdStatus2)
        {

            if ((Object)eMAIdStatus1 == null)
                throw new ArgumentNullException("The given eMAIdStatus1 must not be null!");

            return eMAIdStatus1.CompareTo(eMAIdStatus2) < 0;

        }

        #endregion

        #region Operator <= (eMAIdStatus1, eMAIdStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus1">A eMAIdStatus.</param>
        /// <param name="eMAIdStatus2">Another eMAIdStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <=(eMAIdStatus eMAIdStatus1, eMAIdStatus eMAIdStatus2)
        {
            return !(eMAIdStatus1 > eMAIdStatus2);
        }

        #endregion

        #region Operator >  (eMAIdStatus1, eMAIdStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus1">A eMAIdStatus.</param>
        /// <param name="eMAIdStatus2">Another eMAIdStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >(eMAIdStatus eMAIdStatus1, eMAIdStatus eMAIdStatus2)
        {

            if ((Object)eMAIdStatus1 == null)
                throw new ArgumentNullException("The given eMAIdStatus1 must not be null!");

            return eMAIdStatus1.CompareTo(eMAIdStatus2) > 0;

        }

        #endregion

        #region Operator >= (eMAIdStatus1, eMAIdStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus1">A eMAIdStatus.</param>
        /// <param name="eMAIdStatus2">Another eMAIdStatus.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >=(eMAIdStatus eMAIdStatus1, eMAIdStatus eMAIdStatus2)
        {
            return !(eMAIdStatus1 < eMAIdStatus2);
        }

        #endregion

        #endregion

        #region IComparable<eMAIdStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eMAIdStatus.
            var eMAIdStatus = Object as eMAIdStatus;
            if ((Object)eMAIdStatus == null)
                throw new ArgumentException("The given object is not a eMAIdStatus!");

            return CompareTo(eMAIdStatus);

        }

        #endregion

        #region CompareTo(eMAIdStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdStatus">An object to compare with.</param>
        public Int32 CompareTo(eMAIdStatus eMAIdStatus)
        {

            if ((Object)eMAIdStatus == null)
                throw new ArgumentNullException("The given eMAIdStatus must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(eMAIdStatus.Id);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = Status.CompareTo(eMAIdStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMAIdStatus> Members

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

            // Check if the given object is an eMAIdStatus.
            var eMAIdStatus = Object as eMAIdStatus;
            if ((Object)eMAIdStatus == null)
                return false;

            return this.Equals(eMAIdStatus);

        }

        #endregion

        #region Equals(eMAIdStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="eMAIdStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMAIdStatus eMAIdStatus)
        {

            if ((Object)eMAIdStatus == null)
                return false;

            return Id.Equals(eMAIdStatus.Id) &&
                   Status.Equals(eMAIdStatus.Status);

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
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {

            return String.Concat(Id, " -> ", Status.ToString());

        }

        #endregion

    }


    /// <summary>
    /// The current status of an eMAId.
    /// </summary>
    public enum eMAIdStatusType
    {

        /// <summary>
        /// Unclear or unknown status of the eMAId.
        /// </summary>
        Unspecified  = 0,

        /// <summary>
        /// The eMAId is invalid.
        /// </summary>
        Invalid      = 1,

        /// <summary>
        /// The processing of the eMAId led to an error.
        /// </summary>
        Error        = 2

    }

}