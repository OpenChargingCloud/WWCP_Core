/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class DeleteRoamingNetworkResult : AEnitityResult<IRoamingNetwork, RoamingNetwork_Id>
    {

        #region Properties

        public IRoamingNetwork?  RoamingNetwork
            => Object;

        #endregion

        #region Constructor(s)

        public DeleteRoamingNetworkResult(IRoamingNetwork        RoamingNetwork,
                                          PushDataResultTypes    Result,
                                          EventTracking_Id?      EventTrackingId   = null,
                                          IId?                   AuthId            = null,
                                          Object?                SendPOIData       = null,
                                          I18NString?            Description       = null,
                                          IEnumerable<Warning>?  Warnings          = null,
                                          TimeSpan?              Runtime           = null)

            : base(RoamingNetwork,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        { }


        public DeleteRoamingNetworkResult(RoamingNetwork_Id      RoamingNetworkId,
                                          PushDataResultTypes    Result,
                                          EventTracking_Id?      EventTrackingId   = null,
                                          IId?                   AuthId            = null,
                                          Object?                SendPOIData       = null,
                                          I18NString?            Description       = null,
                                          IEnumerable<Warning>?  Warnings          = null,
                                          TimeSpan?              Runtime           = null)

            : base(RoamingNetworkId,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) NoOperation

        public static DeleteRoamingNetworkResult

            NoOperation(IRoamingNetwork        RoamingNetwork,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (RoamingNetwork,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        Description,
                        Warnings,
                        Runtime);


        public static DeleteRoamingNetworkResult

            NoOperation(RoamingNetwork_Id      RoamingNetworkId,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (RoamingNetworkId,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


    }

}
