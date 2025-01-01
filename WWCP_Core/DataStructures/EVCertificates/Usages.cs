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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{

    public abstract class Usages
    {

        public String Id { get; }

        public Usages(String Id)
        {
            this.Id = Id;
        }

        public virtual JObject ToJSON(Boolean Embedded = false)
        {

            var json = JSONObject.Create(
                           new JProperty("@id", Id)
                       );

            return json;

        }

        public override String ToString()
            => Id;

    }


    public class SignData : Usages
    {
        public SignData()

            : base("https://open.charging.cloud/context/certificates/usages/signData")

        { }

    }

    public class EncryptData : Usages
    {
        public EncryptData()

            : base("https://open.charging.cloud/context/certificates/usages/encryptData")

        { }

    }



    public class SignCertificates : Usages
    {

        public Int16? MaxPathLength { get; }

        public SignCertificates(Int16? MaxPathLength = null)

            : base("https://open.charging.cloud/context/certificates/usages/signCertificates")

        {

            this.MaxPathLength = MaxPathLength;

        }

        public override JObject ToJSON(Boolean Embedded = false)
        {

            var json = JSONObject.Create(

                                 new JProperty("@id",            Id),

                           MaxPathLength.HasValue
                               ? new JProperty("maxPathLength",  MaxPathLength.Value)
                               : null

                       );

            return json;

        }

    }

    public class RevokeCertificates : Usages
    {
        public RevokeCertificates()

            : base("https://open.charging.cloud/context/certificates/usages/revokeCertificates")

        { }

    }



    public class TLSServer : Usages
    {
        public TLSServer()

            : base("https://open.charging.cloud/context/certificates/usages/tlsServer")

        { }

    }

    public class TLSClient : Usages
    {
        public TLSClient()

            : base("https://open.charging.cloud/context/certificates/usages/tlsClient")

        { }

    }


    public class EMailSignature : Usages
    {
        public EMailSignature()

            : base("https://open.charging.cloud/context/certificates/usages/eMailSignature")

        { }

    }
    public class EMailEncryption : Usages
    {
        public EMailEncryption()

            : base("https://open.charging.cloud/context/certificates/usages/eMailEncryption")

        { }

    }

}
