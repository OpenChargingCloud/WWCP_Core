/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// WWCP Roaming networks defaults.
    /// </summary>
    public abstract class AEVSETests : AChargingStationTests
    {

        #region Data

        protected IEVSE? DE_GEF_E0001_AAAA_1;

        #endregion


        #region SetupEachTest()

        [SetUp]
        public override void SetupEachTest()
        {

            base.SetupEachTest();

            if (roamingNetwork    is not null &&
                DE_GEF            is not null &&
                DE_GEF_P0001      is not null &&
                DE_GEF_S0001_AAAA is not null)
            {

                DE_GEF_E0001_AAAA_1 = DE_GEF_S0001_AAAA.AddEVSE(
                                                            Id:                  EVSE_Id.Parse(DE_GEF_S0001_AAAA.Id, "1"),
                                                            Name:                I18NString.Create(Languages.de, "GraphDefined EVSE #1"),
                                                            Description:         I18NString.Create(Languages.de, "powered by GraphDefined EVSEs GmbH"),
                                                            InitialAdminStatus:  EVSEAdminStatusTypes.OutOfService,
                                                            InitialStatus:       EVSEStatusTypes.Offline
                                                        ).Result.EVSE;

                Assert.IsNotNull(DE_GEF_E0001_AAAA_1);

            }

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public override void ShutdownEachTest()
        {

            base.ShutdownEachTest();

            DE_GEF_E0001_AAAA_1 = null;

        }

        #endregion

    }

}
