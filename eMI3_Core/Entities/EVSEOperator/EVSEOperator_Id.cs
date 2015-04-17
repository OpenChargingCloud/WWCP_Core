﻿/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment Operator (EVSE Op).
    /// </summary>
    public class EVSEOperator_Id : IId,
                                   IEquatable<EVSEOperator_Id>,
                                   IComparable<EVSEOperator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE Operator identification.
        /// </summary>
        public  const    String   OperatorId_RegEx            = @"^[A-Z0-9]{3}$";

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an EVSE Operator identification.
        /// </summary>
        public  const    String   CountryAndOperatorId_RegEx  = @"^([A-Z]{2})\*?([A-Z0-9]{3})$ | ^\+?([0-9]{1,5})\*([0-9]{3})$ | ^([0-9]{3})$";

        #endregion

        #region Properties

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return (UInt64) (_CountryCode.Alpha2Code.Length + _OperatorId.Length);
            }
        }

        #endregion

        #region CountryCode

        /// <summary>
        /// The internal Alpha-2-CountryCode.
        /// </summary>
        private readonly Country _CountryCode;

        public Country CountryCode
        {
            get
            {
                return _CountryCode;
            }
        }

        #endregion

        #region OperatorId

        /// <summary>
        /// The internal EVSE Operator identification.
        /// </summary>
        private readonly String _OperatorId;

        public String OperatorId
        {
            get
            {
                return _OperatorId;
            }
        }

        #endregion

        #region Id2

        /// <summary>
        /// The EVSE Operator identification as e.g. "+49*822".
        /// </summary>
        public String Id2
        {
            get
            {
                return String.Concat("+", _CountryCode.TelefonCode, "*", _OperatorId);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSE Op) identification.
        /// </summary>
        /// <param name="CountryCode">The Alpha-2-CountryCode.</param>
        /// <param name="OperatorId">The EVSE Operator identification.</param>
        private EVSEOperator_Id(Country  CountryCode,
                                String   OperatorId)
        {
            _CountryCode  = CountryCode;
            _OperatorId   = OperatorId;
        }

        #endregion


        #region Parse(CountryAndOperatorId)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryAndOperatorId">An EVSE Operator identification as string.</param>
        public static EVSEOperator_Id Parse(String CountryAndOperatorId)
        {


            var _MatchCollection = Regex.Matches(CountryAndOperatorId.Trim().ToUpper(),
                                                 CountryAndOperatorId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE Operator identification!", "OperatorId");

            Country __CountryCode;

            if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
                return new EVSEOperator_Id(__CountryCode,
                                           _MatchCollection[0].Groups[2].Value);

            if (Country.TryParseTelefonCode(_MatchCollection[0].Groups[3].Value, out __CountryCode))
                return new EVSEOperator_Id(__CountryCode,
                                           _MatchCollection[0].Groups[4].Value);

            // Just e.g. "882"...
            return new EVSEOperator_Id(Country.Germany,
                                       _MatchCollection[0].Groups[5].Value);

        }

        #endregion

        #region Parse(CountryCode, OperatorId)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryAndOperatorId">An EVSE Operator identification as string.</param>
        public static EVSEOperator_Id Parse(Country CountryCode, String OperatorId)
        {

            var _MatchCollection = Regex.Matches(OperatorId.Trim().ToUpper(),
                                                 OperatorId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE Operator identification!", "OperatorId");

            return new EVSEOperator_Id(CountryCode, _MatchCollection[0].Value);

        }

        #endregion

        #region TryParse(CountryAndOperatorId, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryAndOperatorId">An EVSE Operator identification as string.</param>
        /// <param name="EVSEOperatorId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(String CountryAndOperatorId, out EVSEOperator_Id EVSEOperatorId)
        {

            try
            {

                var _MatchCollection = Regex.Matches(CountryAndOperatorId.Trim().ToUpper(),
                                                     CountryAndOperatorId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEOperatorId = null;
                    return false;
                }

                Country __CountryCode;

                if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
                {
                    EVSEOperatorId = new EVSEOperator_Id(__CountryCode,
                                                         _MatchCollection[0].Groups[2].Value);
                    return true;
                }

                if (Country.TryParseTelefonCode(_MatchCollection[0].Groups[3].Value, out __CountryCode))
                {
                    EVSEOperatorId = new EVSEOperator_Id(__CountryCode,
                                                         _MatchCollection[0].Groups[4].Value);
                    return true;
                }

                // Just e.g. "882"...
                EVSEOperatorId = new EVSEOperator_Id(Country.Germany,
                                                     _MatchCollection[0].Groups[5].Value);

                return true;

            }


            catch (Exception e)
            {
                EVSEOperatorId = null;
                return false;
            }

        }

        #endregion

        #region TryParse(CountryAndOperatorId, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryAndOperatorId">An EVSE Operator identification as string.</param>
        /// <param name="EVSEOperatorId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(Country CountryCode, String OperatorId, out EVSEOperator_Id EVSEOperatorId)
        {

            try
            {

                var _MatchCollection = Regex.Matches(OperatorId.Trim().ToUpper(),
                                                     OperatorId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEOperatorId = null;
                    return false;
                }

                EVSEOperatorId = new EVSEOperator_Id(CountryCode, _MatchCollection[0].Value);
                return true;

            }

            catch (Exception e)
            {
                EVSEOperatorId = null;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSEOperator_Id.
        /// </summary>
        public EVSEOperator_Id Clone
        {
            get
            {

                return new EVSEOperator_Id(_CountryCode,
                                           new String(_OperatorId.ToCharArray()));

            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEOperator_Id1, EVSEOperator_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEOperator_Id1 == null) || ((Object) EVSEOperator_Id2 == null))
                return false;

            return EVSEOperator_Id1.Equals(EVSEOperator_Id2);

        }

        #endregion

        #region Operator != (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 == EVSEOperator_Id2);
        }

        #endregion

        #region Operator <  (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {

            if ((Object) EVSEOperator_Id1 == null)
                throw new ArgumentNullException("The given EVSEOperator_Id1 must not be null!");

            return EVSEOperator_Id1.CompareTo(EVSEOperator_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 > EVSEOperator_Id2);
        }

        #endregion

        #region Operator >  (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {

            if ((Object) EVSEOperator_Id1 == null)
                throw new ArgumentNullException("The given EVSEOperator_Id1 must not be null!");

            return EVSEOperator_Id1.CompareTo(EVSEOperator_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEOperator_Id EVSEOperator_Id1, EVSEOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 < EVSEOperator_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVSEOperator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSEOperator_Id.
            var EVSEOperator_Id = Object as EVSEOperator_Id;
            if ((Object) EVSEOperator_Id == null)
                throw new ArgumentException("The given object is not a EVSEOperator_Id!");

            return CompareTo(EVSEOperator_Id);

        }

        #endregion

        #region CompareTo(EVSEOperator_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id">An object to compare with.</param>
        public Int32 CompareTo(EVSEOperator_Id EVSEOperator_Id)
        {

            if ((Object) EVSEOperator_Id == null)
                throw new ArgumentNullException("The given EVSEOperator_Id must not be null!");

            // Compare the length of the EVSEOperator_Ids
            var _Result = this.Length.CompareTo(EVSEOperator_Id.Length);

            // If equal: Compare CountryIds
            if (_Result == 0)
                _Result = _CountryCode.CompareTo(EVSEOperator_Id._CountryCode);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = _OperatorId.CompareTo(EVSEOperator_Id._OperatorId);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEOperator_Id> Members

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

            // Check if the given object is an EVSEOperator_Id.
            var EVSEOperator_Id = Object as EVSEOperator_Id;
            if ((Object) EVSEOperator_Id == null)
                return false;

            return this.Equals(EVSEOperator_Id);

        }

        #endregion

        #region Equals(EVSEOperator_Id)

        /// <summary>
        /// Compares two EVSEOperator_Ids for equality.
        /// </summary>
        /// <param name="EVSEOperatorId">A EVSEOperator_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEOperator_Id EVSEOperatorId)
        {

            if ((Object) EVSEOperatorId == null)
                return false;

            return _CountryCode.Equals(EVSEOperatorId._CountryCode) &&
                   _OperatorId. Equals(EVSEOperatorId._OperatorId);

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
            return _CountryCode.Alpha2Code.GetHashCode() ^ _OperatorId.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return _CountryCode.Alpha2Code + "*" + _OperatorId.ToString();
        }

        #endregion

    }

}
