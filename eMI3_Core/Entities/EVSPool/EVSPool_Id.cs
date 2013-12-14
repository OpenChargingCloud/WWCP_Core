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
    /// The unique identification of an Electric Vehicle Pool (EVP Id).
    /// </summary>
    public class EVSPool_Id : IId, IEquatable<EVSPool_Id>, IComparable<EVSPool_Id>
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

        #region EVP_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Pool identification (EVP Id).
        /// </summary>
        public EVSPool_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region EVP_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Pool identification (EVP Id)
        /// based on the given string.
        /// </summary>
        public EVSPool_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new EVP_Id.
        /// </summary>
        public static EVSPool_Id New
        {
            get
            {
                return new EVSPool_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVP_Id.
        /// </summary>
        public EVSPool_Id Clone
        {
            get
            {
                return new EVSPool_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVP_Id1, EVP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id1">A EVP_Id.</param>
        /// <param name="EVP_Id2">Another EVP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSPool_Id EVP_Id1, EVSPool_Id EVP_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVP_Id1, EVP_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVP_Id1 == null) || ((Object) EVP_Id2 == null))
                return false;

            return EVP_Id1.Equals(EVP_Id2);

        }

        #endregion

        #region Operator != (EVP_Id1, EVP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id1">A EVP_Id.</param>
        /// <param name="EVP_Id2">Another EVP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSPool_Id EVP_Id1, EVSPool_Id EVP_Id2)
        {
            return !(EVP_Id1 == EVP_Id2);
        }

        #endregion

        #region Operator <  (EVP_Id1, EVP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id1">A EVP_Id.</param>
        /// <param name="EVP_Id2">Another EVP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSPool_Id EVP_Id1, EVSPool_Id EVP_Id2)
        {

            if ((Object) EVP_Id1 == null)
                throw new ArgumentNullException("The given EVP_Id1 must not be null!");

            return EVP_Id1.CompareTo(EVP_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVP_Id1, EVP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id1">A EVP_Id.</param>
        /// <param name="EVP_Id2">Another EVP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSPool_Id EVP_Id1, EVSPool_Id EVP_Id2)
        {
            return !(EVP_Id1 > EVP_Id2);
        }

        #endregion

        #region Operator >  (EVP_Id1, EVP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id1">A EVP_Id.</param>
        /// <param name="EVP_Id2">Another EVP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSPool_Id EVP_Id1, EVSPool_Id EVP_Id2)
        {

            if ((Object) EVP_Id1 == null)
                throw new ArgumentNullException("The given EVP_Id1 must not be null!");

            return EVP_Id1.CompareTo(EVP_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVP_Id1, EVP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id1">A EVP_Id.</param>
        /// <param name="EVP_Id2">Another EVP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSPool_Id EVP_Id1, EVSPool_Id EVP_Id2)
        {
            return !(EVP_Id1 < EVP_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVP_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVP_Id.
            var EVP_Id = Object as EVSPool_Id;
            if ((Object) EVP_Id == null)
                throw new ArgumentException("The given object is not a EVP_Id!");

            return CompareTo(EVP_Id);

        }

        #endregion

        #region CompareTo(EVP_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVP_Id">An object to compare with.</param>
        public Int32 CompareTo(EVSPool_Id EVP_Id)
        {

            if ((Object) EVP_Id == null)
                throw new ArgumentNullException("The given EVP_Id must not be null!");

            // Compare the length of the EVP_Ids
            var _Result = this.Length.CompareTo(EVP_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVP_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVP_Id> Members

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

            // Check if the given object is an EVP_Id.
            var EVP_Id = Object as EVSPool_Id;
            if ((Object) EVP_Id == null)
                return false;

            return this.Equals(EVP_Id);

        }

        #endregion

        #region Equals(EVP_Id)

        /// <summary>
        /// Compares two EVP_Ids for equality.
        /// </summary>
        /// <param name="EVP_Id">A EVP_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSPool_Id EVP_Id)
        {

            if ((Object) EVP_Id == null)
                return false;

            return _Id.Equals(EVP_Id._Id);

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
