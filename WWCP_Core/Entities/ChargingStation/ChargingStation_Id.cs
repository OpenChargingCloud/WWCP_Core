/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging station identifications.
    /// </summary>
    /// <param name="ChargingStationId">A charging station identification to include.</param>
    public delegate Boolean IncludeChargingStationIdDelegate(ChargingStation_Id ChargingStationId);


    /// <summary>
    /// Extension methods for charging station identifications.
    /// </summary>
    public static class ChargingStationIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging station identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingStation_Id? ChargingStationId)
            => !ChargingStationId.HasValue || ChargingStationId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station identification is null or empty.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStation_Id? ChargingStationId)
            => ChargingStationId.HasValue && ChargingStationId.Value.IsNotNullOrEmpty;


        /// <summary>
        /// Create a new EVSE identification
        /// based on the given charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="AdditionalSuffix">An optional additional EVSE suffix.</param>
        public static EVSE_Id CreateEVSEId(this ChargingStation_Id  ChargingStationId,
                                           String?                  AdditionalSuffix   = null)

               // (S)TATION => (E)VSE
            => ChargingStationId.Suffix.StartsWith("TATION", StringComparison.Ordinal)
                   ? EVSE_Id.Parse(ChargingStation_Id.Parse(ChargingStationId.OperatorId, "VSE" + ChargingStationId.Suffix[6..]), AdditionalSuffix ?? "")
                   : EVSE_Id.Parse(ChargingStationId, AdditionalSuffix ?? "");

    }


    /// <summary>
    /// The unique identification of an electric vehicle charging station (EVCS).
    /// </summary>
    public readonly struct ChargingStation_Id : IId,
                                                IEquatable<ChargingStation_Id>,
                                                IComparable<ChargingStation_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// </summary>
        public  static readonly Regex  ChargingStationId_RegEx  = new (@"^([a-zA-Z]{2}\*?[a-zA-Z0-9]{3})\*?S([a-zA-Z0-9][a-zA-Z0-9\*]{0,50})$",
                                                                       RegexOptions.IgnorePatternWhitespace);

        private static readonly Char[] StarSplitter             = new Char[] { '*' };

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                      Suffix       { get; }


        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => !Suffix.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length

            => OperatorId.Format switch {
                   OperatorIdFormats.DIN       => OperatorId.Length + 1 + (UInt64) Suffix.Length,
                   OperatorIdFormats.ISO_STAR  => OperatorId.Length + 2 + (UInt64) Suffix.Length,
                   _                           => OperatorId.Length + 1 + (UInt64) Suffix.Length,  // ISO
               };

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station identification based on the given
        /// charging station operator and charging tariff identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        private ChargingStation_Id(ChargingStationOperator_Id  OperatorId,
                                   String                      Suffix)
        {
            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;
        }

        #endregion


        #region (static) NewRandom(OperatorId,     Length = 20, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                   Byte                        Length   = 20,
                                                   Func<String, String>?       Mapper   = null)

        {

            if (Length < 5 || Length > 50)
                Length = 50;

            return Parse(OperatorId,
                          Mapper is not null
                              ? Mapper(RandomExtensions.RandomString(Length))
                              :        RandomExtensions.RandomString(Length));

        }

        #endregion

        #region (static) NewRandom(ChargingPoolId, Length = 20, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id NewRandom(ChargingPool_Id        ChargingPoolId,
                                                   Byte                   Length   = 20,
                                                   Func<String, String>?  Mapper   = null)

        {

            if (Length < 5 || Length > 40)
                Length = 40;

            return Parse(ChargingPoolId,
                          Mapper is not null
                              ? Mapper(RandomExtensions.RandomString(Length))
                              :        RandomExtensions.RandomString(Length));

        }

        #endregion


        #region (static) TryCreate            (EVSEIds,                                           Mapper = null, HashHelper = "")

        /// <summary>
        /// Create an unique charging station identification based on the given enumeration of EVSE identifications.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        /// <param name="Mapper">An optional charging station identification suffix mapper.</param>
        /// <param name="HashHelper">An optional hashing helper.</param>
        public static ChargingStation_Id? TryCreate(IEnumerable<EVSE_Id>   EVSEIds,
                                                    Func<String, String>?  Mapper       = null,
                                                    String?                HashHelper   = null)

            => TryCreate(EVSEIds, out var chargingStationId, out _, Mapper, HashHelper)
                   ? chargingStationId
                   : null;

        #endregion

        #region (static) TryCreate            (EVSEIds, out ChargingStationId, out ErrorResponse, Mapper = null, HashHelper = "")

        /// <summary>
        /// Create an unique charging station identification based on the given enumeration of EVSE identifications.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Mapper">An optional charging station identification suffix mapper.</param>
        /// <param name="HashHelper">An optional hashing helper.</param>
        public static Boolean TryCreate(IEnumerable<EVSE_Id>     EVSEIds,
                                        out ChargingStation_Id?  ChargingStationId,
                                        out String?              ErrorResponse,
                                        Func<String, String>?    Mapper       = null,
                                        String?                  HashHelper   = null)
        {

            ChargingStationId  = null;
            ErrorResponse      = null;

            #region Initial checks

            var evseIds = EVSEIds.Distinct().ToArray();

            if (evseIds.Length == 0)
            {
                ErrorResponse = "The given enumeration of EVSE identifications must not be empty!";
                return false;
            }

            if (evseIds.Length == 1)
            {
                ChargingStationId = Parse(evseIds[0]);
                return true;
            }

            if (evseIds.Select(evseId => evseId.OperatorId.ToString()).Distinct().Count() > 1)
            {
                ErrorResponse = "The given enumeration of EVSE identifications must be of the same operator!";
                return false;
            }

            #endregion


            #region EVSEIdSuffix contains '*'...

            if (evseIds.Any(evseId => evseId.Suffix.Contains('*') ||
                                      evseId.Suffix.Contains('-')))
            {

                var evseIdPrefixStrings = evseIds.
                                              Select(EVSEId => EVSEId.Suffix.Split(StarSplitter, StringSplitOptions.RemoveEmptyEntries)).
                                              Select(SuffixElements => SuffixElements.Take(SuffixElements.Length - 1).AggregateWith("*", "")).
                                              Where(NewSuffix => NewSuffix != "").
                                              Distinct().
                                              ToArray();

                if (evseIdPrefixStrings.Length != 1)
                {
                    ErrorResponse = "The given enumeration of EVSE identifications must have the same prefix!";
                    return false;
                }

                ChargingStationId = Parse(evseIds[0].OperatorId,
                                           evseIdPrefixStrings[0]);
                return true;

            }

            #endregion

            #region ...or use longest prefix...

            else
            {

                var evseIdArray  = evseIds.Select(evseId => evseId.ToString()).ToArray();
                var minLength    = (Int32) evseIdArray.Select(evseId => evseId.Length).Min();

                var prefix       = "";

                for (var i = 0; i < minLength; i++)
                {
                    if (evseIdArray.All(v => v[i] == evseIdArray[0][i]))
                        prefix += evseIdArray[0][i];
                }

                if (((UInt64) prefix.Length) > evseIds[0].OperatorId.Length + 1)
                {

                    var tmpEVSEId = EVSE_Id.Parse(prefix);

                    if (tmpEVSEId.OperatorId.Format == OperatorIdFormats.ISO)
                    {
                        if (((UInt64) prefix.Length) > evseIds[0].OperatorId.Length + 2)
                            prefix = tmpEVSEId.Suffix; //TmpEVSEId.OperatorId + "*S" + TmpEVSEId.Suffix;
                        else
                        {
                            ErrorResponse = "The given enumeration of EVSE identifications must have the same prefix!";
                            return false;
                        }
                    }

                    ChargingStationId = Parse(evseIds[0].OperatorId, prefix);
                    return true;

                }

            }

            #endregion

            #region ...or generate a hash of the EVSE identifications!

            return TryGenerateViaHashing(EVSEIds,
                                      out ChargingStationId,
                                      out ErrorResponse,
                                      Mapper,
                                      HashHelper);

            #endregion

        }

        #endregion

        #region (static) TryGenerateViaHashing(EVSEIds,                                           Mapper = null, HashHelper = "")

        /// <summary>
        /// Generate an unique charging station identification based on a hash
        /// of the given enumeration of EVSE identifications.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        /// <param name="Mapper">An optional charging station identification suffix mapper.</param>
        /// <param name="HashHelper">An optional hashing helper.</param>
        public static ChargingStation_Id? TryGenerateViaHashing(IEnumerable<EVSE_Id>   EVSEIds,
                                                                Func<String, String>?  Mapper       = null,
                                                                String?                HashHelper   = null)

            => TryGenerateViaHashing(EVSEIds, out var chargingStationId, out _, Mapper, HashHelper)
                   ? chargingStationId
                   : null;

        #endregion

        #region (static) TryGenerateViaHashing(EVSEIds, out ChargingStationId, out ErrorResponse, Mapper = null, HashHelper = "")

        /// <summary>
        /// Generate an unique charging station identification based on a hash
        /// of the given enumeration of EVSE identifications.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Mapper">An optional charging station identification suffix mapper.</param>
        /// <param name="HashHelper">An optional hashing helper.</param>
        public static Boolean TryGenerateViaHashing(IEnumerable<EVSE_Id>     EVSEIds,
                                                    out ChargingStation_Id?  ChargingStationId,
                                                    out String?              ErrorResponse,
                                                    Func<String, String>?    Mapper       = null,
                                                    String?                  HashHelper   = null)
        {

            ChargingStationId  = null;
            ErrorResponse      = null;

            #region Initial checks

            var evseIds = EVSEIds.Distinct().ToArray();

            if (evseIds.Length == 0)
            {
                ErrorResponse = "The given enumeration of EVSE identifications must not be empty!";
                return false;
            }

            if (evseIds.Length == 1)
            {
                ChargingStationId = Parse(evseIds[0]);
                return true;
            }

            #endregion

            var suffix = SHA1.HashData(Encoding.UTF8.GetBytes(EVSEIds.Select(evseid => evseid.ToString()).AggregateWith(HashHelper ?? "|"))).
                              ToHexString().
                              ToUpper();

            ChargingStationId = Parse(evseIds[0].OperatorId,
                                       Mapper is not null
                                          ? Mapper(suffix)
                                          : suffix);

            return true;

        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingStationId))
                return chargingStationId;

            throw new ArgumentException($"Invalid text representation of a charging station identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse   (ChargingStationOperatorId, Suffix)

        /// <summary>
        /// Create a charging station identification based on the given charging station operator identification and suffix.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static ChargingStation_Id Parse(ChargingStationOperator_Id  ChargingStationOperatorId,
                                               String                      Suffix)

            => ChargingStationOperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => Parse(String.Concat(ChargingStationOperatorId.ToString(),                           "*S", Suffix)),
                   OperatorIdFormats.ISO       => Parse(String.Concat(ChargingStationOperatorId.ToString(),                            "S", Suffix)),
                   _                           => Parse(String.Concat(ChargingStationOperatorId.ToString(OperatorIdFormats.ISO_STAR), "*S", Suffix))
               };

        #endregion

        #region (static) Parse   (ChargingPoolId,            Suffix)

        /// <summary>
        /// Create a charging station identification based on the given charging pool identification and suffix.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static ChargingStation_Id Parse(ChargingPool_Id  ChargingPoolId,
                                               String           Suffix)

            => ChargingPoolId.OperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => Parse(String.Concat(ChargingPoolId.OperatorId.ToString(),                           "*S", ChargingPoolId.Suffix, Suffix.IsNeitherNullNorEmpty() ? "*" + Suffix : "")),
                   OperatorIdFormats.ISO       => Parse(String.Concat(ChargingPoolId.OperatorId.ToString(),                            "S", ChargingPoolId.Suffix, Suffix)),
                   _                           => Parse(String.Concat(ChargingPoolId.OperatorId.ToString(OperatorIdFormats.ISO_STAR), "*S", ChargingPoolId.Suffix, Suffix.IsNeitherNullNorEmpty() ? "*" + Suffix : ""))
               };

        #endregion

        #region (static) Parse   (EVSEId,                                                   RemoveLastStar = true)

        /// <summary>
        /// Create a charging station identification based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="RemoveLastStar">Generate a charging station identification by removing the last star part.</param>
        public static ChargingStation_Id Parse(EVSE_Id  EVSEId,
                                               Boolean  RemoveLastStar = true)
        {

            var suffix = EVSEId.Suffix;

            if (RemoveLastStar)
            {

                var starPosition = EVSEId.Suffix.LastIndexOf("*");

                if (starPosition > 0)
                    suffix = suffix[..starPosition];

            }

            return Parse(EVSEId.OperatorId,
                          suffix.ToUpper());

        }

        #endregion


        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        public static ChargingStation_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationId))
                return chargingStationId;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargingStationOperatorId, Suffix)

        /// <summary>
        /// Parse the given charging station operator identification and suffix as a charging station identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static ChargingStation_Id? TryParse(ChargingStationOperator_Id  ChargingStationOperatorId,
                                                   String                      Suffix)

            => ChargingStationOperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                           "*S", Suffix)),
                   OperatorIdFormats.ISO       => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                            "S", Suffix)),
                   _                           => TryParse(String.Concat(ChargingStationOperatorId.ToString(OperatorIdFormats.ISO_STAR), "*S", Suffix))
               };

        #endregion

        #region (static) TryParse(ChargingPoolId,            Suffix)

        /// <summary>
        /// Parse the given charging pool identification and suffix as a charging station identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static ChargingStation_Id? TryParse(ChargingPool_Id  ChargingPoolId,
                                                   String           Suffix)

            => ChargingPoolId.OperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(ChargingPoolId.OperatorId.ToString(),                           "*S", ChargingPoolId.Suffix, Suffix.IsNeitherNullNorEmpty() ? "*" + Suffix : "")),
                   OperatorIdFormats.ISO       => TryParse(String.Concat(ChargingPoolId.OperatorId.ToString(),                            "S", ChargingPoolId.Suffix, Suffix)),
                   _                           => TryParse(String.Concat(ChargingPoolId.OperatorId.ToString(OperatorIdFormats.ISO_STAR), "*S", ChargingPoolId.Suffix, Suffix.IsNeitherNullNorEmpty() ? "*" + Suffix : ""))
               };

        #endregion

        #region (static) Parse   (EVSEId,                                                   RemoveLastStar = true)

        /// <summary>
        /// Create a charging station identification based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="RemoveLastStar">Generate a charging station identification by removing the last star part.</param>
        public static ChargingStation_Id? TryParse(EVSE_Id  EVSEId,
                                                   Boolean  RemoveLastStar = true)

            => TryParse(EVSEId, out var chargingStationId, RemoveLastStar)
                   ? chargingStationId
                   : null;

        #endregion


        #region (static) TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {

            #region Initial checks

            ChargingStationId = default;

            if (Text is null)
                return false;

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = ChargingStationId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;

                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out var chargingStationOperatorId))
                {

                    ChargingStationId = new ChargingStation_Id(chargingStationOperatorId,
                                                               matchCollection[0].Groups[2].Value);

                    return true;

                }

            }
            catch
            { }

            return false;

        }

        #endregion

        #region (static) TryParse(ChargingStationOperatorId, Suffix, out ChargingStationId)

        /// <summary>
        /// Parse the given charging station operator identification and suffix as a charging station identification.
        /// </summary>
        /// <param name="ChargingStationOperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  ChargingStationOperatorId,
                                       String                      Suffix,
                                       out ChargingStation_Id      ChargingStationId)

            => ChargingStationOperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                           "*S", Suffix), out ChargingStationId),
                   OperatorIdFormats.ISO       => TryParse(String.Concat(ChargingStationOperatorId.ToString(),                            "S", Suffix), out ChargingStationId),
                   _                           => TryParse(String.Concat(ChargingStationOperatorId.ToString(OperatorIdFormats.ISO_STAR), "*S", Suffix), out ChargingStationId)
               };

        #endregion

        #region (static) TryParse(ChargingPoolId,            Suffix, out ChargingStationId)

        /// <summary>
        /// Parse the given charging pool identification and suffix as a charging station identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static Boolean TryParse(ChargingPool_Id         ChargingPoolId,
                                       String                  Suffix,
                                       out ChargingStation_Id  ChargingStationId)

            => ChargingPoolId.OperatorId.Format switch {
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(ChargingPoolId.OperatorId.ToString(),                           "*S", ChargingPoolId.Suffix, Suffix.IsNeitherNullNorEmpty() ? "*" + Suffix : ""), out ChargingStationId),
                   OperatorIdFormats.ISO       => TryParse(String.Concat(ChargingPoolId.OperatorId.ToString(),                            "S", ChargingPoolId.Suffix, Suffix),                                             out ChargingStationId),
                   _                           => TryParse(String.Concat(ChargingPoolId.OperatorId.ToString(OperatorIdFormats.ISO_STAR), "*S", ChargingPoolId.Suffix, Suffix.IsNeitherNullNorEmpty() ? "*" + Suffix : ""), out ChargingStationId)
               };

        #endregion

        #region (static) TryParse(EVSEId,                            out ChargingStationId, RemoveLastStar = true)

        /// <summary>
        /// Create a charging station identification based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="RemoveLastStar">Generate a charging station identification by removing the last star part.</param>
        public static Boolean TryParse(EVSE_Id                 EVSEId,
                                       out ChargingStation_Id  ChargingStationId,
                                       Boolean                 RemoveLastStar = true)
        {

            var suffix = EVSEId.Suffix;

            if (RemoveLastStar)
            {

                var starPosition = EVSEId.Suffix.LastIndexOf("*");

                if (starPosition > 0)
                    suffix = suffix[..starPosition];

            }

            return TryParse(EVSEId.OperatorId,
                            suffix.ToUpper(),
                            out ChargingStationId);

        }

        #endregion


        #region Clone

        /// <summary>
        /// Clone this charging station identification.
        /// </summary>
        public ChargingStation_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix?.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.Equals(ChargingStationId2);

        #endregion

        #region Operator != (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => !ChargingStationId1.Equals(ChargingStationId2);

        #endregion

        #region Operator <  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStationId1,
                                          ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) < 0;

        #endregion

        #region Operator <= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) <= 0;

        #endregion

        #region Operator >  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStationId1,
                                          ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) > 0;

        #endregion

        #region Operator >= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStationId1,
                                           ChargingStation_Id ChargingStationId2)

            => ChargingStationId1.CompareTo(ChargingStationId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station identifications.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStation_Id chargingStationId
                   ? CompareTo(chargingStationId)
                   : throw new ArgumentException("The given object is not a charging station identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification to compare with.</param>
        public Int32 CompareTo(ChargingStation_Id ChargingStationId)
        {

            var c = OperatorId.CompareTo(ChargingStationId.OperatorId);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargingStationId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="Object">A charging station identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStation_Id chargingStationId &&
                   Equals(chargingStationId);

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification to compare with.</param>
        public Boolean Equals(ChargingStation_Id ChargingStationId)

            => OperatorId.Equals(ChargingStationId.OperatorId) &&

               String.Equals(Suffix.                  Replace("*", ""),
                             ChargingStationId.Suffix.Replace("*", ""),
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
                   OperatorIdFormats.DIN       => "+" + OperatorId.CountryCode.TelefonCode.ToString() + "*" + (OperatorId.Suffix ?? "") + "*S" + (Suffix ?? ""),
                   OperatorIdFormats.ISO_STAR  =>       OperatorId.CountryCode.Alpha2Code +             "*" + (OperatorId.Suffix ?? "") + "*S" + (Suffix ?? ""),
                   _                           =>       OperatorId.CountryCode.Alpha2Code +                   (OperatorId.Suffix ?? "") +  "S" + (Suffix ?? "")
               };

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => String.Concat(OperatorId,  "S", Suffix),
                   OperatorIdFormats.ISO_STAR  => String.Concat(OperatorId, "*S", Suffix),
                   _                           => String.Concat(OperatorId, "*S", Suffix)
               };

        #endregion

    }

}
