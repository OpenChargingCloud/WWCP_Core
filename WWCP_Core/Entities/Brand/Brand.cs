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
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using System.Threading.Tasks;
using System.Threading;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (Brand) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class Brand : AEMobilityEntity<Brand_Id>,
                         IEquatable<Brand>, IComparable<Brand>, IComparable

    {

        #region Properties

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The multi-language brand name.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {
                return _Name;
            }

            set
            {
                if (_Name != value)
                    SetProperty<I18NString>(ref _Name, value);
            }

        }

        #endregion

        #region LogoURI

        private String _LogoURI;

        /// <summary>
        /// An URI to the logo of this brand.
        /// </summary>
        [Optional]
        public String LogoURI
        {

            get
            {
                return _LogoURI;
            }

            set
            {
                if (_LogoURI != value)
                    SetProperty<String>(ref _LogoURI, value);
            }

        }

        #endregion

        #region Homepage

        private String _Homepage;

        /// <summary>
        /// The homepage of this brand.
        /// </summary>
        [Optional]
        public String Homepage
        {

            get
            {
                return _Homepage;
            }

            set
            {
                if (_Homepage != value)
                    SetProperty<String>(ref _Homepage, value);
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new brand having the given brand identification.
        /// </summary>
        /// <param name="Id">The unique identification of this brand.</param>
        /// <param name="Name">The multi-language brand name.</param>
        /// <param name="Logo">The logo of this brand.</param>
        /// <param name="Homepage">The homepage of this brand.</param>
        public Brand(Brand_Id    Id,
                     I18NString  Name,
                     String      Logo     = null,
                     String      Homepage = null)

            : base(Id)

        {

            #region Initial checks

            if (Name == null || !Name.Any())
                throw new ArgumentNullException("Name", "The given brand name must not be null or empty!");

            #endregion

            this._Name      = Name;
            this._LogoURI      = Logo;
            this._Homepage  = Homepage;

        }

        #endregion


        #region IComparable<Brand> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a brand.
            var Brand = Object as Brand;
            if ((Object) Brand == null)
                throw new ArgumentException("The given object is not a brand!");

            return CompareTo(Brand);

        }

        #endregion

        #region CompareTo(Brand)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Brand">Another brand to compare with.</param>
        public Int32 CompareTo(Brand Brand)
        {

            if ((Object) Brand == null)
                throw new ArgumentNullException("The given brand must not be null!");

            return _Id.CompareTo(Brand._Id);

        }

        #endregion

        #endregion

        #region IEquatable<Brand> Members

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

            // Check if the given object is a brand.
            var Brand = Object as Brand;
            if ((Object) Brand == null)
                return false;

            return this.Equals(Brand);

        }

        #endregion

        #region Equals(Brand)

        /// <summary>
        /// Compares two Brands for equality.
        /// </summary>
        /// <param name="Brand">An Brand to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Brand Brand)
        {

            if ((Object) Brand == null)
                return false;

            return _Id.Equals(Brand._Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
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
