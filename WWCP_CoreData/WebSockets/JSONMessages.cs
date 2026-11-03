/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    /// <summary>
    /// The JSON of a message as it came over a WebSocket.
    /// </summary>
    public static class JSONMessages
    {

        #region ParseArray(Text)

        /// <summary>
        /// The JSON array a text message is, with every string as it was sent.
        /// </summary>
        /// <remarks>
        /// <para>
        /// JArray.Parse reads with Newtonsoft's default DateParseHandling, which
        /// turns every string that looks like a time into a DateTime. Such a
        /// string then reaches whoever reads the message in another form:
        /// "2026-11-02T12:00:00.000Z" as "11/02/2026 12:00:00", in the value of a
        /// variable, a free text, an id. And a time given with an offset loses
        /// it to the machine's own.
        /// </para>
        /// <para>
        /// Read here with DateParseHandling.None, so that a string stays the
        /// string that was sent. A parser of a time reads it from that string,
        /// as it reads every other one. Otherwise this is what JArray.Parse is:
        /// one array, and nothing after it but comments.
        /// </para>
        /// </remarks>
        /// <param name="Text">The text of the message.</param>
        /// <exception cref="JsonReaderException">The text is no JSON array.</exception>
        public static JArray ParseArray(String Text)
        {

            using var reader = new JsonTextReader(new StringReader(Text)) {
                                   DateParseHandling = DateParseHandling.None
                               };

            var array = JArray.Load(reader);

            while (reader.Read())
                if (reader.TokenType != JsonToken.Comment)
                    throw new JsonReaderException("Additional text found in JSON string after finishing reading JSON content.");

            return array;

        }

        #endregion

    }

}
