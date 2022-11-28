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
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.WWCP;
using cloud.charging.open.protocols.WWCP.Net;

#endregion

namespace cloud.charging.open.protocols.WWCP.Networking
{

    ///// <summary>
    ///// An abstract remote charging station operator attached via a computer network (HTTPS/TCP/IP).
    ///// </summary>
    //public abstract class ANetworkChargingStationOperator : IHTTPClient,
    //                                                        IRemoteChargingStationOperator
    //{

    //    #region Data

    //    /// <summary>
    //    /// The default HTTP port.
    //    /// </summary>
    //    public static readonly IPPort             DefaultRemotePort          = IPPort.HTTPS;

    //    /// <summary>
    //    /// The default roaming network identification.
    //    /// </summary>
    //    public static readonly RoamingNetwork_Id  DefaultRoamingNetworkId    = RoamingNetwork_Id.Parse("Prod");

    //    /// <summary>
    //    /// The default request timeout.
    //    /// </summary>
    //    public static readonly TimeSpan           DefaultRequestTimeout      = TimeSpan.FromSeconds(180);

    //    #endregion

    //    #region Properties

    //    /// <summary>
    //    /// The remote hostname.
    //    /// </summary>
    //    public HTTPHostname                         Hostname                      { get; }

    //    /// <summary>
    //    /// The remote virtual hostname.
    //    /// </summary>
    //    public HTTPHostname?                        VirtualHostname               { get; }

    //    /// <summary>
    //    /// The remote HTTPS port.
    //    /// </summary>
    //    public IPPort                               RemotePort                    { get; }

    //    /// <summary>
    //    /// The remote SSL/TLS certificate validator.
    //    /// </summary>
    //    public RemoteCertificateValidationCallback  RemoteCertificateValidator    { get; }

    //    /// <summary>
    //    /// The roaming network identification.
    //    /// </summary>
    //    public RoamingNetwork_Id                    RoamingNetworkId              { get; }

    //    /// <summary>
    //    /// The request timeout.
    //    /// </summary>
    //    public TimeSpan?                            RequestTimeout                { get; }

    //    /// <summary>
    //    /// The DNS client to use.
    //    /// </summary>
    //    public DNSClient                            DNSClient                     { get; }

    //    #endregion

    //    #region Events

    //    /// <summary>
    //    /// An event sent whenever a GetCDRs request was sent.
    //    /// </summary>
    //    public event ClientRequestLogHandler   OnGetCDRsHTTPRequest;

    //    /// <summary>
    //    /// An event sent whenever a response to a GetCDRs request was received.
    //    /// </summary>
    //    public event ClientResponseLogHandler  OnGetCDRsHTTPResponse;


    //    /// <summary>
    //    /// An event sent whenever a remote reservation start request was sent.
    //    /// </summary>
    //    public event ClientRequestLogHandler   OnRemoteReservationStartHTTPRequest;

    //    /// <summary>
    //    /// An event sent whenever a response to a remote reservation start request was received.
    //    /// </summary>
    //    public event ClientResponseLogHandler  OnRemoteReservationStartHTTPResponse;


    //    /// <summary>
    //    /// An event sent whenever a remote reservation stop request was sent.
    //    /// </summary>
    //    public event ClientRequestLogHandler   OnRemoteReservationStopHTTPRequest;

    //    /// <summary>
    //    /// An event sent whenever a response to a remote reservation stop request was received.
    //    /// </summary>
    //    public event ClientResponseLogHandler  OnRemoteReservationStopHTTPResponse;


    //    /// <summary>
    //    /// An event sent whenever a remotestart request was sent.
    //    /// </summary>
    //    public event ClientRequestLogHandler   OnRemoteStartHTTPRequest;

    //    /// <summary>
    //    /// An event sent whenever a response to a remotestart request was received.
    //    /// </summary>
    //    public event ClientResponseLogHandler  OnRemoteStartHTTPResponse;


    //    /// <summary>
    //    /// An event sent whenever a remotestop request was sent.
    //    /// </summary>
    //    public event ClientRequestLogHandler   OnRemoteStopHTTPRequest;

    //    /// <summary>
    //    /// An event sent whenever a response to a remotestop request was received.
    //    /// </summary>
    //    public event ClientResponseLogHandler  OnRemoteStopHTTPResponse;

    //    #endregion

    //    #region Constructor(s)

    //    /// <summary>
    //    /// Create a new abstract remote charging station operator attached via a computer network (HTTPS/TCP/IP).
    //    /// </summary>
    //    /// <param name="Hostname">The remote hostname.</param>
    //    /// <param name="VirtualHostname">An optional remote virtual hostname.</param>
    //    /// <param name="RemotePort">An optional remote HTTPS port.</param>
    //    /// <param name="RemoteCertificateValidator">An optional remote SSL/TLS certificate validator.</param>
    //    /// <param name="RoamingNetworkId">An optional roaming network identification.</param>
    //    /// <param name="RequestTimeout">An optional request timeout.</param>
    //    /// <param name="DNSClient">An optional DNS client to use.</param>
    //    public ANetworkChargingStationOperator(HTTPHostname                         Hostname,
    //                                           HTTPHostname?                        VirtualHostname              = null,
    //                                           IPPort?                              RemotePort                   = null,
    //                                           RemoteCertificateValidationCallback  RemoteCertificateValidator   = null,
    //                                           RoamingNetwork_Id?                   RoamingNetworkId             = null,
    //                                           TimeSpan?                            RequestTimeout               = null,
    //                                           DNSClient                            DNSClient                    = null)
    //    {

    //        this.Hostname                    = Hostname;
    //        this.VirtualHostname             = VirtualHostname            ?? this.Hostname;
    //        this.RemotePort                  = RemotePort                 ?? DefaultRemotePort;
    //        this.RemoteCertificateValidator  = RemoteCertificateValidator ?? ((sender, certificate, chain, policyErrors) => true);
    //        this.RoamingNetworkId            = RoamingNetworkId           ?? DefaultRoamingNetworkId;
    //        this.RequestTimeout              = RequestTimeout             ?? DefaultRequestTimeout;
    //        this.DNSClient                   = DNSClient;

    //    }

    //    #endregion


    //    #region Reservations...

    //    #region Data

    //    private readonly Dictionary<ChargingReservation_Id, ChargingReservation> _Reservations;

    //    /// <summary>
    //    /// All current charging reservations.
    //    /// </summary>
    //    public IEnumerable<ChargingReservation> ChargingReservations
    //        => _Reservations.Select(_ => _.Value);

    //    #region TryGetReservationById(ReservationId, out Reservation)

    //    /// <summary>
    //    /// Return the charging reservation specified by the given identification.
    //    /// </summary>
    //    /// <param name="ReservationId">The charging reservation identification.</param>
    //    /// <param name="Reservation">The charging reservation.</param>
    //    public Boolean TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation Reservation)
    //        => _Reservations.TryGetValue(ReservationId, out Reservation);

    //    #endregion

    //    #endregion

    //    #region Events

    //    /// <summary>
    //    /// An event fired whenever a charging location is being reserved.
    //    /// </summary>
    //    public event OnReserveRequestDelegate             OnReserveRequest;

    //    /// <summary>
    //    /// An event fired whenever a charging location was reserved.
    //    /// </summary>
    //    public event OnReserveResponseDelegate            OnReserveResponse;

    //    /// <summary>
    //    /// An event fired whenever a new charging reservation was created.
    //    /// </summary>
    //    public event OnNewReservationDelegate             OnNewReservation;


    //    /// <summary>
    //    /// An event fired whenever a charging reservation is being canceled.
    //    /// </summary>
    //    public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

    //    /// <summary>
    //    /// An event fired whenever a charging reservation was canceled.
    //    /// </summary>
    //    public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

    //    /// <summary>
    //    /// An event fired whenever a charging reservation was canceled.
    //    /// </summary>
    //    public event OnReservationCanceledDelegate        OnReservationCanceled;

    //    #endregion

    //    #region Reserve(ChargingLocation, ReservationLevel = EVSE, StartTime = null, Duration = null, ReservationId = null, ProviderId = null, ...)

    //    /// <summary>
    //    /// Reserve the possibility to charge at the given charging location.
    //    /// </summary>
    //    /// <param name="ChargingLocation">A charging location.</param>
    //    /// <param name="ReservationLevel">The level of the reservation to create (EVSE, charging station, ...).</param>
    //    /// <param name="ReservationStartTime">The starting time of the reservation.</param>
    //    /// <param name="Duration">The duration of the reservation.</param>
    //    /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
    //    /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
    //    /// <param name="RemoteAuthentication">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
    //    /// <param name="ChargingProduct">The charging product to be reserved.</param>
    //    /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
    //    /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
    //    /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public virtual Task<ReservationResult>

    //        Reserve(ChargingLocation                  ChargingLocation,
    //                ChargingReservationLevel          ReservationLevel       = ChargingReservationLevel.EVSE,
    //                DateTime?                         StartTime              = null,
    //                TimeSpan?                         Duration               = null,
    //                ChargingReservation_Id?           ReservationId          = null,
    //                eMobilityProvider_Id?             ProviderId             = null,
    //                RemoteAuthentication              RemoteAuthentication   = null,
    //                ChargingProduct                   ChargingProduct        = null,
    //                IEnumerable<AuthenticationToken>           AuthTokens             = null,
    //                IEnumerable<eMobilityAccount_Id>  eMAIds                 = null,
    //                IEnumerable<UInt32>               PINs                   = null,

    //                DateTime?                         Timestamp              = null,
    //                CancellationToken?                CancellationToken      = null,
    //                EventTracking_Id                  EventTrackingId        = null,
    //                TimeSpan?                         RequestTimeout         = null)

    //    {

    //        return Task.FromResult(ReservationResult.OutOfService);

    //    }

    //    #endregion

    //    #region CancelReservation(ReservationId, Reason, ...)

    //    /// <summary>
    //    /// Try to remove the given charging reservation.
    //    /// </summary>
    //    /// <param name="ReservationId">The unique charging reservation identification.</param>
    //    /// <param name="Reason">A reason for this cancellation.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public virtual Task<CancelReservationResult>

    //        CancelReservation(ChargingReservation_Id                 ReservationId,
    //                          ChargingReservationCancellationReason  Reason,

    //                          DateTime?                              Timestamp          = null,
    //                          CancellationToken?                     CancellationToken  = null,
    //                          EventTracking_Id                       EventTrackingId    = null,
    //                          TimeSpan?                              RequestTimeout     = null)

    //    {

    //        return Task.FromResult(CancelReservationResult.OutOfService(ReservationId,
    //                                                                    ChargingReservationCancellationReason.Aborted));

    //    }

    //    #endregion


    //    #region (internal) SendNewReservation     (Timestamp, Sender, Reservation)

    //    internal void SendNewReservation(DateTime             Timestamp,
    //                                     Object               Sender,
    //                                     ChargingReservation  Reservation)
    //    {

    //        OnNewReservation?.Invoke(Timestamp, Sender, Reservation);

    //    }

    //    #endregion

    //    #region (internal) SendReservationCanceled(Timestamp, Sender, Reservation, Reason)

    //    internal void SendReservationCanceled(DateTime                               Timestamp,
    //                                     Object                                 Sender,
    //                                     ChargingReservation                    Reservation,
    //                                     ChargingReservationCancellationReason  Reason)
    //    {

    //        OnReservationCanceled?.Invoke(Timestamp, Sender, Reservation, Reason);

    //    }

    //    #endregion

    //    #endregion

    //    #region RemoteStart/-Stop and Sessions...

    //    #region Data

    //    private readonly Dictionary<ChargingSession_Id, ChargingSession> _ChargingSessions;

    //    public IEnumerable<ChargingSession> ChargingSessions
    //        => _ChargingSessions.Select(_ => _.Value);

    //    #region TryGetChargingSessionById(SessionId, out ChargingSession)

    //    /// <summary>
    //    /// Return the charging session specified by the given identification.
    //    /// </summary>
    //    /// <param name="SessionId">The charging session identification.</param>
    //    /// <param name="ChargingSession">The charging session.</param>
    //    public Boolean TryGetChargingSessionById(ChargingSession_Id SessionId, out ChargingSession ChargingSession)
    //        => _ChargingSessions.TryGetValue(SessionId, out ChargingSession);

    //    #endregion

    //    #endregion

    //    #region Events

    //    /// <summary>
    //    /// An event fired whenever a remote start command was received.
    //    /// </summary>
    //    public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

    //    /// <summary>
    //    /// An event fired whenever a remote start command completed.
    //    /// </summary>
    //    public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;


    //    /// <summary>
    //    /// An event fired whenever a remote stop command was received.
    //    /// </summary>
    //    public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

    //    /// <summary>
    //    /// An event fired whenever a remote stop command completed.
    //    /// </summary>
    //    public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;


    //    /// <summary>
    //    /// An event fired whenever a new charging session was created.
    //    /// </summary>
    //    public event OnNewChargingSessionDelegate     OnNewChargingSession;

    //    /// <summary>
    //    /// An event fired whenever a new charge detail record was created.
    //    /// </summary>
    //    public event OnNewChargeDetailRecordDelegate  OnNewChargeDetailRecord;

    //    #endregion

    //    #region RemoteStart(ChargingLocation, ChargingProduct = null, ReservationId = null, SessionId = null, ProviderId = null, RemoteAuthentication = null, ...)

    //    /// <summary>
    //    /// Start a charging session.
    //    /// </summary>
    //    /// <param name="ChargingLocation">The charging location.</param>
    //    /// <param name="ChargingProduct">The choosen charging product.</param>
    //    /// <param name="ReservationId">The unique identification for a charging reservation.</param>
    //    /// <param name="SessionId">The unique identification for this charging session.</param>
    //    /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
    //    /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public virtual Task<RemoteStartResult>

    //        RemoteStart(ChargingLocation         ChargingLocation,
    //                    ChargingProduct          ChargingProduct        = null,
    //                    ChargingReservation_Id?  ReservationId          = null,
    //                    ChargingSession_Id?      SessionId              = null,
    //                    eMobilityProvider_Id?    ProviderId             = null,
    //                    RemoteAuthentication     RemoteAuthentication   = null,

    //                    DateTime?                Timestamp              = null,
    //                    CancellationToken?       CancellationToken      = null,
    //                    EventTracking_Id         EventTrackingId        = null,
    //                    TimeSpan?                RequestTimeout         = null)
    //    {

    //        return Task.FromResult(RemoteStartResult.OutOfService());

    //    }

    //    #endregion

    //    #region RemoteStop (SessionId, ReservationHandling = null, ProviderId = null, RemoteAuthentication = null, ...)

    //    /// <summary>
    //    /// Stop the given charging session.
    //    /// </summary>
    //    /// <param name="SessionId">The unique identification for this charging session.</param>
    //    /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
    //    /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
    //    /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
    //    /// 
    //    /// <param name="Timestamp">The optional timestamp of the request.</param>
    //    /// <param name="CancellationToken">An optional token to cancel this request.</param>
    //    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    //    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    //    public virtual Task<RemoteStopResult>

    //        RemoteStop(ChargingSession_Id     SessionId,
    //                   ReservationHandling?   ReservationHandling    = null,
    //                   eMobilityProvider_Id?  ProviderId             = null,
    //                   RemoteAuthentication   RemoteAuthentication   = null,

    //                   DateTime?              Timestamp              = null,
    //                   CancellationToken?     CancellationToken      = null,
    //                   EventTracking_Id       EventTrackingId        = null,
    //                   TimeSpan?              RequestTimeout         = null)

    //    {

    //        return Task.FromResult(RemoteStopResult.OutOfService(SessionId));

    //    }

    //    #endregion


    //    #region (internal) SendNewChargingSession   (Timestamp, Sender, Session)

    //    internal void SendNewChargingSession(DateTime         Timestamp,
    //                                         Object           Sender,
    //                                         ChargingSession  Session)
    //    {

    //        if (Session != null)
    //        {

    //            //if (Session.ChargingStationOperator == null)
    //            //    Session.ChargingStationOperator = Id;

    //            OnNewChargingSession?.Invoke(Timestamp, Sender, Session);

    //        }

    //    }

    //    #endregion

    //    #region (internal) SendNewChargeDetailRecord(Timestamp, Sender, ChargeDetailRecord)

    //    internal void SendNewChargeDetailRecord(DateTime            Timestamp,
    //                                            Object              Sender,
    //                                            ChargeDetailRecord  ChargeDetailRecord)
    //    {

    //        if (ChargeDetailRecord != null)
    //            OnNewChargeDetailRecord?.Invoke(Timestamp, Sender, ChargeDetailRecord);

    //    }

    //    #endregion

    //    #endregion


    //    #region Dispose()

    //    /// <summary>
    //    /// Dispose this object.
    //    /// </summary>
    //    public void Dispose()
    //    { }

    //    #endregion

    //}

}
