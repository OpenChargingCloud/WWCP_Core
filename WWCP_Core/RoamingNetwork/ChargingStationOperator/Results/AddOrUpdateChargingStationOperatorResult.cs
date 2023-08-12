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

    public class AddOrUpdateChargingStationOperatorResult : AEnitityResult<IChargingStationOperator, ChargingStationOperator_Id>
    {

        #region Properties

        public IChargingStationOperator?  ChargingStationOperator
            => Object;

        public IRoamingNetwork?           RoamingNetwork    { get; internal set; }

        public AddedOrUpdated?            AddedOrUpdated    { get; internal set; }

        #endregion

        #region Constructor(s)

        public AddOrUpdateChargingStationOperatorResult(IChargingStationOperator  ChargingStationOperator,
                                                        PushDataResultTypes       Result,
                                                        EventTracking_Id?         EventTrackingId   = null,
                                                        IId?                      AuthId            = null,
                                                        Object?                   SendPOIData       = null,
                                                        IRoamingNetwork?          RoamingNetwork    = null,
                                                        AddedOrUpdated?           AddedOrUpdated    = null,
                                                        I18NString?               Description       = null,
                                                        IEnumerable<Warning>?     Warnings          = null,
                                                        TimeSpan?                 Runtime           = null)

            : base(ChargingStationOperator,
                   Result,
                   EventTrackingId,
                   AuthId,
                   SendPOIData,
                   Description,
                   Warnings,
                   Runtime)

        {

            this.RoamingNetwork  = RoamingNetwork;
            this.AddedOrUpdated  = AddedOrUpdated;

        }

        #endregion


        #region (static) NoOperation

        public static AddOrUpdateChargingStationOperatorResult

            NoOperation(IChargingStationOperator  ChargingStationOperator,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      AuthId            = null,
                        Object?                   SendPOIData       = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        I18NString?               Description       = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.NoOperation,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.NoOperation,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) ArgumentError(...)

        public static AddOrUpdateChargingStationOperatorResult

            ArgumentError(IChargingStationOperator  ChargingStationOperator,
                          EventTracking_Id?         EventTrackingId   = null,
                          IId?                      AuthId            = null,
                          Object?                   SendPOIData       = null,
                          IRoamingNetwork?          RoamingNetwork    = null,
                          I18NString?               Description       = null,
                          IEnumerable<Warning>?     Warnings          = null,
                          TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.ArgumentError,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Added(...)

        public static AddOrUpdateChargingStationOperatorResult

            Added(IChargingStationOperator  ChargingStationOperator,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  I18NString?               Description       = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.Add,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Updated(...)

        public static AddOrUpdateChargingStationOperatorResult

            Updated(IChargingStationOperator  ChargingStationOperator,
                    EventTracking_Id?         EventTrackingId   = null,
                    IId?                      AuthId            = null,
                    Object?                   SendPOIData       = null,
                    IRoamingNetwork?          RoamingNetwork    = null,
                    I18NString?               Description       = null,
                    IEnumerable<Warning>?     Warnings          = null,
                    TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Success,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.Update,
                        Description,
                        Warnings,
                        Runtime);

        #endregion


        #region (static) Error(ChargingStationOperator, Description, ...)

        public static AddOrUpdateChargingStationOperatorResult

            Error(IChargingStationOperator  ChargingStationOperator,
                  I18NString                Description,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.Failed,
                        Description,
                        Warnings,
                        Runtime);

        #endregion

        #region (static) Error(ChargingStationOperator, Exception,   ...)

        public static AddOrUpdateChargingStationOperatorResult

            Error(IChargingStationOperator  ChargingStationOperator,
                  Exception                 Exception,
                  EventTracking_Id?         EventTrackingId   = null,
                  IId?                      AuthId            = null,
                  Object?                   SendPOIData       = null,
                  IRoamingNetwork?          RoamingNetwork    = null,
                  IEnumerable<Warning>?     Warnings          = null,
                  TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.Error,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.Failed,
                        I18NString.Create(
                            Languages.en,
                            Exception.Message
                        ),
                        Warnings,
                        Runtime);

        #endregion

        #region (static) LockTimeout(Timeout, ...)

        public static AddOrUpdateChargingStationOperatorResult

            LockTimeout(IChargingStationOperator  ChargingStationOperator,
                        TimeSpan                  Timeout,
                        EventTracking_Id?         EventTrackingId   = null,
                        IId?                      AuthId            = null,
                        Object?                   SendPOIData       = null,
                        IRoamingNetwork?          RoamingNetwork    = null,
                        IEnumerable<Warning>?     Warnings          = null,
                        TimeSpan?                 Runtime           = null)

                => new (ChargingStationOperator,
                        PushDataResultTypes.LockTimeout,
                        EventTrackingId,
                        AuthId,
                        SendPOIData,
                        RoamingNetwork,
                        social.OpenData.UsersAPI.AddedOrUpdated.Failed,
                        I18NString.Create(
                            Languages.en,
                            $"Lock timeout after {Timeout.TotalSeconds} seconds!"
                        ),
                        Warnings,
                        Runtime);

        #endregion



    }

}
