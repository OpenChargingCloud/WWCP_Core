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

using System.Collections.Concurrent;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Hermod;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// A string that looks like a time reaches whoever reads a message as the
    /// string that was sent - not as a DateTime written in the machine's
    /// culture, and not with the machine's offset for its own.
    /// </summary>
    /// <remarks>
    /// Found with the clock of OCPP 2.1: a CSMS read the variable value
    /// "2026-11-02T12:00:00.000Z" as "11/02/2026 12:00:00" (NTS concept B.16).
    /// </remarks>
    [TestFixture]
    public class DateLikeStringsTests
    {

        #region (class) Node

        /// <summary>
        /// A networking node of no particular kind.
        /// </summary>
        private sealed class Node : AWWCPNetworkingNode
        {
            public Node()
                : base(NetworkingNode_Id.Parse("csms"))
            { }
        }

        #endregion

        #region Data

        private const String  UTC         = "2026-11-02T12:00:00.000Z";
        private const String  WithOffset  = "2026-11-02T13:00:00+01:00";

        #endregion


        #region AMessageIsReadWithItsStringsAsTheyWereSent()

        /// <summary>
        /// The JSON of a message keeps every string as it was sent, a time in UTC
        /// and one with an offset alike.
        /// </summary>
        [Test]
        public void AMessageIsReadWithItsStringsAsTheyWereSent()
        {

            var message = JSONMessages.ParseArray($$"""[ 2, "42", "SetVariables", { "value": "{{UTC}}", "other": "{{WithOffset}}" } ]""");

            Assert.Multiple(() => {
                Assert.That(message[3]!["value"]!.Type,             Is.EqualTo(JTokenType.String));
                Assert.That(message[3]!.Value<String>("value"),     Is.EqualTo(UTC));
                Assert.That(message[3]!.Value<String>("other"),     Is.EqualTo(WithOffset));
                Assert.That(message.ToString(Formatting.None),      Does.Contain(UTC).And.Contain(WithOffset));
            });

        }

        #endregion

        #region AMessageIsOneArrayAndNothingAfterIt()

        /// <summary>
        /// As JArray.Parse: one array, a comment after it, and nothing else.
        /// </summary>
        [Test]
        public void AMessageIsOneArrayAndNothingAfterIt()
        {

            Assert.Multiple(() => {
                Assert.That(JSONMessages.ParseArray("""[ 3, "42", { } ] // done""").Count,  Is.EqualTo(3));
                Assert.Throws<JsonReaderException>(() => JSONMessages.ParseArray("""[ 3, "42", { } ] [ 4 ]"""));
                Assert.Throws<JsonReaderException>(() => JSONMessages.ParseArray("""{ "not": "an array" }"""));
            });

        }

        #endregion

        #region TheServerHandsOnAMessageWithItsStringsAsTheyWereSent()

        /// <summary>
        /// A message a station sends over its WebSocket reaches whoever listens
        /// at the server with its strings as they were sent.
        /// </summary>
        [Test]
        public async Task TheServerHandsOnAMessageWithItsStringsAsTheyWereSent()
        {

            var port      = TestPorts.Free();
            var received  = new ConcurrentQueue<JArray>();
            var server    = new WWCPWebSocketServer(new Node(),
                                                    HTTPPort:               port,
                                                    RequireAuthentication:  false,
                                                    SecWebSocketProtocols:  [ "ocpp2.1" ],
                                                    DisableWebSocketPings:  true);

            server.OnJSONMessageReceived += (_, _, _, _, _, _, message, _) => {
                received.Enqueue(message);
                return Task.CompletedTask;
            };

            await server.Start();

            try
            {

                using var client = await RawWebSocketClient.Upgrade(port, "/CS001");

                Assert.That(client.StatusCode, Is.EqualTo(101));

                await client.SendText($$"""[ 2, "42", "NotifyEvent", { "eventData": [ { "actualValue": "{{UTC}}", "timestamp": "{{WithOffset}}" } ] } ]""");

                for (var waited = 0; received.IsEmpty && waited < 10_000; waited += 20)
                    await Task.Delay(20);

                Assert.That(received.TryPeek(out var message), Is.True, "the server handed on no message");

                var eventData = message![3]!["eventData"]![0]!;

                Assert.Multiple(() => {
                    Assert.That(eventData.Value<String>("actualValue"),  Is.EqualTo(UTC));
                    Assert.That(eventData.Value<String>("timestamp"),    Is.EqualTo(WithOffset));
                });

            }
            finally
            {
                await server.Shutdown();
                await server.DisposeAsync();
            }

        }

        #endregion

    }

}
