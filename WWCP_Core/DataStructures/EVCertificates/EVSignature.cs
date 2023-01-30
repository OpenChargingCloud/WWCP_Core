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
using System.Security.Cryptography;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{


    public abstract class EVSignature
    {

        public EVCertificate?        Certificate            { get; }


        public EVSignature_Id        Id                     { get; private set; }
        public String                Name                   { get; }
        public EVPublicKey?          PublicKey              { get; }
        public SimpleEMailAddress?   EMail                  { get; }
        public URL?                  WWW                    { get; }
        public DateTime?             NotBefore              { get; }
        public DateTime?             NotAfter               { get; }
        public HashingAlgorithm?     HashingAlgorithm       { get; }
        public EncryptionAlgorithm?  EncryptionAlgorithm    { get; }
        public Encoding?             Encoding               { get; }


        public EVSignature(//String                Id,
                           String                Name,
                           EVPublicKey?          PublicKey,
                           SimpleEMailAddress?   EMail,
                           URL?                  WWW,
                           DateTime?             NotBefore,
                           DateTime?             NotAfter,
                           HashingAlgorithm?     HashingAlgorithm,
                           EncryptionAlgorithm?  EncryptionAlgorithm,
                           Encoding?             Encoding,
                           EVCertificate?        Certificate)
        {

            //this.Id                   = Id;
            this.Name                 = Name;
            this.PublicKey            = PublicKey;
            this.EMail                = EMail;
            this.WWW                  = WWW;
            this.NotBefore            = NotBefore;
            this.NotAfter             = NotAfter;
            this.HashingAlgorithm     = HashingAlgorithm;
            this.EncryptionAlgorithm  = EncryptionAlgorithm;
            this.Encoding             = Encoding;
            this.Certificate          = Certificate;

        }

        protected void CalcId()
        {

            var cc = new Newtonsoft.Json.Converters.IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ"
            };

            var json1  = JObject.Parse(ToJSON(Embedded: false).ToString(Newtonsoft.Json.Formatting.None, cc));
            json1.Remove("@id");

            this.Id    = EVSignature_Id.Parse(SHA256.Create().ComputeHash(json1.ToString(Newtonsoft.Json.Formatting.None, cc).ToUTF8Bytes()).ToHexString());

        }



        public abstract JObject ToJSON(Boolean    Embedded   = false,
                                       Encoding?  Encoding   = null);


        public abstract Boolean Verify(EVPublicKey? PublicKey = null);

    }

}
