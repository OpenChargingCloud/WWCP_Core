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

using System.Collections.ObjectModel;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Security.Cryptography;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{

    public class EVCertificate
    {

        public const String DefaultContext = "https://open.charging.cloud/context/evCertificate";

        private readonly List<EVSignature> signatures;


        public EVCertificate_Id                            Id                              { get; private set; }

        public String                                      Context                         { get; }

        public String                                      Description                     { get; }

        public IEnumerable<EVPublicKey>                    PublicKeys                      { get; }

        public IEnumerable<Usages>                         Usages                          { get; }

        public DateTime                                    NotBefore                       { get; }

        public DateTime                                    NotAfter                        { get; }

        public Owner                                       Owner                           { get; }

        public IEnumerable<EVSignature>                    Signatures
            => signatures;

        public URL?                                        Policy                          { get; }

        public IEnumerable<URL>                            DistributionPoints              { get; }

        public IEnumerable<URL>                            RevocationDistributionPoints    { get; }

        public IEnumerable<URL>                            DeltaDistributionPoints         { get; }

        public IEnumerable<Tuple<Edge, EVCertificate_Id>>  Edges                           { get; }


        public EVCertificate(String                                       Description,

                             IEnumerable<EVPublicKey>                     PublicKeys,

                             IEnumerable<Usages>                          Usages,

                             DateTime                                     NotBefore,

                             DateTime                                     NotAfter,

                             Owner                                        Owner,

                             IEnumerable<EVSignature>?                    Signatures                     = null,

                             URL?                                         Policy                         = null,

                             IEnumerable<URL>?                            DistributionPoints             = null,

                             IEnumerable<URL>?                            RevocationDistributionPoints   = null,

                             IEnumerable<URL>?                            DeltaDistributionPoints        = null,

                             IEnumerable<Tuple<Edge, EVCertificate_Id>>?  Edges                          = null,

                             String?                                      Context                        = null)

        {

            this.Context                       = Context                      ?? DefaultContext;
            this.Description                   = Description;
            this.PublicKeys                    = PublicKeys.Distinct();
            this.Usages                        = Usages.    Distinct();
            this.NotBefore                     = NotBefore;
            this.NotAfter                      = NotAfter;
            this.Owner                         = Owner;
            this.signatures                    = Signatures is not null
                                                     ? new List<EVSignature>(Signatures.Distinct())
                                                     : new List<EVSignature>();
            this.Policy                        = Policy;
            this.DistributionPoints            = DistributionPoints?.          Distinct() ?? Array.Empty<URL>();
            this.RevocationDistributionPoints  = RevocationDistributionPoints?.Distinct() ?? Array.Empty<URL>();
            this.DeltaDistributionPoints       = DeltaDistributionPoints?.     Distinct() ?? Array.Empty<URL>();
            this.Edges                         = Edges is not null
                                                     ? Edges.Distinct()
                                                     : Array.Empty<Tuple<Edge, EVCertificate_Id>>();


            var cc = new Newtonsoft.Json.Converters.IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"
            };

            var json1  = JObject.Parse(ToJSON(Embedded: false).ToString(Newtonsoft.Json.Formatting.None, cc));
            json1.Remove("@id");
            json1.Remove("signatures");

            this.Id    = EVCertificate_Id.Parse(SHA256.Create().ComputeHash(json1.ToString(Newtonsoft.Json.Formatting.None, cc).ToUTF8Bytes()).ToHexString());

        }



        public JObject ToJSON(Boolean Embedded = false)
        {

            var json = JSONObject.Create(

                                 new JProperty("@id",          Id.       ToString()),

                           Embedded
                               ? null
                               : new JProperty("@context",     Context),

                           Description is not null && Description.IsNotNullOrEmpty()
                               ? new JProperty("description",  Description)
                               : null,

                                 new JProperty("publicKeys",   new JArray(PublicKeys.Select(publicKey => publicKey.ToJSON(Embedded: true)))),

                           Usages.Any()
                               ? new JProperty("usages",       new JArray(Usages.    Select(usage     => usage.    ToJSON(Embedded: true))))
                               : null,

                                 new JProperty("notBefore",    NotBefore.ToIso8601()),

                                 new JProperty("notAfter",     NotAfter. ToIso8601()),

                                 new JProperty("owner",        Owner.    ToJSON(Embedded: true)),

                           Edges.Any()
                               ? new JProperty("edges",        new JArray(Edges.     Select(edge      => new JArray(edge.Item1.ToString(),
                                                                                                                    edge.Item2.ToString()))))
                               : null,

                           Signatures.Any()
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(Embedded: true))))
                               : null

                );

            return json;

        }



        public EVCertificate Sign(ECPrivateKeyParameters  PrivateKey,
                                  String                  Name,
                                  ECCPublicKey?           PublicKey             = null,
                                  SimpleEMailAddress?     EMailAddress          = null,
                                  URL?                    WWW                   = null,
                                  DateTime?               NotBefore             = null,
                                  DateTime?               NotAfter              = null,
                                  HashingAlgorithm?       HashingAlgorithm      = null,
                                  EncryptionAlgorithm?    EncryptionAlgorithm   = null,
                                  Encoding?               Encoding              = null)
        {

            var cc = new Newtonsoft.Json.Converters.IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"
            };

            var json1       = JObject.Parse(ToJSON(Embedded: false).ToString(Newtonsoft.Json.Formatting.None, cc));
            json1.Remove("signatures");

            var sha512hash  = SHA512.Create().ComputeHash(json1.ToString(Newtonsoft.Json.Formatting.None, cc).ToUTF8Bytes());
            var blockSize   = 64;

            var signer      = SignerUtilities.GetSigner("NONEwithECDSA");
            signer.Init(true, PrivateKey);
            signer.BlockUpdate(sha512hash, 0, blockSize);

            signatures.Add(new ECCSignature(Name,
                                            PublicKey,
                                            EMailAddress,
                                            WWW,
                                            signer.GenerateSignature(),
                                            NotBefore,
                                            NotAfter,
                                            HashingAlgorithm,
                                            EncryptionAlgorithm,
                                            Encoding,
                                            this));

            return this;

        }


    }
}
