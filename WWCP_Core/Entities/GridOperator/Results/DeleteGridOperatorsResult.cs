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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of a delete grid operators request.
    /// </summary>
    public class DeleteGridOperatorsResult : AEnititiesResult<DeleteGridOperatorResult,
                                                                         IGridOperator,
                                                                         GridOperator_Id>
    {

        #region Constructor(s)

        public DeleteGridOperatorsResult(CommandResult                                      Result,
                                                    IEnumerable<DeleteGridOperatorResult>?  SuccessfulGridOperators   = null,
                                                    IEnumerable<DeleteGridOperatorResult>?  RejectedGridOperators     = null,
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

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.AdminDown(chargingStationOperator,
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

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.NoOperation(chargingStationOperator,
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

        public static DeleteGridOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => DeleteGridOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                               EventTrackingId,
                                                                                                                               SenderId,
                                                                                                                               Sender)),
                        Array.Empty<DeleteGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static DeleteGridOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => DeleteGridOperatorResult.Success(chargingStationOperator,
                                                                                                                              EventTrackingId,
                                                                                                                              SenderId,
                                                                                                                              Sender)),
                        Array.Empty<DeleteGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedGridOperators, Description, ...)

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.ArgumentError(chargingStationOperator,
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

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.Error(chargingStationOperator,
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

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.Error(chargingStationOperator,
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

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.Timeout(chargingStationOperator,
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

        public static DeleteGridOperatorsResult

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
                        Array.Empty<DeleteGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => DeleteGridOperatorResult.LockTimeout(chargingStationOperator,
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
