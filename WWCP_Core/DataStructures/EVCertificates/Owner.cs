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
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.EVCertificates
{

    public class Owner
    {

        public const String Context = "https://open.charging.cloud/context/certificates/owner";


        public String               Name            { get; }
        public String?              Description     { get; }
        public String?              Id              { get; }
        public SimpleEMailAddress?  EMail           { get; }
        public URL?                 WWW             { get; }

        public Owner(String               Name,
                     String?              Description   = null,
                     String?              Id            = null,
                     SimpleEMailAddress?  EMail         = null,
                     URL?                 WWW           = null)
        {

            this.Name          = Name;
            this.Description   = Description;
            this.Id            = Id;
            this.EMail         = EMail;
            this.WWW           = WWW;

        }


        public JObject ToJSON(Boolean Embedded = false)
        {

            var json = JSONObject.Create(

                           Embedded
                               ? null
                               : new JProperty("@context",      Context),

                                 new JProperty("name",          Name),

                           Description is not null && Description.IsNotNullOrEmpty()
                               ? new JProperty("description",   Description)
                               : null,

                           Id          is not null
                               ? new JProperty("@id",           Id)
                               : null,

                           EMail.      HasValue
                               ? new JProperty("eMail",         EMail.ToString())
                               : null,

                           WWW.        HasValue
                               ? new JProperty("www",           WWW.  ToString())
                               : null

                );

            return json;

        }

    }

}
