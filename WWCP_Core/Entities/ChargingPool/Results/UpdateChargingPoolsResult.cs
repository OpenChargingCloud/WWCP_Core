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
    /// The results of an update charging pools request.
    /// </summary>
    public class UpdateChargingPoolsResult : AEnititiesResult<UpdateChargingPoolResult,
                                                              IChargingPool,
                                                              ChargingPool_Id>
    {

        #region Constructor(s)

        public UpdateChargingPoolsResult(CommandResult                     Result,
                                         IEnumerable<UpdateChargingPoolResult>?  SuccessfulChargingPools   = null,
                                         IEnumerable<UpdateChargingPoolResult>?  RejectedChargingPools     = null,
                                         IId?                                    AuthId                    = null,
                                         Object?                                 SendPOIData               = null,
                                         EventTracking_Id?                       EventTrackingId           = null,
                                         I18NString?                             Description               = null,
                                         IEnumerable<Warning>?                   Warnings                  = null,
                                         TimeSpan?                               Runtime                   = null)

            : base(Result,
                   SuccessfulChargingPools,
                   RejectedChargingPools,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedChargingPools,   ...)

        public static UpdateChargingPoolsResult

            AdminDown(IEnumerable<IChargingPool>  RejectedChargingPools,
                      IId?                        AuthId            = null,
                      Object?                     SendPOIData       = null,
                      EventTracking_Id?           EventTrackingId   = null,
                      I18NString?                 Description       = null,
                      IEnumerable<Warning>?       Warnings          = null,
                      TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<UpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => UpdateChargingPoolResult.AdminDown(chargingPool,
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

        #region (static) NoOperation  (RejectedChargingPools,   ...)

        public static UpdateChargingPoolsResult

            NoOperation(IEnumerable<IChargingPool>  RejectedChargingPools,
                        IId?                        AuthId            = null,
                        Object?                     SendPOIData       = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        I18NString?                 Description       = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<UpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => UpdateChargingPoolResult.NoOperation(chargingPool,
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

        public static UpdateChargingPoolsResult

            Enqueued(IEnumerable<IChargingPool>  SuccessfulChargingPools,
                     IId?                        AuthId            = null,
                     Object?                     SendPOIData       = null,
                     EventTracking_Id?           EventTrackingId   = null,
                     I18NString?                 Description       = null,
                     IEnumerable<Warning>?       Warnings          = null,
                     TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(chargingPool => UpdateChargingPoolResult.Enqueued(chargingPool,
                                                                                                         EventTrackingId,
                                                                                                         AuthId,
                                                                                                         SendPOIData)),
                        Array.Empty<UpdateChargingPoolResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static UpdateChargingPoolsResult

            Success(IEnumerable<IChargingPool>  SuccessfulChargingPools,
                    IId?                        AuthId            = null,
                    Object?                     SendPOIData       = null,
                    EventTracking_Id?           EventTrackingId   = null,
                    I18NString?                 Description       = null,
                    IEnumerable<Warning>?       Warnings          = null,
                    TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingPool => UpdateChargingPoolResult.Success(chargingPool,
                                                                                                        EventTrackingId,
                                                                                                        AuthId,
                                                                                                        SendPOIData)),
                        Array.Empty<UpdateChargingPoolResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingPools, Description, ...)

        public static UpdateChargingPoolsResult

            ArgumentError(IEnumerable<IChargingPool>  RejectedChargingPools,
                          I18NString                  Description,
                          EventTracking_Id?           EventTrackingId   = null,
                          IId?                        AuthId            = null,
                          Object?                     SendPOIData       = null,
                          IEnumerable<Warning>?       Warnings          = null,
                          TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<UpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => UpdateChargingPoolResult.ArgumentError(chargingPool,
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

        #region (static) Error        (RejectedChargingPools, Description, ...)

        public static UpdateChargingPoolsResult

            Error(IEnumerable<IChargingPool>  RejectedChargingPools,
                  I18NString                  Description,
                  EventTracking_Id?           EventTrackingId   = null,
                  IId?                        AuthId            = null,
                  Object?                     SendPOIData       = null,
                  IEnumerable<Warning>?       Warnings          = null,
                  TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => UpdateChargingPoolResult.Error(chargingPool,
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

        #region (static) Error        (RejectedChargingPools, Exception,   ...)

        public static UpdateChargingPoolsResult

            Error(IEnumerable<IChargingPool>  RejectedChargingPools,
                  Exception                   Exception,
                  EventTracking_Id?           EventTrackingId   = null,
                  IId?                        AuthId            = null,
                  Object?                     SendPOIData       = null,
                  IEnumerable<Warning>?       Warnings          = null,
                  TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => UpdateChargingPoolResult.Error(chargingPool,
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

        #region (static) LockTimeout  (RejectedChargingPools, Timeout, ...)

        public static UpdateChargingPoolsResult

            LockTimeout(IEnumerable<IChargingPool>  RejectedChargingPools,
                        TimeSpan                    Timeout,
                        IId?                        AuthId            = null,
                        Object?                     SendPOIData       = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        I18NString?                 Description       = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<UpdateChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => UpdateChargingPoolResult.LockTimeout(chargingPool,
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
