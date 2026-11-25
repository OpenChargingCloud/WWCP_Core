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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.WebSockets
{

    /// <summary>
    /// TCP ports of the loopback interface found free, and the servers of the tests bound to them.
    /// </summary>
    /// <remarks>
    /// A port found free is let go again before a server binds it, and on a machine running other
    /// test suites at the same time somebody else may take it in between: a test then failed in
    /// its set-up with AddressAlreadyInUse, and was green alone (MixedSecurityProfilesTests in
    /// EMSPCLI's nightly of 2026-11-24). <see cref="Start"/> and <see cref="OnAFreePort"/> try
    /// again on a port found anew where - and only where - the port found was taken; any other
    /// reason not to start is the test's.
    /// </remarks>
    internal static class TestPorts
    {

        #region Free()

        /// <summary>
        /// A TCP port nothing listens on, as far as anybody can tell.
        /// </summary>
        public static IPPort Free()
        {

            var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);

            listener.Start();

            try
            {
                return IPPort.Parse((UInt16) ((IPEndPoint) listener.LocalEndpoint).Port);
            }
            finally
            {
                listener.Stop();
            }

        }

        #endregion

        #region OnAFreePort(Bind, Attempts = 5)

        /// <summary>
        /// Bind something to a port found free - and to another one found anew, at most Attempts
        /// times in all, where the port was taken before it was bound. Bind lets go of whatever
        /// it made when it throws.
        /// </summary>
        /// <param name="Bind">Binds something to the given port, and returns it.</param>
        /// <param name="Attempts">How often to try at most.</param>
        public static async Task<T> OnAFreePort<T>(Func<IPPort, Task<T>>  Bind,
                                                   Int32                  Attempts = 5)
        {

            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    return await Bind(Free());
                }
                catch (Exception e) when (attempt < Attempts && IsAPortTaken(e))
                { }
            }

        }

        #endregion

        #region Start(MakeServer, Attempts = 5)

        /// <summary>
        /// Make a WebSocket server on a port found free and start it - and make it again on a port
        /// found anew, at most Attempts times in all, where the port was taken before it was bound.
        /// A server that did not start is disposed of. What a test needs to know of the port it
        /// reads from the server it gets back (IPPort).
        /// </summary>
        /// <param name="MakeServer">Makes the server on the given port, not yet started.</param>
        /// <param name="Attempts">How often to make it at most.</param>
        public static Task<TServer> Start<TServer>(Func<IPPort, TServer>  MakeServer,
                                                   Int32                  Attempts = 5)

            where TServer : AWebSocketServer

            => OnAFreePort(async port => {

                   var server = MakeServer(port);

                   try
                   {
                       await server.Start();
                       return server;
                   }
                   catch
                   {
                       await server.DisposeAsync();
                       throw;
                   }

               }, Attempts);

        #endregion

        #region IsAPortTaken(Exception)

        /// <summary>
        /// Whether the given exception, or one it was thrown for, says that a port to be bound was
        /// taken - however many layers between the socket and the server wrapped it.
        /// </summary>
        public static Boolean IsAPortTaken(Exception? Exception)
        {

            for (var e = Exception; e is not null; e = e.InnerException)
            {

                if (e is SocketException { SocketErrorCode: SocketError.AddressAlreadyInUse })
                    return true;

                if (e is AggregateException aggregate && aggregate.InnerExceptions.Any(IsAPortTaken))
                    return true;

            }

            return false;

        }

        #endregion

    }

}
