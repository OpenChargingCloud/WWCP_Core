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
    /// The result of a replace charging station operators request.
    /// </summary>
    public class ReplaceChargingStationOperatorsResult : AEnititiesResult<AddOrUpdateChargingStationOperatorResult,
                                                                          IChargingStationOperator,
                                                                          ChargingStationOperator_Id>
    {

        #region Constructor(s)

        public ReplaceChargingStationOperatorsResult(CommandResult                                           Result,
                                                     IEnumerable<AddOrUpdateChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                     IEnumerable<AddOrUpdateChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
                                                     IId?                                                    SenderId                             = null,
                                                     Object?                                                 Sender                               = null,
                                                     EventTracking_Id?                                       EventTrackingId                      = null,
                                                     I18NString?                                             Description                          = null,
                                                     IEnumerable<Warning>?                                   Warnings                             = null,
                                                     TimeSpan?                                               Runtime                              = null)

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

        public static ReplaceChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.AdminDown(chargingStationOperator,
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

        public static ReplaceChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.NoOperation(chargingStationOperator,
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

        public static ReplaceChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                                    EventTrackingId,
                                                                                                                                    SenderId,
                                                                                                                                    Sender)),
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Added        (SuccessfulChargingPools, ...)

        public static ReplaceChargingStationOperatorsResult

            Added(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                  IId?                                   SenderId          = null,
                  Object?                                Sender            = null,
                  EventTracking_Id?                      EventTrackingId   = null,
                  I18NString?                            Description       = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Added(chargingStationOperator,
                                                                                                                                 EventTrackingId,
                                                                                                                                 SenderId,
                                                                                                                                 Sender)),
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Updated      (SuccessfulChargingPools, ...)

        public static ReplaceChargingStationOperatorsResult

            Updated(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                    IId?                                   SenderId          = null,
                    Object?                                Sender            = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Updated(chargingStationOperator,
                                                                                                                                   EventTrackingId,
                                                                                                                                   SenderId,
                                                                                                                                   Sender)),
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStationOperators, Description, ...)

        public static ReplaceChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.ArgumentError(chargingStationOperator,
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

        public static ReplaceChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static ReplaceChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static ReplaceChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Timeout(chargingStationOperator,
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

        public static ReplaceChargingStationOperatorsResult

            LockTimeout(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.LockTimeout(chargingStationOperator,
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
