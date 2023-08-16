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
    /// The result of an add or update energy meter request.
    /// </summary>
    public class AddOrUpdateEnergyMeterResult : AEnitityResult<IEnergyMeter, EnergyMeter_Id>
    {

        #region Properties

        public IEnergyMeter?    EnergyMeter
            => Object;

        public IChargingPool?   ChargingPool      { get; internal set; }

        public AddedOrUpdated?  AddedOrUpdated    { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateEnergyMeterResult(IEnergyMeter           EnergyMeter,
                                            CommandResult    Result,
                                            EventTracking_Id?      EventTrackingId   = null,
                                            IId?                   AuthId            = null,
                                            Object?                SendPOIData       = null,
                                            IChargingPool?         ChargingPool      = null,
                                            AddedOrUpdated?        AddedOrUpdated    = null,
                                            I18NString?            Description       = null,
                                            IEnumerable<Warning>?  Warnings          = null,
                                            TimeSpan?              Runtime           = null)

            : base(EnergyMeter,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingPool    = ChargingPool;
            this.AddedOrUpdated  = AddedOrUpdated;

        }

        #endregion


        #region (static) AdminDown    (EnergyMeter, ...)

        public static AddOrUpdateEnergyMeterResult

            AdminDown(IEnergyMeter           EnergyMeter,
                      EventTracking_Id?      EventTrackingId   = null,
                      IId?                   AuthId            = null,
                      Object?                SendPOIData       = null,
                      IChargingPool?         ChargingPool      = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (EnergyMeter, ...)

        public static AddOrUpdateEnergyMeterResult

            NoOperation(IEnergyMeter           EnergyMeter,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingPool?         ChargingPool      = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (EnergyMeter, ...)

        public static AddOrUpdateEnergyMeterResult

            Enqueued(IEnergyMeter           EnergyMeter,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   AuthId            = null,
                     Object?                SendPOIData       = null,
                     IChargingPool?         ChargingPool      = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Enqueued,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Added        (EnergyMeter,...)

        public static AddOrUpdateEnergyMeterResult

            Added(IEnergyMeter           EnergyMeter,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  I18NString?            Description       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated      (EnergyMeter,...)

        public static AddOrUpdateEnergyMeterResult

            Updated(IEnergyMeter           EnergyMeter,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   AuthId            = null,
                    Object?                SendPOIData       = null,
                    IChargingPool?         ChargingPool      = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(EnergyMeter, Description, ...)

        public static AddOrUpdateEnergyMeterResult

            ArgumentError(IEnergyMeter           EnergyMeter,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IChargingPool?         ChargingPool      = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (EnergyMeter, Description, ...)

        public static AddOrUpdateEnergyMeterResult

            Error(IEnergyMeter           EnergyMeter,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (EnergyMeter, Exception,   ...)

        public static AddOrUpdateEnergyMeterResult

            Error(IEnergyMeter           EnergyMeter,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingPool?         ChargingPool      = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EnergyMeter,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (EnergyMeter, Timeout,     ...)

        public static AddOrUpdateEnergyMeterResult

            LockTimeout(IEnergyMeter           EnergyMeter,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingPool?         ChargingPool      = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new(EnergyMeter,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingPool,
                        org.GraphDefined.Vanaheimr.Hermod.AddedOrUpdated.Failed,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
