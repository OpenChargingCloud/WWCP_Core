/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.MCL
{

    /// <summary>
    /// Extension methods for national contact point identifications.
    /// </summary>
    public static class NationalContactPointIdExtensions
    {

        /// <summary>
        /// Indicates whether this national contact point identification is null or empty.
        /// </summary>
        /// <param name="NationalContactPointId">A national contact point identification.</param>
        public static Boolean IsNullOrEmpty(this NationalContactPoint_Id? NationalContactPointId)
            => !NationalContactPointId.HasValue || NationalContactPointId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this national contact point identification is NOT null or empty.
        /// </summary>
        /// <param name="NationalContactPointId">A national contact point identification.</param>
        public static Boolean IsNotNullOrEmpty(this NationalContactPoint_Id? NationalContactPointId)
            => NationalContactPointId.HasValue && NationalContactPointId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a national contact point.
    /// </summary>
    public readonly struct NationalContactPoint_Id : IId<NationalContactPoint_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this national contact point identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this national contact point identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the national contact point identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new national contact point identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a national contact point identification.</param>
        private NationalContactPoint_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random national contact point identification.
        /// </summary>
        /// <param name="Length">The expected length of the national contact point identification.</param>
        public static NationalContactPoint_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a national contact point identification.
        /// </summary>
        /// <param name="Text">A text representation of a national contact point identification.</param>
        public static NationalContactPoint_Id Parse(String Text)
        {

            if (TryParse(Text, out var nationalContactPointId))
                return nationalContactPointId;

            throw new ArgumentException($"Invalid text representation of a national contact point identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a national contact point identification.
        /// </summary>
        /// <param name="Text">A text representation of a national contact point identification.</param>
        public static NationalContactPoint_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var nationalContactPointId))
                return nationalContactPointId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out NationalContactPointId)

        /// <summary>
        /// Try to parse the given text as a national contact point identification.
        /// </summary>
        /// <param name="Text">A text representation of a national contact point identification.</param>
        /// <param name="NationalContactPointId">The parsed national contact point identification.</param>
        public static Boolean TryParse(String Text, out NationalContactPoint_Id NationalContactPointId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    NationalContactPointId = new NationalContactPoint_Id(Text);
                    return true;
                }
                catch
                { }
            }

            NationalContactPointId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this national contact point identification.
        /// </summary>
        public NationalContactPoint_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (NationalContactPointId1, NationalContactPointId2)

        /// <summary>
        /// Compares two national contact point identifications for equality.
        /// </summary>
        /// <param name="NationalContactPointId1">A national contact point identification.</param>
        /// <param name="NationalContactPointId2">Another national contact point identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (NationalContactPoint_Id NationalContactPointId1,
                                           NationalContactPoint_Id NationalContactPointId2)

            => NationalContactPointId1.Equals(NationalContactPointId2);

        #endregion

        #region Operator != (NationalContactPointId1, NationalContactPointId2)

        /// <summary>
        /// Compares two national contact point identifications for inequality.
        /// </summary>
        /// <param name="NationalContactPointId1">A national contact point identification.</param>
        /// <param name="NationalContactPointId2">Another national contact point identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (NationalContactPoint_Id NationalContactPointId1,
                                           NationalContactPoint_Id NationalContactPointId2)

            => !NationalContactPointId1.Equals(NationalContactPointId2);

        #endregion

        #region Operator <  (NationalContactPointId1, NationalContactPointId2)

        /// <summary>
        /// Compares two national contact point identifications.
        /// </summary>
        /// <param name="NationalContactPointId1">A national contact point identification.</param>
        /// <param name="NationalContactPointId2">Another national contact point identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (NationalContactPoint_Id NationalContactPointId1,
                                          NationalContactPoint_Id NationalContactPointId2)

            => NationalContactPointId1.CompareTo(NationalContactPointId2) < 0;

        #endregion

        #region Operator <= (NationalContactPointId1, NationalContactPointId2)

        /// <summary>
        /// Compares two national contact point identifications.
        /// </summary>
        /// <param name="NationalContactPointId1">A national contact point identification.</param>
        /// <param name="NationalContactPointId2">Another national contact point identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (NationalContactPoint_Id NationalContactPointId1,
                                           NationalContactPoint_Id NationalContactPointId2)

            => NationalContactPointId1.CompareTo(NationalContactPointId2) <= 0;

        #endregion

        #region Operator >  (NationalContactPointId1, NationalContactPointId2)

        /// <summary>
        /// Compares two national contact point identifications.
        /// </summary>
        /// <param name="NationalContactPointId1">A national contact point identification.</param>
        /// <param name="NationalContactPointId2">Another national contact point identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (NationalContactPoint_Id NationalContactPointId1,
                                          NationalContactPoint_Id NationalContactPointId2)

            => NationalContactPointId1.CompareTo(NationalContactPointId2) > 0;

        #endregion

        #region Operator >= (NationalContactPointId1, NationalContactPointId2)

        /// <summary>
        /// Compares two national contact point identifications.
        /// </summary>
        /// <param name="NationalContactPointId1">A national contact point identification.</param>
        /// <param name="NationalContactPointId2">Another national contact point identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (NationalContactPoint_Id NationalContactPointId1,
                                           NationalContactPoint_Id NationalContactPointId2)

            => NationalContactPointId1.CompareTo(NationalContactPointId2) >= 0;

        #endregion

        #endregion

        #region IComparable<NationalContactPointId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two national contact point identifications.
        /// </summary>
        /// <param name="Object">A national contact point identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is NationalContactPoint_Id nationalContactPointId
                   ? CompareTo(nationalContactPointId)
                   : throw new ArgumentException("The given object is not a national contact point identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(NationalContactPointId)

        /// <summary>
        /// Compares two national contact point identifications.
        /// </summary>
        /// <param name="NationalContactPointId">A national contact point identification to compare with.</param>
        public Int32 CompareTo(NationalContactPoint_Id NationalContactPointId)

            => String.Compare(InternalId,
                              NationalContactPointId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<NationalContactPointId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two national contact point identifications for equality.
        /// </summary>
        /// <param name="Object">A national contact point identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is NationalContactPoint_Id nationalContactPointId &&
                   Equals(nationalContactPointId);

        #endregion

        #region Equals(NationalContactPointId)

        /// <summary>
        /// Compares two national contact point identifications for equality.
        /// </summary>
        /// <param name="NationalContactPointId">A national contact point identification to compare with.</param>
        public Boolean Equals(NationalContactPoint_Id NationalContactPointId)

            => String.Equals(InternalId,
                             NationalContactPointId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

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
