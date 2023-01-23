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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{

    public readonly struct EncryptionAlgorithm
    {

        private readonly String Id;

        public String BouncyCastleId { get; }


        private EncryptionAlgorithm(URL     Id,
                                    String? BouncyCastleId = null)
        {

            this.Id              = Id.ToString();
            this.BouncyCastleId  = BouncyCastleId ?? String.Empty;

        }


        public static EncryptionAlgorithm Parse(URL Id)
            => new (Id);


        public static EncryptionAlgorithm SECP192r1
            => new (URL.Parse("https://open.charging.cloud/context/certificates/algorithm/secp192r1"),
                    "P-192");

        public static EncryptionAlgorithm SECP256r1
            => new (URL.Parse("https://open.charging.cloud/context/certificates/algorithm/secp256r1"),
                    "P-256");

        public static EncryptionAlgorithm SECP521r1
            => new (URL.Parse("https://open.charging.cloud/context/certificates/algorithm/secp521r1"),
                    "P-521");


        public override String ToString()

            => Id;

    }
}
