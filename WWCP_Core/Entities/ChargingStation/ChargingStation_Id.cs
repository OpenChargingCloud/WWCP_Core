/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

using org.GraphDefined.Vanaheimr.Illias;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Charging Station (EVCS Id).
    /// </summary>
    public class ChargingStation_Id : IId,
                                      IEquatable<ChargingStation_Id>,
                                      IComparable<ChargingStation_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// </summary>
        public    const    String  ChargingStationId_RegEx  = @"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?S([A-Z0-9][A-Z0-9\*]{0,30})$ | ^(\+?[0-9]{1,3}\*?[0-9]{3})\*?S([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// </summary>
        public    const    String  IdSuffix_RegEx           = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

        #endregion

        #region Properties

        #region OperatorId

        private readonly EVSEOperator_Id _OperatorId;

        /// <summary>
        /// The internal identification.
        /// </summary>
        public EVSEOperator_Id OperatorId
        {
            get
            {
                return _OperatorId;
            }
        }

        #endregion

        #region Suffix

        private readonly String _Suffix;

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String Suffix
        {
            get
            {
                return _Suffix;
            }
        }

        #endregion

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return _OperatorId.Length + 2 + (UInt64) _Suffix.Length;
            }
        }

        #endregion

        #region Format

        private readonly IdFormatType _Format;

        public IdFormatType Format
        {
            get
            {
                return _Format;
            }
        }

        #endregion

        #region OriginId

        public String OriginId
        {
            get
            {
                return ToFormat(_Format);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle charging station identification (EVCS Id)
        /// based on the given string.
        /// </summary>
        private ChargingStation_Id(EVSEOperator_Id   OperatorId,
                                   String            Suffix,
                                   IdFormatType      Format = IdFormatType.NEW)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The parameter must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException("IdSuffix", "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = Regex.Matches(Suffix.Trim().ToUpper(),
                                                 IdSuffix_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging station identification suffix '" + Suffix + "'!", "IdSuffix");

            this._OperatorId  = OperatorId;
            this._Suffix      = _MatchCollection[0].Value;
            this._Format      = Format;

        }

        #endregion


        #region Create(EVSEId)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSE identification.
        /// </summary>
        /// <param name="EVSEId">An EVSEId.</param>
        public static ChargingStation_Id Create(EVSE_Id  EVSEId)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentException("The parameter must not be null or empty!", "EVSEId");

            #endregion


            var _Array = EVSEId.OriginId.Split('*', '-');

            if (EVSEId.Format == IdFormatType.NEW)
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

                if (EVSEId.OriginId.Contains('-'))
                    return ChargingStation_Id.Parse(_Array.AggregateWith("-"));

                return ChargingStation_Id.Parse(_Array.AggregateWith("*"));

            }

            // e.g. "DE*822*E123456*1" => "DE*822*S123456"
            if (EVSEId.OriginId.Contains('-'))
                return ChargingStation_Id.Parse(_Array.Take(_Array.Length - 1).AggregateWith("-"));

            return ChargingStation_Id.Parse(_Array.Take(_Array.Length - 1).AggregateWith("*"));

        }

        #endregion

        #region Create(EVSEIds)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSEIds.</param>
        public static ChargingStation_Id Create(IEnumerable<EVSE_Id> EVSEIds)
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
                                          Select(EVSEId         => EVSEId.OriginId.Split('*', '-')).
                                          Select(EVSEIdElements => {

                                              if (EVSEIdElements.Length < 4)
                                                  return new String[] { "" };

                                              if (_EVSEIds[0].Format == IdFormatType.NEW)
                                                  if (EVSEIdElements[2].StartsWith("E"))
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

                var _Array      = _EVSEIds.Select(EVSEId => EVSEId.OriginId).ToArray();
                var _MinLength  = _Array.Select(v => v.Length).Min();

                var _Prefix     = "";

                for (var i = 0; i < _MinLength; i++)
                {
                    if (_Array.All(v => v[i] == _Array[0][i]))
                        _Prefix += _Array[0][i];
                }

                if (_Prefix.Length > _EVSEIds[0].OperatorId.OriginId.Length + 1)
                {

                    var TmpEVSEId = EVSE_Id.Parse(_Prefix);

                    if (TmpEVSEId.Format == IdFormatType.NEW)
                    {
                        if (_Prefix.Length > _EVSEIds[0].OperatorId.OriginId.Length + 2)
                            _Prefix = TmpEVSEId.OperatorId.OriginId + "*S" + TmpEVSEId.Suffix;
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

                return ChargingStation_Id.Parse(IdElements[0] + "*" + IdElements[1] + "*S" + IdElements.Skip(2).Aggregate("*"));

            }

            #endregion

            return null;

        }

        #endregion

        #region Random(OperatorId, Mapper = null, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle Charging Station (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        /// <param name="IdFormat">The (EVSE-)format of the charging station identification [old|new].</param>
        public static ChargingStation_Id Random(EVSEOperator_Id       OperatorId,
                                                Func<String, String>  Mapper    = null,
                                                IdFormatType          IdFormat  = IdFormatType.NEW)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentException("The parameter must not be null!", "OperatorId");

            #endregion

            return new ChargingStation_Id(OperatorId,
                                          Mapper != null ? Mapper(_Random.RandomString(12)) : _Random.RandomString(12),
                                          IdFormat);

        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "Text");

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 ChargingStationId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging station identification '" + Text + "'!", "Text");

            EVSEOperator_Id __EVSEOperatorId = null;

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new ChargingStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[2].Value,
                                              IdFormatType.NEW);

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new ChargingStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[4].Value,
                                              IdFormatType.OLD);


            throw new ArgumentException("Illegal charging station identification '" + Text + "'!", "Text");

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="Suffix">A text representation of a charging station identification.</param>
        public static ChargingStation_Id Parse(EVSEOperator_Id OperatorId, String Suffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentException("The parameter must not be null!", "OperatorId");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "IdSuffix");

            #endregion

            ChargingStation_Id _ChargingStationId = null;

            if (ChargingStation_Id.TryParse(OperatorId, Suffix, out _ChargingStationId))
                return _ChargingStationId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingStationId = null;

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     ChargingStationId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                EVSEOperator_Id __EVSEOperatorId = null;

                // New format...
                if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    ChargingStationId = new ChargingStation_Id(__EVSEOperatorId,
                                                               _MatchCollection[0].Groups[2].Value,
                                                               IdFormatType.NEW);

                    return true;

                }

                // Old format...
                else if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    ChargingStationId = new ChargingStation_Id(__EVSEOperatorId,
                                                               _MatchCollection[0].Groups[4].Value,
                                                               IdFormatType.OLD);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ChargingStationId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, Suffix, out ChargingStationId)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="Suffix">A text representation of a charging station identification.</param>
        /// <param name="ChargingStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(EVSEOperator_Id         OperatorId,
                                       String                  Suffix,
                                       out ChargingStation_Id  ChargingStationId)
        {

            #region Initial checks

            if (OperatorId == null || Suffix.IsNullOrEmpty())
            {
                ChargingStationId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingStationId = null;

                var _MatchCollection = Regex.Matches(Suffix.Trim().ToUpper(),
                                                     IdSuffix_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationId = new ChargingStation_Id(OperatorId,
                                                           _MatchCollection[0].Groups[0].Value,
                                                           OperatorId.Format);

                return true;

            }
            catch (Exception e)
            { }

            ChargingStationId = null;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public ChargingStation_Id Clone
        {
            get
            {
                return new ChargingStation_Id(OperatorId.Clone,
                                              new String(_Suffix.ToCharArray()),
                                              Format);
            }
        }

        #endregion


        #region ToFormat(IdFormat)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="IdFormat">The format.</param>
        public String ToFormat(IdFormatType IdFormat)
        {

            return (IdFormat == IdFormatType.NEW)
                       ? String.Concat(_OperatorId.ToFormat(IdFormat), "*S", _Suffix)
                       : String.Concat(_OperatorId.ToFormat(IdFormat),  "*", _Suffix);

        }

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="IdFormat">The format.</param>
        public String ToFormat(IdFormatOriginType IdFormat)
        {

            if (IdFormat == IdFormatOriginType.Origin)
                return ToFormat(this.Format);

            return ToFormat((IdFormatType) IdFormat);

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A ChargingStation_Id.</param>
        /// <param name="ChargingStationId2">Another ChargingStation_Id.</param>
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
        /// <param name="ChargingStationId1">A ChargingStation_Id.</param>
        /// <param name="ChargingStationId2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {
            return !(ChargingStationId1 == ChargingStationId2);
        }

        #endregion

        #region Operator <  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A ChargingStation_Id.</param>
        /// <param name="ChargingStationId2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {

            if ((Object) ChargingStationId1 == null)
                throw new ArgumentNullException("The given ChargingStationId1 must not be null!");

            return ChargingStationId1.CompareTo(ChargingStationId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A ChargingStation_Id.</param>
        /// <param name="ChargingStationId2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {
            return !(ChargingStationId1 > ChargingStationId2);
        }

        #endregion

        #region Operator >  (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A ChargingStation_Id.</param>
        /// <param name="ChargingStationId2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {

            if ((Object) ChargingStationId1 == null)
                throw new ArgumentNullException("The given ChargingStationId1 must not be null!");

            return ChargingStationId1.CompareTo(ChargingStationId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationId1, ChargingStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId1">A ChargingStation_Id.</param>
        /// <param name="ChargingStationId2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStationId1, ChargingStation_Id ChargingStationId2)
        {
            return !(ChargingStationId1 < ChargingStationId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStation_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingStationId.
            var ChargingStationId = Object as ChargingStation_Id;
            if ((Object) ChargingStationId == null)
                throw new ArgumentException("The given object is not a ChargingStationId!");

            return CompareTo(ChargingStationId);

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
                throw new ArgumentNullException("The given ChargingStationId must not be null!");

            // Compare the length of the ChargingStationIds
            var _Result = this.Length.CompareTo(ChargingStationId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = _OperatorId.CompareTo(ChargingStationId._OperatorId);

            // If equal: Compare ChargingStationId suffix
            if (_Result == 0)
                _Result = _Suffix.CompareTo(ChargingStationId._Suffix);

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

            // Check if the given object is an ChargingStationId.
            var ChargingStationId = Object as ChargingStation_Id;
            if ((Object) ChargingStationId == null)
                return false;

            return this.Equals(ChargingStationId);

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

            return _OperatorId.Equals(ChargingStationId._OperatorId) &&
                   _Suffix.  Equals(ChargingStationId._Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return _OperatorId.GetHashCode() ^ _Suffix.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_OperatorId, "*S", _Suffix);
        }

        #endregion

    }

}
