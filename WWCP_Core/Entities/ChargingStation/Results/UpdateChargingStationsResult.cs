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
    /// The results of an update charging stations request.
    /// </summary>
    public class UpdateChargingStationsResult : AEnititiesResult<UpdateChargingStationResult,
                                                                 IChargingStation,
                                                                 ChargingStation_Id>
    {

        #region Constructor(s)

        public UpdateChargingStationsResult(CommandResult                              Result,
                                            IEnumerable<UpdateChargingStationResult>?  SuccessfulChargingStations   = null,
                                            IEnumerable<UpdateChargingStationResult>?  RejectedChargingStations     = null,
                                            IId?                                       SenderId                     = null,
                                            Object?                                    Sender                       = null,
                                            EventTracking_Id?                          EventTrackingId              = null,
                                            I18NString?                                Description                  = null,
                                            IEnumerable<Warning>?                      Warnings                     = null,
                                            TimeSpan?                                  Runtime                      = null)

            : base(Result,
                   SuccessfulChargingStations,
                   RejectedChargingStations,
                   SenderId,
                   Sender,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedChargingStations,   ...)

        public static UpdateChargingStationsResult

            AdminDown(IEnumerable<IChargingStation>  RejectedChargingStations,
                      IId?                           SenderId          = null,
                      Object?                        Sender            = null,
                      EventTracking_Id?              EventTrackingId   = null,
                      I18NString?                    Description       = null,
                      IEnumerable<Warning>?          Warnings          = null,
                      TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(evse => UpdateChargingStationResult.AdminDown(evse,
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

        #region (static) NoOperation  (RejectedChargingStations,   ...)

        public static UpdateChargingStationsResult

            NoOperation(IEnumerable<IChargingStation>  RejectedChargingStations,
                        IId?                           SenderId          = null,
                        Object?                        Sender            = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => UpdateChargingStationResult.NoOperation(chargingStation,
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


        #region (static) Enqueued     (SuccessfulChargingStations, ...)

        public static UpdateChargingStationsResult

            Enqueued(IEnumerable<IChargingStation>  SuccessfulChargingStations,
                     IId?                           SenderId          = null,
                     Object?                        Sender            = null,
                     EventTracking_Id?              EventTrackingId   = null,
                     I18NString?                    Description       = null,
                     IEnumerable<Warning>?          Warnings          = null,
                     TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingStations.Select(chargingStation => UpdateChargingStationResult.Enqueued(chargingStation,
                                                                                                                  EventTrackingId,
                                                                                                                  SenderId,
                                                                                                                  Sender)),
                        Array.Empty<UpdateChargingStationResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingStations, ...)

        public static UpdateChargingStationsResult

            Success(IEnumerable<IChargingStation>  SuccessfulChargingStations,
                    IId?                           SenderId          = null,
                    Object?                        Sender            = null,
                    EventTracking_Id?              EventTrackingId   = null,
                    I18NString?                    Description       = null,
                    IEnumerable<Warning>?          Warnings          = null,
                    TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingStations.Select(chargingStation => UpdateChargingStationResult.Success(chargingStation,
                                                                                                                 EventTrackingId,
                                                                                                                 SenderId,
                                                                                                                 Sender)),
                        Array.Empty<UpdateChargingStationResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStations, Description, ...)

        public static UpdateChargingStationsResult

            ArgumentError(IEnumerable<IChargingStation>  RejectedChargingStations,
                          I18NString                     Description,
                          EventTracking_Id?              EventTrackingId   = null,
                          IId?                           SenderId          = null,
                          Object?                        Sender            = null,
                          IEnumerable<Warning>?          Warnings          = null,
                          TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => UpdateChargingStationResult.ArgumentError(chargingStation,
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

        #region (static) Error        (RejectedChargingStations, Description, ...)

        public static UpdateChargingStationsResult

            Error(IEnumerable<IChargingStation>  RejectedChargingStations,
                  I18NString                     Description,
                  EventTracking_Id?              EventTrackingId   = null,
                  IId?                           SenderId          = null,
                  Object?                        Sender            = null,
                  IEnumerable<Warning>?          Warnings          = null,
                  TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => UpdateChargingStationResult.Error(chargingStation,
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

        #region (static) Error        (RejectedChargingStations, Exception,   ...)

        public static UpdateChargingStationsResult

            Error(IEnumerable<IChargingStation>  RejectedChargingStations,
                  Exception                      Exception,
                  EventTracking_Id?              EventTrackingId   = null,
                  IId?                           SenderId          = null,
                  Object?                        Sender            = null,
                  IEnumerable<Warning>?          Warnings          = null,
                  TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => UpdateChargingStationResult.Error(chargingStation,
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

        #region (static) Timeout      (RejectedChargingStations, Timeout, ...)

        public static UpdateChargingStationsResult

            Timeout(IEnumerable<IChargingStation>  RejectedChargingStations,
                    TimeSpan                       Timeout,
                    IId?                           SenderId          = null,
                    Object?                        Sender            = null,
                    EventTracking_Id?              EventTrackingId   = null,
                    I18NString?                    Description       = null,
                    IEnumerable<Warning>?          Warnings          = null,
                    TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Timeout,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => UpdateChargingStationResult.Timeout(chargingStation,
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

        #region (static) LockTimeout  (RejectedChargingStations, Timeout, ...)

        public static UpdateChargingStationsResult

            LockTimeout(IEnumerable<IChargingStation>  RejectedChargingStations,
                        TimeSpan                       Timeout,
                        IId?                           SenderId          = null,
                        Object?                        Sender            = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<UpdateChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => UpdateChargingStationResult.LockTimeout(chargingStation,
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
