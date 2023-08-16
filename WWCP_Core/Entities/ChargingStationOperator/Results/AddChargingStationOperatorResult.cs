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
    /// The result of an add charging station operator request.
    /// </summary>
    public class AddChargingStationOperatorResult : AEnitityResult<IChargingStationOperator, ChargingStationOperator_Id>
    {

        #region Properties

        public IChargingStationOperator?  ChargingStationOperator
            => Object;

        public IRoamingNetwork?           RoamingNetwork    { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                CommandResult       Result,
                                                EventTracking_Id?         EventTrackingId   = null,
                                                IId?                      AuthId            = null,
                                                Object?                   SendPOIData       = null,
                                                IRoamingNetwork?          RoamingNetwork    = null,
                                                I18NString?               Description       = null,
                                                IEnumerable<Warning>?     Warnings          = null,
                                                TimeSpan?                 Runtime           = null)

            : base(ChargingStationOperator,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.RoamingNetwork = RoamingNetwork;

        }

        #endregion


        #region (static) AdminDown    (ChargingStationOperator, ...)

        public static AddChargingStationOperatorResult

            AdminDown(IChargingStationOperator  ChargingStationOperator,
                      EventTracking_Id?         EventTrackingId   = null,
                      IId?                      AuthId            = null,
                      Object?                   SendPOIData       = null,
                      IRoamingNetwork?          RoamingNetwork    = null,
                      I18NString?               Description       = null,
                      IEnumerable<Warning>?     Warnings          = null,
                      TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.AdminDown,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (ChargingStationOperator, ...)

        public static AddChargingStationOperatorResult

            NoOperation(IChargingStationOperator  ChargingStationOperator,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      AuthId            = null,
                        Object?                   SendPOIData       = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        I18NString?               Description       = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (ChargingStationOperator, ...)

        public static AddChargingStationOperatorResult

            Enqueued(IChargingStationOperator  ChargingStationOperator,
                     EventTracking_Id?         EventTrackingId   = null,
                     IId?                      AuthId            = null,
                     Object?                   SendPOIData       = null,
                     IRoamingNetwork?          RoamingNetwork    = null,
                     I18NString?               Description       = null,
                     IEnumerable<Warning>?     Warnings          = null,
                     TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success      (ChargingStationOperator, ...)

        public static AddChargingStationOperatorResult

            Success(IChargingStationOperator  ChargingStationOperator,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      AuthId            = null,
                    Object?                   SendPOIData       = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    I18NString?               Description       = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Exists(...)

        public static AddChargingStationOperatorResult

            Exists(IChargingStationOperator  ChargingStationOperator,
                   EventTracking_Id?         EventTrackingId   = null,
                   IId?                      AuthId            = null,
                   Object?                   SendPOIData       = null,
                   IRoamingNetwork?          RoamingNetwork    = null,
                   I18NString?               Description       = null,
                   IEnumerable<Warning>?     Warnings          = null,
                   TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Exists,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(ChargingStationOperator, Description, ...)

        public static AddChargingStationOperatorResult

            ArgumentError(IChargingStationOperator  ChargingStationOperator,
                          I18NString                Description,
                          EventTracking_Id?         EventTrackingId   = null,
                          IId?                      AuthId            = null,
                          Object?                   SendPOIData       = null,
                          IRoamingNetwork?          RoamingNetwork    = null,
                          IEnumerable<Warning>?     Warnings          = null,
                          TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStationOperator, Description, ...)

        public static AddChargingStationOperatorResult

            Error(IChargingStationOperator  ChargingStationOperator,
                  I18NString                Description,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStationOperator, Exception,   ...)

        public static AddChargingStationOperatorResult

            Error(IChargingStationOperator  ChargingStationOperator,
                  Exception                 Exception,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (ChargingStationOperator, Timeout,     ...)

        public static AddChargingStationOperatorResult

            LockTimeout(IChargingStationOperator  ChargingStationOperator,
                        TimeSpan                  Timeout,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      AuthId            = null,
                        Object?                   SendPOIData       = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        CommandResult.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
