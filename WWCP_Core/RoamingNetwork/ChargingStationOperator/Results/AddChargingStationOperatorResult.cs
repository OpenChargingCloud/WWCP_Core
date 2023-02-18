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

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class AddChargingStationOperatorResult : AEnitityResult<IChargingStationOperator, ChargingStationOperator_Id>
    {

        public IChargingStationOperator? ChargingStationOperator
            => Object;

        public IRoamingNetwork?          RoamingNetwork    { get; internal set; }


        public AddChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                EventTracking_Id          EventTrackingId,
                                                Boolean                   IsSuccess,
                                                String?                   Argument           = null,
                                                I18NString?               ErrorDescription   = null,
                                                IRoamingNetwork?          RoamingNetwork     = null)

            : base(ChargingStationOperator,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.RoamingNetwork = RoamingNetwork;

        }

        public AddChargingStationOperatorResult(ChargingStationOperator_Id  Id,
                                                EventTracking_Id            EventTrackingId,
                                                Boolean                     IsSuccess,
                                                String?                     Argument           = null,
                                                I18NString?                 ErrorDescription   = null,
                                                IRoamingNetwork?            RoamingNetwork     = null)

            : base(Id,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.RoamingNetwork = RoamingNetwork;

        }


        public static AddChargingStationOperatorResult Success(IChargingStationOperator  ChargingStationOperator,
                                                               EventTracking_Id          EventTrackingId,
                                                               IRoamingNetwork?          RoamingNetwork   = null)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    true,
                    null,
                    null,
                    RoamingNetwork);


        public static AddChargingStationOperatorResult ArgumentError(IChargingStationOperator  ChargingStationOperator,
                                                                     EventTracking_Id          EventTrackingId,
                                                                     String                    Argument,
                                                                     String                    Description)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    Argument,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ));

        public static AddChargingStationOperatorResult ArgumentError(IChargingStationOperator  ChargingStationOperator,
                                                                     EventTracking_Id          EventTrackingId,
                                                                     String                    Argument,
                                                                     I18NString                Description)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static AddChargingStationOperatorResult Failed(ChargingStationOperator_Id  Id,
                                                              EventTracking_Id            EventTrackingId,
                                                              String                      Description,
                                                              IRoamingNetwork?            RoamingNetwork   = null)

            => new (Id,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    RoamingNetwork);

        public static AddChargingStationOperatorResult Failed(IChargingStationOperator  ChargingStationOperator,
                                                              EventTracking_Id          EventTrackingId,
                                                              String                    Description,
                                                              IRoamingNetwork?          RoamingNetwork   = null)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    RoamingNetwork);

        public static AddChargingStationOperatorResult Failed(IChargingStationOperator  ChargingStationOperator,
                                                              EventTracking_Id          EventTrackingId,
                                                              I18NString                Description,
                                                              IRoamingNetwork?          RoamingNetwork   = null)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    null,
                    Description,
                    RoamingNetwork);

        public static AddChargingStationOperatorResult Failed(IChargingStationOperator  ChargingStationOperator,
                                                              EventTracking_Id          EventTrackingId,
                                                              Exception                 Exception,
                                                              IRoamingNetwork?          RoamingNetwork   = null)

            => new (ChargingStationOperator,
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
