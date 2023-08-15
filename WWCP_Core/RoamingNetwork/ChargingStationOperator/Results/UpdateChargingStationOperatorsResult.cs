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
    /// The result of an update charging station operators request.
    /// </summary>
    public class UpdateChargingStationOperatorsResult : AEnititiesResult<UpdateChargingStationOperatorResult,
                                                                         IChargingStationOperator,
                                                                         ChargingStationOperator_Id>
    {

        #region Constructor(s)

        public UpdateChargingStationOperatorsResult(PushDataResultTypes                                Result,
                                                    IEnumerable<UpdateChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                    IEnumerable<UpdateChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
                                                    IId?                                               AuthId                               = null,
                                                    Object?                                            SendPOIData                          = null,
                                                    EventTracking_Id?                                  EventTrackingId                      = null,
                                                    I18NString?                                        Description                          = null,
                                                    IEnumerable<Warning>?                              Warnings                             = null,
                                                    TimeSpan?                                          Runtime                              = null)

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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.AdminDown(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.NoOperation(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                               EventTrackingId,
                                                                                                                               AuthId,
                                                                                                                               SendPOIData)),
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static UpdateChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Success(chargingStationOperator,
                                                                                                                              EventTrackingId,
                                                                                                                              AuthId,
                                                                                                                              SendPOIData)),
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
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
                          IId?                                   AuthId            = null,
                          Object?                                SendPOIData       = null,
                          IEnumerable<Warning>?                  Warnings          = null,
                          TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.ArgumentError,
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.ArgumentError(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static UpdateChargingStationOperatorsResult

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
                        Array.Empty<UpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => UpdateChargingStationOperatorResult.LockTimeout(chargingStationOperator,
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
