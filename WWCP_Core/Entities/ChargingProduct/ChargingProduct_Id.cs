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
    /// The unique identification of an Electric Vehicle Charging Product.
    /// </summary>
    public class ChargingProduct_Id : IId,
                                      IEquatable<ChargingProduct_Id>,
                                      IComparable<ChargingProduct_Id>

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
        /// Returns a new Electric Vehicle Charging Product identification.
        /// </summary>
        public static ChargingProduct_Id New
        {
            get
            {
                return ChargingProduct_Id.Parse(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Charging Product identification.
        /// based on the given string.
        /// </summary>
        private ChargingProduct_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Product identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Product identification.</param>
        public static ChargingProduct_Id Parse(String Text)
        {
            return new ChargingProduct_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingProductId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Product identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Product identification.</param>
        /// <param name="ChargingProductId">The parsed Electric Vehicle Charging Product identification.</param>
        public static Boolean TryParse(String Text, out ChargingProduct_Id ChargingProductId)
        {
            try
            {
                ChargingProductId = new ChargingProduct_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingProductId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Product identification.
        /// </summary>
        public ChargingProduct_Id Clone
        {
            get
            {
                return new ChargingProduct_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A ChargingProductId.</param>
        /// <param name="ChargingProductId2">Another ChargingProductId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingProductId1, ChargingProductId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingProductId1 == null) || ((Object) ChargingProductId2 == null))
                return false;

            return ChargingProductId1.Equals(ChargingProductId2);

        }

        #endregion

        #region Operator != (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A ChargingProductId.</param>
        /// <param name="ChargingProductId2">Another ChargingProductId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {
            return !(ChargingProductId1 == ChargingProductId2);
        }

        #endregion

        #region Operator <  (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A ChargingProductId.</param>
        /// <param name="ChargingProductId2">Another ChargingProductId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {

            if ((Object) ChargingProductId1 == null)
                throw new ArgumentNullException("The given ChargingProductId1 must not be null!");

            return ChargingProductId1.CompareTo(ChargingProductId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A ChargingProductId.</param>
        /// <param name="ChargingProductId2">Another ChargingProductId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {
            return !(ChargingProductId1 > ChargingProductId2);
        }

        #endregion

        #region Operator >  (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A ChargingProductId.</param>
        /// <param name="ChargingProductId2">Another ChargingProductId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {

            if ((Object) ChargingProductId1 == null)
                throw new ArgumentNullException("The given ChargingProductId1 must not be null!");

            return ChargingProductId1.CompareTo(ChargingProductId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A ChargingProductId.</param>
        /// <param name="ChargingProductId2">Another ChargingProductId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {
            return !(ChargingProductId1 < ChargingProductId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingProductId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingProductId.
            var ChargingProductId = Object as ChargingProduct_Id;
            if ((Object) ChargingProductId == null)
                throw new ArgumentException("The given object is not a ChargingProductId!");

            return CompareTo(ChargingProductId);

        }

        #endregion

        #region CompareTo(ChargingProductId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId">An object to compare with.</param>
        public Int32 CompareTo(ChargingProduct_Id ChargingProductId)
        {

            if ((Object) ChargingProductId == null)
                throw new ArgumentNullException("The given ChargingProductId must not be null!");

            // Compare the length of the ChargingProductIds
            var _Result = this.Length.CompareTo(ChargingProductId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingProductId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingProductId> Members

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

            // Check if the given object is an ChargingProductId.
            var ChargingProductId = Object as ChargingProduct_Id;
            if ((Object) ChargingProductId == null)
                return false;

            return this.Equals(ChargingProductId);

        }

        #endregion

        #region Equals(ChargingProductId)

        /// <summary>
        /// Compares two ChargingProductIds for equality.
        /// </summary>
        /// <param name="ChargingProductId">A ChargingProductId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingProduct_Id ChargingProductId)
        {

            if ((Object) ChargingProductId == null)
                return false;

            return _Id.Equals(ChargingProductId._Id);

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

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
