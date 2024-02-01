/*
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
    /// The result of an add or update charging station operator request.
    /// </summary>
    public class AddOrUpdateChargingStationOperatorResult : AEnitityResult<IChargingStationOperator, ChargingStationOperator_Id>
    {

        #region Properties

        public IChargingStationOperator?  ChargingStationOperator
            => Entity;

        public IRoamingNetwork?           RoamingNetwork    { get; internal set; }

        public AddedOrUpdated?            AddedOrUpdated    { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                        CommandResult       Result,
                                                        EventTracking_Id?         EventTrackingId   = null,
                                                        IId?                      SenderId          = null,
                                                        Object?                   Sender            = null,
                                                        IRoamingNetwork?          RoamingNetwork    = null,
                                                        AddedOrUpdated?           AddedOrUpdated    = null,
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

            this.RoamingNetwork  = RoamingNetwork;
            this.AddedOrUpdated  = AddedOrUpdated;

        }

        #endregion


        #region (static) AdminDown    (ChargingStationOperator, ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) NoOperation  (ChargingStationOperator, ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Enqueued     (ChargingStationOperator, ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Enqueued,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Added        (ChargingStationOperator,...)

        public static AddOrUpdateChargingStationOperatorResult

            Added(IChargingStationOperator  ChargingStationOperator,
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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated      (ChargingStationOperator,...)

        public static AddOrUpdateChargingStationOperatorResult

            Updated(IChargingStationOperator  ChargingStationOperator,
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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) ArgumentError(ChargingStationOperator, Description, ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStationOperator, Description, ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error        (ChargingStationOperator, Exception,   ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Timeout      (ChargingStationOperator, Timeout,     ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        $"Timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout  (ChargingStationOperator, Timeout,     ...)

        public static AddOrUpdateChargingStationOperatorResult

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
                        org.GraphDefined.Vanaheimr.Illias.AddedOrUpdated.Failed,
                        $"Lock timeout after {Timeout.TotalSeconds} seconds!".ToI18NString(),
                        Warnings,
                        Runtime);

        #endregion


    }

}
