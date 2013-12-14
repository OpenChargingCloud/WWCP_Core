/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Mockup <http://www.github.com/eMI3/Mockup>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

namespace de.eMI3
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment (EVSE_Id)
    /// </summary>
    public class EVSE_Id : IId, IEquatable<EVSE_Id>, IComparable<EVSE_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String _Id;

        #endregion

        #region Properties

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return (UInt64) _Id.Length;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSE_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification (EVSE_Id).
        /// </summary>
        public EVSE_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region EVSE_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification (EVSE_Id)
        /// based on the given string.
        /// </summary>
        public EVSE_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new EVSE_Id.
        /// </summary>
        public static EVSE_Id New
        {
            get
            {
                return new EVSE_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSE_Id.
        /// </summary>
        public EVSE_Id Clone
        {
            get
            {
                return new EVSE_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSE_Id1, EVSE_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSE_Id1 == null) || ((Object) EVSE_Id2 == null))
                return false;

            return EVSE_Id1.Equals(EVSE_Id2);

        }

        #endregion

        #region Operator != (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {
            return !(EVSE_Id1 == EVSE_Id2);
        }

        #endregion

        #region Operator <  (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {

            if ((Object) EVSE_Id1 == null)
                throw new ArgumentNullException("The given EVSE_Id1 must not be null!");

            return EVSE_Id1.CompareTo(EVSE_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {
            return !(EVSE_Id1 > EVSE_Id2);
        }

        #endregion

        #region Operator >  (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {

            if ((Object) EVSE_Id1 == null)
                throw new ArgumentNullException("The given EVSE_Id1 must not be null!");

            return EVSE_Id1.CompareTo(EVSE_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {
            return !(EVSE_Id1 < EVSE_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVSE_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE_Id.
            var EVSE_Id = Object as EVSE_Id;
            if ((Object) EVSE_Id == null)
                throw new ArgumentException("The given object is not a EVSE_Id!");

            return CompareTo(EVSE_Id);

        }

        #endregion

        #region CompareTo(EVSE_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSE_Id)
        {

            if ((Object) EVSE_Id == null)
                throw new ArgumentNullException("The given EVSE_Id must not be null!");

            // Compare the length of the EVSE_Ids
            var _Result = this.Length.CompareTo(EVSE_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVSE_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSE_Id> Members

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

            // Check if the given object is an EVSE_Id.
            var EVSE_Id = Object as EVSE_Id;
            if ((Object) EVSE_Id == null)
                return false;

            return this.Equals(EVSE_Id);

        }

        #endregion

        #region Equals(EVSE_Id)

        /// <summary>
        /// Compares two EVSE_Ids for equality.
        /// </summary>
        /// <param name="EVSE_Id">A EVSE_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSE_Id)
        {

            if ((Object) EVSE_Id == null)
                return false;

            return _Id.Equals(EVSE_Id._Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return _Id.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
