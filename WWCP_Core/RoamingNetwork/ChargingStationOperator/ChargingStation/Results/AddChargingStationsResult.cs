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
    /// The results of an add charging stations request.
    /// </summary>
    public class AddChargingStationsResult : AEnititiesResult<AddChargingStationResult,
                                                              IChargingStation,
                                                              ChargingStation_Id>
    {

        #region Constructor(s)

        public AddChargingStationsResult(PushDataResultTypes                     Result,
                                         IEnumerable<AddChargingStationResult>?  SuccessfulChargingStations   = null,
                                         IEnumerable<AddChargingStationResult>?  RejectedChargingStations     = null,
                                         IId?                                    AuthId                       = null,
                                         Object?                                 SendPOIData                  = null,
                                         EventTracking_Id?                       EventTrackingId              = null,
                                         I18NString?                             Description                  = null,
                                         IEnumerable<Warning>?                   Warnings                     = null,
                                         TimeSpan?                               Runtime                      = null)

            : base(Result,
                   SuccessfulChargingStations,
                   RejectedChargingStations,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) NoOperation  (RejectedChargingStations,   ...)

        public static AddChargingStationsResult

            NoOperation(IEnumerable<IChargingStation>  RejectedChargingStations,
                        IId?                           AuthId            = null,
                        Object?                        SendPOIData       = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.NoOperation,
                        Array.Empty<AddChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => AddChargingStationResult.NoOperation(chargingStation,
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


        #region (static) Enqueued     (SuccessfulChargingStations, ...)

        public static AddChargingStationsResult

            Enqueued(IEnumerable<IChargingStation>  SuccessfulChargingStations,
                     IId?                           AuthId            = null,
                     Object?                        SendPOIData       = null,
                     EventTracking_Id?              EventTrackingId   = null,
                     I18NString?                    Description       = null,
                     IEnumerable<Warning>?          Warnings          = null,
                     TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Enqueued,
                        SuccessfulChargingStations.Select(chargingStation => AddChargingStationResult.Enqueued(chargingStation,
                                                                                                               EventTrackingId,
                                                                                                               AuthId,
                                                                                                               SendPOIData)),
                        Array.Empty<AddChargingStationResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingStations, ...)

        public static AddChargingStationsResult

            Success(IEnumerable<IChargingStation>  SuccessfulChargingStations,
                    IId?                           AuthId            = null,
                    Object?                        SendPOIData       = null,
                    EventTracking_Id?              EventTrackingId   = null,
                    I18NString?                    Description       = null,
                    IEnumerable<Warning>?          Warnings          = null,
                    TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingStations.Select(chargingStation => AddChargingStationResult.Success(chargingStation,
                                                                                                              EventTrackingId,
                                                                                                              AuthId,
                                                                                                              SendPOIData)),
                        Array.Empty<AddChargingStationResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedChargingStations, Description, ...)

        public static AddChargingStationsResult

            ArgumentError(IEnumerable<IChargingStation>  RejectedChargingStations,
                          I18NString                     Description,
                          EventTracking_Id?              EventTrackingId   = null,
                          IId?                           AuthId            = null,
                          Object?                        SendPOIData       = null,
                          IEnumerable<Warning>?          Warnings          = null,
                          TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.ArgumentError,
                        Array.Empty<AddChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => AddChargingStationResult.ArgumentError(chargingStation,
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

        #region (static) Error        (RejectedChargingStations, Description, ...)

        public static AddChargingStationsResult

            Error(IEnumerable<IChargingStation>  RejectedChargingStations,
                  I18NString                     Description,
                  EventTracking_Id?              EventTrackingId   = null,
                  IId?                           AuthId            = null,
                  Object?                        SendPOIData       = null,
                  IEnumerable<Warning>?          Warnings          = null,
                  TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<AddChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => AddChargingStationResult.Error(chargingStation,
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

        #region (static) Error        (RejectedChargingStations, Exception,   ...)

        public static AddChargingStationsResult

            Error(IEnumerable<IChargingStation>  RejectedChargingStations,
                  Exception                      Exception,
                  EventTracking_Id?              EventTrackingId   = null,
                  IId?                           AuthId            = null,
                  Object?                        SendPOIData       = null,
                  IEnumerable<Warning>?          Warnings          = null,
                  TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<AddChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => AddChargingStationResult.Error(chargingStation,
                                                                                                          Exception,
                                                                                                          EventTrackingId,
                                                                                                          AuthId,
                                                                                                          SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Exception.Message.ToI18NString(Languages.en),
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) LockTimeout  (RejectedChargingStations, Timeout, ...)

        public static AddChargingStationsResult

            LockTimeout(IEnumerable<IChargingStation>  RejectedChargingStations,
                        TimeSpan                       Timeout,
                        IId?                           AuthId            = null,
                        Object?                        SendPOIData       = null,
                        EventTracking_Id?              EventTrackingId   = null,
                        I18NString?                    Description       = null,
                        IEnumerable<Warning>?          Warnings          = null,
                        TimeSpan?                      Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.LockTimeout,
                        Array.Empty<AddChargingStationResult>(),
                        RejectedChargingStations.Select(chargingStation => AddChargingStationResult.LockTimeout(chargingStation,
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
