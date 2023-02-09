/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for energy source categories.
    /// </summary>
    public static class EnergySourceCategoriesExtensions
    {

        /// <summary>
        /// Indicates whether this energy source category is null or empty.
        /// </summary>
        /// <param name="EnergySourceCategory">An energy source category.</param>
        public static Boolean IsNullOrEmpty(this EnergySourceCategories? EnergySourceCategory)
            => !EnergySourceCategory.HasValue || EnergySourceCategory.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this energy source category is null or empty.
        /// </summary>
        /// <param name="EnergySourceCategory">An energy source category.</param>
        public static Boolean IsNotNullOrEmpty(this EnergySourceCategories? EnergySourceCategory)
            => EnergySourceCategory.HasValue && EnergySourceCategory.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status type of a EVSE.
    /// </summary>
    public readonly struct EnergySourceCategories : IId,
                                                  IEquatable <EnergySourceCategories>,
                                                  IComparable<EnergySourceCategories>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the EVSE admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy source category based on the given string.
        /// </summary>
        private EnergySourceCategories(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an energy source category.
        /// </summary>
        /// <param name="Text">A text representation of an energy source category.</param>
        public static EnergySourceCategories Parse(String Text)
        {

            if (TryParse(Text, out EnergySourceCategories energySourceCategory))
                return energySourceCategory;

            throw new ArgumentException("Invalid text representation of an energy source category: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an energy source category.
        /// </summary>
        /// <param name="Text">A text representation of an energy source category.</param>
        public static EnergySourceCategories? TryParse(String Text)
        {

            if (TryParse(Text, out EnergySourceCategories energySourceCategory))
                return energySourceCategory;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EnergySourceCategory)

        /// <summary>
        /// Parse the given string as an energy source category.
        /// </summary>
        /// <param name="Text">A text representation of an energy source category.</param>
        /// <param name="EnergySourceCategory">The parsed energy source category.</param>
        public static Boolean TryParse(String Text, out EnergySourceCategories EnergySourceCategory)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EnergySourceCategory = new EnergySourceCategories(Text);
                    return true;
                }
                catch
                { }
            }

            EnergySourceCategory = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this energy source category.
        /// </summary>
        public EnergySourceCategories Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unkown energy source
        /// </summary>
        public static readonly EnergySourceCategories  Unkown           = new("unkown");


        /// <summary>
        /// Gas
        /// </summary>
        public static readonly EnergySourceCategories  Gas              = new("Gas");

        /// <summary>
        /// Coal
        /// </summary>
        public static readonly EnergySourceCategories  Coal             = new("Coal");

        /// <summary>
        /// Stone coal
        /// </summary>
        public static readonly EnergySourceCategories  StoneCoal        = new("StoneCoal");

        /// <summary>
        /// Oil
        /// </summary>
        public static readonly EnergySourceCategories  Oil              = new("Oil");

        /// <summary>
        /// Other fossil energy source
        /// </summary>
        public static readonly EnergySourceCategories  OtherFossil      = new("OtherFossil");


        /// <summary>
        /// Nuclear fision
        /// </summary>
        public static readonly EnergySourceCategories  NuclearFision    = new("NuclearFision");

        /// <summary>
        /// Nuclear fusion
        /// </summary>
        public static readonly EnergySourceCategories  NuclearFusion    = new("NuclearFusion");


        /// <summary>
        /// Sun
        /// </summary>
        public static readonly EnergySourceCategories  Sun              = new("Sun");

        /// <summary>
        /// Wind
        /// </summary>
        public static readonly EnergySourceCategories  Wind             = new("Wind");

        /// <summary>
        /// Geothermal
        /// </summary>
        public static readonly EnergySourceCategories  Geothermal       = new("Geothermal");

        /// <summary>
        /// Waste
        /// </summary>
        public static readonly EnergySourceCategories  Waste            = new("Waste");

        /// <summary>
        /// Biogas
        /// </summary>
        public static readonly EnergySourceCategories  Biogas           = new("Biogas");

        /// <summary>
        /// Other green energy source
        /// </summary>
        public static readonly EnergySourceCategories  OtherGreen       = new("OtherGreen");

        #endregion


        #region Operator overloading

        #region Operator == (EnergySourceCategory1, EnergySourceCategory2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySourceCategory1">An energy source category.</param>
        /// <param name="EnergySourceCategory2">Another energy source category.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergySourceCategories EnergySourceCategory1,
                                           EnergySourceCategories EnergySourceCategory2)

            => EnergySourceCategory1.Equals(EnergySourceCategory2);

        #endregion

        #region Operator != (EnergySourceCategory1, EnergySourceCategory2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySourceCategory1">An energy source category.</param>
        /// <param name="EnergySourceCategory2">Another energy source category.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergySourceCategories EnergySourceCategory1,
                                           EnergySourceCategories EnergySourceCategory2)

            => !EnergySourceCategory1.Equals(EnergySourceCategory2);

        #endregion

        #region Operator <  (EnergySourceCategory1, EnergySourceCategory2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySourceCategory1">An energy source category.</param>
        /// <param name="EnergySourceCategory2">Another energy source category.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergySourceCategories EnergySourceCategory1,
                                          EnergySourceCategories EnergySourceCategory2)

            => EnergySourceCategory1.CompareTo(EnergySourceCategory2) < 0;

        #endregion

        #region Operator <= (EnergySourceCategory1, EnergySourceCategory2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySourceCategory1">An energy source category.</param>
        /// <param name="EnergySourceCategory2">Another energy source category.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergySourceCategories EnergySourceCategory1,
                                           EnergySourceCategories EnergySourceCategory2)

            => EnergySourceCategory1.CompareTo(EnergySourceCategory2) <= 0;

        #endregion

        #region Operator >  (EnergySourceCategory1, EnergySourceCategory2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySourceCategory1">An energy source category.</param>
        /// <param name="EnergySourceCategory2">Another energy source category.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergySourceCategories EnergySourceCategory1,
                                          EnergySourceCategories EnergySourceCategory2)

            => EnergySourceCategory1.CompareTo(EnergySourceCategory2) > 0;

        #endregion

        #region Operator >= (EnergySourceCategory1, EnergySourceCategory2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergySourceCategory1">An energy source category.</param>
        /// <param name="EnergySourceCategory2">Another energy source category.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergySourceCategories EnergySourceCategory1,
                                           EnergySourceCategories EnergySourceCategory2)

            => EnergySourceCategory1.CompareTo(EnergySourceCategory2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergySourceCategories> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy source categories.
        /// </summary>
        /// <param name="Object">An energy source category to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergySourceCategories energySourceCategory
                   ? CompareTo(energySourceCategory)
                   : throw new ArgumentException("The given object is not an energy source category!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergySourceCategory)

        /// <summary>
        /// Compares two energy source categories.
        /// </summary>
        /// <param name="EnergySourceCategory">An energy source category to compare with.</param>
        public Int32 CompareTo(EnergySourceCategories EnergySourceCategory)

            => String.Compare(InternalId,
                              EnergySourceCategory.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EnergySourceCategories> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy source categories for equality.
        /// </summary>
        /// <param name="Object">An energy source category to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergySourceCategories energySourceCategory &&
                   Equals(energySourceCategory);

        #endregion

        #region Equals(EnergySourceCategory)

        /// <summary>
        /// Compares two energy source categories for equality.
        /// </summary>
        /// <param name="EnergySourceCategory">An energy source category to compare with.</param>
        public Boolean Equals(EnergySourceCategories EnergySourceCategory)

            => String.Equals(InternalId,
                             EnergySourceCategory.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
