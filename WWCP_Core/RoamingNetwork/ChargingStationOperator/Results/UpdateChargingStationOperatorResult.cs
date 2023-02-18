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

    public class UpdateChargingStationOperatorResult : AEnitityResult<IChargingStationOperator, ChargingStationOperator_Id>
    {

        public IChargingStationOperator? ChargingStationOperator
            => Object;


        public UpdateChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                   EventTracking_Id          EventTrackingId,
                                                   Boolean                   IsSuccess,
                                                   String?                   Argument           = null,
                                                   I18NString?               ErrorDescription   = null)

            : base(ChargingStationOperator,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        { }


        public static UpdateChargingStationOperatorResult Success(IChargingStationOperator  ChargingStationOperator,
                                                                  EventTracking_Id          EventTrackingId)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    true,
                    null,
                    null);


        public static UpdateChargingStationOperatorResult ArgumentError(IChargingStationOperator  ChargingStationOperator,
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

        public static UpdateChargingStationOperatorResult ArgumentError(IChargingStationOperator  ChargingStationOperator,
                                                                        EventTracking_Id          EventTrackingId,
                                                                        String                    Argument,
                                                                        I18NString                Description)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static UpdateChargingStationOperatorResult Failed(IChargingStationOperator  ChargingStationOperator,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 String                    Description)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ));

        public static UpdateChargingStationOperatorResult Failed(IChargingStationOperator  ChargingStationOperator,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 I18NString                Description)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    null,
                    Description);

        public static UpdateChargingStationOperatorResult Failed(IChargingStationOperator  ChargingStationOperator,
                                                                 EventTracking_Id          EventTrackingId,
                                                                 Exception                 Exception)

            => new (ChargingStationOperator,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Exception.Message
                    ));

    }

}
