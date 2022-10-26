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
        {
            get
            {
                return OperatorId.Format switch {
                    OperatorIdFormats.DIN       => (UInt64) (OperatorId.CountryCode.TelefonCode.ToString().Length + 1 + (OperatorId.Suffix?.Length ?? 0) + 2 + (Suffix?.Length ?? 0)),
                    OperatorIdFormats.ISO_STAR  => (UInt64) (OperatorId.CountryCode.Alpha2Code.            Length + 1 + (OperatorId.Suffix?.Length ?? 0) + 2 + (Suffix?.Length ?? 0)),
                    _                           => (UInt64) (OperatorId.CountryCode.Alpha2Code.            Length +     (OperatorId.Suffix?.Length ?? 0) + 1 + (Suffix?.Length ?? 0)), // also ISO!
                };
            }
        }

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


        #region (static) Create(EVSEId,  RemoveLastStar = true)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSEId.</param>
        /// <param name="RemoveLastStar">Generate a charging station identification by removing the last star part.</param>
        public static ChargingStation_Id Create(EVSE_Id  EVSEId,
                                                Boolean  RemoveLastStar = true)
        {

            var suffix = EVSEId.Suffix;

            if (RemoveLastStar)
            {

                var hasAStar = EVSEId.Suffix.LastIndexOf("*");

                if (hasAStar > 0)
                    suffix = suffix[..hasAStar];

            }

            return Parse(EVSEId.OperatorId,
                         suffix.ToUpper());

            //var _Array = new String[] {
            //                 EVSEId.OperatorId.CountryCode.Alpha2Code,
            //                 EVSEId.OperatorId.Suffix
            //             }.Concat(EVSEId.ToString().Substring(2 + EVSEId.OperatorId.Suffix.Length).ToUpper().Split('*', '-')).ToArray();

            //if (EVSEId.Format == OperatorIdFormats.ISO || EVSEId.Format == OperatorIdFormats.ISO_STAR)
            //{
            //    if (_Array[2].StartsWith("E", StringComparison.Ordinal))
            //        _Array[2] = "S" + _Array[2].Substring(1);
            //}
            //else
            //{
            //    if (!_Array[2].StartsWith("S", StringComparison.Ordinal))
            //         _Array[2] = "S" + _Array[2];
            //}


            //// e.g. "DE*822*E123456"
            //if (_Array.Length == 3)
            //{

            //    if (EVSEId.ToString().Contains('-'))
            //        return Parse(_Array.AggregateWith("-"));

            //    return Parse(_Array.AggregateWith("*"));

            //}

            //// e.g. "DE*822*E123456*1" => "DE*822*S123456"
            //if (EVSEId.ToString().Contains('-'))
            //    return Parse(_Array.Take(_Array.Length - 1).AggregateWith("-"));

            //if (_Array[0].StartsWith("+"))
            //    return Parse(_Array.Take(1).Select(item => Country.ParseTelefonCode(item.Substring(1)).Alpha2Code).Concat(_Array.Skip(1).Take(_Array.Length - 1)).AggregateWith("*"));

            //else
            //    return Parse(_Array.Take(_Array.Length - 1).AggregateWith("*"));

        }

        #endregion

        #region (static) Create(EVSEIds, HelperId = "", Length = 15, Mapper = null)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSEIds.</param>
        public static ChargingStation_Id? Create(IEnumerable<EVSE_Id>  EVSEIds,
                                                 String                HelperId  = "",
                                                 Byte                  Length    = 15,
                                                 Func<String, String>  Mapper    = null)
        {

            #region Initial checks

            if (EVSEIds == null)
                return null;

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                return null;

            #endregion

            #region A single EVSE Id...

            // It is just a single EVSE Id...
            if (_EVSEIds.Length == 1)
                return Create(_EVSEIds[0]);

            #endregion


            // Multiple OperatorIds which do not match!
            if (_EVSEIds.Select(evse => evse.OperatorId.ToString()).Distinct().Count() > 1)
                return null;

            #region EVSEIdSuffix contains '*'...

            if (_EVSEIds.Any(EVSEId => EVSEId.Suffix.Contains('*') ||
                                       EVSEId.Suffix.Contains('-')))
            {

                var EVSEIdPrefixStrings = _EVSEIds.
                                              Select(EVSEId         => EVSEId.Suffix.Split(StarSplitter, StringSplitOptions.RemoveEmptyEntries)).
                                              Select(SuffixElements => SuffixElements.Take(SuffixElements.Length - 1).AggregateWith("*", "")).
                                              Where (NewSuffix      => NewSuffix != "").
                                              Distinct().
                                              ToArray();

                if (EVSEIdPrefixStrings.Length != 1)
                    return null;

                return Parse(_EVSEIds[0].OperatorId, EVSEIdPrefixStrings[0]);

            }

            #endregion

            #region ...or use longest prefix...

            else
            {

                var _Array      = _EVSEIds.Select(evse => evse.ToString()).ToArray();
                var _MinLength  = (Int32) _Array.Select(v => v.Length).Min();

                var _Prefix     = "";

                for (var i = 0; i < _MinLength; i++)
                {
                    if (_Array.All(v => v[i] == _Array[0][i]))
                        _Prefix += _Array[0][i];
                }

                if (((UInt64) _Prefix.Length) > _EVSEIds[0].OperatorId.Length + 1)
                {

                    var TmpEVSEId = EVSE_Id.Parse(_Prefix);

                    if (TmpEVSEId.Format == OperatorIdFormats.ISO)
                    {
                        if (((UInt64) _Prefix.Length) > _EVSEIds[0].OperatorId.Length + 2)
                            _Prefix = TmpEVSEId.Suffix; //TmpEVSEId.OperatorId + "*S" + TmpEVSEId.Suffix;
                        else
                            return null;
                    }

                    return Parse(_EVSEIds[0].OperatorId, _Prefix);

                }

            }

            #endregion

            #region ...or generate a hash of the EVSEIds!

            if (Length < 12)
                Length = 12;

            if (Length > 50)
                Length = 50;

            var Suffíx = new SHA1CryptoServiceProvider().
                             ComputeHash(Encoding.UTF8.GetBytes(EVSEIds.Select(evseid => evseid.ToString()).
                                                                        AggregateWith(HelperId ?? ","))).
                             ToHexString().
                             SubstringMax(Length).
                             ToUpper();

            return Parse(_EVSEIds[0].OperatorId,
                         Mapper != null
                            ? Mapper(Suffíx)
                            : Suffíx);

            #endregion

        }

        #endregion


        #region (static) NewRandom(OperatorId,     Length = 50, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                   Byte                        Length   = 50,
                                                   Func<String, String>?       Mapper   = null)

        {

            if (Length < 12 || Length > 50)
                Length = 50;

            return Parse(OperatorId,
                         Mapper is not null
                             ? Mapper(RandomExtensions.RandomString(Length))
                             :        RandomExtensions.RandomString(Length));

        }

        #endregion

        #region (static) NewRandom(ChargingPoolId, Length = 40, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id NewRandom(ChargingPool_Id        ChargingPoolId,
                                                   Byte                   Length   = 40,
                                                   Func<String, String>?  Mapper   = null)

        {

            if (Length < 12 || Length > 40)
                Length = 40;

            return Parse(ChargingPoolId,
                         Mapper is not null
                             ? Mapper(RandomExtensions.RandomString(Length))
                             :        RandomExtensions.RandomString(Length));

        }

        #endregion


        #region (static) Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargingStation_Id chargingStationId))
                return chargingStationId;

            throw new ArgumentException("Illegal charging station identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse(ChargingStationOperatorId, Suffix)

        /// <summary>
        /// Parse the given charging station operator identification and suffix as a charging station identification.
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

        #region (static) Parse(ChargingPoolId,            Suffix)

        /// <summary>
        /// Parse the given charging pool identification and suffix as a charging station identification.
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


        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        public static ChargingStation_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingStation_Id chargingStationId))
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

                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out ChargingStationOperator_Id chargingStationOperatorId))
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

                OperatorIdFormats.ISO       => String.Concat(OperatorId.CountryCode.Alpha2Code,
                                                             OperatorId.Suffix,
                                                             "S",
                                                             Suffix),

                OperatorIdFormats.ISO_STAR  => String.Concat(OperatorId.CountryCode.Alpha2Code,
                                                             "*",
                                                             OperatorId.Suffix,
                                                             "*S",
                                                             Suffix),

                _                           => String.Concat("+",
                                                             OperatorId.CountryCode.TelefonCode,
                                                             "*",
                                                             OperatorId.Suffix,
                                                             "*S",
                                                             Suffix)

            };

        #endregion

    }

}
