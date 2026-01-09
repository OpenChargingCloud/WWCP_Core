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
    /// The results of a delete charging pools request.
    /// </summary>
    public class DeleteChargingPoolsResult : AEnititiesResult<DeleteChargingPoolResult, IChargingPool, ChargingPool_Id>
    {

        #region Constructor(s)

        public DeleteChargingPoolsResult(CommandResult                     Result,
                                         IEnumerable<DeleteChargingPoolResult>?  SuccessfulChargingPools   = null,
                                         IEnumerable<DeleteChargingPoolResult>?  RejectedChargingPools     = null,
                                         IId?                                    SenderId                  = null,
                                         Object?                                 Sender                    = null,
                                         EventTracking_Id?                       EventTrackingId           = null,
                                         I18NString?                             Description               = null,
                                         IEnumerable<Warning>?                   Warnings                  = null,
                                         TimeSpan?                               Runtime                   = null)

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

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.AdminDown(chargingPool,
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

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.NoOperation(chargingPool,
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

        public static DeleteChargingPoolsResult

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
                        SuccessfulChargingPools.Select(chargingPool => DeleteChargingPoolResult.Enqueued(chargingPool,
                                                                                                         EventTrackingId,
                                                                                                         SenderId,
                                                                                                         Sender)),
                        Array.Empty<DeleteChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static DeleteChargingPoolsResult

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
                        SuccessfulChargingPools.Select(chargingPool => DeleteChargingPoolResult.Success(chargingPool,
                                                                                                        EventTrackingId,
                                                                                                        SenderId,
                                                                                                        Sender)),
                        Array.Empty<DeleteChargingPoolResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingPools, Description, ...)

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.ArgumentError(chargingPool,
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

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.Error(chargingPool,
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

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.Error(chargingPool,
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

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.Timeout(chargingPool,
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

        public static DeleteChargingPoolsResult

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
                        Array.Empty<DeleteChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => DeleteChargingPoolResult.LockTimeout(chargingPool,
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
