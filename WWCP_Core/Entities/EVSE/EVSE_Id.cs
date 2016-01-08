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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment.
    /// </summary>
    public class EVSE_Id : IId,
                           IEquatable<EVSE_Id>,
                           IComparable<EVSE_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// </summary>
        public    const    String  EVSEId_RegEx    = @"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?E([A-Z0-9][A-Z0-9\*]{0,30})$ | ^(\+?[0-9]{1,3}\*?[0-9]{3})\*?([A-Z0-9][A-Z0-9\*]{0,29})$";
                                             // Hubject ([A-Za-z]{2}\*?[A-Za-z0-9]{3}\*?E[A-Za-z0-9\*]{1,30})  |  (\+?[0-9]{1,3}\*[0-9]{3,6}\*[0-9\*]{1,32})
                                             // OCHP.eu                                                           /^\+[0-9]{1,3}\*?[A-Z0-9]{3}\*?[A-Z0-9\*]{0,40}(?=$)/i;
                                             // var valid_evse_warning= /^(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9\*]*/; // look ahead: at least one upper and one lower case letter

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// </summary>
        public    const    String  IdSuffix_RegEx  = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

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
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification
        /// based on the given string.
        /// </summary>
        private EVSE_Id(EVSEOperator_Id   OperatorId,
                        String            IdSuffix,
                        IdFormatType      IdFormat = IdFormatType.NEW)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The parameter must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException("IdSuffix", "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = Regex.Matches(IdSuffix.Trim().ToUpper(),
                                                 IdSuffix_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + OperatorId.ToString() + "' with suffix '" + IdSuffix + "'!", "IdSuffix");

            this._OperatorId  = OperatorId;
            this._Suffix      = _MatchCollection[0].Value;
            this._Format      = IdFormat;

        }

        #endregion


        #region Parse(EVSEId)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static EVSE_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "Text");

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 EVSEId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!", "EVSEId");

            EVSEOperator_Id __EVSEOperatorId = null;

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new EVSE_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[2].Value,
                                   IdFormatType.NEW);

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new EVSE_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[4].Value,
                                   IdFormatType.OLD);


            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!", "EVSEId");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static EVSE_Id Parse(EVSEOperator_Id OperatorId, String IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentException("The parameter must not be null or empty!", "OperatorId");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "IdSuffix");

            #endregion

            return EVSE_Id.Parse(OperatorId.ToString() + "*" + IdSuffix);

        }

        #endregion

        #region TryParse(Text, out EVSE_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Boolean TryParse(String Text, out EVSE_Id EVSEId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                EVSEId = null;
                return false;
            }

            #endregion

            try
            {

                EVSEId = null;

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     EVSEId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                EVSEOperator_Id __EVSEOperatorId = null;

                // New format...
                if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    EVSEId = new EVSE_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value,
                                         IdFormatType.NEW);

                    return true;

                }

                // Old format...
                else if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    EVSEId = new EVSE_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value,
                                         IdFormatType.OLD);

                    return true;

                }

            }
            catch (Exception e)
            { }

            EVSEId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out EVSEId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out EVSE_Id EVSE_Id)
        //{

        //    try
        //    {
        //        EVSE_Id = new EVSE_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    EVSE_Id = null;
        //    return false;

        //}

        #endregion

        #region ChangeFormat

        /// <summary>
        /// Return a new EVSE identification in the given format.
        /// </summary>
        /// <param name="Format">An EVSE identification format.</param>
        public EVSE_Id ChangeFormat(IdFormatType Format)
        {
            return new EVSE_Id(this._OperatorId.ChangeFormat(Format), this._Suffix, Format);
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSE_Id.
        /// </summary>
        public EVSE_Id Clone
        {
            get
            {
                return new EVSE_Id(_OperatorId.Clone,
                                   new String(_Suffix.ToCharArray()),
                                   _Format);
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
                       ? String.Concat(_OperatorId.ToFormat(IdFormat), "*E", _Suffix)
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

        #region Operator == (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE_Id.</param>
        /// <param name="EVSEId2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEId1, EVSEId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEId1 == null) || ((Object) EVSEId2 == null))
                return false;

            return EVSEId1.Equals(EVSEId2);

        }

        #endregion

        #region Operator != (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE_Id.</param>
        /// <param name="EVSEId2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {
            return !(EVSEId1 == EVSEId2);
        }

        #endregion

        #region Operator <  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE_Id.</param>
        /// <param name="EVSEId2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException("The given EVSEId1 must not be null!");

            return EVSEId1.CompareTo(EVSEId2) < 0;

        }

        #endregion

        #region Operator <= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE_Id.</param>
        /// <param name="EVSEId2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {
            return !(EVSEId1 > EVSEId2);
        }

        #endregion

        #region Operator >  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE_Id.</param>
        /// <param name="EVSEId2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException("The given EVSEId1 must not be null!");

            return EVSEId1.CompareTo(EVSEId2) > 0;

        }

        #endregion

        #region Operator >= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A EVSE_Id.</param>
        /// <param name="EVSEId2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {
            return !(EVSEId1 < EVSEId2);
        }

        #endregion

        #endregion

        #region IComparable<EVSE_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSEId.
            var EVSEId = Object as EVSE_Id;
            if ((Object) EVSEId == null)
                throw new ArgumentException("The given object is not a EVSEId!");

            return CompareTo(EVSEId);

        }

        #endregion

        #region CompareTo(EVSEId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                throw new ArgumentNullException("The given EVSEId must not be null!");

            // Compare the length of the EVSEIds
            var _Result = this.Length.CompareTo(EVSEId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = _OperatorId.CompareTo(EVSEId._OperatorId);

            // If equal: Compare EVSEId suffix
            if (_Result == 0)
                _Result = _Suffix.CompareTo(EVSEId._Suffix);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSE_Id> Members

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

            // Check if the given object is an EVSEId.
            var EVSEId = Object as EVSE_Id;
            if ((Object) EVSEId == null)
                return false;

            return this.Equals(EVSEId);

        }

        #endregion

        #region Equals(EVSEId)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                return false;

            return _OperatorId.Equals(EVSEId._OperatorId) &&
                   _Suffix.  Equals(EVSEId._Suffix);

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
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {

            return Format == IdFormatType.NEW
                       ? String.Concat(_OperatorId, "*E", _Suffix)
                       : String.Concat(_OperatorId, "*",  _Suffix);

        }

        #endregion

    }

}
