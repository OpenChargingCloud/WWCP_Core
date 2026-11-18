/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A general result.
    /// </summary>
    public interface IResult : IEquatable<IResult>
    {

        /// <summary>
        /// The optional human-readable error description.
        /// </summary>
        String?     Description       { get; }

        /// <summary>
        /// Optional error details.
        /// </summary>
        JObject?    Details           { get; }

        /// <summary>
        /// The optional response message.
        /// </summary>
        JObject?    Response          { get; }

        /// <summary>
        /// The optional binary response message.
        /// </summary>
        Byte[]?     BinaryResponse    { get; }

    }

}
