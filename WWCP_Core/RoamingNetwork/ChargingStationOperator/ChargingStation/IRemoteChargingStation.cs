/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The interface of a remote charging station.
    /// </summary>
    public interface IRemoteChargingStation : ILocalReserveRemoteStartStop
    {

        ChargingStation_Id                           Id          { get; }
        Timestamped<ChargingStationStatusTypes>      Status      { get; }

        Timestamped<ChargingStationAdminStatusTypes> AdminStatus { get; set; }

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        #region OnChargingStationData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingStationDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingStationAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteChargingStationStatusChangedDelegate       OnStatusChanged;

        #endregion



        IRemoteEVSE AddEVSE(IRemoteEVSE                       EVSE,
                            Action<IRemoteEVSE>               Configurator  = null,
                            Action<IRemoteEVSE>               OnSuccess     = null,
                            Action<ChargingStation, EVSE_Id>  OnError       = null);


        Task<IEnumerable<EVSEStatus>> GetEVSEStatus(DateTime                 Timestamp,
                                                    CancellationToken        CancellationToken,
                                                    EventTracking_Id         EventTrackingId,
                                                    TimeSpan?                RequestTimeout = null);


        IEnumerable<IRemoteEVSE> EVSEs { get; }



        Boolean ContainsEVSE(EVSE_Id EVSEId);

        IRemoteEVSE GetEVSEById(EVSE_Id EVSEId);

        Boolean TryGetEVSEById(EVSE_Id EVSEId, out IRemoteEVSE EVSE);



        ChargingStation_Id RemoteChargingStationId { get; set; }
        String RemoteEVSEIdPrefix { get; set; }
        void AddMapping(EVSE_Id LocalEVSEId, EVSE_Id RemoteEVSEId);


    }

}