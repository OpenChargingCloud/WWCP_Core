/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        public AddChargingStationOperatorsResult(PushDataResultTypes                             Result,
                                                 IEnumerable<AddChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                 IEnumerable<AddChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
                                                 IId?                                            AuthId                               = null,
                                                 Object?                                         SendPOIData                          = null,
                                                 EventTracking_Id?                               EventTrackingId                      = null,
                                                 I18NString?                                     Description                          = null,
                                                 IEnumerable<Warning>?                           Warnings                             = null,
                                                 TimeSpan?                                       Runtime                              = null)

            : base(Result,
                   SuccessfulChargingStationOperators,
                   RejectedChargingStationOperators,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedChargingStationOperators,   ...)

        public static AddChargingStationOperatorsResult

            AdminDown(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                      IId?                                   AuthId            = null,
                      Object?                                SendPOIData       = null,
                      EventTracking_Id?                      EventTrackingId   = null,
                      I18NString?                            Description       = null,
                      IEnumerable<Warning>?                  Warnings          = null,
                      TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.AdminDown,
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.AdminDown(chargingStationOperator,
                                                                                                                                      EventTrackingId,
                                                                                                                                      AuthId,
                                                                                                                                      SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) NoOperation  (RejectedChargingStationOperators,   ...)

        public static AddChargingStationOperatorsResult

            NoOperation(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                        IId?                                   AuthId            = null,
                        Object?                                SendPOIData       = null,
                        EventTracking_Id?                      EventTrackingId   = null,
                        I18NString?                            Description       = null,
                        IEnumerable<Warning>?                  Warnings          = null,
                        TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.NoOperation,
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.NoOperation(chargingStationOperator,
                                                                                                                                        EventTrackingId,
                                                                                                                                        AuthId,
                                                                                                                                        SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) Enqueued     (SuccessfulChargingPools, ...)

        public static AddChargingStationOperatorsResult

            Enqueued(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                     IId?                                   AuthId            = null,
                     Object?                                SendPOIData       = null,
                     EventTracking_Id?                      EventTrackingId   = null,
                     I18NString?                            Description       = null,
                     IEnumerable<Warning>?                  Warnings          = null,
                     TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Enqueued,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                            EventTrackingId,
                                                                                                                            AuthId,
                                                                                                                            SendPOIData)),
                        Array.Empty<AddChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static AddChargingStationOperatorsResult

            Success(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                    IId?                                   AuthId            = null,
                    Object?                                SendPOIData       = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddChargingStationOperatorResult.Success(chargingStationOperator,
                                                                                                                           EventTrackingId,
                                                                                                                           AuthId,
                                                                                                                           SendPOIData)),
                        Array.Empty<AddChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
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
                          IId?                                   AuthId            = null,
                          Object?                                SendPOIData       = null,
                          IEnumerable<Warning>?                  Warnings          = null,
                          TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.ArgumentError,
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.ArgumentError(chargingStationOperator,
                                                                                                                                          Description,
                                                                                                                                          EventTrackingId,
                                                                                                                                          AuthId,
                                                                                                                                          SendPOIData)),
                        AuthId,
                        SendPOIData,
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
                  IId?                                   AuthId            = null,
                  Object?                                SendPOIData       = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.Error(chargingStationOperator,
                                                                                                                                  Description,
                                                                                                                                  EventTrackingId,
                                                                                                                                  AuthId,
                                                                                                                                  SendPOIData)),
                        AuthId,
                        SendPOIData,
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
                  IId?                                   AuthId            = null,
                  Object?                                SendPOIData       = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.Error(chargingStationOperator,
                                                                                                                                  Exception,
                                                                                                                                  EventTrackingId,
                                                                                                                                  AuthId,
                                                                                                                                  SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Exception.Message.ToI18NString(),
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) LockTimeout  (RejectedChargingStationOperators, Timeout,     ...)

        public static AddChargingStationOperatorsResult

            LockTimeout(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                        TimeSpan                               Timeout,
                        IId?                                   AuthId            = null,
                        Object?                                SendPOIData       = null,
                        EventTracking_Id?                      EventTrackingId   = null,
                        I18NString?                            Description       = null,
                        IEnumerable<Warning>?                  Warnings          = null,
                        TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.LockTimeout,
                        Array.Empty<AddChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddChargingStationOperatorResult.LockTimeout(chargingStationOperator,
                                                                                                                                        Timeout,
                                                                                                                                        EventTrackingId,
                                                                                                                                        AuthId,
                                                                                                                                        SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


    }

}
