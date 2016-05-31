/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
using org.GraphDefined.Vanaheimr.Illias;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A e-mobility roaming provider.
    /// </summary>
    public class EMPRoamingProvider : ARoamingProvider,
                                      IEMPRoamingService
    {

        #region Properties

        #region eMobilityRoamingService

        private readonly IEMPRoamingService _EMPRoamingService;

        public IEMPRoamingService eMobilityRoamingService
        {
            get
            {
                return _EMPRoamingService;
            }
        }

        #endregion


        public Authorizator_Id AuthorizatorId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RoamingNetwork_Id RoamingNetworkId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Events

        // Client methods (logging)


        // Server methods

        #region OnAuthorizeStart/-Stop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStartEVSEDelegate OnAuthorizeStartEVSE
        {

            add
            {
                _EMPRoamingService.OnAuthorizeStartEVSE += value;
            }

            remove
            {
                _EMPRoamingService.OnAuthorizeStartEVSE -= value;
            }

        }

        #endregion

        #region OnAuthorizeStop

        /// <summary>
        /// An event sent whenever a authorize start command was received.
        /// </summary>
        public event OnAuthorizeStopEVSEDelegate OnAuthorizeStopEVSE
        {

            add
            {
                _EMPRoamingService.OnAuthorizeStopEVSE += value;
            }

            remove
            {
                _EMPRoamingService.OnAuthorizeStopEVSE -= value;
            }

        }

        #endregion

        #region OnChargeDetailRecord

        /// <summary>
        /// An event sent whenever a charge detail record was received.
        /// </summary>
        public event OnChargeDetailRecordDelegate OnChargeDetailRecord
        {

            add
            {
                _EMPRoamingService.OnChargeDetailRecord += value;
            }

            remove
            {
                _EMPRoamingService.OnChargeDetailRecord -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an e-mobility roaming provider.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        /// <param name="EMPRoamingService">The attached local or remote e-mobility roaming service.</param>
        internal EMPRoamingProvider(RoamingProvider_Id  Id,
                                    I18NString          Name,
                                    RoamingNetwork      RoamingNetwork,
                                    IEMPRoamingService  EMPRoamingService)

            : base(Id, Name, RoamingNetwork)

        {

            #region Initial Checks

            if (EMPRoamingService == null)
                throw new ArgumentNullException(nameof(EMPRoamingService),  "The given e-mobility roaming service must not be null!");

            #endregion


            this._EMPRoamingService  = EMPRoamingService;


            #region Link AuthorizeStart/-Stop and SendCDR to the roaming network

            this.OnAuthorizeStartEVSE += (Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          OperatorId,
                                          AuthToken,
                                          EVSEId,
                                          ChargingProductId,
                                          SessionId,
                                          QueryTimeout) => RoamingNetwork.AuthorizeStart(Timestamp,
                                                                                         CancellationToken,
                                                                                         EventTrackingId,
                                                                                         OperatorId,
                                                                                         AuthToken,
                                                                                         EVSEId,
                                                                                         ChargingProductId,
                                                                                         SessionId,
                                                                                         QueryTimeout);

            this.OnAuthorizeStopEVSE += (Timestamp,
                                         CancellationToken,
                                         EventTrackingId,
                                         OperatorId,
                                         EVSEId,
                                         SessionId,
                                         AuthToken,
                                         QueryTimeout) => RoamingNetwork.AuthorizeStop(Timestamp,
                                                                                       CancellationToken,
                                                                                       EventTrackingId,
                                                                                       OperatorId,
                                                                                       SessionId,
                                                                                       AuthToken,
                                                                                       EVSEId,
                                                                                       QueryTimeout);

            this.OnChargeDetailRecord += (Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          ChargeDetailRecord,
                                          QueryTimeout) => RoamingNetwork.SendChargeDetailRecord(Timestamp,
                                                                                                 CancellationToken,
                                                                                                 EventTrackingId,
                                                                                                 ChargeDetailRecord,
                                                                                                 QueryTimeout);

            #endregion

        }


        #endregion


        #region Reserve(...EVSEId, StartTime, Duration, ReservationId = null, ProviderId = null, ...)

        /// <summary>
        /// Reserve the possibility to charge at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be reserved.</param>
        /// <param name="StartTime">The starting time of the reservation.</param>
        /// <param name="Duration">The duration of the reservation.</param>
        /// <param name="ReservationId">An optional unique identification of the reservation. Mandatory for updates.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="eMAId">An optional unique identification of e-Mobility account/customer requesting this reservation.</param>
        /// <param name="ChargingProductId">An optional unique identification of the charging product to be reserved.</param>
        /// <param name="AuthTokens">A list of authentication tokens, who can use this reservation.</param>
        /// <param name="eMAIds">A list of eMobility account identifications, who can use this reservation.</param>
        /// <param name="PINs">A list of PINs, who can be entered into a pinpad to use this reservation.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<ReservationResult>

            Reserve(DateTime                 Timestamp,
                    CancellationToken        CancellationToken,
                    EventTracking_Id         EventTrackingId,
                    EVSE_Id                  EVSEId,
                    DateTime?                StartTime          = null,
                    TimeSpan?                Duration           = null,
                    ChargingReservation_Id   ReservationId      = null,
                    EVSP_Id                  ProviderId         = null,
                    eMA_Id                   eMAId              = null,
                    ChargingProduct_Id       ChargingProductId  = null,
                    IEnumerable<Auth_Token>  AuthTokens         = null,
                    IEnumerable<eMA_Id>      eMAIds             = null,
                    IEnumerable<UInt32>      PINs               = null,
                    TimeSpan?                QueryTimeout       = null)

        {

            return await this._EMPRoamingService.Reserve(Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         EVSEId,
                                                         StartTime,
                                                         Duration,
                                                         ReservationId,
                                                         ProviderId,
                                                         eMAId,
                                                         ChargingProductId,
                                                         AuthTokens,
                                                         eMAIds,
                                                         PINs,
                                                         QueryTimeout);

        }

        #endregion

        #region CancelReservation(...ReservationId, Reason, ProviderId = null, ...)

        /// <summary>
        /// Try to remove the given charging reservation.
        /// </summary>
        /// <param name="Timestamp">The timestamp of this request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ReservationId">The unique charging reservation identification.</param>
        /// <param name="Reason">A reason for this cancellation.</param>
        /// <param name="ProviderId">An optional unique identification of e-Mobility service provider.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<CancelReservationResult>

            CancelReservation(DateTime                               Timestamp,
                              CancellationToken                      CancellationToken,
                              EventTracking_Id                       EventTrackingId,
                              ChargingReservation_Id                 ReservationId,
                              ChargingReservationCancellationReason  Reason,
                              EVSP_Id                                ProviderId    = null,
                              TimeSpan?                              QueryTimeout  = null)

        {

            return await this._EMPRoamingService.CancelReservation(Timestamp,
                                                                   CancellationToken,
                                                                   EventTrackingId,
                                                                   ReservationId,
                                                                   Reason,
                                                                   ProviderId,
                                                                   QueryTimeout);

        }

        #endregion


        #region RemoteStart(...EVSEId, ChargingProductId = null, ReservationId = null, SessionId = null, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Start a charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be started.</param>
        /// <param name="ChargingProductId">The unique identification of the choosen charging product.</param>
        /// <param name="ReservationId">The unique identification for a charging reservation.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider for the case it is different from the current message sender.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStartEVSEResult> RemoteStart(DateTime                Timestamp,
                                                             CancellationToken       CancellationToken,
                                                             EventTracking_Id        EventTrackingId,
                                                             EVSE_Id                 EVSEId,
                                                             ChargingProduct_Id      ChargingProductId  = null,
                                                             ChargingReservation_Id  ReservationId      = null,
                                                             ChargingSession_Id      SessionId          = null,
                                                             EVSP_Id                 ProviderId         = null,
                                                             eMA_Id                  eMAId              = null,
                                                             TimeSpan?               QueryTimeout       = default(TimeSpan?))
        {

            return await this._EMPRoamingService.RemoteStart(Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             EVSEId,
                                                             ChargingProductId,
                                                             ReservationId,
                                                             SessionId,
                                                             ProviderId,
                                                             eMAId,
                                                             QueryTimeout);

        }

        #endregion

        #region RemoteStop(...EVSEId, SessionId, ReservationHandling, ProviderId = null, eMAId = null, ...)

        /// <summary>
        /// Stop the given charging session at the given EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The unique identification of the EVSE to be stopped.</param>
        /// <param name="SessionId">The unique identification for this charging session.</param>
        /// <param name="ReservationHandling">Wether to remove the reservation after session end, or to keep it open for some more time.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility service provider.</param>
        /// <param name="eMAId">The unique identification of the e-mobility account.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<RemoteStopEVSEResult>

            RemoteStop(DateTime             Timestamp,
                       CancellationToken    CancellationToken,
                       EventTracking_Id     EventTrackingId,
                       EVSE_Id              EVSEId,
                       ChargingSession_Id   SessionId,
                       ReservationHandling  ReservationHandling,
                       EVSP_Id              ProviderId    = null,
                       eMA_Id               eMAId         = null,
                       TimeSpan?            QueryTimeout  = null)

        {

            return await this._EMPRoamingService.RemoteStop(Timestamp,
                                                             CancellationToken,
                                                             EventTrackingId,
                                                             EVSEId,
                                                             SessionId,
                                                             ReservationHandling,
                                                             ProviderId,
                                                             eMAId,
                                                             QueryTimeout);

        }

        #endregion



        #region IComparable<RoamingProvider> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a roaming provider.
            var RoamingProvider = Object as EMPRoamingProvider;
            if ((Object) RoamingProvider == null)
                throw new ArgumentException("The given object is not a roaming provider!");

            return CompareTo(RoamingProvider);

        }

        #endregion

        #region CompareTo(RoamingProvider)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingProvider">A roaming provider object to compare with.</param>
        public Int32 CompareTo(EMPRoamingProvider RoamingProvider)
        {

            if ((Object) RoamingProvider == null)
                throw new ArgumentNullException("The given roaming provider must not be null!");

            return Id.CompareTo(RoamingProvider.Id);

        }

        #endregion

        #endregion

        #region IEquatable<RoamingProvider> Members

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

            // Check if the given object is a roaming provider.
            var RoamingProvider = Object as EMPRoamingProvider;
            if ((Object) RoamingProvider == null)
                return false;

            return this.Equals(RoamingProvider);

        }

        #endregion

        #region Equals(RoamingProvider)

        /// <summary>
        /// Compares two roaming provider for equality.
        /// </summary>
        /// <param name="RoamingProvider">A roaming provider to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMPRoamingProvider RoamingProvider)
        {

            if ((Object) RoamingProvider == null)
                return false;

            return Id.Equals(RoamingProvider.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return Id.ToString();
        }

        #endregion

    }

}

