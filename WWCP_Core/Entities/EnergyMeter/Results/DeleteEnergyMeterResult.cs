/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a delete energy meter request.
    /// </summary>
    public class DeleteEnergyMeterResult : AEnitityResult<IEnergyMeter, EnergyMeter_Id>
    {

        #region Properties

        public IEnergyMeter?   EnergyMeter
            => Entity;

        public IChargingPool?  ChargingPool    { get; internal set; }

        #endregion

        #region Constructor(s)

        public DeleteEnergyMeterResult(IEnergyMeter           EnergyMeter,
                                       CommandResult    Result,
                                       EventTracking_Id?      EventTrackingId   = null,
                                       IId?                   SenderId          = null,
                                       Object?                Sender            = null,
                                       IChargingPool?         ChargingPool      = null,
                                       I18NString?            Description       = null,
                                       IEnumerable<Warning>?  Warnings          = null,
                                       TimeSpan?              Runtime           = null)

            : base(EnergyMeter,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingPool = ChargingPool;

        }


        public DeleteEnergyMeterResult(EnergyMeter_Id         EnergyMeterId,
                                       CommandResult    Result,
                                       EventTracking_Id?      EventTrackingId   = null,
                                       IId?                   SenderId          = null,
                                       Object?                Sender            = null,
                                       IChargingPool?         ChargingPool      = null,
                                       I18NString?            Description       = null,
                                       IEnumerable<Warning>?  Warnings          = null,
                                       TimeSpan?              Runtime           = null)

            : base(EnergyMeterId,
                   Result,
                   EventTrackingId,
                   SenderId,
                   Sender,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingPool = ChargingPool;

        }

        #endregion


        #region (static) AdminDown      (EnergyMeter, ...)

        public static DeleteEnergyMeterResult

            AdminDown(IEnergyMeter           EnergyMeter,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   SenderId          = null,
                      Object?                Sender            = null,
                      IChargingPool?         ChargingPool      = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (EnergyMeter, ...)

        public static DeleteEnergyMeterResult

            NoOperation(IEnergyMeter           EnergyMeter,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IChargingPool?         ChargingPool      = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (EnergyMeter, ...)

        public static DeleteEnergyMeterResult

            Enqueued(IEnergyMeter           EnergyMeter,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   SenderId          = null,
                     Object?                Sender            = null,
                     IChargingPool?         ChargingPool      = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (EnergyMeter, ...)

        public static DeleteEnergyMeterResult

            Success(IEnergyMeter           EnergyMeter,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   SenderId          = null,
                    Object?                Sender            = null,
                    IChargingPool?         ChargingPool      = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Success,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(EnergyMeter, ...)

        public static DeleteEnergyMeterResult

            CanNotBeRemoved(IEnergyMeter           EnergyMeter,
                            EventTracking_Id?      EventTrackingId   = null,
                            IId?                   SenderId          = null,
                            Object?                Sender            = null,
                            IChargingPool?         ChargingPool      = null,
                            I18NString?            Description       = null,
                            IEnumerable<Warning>?  Warnings          = null,
                            TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.CanNotBeRemoved,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError  (EnergyMeter,   Description, ...)

        public static DeleteEnergyMeterResult

            ArgumentError(IEnergyMeter           EnergyMeter,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   SenderId          = null,
                          Object?                Sender            = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError  (EnergyMeterId, Description, ...)

        public static DeleteEnergyMeterResult

            ArgumentError(EnergyMeter_Id         EnergyMeterId,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   SenderId          = null,
                          Object?                Sender            = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EnergyMeterId,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (EnergyMeter,   Description, ...)

        public static DeleteEnergyMeterResult

            Error(IEnergyMeter           EnergyMeter,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error          (EnergyMeter,   Exception,   ...)

        public static DeleteEnergyMeterResult

            Error(IEnergyMeter           EnergyMeter,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   SenderId          = null,
                  Object?                Sender            = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Error,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (EnergyMeter,   Timeout,     ...)

        public static DeleteEnergyMeterResult

            LockTimeout(IEnergyMeter           EnergyMeter,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   SenderId          = null,
                        Object?                Sender            = null,
                        IChargingPool?         ChargingPool      = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
