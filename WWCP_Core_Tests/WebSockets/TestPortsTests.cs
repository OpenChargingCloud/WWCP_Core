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

using System.Net;
using System.Net.Sockets;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// A port found free and taken by somebody else before it was bound is found anew.
    /// </summary>
    [TestFixture]
    public class TestPortsTests
    {

        #region (private static) Hold()

        /// <summary>
        /// A port of the loopback interface held by a listener of this test - as one is that
        /// somebody else took between being found free and being bound.
        /// </summary>
        private static TcpListener Hold()
        {
            var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);
            listener.Start();
            return listener;
        }

        private static IPPort PortOf(TcpListener Listener)
            => IPPort.Parse(((IPEndPoint) Listener.LocalEndpoint).Port);

        #endregion


        #region APortTakenOnTheFirstAttemptIsFoundAnew()

        /// <summary>
        /// Where the first attempt binds a port somebody else holds, the second binds the port it
        /// is given, found free anew - and the test gets what that one bound.
        /// </summary>
        [Test]
        public async Task APortTakenOnTheFirstAttemptIsFoundAnew()
        {

            var taken     = Hold();
            var attempts  = 0;

            try
            {

                var bound = await TestPorts.OnAFreePort(port => {

                                attempts++;

                                var listener = new TcpListener(System.Net.IPAddress.Loopback,
                                                               attempts == 1 ? PortOf(taken).ToUInt16() : port.ToUInt16());
                                listener.Start();

                                return Task.FromResult(listener);

                            });

                Assert.Multiple(() => {
                    Assert.That(attempts,      Is.EqualTo(2));
                    Assert.That(PortOf(bound), Is.Not.EqualTo(PortOf(taken)));
                });

                bound.Stop();

            }
            finally
            {
                taken.Stop();
            }

        }

        #endregion

        #region APortTakenOnEveryAttemptIsTheTestsTrouble()

        /// <summary>
        /// Where every attempt finds its port taken, the last one's exception is the test's.
        /// </summary>
        [Test]
        public async Task APortTakenOnEveryAttemptIsTheTestsTrouble()
        {

            var taken     = Hold();
            var attempts  = 0;

            try
            {

                var e = await Assert.ThrowsAsync<SocketException>(() => TestPorts.OnAFreePort(port => {

                            attempts++;

                            var listener = new TcpListener(System.Net.IPAddress.Loopback, PortOf(taken).ToUInt16());
                            listener.Start();

                            return Task.FromResult(listener);

                        }, Attempts: 3));

                Assert.Multiple(() => {
                    Assert.That(attempts,          Is.EqualTo(3));
                    Assert.That(e!.SocketErrorCode, Is.EqualTo(SocketError.AddressAlreadyInUse));
                });

            }
            finally
            {
                taken.Stop();
            }

        }

        #endregion

        #region AnyOtherTroubleIsNotTriedAgain()

        /// <summary>
        /// Any other reason not to bind is the test's at once.
        /// </summary>
        [Test]
        public async Task AnyOtherTroubleIsNotTriedAgain()
        {

            var attempts = 0;

            await Assert.ThrowsAsync<InvalidOperationException>(() => TestPorts.OnAFreePort<TcpListener>(port => {
                attempts++;
                throw new InvalidOperationException("Not a port's trouble.");
            }));

            Assert.That(attempts, Is.EqualTo(1));

        }

        #endregion

    }

}
