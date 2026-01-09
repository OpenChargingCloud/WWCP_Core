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
    /// The results of an add charging pools request.
    /// </summary>
    public class AddChargingPoolsResult : AEnititiesResult<AddChargingPoolResult,
                                                           IChargingPool,
                                                           ChargingPool_Id>
    {

        #region Constructor(s)

        public AddChargingPoolsResult(CommandResult                  Result,
                                      IEnumerable<AddChargingPoolResult>?  SuccessfulChargingPools   = null,
                                      IEnumerable<AddChargingPoolResult>?  RejectedChargingPools     = null,
                                      IId?                                 SenderId                  = null,
                                      Object?                              Sender                    = null,
                                      EventTracking_Id?                    EventTrackingId           = null,
                                      I18NString?                          Description               = null,
                                      IEnumerable<Warning>?                Warnings                  = null,
                                      TimeSpan?                            Runtime                   = null)

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

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.AdminDown(chargingPool,
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

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.NoOperation(chargingPool,
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

        public static AddChargingPoolsResult

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
                        SuccessfulChargingPools.Select(chargingPool => AddChargingPoolResult.Enqueued(chargingPool,
                                                                                                      EventTrackingId,
                                                                                                      SenderId,
                                                                                                      Sender)),
                        Array.Empty<AddChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static AddChargingPoolsResult

            Success(IEnumerable<IChargingPool>  SuccessfulChargingPools,
                    IId?                        SenderId          = null,
                    Object?                     Sender            = null,
                    EventTracking_Id?           EventTrackingId   = null,
                    I18NString?                 Description       = null,
                    IEnumerable<Warning>?       Warnings          = null,
                    TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingPool => AddChargingPoolResult.Success(chargingPool,
                                                                                                     EventTrackingId,
                                                                                                     SenderId,
                                                                                                     Sender)),
                        Array.Empty<AddChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingPools, Description, ...)

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.ArgumentError(chargingPool,
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

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.Error(chargingPool,
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

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.Error(chargingPool,
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

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.Timeout(chargingPool,
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

        public static AddChargingPoolsResult

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
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.LockTimeout(chargingPool,
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
