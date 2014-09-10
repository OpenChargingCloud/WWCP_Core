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
        private EVSE_Id(EVSEOperator_Id  OperatorId,
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

        #region Parse(OperatorId, EVSEIdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static EVSE_Id Parse(EVSEOperator_Id OperatorId, String EVSEIdSuffix)
        {
            return new EVSE_Id(OperatorId, EVSEIdSuffix);
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

        #region TryParse(EVSEId, out EVSE_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Boolean TryParse(EVSEOperator_Id OperatorId, String EVSEIdSuffix, out EVSE_Id EVSE_Id)
        {

            try
            {
                EVSE_Id = new EVSE_Id(OperatorId, EVSEIdSuffix);
                return true;
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
                _Result = _EVSEIdSuffix.CompareTo(EVSEId._EVSEIdSuffix);

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
        /// Compares two EVSE_Ids for equality.
        /// </summary>
        /// <param name="EVSEId">A EVSE_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                return false;

            return _OperatorId.  Equals(EVSEId._OperatorId) &&
                   _EVSEIdSuffix.Equals(EVSEId._EVSEIdSuffix);

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
