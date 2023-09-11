/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of a parking product.
    /// </summary>
    public readonly struct ParkingProduct_Id : IId,
                                               IEquatable <ParkingProduct_Id>,
                                               IComparable<ParkingProduct_Id>

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
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new parking product identification.
        /// based on the given string.
        /// </summary>
        private ParkingProduct_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region New

        /// <summary>
        /// Returns a new parking product identification.
        /// </summary>
        public static ParkingProduct_Id New

            => ParkingProduct_Id.Parse(Guid.NewGuid().ToString());

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a parking product identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking product identification.</param>
        public static ParkingProduct_Id Parse(String Text)

            => new ParkingProduct_Id(Text);

        #endregion

        #region TryParse(Text, out ParkingProductId)

        /// <summary>
        /// Parse the given string as a parking product identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking product identification.</param>
        /// <param name="ParkingProductId">The parsed parking product identification.</param>
        public static Boolean TryParse(String Text, out ParkingProduct_Id ParkingProductId)
        {
            try
            {

                ParkingProductId = new ParkingProduct_Id(Text);

                return true;

            }
            catch
            {
                ParkingProductId = default(ParkingProduct_Id);
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this parking product identification.
        /// </summary>
        public ParkingProduct_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ParkingProductId1, ParkingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId1">A parking product identification.</param>
        /// <param name="ParkingProductId2">Another parking product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ParkingProduct_Id ParkingProductId1, ParkingProduct_Id ParkingProductId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ParkingProductId1, ParkingProductId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ParkingProductId1 == null) || ((Object) ParkingProductId2 == null))
                return false;

            return ParkingProductId1.Equals(ParkingProductId2);

        }

        #endregion

        #region Operator != (ParkingProductId1, ParkingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId1">A parking product identification.</param>
        /// <param name="ParkingProductId2">Another parking product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ParkingProduct_Id ParkingProductId1, ParkingProduct_Id ParkingProductId2)
            => !(ParkingProductId1 == ParkingProductId2);

        #endregion

        #region Operator <  (ParkingProductId1, ParkingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId1">A parking product identification.</param>
        /// <param name="ParkingProductId2">Another parking product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ParkingProduct_Id ParkingProductId1, ParkingProduct_Id ParkingProductId2)
        {

            if ((Object) ParkingProductId1 == null)
                throw new ArgumentNullException(nameof(ParkingProductId1), "The given ParkingProductId1 must not be null!");

            return ParkingProductId1.CompareTo(ParkingProductId2) < 0;

        }

        #endregion

        #region Operator <= (ParkingProductId1, ParkingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId1">A parking product identification.</param>
        /// <param name="ParkingProductId2">Another parking product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ParkingProduct_Id ParkingProductId1, ParkingProduct_Id ParkingProductId2)
            => !(ParkingProductId1 > ParkingProductId2);

        #endregion

        #region Operator >  (ParkingProductId1, ParkingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId1">A parking product identification.</param>
        /// <param name="ParkingProductId2">Another parking product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ParkingProduct_Id ParkingProductId1, ParkingProduct_Id ParkingProductId2)
        {

            if ((Object) ParkingProductId1 == null)
                throw new ArgumentNullException(nameof(ParkingProductId1), "The given ParkingProductId1 must not be null!");

            return ParkingProductId1.CompareTo(ParkingProductId2) > 0;

        }

        #endregion

        #region Operator >= (ParkingProductId1, ParkingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId1">A parking product identification.</param>
        /// <param name="ParkingProductId2">Another parking product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ParkingProduct_Id ParkingProductId1, ParkingProduct_Id ParkingProductId2)
            => !(ParkingProductId1 < ParkingProductId2);

        #endregion

        #endregion

        #region IComparable<ParkingProductId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ParkingProduct_Id))
                throw new ArgumentException("The given object is not a parking product identification!",
                                            nameof(Object));

            return CompareTo((ParkingProduct_Id) Object);

        }

        #endregion

        #region CompareTo(ParkingProductId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProductId">An object to compare with.</param>
        public Int32 CompareTo(ParkingProduct_Id ParkingProductId)
        {

            if ((Object) ParkingProductId == null)
                throw new ArgumentNullException(nameof(ParkingProductId),  "The given parking product identification must not be null!");

            // Compare the length of the ParkingProductIds
            var _Result = this.Length.CompareTo(ParkingProductId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, ParkingProductId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ParkingProductId> Members

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

            if (!(Object is ParkingProduct_Id))
                return false;

            return Equals((ParkingProduct_Id) Object);

        }

        #endregion

        #region Equals(ParkingProductId)

        /// <summary>
        /// Compares two ParkingProductIds for equality.
        /// </summary>
        /// <param name="ParkingProductId">A ParkingProductId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingProduct_Id ParkingProductId)
        {

            if ((Object) ParkingProductId == null)
                return false;

            return InternalId.Equals(ParkingProductId.InternalId);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

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
