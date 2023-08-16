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
    /// The result of an add or update charging station operators request.
    /// </summary>
    public class AddOrUpdateChargingStationOperatorsResult : AEnititiesResult<AddOrUpdateChargingStationOperatorResult,
                                                                              IChargingStationOperator,
                                                                              ChargingStationOperator_Id>
    {

        #region Constructor(s)

        public AddOrUpdateChargingStationOperatorsResult(PushDataResultTypes                                     Result,
                                                         IEnumerable<AddOrUpdateChargingStationOperatorResult>?  SuccessfulChargingStationOperators   = null,
                                                         IEnumerable<AddOrUpdateChargingStationOperatorResult>?  RejectedChargingStationOperators     = null,
                                                         IId?                                                    AuthId                               = null,
                                                         Object?                                                 SendPOIData                          = null,
                                                         EventTracking_Id?                                       EventTrackingId                      = null,
                                                         I18NString?                                             Description                          = null,
                                                         IEnumerable<Warning>?                                   Warnings                             = null,
                                                         TimeSpan?                                               Runtime                              = null)

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

        public static AddOrUpdateChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.AdminDown(chargingStationOperator,
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

        public static AddOrUpdateChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.NoOperation(chargingStationOperator,
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

        public static AddOrUpdateChargingStationOperatorsResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Enqueued(chargingStationOperator,
                                                                                                                                    EventTrackingId,
                                                                                                                                    AuthId,
                                                                                                                                    SendPOIData)),
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Added        (SuccessfulChargingPools, ...)

        public static AddOrUpdateChargingStationOperatorsResult

            Added(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                  IId?                                   AuthId            = null,
                  Object?                                SendPOIData       = null,
                  EventTracking_Id?                      EventTrackingId   = null,
                  I18NString?                            Description       = null,
                  IEnumerable<Warning>?                  Warnings          = null,
                  TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Added(chargingStationOperator,
                                                                                                                                 EventTrackingId,
                                                                                                                                 AuthId,
                                                                                                                                 SendPOIData)),
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Updated      (SuccessfulChargingPools, ...)

        public static AddOrUpdateChargingStationOperatorsResult

            Updated(IEnumerable<IChargingStationOperator>  SuccessfulChargingPools,
                    IId?                                   AuthId            = null,
                    Object?                                SendPOIData       = null,
                    EventTracking_Id?                      EventTrackingId   = null,
                    I18NString?                            Description       = null,
                    IEnumerable<Warning>?                  Warnings          = null,
                    TimeSpan?                              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Updated(chargingStationOperator,
                                                                                                                                   EventTrackingId,
                                                                                                                                   AuthId,
                                                                                                                                   SendPOIData)),
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStationOperators, Description, ...)

        public static AddOrUpdateChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.ArgumentError(chargingStationOperator,
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

        public static AddOrUpdateChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static AddOrUpdateChargingStationOperatorsResult

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
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.Error(chargingStationOperator,
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

        public static AddOrUpdateChargingStationOperatorsResult

            LockTimeout(IEnumerable<IChargingStationOperator>  RejectedChargingStationOperators,
                        TimeSpan                    Timeout,
                        IId?                        AuthId            = null,
                        Object?                     SendPOIData       = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        I18NString?                 Description       = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.LockTimeout,
                        Array.Empty<AddOrUpdateChargingStationOperatorResult>(),
                        RejectedChargingStationOperators.Select(chargingStationOperator => AddOrUpdateChargingStationOperatorResult.LockTimeout(chargingStationOperator,
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
