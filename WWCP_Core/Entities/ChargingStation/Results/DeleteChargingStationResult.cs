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
    /// The result of a delete charging station request.
    /// </summary>
    public class DeleteChargingStationResult : AEnitityResult<IChargingStation, ChargingStation_Id>
    {

        #region Properties

        public IChargingStation?  ChargingStation
            => Object;

        public IChargingPool?     ChargingPool    { get; internal set; }

        #endregion

        #region Constructor(s)

        public DeleteChargingStationResult(IChargingStation       ChargingStation,
                                           CommandResult    Result,
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


        public DeleteChargingStationResult(ChargingStation_Id     Id,
                                           CommandResult    Result,
                                           EventTracking_Id?      EventTrackingId   = null,
                                           IId?                   AuthId            = null,
                                           Object?                SendPOIData       = null,
                                           IChargingPool?         ChargingPool      = null,
                                           I18NString?            Description       = null,
                                           IEnumerable<Warning>?  Warnings          = null,
                                           TimeSpan?              Runtime           = null)

            : base(Id,
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


        #region (static) AdminDown      (ChargingStation, ...)

        public static DeleteChargingStationResult

            AdminDown(IChargingStation       ChargingStation,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   AuthId            = null,
                      Object?                SendPOIData       = null,
                      IChargingPool?         ChargingPool      = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (ChargingStation, ...)

        public static DeleteChargingStationResult

            NoOperation(IChargingStation       ChargingStation,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingPool?         ChargingPool      = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (ChargingStation, ...)

        public static DeleteChargingStationResult

            Enqueued(IChargingStation       ChargingStation,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   AuthId            = null,
                     Object?                SendPOIData       = null,
                     IChargingPool?         ChargingPool      = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (ChargingStation, ...)

        public static DeleteChargingStationResult

            Success(IChargingStation       ChargingStation,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   AuthId            = null,
                    Object?                SendPOIData       = null,
                    IChargingPool?         ChargingPool      = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(ChargingStation, ...)

        public static DeleteChargingStationResult

            CanNotBeRemoved(IChargingStation       ChargingStation,
                            EventTracking_Id?      EventTrackingId   = null,
                            IId?                   AuthId            = null,
                            Object?                SendPOIData       = null,
                            IChargingPool?         ChargingPool      = null,
                            I18NString?            Description       = null,
                            IEnumerable<Warning>?  Warnings          = null,
                            TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.CanNotBeRemoved,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError  (ChargingStation,   Description, ...)

        public static DeleteChargingStationResult

            ArgumentError(IChargingStation       ChargingStation,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError  (ChargingStationId, Description, ...)

        public static DeleteChargingStationResult

            ArgumentError(ChargingStation_Id     ChargingStationId,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (ChargingStationId,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingStation,   Description, ...)

        public static DeleteChargingStationResult

            Error(IChargingStation       ChargingStation,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingStation,   Exception,   ...)

        public static DeleteChargingStationResult

            Error(IChargingStation       ChargingStation,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (ChargingStation,   Timeout,     ...)

        public static DeleteChargingStationResult

            LockTimeout(IChargingStation       ChargingStation,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingPool?         ChargingPool      = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (ChargingStation,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
