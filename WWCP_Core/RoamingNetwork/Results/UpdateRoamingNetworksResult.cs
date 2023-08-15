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
    /// The result of an update roaming networks request.
    /// </summary>
    public class UpdateRoamingNetworksResult : AEnititiesResult<UpdateRoamingNetworkResult, IRoamingNetwork, RoamingNetwork_Id>
    {

        #region Constructor(s)

        public UpdateRoamingNetworksResult(PushDataResultTypes                       Result,
                                           IEnumerable<UpdateRoamingNetworkResult>?  SuccessfulRoamingNetworks   = null,
                                           IEnumerable<UpdateRoamingNetworkResult>?  RejectedRoamingNetworks     = null,
                                           IId?                                      AuthId                      = null,
                                           Object?                                   SendPOIData                 = null,
                                           EventTracking_Id?                         EventTrackingId             = null,
                                           I18NString?                               Description                 = null,
                                           IEnumerable<Warning>?                     Warnings                    = null,
                                           TimeSpan?                                 Runtime                     = null)

            : base(Result,
                   SuccessfulRoamingNetworks,
                   RejectedRoamingNetworks,
                   AuthId,
                   SendPOIData,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedRoamingNetworks,   ...)

        public static UpdateRoamingNetworksResult

            AdminDown(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                      IId?                          AuthId            = null,
                      Object?                       SendPOIData       = null,
                      EventTracking_Id?             EventTrackingId   = null,
                      I18NString?                   Description       = null,
                      IEnumerable<Warning>?         Warnings          = null,
                      TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.AdminDown,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.AdminDown(chargingStationOperator,
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

        #region (static) NoOperation  (RejectedRoamingNetworks,   ...)

        public static UpdateRoamingNetworksResult

            NoOperation(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                        IId?                          AuthId            = null,
                        Object?                       SendPOIData       = null,
                        EventTracking_Id?             EventTrackingId   = null,
                        I18NString?                   Description       = null,
                        IEnumerable<Warning>?         Warnings          = null,
                        TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.NoOperation,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.NoOperation(chargingStationOperator,
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

        public static UpdateRoamingNetworksResult

            Enqueued(IEnumerable<IRoamingNetwork>  SuccessfulChargingPools,
                     IId?                          AuthId            = null,
                     Object?                       SendPOIData       = null,
                     EventTracking_Id?             EventTrackingId   = null,
                     I18NString?                   Description       = null,
                     IEnumerable<Warning>?         Warnings          = null,
                     TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Enqueued,
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateRoamingNetworkResult.Enqueued(chargingStationOperator,
                                                                                                                      EventTrackingId,
                                                                                                                      AuthId,
                                                                                                                      SendPOIData)),
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static UpdateRoamingNetworksResult

            Success(IEnumerable<IRoamingNetwork>  SuccessfulChargingPools,
                    IId?                          AuthId            = null,
                    Object?                       SendPOIData       = null,
                    EventTracking_Id?             EventTrackingId   = null,
                    I18NString?                   Description       = null,
                    IEnumerable<Warning>?         Warnings          = null,
                    TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateRoamingNetworkResult.Success(chargingStationOperator,
                                                                                                                     EventTrackingId,
                                                                                                                     AuthId,
                                                                                                                     SendPOIData)),
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        AuthId,
                        SendPOIData,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedRoamingNetworks, Description, ...)

        public static UpdateRoamingNetworksResult

            ArgumentError(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                          I18NString                    Description,
                          EventTracking_Id?             EventTrackingId   = null,
                          IId?                          AuthId            = null,
                          Object?                       SendPOIData       = null,
                          IEnumerable<Warning>?         Warnings          = null,
                          TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.ArgumentError,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.ArgumentError(chargingStationOperator,
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

        #region (static) Error        (RejectedRoamingNetworks, Description, ...)

        public static UpdateRoamingNetworksResult

            Error(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                  I18NString                    Description,
                  EventTracking_Id?             EventTrackingId   = null,
                  IId?                          AuthId            = null,
                  Object?                       SendPOIData       = null,
                  IEnumerable<Warning>?         Warnings          = null,
                  TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.Error(chargingStationOperator,
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

        #region (static) Error        (RejectedRoamingNetworks, Exception,   ...)

        public static UpdateRoamingNetworksResult

            Error(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                  Exception                     Exception,
                  EventTracking_Id?             EventTrackingId   = null,
                  IId?                          AuthId            = null,
                  Object?                       SendPOIData       = null,
                  IEnumerable<Warning>?         Warnings          = null,
                  TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.Error,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.Error(chargingStationOperator,
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

        #region (static) LockTimeout  (RejectedRoamingNetworks, Timeout,     ...)

        public static UpdateRoamingNetworksResult

            LockTimeout(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                        TimeSpan                      Timeout,
                        IId?                          AuthId            = null,
                        Object?                       SendPOIData       = null,
                        EventTracking_Id?             EventTrackingId   = null,
                        I18NString?                   Description       = null,
                        IEnumerable<Warning>?         Warnings          = null,
                        TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (PushDataResultTypes.LockTimeout,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.LockTimeout(chargingStationOperator,
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
