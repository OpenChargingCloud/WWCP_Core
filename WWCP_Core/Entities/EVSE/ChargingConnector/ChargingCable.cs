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
    /// A charging cable with loss compensation characteristics.
    /// </summary>
    public class ChargingCable : IEquatable<ChargingCable>
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String  JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/chargingCable";

        #endregion

        #region Properties

        /// <summary>
        /// The optional length of the charging cable.
        /// </summary>
        [Optional]
        public Meter?   Length                            { get; }

        /// <summary>
        /// The optional resistance of the charging cable, which is used to calculate the
        /// voltage drop and power loss during charging and how to compensate for it.
        /// </summary>
        [Optional]
        public Ohm?     Resistance                        { get; }

        /// <summary>
        /// A meter can use this value for adding a traceability text for justifying cable loss characteristics.
        /// </summary>
        [Optional]
        public String?  LossCompensationName              { get; }

        /// <summary>
        /// A meter can use this value for adding a traceability ID number for justifying cable loss
        /// characteristics from a lookup table specified in meter's documentation.
        /// </summary>
        [Optional]
        public Int64?   LossCompensationIdentification    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging cable.
        /// </summary>
        /// <param name="Length">The optional length of the charging cable.</param>
        /// <param name="Resistance">The optional resistance of the charging cable.</param>
        /// <param name="LossCompensationName">A traceability text for justifying cable loss characteristics.</param>
        /// <param name="LossCompensationIdentification">A traceability ID number for justifying cable loss characteristics.</param>
        public ChargingCable(Meter?   Length                           = null,
                             Ohm?     Resistance                       = null,
                             String?  LossCompensationName             = null,
                             Int64?   LossCompensationIdentification   = null)
        {

            this.Length                          = Length;
            this.Resistance                      = Resistance;
            this.LossCompensationName            = LossCompensationName;
            this.LossCompensationIdentification  = LossCompensationIdentification;

            unchecked
            {

                hashCode = (this.Length?.                        GetHashCode() ?? 0) * 7 ^
                           (this.Resistance?.                    GetHashCode() ?? 0) * 5 ^
                           (this.LossCompensationName?.          GetHashCode() ?? 0) * 3 ^
                            this.LossCompensationIdentification?.GetHashCode() ?? 0;

            }

        }

        #endregion


        #region (static) Parse    (JSON, CustomChargingCableParser = null)

        /// <summary>
        /// Parse the given JSON representation of a charging cable.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomChargingCableParser">An optional delegate to parse custom ChargingCable JSON objects.</param>
        public static ChargingCable Parse(JObject                                      JSON,
                                          CustomJObjectParserDelegate<ChargingCable>?  CustomChargingCableParser   = null)
        {

            if (TryParse(JSON,
                         out var chargingCable,
                         out var errorResponse,
                         CustomChargingCableParser))
            {
                return chargingCable;
            }

            throw new ArgumentException("The given JSON representation of a charging cable is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ChargingCable, out ErrorResponse, CustomChargingCableParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a charging cable.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingCable">The parsed charging cable.</param>
        public static Boolean TryParse(JObject                                  JSON,
                                       [NotNullWhen(true)]  out ChargingCable?  ChargingCable,
                                       [NotNullWhen(false)] out String?         ErrorResponse)

            => TryParse(JSON,
                        out ChargingCable,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a charging cable.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ChargingCable">The parsed charging cable.</param>
        /// <param name="CustomChargingCableParser">An optional delegate to parse custom ChargingCable JSON objects.</param>
        public static Boolean TryParse(JObject                                      JSON,
                                       [NotNullWhen(true)]  out ChargingCable?      ChargingCable,
                                       [NotNullWhen(false)] out String?             ErrorResponse,
                                       CustomJObjectParserDelegate<ChargingCable>?  CustomChargingCableParser   = null)
        {

            try
            {

                ChargingCable = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Length                            [optional]

                if (!JSON.ParseOptional("length",
                                        "charging cable length",
                                        Meter.TryParse,
                                        out Meter? length,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Resistance                        [optional]

                if (!JSON.ParseOptional("resistance",
                                        "charging cable resistance",
                                        Ohm.TryParse,
                                        out Ohm? resistance,
                                        out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse LossCompensationName              [optional]

                var lossCompensationName = JSON["lossCompensationName"]?.Value<String>();

                #endregion

                #region Parse LossCompensationIdentification    [optional]

                if (JSON.ParseOptional("lossCompensationIdentification",
                                       "loss compensation identification",
                                       out Int64? lossCompensationIdentification,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ChargingCable = new ChargingCable(
                                        length,
                                        resistance,
                                        lossCompensationName,
                                        lossCompensationIdentification
                                    );

                if (CustomChargingCableParser is not null)
                    ChargingCable = CustomChargingCableParser(JSON,
                                                              ChargingCable);

                return true;

            }
            catch (Exception e)
            {
                ChargingCable  = default;
                ErrorResponse  = "The given JSON representation of a ChargingCable is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(Embedded = false, CustomChargingCableSerializer = null)

        public JObject? ToJSON(Boolean                                          Embedded                        = false,
                               CustomJObjectSerializerDelegate<ChargingCable>?  CustomChargingCableSerializer   = null)
        {

            var json = JSONObject.Create(

                        !Embedded
                            ? new JProperty("@context",                         JSONLDContext)
                            : null,

                        Length.    HasValue
                            ? new JProperty("length",                           Length.    Value.m)
                            : null,

                        Resistance.HasValue
                            ? new JProperty("resistance",                       Resistance.Value.µΩ)
                            : null,

                        LossCompensationName.IsNotNullOrEmpty()
                            ? new JProperty("lossCompensationName",             LossCompensationName)
                            : null,

                        LossCompensationIdentification.HasValue
                            ? new JProperty("lossCompensationIdentification",   LossCompensationIdentification.Value)
                            : null

                        );

            return CustomChargingCableSerializer is not null
                        ? CustomChargingCableSerializer(this, json)
                        : json;

        }

        #endregion


        public static ChargingCable WithLength(Meter Meters)

            => new (
                   Meters
               );


        #region Operator overloading

        #region Operator == (ChargingCable1, ChargingCable2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingCable1">A charging connector.</param>
        /// <param name="ChargingCable2">Another charging connector.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingCable ChargingCable1,
                                           ChargingCable ChargingCable2)
        {

            if (ReferenceEquals(ChargingCable1, ChargingCable2))
                return true;

            if (ChargingCable1 is null || ChargingCable2 is null)
                return false;

            return ChargingCable1.Equals(ChargingCable2);

        }

        #endregion

        #region Operator != (ChargingCable1, ChargingCable2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingCable1">A charging connector.</param>
        /// <param name="ChargingCable2">Another charging connector.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingCable ChargingCable1,
                                           ChargingCable ChargingCable2)

            => !(ChargingCable1 == ChargingCable2);

        #endregion

        #endregion

        #region IEquatable<ChargingCable> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging connectors for equality.
        /// </summary>
        /// <param name="Object">A charging connector to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingCable chargingCable &&
                   Equals(chargingCable);

        #endregion

        #region Equals(ChargingCable)

        /// <summary>
        /// Compares two charging connectors for equality.
        /// </summary>
        /// <param name="ChargingCable">A charging connector to compare with.</param>
        public Boolean Equals(ChargingCable? ChargingCable)

            => ChargingCable is not null &&

               Length.Equals(ChargingCable.Length) &&

             ((!Resistance.HasValue && !ChargingCable.Resistance.HasValue) ||
               (Resistance.HasValue &&  ChargingCable.Resistance.HasValue && Resistance.Equals(ChargingCable.Resistance)));

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

                   $"{Length} m ",

                   Resistance.HasValue
                       ? $", {Resistance.Value.µΩ} µΩ"
                       : ""

               );

        #endregion

    }

}
