/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System.Collections.Concurrent;

using NUnit.Framework;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.SSH;
using org.GraphDefined.Vanaheimr.Hermod.SSH.Client;
using org.GraphDefined.Vanaheimr.Hermod.SSH.Server;
using org.GraphDefined.Vanaheimr.Hermod.Messaging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.SSH;
using cloud.charging.open.protocols.WWCP.WebSockets;
using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.WWCP.UnitTests.WebSockets;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.SSH
{

    /// <summary>
    /// Networking nodes over an SSH subsystem: a CSMS serving "wwcp@open.charging.cloud" with a
    /// <see cref="WWCPSshServer"/> behind a Hermod SSH server, and charging stations connecting with a
    /// <see cref="WWCPSshClient"/>. Who a station is, what it may connect as, which of its connections is
    /// the one, how it networks - and messages both ways.
    /// </summary>
    [TestFixture]
    public class SshTransportTests
    {

        #region (class) Node

        /// <summary>
        /// A networking node of no particular kind.
        /// </summary>
        private sealed class Node(String Id) : AWWCPNetworkingNode(NetworkingNode_Id.Parse(Id),
                                                                   DisableSendHeartbeats:    true,
                                                                   DisableMaintenanceTasks:  true)
        { }

        #endregion

        #region Data

        private static readonly NetworkingNode_Id  csmsId      = NetworkingNode_Id.Parse("csms");
        private static readonly NetworkingNode_Id  cs001       = NetworkingNode_Id.Parse("CS001");
        private static readonly NetworkingNode_Id  cs002       = NetworkingNode_Id.Parse("CS002");

        private readonly ISshHostKey               hostKey     = SshHostKey.GenerateEd25519();
        private readonly ISshHostKey               stationKey  = SshHostKey.GenerateEd25519();

        private Node?                              csms;
        private WWCPSshServer?                     sshServer;
        private SshServer?                         listener;
        private IPPort                             port;

        private readonly List<Node>                stations    = [];

        #endregion

        #region SetUp() / TearDown()

        [SetUp]
        public async Task SetUp()
        {

            csms       = new Node("csms");
            sshServer  = csms.AttachSshServer(new WWCPSshServer(csms));
            port       = TestPorts.Free();
            listener   = await Listen(port);

        }

        [TearDown]
        public async Task TearDown()
        {

            foreach (var station in stations)
                await station.Stop();

            stations.Clear();

            if (csms is not null)
                await csms.Stop();

            if (listener is not null)
                await listener.DisposeAsync();

        }

        #endregion

        #region (private) Helpers

        private async Task<SshServer> Listen(IPPort Port)
        {

            var server = new SshServer(new SshServerOptions {
                             HostKeys       = [ hostKey ],
                             Authenticator  = SshUserAuthenticator.ForAuthorizedKeys(stationKey.PublicKeyBlob),
                             Subsystems     = (session, name, ct) => ValueTask.FromResult(name == WWCPSshServer.WWCPSubsystem ? sshServer!.Handler : null)
                         });

            await server.StartAsync(new IPSocket(IPv4Address.Localhost, Port), CancellationToken.None);

            return server;

        }

        private Node Station(String Id)
        {
            var station = new Node(Id);
            stations.Add(station);
            return station;
        }

        private WWCPSshClient Client(Node                             Station,
                                     String                           Login,
                                     NetworkingMode?                  NetworkingMode   = null,
                                     NetworkingNode_Id?               ConnectAs        = null,
                                     JObject?                         Hello            = null,
                                     WebSocketClientReconnectPolicy?  Reconnect        = null)

            => new (Station,
                    new SshMessageClientOptions {
                        Host             = "127.0.0.1",
                        Port             = port.ToUInt16(),
                        Ssh              = new SshClientOptions {
                                               Username       = Login,
                                               VerifyHostKey  = blob => blob.AsSpan().SequenceEqual(hostKey.PublicKeyBlob),
                                               Credentials    = [ stationKey ]
                                           },
                        Subsystem        = WWCPSshServer.WWCPSubsystem,
                        HelloProperties  = Hello,
                        Reconnect        = Reconnect
                    },
                    NetworkingMode,
                    ConnectAs);

        private static async Task<T> Within<T>(Task<T> Happening, String What)
        {
            if (await Task.WhenAny(Happening, Task.Delay(TimeSpan.FromSeconds(10))) != Happening)
                Assert.Fail($"{What} did not happen within 10 s.");
            return await Happening;
        }

        private static async Task Within(Task Happening, String What)
        {
            if (await Task.WhenAny(Happening, Task.Delay(TimeSpan.FromSeconds(10))) != Happening)
                Assert.Fail($"{What} did not happen within 10 s.");
            await Happening;
        }

        /// <summary>
        /// The server has put a connection of the given networking node on its books - which it does right
        /// after its WELCOME, and so possibly after the client heard it, but before it reads the client's
        /// first message.
        /// </summary>
        private Task<SshMessageConnection> Registered(NetworkingNode_Id NetworkingNodeId, SshMessageConnection? Besides = null)

            => Within(Task.Run(async () => {
                   while (true)
                   {
                       var connection = sshServer!.GetConnectionsFor(NetworkingNodeId).FirstOrDefault(connection => !ReferenceEquals(connection, Besides));
                       if (connection is not null)
                           return connection;
                       await Task.Delay(10);
                   }
               }), $"Registering '{NetworkingNodeId}'");

        private static async Task Until(Func<Boolean> Condition, String What)
        {
            var until = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10);
            while (!Condition())
            {
                if (DateTimeOffset.UtcNow > until)
                    Assert.Fail($"{What} did not happen within 10 s.");
                await Task.Delay(10);
            }
        }

        #endregion


        #region TheLoginIsTheNetworkingNode()

        /// <summary>
        /// A station is who signed in: registered under its login, routed to over the SSH server from the
        /// moment it was welcomed, and its connection says so in the keys the WebSocket transport uses.
        /// </summary>
        [Test]
        public async Task TheLoginIsTheNetworkingNode()
        {

            var station     = Station("CS001");
            var client      = Client(station, "CS001");

            var connection  = await station.ConnectSshClient(client, NextHopNetworkingNodeId: csmsId);

            var serverSide  = await Registered(cs001);

            Assert.That(sshServer!.Connections,                                  Is.EqualTo(new[] { serverSide }));
            Assert.That(sshServer.ConnectedNetworkingNodeIds,                    Is.EqualTo(new[] { cs001 }));
            Assert.That(serverSide.NetworkingNodeIdOf(),                         Is.EqualTo(cs001));
            Assert.That(serverSide.HasCustomData(WebSocketKeys.ActingNetworkingNodeId), Is.False);
            Assert.That(serverSide.HasCustomData(WebSocketKeys.NetworkingMode),  Is.False, "A standard client says nothing about its networking mode.");

            Assert.That(csms!.Routing.LookupNetworkingNode(cs001, out var toStation), Is.True);
            Assert.That(toStation!.Server,                                       Is.SameAs(sshServer));
            Assert.That(toStation.Transport!.Transport,                          Is.EqualTo(MessageTransport.SSH));
            Assert.That(toStation.WWCPWebSocketServer,                           Is.Null);

            // The client's side: whom it reaches, and that its connection belongs to it.
            Assert.That(connection.NetworkingNodeIdOf(),                         Is.EqualTo(csmsId));
            Assert.That(connection.WWCPClientOf(),                               Is.SameAs(client));
            Assert.That(serverSide.WWCPClientOf(),                               Is.Null);

            Assert.That(station.Routing.LookupNetworkingNode(csmsId, out var toCSMS), Is.True);
            Assert.That(toCSMS!.Client,                                          Is.SameAs(client));
            Assert.That(station.Routing.AllClients,                              Does.Contain(client));
            Assert.That(station.WWCPClients,                                     Does.Contain(client));

        }

        #endregion

        #region MessagesGoBothWays()

        /// <summary>
        /// JSON and binary messages, from the station and to it - each side knowing whom they came from.
        /// </summary>
        [Test]
        public async Task MessagesGoBothWays()
        {

            var atServer  = new ConcurrentQueue<(NetworkingNode_Id Source, Object Message)>();
            var atStation = new ConcurrentQueue<(NetworkingNode_Id Source, Object Message)>();

            sshServer!.OnJSONMessageReceived   += (_, transport, _, _, _, source, json,  _) => { Assert.That(transport, Is.SameAs(sshServer)); atServer.Enqueue((source, json));  return Task.CompletedTask; };
            sshServer. OnBinaryMessageReceived += (_, _,         _, _, _, source, bytes, _) => { atServer.Enqueue((source, bytes)); return Task.CompletedTask; };

            var station   = Station("CS001");
            var client    = Client(station, "CS001");

            client.OnJSONMessageReceived       += (_, transport, _, _, _, source, json,  _) => { Assert.That(transport, Is.SameAs(client));    atStation.Enqueue((source, json));  return Task.CompletedTask; };
            client.OnBinaryMessageReceived     += (_, _,         _, _, _, source, bytes, _) => { atStation.Enqueue((source, bytes)); return Task.CompletedTask; };

            await station.ConnectSshClient(client, NextHopNetworkingNodeId: csmsId);
            await Registered(cs001);

            var boot      = JArray.Parse("""[2,"1","BootNotification",{"reason":"PowerUp"}]""");
            var answer    = JArray.Parse("""[3,"1",{"status":"Accepted"}]""");

            Assert.That((await client.   SendJSONMessage  (boot,          Timestamp.Now, EventTracking_Id.New)).Result, Is.EqualTo(SentMessageResults.Success));
            Assert.That((await client.   SendBinaryMessage([ 1, 2, 3 ],   Timestamp.Now, EventTracking_Id.New)).Result, Is.EqualTo(SentMessageResults.Success));
            Assert.That((await sshServer.SendJSONMessage  (cs001, answer, Timestamp.Now, EventTracking_Id.New)).Result, Is.EqualTo(SentMessageResults.Success));
            Assert.That((await sshServer.SendBinaryMessage(cs001, [ 4, 5 ], Timestamp.Now, EventTracking_Id.New)).Result, Is.EqualTo(SentMessageResults.Success));

            await Until(() => atServer.Count == 2 && atStation.Count == 2, "Two messages each way");

            var serverMessages  = atServer. ToArray();
            var stationMessages = atStation.ToArray();

            Assert.That(serverMessages. Select(message => message.Source), Is.All.EqualTo(cs001));
            Assert.That(stationMessages.Select(message => message.Source), Is.All.EqualTo(csmsId));

            Assert.That(JToken.DeepEquals((JArray) serverMessages. Single(message => message.Message is JArray).Message, boot),   Is.True);
            Assert.That(JToken.DeepEquals((JArray) stationMessages.Single(message => message.Message is JArray).Message, answer), Is.True);
            Assert.That((Byte[]) serverMessages. Single(message => message.Message is Byte[]).Message, Is.EqualTo(new Byte[] { 1, 2, 3 }));
            Assert.That((Byte[]) stationMessages.Single(message => message.Message is Byte[]).Message, Is.EqualTo(new Byte[] { 4, 5 }));

            // Nobody of that name: nothing sent.
            Assert.That((await sshServer.SendJSONMessage(cs002, answer, Timestamp.Now, EventTracking_Id.New)).Result, Is.EqualTo(SentMessageResults.UnknownClient));

        }

        #endregion

        #region ALoginConnectsAsAnotherOnlyWhereAllowed()

        /// <summary>
        /// A local controller's login may connect as one of its stations - naming it in the HELLO - only where
        /// the server allows it to, and then the connection is the station's, and says who opened it.
        /// </summary>
        [Test]
        public async Task ALoginConnectsAsAnotherOnlyWhereAllowed()
        {

            var station  = Station("CS002");

            var refused  = await Assert.ThrowsAsync<SshMessageRefusedException>(() => station.ConnectSshClient(Client(station, "CS001", ConnectAs: cs002)));

            Assert.That(refused!.Code,                       Is.EqualTo(WWCPSshServer.RefusalNotPermitted));
            Assert.That(sshServer!.ConnectedNetworkingNodeIds, Is.Empty);

            sshServer.AllowToActFor(cs001, [ cs002 ]);

            await station.ConnectSshClient(Client(station, "CS001", ConnectAs: cs002));

            var serverSide = await Registered(cs002);

            Assert.That(sshServer.ConnectedNetworkingNodeIds,                                                Is.EqualTo(new[] { cs002 }));
            Assert.That(serverSide.NetworkingNodeIdOf(),                                                     Is.EqualTo(cs002));
            Assert.That(serverSide.TryGetCustomDataAs<NetworkingNode_Id>(WebSocketKeys.ActingNetworkingNodeId), Is.EqualTo(cs001));

            // Naming itself is no acting for anybody.
            var other = Station("CS001");

            await other.ConnectSshClient(Client(other, "CS001", ConnectAs: cs001));

            Assert.That((await Registered(cs001)).HasCustomData(WebSocketKeys.ActingNetworkingNodeId), Is.False);

            // Allowed no more: refused again - and the open connection stays.
            sshServer.DisallowToActFor(cs001, [ cs002 ]);

            var refusedAgain = await Assert.ThrowsAsync<SshMessageRefusedException>(() => station.ConnectSshClient(Client(station, "CS001", ConnectAs: cs002)));

            Assert.That(refusedAgain!.Code,                       Is.EqualTo(WWCPSshServer.RefusalNotPermitted));
            Assert.That(sshServer.GetConnectionsFor(cs002).Single(), Is.SameAs(serverSide));

        }

        #endregion

        #region TheNewestConnectionIsTheOne()

        /// <summary>
        /// A station that connects again replaces its older connection, which is closed - and whose end, when
        /// it comes, does not take the newer one off the books.
        /// </summary>
        [Test]
        public async Task TheNewestConnectionIsTheOne()
        {

            var station  = Station("CS001");
            var older    = Client(station, "CS001");
            var newer    = Client(station, "CS001");

            var olderClosed = new TaskCompletionSource<SshMessageCloseInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
            older.OnConnectionClosed += (_, _, _, _, close, _) => { olderClosed.TrySetResult(close); return Task.CompletedTask; };

            await station.ConnectSshClient(older, NextHopNetworkingNodeId: csmsId);

            var olderServerSide = await Registered(cs001);

            await station.ConnectSshClient(newer, NextHopNetworkingNodeId: csmsId);

            var newerServerSide = await Registered(cs001, Besides: olderServerSide);

            var close = await Within(olderClosed.Task, "The older connection's end");

            Assert.That(close.Code,   Is.EqualTo(MessageCloseCode.NormalClosure));
            Assert.That(close.ByPeer, Is.True);

            // Its end has reached the server by the time it has no connection under it any more.
            await Until(() => !sshServer!.Connections.Contains(olderServerSide), "The older connection's end at the server");

            Assert.That(sshServer!.GetConnectionsFor(cs001),  Is.EqualTo(new[] { newerServerSide }));
            Assert.That(sshServer.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { cs001 }));

            // And the newer one going leaves nobody.
            await newer.Close();

            await Until(() => !sshServer.ConnectedNetworkingNodeIds.Any(), "The newer connection's end at the server");

            Assert.That(sshServer.GetConnectionsFor(cs001), Is.Empty);

        }

        #endregion

        #region TheNetworkingModeIsTold()

        /// <summary>
        /// An overlay network client says so in its HELLO; a mode that is no mode's name is refused.
        /// </summary>
        [Test]
        public async Task TheNetworkingModeIsTold()
        {

            var station = Station("CS001");

            await station.ConnectSshClient(Client(station, "CS001", NetworkingMode.OverlayNetwork));

            Assert.That((await Registered(cs001)).TryGetCustomDataAs<NetworkingMode>(WebSocketKeys.NetworkingMode), Is.EqualTo(NetworkingMode.OverlayNetwork));

            foreach (var notAMode in new JToken[] { "2", "overlaynetwork", "Standard, OverlayNetwork", 1, JValue.CreateNull() })
            {

                var refused = await Assert.ThrowsAsync<SshMessageRefusedException>(() => Client(station, "CS002", Hello: new JObject(new JProperty(WWCPSshServer.HelloNetworkingMode, notAMode))).Connect(),
                                                                             $"networkingMode {notAMode.ToString(Newtonsoft.Json.Formatting.None)}");

                Assert.That(refused!.Code, Is.EqualTo(SshMessageRefusal.InvalidHello), $"networkingMode {notAMode.ToString(Newtonsoft.Json.Formatting.None)}");

            }

            var badId = await Assert.ThrowsAsync<SshMessageRefusedException>(() => Client(station, "CS002", Hello: new JObject(new JProperty(WWCPSshServer.HelloNetworkingNodeId, 42))).Connect());

            Assert.That(badId!.Code, Is.EqualTo(SshMessageRefusal.InvalidHello));

        }

        #endregion

        #region AFirstConnectThatFailsIsTriedAgain()

        /// <summary>
        /// A station that starts while its CSMS is down reaches it once it is up, where it has a reconnect
        /// policy - as a WebSocket client does.
        /// </summary>
        [Test]
        public async Task AFirstConnectThatFailsIsTriedAgain()
        {

            await listener!.DisposeAsync();
            listener = null;

            var station   = Station("CS001");
            var client    = Client(station, "CS001", Reconnect: new WebSocketClientReconnectPolicy(InitialDelay: TimeSpan.FromMilliseconds(100), MaxDelay: TimeSpan.FromMilliseconds(200)));

            var connected = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            client.OnConnected += (_, _, _, _, _, _, _) => { connected.TrySetResult(); return Task.CompletedTask; };

            Assert.That(async () => await station.ConnectSshClient(client, NextHopNetworkingNodeId: csmsId), Throws.Exception);

            listener = await Listen(port);

            await Within(connected.Task, "The connection once the CSMS is up");
            await Registered(cs001);

            Assert.That(sshServer!.ConnectedNetworkingNodeIds, Is.EqualTo(new[] { cs001 }));
            Assert.That(client.Connection!.NetworkingNodeIdOf(), Is.EqualTo(csmsId), "Every connection the client makes is written down.");

        }

        #endregion

        #region StoppingTheNodesEndsTheirConnections()

        /// <summary>
        /// A CSMS that stops tells its stations it is going away; a station that stops ends its connection
        /// and connects no more.
        /// </summary>
        [Test]
        public async Task StoppingTheNodesEndsTheirConnections()
        {

            var station = Station("CS001");
            var client  = Client(station, "CS001", Reconnect: new WebSocketClientReconnectPolicy(InitialDelay: TimeSpan.FromMilliseconds(100)));

            var closed  = new TaskCompletionSource<SshMessageCloseInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
            client.OnConnectionClosed += (_, _, _, _, close, _) => { closed.TrySetResult(close); return Task.CompletedTask; };

            await station.ConnectSshClient(client, NextHopNetworkingNodeId: csmsId);
            await Registered(cs001);

            await csms!.Stop();

            var close = await Within(closed.Task, "The station hearing the CSMS go");

            Assert.That(close.Code, Is.EqualTo(MessageCloseCode.GoingAway));

            // It comes back: the subsystem is served for as long as the SSH server serves it.
            await Until(() => client.IsConnected, "The station's reconnect");

            await station.Stop();

            Assert.That(client.IsConnected, Is.False);

            await Task.Delay(400);

            Assert.That(sshServer!.ConnectedNetworkingNodeIds, Is.Empty, "A stopped station connects no more.");

        }

        #endregion

    }

}
