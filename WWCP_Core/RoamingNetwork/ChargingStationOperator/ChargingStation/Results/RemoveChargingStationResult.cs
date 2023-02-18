﻿/*
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

    public class RemoveChargingStationResult : AEnitityResult<IChargingStation, ChargingStation_Id>
    {

        public IChargingStation? ChargingStation
            => Object;

        public IChargingPool?    ChargingPool    { get; internal set; }


        public RemoveChargingStationResult(IChargingStation  ChargingStation,
                                           EventTracking_Id  EventTrackingId,
                                           Boolean           IsSuccess,
                                           String?           Argument           = null,
                                           I18NString?       ErrorDescription   = null,
                                           IChargingPool?    ChargingPool       = null)

            : base(ChargingStation,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.ChargingPool = ChargingPool;

        }

        public RemoveChargingStationResult(ChargingStation_Id  Id,
                                           EventTracking_Id    EventTrackingId,
                                           Boolean             IsSuccess,
                                           String?             Argument           = null,
                                           I18NString?         ErrorDescription   = null,
                                           IChargingPool?      ChargingPool       = null)

            : base(Id,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.ChargingPool = ChargingPool;

        }


        public static RemoveChargingStationResult Success(IChargingStation  ChargingStation,
                                                          EventTracking_Id  EventTrackingId,
                                                          IChargingPool?    ChargingPool   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    true,
                    null,
                    null,
                    ChargingPool);


        public static RemoveChargingStationResult ArgumentError(IChargingStation  ChargingStation,
                                                                EventTracking_Id  EventTrackingId,
                                                                String            Argument,
                                                                String            Description)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    Argument,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ));

        public static RemoveChargingStationResult ArgumentError(IChargingStation  ChargingStation,
                                                                EventTracking_Id  EventTrackingId,
                                                                String            Argument,
                                                                I18NString        Description)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static RemoveChargingStationResult Failed(ChargingStation_Id  Id,
                                                         EventTracking_Id    EventTrackingId,
                                                         String              Description,
                                                         IChargingPool?      ChargingPool   = null)

            => new (Id,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    ChargingPool);


        public static RemoveChargingStationResult Failed(IChargingStation  ChargingStation,
                                                         EventTracking_Id  EventTrackingId,
                                                         String            Description,
                                                         IChargingPool?    ChargingPool   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    ChargingPool);

        public static RemoveChargingStationResult Failed(IChargingStation  ChargingStation,
                                                         EventTracking_Id  EventTrackingId,
                                                         I18NString        Description,
                                                         IChargingPool?    ChargingPool   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    null,
                    Description,
                    ChargingPool);

        public static RemoveChargingStationResult Failed(IChargingStation  ChargingStation,
                                                         EventTracking_Id  EventTrackingId,
                                                         Exception         Exception,
                                                         IChargingPool?    ChargingPool   = null)

            => new (ChargingStation,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Exception.Message
                    ),
                    ChargingPool);

    }

}
