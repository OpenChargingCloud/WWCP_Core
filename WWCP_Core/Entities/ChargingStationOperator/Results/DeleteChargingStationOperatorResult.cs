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
            => Object;

        public IRoamingNetwork?           RoamingNetwork    { get; internal set; }

        #endregion

        #region Constructor(s)

        public DeleteChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                   PushDataResultTypes       Result,
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


        public DeleteChargingStationOperatorResult(ChargingStationOperator_Id  ChargingStationOperatorId,
                                                   PushDataResultTypes         Result,
                                                   EventTracking_Id?           EventTrackingId   = null,
                                                   IId?                        AuthId            = null,
                                                   Object?                     SendPOIData       = null,
                                                   IRoamingNetwork?            RoamingNetwork    = null,
                                                   I18NString?                 Description       = null,
                                                   IEnumerable<Warning>?       Warnings          = null,
                                                   TimeSpan?                   Runtime           = null)

            : base(ChargingStationOperatorId,
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


        #region (static) AdminDown      (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            AdminDown(IChargingStationOperator  ChargingStationOperator,
                      EventTracking_Id?         EventTrackingId   = null,
                      IId?                      AuthId            = null,
                      Object?                   SendPOIData       = null,
                      IRoamingNetwork?          RoamingNetwork    = null,
                      I18NString?               Description       = null,
                      IEnumerable<Warning>?     Warnings          = null,
                      TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.AdminDown,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation    (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            NoOperation(IChargingStationOperator  ChargingStationOperator,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      AuthId            = null,
                        Object?                   SendPOIData       = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        I18NString?               Description       = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued       (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            Enqueued(IChargingStationOperator  ChargingStationOperator,
                     EventTracking_Id?         EventTrackingId   = null,
                     IId?                      AuthId            = null,
                     Object?                   SendPOIData       = null,
                     IRoamingNetwork?          RoamingNetwork    = null,
                     I18NString?               Description       = null,
                     IEnumerable<Warning>?     Warnings          = null,
                     TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success        (ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            Success(IChargingStationOperator  ChargingStationOperator,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      AuthId            = null,
                    Object?                   SendPOIData       = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    I18NString?               Description       = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) CanNotBeRemoved(ChargingStationOperator, ...)

        public static DeleteChargingStationOperatorResult

            CanNotBeRemoved(IChargingStationOperator  ChargingStationOperator,
                            EventTracking_Id?         EventTrackingId   = null,
                            IId?                      AuthId            = null,
                            Object?                   SendPOIData       = null,
                            IRoamingNetwork?          RoamingNetwork    = null,
                            I18NString?               Description       = null,
                            IEnumerable<Warning>?     Warnings          = null,
                            TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.CanNotBeRemoved,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
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
                          IId?                      AuthId            = null,
                          Object?                   SendPOIData       = null,
                          IRoamingNetwork?          RoamingNetwork    = null,
                          IEnumerable<Warning>?     Warnings          = null,
                          TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
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
                          IId?                        AuthId            = null,
                          Object?                     SendPOIData       = null,
                          IRoamingNetwork?            RoamingNetwork    = null,
                          IEnumerable<Warning>?       Warnings          = null,
                          TimeSpan?                   Runtime           = null)

                => new (ChargingStationOperatorId,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
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
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
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
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout    (ChargingStationOperator,   Timeout,     ...)

        public static DeleteChargingStationOperatorResult

            LockTimeout(IChargingStationOperator  ChargingStationOperator,
                        TimeSpan                  Timeout,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      AuthId            = null,
                        Object?                   SendPOIData       = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.LockTimeout,
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
