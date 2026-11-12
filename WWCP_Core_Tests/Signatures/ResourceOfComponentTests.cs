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

using NUnit.Framework;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// The resource a device model component belongs to, when a signed GetVariables or
    /// SetVariables is decided entry by entry: every component of how a node resolves
    /// names is "dns", every component of how it keeps its time is "nts" - not only the
    /// one a request names most often.
    /// </summary>
    /// <remarks>
    /// The name servers and the time servers a node shows over OCPP (WWCP_OCPP_Node's
    /// DeviceModelBridge) are DNSClientController with a DNSServerController for each
    /// server, and ClockCtrlr, NTPController, an NTPClientGroupController for each group
    /// and an NTPClientController for each server. A component left out fell to
    /// "configuration": a role that may change the name servers could not add one, and
    /// a role that may change the configuration could add one without "dns:edit".
    /// </remarks>
    [TestFixture]
    public class ResourceOfComponentTests
    {

        [TestCase("DNSClientController",        "dns")]
        [TestCase("DNSServerController",        "dns")]
        [TestCase("ClockCtrlr",                 "nts")]
        [TestCase("NTPController",              "nts")]
        [TestCase("NTPClientGroupController",   "nts")]
        [TestCase("NTPClientController",        "nts")]
        [TestCase("NTSClientController",        "nts")]
        [TestCase("MessageSignatureController", "signatures")]
        [TestCase("SignerRoleController",       "signatures")]
        [TestCase("SecurityCtrlr",              "configuration")]
        [TestCase("SomethingNobodyKnows",       "configuration")]
        public void EveryComponentOfNamesAndTimeIsItsResource(String Component, String Resource)
        {

            Assert.That(SignaturePolicy.DefaultResourceOfComponent(Component), Is.EqualTo(Resource));

            // And what a policy asks unless the node says otherwise.
            Assert.That(new SignaturePolicy().ResourceOfComponent(Component), Is.EqualTo(Resource));

        }

    }

}
