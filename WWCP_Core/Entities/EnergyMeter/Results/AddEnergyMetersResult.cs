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
    /// The result of an add energy meters request.
    /// </summary>
    public class AddEnergyMetersResult : AEnititiesResult<AddEnergyMeterResult,
                                                                      IEnergyMeter,
                                                                      EnergyMeter_Id>
    {

        #region Constructor(s)

        public AddEnergyMetersResult(CommandResult                 Result,
                                     IEnumerable<AddEnergyMeterResult>?  SuccessfulEnergyMeters   = null,
                                     IEnumerable<AddEnergyMeterResult>?  RejectedEnergyMeters     = null,
                                     IId?                                AuthId                   = null,
                                     Object?                             SendPOIData              = null,
                                     EventTracking_Id?                   EventTrackingId          = null,
                                     I18NString?                         Description              = null,
                                     IEnumerable<Warning>?               Warnings                 = null,
                                     TimeSpan?                           Runtime                  = null)

            : base(Result,
                   SuccessfulEnergyMeters,
                   RejectedEnergyMeters,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedEnergyMeters,   ...)

        public static AddEnergyMetersResult

            AdminDown(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                      IId?                       AuthId            = null,
                      Object?                    SendPOIData       = null,
                      EventTracking_Id?          EventTrackingId   = null,
                      I18NString?                Description       = null,
                      IEnumerable<Warning>?      Warnings          = null,
                      TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<AddEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddEnergyMeterResult.AdminDown(energyMeter,
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

        #region (static) NoOperation  (RejectedEnergyMeters,   ...)

        public static AddEnergyMetersResult

            NoOperation(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                        IId?                       AuthId            = null,
                        Object?                    SendPOIData       = null,
                        EventTracking_Id?          EventTrackingId   = null,
                        I18NString?                Description       = null,
                        IEnumerable<Warning>?      Warnings          = null,
                        TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<AddEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddEnergyMeterResult.NoOperation(energyMeter,
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


        #region (static) Enqueued     (SuccessfulChargingPools, ...)

        public static AddEnergyMetersResult

            Enqueued(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                     IId?                       AuthId            = null,
                     Object?                    SendPOIData       = null,
                     EventTracking_Id?          EventTrackingId   = null,
                     I18NString?                Description       = null,
                     IEnumerable<Warning>?      Warnings          = null,
                     TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(energyMeter => AddEnergyMeterResult.Enqueued(energyMeter,
                                                                                                    EventTrackingId,
                                                                                                    AuthId,
                                                                                                    SendPOIData)),
                        Array.Empty<AddEnergyMeterResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static AddEnergyMetersResult

            Success(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                    IId?                       AuthId            = null,
                    Object?                    SendPOIData       = null,
                    EventTracking_Id?          EventTrackingId   = null,
                    I18NString?                Description       = null,
                    IEnumerable<Warning>?      Warnings          = null,
                    TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(energyMeter => AddEnergyMeterResult.Success(energyMeter,
                                                                                                   EventTrackingId,
                                                                                                   AuthId,
                                                                                                   SendPOIData)),
                        Array.Empty<AddEnergyMeterResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedEnergyMeters, Description, ...)

        public static AddEnergyMetersResult

            ArgumentError(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                          I18NString                 Description,
                          EventTracking_Id?          EventTrackingId   = null,
                          IId?                       AuthId            = null,
                          Object?                    SendPOIData       = null,
                          IEnumerable<Warning>?      Warnings          = null,
                          TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<AddEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddEnergyMeterResult.ArgumentError(energyMeter,
                                                                                                      Description,
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

        #region (static) Error        (RejectedEnergyMeters, Description, ...)

        public static AddEnergyMetersResult

            Error(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                  I18NString                 Description,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       AuthId            = null,
                  Object?                    SendPOIData       = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<AddEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddEnergyMeterResult.Error(energyMeter,
                                                                                              Description,
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

        #region (static) Error        (RejectedEnergyMeters, Exception,   ...)

        public static AddEnergyMetersResult

            Error(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                  Exception                  Exception,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       AuthId            = null,
                  Object?                    SendPOIData       = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<AddEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddEnergyMeterResult.Error(energyMeter,
                                                                                              Exception,
                                                                                              EventTrackingId,
                                                                                              AuthId,
                                                                                              SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) LockTimeout  (RejectedEnergyMeters, Timeout,     ...)

        public static AddEnergyMetersResult

            LockTimeout(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                        TimeSpan                   Timeout,
                        IId?                       AuthId            = null,
                        Object?                    SendPOIData       = null,
                        EventTracking_Id?          EventTrackingId   = null,
                        I18NString?                Description       = null,
                        IEnumerable<Warning>?      Warnings          = null,
                        TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<AddEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddEnergyMeterResult.LockTimeout(energyMeter,
                                                                                                    Timeout,
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
