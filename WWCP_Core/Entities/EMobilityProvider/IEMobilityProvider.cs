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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The e-mobility provider is not only the main contract party of the EV driver,
    /// the e-mobility provider also takes care of the EV driver master data,
    /// the authentication and authorisation process before charging and for the
    /// billing process after charging.
    /// The e-mobility provider provides the EV driver one or multiple methods for
    /// authentication (e.g. based on RFID cards, login/passwords, client certificates).
    /// The e-mobility provider takes care that none of the provided authentication
    /// methods can be misused by any entity in the ev charging process to track the
    /// ev driver or its behaviour.
    /// </summary>
    public interface IEMobilityProvider : IEntity<EMobilityProvider_Id>,
                                          IAdminStatus<EMobilityProviderAdminStatusTypes>,
                                          IStatus<EMobilityProviderStatusTypes>,
                                          ISendChargeDetailRecords,
                                          IChargingReservations,
                                          IRemoteStartStop,
                                          IChargingSessions,
                                          ISend2RemoteEMobilityProvider,
                                          IEquatable<IEMobilityProvider>,
                                          IComparable<IEMobilityProvider>,
                                          IComparable
    {

        /// <summary>
        /// The roaming network of this charging pool.
        /// </summary>
        IRoamingNetwork?                RoamingNetwork                   { get; }


        Address Address { get; set; }
        ReactiveSet<DataLicense> DataLicenses { get; set; }
        SimpleEMailAddress? EMailAddress { get; set; }
        IEnumerable<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusTypes>> EMobilityStationAdminStatus { get; }
        IEnumerable<eMobilityStation> EMobilityStations { get; }
        IEnumerable<KeyValuePair<EVehicle_Id, eVehicleAdminStatusTypes>> EVehicleAdminStatus { get; }
        IEnumerable<EVehicle> EVehicles { get; }
        IEnumerable<KeyValuePair<EVehicle_Id, eVehicleStatusTypes>> EVehicleStatus { get; }
        GeoCoordinate GeoLocation { get; set; }
        URL? Homepage { get; set; }
        PhoneNumber? HotlinePhoneNumber { get; set; }
        String Logo { get; set; }
        IVotingSender<DateTimeOffset, EMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationAddition { get; }
        IVotingSender<DateTimeOffset, EMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationRemoval { get; }
        IVotingSender<DateTimeOffset, EMobilityProvider, EVehicle, Boolean> OnEVehicleAddition { get; }
        IVotingSender<DateTimeOffset, EMobilityProvider, EVehicle, Boolean> OnEVehicleRemoval { get; }
        EMobilityProviderPriority Priority { get; set; }
        IRemoteEMobilityProvider RemoteEMobilityProvider { get; }
        TimeSpan? RequestTimeout { get; }
        PhoneNumber? Telephone { get; set; }


        #region Events

        event OnNewChargingSessionDelegate?                   OnNewChargingSession;

        event OnNewChargeDetailRecordDelegate?                OnNewChargeDetailRecord;


        event OnEMobilityStationAdminStatusChangedDelegate?   OnEMobilityStationAdminStatusChanged;
        event OnEMobilityStationStatusChangedDelegate?        OnEMobilityStationStatusChanged;
        event OnEMobilityStationDataChangedDelegate?          OnEMobilityStationDataChanged;


        event OnEVehicleAdminStatusChangedDelegate?           OnEVehicleAdminStatusChanged;
        event OnEVehicleStatusChangedDelegate?                OnEVehicleStatusChanged;
        event OnEVehicleDataChangedDelegate?                  OnEVehicleDataChanged;
        event OnEVehicleGeoLocationChangedDelegate?           OnEVehicleGeoLocationChanged;

        #endregion


        void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId, eMobilityStationAdminStatusTypes NewStatus, DateTimeOffset Timestamp);
        void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId, IEnumerable<Timestamped<eMobilityStationAdminStatusTypes>> StatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId, Timestamped<eMobilityStationAdminStatusTypes> NewStatus, Boolean SendUpstream = false);
        eMobilityStation CreateNeweMobilityStation(eMobilityStation_Id eMobilityStationId = null, Action<eMobilityStation> Configurator = null, RemoteEMobilityStationCreatorDelegate RemoteeMobilityStationCreator = null, eMobilityStationAdminStatusTypes AdminStatus = eMobilityStationAdminStatusTypes.Operational, Action<eMobilityStation> OnSuccess = null, Action<EMobilityProvider, eMobilityStation_Id> OnError = null);
        Boolean TryRemoveeMobilityStation(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation);
        eMobilityStation RemoveeMobilityStation(eMobilityStation_Id eMobilityStationId);
        Boolean ContainseMobilityStation(eMobilityStation eMobilityStation);
        Boolean ContainseMobilityStation(eMobilityStation_Id eMobilityStationId);
        Boolean TryGeteMobilityStationById(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation);
        eMobilityStation GeteMobilityStationById(eMobilityStation_Id eMobilityStationId);


        void SetEVehicleAdminStatus(EVehicle_Id eVehicleId, eVehicleAdminStatusTypes NewStatus, DateTimeOffset Timestamp);
        void SetEVehicleAdminStatus(EVehicle_Id eVehicleId, IEnumerable<Timestamped<eVehicleAdminStatusTypes>> StatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SeteVehicleAdminStatus(EVehicle_Id eVehicleId, Timestamped<eVehicleAdminStatusTypes> NewStatus, Boolean SendUpstream = false);
        EVehicle CreateNeweVehicle(EVehicle_Id eVehicleId = null, Action<EVehicle> Configurator = null, RemoteEVehicleCreatorDelegate RemoteeVehicleCreator = null, eVehicleAdminStatusTypes AdminStatus = eVehicleAdminStatusTypes.Operational, eVehicleStatusTypes Status = eVehicleStatusTypes.Available, Action<EVehicle> OnSuccess = null, Action<EMobilityProvider, EVehicle_Id> OnError = null);
        Boolean TryRemoveEVehicle(EVehicle_Id eVehicleId, out EVehicle eVehicle);
        EVehicle RemoveEVehicle(EVehicle_Id eVehicleId);
        Boolean ContainseVehicle(EVehicle eVehicle);
        Boolean ContainseVehicle(EVehicle_Id eVehicleId);
        Boolean TryGetEVehicleById(EVehicle_Id eVehicleId, out EVehicle eVehicle);
        EVehicle GetEVehicleById(EVehicle_Id eVehicleId);


    }

}
