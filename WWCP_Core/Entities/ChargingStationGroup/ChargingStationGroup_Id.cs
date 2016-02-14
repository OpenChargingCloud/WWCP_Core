/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of a charging station group.
    /// </summary>
    public class ChargingStationGroup_Id : IId,
                                           IEquatable<ChargingStationGroup_Id>,
                                           IComparable<ChargingStationGroup_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging group identification.
        /// </summary>
        public    const    String  ChargingGroupId_RegEx  = @"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?G([A-Z0-9][A-Z0-9\*]{0,30})$ | ^(\+?[0-9]{1,3}\*?[0-9]{3})\*?([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing an charging group identification.
        /// </summary>
        public    const    String  IdSuffix_RegEx        = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

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
        /// Generate a new charging station group identification
        /// based on the given string.
        /// </summary>
        private ChargingStationGroup_Id(EVSEOperator_Id   OperatorId,
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
                throw new ArgumentException("Illegal charging station group identification suffix '" + Suffix + "'!", "IdSuffix");

            this._OperatorId  = OperatorId;
            this._Suffix      = _MatchCollection[0].Value;
            this._Format      = Format;

        }

        #endregion


        #region Random(OperatorId, Mapper = null, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Generate a new unique identification of a charging station group.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station group identification.</param>
        /// <param name="IdFormat">The (EVSE-)format of the charging station identification [old|new].</param>
        public static ChargingStationGroup_Id Random(EVSEOperator_Id       OperatorId,
                                                     Func<String, String>  Mapper    = null,
                                                     IdFormatType          IdFormat  = IdFormatType.NEW)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentException("The parameter must not be null!", "OperatorId");

            #endregion

            return new ChargingStationGroup_Id(OperatorId,
                                       Mapper != null ? Mapper(_Random.RandomString(12)) : _Random.RandomString(12),
                                       IdFormat);

        }

        #endregion

        #region Generate(EVSEOperatorId, Address, GeoLocation, Lenght = 12, Mapper = null)

        /// <summary>
        /// Create a valid charging station group identification based on the given parameters.
        /// </summary>
        /// <param name="EVSEOperatorId">The identification of an EVSE operator.</param>
        /// <param name="Address">The address of the charging station group.</param>
        /// <param name="GeoLocation">The geo location of the charging station group.</param>
        /// <param name="Length">The maximum size of the generated charging station group identification suffix.</param>
        /// <param name="Mapper">A delegate to modify a generated charging station group identification suffix.</param>
        public static ChargingStationGroup_Id Generate(EVSEOperator_Id       EVSEOperatorId,
                                                       Address               Address,
                                                       GeoCoordinate         GeoLocation,
                                                       Byte                  Length = 12,
                                                       Func<String, String>  Mapper = null)
        {

            var Suffíx = new SHA1CryptoServiceProvider().
                             ComputeHash(Encoding.UTF8.GetBytes(Address.    ToString() +
                                                                GeoLocation.ToString())).
                                         ToHexString().
                                         Substring(0, Math.Min(40, (Int32) Length)).
                                         ToUpper();

            return ChargingStationGroup_Id.Parse(EVSEOperatorId, Mapper != null ? Mapper(Suffíx) : Suffíx);

        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station group identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station group identification.</param>
        public static ChargingStationGroup_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "Text");

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 ChargingGroupId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging station group identification '" + Text + "'!", "Text");

            EVSEOperator_Id __EVSEOperatorId = null;

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new ChargingStationGroup_Id(__EVSEOperatorId,
                                           _MatchCollection[0].Groups[2].Value,
                                           IdFormatType.NEW);

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new ChargingStationGroup_Id(__EVSEOperatorId,
                                           _MatchCollection[0].Groups[4].Value,
                                           IdFormatType.OLD);


            throw new ArgumentException("Illegal charging station group identification '" + Text + "'!", "Text");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a charging station group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="IdSuffix">A text representation of a charging station group identification.</param>
        public static ChargingStationGroup_Id Parse(EVSEOperator_Id OperatorId, String IdSuffix)
        {

            ChargingStationGroup_Id _ChargingStationGroupId = null;

            if (ChargingStationGroup_Id.TryParse(OperatorId, IdSuffix, out _ChargingStationGroupId))
                return _ChargingStationGroupId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingStationGroupId)

        /// <summary>
        /// Parse the given string as a charging station group identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station group identification.</param>
        /// <param name="ChargingStationGroupId">The parsed charging station group identification.</param>
        public static Boolean TryParse(String Text, out ChargingStationGroup_Id ChargingStationGroupId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingStationGroupId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingStationGroupId = null;

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     ChargingGroupId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                EVSEOperator_Id __EVSEOperatorId = null;

                // New format...
                if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    ChargingStationGroupId = new ChargingStationGroup_Id(__EVSEOperatorId,
                                                         _MatchCollection[0].Groups[2].Value,
                                                         IdFormatType.NEW);

                    return true;

                }

                // Old format...
                else if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    ChargingStationGroupId = new ChargingStationGroup_Id(__EVSEOperatorId,
                                                         _MatchCollection[0].Groups[4].Value,
                                                         IdFormatType.OLD);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ChargingStationGroupId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out ChargingStationGroupId)

        /// <summary>
        /// Parse the given string as a charging station group identification (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="IdSuffix">A text representation of a charging station group identification.</param>
        /// <param name="ChargingStationGroupId">The parsed charging station group identification.</param>
        public static Boolean TryParse(EVSEOperator_Id      OperatorId,
                                       String               IdSuffix,
                                       out ChargingStationGroup_Id  ChargingStationGroupId)
        {

            #region Initial checks

            if (OperatorId == null || IdSuffix.IsNullOrEmpty())
            {
                ChargingStationGroupId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingStationGroupId = null;

                var _MatchCollection = Regex.Matches(IdSuffix.Trim().ToUpper(),
                                                     IdSuffix_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationGroupId = new ChargingStationGroup_Id(OperatorId,
                                                                     _MatchCollection[0].Groups[0].Value,
                                                                     OperatorId.Format);

                return true;

            }
            catch (Exception e)
            { }

            ChargingStationGroupId = null;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public ChargingStationGroup_Id Clone
        {
            get
            {
                return new ChargingStationGroup_Id(OperatorId.Clone,
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
                       ? String.Concat(_OperatorId.ToFormat(IdFormat), "*P", _Suffix)
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

        #region Operator == (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A ChargingStationGroupId.</param>
        /// <param name="ChargingStationGroupId2">Another ChargingStationGroupId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStationGroupId1, ChargingStationGroupId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationGroupId1 == null) || ((Object) ChargingStationGroupId2 == null))
                return false;

            return ChargingStationGroupId1.Equals(ChargingStationGroupId2);

        }

        #endregion

        #region Operator != (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A ChargingStationGroupId.</param>
        /// <param name="ChargingStationGroupId2">Another ChargingStationGroupId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {
            return !(ChargingStationGroupId1 == ChargingStationGroupId2);
        }

        #endregion

        #region Operator <  (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A ChargingStationGroupId.</param>
        /// <param name="ChargingStationGroupId2">Another ChargingStationGroupId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {

            if ((Object) ChargingStationGroupId1 == null)
                throw new ArgumentNullException("The given ChargingStationGroupId1 must not be null!");

            return ChargingStationGroupId1.CompareTo(ChargingStationGroupId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A ChargingStationGroupId.</param>
        /// <param name="ChargingStationGroupId2">Another ChargingStationGroupId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {
            return !(ChargingStationGroupId1 > ChargingStationGroupId2);
        }

        #endregion

        #region Operator >  (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A ChargingStationGroupId.</param>
        /// <param name="ChargingStationGroupId2">Another ChargingStationGroupId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {

            if ((Object) ChargingStationGroupId1 == null)
                throw new ArgumentNullException("The given ChargingStationGroupId1 must not be null!");

            return ChargingStationGroupId1.CompareTo(ChargingStationGroupId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A ChargingStationGroupId.</param>
        /// <param name="ChargingStationGroupId2">Another ChargingStationGroupId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {
            return !(ChargingStationGroupId1 < ChargingStationGroupId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingGroup_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingStationGroupId.
            var ChargingStationGroupId = Object as ChargingStationGroup_Id;
            if ((Object) ChargingStationGroupId == null)
                throw new ArgumentException("The given object is not a ChargingStationGroupId!");

            return CompareTo(ChargingStationGroupId);

        }

        #endregion

        #region CompareTo(ChargingStationGroupId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationGroup_Id ChargingStationGroupId)
        {

            if ((Object) ChargingStationGroupId == null)
                throw new ArgumentNullException("The given ChargingStationGroupId must not be null!");

            // Compare the length of the ChargingStationIds
            var _Result = this.Length.CompareTo(ChargingStationGroupId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = _OperatorId.CompareTo(ChargingStationGroupId._OperatorId);

            // If equal: Compare ChargingStationId suffix
            if (_Result == 0)
                _Result = _Suffix.CompareTo(ChargingStationGroupId._Suffix);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingGroup_Id> Members

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

            // Check if the given object is an ChargingStationGroupId.
            var ChargingStationGroupId = Object as ChargingStationGroup_Id;
            if ((Object) ChargingStationGroupId == null)
                return false;

            return this.Equals(ChargingStationGroupId);

        }

        #endregion

        #region Equals(ChargingStationGroupId)

        /// <summary>
        /// Compares two ChargingStationGroupIds for equality.
        /// </summary>
        /// <param name="ChargingStationGroupId">A ChargingStationGroupId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationGroup_Id ChargingStationGroupId)
        {

            if ((Object) ChargingStationGroupId == null)
                return false;

            return _OperatorId.Equals(ChargingStationGroupId._OperatorId) &&
                   _Suffix.  Equals(ChargingStationGroupId._Suffix);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_OperatorId, "*G", _Suffix);
        }

        #endregion

    }

}
