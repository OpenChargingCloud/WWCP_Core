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
    /// The result of an update charging station operators request.
    /// </summary>
    public class UpdateChargingStationOperatorsResult : AEnititiesResult<UpdateChargingStationOperatorResult,
                                                                         IChargingStationOperator,
                                                                         ChargingStationOperator_Id>
    {

        #region Constructor(s)

        public UpdateChargingStationOperatorsResult(CommandResult                                      Result,
                                                    IEnumerable<UpdateChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                    IEnumerable<UpdateChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.AdminDown(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.NoOperation(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                               EventTrackingId,
                                                                                                                               SenderId,
                                                                                                                               Sender)),
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static UpdateChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Success(chargingStationOperator,
                                                                                                                              EventTrackingId,
                                                                                                                              SenderId,
                                                                                                                              Sender)),
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStationOperators, Description, ...)

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.ArgumentError(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Timeout(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.LockTimeout(chargingStationOperator,
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
