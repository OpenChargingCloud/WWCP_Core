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

using System.Collections.Concurrent;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.Virtual
{

    /// <summary>
    /// Extension methods for virtual EVSEs.
    /// </summary>
    public static class VirtualEVSEExtensions
    {

        #region CreateVirtualEVSE(this ChargingStation, EVSEId = null, EVSEConfigurator = null, VirtualEVSEConfigurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create a new virtual charging station.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="EVSEId">The EVSE identification for the EVSE to be created.</param>
        /// <param name="EVSEConfigurator">An optional delegate to configure the new (local) EVSE.</param>
        /// <param name="VirtualEVSEConfigurator">An optional delegate to configure the new EVSE.</param>
        /// <param name="OnSuccess">An optional delegate for reporting success.</param>
        /// <param name="OnError">An optional delegate for reporting an error.</param>
        public static IEVSE? CreateVirtualEVSE(this IChargingStation               ChargingStation,
                                               EVSE_Id                             EVSEId,
                                               I18NString?                         Name                      = null,
                                               I18NString?                         Description               = null,
                                               EVSEAdminStatusTypes?               InitialAdminStatus        = null,
                                               EVSEStatusTypes?                    InitialStatus             = null,
                                               EnergyMeter_Id?                     EnergyMeterId             = null,
                                               String                              EllipticCurve             = "P-256",
                                               ECPrivateKeyParameters?             PrivateKey                = null,
                                               PublicKeyCertificates?              PublicKeyCertificates     = null,
                                               TimeSpan?                           SelfCheckTimeSpan         = null,
                                               UInt16                              MaxAdminStatusListSize    = VirtualEVSE.DefaultMaxAdminStatusListSize,
                                               UInt16                              MaxStatusListSize         = VirtualEVSE.DefaultMaxStatusListSize,
                                               Action<IEVSE>?                      EVSEConfigurator          = null,
                                               Action<VirtualEVSE>?                VirtualEVSEConfigurator   = null,
                                               Action<IEVSE>?                      OnSuccess                 = null,
                                               Action<IChargingStation, EVSE_Id>?  OnError                   = null)
        {

            #region Initial checks

            if (ChargingStation is null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return ChargingStation.CreateEVSE(EVSEId,
                                              Name,
                                              Description,
                                              EVSEConfigurator,
                                              newEVSE => {

                                                  var virtualevse = new VirtualEVSE(newEVSE.Id,
                                                                                    ChargingStation.RoamingNetwork,
                                                                                    newEVSE.Name,
                                                                                    newEVSE.Description,
                                                                                    InitialAdminStatus ?? EVSEAdminStatusTypes.Operational,
                                                                                    InitialStatus      ?? EVSEStatusTypes.Available,
                                                                                    EnergyMeterId,
                                                                                    EllipticCurve,
                                                                                    PrivateKey,
                                                                                    PublicKeyCertificates,
                                                                                    SelfCheckTimeSpan,
                                                                                    MaxAdminStatusListSize,
                                                                                    MaxStatusListSize);

                                                  VirtualEVSEConfigurator?.Invoke(virtualevse);

                                                  return virtualevse;

                                              },

                                              OnSuccess: OnSuccess,
                                              OnError:   OnError);

        }

        #endregion

    }


    /// <summary>
    /// A virtual EVSE.
    /// </summary>
    public class VirtualEVSE : ACryptoEMobilityEntity<EVSE_Id,
                                                      EVSEAdminStatusTypes,
                                                      EVSEStatusTypes>,
                               IEquatable<VirtualEVSE>, IComparable<VirtualEVSE>, IComparable,
                               IEnumerable<SocketOutlet>,
                               IRemoteEVSE
    {

        #region Data

        /// <summary>
        /// The default max size of the admin status history.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 50;

        /// <summary>
        /// The default max size of the status history.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 50;


        /// <summary>
        /// The maximum time span for a reservation.
        /// </summary>
        public  static readonly TimeSpan  MaxReservationDuration    = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The default time span between self checks.
        /// </summary>
        public  static readonly TimeSpan  DefaultSelfCheckTimeSpan  = TimeSpan.FromSeconds(15);

        private static readonly Random    _random                   = new Random();

        private        readonly Object    EnergyMeterLock;
        private                 Timer     EnergyMeterTimer;

        private        readonly Object    ReservationExpiredLock;
        private        readonly Timer     ReservationExpiredTimer;

        #endregion

        #region Properties

        /// <summary>
        /// The identification of the operator of this virtual EVSE.
        /// </summary>
        [InternalUseOnly]
        public ChargingStationOperator_Id OperatorId
            => Id.OperatorId;

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An description of this EVSE.
        /// </summary>
        [Mandatory]
        public I18NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                    SetProperty(ref _Description, value);
            }

        }

        #endregion


        #region ChargingModes

        private ReactiveSet<ChargingModes> _ChargingModes;

        /// <summary>
        /// Charging modes.
        /// </summary>
        [Mandatory]
        public ReactiveSet<ChargingModes> ChargingModes
        {

            get
            {
                return _ChargingModes;
            }

            set
            {

                if (_ChargingModes != value)
                    SetProperty(ref _ChargingModes, value);

            }

        }

        #endregion

        #region AverageVoltage

        private Double _AverageVoltage;

        /// <summary>
        /// The average voltage.
        /// </summary>
        [Mandatory]
        public Double AverageVoltage
        {

            get
            {
                return _AverageVoltage;
            }

            set
            {

                if (_AverageVoltage != value)
                    SetProperty(ref _AverageVoltage, value);

            }

        }

        #endregion

        #region CurrentType

        private CurrentTypes _CurrentType;

        /// <summary>
        /// The type of the current.
        /// </summary>
        [Mandatory]
        public CurrentTypes CurrentType
        {

            get
            {
                return _CurrentType;
            }

            set
            {

                if (_CurrentType != value)
                    SetProperty(ref _CurrentType, value);

            }

        }

        #endregion

        #region MaxCurrent

        private Double _MaxCurrent;

        /// <summary>
        /// The maximum current [Ampere].
        /// </summary>
        [Mandatory]
        public Double MaxCurrent
        {

            get
            {
                return _MaxCurrent;
            }

            set
            {

                if (_MaxCurrent != value)
                    SetProperty(ref _MaxCurrent, value);

            }

        }

        #endregion

        #region MaxPower

        private Double _MaxPower;

        /// <summary>
        /// The maximum power [kWatt].
        /// </summary>
        [Mandatory]
        public Double MaxPower
        {

            get
            {
                return _MaxPower;
            }

            set
            {

                if (_MaxPower != value)
                    SetProperty(ref _MaxPower, value);

            }

        }

        #endregion

        #region RealTimePower

        private Double _RealTimePower;

        /// <summary>
        /// The current real-time power delivery [Watt].
        /// </summary>
        [Mandatory]
        public Double RealTimePower
        {

            get
            {
                return _RealTimePower;
            }

            set
            {

                if (_RealTimePower != value)
                    SetProperty(ref _RealTimePower, value);

            }

        }

        #endregion

        #region MaxCapacity

        private Double? _MaxCapacity;

        /// <summary>
        /// The maximum capacity [kWh].
        /// </summary>
        [Mandatory]
        public Double? MaxCapacity
        {

            get
            {
                return _MaxCapacity;
            }

            set
            {

                if (_MaxCapacity != value)
                    SetProperty(ref _MaxCapacity, value);

            }

        }

        #endregion


        #region SocketOutlets

        private ReactiveSet<SocketOutlet> _SocketOutlets;

        public ReactiveSet<SocketOutlet> SocketOutlets
        {

            get
            {
                return _SocketOutlets;
            }

            set
            {

                if (_SocketOutlets != value)
                    SetProperty(ref _SocketOutlets, value);

            }

        }

        #endregion


        /// <summary>
        /// The time span between self checks.
        /// </summary>
        public TimeSpan SelfCheckTimeSpan { get; }


        #region EnergyMeterId

        private EnergyMeter_Id? _EnergyMeterId;

        /// <summary>
        /// The energy meter identification.
        /// </summary>
        [Optional]
        public EnergyMeter_Id? EnergyMeterId
        {

            get
            {
                return _EnergyMeterId;
            }

            set
            {

                if (value != null)
                    SetProperty(ref _EnergyMeterId, value);

                else
                    DeleteProperty(ref _EnergyMeterId);

            }

        }

        #endregion

        public TimeSpan                EnergyMeterInterval      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new virtual EVSE.
        /// </summary>
        /// <param name="Id">The unique identification of this EVSE.</param>
        /// <param name="MaxAdminStatusListSize">The maximum size of the EVSE admin status list.</param>
        /// <param name="MaxStatusListSize">The maximum size of the EVSE status list.</param>
        internal VirtualEVSE(EVSE_Id                  Id,
                             IRoamingNetwork          RoamingNetwork,
                             I18NString?              Name                     = null,
                             I18NString?              Description              = null,
                             EVSEAdminStatusTypes?    InitialAdminStatus       = null,
                             EVSEStatusTypes?         InitialStatus            = null,
                             EnergyMeter_Id?          EnergyMeterId            = null,
                             String?                  EllipticCurve            = null,
                             ECPrivateKeyParameters?  PrivateKey               = null,
                             PublicKeyCertificates?   PublicKeyCertificates    = null,
                             TimeSpan?                SelfCheckTimeSpan        = null,
                             UInt16?                  MaxAdminStatusListSize   = null,
                             UInt16?                  MaxStatusListSize        = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   InitialAdminStatus     ?? EVSEAdminStatusTypes.Operational,
                   InitialStatus          ?? EVSEStatusTypes.Available,
                   MaxAdminStatusListSize ?? DefaultMaxAdminStatusListSize,
                   MaxStatusListSize      ?? DefaultMaxStatusListSize)

        {

            #region Init data and properties

            this._Description           = Description ?? I18NString.Empty;
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this.EnergyMeterId          = EnergyMeterId;

            this.SelfCheckTimeSpan      = SelfCheckTimeSpan != null && SelfCheckTimeSpan.HasValue ? SelfCheckTimeSpan.Value : DefaultSelfCheckTimeSpan;

            this.reservations          = new Dictionary<ChargingReservation_Id, ChargingReservation>();

            #endregion

            #region Setup crypto

            if (PrivateKey == null && PublicKeyCertificates == null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;
                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKeys:      new PublicKeyLifetime[] {
                                                                       new PublicKeyLifetime(
                                                                           PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                           NotBefore:  Timestamp.Now,
                                                                           NotAfter:   Timestamp.Now + TimeSpan.FromDays(365),
                                                                           Algorithm:  "P-256",
                                                                           Comment:    I18NString.Empty
                                                                       )
                                                                   },
                                                  Description:     I18NString.Create(Languages.en, "Auto-generated test keys for a virtual EVSE!"),
                                                  Operations:      JSONObject.Create(
                                                                       new JProperty("signMeterValues",  true),
                                                                       new JProperty("signCertificates", false)
                                                                   ),
                                                  EVSEId:          Id,
                                                  EnergyMeterId:   EnergyMeterId);

            }

            #endregion

            #region Link events

            this.adminStatusSchedule.OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            this.statusSchedule.     OnStatusChanged += (Timestamp, EventTrackingId, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus);

            #endregion

            ReservationExpiredLock   = new Object();
            ReservationExpiredTimer  = new Timer(CheckIfReservationIsExpired, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            EnergyMeterLock          = new Object();
            EnergyMeterTimer         = new Timer(ReadEnergyMeter,             null, Timeout.Infinite,        Timeout.Infinite);
            EnergyMeterInterval      = TimeSpan.FromSeconds(30);

        }

        //event OnEVSEAdminStatusChangedDelegate? IEVSE.OnAdminStatusChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //event OnEVSEDataChangedDelegate? IEVSE.OnDataChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //event OnEVSEStatusChangedDelegate? IEVSE.OnStatusChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        #endregion


        #region (private, Timer) ReadEnergyMeter(Status)

        private void ReadEnergyMeter(Object Status)
        {

            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            if (Monitor.TryEnter(EnergyMeterLock))
            {

                try
                {

                    ChargingSession.AddEnergyMeterValue(new Timestamped<Decimal>(Timestamp.Now, 1));

                }
                catch (Exception e)
                {
                    DebugX.LogT("'ReadEnergyMeter' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);
                }

                finally
                {
                    Monitor.Exit(EnergyMeterLock);
                }

            }

            else
                DebugX.LogT("'ReadEnergyMeter' skipped!");

        }

        #endregion



        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the EVSE changed.
        /// </summary>
        public event OnEVSEDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the EVSE changed.
        /// </summary>
        public event OnEVSEStatusChangedDelegate?       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of the EVSE changed.
        /// </summary>
        public event OnEVSEAdminStatusChangedDelegate?  OnAdminStatusChanged;

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal async Task UpdateAdminStatus(DateTime                           Timestamp,
                                              EventTracking_Id                   EventTrackingId,
                                              Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                              Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var OnAdminStatusChangedLocal = OnAdminStatusChanged;
            if (OnAdminStatusChangedLocal is not null)
                await OnAdminStatusChangedLocal(Timestamp,
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
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateStatus(DateTime                      Timestamp,
                                         EventTracking_Id              EventTrackingId,
                                         Timestamped<EVSEStatusTypes>  OldStatus,
                                         Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnStatusChangedLocal = OnStatusChanged;
            if (OnStatusChangedLocal is not null)
                await OnStatusChangedLocal(Timestamp,
                                           EventTrackingId,
                                           this,
                                           OldStatus,
                                           NewStatus);

        }

        #endregion

        #endregion

        #region Reservations...

        #region Data

        private readonly Dictionary<ChargingReservation_Id, ChargingReservation> reservations;

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> ChargingReservations
            => reservations.Select(_ => _.Value);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
            => reservations.TryGetValue(ReservationId, out Reservation);

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a charging location is being reserved.
        /// </summary>
        public event OnReserveRequestDelegate?             OnReserveRequest;

        /// <summary>
        /// An event fired whenever a charging location was reserved.
        /// </summary>
        public event OnReserveResponseDelegate?            OnReserveResponse;

        /// <summary>
        /// An event fired whenever a new charging reservation was created.
        /// </summary>
        public event OnNewReservationDelegate?             OnNewReservation;


        /// <summary>
        /// An event fired whenever a charging reservation is being canceled.
        /// </summary>
        public event OnCancelReservationRequestDelegate?   OnCancelReservationRequest;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnCancelReservationResponseDelegate?  OnCancelReservationResponse;

        /// <summary>
        /// An event fired whenever a charging reservation was canceled.
        /// </summary>
        public event OnReservationCanceledDelegate?        OnReservationCanceled;

        #endregion

        #region Reserve(                                           StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at this EVSE.
        /// </summary>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
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

            Reserve(DateTime?                          StartTime              = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken?                 CancellationToken      = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)


                => Reserve(ChargingLocation.FromEVSEId(Id),
                           ChargingReservationLevel.EVSE,
                           StartTime,
                           Duration,
                           ReservationId,
                           LinkedReservationId,
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
        /// <param name="LinkedReservationId">An existing linked charging reservation identification.</param>
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

            Reserve(ChargingLocation                   ChargingLocation,
                    ChargingReservationLevel           ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                          ReservationStartTime   = null,
                    TimeSpan?                          Duration               = null,
                    ChargingReservation_Id?            ReservationId          = null,
                    ChargingReservation_Id?            LinkedReservationId    = null,
                    EMobilityProvider_Id?              ProviderId             = null,
                    RemoteAuthentication?              RemoteAuthentication   = null,
                    ChargingProduct?                   ChargingProduct        = null,
                    IEnumerable<AuthenticationToken>?           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>?  eMAIds                 = null,
                    IEnumerable<UInt32>?               PINs                   = null,

                    DateTime?                          Timestamp              = null,
                    CancellationToken?                 CancellationToken      = null,
                    EventTracking_Id?                  EventTrackingId        = null,
                    TimeSpan?                          RequestTimeout         = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            EventTrackingId ??= EventTracking_Id.New;


            ChargingReservation? newReservation   = null;
            ReservationResult?   result           = null;

            #endregion

            #region Send OnReserveRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnReserveRequest?.Invoke(startTime,
                                         Timestamp.Value,
                                         this,
                                         EventTrackingId,
                                         RoamingNetwork.Id,
                                         ReservationId,
                                         LinkedReservationId,
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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnReserveRequest));
            }

            #endregion

            try
            {

                if (ChargingLocation.EVSEId.HasValue && ChargingLocation.EVSEId.Value != Id)
                    result = ReservationResult.UnknownLocation;

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    lock (reservations)
                    {

                        #region Check if this is a reservation update...

                        if (ReservationId.HasValue &&
                            reservations.TryGetValue(ReservationId.Value, out ChargingReservation oldReservation))
                        {

                            //ToDo: Calc if this reservation update is possible!
                            //      When their are other reservations => conflicts!

                            var updatedReservation  = reservations[ReservationId.Value]
                                                    = new ChargingReservation(oldReservation.Id,
                                                                              Timestamp.Value,
                                                                              oldReservation.StartTime,
                                                                              Duration ?? MaxReservationDuration,
                                                                              (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                                              oldReservation.ConsumedReservationTime + oldReservation.Duration - oldReservation.TimeLeft,
                                                                              ReservationLevel,
                                                                              ProviderId,
                                                                              RemoteAuthentication,
                                                                              RoamingNetwork.Id,
                                                                              null, //ChargingStation.ChargingPool.EVSEOperator.RoamingNetwork,
                                                                              null, //ChargingStation.ChargingPool.Id,
                                                                              null, //ChargingStation.Id,
                                                                              Id,
                                                                              ChargingProduct,
                                                                              AuthTokens,
                                                                              eMAIds,
                                                                              PINs);

                            OnNewReservation?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, this, updatedReservation);

                            result = ReservationResult.Success(updatedReservation);

                        }

                        #endregion

                        #region ...or a new reservation

                        else
                        {

                            if (Status.Value == EVSEStatusTypes.OutOfService)
                                result = ReservationResult.OutOfService;

                            else if (Status.Value == EVSEStatusTypes.Charging ||
                                     Status.Value == EVSEStatusTypes.Reserved ||
                                     Status.Value == EVSEStatusTypes.Available)
                            {

                                 newReservation = new ChargingReservation(
                                                      Id:                      ReservationId ?? ChargingReservation_Id.Random(OperatorId),
                                                      Timestamp:               Timestamp.Value,
                                                      StartTime:               ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                      Duration:                Duration  ?? MaxReservationDuration,
                                                      EndTime:                 (ReservationStartTime ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now) + (Duration ?? MaxReservationDuration),
                                                      ConsumedReservationTime: TimeSpan.FromSeconds(0),
                                                      ReservationLevel:        ReservationLevel,
                                                      ProviderId:              ProviderId,
                                                      StartAuthentication:     RemoteAuthentication,
                                                      RoamingNetworkId:        RoamingNetwork.Id,
                                                      ChargingPoolId:          null,
                                                      ChargingStationId:       null,
                                                      EVSEId:                  Id,
                                                      ChargingProduct:         ChargingProduct,
                                                      AuthTokens:              AuthTokens,
                                                      eMAIds:                  eMAIds,
                                                      PINs:                    PINs ?? (new UInt32[] { (UInt32) (_random.Next(1000000) + 100000) })
                                                  );

                                 reservations.Add(newReservation.Id, newReservation);

                                 result = ReservationResult.Success(newReservation);

                            }

                            else
                                result = ReservationResult.Error();

                        }

                        #endregion

                    }

                }
                else
                {
                    result = AdminStatus.Value switch {
                        _ => ReservationResult.OutOfService,
                    };
                }


                if (result.Result  == ReservationResultType.Success &&
                    newReservation != null)
                {

                    Status = EVSEStatusTypes.Reserved;

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
                                          LinkedReservationId,
                                          ChargingLocation,
                                          ReservationStartTime,
                                          Duration,
                                          ProviderId,
                                          RemoteAuthentication,
                                          ChargingProduct,
                                          AuthTokens,
                                          eMAIds,
                                          PINs,
                                          result,
                                          EndTime - startTime,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnReserveResponse));
            }

            #endregion

            return Task.FromResult(result);

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
        public Task<CancelReservationResult>

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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnCancelReservationRequest));
            }

            #endregion


            try
            {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    lock (reservations)
                    {

                        if (!reservations.TryGetValue(ReservationId, out canceledReservation))
                            return Task.FromResult(CancelReservationResult.UnknownReservationId(ReservationId,
                                                                                                Reason));

                        reservations.Remove(ReservationId);

                    }

                    result = CancelReservationResult.Success(ReservationId,
                                                             Reason,
                                                             canceledReservation);

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


                if (result.Result == CancelReservationResultTypes.Success)
                {

                    if (Status.Value == EVSEStatusTypes.Reserved &&
                    !reservations.Any())
                    {
                        // Will send events!
                        Status = EVSEStatusTypes.Available;
                    }

                    OnReservationCanceled?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                  this,
                                                  canceledReservation,
                                                  Reason);

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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnCancelReservationResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #region CheckIfReservationIsExpired(State)

        /// <summary>
        /// Check if the reservation is expired.
        /// </summary>
        public void CheckIfReservationIsExpired(Object State)
        {

            if (Monitor.TryEnter(ReservationExpiredLock))
            {

                try
                {

                    ChargingReservation[] expiredReservations = null;

                    lock (reservations)
                    {
                        expiredReservations = reservations.Values.Where(reservation => reservation.IsExpired()).ToArray();
                    }

                    foreach (var expiredReservation in expiredReservations)
                    {

                        lock (reservations)
                        {
                            reservations.Remove(expiredReservation.Id);
                        }

                        if (Status.Value == EVSEStatusTypes.Reserved &&
                            !reservations.Any())
                        {
                            // Will send events!
                            Status = EVSEStatusTypes.Available;
                        }

                        OnReservationCanceled?.Invoke(Timestamp.Now,
                                                      this,
                                                      expiredReservation,
                                                      ChargingReservationCancellationReason.Expired);

                    }

                }
                catch (Exception e)
                {
                    DebugX.LogT(e.Message);
                }
                finally
                {
                    Monitor.Exit(ReservationExpiredLock);
                }

            }

        }

        #endregion

        #endregion

        #region RemoteStart/-Stop and Sessions...

        #region Data

        private ChargingSession? chargingSession;


        public IEnumerable<ChargingSession> ChargingSessions

            => chargingSession is not null
                   ? new ChargingSession[] { chargingSession }
                   : Array.Empty<ChargingSession>();


        #region TryGetChargingSessionById(SessionId, out ChargingSession)

        /// <summary>
        /// Return the charging session specified by the given identification.
        /// </summary>
        /// <param name="SessionId">The charging session identification.</param>
        /// <param name="ChargingSession">The charging session.</param>
        public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
        {

            if (SessionId == chargingSession.Id)
            {
                ChargingSession = chargingSession;
                return true;
            }

            ChargingSession = null;
            return false;

        }

        #endregion

        /// <summary>
        /// The current charging session, if available.
        /// </summary>
        [InternalUseOnly]
        public ChargingSession? ChargingSession
        {

            get
            {
                return chargingSession;
            }

            set
            {

                // Skip, if the charging session is already known... 
                if (chargingSession != value)
                {

                    chargingSession = value;

                    if (chargingSession is not null)
                    {

                        Status = EVSEStatusTypes.Charging;

                        OnNewChargingSession?.Invoke(Timestamp.Now,
                                                     this,
                                                     chargingSession);

                    }

                    else
                        Status = EVSEStatusTypes.Available;

                }

            }

        }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate?     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate?    OnRemoteStartResponse;

        /// <summary>
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate?     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate?      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate?     OnRemoteStopResponse;

        /// <summary>
        /// An event fired whenever a new charge detail record was created.
        /// </summary>
        public event OnNewChargeDetailRecordDelegate?  OnNewChargeDetailRecord;

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


                => RemoteStart(ChargingLocation.FromEVSEId(Id),
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
        /// Start a charging session.
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
        public Task<RemoteStartResult>

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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStartRequest));
            }

            #endregion


            try
            {

                if (ChargingLocation.EVSEId.HasValue &&
                    ChargingLocation.EVSEId.Value != Id)
                {
                    result = RemoteStartResult.UnknownLocation();
                }

                else if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                         AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {


                    #region Available

                    if (Status.Value == EVSEStatusTypes.Available ||
                        Status.Value == EVSEStatusTypes.DoorNotClosed)
                    {

                        chargingSession = new ChargingSession(SessionId ?? ChargingSession_Id.NewRandom) {
                            EventTrackingId      = EventTrackingId,
                            ReservationId        = ReservationId,
                            Reservation          = reservations.Values.FirstOrDefault(reservation => reservation.Id == ReservationId),
                            EVSEId               = Id,
                            ChargingProduct      = ChargingProduct,
                            ProviderIdStart      = ProviderId,
                            AuthenticationStart  = RemoteAuthentication
                        };

                        chargingSession.AddEnergyMeterValue(new Timestamped<Decimal>(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, 0));
                        EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                        Status = EVSEStatusTypes.Charging;

                        result = RemoteStartResult.Success(chargingSession);

                    }

                    #endregion

                    #region Reserved

                    else if (Status.Value == EVSEStatusTypes.Reserved)
                    {

                        var firstReservation = reservations.Values.OrderBy(reservation => reservation.StartTime).FirstOrDefault();

                        #region Not matching reservation identifications...

                        if (firstReservation != null && !ReservationId.HasValue)
                            result = RemoteStartResult.Reserved(I18NString.Create(Languages.en, "Missing reservation identification!"));

                        else if (firstReservation != null && ReservationId.HasValue && firstReservation.Id != ReservationId.Value)
                            result = RemoteStartResult.Reserved(I18NString.Create(Languages.en, "Invalid reservation identification!"));

                        #endregion

                        #region ...or a matching reservation identification!

                        // Check if this remote start is allowed!
                        else if (RemoteAuthentication?.RemoteIdentification.HasValue == true &&
                            !firstReservation.eMAIds.Contains(RemoteAuthentication.RemoteIdentification.Value))
                        {
                            result = RemoteStartResult.InvalidCredentials();
                        }

                        else
                        {

                            firstReservation.AddToConsumedReservationTime(firstReservation.Duration - firstReservation.TimeLeft);

                            // Will also set the status -> EVSEStatusType.Charging;
                            chargingSession = new ChargingSession(SessionId ?? ChargingSession_Id.NewRandom) {
                                EventTrackingId      = EventTrackingId,
                                ReservationId        = ReservationId,
                                Reservation          = firstReservation,
                                EVSEId               = Id,
                                ChargingProduct      = ChargingProduct,
                                ProviderIdStart      = ProviderId,
                                AuthenticationStart  = RemoteAuthentication
                            };

                            firstReservation.ChargingSession = ChargingSession;

                            chargingSession.AddEnergyMeterValue(new Timestamped<Decimal>(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, 0));
                            EnergyMeterTimer.Change(EnergyMeterInterval, EnergyMeterInterval);

                            Status = EVSEStatusTypes.Charging;

                            result = RemoteStartResult.Success(chargingSession);

                        }

                        #endregion

                    }

                    #endregion

                    #region Charging

                    else if (Status.Value == EVSEStatusTypes.Charging)
                        result = RemoteStartResult.AlreadyInUse();

                    #endregion

                    #region OutOfService

                    else if (Status.Value == EVSEStatusTypes.OutOfService)
                        result = RemoteStartResult.OutOfService();

                    #endregion

                    #region Offline

                    else if (Status.Value == EVSEStatusTypes.Offline)
                        result = RemoteStartResult.Offline();

                    #endregion

                    else
                        result = RemoteStartResult.Error("Could not start charging!");

                }
                else
                {

                    result = AdminStatus.Value switch {
                        _ => RemoteStartResult.OutOfService(),
                    };
                }


            } catch (Exception e)
            {
                result = RemoteStartResult.Error(e.Message);
            }


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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStartResponse));
            }

            #endregion

            return Task.FromResult(result);

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
        public Task<RemoteStopResult>

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

            if (SessionId == null)
                SessionId = ChargingSession_Id.NewRandom;

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;


            RemoteStopResult result = null;

            #endregion

            #region Send OnRemoteStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnRemoteStopRequest?.Invoke(StartTime,
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
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStopRequest));
            }

            #endregion


            try {

                if (AdminStatus.Value == EVSEAdminStatusTypes.Operational ||
                    AdminStatus.Value == EVSEAdminStatusTypes.InternalUse)
                {

                    #region Available

                    if (Status.Value == EVSEStatusTypes.Available)
                        result = RemoteStopResult.InvalidSessionId(SessionId);

                    #endregion

                    #region Reserved

                    else if (Status.Value == EVSEStatusTypes.Reserved)
                        result = RemoteStopResult.InvalidSessionId(SessionId);

                    #endregion

                    #region Charging

                    else if (Status.Value == EVSEStatusTypes.Charging)
                    {

                        #region Matching session identification...

                        if (chargingSession.Id == SessionId)
                        {

                            EnergyMeterTimer.Change(Timeout.Infinite, Timeout.Infinite);

                            var __ChargingSession    = chargingSession;
                            var Now                  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            var SessionTime          = __ChargingSession.SessionTime;
                            SessionTime.EndTime = Now;
                            __ChargingSession.SessionTime.EndTime = Now;
                            var Duration             = Now - __ChargingSession.SessionTime.StartTime;
                            var Consumption          = (Decimal) Math.Round(Duration.TotalHours * MaxPower, 2);

                            ChargingSession.AddEnergyMeterValue(new Timestamped<Decimal>(Now, Consumption));

                            var _ChargeDetailRecord  = new ChargeDetailRecord(Id:                        ChargeDetailRecord_Id.Parse(__ChargingSession.Id.ToString()),
                                                                              SessionId:                 __ChargingSession.Id,
                                                                              Reservation:               __ChargingSession.Reservation,
                                                                              EVSEId:                    __ChargingSession.EVSEId,
                                                                              EVSE:                      __ChargingSession.EVSE,
                                                                              ChargingStation:           __ChargingSession.EVSE?.ChargingStation,
                                                                              ChargingPool:              __ChargingSession.EVSE?.ChargingStation?.ChargingPool,
                                                                              ChargingStationOperator:   __ChargingSession.EVSE?.Operator,
                                                                              ChargingProduct:           __ChargingSession.ChargingProduct,
                                                                              ProviderIdStart:           __ChargingSession.ProviderIdStart,
                                                                              ProviderIdStop:            __ChargingSession.ProviderIdStop,
                                                                              SessionTime:               __ChargingSession.SessionTime,

                                                                              AuthenticationStart:       __ChargingSession.AuthenticationStart,
                                                                              AuthenticationStop:        __ChargingSession.AuthenticationStop,

                                                                              EnergyMeterId:             EnergyMeterId,
                                                                              EnergyMeteringValues:      __ChargingSession.EnergyMeteringValues
                                                                              //SignedMeteringValues:      ChargingSession.EnergyMeteringValues.Select(metervalue =>
                                                                              //                               new SignedMeteringValue(metervalue.Timestamp,
                                                                              //                                                       metervalue.Value,
                                                                              //                                                       EnergyMeterId.Value,
                                                                              //                                                       Id,
                                                                              //                                                       _ChargingSession.IdentificationStart,
                                                                              //                                                       PublicKeyRing.First()).
                                                                              //                                   Sign(SecretKeyRing.First(),
                                                                              //                                        Passphrase))
                                                                                                               //                      PublicKeys.First().First()).
                                                                                                               //  Sign(SecretKeys.First().First(),
                                                                                                               //       Passphrase),


                                                                             );

                            // Will do: Status = EVSEStatusType.Available
                            ChargingSession = null;

                            if (!ReservationHandling.HasValue ||
                                (ReservationHandling.HasValue &&
                                !ReservationHandling.Value.IsKeepAlive))
                            {

                                // Will do: Status = EVSEStatusType.Available
                                //Reservation = null;

                            }

                            else
                            {
                                //ToDo: Reservation will live on!
                            }


                            //OnNewChargeDetailRecord?.Invoke(Timestamp.Now,
                            //                                this,
                            //                                _ChargeDetailRecord);

                            result = RemoteStopResult.Success(SessionId,
                                                              null,
                                                              null,
                                                              __ChargingSession.Reservation?.Id,
                                                              ReservationHandling,
                                                              _ChargeDetailRecord);

                        }

                        #endregion

                        #region ...or unknown session identification!

                            else
                                result = RemoteStopResult.InvalidSessionId(SessionId);

                            #endregion

                    }

                    #endregion

                    #region OutOfService

                    else if (Status.Value == EVSEStatusTypes.OutOfService)
                        result = RemoteStopResult.OutOfService(SessionId);

                    #endregion

                    #region Offline

                    else if (Status.Value == EVSEStatusTypes.Offline)
                        result = RemoteStopResult.Offline(SessionId);

                    #endregion

                    else
                        result = RemoteStopResult.Error(SessionId);

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
                                             EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(VirtualEVSE) + "." + nameof(OnRemoteStopResponse));
            }

            #endregion

            return Task.FromResult(result);

        }

        #endregion

        #endregion


        #region IEnumerable<SocketOutlet> Members

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => _SocketOutlets.GetEnumerator();

        /// <summary>
        /// Return a socket outlet enumerator.
        /// </summary>
        public IEnumerator<SocketOutlet> GetEnumerator()
            => _SocketOutlets.GetEnumerator();

        #endregion


        #region Operator overloading

        #region Operator == (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(VirtualEVSE1, VirtualEVSE2))
                return true;

            // If one is null, but not both, return false.
            if ((VirtualEVSE1 is null) || (VirtualEVSE2 is null))
                return false;

            return VirtualEVSE1.Equals(VirtualEVSE2);

        }

        #endregion

        #region Operator != (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
            => !(VirtualEVSE1 == VirtualEVSE2);

        #endregion

        #region Operator <  (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
        {

            if (VirtualEVSE1 is null)
                throw new ArgumentNullException(nameof(VirtualEVSE1), "The given VirtualEVSE1 must not be null!");

            return VirtualEVSE1.CompareTo(VirtualEVSE2) < 0;

        }

        #endregion

        #region Operator <= (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
            => !(VirtualEVSE1 > VirtualEVSE2);

        #endregion

        #region Operator >  (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
        {

            if (VirtualEVSE1 is null)
                throw new ArgumentNullException(nameof(VirtualEVSE1), "The given VirtualEVSE1 must not be null!");

            return VirtualEVSE1.CompareTo(VirtualEVSE2) > 0;

        }

        #endregion

        #region Operator >= (VirtualEVSE1, VirtualEVSE2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE1">A virtual EVSE.</param>
        /// <param name="VirtualEVSE2">Another virtual EVSE.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (VirtualEVSE VirtualEVSE1, VirtualEVSE VirtualEVSE2)
            => !(VirtualEVSE1 < VirtualEVSE2);

        #endregion

        #endregion

        #region IComparable<VirtualEVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is VirtualEVSE VirtualEVSE))
                throw new ArgumentException("The given object is not a virtual EVSE!");

            return CompareTo(VirtualEVSE);

        }

        #endregion

        #region CompareTo(VirtualEVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="VirtualEVSE">An virtual EVSE to compare with.</param>
        public Int32 CompareTo(VirtualEVSE VirtualEVSE)
        {

            if (VirtualEVSE is null)
                throw new ArgumentNullException(nameof(VirtualEVSE),  "The given virtual EVSE must not be null!");

            return Id.CompareTo(VirtualEVSE.Id);

        }

        #endregion

        #endregion

        #region IEquatable<VirtualEVSE> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is VirtualEVSE VirtualEVSE))
                return false;

            return Equals(VirtualEVSE);

        }

        #endregion

        #region Equals(VirtualEVSE)

        /// <summary>
        /// Compares two virtual EVSEs for equality.
        /// </summary>
        /// <param name="VirtualEVSE">A virtual EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(VirtualEVSE VirtualEVSE)
        {

            if (VirtualEVSE is null)
                return false;

            return Id.Equals(VirtualEVSE.Id);

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

        #endregion


        public Int32 CompareTo(EVSE? EVSE)
        {
            throw new NotImplementedException();
        }

        public Boolean Equals(EVSE? EVSE)
        {
            throw new NotImplementedException();
        }

        public JObject ToJSON(Boolean Embedded = false, InfoStatus ExpandRoamingNetworkId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationOperatorId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingPoolId = InfoStatus.ShowIdOnly, InfoStatus ExpandChargingStationId = InfoStatus.ShowIdOnly, InfoStatus ExpandBrandIds = InfoStatus.ShowIdOnly, InfoStatus ExpandDataLicenses = InfoStatus.ShowIdOnly, CustomJObjectSerializerDelegate<EVSE>? CustomEVSESerializer = null)
        {
            throw new NotImplementedException();
        }

        public EVSE UpdateWith(EVSE OtherEVSE)
        {
            throw new NotImplementedException();
        }

        public Boolean Equals(IEVSE? other)
        {
            throw new NotImplementedException();
        }

        public Int32 CompareTo(IEVSE? other)
        {
            throw new NotImplementedException();
        }

        public ChargingStationOperator? Operator => throw new NotImplementedException();

        public IRemoteEVSE? RemoteEVSE => throw new NotImplementedException();

        Decimal? IEVSE.AverageVoltage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Brand> Brands => throw new NotImplementedException();

        public ChargingPool? ChargingPool => throw new NotImplementedException();

        public ChargingStation? ChargingStation => throw new NotImplementedException();

        public ReactiveSet<DataLicense> DataLicenses => throw new NotImplementedException();

        public EnergyMeter? EnergyMeter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EnergyMix? EnergyMix { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Timestamped<EnergyMix>? EnergyMixRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Boolean IsFreeOfCharge { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? LastStatusUpdate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Decimal? IEVSE.MaxCapacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Timestamped<Decimal>> MaxCapacityPrognoses => throw new NotImplementedException();

        public Timestamped<Decimal>? MaxCapacityRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Decimal? IEVSE.MaxCurrent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Timestamped<Decimal>> MaxCurrentPrognoses => throw new NotImplementedException();

        public Timestamped<Decimal>? MaxCurrentRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Decimal? IEVSE.MaxPower { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<Timestamped<Decimal>> MaxPowerPrognoses => throw new NotImplementedException();

        public Timestamped<Decimal>? MaxPowerRealTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<ChargingReservation> Reservations => throw new NotImplementedException();

        public EnergyMixPrognosis? EnergyMixPrognoses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        TimeSpan IReserveRemoteStartStop.MaxReservationDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReactiveSet<URL> PhotoURLs => throw new NotImplementedException();

        public ReactiveSet<ChargingProduct> ChargingProducts => throw new NotImplementedException();

        public ReactiveSet<ChargingTariff> ChargingTariffs => throw new NotImplementedException();
    }

}
