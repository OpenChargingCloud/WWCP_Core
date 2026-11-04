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

using System.Diagnostics;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using HermodDNSClient = org.GraphDefined.Vanaheimr.Hermod.DNS.DNSClient;

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.NetworkingNode
{

    /// <summary>
    /// A networking node's maintenance and heartbeat timers start the work of
    /// a tick and do not wait for it, and leave a tick out while the work of
    /// the tick before is still going on.
    /// </summary>
    /// <remarks>
    /// The maintenance ticks every second. Its tick used to wait for the
    /// maintenance with .Wait() on a thread of the thread pool, and the
    /// maintenance waited for up to five seconds for a lock every node of the
    /// process shared; a kind of node waited for its heartbeats the same way.
    /// A charging station's tests, which make a station with two nodes for
    /// nearly every test, hung on GitHub's Windows runners, with four cores:
    /// hundreds of nodes were ticking by then, and their waits had taken every
    /// thread of the pool.
    /// </remarks>
    [TestFixture]
    public class TimerTicksTests
    {

        #region (class) Node

        /// <summary>
        /// A networking node whose maintenance and heartbeats count themselves
        /// and then wait for whatever the test gives them to wait for.
        /// </summary>
        private sealed class Node(String     Id,
                                  TimeSpan   Every,
                                  IDNSClient DNSClient,
                                  Boolean    NoMaintenance  = false,
                                  Boolean    NoHeartbeats   = false)

            : AWWCPNetworkingNode(NetworkingNode_Id.Parse(Id),
                                  DisableSendHeartbeats:     NoHeartbeats,
                                  SendHeartbeatsEvery:       Every,
                                  DisableMaintenanceTasks:   NoMaintenance,
                                  MaintenanceEvery:          Every,
                                  DNSClient:                 DNSClient)

        {

            private Int32 maintained;
            private Int32 heartbeats;

            public Int32  Maintained  => Volatile.Read(ref maintained);
            public Int32  Heartbeats  => Volatile.Read(ref heartbeats);

            /// <summary>
            /// What the maintenance waits for once it has counted itself.
            /// </summary>
            public Task   Maintenance { get; set; } = Task.CompletedTask;

            /// <summary>
            /// What the heartbeats wait for once they have counted themselves.
            /// </summary>
            public Task   Heartbeat   { get; set; } = Task.CompletedTask;

            protected override Task DoMaintenanceAsync(Object? State)
            {
                Interlocked.Increment(ref maintained);
                return Maintenance;
            }

            protected override Task DoSendHeartbeatsAsync(Object? State)
            {
                Interlocked.Increment(ref heartbeats);
                return Heartbeat;
            }

        }

        #endregion

        #region Data

        /// <summary>
        /// How often the nodes of these tests tick: often enough for a few
        /// hundred ticks to come and go while a test waits.
        /// </summary>
        private static readonly TimeSpan Often = TimeSpan.FromMilliseconds(20);

        /// <summary>
        /// How long a test waits for what should happen at the next tick or
        /// two before it says it did not.
        /// </summary>
        private static readonly TimeSpan Patience = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Name servers written down rather than looked for: a node makes
        /// itself a resolver otherwise, which asks the operating system for
        /// its network adapters, and these tests make many nodes. Nothing
        /// is resolved.
        /// </summary>
        private static readonly HermodDNSClient Resolver = new ([
                                                         new DNSServerConfig(IPv4Address.Parse("192.0.2.53"), IPPort.DNS)
                                                     ]);

        private readonly List<Node>                    nodes  = [];
        private          TaskCompletionSource<Object>  gate   = default!;

        #endregion

        #region SetUp / TearDown

        /// <summary>
        /// A gate of its own for each test: what the nodes of a test wait for
        /// until the test or its end opens it.
        /// </summary>
        [SetUp]
        public void MakeAGate()
        {
            gate = new (TaskCreationOptions.RunContinuationsAsynchronously);
        }

        [TearDown]
        public async Task StopTheNodes()
        {

            gate.TrySetResult(new Object());

            foreach (var node in nodes)
                await node.Stop();

            nodes.Clear();

        }

        #endregion


        #region (private) NewNode(...)

        private Node NewNode(String   Id,
                             Boolean  NoMaintenance  = false,
                             Boolean  NoHeartbeats   = false)
        {

            var node = new Node(Id, Often, Resolver, NoMaintenance, NoHeartbeats);

            nodes.Add(node);

            return node;

        }

        #endregion

        #region (private static) Within(Patience, Condition)

        /// <summary>
        /// Whether the condition comes true within the given time.
        /// </summary>
        private static async Task<Boolean> Within(TimeSpan      Patience,
                                                  Func<Boolean>  Condition)
        {

            var watch = Stopwatch.StartNew();

            while (!Condition())
            {

                if (watch.Elapsed > Patience)
                    return false;

                await Task.Delay(10);

            }

            return true;

        }

        #endregion


        #region ATickIsLeftOutWhileTheMaintenanceBeforeIsStillAtWork()

        /// <summary>
        /// While the maintenance of one tick is still at work, the ticks after
        /// it are left out, and the one after it is done starts it again.
        /// </summary>
        [Test]
        public async Task ATickIsLeftOutWhileTheMaintenanceBeforeIsStillAtWork()
        {

            var node = NewNode("maintained", NoHeartbeats: true);

            node.Maintenance = gate.Task;

            Assert.That(await Within(Patience, () => node.Maintained == 1), Is.True, "the first tick started the maintenance");

            await Task.Delay(Often * 15);

            Assert.That(node.Maintained, Is.EqualTo(1), "the ticks while it was at work were left out");

            node.Maintenance = Task.CompletedTask;
            gate.TrySetResult(new Object());

            Assert.That(await Within(Patience, () => node.Maintained > 1), Is.True, "a tick after it was done started it again");

        }

        #endregion

        #region EachNodeIsMaintainedOnItsOwn()

        /// <summary>
        /// A node whose maintenance does not end keeps no other node from
        /// being maintained.
        /// </summary>
        [Test]
        public async Task EachNodeIsMaintainedOnItsOwn()
        {

            var stuck  = NewNode("stuck",  NoHeartbeats: true);
            stuck.Maintenance = gate.Task;

            Assert.That(await Within(Patience, () => stuck.Maintained == 1), Is.True, "the first node's maintenance started");

            var other  = NewNode("other",  NoHeartbeats: true);

            Assert.That(await Within(Patience, () => other.Maintained > 2), Is.True,
                        "the other node was maintained while the first one's maintenance was still at work");

        }

        #endregion

        #region AHeartbeatTickIsLeftOutWhileTheHeartbeatsBeforeAreStillBeingSent()

        /// <summary>
        /// While the heartbeats of one tick are still being sent, the ticks
        /// after it are left out, and the one after they are sent sends them
        /// again.
        /// </summary>
        [Test]
        public async Task AHeartbeatTickIsLeftOutWhileTheHeartbeatsBeforeAreStillBeingSent()
        {

            var node = NewNode("beating", NoMaintenance: true);

            node.Heartbeat = gate.Task;

            Assert.That(await Within(Patience, () => node.Heartbeats == 1), Is.True, "the first tick sent the heartbeats");

            await Task.Delay(Often * 15);

            Assert.That(node.Heartbeats, Is.EqualTo(1), "the ticks while they were being sent were left out");

            node.Heartbeat = Task.CompletedTask;
            gate.TrySetResult(new Object());

            Assert.That(await Within(Patience, () => node.Heartbeats > 1), Is.True, "a tick after they were sent sent them again");

        }

        #endregion

        #region NeitherTicksWhereItIsSwitchedOff()

        /// <summary>
        /// A node with its maintenance switched off is not maintained, and one
        /// with its heartbeats switched off sends none - which a kind of node
        /// used to have to remember to ask itself.
        /// </summary>
        [Test]
        public async Task NeitherTicksWhereItIsSwitchedOff()
        {

            var quiet    = NewNode("quiet", NoMaintenance: true, NoHeartbeats: true);
            var ticking  = NewNode("ticking");

            Assert.That(await Within(Patience, () => ticking.Maintained > 2 && ticking.Heartbeats > 2), Is.True, "a node with both switched on ticks");

            Assert.Multiple(() => {
                Assert.That(quiet.Maintained,  Is.Zero, "maintained although that was switched off");
                Assert.That(quiet.Heartbeats,  Is.Zero, "heartbeats sent although they were switched off");
            });

        }

        #endregion

        #region ManyNodesWaitingInTheirTicksHoldNoThread()

        /// <summary>
        /// A few hundred nodes whose maintenance and heartbeats are all waiting
        /// leave the thread pool free: what is given to it meanwhile is done
        /// at once, and it needs hardly a thread more than before.
        /// </summary>
        [Test]
        public async Task ManyNodesWaitingInTheirTicksHoldNoThread()
        {

            const Int32 count = 300;

            var threadsBefore = ThreadPool.ThreadCount;

            for (var number = 0; number < count; number++)
            {
                var node = NewNode($"waiting{number}");
                node.Maintenance  = gate.Task;
                node.Heartbeat    = gate.Task;
            }

            Assert.That(await Within(Patience, () => nodes.All(node => node.Maintained == 1 && node.Heartbeats == 1)), Is.True,
                        "every node started its maintenance and its heartbeats");

            // Some hundred ticks of each node come and go meanwhile.
            await Task.Delay(Often * 50);

            var watch  = Stopwatch.StartNew();
            var done   = await Task.Run(() => true).WaitAsync(Patience);

            Assert.Multiple(() => {
                Assert.That(done,                                   Is.True);
                Assert.That(watch.Elapsed,                          Is.LessThan(TimeSpan.FromSeconds(1)),  "something given to the thread pool waited for a thread");
                Assert.That(ThreadPool.ThreadCount - threadsBefore, Is.LessThan(count / 10),               "the thread pool grew by a thread for many of the nodes");
                Assert.That(nodes.All(node => node.Maintained == 1 && node.Heartbeats == 1), Is.True,     "a tick was not left out");
            });

        }

        #endregion

    }

}
