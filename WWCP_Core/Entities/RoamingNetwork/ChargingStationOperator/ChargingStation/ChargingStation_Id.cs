/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an electric vehicle charging station (EVCS).
    /// </summary>
    public struct ChargingStation_Id : IId,
                                       IEquatable<ChargingStation_Id>,
                                       IComparable<ChargingStation_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random                  = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// </summary>
        public  static readonly Regex  ChargingStationId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?S([A-Z0-9][A-Z0-9\*]{0,30})$",
                                                                            RegexOptions.IgnorePatternWhitespace);

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
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (OperatorId.ToFormat(OperatorIdFormats.ISO_STAR).Length + 2 + Suffix.Length);

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


        #region Create(EVSEId)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSEId.</param>
        public static ChargingStation_Id Create(EVSE_Id  EVSEId)
        {

            var _Array = EVSEId.ToString().Split('*', '-');

            if (EVSEId.Format == OperatorIdFormats.ISO)
            {
                if (_Array[2].StartsWith("E"))
                    _Array[2] = "S" + _Array[2].Substring(1);
            }
            else
            {
                if (!_Array[2].StartsWith("S"))
                     _Array[2] = "S" + _Array[2];
            }


            // e.g. "DE*822*E123456"
            if (_Array.Length == 3)
            {

                if (EVSEId.ToString().Contains('-'))
                    return Parse(_Array.AggregateWith("-"));

                return Parse(_Array.AggregateWith("*"));

            }

            // e.g. "DE*822*E123456*1" => "DE*822*S123456"
            if (EVSEId.ToString().Contains('-'))
                return Parse(_Array.Take(_Array.Length - 1).AggregateWith("-"));

            return Parse(_Array.Take(_Array.Length - 1).AggregateWith("*"));

        }

        #endregion

        #region Create(EVSEIds)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSEIds.</param>
        public static ChargingStation_Id? Create(IEnumerable<EVSE_Id> EVSEIds)
        {

            #region Initial checks

            if (EVSEIds == null)
                return null;

            var _EVSEIds = EVSEIds.ToArray();

            if (_EVSEIds.Length == 0)
                return null;

            // It is just a single EVSE Id...
            if (_EVSEIds.Length == 1)
                return Create(_EVSEIds[0]);

            #endregion

            #region Multiple EVSE Ids...

            String[] EVSEIdPrefixStrings = null;

            #region EVSEIdSuffix contains '*' or '-'...

            if (_EVSEIds.Any(EVSEId => EVSEId.Suffix.Contains('*') ||
                                       EVSEId.Suffix.Contains('-')))
            {

                EVSEIdPrefixStrings = _EVSEIds.
                                          Select(EVSEId         => EVSEId.ToString().Split('*', '-')).
                                          Select(EVSEIdElements => {

                                              if (EVSEIdElements.Length < 4)
                                                  return new String[] { "" };

                                              if (_EVSEIds[0].Format == OperatorIdFormats.ISO)
                                                  if (EVSEIdElements[2].StartsWith("E", StringComparison.Ordinal))
                                                      EVSEIdElements[2] = "S" + EVSEIdElements[2].Substring(1);

                                              return EVSEIdElements;

                                          }).
                                          Select(EVSEIdElements => EVSEIdElements.
                                                                       Take(EVSEIdElements.Length - 1).
                                                                       AggregateWith("*", "")).
                                          Where(v => v != "").
                                          Distinct().
                                          ToArray();

            }

            #endregion

            #region ...or use longest prefix!

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
                            _Prefix = TmpEVSEId.OperatorId + "*S" + TmpEVSEId.Suffix;
                        else
                            return null;
                    }

                    EVSEIdPrefixStrings = new String[1] { _Prefix };

                }

                else
                    return null;

            }

            #endregion


            if (EVSEIdPrefixStrings.Length == 1)
            {

                var Id = EVSEIdPrefixStrings.First();

                if (Id.Contains('-'))
                    Id = Id.Replace("-", "*");

                var IdElements = Id.Split(new String[] { "*" }, StringSplitOptions.None);

                return Parse(IdElements[0] + "*" + IdElements[1] + "*" + IdElements.Skip(2).Aggregate("*"));

            }

            #endregion

            return null;

        }

        #endregion

        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id Random(ChargingStationOperator_Id  OperatorId,
                                                Func<String, String>        Mapper  = null)


            => new ChargingStation_Id(OperatorId,
                                      Mapper != null ? Mapper(_Random.RandomString(30)) : _Random.RandomString(30));

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

            => Parse(OperatorId.ToFormat(OperatorIdFormats.ISO_STAR) + "*S" + Suffix);

        #endregion

        #region TryParse(Text, out EVSEGroupId)

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
            if (Object.ReferenceEquals(ChargingStationId1, ChargingStationId2))
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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*S", Suffix);

        #endregion

    }

}
