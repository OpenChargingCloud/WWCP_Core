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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public interface IEMobilityProvider : IEntity<EMobilityProvider_Id>,
                                          IAdminStatus<EMobilityProviderAdminStatusTypes>,
                                          IStatus<EMobilityProviderStatusTypes>,
                                          ISendChargeDetailRecords,
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
        ChargeDetailRecordFilterDelegate ChargeDetailRecordFilter { get; }
        ReactiveSet<OpenDataLicense> DataLicenses { get; set; }
        Boolean DisableAuthentication { get; set; }
        Boolean DisablePushAdminStatus { get; set; }
        Boolean DisablePushStatus { get; set; }
        Boolean DisableSendChargeDetailRecords { get; set; }
        SimpleEMailAddress? EMailAddress { get; set; }
        IEnumerable<KeyValuePair<eMobilityStation_Id, eMobilityStationAdminStatusTypes>> eMobilityStationAdminStatus { get; }
        IEnumerable<eMobilityStation> eMobilityStations { get; }
        IEnumerable<KeyValuePair<eVehicle_Id, eVehicleAdminStatusTypes>> eVehicleAdminStatus { get; }
        IEnumerable<eVehicle> eVehicles { get; }
        IEnumerable<KeyValuePair<eVehicle_Id, eVehicleStatusTypes>> eVehicleStatus { get; }
        GeoCoordinate GeoLocation { get; set; }
        URL? Homepage { get; set; }
        PhoneNumber? HotlinePhoneNumber { get; set; }
        String Logo { get; set; }
        IVotingSender<DateTime, EMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationAddition { get; }
        IVotingSender<DateTime, EMobilityProvider, eMobilityStation, Boolean> OnEMobilityStationRemoval { get; }
        IVotingSender<DateTime, EMobilityProvider, eVehicle, Boolean> OnEVehicleAddition { get; }
        IVotingSender<DateTime, EMobilityProvider, eVehicle, Boolean> OnEVehicleRemoval { get; }
        eMobilityProviderPriority Priority { get; set; }
        IRemoteEMobilityProvider RemoteEMobilityProvider { get; }
        TimeSpan? RequestTimeout { get; }
        PhoneNumber? Telephone { get; set; }

        event OnAuthorizeStartRequestDelegate OnAuthorizeStartRequest;
        event OnAuthorizeStartResponseDelegate OnAuthorizeStartResponse;
        event OnAuthorizeStopRequestDelegate OnAuthorizeStopRequest;
        event OnAuthorizeStopResponseDelegate OnAuthorizeStopResponse;
        event OnCancelReservationRequestDelegate? OnCancelReservationRequest;
        event OnCancelReservationResponseDelegate? OnCancelReservationResponse;
        event OnEMobilityStationAdminStatusChangedDelegate OnEMobilityStationAdminStatusChanged;
        event OnEMobilityStationDataChangedDelegate OnEMobilityStationDataChanged;
        event OnEVehicleAdminStatusChangedDelegate OnEVehicleAdminStatusChanged;
        event OnEVehicleDataChangedDelegate OnEVehicleDataChanged;
        event OnEVehicleGeoLocationChangedDelegate OnEVehicleGeoLocationChanged;
        event OnEVehicleStatusChangedDelegate OnEVehicleStatusChanged;
        event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;
        event OnNewChargingSessionDelegate OnNewChargingSession;
        event OnNewReservationDelegate? OnNewReservation;
        event OnRemoteStartRequestDelegate OnRemoteStartRequest;
        event OnRemoteStartResponseDelegate OnRemoteStartResponse;
        event OnRemoteStopRequestDelegate OnRemoteStopRequest;
        event OnRemoteStopResponseDelegate OnRemoteStopResponse;
        event OnReservationCanceledDelegate? OnReservationCanceled;
        event OnReserveRequestDelegate? OnReserveRequest;
        event OnReserveResponseDelegate? OnReserveResponse;
        event OnSendCDRsResponseDelegate OnSendCDRsResponse;

        Task<AuthStartResult> AuthorizeStart(LocalAuthentication LocalAuthentication, ChargingLocation ChargingLocation = null, ChargingProduct ChargingProduct = null, ChargingSession_Id? SessionId = null, ChargingSession_Id? CPOPartnerSessionId = null, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStopResult> AuthorizeStop(ChargingSession_Id SessionId, LocalAuthentication LocalAuthentication, ChargingLocation ChargingLocation = null, ChargingSession_Id? CPOPartnerSessionId = null, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<CancelReservationResult> CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, EMobilityProvider_Id? ProviderId = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Boolean ContainseMobilityStation(eMobilityStation eMobilityStation);
        Boolean ContainseMobilityStation(eMobilityStation_Id eMobilityStationId);
        Boolean ContainseVehicle(eVehicle eVehicle);
        Boolean ContainseVehicle(eVehicle_Id eVehicleId);
        eMobilityStation CreateNeweMobilityStation(eMobilityStation_Id eMobilityStationId = null, Action<eMobilityStation> Configurator = null, RemoteEMobilityStationCreatorDelegate RemoteeMobilityStationCreator = null, eMobilityStationAdminStatusTypes AdminStatus = eMobilityStationAdminStatusTypes.Operational, Action<eMobilityStation> OnSuccess = null, Action<EMobilityProvider, eMobilityStation_Id> OnError = null);
        eVehicle CreateNeweVehicle(eVehicle_Id eVehicleId = null, Action<eVehicle> Configurator = null, RemoteEVehicleCreatorDelegate RemoteeVehicleCreator = null, eVehicleAdminStatusTypes AdminStatus = eVehicleAdminStatusTypes.Operational, eVehicleStatusTypes Status = eVehicleStatusTypes.Available, Action<eVehicle> OnSuccess = null, Action<EMobilityProvider, eVehicle_Id> OnError = null);
        eMobilityStation GeteMobilityStationById(eMobilityStation_Id eMobilityStationId);
        eVehicle GetEVehicleById(eVehicle_Id eVehicleId);
        Task<RemoteStartResult> RemoteStart(ChargingLocation ChargingLocation, ChargingProduct ChargingProduct = null, ChargingReservation_Id? ReservationId = null, ChargingSession_Id? SessionId = null, RemoteAuthentication RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<RemoteStopResult> RemoteStop(ChargingSession_Id SessionId, ReservationHandling? ReservationHandling = null, RemoteAuthentication RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        eMobilityStation RemoveeMobilityStation(eMobilityStation_Id eMobilityStationId);
        eVehicle RemoveEVehicle(eVehicle_Id eVehicleId);
        Task<ReservationResult> Reserve(ChargingLocation ChargingLocation, ChargingReservationLevel ReservationLevel = ChargingReservationLevel.EVSE, DateTime? ReservationStartTime = null, TimeSpan? Duration = null, ChargingReservation_Id? ReservationId = null, ChargingReservation_Id? LinkedReservationId = null, EMobilityProvider_Id? ProviderId = null, RemoteAuthentication? RemoteAuthentication = null, ChargingProduct? ChargingProduct = null, IEnumerable<AuthenticationToken>? AuthTokens = null, IEnumerable<eMobilityAccount_Id>? eMAIds = null, IEnumerable<UInt32>? PINs = null, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<SendCDRsResult> SendChargeDetailRecords(IEnumerable<ChargeDetailRecord> ChargeDetailRecords, TransmissionTypes TransmissionType, DateTime? Timestamp = null, CancellationToken CancellationToken = default, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId, eMobilityStationAdminStatusTypes NewStatus, DateTime Timestamp);
        void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId, IEnumerable<Timestamped<eMobilityStationAdminStatusTypes>> StatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetEMobilityStationAdminStatus(eMobilityStation_Id eMobilityStationId, Timestamped<eMobilityStationAdminStatusTypes> NewStatus, Boolean SendUpstream = false);
        void SetEVehicleAdminStatus(eVehicle_Id eVehicleId, eVehicleAdminStatusTypes NewStatus, DateTime Timestamp);
        void SetEVehicleAdminStatus(eVehicle_Id eVehicleId, IEnumerable<Timestamped<eVehicleAdminStatusTypes>> StatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SeteVehicleAdminStatus(eVehicle_Id eVehicleId, Timestamped<eVehicleAdminStatusTypes> NewStatus, Boolean SendUpstream = false);
        Boolean TryGeteMobilityStationById(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation);
        Boolean TryGetEVehicleById(eVehicle_Id eVehicleId, out eVehicle eVehicle);
        Boolean TryRemoveeMobilityStation(eMobilityStation_Id eMobilityStationId, out eMobilityStation eMobilityStation);
        Boolean TryRemoveEVehicle(eVehicle_Id eVehicleId, out eVehicle eVehicle);

    }

}
