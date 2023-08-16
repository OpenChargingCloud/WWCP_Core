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
    /// The result of an add or update energy meters request.
    /// </summary>
    public class AddOrUpdateEnergyMetersResult : AEnititiesResult<AddOrUpdateEnergyMeterResult,
                                                                              IEnergyMeter,
                                                                              EnergyMeter_Id>
    {

        #region Constructor(s)

        public AddOrUpdateEnergyMetersResult(PushDataResultTypes                         Result,
                                             IEnumerable<AddOrUpdateEnergyMeterResult>?  SuccessfulEnergyMeters   = null,
                                             IEnumerable<AddOrUpdateEnergyMeterResult>?  RejectedEnergyMeters     = null,
                                             IId?                                        AuthId                   = null,
                                             Object?                                     SendPOIData              = null,
                                             EventTracking_Id?                           EventTrackingId          = null,
                                             I18NString?                                 Description              = null,
                                             IEnumerable<Warning>?                       Warnings                 = null,
                                             TimeSpan?                                   Runtime                  = null)

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

        public static AddOrUpdateEnergyMetersResult

            AdminDown(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                      IId?                       AuthId            = null,
                      Object?                    SendPOIData       = null,
                      EventTracking_Id?          EventTrackingId   = null,
                      I18NString?                Description       = null,
                      IEnumerable<Warning>?      Warnings          = null,
                      TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.AdminDown,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.AdminDown(energyMeter,
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

        public static AddOrUpdateEnergyMetersResult

            NoOperation(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                        IId?                       AuthId            = null,
                        Object?                    SendPOIData       = null,
                        EventTracking_Id?          EventTrackingId   = null,
                        I18NString?                Description       = null,
                        IEnumerable<Warning>?      Warnings          = null,
                        TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.NoOperation,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.NoOperation(energyMeter,
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

        public static AddOrUpdateEnergyMetersResult

            Enqueued(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                     IId?                       AuthId            = null,
                     Object?                    SendPOIData       = null,
                     EventTracking_Id?          EventTrackingId   = null,
                     I18NString?                Description       = null,
                     IEnumerable<Warning>?      Warnings          = null,
                     TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Enqueued,
                        SuccessfulChargingPools.Select(energyMeter => AddOrUpdateEnergyMeterResult.Enqueued(energyMeter,
                                                                                                            EventTrackingId,
                                                                                                            AuthId,
                                                                                                            SendPOIData)),
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Added        (SuccessfulChargingPools, ...)

        public static AddOrUpdateEnergyMetersResult

            Added(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                  IId?                       AuthId            = null,
                  Object?                    SendPOIData       = null,
                  EventTracking_Id?          EventTrackingId   = null,
                  I18NString?                Description       = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingPools.Select(energyMeter => AddOrUpdateEnergyMeterResult.Added(energyMeter,
                                                                                                         EventTrackingId,
                                                                                                         AuthId,
                                                                                                         SendPOIData)),
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Updated      (SuccessfulChargingPools, ...)

        public static AddOrUpdateEnergyMetersResult

            Updated(IEnumerable<IEnergyMeter>  SuccessfulChargingPools,
                    IId?                       AuthId            = null,
                    Object?                    SendPOIData       = null,
                    EventTracking_Id?          EventTrackingId   = null,
                    I18NString?                Description       = null,
                    IEnumerable<Warning>?      Warnings          = null,
                    TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingPools.Select(energyMeter => AddOrUpdateEnergyMeterResult.Updated(energyMeter,
                                                                                                           EventTrackingId,
                                                                                                           AuthId,
                                                                                                           SendPOIData)),
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        AuthId,
                        SendPOIData,
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
                          IId?                       AuthId            = null,
                          Object?                    SendPOIData       = null,
                          IEnumerable<Warning>?      Warnings          = null,
                          TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.ArgumentError,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.ArgumentError(energyMeter,
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

        public static AddOrUpdateEnergyMetersResult

            Error(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                  I18NString                 Description,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       AuthId            = null,
                  Object?                    SendPOIData       = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.Error(energyMeter,
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

        public static AddOrUpdateEnergyMetersResult

            Error(IEnumerable<IEnergyMeter>  RejectedEnergyMeters,
                  Exception                  Exception,
                  EventTracking_Id?          EventTrackingId   = null,
                  IId?                       AuthId            = null,
                  Object?                    SendPOIData       = null,
                  IEnumerable<Warning>?      Warnings          = null,
                  TimeSpan?                  Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.Error(energyMeter,
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

        public static AddOrUpdateEnergyMetersResult

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

            return new (PushDataResultTypes.LockTimeout,
                        Array.Empty<AddOrUpdateEnergyMeterResult>(),
                        RejectedEnergyMeters.Select(energyMeter => AddOrUpdateEnergyMeterResult.LockTimeout(energyMeter,
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
