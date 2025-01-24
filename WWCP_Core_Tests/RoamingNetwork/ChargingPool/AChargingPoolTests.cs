/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// WWCP Roaming networks defaults.
    /// </summary>
    public abstract class AChargingPoolTests : AChargingStationOperatorTests
    {

        #region Data

        protected IChargingPool? DE_GEF_P0001;

        #endregion


        #region SetupEachTest()

        [SetUp]
        public override void SetupEachTest()
        {

            base.SetupEachTest();

            if (roamingNetwork is not null &&
                DE_GEF         is not null)
            {

                DE_GEF_P0001 = DE_GEF.AddChargingPool(
                                   Id:                  ChargingPool_Id.Parse(DE_GEF.Id, "0001"),
                                   Name:                I18NString.Create(Languages.de, "GraphDefined Charging Pool #1"),
                                   Description:         I18NString.Create(Languages.de, "powered by GraphDefined Charging Pools GmbH"),
                                   InitialAdminStatus:  ChargingPoolAdminStatusType.OutOfService,
                                   InitialStatus:       ChargingPoolStatusType.Offline
                               ).Result.ChargingPool;

                ClassicAssert.IsNotNull(DE_GEF_P0001);

            }

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public override void ShutdownEachTest()
        {

            base.ShutdownEachTest();

            DE_GEF_P0001 = null;

        }

        #endregion

    }

}
