﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of an add or update energy meters request.
    /// </summary>
    public class AddOrUpdateEnergyMetersResult : AEnititiesResult<AddOrUpdateEnergyMeterResult,
                                                                  IEnergyMeter,
                                                                  EnergyMeter_Id>
    {

        #region Constructor(s)

        public AddOrUpdateEnergyMetersResult(CommandResult                               Result,
                                             IEnumerable<AddOrUpdateEnergyMeterResult>?  SuccessfulEnergyMeters   = null,
                                             IEnumerable<AddOrUpdateEnergyMeterResult>?  RejectedEnergyMeters     = null,
                                             IId?                                        SenderId                 = null,
                                             Object?                                     Sender                   = null,
                                             EventTracking_Id?                           EventTrackingId          = null,
                                             I18NString?                                 Description              = null,
                                             IEnumerable<Warning>?                       Warnings                 = null,
                                             TimeSpan?                                   Runtime                  = null)

            : base(Result,
                   SuccessfulEnergyMeters,
                   RejectedEnergyMeters,
                   SenderId,
                   Sender,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedEnergyMeters,   ...)

        public static AddOrUpdateEnergyMetersResult

            AdminDown(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                      IId?                       SenderId          = null,
                      Object?                    Sender            = null,
                      EventTracking_Id?          EventTrackingId   = null,
                      I18NString?                Description       = null,
                      IEnumerable<Warning>?      Warnings          = null,
                      TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.AdminDown(energyMeter,
                                                                                                          EventTrackingId,
                                                                                                          SenderId,
                                                                                                          Sender)),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) NoOperation  (RejectedEnergyMeters,   ...)

        public static AddOrUpdateEnergyMetersResult

            NoOperation(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                        IId?                       SenderId          = null,
                        Object?                    Sender            = null,
                        EventTracking_Id?          EventTrackingId   = null,
                        I18NString?                Description       = null,
                        IEnumerable<Warning>?      Warnings          = null,
                        TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.NoOperation(energyMeter,
                                                                                                            EventTrackingId,
                                                                                                            SenderId,
                                                                                                            Sender)),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) Enqueued     (SuccessfulChargingPools, ...)

        public static AddOrUpdateEnergyMetersResult

            Enqueued(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                     IId?                       SenderId          = null,
                     Object?                    Sender            = null,
                     EventTracking_Id?          EventTrackingId   = null,
                     I18NString?                Description       = null,
                     IEnumerable<Warning>?      Warnings          = null,
                     TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(energyMeter => AddOrUpdateEnergyMeterResult.Enqueued(energyMeter,
                                                                                                            EventTrackingId,
                                                                                                            SenderId,
                                                                                                            Sender)),
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Added        (SuccessfulChargingPools, ...)

        public static AddOrUpdateEnergyMetersResult

            Added(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                  IId?                       SenderId          = null,
                  Object?                    Sender            = null,
                  EventTracking_Id?          EventTrackingId   = null,
                  I18NString?                Description       = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(energyMeter => AddOrUpdateEnergyMeterResult.Added(energyMeter,
                                                                                                         EventTrackingId,
                                                                                                         SenderId,
                                                                                                         Sender)),
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Updated      (SuccessfulChargingPools, ...)

        public static AddOrUpdateEnergyMetersResult

            Updated(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                    IId?                       SenderId          = null,
                    Object?                    Sender            = null,
                    EventTracking_Id?          EventTrackingId   = null,
                    I18NString?                Description       = null,
                    IEnumerable<Warning>?      Warnings          = null,
                    TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(energyMeter => AddOrUpdateEnergyMeterResult.Updated(energyMeter,
                                                                                                           EventTrackingId,
                                                                                                           SenderId,
                                                                                                           Sender)),
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedEnergyMeters, Description, ...)

        public static AddOrUpdateEnergyMetersResult

            ArgumentError(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                          I18NString                 Description,
                          EventTracking_Id?          EventTrackingId   = null,
                          IId?                       SenderId          = null,
                          Object?                    Sender            = null,
                          IEnumerable<Warning>?      Warnings          = null,
                          TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.ArgumentError(energyMeter,
                                                                                                              Description,
                                                                                                              EventTrackingId,
                                                                                                              SenderId,
                                                                                                              Sender)),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Error        (RejectedEnergyMeters, Description, ...)

        public static AddOrUpdateEnergyMetersResult

            Error(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                  I18NString                 Description,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       SenderId          = null,
                  Object?                    Sender            = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.Error(energyMeter,
                                                                                                      Description,
                                                                                                      EventTrackingId,
                                                                                                      SenderId,
                                                                                                      Sender)),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Error        (RejectedEnergyMeters, Exception,   ...)

        public static AddOrUpdateEnergyMetersResult

            Error(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                  Exception                  Exception,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       SenderId          = null,
                  Object?                    Sender            = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.Error(energyMeter,
                                                                                                      Exception,
                                                                                                      EventTrackingId,
                                                                                                      SenderId,
                                                                                                      Sender)),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) LockTimeout  (RejectedEnergyMeters, Timeout,     ...)

        public static AddOrUpdateEnergyMetersResult

            LockTimeout(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                        TimeSpan                   Timeout,
                        IId?                       SenderId          = null,
                        Object?                    Sender            = null,
                        EventTracking_Id?          EventTrackingId   = null,
                        I18NString?                Description       = null,
                        IEnumerable<Warning>?      Warnings          = null,
                        TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.LockTimeout(energyMeter,
                                                                                                            Timeout,
                                                                                                            EventTrackingId,
                                                                                                            SenderId,
                                                                                                            Sender)),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


    }

}
