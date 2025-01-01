/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of an add grid operators request.
    /// </summary>
    public class AddGridOperatorsResult : AEnititiesResult<AddGridOperatorResult,
                                                                      IGridOperator,
                                                                      GridOperator_Id>
    {

        #region Constructor(s)

        public AddGridOperatorsResult(CommandResult                        Result,
                                      IEnumerable<AddGridOperatorResult>?  SuccessfulGridOperators   = null,
                                      IEnumerable<AddGridOperatorResult>?  RejectedGridOperators     = null,
                                      IId?                                 SenderId                  = null,
                                      Object?                              Sender                    = null,
                                      EventTracking_Id?                    EventTrackingId           = null,
                                      I18NString?                          Description               = null,
                                      IEnumerable<Warning>?                Warnings                  = null,
                                      TimeSpan?                            Runtime                   = null)

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

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.AdminDown(chargingStationOperator,
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

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.NoOperation(chargingStationOperator,
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

        public static AddGridOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddGridOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                            EventTrackingId,
                                                                                                                            SenderId,
                                                                                                                            Sender)),
                        Array.Empty<AddGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static AddGridOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddGridOperatorResult.Success(chargingStationOperator,
                                                                                                                           EventTrackingId,
                                                                                                                           SenderId,
                                                                                                                           Sender)),
                        Array.Empty<AddGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedGridOperators, Description, ...)

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.ArgumentError(chargingStationOperator,
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

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.Error(chargingStationOperator,
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

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.Error(chargingStationOperator,
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

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.Timeout(chargingStationOperator,
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

        public static AddGridOperatorsResult

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
                        Array.Empty<AddGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddGridOperatorResult.LockTimeout(chargingStationOperator,
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
