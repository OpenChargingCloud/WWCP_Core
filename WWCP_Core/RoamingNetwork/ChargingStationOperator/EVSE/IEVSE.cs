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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for the common Electric Vehicle Supply Equipments (EVSEs) interface.
    /// </summary>
    public static class EVSEExtensions
    {

        #region ToJSON(this EVSEs, Skip = null, Take = null, Embedded = false, ...)

        /// <summary>
        /// Return a JSON representation for the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="Skip">The optional number of EVSEs to skip.</param>
        /// <param name="Take">The optional number of EVSEs to return.</param>
        /// <param name="Embedded">Whether this data is embedded into another data structure, e.g. into a charging station.</param>
        public static JArray ToJSON(this IEnumerable<IEVSE>                 EVSEs,
                                    UInt64?                                 Skip                              = null,
                                    UInt64?                                 Take                              = null,
                                    Boolean                                 Embedded                          = false,
                                    InfoStatus                              ExpandRoamingNetworkId            = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingStationOperatorId   = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingPoolId              = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandChargingStationId           = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandBrandIds                    = InfoStatus.ShowIdOnly,
                                    InfoStatus                              ExpandDataLicenses                = InfoStatus.ShowIdOnly,
                                    CustomJObjectSerializerDelegate<EVSE>?  CustomEVSESerializer              = null)


            => EVSEs is not null && EVSEs.Any()

                   ? new JArray(EVSEs.Where         (evse => evse is not null).
                                      OrderBy       (evse => evse.Id).
                                      SkipTakeFilter(Skip, Take).
                                      SafeSelect    (evse => evse.ToJSON(Embedded,
                                                                         ExpandRoamingNetworkId,
                                                                         ExpandChargingStationOperatorId,
                                                                         ExpandChargingPoolId,
                                                                         ExpandChargingStationId,
                                                                         ExpandBrandIds,
                                                                         ExpandDataLicenses,
                                                                         CustomEVSESerializer)).
                                      Where         (json => json is not null))

                   : new JArray();

        #endregion

    }


    /// <summary>
    /// The commom Electric Vehicle Supply Equipments (EVSEs) interface.
    /// </summary>
    public interface IEVSE : IEntity<EVSE_Id>,
                             IAdminStatus<EVSEAdminStatusTypes>,
                             IStatus<EVSEStatusTypes>,
                             IEquatable<IEVSE>, IComparable<IEVSE>, IComparable,
                             IEnumerable<SocketOutlet>
    {

        /// <summary>
        /// The roaming network of this EVSE.
        /// </summary>
        IRoamingNetwork?          RoamingNetwork        { get; }

        /// <summary>
        /// The charging station operator of this EVSE.
        /// </summary>
        [Optional]
        ChargingStationOperator?  Operator              { get; }

        /// <summary>
        /// The remote EVSE.
        /// </summary>
        [Optional]
        IRemoteEVSE?              RemoteEVSE            { get; }



        decimal? AverageVoltage { get; set; }
        EntityHashSet<EVSE, Brand_Id, Brand> Brands { get; }
        ReactiveSet<ChargingModes> ChargingModes { get; }
        ChargingPool? ChargingPool { get; }
        ChargingSession? ChargingSession { get; }
        IEnumerable<ChargingSession> ChargingSessions { get; }
        ChargingStation? ChargingStation { get; }
        CurrentTypes? CurrentType { get; set; }
        ReactiveSet<DataLicense> DataLicenses { get; }
        EnergyMeter? EnergyMeter { get; set; }
        EnergyMix? EnergyMix { get; set; }
        ReactiveSet<Timestamped<EnergyMix>> EnergyMixPrognoses { get; }
        Timestamped<EnergyMix>? EnergyMixRealTime { get; set; }
        bool IsFreeOfCharge { get; set; }
        DateTime? LastStatusUpdate { get; set; }
        decimal? MaxCapacity { get; set; }
        ReactiveSet<Timestamped<decimal>> MaxCapacityPrognoses { get; }
        Timestamped<decimal>? MaxCapacityRealTime { get; set; }
        decimal? MaxCurrent { get; set; }
        ReactiveSet<Timestamped<decimal>> MaxCurrentPrognoses { get; }
        Timestamped<decimal>? MaxCurrentRealTime { get; set; }
        decimal? MaxPower { get; set; }
        ReactiveSet<Timestamped<decimal>> MaxPowerPrognoses { get; }
        Timestamped<decimal>? MaxPowerRealTime { get; set; }
        TimeSpan MaxReservationDuration { get; set; }
        IEnumerable<ChargingReservation> Reservations { get; }
        ReactiveSet<SocketOutlet> SocketOutlets { get; }

        event OnEVSEAdminStatusChangedDelegate? OnAdminStatusChanged;
        event OnCancelReservationRequestDelegate? OnCancelReservationRequest;
        event OnCancelReservationResponseDelegate? OnCancelReservationResponse;
        event OnEVSEDataChangedDelegate? OnDataChanged;
        event OnNewChargeDetailRecordDelegate? OnNewChargeDetailRecord;
        event OnNewChargingSessionDelegate? OnNewChargingSession;
        event OnNewReservationDelegate? OnNewReservation;
        event OnRemoteStartRequestDelegate? OnRemoteStartRequest;
        event OnRemoteStartResponseDelegate? OnRemoteStartResponse;
        event OnRemoteStopRequestDelegate? OnRemoteStopRequest;
        event OnRemoteStopResponseDelegate? OnRemoteStopResponse;
        event OnReservationCanceledDelegate? OnReservationCanceled;
        event OnReserveRequestDelegate? OnReserveRequest;
        event OnReserveResponseDelegate? OnReserveResponse;
        event OnEVSEStatusChangedDelegate? OnStatusChanged;

        void AddCurrentType(CurrentTypes CurrentType);
        Task<CancelReservationResult> CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        int CompareTo(EVSE? EVSE);
        bool Equals(EVSE? EVSE);
        bool Equals(object? Object);
        int GetHashCode();
        Task<RemoteStartResult> RemoteStart(ChargingProduct? ChargingProduct = null, ChargingReservation_Id? ReservationId = null, ChargingSession_Id? SessionId = null, EMobilityProvider_Id? ProviderId = null, RemoteAuthentication? RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<RemoteStartResult> RemoteStart(ChargingLocation ChargingLocation, ChargingProduct? ChargingProduct = null, ChargingReservation_Id? ReservationId = null, ChargingSession_Id? SessionId = null, EMobilityProvider_Id? ProviderId = null, RemoteAuthentication? RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<RemoteStopResult> RemoteStop(ChargingSession_Id SessionId, ReservationHandling? ReservationHandling = null, EMobilityProvider_Id? ProviderId = null, RemoteAuthentication? RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<ReservationResult> Reserve(DateTime? StartTime = null, TimeSpan? Duration = null, ChargingReservation_Id? ReservationId = null, EMobilityProvider_Id? ProviderId = null, RemoteAuthentication? RemoteAuthentication = null, ChargingProduct? ChargingProduct = null, IEnumerable<Auth_Token>? AuthTokens = null, IEnumerable<eMobilityAccount_Id>? eMAIds = null, IEnumerable<uint>? PINs = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        Task<ReservationResult> Reserve(ChargingLocation ChargingLocation, ChargingReservationLevel ReservationLevel = ChargingReservationLevel.EVSE, DateTime? ReservationStartTime = null, TimeSpan? Duration = null, ChargingReservation_Id? ReservationId = null, EMobilityProvider_Id? ProviderId = null, RemoteAuthentication? RemoteAuthentication = null, ChargingProduct? ChargingProduct = null, IEnumerable<Auth_Token>? AuthTokens = null, IEnumerable<eMobilityAccount_Id>? eMAIds = null, IEnumerable<uint>? PINs = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id? EventTrackingId = null, TimeSpan? RequestTimeout = null);
        JObject ToJSON(bool Embedded = false, InfoStatus ExpandRoamingNetworkId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationOperatorId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingPoolId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationId = InfoStatus.ShowIdOnly, InfoStatus ExpandBrandIds = InfoStatus.ShowIdOnly, InfoStatus ExpandDataLicenses = InfoStatus.ShowIdOnly, CustomJObjectSerializerDelegate<EVSE>? CustomEVSESerializer = null);
        string ToString();
        bool TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation? ChargingReservation);
        bool TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession? chargingSession);
        EVSE UpdateWith(EVSE OtherEVSE);

    }

}
