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

using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class UpdateEVSEResult : AEnitityResult<IEVSE, EVSE_Id>
    {

        #region Properties

        public IEVSE?             EVSE
            => Object;

        public IChargingStation?  ChargingStation    { get; internal set; }

        #endregion

        #region Constructor(s)

        public UpdateEVSEResult(IEVSE                  EVSE,
                                PushDataResultTypes    Result,
                                EventTracking_Id?      EventTrackingId   = null,
                                IId?                   AuthId            = null,
                                Object?                SendPOIData       = null,
                                IChargingStation?      ChargingStation   = null,
                                I18NString?            Description       = null,
                                IEnumerable<Warning>?  Warnings          = null,
                                TimeSpan?              Runtime           = null)

            : base(EVSE,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.ChargingStation = ChargingStation;

        }

        #endregion


        //public static UpdateEVSEResult Success(IEVSE             EVSE,
        //                                       EventTracking_Id  EventTrackingId)

        //    => new (EVSE,
        //            EventTrackingId,
        //            true,
        //            null,
        //            null);


        //public static UpdateEVSEResult ArgumentError(IEVSE             EVSE,
        //                                             EventTracking_Id  EventTrackingId,
        //                                             String            Argument,
        //                                             String            Description)

        //    => new (EVSE,
        //            EventTrackingId,
        //            false,
        //            Argument,
        //            I18NString.Create(
        //                Languages.en,
        //                Description
        //            ));

        //public static UpdateEVSEResult ArgumentError(IEVSE             EVSE,
        //                                             EventTracking_Id  EventTrackingId,
        //                                             String            Argument,
        //                                             I18NString        Description)

        //    => new (EVSE,
        //            EventTrackingId,
        //            false,
        //            Argument,
        //            Description);


        //public static UpdateEVSEResult Failed(IEVSE             EVSE,
        //                                      EventTracking_Id  EventTrackingId,
        //                                      String            Description)

        //    => new (EVSE,
        //            EventTrackingId,
        //            false,
        //            null,
        //            I18NString.Create(
        //                Languages.en,
        //                Description
        //            ));

        //public static UpdateEVSEResult Failed(IEVSE             EVSE,
        //                                      EventTracking_Id  EventTrackingId,
        //                                      I18NString        Description)

        //    => new (EVSE,
        //            EventTrackingId,
        //            false,
        //            null,
        //            Description);

        //public static UpdateEVSEResult Failed(IEVSE             EVSE,
        //                                      EventTracking_Id  EventTrackingId,
        //                                      Exception         Exception)

        //    => new (EVSE,
        //            EventTrackingId,
        //            false,
        //            null,
        //            I18NString.Create(
        //                Languages.en,
        //                Exception.Message
        //            ));



        #region (static) NoOperation

        public static UpdateEVSEResult

            NoOperation(IEVSE                  EVSE,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingStation?      ChargingStation   = null,
                        I18NString?            Description       = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError(...)

        public static UpdateEVSEResult

            ArgumentError(IEVSE                  EVSE,
                          EventTracking_Id?      EventTrackingId   = null,
                          IId?                   AuthId            = null,
                          Object?                SendPOIData       = null,
                          IChargingStation?      ChargingStation   = null,
                          I18NString?            Description       = null,
                          IEnumerable<Warning>?  Warnings          = null,
                          TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Enqueued(...)

        public static UpdateEVSEResult

            Enqueued(IEVSE                  EVSE,
                     EventTracking_Id?      EventTrackingId   = null,
                     IId?                   AuthId            = null,
                     Object?                SendPOIData       = null,
                     IChargingStation?      ChargingStation   = null,
                     I18NString?            Description       = null,
                     IEnumerable<Warning>?  Warnings          = null,
                     TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.Enqueued,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Success(...)

        public static UpdateEVSEResult

            Success(IEVSE                  EVSE,
                    EventTracking_Id?      EventTrackingId   = null,
                    IId?                   AuthId            = null,
                    Object?                SendPOIData       = null,
                    IChargingStation?      ChargingStation   = null,
                    I18NString?            Description       = null,
                    IEnumerable<Warning>?  Warnings          = null,
                    TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Error(ChargingStationOperator, Description, ...)

        public static UpdateEVSEResult

            Error(IEVSE                  EVSE,
                  I18NString             Description,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingStation?      ChargingStation   = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error(ChargingStationOperator, Exception,   ...)

        public static UpdateEVSEResult

            Error(IEVSE                  EVSE,
                  Exception              Exception,
                  EventTracking_Id?      EventTrackingId   = null,
                  IId?                   AuthId            = null,
                  Object?                SendPOIData       = null,
                  IChargingStation?      ChargingStation   = null,
                  IEnumerable<Warning>?  Warnings          = null,
                  TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        I18NString.Create(
                            Languages.en,
                            Exception.Message
                        ),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout(Timeout, ...)

        public static UpdateEVSEResult

            LockTimeout(IEVSE                  EVSE,
                        TimeSpan               Timeout,
                        EventTracking_Id?      EventTrackingId   = null,
                        IId?                   AuthId            = null,
                        Object?                SendPOIData       = null,
                        IChargingStation?      ChargingStation   = null,
                        IEnumerable<Warning>?  Warnings          = null,
                        TimeSpan?              Runtime           = null)

                => new (EVSE,
                        PushDataResultTypes.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        ChargingStation,
                        I18NString.Create(
                            Languages.en,
                            $"Lock timeout after {Timeout.TotalSeconds} seconds!"
                        ),
                        Warnings,
                        Runtime);

        #endregion


    }

}
