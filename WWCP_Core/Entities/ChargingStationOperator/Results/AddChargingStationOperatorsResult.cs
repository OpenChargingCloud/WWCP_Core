/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// The result of an add charging station operators request.
    /// </summary>
    public class AddChargingStationOperatorsResult : AEnititiesResult<AddChargingStationOperatorResult,
                                                                      IChargingStationOperator,
                                                                      ChargingStationOperator_Id>
    {

        #region Constructor(s)

        public AddChargingStationOperatorsResult(CommandResult                                   Result,
                                                 IEnumerable<AddChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                 IEnumerable<AddChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
                                                 IId?                                            SenderId                             = null,
                                                 Object?                                         Sender                               = null,
                                                 EventTracking_Id?                               EventTrackingId                      = null,
                                                 I18NString?                                     Description                          = null,
                                                 IEnumerable<Warning>?                           Warnings                             = null,
                                                 TimeSpan?                                       Runtime                              = null)

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

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.AdminDown(chargingStationOperator,
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

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.NoOperation(chargingStationOperator,
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

        public static AddChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                            EventTrackingId,
                                                                                                                            SenderId,
                                                                                                                            Sender)),
                        Array.Empty<AddChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static AddChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddChargingStationOperatorResult.Success(chargingStationOperator,
                                                                                                                           EventTrackingId,
                                                                                                                           SenderId,
                                                                                                                           Sender)),
                        Array.Empty<AddChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStationOperators, Description, ...)

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.ArgumentError(chargingStationOperator,
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

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.Timeout(chargingStationOperator,
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

        public static AddChargingStationOperatorsResult

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
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.LockTimeout(chargingStationOperator,
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
