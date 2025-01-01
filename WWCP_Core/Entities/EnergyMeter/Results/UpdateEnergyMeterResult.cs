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
    /// The result of an update energy meter request.
    /// </summary>
    public class UpdateEnergyMeterResult : AEnitityResult<IEnergyMeter, EnergyMeter_Id>
    {

        #region Properties

        public IEnergyMeter?   EnergyMeter
            => Entity;

        public IChargingPool?  ChargingPool    { get; internal set; }

        #endregion

        #region Constructor(s)

        public UpdateEnergyMeterResult(IEnergyMeter           EnergyMeter,
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

        #endregion


        #region (static) AdminDown    (EnergyMeter, ...)

        public static UpdateEnergyMeterResult

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

        #region (static) NoOperation  (EnergyMeter, ...)

        public static UpdateEnergyMeterResult

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


        #region (static) Enqueued     (EnergyMeter, ...)

        public static UpdateEnergyMeterResult

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

        #region (static) Success      (EnergyMeter, ...)

        public static UpdateEnergyMeterResult

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

        #region (static) Exists(...)

        public static UpdateEnergyMeterResult

            Exists(IEnergyMeter           EnergyMeter,
                   EventTracking_Id?      EventTrackingId   = null,
                   IId?                   SenderId          = null,
                   Object?                Sender            = null,
                   IChargingPool?         ChargingPool      = null,
                   I18NString?            Description       = null,
                   IEnumerable<Warning>?  Warnings          = null,
                   TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Exists,
                        EventTrackingId,
                        SenderId,
                        Sender,
                        ChargingPool,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(EnergyMeter, Description, ...)

        public static UpdateEnergyMeterResult

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

        #region (static) Error        (EnergyMeter, Description, ...)

        public static UpdateEnergyMeterResult

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

        #region (static) Error        (EnergyMeter, Exception,   ...)

        public static UpdateEnergyMeterResult

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

        #region (static) LockTimeout  (EnergyMeter, Timeout,     ...)

        public static UpdateEnergyMeterResult

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
