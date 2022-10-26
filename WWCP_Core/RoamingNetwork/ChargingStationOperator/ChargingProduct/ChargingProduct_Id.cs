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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging product identifications.
    /// </summary>
    /// <param name="ChargingProductId">A charging product identification to include.</param>
    public delegate Boolean IncludeChargingProductIdDelegate(ChargingProduct_Id ChargingProductId);


    /// <summary>
    /// Extension methods for charging product identifications.
    /// </summary>
    public static class ChargingProductIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging product identification is null or empty.
        /// </summary>
        /// <param name="ChargingProductId">A charging product identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingProduct_Id? ChargingProductId)
            => !ChargingProductId.HasValue || ChargingProductId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging product identification is null or empty.
        /// </summary>
        /// <param name="ChargingProductId">A charging product identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingProduct_Id? ChargingProductId)
            => ChargingProductId.HasValue && ChargingProductId.Value.IsNotNullOrEmpty;

    }

    /// <summary>
    /// The unique identification of a charging product.
    /// </summary>
    public readonly struct ChargingProduct_Id : IId,
                                                IEquatable<ChargingProduct_Id>,
                                                IComparable<ChargingProduct_Id>

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
        /// The length of the EVSE admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging product identification based on the given string.
        /// </summary>
        private ChargingProduct_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging product identification.
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated charging product identification.</param>
        public static ChargingProduct_Id NewRandom(Func<String, String>? Mapper   = null)

            => new (Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(50))
                        :        RandomExtensions.RandomString(50));

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging product identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging product identification.</param>
        public static ChargingProduct_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargingProduct_Id chargingProductId))
                return chargingProductId;

            throw new ArgumentException("Invalid text-representation of a charging product identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Parse the given string as a charging product identification.
        /// </summary>
        public static ChargingProduct_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingProduct_Id chargingProductId))
                return chargingProductId;

            return null;

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

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingProductId = new ChargingProduct_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingProductId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging product identification.
        /// </summary>
        public ChargingProduct_Id Clone

            => new (new String(InternalId?.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingProduct_Id ChargingProductId1,
                                           ChargingProduct_Id ChargingProductId2)

            => ChargingProductId1.Equals(ChargingProductId2);

        #endregion

        #region Operator != (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingProduct_Id ChargingProductId1,
                                           ChargingProduct_Id ChargingProductId2)

            => !ChargingProductId1.Equals(ChargingProductId2);

        #endregion

        #region Operator <  (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingProduct_Id ChargingProductId1,
                                          ChargingProduct_Id ChargingProductId2)

            => ChargingProductId1.CompareTo(ChargingProductId2) < 0;

        #endregion

        #region Operator <= (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingProduct_Id ChargingProductId1,
                                           ChargingProduct_Id ChargingProductId2)

            => ChargingProductId1.CompareTo(ChargingProductId2) <= 0;

        #endregion

        #region Operator >  (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingProduct_Id ChargingProductId1,
                                          ChargingProduct_Id ChargingProductId2)

            => ChargingProductId1.CompareTo(ChargingProductId2) > 0;

        #endregion

        #region Operator >= (ChargingProductId1, ChargingProductId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProductId1">A charging product identification.</param>
        /// <param name="ChargingProductId2">Another charging product identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingProduct_Id ChargingProductId1,
                                           ChargingProduct_Id ChargingProductId2)

            => ChargingProductId1.CompareTo(ChargingProductId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingProductId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging product identifications.
        /// </summary>
        /// <param name="Object">A charging product identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingProduct_Id chargingProductId
                   ? CompareTo(chargingProductId)
                   : throw new ArgumentException("The given object is not a charging product identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingProductId)

        /// <summary>
        /// Compares two charging product identifications.
        /// </summary>
        /// <param name="ChargingProductId">A charging product identification to compare with.</param>
        public Int32 CompareTo(ChargingProduct_Id ChargingProductId)

            => String.Compare(InternalId,
                              ChargingProductId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingProductId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging product identifications for equality.
        /// </summary>
        /// <param name="Object">A charging product identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingProduct_Id chargingProductId &&
                   Equals(chargingProductId);

        #endregion

        #region Equals(ChargingProductId)

        /// <summary>
        /// Compares two charging product identifications for equality.
        /// </summary>
        /// <param name="ChargingProductId">A charging product identification to compare with.</param>
        public Boolean Equals(ChargingProduct_Id ChargingProductId)

            => String.Equals(InternalId,
                             ChargingProductId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
