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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{

    public readonly struct Encoding
    {

        private readonly String Id;

        private Encoding(URL Id)
        {
            this.Id = Id.ToString();
        }


        public static Encoding Parse(URL Id)
            => new (Id);


        public static Encoding HEX
            => new (URL.Parse("https://open.charging.cloud/context/certificates/encoding/hex"));

        public static Encoding BASE64
            => new(URL.Parse("https://open.charging.cloud/context/certificates/encoding/base64"));


        public override String ToString()

            => Id;

    }
}
