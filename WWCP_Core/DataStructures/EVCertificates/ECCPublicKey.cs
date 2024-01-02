/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{

    public class ECCPublicKey : EVPublicKey
    {

        public const String Context = "https://open.charging.cloud/context/certificates/eccPublicKey";

        public String                 X
            => PublicKey.Q.XCoord.ToBigInteger().ToByteArray().ToBase64();

        public String                 Y
            => PublicKey.Q.YCoord.ToBigInteger().ToByteArray().ToBase64();

        public String                 XY
            => PublicKey.Q.GetEncoded().ToBase64();

        public ECPublicKeyParameters  PublicKey    { get; }



        public ECCPublicKey(String                X,
                            String                Y,
                            EncryptionAlgorithm?  Algorithm   = null,
                            Encoding?             Encoding    = null)

            : base(Algorithm,
                   Encoding)

        {

            var x    = Array.Empty<Byte>();
            var y    = Array.Empty<Byte>();

            if (!Encoding.HasValue ||
                 Encoding. ToString() == EVCertificates.Encoding.BASE64.ToString())
            {
                x = X.FromBase64();
                y = Y.FromBase64();
            }

            if ( Encoding?.ToString() == EVCertificates.Encoding.HEX.   ToString())
            {
                x = X.HexStringToByteArray();
                y = Y.HexStringToByteArray();
            }

            var encodedPublicKey = new Byte[1 + x.Length + y.Length];
            encodedPublicKey[0] = 0x04;
            Array.Copy(x, 0, encodedPublicKey, 1,            x.Length);
            Array.Copy(y, 0, encodedPublicKey, 1 + x.Length, y.Length);

            var ECParameters = NistNamedCurves.GetByName(Algorithm?.BouncyCastleId ?? "P-256");

            this.PublicKey = new("ECDSA",
                                 ECParameters.Curve.DecodePoint(encodedPublicKey),
                                 new ECDomainParameters(ECParameters.Curve,
                                                        ECParameters.G,
                                                        ECParameters.N,
                                                        ECParameters.H,
                                                        ECParameters.GetSeed()));

        }

        public ECCPublicKey(Byte[]                EncodedPublicKey,
                            EncryptionAlgorithm?  EncryptionAlgorithm   = null,
                            Encoding?             Encoding              = null)

            : base(EncryptionAlgorithm,
                   Encoding)

        {

            var ECParameters = NistNamedCurves.GetByName(EncryptionAlgorithm?.BouncyCastleId ?? "P-256");

            this.PublicKey = new ("ECDSA",
                                  ECParameters.Curve.DecodePoint(EncodedPublicKey),
                                  new ECDomainParameters(ECParameters.Curve,
                                                         ECParameters.G,
                                                         ECParameters.N,
                                                         ECParameters.H,
                                                         ECParameters.GetSeed()));

        }

        public ECCPublicKey(String                EncodedPublicKey,
                            EncryptionAlgorithm?  Algorithm   = null,
                            Encoding?             Encoding    = null)

            : base(Algorithm,
                   Encoding)

        {

            var ECParameters  = NistNamedCurves.GetByName(Algorithm?.BouncyCastleId ?? "P-256");
            var data          = Array.Empty<Byte>();

            if (!Encoding.HasValue ||
                 Encoding. ToString() == EVCertificates.Encoding.BASE64.ToString())
                data = EncodedPublicKey.FromBase64();

            if ( Encoding?.ToString() == EVCertificates.Encoding.HEX.   ToString())
                data = EncodedPublicKey.HexStringToByteArray();

            this.PublicKey = new("ECDSA",
                                 ECParameters.Curve.DecodePoint(data),
                                 new ECDomainParameters(ECParameters.Curve,
                                                        ECParameters.G,
                                                        ECParameters.N,
                                                        ECParameters.H,
                                                        ECParameters.GetSeed()));

        }


        public override JObject ToJSON(Boolean    Embedded   = false,
                                       Encoding?  Encoding   = null)
        {

            var encoding  = Encoding ?? this.Encoding ?? EVCertificates.Encoding.BASE64;
            var x         = String.Empty;
            var y         = String.Empty;

            if (encoding.ToString() == EVCertificates.Encoding.BASE64.ToString())
            {
                x = PublicKey.Q.XCoord.ToBigInteger().ToByteArray().ToBase64();
                y = PublicKey.Q.YCoord.ToBigInteger().ToByteArray().ToBase64();
            }

            if (encoding.ToString() == EVCertificates.Encoding.HEX.   ToString())
            {
                x = PublicKey.Q.XCoord.ToBigInteger().ToByteArray().ToHexString();
                y = PublicKey.Q.YCoord.ToBigInteger().ToByteArray().ToHexString();
            }

            var json = JSONObject.Create(

                           Embedded
                               ? null
                               : new JProperty("@context",   Context),

                                 new JProperty("x",          x),
                                 new JProperty("y",          y),

                           EncryptionAlgorithm.HasValue //&& EncryptionAlgorithm.Value.ToString() != EVCertificates.EncryptionAlgorithm.SECP521r1.ToString()
                               ? new JProperty("algorithm",  EncryptionAlgorithm.Value.  ToString())
                               : null,

                           (Encoding ?? this.Encoding).HasValue
                               ? new JProperty("encoding",   (Encoding ?? this.Encoding).ToString())
                               : null

                       );

            return json;

        }

    }

}
