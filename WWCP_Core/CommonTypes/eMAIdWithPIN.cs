/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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

using org.GraphDefined.Vanaheimr.Illias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP
{

    public enum PINCrypto
    {
        none,
        MD5,
        SHA1
    }


    public class eMAIdWithPIN2 : IEquatable<eMAIdWithPIN2>,
                                 IComparable<eMAIdWithPIN2>
    {

        #region Properties

        public eMobilityAccount_Id  eMAId           { get; }

        public String               PIN             { get; }

        public PINCrypto            Function        { get; }

        public String               Salt            { get; }

        #endregion

        #region Constructor(s)

        #region eMAIdWithPIN(eMAId, PIN)

        public eMAIdWithPIN2(eMobilityAccount_Id  eMAId,
                             String               PIN)
        {

            this.eMAId     = eMAId;
            this.PIN       = PIN;
            this.Function  = PINCrypto.none;

        }

        #endregion

        #region eMAIdWithPIN(eMAId, PIN, Function, Salt = "")

        public eMAIdWithPIN2(eMobilityAccount_Id  eMAId,
                             String               PIN,
                             PINCrypto            Function,
                             String               Salt = "")
        {

            this.eMAId     = eMAId;
            this.PIN       = PIN;
            this.Function  = Function;
            this.Salt      = Salt;

        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">A eMAIdWithPIN2.</param>
        /// <param name="eMAIdWithPIN22">Another eMAIdWithPIN2.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (eMAIdWithPIN2 eMAIdWithPIN21, eMAIdWithPIN2 eMAIdWithPIN22)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(eMAIdWithPIN21, eMAIdWithPIN22))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMAIdWithPIN21 == null) || ((Object) eMAIdWithPIN22 == null))
                return false;

            return eMAIdWithPIN21.Equals(eMAIdWithPIN22);

        }

        #endregion

        #region Operator != (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">A eMAIdWithPIN2.</param>
        /// <param name="eMAIdWithPIN22">Another eMAIdWithPIN2.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eMAIdWithPIN2 eMAIdWithPIN21, eMAIdWithPIN2 eMAIdWithPIN22)
        {
            return !(eMAIdWithPIN21 == eMAIdWithPIN22);
        }

        #endregion

        #region Operator <  (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">A eMAIdWithPIN2.</param>
        /// <param name="eMAIdWithPIN22">Another eMAIdWithPIN2.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eMAIdWithPIN2 eMAIdWithPIN21, eMAIdWithPIN2 eMAIdWithPIN22)
        {

            if ((Object) eMAIdWithPIN21 == null)
                throw new ArgumentNullException("The given eMAIdWithPIN21 must not be null!");

            return eMAIdWithPIN21.CompareTo(eMAIdWithPIN22) < 0;

        }

        #endregion

        #region Operator <= (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">A eMAIdWithPIN2.</param>
        /// <param name="eMAIdWithPIN22">Another eMAIdWithPIN2.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eMAIdWithPIN2 eMAIdWithPIN21, eMAIdWithPIN2 eMAIdWithPIN22)
        {
            return !(eMAIdWithPIN21 > eMAIdWithPIN22);
        }

        #endregion

        #region Operator >  (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">A eMAIdWithPIN2.</param>
        /// <param name="eMAIdWithPIN22">Another eMAIdWithPIN2.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eMAIdWithPIN2 eMAIdWithPIN21, eMAIdWithPIN2 eMAIdWithPIN22)
        {

            if ((Object) eMAIdWithPIN21 == null)
                throw new ArgumentNullException("The given eMAIdWithPIN21 must not be null!");

            return eMAIdWithPIN21.CompareTo(eMAIdWithPIN22) > 0;

        }

        #endregion

        #region Operator >= (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">A eMAIdWithPIN2.</param>
        /// <param name="eMAIdWithPIN22">Another eMAIdWithPIN2.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eMAIdWithPIN2 eMAIdWithPIN21, eMAIdWithPIN2 eMAIdWithPIN22)
        {
            return !(eMAIdWithPIN21 < eMAIdWithPIN22);
        }

        #endregion

        #endregion

        #region IComparable<eMAIdWithPIN2> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eMAIdWithPIN2.
            var eMAIdWithPIN2 = Object as eMAIdWithPIN2;
            if ((Object) eMAIdWithPIN2 == null)
                throw new ArgumentException("The given object is not a eMAIdWithPIN2!");

            return CompareTo(eMAIdWithPIN2);

        }

        #endregion

        #region CompareTo(eMAIdWithPIN2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN2">An object to compare with.</param>
        public Int32 CompareTo(eMAIdWithPIN2 eMAIdWithPIN2)
        {

            if ((Object) eMAIdWithPIN2 == null)
                throw new ArgumentNullException("The given eMAIdWithPIN2 must not be null!");

            // Compare EVSE Ids
            var _Result = eMAId.CompareTo(eMAIdWithPIN2.eMAId);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = PIN.CompareTo(eMAIdWithPIN2.PIN);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMAIdWithPIN2> Members

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

            // Check if the given object is an eMAIdWithPIN2.
            var eMAIdWithPIN2 = Object as eMAIdWithPIN2;
            if ((Object) eMAIdWithPIN2 == null)
                return false;

            return this.Equals(eMAIdWithPIN2);

        }

        #endregion

        #region Equals(eMAIdWithPIN2)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="eMAIdWithPIN2">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMAIdWithPIN2 eMAIdWithPIN2)
        {

            if ((Object) eMAIdWithPIN2 == null)
                return false;

            return eMAId.Equals(eMAIdWithPIN2.eMAId) &&
                   PIN.  Equals(eMAIdWithPIN2.PIN);

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
            unchecked
            {
                return eMAId.GetHashCode() * 17 ^ PIN.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(eMAId.ToString(), " -", Function != PINCrypto.none ? Function.ToString(): "", "-> ", PIN );
        }

        #endregion

    }

}
