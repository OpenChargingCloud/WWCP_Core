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

    public class RemoveEVSEResult : AEnitityResult<IEVSE, EVSE_Id>
    {

        public IEVSE? EVSE
            => Object;

        public IChargingStation?  ChargingStation    { get; internal set; }


        public RemoveEVSEResult(IEVSE              EVSE,
                                EventTracking_Id   EventTrackingId,
                                Boolean            IsSuccess,
                                String?            Argument           = null,
                                I18NString?        ErrorDescription   = null,
                                IChargingStation?  ChargingStation    = null)

            : base(EVSE,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.ChargingStation = ChargingStation;

        }

        public RemoveEVSEResult(EVSE_Id            Id,
                                EventTracking_Id   EventTrackingId,
                                Boolean            IsSuccess,
                                String?            Argument           = null,
                                I18NString?        ErrorDescription   = null,
                                IChargingStation?  ChargingStation    = null)

            : base(Id,
                   EventTrackingId,
                   IsSuccess,
                   Argument,
                   ErrorDescription)

        {

            this.ChargingStation = ChargingStation;

        }


        public static RemoveEVSEResult Success(IEVSE              EVSE,
                                               EventTracking_Id   EventTrackingId,
                                               IChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    true,
                    null,
                    null,
                    ChargingStation);


        public static RemoveEVSEResult ArgumentError(IEVSE             EVSE,
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

        public static RemoveEVSEResult ArgumentError(EVSE              EVSE,
                                                     EventTracking_Id  EventTrackingId,
                                                     String            Argument,
                                                     I18NString        Description)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    Argument,
                    Description);


        public static RemoveEVSEResult Failed(EVSE_Id            Id,
                                              EventTracking_Id   EventTrackingId,
                                              String             Description,
                                              IChargingStation?  ChargingStation   = null)

            => new (Id,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    ChargingStation);

        public static RemoveEVSEResult Failed(IEVSE              EVSE,
                                              EventTracking_Id   EventTrackingId,
                                              String             Description,
                                              IChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    null,
                    I18NString.Create(
                        Languages.en,
                        Description
                    ),
                    ChargingStation);

        public static RemoveEVSEResult Failed(IEVSE              EVSE,
                                              EventTracking_Id   EventTrackingId,
                                              I18NString         Description,
                                              IChargingStation?  ChargingStation   = null)

            => new (EVSE,
                    EventTrackingId,
                    false,
                    null,
                    Description,
                    ChargingStation);

        public static RemoveEVSEResult Failed(IEVSE              EVSE,
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
