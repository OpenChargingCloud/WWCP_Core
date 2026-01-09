/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of an update grid operators request.
    /// </summary>
    public class UpdateGridOperatorsResult : AEnititiesResult<UpdateGridOperatorResult,
                                                              IGridOperator,
                                                              GridOperator_Id>
    {

        #region Constructor(s)

        public UpdateGridOperatorsResult(CommandResult                                      Result,
                                                    IEnumerable<UpdateGridOperatorResult>?  SuccessfulGridOperators   = null,
                                                    IEnumerable<UpdateGridOperatorResult>?  RejectedGridOperators     = null,
                                                    IId?                                               SenderId                             = null,
                                                    Object?                                            Sender                               = null,
                                                    EventTracking_Id?                                  EventTrackingId                      = null,
                                                    I18NString?                                        Description                          = null,
                                                    IEnumerable<Warning>?                              Warnings                             = null,
                                                    TimeSpan?                                          Runtime                              = null)

            : base(Result,
                   SuccessfulGridOperators,
                   RejectedGridOperators,
                   SenderId,
                   Sender,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedGridOperators,   ...)

        public static UpdateGridOperatorsResult

            AdminDown(IEnumerable<IGridOperator>  RejectedGridOperators,
                      IId?                                   SenderId          = null,
                      Object?                                Sender            = null,
                      EventTracking_Id?                      EventTrackingId   = null,
                      I18NString?                            Description       = null,
                      IEnumerable<Warning>?                  Warnings          = null,
                      TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.AdminDown(chargingStationOperator,
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

        #region (static) NoOperation  (RejectedGridOperators,   ...)

        public static UpdateGridOperatorsResult

            NoOperation(IEnumerable<IGridOperator>  RejectedGridOperators,
                        IId?                                   SenderId          = null,
                        Object?                                Sender            = null,
                        EventTracking_Id?                      EventTrackingId   = null,
                        I18NString?                            Description       = null,
                        IEnumerable<Warning>?                  Warnings          = null,
                        TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.NoOperation(chargingStationOperator,
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

        public static UpdateGridOperatorsResult

            Enqueued(IEnumerable<IGridOperator>  SuccessfulChargingPools,
                     IId?                                   SenderId          = null,
                     Object?                                Sender            = null,
                     EventTracking_Id?                      EventTrackingId   = null,
                     I18NString?                            Description       = null,
                     IEnumerable<Warning>?                  Warnings          = null,
                     TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateGridOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                               EventTrackingId,
                                                                                                                               SenderId,
                                                                                                                               Sender)),
                        Array.Empty<UpdateGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static UpdateGridOperatorsResult

            Success(IEnumerable<IGridOperator>  SuccessfulChargingPools,
                    IId?                                   SenderId          = null,
                    Object?                                Sender            = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateGridOperatorResult.Success(chargingStationOperator,
                                                                                                                              EventTrackingId,
                                                                                                                              SenderId,
                                                                                                                              Sender)),
                        Array.Empty<UpdateGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedGridOperators, Description, ...)

        public static UpdateGridOperatorsResult

            ArgumentError(IEnumerable<IGridOperator>  RejectedGridOperators,
                          I18NString                             Description,
                          EventTracking_Id?                      EventTrackingId   = null,
                          IId?                                   SenderId          = null,
                          Object?                                Sender            = null,
                          IEnumerable<Warning>?                  Warnings          = null,
                          TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.ArgumentError(chargingStationOperator,
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

        #region (static) Error        (RejectedGridOperators, Description, ...)

        public static UpdateGridOperatorsResult

            Error(IEnumerable<IGridOperator>  RejectedGridOperators,
                  I18NString                             Description,
                  EventTracking_Id?                      EventTrackingId   = null,
                  IId?                                   SenderId          = null,
                  Object?                                Sender            = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.Error(chargingStationOperator,
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

        #region (static) Error        (RejectedGridOperators, Exception,   ...)

        public static UpdateGridOperatorsResult

            Error(IEnumerable<IGridOperator>  RejectedGridOperators,
                  Exception                              Exception,
                  EventTracking_Id?                      EventTrackingId   = null,
                  IId?                                   SenderId          = null,
                  Object?                                Sender            = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.Error(chargingStationOperator,
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

        #region (static) Timeout      (RejectedGridOperators, Timeout,     ...)

        public static UpdateGridOperatorsResult

            Timeout(IEnumerable<IGridOperator>  RejectedGridOperators,
                    TimeSpan                               Timeout,
                    IId?                                   SenderId          = null,
                    Object?                                Sender            = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Timeout,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.Timeout(chargingStationOperator,
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

        #region (static) LockTimeout  (RejectedGridOperators, Timeout,     ...)

        public static UpdateGridOperatorsResult

            LockTimeout(IEnumerable<IGridOperator>  RejectedGridOperators,
                        TimeSpan                               Timeout,
                        IId?                                   SenderId          = null,
                        Object?                                Sender            = null,
                        EventTracking_Id?                      EventTrackingId   = null,
                        I18NString?                            Description       = null,
                        IEnumerable<Warning>?                  Warnings          = null,
                        TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<UpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => UpdateGridOperatorResult.LockTimeout(chargingStationOperator,
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
