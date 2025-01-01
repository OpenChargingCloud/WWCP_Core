/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class EnergyMeteringValue(DateTime                   Timestamp,
                                     WattHour                   WattHours,
                                     EnergyMeteringValueTypes?  Type,
                                     String?                    SignedData   = null,
                                     String?                    Signature    = null)
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of the object.
        /// </summary>
        public const String JSONLDContext  = "https://open.charging.cloud/contexts/wwcp+json/energyMeteringValue";

        #endregion

        #region Properties

        public DateTime                   Timestamp     { get; } = Timestamp;
        public WattHour                   WattHours     { get; } = WattHours;
        public EnergyMeteringValueTypes?  Type          { get; } = Type;
        public String?                    SignedData    { get; } = SignedData;
        public String?                    Signature     { get; } = Signature;

        #endregion


        #region ToJSON(Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation of the given energy metering value.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        /// <param name="CustomEnergyMeteringValueSerializer">A custom energy metering value serializer.</param>
        public JObject ToJSON(Boolean                                                Embedded                              = false,
                              CustomJObjectSerializerDelegate<EnergyMeteringValue>?  CustomEnergyMeteringValueSerializer   = null)
        {

            var json = JSONObject.Create(

                           Embedded
                               ? null
                               : new JProperty("@context",     JSONLDContext),

                                 new JProperty("timestamp",    Timestamp.ToIso8601()),
                                 new JProperty("value",        WattHours.kWh),

                           Type.HasValue
                               ? new JProperty("type",         Type.Value.AsText())
                               : null,

                           SignedData is not null && SignedData.IsNotNullOrEmpty()
                               ? new JProperty("signedData",   SignedData)
                               : null,

                           Signature  is not null && Signature. IsNotNullOrEmpty()
                               ? new JProperty("signature",    Signature)
                               : null

                       );

            return CustomEnergyMeteringValueSerializer is not null
                       ? CustomEnergyMeteringValueSerializer(this, json)
                       : json;

        }

        #endregion

        #region TryParse(JSON, out EnergyMeteringValue, out ErrorResponse)

        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out EnergyMeteringValue?  EnergyMeteringValue,
                                       [NotNullWhen(false)] out String?               ErrorResponse)
        {

            EnergyMeteringValue = null;

            try
            {

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Context       [optional]

                var context = JSON["@context"]?.Value<String>();

                #endregion

                #region Parse Timestamp     [mandatory]

                if (!JSON.ParseMandatory("timestamp",
                                         "timestamp",
                                         out DateTime Timestamp,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse WattHours     [mandatory]

                var valueString = JSON["value"]?.Value<String>() ?? "";

                if (!WattHour.TryParse(valueString, out var Value))
                {
                    ErrorResponse = $"Invalid energy metering value '{valueString}'!";
                    return false;
                }

                #endregion

                #region Parse Type          [optional]

                if (JSON.ParseOptional("type",
                                       "type",
                                       EnergyMeteringValueTypesExtensions.TryParse,
                                       out EnergyMeteringValueTypes? Type,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse SignedData    [optional]

                var SignedData = JSON["signedData"]?.Value<String>() ?? "";

                #endregion

                #region Parse Signature     [optional]

                var Signature  = JSON["signature"]?.Value<String>() ?? "";

                #endregion


                EnergyMeteringValue = new EnergyMeteringValue(
                                          Timestamp,
                                          Value,
                                          Type,
                                          SignedData,
                                          Signature
                                      );

                return true;

            }
            catch (Exception e)
            {
                EnergyMeteringValue  = null;
                ErrorResponse        = "The given JSON representation of an energy metering value is invalid: " + e.Message;
            }

            return false;

        }

        #endregion



        //public SignedMeteringValue<T> Sign(PgpSecretKey  SecretKey,
        //                                   String        Passphrase)
        //{

        //    var SignatureGenerator = new PgpSignatureGenerator(SecretKey.PublicKey.Algorithm,
        //                                                       HashAlgorithmTag.Sha512);

        //    SignatureGenerator.InitSign(PgpSignature.BinaryDocument,
        //                                SecretKey.ExtractPrivateKey(Passphrase.ToCharArray()));

        //    var JSON             = ToJSON();
        //    var JSONText         = JSON.ToString().Replace(Environment.NewLine, " ");
        //    var JSONBlob         = JSON.ToUTF8Bytes();

        //    SignatureGenerator.Update(JSONBlob, 0, JSONBlob.Length);

        //    var _Signature       = SignatureGenerator.Generate();
        //    var OutputStream     = new MemoryStream();
        //    var SignatureStream  = new BcpgOutputStream(new ArmoredOutputStream(OutputStream));

        //    _Signature.Encode(SignatureStream);

        //    SignatureStream.Flush();
        //    SignatureStream.Close();

        //    OutputStream.Flush();
        //    OutputStream.Close();

        //    JSON["signature"] = this.Signature = OutputStream.ToArray().ToHexString();

        //    return this;

        //}

    }

}
