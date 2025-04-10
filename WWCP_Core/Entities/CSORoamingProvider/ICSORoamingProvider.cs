﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The interface of all e-mobility provider roaming providers.
    /// </summary>
    public interface ICSORoamingProvider : ISendRoamingNetworkData,
                                           ISendChargingStationOperatorData,
                                           ISendChargingPoolData,
                                           ISendChargingStationData,
                                           ISendEVSEData,

                                           ISendAdminStatus,
                                           ISendStatus,
                                           ISendEnergyStatus,

                                           ISendAuthorizeStartStop,
                                           ISendChargeDetailRecords

    {

        #region Properties

        /// <summary>
        /// The unique identification of the roaming provider.
        /// </summary>
        CSORoamingProvider_Id  Id               { get; }

        /// <summary>
        /// The official (multi-language) name of the roaming provider.
        /// </summary>
        I18NString             Name             { get; }

        /// <summary>
        /// The official (multi-language) description of the roaming provider.
        /// </summary>
        I18NString             Description      { get; }

        /// <summary>
        /// The hosting WWCP roaming network.
        /// </summary>
        IRoamingNetwork        RoamingNetwork   { get; }

        #endregion

    }

}
