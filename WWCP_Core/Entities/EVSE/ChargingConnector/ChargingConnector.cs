/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging connector to connect an electric vehicle (EV)
    /// to an Electric Vehicle Supply Equipment (EVSE).
    /// </summary>
    public class ChargingConnector : IEquatable<ChargingConnector>,
                                     IChargingConnector
    {

        // See also: https://www.worldstandards.eu/cars/connector-types/

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String  JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/chargingConnector";

        #endregion

        #region Properties

        /// <summary>
        /// The parent EVSE of this charging connector.
        /// </summary>
        [InternalUseOnly]
        public IEVSE?                          EVSE                  { get; set; }

        /// <summary>
        /// The optional charging connector identification.
        /// </summary>
        [Mandatory]
        public ChargingConnector_Id            Id                    { get; }


        [Obsolete("The 'plug' property is deprecated. Please use the 'type' property instead!")]
        public ChargingPlugTypes               Plug                  {
            get
            {

                if (Type == ChargingConnectorType.IEC_62196_T2)
                    return ChargingPlugTypes.Type2Outlet;

                if (Type == ChargingConnectorType.DOMESTIC_F_SchuKo)
                    return ChargingPlugTypes.TypeFSchuko;

                if (Type == ChargingConnectorType.CHAdeMO)
                    return ChargingPlugTypes.CHAdeMO;

                if (Type == ChargingConnectorType.IEC_62196_T2_COMBO)
                    return ChargingPlugTypes.CCSCombo2Plug_CableAttached;

                if (Type == ChargingConnectorType.IEC_62196_T1_COMBO)
                    return ChargingPlugTypes.CCSCombo1Plug_CableAttached;

                // SmallPaddleInductive,
                // LargePaddleInductive,
                // AVCONConnector,
                // 
                // TeslaConnector,
                // TESLA_Roadster,
                // TESLA_ModelS,
                // 
                // IEC60309SinglePhase,
                // IEC60309ThreePhase,
                // 
                // NEMA5_20,
                // TypeEFrenchStandard,
                // TypeGBritishStandard,
                // TypeJSwissStandard,
                //
                // Type1Connector_CableAttached,
                // Type2Outlet,
                // Type2Connector_CableAttached,
                // Type3Outlet,
                // 
                // CCSCombo1Plug_CableAttached,
                // CCSCombo2Plug_CableAttached,
                // 
                // CEE3,
                // CEE5

                return ChargingPlugTypes.Unspecified;

            }
        }


        /// <summary>
        /// The type of the charging connector.
        /// </summary>
        [Mandatory]
        public ChargingConnectorType           Type                  { get; }

        ///// <summary>
        ///// Whether the charging connector is DC or AC.
        ///// </summary>
        //[Mandatory]
        //public Boolean                         IsDC
        //    => Plug.IsDC();

        /// <summary>
        /// The optional charging cable attached.
        /// </summary>
        [Optional]
        public ChargingCable?                  ChargingCable         { get; }

        /// <summary>
        /// Whether the charging connector is lockable or not.
        /// </summary>
        [Optional]
        public Boolean?                        Lockable              { get; }

        /// <summary>
        /// Optional tariff identifications that can be used with this charging connector.
        /// </summary>
        [Optional]
        public IEnumerable<ChargingTariff_Id>  TariffIds             { get; }

        /// <summary>
        /// URL to the operator’s terms and conditions.
        /// </summary>
        /// <remarks>Ask OCPI why this is here!</remarks>
        [Optional]
        public URL?                            TermsAndConditions    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Id">A charging connector identification.</param>
        /// <param name="Type">The type of the charging connector.</param>
        /// <param name="ChargingCable">An optional charging cable attached.</param>
        /// <param name="Lockable">Whether the charging connector is lockable or not.</param>
        /// <param name="TariffIds">Optional tariff identifications that can be used with this charging connector.</param>
        /// <param name="TermsAndConditions">URL to the operator’s terms and conditions.</param>
        public ChargingConnector(ChargingConnector_Id             Id,
                                 ChargingConnectorType            Type,
                                 ChargingCable?                   ChargingCable        = null,
                                 Boolean?                         Lockable             = null,
                                 IEnumerable<ChargingTariff_Id>?  TariffIds            = null,
                                 URL?                             TermsAndConditions   = null)
        {

            this.Id                  = Id;
            this.Type                = Type;
            this.ChargingCable       = ChargingCable;
            this.Lockable            = Lockable;
            this.TariffIds           = TariffIds ?? [];
            this.TermsAndConditions  = TermsAndConditions;

            unchecked
            {

                hashCode = this.Id.                 GetHashCode()       * 17 ^
                           this.Type.               GetHashCode()       * 13 ^
                          (this.ChargingCable?.     GetHashCode() ?? 0) * 11 ^
                          (this.Lockable?.          GetHashCode() ?? 0) *  7 ^
                           this.TariffIds.          CalcHashCode()      *  5 ^
                          (this.TermsAndConditions?.GetHashCode() ?? 0) *  3 ^
                           base.                    GetHashCode();

            }

        }


        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Id">A charging connector identification.</param>
        /// <param name="Type">The type of the charging connector.</param>
        /// <param name="ChargingCable">An optional charging cable attached.</param>
        /// <param name="Lockable">Whether the charging connector is lockable or not.</param>
        /// <param name="Tariffs">The tariffs that can be used with this charging connector.</param>
        /// <param name="TermsAndConditions">URL to the operator’s terms and conditions.</param>
        public ChargingConnector(IEVSE?                           ParentEVSE,
                                 ChargingConnector_Id             Id,
                                 ChargingConnectorType            Type,
                                 ChargingCable?                   ChargingCable        = null,
                                 Boolean?                         Lockable             = null,
                                 IEnumerable<ChargingTariff_Id>?  Tariffs              = null,
                                 URL?                             TermsAndConditions   = null)
        {

            this.EVSE                = ParentEVSE;
            this.Id                  = Id;
            this.Type                = Type;
            this.ChargingCable       = ChargingCable;
            this.Lockable            = Lockable;
            this.TariffIds           = Tariffs ?? [];
            this.TermsAndConditions  = TermsAndConditions;

            unchecked
            {

                hashCode = this.Id.                 GetHashCode()       * 17 ^
                           this.Type.               GetHashCode()       * 13 ^
                          (this.ChargingCable?.     GetHashCode() ?? 0) * 11 ^
                          (this.Lockable?.          GetHashCode() ?? 0) *  7 ^
                           this.TariffIds.          CalcHashCode()      *  5 ^
                          (this.TermsAndConditions?.GetHashCode() ?? 0) *  3 ^
                           base.                    GetHashCode();

            }

        }


        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Type">The type of the charging connector.</param>
        /// <param name="ChargingCable">An optional charging cable attached.</param>
        /// <param name="Lockable">Whether the charging connector is lockable or not.</param>
        /// <param name="Tariffs">The tariffs that can be used with this charging connector.</param>
        /// <param name="TermsAndConditions">URL to the operator’s terms and conditions.</param>
        public ChargingConnector(ChargingConnectorType            Type,
                                 ChargingCable?                   ChargingCable        = null,
                                 Boolean?                         Lockable             = null,
                                 IEnumerable<ChargingTariff_Id>?  Tariffs              = null,
                                 URL?                             TermsAndConditions   = null)
        {

            this.Id                  = ChargingConnector_Id.Parse(1);
            this.Type                = Type;
            this.ChargingCable       = ChargingCable;
            this.Lockable            = Lockable;
            this.TariffIds           = Tariffs ?? [];
            this.TermsAndConditions  = TermsAndConditions;

            unchecked
            {

                hashCode = this.Id.                 GetHashCode()       * 17 ^
                           this.Type.               GetHashCode()       * 13 ^
                          (this.ChargingCable?.     GetHashCode() ?? 0) * 11 ^
                          (this.Lockable?.          GetHashCode() ?? 0) *  7 ^
                           this.TariffIds.          CalcHashCode()      *  5 ^
                          (this.TermsAndConditions?.GetHashCode() ?? 0) *  3 ^
                           base.                    GetHashCode();

            }

        }


        /// <summary>
        /// Create a new charging connector.
        /// </summary>
        /// <param name="Type">The type of the charging connector.</param>
        /// <param name="ChargingCable">An optional charging cable attached.</param>
        /// <param name="Lockable">Whether the charging connector is lockable or not.</param>
        /// <param name="Tariffs">The tariffs that can be used with this charging connector.</param>
        /// <param name="TermsAndConditions">URL to the operator’s terms and conditions.</param>
        public ChargingConnector(IEVSE?                           ParentEVSE,
                                 ChargingConnectorType            Type,
                                 ChargingCable?                   ChargingCable        = null,
                                 Boolean?                         Lockable             = null,
                                 IEnumerable<ChargingTariff_Id>?  Tariffs              = null,
                                 URL?                             TermsAndConditions   = null)
        {

            this.EVSE                = ParentEVSE;
            this.Id                  = ChargingConnector_Id.Parse(1);
            this.Type                = Type;
            this.ChargingCable       = ChargingCable;
            this.Lockable            = Lockable;
            this.TariffIds           = Tariffs ?? [];
            this.TermsAndConditions  = TermsAndConditions;

            unchecked
            {

                hashCode = this.Id.                 GetHashCode()       * 17 ^
                           this.Type.               GetHashCode()       * 13 ^
                          (this.ChargingCable?.     GetHashCode() ?? 0) * 11 ^
                          (this.Lockable?.          GetHashCode() ?? 0) *  7 ^
                           this.TariffIds.          CalcHashCode()      *  5 ^
                          (this.TermsAndConditions?.GetHashCode() ?? 0) *  3 ^
                           base.                    GetHashCode();

            }

        }

        #endregion


        #region (static) Parse    (JSON, CustomChargingConnectorParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging connector.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargingConnectorParser">An optional delegate to parse custom ChargingConnector JSON objects.</param>
        /// <param name="CustomChargingCableParser">An optional delegate to parse custom ChargingCable JSON objects.</param>
        public static ChargingConnector Parse(JObject                                          JSON,
                                              CustomJObjectParserDelegate<ChargingConnector>?  CustomChargingConnectorParser   = null,
                                              CustomJObjectParserDelegate<ChargingCable>?      CustomChargingCableParser       = null)
        {

            if (TryParse(JSON,
                         out var chargingConnector,
                         out var errorResponse,
                         CustomChargingConnectorParser))
            {
                return chargingConnector;
            }

            throw new ArgumentException("The given JSON representation of a charging connector is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ChargingConnector, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a charging connector.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingConnector">The parsed charging connector.</param>
        public static Boolean TryParse(JObject                                      JSON,
                                       [NotNullWhen(true)]  out ChargingConnector?  ChargingConnector,
                                       [NotNullWhen(false)] out String?             ErrorResponse)

            => TryParse(JSON,
                        out ChargingConnector,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a charging connector.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingConnector">The parsed charging connector.</param>
        /// <param name="CustomChargingConnectorParser">An optional delegate to parse custom ChargingConnector JSON objects.</param>
        /// <param name="CustomChargingCableParser">An optional delegate to parse custom ChargingCable JSON objects.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       [NotNullWhen(true)]  out ChargingConnector?      ChargingConnector,
                                       [NotNullWhen(false)] out String?                 ErrorResponse,
                                       CustomJObjectParserDelegate<ChargingConnector>?  CustomChargingConnectorParser   = null,
                                       CustomJObjectParserDelegate<ChargingCable>?      CustomChargingCableParser       = null)
        {

            try
            {

                ChargingConnector = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Id                    [mandatory]

                if (!JSON.ParseMandatory("@id",
                                         "charging connector identification",
                                         ChargingConnector_Id.TryParse,
                                         out ChargingConnector_Id id,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Type                  [mandatory]

                if (!JSON.ParseMandatory("type",
                                         "charging connector type",
                                         ChargingConnectorType.TryParse,
                                         out ChargingConnectorType type,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingCable         [optional]

                if (JSON.ParseOptionalJSON("cable",
                                           "charging cable",
                                           ChargingCable.TryParse,
                                           out ChargingCable? chargingCable,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Lockable              [optional]

                if (JSON.ParseOptional("lockable",
                                       "lockable",
                                       out Boolean? lockable,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TariffIds             [optional]

                if (JSON.ParseOptionalHashSet("tariffIds",
                                              "charging tariff identifications",
                                              ChargingTariff_Id.TryParse,
                                              out HashSet<ChargingTariff_Id>? tariffIds,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse TermsAndConditions    [optional]

                if (JSON.ParseOptional("termsAndConditions",
                                       "terms and conditions",
                                       URL.TryParse,
                                       out URL? termsAndConditions,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ChargingConnector = new ChargingConnector(
                                        id,
                                        type,
                                        chargingCable,
                                        lockable,
                                        tariffIds,
                                        termsAndConditions
                                    );

                if (CustomChargingConnectorParser is not null)
                    ChargingConnector = CustomChargingConnectorParser(JSON,
                                                                      ChargingConnector);

                return true;

            }
            catch (Exception e)
            {
                ChargingConnector  = default;
                ErrorResponse        = "The given JSON representation of a ChargingConnector is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSONNew(Embedded = false, CustomChargingConnectorSerializer = null)

        public JObject? ToJSONNew(Boolean                                              Embedded                            = false,
                                  CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null,
                                  CustomJObjectSerializerDelegate<ChargingCable>?      CustomChargingCableSerializer       = null)
        {

            try
            {

                var json = JSONObject.Create(

                                     new JProperty("@id",                  Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context",             JSONLDContext)
                                   : null,

                                     //new JProperty("plug",                 Type.         ToString()), // legacy!!!
                                     new JProperty("type",                 Type.         ToString()),

                               ChargingCable is not null
                                   ? new JProperty("cable",                ChargingCable.ToJSON(Embedded:  true,
                                                                                                CustomChargingCableSerializer))
                                   : null,

                               TariffIds.Any()
                                   ? new JProperty("tariffIds",            new JArray(TariffIds.Select(tariffId => tariffId.ToString())))
                                   : null,

                               TermsAndConditions.HasValue
                                   ? new JProperty("termsAndConditions",   TermsAndConditions.Value.ToString())
                                   : null

                           );

                return CustomChargingConnectorSerializer is not null
                           ? CustomChargingConnectorSerializer(this, json)
                           : json;

            }
            catch (Exception e)
            {
                return new JObject(
                           new JProperty("@id",         Id.ToString()),
                           new JProperty("@context",    JSONLDContext),
                           new JProperty("exception",   e.Message),
                           new JProperty("stackTrace",  e.StackTrace)
                       );
            }

        }

        #endregion

        #region ToJSON(Embedded = false, ...)  [Obsolete!]

        [Obsolete("The 'plug' property is deprecated. Please use the 'type' property instead!")]
        public JObject? ToJSON(Boolean                                              Embedded                            = false,
                               CustomJObjectSerializerDelegate<ChargingConnector>?  CustomChargingConnectorSerializer   = null,
                               CustomJObjectSerializerDelegate<ChargingCable>?      CustomChargingCableSerializer       = null)
        {

            try
            {

                var json = JSONObject.Create(

                                     new JProperty("@id",             Id.ToString()),

                               !Embedded
                                   ? new JProperty("@context",        JSONLDContext)
                                   : null,

                                     new JProperty("plug",            Plug.                       ToString()),
                                     new JProperty("cableAttached",   (ChargingCable is not null).ToString())

                               //CableLength.HasValue
                               //    ? new JProperty("cableLength",     CableLength.Value)
                               //    : null

                               );

                return CustomChargingConnectorSerializer is not null
                           ? CustomChargingConnectorSerializer(this, json)
                           : json;

            }
            catch (Exception e)
            {
                return new JObject(
                           new JProperty("@id",         Id.ToString()),
                           new JProperty("@context",    JSONLDContext),
                           new JProperty("exception",   e.Message),
                           new JProperty("stackTrace",  e.StackTrace)
                       );
            }

        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingConnector1, ChargingConnector2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnector1">A charging connector.</param>
        /// <param name="ChargingConnector2">Another charging connector.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingConnector ChargingConnector1,
                                           ChargingConnector ChargingConnector2)
        {

            if (ReferenceEquals(ChargingConnector1, ChargingConnector2))
                return true;

            if (ChargingConnector1 is null || ChargingConnector2 is null)
                return false;

            return ChargingConnector1.Equals(ChargingConnector2);

        }

        #endregion

        #region Operator != (ChargingConnector1, ChargingConnector2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingConnector1">A charging connector.</param>
        /// <param name="ChargingConnector2">Another charging connector.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingConnector ChargingConnector1,
                                           ChargingConnector ChargingConnector2)

            => !(ChargingConnector1 == ChargingConnector2);

        #endregion

        #endregion

        #region IEquatable<ChargingConnector> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging connectors for equality.
        /// </summary>
        /// <param name="Object">A charging connector to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingConnector chargingConnector &&
                   Equals(chargingConnector);

        #endregion

        #region Equals(ChargingConnector)

        /// <summary>
        /// Compares two charging connectors for equality.
        /// </summary>
        /// <param name="ChargingConnector">A charging connector to compare with.</param>
        public Boolean Equals(ChargingConnector? ChargingConnector)

            => ChargingConnector is not null &&

               Id.  Equals(ChargingConnector.Id)   &&
               Type.Equals(ChargingConnector.Type) &&

             ((!Lockable.     HasValue    && !ChargingConnector.Lockable.     HasValue) ||
               (Lockable.     HasValue    &&  ChargingConnector.Lockable.     HasValue    && Lockable.     Equals(ChargingConnector.Lockable))) &&

              ((ChargingCable is     null &&  ChargingConnector.ChargingCable is     null ) ||
               (ChargingCable is not null &&  ChargingConnector.ChargingCable is not null && ChargingCable.Equals(ChargingConnector.ChargingCable)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   $"{Id}: {Type}",

                   Lockable.HasValue
                       ? ", lockable "
                       : "",

                   ChargingCable is not null
                       ? $", with {ChargingCable.Length} m cable"
                       : "",

                   TariffIds.Any()
                       ? $", with tariffIds: {TariffIds.AggregateWith(", ")}"
                       : "",

                   TermsAndConditions.HasValue
                       ? $", t&c: {TermsAndConditions.Value}"
                       : ""

               );

        #endregion

    }

}
