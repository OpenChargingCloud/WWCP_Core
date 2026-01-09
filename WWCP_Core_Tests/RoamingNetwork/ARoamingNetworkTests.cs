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

using NUnit.Framework;
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// WWCP Roaming networks defaults.
    /// </summary>
    public abstract class ARoamingNetworkTests
    {

        #region Data

        protected WWCP.RoamingNetwork? roamingNetwork;

        #endregion


        #region SetupOnce()

        [OneTimeSetUp]
        public virtual void SetupOnce()
        {

        }

        #endregion

        #region SetupEachTest()

        [SetUp]
        public virtual void SetupEachTest()
        {

            Timestamp.Reset();

            roamingNetwork = new WWCP.RoamingNetwork(
                                 Id:                          RoamingNetwork_Id.Parse("PROD"),
                                 Name:                        I18NString.Create("PRODUCTION"),
                                 Description:                 I18NString.Create("The main production roaming network"),
                                 InitialAdminStatus:          RoamingNetworkAdminStatusTypes.OutOfService,
                                 InitialStatus:               RoamingNetworkStatusTypes.Offline,
                                 MaxAdminStatusScheduleSize:  15,
                                 MaxStatusScheduleSize:       15,
                                 DisableNetworkSync:          true
                             );

            ClassicAssert.IsNotNull(roamingNetwork);


            //empClientAPI.OnPullEVSEData                    += (timestamp, empClientAPI, pullEVSEDataRequest)                    => {

            //    return Task.FromResult(
            //        OICPResult<PullEVSEDataResponse>.Success(
            //            pullEVSEDataRequest,
            //            new PullEVSEDataResponse(
            //                Timestamp.Now,
            //                pullEVSEDataRequest.EventTrackingId ?? EventTracking_Id.New,
            //                Process_Id.NewRandom,
            //                Timestamp.Now - pullEVSEDataRequest.Timestamp,
            //                Array.Empty<EVSEDataRecord>(),
            //                pullEVSEDataRequest,
            //                StatusCode: new StatusCode(
            //                                StatusCodes.Success
            //                            )
            //            )
            //        )
            //    );

            //};

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public virtual void ShutdownEachTest()
        {
            roamingNetwork = null;
        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public virtual void ShutdownOnce()
        {

        }

        #endregion

    }

}
