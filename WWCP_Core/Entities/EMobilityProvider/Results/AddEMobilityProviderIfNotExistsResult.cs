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

    public class AddEMobilityProviderIfNotExistsResult : AEnitityResult<IEMobilityProvider, EMobilityProvider_Id>
    {

        public IEMobilityProvider? EMobilityProvider
            => Object;

        public IRoamingNetwork?          RoamingNetwork    { get; internal set; }

        public AddedOrIgnored?           AddedOrIgnored    { get; internal set; }


        public AddEMobilityProviderIfNotExistsResult(IEMobilityProvider  EMobilityProvider,
                                                           EventTracking_Id          EventTrackingId,
                                                           Boolean                   IsSuccess,
                                                           String?                   Argument           = null,
                                                           I18NString?               ErrorDescription   = null,
                                                           IRoamingNetwork?          RoamingNetwork     = null,
                                                           AddedOrIgnored?           AddedOrIgnored     = null)

            : base(EMobilityProvider,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.RoamingNetwork    = RoamingNetwork;
            this.AddedOrIgnored  = AddedOrIgnored;

        }


        public static AddEMobilityProviderIfNotExistsResult Success(IEMobilityProvider  EMobilityProvider,
                                                                          AddedOrIgnored            AddedOrIgnored,
                                                                          EventTracking_Id          EventTrackingId,
                                                                          IRoamingNetwork?          RoamingNetwork   = null)

            => new (EMobilityProvider,
                    EventTrackingId,
                    true,
                    null,
                    null,
                    RoamingNetwork,
                    AddedOrIgnored);


        public static AddEMobilityProviderIfNotExistsResult ArgumentError(IEMobilityProvider  EMobilityProvider,
                                                                                EventTracking_Id          EventTrackingId,
                                                                                String                    Argument,
                                                                                String                    Description)

            => new (EMobilityProvider,
                    EventTrackingId,
                    false,
                    Argument,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ));

        public static AddEMobilityProviderIfNotExistsResult ArgumentError(IEMobilityProvider  EMobilityProvider,
                                                                                EventTracking_Id          EventTrackingId,
                                                                                String                    Argument,
                                                                                I18NString                Description)

            => new (EMobilityProvider,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static AddEMobilityProviderIfNotExistsResult Failed(IEMobilityProvider  EMobilityProvider,
                                                                         EventTracking_Id          EventTrackingId,
                                                                         String                    Description,
                                                                         IRoamingNetwork?          RoamingNetwork   = null)

            => new (EMobilityProvider,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    RoamingNetwork);

        public static AddEMobilityProviderIfNotExistsResult Failed(IEMobilityProvider  EMobilityProvider,
                                                                         EventTracking_Id          EventTrackingId,
                                                                         I18NString                Description,
                                                                         IRoamingNetwork?          RoamingNetwork   = null)

            => new (EMobilityProvider,
                    EventTrackingId,
                    false,
                    null,
                    Description,
                    RoamingNetwork);

        public static AddEMobilityProviderIfNotExistsResult Failed(IEMobilityProvider  EMobilityProvider,
                                                                         EventTracking_Id          EventTrackingId,
                                                                         Exception                 Exception,
                                                                         IRoamingNetwork?          RoamingNetwork   = null)

            => new (EMobilityProvider,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Exception.Message
                    ),
                    RoamingNetwork);

    }

}
