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

using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging pool identifications.
    /// </summary>
    /// <param name="ChargingPoolId">A charging pool identification to include.</param>
    public delegate Boolean IncludeChargingPoolIdDelegate(ChargingPool_Id ChargingPoolId);


    /// <summary>
    /// Extension methods for charging pool identifications.
    /// </summary>
    public static class ChargingPoolIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging pool identification is null or empty.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingPool_Id? ChargingPoolId)
            => !ChargingPoolId.HasValue || ChargingPoolId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging pool identification is null or empty.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingPool_Id? ChargingPoolId)
            => ChargingPoolId.HasValue && ChargingPoolId.Value.IsNotNullOrEmpty;


        /// <summary>
        /// Create a new charging station identification based
        /// on the given charging pool identification.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification.</param>
        /// <param name="AdditionalSuffix">An additional EVSE suffix.</param>
        public static ChargingStation_Id CreateStationId(this ChargingPool_Id  ChargingPoolId,
                                                         String?               AdditionalSuffix   = null)

               // (P)OOL => (S)TATION
            => ChargingPoolId.Suffix.StartsWith("OOL", StringComparison.Ordinal)
                   ? ChargingStation_Id.Parse(ChargingPool_Id.Parse(ChargingPoolId.OperatorId, "TATION" + ChargingPoolId.Suffix[3..]), AdditionalSuffix ?? "")
                   : ChargingStation_Id.Parse(ChargingPoolId, AdditionalSuffix ?? "");

    }

    /// <summary>
    /// The unique identification of an electric vehicle charging pool (EVCP).
    /// </summary>
    public readonly struct ChargingPool_Id : IId,
                                             IEquatable<ChargingPool_Id>,
                                             IComparable<ChargingPool_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging pool identification.
        /// </summary>
        public static readonly Regex ChargingPoolId_RegEx  = new (@"^([a-zA-Z]{2}\*?[a-zA-Z0-9]{3})\*?P([a-zA-Z0-9][a-zA-Z0-9\*]{0,50})$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId    { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                      Suffix        { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (OperatorId.ToString(OperatorIdFormats.ISO_STAR).Length + 2 + Suffix.Length); // +2 because of "*P"

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging pool identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        private ChargingPool_Id(ChargingStationOperator_Id  OperatorId,
                                String                      Suffix)
        {

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging pool identification suffix must not be null or empty!");

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region (static) Generate (ChargingStationOperatorId, Address, GeoLocation, Length = 50, Mapper = null)

        /// <summary>
        /// Generate a new charging pool identification based on the given parameters.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The identification of the charging station operator.</param>
        /// <param name="Address">The address of the charging pool.</param>
        /// <param name="GeoLocation">The geo location of the charging pool.</param>
        /// <param name="Length">The maximum size of the generated charging pool identification suffix [12 &lt; n &lt; 50].</param>
        /// <param name="Mapper">A delegate to modify a generated charging pool identification suffix.</param>
        public static ChargingPool_Id Generate(ChargingStationOperator_Id  ChargingStationOperatorId,
                                               Address                     Address,
                                               GeoCoordinate?              GeoLocation       = default,
                                               String?                     PoolName          = default,
                                               String?                     PoolDescription   = default,
                                               Byte                        Length            = 15,
                                               Func<String, String>?       Mapper            = null)
        {

            if (Length < 12)
                Length = 12;

            if (Length > 50)
                Length = 50;

            var Suffix = SHA256.HashData(Encoding.UTF8.GetBytes(
                                             String.Concat(
                                                 ChargingStationOperatorId.ToString(),
                                                 Address.                  ToString(),
                                                 GeoLocation?.             ToString() ?? "",
                                                 PoolName                             ?? "",
                                                 PoolDescription                      ?? ""
                                             )
                                         )).
                                         ToHexString().
                                         SubstringMax(Length).
                                         ToUpper();

            return Parse(ChargingStationOperatorId,
                         Mapper is not null
                            ? Mapper(Suffix)
                            : Suffix);

        }

        #endregion

        #region (static) NewRandom(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging pool identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging pool identification.</param>
        public static ChargingPool_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                Func<String, String>?       Mapper  = null)

            => new (OperatorId,
                    Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(50))
                        :        RandomExtensions.RandomString(50));

        #endregion


        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargingPool_Id chargingPoolId))
                return chargingPoolId;

            throw new ArgumentException("Invalid text-representation of a charging pool identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse    (ChargingStationOperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging pool operator.</param>
        /// <param name="Suffix">The suffix of the charging pool identification.</param>
        public static ChargingPool_Id Parse(ChargingStationOperator_Id  ChargingStationOperatorId,
                                            String                      Suffix)

            => ChargingStationOperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => Parse(String.Concat(ChargingStationOperatorId.ToString(),                           "*P", Suffix)),
                   OperatorIdFormats.ISO       => Parse(String.Concat(ChargingStationOperatorId.ToString(),                            "P", Suffix)),
                   _                           => Parse(String.Concat(ChargingStationOperatorId.ToString(OperatorIdFormats.ISO_STAR), "*P", Suffix))
               };

        #endregion


        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        public static ChargingPool_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingPool_Id chargingPoolId))
                return chargingPoolId;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargingStationOperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging pool identification.</param>
        public static ChargingPool_Id? TryParse(ChargingStationOperator_Id  ChargingStationOperatorId,
                                                String                      Suffix)

            => ChargingStationOperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                           "*P", Suffix)),
                   OperatorIdFormats.ISO       => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                            "P", Suffix)),
                   _                           => TryParse(String.Concat(ChargingStationOperatorId.ToString(OperatorIdFormats.ISO_STAR), "*P", Suffix))
               };

        #endregion


        #region (static) TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            #region Initial checks

            ChargingPoolId = default;

            if (Text is null)
                return false;

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = ChargingPoolId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;

                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out ChargingStationOperator_Id chargingStationOperatorId))
                {

                    ChargingPoolId = new ChargingPool_Id(chargingStationOperatorId,
                                                         matchCollection[0].Groups[2].Value);

                    return true;

                }

            }
            catch (Exception)
            { }

            return false;

        }

        #endregion

        #region (static) TryParse(ChargingPoolOperatorId, Suffix, out ChargingPoolId)

        /// <summary>
        /// Parse the given charging station operator identification and suffix as a charging pool identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging pool identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  ChargingStationOperatorId,
                                       String                      Suffix,
                                       out ChargingPool_Id         ChargingPoolId)

            => ChargingStationOperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                           "*P", Suffix), out ChargingPoolId),
                   OperatorIdFormats.ISO       => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                            "P", Suffix), out ChargingPoolId),
                   _                           => TryParse(String.Concat(ChargingStationOperatorId.ToString(OperatorIdFormats.ISO_STAR), "*P", Suffix), out ChargingPoolId)
               };

        #endregion


        #region Clone

        /// <summary>
        /// Clone this charging pool identification.
        /// </summary>
        public ChargingPool_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix?.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.Equals(ChargingPoolId2);

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => !ChargingPoolId1.Equals(ChargingPoolId2);

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1,
                                          ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1,
                                          ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A charging pool identification.</param>
        /// <param name="ChargingPoolId2">Another charging pool identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1,
                                           ChargingPool_Id ChargingPoolId2)

            => ChargingPoolId1.CompareTo(ChargingPoolId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool identifications.
        /// </summary>
        /// <param name="Object">A charging pool identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPool_Id chargingPoolId
                   ? CompareTo(chargingPoolId)
                   : throw new ArgumentException("The given object is not a charging pool identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)
        {

            var c = OperatorId.CompareTo(ChargingPoolId.OperatorId);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargingPoolId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPoolId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="Object">A charging pool identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPool_Id chargingPoolId &&
                   Equals(chargingPoolId);

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two charging pool identifications for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A charging pool identification to compare with.</param>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)

            => OperatorId.Equals(ChargingPoolId.OperatorId) &&
               String.Equals(Suffix.               Replace("*", ""),
                             ChargingPoolId.Suffix.Replace("*", ""),
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.               GetHashCode() ^
               Suffix?.Replace("*", "")?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => OperatorId.Format switch {

                OperatorIdFormats.ISO       => String.Concat(OperatorId.CountryCode.Alpha2Code,
                                                             OperatorId.Suffix,
                                                             "P",
                                                             Suffix),

                OperatorIdFormats.ISO_STAR  => String.Concat(OperatorId.CountryCode.Alpha2Code,
                                                             "*",
                                                             OperatorId.Suffix,
                                                             "*P",
                                                             Suffix),

                _                           => String.Concat("+",
                                                             OperatorId.CountryCode.TelefonCode,
                                                             "*",
                                                             OperatorId.Suffix,
                                                             "*P",
                                                             Suffix)

            };

        #endregion

    }

}
