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
    /// The result of a delete roaming networks request.
    /// </summary>
    public class DeleteRoamingNetworksResult : AEnititiesResult<DeleteRoamingNetworkResult, IRoamingNetwork, RoamingNetwork_Id>
    {

        #region Constructor(s)

        public DeleteRoamingNetworksResult(CommandResult                             Result,
                                           IEnumerable<DeleteRoamingNetworkResult>?  SuccessfulRoamingNetworks   = null,
                                           IEnumerable<DeleteRoamingNetworkResult>?  RejectedRoamingNetworks     = null,
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

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.AdminDown(chargingStationOperator,
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

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.NoOperation(chargingStationOperator,
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

        public static DeleteRoamingNetworksResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => DeleteRoamingNetworkResult.Enqueued(chargingStationOperator,
                                                                                                                      EventTrackingId,
                                                                                                                      SenderId,
                                                                                                                      Sender)),
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion

        #region (static) Success      (SuccessfulChargingPools, ...)

        public static DeleteRoamingNetworksResult

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
                        SuccessfulChargingPools.Select(chargingStationOperator => DeleteRoamingNetworkResult.Success(chargingStationOperator,
                                                                                                                     EventTrackingId,
                                                                                                                     SenderId,
                                                                                                                     Sender)),
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        SenderId,
                        Sender,
                        EventTrackingId,
                        Description,
                        Warnings,
                        Runtime);

        }

        #endregion


        #region (static) ArgumentError(RejectedRoamingNetworks, Description, ...)

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.ArgumentError(chargingStationOperator,
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

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.Error(chargingStationOperator,
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

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.Error(chargingStationOperator,
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

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.Timeout(chargingStationOperator,
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

        public static DeleteRoamingNetworksResult

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
                        Array.Empty<DeleteRoamingNetworkResult>(),
                        RejectedRoamingNetworks.Select(chargingStationOperator => DeleteRoamingNetworkResult.LockTimeout(chargingStationOperator,
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
