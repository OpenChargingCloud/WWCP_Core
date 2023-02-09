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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using cloud.charging.open.protocols.WWCP.Net.IO.JSON;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{


    /// <summary>
    /// Extension methods for smart energy meters.
    /// </summary>
    public static partial class EnergyMeterExtensions
    {

        #region ToJSON(this EnergyMeters, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of EnergyMeters.
        /// </summary>
        /// <param name="EnergyMeters">An enumeration of smart energy meters.</param>
        /// <param name="Skip">The optional number of smart energy meters to skip.</param>
        /// <param name="Take">The optional number of smart energy meters to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into an EVSE.</param>
        public static JArray ToJSON(this IEnumerable<EnergyMeter>                  EnergyMeters,
                                    UInt64?                                        Skip                          = null,
                                    UInt64?                                        Take                          = null,
                                    Boolean                                        Embedded                      = false,
                                    InfoStatus                                     ExpandDataLicenses            = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<EnergyMeter>?  CustomEnergyMeterSerializer   = null)


            => EnergyMeters?.Any() == true

                   ? new JArray(EnergyMeters.Where         (evse => evse is not null).
                                             OrderBy       (evse => evse.Id).
                                             SkipTakeFilter(Skip, Take).
                                             SafeSelect    (evse => evse.ToJSON(Embedded,
                                                                                ExpandDataLicenses,
                                                                                CustomEnergyMeterSerializer)).
                                             Where         (evse => evse is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// A smart energy meter.
    /// </summary>
    public class EnergyMeter : AInternalData,
                               IEquatable<EnergyMeter>, IComparable<EnergyMeter>, IComparable
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const           String    JSONLDContext                           = "https://open.charging.cloud/contexts/wwcp+json/energyMeter";

        private readonly       Decimal   EPSILON                                 = 0.01m;

        #endregion

        #region Properties

        /// <summary>
        /// The global unique identification of this smart energy meter.
        /// </summary>
        [Mandatory]
        public EnergyMeter_Id            Id                 { get; }

        /// <summary>
        /// The multi-language description of this smart energy meter.
        /// </summary>
        [Optional]
        public I18NString?               Description        { get; }

        /// <summary>
        /// The optional manufacturer of this smart energy meter.
        /// </summary>
        [Optional]
        public String?                   Manufacturer       { get; }

        /// <summary>
        /// An optional URL to the manufacturer of the smart energy meter.
        /// </summary>
        [Optional]
        public URL?                      ManufacturerURL    { get; }

        /// <summary>
        /// The optional model of this smart energy meter.
        /// </summary>
        [Optional]
        public String?                   Model              { get; }

        /// <summary>
        /// The optional URL to the model of this smart energy meter.
        /// </summary>
        [Optional]
        public URL?                      ModelURL           { get; }

        /// <summary>
        /// The optional firmware version of this smart energy meter.
        /// </summary>
        [Optional]
        public String?                   FirmwareVersion    { get; }

        /// <summary>
        /// The optional hardware version of this smart energy meter.
        /// </summary>
        [Optional]
        public String?                   HardwareVersion    { get; }


        // From Chargy Transparency Software!
        // signatureInfos?:            ISignatureInfos;
        // signatureFormat:            string;
        // publicKeys?:                Array<IPublicKey>;

        #region PublicKey

        private String? publicKey;

        /// <summary>
        /// The public key of the energy meter.
        /// </summary>
        [Optional]
        public String? PublicKey
        {

            get
            {
                return publicKey;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref publicKey,
                                value,
                                EventTracking_Id.New);

                else
                    DeleteProperty(ref publicKey);

            }

        }

        #endregion

        public DateTime? LastStatusUpdate { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart energy meter.
        /// </summary>
        /// <param name="Id">The unique identification of the smart energy meter.</param>
        /// <param name="PublicKey">The public key of the smart energy meter.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">An optional dictionary of internal data.</param>
        public EnergyMeter(EnergyMeter_Id          Id,
                           String?                 PublicKey      = null,

                           JObject?                CustomData     = null,
                           UserDefinedDictionary?  InternalData   = null)

            : base(CustomData,
                   InternalData)

        {

            this.Id         = Id;
            this.PublicKey  = PublicKey;

        }

        #endregion


        #region ToJSON(this EnergyMeter, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given EnergyMeter.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public JObject ToJSON(Boolean                                        Embedded                      = false,
                              InfoStatus                                     ExpandDataLicenses            = InfoStatus.ShowIdOnly,
                              CustomJObjectSerializerDelegate<EnergyMeter>?  CustomEnergyMeterSerializer   = null)

        {

            var JSON = JSONObject.Create(

                           new JProperty("@id", Id.ToString()),

                           !Embedded
                               ? new JProperty("@context",  JSONLDContext)
                               : null,

                           Description is not null && Description.IsNeitherNullNorEmpty()
                               ? new JProperty("description", Description.ToJSON())
                               : null

                     );

            return CustomEnergyMeterSerializer is not null
                       ? CustomEnergyMeterSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">A smart energy meter.</param>
        /// <param name="EnergyMeter2">Another smart energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergyMeter EnergyMeter1, EnergyMeter EnergyMeter2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EnergyMeter1, EnergyMeter2))
                return true;

            // If one is null, but not both, return false.
            if (EnergyMeter1 is null || EnergyMeter2 is null)
                return false;

            return EnergyMeter1.Equals(EnergyMeter2);

        }

        #endregion

        #region Operator != (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">A smart energy meter.</param>
        /// <param name="EnergyMeter2">Another smart energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergyMeter EnergyMeter1, EnergyMeter EnergyMeter2)
            => !(EnergyMeter1 == EnergyMeter2);

        #endregion

        #region Operator <  (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">A smart energy meter.</param>
        /// <param name="EnergyMeter2">Another smart energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergyMeter EnergyMeter1, EnergyMeter EnergyMeter2)
        {

            if (EnergyMeter1 is null)
                throw new ArgumentNullException(nameof(EnergyMeter1), "The given EnergyMeter1 must not be null!");

            return EnergyMeter1.CompareTo(EnergyMeter2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">A smart energy meter.</param>
        /// <param name="EnergyMeter2">Another smart energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergyMeter EnergyMeter1, EnergyMeter EnergyMeter2)
            => !(EnergyMeter1 > EnergyMeter2);

        #endregion

        #region Operator >  (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">A smart energy meter.</param>
        /// <param name="EnergyMeter2">Another smart energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergyMeter EnergyMeter1, EnergyMeter EnergyMeter2)
        {

            if (EnergyMeter1 is null)
                throw new ArgumentNullException(nameof(EnergyMeter1), "The given EnergyMeter1 must not be null!");

            return EnergyMeter1.CompareTo(EnergyMeter2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">A smart energy meter.</param>
        /// <param name="EnergyMeter2">Another smart energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergyMeter EnergyMeter1, EnergyMeter EnergyMeter2)
            => !(EnergyMeter1 < EnergyMeter2);

        #endregion

        #endregion

        #region IComparable<EnergyMeter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeter energyMeter
                   ? CompareTo(energyMeter)
                   : throw new ArgumentException("The given object is not a smart energy meter!", nameof(Object));

        #endregion

        #region CompareTo(EnergyMeter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter">A smart energy meter to compare with.</param>
        public Int32 CompareTo(EnergyMeter? EnergyMeter)

            => EnergyMeter is not null
                   ? Id.CompareTo(EnergyMeter.Id)
                   : throw new ArgumentException("The given object is not a smart energy meter!", nameof(Object));

        #endregion

        #endregion

        #region IEquatable<EnergyMeter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeter energyMeter &&
                  Equals(energyMeter);

        #endregion

        #region Equals(EnergyMeter)

        /// <summary>
        /// Compares two EnergyMeters for equality.
        /// </summary>
        /// <param name="EnergyMeter">A smart energy meter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EnergyMeter? EnergyMeter)

            => EnergyMeter is not null &&
                   Id.Equals(EnergyMeter.Id);

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
