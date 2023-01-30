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

using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using System.Security.Cryptography;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.Ocsp;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{


    public class ECCSignature : EVSignature
    {

        public const String Context = "https://open.charging.cloud/context/certificates/eccSignature";

        public Byte[]  R    { get; }
        public Byte[]  S    { get; }
        public Byte[]  RS   { get; }

        public ECCSignature(String                Name,
                            ECCPublicKey?         PublicKey,
                            SimpleEMailAddress?   EMailAddress,
                            URL?                  WWW,
                            Byte[]                RS,
                            DateTime?             NotBefore,
                            DateTime?             NotAfter,
                            HashingAlgorithm?     HashingAlgorithm,
                            EncryptionAlgorithm?  EncryptionAlgorithm,
                            Encoding?             Encoding,
                            EVCertificate?        Certificate)

            : base(Name,
                   PublicKey,
                   EMailAddress,
                   WWW,
                   NotBefore,
                   NotAfter,
                   HashingAlgorithm,
                   EncryptionAlgorithm,
                   Encoding,
                   Certificate)

        {

            var input  = new Asn1InputStream(RS);
            var seq    = input.ReadObject();
            this.R     = ((seq as DerSequence)![0] as DerInteger)!.Value.ToByteArray();
            this.S     = ((seq as DerSequence)![1] as DerInteger)!.Value.ToByteArray();

            this.RS    = RS;

            CalcId();

        }

        public ECCSignature(String                Name,
                            ECCPublicKey          PublicKey,
                            SimpleEMailAddress?   EMail,
                            URL?                  WWW,
                            String                R,
                            String                S,
                            DateTime?             NotBefore,
                            DateTime?             NotAfter,
                            HashingAlgorithm?     HashingAlgorithm,
                            EncryptionAlgorithm?  EncryptionAlgorithm,
                            Encoding?             Encoding,
                            EVCertificate?        Certificate)

            : base(Name,
                   PublicKey,
                   EMail,
                   WWW,
                   NotBefore,
                   NotAfter,
                   HashingAlgorithm,
                   EncryptionAlgorithm,
                   Encoding,
                   Certificate)

        {

            if (!Encoding.HasValue ||
                 Encoding.ToString() == EVCertificates.Encoding.BASE64.ToString())
            {
                this.R = R.FromBase64();
                this.S = S.FromBase64();
            }

            if ( Encoding.ToString() == EVCertificates.Encoding.HEX.   ToString())
            {
                this.R = R.HexStringToByteArray();
                this.S = S.HexStringToByteArray();
            }

            CalcId();

        }


        public override JObject ToJSON(Boolean    Embedded   = false,
                                       Encoding?  Encoding   = null)
        {

            var encoding  = Encoding ?? this.Encoding ?? EVCertificates.Encoding.BASE64;
            var r         = String.Empty;
            var s         = String.Empty;

            if (encoding.ToString() == EVCertificates.Encoding.BASE64.ToString())
            {
                r = R.ToBase64();
                s = S.ToBase64();
            }

            if (encoding.ToString() == EVCertificates.Encoding.HEX.   ToString())
            {
                r = R.ToHexString();
                s = S.ToHexString();
            }


            var json = JSONObject.Create(

                                 new JProperty("@id",                   Id.                       ToString()),

                           Embedded
                               ? null
                               : new JProperty("@context",              Context),

                                 new JProperty("name",                  Name),

                           EMail.              HasValue
                               ? new JProperty("eMail",                 EMail.                    ToString())
                               : null,

                           WWW.                HasValue
                               ? new JProperty("www",                   WWW.                      ToString())
                               : null,

                                 new JProperty("r",                     r),

                                 new JProperty("s",                     s),

                           NotBefore.          HasValue
                               ? new JProperty("notBefore",             NotBefore.          Value.ToIso8601())
                               : null,

                           NotAfter.           HasValue
                               ? new JProperty("notAfter",              NotAfter.           Value.ToIso8601())
                               : null,

                           HashingAlgorithm.   HasValue
                               ? new JProperty("hashingAlgorithm",      HashingAlgorithm.   Value.ToString())
                               : null,

                           EncryptionAlgorithm.HasValue
                               ? new JProperty("encryptionAlgorithm",   EncryptionAlgorithm.Value.ToString())
                               : null,

                           (Encoding ?? this.Encoding).HasValue
                               ? new JProperty("encoding",              (Encoding ?? this.Encoding).ToString())
                               : null

                );

            return json;

        }


        public override Boolean Verify(EVPublicKey? PublicKey = null)
        {

            var publicKey = ((this.PublicKey ?? PublicKey) as ECCPublicKey)?.PublicKey;

            //var ECParameters = NistNamedCurves.GetByName(EncryptionAlgorithm?.BouncyCastleId ?? "P-256");

            //var pubKeyParams = new ECPublicKeyParameters("ECDSA",
            //                                             ECParameters.Curve.DecodePoint(this.RS),
            //                                             new ECDomainParameters(ECParameters.Curve,
            //                                                                    ECParameters.G,
            //                                                                    ECParameters.N,
            //                                                                    ECParameters.H,
            //                                                                    ECParameters.GetSeed()));


            var cc = new Newtonsoft.Json.Converters.IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"
            };

            var json1       = JObject.Parse(this.Certificate.ToJSON(Embedded: false).ToString(Newtonsoft.Json.Formatting.None, cc));
            json1.Remove("signatures");

            var sha512hash  = SHA512.Create().ComputeHash(json1.ToString(Newtonsoft.Json.Formatting.None, cc).ToUTF8Bytes());
            var blockSize   = 64;

            var verifier = SignerUtilities.GetSigner("NONEwithECDSA");
            verifier.Init(false, publicKey);
            verifier.BlockUpdate(sha512hash, 0, blockSize);

            return verifier.VerifySignature(RS);

        }


    }

}
