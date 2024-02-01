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
    /// The results of an add or update charging pools request.
    /// </summary>
    public class AddOrUpdateChargingPoolsResult : AEnititiesResult<AddOrUpdateChargingPoolResult,
                                                                   IChargingPool,
                                                                   ChargingPool_Id>
    {

        #region Constructor(s)

        public AddOrUpdateChargingPoolsResult(CommandResult                          Result,
                                              IEnumerable<AddOrUpdateChargingPoolResult>?  SuccessfulChargingPools   = null,
                                              IEnumerable<AddOrUpdateChargingPoolResult>?  RejectedChargingPools     = null,
                                              IId?                                         SenderId                  = null,
                                              Object?                                      Sender                    = null,
                                              EventTracking_Id?                            EventTrackingId           = null,
                                              I18NString?                                  Description               = null,
                                              IEnumerable<Warning>?                        Warnings                  = null,
                                              TimeSpan?                                    Runtime                   = null)

            : base(Result,
                   SuccessfulChargingPools,
                   RejectedChargingPools,
                   SenderId,
                   Sender,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedChargingPools,   ...)

        public static AddOrUpdateChargingPoolsResult

            AdminDown(IEnumerable<IChargingPool>  RejectedChargingPools,
                      IId?                        SenderId          = null,
                      Object?                     Sender            = null,
                      EventTracking_Id?           EventTrackingId   = null,
                      I18NString?                 Description       = null,
                      IEnumerable<Warning>?       Warnings          = null,
                      TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.AdminDown(chargingPool,
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

        #region (static) NoOperation  (RejectedChargingPools,   ...)

        public static AddOrUpdateChargingPoolsResult

            NoOperation(IEnumerable<IChargingPool>  RejectedChargingPools,
                        IId?                        SenderId          = null,
                        Object?                     Sender            = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        I18NString?                 Description       = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.NoOperation(chargingPool,
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

        public static AddOrUpdateChargingPoolsResult

            Enqueued(IEnumerable<IChargingPool>  SuccessfulChargingPools,
                     IId?                        SenderId          = null,
                     Object?                     Sender            = null,
                     EventTracking_Id?           EventTrackingId   = null,
                     I18NString?                 Description       = null,
                     IEnumerable<Warning>?       Warnings          = null,
                     TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.Enqueued(chargingPool,
                                                                                                              EventTrackingId,
                                                                                                              SenderId,
                                                                                                              Sender)),
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Added        (SuccessfulChargingPools, ...)

        public static AddOrUpdateChargingPoolsResult

            Added(IEnumerable<IChargingPool>  SuccessfulChargingPools,
                  IId?                        SenderId          = null,
                  Object?                     Sender            = null,
                  EventTracking_Id?           EventTrackingId   = null,
                  I18NString?                 Description       = null,
                  IEnumerable<Warning>?       Warnings          = null,
                  TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.Added(chargingPool,
                                                                                                           EventTrackingId,
                                                                                                           SenderId,
                                                                                                           Sender)),
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Updated      (SuccessfulChargingPools, ...)

        public static AddOrUpdateChargingPoolsResult

            Updated(IEnumerable<IChargingPool>  SuccessfulChargingPools,
                    IId?                        SenderId          = null,
                    Object?                     Sender            = null,
                    EventTracking_Id?           EventTrackingId   = null,
                    I18NString?                 Description       = null,
                    IEnumerable<Warning>?       Warnings          = null,
                    TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.Updated(chargingPool,
                                                                                                             EventTrackingId,
                                                                                                             SenderId,
                                                                                                             Sender)),
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingPools, Description, ...)

        public static AddOrUpdateChargingPoolsResult

            ArgumentError(IEnumerable<IChargingPool>  RejectedChargingPools,
                          I18NString                  Description,
                          EventTracking_Id?           EventTrackingId   = null,
                          IId?                        SenderId          = null,
                          Object?                     Sender            = null,
                          IEnumerable<Warning>?       Warnings          = null,
                          TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.ArgumentError(chargingPool,
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

        #region (static) Error        (RejectedChargingPools, Description, ...)

        public static AddOrUpdateChargingPoolsResult

            Error(IEnumerable<IChargingPool>  RejectedChargingPools,
                  I18NString                  Description,
                  EventTracking_Id?           EventTrackingId   = null,
                  IId?                        SenderId          = null,
                  Object?                     Sender            = null,
                  IEnumerable<Warning>?       Warnings          = null,
                  TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.Error(chargingPool,
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

        #region (static) Error        (RejectedChargingPools, Exception,   ...)

        public static AddOrUpdateChargingPoolsResult

            Error(IEnumerable<IChargingPool>  RejectedChargingPools,
                  Exception                   Exception,
                  EventTracking_Id?           EventTrackingId   = null,
                  IId?                        SenderId          = null,
                  Object?                     Sender            = null,
                  IEnumerable<Warning>?       Warnings          = null,
                  TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.Error(chargingPool,
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

        #region (static) Timeout      (RejectedChargingPools, Timeout, ...)

        public static AddOrUpdateChargingPoolsResult

            Timeout(IEnumerable<IChargingPool>  RejectedChargingPools,
                    TimeSpan                    Timeout,
                    IId?                        SenderId          = null,
                    Object?                     Sender            = null,
                    EventTracking_Id?           EventTrackingId   = null,
                    I18NString?                 Description       = null,
                    IEnumerable<Warning>?       Warnings          = null,
                    TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Timeout,
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.Timeout(chargingPool,
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

        #region (static) LockTimeout  (RejectedChargingPools, Timeout, ...)

        public static AddOrUpdateChargingPoolsResult

            LockTimeout(IEnumerable<IChargingPool>  RejectedChargingPools,
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
                        Array.Empty<AddOrUpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddOrUpdateChargingPoolResult.LockTimeout(chargingPool,
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
