/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class EnergyMeteringValue
    {

        public DateTime                   Timestamp     { get; }
        public Decimal                    Value         { get; }
        public EnergyMeteringValueTypes?  Type          { get; }
        public String?                    SignedData    { get; }
        public String?                    Signature     { get; }


        public EnergyMeteringValue(DateTime                   Timestamp,
                                   Decimal                    Value,
                                   EnergyMeteringValueTypes?  Type,
                                   String?                    SignedData   = null,
                                   String?                    Signature    = null)
        {

            this.Timestamp   = Timestamp;
            this.Value       = Value;
            this.Type        = Type;
            this.SignedData  = SignedData;
            this.Signature   = Signature;

        }


        public JObject ToJSON()
        {

            var json = JSONObject.Create(

                                 new JProperty("timestamp",    Timestamp.ToIso8601()),
                                 new JProperty("value",        Value),

                           Type.HasValue
                               ? new JProperty("type",         Type.Value.AsText())
                               : null,

                           SignedData is not null && SignedData.IsNotNullOrEmpty()
                               ? new JProperty("signedData",   SignedData)
                               : null,

                           Signature is not null && SignedData.IsNotNullOrEmpty()
                               ? new JProperty("signature",    Signature)
                               : null

                       );

            return json;

        }


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
