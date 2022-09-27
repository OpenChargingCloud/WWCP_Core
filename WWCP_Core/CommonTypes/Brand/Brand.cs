/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for brands.
    /// </summary>
    public static class BrandExtensions
    {

        #region ToJSON(this Brands, Skip = null, Take = null, Embedded = false, ExpandDataLicenses = null)

        /// <summary>
        /// Return a JSON representation for the given enumeration of brands.
        /// </summary>
        /// <param name="Brands">An enumeration of brands.</param>
        /// <param name="Skip">The optional number of charging station operators to skip.</param>
        /// <param name="Take">The optional number of charging station operators to return.</param>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="ExpandDataLicenses">Whether to expand data licenses.</param>
        public static JArray ToJSON(this IEnumerable<Brand>  Brands,
                                    UInt64?                  Skip                 = null,
                                    UInt64?                  Take                 = null,
                                    Boolean                  Embedded             = false,
                                    InfoStatus?              ExpandDataLicenses   = null)


            => Brands is null || !Brands.Any()

                   ? new JArray()

                   : new JArray(Brands.
                                    Where         (brand => brand is not null).
                                    OrderBy       (brand => brand.Id).
                                    SkipTakeFilter(Skip, Take).
                                    SafeSelect    (brand => brand.ToJSON(Embedded,
                                                                         ExpandDataLicenses)));

        #endregion

    }


    /// <summary>
    /// An Electric Vehicle Supply Equipment (Brand) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class Brand : IHasId<Brand_Id>,
                         IEntity<Brand_Id>,
                         IEquatable<Brand>,
                         IComparable<Brand>,
                         IComparable
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/brand";

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of the brand.
        /// </summary>
        public Brand_Id                  Id              { get; }

        /// <summary>
        /// The multi-language brand name.
        /// </summary>
        public I18NString                Name            { get; }

        /// <summary>
        /// The optional URL of the logo of this brand.
        /// </summary>
        [Optional]
        public URL?                      Logo            { get; }

        /// <summary>
        /// The homepage of this brand.
        /// </summary>
        [Optional]
        public URL?                      Homepage        { get; }

        /// <summary>
        /// The optional data licenses of this brand.
        /// </summary>
        [Optional]
        public IEnumerable<DataLicense>  DataLicenses    { get; }

        public DateTime LastChange
            => throw new NotImplementedException();

        #endregion

        #region Event

        public event OnPropertyChangedDelegate?  OnPropertyChanged;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new brand having the given brand identification.
        /// </summary>
        /// <param name="Id">The unique identification of this brand.</param>
        /// <param name="Name">The multi-language brand name.</param>
        /// <param name="Logo">An optional logo of this brand.</param>
        /// <param name="Homepage">An optional homepage of this brand.</param>
        /// <param name="DataLicenses">The optional data licenses of this brand.</param>
        public Brand(Brand_Id                   Id,
                     I18NString                 Name,
                     URL?                       Logo           = null,
                     URL?                       Homepage       = null,
                     IEnumerable<DataLicense>?  DataLicenses   = null)
        {

            #region Initial checks

            if (Name is null || Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name), "The given brand name must not be null or empty!");

            #endregion

            this.Id            = Id;
            this.Name          = Name;
            this.Logo          = Logo;
            this.Homepage      = Homepage;
            this.DataLicenses  = DataLicenses?.Distinct() ?? Array.Empty<DataLicense>();

        }

        #endregion


        #region ToJSON(Embedded = false, ExpandDataLicenses = InfoStatus.ShowIdOnly)

        /// <summary>
        /// Return a JSON representation of the given status report.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="ExpandDataLicenses">Whether to expand the data licenses.</param>
        /// <param name="CustomBrandSerializer">A delegate to serialize custom brand JSON elements.</param>
        public JObject ToJSON(Boolean                                  Embedded                = false,
                              InfoStatus?                              ExpandDataLicenses      = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<Brand>?  CustomBrandSerializer   = null)
        {

            var json = JSONObject.Create(

                           Embedded
                               ? new JProperty("@context",      JSONLDContext)
                               : null,

                           new JProperty("id",                  Id.      ToString()),
                           new JProperty("name",                Name.    ToJSON()),

                           Logo.IsNotNullOrEmpty()
                               ? new JProperty("logo",          Logo.    ToString())
                               : null,

                           Homepage.IsNotNullOrEmpty()
                               ? new JProperty("homepage",      Homepage.ToString())
                               : null,

                           DataLicenses.Any()
                               ? new JProperty("dataLicenses",  ExpandDataLicenses.Switch(
                                                                    () => new JArray(DataLicenses.Select(dataLicense => dataLicense.Id.ToString())),
                                                                    () => new JArray(DataLicenses.Select(dataLicense => dataLicense.ToJSON()))
                                                                ))
                               : null

                       );

            return CustomBrandSerializer is not null
                       ? CustomBrandSerializer(this, json)
                       : json;

        }

        #endregion


        #region IComparable<Brand> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Brand brand
                   ? CompareTo(brand)
                   : throw new ArgumentException("The given object is not a brand!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Brand)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Brand">Another brand to compare with.</param>
        public Int32 CompareTo(Brand? Brand)
        {

            if (Brand is null)
                throw new ArgumentNullException(nameof(Brand), "The given brand must not be null!");

            var c = Id.CompareTo(Brand.Id);

            //if (c == 0)
            //    c = Name.CompareTo(Brand.Name);

            if (c == 0)
                c = Logo.HasValue && Brand.Logo.HasValue
                        ? Logo.Value.CompareTo(Brand.Logo.Value)
                        : 0;

            if (c == 0)
                c = Homepage.HasValue && Brand.Homepage.HasValue
                    ? Homepage.Value.CompareTo(Brand.Homepage.Value)
                    : 0;

            return c;

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
        public override Boolean Equals(Object? Object)

            => Object is Brand brand &&
                   Equals(brand);

        #endregion

        #region Equals(Brand)

        /// <summary>
        /// Compares two Brands for equality.
        /// </summary>
        /// <param name="Brand">An Brand to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Brand? Brand)

            => Brand is not null &&

               Id   == Brand.Id   &&
               Name == Brand.Name &&

               ((!Logo.    HasValue && !Brand.Logo.    HasValue) ||
                ( Logo.    HasValue &&  Brand.Logo.    HasValue && Logo.    Equals(Brand.Logo))) &&

               ((!Homepage.HasValue && !Brand.Homepage.HasValue) ||
                ( Homepage.HasValue &&  Brand.Homepage.HasValue && Homepage.Equals(Brand.Homepage)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        #endregion

    }

}
