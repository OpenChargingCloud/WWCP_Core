/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An address.
    /// </summary>
    public class Address : IEquatable<Address>
    {

        #region Properties

        #region Street

        private readonly String _Street;

        /// <summary>
        /// The name of the street.
        /// </summary>
        public String Street
        {
            get
            {
                return _Street;
            }
        }

        #endregion

        #region HouseNumber

        private readonly String _HouseNumber;

        /// <summary>
        /// The house number.
        /// </summary>
        public String HouseNumber
        {
            get
            {
                return _HouseNumber;
            }
        }

        #endregion

        #region FloorLevel

        private readonly String _FloorLevel;

        /// <summary>
        /// The floor level.
        /// </summary>
        public String FloorLevel
        {
            get
            {
                return _FloorLevel;
            }
        }

        #endregion

        #region PostalCode

        private readonly String _PostalCode;

        /// <summary>
        /// The postal code.
        /// </summary>
        public String PostalCode
        {
            get
            {
                return _PostalCode;
            }
        }

        #endregion

        #region PostalCodeSub

        private readonly String _PostalCodeSub;

        /// <summary>
        /// The postal code sub.
        /// </summary>
        public String PostalCodeSub
        {
            get
            {
                return _PostalCodeSub;
            }
        }

        #endregion

        #region City

        private readonly String _City;

        /// <summary>
        /// The city.
        /// </summary>
        public String City
        {
            get
            {
                return _City;
            }
        }

        #endregion

        #region Country

        private readonly Country _Country;

        /// <summary>
        /// The city.
        /// </summary>
        public Country Country
        {
            get
            {
                return _Country;
            }
        }

        #endregion

        #region Comment

        private readonly I18NString _Comment;

        /// <summary>
        /// An optional text/comment to describe the address.
        /// </summary>
        public I18NString Comment
        {
            get
            {
                return _Comment;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region Address()

        /// <summary>
        /// Create a new address.
        /// </summary>
        public Address()
        {

            this._FloorLevel     = "";
            this._HouseNumber    = "";
            this._Street         = "";
            this._PostalCode     = "";
            this._PostalCodeSub  = "";
            this._City           = "";
            this._Country        = Country.unknown;
            this._Comment        = new I18NString();

        }

        #endregion

        #region Address(Street, HouseNumber, FloorLevel, PostalCode, PostalCodeSub, City, Country, FreeText = null)

        /// <summary>
        /// Create a new address.
        /// </summary>
        /// <param name="Street">The name of the street.</param>
        /// <param name="HouseNumber">The house number.</param>
        /// <param name="FloorLevel">The floor level.</param>
        /// <param name="PostalCode">The postal code</param>
        /// <param name="PostalCodeSub">The postal code sub</param>
        /// <param name="City">The city.</param>
        /// <param name="Country">The country.</param>
        /// <param name="Comment">An optional text/comment to describe the address.</param>
        public Address(String      Street,
                       String      HouseNumber,
                       String      FloorLevel,
                       String      PostalCode,
                       String      PostalCodeSub,
                       String      City,
                       Country     Country,
                       I18NString  Comment = null)

        {

            this._Street         = Street;
            this._HouseNumber    = HouseNumber;
            this._FloorLevel     = FloorLevel;
            this._PostalCode     = PostalCode;
            this._PostalCodeSub  = PostalCodeSub;
            this._City           = City;
            this._Country        = Country;
            this._Comment        = Comment != null ? Comment : new I18NString();

        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (Address1, Address2)

        /// <summary>
        /// Compares two addresses for equality.
        /// </summary>
        /// <param name="Address1">A geo coordinate.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Address Address1, Address Address2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Address1, Address2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Address1 == null) || ((Object) Address2 == null))
                return false;

            return Address1.Equals(Address2);

        }

        #endregion

        #region Operator != (Address1, Address2)

        /// <summary>
        /// Compares two addresses for inequality.
        /// </summary>
        /// <param name="Address1">A geo coordinate.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Address Address1, Address Address2)
        {
            return !(Address1 == Address2);
        }

        #endregion

        #endregion

        #region IEquatable<Address> Members

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

            // Check if the given object is an Address.
            var Address = Object as Address;
            if ((Object) Address == null)
                return false;

            return this.Equals(Address);

        }

        #endregion

        #region Equals(Address)

        /// <summary>
        /// Compares two EVSE_Ids for equality.
        /// </summary>
        /// <param name="Address">An address to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Address Address)
        {

            if ((Object) Address == null)
                return false;

            return _Street.        Equals(Address.Street) &&
                   _HouseNumber.   Equals(Address.HouseNumber) &&
                   _FloorLevel.    Equals(Address.FloorLevel) &&
                   _PostalCode.    Equals(Address.PostalCode) &&
                   _PostalCodeSub. Equals(Address.PostalCodeSub) &&
                   _City.          Equals(Address.City) &&
                   _Country.       Equals(Address.Country);

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

            return _Street.       GetHashCode() ^
                   _HouseNumber.  GetHashCode() ^
                   _FloorLevel.   GetHashCode() ^
                   _PostalCode.   GetHashCode() ^
                   _PostalCodeSub.GetHashCode() ^
                   _City.         GetHashCode() ^
                   _Country.      GetHashCode();

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            return Street                         + " " +
                   HouseNumber                    + " " +
                   FloorLevel                     + ", " +
                   PostalCode                     + " " +
                   PostalCodeSub                  + " " +
                   City                           + ", " +
                   Country.CountryName.ToString() + " / " +
                   Comment.ToString();

        }

        #endregion

    }

}
