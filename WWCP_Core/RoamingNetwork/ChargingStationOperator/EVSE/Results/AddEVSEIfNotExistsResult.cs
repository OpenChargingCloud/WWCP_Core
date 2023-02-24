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

    public class AddEVSEIfNotExistsResult : AEnitityResult<IEVSE, EVSE_Id>
    {

        public IEVSE? EVSE
            => Object;

        public IChargingStation?  ChargingStation    { get; internal set; }

        public AddedOrIgnored?    AddedOrIgnored     { get; internal set; }


        public AddEVSEIfNotExistsResult(IEVSE              EVSE,
                                        EventTracking_Id   EventTrackingId,
                                        Boolean            IsSuccess,
                                        String?            Argument           = null,
                                        I18NString?        ErrorDescription   = null,
                                        IChargingStation?  ChargingStation    = null,
                                        AddedOrIgnored?    AddedOrIgnored     = null)

            : base(EVSE,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.ChargingStation  = ChargingStation;
            this.AddedOrIgnored   = AddedOrIgnored;

        }


        public static AddEVSEIfNotExistsResult Success(IEVSE              EVSE,
                                                       AddedOrIgnored     AddedOrIgnored,
                                                       EventTracking_Id   EventTrackingId,
                                                       IChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    true,
                    null,
                    null,
                    ChargingStation,
                    AddedOrIgnored);


        public static AddEVSEIfNotExistsResult ArgumentError(IEVSE             EVSE,
                                                             EventTracking_Id  EventTrackingId,
                                                             String            Argument,
                                                             String            Description)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    Argument,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ));

        public static AddEVSEIfNotExistsResult ArgumentError(IEVSE             EVSE,
                                                             EventTracking_Id  EventTrackingId,
                                                             String            Argument,
                                                             I18NString        Description)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static AddEVSEIfNotExistsResult Failed(IEVSE             EVSE,
                                                      EventTracking_Id  EventTrackingId,
                                                      String            Description,
                                                      ChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    ChargingStation);

        public static AddEVSEIfNotExistsResult Failed(IEVSE              EVSE,
                                                      EventTracking_Id   EventTrackingId,
                                                      I18NString         Description,
                                                      IChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    null,
                    Description,
                    ChargingStation);

        public static AddEVSEIfNotExistsResult Failed(IEVSE              EVSE,
                                                      EventTracking_Id   EventTrackingId,
                                                      Exception          Exception,
                                                      IChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Exception.Message
                    ),
                    ChargingStation);

    }

}