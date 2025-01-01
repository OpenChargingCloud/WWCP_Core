///*
// * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using Newtonsoft.Json.Linq;

//using org.GraphDefined.Vanaheimr.Hermod.Mail;
//using org.GraphDefined.Vanaheimr.Hermod.HTTP;
//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace cloud.charging.open.protocols.WWCP.EVCertificates
//{


//    public class RSASignature : EVSignature
//    {

//        public const String Context = "https://open.charging.cloud/context/certificates/rsaSignature";

//        public String  Data    { get; }

//        public RSASignature(String                Name,
//                            RSAPublicKey          PublicKey,
//                            SimpleEMailAddress?   EMail,
//                            URL?                  WWW,
//                            String                Data,
//                            DateTime?             NotBefore,
//                            DateTime?             NotAfter,
//                            HashingAlgorithm?     HashingAlgorithm,
//                            EncryptionAlgorithm?  EncryptionAlgorithm,
//                            Encoding?             Encoding,
//                            EVCertificate?        Certificate)

//            : base(Name,
//                   PublicKey,
//                   EMail,
//                   WWW,
//                   NotBefore,
//                   NotAfter,
//                   HashingAlgorithm,
//                   EncryptionAlgorithm,
//                   Encoding,
//                   Certificate)

//        {

//            this.Data = Data;

//            CalcId();

//        }


//        public override JObject ToJSON(Boolean    Embedded   = false,
//                                       Encoding?  Encoding   = null)
//        {

//            var json = JSONObject.Create(

//                                 new JProperty("@id",                   Id.                       ToString()),

//                           Embedded
//                               ? null
//                               : new JProperty("@context",              Context),

//                                 new JProperty("name",                  Name),

//                           EMail.HasValue
//                               ? new JProperty("eMail",                 EMail.                    ToString())
//                               : null,

//                           WWW.         HasValue
//                               ? new JProperty("www",                   WWW.                      ToString())
//                               : null,

//                                 new JProperty("data",                  Data),

//                           NotBefore.   HasValue
//                               ? new JProperty("notBefore",             NotBefore.          Value.ToIso8601())
//                               : null,

//                           NotAfter.    HasValue
//                               ? new JProperty("notAfter",              NotAfter.           Value.ToIso8601())
//                               : null,

//                           HashingAlgorithm.   HasValue
//                               ? new JProperty("hashingAlgorithm",      HashingAlgorithm.   Value.ToString())
//                               : null,

//                           EncryptionAlgorithm.HasValue
//                               ? new JProperty("encryptionAlgorithm",   EncryptionAlgorithm.Value.ToString())
//                               : null,

//                           Encoding.           HasValue
//                               ? new JProperty("encoding",              Encoding.           Value.ToString())
//                               : null

//                );

//            return json;

//        }


//        public override Boolean Verify(EVPublicKey? PublicKey = null)
//        {
//            return false;
//        }

//    }

//}
