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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of an add or update grid operator request.
    /// </summary>
    public class AddOrUpdateGridOperatorResult : AEnitityResult<IGridOperator, GridOperator_Id>
    {

        #region Properties

        public IGridOperator?  GridOperator
            => Entity;

        public IRoamingNetwork?           RoamingNetwork    { get; internal set; }

        public AddedOrUpdated?            AddedOrUpdated    { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateGridOperatorResult(IGridOperator  GridOperator,
                                                        CommandResult       Result,
                                                        EventTracking_Id?         EventTrackingId   = null,
                                                        IId?                      SenderId          = null,
                                                        Object?                   Sender            = null,
                                                        IRoamingNetwork?          RoamingNetwork    = null,
                                                        AddedOrUpdated?           AddedOrUpdated    = null,
                                                        I18NString?               Description       = null,
                                                        IEnumerable<Warning>?     Warnings          = null,
                                                        TimeSpan?                 Runtime           = null)

            : base(GridOperator,
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


        #region (static) AdminDown    (GridOperator, ...)

        public static AddOrUpdateGridOperatorResult

            AdminDown(IGridOperator  GridOperator,
                      EventTracking_Id?         EventTrackingId   = null,
                      IId?                      SenderId          = null,
                      Object?                   Sender            = null,
                      IRoamingNetwork?          RoamingNetwork    = null,
                      I18NString?               Description       = null,
                      IEnumerable<Warning>?     Warnings          = null,
                      TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) NoOperation  (GridOperator, ...)

        public static AddOrUpdateGridOperatorResult

            NoOperation(IGridOperator  GridOperator,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      SenderId          = null,
                        Object?                   Sender            = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        I18NString?               Description       = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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


        #region (static) Enqueued     (GridOperator, ...)

        public static AddOrUpdateGridOperatorResult

            Enqueued(IGridOperator  GridOperator,
                     EventTracking_Id?         EventTrackingId   = null,
                     IId?                      SenderId          = null,
                     Object?                   Sender            = null,
                     IRoamingNetwork?          RoamingNetwork    = null,
                     I18NString?               Description       = null,
                     IEnumerable<Warning>?     Warnings          = null,
                     TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) Added        (GridOperator,...)

        public static AddOrUpdateGridOperatorResult

            Added(IGridOperator  GridOperator,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      SenderId          = null,
                  Object?                   Sender            = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  I18NString?               Description       = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) Updated      (GridOperator,...)

        public static AddOrUpdateGridOperatorResult

            Updated(IGridOperator  GridOperator,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      SenderId          = null,
                    Object?                   Sender            = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    I18NString?               Description       = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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


        #region (static) ArgumentError(GridOperator, Description, ...)

        public static AddOrUpdateGridOperatorResult

            ArgumentError(IGridOperator  GridOperator,
                          I18NString                Description,
                          EventTracking_Id?         EventTrackingId   = null,
                          IId?                      SenderId          = null,
                          Object?                   Sender            = null,
                          IRoamingNetwork?          RoamingNetwork    = null,
                          IEnumerable<Warning>?     Warnings          = null,
                          TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) Error        (GridOperator, Description, ...)

        public static AddOrUpdateGridOperatorResult

            Error(IGridOperator  GridOperator,
                  I18NString                Description,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      SenderId          = null,
                  Object?                   Sender            = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) Error        (GridOperator, Exception,   ...)

        public static AddOrUpdateGridOperatorResult

            Error(IGridOperator  GridOperator,
                  Exception                 Exception,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      SenderId          = null,
                  Object?                   Sender            = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) Timeout      (GridOperator, Timeout,     ...)

        public static AddOrUpdateGridOperatorResult

            Timeout(IGridOperator  GridOperator,
                    TimeSpan                  Timeout,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      SenderId          = null,
                    Object?                   Sender            = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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

        #region (static) LockTimeout  (GridOperator, Timeout,     ...)

        public static AddOrUpdateGridOperatorResult

            LockTimeout(IGridOperator  GridOperator,
                        TimeSpan                  Timeout,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      SenderId          = null,
                        Object?                   Sender            = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (GridOperator,
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
