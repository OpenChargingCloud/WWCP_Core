/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
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

namespace com.graphdefined.eMI3
{

    /// <summary>
    /// An address.
    /// </summary>
    public class Address
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

    }

}
