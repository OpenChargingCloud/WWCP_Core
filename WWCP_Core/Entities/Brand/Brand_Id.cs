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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of a brand.
    /// </summary>
    public class Brand_Id : IId,
                            IEquatable<Brand_Id>,
                            IComparable<Brand_Id>

    {

        #region Data

        private readonly String _Id;

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

        #region Clone

        /// <summary>
        /// Clone this brand identification.
        /// </summary>
        public Brand_Id Clone
        {
            get
            {
                return new Brand_Id(new String(_Id.ToCharArray()));
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new brand identification
        /// based on the given string.
        /// </summary>
        private Brand_Id(String  Id)
        {

            #region Initial checks

            if (Id.IsNullOrEmpty())
                throw new ArgumentNullException("Id", "The parameter must not be null or empty!");

            #endregion

            this._Id  = Id;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a brand identification.
        /// </summary>
        public static Brand_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "Text");

            #endregion

            return new Brand_Id(Text);

        }

        #endregion

        #region TryParse(Text, out BrandId)

        /// <summary>
        /// Try to parse the given string as a brand identification.
        /// </summary>
        /// <param name="Text">A string to parse.</param>
        /// <param name="BrandId">The parsed unique brand identification.</param>
        public static Boolean TryParse(String Text, out Brand_Id BrandId)
        {

            #region Initial checks

            if (Text == null)
                throw new ArgumentNullException("Text", "The given text must not be null or empty!");

            #endregion

            BrandId = new Brand_Id(Text);

            return true;

        }

        #endregion


        #region Operator overloading

        #region Operator == (BrandId1, BrandId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId1">A brand identification.</param>
        /// <param name="BrandId2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Brand_Id BrandId1, Brand_Id BrandId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(BrandId1, BrandId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) BrandId1 == null) || ((Object) BrandId2 == null))
                return false;

            return BrandId1.Equals(BrandId2);

        }

        #endregion

        #region Operator != (BrandId1, BrandId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId1">A brand identification.</param>
        /// <param name="BrandId2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Brand_Id BrandId1, Brand_Id BrandId2)
        {
            return !(BrandId1 == BrandId2);
        }

        #endregion

        #region Operator <  (BrandId1, BrandId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId1">A brand identification.</param>
        /// <param name="BrandId2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Brand_Id BrandId1, Brand_Id BrandId2)
        {

            if ((Object) BrandId1 == null)
                throw new ArgumentNullException("The given brand identification must not be null!");

            return BrandId1.CompareTo(BrandId2) < 0;

        }

        #endregion

        #region Operator <= (BrandId1, BrandId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId1">A brand identification.</param>
        /// <param name="BrandId2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Brand_Id BrandId1, Brand_Id BrandId2)
        {
            return !(BrandId1 > BrandId2);
        }

        #endregion

        #region Operator >  (BrandId1, BrandId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId1">A brand identification.</param>
        /// <param name="BrandId2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Brand_Id BrandId1, Brand_Id BrandId2)
        {

            if ((Object) BrandId1 == null)
                throw new ArgumentNullException("The given BrandId1 must not be null!");

            return BrandId1.CompareTo(BrandId2) > 0;

        }

        #endregion

        #region Operator >= (BrandId1, BrandId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId1">A brand identification.</param>
        /// <param name="BrandId2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Brand_Id BrandId1, Brand_Id BrandId2)
        {
            return !(BrandId1 < BrandId2);
        }

        #endregion

        #endregion

        #region IComparable<Brand_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an BrandId.
            var BrandId = Object as Brand_Id;
            if ((Object) BrandId == null)
                throw new ArgumentException("The given object is not a BrandId!");

            return CompareTo(BrandId);

        }

        #endregion

        #region CompareTo(BrandId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="BrandId">An object to compare with.</param>
        public Int32 CompareTo(Brand_Id BrandId)
        {

            if ((Object) BrandId == null)
                throw new ArgumentNullException("The given BrandId must not be null!");

            return _Id.CompareTo(BrandId._Id);

        }

        #endregion

        #endregion

        #region IEquatable<Brand_Id> Members

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

            // Check if the given object is a brand identification.
            var BrandId = Object as Brand_Id;
            if ((Object) BrandId == null)
                return false;

            return this.Equals(BrandId);

        }

        #endregion

        #region Equals(BrandId)

        /// <summary>
        /// Compares two brand identifications for equality.
        /// </summary>
        /// <param name="BrandId">A brand identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Brand_Id BrandId)
        {

            if ((Object) BrandId == null)
                return false;

            return _Id.Equals(BrandId._Id);

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
            return _Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {

            return _Id;

        }

        #endregion

    }

}
