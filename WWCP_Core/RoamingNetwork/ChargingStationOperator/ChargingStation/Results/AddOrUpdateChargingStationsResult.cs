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

    public class AddOrUpdateChargingStationsResult : AEnititiesResult<AddOrUpdateChargingStationResult,
                                                                      IChargingStation,
                                                                      ChargingStation_Id>
    {

        #region Constructor(s)

        public AddOrUpdateChargingStationsResult(PushDataResultTypes                             Result,
                                                 IEnumerable<AddOrUpdateChargingStationResult>?  SuccessfulChargingStations   = null,
                                                 IEnumerable<AddOrUpdateChargingStationResult>?  RejectedChargingStations     = null,
                                                 IId?                                            AuthId                       = null,
                                                 Object?                                         SendPOIData                  = null,
                                                 EventTracking_Id?                               EventTrackingId              = null,
                                                 I18NString?                                     Description                  = null,
                                                 IEnumerable<Warning>?                           Warnings                     = null,
                                                 TimeSpan?                                       Runtime                      = null)

            : base(Result,
                   SuccessfulChargingStations,
                   RejectedChargingStations,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) NoOperation

        public static AddOrUpdateChargingStationsResult

            NoOperation(IEnumerable<IChargingStation>  RejectedChargingStations,
                        IId?                           AuthId            = null,
                        Object?                        SendPOIData       = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

            {

                EventTrackingId ??= EventTracking_Id.New;

                return new (PushDataResultTypes.NoOperation,
                            Array.Empty<AddOrUpdateChargingStationResult>(),
                            RejectedChargingStations.Select(chargingStation => AddOrUpdateChargingStationResult.NoOperation(chargingStation,
                                                                                                                            EventTrackingId,
                                                                                                                            AuthId,
                                                                                                                            SendPOIData)),
                            AuthId,
                            SendPOIData,
                            EventTrackingId,
                            Description,
                            Warnings,
                            Runtime);

            }

        #endregion


    }

}
