/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Bcpg;
using System.IO;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Parameters;
using org.GraphDefined.Vanaheimr.Hermod.JSON;

#endregion

namespace org.GraphDefined.WWCP
{

    public class SignedMeteringValue<T> {

        public DateTime            Timestamp        { get; }
        public T                   MeterValue       { get; }
        public EnergyMeter_Id      MeterId          { get; }
        public EVSE_Id             EVSEId           { get; }
        public AAuthentication     UserId           { get; }
        public PgpPublicKey        PublicKey        { get; }
        public String              lastSignature    { get; }
        public String              Signature        { get; private set; }


        public SignedMeteringValue(DateTime            Timestamp,
                                   T                   MeterValue,
                                   EnergyMeter_Id      MeterId,
                                   EVSE_Id             EVSEId,
                                   AAuthentication     UserId,
                                   PgpPublicKey        PublicKey,
                                   String              lastSignature  = "",
                                   String              Signature      = "")
        {

            this.Timestamp      = Timestamp;
            this.MeterValue     = MeterValue;
            this.MeterId        = MeterId;
            this.EVSEId         = EVSEId;
            this.UserId         = UserId;
            this.PublicKey      = PublicKey;
            this.lastSignature  = lastSignature != null ? lastSignature.Trim() : "";
            this.Signature      = Signature     != null ? Signature.    Trim() : "";

            if (UserId == null)
                new ArgumentNullException(nameof(UserId), "A signed meter value must have some kind of user identification!");

        }


        public JObject ToJSON()

            => JSONObject.Create(
                           new JProperty("timestamp",      Timestamp.ToIso8601()),
                           new JProperty("meterValue",     MeterValue),
                           new JProperty("meterId",        MeterId.ToString()),
                           new JProperty("evseId",         EVSEId. ToString()),
                           new JProperty("userId",         UserId),
                           new JProperty("publicKey",      PublicKey.Fingerprint.ToHexString()),
                           new JProperty("lastSignature",  lastSignature),
                           new JProperty("signature",      Signature)
                       );


        public SignedMeteringValue<T> Sign(PgpSecretKey  SecretKey,
                                           String        Passphrase)
        {

            var SignatureGenerator = new PgpSignatureGenerator(SecretKey.PublicKey.Algorithm,
                                                               HashAlgorithms.Sha512);

            SignatureGenerator.InitSign(PgpSignatureTypes.BinaryDocument,
                                        SecretKey.ExtractPrivateKey(Passphrase));

            var JSON             = ToJSON();
            var JSONText         = JSON.ToString().Replace(Environment.NewLine, " ");
            var JSONBlob         = JSON.ToUTF8Bytes();

            SignatureGenerator.Update(JSONBlob, 0, JSONBlob.Length);

            var _Signature       = SignatureGenerator.Generate();
            var OutputStream     = new MemoryStream();
            var SignatureStream  = new BcpgOutputStream(new ArmoredOutputStream(OutputStream));

            _Signature.Encode(SignatureStream);

            SignatureStream.Flush();
            SignatureStream.Close();

            OutputStream.Flush();
            OutputStream.Close();

            JSON["signature"] = this.Signature = OutputStream.ToArray().ToHexString();

            return this;

        }

    }

}
