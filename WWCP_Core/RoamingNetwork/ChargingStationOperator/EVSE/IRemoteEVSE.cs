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
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The interface of a remote EVSE.
    /// </summary>
    public interface IRemoteEVSE : ILocalReserveRemoteStartStop
    {

        #region Properties

        /// <summary>
        /// The unique identification of this EVSE.
        /// </summary>
        EVSE_Id                     Id                      { get; }

        /// <summary>
        /// An description of this EVSE.
        /// </summary>
        I18NString                  Description             { get; set; }


        /// <summary>
        /// Charging modes.
        /// </summary>
        ReactiveSet<ChargingModes>  ChargingModes           { get; set; }

        /// <summary>
        /// The average voltage.
        /// </summary>
        Double                      AverageVoltage          { get; set; }

        /// <summary>
        /// The type of the current.
        /// </summary>
        CurrentTypes                CurrentType             { get; set; }

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        Double                      MaxCurrent              { get; set; }

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        Double                      MaxPower                { get; set; }

        /// <summary>
        /// The current real-time power delivery [Watt].
        /// </summary>
        Double                      RealTimePower           { get; set; }

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        Double?                     MaxCapacity             { get; set; }

        /// <summary>
        /// The energy meter identification.
        /// </summary>
        EnergyMeter_Id?             EnergyMeterId           { get; set; }


        ReactiveSet<SocketOutlet> SocketOutlets { get; set; }

        #endregion

        #region Events

        #region OnEVSEData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        event OnRemoteEVSEStatusChangedDelegate       OnStatusChanged;

        #endregion

        #endregion


        #region (Admin-)Status

        void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus);
        void SetAdminStatus(Timestamped<EVSEAdminStatusTypes> NewTimestampedAdminStatus);
        void SetAdminStatus(IEnumerable<Timestamped<EVSEAdminStatusTypes>> NewAdminStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetAdminStatus(EVSEAdminStatusTypes NewAdminStatus, DateTime Timestamp);

        Timestamped<EVSEAdminStatusTypes>              AdminStatus         { get; set; }
        IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusSchedule { get; }


        void SetStatus(EVSEStatusTypes NewStatus);
        void SetStatus(Timestamped<EVSEStatusTypes> NewTimestampedStatus);
        void SetStatus(IEnumerable<Timestamped<EVSEStatusTypes>> NewStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetStatus(EVSEStatusTypes NewStatus, DateTime Timestamp);

        Timestamped<EVSEStatusTypes>              Status         { get; set; }
        IEnumerable<Timestamped<EVSEStatusTypes>> StatusSchedule { get; }

        #endregion



        //Task CheckIfReservationIsExpired();

        //IRemoteChargingStation ChargingStation { get; }
        ChargingStationOperator_Id OperatorId { get; }

        IEnumerator<SocketOutlet> GetEnumerator();

        Int32   CompareTo(Object Object);
        Boolean Equals(Object Object);

        Int32   GetHashCode();
        String  ToString();

    }

}