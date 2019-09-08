using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

namespace org.GraphDefined.WWCP
{
    public interface IRoamingNetwork1
    {

        IEnumerable<IEnumerable<ChargeDetailRecord>> ChargeDetailRecords { get; }
        ChargeDetailRecordsStore ChargeDetailRecordsStore { get; }

        ChargingPoolSignatureDelegate ChargingPoolSignatureGenerator { get; }
        IEnumerable<ChargingSession> ChargingSessions { get; }


        IEnumerable<ICSORoamingProvider> ChargingStationOperatorRoamingProviders { get; }

        ChargingStationOperatorSignatureDelegate ChargingStationOperatorSignatureGenerator { get; }
        IEnumerable<KeyValuePair<ChargingStationOperator_Id, IEnumerable<Timestamped<ChargingStationOperatorStatusTypes>>>> ChargingStationOperatorStatus { get; }
        ChargingStationSignatureDelegate ChargingStationSignatureGenerator { get; }

        bool DisableAuthentication { get; set; }
        bool DisableSendChargeDetailRecords { get; set; }


        IEnumerable<GridOperator> GridOperators { get; }
        IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusType>>>> GridOperatorsAdminStatus { get; }
        IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusType>>>> GridOperatorsStatus { get; }
        IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderAdminStatusType>>>> NavigationProviderAdminStatus { get; }
        IEnumerable<NavigationProvider> NavigationProviders { get; }
        IEnumerable<KeyValuePair<NavigationProvider_Id, IEnumerable<Timestamped<NavigationProviderStatusType>>>> NavigationProviderStatus { get; }
        IVotingSender<DateTime, ChargingStationOperator, ChargingPool, bool> OnChargingPoolAddition { get; }
        IVotingSender<DateTime, ChargingStationOperator, ChargingPool, bool> OnChargingPoolRemoval { get; }
        IVotingSender<DateTime, ChargingPool, ChargingStation, bool> OnChargingStationAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, ChargingStationOperator, bool> OnChargingStationOperatorAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, ChargingStationOperator, bool> OnChargingStationOperatorRemoval { get; }
        IVotingSender<DateTime, ChargingPool, ChargingStation, bool> OnChargingStationRemoval { get; }
        IVotingSender<RoamingNetwork, ICSORoamingProvider, bool> OnCPORoamingProviderAddition { get; }
        IVotingSender<RoamingNetwork, ICSORoamingProvider, bool> OnCPORoamingProviderRemoval { get; }
        IVotingSender<DateTime, RoamingNetwork, eMobilityProvider, bool> OnEMobilityProviderAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, eMobilityProvider, bool> OnEMobilityProviderRemoval { get; }
        IVotingSender<RoamingNetwork, IEMPRoamingProvider, bool> OnEMPRoamingProviderAddition { get; }
        IVotingSender<RoamingNetwork, IEMPRoamingProvider, bool> OnEMPRoamingProviderRemoval { get; }
        IVotingSender<DateTime, ChargingStation, EVSE, bool> OnEVSEAddition { get; }
        IVotingSender<DateTime, ChargingStation, EVSE, bool> OnEVSERemoval { get; }
        IVotingSender<DateTime, RoamingNetwork, GridOperator, bool> OnGridOperatorAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, GridOperator, bool> OnGridOperatorRemoval { get; }
        IVotingSender<DateTime, RoamingNetwork, NavigationProvider, bool> OnNavigationProviderAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, NavigationProvider, bool> OnNavigationProviderRemoval { get; }
        IVotingSender<DateTime, RoamingNetwork, ParkingOperator, bool> OnParkingOperatorAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, ParkingOperator, bool> OnParkingOperatorRemoval { get; }
        IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, bool> OnSmartCityAddition { get; }
        IVotingSender<DateTime, RoamingNetwork, SmartCityProxy, bool> OnSmartCityRemoval { get; }
        IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorAdminStatusType>>>> ParkingOperatorAdminStatus { get; }
        IEnumerable<ParkingOperator> ParkingOperators { get; }
        IEnumerable<KeyValuePair<ParkingOperator_Id, IEnumerable<Timestamped<ParkingOperatorStatusType>>>> ParkingOperatorStatus { get; }
        IEnumerable<ChargingReservation> ChargingReservations { get; }
        ChargingReservationsStore ReservationsStore { get; }
        ChargingSessionsStore SessionsStore { get; }
        IEnumerable<SmartCityProxy> SmartCities { get; }
        IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityAdminStatusType>>>> SmartCitiesAdminStatus { get; }
        IEnumerable<KeyValuePair<SmartCity_Id, IEnumerable<Timestamped<SmartCityStatusType>>>> SmartCitiesStatus { get; }


        event OnRoamingNetworkAdminStatusChangedDelegate OnAggregatedAdminStatusChanged;
        event OnAuthorizeChargingPoolStartRequestDelegate OnAuthorizeChargingPoolStartRequest;
        event OnAuthorizeChargingPoolStartResponseDelegate OnAuthorizeChargingPoolStartResponse;
        event OnAuthorizeChargingPoolStopRequestDelegate OnAuthorizeChargingPoolStopRequest;
        event OnAuthorizeChargingPoolStopResponseDelegate OnAuthorizeChargingPoolStopResponse;
        event OnAuthorizeChargingStationStartRequestDelegate OnAuthorizeChargingStationStartRequest;
        event OnAuthorizeChargingStationStartResponseDelegate OnAuthorizeChargingStationStartResponse;
        event OnAuthorizeChargingStationStopRequestDelegate OnAuthorizeChargingStationStopRequest;
        event OnAuthorizeChargingStationStopResponseDelegate OnAuthorizeChargingStationStopResponse;
        event OnAuthorizeEVSEStartRequestDelegate OnAuthorizeEVSEStartRequest;
        event OnAuthorizeEVSEStartResponseDelegate OnAuthorizeEVSEStartResponse;
        event OnAuthorizeEVSEStopRequestDelegate OnAuthorizeEVSEStopRequest;
        event OnAuthorizeEVSEStopResponseDelegate OnAuthorizeEVSEStopResponse;
        event OnAuthorizeStartRequestDelegate OnAuthorizeStartRequest;
        event OnAuthorizeStartResponseDelegate OnAuthorizeStartResponse;
        event OnAuthorizeStopRequestDelegate OnAuthorizeStopRequest;
        event OnAuthorizeStopResponseDelegate OnAuthorizeStopResponse;
        event OnCancelReservationRequestDelegate OnCancelReservationRequest;
        event OnCancelReservationResponseDelegate OnCancelReservationResponse;
        event RoamingNetwork.OnCDRWasFilteredDelegate OnCDRWasFiltered;
        event RoamingNetwork.OnChargingPoolAdminDiffDelegate OnChargingPoolAdminDiff;
        event OnChargingPoolAdminStatusChangedDelegate OnChargingPoolAdminStatusChanged;
        event OnChargingPoolDataChangedDelegate OnChargingPoolDataChanged;
        event OnChargingPoolStatusChangedDelegate OnChargingPoolStatusChanged;
        event RoamingNetwork.OnChargingStationAdminDiffDelegate OnChargingStationAdminDiff;
        event OnChargingStationAdminStatusChangedDelegate OnChargingStationAdminStatusChanged;
        event OnChargingStationDataChangedDelegate OnChargingStationDataChanged;
        event OnChargingStationOperatorAdminStatusChangedDelegate OnChargingStationOperatorAdminStatusChanged;
        event OnChargingStationOperatorDataChangedDelegate OnChargingStationOperatorDataChanged;
        event OnChargingStationOperatorStatusChangedDelegate OnChargingStationOperatorStatusChanged;
        event OnChargingStationStatusChangedDelegate OnChargingStationStatusChanged;
        event OnRoamingNetworkDataChangedDelegate OnDataChanged;
        event OnEVSEAdminStatusChangedDelegate OnEVSEAdminStatusChanged;
        event OnEVSEDataChangedDelegate OnEVSEDataChanged;
        event OnEVSEStatusChangedDelegate OnEVSEStatusChanged;
        event RoamingNetwork.OnEVSEStatusDiffDelegate OnEVSEStatusDiff;
        event RoamingNetwork.OnFilterCDRRecordsDelegate OnFilterCDRRecords;
        event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;
        event OnNewChargingSessionDelegate OnNewChargingSession;
        event OnNewReservationDelegate OnNewReservation;
        event OnParkingOperatorAdminStatusChangedDelegate OnParkingOperatorAdminStatusChanged;
        event OnParkingOperatorDataChangedDelegate OnParkingOperatorDataChanged;
        event OnParkingOperatorStatusChangedDelegate OnParkingOperatorStatusChanged;
        event OnRemoteStartRequestDelegate OnRemoteStartRequest;
        event OnRemoteStartResponseDelegate OnRemoteStartResponse;
        event OnRemoteStopRequestDelegate OnRemoteStopRequest;
        event OnRemoteStopResponseDelegate OnRemoteStopResponse;
        event OnReservationCanceledDelegate OnReservationCanceled;
        event OnReserveRequestDelegate OnReserveRequest;
        event OnReserveResponseDelegate OnReserveResponse;
        event OnSendCDRRequestDelegate OnSendCDRsRequest;
        event OnSendCDRResponseDelegate OnSendCDRsResponse;
        event OnRoamingNetworkStatusChangedDelegate OnStatusChanged;


        Task<AuthStartResult> AuthorizeStart(LocalAuthentication LocalAuthentication, ChargingProduct ChargingProduct = null, ChargingSession_Id? SessionId = null, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStartChargingPoolResult> AuthorizeStart(LocalAuthentication LocalAuthentication, ChargingPool_Id ChargingPoolId, ChargingProduct ChargingProduct = null, ChargingSession_Id? SessionId = null, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStartChargingStationResult> AuthorizeStart(LocalAuthentication LocalAuthentication, ChargingStation_Id ChargingStationId, ChargingProduct ChargingProduct = null, ChargingSession_Id? SessionId = null, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStartEVSEResult> AuthorizeStart(LocalAuthentication LocalAuthentication, EVSE_Id EVSEId, ChargingProduct ChargingProduct = null, ChargingSession_Id? SessionId = null, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStopResult> AuthorizeStop(ChargingSession_Id SessionId, LocalAuthentication LocalAuthentication, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStopChargingPoolResult> AuthorizeStop(ChargingSession_Id SessionId, LocalAuthentication LocalAuthentication, ChargingPool_Id ChargingPoolId, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStopChargingStationResult> AuthorizeStop(ChargingSession_Id SessionId, LocalAuthentication LocalAuthentication, ChargingStation_Id ChargingStationId, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<AuthStopEVSEResult> AuthorizeStop(ChargingSession_Id SessionId, LocalAuthentication LocalAuthentication, EVSE_Id EVSEId, ChargingStationOperator_Id? OperatorId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<CancelReservationResult> CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);

        int CompareTo(object Object);
        int CompareTo(RoamingNetwork RoamingNetwork);
        bool ContainsGridOperator(GridOperator GridOperator);
        bool ContainsGridOperator(GridOperator_Id GridOperatorId);
        bool ContainsNavigationProvider(NavigationProvider NavigationProvider);
        bool ContainsNavigationProvider(NavigationProvider_Id NavigationProviderId);
        bool ContainsParkingOperator(ParkingOperator ParkingOperator);
        bool ContainsParkingOperator(ParkingOperator_Id ParkingOperatorId);
        bool ContainsSmartCity(SmartCity_Id SmartCityId);
        bool ContainsSmartCity(SmartCityProxy SmartCity);
        GridOperator CreateNewGridOperator(GridOperator_Id GridOperatorId, I18NString Name = null, I18NString Description = null, GridOperatorPriority Priority = null, GridOperatorAdminStatusType AdminStatus = GridOperatorAdminStatusType.Available, GridOperatorStatusType Status = GridOperatorStatusType.Available, Action<GridOperator> Configurator = null, Action<GridOperator> OnSuccess = null, Action<RoamingNetwork, GridOperator_Id> OnError = null, RemoteGridOperatorCreatorDelegate RemoteGridOperatorCreator = null);
        NavigationProvider CreateNewNavigationProvider(NavigationProvider_Id NavigationProviderId, I18NString Name = null, I18NString Description = null, NavigationProviderPriority Priority = null, NavigationProviderAdminStatusType AdminStatus = NavigationProviderAdminStatusType.Available, NavigationProviderStatusType Status = NavigationProviderStatusType.Available, Action<NavigationProvider> Configurator = null, Action<NavigationProvider> OnSuccess = null, Action<RoamingNetwork, NavigationProvider_Id> OnError = null, RemoteNavigationProviderCreatorDelegate RemoteNavigationProviderCreator = null);
        ParkingOperator CreateNewParkingOperator(ParkingOperator_Id ParkingOperatorId, I18NString Name = null, I18NString Description = null, Action<ParkingOperator> Configurator = null, RemoteParkingOperatorCreatorDelegate RemoteParkingOperatorCreator = null, ParkingOperatorAdminStatusType AdminStatus = ParkingOperatorAdminStatusType.Operational, ParkingOperatorStatusType Status = ParkingOperatorStatusType.Available, Action<ParkingOperator> OnSuccess = null, Action<RoamingNetwork, ParkingOperator_Id> OnError = null);
        SmartCityProxy CreateNewSmartCity(SmartCity_Id SmartCityId, I18NString Name = null, I18NString Description = null, SmartCityPriority Priority = null, SmartCityAdminStatusType AdminStatus = SmartCityAdminStatusType.Available, SmartCityStatusType Status = SmartCityStatusType.Available, Action<SmartCityProxy> Configurator = null, Action<SmartCityProxy> OnSuccess = null, Action<RoamingNetwork, SmartCity_Id> OnError = null, RemoteSmartCityCreatorDelegate RemoteSmartCityCreator = null);
        ParkingSpace CreateParkingSpace(ParkingSpace_Id ParkingSpaceId, Action<ParkingSpace> Configurator = null, Action<ParkingSpace> OnSuccess = null, Action<ChargingStationOperator, ParkingSpace_Id> OnError = null);
        bool Equals(object Object);
        bool Equals(RoamingNetwork RoamingNetwork);

        IEnumerator<IEntity> GetEnumerator();
        GridOperator GetGridOperatorById(GridOperator_Id GridOperatorId);
        int GetHashCode();
        NavigationProvider GetNavigationProviderById(NavigationProvider_Id NavigationProviderId);
        ParkingOperator GetParkingOperatorById(ParkingOperator_Id ParkingOperatorId);
        SmartCityProxy GetSmartCityById(SmartCity_Id SmartCityId);
        void RegisterExternalChargingSession(DateTime Timestamp, object Sender, ChargingSession ChargingSession);
        Task<RemoteStartResult> RemoteStart(ChargingLocation ChargingLocation, ChargingProduct ChargingProduct = null, ChargingReservation_Id? ReservationId = null, ChargingSession_Id? SessionId = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<RemoteStopResult> RemoteStop(ChargingSession_Id SessionId, ReservationHandling? ReservationHandling = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        ChargingStationOperator RemoveChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId);
        eMobilityProvider RemoveEMobilityProvider(eMobilityProvider_Id EMobilityProviderId);
        void RemoveExternalChargingSession(DateTime Timestamp, object Sender, ChargingSession ChargingSession);
        GridOperator RemoveGridOperator(GridOperator_Id GridOperatorId);
        NavigationProvider RemoveNavigationProvider(NavigationProvider_Id NavigationProviderId);
        ParkingOperator RemoveParkingOperator(ParkingOperator_Id ParkingOperatorId);
        SmartCityProxy RemoveSmartCity(SmartCity_Id SmartCityId);
        Task<ReservationResult> Reserve(ChargingLocation ChargingLocation, ChargingReservationLevel ReservationLevel = ChargingReservationLevel.EVSE, DateTime? ReservationStartTime = null, TimeSpan? Duration = null, ChargingReservation_Id? ReservationId = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication RemoteAuthentication = null, ChargingProduct ChargingProduct = null, IEnumerable<Auth_Token> AuthTokens = null, IEnumerable<eMobilityAccount_Id> eMAIds = null, IEnumerable<uint> PINs = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<SendCDRsResult> SendChargeDetailRecords(IEnumerable<ChargeDetailRecord> ChargeDetailRecords, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null);
        void SetChargingPoolAdminStatus(ChargingPool_Id ChargingPoolId, IEnumerable<Timestamped<ChargingPoolAdminStatusTypes>> StatusList);
        void SetChargingStationAdminStatus(ChargingStation_Id ChargingStationId, IEnumerable<Timestamped<ChargingStationAdminStatusTypes>> StatusList);
        void SetChargingStationAdminStatus(ChargingStation_Id ChargingStationId, Timestamped<ChargingStationAdminStatusTypes> CurrentStatus);
        void SetChargingStationStatus(ChargingStation_Id ChargingStationId, Timestamped<ChargingStationStatusTypes> CurrentStatus);
        void SetEVSEAdminStatus(EVSE_Id EVSEId, DateTime Timestamp, EVSEAdminStatusTypes NewAdminStatus);
        void SetEVSEAdminStatus(EVSE_Id EVSEId, IEnumerable<Timestamped<EVSEAdminStatusTypes>> AdminStatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetEVSEAdminStatus(EVSE_Id EVSEId, Timestamped<EVSEAdminStatusTypes> NewAdminStatus);
        void SetEVSEStatus(EVSE_Id EVSEId, DateTime Timestamp, EVSEStatusTypes NewStatus);
        void SetEVSEStatus(EVSE_Id EVSEId, IEnumerable<Timestamped<EVSEStatusTypes>> StatusList, ChangeMethods ChangeMethod = ChangeMethods.Replace);
        void SetEVSEStatus(EVSE_Id EVSEId, Timestamped<EVSEStatusTypes> NewStatus);
        void SetEVSEStatus(IEnumerable<EVSEStatus> EVSEStatusList);
        bool SetRoamingProviderPriority(IEMPRoamingProvider eMobilityRoamingService, uint Priority);
        IEnumerable<Timestamped<RoamingNetworkStatusTypes>> StatusSchedule(ulong? HistorySize = null);
        string ToString();
        bool TryGet(CSORoamingProvider_Id Id, out ICSORoamingProvider CSORoamingProvider);
        bool TryGet(EMPRoamingProvider_Id Id, out IEMPRoamingProvider EMPRoamingProvider);
        bool TryGetChargingReservationById(ChargingReservation_Id Id, out ChargingReservation ChargingReservation);
        bool TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession);

        bool TryGetEMobilityProviderById(eMobilityProvider_Id EMobilityProviderId, out eMobilityProvider EMobilityProvider);
        bool TryGetGridOperatorById(GridOperator_Id GridOperatorId, out GridOperator GridOperator);
        bool TryGetNavigationProviderById(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider);
        bool TryGetParkingOperatorById(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator);
        bool TryGetSmartCityById(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity);
        bool TryRemoveChargingStationOperator(ChargingStationOperator_Id ChargingStationOperatorId, out ChargingStationOperator ChargingStationOperator);
        bool TryRemoveEMobilityProvider(eMobilityProvider_Id EMobilityProviderId, out eMobilityProvider EMobilityProvider);
        bool TryRemoveGridOperator(GridOperator_Id GridOperatorId, out GridOperator GridOperator);
        bool TryRemoveNavigationProvider(NavigationProvider_Id NavigationProviderId, out NavigationProvider NavigationProvider);
        bool TryRemoveParkingOperator(ParkingOperator_Id ParkingOperatorId, out ParkingOperator ParkingOperator);
        bool TryRemoveSmartCity(SmartCity_Id SmartCityId, out SmartCityProxy SmartCity);
    }
}