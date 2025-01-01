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

namespace cloud.charging.open.protocols.WWCP.MobilityProvider
{

    /// <summary>
    /// The common interface of a response message.
    /// </summary>
    public interface IResponse
    {

        /// <summary>
        /// The timestamp of the response.
        /// </summary>
        DateTime  ResponseTimestamp    { get; }

        /// <summary>
        /// The runtime of the request leading to this response.
        /// </summary>
        TimeSpan  Runtime              { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        JObject?  CustomData           { get; set; }


        //JObject ToJSON();

    }

}
