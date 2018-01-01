/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging tariff to charge an electric vehicle.
    /// </summary>
    public class ChargingTariff : AEMobilityEntity<ChargingTariff_Id>,
                                  IEquatable<ChargingTariff>,
                                  IComparable<ChargingTariff>,
                                  IComparable
    {

        #region Properties

        /// <summary>
        /// The offical (multi-language) name of this charging tariff.
        /// </summary>
        [Mandatory]
        public I18NString  Name           { get; }

        /// <summary>
        /// An optional (multi-language) description of this charging tariff.
        /// </summary>
        [Optional]
        public I18NString  Description    { get; }

        /// <summary>
        /// An optional brand for this charging tariff.
        /// </summary>
        [Optional]
        public Brand       Brand          { get; }

        /// <summary>
        /// An URI for more information about this tariff.
        /// </summary>
        [Optional]
        public Uri         TariffURI      { get; }

        /// <summary>
        /// ISO 4217 code of the currency used for this tariff.
        /// </summary>
        [Mandatory]
        public Currency    Currency       { get; }

        /// <summary>
        /// The energy mix.
        /// </summary>
        [Optional]
        public EnergyMix   EnergyMix      { get;  }

                /// <summary>
        /// An enumeration of tariff elements.
        /// </summary>
        [Mandatory]
        public IEnumerable<ChargingTariffElement>  TariffElements    { get; }


        #endregion

        #region Links

        /// <summary>
        /// The charging tariff operator of this charging tariff.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator  Operator   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging tariff having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Operator">The charging station operator of this charging tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        public ChargingTariff(ChargingTariff_Id                   Id,
                              ChargingStationOperator             Operator,
                              I18NString                          Name,
                              I18NString                          Description,
                              Brand                               Brand,

                              Uri                                 TariffUrl,
                              Currency                            Currency,
                              EnergyMix                           EnergyMix,
                              IEnumerable<ChargingTariffElement>  TariffElements)

            : base(Id)

        {

            #region Initial checks

            if (Operator == null)
                throw new ArgumentNullException(nameof(Operator),        "The given charging station operator must not be null!");

            if (TariffElements == null || !TariffElements.Any())
                throw new ArgumentNullException(nameof(TariffElements),  "The given enumeration of tariff elements must not be null or empty!");

            #endregion

            #region Init data and properties

            this.Operator        = Operator;
            this.Name            = Name;
            this.Description     = Description ?? new I18NString();
            this.Brand           = Brand;

            this.TariffURI       = TariffUrl;
            this.Currency        = Currency;
            this.EnergyMix       = EnergyMix;
            this.TariffElements  = TariffElements;

            #endregion

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingTariff1, ChargingTariff2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingTariff1 == null) || ((Object) ChargingTariff2 == null))
                return false;

            return ChargingTariff1.Equals(ChargingTariff2);

        }

        #endregion

        #region Operator != (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
            => !(ChargingTariff1 == ChargingTariff2);

        #endregion

        #region Operator <  (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
        {

            if ((Object) ChargingTariff1 == null)
                throw new ArgumentNullException(nameof(ChargingTariff1), "The given ChargingTariff1 must not be null!");

            return ChargingTariff1.CompareTo(ChargingTariff2) < 0;

        }

        #endregion

        #region Operator <= (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
            => !(ChargingTariff1 > ChargingTariff2);

        #endregion

        #region Operator >  (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
        {

            if ((Object) ChargingTariff1 == null)
                throw new ArgumentNullException(nameof(ChargingTariff1), "The given ChargingTariff1 must not be null!");

            return ChargingTariff1.CompareTo(ChargingTariff2) > 0;

        }

        #endregion

        #region Operator >= (ChargingTariff1, ChargingTariff2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff1">A charging tariff.</param>
        /// <param name="ChargingTariff2">Another charging tariff.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingTariff ChargingTariff1, ChargingTariff ChargingTariff2)
            => !(ChargingTariff1 < ChargingTariff2);

        #endregion

        #endregion

        #region IComparable<ChargingTariff> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargingTariff = Object as ChargingTariff;
            if ((Object) ChargingTariff == null)
                throw new ArgumentException("The given object is not a charging tariff!", nameof(Object));

            return CompareTo(ChargingTariff);

        }

        #endregion

        #region CompareTo(ChargingTariff)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff object to compare with.</param>
        public Int32 CompareTo(ChargingTariff ChargingTariff)
        {

            if ((Object) ChargingTariff == null)
                throw new ArgumentNullException(nameof(ChargingTariff), "The given charging tariff must not be null!");

            return Id.CompareTo(ChargingTariff.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingTariff> Members

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

            var ChargingTariff = Object as ChargingTariff;
            if ((Object) ChargingTariff == null)
                return false;

            return Equals(ChargingTariff);

        }

        #endregion

        #region Equals(ChargingTariff)

        /// <summary>
        /// Compares two charging tariffs for equality.
        /// </summary>
        /// <param name="ChargingTariff">A charging tariff to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingTariff ChargingTariff)
        {

            if ((Object) ChargingTariff == null)
                return false;

            return Id.Equals(ChargingTariff.Id);

        }

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
