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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of an add or update grid operators request.
    /// </summary>
    public class AddOrUpdateGridOperatorsResult : AEnititiesResult<AddOrUpdateGridOperatorResult,
                                                                              IGridOperator,
                                                                              GridOperator_Id>
    {

        #region Constructor(s)

        public AddOrUpdateGridOperatorsResult(CommandResult                                           Result,
                                                         IEnumerable<AddOrUpdateGridOperatorResult>?  SuccessfulGridOperators   = null,
                                                         IEnumerable<AddOrUpdateGridOperatorResult>?  RejectedGridOperators     = null,
                                                         IId?                                                    SenderId                             = null,
                                                         Object?                                                 Sender                               = null,
                                                         EventTracking_Id?                                       EventTrackingId                      = null,
                                                         I18NString?                                             Description                          = null,
                                                         IEnumerable<Warning>?                                   Warnings                             = null,
                                                         TimeSpan?                                               Runtime                              = null)

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

        public static AddOrUpdateGridOperatorsResult

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
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.AdminDown(chargingStationOperator,
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

        public static AddOrUpdateGridOperatorsResult

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
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.NoOperation(chargingStationOperator,
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

        public static AddOrUpdateGridOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                                    EventTrackingId,
                                                                                                                                    SenderId,
                                                                                                                                    Sender)),
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Added        (SuccessfulChargingPools, ...)

        public static AddOrUpdateGridOperatorsResult

            Added(IEnumerable<IGridOperator>  SuccessfulChargingPools,
                  IId?                                   SenderId          = null,
                  Object?                                Sender            = null,
                  EventTracking_Id?                      EventTrackingId   = null,
                  I18NString?                            Description       = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.Added(chargingStationOperator,
                                                                                                                                 EventTrackingId,
                                                                                                                                 SenderId,
                                                                                                                                 Sender)),
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Updated      (SuccessfulChargingPools, ...)

        public static AddOrUpdateGridOperatorsResult

            Updated(IEnumerable<IGridOperator>  SuccessfulChargingPools,
                    IId?                                   SenderId          = null,
                    Object?                                Sender            = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.Updated(chargingStationOperator,
                                                                                                                                   EventTrackingId,
                                                                                                                                   SenderId,
                                                                                                                                   Sender)),
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedGridOperators, Description, ...)

        public static AddOrUpdateGridOperatorsResult

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
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.ArgumentError(chargingStationOperator,
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

        public static AddOrUpdateGridOperatorsResult

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
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.Error(chargingStationOperator,
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

        public static AddOrUpdateGridOperatorsResult

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
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.Error(chargingStationOperator,
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

        public static AddOrUpdateGridOperatorsResult

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
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.Timeout(chargingStationOperator,
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

        public static AddOrUpdateGridOperatorsResult

            LockTimeout(IEnumerable<IGridOperator>  RejectedGridOperators,
                        TimeSpan                    Timeout,
                        IId?                        SenderId          = null,
                        Object?                     Sender            = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        I18NString?                 Description       = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<AddOrUpdateGridOperatorResult>(),
                        RejectedGridOperators.Select(chargingStationOperator => AddOrUpdateGridOperatorResult.LockTimeout(chargingStationOperator,
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
