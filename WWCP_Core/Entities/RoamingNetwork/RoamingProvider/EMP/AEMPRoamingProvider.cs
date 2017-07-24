///*
// * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace org.GraphDefined.WWCP
//{

//    /// <summary>
//    /// An abstract e-mobility roaming provider.
//    /// </summary>
//    public abstract class AEMPRoamingProvider : ABaseEMobilityEntity<EMPRoamingProvider_Id>,
//                                                IEMPRoamingProvider
//    {

//        #region Properties

//        public I18NString  Name { get;  }

//        EMPRoamingProvider_Id IEMPRoamingProvider.Id
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        I18NString IEMPRoamingProvider.Name
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        RoamingNetwork IEMPRoamingProvider.RoamingNetwork
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        #endregion

//        #region Events

//        // Client methods (logging)

//        #region OnReserveEVSERequest/-Response

//        /// <summary>
//        /// An event sent whenever a reserve EVSE command will be send.
//        /// </summary>
//        public abstract event OnReserveEVSERequestDelegate         OnReserveEVSERequest;

//        /// <summary>
//        /// An event sent whenever a reserve EVSE command was sent.
//        /// </summary>
//        public abstract event OnReserveEVSEResponseDelegate        OnReserveEVSEResponse;

//        #endregion

//        #region OnCancelReservationRequest/-Response

//        /// <summary>
//        /// An event sent whenever a cancel reservation command will be send.
//        /// </summary>
//        public abstract event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

//        /// <summary>
//        /// An event sent whenever a cancel reservation command was sent.
//        /// </summary>
//        public abstract event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

//        #endregion

//        #region OnRemoteStartEVSERequest/-Response

//        /// <summary>
//        /// An event sent whenever a remote start EVSE command will be send.
//        /// </summary>
//        public abstract event OnRemoteStartEVSERequestDelegate     OnRemoteStartEVSERequest;

//        /// <summary>
//        /// An event sent whenever a remote start EVSE command was sent.
//        /// </summary>
//        public abstract event OnRemoteStartEVSEResponseDelegate    OnRemoteStartEVSEResponse;

//        #endregion

//        #region OnRemoteStopEVSERequest/-Response

//        /// <summary>
//        /// An event sent whenever a remote stop EVSE command will be send.
//        /// </summary>
//        public abstract event OnRemoteStopEVSERequestDelegate      OnRemoteStopEVSERequest;

//        /// <summary>
//        /// An event sent whenever a remote stop EVSE command was sent.
//        /// </summary>
//        public abstract event OnRemoteStopEVSEResponseDelegate     OnRemoteStopEVSEResponse;

//        #endregion


//        // Server methods

//        #region OnAuthorizeStart/-StopEVSE

//        /// <summary>
//        /// An event sent whenever a authorize start command was received.
//        /// </summary>
//        public abstract event OnAuthorizeStartEVSEDelegate  OnAuthorizeStartEVSE;

//        /// <summary>
//        /// An event sent whenever a authorize start command was received.
//        /// </summary>
//        public abstract event OnAuthorizeStopEVSEDelegate   OnAuthorizeStopEVSE;

//        #endregion

//        #region OnChargeDetailRecord

//        /// <summary>
//        /// An event sent whenever a charge detail record was received.
//        /// </summary>
//        public abstract event OnChargeDetailRecordDelegate OnChargeDetailRecord;

//        #endregion

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create an e-mobility roaming provider.
//        /// </summary>
//        /// <param name="Id">The unique identification of the roaming provider.</param>
//        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
//        /// <param name="RoamingNetwork">The associated roaming network.</param>
//        protected AEMPRoamingProvider(EMPRoamingProvider_Id  Id,
//                                      I18NString             Name,
//                                      RoamingNetwork         RoamingNetwork)

//            : base(Id,
//                   RoamingNetwork)

//        {

//            this.Name = Name;

//            #region Link AuthorizeStart/-Stop and SendCDR to the roaming network

//            this.OnAuthorizeStartEVSE += (Timestamp,
//                                          CancellationToken,
//                                          EventTrackingId,
//                                          OperatorId,
//                                          AuthToken,
//                                          EVSEId,
//                                          ChargingProductId,
//                                          SessionId,
//                                          RequestTimeout) => RoamingNetwork.AuthorizeStart(AuthToken,
//                                                                                           EVSEId,
//                                                                                           ChargingProductId,
//                                                                                           SessionId,
//                                                                                           OperatorId,

//                                                                                           Timestamp,
//                                                                                           CancellationToken,
//                                                                                           EventTrackingId,
//                                                                                           RequestTimeout);

//            this.OnAuthorizeStopEVSE += (Timestamp,
//                                         CancellationToken,
//                                         EventTrackingId,
//                                         OperatorId,
//                                         EVSEId,
//                                         SessionId,
//                                         AuthToken,
//                                         RequestTimeout) => RoamingNetwork.AuthorizeStop(SessionId,
//                                                                                         AuthToken,
//                                                                                         EVSEId,
//                                                                                         OperatorId,

//                                                                                         Timestamp,
//                                                                                         CancellationToken,
//                                                                                         EventTrackingId,
//                                                                                         RequestTimeout);

//            this.OnChargeDetailRecord += (Timestamp,
//                                          CancellationToken,
//                                          EventTrackingId,
//                                          ChargeDetailRecord,
//                                          RequestTimeout) => RoamingNetwork.SendChargeDetailRecord(DateTime.Now,
//                                                                                                   Timestamp,
//                                                                                                   CancellationToken,
//                                                                                                   EventTrackingId,
//                                                                                                   ChargeDetailRecord,
//                                                                                                   RequestTimeout);

//            #endregion

//        }

//        event OnReserveEVSERequestDelegate IEMPRoamingProvider.OnReserveEVSERequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnReserveEVSEResponseDelegate IEMPRoamingProvider.OnReserveEVSEResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnCancelReservationRequestDelegate IEMPRoamingProvider.OnCancelReservationRequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnCancelReservationResponseDelegate IEMPRoamingProvider.OnCancelReservationResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStartEVSERequestDelegate IEMPRoamingProvider.OnRemoteStartEVSERequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStartEVSEResponseDelegate IEMPRoamingProvider.OnRemoteStartEVSEResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStopEVSERequestDelegate IEMPRoamingProvider.OnRemoteStopEVSERequest
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnRemoteStopEVSEResponseDelegate IEMPRoamingProvider.OnRemoteStopEVSEResponse
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnAuthorizeStartEVSEDelegate IEMPRoamingProvider.OnAuthorizeStartEVSE
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnAuthorizeStopEVSEDelegate IEMPRoamingProvider.OnAuthorizeStopEVSE
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }

//        event OnChargeDetailRecordDelegate IEMPRoamingProvider.OnChargeDetailRecord
//        {
//            add
//            {
//                throw new NotImplementedException();
//            }

//            remove
//            {
//                throw new NotImplementedException();
//            }
//        }


//        #endregion


//        #region Reserve(EVSEId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

//        /// <summary>
//        /// Reserve the possibility to charge at the given EVSE.
//        /// </summary>
//        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
//        /// <param name="StartTime">The starting time of the reservation.</param>
//        /// <param name="Duration">The duration of the reservation.</param>
//        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
//        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
//        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
//        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
//        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
//        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
//        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public abstract Task<ReservationResult>

//            Reserve(EVSE_Id                           EVSEId,
//                    DateTime?                         StartTime           = null,
//                    TimeSpan?                         Duration            = null,
//                    ChargingReservation_Id?           ReservationId       = null,
//                    eMobilityProvider_Id?             ProviderId          = null,
//                    eMobilityAccount_Id?              eMAId               = null,
//                    ChargingProduct_Id?               ChargingProductId   = null,
//                    IEnumerable<Auth_Token>           AuthTokens          = null,
//                    IEnumerable<eMobilityAccount_Id>  eMAIds              = null,
//                    IEnumerable<UInt32>               PINs                = null,

//                    DateTime?                         Timestamp           = null,
//                    CancellationToken?                CancellationToken   = null,
//                    EventTracking_Id                  EventTrackingId     = null,
//                    TimeSpan?                         RequestTimeout      = null);

//        #endregion

//        #region CancelReservation(ReservationId, Reason, ProviderId = null, EVSEId = null, ...)

//        /// <summary>
//        /// Try to remove the given charging reservation.
//        /// </summary>
//        /// <param name="ReservationId">The unique charging reservation identification.</param>
//        /// <param name="Reason">A reason for this cancellation.</param>
//        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
//        /// <param name="EVSEId">An optional identification of the EVSE.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public abstract Task<CancelReservationResult>

//            CancelReservation(ChargingReservation_Id                 ReservationId,
//                              ChargingReservationCancellationReason  Reason,
//                              eMobilityProvider_Id?                  ProviderId          = null,
//                              EVSE_Id?                               EVSEId              = null,

//                              DateTime?                              Timestamp           = null,
//                              CancellationToken?                     CancellationToken   = null,
//                              EventTracking_Id                       EventTrackingId     = null,
//                              TimeSpan?                              RequestTimeout      = null);

//        #endregion


//        #region RemoteStart(EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

//        /// <summary>
//        /// Start a charging session at the given EVSE.
//        /// </summary>
//        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
//        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
//        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
//        /// <param name="SessionId">The unique identification for this charging session.</param>
//        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
//        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public abstract Task<RemoteStartEVSEResult>

//            RemoteStart(EVSE_Id                  EVSEId,
//                        ChargingProduct_Id?      ChargingProductId   = null,
//                        ChargingReservation_Id?  ReservationId       = null,
//                        ChargingSession_Id?      SessionId           = null,
//                        eMobilityProvider_Id?    ProviderId          = null,
//                        eMobilityAccount_Id?     eMAId               = null,

//                        DateTime?                Timestamp           = null,
//                        CancellationToken?       CancellationToken   = null,
//                        EventTracking_Id         EventTrackingId     = null,
//                        TimeSpan?                RequestTimeout      = null);

//        #endregion

//        #region RemoteStop(EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

//        /// <summary>
//        /// Stop the given charging session at the given EVSE.
//        /// </summary>
//        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
//        /// <param name="SessionId">The unique identification for this charging session.</param>
//        /// <param name="ReservationHandling">Whether to remove the reservation after session end, or to keep it open for some more time.</param>
//        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
//        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public abstract Task<RemoteStopEVSEResult>

//            RemoteStop(EVSE_Id                EVSEId,
//                       ChargingSession_Id     SessionId,
//                       ReservationHandling    ReservationHandling,
//                       eMobilityProvider_Id?  ProviderId          = null,
//                       eMobilityAccount_Id?   eMAId               = null,

//                       DateTime?              Timestamp           = null,
//                       CancellationToken?     CancellationToken   = null,
//                       EventTracking_Id       EventTrackingId     = null,
//                       TimeSpan?              RequestTimeout      = null);

//        #endregion


//        #region GetChargeDetailRecords(From, To = null, ProviderId = null, ...)

//        /// <summary>
//        /// Download all charge detail records from the OICP server.
//        /// </summary>
//        /// <param name="From">The starting time.</param>
//        /// <param name="To">An optional end time. [default: current time].</param>
//        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
//        /// 
//        /// <param name="Timestamp">The optional timestamp of the request.</param>
//        /// <param name="CancellationToken">An optional token to cancel this request.</param>
//        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
//        /// <param name="RequestTimeout">An optional timeout for this request.</param>
//        public abstract Task<IEnumerable<ChargeDetailRecord>>

//            GetChargeDetailRecords(DateTime               From,
//                                   DateTime?              To                  = null,
//                                   eMobilityProvider_Id?  ProviderId          = null,

//                                   DateTime?              Timestamp           = null,
//                                   CancellationToken?     CancellationToken   = null,
//                                   EventTracking_Id       EventTrackingId     = null,
//                                   TimeSpan?              RequestTimeout      = null);

//        #endregion


//        #region IComparable<RoamingProvider> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException("The given object must not be null!");

//            // Check if the given object is a roaming provider.
//            var RoamingProvider = Object as AEMPRoamingProvider;
//            if ((Object) RoamingProvider == null)
//                throw new ArgumentException("The given object is not a roaming provider!");

//            return CompareTo(RoamingProvider);

//        }

//        #endregion

//        #region CompareTo(RoamingProvider)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="RoamingProvider">A roaming provider object to compare with.</param>
//        public Int32 CompareTo(AEMPRoamingProvider RoamingProvider)
//        {

//            if ((Object) RoamingProvider == null)
//                throw new ArgumentNullException("The given roaming provider must not be null!");

//            return Id.CompareTo(RoamingProvider.Id);

//        }

//        #endregion

//        #endregion

//        #region IEquatable<RoamingProvider> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>true|false</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            // Check if the given object is a roaming provider.
//            var RoamingProvider = Object as AEMPRoamingProvider;
//            if ((Object) RoamingProvider == null)
//                return false;

//            return this.Equals(RoamingProvider);

//        }

//        #endregion

//        #region Equals(RoamingProvider)

//        /// <summary>
//        /// Compares two roaming provider for equality.
//        /// </summary>
//        /// <param name="RoamingProvider">A roaming provider to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(AEMPRoamingProvider RoamingProvider)
//        {

//            if ((Object) RoamingProvider == null)
//                return false;

//            return Id.Equals(RoamingProvider.Id);

//        }

//        #endregion

//        #endregion

//        #region GetHashCode()

//        /// <summary>
//        /// Get the hashcode of this object.
//        /// </summary>
//        public override Int32 GetHashCode()
//            => Id.GetHashCode();

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a string representation of this object.
//        /// </summary>
//        public override String ToString()
//            => Id.ToString();

//        #endregion

//        Task<IEnumerable<ChargeDetailRecord>> IEMPRoamingProvider.GetChargeDetailRecords(DateTime From, DateTime? To, eMobilityProvider_Id? ProviderId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<ReservationResult> IReserveRemoteStartStop.Reserve(EVSE_Id EVSEId, DateTime? StartTime, TimeSpan? Duration, ChargingReservation_Id? ReservationId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, ChargingProduct_Id? ChargingProductId, IEnumerable<Auth_Token> AuthTokens, IEnumerable<eMobilityAccount_Id> eMAIds, IEnumerable<uint> PINs, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<CancelReservationResult> IReserveRemoteStartStop.CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, eMobilityProvider_Id? ProviderId, EVSE_Id? EVSEId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStartEVSEResult> IReserveRemoteStartStop.RemoteStart(EVSE_Id EVSEId, ChargingProduct_Id? ChargingProductId, ChargingReservation_Id? ReservationId, ChargingSession_Id? SessionId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStartChargingStationResult> IReserveRemoteStartStop.RemoteStart(ChargingStation_Id ChargingStationId, ChargingProduct_Id? ChargingProductId, ChargingReservation_Id? ReservationId, ChargingSession_Id? SessionId, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStopResult> IReserveRemoteStartStop.RemoteStop(ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStopEVSEResult> IReserveRemoteStartStop.RemoteStop(EVSE_Id EVSEId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//        Task<RemoteStopChargingStationResult> IReserveRemoteStartStop.RemoteStop(ChargingStation_Id ChargingStationId, ChargingSession_Id SessionId, ReservationHandling ReservationHandling, eMobilityProvider_Id? ProviderId, eMobilityAccount_Id? eMAId, DateTime? Timestamp, CancellationToken? CancellationToken, EventTracking_Id EventTrackingId, TimeSpan? RequestTimeout)
//        {
//            throw new NotImplementedException();
//        }

//    }

//}
