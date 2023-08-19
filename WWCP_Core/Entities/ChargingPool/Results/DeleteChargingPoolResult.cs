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
    /// The result of a delete charging pool request.
    /// </summary>
    public class DeleteChargingPoolResult : AEnitityResult<IChargingPool, ChargingPool_Id>
    {

        #region Properties

        public IChargingPool?             ChargingPool
            => Entity;

        public IChargingStationOperator?  ChargingStationOperator    { get; internal set; }

        #endregion

        #region Constructor(s)

        public DeleteChargingPoolResult(IChargingPool              ChargingPool,
                                        CommandResult        Result,
                                        EventTracking_Id?          EventTrackingId           = null,
                                        IId?                       SenderId                  = null,
                                        Object?                    Sender                    = null,
                                        IChargingStationOperator?  ChargingStationOperator   = null,
                                        I18NString?                Description               = null,
                                        IEnumerable<Warning>?      Warnings                  = null,
                                        TimeSpan?                  Runtime                   = null)

            : base(ChargingPool,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStationOperator = ChargingStationOperator;

        }


        public DeleteChargingPoolResult(ChargingPool_Id            Id,
                                        CommandResult        Result,
                                        EventTracking_Id?          EventTrackingId           = null,
                                        IId?                       SenderId                  = null,
                                        Object?                    Sender                    = null,
                                        IChargingStationOperator?  ChargingStationOperator   = null,
                                        I18NString?                Description               = null,
                                        IEnumerable<Warning>?      Warnings                  = null,
                                        TimeSpan?                  Runtime                   = null)

            : base(Id,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStationOperator = ChargingStationOperator;

        }

        #endregion


        #region (static) AdminDown      (ChargingPool, ...)

        public static DeleteChargingPoolResult

            AdminDown(IChargingPool              ChargingPool,
                      EventTracking_Id?          EventTrackingId           = null,
                      IId?                       SenderId                  = null,
                      Object?                    Sender                    = null,
                      IChargingStationOperator?  ChargingStationOperator   = null,
                      I18NString?                Description               = null,
                      IEnumerable<Warning>?      Warnings                  = null,
                      TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (ChargingPool, ...)

        public static DeleteChargingPoolResult

            NoOperation(IChargingPool              ChargingPool,
                        EventTracking_Id?          EventTrackingId           = null,
                        IId?                       SenderId                  = null,
                        Object?                    Sender                    = null,
                        IChargingStationOperator?  ChargingStationOperator   = null,
                        I18NString?                Description               = null,
                        IEnumerable<Warning>?      Warnings                  = null,
                        TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (ChargingPool, ...)

        public static DeleteChargingPoolResult

            Enqueued(IChargingPool              ChargingPool,
                     EventTracking_Id?          EventTrackingId           = null,
                     IId?                       SenderId                  = null,
                     Object?                    Sender                    = null,
                     IChargingStationOperator?  ChargingStationOperator   = null,
                     I18NString?                Description               = null,
                     IEnumerable<Warning>?      Warnings                  = null,
                     TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (ChargingPool, ...)

        public static DeleteChargingPoolResult

            Success(IChargingPool              ChargingPool,
                    EventTracking_Id?          EventTrackingId           = null,
                    IId?                       SenderId                  = null,
                    Object?                    Sender                    = null,
                    IChargingStationOperator?  ChargingStationOperator   = null,
                    I18NString?                Description               = null,
                    IEnumerable<Warning>?      Warnings                  = null,
                    TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(ChargingPool, ...)

        public static DeleteChargingPoolResult

            CanNotBeRemoved(IChargingPool              ChargingPool,
                            EventTracking_Id?          EventTrackingId           = null,
                            IId?                       SenderId                  = null,
                            Object?                    Sender                    = null,
                            IChargingStationOperator?  ChargingStationOperator   = null,
                            I18NString?                Description               = null,
                            IEnumerable<Warning>?      Warnings                  = null,
                            TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.CanNotBeRemoved,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError  (ChargingPool,   Description, ...)

        public static DeleteChargingPoolResult

            ArgumentError(IChargingPool              ChargingPool,
                          I18NString                 Description,
                          EventTracking_Id?          EventTrackingId           = null,
                          IId?                       SenderId                  = null,
                          Object?                    Sender                    = null,
                          IChargingStationOperator?  ChargingStationOperator   = null,
                          IEnumerable<Warning>?      Warnings                  = null,
                          TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError  (ChargingPoolId, Description, ...)

        public static DeleteChargingPoolResult

            ArgumentError(ChargingPool_Id            ChargingPoolId,
                          I18NString                 Description,
                          EventTracking_Id?          EventTrackingId           = null,
                          IId?                       SenderId                  = null,
                          Object?                    Sender                    = null,
                          IChargingStationOperator?  ChargingStationOperator   = null,
                          IEnumerable<Warning>?      Warnings                  = null,
                          TimeSpan?                  Runtime                   = null)

                => new (ChargingPoolId,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingPool,   Description, ...)

        public static DeleteChargingPoolResult

            Error(IChargingPool              ChargingPool,
                  I18NString                 Description,
                  EventTracking_Id?          EventTrackingId           = null,
                  IId?                       SenderId                  = null,
                  Object?                    Sender                    = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingPool,   Exception,   ...)

        public static DeleteChargingPoolResult

            Error(IChargingPool              ChargingPool,
                  Exception                  Exception,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       SenderId                  = null,
                  Object?                    Sender                    = null,
                  IChargingStationOperator?  ChargingStationOperator   = null,
                  IEnumerable<Warning>?      Warnings                  = null,
                  TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout        (ChargingPool,   Timeout, ...)

        public static DeleteChargingPoolResult

            Timeout(IChargingPool              ChargingPool,
                    TimeSpan                   Timeout,
                    EventTracking_Id?          EventTrackingId           = null,
                    IId?                       SenderId                  = null,
                    Object?                    Sender                    = null,
                    IChargingStationOperator?  ChargingStationOperator   = null,
                    IEnumerable<Warning>?      Warnings                  = null,
                    TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.Timeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (ChargingPool,   Timeout, ...)

        public static DeleteChargingPoolResult

            LockTimeout(IChargingPool              ChargingPool,
                        TimeSpan                   Timeout,
                        EventTracking_Id?          EventTrackingId           = null,
                        IId?                       SenderId                  = null,
                        Object?                    Sender                    = null,
                        IChargingStationOperator?  ChargingStationOperator   = null,
                        IEnumerable<Warning>?      Warnings                  = null,
                        TimeSpan?                  Runtime                   = null)

                => new (ChargingPool,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingStationOperator,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
