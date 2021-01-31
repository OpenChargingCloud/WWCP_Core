/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of a charging product.
    /// </summary>
    public struct ChargingProduct_Id : IId,
                                       IEquatable <ChargingProduct_Id>,
                                       IComparable<ChargingProduct_Id>
    {

        #region Data

        private readonly static Random _Random = new Random(DateTime.Now.Millisecond);

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
        /// Create a new charging product identification.
        /// based on the given string.
        /// </summary>
        private ChargingProduct_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        /// <summary>
        /// Returns a new charging product identification.
        /// </summary>
        public static ChargingProduct_Id Random(Byte Length = 20)
            => new ChargingProduct_Id(_Random.RandomString(Length));

        #endregion


        #region (static) New

        public static ChargingProduct_Id New

            => ChargingProduct_Id.Parse(Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging product identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging product identification.</param>
        public static ChargingProduct_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging product identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out ChargingProduct_Id ChargingProductId))
                return ChargingProductId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a charging product identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging product identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging product identification.</param>
        public static ChargingProduct_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingProduct_Id ChargingProductId))
                return ChargingProductId;

            return new ChargingProduct_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out ChargingProductId)

        /// <summary>
        /// Parse the given string as a charging product identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging product identification.</param>
        /// <param name="ChargingProductId">The parsed charging product identification.</param>
        public static Boolean TryParse(String Text, out ChargingProduct_Id ChargingProductId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                ChargingProductId = default;
                return false;
            }

            #endregion

             try
             {
                 ChargingProductId = new ChargingProduct_Id(Text);
                 return true;
             }
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
             catch (Exception)
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
             { }

            ChargingProductId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging product identification.
        /// </summary>
        public ChargingProduct_Id Clone

            => new ChargingProduct_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingProductId1, ChargingProductId2))
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
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
            => !(ChargingProductId1 == ChargingProductId2);

        #endregion

        #region Operator <  (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {

            if ((Object) ChargingProductId1 == null)
                throw new ArgumentNullException(nameof(ChargingProductId1), "The given ChargingProductId1 must not be null!");

            return ChargingProductId1.CompareTo(ChargingProductId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
            => !(ChargingProductId1 > ChargingProductId2);

        #endregion

        #region Operator >  (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
        {

            if ((Object) ChargingProductId1 == null)
                throw new ArgumentNullException(nameof(ChargingProductId1), "The given ChargingProductId1 must not be null!");

            return ChargingProductId1.CompareTo(ChargingProductId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingProduct_Id ChargingProductId1, ChargingProduct_Id ChargingProductId2)
            => !(ChargingProductId1 < ChargingProductId2);

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

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingProduct_Id ChargingProductId))
                throw new ArgumentException("The given object is not a charging product identification!",
                                            nameof(Object));

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
                throw new ArgumentNullException(nameof(ChargingProductId),  "The given charging product identification must not be null!");

            return String.Compare(InternalId, ChargingProductId.InternalId, StringComparison.OrdinalIgnoreCase);

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

            if (!(Object is ChargingProduct_Id ChargingProductId))
                return false;

            return Equals(ChargingProductId);

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

            return InternalId.ToLower().Equals(ChargingProductId.InternalId.ToLower());

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
