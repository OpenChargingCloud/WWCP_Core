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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Mobility Account (driver contract) (eMAId).
    /// </summary>
    public class eMA_Id : IEquatable<eMA_Id>, IComparable<eMA_Id>, IComparable
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String _Id;

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an EV Service Provider identification.
        /// The ISO format onyl allows '-' as a separator!
        /// </summary>
        public const String eMAId_RegEx = @"^(([A-Z]{2})([\*|\-]?)([A-Z0-9]{3}))([\*|\-]?)([A-Za-z0-9]{6,9})([\*|\-]?)([\d|X])$";
        // ([A-Za-z]{2}    \- ?[A-Za-z0-9]{3}    \- ?C[A-Za-z0-9]{8}[\*|\-]?[\d|X])  ISO
        // ([A-Za-z]{2}[\*|\-]?[A-Za-z0-9]{3}[\*|\-]? [A-Za-z0-9]{6}[\*|\-]?[\d|X])  DIN

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
                return (UInt64) _Id.Length;
            }
        }

        #endregion

        #region ProviderId

        private readonly EVSP_Id _ProviderId;

        /// <summary>
        /// The EVSP Id format.
        /// </summary>
        public EVSP_Id ProviderId
        {
            get
            {
                return _ProviderId;
            }
        }

        #endregion

        #region IdFormat

        /// <summary>
        /// The EVSP Id format.
        /// </summary>
        public ProviderIdFormats IdFormat
        {
            get
            {
                return _ProviderId.IdFormat;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Mobility Account (driver contract) identification (eMA_Id)
        /// based on the given string.
        /// </summary>
        private eMA_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Mobility Account (driver contract) (eMA_Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Mobility Account (driver contract) identification.</param>
        public static eMA_Id Parse(String Text)
        {
            return new eMA_Id(Text);
        }

        #endregion

        #region TryParse(Text, out eMAId)

        /// <summary>
        /// Parse the given string as an Electric Mobility Account (driver contract) (eMA_Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Mobility Account (driver contract) identification.</param>
        /// <param name="eMAId">The parsed Electric Mobility Account (driver contract) identification.</param>
        public static Boolean TryParse(String Text, out eMA_Id eMAId)
        {
            try
            {
                eMAId = new eMA_Id(Text);
                return true;
            }
            catch (Exception)
            {
                eMAId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Mobility Account (driver contract) identification.
        /// </summary>
        public eMA_Id Clone
        {
            get
            {
                return new eMA_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (eMA_Id1, eMA_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMA_Id1">A eMA_Id.</param>
        /// <param name="eMA_Id2">Another eMA_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (eMA_Id eMA_Id1, eMA_Id eMA_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(eMA_Id1, eMA_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMA_Id1 == null) || ((Object) eMA_Id2 == null))
                return false;

            return eMA_Id1.Equals(eMA_Id2);

        }

        #endregion

        #region Operator != (eMA_Id1, eMA_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMA_Id1">A eMA_Id.</param>
        /// <param name="eMA_Id2">Another eMA_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eMA_Id eMA_Id1, eMA_Id eMA_Id2)
        {
            return !(eMA_Id1 == eMA_Id2);
        }

        #endregion

        #region Operator <  (eMA_Id1, eMA_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMA_Id1">A eMA_Id.</param>
        /// <param name="eMA_Id2">Another eMA_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eMA_Id eMA_Id1, eMA_Id eMA_Id2)
        {

            if ((Object) eMA_Id1 == null)
                throw new ArgumentNullException("The given eMA_Id1 must not be null!");

            return eMA_Id1.CompareTo(eMA_Id2) < 0;

        }

        #endregion

        #region Operator <= (eMA_Id1, eMA_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMA_Id1">A eMA_Id.</param>
        /// <param name="eMA_Id2">Another eMA_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eMA_Id eMA_Id1, eMA_Id eMA_Id2)
        {
            return !(eMA_Id1 > eMA_Id2);
        }

        #endregion

        #region Operator >  (eMA_Id1, eMA_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMA_Id1">A eMA_Id.</param>
        /// <param name="eMA_Id2">Another eMA_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eMA_Id eMA_Id1, eMA_Id eMA_Id2)
        {

            if ((Object) eMA_Id1 == null)
                throw new ArgumentNullException("The given eMA_Id1 must not be null!");

            return eMA_Id1.CompareTo(eMA_Id2) > 0;

        }

        #endregion

        #region Operator >= (eMA_Id1, eMA_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMA_Id1">A eMA_Id.</param>
        /// <param name="eMA_Id2">Another eMA_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eMA_Id eMA_Id1, eMA_Id eMA_Id2)
        {
            return !(eMA_Id1 < eMA_Id2);
        }

        #endregion

        #endregion

        #region IComparable<eMA_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eMAId.
            var eMAId = Object as eMA_Id;
            if ((Object) eMAId == null)
                throw new ArgumentException("The given object is not a eMAId!");

            return CompareTo(eMAId);

        }

        #endregion

        #region CompareTo(eMAId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAId">An object to compare with.</param>
        public Int32 CompareTo(eMA_Id eMAId)
        {

            if ((Object) eMAId == null)
                throw new ArgumentNullException("The given eMAId must not be null!");

            // Compare the length of the eMAIds
            var _Result = this.Length.CompareTo(eMAId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(eMAId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMA_Id> Members

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

            // Check if the given object is an eMAId.
            var eMAId = Object as eMA_Id;
            if ((Object) eMAId == null)
                return false;

            return this.Equals(eMAId);

        }

        #endregion

        #region Equals(eMAId)

        /// <summary>
        /// Compares two eMAIds for equality.
        /// </summary>
        /// <param name="eMAId">A eMAId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMA_Id eMAId)
        {

            if ((Object) eMAId == null)
                return false;

            return _Id.Equals(eMAId._Id);

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
            return _Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
