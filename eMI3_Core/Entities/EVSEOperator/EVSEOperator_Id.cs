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
    /// The unique identification of an Electric Vehicle Supply Equipment Operator (EVSE Op).
    /// </summary>
    public class EVSEOperator_Id : IId, IEquatable<EVSEOperator_Id>, IComparable<EVSEOperator_Id>
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

        #region EVSEOperator_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment Operator (EVSE Op) identification.
        /// </summary>
        public EVSEOperator_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region EVSEOperator_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment Operator (EVSE Op) identification.
        /// based on the given string.
        /// </summary>
        public EVSEOperator_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new EVSEOperator_Id.
        /// </summary>
        public static EVSEOperator_Id New
        {
            get
            {
                return new EVSEOperator_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSEOperator_Id.
        /// </summary>
        public EVSEOperator_Id Clone
        {
            get
            {
                return new EVSEOperator_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEOperator_Id1, EVSEOperator_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEOperator_Id1 == null) || ((Object) EVSEOperator_Id2 == null))
                return false;

            return EVSEOperator_Id1.Equals(EVSEOperator_Id2);

        }

        #endregion

        #region Operator != (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 == EVSEOperator_Id2);
        }

        #endregion

        #region Operator <  (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {

            if ((Object) EVSEOperator_Id1 == null)
                throw new ArgumentNullException("The given EVSEOperator_Id1 must not be null!");

            return EVSEOperator_Id1.CompareTo(EVSEOperator_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 > EVSEOperator_Id2);
        }

        #endregion

        #region Operator >  (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {

            if ((Object) EVSEOperator_Id1 == null)
                throw new ArgumentNullException("The given EVSEOperator_Id1 must not be null!");

            return EVSEOperator_Id1.CompareTo(EVSEOperator_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 < EVSEOperator_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVSEOperator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSEOperator_Id.
            var EVSEOperator_Id = Object as EVSEOperator_Id;
            if ((Object) EVSEOperator_Id == null)
                throw new ArgumentException("The given object is not a EVSEOperator_Id!");

            return CompareTo(EVSEOperator_Id);

        }

        #endregion

        #region CompareTo(EVSEOperator_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id">An object to compare with.</param>
        public Int32 CompareTo(EVSEOperator_Id EVSEOperator_Id)
        {

            if ((Object) EVSEOperator_Id == null)
                throw new ArgumentNullException("The given EVSEOperator_Id must not be null!");

            // Compare the length of the EVSEOperator_Ids
            var _Result = this.Length.CompareTo(EVSEOperator_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVSEOperator_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEOperator_Id> Members

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

            // Check if the given object is an EVSEOperator_Id.
            var EVSEOperator_Id = Object as EVSEOperator_Id;
            if ((Object) EVSEOperator_Id == null)
                return false;

            return this.Equals(EVSEOperator_Id);

        }

        #endregion

        #region Equals(EVSEOperator_Id)

        /// <summary>
        /// Compares two EVSEOperator_Ids for equality.
        /// </summary>
        /// <param name="EVSEOperator_Id">A EVSEOperator_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEOperator_Id EVSEOperator_Id)
        {

            if ((Object) EVSEOperator_Id == null)
                return false;

            return _Id.Equals(EVSEOperator_Id._Id);

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
