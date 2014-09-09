/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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

#endregion

namespace com.graphdefined.eMI3
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Supply Equipment (EVSE_Id)
    /// </summary>
    public class EVSE_Id : IId,
                           IEquatable<EVSE_Id>,
                           IComparable<EVSE_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// </summary>
        public  const    String             EVSEId_RegEx        = @"^([A-Z]{2}\*?[A-Z0-9]{3})\*?E([A-Z0-9][A-Z0-9\*]{0,30})$ | ^(\+?[0-9]{1,3}\*?[0-9]{3})\*?([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing an EVSE identification.
        /// </summary>
        public  const    String             EVSEIdSuffix_RegEx  = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly EVSEOperator_Id  _OperatorId;

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String           _EVSEIdSuffix;

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
                return _OperatorId.Length + (UInt64) _EVSEIdSuffix.Length;
            }
        }

        #endregion

        #region OldEVSEId

        public String OldEVSEId
        {
            get
            {
                return String.Concat(_OperatorId.Id2, "*", _EVSEIdSuffix);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification (EVSE_Id)
        /// based on the given string.
        /// </summary>
        public EVSE_Id(EVSEOperator_Id  OperatorId,
                       String           EVSEIdSuffix)
        {

            if (OperatorId == null)
                throw new ArgumentNullException("OperatorId", "The OperatorId must not be null!");

            var _MatchCollection = Regex.Matches(EVSEIdSuffix.Trim().ToUpper(),
                                                 EVSEIdSuffix_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification!", "EVSEIdSuffix");

            _OperatorId    = OperatorId;
            _EVSEIdSuffix  = _MatchCollection[0].Value;

        }

        #endregion


        #region Parse(EVSEId)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static EVSE_Id Parse(String EVSEId)
        {

            var _MatchCollection = Regex.Matches(EVSEId.Trim().ToUpper(),
                                                 EVSEId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification!", "EVSEId");

            EVSEOperator_Id __EVSEOperatorId = null;

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new EVSE_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[2].Value);

            if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new EVSE_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[4].Value);

            throw new ArgumentException("Illegal EVSE identification!", "EVSEId");

        }

        #endregion

        #region TryParse(EVSEId, out EVSE_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Boolean TryParse(String EVSEId, out EVSE_Id EVSE_Id)
        {

            try
            {

                var _MatchCollection = Regex.Matches(EVSEId.Trim().ToUpper(),
                                                     EVSEId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    throw new ArgumentException("Illegal EVSE identification!", "EVSEId");

                EVSEOperator_Id __EVSEOperatorId = null;

                // New format...
                if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    EVSE_Id = new EVSE_Id(__EVSEOperatorId,
                                          _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (EVSEOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    EVSE_Id = new EVSE_Id(__EVSEOperatorId,
                                          _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            EVSE_Id = null;
            return false;

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
                return new EVSE_Id(_OperatorId,
                                   new String(_EVSEIdSuffix.ToCharArray()));
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSE_Id1, EVSE_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSE_Id1 == null) || ((Object) EVSE_Id2 == null))
                return false;

            return EVSE_Id1.Equals(EVSE_Id2);

        }

        #endregion

        #region Operator != (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {
            return !(EVSE_Id1 == EVSE_Id2);
        }

        #endregion

        #region Operator <  (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {

            if ((Object) EVSE_Id1 == null)
                throw new ArgumentNullException("The given EVSE_Id1 must not be null!");

            return EVSE_Id1.CompareTo(EVSE_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {
            return !(EVSE_Id1 > EVSE_Id2);
        }

        #endregion

        #region Operator >  (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {

            if ((Object) EVSE_Id1 == null)
                throw new ArgumentNullException("The given EVSE_Id1 must not be null!");

            return EVSE_Id1.CompareTo(EVSE_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSE_Id1, EVSE_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id1">A EVSE_Id.</param>
        /// <param name="EVSE_Id2">Another EVSE_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSE_Id1, EVSE_Id EVSE_Id2)
        {
            return !(EVSE_Id1 < EVSE_Id2);
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

            // Check if the given object is an EVSE_Id.
            var EVSE_Id = Object as EVSE_Id;
            if ((Object) EVSE_Id == null)
                throw new ArgumentException("The given object is not a EVSE_Id!");

            return CompareTo(EVSE_Id);

        }

        #endregion

        #region CompareTo(EVSE_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Id">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSE_Id)
        {

            if ((Object) EVSE_Id == null)
                throw new ArgumentNullException("The given EVSE_Id must not be null!");

            // Compare the length of the EVSE_Ids
            var _Result = this.Length.CompareTo(EVSE_Id.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = _OperatorId.CompareTo(EVSE_Id._OperatorId);

            // If equal: Compare EVSE Id suffix
            if (_Result == 0)
                _Result = _EVSEIdSuffix.CompareTo(EVSE_Id._EVSEIdSuffix);

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

            // Check if the given object is an EVSE_Id.
            var EVSE_Id = Object as EVSE_Id;
            if ((Object) EVSE_Id == null)
                return false;

            return this.Equals(EVSE_Id);

        }

        #endregion

        #region Equals(EVSE_Id)

        /// <summary>
        /// Compares two EVSE_Ids for equality.
        /// </summary>
        /// <param name="EVSE_Id">A EVSE_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSE_Id)
        {

            if ((Object) EVSE_Id == null)
                return false;

            return _OperatorId.  Equals(EVSE_Id._OperatorId) &&
                   _EVSEIdSuffix.Equals(EVSE_Id._EVSEIdSuffix);

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
            return _OperatorId.GetHashCode() ^ _EVSEIdSuffix.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_OperatorId.ToString(), "*E", _EVSEIdSuffix);
        }

        #endregion

    }

}
