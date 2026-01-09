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
    /// The result of an update roaming networks request.
    /// </summary>
    public class UpdateRoamingNetworksResult : AEnititiesResult<UpdateRoamingNetworkResult, IRoamingNetwork, RoamingNetwork_Id>
    {

        #region Constructor(s)

        public UpdateRoamingNetworksResult(CommandResult                             Result,
                                           IEnumerable<UpdateRoamingNetworkResult>?  SuccessfulRoamingNetworks   = null,
                                           IEnumerable<UpdateRoamingNetworkResult>?  RejectedRoamingNetworks     = null,
                                           IId?                                      SenderId                    = null,
                                           Object?                                   Sender                      = null,
                                           EventTracking_Id?                         EventTrackingId             = null,
                                           I18NString?                               Description                 = null,
                                           IEnumerable<Warning>?                     Warnings                    = null,
                                           TimeSpan?                                 Runtime                     = null)

            : base(Result,
                   SuccessfulRoamingNetworks,
                   RejectedRoamingNetworks,
                   SenderId,
                   Sender,
                   EventTrackingId,
                   Description,
                   Warnings,
                   Runtime)

        { }

        #endregion


        #region (static) AdminDown    (RejectedRoamingNetworks,   ...)

        public static UpdateRoamingNetworksResult

            AdminDown(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                      IId?                          SenderId          = null,
                      Object?                       Sender            = null,
                      EventTracking_Id?             EventTrackingId   = null,
                      I18NString?                   Description       = null,
                      IEnumerable<Warning>?         Warnings          = null,
                      TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.AdminDown,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.AdminDown(chargingStationOperator,
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

        #region (static) NoOperation  (RejectedRoamingNetworks,   ...)

        public static UpdateRoamingNetworksResult

            NoOperation(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                        IId?                          SenderId          = null,
                        Object?                       Sender            = null,
                        EventTracking_Id?             EventTrackingId   = null,
                        I18NString?                   Description       = null,
                        IEnumerable<Warning>?         Warnings          = null,
                        TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.NoOperation,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.NoOperation(chargingStationOperator,
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

        public static UpdateRoamingNetworksResult

            Enqueued(IEnumerable<IRoamingNetwork>  SuccessfulChargingPools,
                     IId?                          SenderId          = null,
                     Object?                       Sender            = null,
                     EventTracking_Id?             EventTrackingId   = null,
                     I18NString?                   Description       = null,
                     IEnumerable<Warning>?         Warnings          = null,
                     TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Enqueued,
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateRoamingNetworkResult.Enqueued(chargingStationOperator,
                                                                                                                      EventTrackingId,
                                                                                                                      SenderId,
                                                                                                                      Sender)),
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static UpdateRoamingNetworksResult

            Success(IEnumerable<IRoamingNetwork>  SuccessfulChargingPools,
                    IId?                          SenderId          = null,
                    Object?                       Sender            = null,
                    EventTracking_Id?             EventTrackingId   = null,
                    I18NString?                   Description       = null,
                    IEnumerable<Warning>?         Warnings          = null,
                    TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Success,
                        SuccessfulChargingPools.Select(chargingStationOperator => UpdateRoamingNetworkResult.Success(chargingStationOperator,
                                                                                                                     EventTrackingId,
                                                                                                                     SenderId,
                                                                                                                     Sender)),
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        SenderId,
                        Sender,
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
                          IId?                          SenderId          = null,
                          Object?                       Sender            = null,
                          IEnumerable<Warning>?         Warnings          = null,
                          TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.ArgumentError,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.ArgumentError(chargingStationOperator,
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

        #region (static) Error        (RejectedRoamingNetworks, Description, ...)

        public static UpdateRoamingNetworksResult

            Error(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                  I18NString                    Description,
                  EventTracking_Id?             EventTrackingId   = null,
                  IId?                          SenderId          = null,
                  Object?                       Sender            = null,
                  IEnumerable<Warning>?         Warnings          = null,
                  TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.Error(chargingStationOperator,
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

        #region (static) Error        (RejectedRoamingNetworks, Exception,   ...)

        public static UpdateRoamingNetworksResult

            Error(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                  Exception                     Exception,
                  EventTracking_Id?             EventTrackingId   = null,
                  IId?                          SenderId          = null,
                  Object?                       Sender            = null,
                  IEnumerable<Warning>?         Warnings          = null,
                  TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Error,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.Error(chargingStationOperator,
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

        #region (static) Timeout      (RejectedRoamingNetworks, Timeout,     ...)

        public static UpdateRoamingNetworksResult

            Timeout(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                    TimeSpan                      Timeout,
                    IId?                          SenderId          = null,
                    Object?                       Sender            = null,
                    EventTracking_Id?             EventTrackingId   = null,
                    I18NString?                   Description       = null,
                    IEnumerable<Warning>?         Warnings          = null,
                    TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.Timeout,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.Timeout(chargingStationOperator,
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

        #region (static) LockTimeout  (RejectedRoamingNetworks, Timeout,     ...)

        public static UpdateRoamingNetworksResult

            LockTimeout(IEnumerable<IRoamingNetwork>  RejectedRoamingNetworks,
                        TimeSpan                      Timeout,
                        IId?                          SenderId          = null,
                        Object?                       Sender            = null,
                        EventTracking_Id?             EventTrackingId   = null,
                        I18NString?                   Description       = null,
                        IEnumerable<Warning>?         Warnings          = null,
                        TimeSpan?                     Runtime           = null)

        {

            EventTrackingId ??= EventTracking_Id.New;

            return new (CommandResult.LockTimeout,
                        Array.Empty<UpdateRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => UpdateRoamingNetworkResult.LockTimeout(chargingStationOperator,
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
