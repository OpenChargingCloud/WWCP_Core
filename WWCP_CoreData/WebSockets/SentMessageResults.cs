﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
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

using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

#endregion

namespace cloud.charging.open.protocols.WWCP.WebSockets
{

    public class SentMessageResult
    {

        #region Properties

        public SentMessageResults     Result        { get; }
        public IWebSocketConnection?  Connection    { get; }
        public Exception?             Exception     { get; }

        #endregion

        #region Constructor(s)

        private SentMessageResult(SentMessageResults     SentMessageResult,
                                  IWebSocketConnection?  WebSocketConnection   = null,
                                  Exception?             Exception             = null)
        {

            this.Result      = SentMessageResult;
            this.Connection  = WebSocketConnection;
            this.Exception   = Exception;

        }

        #endregion


        public static SentMessageResult Unknown()

            => new (SentMessageResults.Unknown);

        public static SentMessageResult UnknownClient()

            => new (SentMessageResults.UnknownClient);

        public static SentMessageResult Broadcast()

            => new (SentMessageResults.Broadcast);

        public static SentMessageResult TransmissionFailed (Exception             Exception)

            => new (SentMessageResults.TransmissionFailed,
                    null,
                    Exception);

        public static SentMessageResult TransmissionFailed (Exception             Exception,
                                                            IWebSocketConnection  WebSocketConnection)

            => new (SentMessageResults.TransmissionFailed,
                    WebSocketConnection,
                    Exception);

        public static SentMessageResult Timeout            (IWebSocketConnection  WebSocketConnection)

            => new (SentMessageResults.Timeout,
                    WebSocketConnection);

        public static SentMessageResult Success            (IWebSocketConnection  WebSocketConnection)

            => new (SentMessageResults.Success,
                    WebSocketConnection);


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Result}{(Connection is not null ? $" via '{Connection}'" : "")}{(Exception is not null ? $", Exception: {Exception.Message}" : "")}";

        #endregion

    }


    public enum SentMessageResults
    {
        Unknown,
        UnknownClient,
        Broadcast,
        TransmissionFailed,
        Timeout,
        Success
    }

}
