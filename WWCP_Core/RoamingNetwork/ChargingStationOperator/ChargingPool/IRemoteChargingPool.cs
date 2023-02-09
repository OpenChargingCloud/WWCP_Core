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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The interface of a remote charging pool.
    /// </summary>
    public interface IRemoteChargingPool : IChargingPool
    {

        #region Properties

        /// <summary>
        /// The unique identification of this charging pool.
        /// </summary>
        ChargingPool_Id             Id                      { get; }

        /// <summary>
        /// The multi-language name of this charging pool.
        /// </summary>
        I18NString                  Name                    { get; }

        /// <summary>
        /// The multi-language description of this charging pool.
        /// </summary>
        I18NString                  Description             { get; }

        #endregion

        #region Events

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingPoolDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingPoolAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingPoolStatusChangedDelegate?       OnStatusChanged;

        #endregion

        #endregion


        #region EVSEs

        //Boolean TryGetEVSEById(EVSE_Id EVSEId, out IRemoteEVSE RemoteEVSE);

        #endregion

        #region Charging stations

        //IEnumerable<IRemoteChargingStation> ChargingStations { get; }

        //Boolean ContainsChargingStationId(ChargingStation_Id ChargingStationId);

        //IRemoteChargingStation GetChargingStationById(ChargingStation_Id ChargingStationId);

        //Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IRemoteChargingStation ChargingStation);

        //Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IRemoteChargingStation ChargingStation);

        #endregion


    }

}