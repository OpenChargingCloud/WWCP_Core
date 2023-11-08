﻿/*
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A energy meter.
    /// </summary>
    public class EnergyMeter : AEMobilityEntity<EnergyMeter_Id,
                                                EnergyMeterAdminStatusTypes,
                                                EnergyMeterStatusTypes>,
                               IEquatable<EnergyMeter>,
                               IComparable<EnergyMeter>,
                               IEnergyMeter
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const            String    JSONLDContext = "https://open.charging.cloud/contexts/wwcp+json/energyMeter";

        private readonly        Decimal   EPSILON = 0.01m;

        /// <summary>
        /// The default max size of the energy meter admin status list.
        /// </summary>
        public const            UInt16    DefaultMaxEnergyMeterAdminStatusScheduleSize    = 15;

        /// <summary>
        /// The default max size of the energy meter status list.
        /// </summary>
        public const            UInt16    DefaultMaxEnergyMeterStatusScheduleSize         = 15;

        #endregion

        #region Properties

        /// <summary>
        /// The optional model of the energy meter.
        /// </summary>
        [Optional]
        public String?                                  Model                        { get; }

        /// <summary>
        /// The optional URL to the model of the energy meter.
        /// </summary>
        [Optional]
        public URL?                                     ModelURL                     { get; }

        /// <summary>
        /// The optional hardware version of the energy meter.
        /// </summary>
        [Optional]
        public String?                                  HardwareVersion              { get; }

        /// <summary>
        /// The optional firmware version of the energy meter.
        /// </summary>
        [Optional]
        public String?                                  FirmwareVersion              { get; }

        /// <summary>
        /// The optional manufacturer of the energy meter.
        /// </summary>
        [Optional]
        public String?                                  Manufacturer                 { get; }

        /// <summary>
        /// The optional URL to the manufacturer of the energy meter.
        /// </summary>
        [Optional]
        public URL?                                     ManufacturerURL              { get; }

        /// <summary>
        /// The optional enumeration of public keys used for signing the energy meter values.
        /// </summary>
        [Optional]
        public IEnumerable<PublicKey>                   PublicKeys                   { get; }

        /// <summary>
        /// One or multiple optional certificates for the public key of the energy meter.
        /// </summary>
        [Optional]
        public CertificateChain?                        PublicKeyCertificateChain    { get; }

        /// <summary>
        /// The enumeration of transparency softwares and their legal status,
        /// which can be used to validate the charging session data.
        /// </summary>
        [Optional]
        public IEnumerable<TransparencySoftwareStatus>  TransparencySoftwares        { get; }

        ///// <summary>
        ///// The timestamp when this energy meter was last updated (or created).
        ///// </summary>
        //[Mandatory]
        //public DateTime                                 LastUpdate                   { get; }


        /// <summary>
        /// An optional remote energy meter.
        /// </summary>
        public IRemoteEnergyMeter?                      RemoteEnergyMeter           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new energy meter.
        /// </summary>
        /// <param name="Id">The identification of the energy meter.</param>
        /// 
        /// <param name="Description">An multi-language description of the energy meter.</param>
        /// 
        /// <param name="Model">An optional model of the energy meter.</param>
        /// <param name="ModelURL">An optional URL to the model of the energy meter.</param>
        /// <param name="HardwareVersion">An optional hardware version of the energy meter.</param>
        /// <param name="FirmwareVersion">An optional firmware version of the energy meter.</param>
        /// <param name="Manufacturer">An optional manufacturer of the energy meter.</param>
        /// <param name="ManufacturerURL">An optional URL to the manufacturer of the energy meter.</param>
        /// <param name="PublicKeys">The optional public key of the energy meter used for signeing the energy meter values.</param>
        /// <param name="PublicKeyCertificateChain">One or multiple optional certificates for the public key of the energy meter.</param>
        /// <param name="TransparencySoftwares">An enumeration of transparency softwares and their legal status, which can be used to validate the charging session data.</param>
        /// 
        /// <param name="LastChange">The timestamp when this energy meter was last updated (or created).</param>
        public EnergyMeter(EnergyMeter_Id                             Id,
                           I18NString?                                Name                         = null,
                           I18NString?                                Description                  = null,

                           String?                                    Model                        = null,
                           URL?                                       ModelURL                     = null,
                           String?                                    HardwareVersion              = null,
                           String?                                    FirmwareVersion              = null,
                           String?                                    Manufacturer                 = null,
                           URL?                                       ManufacturerURL              = null,
                           IEnumerable<PublicKey>?                    PublicKeys                   = null,
                           CertificateChain?                          PublicKeyCertificateChain    = null,
                           IEnumerable<TransparencySoftwareStatus>?   TransparencySoftwares        = null,

                           Timestamped<EnergyMeterAdminStatusTypes>?  InitialAdminStatus           = null,
                           Timestamped<EnergyMeterStatusTypes>?       InitialStatus                = null,
                           UInt16?                                    MaxAdminStatusScheduleSize   = null,
                           UInt16?                                    MaxStatusScheduleSize        = null,

                           String?                                    DataSource                   = null,
                           DateTime?                                  LastChange                   = null,

                           JObject?                                   CustomData                   = null,
                           UserDefinedDictionary?                     InternalData                 = null,

                           Action<EnergyMeter>?                       Configurator                 = null,
                           RemoteEnergyMeterCreatorDelegate?          RemoteEnergyMeterCreator     = null)

            : base(Id,
                   Name,
                   Description,
                   InitialAdminStatus         ?? EnergyMeterAdminStatusTypes.Operational,
                   InitialStatus              ?? EnergyMeterStatusTypes.     Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxEnergyMeterAdminStatusScheduleSize,
                   MaxStatusScheduleSize      ?? DefaultMaxEnergyMeterStatusScheduleSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            this.Model                      = Model;
            this.ModelURL                   = ModelURL;
            this.HardwareVersion            = HardwareVersion;
            this.FirmwareVersion            = FirmwareVersion;
            this.Manufacturer               = Manufacturer;
            this.ManufacturerURL            = ManufacturerURL;
            this.PublicKeys                 = PublicKeys?.Distinct()            ?? Array.Empty<PublicKey>();
            this.PublicKeyCertificateChain  = PublicKeyCertificateChain;
            this.TransparencySoftwares      = TransparencySoftwares?.Distinct() ?? Array.Empty<TransparencySoftwareStatus>();

            Configurator?.Invoke(this);

            this.RemoteEnergyMeter          = RemoteEnergyMeterCreator?.Invoke(this);

        }

        #endregion


        #region (static) Parse   (JSON, CustomEnergyMeterParser = null)

        /// <summary>
        /// Parse the given JSON representation of an energy meter.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomEnergyMeterParser">A delegate to parse custom energy meter JSON objects.</param>
        public static EnergyMeter Parse(JObject                                    JSON,
                                        CustomJObjectParserDelegate<EnergyMeter>?  CustomEnergyMeterParser   = null)
        {

            if (TryParse(JSON,
                         out var energyMeter,
                         out var errorResponse,
                         CustomEnergyMeterParser))
            {
                return energyMeter!;
            }

            throw new ArgumentException("The given JSON representation of an energy meter is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out EnergyMeter, out ErrorResponse, CustomEnergyMeterParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an energy meter.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EnergyMeter">The parsed energy meter.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject           JSON,
                                       out EnergyMeter?  EnergyMeter,
                                       out String?       ErrorResponse)

            => TryParse(JSON,
                        out EnergyMeter,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an energy meter.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="EnergyMeter">The parsed energy meter.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomEnergyMeterParser">A delegate to parse custom energy meter JSON objects.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       out EnergyMeter?                           EnergyMeter,
                                       out String?                                ErrorResponse,
                                       CustomJObjectParserDelegate<EnergyMeter>?  CustomEnergyMeterParser   = null)
        {

            try
            {

                EnergyMeter = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Id                            [mandatory]

                if (!JSON.ParseMandatory("id",
                                         "energy meter identification",
                                         EnergyMeter_Id.TryParse,
                                         out EnergyMeter_Id Id,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Name                          [optional]

                if (JSON.ParseOptional("name",
                                       "energy meter name",
                                       I18NString.TryParse,
                                       out I18NString? Name,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Description                   [optional]

                if (JSON.ParseOptional("description",
                                       "energy meter description",
                                       I18NString.TryParse,
                                       out I18NString? Description,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Model                         [optional]

                var Model = JSON.GetString("model");

                #endregion

                #region Parse ModelURL                      [optional]

                if (JSON.ParseOptional("modelURL",
                                       "energy meter model URL",
                                       URL.TryParse,
                                       out URL? ModelURL,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse HardwareVersion               [optional]

                var HardwareVersion = JSON.GetString("hardwareVersion");

                #endregion

                #region Parse FirmwareVersion               [optional]

                var FirmwareVersion = JSON.GetString("firmwareVersion");

                #endregion

                #region Parse Vendor                        [optional]

                var Manufacturer = JSON.GetString("manufacturer");

                #endregion

                #region Parse ManufacturerURL               [optional]

                if (JSON.ParseOptional("manufacturerURL",
                                       "energy meter manufacturer URL",
                                       URL.TryParse,
                                       out URL? ManufacturerURL,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PublicKeys                    [optional]

                if (JSON.ParseOptionalHashSet("publicKeys",
                                              "energy meter public keys",
                                              PublicKey.TryParse,
                                              out HashSet<PublicKey> PublicKeys,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse PublicKeyCertificateChain     [optional]

                if (JSON.ParseOptional("publicKeyCertificateChain",
                                       "energy meter public key certificate chain",
                                       CertificateChain.TryParse,
                                       out CertificateChain? PublicKeyCertificateChain,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TransparencySoftwareStatus    [optional]

                if (JSON.ParseOptionalHashSet("transparencySoftwares",
                                              "transparency softwares",
                                              TransparencySoftwareStatus.TryParse,
                                              out HashSet<TransparencySoftwareStatus> TransparencySoftwares,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse LastChange                    [mandatory]

                if (!JSON.ParseMandatory("lastChange",
                                         "last change",
                                         out DateTime LastChange,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                EnergyMeter = new EnergyMeter(Id,

                                              Name,
                                              Description,

                                              Model,
                                              ModelURL,
                                              HardwareVersion,
                                              FirmwareVersion,
                                              Manufacturer,
                                              ManufacturerURL,
                                              PublicKeys,
                                              PublicKeyCertificateChain,
                                              TransparencySoftwares,

                                              null, // InitialAdminStatus
                                              null, // InitialStatus

                                              null, // MaxAdminStatusScheduleSize
                                              null, // MaxStatusScheduleSize

                                              null, // DataSource

                                              LastChange,
                                              null, // CustomData
                                              null);// InternalData


                if (CustomEnergyMeterParser is not null)
                    EnergyMeter = CustomEnergyMeterParser(JSON,
                                                          EnergyMeter);

                return true;

            }
            catch (Exception e)
            {
                EnergyMeter    = default;
                ErrorResponse  = "The given JSON representation of an energy meter is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(this EnergyMeter, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        /// <param name="CustomEnergyMeterSerializer">A delegate to serialize custom energy meter JSON objects.</param>
        /// <param name="CustomTransparencySoftwareStatusSerializer">A delegate to serialize custom transparency software status JSON objects.</param>
        /// <param name="CustomTransparencySoftwareSerializer">A delegate to serialize custom transparency software JSON objects.</param>
        public JObject ToJSON(Boolean                                                       Embedded                                     = false,
                              CustomJObjectSerializerDelegate<IEnergyMeter>?                CustomEnergyMeterSerializer                  = null,
                              CustomJObjectSerializerDelegate<TransparencySoftwareStatus>?  CustomTransparencySoftwareStatusSerializer   = null,
                              CustomJObjectSerializerDelegate<TransparencySoftware>?        CustomTransparencySoftwareSerializer         = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("id",                          Id.ToString()),

                           !Embedded
                               ? new JProperty("@context",                    JSONLDContext)
                               : null,

                           Model is not null
                               ? new JProperty("model",                       Model)
                               : null,

                           ModelURL.HasValue
                               ? new JProperty("modelURL",                    ModelURL.Value.ToString())
                               : null,

                           HardwareVersion is not null
                               ? new JProperty("hardwareVersion",             HardwareVersion)
                               : null,

                           FirmwareVersion is not null
                               ? new JProperty("firmwareVersion",             FirmwareVersion)
                               : null,

                           Manufacturer is not null
                               ? new JProperty("manufacturer",                Manufacturer)
                               : null,

                           ManufacturerURL.HasValue
                               ? new JProperty("manufacturerURL",             ManufacturerURL.Value.ToString())
                               : null,

                           PublicKeys.Any()
                               ? new JProperty("publicKeys",                  new JArray(PublicKeys.Select(publicKey => publicKey.ToString())))
                               : null,

                           PublicKeyCertificateChain.HasValue
                               ? new JProperty("publicKeyCertificateChain",   PublicKeyCertificateChain.Value.ToString())
                               : null,

                           TransparencySoftwares.Any()
                               ? new JProperty("transparencySoftwares",       new JArray(TransparencySoftwares.Select(transparencySoftwareStatus => transparencySoftwareStatus.ToJSON(CustomTransparencySoftwareStatusSerializer,
                                                                                                                                                                                      CustomTransparencySoftwareSerializer))))
                               : null,

                           Description is not null && Description.IsNotNullOrEmpty()
                               ? new JProperty("description",                 Description.ToJSON())
                               : null,

                                 new JProperty("lastChange",                  LastChangeDate. ToIso8601())

                       );

            return CustomEnergyMeterSerializer is not null
                       ? CustomEnergyMeterSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this object.
        /// </summary>
        public EnergyMeter Clone()

            => new (
                   Id.Clone,
                   Name.       IsNotNullOrEmpty() ? Name.       Clone : I18NString.Empty,
                   Description.IsNotNullOrEmpty() ? Description.Clone : I18NString.Empty,
                   Model           is not null ? new String(Model.ToCharArray()) : null,
                   ModelURL.HasValue           ? ModelURL.Value.Clone : null,
                   HardwareVersion is not null ? new String(HardwareVersion.ToCharArray()) : null,
                   FirmwareVersion is not null ? new String(FirmwareVersion.ToCharArray()) : null,
                   Manufacturer    is not null ? new String(Manufacturer.ToCharArray()) : null,
                   ManufacturerURL.HasValue    ? ManufacturerURL.Value.Clone : null,
                   PublicKeys.Select(publicKey => publicKey.Clone).ToArray(),
                   PublicKeyCertificateChain.HasValue ? PublicKeyCertificateChain.Value.Clone : null,
                   TransparencySoftwares.Select(transparencySoftwareStatus => transparencySoftwareStatus.Clone()).ToArray(),

                   AdminStatus,
                   Status,
                   adminStatusSchedule.MaxStatusHistorySize,
                   statusSchedule.     MaxStatusHistorySize,

                   DataSource is not null
                       ? new String(DataSource.ToCharArray())
                       : null,
                   LastChangeDate,

                   JObject.Parse(CustomData.ToString()),
                   InternalData
               );

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">An energy meter.</param>
        /// <param name="EnergyMeter2">Another energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergyMeter? EnergyMeter1,
                                           EnergyMeter? EnergyMeter2)
        {

            if (Object.ReferenceEquals(EnergyMeter1, EnergyMeter2))
                return true;

            if (EnergyMeter1 is null || EnergyMeter2 is null)
                return false;

            return EnergyMeter1.Equals(EnergyMeter2);

        }

        #endregion

        #region Operator != (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">An energy meter.</param>
        /// <param name="EnergyMeter2">Another energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergyMeter? EnergyMeter1,
                                           EnergyMeter? EnergyMeter2)

            => !(EnergyMeter1 == EnergyMeter2);

        #endregion

        #region Operator <  (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">An energy meter.</param>
        /// <param name="EnergyMeter2">Another energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergyMeter? EnergyMeter1,
                                          EnergyMeter? EnergyMeter2)

            => EnergyMeter1 is null
                   ? throw new ArgumentNullException(nameof(EnergyMeter1), "The given energy meter must not be null!")
                   : EnergyMeter1.CompareTo(EnergyMeter2) < 0;

        #endregion

        #region Operator <= (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">An energy meter.</param>
        /// <param name="EnergyMeter2">Another energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergyMeter? EnergyMeter1,
                                           EnergyMeter? EnergyMeter2)

            => !(EnergyMeter1 > EnergyMeter2);

        #endregion

        #region Operator >  (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">An energy meter.</param>
        /// <param name="EnergyMeter2">Another energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergyMeter? EnergyMeter1,
                                          EnergyMeter? EnergyMeter2)

            => EnergyMeter1 is null
                   ? throw new ArgumentNullException(nameof(EnergyMeter1), "The given energy meter must not be null!")
                   : EnergyMeter1.CompareTo(EnergyMeter2) > 0;

        #endregion

        #region Operator >= (EnergyMeter1, EnergyMeter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter1">An energy meter.</param>
        /// <param name="EnergyMeter2">Another energy meter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergyMeter? EnergyMeter1,
                                           EnergyMeter? EnergyMeter2)

            => !(EnergyMeter1 < EnergyMeter2);

        #endregion

        #endregion

        #region IComparable<EnergyMeter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meters.
        /// </summary>
        /// <param name="Object">An energy meter to compare with.</param>
        public override Int32 CompareTo(Object? Object)

            => Object is EnergyMeter energyMeter
                   ? CompareTo(energyMeter)
                   : throw new ArgumentException("The given object is not an energy meter!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter">A smart energy meter to compare with.</param>
        public Int32 CompareTo(EnergyMeter? EnergyMeter)

            => CompareTo(EnergyMeter as IEnergyMeter);


        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeter">A smart energy meter to compare with.</param>
        public Int32 CompareTo(IEnergyMeter? EnergyMeter)
        {

            if (EnergyMeter is null)
                throw new ArgumentNullException(nameof(EnergyMeter), "The given energy meter must not be null!");

            var c = Id.CompareTo(EnergyMeter.Id);

            if (c == 0)
                c = LastChangeDate.ToIso8601().CompareTo(EnergyMeter.LastChangeDate.ToIso8601());

            if (c == 0)
                c = Model is not null && EnergyMeter.Model is not null
                        ? Model.CompareTo(EnergyMeter.Model)
                        : 0;

            if (c == 0)
                c = ModelURL.HasValue && EnergyMeter.ModelURL.HasValue
                        ? ModelURL.Value.CompareTo(EnergyMeter.ModelURL.Value)
                        : 0;

            if (c == 0)
                c = HardwareVersion is not null && EnergyMeter.HardwareVersion is not null
                        ? HardwareVersion.CompareTo(EnergyMeter.HardwareVersion)
                        : 0;

            if (c == 0)
                c = FirmwareVersion is not null && EnergyMeter.FirmwareVersion is not null
                        ? FirmwareVersion.CompareTo(EnergyMeter.FirmwareVersion)
                        : 0;

            if (c == 0)
                c = Manufacturer is not null && EnergyMeter.Manufacturer is not null
                        ? Manufacturer.CompareTo(EnergyMeter.Manufacturer)
                        : 0;

            if (c == 0)
                c = ManufacturerURL.HasValue && EnergyMeter.ManufacturerURL.HasValue
                        ? ManufacturerURL.Value.CompareTo(EnergyMeter.ManufacturerURL.Value)
                        : 0;

            // PublicKeys

            if (c == 0)
                c = PublicKeyCertificateChain.HasValue && EnergyMeter.PublicKeyCertificateChain.HasValue
                        ? PublicKeyCertificateChain.Value.CompareTo(EnergyMeter.PublicKeyCertificateChain.Value)
                        : 0;

            // TransparencySoftwares
            // Description

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meters for equality.
        /// </summary>
        /// <param name="Object">An energy meter to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeter energyMeter &&
                   Equals(energyMeter);

        #endregion

        #region Equals(EnergyMeter)

        /// <summary>
        /// Compares two energy meters for equality.
        /// </summary>
        /// <param name="EnergyMeter">An energy meter to compare with.</param>
        public Boolean Equals(EnergyMeter? EnergyMeter)

            => Equals(EnergyMeter as IEnergyMeter);


        /// <summary>
        /// Compares two energy meters for equality.
        /// </summary>
        /// <param name="EnergyMeter">An energy meter to compare with.</param>
        public Boolean Equals(IEnergyMeter? EnergyMeter)

            => EnergyMeter is not null &&

               Id.                    Equals(EnergyMeter.Id) &&
               LastChangeDate.ToIso8601().Equals(EnergyMeter.LastChangeDate.ToIso8601()) &&

             ((Model is null && EnergyMeter.Model is null) ||
              (Model is not null && EnergyMeter.Model is not null && Model.Equals(EnergyMeter.Model))) &&

            ((!ModelURL.HasValue && !EnergyMeter.ModelURL.HasValue) ||
              (ModelURL. HasValue && EnergyMeter.ModelURL.HasValue && ModelURL.Value.Equals(EnergyMeter.ModelURL.Value))) &&

             ((HardwareVersion is null && EnergyMeter.HardwareVersion is null) ||
              (HardwareVersion is not null && EnergyMeter.HardwareVersion is not null && HardwareVersion.Equals(EnergyMeter.HardwareVersion))) &&

             ((FirmwareVersion is null && EnergyMeter.FirmwareVersion is null) ||
              (FirmwareVersion is not null && EnergyMeter.FirmwareVersion is not null && FirmwareVersion.Equals(EnergyMeter.FirmwareVersion))) &&

             ((Manufacturer    is null && EnergyMeter.Manufacturer is null) ||
              (Manufacturer    is not null && EnergyMeter.Manufacturer is not null && Manufacturer.Equals(EnergyMeter.Manufacturer))) &&

            ((!ManufacturerURL.HasValue && !EnergyMeter.ManufacturerURL.HasValue) ||
              (ManufacturerURL.HasValue && EnergyMeter.ManufacturerURL.HasValue && ManufacturerURL.Value.Equals(EnergyMeter.ManufacturerURL.Value))) &&

            ((!PublicKeyCertificateChain.HasValue && !EnergyMeter.PublicKeyCertificateChain.HasValue) ||
              (PublicKeyCertificateChain.HasValue && EnergyMeter.PublicKeyCertificateChain.HasValue && PublicKeyCertificateChain.Value.Equals(EnergyMeter.PublicKeyCertificateChain.Value))) &&

             ((Description is null && EnergyMeter.Description is null) ||
              (Description is not null && EnergyMeter.Description is not null && Description.Equals(EnergyMeter.Description))) &&

               PublicKeys.Count().Equals(EnergyMeter.PublicKeys.Count()) &&
               PublicKeys.All(publicKey => EnergyMeter.PublicKeys.Contains(publicKey)) &&

               TransparencySoftwares.Count().Equals(EnergyMeter.TransparencySoftwares.Count()) &&
               TransparencySoftwares.All(transparencySoftwareStatus => EnergyMeter.TransparencySoftwares.Contains(transparencySoftwareStatus));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Id.                        GetHashCode()        * 31 ^
                      (Model?.                    GetHashCode()  ?? 0) * 29 ^
                      (ModelURL?.                 GetHashCode()  ?? 0) * 27 ^
                      (HardwareVersion?.          GetHashCode()  ?? 0) * 23 ^
                      (FirmwareVersion?.          GetHashCode()  ?? 0) * 19 ^
                      (Manufacturer?.             GetHashCode()  ?? 0) * 17 ^
                      (ManufacturerURL?.          GetHashCode()  ?? 0) * 13 ^
                      (PublicKeys?.               CalcHashCode() ?? 0) * 11 ^
                      (PublicKeyCertificateChain?.GetHashCode()  ?? 0) * 7 ^
                      (TransparencySoftwares?.    CalcHashCode() ?? 0) * 5 ^
                      (Description?.              GetHashCode()  ?? 0) * 3 ^
                       LastChangeDate.                GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => new[] {

                   $"Id: {Id}",

                   Model.IsNotNullOrEmpty()
                       ? $"Model: {Model}"
                       : String.Empty,

                   ModelURL.HasValue
                       ? $"Model URL: {ModelURL}"
                       : String.Empty,

                   HardwareVersion.IsNotNullOrEmpty()
                       ? $"Hardware version: {HardwareVersion}"
                       : String.Empty,

                   FirmwareVersion.IsNotNullOrEmpty()
                       ? $"Firmware version: {FirmwareVersion}"
                       : String.Empty,

                   Manufacturer.IsNotNullOrEmpty()
                       ? $"Manufacturer: {Manufacturer}"
                       : String.Empty,

                   ManufacturerURL.HasValue
                       ? $"Manufacturer URL: {ManufacturerURL}"
                       : String.Empty,

                   PublicKeys.Any()
                       ? "public keys: " + PublicKeys.Select(publicKey => publicKey.ToString().SubstringMax(20)).AggregateWith(", ")
                       : String.Empty,

                   PublicKeyCertificateChain.HasValue
                       ? $"public key certificate chain: {PublicKeyCertificateChain.Value.ToString().SubstringMax(20)}"
                       : String.Empty,

                   TransparencySoftwares.Any()
                       ? $"{TransparencySoftwares.Count()} transparency software(s)"
                       : String.Empty,

                   Description is not null && Description.IsNotNullOrEmpty()
                       ? $"Description: {Description}"
                       : String.Empty,

                   $"Last change: {LastChangeDate.ToIso8601()}"

            }.Where(_ => _.IsNotNullOrEmpty()).
              AggregateWith(", ");

        #endregion


    }

}
