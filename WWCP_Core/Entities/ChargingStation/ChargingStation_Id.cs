/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
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

        /// <summary>
        /// The regular expression for parsing a charging station identification.
        /// </summary>
        public    const    String  ChargingStationId_RegEx  = @"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?S([A-Z0-9][A-Z0-9\*]{0,30})$ | ^(\+?[0-9]{1,3}\*?[0-9]{3})\*?([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// </summary>
        public    const    String  IdSuffix_RegEx           = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String  _IdSuffix;

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

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return _OperatorId.Length + 2 + (UInt64) _IdSuffix.Length;
            }
        }

        #endregion

        #region OldEVCSId

        public String OldEVCSId
        {
            get
            {
                return String.Concat(_OperatorId.IdOld, "*", _IdSuffix);
            }
        }

        #endregion

        #region OriginFormat

        private readonly OriginFormatType _OriginFormat;

        public OriginFormatType OriginFormat
        {
            get
            {
                return _OriginFormat;
            }
        }

        #endregion

        #region OriginEVCSId

        public String OriginEVCSId
        {
            get
            {
                return (_OriginFormat == OriginFormatType.NEW)
                          ? String.Concat(_OperatorId.ToString(), "*S", _IdSuffix)
                          : String.Concat(_OperatorId.IdOld,       "*", _IdSuffix);
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
                                   String            IdSuffix,
                                   OriginFormatType  OriginFormat)
        {

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The OperatorId must not be null!");

            var _MatchCollection = Regex.Matches(IdSuffix.Trim().ToUpper(),
                                                 IdSuffix_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging station identification suffix '" + IdSuffix + "'!", "IdSuffix");

            _OperatorId    = OperatorId;
            _IdSuffix      = _MatchCollection[0].Value;
            _OriginFormat  = OriginFormat;

        }

        #endregion


        #region Create(params EVSEIds)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An array of EVSEIds.</param>
        public static ChargingStation_Id Create(params EVSE_Id[] EVSEIds)
        {
            return Create((IEnumerable<EVSE_Id>) EVSEIds);
        }

        #endregion

        #region Create(EVSEIds)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSEIds.</param>
        public static ChargingStation_Id Create(IEnumerable<EVSE_Id> EVSEIds)
        {

            var EVSEIdPrefixStrings = EVSEIds.
                                          Select(EVSEId => EVSEId.OriginEVSEId.Split('*')).
                                          Select(EVSEIdElements => EVSEIdElements.
                                                                       Take(EVSEIdElements.Length - 1).
                                                                       AggregateWith("*")).
                                          Distinct().
                                          ToArray();

            if (EVSEIdPrefixStrings.Length == 1)
                return ChargingStation_Id.Parse(EVSEIdPrefixStrings.First().Replace("*E", "*S"));

            throw new ApplicationException("Could not create a common ChargingStationId based on the EVSEId prefixes " + EVSEIdPrefixStrings.Select(v => "'" + v + "'").AggregateWith(", ") + "!");

        }

        #endregion

        #region New(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle Charging Station (EVCS Id).
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id New(EVSEOperator_Id       OperatorId,
                                             Func<String, String>  Mapper = null)
        {

            return new ChargingStation_Id(OperatorId,
                                          Mapper != null ? Mapper(Guid.NewGuid().ToString()) : Guid.NewGuid().ToString(),
                                          OriginFormatType.NEW);

        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 ChargingStationId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging station identification '" + Text + "'!", "ChargingStationId");

            EVSEOperator_Id __EVSEOperatorId = null;

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new ChargingStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[2].Value,
                                              OriginFormatType.NEW);

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new ChargingStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[4].Value,
                                              OriginFormatType.OLD);


            throw new ArgumentException("Illegal charging station identification '" + Text + "'!", "ChargingStationId");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        public static ChargingStation_Id Parse(EVSEOperator_Id OperatorId, String IdSuffix)
        {
            return ChargingStation_Id.Parse(OperatorId.ToString() + "*" + IdSuffix);
        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Station identification.</param>
        /// <param name="ChargingStationId">The parsed Electric Vehicle Charging Station identification.</param>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {

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
                                                               OriginFormatType.NEW);

                    return true;

                }

                // Old format...
                else if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    ChargingStationId = new ChargingStation_Id(__EVSEOperatorId,
                                                               _MatchCollection[0].Groups[4].Value,
                                                               OriginFormatType.OLD);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ChargingStationId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out ChargingStationId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String EVSEIdSuffix, out EVSE_Id EVSE_Id)
        //{

        //    try
        //    {
        //        EVSE_Id = new EVSE_Id(OperatorId, EVSEIdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    EVSE_Id = null;
        //    return false;

        //}

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
                                              new String(_IdSuffix.ToCharArray()),
                                              OriginFormat);
            }
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
                _Result = _IdSuffix.CompareTo(ChargingStationId._IdSuffix);

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
                   _IdSuffix.  Equals(ChargingStationId._IdSuffix);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return _OperatorId.GetHashCode() ^ _IdSuffix.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_OperatorId.ToString(), "*S", _IdSuffix);
        }

        #endregion

    }

}
