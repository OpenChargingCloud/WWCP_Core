/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The interface of all charging station roaming providers.
    /// </summary>
    public interface ICSORoamingProvider : IRemotePushData,
                                           IRemotePushStatus,
                                           IRemoteAuthorizeStartStop,
                                           IRemoteSendChargeDetailRecord

    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming provider.
        /// </summary>
        CSORoamingProvider_Id  Id               { get; }

        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        I18NString             Name             { get; }

        /// <summary>
        /// The hosting WWCP roaming network.
        /// </summary>
        RoamingNetwork         RoamingNetwork   { get; }

        #endregion

        #region Events

        // Client methods (logging)

        #region OnAuthorizeStart/-Started

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        event OnAuthorizeStartRequestDelegate                   OnAuthorizeStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        event OnAuthorizeStartResponseDelegate                 OnAuthorizeStarted;

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        event OnAuthorizeEVSEStartRequestDelegate               OnAuthorizeEVSEStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStarted;

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        event OnAuthorizeChargingStationStartRequestDelegate    OnAuthorizeChargingStationStart;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStarted;

        #endregion

        #region OnAuthorizeStop/-Stopped

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        event OnAuthorizeStopRequestDelegate                    OnAuthorizeStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopped;

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        event OnAuthorizeEVSEStopRequestDelegate                OnAuthorizeEVSEStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopped;

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        event OnAuthorizeChargingStationStopRequestDelegate     OnAuthorizeChargingStationStop;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopped;

        #endregion

        #endregion

    }

}
