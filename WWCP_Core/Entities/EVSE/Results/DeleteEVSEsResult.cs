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
    /// The results of a delete EVSEs request.
    /// </summary>
    public class DeleteEVSEsResult : AEnititiesResult<DeleteEVSEResult, IEVSE, EVSE_Id>
    {

        #region Constructor(s)

        public DeleteEVSEsResult(CommandResult             Result,
                                 IEnumerable<DeleteEVSEResult>?  SuccessfulEVSEs   = null,
                                 IEnumerable<DeleteEVSEResult>?  RejectedEVSEs     = null,
                                 IId?                            AuthId            = null,
                                 Object?                         SendPOIData       = null,
                                 EventTracking_Id?               EventTrackingId   = null,
                                 I18NString?                     Description       = null,
                                 IEnumerable<Warning>?           Warnings          = null,
                                 TimeSpan?                       Runtime           = null)

            : base(Result,
                   SuccessfulEVSEs,
                   RejectedEVSEs,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedEVSEs,   ...)

        public static DeleteEVSEsResult

            AdminDown(IEnumerable<IEVSE>     RejectedEVSEs,
                      IId?                   AuthId            = null,
                      Object?                SendPOIData       = null,
                      EventTracking_Id?      EventTrackingId   = null,
                      I18NString?            Description       = null,
                      IEnumerable<Warning>?  Warnings          = null,
                      TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<DeleteEVSEResult>(),
                        RejectedEVSEs.Select(evse => DeleteEVSEResult.AdminDown(evse,
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

        #region (static) NoOperation  (RejectedEVSEs,   ...)

        public static DeleteEVSEsResult

            NoOperation(IEnumerable<IEVSE>     RejectedEVSEs,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        EventTracking_Id?      EventTrackingId   = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<DeleteEVSEResult>(),
                        RejectedEVSEs.Select(evse => DeleteEVSEResult.NoOperation(evse,
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


        #region (static) Enqueued     (SuccessfulEVSEs, ...)

        public static DeleteEVSEsResult

            Enqueued(IEnumerable<IEVSE>     SuccessfulEVSEs,
                     IId?                   AuthId            = null,
                     Object?                SendPOIData       = null,
                     EventTracking_Id?      EventTrackingId   = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulEVSEs.Select(evse => DeleteEVSEResult.Enqueued(evse,
                                                                                 EventTrackingId,
                                                                                 AuthId,
                                                                                 SendPOIData)),
                        Array.Empty<DeleteEVSEResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulEVSEs, ...)

        public static DeleteEVSEsResult

            Success(IEnumerable<IEVSE>     SuccessfulEVSEs,
                    IId?                   AuthId            = null,
                    Object?                SendPOIData       = null,
                    EventTracking_Id?      EventTrackingId   = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulEVSEs.Select(evse => DeleteEVSEResult.Success(evse,
                                                                                EventTrackingId,
                                                                                AuthId,
                                                                                SendPOIData)),
                        Array.Empty<DeleteEVSEResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedEVSEs, Description, ...)

        public static DeleteEVSEsResult

            ArgumentError(IEnumerable<IEVSE>     RejectedEVSEs,
                          I18NString             Description,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<DeleteEVSEResult>(),
                        RejectedEVSEs.Select(evse => DeleteEVSEResult.ArgumentError(evse,
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

        #region (static) Error        (RejectedEVSEs, Description, ...)

        public static DeleteEVSEsResult

            Error(IEnumerable<IEVSE>     RejectedEVSEs,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<DeleteEVSEResult>(),
                        RejectedEVSEs.Select(evse => DeleteEVSEResult.Error(evse,
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

        #region (static) Error        (RejectedEVSEs, Exception,   ...)

        public static DeleteEVSEsResult

            Error(IEnumerable<IEVSE>     RejectedEVSEs,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<DeleteEVSEResult>(),
                        RejectedEVSEs.Select(evse => DeleteEVSEResult.Error(evse,
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

        #region (static) LockTimeout  (RejectedEVSEs, Timeout, ...)

        public static DeleteEVSEsResult

            LockTimeout(IEnumerable<IEVSE>     RejectedEVSEs,
                        TimeSpan               Timeout,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        EventTracking_Id?      EventTrackingId   = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<DeleteEVSEResult>(),
                        RejectedEVSEs.Select(evse => DeleteEVSEResult.LockTimeout(evse,
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
