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
    /// The unique identification of an Electric Vehicle Charging Pool (EVCP Id).
    /// </summary>
    public class ChargingPool_Id : IId,
                                   IEquatable<ChargingPool_Id>,
                                   IComparable<ChargingPool_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging pool identification.
        /// </summary>
        public    const    String  ChargingPoolId_RegEx  = @"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?P([A-Z0-9][A-Z0-9\*]{0,30})$ | ^(\+?[0-9]{1,3}\*?[0-9]{3})\*?([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing an charging pool identification.
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
        /// Generate a new Electric Vehicle Charging Pool identification (EVCP Id)
        /// based on the given string.
        /// </summary>
        private ChargingPool_Id(EVSEOperator_Id   OperatorId,
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
                throw new ArgumentException("Illegal charging pool identification suffix '" + Suffix + "'!", "IdSuffix");

            this._OperatorId  = OperatorId;
            this._Suffix      = _MatchCollection[0].Value;
            this._Format      = Format;

        }

        #endregion


        #region Random(OperatorId, Mapper = null, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle Charging Station (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        /// <param name="IdFormat">The (EVSE-)format of the charging station identification [old|new].</param>
        public static ChargingPool_Id Random(EVSEOperator_Id       OperatorId,
                                             Func<String, String>  Mapper    = null,
                                             IdFormatType          IdFormat  = IdFormatType.NEW)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentException("The parameter must not be null!", "OperatorId");

            #endregion

            return new ChargingPool_Id(OperatorId,
                                       Mapper != null ? Mapper(_Random.RandomString(12)) : _Random.RandomString(12),
                                       IdFormat);

        }

        #endregion

        #region Generate(EVSEOperatorId, Address, GeoLocation, Lenght = 12, Mapper = null)

        /// <summary>
        /// Create a valid charging pool identification based on the given parameters.
        /// </summary>
        /// <param name="EVSEOperatorId">The identification of an EVSE operator.</param>
        /// <param name="Address">The address of the charging pool.</param>
        /// <param name="GeoLocation">The geo location of the charging pool.</param>
        /// <param name="Length">The maximum size of the generated charging pool identification suffix.</param>
        /// <param name="Mapper">A delegate to modify a generated charging pool identification suffix.</param>
        public static ChargingPool_Id Generate(EVSEOperator_Id       EVSEOperatorId,
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

            return ChargingPool_Id.Parse(EVSEOperatorId, Mapper != null ? Mapper(Suffíx) : Suffíx);

        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging pool identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "Text");

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 ChargingPoolId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging pool identification '" + Text + "'!", "Text");

            EVSEOperator_Id __EVSEOperatorId = null;

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new ChargingPool_Id(__EVSEOperatorId,
                                           _MatchCollection[0].Groups[2].Value,
                                           IdFormatType.NEW);

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new ChargingPool_Id(__EVSEOperatorId,
                                           _MatchCollection[0].Groups[4].Value,
                                           IdFormatType.OLD);


            throw new ArgumentException("Illegal charging pool identification '" + Text + "'!", "Text");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a charging pool identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="IdSuffix">A text representation of a charging pool identification.</param>
        public static ChargingPool_Id Parse(EVSEOperator_Id OperatorId, String IdSuffix)
        {

            ChargingPool_Id _ChargingPoolId = null;

            if (ChargingPool_Id.TryParse(OperatorId, IdSuffix, out _ChargingPoolId))
                return _ChargingPoolId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as a charging pool identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(String Text, out ChargingPool_Id ChargingPoolId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingPoolId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingPoolId = null;

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     ChargingPoolId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                EVSEOperator_Id __EVSEOperatorId = null;

                // New format...
                if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    ChargingPoolId = new ChargingPool_Id(__EVSEOperatorId,
                                                         _MatchCollection[0].Groups[2].Value,
                                                         IdFormatType.NEW);

                    return true;

                }

                // Old format...
                else if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    ChargingPoolId = new ChargingPool_Id(__EVSEOperatorId,
                                                         _MatchCollection[0].Groups[4].Value,
                                                         IdFormatType.OLD);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ChargingPoolId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as a charging pool identification (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an EVSE operator.</param>
        /// <param name="IdSuffix">A text representation of a charging pool identification.</param>
        /// <param name="ChargingPoolId">The parsed charging pool identification.</param>
        public static Boolean TryParse(EVSEOperator_Id      OperatorId,
                                       String               IdSuffix,
                                       out ChargingPool_Id  ChargingPoolId)
        {

            #region Initial checks

            if (OperatorId == null || IdSuffix.IsNullOrEmpty())
            {
                ChargingPoolId = null;
                return false;
            }

            #endregion

            try
            {

                ChargingPoolId = null;

                var _MatchCollection = Regex.Matches(IdSuffix.Trim().ToUpper(),
                                                     IdSuffix_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingPoolId = new ChargingPool_Id(OperatorId,
                                                     _MatchCollection[0].Groups[0].Value,
                                                     OperatorId.Format);

                return true;

            }
            catch (Exception e)
            { }

            ChargingPoolId = null;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public ChargingPool_Id Clone
        {
            get
            {
                return new ChargingPool_Id(OperatorId.Clone,
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

        #region Operator == (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingPoolId1, ChargingPoolId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolId1 == null) || ((Object) ChargingPoolId2 == null))
                return false;

            return ChargingPoolId1.Equals(ChargingPoolId2);

        }

        #endregion

        #region Operator != (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {
            return !(ChargingPoolId1 == ChargingPoolId2);
        }

        #endregion

        #region Operator <  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException("The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {
            return !(ChargingPoolId1 > ChargingPoolId2);
        }

        #endregion

        #region Operator >  (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {

            if ((Object) ChargingPoolId1 == null)
                throw new ArgumentNullException("The given ChargingPoolId1 must not be null!");

            return ChargingPoolId1.CompareTo(ChargingPoolId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolId1, ChargingPoolId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId1">A ChargingPoolId.</param>
        /// <param name="ChargingPoolId2">Another ChargingPoolId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPool_Id ChargingPoolId1, ChargingPool_Id ChargingPoolId2)
        {
            return !(ChargingPoolId1 < ChargingPoolId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingPool_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingPoolId.
            var ChargingPoolId = Object as ChargingPool_Id;
            if ((Object) ChargingPoolId == null)
                throw new ArgumentException("The given object is not a ChargingPoolId!");

            return CompareTo(ChargingPoolId);

        }

        #endregion

        #region CompareTo(ChargingPoolId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolId">An object to compare with.</param>
        public Int32 CompareTo(ChargingPool_Id ChargingPoolId)
        {

            if ((Object) ChargingPoolId == null)
                throw new ArgumentNullException("The given ChargingPoolId must not be null!");

            // Compare the length of the ChargingStationIds
            var _Result = this.Length.CompareTo(ChargingPoolId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = _OperatorId.CompareTo(ChargingPoolId._OperatorId);

            // If equal: Compare ChargingStationId suffix
            if (_Result == 0)
                _Result = _Suffix.CompareTo(ChargingPoolId._Suffix);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingPool_Id> Members

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

            // Check if the given object is an ChargingPoolId.
            var ChargingPoolId = Object as ChargingPool_Id;
            if ((Object) ChargingPoolId == null)
                return false;

            return this.Equals(ChargingPoolId);

        }

        #endregion

        #region Equals(ChargingPoolId)

        /// <summary>
        /// Compares two ChargingPoolIds for equality.
        /// </summary>
        /// <param name="ChargingPoolId">A ChargingPoolId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPool_Id ChargingPoolId)
        {

            if ((Object) ChargingPoolId == null)
                return false;

            return _OperatorId.Equals(ChargingPoolId._OperatorId) &&
                   _Suffix.  Equals(ChargingPoolId._Suffix);

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
            return String.Concat(_OperatorId, "*P", _Suffix);
        }

        #endregion

    }

}
