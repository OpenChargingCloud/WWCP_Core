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

    /// <summary>
    /// The result of an update charging station request.
    /// </summary>
    public class UpdateChargingStationResult : AEnitityResult<IChargingStation, ChargingStation_Id>
    {

        #region Properties

        public IChargingStation?  ChargingStation
            => Object;

        public IChargingPool?     ChargingPool    { get; internal set; }

        #endregion

        #region Constructor(s)

        public UpdateChargingStationResult(IChargingStation       ChargingStation,
                                           PushDataResultTypes    Result,
                                           EventTracking_Id?      EventTrackingId   = null,
                                           IId?                   AuthId            = null,
                                           Object?                SendPOIData       = null,
                                           IChargingPool?         ChargingPool      = null,
                                           I18NString?            Description       = null,
                                           IEnumerable<Warning>?  Warnings          = null,
                                           TimeSpan?              Runtime           = null)

            : base(ChargingStation,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingPool = ChargingPool;

        }


        public UpdateChargingStationResult(ChargingStation_Id     ChargingStationId,
                                           PushDataResultTypes    Result,
                                           EventTracking_Id?      EventTrackingId   = null,
                                           IId?                   AuthId            = null,
                                           Object?                SendPOIData       = null,
                                           IChargingPool?         ChargingPool      = null,
                                           I18NString?            Description       = null,
                                           IEnumerable<Warning>?  Warnings          = null,
                                           TimeSpan?              Runtime           = null)

            : base(ChargingStationId,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingPool = ChargingPool;

        }

        #endregion


        #region (static) NoOperation  (ChargingStation, ...)

        public static UpdateChargingStationResult

            NoOperation(IChargingStation       ChargingStation,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingPool?         ChargingPool      = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (ChargingStation, ...)

        public static UpdateChargingStationResult

            Enqueued(IChargingStation       ChargingStation,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   AuthId            = null,
                     Object?                SendPOIData       = null,
                     IChargingPool?         ChargingPool      = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success      (ChargingStation, ...)

        public static UpdateChargingStationResult

            Success(IChargingStation       ChargingStation,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   AuthId            = null,
                    Object?                SendPOIData       = null,
                    IChargingPool?         ChargingPool      = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(ChargingStation,   Description, ...)

        public static UpdateChargingStationResult

            ArgumentError(IChargingStation       ChargingStation,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError(ChargingStationId, Description, ...)

        public static UpdateChargingStationResult

            ArgumentError(ChargingStation_Id     ChargingStationId,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (ChargingStationId,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStation,   Description, ...)

        public static UpdateChargingStationResult

            Error(IChargingStation       ChargingStation,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStation,   Exception,   ...)

        public static UpdateChargingStationResult

            Error(IChargingStation       ChargingStation,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Exception.Message.ToI18NString(Languages.en),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (ChargingStation,   Timeout,     ...)

        public static UpdateChargingStationResult

            LockTimeout(IChargingStation       ChargingStation,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingPool?         ChargingPool      = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        PushDataResultTypes.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(Languages.en),
                        Warnings,
                        Runtime);

        #endregion


    }

}
