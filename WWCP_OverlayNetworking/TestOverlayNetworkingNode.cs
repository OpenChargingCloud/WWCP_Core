/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Security.Cryptography;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    /// <summary>
    /// A networking node for testing.
    /// </summary>
    public partial class TestOverlayNetworkingNode : AOverlayNetworkingNode
    {

        #region Properties


        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station management system for testing.
        /// </summary>
        /// <param name="Id">The unique identification of this charging station management system.</param>
        public TestOverlayNetworkingNode(NetworkingNode_Id  Id,
                                         I18NString?        Description                 = null,

                                         SignaturePolicy?   SignaturePolicy             = null,
                                         SignaturePolicy?   ForwardingSignaturePolicy   = null,

                                         TimeSpan?          DefaultRequestTimeout       = null,

                                         Boolean            DisableSendHeartbeats       = false,
                                         TimeSpan?          SendHeartbeatsEvery         = null,

                                         Boolean            DisableMaintenanceTasks     = false,
                                         TimeSpan?          MaintenanceEvery            = null,

                                         DNSClient?         DNSClient                   = null)

            : base(Id,
                   Description,

                   SignaturePolicy,
                   ForwardingSignaturePolicy,

                   DisableSendHeartbeats,
                   SendHeartbeatsEvery,

                   DefaultRequestTimeout,

                   DisableMaintenanceTasks,
                   MaintenanceEvery,

                   DNSClient)

        {



        }

        #endregion


    }

}
