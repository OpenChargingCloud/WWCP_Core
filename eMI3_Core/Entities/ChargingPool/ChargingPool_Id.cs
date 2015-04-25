/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3_Core>
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

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Charging Pool (EVCP Id).
    /// </summary>
    public class ChargingPool_Id : IId,
                                   IEquatable<ChargingPool_Id>,
                                   IComparable<ChargingPool_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String _Id;

        #endregion

        #region Properties

        #region New

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle Charging Pool (EVCP Id).
        /// </summary>
        public static ChargingPool_Id New
        {
            get
            {
                return new ChargingPool_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

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

        /// <summary>
        /// Generate a new Electric Vehicle Charging Pool identification (EVCP Id)
        /// based on the given string.
        /// </summary>
        private ChargingPool_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Pool identification (EVCP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {
            return new ChargingPool_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Pool identification (EVCP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Pool identification.</param>
        /// <param name="ChargingPoolId">The parsed Electric Vehicle Charging Pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {
            try
            {
                ChargingPoolId = new ChargingPool_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingPoolId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Pool identification.
        /// </summary>
        public ChargingPool_Id Clone
        {
            get
            {
                return new ChargingPool_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingPoolId1, ChargingPoolId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolId1 == null) || ((Object) ChargingPoolId2 == null))
                return false;

            return ChargingPoolId1.Equals(ChargingPoolId2);

        }

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {
            return !(ChargingPoolId1 == ChargingPoolId2);
        }

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException("The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {
            return !(ChargingPoolId1 > ChargingPoolId2);
        }

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException("The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {
            return !(ChargingPoolId1 < ChargingPoolId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingPool_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingPoolId.
            var ChargingPoolId = Object as ChargingPool_Id;
            if ((Object) ChargingPoolId == null)
                throw new ArgumentException("The given object is not a ChargingPoolId!");

            return CompareTo(ChargingPoolId);

        }

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId">An object to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)
        {

            if ((Object) ChargingPoolId == null)
                throw new ArgumentNullException("The given ChargingPoolId must not be null!");

            // Compare the length of the ChargingPoolIds
            var _Result = this.Length.CompareTo(ChargingPoolId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingPoolId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPool_Id> Members

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

            // Check if the given object is an ChargingPoolId.
            var ChargingPoolId = Object as ChargingPool_Id;
            if ((Object) ChargingPoolId == null)
                return false;

            return this.Equals(ChargingPoolId);

        }

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two ChargingPoolIds for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A ChargingPoolId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)
        {

            if ((Object) ChargingPoolId == null)
                return false;

            return _Id.Equals(ChargingPoolId._Id);

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
