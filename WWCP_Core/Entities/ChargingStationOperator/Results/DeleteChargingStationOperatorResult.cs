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
    /// The result of a delete charging station operator request.
    /// </summary>
    public class DeleteChargingStationOperatorResult : AEnitityResult<IChargingStationOperator, ChargingStationOperator_Id>
    {

        #region Properties

        public IChargingStationOperator?  ChargingStationOperator
            => Entity;

        public IRoamingNetwork?           RoamingNetwork    { get; internal set; }

        #endregion

        #region Constructor(s)

        public DeleteChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                   CommandResult       Result,
                                                   EventTracking_Id?         EventTrackingId   = null,
                                                   IId?                      SenderId          = null,
                                                   Object?                   Sender            = null,
                                                   IRoamingNetwork?          RoamingNetwork    = null,
                                                   I18NString?               Description       = null,
                                                   IEnumerable<Warning>?     Warnings          = null,
                                                   TimeSpan?                 Runtime           = null)

            : base(ChargingStationOperator,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.RoamingNetwork = RoamingNetwork;

        }


        public DeleteChargingStationOperatorResult(ChargingStationOperator_Id  ChargingStationOperatorId,
                                                   CommandResult         Result,
                                                   EventTracking_Id?           EventTrackingId   = null,
                                                   IId?                        SenderId          = null,
                                                   Object?                     Sender            = null,
                                                   IRoamingNetwork?            RoamingNetwork    = null,
                                                   I18NString?                 Description       = null,
                                                   IEnumerable<Warning>?       Warnings          = null,
                                                   TimeSpan?                   Runtime           = null)

            : base(ChargingStationOperatorId,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.RoamingNetwork = RoamingNetwork;

        }

        #endregion


        #region (static) AdminDown      (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            AdminDown(IChargingStationOperator  ChargingStationOperator,
                      EventTracking_Id?         EventTrackingId   = null,
                      IId?                      SenderId          = null,
                      Object?                   Sender            = null,
                      IRoamingNetwork?          RoamingNetwork    = null,
                      I18NString?               Description       = null,
                      IEnumerable<Warning>?     Warnings          = null,
                      TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            NoOperation(IChargingStationOperator  ChargingStationOperator,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      SenderId          = null,
                        Object?                   Sender            = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        I18NString?               Description       = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            Enqueued(IChargingStationOperator  ChargingStationOperator,
                     EventTracking_Id?         EventTrackingId   = null,
                     IId?                      SenderId          = null,
                     Object?                   Sender            = null,
                     IRoamingNetwork?          RoamingNetwork    = null,
                     I18NString?               Description       = null,
                     IEnumerable<Warning>?     Warnings          = null,
                     TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            Success(IChargingStationOperator  ChargingStationOperator,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      SenderId          = null,
                    Object?                   Sender            = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    I18NString?               Description       = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            CanNotBeRemoved(IChargingStationOperator  ChargingStationOperator,
                            EventTracking_Id?         EventTrackingId   = null,
                            IId?                      SenderId          = null,
                            Object?                   Sender            = null,
                            IRoamingNetwork?          RoamingNetwork    = null,
                            I18NString?               Description       = null,
                            IEnumerable<Warning>?     Warnings          = null,
                            TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.CanNotBeRemoved,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError  (ChargingStationOperator,   Description, ...)

        public static DeleteChargingStationOperatorResult

            ArgumentError(IChargingStationOperator  ChargingStationOperator,
                          I18NString                Description,
                          EventTracking_Id?         EventTrackingId   = null,
                          IId?                      SenderId          = null,
                          Object?                   Sender            = null,
                          IRoamingNetwork?          RoamingNetwork    = null,
                          IEnumerable<Warning>?     Warnings          = null,
                          TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError  (ChargingStationOperatorId, Description, ...)

        public static DeleteChargingStationOperatorResult

            ArgumentError(ChargingStationOperator_Id  ChargingStationOperatorId,
                          I18NString                  Description,
                          EventTracking_Id?           EventTrackingId   = null,
                          IId?                        SenderId          = null,
                          Object?                     Sender            = null,
                          IRoamingNetwork?            RoamingNetwork    = null,
                          IEnumerable<Warning>?       Warnings          = null,
                          TimeSpan?                   Runtime           = null)

                => new (ChargingStationOperatorId,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingStationOperator,   Description, ...)

        public static DeleteChargingStationOperatorResult

            Error(IChargingStationOperator  ChargingStationOperator,
                  I18NString                Description,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      SenderId          = null,
                  Object?                   Sender            = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (ChargingStationOperator,   Exception,   ...)

        public static DeleteChargingStationOperatorResult

            Error(IChargingStationOperator  ChargingStationOperator,
                  Exception                 Exception,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      SenderId          = null,
                  Object?                   Sender            = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout        (ChargingStationOperator,   Timeout,     ...)

        public static DeleteChargingStationOperatorResult

            Timeout(IChargingStationOperator  ChargingStationOperator,
                    TimeSpan                  Timeout,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      SenderId          = null,
                    Object?                   Sender            = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Timeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (ChargingStationOperator,   Timeout,     ...)

        public static DeleteChargingStationOperatorResult

            LockTimeout(IChargingStationOperator  ChargingStationOperator,
                        TimeSpan                  Timeout,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      SenderId          = null,
                        Object?                   Sender            = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        RoamingNetwork,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
