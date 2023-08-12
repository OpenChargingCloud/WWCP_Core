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

    public class AddChargingPoolsResult : AEnititiesResult<AddChargingPoolResult,
                                                           IChargingPool,
                                                           ChargingPool_Id>
    {

        #region Constructor(s)

        public AddChargingPoolsResult(PushDataResultTypes                  Result,
                                      IEnumerable<AddChargingPoolResult>?  SuccessfulChargingPools   = null,
                                      IEnumerable<AddChargingPoolResult>?  RejectedChargingPools     = null,
                                      IId?                                 AuthId                    = null,
                                      Object?                              SendPOIData               = null,
                                      EventTracking_Id?                    EventTrackingId           = null,
                                      I18NString?                          Description               = null,
                                      IEnumerable<Warning>?                Warnings                  = null,
                                      TimeSpan?                            Runtime                   = null)

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


        #region (static) NoOperation

        public static AddChargingPoolsResult

            NoOperation(IEnumerable<IChargingPool>  RejectedChargingPools,
                        IId?                        AuthId            = null,
                        Object?                     SendPOIData       = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        I18NString?                 Description       = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

            {

                EventTrackingId ??= EventTracking_Id.New;

                return new (PushDataResultTypes.NoOperation,
                            Array.Empty<AddChargingPoolResult>(),
                            RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.NoOperation(chargingPool,
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

        public static AddChargingPoolsResult

            LockTimeout(IEnumerable<IChargingPool>  RejectedChargingPools,
                        TimeSpan                    Timeout,
                        IId?                        AuthId            = null,
                        Object?                     SendPOIData       = null,
                        EventTracking_Id?           EventTrackingId   = null,
                        IEnumerable<Warning>?       Warnings          = null,
                        TimeSpan?                   Runtime           = null)

                => new (PushDataResultTypes.LockTimeout,
                        Array.Empty<AddChargingPoolResult>(),
                        RejectedChargingPools.Select(chargingPool => AddChargingPoolResult.LockTimeout(chargingPool,
                                                                                                       Timeout,
                                                                                                       EventTrackingId,
                                                                                                       AuthId,
                                                                                                       SendPOIData)),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        I18NString.Create(
                            Languages.en,
                            $"Lock timeout after {Timeout.TotalSeconds} seconds!"
                        ),
                        Warnings,
                        Runtime);

        #endregion



    }

}
