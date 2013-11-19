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
    /// The unique identification of an Electric Vehicle Supply Equipment (ChargingStation_Id)
    /// </summary>
    public class ChargingStation_Id : IId, IEquatable<ChargingStation_Id>, IComparable<ChargingStation_Id>
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

        #region ChargingStation_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification (ChargingStation_Id).
        /// </summary>
        public ChargingStation_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region ChargingStation_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification (ChargingStation_Id)
        /// based on the given string.
        /// </summary>
        public ChargingStation_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new ChargingStation_Id.
        /// </summary>
        public static ChargingStation_Id New
        {
            get
            {
                return new ChargingStation_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an ChargingStation_Id.
        /// </summary>
        public ChargingStation_Id Clone
        {
            get
            {
                return new ChargingStation_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStation_Id1, ChargingStation_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStation_Id1 == null) || ((Object) ChargingStation_Id2 == null))
                return false;

            return ChargingStation_Id1.Equals(ChargingStation_Id2);

        }

        #endregion

        #region Operator != (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {
            return !(ChargingStation_Id1 == ChargingStation_Id2);
        }

        #endregion

        #region Operator <  (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {

            if ((Object) ChargingStation_Id1 == null)
                throw new ArgumentNullException("The given ChargingStation_Id1 must not be null!");

            return ChargingStation_Id1.CompareTo(ChargingStation_Id2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {
            return !(ChargingStation_Id1 > ChargingStation_Id2);
        }

        #endregion

        #region Operator >  (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {

            if ((Object) ChargingStation_Id1 == null)
                throw new ArgumentNullException("The given ChargingStation_Id1 must not be null!");

            return ChargingStation_Id1.CompareTo(ChargingStation_Id2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {
            return !(ChargingStation_Id1 < ChargingStation_Id2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStation_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingStation_Id.
            var ChargingStation_Id = Object as ChargingStation_Id;
            if ((Object) ChargingStation_Id == null)
                throw new ArgumentException("The given object is not a ChargingStation_Id!");

            return CompareTo(ChargingStation_Id);

        }

        #endregion

        #region CompareTo(ChargingStation_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id">An object to compare with.</param>
        public Int32 CompareTo(ChargingStation_Id ChargingStation_Id)
        {

            if ((Object) ChargingStation_Id == null)
                throw new ArgumentNullException("The given ChargingStation_Id must not be null!");

            // Compare the length of the ChargingStation_Ids
            var _Result = this.Length.CompareTo(ChargingStation_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingStation_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation_Id> Members

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

            // Check if the given object is an ChargingStation_Id.
            var ChargingStation_Id = Object as ChargingStation_Id;
            if ((Object) ChargingStation_Id == null)
                return false;

            return this.Equals(ChargingStation_Id);

        }

        #endregion

        #region Equals(ChargingStation_Id)

        /// <summary>
        /// Compares two ChargingStation_Ids for equality.
        /// </summary>
        /// <param name="ChargingStation_Id">A ChargingStation_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation_Id ChargingStation_Id)
        {

            if ((Object) ChargingStation_Id == null)
                return false;

            return _Id.Equals(ChargingStation_Id._Id);

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
