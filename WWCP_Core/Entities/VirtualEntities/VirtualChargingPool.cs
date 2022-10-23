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

using System.Collections;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.Virtual
{

    /// <summary>
    /// A virtual charging pool.
    /// </summary>
    public class VirtualChargingPool : ACryptoEMobilityEntity<ChargingPool_Id,
                                                              ChargingPoolAdminStatusTypes,
                                                              ChargingPoolStatusTypes>,
                                       IEquatable<VirtualChargingPool>, IComparable<VirtualChargingPool>, IComparable,
                                       IRemoteChargingPool
    {

        #region Data

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 50;

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 50;

        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public static readonly TimeSpan MaxReservationDuration = TimeSpan.FromMinutes(15);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id OperatorId
            => Id.OperatorId;

        #endregion

        public ChargingStationOperator Operator => throw new NotImplementedException();

        public IRemoteChargingPool RemoteChargingPool => throw new NotImplementedException();

        public Address? Address => throw new NotImplementedException();

        public GeoCoordinate? GeoLocation => throw new NotImplementedException();

        public OpeningTimes OpeningTimes => throw new NotImplementedException();

        public IEnumerable<IEVSE> EVSEs => throw new NotImplementedException();

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual charging pool.
        /// </summary>
        public VirtualChargingPool(ChargingPool_Id                Id,
                                   IRoamingNetwork                RoamingNetwork,
                                   I18NString?                    Name                         = null,
                                   I18NString?                    Description                  = null,
                                   ChargingPoolAdminStatusTypes?  InitialAdminStatus           = null,
                                   ChargingPoolStatusTypes?       InitialStatus                = null,
                                   String                         EllipticCurve                = "P-256",
                                   ECPrivateKeyParameters?        PrivateKey                   = null,
                                   PublicKeyCertificates?         PublicKeyCertificates        = null,
                                   TimeSpan?                      SelfCheckTimeSpan            = null,
                                   UInt16?                        MaxAdminStatusScheduleSize   = null,
                                   UInt16?                        MaxStatusScheduleSize        = null,

                                   String?                        DataSource                   = null,
                                   DateTime?                      LastChange                   = null,

                                   JObject?                       CustomData                   = null,
                                   UserDefinedDictionary?         InternalData                 = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus         ?? ChargingPoolAdminStatusTypes.Operational,
                   InitialStatus              ?? ChargingPoolStatusTypes.Available,
                   MaxAdminStatusScheduleSize ?? DefaultMaxAdminStatusListSize,
                   MaxStatusScheduleSize      ?? DefaultMaxStatusListSize,
                   DataSource,
                   LastChange,
                   CustomData,
                   InternalData)

        {

            #region Init data and properties

            this.chargingStations     = new HashSet<IChargingStation>();

            #endregion

            #region Setup crypto

            if (PrivateKey            is null &&
                PublicKeyCertificates is null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;
                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKey:           new PublicKeyLifetime(
                                                                           PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                           NotBefore:  Timestamp.Now,
                                                                           NotAfter:   Timestamp.Now + TimeSpan.FromDays(365),
                                                                           Algorithm:  "P-256",
                                                                           Comment:    I18NString.Empty
                                                                       ),
                                                  Description:         I18NString.Create(Languages.en, "Auto-generated test keys for a virtual charging pool!"),
                                                  Operations:          JSONObject.Create(
                                                                           new JProperty("signMeterValues",  true),
                                                                           new JProperty("signCertificates",
                                                                               JSONObject.Create(
                                                                                   new JProperty("maxChilds", 2)
                                                                               ))
                                                                       ),
                                                  ChargingPoolId:      Id);

            }

            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this.statusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

        }

        #endregion


        #region EVSEs...

        #region TryGetEVSEById(EVSEId, out RemoteEVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IEVSE? RemoteEVSE)
        {

            foreach (var station in chargingStations)
            {
                if (station is not null &&
                    station.TryGetEVSEById(EVSEId, out RemoteEVSE))
                {
                    return true;
                }
            }

            RemoteEVSE = null;
            return false;

        }

        #endregion

        #endregion

        #region Charging stations...

        #region Stations

        private readonly HashSet<IChargingStation> chargingStations;

        /// <summary>
        /// All registered charging stations.
        /// </summary>
        public IEnumerable<IChargingStation> ChargingStations
            => chargingStations;

        #endregion

        #region CreateVirtualStation(ChargingStationId, ..., Configurator = null, OnSuccess = null, OnError = null, ...)

        /// <summary>
        /// Create and register a new charging station having the given
        /// unique charging station identification.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the new charging station.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging station after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the charging station.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public VirtualChargingStation CreateVirtualStation(ChargingStation_Id                                   ChargingStationId,
                                                           I18NString?                                          Name                         = null,
                                                           I18NString?                                          Description                  = null,
                                                           ChargingStationAdminStatusTypes?                     InitialAdminStatus           = null,
                                                           ChargingStationStatusTypes?                          InitialStatus                = null,
                                                           String?                                              EllipticCurve                = null,
                                                           ECPrivateKeyParameters?                              PrivateKey                   = null,
                                                           PublicKeyCertificates?                               PublicKeyCertificates        = null,
                                                           TimeSpan?                                            SelfCheckTimeSpan            = null,
                                                           Action<VirtualChargingStation>?                      Configurator                 = null,
                                                           Action<VirtualChargingStation>?                      OnSuccess                    = null,
                                                           Action<VirtualChargingStation, ChargingStation_Id>?  OnError                      = null,
                                                           UInt16?                                              MaxAdminStatusScheduleSize   = null,
                                                           UInt16?                                              MaxStatusScheduleSize        = null)
        {

            #region Initial checks

            if (chargingStations.Any(station => station.Id == ChargingStationId))
            {
                throw new Exception("StationAlreadyExistsInPool");
                //if (OnError == null)
                //    throw new ChargingStationAlreadyExistsInStation(this.ChargingStation, ChargingStation.Id);
                //else
                //    OnError?.Invoke(this, ChargingStation.Id);
            }

            #endregion

            var Now              = Timestamp.Now;
            var _VirtualStation  = new VirtualChargingStation(ChargingStationId,
                                                              RoamingNetwork,
                                                              Name,
                                                              Description,
                                                              InitialAdminStatus ?? ChargingStationAdminStatusTypes.Operational,
                                                              InitialStatus      ?? ChargingStationStatusTypes.     Available,
                                                              EllipticCurve,
                                                              PrivateKey,
                                                              PublicKeyCertificates,
                                                              SelfCheckTimeSpan,
                                                              MaxAdminStatusScheduleSize,
                                                              MaxStatusScheduleSize);

            Configurator?.Invoke(_VirtualStation);

            if (chargingStations.Add(_VirtualStation))
            {

                //_VirtualEVSE.OnPropertyChanged        += (Timestamp, Sender, PropertyName, OldValue, NewValue)
                //                                           => UpdateEVSEData(Timestamp, Sender as VirtualEVSE, PropertyName, OldValue, NewValue);
                //
                //_VirtualEVSE.OnStatusChanged          += UpdateEVSEStatus;
                //_VirtualEVSE.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                //_VirtualEVSE.OnNewReservation         += SendNewReservation;
                //_VirtualEVSE.OnNewChargingSession     += SendNewChargingSession;
                //_VirtualEVSE.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;

                OnSuccess?.Invoke(_VirtualStation);

                return _VirtualStation;

            }

            return null;

        }

        #endregion


        #region ContainsChargingStationId(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the charging station.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of an ChargingStation.</param>
        public Boolean ContainsChargingStationId(ChargingStation_Id ChargingStationId)
            => chargingStations.Any(evse => evse.Id == ChargingStationId);

        #endregion

        #region GetChargingStationById(ChargingStationId)

        public IChargingStation? GetChargingStationById(ChargingStation_Id ChargingStationId)
            => chargingStations.FirstOrDefault(evse => evse.Id == ChargingStationId);

        #endregion

        #region TryGetChargingStationById(ChargingStationId, out ChargingStation)

        public Boolean TryGetChargingStationById(ChargingStation_Id ChargingStationId, out IChargingStation? ChargingStation)
        {

            ChargingStation = GetChargingStationById(ChargingStationId);

            return ChargingStation is not null;

        }

        #endregion


        #region TryGetChargingStationByEVSEId(EVSEId, out RemoteChargingStation)

        public Boolean TryGetChargingStationByEVSEId(EVSE_Id EVSEId, out IChargingStation? RemoteChargingStation)
        {

            foreach (var station in chargingStations)
            {

                if (station.TryGetEVSEById(EVSEId, out var RemoteEVSE))
                {
                    RemoteChargingStation = station;
                    return true;
                }

            }

            RemoteChargingStation = null;
            return false;

        }

        #endregion

        #endregion


        #region (Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolAdminStatusChangedDelegate  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingPoolStatusChangedDelegate       OnStatusChanged;

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                                   Timestamp,
                                              EventTracking_Id                           EventTrackingId,
                                              Timestamped<ChargingPoolAdminStatusTypes>  OldStatus,
                                              Timestamped<ChargingPoolAdminStatusTypes>  NewStatus)
        {

            OnAdminStatusChanged?.Invoke(Timestamp,
                                         EventTrackingId,
                                         this,
                                         OldStatus,
                                         NewStatus);

        }

        #endregion

        #region (internal) UpdateStatus     (Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                              Timestamp,
                                         EventTracking_Id                      EventTrackingId,
                                         Timestamped<ChargingPoolStatusTypes>  OldStatus,
                                         Timestamped<ChargingPoolStatusTypes>  NewStatus)
        {

            OnStatusChanged?.Invoke(Timestamp,
                                    EventTrackingId,
                                    this,
                                    OldStatus,
                                    NewStatus);

        }

        #endregion

        #endregion


        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => _Reservations.Select(_ => _.Value);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
            => _Reservations.TryGetValue(ReservationId, out Reservation);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate        OnReservationCanceled;

        #endregion

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this charging pool.
        /// </summary>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<ReservationResult>

            Reserve(DateTime?                         StartTime              = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    EMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)


                => Reserve(ChargingLocation.FromChargingPoolId(Id),
                           ChargingReservationLevel.ChargingPool,
                           StartTime,
                           Duration,
                           ReservationId,
                           ProviderId,
                           RemoteAuthentication,
                           ChargingProduct,
                           AuthTokens,
                           eMAIds,
                           PINs,

                           Timestamp,
                           CancellationToken,
                           EventTrackingId,
                           RequestTimeout);

        #endregion

        #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given charging location.
        /// </summary>
        /// <param name="ChargingLocation">A charging location.</param>
        /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
        /// <param name="ReservationStartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProduct">The charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(ChargingLocation                  ChargingLocation,
                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                         ReservationStartTime   = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    EMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation newReservation  = null;
            ReservationResult   result          = null;

            #endregion

            #region Send OnReserveRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(StartTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ReservationId,
                                         ChargingLocation,
                                         ReservationStartTime,
                                         Duration,
                                         ProviderId,
                                         RemoteAuthentication,
                                         ChargingProduct,
                                         AuthTokens,
                                         eMAIds,
                                         PINs,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnReserveRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingPoolId.HasValue && ChargingLocation.ChargingPoolId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (ReservationLevel == ChargingReservationLevel.EVSE                                   &&
                        ChargingLocation.EVSEId.HasValue                                                    &&
                        TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.Value, out var remoteStation) &&
                        remoteStation is not null)
                    {

                        result = await remoteStation.
                                           Reserve(ChargingLocation,
                                                   ReservationLevel,
                                                   ReservationStartTime,
                                                   Duration,
                                                   ReservationId,
                                                   ProviderId,
                                                   RemoteAuthentication,
                                                   ChargingProduct,
                                                   AuthTokens,
                                                   eMAIds,
                                                   PINs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

                        newReservation = result.Reservation;

                    }

                    else if (ReservationLevel == ChargingReservationLevel.ChargingPool &&
                             ChargingLocation.ChargingPoolId.HasValue)
                    {

                        var results = new List<ReservationResult>();

                        foreach (var remoteStation2 in chargingStations)
                        {

                            results.Add(await remoteStation2.
                                                  Reserve(ChargingLocation,
                                                          ReservationLevel,
                                                          ReservationStartTime,
                                                          Duration,
                                                          ChargingReservation_Id.Random(OperatorId),
                                                          ProviderId,
                                                          RemoteAuthentication,
                                                          ChargingProduct,
                                                          AuthTokens,
                                                          eMAIds,
                                                          PINs,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout));

                        }

                        var newReservations = results.Where (_result => _result.Result == ReservationResultType.Success).
                                                      Select(_result => _result.Reservation).
                                                      ToArray();

                        if (newReservations.Length > 0)
                        {

                            newReservation = new ChargingReservation(Id:                      ReservationId ?? ChargingReservation_Id.Random(OperatorId),
                                                                     Timestamp:               Timestamp.Value,
                                                                     StartTime:               ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                     Duration:                Duration  ?? MaxReservationDuration,
                                                                     EndTime:                 (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                                     ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                                     ReservationLevel:        ReservationLevel,
                                                                     ProviderId:              ProviderId,
                                                                     StartAuthentication:     RemoteAuthentication,
                                                                     RoamingNetworkId:        RoamingNetwork.Id,
                                                                     ChargingPoolId:          Id,
                                                                     ChargingStationId:       null,
                                                                     EVSEId:                  null,
                                                                     ChargingProduct:         ChargingProduct,
                                                                     SubReservations:         newReservations);

                            foreach (var subReservation in newReservation.SubReservations)
                            {
                                subReservation.ParentReservation = newReservation;
                                subReservation.ChargingPoolId    = Id;
                            }

                            result = ReservationResult.Success(newReservation);

                        }

                        else
                            result = ReservationResult.AlreadyReserved;

                    }

                    else
                        result = ReservationResult.UnknownLocation;

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = ReservationResult.OutOfService;
                            break;

                    }

                }


                if (result.Result == ReservationResultType.Success &&
                    newReservation != null)
                {

                    _Reservations.Add(newReservation.Id, newReservation);

                    foreach (var subReservation in newReservation.SubReservations)
                        _Reservations.Add(subReservation.Id, subReservation);

                    OnNewReservation?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                             this,
                                             newReservation);

                }

            }
            catch (Exception e)
            {
                result = ReservationResult.Error(e.Message);
            }


            #region Send OnReserveResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveResponse?.Invoke(EndTime,
                                          Timestamp.Value,
                                          this,
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ReservationId,
                                          ChargingLocation,
                                          StartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          EndTime - StartTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CancelReservation(ReservationId, Reason, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            ChargingReservation     canceledReservation  = null;
            CancelReservationResult result               = null;

            #endregion

            #region Send OnCancelReservationRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   ReservationId,
                                                   Reason,
                                                   RequestTimeout);


            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    var _Reservation = ChargingReservations.FirstOrDefault(reservation => reservation.Id == ReservationId);

                    if (_Reservation != null &&
                        _Reservation.ChargingStationId.HasValue)
                    {

                        result = await GetChargingStationById(_Reservation.ChargingStationId.Value).
                                           CancelReservation(ReservationId,
                                                             Reason,

                                                             Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             RequestTimeout);

                        if (result.Result != CancelReservationResultTypes.UnknownReservationId)
                            return result;

                    }

                    foreach (var chargingStation in chargingStations)
                    {

                        result = await chargingStation.CancelReservation(ReservationId,
                                                                         Reason,

                                                                         Timestamp,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         RequestTimeout);

                        if (result.Result != CancelReservationResultTypes.UnknownReservationId)
                            break;

                    }

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = CancelReservationResult.OutOfService(ReservationId,
                                                                          Reason);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = CancelReservationResult.Error(ReservationId,
                                                       Reason,
                                                       e.Message);
            }


            #region Send OnCancelReservationResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnCancelReservationResponse?.Invoke(EndTime,
                                                    Timestamp.Value,
                                                    this,
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    ReservationId,
                                                    canceledReservation,
                                                    Reason,
                                                    result,
                                                    EndTime - StartTime,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

        internal void SendNewReservation(DateTime             Timestamp,
                                         Object               Sender,
                                         ChargingReservation  Reservation)
        {

            OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

        }

        #endregion

        #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

        internal void SendReservationCanceled(DateTime                               Timestamp,
                                              Object                                 Sender,
                                              ChargingReservation                    Reservation,
                                              ChargingReservationCancellationReason  Reason)
        {

            OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions

        #region Data

        private readonly Dictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

        public IEnumerable<ChargingSession> ChargingSessions
            => _ChargingSessions.Select(_ => _.Value);

        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
            => _ChargingSessions.TryGetValue(SessionId, out ChargingSession);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;


        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

        #endregion

        #region RemoteStart(                  ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session.
        /// </summary>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<RemoteStartResult>

            RemoteStart(ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromChargingPoolId(Id),
                               ChargingProduct,
                               ReservationId,
                               SessionId,
                               ProviderId,
                               RemoteAuthentication,

                               Timestamp,
                               CancellationToken,
                               EventTrackingId,
                               RequestTimeout);

        #endregion

        #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">The choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct?         ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        EMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication?    RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id?        EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)
        {

            #region Initial checks

            if (SessionId == null)
                SessionId = ChargingSession_Id.NewRandom;

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            RemoteStartResult? result = null;

            #endregion

            #region Send OnRemoteStartRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartRequest?.Invoke(startTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             ChargingLocation,
                                             ChargingProduct,
                                             ReservationId,
                                             SessionId,
                                             null,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.ChargingPoolId.HasValue &&
                    ChargingLocation.ChargingPoolId.Value != Id)
                {
                    result = RemoteStartResult.UnknownLocation();
                }

                else if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                         AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (!ChargingLocation.EVSEId.HasValue)
                        result = RemoteStartResult.UnknownLocation();

                    else if (!TryGetChargingStationByEVSEId(ChargingLocation.EVSEId.Value, out var remoteStation))
                        result = RemoteStartResult.UnknownLocation();

                    else if (remoteStation is not null)
                        result = await remoteStation.
                                           RemoteStart(ChargingProduct,
                                                       ReservationId,
                                                       SessionId,
                                                       ProviderId,
                                                       RemoteAuthentication,

                                                       Timestamp,
                                                       CancellationToken,
                                                       EventTrackingId,
                                                       RequestTimeout);

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStartResult.OutOfService();
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }

            result ??= RemoteStartResult.Error("unknown");


            #region Send OnRemoteStartResponse event

            var endTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStartResponse?.Invoke(endTime,
                                              Timestamp.Value,
                                              this,
                                              EventTrackingId,
                                              RoamingNetwork.Id,
                                              ChargingLocation,
                                              ChargingProduct,
                                              ReservationId,
                                              SessionId,
                                              null,
                                              null,
                                              ProviderId,
                                              RemoteAuthentication,
                                              RequestTimeout,
                                              result,
                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

        /// <summary>
        /// Stop the given charging session.
        /// </summary>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       EMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication?  RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id?      EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            RemoteStopResult? result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(startTime,
                                            Timestamp.Value,
                                            this,
                                            EventTrackingId,
                                            RoamingNetwork.Id,
                                            SessionId,
                                            ReservationHandling,
                                            null,
                                            null,
                                            ProviderId,
                                            RemoteAuthentication,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == ChargingPoolAdminStatusTypes.Operational ||
                    AdminStatus.Value == ChargingPoolAdminStatusTypes.InternalUse)
                {

                    if (!TryGetChargingSessionById(SessionId, out var chargingSession))
                    {

                        foreach (var remoteStation in chargingStations)
                        {

                            result = await remoteStation.
                                               RemoteStop(SessionId,
                                                          ReservationHandling,
                                                          ProviderId,
                                                          RemoteAuthentication,

                                                          Timestamp,
                                                          CancellationToken,
                                                          EventTrackingId,
                                                          RequestTimeout);

                            if (result?.Result == RemoteStopResultTypes.Success)
                                break;

                        }

                        if (result?.Result != RemoteStopResultTypes.Success)
                            result = RemoteStopResult.InvalidSessionId(SessionId);

                    }

                    else if (chargingSession.ChargingStation                       is not null &&
                             chargingSession.ChargingStation.RemoteChargingStation is not null)
                    {

                        result = await chargingSession.ChargingStation.RemoteChargingStation.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                    }

                    else if (chargingSession.ChargingStationId.HasValue &&
                             TryGetChargingStationById(chargingSession.ChargingStationId.Value, out var remoteStation))
                    {

                        result = await remoteStation.
                                           RemoteStop(SessionId,
                                                      ReservationHandling,
                                                      ProviderId,
                                                      RemoteAuthentication,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

                    }

                    result = RemoteStopResult.UnknownLocation(SessionId);

                }
                else
                {

                    switch (AdminStatus.Value)
                    {

                        default:
                            result = RemoteStopResult.OutOfService(SessionId);
                            break;

                    }

                }

            }
            catch (Exception e)
            {
                result = RemoteStopResult.Error(SessionId,
                                                e.Message);
            }


            #region Send OnRemoteStopResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopResponse?.Invoke(EndTime,
                                             Timestamp.Value,
                                             this,
                                             EventTrackingId,
                                             RoamingNetwork.Id,
                                             SessionId,
                                             ReservationHandling,
                                             null,
                                             null,
                                             ProviderId,
                                             RemoteAuthentication,
                                             RequestTimeout,
                                             result,
                                             EndTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualChargingPool) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session != null)
            {

                if (Session.ChargingPool == null)
                    Session.ChargingPoolId  = Id;

                OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

            }

        }

        #endregion

        #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

        internal void SendNewChargeDetailRecord(DateTime            Timestamp,
                                                Object              Sender,
                                                ChargeDetailRecord  ChargeDetailRecord)
        {

            if (ChargeDetailRecord != null)
                OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(VirtualChargingPool1, VirtualChargingPool2))
                return true;

            // If one is null, but not both, return false.
            if ((VirtualChargingPool1 is null) || (VirtualChargingPool2 is null))
                return false;

            return VirtualChargingPool1.Equals(VirtualChargingPool2);

        }

        #endregion

        #region Operator != (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
            => !(VirtualChargingPool1 == VirtualChargingPool2);

        #endregion

        #region Operator <  (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
        {

            if (VirtualChargingPool1 is null)
                throw new ArgumentNullException(nameof(VirtualChargingPool1), "The given VirtualChargingPool1 must not be null!");

            return VirtualChargingPool1.CompareTo(VirtualChargingPool2) < 0;

        }

        #endregion

        #region Operator <= (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
            => !(VirtualChargingPool1 > VirtualChargingPool2);

        #endregion

        #region Operator >  (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
        {

            if (VirtualChargingPool1 is null)
                throw new ArgumentNullException(nameof(VirtualChargingPool1), "The given VirtualChargingPool1 must not be null!");

            return VirtualChargingPool1.CompareTo(VirtualChargingPool2) > 0;

        }

        #endregion

        #region Operator >= (VirtualChargingPool1, VirtualChargingPool2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool1">A virtual charging pool.</param>
        /// <param name="VirtualChargingPool2">Another virtual charging pool.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (VirtualChargingPool VirtualChargingPool1, VirtualChargingPool VirtualChargingPool2)
            => !(VirtualChargingPool1 < VirtualChargingPool2);

        #endregion

        #endregion

        #region IComparable<VirtualChargingPool> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is VirtualChargingPool VirtualChargingPool))
                throw new ArgumentException("The given object is not a virtual charging station!");

            return CompareTo(VirtualChargingPool);

        }

        #endregion

        #region CompareTo(VirtualChargingPool)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualChargingPool">An virtual charging station to compare with.</param>
        public Int32 CompareTo(VirtualChargingPool VirtualChargingPool)
        {

            if (VirtualChargingPool is null)
                throw new ArgumentNullException(nameof(VirtualChargingPool),  "The given virtual charging station must not be null!");

            return Id.CompareTo(VirtualChargingPool.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualChargingPool> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            if (!(Object is VirtualChargingPool VirtualChargingPool))
                return false;

            return Equals(VirtualChargingPool);

        }

        #endregion

        #region Equals(VirtualChargingPool)

        /// <summary>
        /// Compares two virtual charging stations for equality.
        /// </summary>
        /// <param name="VirtualChargingPool">A virtual charging station to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualChargingPool VirtualChargingPool)
        {

            if (VirtualChargingPool is null)
                return false;

            return Id.Equals(VirtualChargingPool.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Id.ToString();

        public JObject ToJSON(Boolean Embedded = false, InfoStatus ExpandRoamingNetworkId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationOperatorId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationIds = InfoStatus.Expanded, InfoStatus ExpandEVSEIds = InfoStatus.Hidden, InfoStatus ExpandBrandIds = InfoStatus.ShowIdOnly, InfoStatus ExpandDataLicenses = InfoStatus.ShowIdOnly, CustomJObjectSerializerDelegate<ChargingPool>? CustomChargingPoolSerializer = null, CustomJObjectSerializerDelegate<ChargingStation>? CustomChargingStationSerializer = null, CustomJObjectSerializerDelegate<EVSE>? CustomEVSESerializer = null)
        {
            throw new NotImplementedException();
        }

        public Boolean Equals(IChargingPool? other)
        {
            throw new NotImplementedException();
        }

        public Int32 CompareTo(IChargingPool? other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ChargingStation> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion


    }

}
