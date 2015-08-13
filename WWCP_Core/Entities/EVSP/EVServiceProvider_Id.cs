/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Service Provider (EVSP Id).
    /// </summary>
    public class EVSP_Id : IId,
                           IEquatable<EVSP_Id>,
                           IComparable<EVSP_Id>

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
        /// Generate a new unique identification of an Electric Vehicle Service Provider (EVSP Id).
        /// </summary>
        public static EVSP_Id New
        {
            get
            {
                return new EVSP_Id(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Service Provider (EVSP Id)
        /// based on the given string.
        /// </summary>
        public EVSP_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Service Provider (EVSP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Service Provider identification.</param>
        public static EVSP_Id Parse(String Text)
        {
            return new EVSP_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Service Provider (EVSP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Service Provider identification.</param>
        /// <param name="ChargingPoolId">The parsed Electric Vehicle Service Provider identification.</param>
        public static Boolean TryParse(String Text, out EVSP_Id ChargingPoolId)
        {
            try
            {
                ChargingPoolId = new EVSP_Id(Text);
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
        /// Clone this Electric Vehicle Service Provider identification.
        /// </summary>
        public EVSP_Id Clone
        {
            get
            {
                return new EVSP_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSP_Id EVSPId1, EVSP_Id EVSPId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSPId1, EVSPId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSPId1 == null) || ((Object) EVSPId2 == null))
                return false;

            return EVSPId1.Equals(EVSPId2);

        }

        #endregion

        #region Operator != (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSP_Id EVSPId1, EVSP_Id EVSPId2)
        {
            return !(EVSPId1 == EVSPId2);
        }

        #endregion

        #region Operator <  (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSP_Id EVSPId1, EVSP_Id EVSPId2)
        {

            if ((Object) EVSPId1 == null)
                throw new ArgumentNullException("The given EVSPId1 must not be null!");

            return EVSPId1.CompareTo(EVSPId2) < 0;

        }

        #endregion

        #region Operator <= (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSP_Id EVSPId1, EVSP_Id EVSPId2)
        {
            return !(EVSPId1 > EVSPId2);
        }

        #endregion

        #region Operator >  (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSP_Id EVSPId1, EVSP_Id EVSPId2)
        {

            if ((Object) EVSPId1 == null)
                throw new ArgumentNullException("The given EVSPId1 must not be null!");

            return EVSPId1.CompareTo(EVSPId2) > 0;

        }

        #endregion

        #region Operator >= (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSP_Id EVSPId1, EVSP_Id EVSPId2)
        {
            return !(EVSPId1 < EVSPId2);
        }

        #endregion

        #endregion

        #region IComparable<EVSP_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSPId.
            var EVSPId = Object as EVSP_Id;
            if ((Object) EVSPId == null)
                throw new ArgumentException("The given object is not a EVSPId!");

            return CompareTo(EVSPId);

        }

        #endregion

        #region CompareTo(EVSPId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId">An object to compare with.</param>
        public Int32 CompareTo(EVSP_Id EVSPId)
        {

            if ((Object) EVSPId == null)
                throw new ArgumentNullException("The given EVSPId must not be null!");

            // Compare the length of the EVSPIds
            var _Result = this.Length.CompareTo(EVSPId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVSPId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSP_Id> Members

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

            // Check if the given object is an EVSPId.
            var EVSPId = Object as EVSP_Id;
            if ((Object) EVSPId == null)
                return false;

            return this.Equals(EVSPId);

        }

        #endregion

        #region Equals(EVSPId)

        /// <summary>
        /// Compares two EVSPIds for equality.
        /// </summary>
        /// <param name="EVSPId">A EVSPId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSP_Id EVSPId)
        {

            if ((Object) EVSPId == null)
                return false;

            return _Id.Equals(EVSPId._Id);

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
