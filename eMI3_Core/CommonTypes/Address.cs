/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// An address.
    /// </summary>
    public class Address : IEquatable<Address>
    {

        #region Properties

        /// <summary>
        /// The FloorLevel.
        /// </summary>
        public String    FloorLevel         { get; set; }

        /// <summary>
        /// The HouseNumber.
        /// </summary>
        public String    HouseNumber        { get; set; }

        /// <summary>
        /// The Street.
        /// </summary>
        public String    Street             { get; set; }

        /// <summary>
        /// The PostalCode.
        /// </summary>
        public String    PostalCode         { get; set; }

        /// <summary>
        /// The PostalCodeSub.
        /// </summary>
        public String    PostalCodeSub      { get; set; }

        /// <summary>
        /// The City.
        /// </summary>
        public String    City               { get; set; }

        /// <summary>
        /// The Country.
        /// </summary>
        public Country   Country            { get; set; }

        /// <summary>
        /// Additional text to describe the address.
        /// </summary>
        public I8NString FreeText           { get; set; }

        #endregion

        #region Constructor(s)

        #region Address()

        /// <summary>
        /// Generate a new address.
        /// </summary>
        public Address()
        { }

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

            if (Country             == null ||
                City                == null ||
                Street              == null ||
                HouseNumber         == null ||
                Address.Country     == null ||
                Address.City        == null ||
                Address.Street      == null ||
                Address.HouseNumber == null)

                return false;

            return Country.    Equals(Address.Country) &&
                   City.       Equals(Address.City)    &&
                   Street.     Equals(Address.Street)  &&
                   HouseNumber.Equals(Address.HouseNumber);

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

            return Country.    GetHashCode() ^
                   City.       GetHashCode() ^
                   Street.     GetHashCode() ^
                   HouseNumber.GetHashCode();

        }

        #endregion


    }

}
