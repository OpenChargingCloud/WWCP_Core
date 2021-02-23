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
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extention methods for charging station identifications.
    /// </summary>
    public static class ChargingStationIdExtentions
    {

        /// <summary>
        /// Create a new EVSE identification
        /// based on the given charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification.</param>
        /// <param name="AdditionalSuffix">An additional EVSE suffix.</param>
        public static EVSE_Id CreateEVSEId(this ChargingStation_Id  ChargingStationId,
                                           String                   AdditionalSuffix)
        {

            var Suffix = ChargingStationId.Suffix;

            if (Suffix.StartsWith("TATION", StringComparison.Ordinal))
                Suffix = "VSE" + Suffix.Substring(6);

            return EVSE_Id.Parse(ChargingStationId.OperatorId, Suffix + AdditionalSuffix ?? "");

        }

    }


    /// <summary>
    /// The unique identification of an electric vehicle charging station (EVCS).
    /// </summary>
    public readonly struct ChargingStation_Id : IId,
                                                IEquatable<ChargingStation_Id>,
                                                IComparable<ChargingStation_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random                  = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// </summary>
        public  static readonly Regex  ChargingStationId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?S([A-Z0-9][A-Z0-9\*]{0,50})$",
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
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {

                switch (OperatorId.Format)
                {

                    case OperatorIdFormats.DIN:
                        return (UInt64) (OperatorId.CountryCode.TelefonCode.ToString().Length + 1 + OperatorId.Suffix.Length + 2 + Suffix.Length);

                    case OperatorIdFormats.ISO_STAR:
                        return (UInt64) (OperatorId.CountryCode.Alpha2Code.Length             + 1 + OperatorId.Suffix.Length + 2 + Suffix.Length);

                    default:  // ISO
                        return (UInt64) (OperatorId.CountryCode.Alpha2Code.Length                 + OperatorId.Suffix.Length + 1 + Suffix.Length);

                }

            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging station identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        private ChargingStation_Id(ChargingStationOperator_Id  OperatorId,
                                   String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging station identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Create(EVSEId, RemoveLastStar = true)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSEId.</param>
        /// <param name="RemoveLastStar">Generate a charging station identification by removing the last star part.</param>
        public static ChargingStation_Id Create(EVSE_Id  EVSEId,
                                                Boolean  RemoveLastStar = true)
        {

            var _Suffix = EVSEId.Suffix;

            if (RemoveLastStar)
            {

                var _HasAStar = EVSEId.Suffix.LastIndexOf("*");

                if (_HasAStar > 0)
                    _Suffix = _Suffix.Substring(0, _HasAStar);

            }

            return Parse(EVSEId.OperatorId, _Suffix.ToUpper());

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

        #region Create(EVSEIds, HelperId = "", Length = 15, Mapper = null)

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

        #region Random(OperatorId, Length = 50, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The desired length of the identification suffix.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id Random(ChargingStationOperator_Id  OperatorId,
                                                Byte                        Length  = 50,
                                                Func<String, String>        Mapper  = null)

        {

            if (Length < 12 || Length > 50)
                Length = 50;

            return new ChargingStation_Id(OperatorId,
                                          Mapper != null
                                              ? Mapper(_Random.RandomString(Length))
                                              : _Random.RandomString(Length));

        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station identification must not be null or empty!");

            #endregion

            var MatchCollection = ChargingStationId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging station identification: '" + Text + "'!",
                                            nameof(Text));

            ChargingStationOperator_Id _OperatorId;

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out _OperatorId))
                return new ChargingStation_Id(_OperatorId,
                                              MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging station identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station identification.</param>
        public static ChargingStation_Id Parse(ChargingStationOperator_Id  OperatorId,
                                               String                      Suffix)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.ISO_STAR:
                    return Parse(OperatorId.ToString() + "*S" + Suffix);

                case OperatorIdFormats.ISO:
                    return Parse(OperatorId.ToString() + "S" + Suffix);

                default:
                    return Parse(OperatorId.ToString(OperatorIdFormats.ISO_STAR) + "*S" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        public static ChargingStation_Id? TryParse(String Text)
        {

            ChargingStation_Id ChargingStationId;

            if (TryParse(Text, out ChargingStationId))
                return ChargingStationId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationId = default(ChargingStation_Id);
                return false;
            }

            #endregion

            try
            {

                ChargingStationId = default(ChargingStation_Id);

                var _MatchCollection = ChargingStationId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id _OperatorId;

                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                {

                    ChargingStationId = new ChargingStation_Id(_OperatorId,
                                                               _MatchCollection[0].Groups[2].Value);

                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception e)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ChargingStationId = default(ChargingStation_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station identification.
        /// </summary>
        public ChargingStation_Id Clone

            => new ChargingStation_Id(OperatorId.Clone,
                                      new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationId1, ChargingStationId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationId1 == null) || ((Object) ChargingStationId2 == null))
                return false;

            return ChargingStationId1.Equals(ChargingStationId2);

        }

        #endregion

        #region Operator != (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
            => !(ChargingStationId1 == ChargingStationId2);

        #endregion

        #region Operator <  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {

            if ((Object) ChargingStationId1 == null)
                throw new ArgumentNullException(nameof(ChargingStationId1), "The given ChargingStationId1 must not be null!");

            return ChargingStationId1.CompareTo(ChargingStationId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
            => !(ChargingStationId1 > ChargingStationId2);

        #endregion

        #region Operator >  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {

            if ((Object) ChargingStationId1 == null)
                throw new ArgumentNullException(nameof(ChargingStationId1), "The given ChargingStationId1 must not be null!");

            return ChargingStationId1.CompareTo(ChargingStationId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A charging station identification.</param>
        /// <param name="ChargingStationId2">Another charging station identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
            => !(ChargingStationId1 < ChargingStationId2);

        #endregion

        #endregion

        #region IComparable<ChargingStationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStation_Id))
                throw new ArgumentException("The given object is not a charging station identification!", nameof(Object));

            return CompareTo((ChargingStation_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingStation_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging station identification must not be null!");

            // Compare the length of the ChargingStationIds
            var _Result = Length.CompareTo(ChargingStationId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = OperatorId.CompareTo(ChargingStationId.OperatorId);

            // If equal: Compare ChargingStationId suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ChargingStationId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation_Id> Members

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

            if (!(Object is ChargingStation_Id))
                return false;

            return Equals((ChargingStation_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                return false;

            return OperatorId.Equals(ChargingStationId.OperatorId) &&
                   Suffix.    Equals(ChargingStationId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
               Suffix.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.DIN:
                    return "+" + OperatorId.CountryCode.TelefonCode.ToString() + "*" + OperatorId.Suffix + "*S" + Suffix;

                case OperatorIdFormats.ISO_STAR:
                    return OperatorId.CountryCode.Alpha2Code + "*" + OperatorId.Suffix + "*S" + Suffix;

                default: // ISO
                    return OperatorId.CountryCode.Alpha2Code       + OperatorId.Suffix + "S" + Suffix;

            }

        }

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.ISO:
                    return String.Concat(OperatorId.CountryCode.Alpha2Code,
                                         OperatorId.Suffix,
                                         "S",
                                         Suffix);

                case OperatorIdFormats.ISO_STAR:
                    return String.Concat(OperatorId.CountryCode.Alpha2Code,
                                         "*",
                                         OperatorId.Suffix,
                                         "*S",
                                         Suffix);

                default: // DIN
                    return String.Concat("+",
                                         OperatorId.CountryCode.TelefonCode,
                                         "*",
                                         OperatorId.Suffix,
                                         "*S",
                                         Suffix);

            }

        }

        #endregion

    }

}
