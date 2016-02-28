/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
    /// The current status of an EVSE.
    /// </summary>
    public class EVSEStatus : IEquatable<EVSEStatus>,
                              IComparable<EVSEStatus>
    {

        #region Properties

        #region EVSE

        private readonly EVSE _EVSE;

        /// <summary>
        /// The related the Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        public EVSE EVSE
        {
            get
            {
                return _EVSE;
            }
        }

        #endregion

        #region Id

        private readonly EVSE_Id _Id;

        /// <summary>
        /// The unique identification of an EVSE.
        /// </summary>
        public EVSE_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Status

        private readonly EVSEStatusType _Status;

        /// <summary>
        /// The current status of an EVSE.
        /// </summary>
        public EVSEStatusType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSEStatusRecord(EVSE)

        /// <summary>
        /// Create a new EVSE status and store
        /// a reference to the given EVSE.
        /// </summary>
        /// <param name="EVSE">The current status of an EVSE.</param>
        public EVSEStatus(EVSE EVSE)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            this._EVSE    = EVSE;
            this._Id      = EVSE.Id;
            this._Status  = EVSE.Status.Value;

        }

        #endregion

        #region EVSEStatusRecord(Id, Status)

        /// <summary>
        /// Create a new EVSE status.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        /// <param name="Status">The current status of an EVSE.</param>
        public EVSEStatus(EVSE_Id         Id,
                          EVSEStatusType  Status)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException(nameof(Id), "The given unique identification of an EVSE must not be null!");

            #endregion

            this._Id      = Id;
            this._Status  = Status;

        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">A EVSEStatus.</param>
        /// <param name="EVSEStatus2">Another EVSEStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatus1 == null) || ((Object) EVSEStatus2 == null))
                return false;

            return EVSEStatus1.Equals(EVSEStatus2);

        }

        #endregion

        #region Operator != (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">A EVSEStatus.</param>
        /// <param name="EVSEStatus2">Another EVSEStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {
            return !(EVSEStatus1 == EVSEStatus2);
        }

        #endregion

        #region Operator <  (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">A EVSEStatus.</param>
        /// <param name="EVSEStatus2">Another EVSEStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            if ((Object) EVSEStatus1 == null)
                throw new ArgumentNullException("The given EVSEStatus1 must not be null!");

            return EVSEStatus1.CompareTo(EVSEStatus2) < 0;

        }

        #endregion

        #region Operator <= (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">A EVSEStatus.</param>
        /// <param name="EVSEStatus2">Another EVSEStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {
            return !(EVSEStatus1 > EVSEStatus2);
        }

        #endregion

        #region Operator >  (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">A EVSEStatus.</param>
        /// <param name="EVSEStatus2">Another EVSEStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            if ((Object) EVSEStatus1 == null)
                throw new ArgumentNullException("The given EVSEStatus1 must not be null!");

            return EVSEStatus1.CompareTo(EVSEStatus2) > 0;

        }

        #endregion

        #region Operator >= (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">A EVSEStatus.</param>
        /// <param name="EVSEStatus2">Another EVSEStatus.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {
            return !(EVSEStatus1 < EVSEStatus2);
        }

        #endregion

        #endregion

        #region IComparable<EVSEStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSEStatus.
            var EVSEStatus = Object as EVSEStatus;
            if ((Object) EVSEStatus == null)
                throw new ArgumentException("The given object is not a EVSEStatus!");

            return CompareTo(EVSEStatus);

        }

        #endregion

        #region CompareTo(EVSEStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus">An object to compare with.</param>
        public Int32 CompareTo(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                throw new ArgumentNullException("The given EVSEStatus must not be null!");

            // Compare EVSE Ids
            var _Result = _Id.CompareTo(EVSEStatus._Id);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = _Status.CompareTo(EVSEStatus._Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatus> Members

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

            // Check if the given object is an EVSEStatus.
            var EVSEStatus = Object as EVSEStatus;
            if ((Object) EVSEStatus == null)
                return false;

            return this.Equals(EVSEStatus);

        }

        #endregion

        #region Equals(EVSEStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                return false;

            return _Id.    Equals(EVSEStatus._Id) &&
                   _Status.Equals(EVSEStatus._Status);

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
                return _Id.GetHashCode() * 17 ^ _Status.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {

            return String.Concat(_Id, " -> ", _Status.ToString());

        }

        #endregion

    }

}
