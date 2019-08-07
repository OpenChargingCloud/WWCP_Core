/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.WWCP.Networking
{

    /// <summary>
    /// An abstract remote charging station attached via a computer network (HTTPS/TCP/IP).
    /// </summary>
    public abstract class ANetworkChargingStation : INetworkChargingStation
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

        public static readonly TimeSpan  DefaultRequestTimeout  = TimeSpan.FromSeconds(180);


        public static readonly IPPort DefaultRemotePort = IPPort.Parse(2348);

        /// <summary>
        /// The default time span between self checks.
        /// </summary>
        public static readonly TimeSpan DefaultSelfCheckTimeSpan = TimeSpan.FromSeconds(5);

        private static readonly HTTPPath DefaultURIPrefix = HTTPPath.Parse("/");

        private Timer _SelfCheckTimer;

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this network charging station.
        /// </summary>
        public ChargingStation_Id Id { get; }

        #region Description

        internal I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this charging station.
        /// </summary>
        [Optional]
        public I18NString Description
        {

            get
            {

                return _Description;

            }

            set
            {

                if (value == _Description)
                    return;

                _Description = value;

            }

        }

        #endregion


        /// <summary>
        /// The remote hostname.
        /// </summary>
        public HTTPHostname                         Hostname                      { get; }

        /// <summary>
        /// The remote virtual hostname.
        /// </summary>
        public HTTPHostname?                        VirtualHostname               { get; }

        /// <summary>
        /// The remote HTTPS port.
        /// </summary>
        public IPPort                               RemotePort                    { get; }

        /// <summary>
        /// The remote SSL/TLS certificate validator.
        /// </summary>
        public RemoteCertificateValidationCallback  RemoteCertificateValidator    { get; }

        /// <summary>
        /// The roaming network identification.
        /// </summary>
        public RoamingNetwork_Id                    RoamingNetworkId              { get; }

        /// <summary>
        /// The request timeout.
        /// </summary>
        public TimeSpan?                            RequestTimeout                { get; }

        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient                            DNSClient                     { get; }







        public IPTransport                          IPTransport                     { get; }

        public HTTPPath                             URIPrefix                       { get; }

        public String                               Service                         { get; }



        public LocalCertificateSelectionCallback    LocalCertificateSelector        { get; }

        public X509Certificate                      ClientCert                      { get; }



        #region Status

        /// <summary>
        /// The current charging station status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<ChargingStationStatusTypes> Status
        {

            get
            {
                return _StatusSchedule.CurrentStatus;
            }

            set
            {

                if (value == null)
                    return;

                if (_StatusSchedule.CurrentValue != value.Value)
                    SetStatus(value);

            }

        }

        #endregion

        #region StatusSchedule

        private StatusSchedule<ChargingStationStatusTypes> _StatusSchedule;

        /// <summary>
        /// The charging station status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationStatusTypes>> StatusSchedule
        {
            get
            {
                return _StatusSchedule;
            }
        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current charging station admin status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<ChargingStationAdminStatusTypes> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {

                if (value == null)
                    return;

                if (_AdminStatusSchedule.CurrentValue != value.Value)
                    SetAdminStatus(value);

            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<ChargingStationAdminStatusTypes> _AdminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<ChargingStationAdminStatusTypes>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule;
            }
        }

        #endregion


        public ChargingStation_Id RemoteChargingStationId { get; set; }
        public String             RemoteEVSEIdPrefix      { get; set; }


        #region SelfCheckTimeSpan

        private readonly TimeSpan _SelfCheckTimeSpan;

        /// <summary>
        /// The time span between self checks.
        /// </summary>
        public TimeSpan SelfCheckTimeSpan
        {
            get
            {
                return _SelfCheckTimeSpan;
            }
        }

        #endregion


        #region EVSEIdMapping

        private readonly Dictionary<EVSE_Id, EVSE_Id> MapOutgoing;
        private readonly Dictionary<EVSE_Id, EVSE_Id> MapIncoming;

        #endregion


        public String                  EllipticCurve            { get; }
        public X9ECParameters          ECP                      { get; }
        public ECDomainParameters      ECSpec                   { get; }
        public FpCurve                 C                        { get; }
        public ECPrivateKeyParameters  PrivateKey               { get; }
        public PublicKeyCertificates   PublicKeyCertificates    { get; }

        #endregion

        #region Events

        // EVSE events

        #region EVSEAddition

        internal readonly IVotingNotificator<DateTime, IRemoteChargingStation, IRemoteEVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<DateTime, IRemoteChargingStation, IRemoteEVSE, Boolean> OnEVSEAddition
        {
            get
            {
                return EVSEAddition;
            }
        }

        #endregion



        // Socket events

        #region SocketOutletAddition

        //internal readonly IVotingNotificator<DateTime, VirtualEVSE, SocketOutlet, Boolean> SocketOutletAddition;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was added.
        ///// </summary>
        //public IVotingSender<DateTime, VirtualEVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        //{
        //    get
        //    {
        //        return SocketOutletAddition;
        //    }
        //}

        #endregion

        #region SocketOutletRemoval

        //internal readonly IVotingNotificator<DateTime, VirtualEVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        ///// <summary>
        ///// Called whenever a socket outlet will be or was removed.
        ///// </summary>
        //public IVotingSender<DateTime, VirtualEVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
        //{
        //    get
        //    {
        //        return SocketOutletRemoval;
        //    }
        //}

        #endregion


        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data of the charging station changed.
        /// </summary>
        public event OnRemoteChargingStationDataChangedDelegate         OnChargingStationDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingStationStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of the charging station changed.
        /// </summary>
        public event OnRemoteChargingStationAdminStatusChangedDelegate  OnAdminStatusChanged;
        public event OnRemoteChargingStationDataChangedDelegate OnDataChanged;

        #endregion

        #endregion

        #region Constructor(s)

        #region NetworkChargingStationStub(Id)

        /// <summary>
        /// A charging station.
        /// </summary>
        public ANetworkChargingStation(ChargingStation_Id               Id,
                                       I18NString                       Description              = null,
                                       ChargingStationAdminStatusTypes  InitialAdminStatus       = ChargingStationAdminStatusTypes.Operational,
                                       ChargingStationStatusTypes       InitialStatus            = ChargingStationStatusTypes.Available,
                                       String                           EllipticCurve            = "P-256",
                                       ECPrivateKeyParameters           PrivateKey               = null,
                                       PublicKeyCertificates            PublicKeyCertificates    = null,
                                       TimeSpan?                        SelfCheckTimeSpan        = null,
                                       UInt16                           MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                                       UInt16                           MaxStatusListSize        = DefaultMaxStatusListSize)
        {

            #region Init data and properties

            this.Id                     = Id;
            this._EVSEs                 = new HashSet<IRemoteEVSE>();

            this._StatusSchedule        = new StatusSchedule<ChargingStationStatusTypes>(MaxStatusListSize);
            this._StatusSchedule.Insert(ChargingStationStatusTypes.OutOfService);

            this._AdminStatusSchedule   = new StatusSchedule<ChargingStationAdminStatusTypes>(MaxStatusListSize);
            this._AdminStatusSchedule.Insert(ChargingStationAdminStatusTypes.OutOfService);

            #endregion

            #region Setup crypto

            this.EllipticCurve          = EllipticCurve ?? "P-256";
            this.ECP                    = ECNamedCurveTable.GetByName(this.EllipticCurve);
            this.ECSpec                 = new ECDomainParameters(ECP.Curve, ECP.G, ECP.N, ECP.H, ECP.GetSeed());
            this.C                      = (FpCurve) ECSpec.Curve;
            this.PrivateKey             = PrivateKey;
            this.PublicKeyCertificates  = PublicKeyCertificates;

            if (PrivateKey == null && PublicKeyCertificates == null)
            {

                var generator = GeneratorUtilities.GetKeyPairGenerator("ECDH");
                generator.Init(new ECKeyGenerationParameters(ECSpec, new SecureRandom()));

                var  keyPair                = generator.GenerateKeyPair();
                this.PrivateKey             = keyPair.Private as ECPrivateKeyParameters;

                this.PublicKeyCertificates  = new PublicKeyCertificate(
                                                  PublicKeys:          new PublicKeyLifetime[] {
                                                                           new PublicKeyLifetime(
                                                                               PublicKey:  keyPair.Public as ECPublicKeyParameters,
                                                                               NotBefore:  DateTime.UtcNow,
                                                                               NotAfter:   DateTime.UtcNow + TimeSpan.FromDays(365),
                                                                               Algorithm:  "P-256",
                                                                               Comment:    I18NString.Empty
                                                                           )
                                                                       },
                                                  Description:         I18NString.Create(Languages.eng, "Auto-generated test keys for a network charging station!"),
                                                  Operations:          JSONObject.Create(
                                                                           new JProperty("signMeterValues",  true),
                                                                           new JProperty("signCertificates",
                                                                               JSONObject.Create(
                                                                                   new JProperty("maxChilds", 1)
                                                                               ))
                                                                       ),
                                                  ChargingStationId:   Id);

            }

            #endregion

            #region Init events

            // ChargingStation events
            this.EVSEAddition = new VotingNotificator<DateTime, IRemoteChargingStation, IRemoteEVSE, Boolean>(() => new VetoVote(), true);
            //  this.EVSERemoval                = new VotingNotificator<DateTime, ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            //  // EVSE events
            //  this.SocketOutletAddition       = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            //  this.SocketOutletRemoval        = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #region NetworkChargingStationStub(Id, IPTransport = IPv4only, DNSClient = null, Hostname = DefaultHostname, TCPPort = null, ...)

        /// <summary>
        /// Create a new remote charging station attached via a computer network (HTTPS/TCP/IP).
        /// </summary>
        /// <param name="ChargingStationId">A local charging station.</param>
        /// <param name="DNSClient">An optional DNS client used to resolve DNS names.</param>
        public ANetworkChargingStation(ChargingStation_Id                   ChargingStationId,
                                       I18NString                           Description                  = null,
                                       ChargingStationAdminStatusTypes      InitialAdminStatus           = ChargingStationAdminStatusTypes.Operational,
                                       ChargingStationStatusTypes           InitialStatus                = ChargingStationStatusTypes.Available,
                                       String                               EllipticCurve                = "P-256",
                                       ECPrivateKeyParameters               PrivateKey                   = null,
                                       PublicKeyCertificates                PublicKeyCertificates        = null,
                                       TimeSpan?                            SelfCheckTimeSpan            = null,
                                       UInt16                               MaxStatusListSize            = DefaultMaxStatusListSize,
                                       UInt16                               MaxAdminStatusListSize       = DefaultMaxAdminStatusListSize,
                                       IPTransport                          IPTransport                  = IPTransport.IPv4only,
                                       DNSClient                            DNSClient                    = null,
                                       HTTPHostname?                        Hostname                     = null,
                                       IPPort?                              RemotePort                   = null,
                                       String                               Service                      = null,
                                       RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
                                       LocalCertificateSelectionCallback    LocalCertificateSelector     = null,
                                       X509Certificate                      ClientCert                   = null,
                                       HTTPHostname?                        VirtualHostname              = null,
                                       HTTPPath?                            URIPrefix                    = null,
                                       TimeSpan?                            RequestTimeout               = null)

            : this(ChargingStationId,
                   Description,
                   InitialAdminStatus,
                   InitialStatus,
                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,
                   SelfCheckTimeSpan,
                   MaxStatusListSize,
                   MaxAdminStatusListSize)

        {

            this.IPTransport                 = IPTransport;
            this.DNSClient                   = DNSClient      ?? new DNSClient(SearchForIPv4DNSServers: true,
                                                                               SearchForIPv6DNSServers: false);
            this.Hostname                    = Hostname.Value;
            this.RemotePort                  = RemotePort     ?? DefaultRemotePort;
            this.Service                     = Service;
            this.RemoteCertificateValidator  = RemoteCertificateValidator;
            this.LocalCertificateSelector    = LocalCertificateSelector;
            this.ClientCert                  = ClientCert;
            this.VirtualHostname             = VirtualHostname;
            this.URIPrefix                   = URIPrefix      ?? DefaultURIPrefix;
            this.RequestTimeout              = RequestTimeout ?? DefaultRequestTimeout;

            this._SelfCheckTimeSpan           = SelfCheckTimeSpan != null && SelfCheckTimeSpan.HasValue ? SelfCheckTimeSpan.Value : DefaultSelfCheckTimeSpan;
            this._SelfCheckTimer              = new Timer(SelfCheck, null, _SelfCheckTimeSpan, _SelfCheckTimeSpan);

            this.MapOutgoing                  = new Dictionary<EVSE_Id, EVSE_Id>();
            this.MapIncoming                  = new Dictionary<EVSE_Id, EVSE_Id>();

        }

        #endregion

        #endregion


        #region (Admin-)Status management

        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new timestamped status.</param>
        public void SetStatus(ChargingStationStatusTypes  NewStatus)
        {
            _StatusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<ChargingStationStatusTypes> NewTimestampedStatus)
        {
            _StatusSchedule.Insert(NewTimestampedStatus);
        }

        #endregion

        #region SetStatus(NewStatus, Timestamp)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetStatus(ChargingStationStatusTypes  NewStatus,
                              DateTime                   Timestamp)
        {
            _StatusSchedule.Insert(NewStatus, Timestamp);
        }

        #endregion

        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetStatus(IEnumerable<Timestamped<ChargingStationStatusTypes>>  NewStatusList,
                              ChangeMethods                                        ChangeMethod = ChangeMethods.Replace)
        {
            _StatusSchedule.Insert(NewStatusList, ChangeMethod);
        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(ChargingStationAdminStatusTypes  NewAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<ChargingStationAdminStatusTypes> NewTimestampedAdminStatus)
        {
            _AdminStatusSchedule.Insert(NewTimestampedAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatus, Timestamp)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new admin status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetAdminStatus(ChargingStationAdminStatusTypes  NewAdminStatus,
                                   DateTime                         Timestamp)
        {
            _AdminStatusSchedule.Insert(NewAdminStatus, Timestamp);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<ChargingStationAdminStatusTypes>>  NewAdminStatusList,
                                   ChangeMethods                                              ChangeMethod = ChangeMethods.Replace)
        {
            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);
        }

        #endregion


        #region (internal) UpdateAdminStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE admin status.</param>
        /// <param name="NewStatus">The new EVSE admin status.</param>
        internal void UpdateAdminStatus(DateTime                                      Timestamp,
                                        EventTracking_Id                              EventTrackingId,
                                        Timestamped<ChargingStationAdminStatusTypes>  OldStatus,
                                        Timestamped<ChargingStationAdminStatusTypes>  NewStatus)
        {

            OnAdminStatusChanged?.Invoke(Timestamp,
                                         EventTrackingId,
                                         this,
                                         OldStatus,
                                         NewStatus);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, EventTrackingId, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal void UpdateStatus(DateTime                                 Timestamp,
                                   EventTracking_Id                         EventTrackingId,
                                   Timestamped<ChargingStationStatusTypes>  OldStatus,
                                   Timestamped<ChargingStationStatusTypes>  NewStatus)
        {

            OnStatusChanged?.Invoke(Timestamp,
                                    EventTrackingId,
                                    this,
                                    OldStatus,
                                    NewStatus);

        }

        #endregion

        #endregion

        #region (private) SelfCheck(Context)

        private void SelfCheck(Object Context)
        {

            //foreach (var _EVSE in _EVSEs)
            //    _EVSE.CheckReservationTime().Wait();

        }

        #endregion


        public void AddMapping(EVSE_Id LocalEVSEId,
                               EVSE_Id RemoteEVSEId)
        {

            MapOutgoing.Add(LocalEVSEId,  RemoteEVSEId);
            MapIncoming.Add(RemoteEVSEId, LocalEVSEId);

        }

        public EVSE_Id MapOutgoingId(EVSE_Id EVSEIdOut)
        {

            if (MapOutgoing.TryGetValue(EVSEIdOut, out EVSE_Id EVSEIdIn))
                return EVSEIdIn;

            return EVSEIdOut;

        }

        public EVSE_Id MapIncomingId(EVSE_Id EVSEIdIn)
        {

            if (MapIncoming.TryGetValue(EVSEIdIn, out EVSE_Id EVSEIdOut))
                return EVSEIdOut;

            return EVSEIdIn;

        }


        #region EVSEs

        #region Data

        private readonly HashSet<IRemoteEVSE> _EVSEs;

        /// <summary>
        /// All registered EVSEs.
        /// </summary>
        public IEnumerable<IRemoteEVSE> EVSEs
            => _EVSEs;

        #region ContainsEVSE(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the charging station.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSE(EVSE_Id EVSEId)
            => _EVSEs.Any(evse => evse.Id == EVSEId);

        #endregion

        #region GetEVSEById(EVSEId)

        public IRemoteEVSE GetEVSEById(EVSE_Id EVSEId)
            => _EVSEs.FirstOrDefault(evse => evse.Id == EVSEId);

        #endregion

        #region TryGetEVSEById(EVSEId, out EVSE)

        public Boolean TryGetEVSEById(EVSE_Id EVSEId, out IRemoteEVSE EVSE)
        {

            EVSE = GetEVSEById(EVSEId);

            return EVSE != null;

        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever the static data of any subordinated EVSE changed.
        /// </summary>
        public event OnRemoteEVSEDataChangedDelegate         OnEVSEDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status of any subordinated EVSE changed.
        /// </summary>
        public event OnRemoteEVSEStatusChangedDelegate       OnEVSEStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status of any subordinated EVSE changed.
        /// </summary>
        public event OnRemoteEVSEAdminStatusChangedDelegate  OnEVSEAdminStatusChanged;

        #endregion

        #region (private) AddEVSE(NewRemoteEVSE)

        private Boolean AddNewEVSE(IRemoteEVSE NewRemoteEVSE)
        {

            if (_EVSEs.Add(NewRemoteEVSE))
            {

                NewRemoteEVSE.OnAdminStatusChanged     += UpdateEVSEAdminStatus;
                NewRemoteEVSE.OnStatusChanged          += UpdateEVSEStatus;

                NewRemoteEVSE.OnNewReservation         += SendNewReservation;
                NewRemoteEVSE.OnReservationCanceled    += SendReservationCanceled;
                NewRemoteEVSE.OnNewChargingSession     += SendNewChargingSession;
                NewRemoteEVSE.OnNewChargeDetailRecord  += SendNewChargeDetailRecord;

                return true;

            }

            return false;

        }

        #endregion

        #region CreateNewEVSE(EVSEId, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new EVSE having the given
        /// unique EVSE identification.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the new EVSE.</param>
        /// <param name="Configurator">An optional delegate to configure the new EVSE after its creation.</param>
        /// <param name="OnSuccess">An optional delegate called after successful creation of the EVSE.</param>
        /// <param name="OnError">An optional delegate for signaling errors.</param>
        public NetworkEVSEStub CreateNewEVSE(EVSE_Id                                   EVSEId,
                                             Action<NetworkEVSEStub>                   Configurator  = null,
                                             Action<NetworkEVSEStub>                   OnSuccess     = null,
                                             Action<ANetworkChargingStation, EVSE_Id>  OnError       = null)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId), "The given EVSE identification must not be null!");

            //if (_EVSEs.Any(evse => evse.Id == EVSEId))
            //{
            //    if (OnError == null)
            //        throw new EVSEAlreadyExistsInStation(this.ChargingStation, EVSEId);
            //    else
            //        OnError?.Invoke(this, EVSEId);
            //}

            #endregion

            var Now           = DateTime.UtcNow;
            var _NetworkEVSE  = new NetworkEVSEStub(EVSEId, this);

            Configurator?.Invoke(_NetworkEVSE);

            if (EVSEAddition.SendVoting(Now, this, _NetworkEVSE))
            {
                if (_EVSEs.Add(_NetworkEVSE))
                {

               //     _EVSE.OnPropertyChanged     += (Timestamp, Sender, PropertyName, OldValue, NewValue)
               //                                     => UpdateEVSEData       (Timestamp, Sender as EVSE, PropertyName, OldValue, NewValue);
               //
               //     _EVSE.OnStatusChanged       += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus)
               //                                     => UpdateEVSEStatus     (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus);
               //
               //     _EVSE.OnAdminStatusChanged  += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus)
               //                                     => UpdateEVSEAdminStatus(Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus);

                    OnSuccess?.Invoke(_NetworkEVSE);
                    EVSEAddition.SendNotification(Now, this, _NetworkEVSE);
               //     UpdateEVSEStatus(Now, _EVSE, new Timestamped<EVSEStatusType>(Now, EVSEStatusType.Unspecified), _EVSE.Status);

                    return _NetworkEVSE;

                }
            }

            //Debug.WriteLine("EVSE '" + EVSEId + "' was not created!");
            return null;

        }

        #endregion

        #region AddEVSE(RemoteEVSE, Configurator = null, OnSuccess = null, OnError = null)

        public IRemoteEVSE AddEVSE(IRemoteEVSE                       RemoteEVSE,
                                   Action<IRemoteEVSE>               Configurator  = null,
                                   Action<IRemoteEVSE>               OnSuccess     = null,
                                   Action<ChargingStation, EVSE_Id>  OnError       = null)

        {

            #region Initial checks

            if (_EVSEs.Any(evse => evse.Id == RemoteEVSE.Id))
            {
                throw new Exception("EVSEAlreadyExistsInStation");
                // if (OnError == null)
                //     throw new EVSEAlreadyExistsInStation(this.ChargingStation, EVSEId);
                // else
                //     OnError?.Invoke(this, EVSEId);
            }

            #endregion

            Configurator?.Invoke(RemoteEVSE);

            if (AddNewEVSE(RemoteEVSE))
            {
                OnSuccess?.Invoke(RemoteEVSE);
                return RemoteEVSE;
            }

            return null;

        }

        #endregion


        #region (internal) UpdateEVSEData(Timestamp, RemoteEVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the data of a remote EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="RemoteEVSE">The remote EVSE.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal void UpdateEVSEData(DateTime     Timestamp,
                                     IRemoteEVSE  RemoteEVSE,
                                     String       PropertyName,
                                     Object       OldValue,
                                     Object       NewValue)
        {

            var OnEVSEDataChangedLocal = OnEVSEDataChanged;
            if (OnEVSEDataChangedLocal != null)
                OnEVSEDataChangedLocal(Timestamp, RemoteEVSE, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateEVSEAdminStatus(Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="RemoteEVSE">The updated remote EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEAdminStatus(DateTime                          Timestamp,
                                                  EventTracking_Id                  EventTrackingId,
                                                  IRemoteEVSE                       RemoteEVSE,
                                                  Timestamped<EVSEAdminStatusTypes>  OldStatus,
                                                  Timestamped<EVSEAdminStatusTypes>  NewStatus)
        {

            var OnEVSEAdminStatusChangedLocal = OnEVSEAdminStatusChanged;
            if (OnEVSEAdminStatusChangedLocal != null)
                await OnEVSEAdminStatusChangedLocal(Timestamp,
                                                    EventTrackingId,
                                                    RemoteEVSE,
                                                    OldStatus,
                                                    NewStatus);

        }

        #endregion

        #region (internal) UpdateEVSEStatus     (Timestamp, EventTrackingId, RemoteEVSE, OldStatus, NewStatus)

        /// <summary>
        /// Update the remote EVSE station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="RemoteEVSE">The updated EVSE.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal async Task UpdateEVSEStatus(DateTime                     Timestamp,
                                             EventTracking_Id             EventTrackingId,
                                             IRemoteEVSE                  RemoteEVSE,
                                             Timestamped<EVSEStatusTypes>  OldStatus,
                                             Timestamped<EVSEStatusTypes>  NewStatus)
        {

            var OnEVSEStatusChangedLocal = OnEVSEStatusChanged;
            if (OnEVSEStatusChangedLocal != null)
                await OnEVSEStatusChangedLocal(Timestamp,
                                               EventTrackingId,
                                               RemoteEVSE,
                                               OldStatus,
                                               NewStatus);

        }

        #endregion

        #endregion



        public virtual async Task<IEnumerable<EVSEStatus>> GetEVSEStatus(DateTime           Timestamp,
                                                                         CancellationToken  CancellationToken,
                                                                         EventTracking_Id   EventTrackingId,
                                                                         TimeSpan?          RequestTimeout = null)

            => new EVSEStatus[] {
                   new EVSEStatus(EVSE_Id.Parse("DE*822*E222*1"), new Timestamped<EVSEStatusTypes>(EVSEStatusTypes.Charging))
               };





        #region Reservations...

        #region Data

        /// <summary>
        /// All current charging reservations.
        /// </summary>
        public IEnumerable<ChargingReservation> Reservations
            => _EVSEs.SelectMany(evse => evse.Reservations);

        #region TryGetReservationById(ReservationId, out Reservation)

        /// <summary>
        /// Return the charging reservation specified by the given identification.
        /// </summary>
        /// <param name="ReservationId">The charging reservation identification.</param>
        /// <param name="Reservation">The charging reservation.</param>
        public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
        {

            foreach (var evse in _EVSEs)
            {
                if (evse.TryGetChargingReservationById(ReservationId, out Reservation))
                    return true;
            }

            Reservation = null;
            return false;

        }

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
        /// Reserve the possibility to charge at this station.
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
                    eMobilityProvider_Id?             ProviderId             = null,
                    RemoteAuthentication              RemoteAuthentication   = null,
                    ChargingProduct                   ChargingProduct        = null,
                    IEnumerable<Auth_Token>           AuthTokens             = null,
                    IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
                    IEnumerable<UInt32>               PINs                   = null,

                    DateTime?                         Timestamp              = null,
                    CancellationToken?                CancellationToken      = null,
                    EventTracking_Id                  EventTrackingId        = null,
                    TimeSpan?                         RequestTimeout         = null)


                => Reserve(ChargingLocation.FromChargingStationId(Id),
                           ChargingReservationLevel.ChargingStation,
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
        public virtual Task<ReservationResult>

            Reserve(ChargingLocation                  ChargingLocation,
                    ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
                    DateTime?                         StartTime              = null,
                    TimeSpan?                         Duration               = null,
                    ChargingReservation_Id?           ReservationId          = null,
                    eMobilityProvider_Id?             ProviderId             = null,
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

            return Task.FromResult(ReservationResult.OutOfService);

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
        public virtual Task<CancelReservationResult>

            CancelReservation(ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,

                              DateTime?                              Timestamp          = null,
                              CancellationToken?                     CancellationToken  = null,
                              EventTracking_Id                       EventTrackingId    = null,
                              TimeSpan?                              RequestTimeout     = null)

        {

            return Task.FromResult(CancelReservationResult.OutOfService(ReservationId, ChargingReservationCancellationReason.Aborted));

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
        /// An event fired whenever a new charging session was created.
        /// </summary>
        public event OnNewChargingSessionDelegate     OnNewChargingSession;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;

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

            RemoteStart(ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)


                => RemoteStart(ChargingLocation.FromChargingStationId(Id),
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
        public virtual Task<RemoteStartResult>

            RemoteStart(ChargingLocation         ChargingLocation,
                        ChargingProduct          ChargingProduct        = null,
                        ChargingReservation_Id?  ReservationId          = null,
                        ChargingSession_Id?      SessionId              = null,
                        eMobilityProvider_Id?    ProviderId             = null,
                        RemoteAuthentication     RemoteAuthentication   = null,

                        DateTime?                Timestamp              = null,
                        CancellationToken?       CancellationToken      = null,
                        EventTracking_Id         EventTrackingId        = null,
                        TimeSpan?                RequestTimeout         = null)
        {

            return Task.FromResult(RemoteStartResult.OutOfService);

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
        public virtual Task<RemoteStopResult>

            RemoteStop(ChargingSession_Id     SessionId,
                       ReservationHandling?   ReservationHandling    = null,
                       eMobilityProvider_Id?  ProviderId             = null,
                       RemoteAuthentication   RemoteAuthentication   = null,

                       DateTime?              Timestamp              = null,
                       CancellationToken?     CancellationToken      = null,
                       EventTracking_Id       EventTrackingId        = null,
                       TimeSpan?              RequestTimeout         = null)

        {

            return Task.FromResult(RemoteStopResult.OutOfService(SessionId));

        }

        #endregion


        #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

        internal void SendNewChargingSession(DateTime         Timestamp,
                                             Object           Sender,
                                             ChargingSession  Session)
        {

            if (Session != null)
            {

                if (Session.ChargingStation == null)
                    Session.ChargingStationId  = Id;

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


        #region Dispose()

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        { }

        #endregion

    }

}
