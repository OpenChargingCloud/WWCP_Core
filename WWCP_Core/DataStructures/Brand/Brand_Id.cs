/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of a brand.
    /// </summary>
    public struct Brand_Id : IId,
                             IEquatable <Brand_Id>,
                             IComparable<Brand_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new brand identification.
        /// based on the given string.
        /// </summary>
        private Brand_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region New

        /// <summary>
        /// Returns a new brand identification.
        /// </summary>
        public static Brand_Id New
            => Parse(Guid.NewGuid().ToString());

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a brand identification.
        /// </summary>
        /// <param name="Text">A text representation of a brand identification.</param>
        public static Brand_Id Parse(String Text)
        {

            if (Text == null || Text.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given brand identification must not be null or empty!");

            return new Brand_Id(Text);

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parse the given string as a brand identification.
        /// </summary>
        /// <param name="Text">A text representation of a brand identification.</param>
        public static Brand_Id? TryParse(String Text)
        {

            if (Text != null &&
                Text.Trim().IsNotNullOrEmpty())
            {

                try
                {

                    return new Brand_Id(Text);

                }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
                catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
                { }

            }

            return default(Brand_Id);

        }

        #endregion

        #region TryParse(Text, out BrandId)

        /// <summary>
        /// Parse the given string as a brand identification.
        /// </summary>
        /// <param name="Text">A text representation of a brand identification.</param>
        /// <param name="BrandId">The parsed brand identification.</param>
        public static Boolean TryParse(String Text, out Brand_Id BrandId)
        {

            if (Text != null &&
                Text.Trim().IsNotNullOrEmpty())
            {

                try
                {

                    BrandId = new Brand_Id(Text);

                    return true;

                }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
                catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
                { }

            }

            BrandId = default(Brand_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this brand identification.
        /// </summary>
        public Brand_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

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
            if (ReferenceEquals(BrandId1, BrandId2))
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
            => !(BrandId1 == BrandId2);

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
                throw new ArgumentNullException(nameof(BrandId1), "The given BrandId1 must not be null!");

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
            => !(BrandId1 > BrandId2);

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
                throw new ArgumentNullException(nameof(BrandId1), "The given BrandId1 must not be null!");

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
            => !(BrandId1 < BrandId2);

        #endregion

        #endregion

        #region IComparable<BrandId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Brand_Id))
                throw new ArgumentException("The given object is not a brand identification!",
                                            nameof(Object));

            return CompareTo((Brand_Id) Object);

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
                throw new ArgumentNullException(nameof(BrandId),  "The given brand identification must not be null!");

            // Compare the length of the BrandIds
            var _Result = this.Length.CompareTo(BrandId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, BrandId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<BrandId> Members

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

            if (!(Object is Brand_Id))
                return false;

            return Equals((Brand_Id) Object);

        }

        #endregion

        #region Equals(BrandId)

        /// <summary>
        /// Compares two BrandIds for equality.
        /// </summary>
        /// <param name="BrandId">A BrandId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Brand_Id BrandId)
        {

            if ((Object) BrandId == null)
                return false;

            return InternalId.Equals(BrandId.InternalId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
