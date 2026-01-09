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
    /// The result of a delete charging station operators request.
    /// </summary>
    public class DeleteChargingStationOperatorsResult : AEnititiesResult<DeleteChargingStationOperatorResult,
                                                                         IChargingStationOperator,
                                                                         ChargingStationOperator_Id>
    {

        #region Constructor(s)

        public DeleteChargingStationOperatorsResult(CommandResult                                      Result,
                                                    IEnumerable<DeleteChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                    IEnumerable<DeleteChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
                                                    IId?                                               SenderId                             = null,
                                                    Object?                                            Sender                               = null,
                                                    EventTracking_Id?                                  EventTrackingId                      = null,
                                                    I18NString?                                        Description                          = null,
                                                    IEnumerable<Warning>?                              Warnings                             = null,
                                                    TimeSpan?                                          Runtime                              = null)

            : base(Result,
                   SuccessfulChargingStationOperators,
                   RejectedChargingStationOperators,
                   SenderId,
                   Sender,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedChargingStationOperators,   ...)

        public static DeleteChargingStationOperatorsResult

            AdminDown(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                      IId?                                   SenderId          = null,
                      Object?                                Sender            = null,
                      EventTracking_Id?                      EventTrackingId   = null,
                      I18NString?                            Description       = null,
                      IEnumerable<Warning>?                  Warnings          = null,
                      TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.AdminDown(chargingStationOperator,
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

        #region (static) NoOperation  (RejectedChargingStationOperators,   ...)

        public static DeleteChargingStationOperatorsResult

            NoOperation(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                        IId?                                   SenderId          = null,
                        Object?                                Sender            = null,
                        EventTracking_Id?                      EventTrackingId   = null,
                        I18NString?                            Description       = null,
                        IEnumerable<Warning>?                  Warnings          = null,
                        TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.NoOperation(chargingStationOperator,
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

        public static DeleteChargingStationOperatorsResult

            Enqueued(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                     IId?                                   SenderId          = null,
                     Object?                                Sender            = null,
                     EventTracking_Id?                      EventTrackingId   = null,
                     I18NString?                            Description       = null,
                     IEnumerable<Warning>?                  Warnings          = null,
                     TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(chargingStationOperator => DeleteChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                               EventTrackingId,
                                                                                                                               SenderId,
                                                                                                                               Sender)),
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static DeleteChargingStationOperatorsResult

            Success(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                    IId?                                   SenderId          = null,
                    Object?                                Sender            = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => DeleteChargingStationOperatorResult.Success(chargingStationOperator,
                                                                                                                              EventTrackingId,
                                                                                                                              SenderId,
                                                                                                                              Sender)),
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStationOperators, Description, ...)

        public static DeleteChargingStationOperatorsResult

            ArgumentError(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                          I18NString                             Description,
                          EventTracking_Id?                      EventTrackingId   = null,
                          IId?                                   SenderId          = null,
                          Object?                                Sender            = null,
                          IEnumerable<Warning>?                  Warnings          = null,
                          TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.ArgumentError(chargingStationOperator,
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

        #region (static) Error        (RejectedChargingStationOperators, Description, ...)

        public static DeleteChargingStationOperatorsResult

            Error(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                  I18NString                             Description,
                  EventTracking_Id?                      EventTrackingId   = null,
                  IId?                                   SenderId          = null,
                  Object?                                Sender            = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.Error(chargingStationOperator,
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

        #region (static) Error        (RejectedChargingStationOperators, Exception,   ...)

        public static DeleteChargingStationOperatorsResult

            Error(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                  Exception                              Exception,
                  EventTracking_Id?                      EventTrackingId   = null,
                  IId?                                   SenderId          = null,
                  Object?                                Sender            = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.Error(chargingStationOperator,
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

        #region (static) Timeout      (RejectedChargingStationOperators, Timeout,     ...)

        public static DeleteChargingStationOperatorsResult

            Timeout(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
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
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.Timeout(chargingStationOperator,
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

        #region (static) LockTimeout  (RejectedChargingStationOperators, Timeout,     ...)

        public static DeleteChargingStationOperatorsResult

            LockTimeout(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
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
                        Array.Empty<DeleteChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => DeleteChargingStationOperatorResult.LockTimeout(chargingStationOperator,
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
