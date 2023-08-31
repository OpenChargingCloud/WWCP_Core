/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class RootCAInfo
    {

        public I18NString             Name         { get; }
        public ECPublicKeyParameters  PublicKey    { get; }
        public DateTime               NotBefore    { get; }
        public DateTime               NotAfter     { get; }
        public String                 Algorithm    { get; }
        public I18NString             Comment      { get; }

        public RootCAInfo(I18NString             Name,
                          ECPublicKeyParameters  PublicKey,
                          DateTime               NotBefore,
                          DateTime               NotAfter,
                          String                 Algorithm   = "P-256",
                          I18NString?            Comment     = null)
        {

            this.Name       = Name;
            this.PublicKey  = PublicKey;
            this.NotBefore  = NotBefore;
            this.NotAfter   = NotAfter;
            this.Algorithm  = Algorithm ?? "P-256";
            this.Comment    = Comment   ?? I18NString.Empty;

        }

    }

}
