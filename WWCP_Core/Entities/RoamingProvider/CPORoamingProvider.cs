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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A e-Mobility Roaming Provider (EMRP).
    /// </summary>
    public class CPORoamingProvider : ARoamingProvider,
                                      IOperatorRoamingService
    {

        #region Properties

        #region OperatorRoamingService

        private readonly IOperatorRoamingService _OperatorRoamingService;

        /// <summary>
        /// The wrapped operator roaming service.
        /// </summary>
        public IOperatorRoamingService OperatorRoamingService
        {
            get
            {
                return _OperatorRoamingService;
            }
        }

        #endregion

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE data will be send upstream.
        /// </summary>
        public event OnEVSEDataPushDelegate OnEVSEDataPush
        {

            add
            {
                _OperatorRoamingService.OnEVSEDataPush += value;
            }

            remove
            {
                _OperatorRoamingService.OnEVSEDataPush -= value;
            }

        }

        /// <summary>
        /// An event fired whenever new EVSE data had been sent upstream.
        /// </summary>
        public event OnEVSEDataPushedDelegate OnEVSEDataPushed
        {

            add
            {
                _OperatorRoamingService.OnEVSEDataPushed += value;
            }

            remove
            {
                _OperatorRoamingService.OnEVSEDataPushed -= value;
            }

        }

        #endregion

        #region OnEVSEStatusPush/-Pushed

        /// <summary>
        /// An event fired whenever new EVSE status will be send upstream.
        /// </summary>
        public event OnEVSEStatusPushDelegate OnEVSEStatusPush
        {

            add
            {
                _OperatorRoamingService.OnEVSEStatusPush += value;
            }

            remove
            {
                _OperatorRoamingService.OnEVSEStatusPush -= value;
            }

        }

        /// <summary>
        /// An event fired whenever new EVSE status had been sent upstream.
        /// </summary>
        public event OnEVSEStatusPushedDelegate OnEVSEStatusPushed
        {

            add
            {
                _OperatorRoamingService.OnEVSEStatusPushed += value;
            }

            remove
            {
                _OperatorRoamingService.OnEVSEStatusPushed -= value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-Mobility Roaming Provider (EMRP)
        /// having the given unique roaming provider identification and name.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        /// <param name="OperatorRoamingService">The attached local or remote EVSE operator roaming service.</param>
        /// <param name="eMobilityRoamingService">The attached local or remote e-mobility roaming service.</param>
        public CPORoamingProvider(RoamingProvider_Id        Id,
                                  I18NString                Name,
                                  RoamingNetwork            RoamingNetwork,
                                  IOperatorRoamingService   OperatorRoamingService)

            : base(Id, Name, RoamingNetwork)

        {

            #region Initial Checks

            if (OperatorRoamingService == null)
                throw new ArgumentNullException(nameof(OperatorRoamingService),  "The given EVSE operator roaming service must not be null!");

            #endregion

            this._OperatorRoamingService   = OperatorRoamingService;

        }

        #endregion


        #region PushEVSEData

        #endregion

        #region PushEVSEStatus

        public Task<Acknowledgement> PushEVSEStatus(ILookup<EVSEOperator, EVSE> GroupedEVSEs, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(EVSE EVSE, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSE> EVSEs, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(ChargingStation ChargingStation, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingStation> ChargingStations, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(ChargingPool ChargingPool, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<ChargingPool> ChargingPools, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(EVSEOperator EVSEOperator, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(IEnumerable<EVSEOperator> EVSEOperators, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> PushEVSEStatus(RoamingNetwork RoamingNetwork, ActionType ActionType = ActionType.update, EVSEOperator_Id OperatorId = null, string OperatorName = null, Func<EVSE, bool> IncludeEVSEs = null, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task PushEVSEStatus(EVSEStatusDiff EVSEStatusDiff, TimeSpan? QueryTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        #endregion

        #region AuthorizeStart/-Stop...

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(DateTime             Timestamp,
                           CancellationToken    CancellationToken,
                           EventTracking_Id     EventTrackingId,
                           EVSEOperator_Id      OperatorId,
                           Auth_Token           AuthToken,
                           ChargingProduct_Id   ChargingProductId  = null,
                           ChargingSession_Id   SessionId          = null,
                           TimeSpan?            QueryTimeout       = null)

        {

            return await OperatorRoamingService.AuthorizeStart(Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               OperatorId,
                                                               AuthToken,
                                                               ChargingProductId,
                                                               SessionId,
                                                               QueryTimeout);

        }

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, EVSEId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(DateTime             Timestamp,
                           CancellationToken    CancellationToken,
                           EventTracking_Id     EventTrackingId,
                           EVSEOperator_Id      OperatorId,
                           Auth_Token           AuthToken,
                           EVSE_Id              EVSEId,
                           ChargingProduct_Id   ChargingProductId  = null,
                           ChargingSession_Id   SessionId          = null,
                           TimeSpan?            QueryTimeout       = null)

        {

            return await OperatorRoamingService.AuthorizeStart(Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               OperatorId,
                                                               AuthToken,
                                                               EVSEId,
                                                               ChargingProductId,
                                                               SessionId,
                                                               QueryTimeout);

        }

        #endregion

        #region AuthorizeStart(...OperatorId, AuthToken, ChargingStationId, ChargingProductId = null, SessionId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE operator identification.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="ChargingProductId">An optional charging product identification.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(DateTime             Timestamp,
                           CancellationToken    CancellationToken,
                           EventTracking_Id     EventTrackingId,
                           EVSEOperator_Id      OperatorId,
                           Auth_Token           AuthToken,
                           ChargingStation_Id   ChargingStationId,
                           ChargingProduct_Id   ChargingProductId  = null,
                           ChargingSession_Id   SessionId          = null,
                           TimeSpan?            QueryTimeout       = null)

        {

            return await OperatorRoamingService.AuthorizeStart(Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               OperatorId,
                                                               AuthToken,
                                                               ChargingStationId,
                                                               ChargingProductId,
                                                               SessionId,
                                                               QueryTimeout);

        }

        #endregion


        #region AuthorizeStop(...OperatorId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)

        {

            return await OperatorRoamingService.AuthorizeStop(Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              OperatorId,
                                                              SessionId,
                                                              AuthToken,
                                                              QueryTimeout);

        }

        #endregion

        #region AuthorizeStop(...OperatorId, EVSEId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          EVSE_Id             EVSEId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)

        {

            return await OperatorRoamingService.AuthorizeStop(Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              OperatorId,
                                                              EVSEId,
                                                              SessionId,
                                                              AuthToken,
                                                              QueryTimeout);

        }

        #endregion

        #region AuthorizeStop(...OperatorId, ChargingStationId, SessionId, AuthToken, ...)

        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="OperatorId">An EVSE Operator identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="SessionId">The OICP session identification from the AuthorizeStart request.</param>
        /// <param name="AuthToken">A (RFID) user identification.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(DateTime            Timestamp,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,
                          EVSEOperator_Id     OperatorId,
                          ChargingStation_Id  ChargingStationId,
                          ChargingSession_Id  SessionId,
                          Auth_Token          AuthToken,
                          TimeSpan?           QueryTimeout  = null)

        {

            return await OperatorRoamingService.AuthorizeStop(Timestamp,
                                                              CancellationToken,
                                                              EventTrackingId,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              AuthToken,
                                                              QueryTimeout);

        }

        #endregion

        #endregion

        #region SendChargeDetailRecord...

        #region SendChargeDetailRecord(...ChargeDetailRecord, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime            Timestamp,
                                   CancellationToken   CancellationToken,
                                   EventTracking_Id    EventTrackingId,
                                   ChargeDetailRecord  ChargeDetailRecord,
                                   TimeSpan?           QueryTimeout = null)

        {

            return await _OperatorRoamingService.SendChargeDetailRecord(Timestamp,
                                                                        CancellationToken,
                                                                        EventTrackingId,
                                                                        ChargeDetailRecord,
                                                                        QueryTimeout);

        }

        #endregion

        #region SendChargeDetailRecord(...EVSEId, SessionId, PartnerProductId, SessionStart, SessionEnd, Identification, ...)

        /// <summary>
        /// Send a charge detail record.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the request.</param>
        /// <param name="CancellationToken">A token to cancel this request.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="SessionId">The OICP session identification from the Authorize Start request.</param>
        /// <param name="PartnerProductId">The ev charging product identification.</param>
        /// <param name="SessionStart">The session start timestamp.</param>
        /// <param name="SessionEnd">The session end timestamp.</param>
        /// <param name="Identification">An identification.</param>
        /// <param name="ChargingStart">An optional charging start timestamp.</param>
        /// <param name="ChargingEnd">An optional charging end timestamp.</param>
        /// <param name="MeterValueStart">An optional initial value of the energy meter.</param>
        /// <param name="MeterValueEnd">An optional final value of the energy meter.</param>
        /// <param name="MeterValuesInBetween">An optional enumeration of meter values during the charging session.</param>
        /// <param name="ConsumedEnergy">The optional amount of consumed energy.</param>
        /// <param name="MeteringSignature">An optional signature for the metering values.</param>
        /// <param name="QueryTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(DateTime             Timestamp,
                                   CancellationToken    CancellationToken,
                                   EventTracking_Id     EventTrackingId,
                                   EVSE_Id              EVSEId,
                                   ChargingSession_Id   SessionId,
                                   ChargingProduct_Id   PartnerProductId,
                                   DateTime             SessionStart,
                                   DateTime             SessionEnd,
                                   AuthInfo             Identification,
                                   DateTime?            ChargingStart         = null,
                                   DateTime?            ChargingEnd           = null,
                                   Double?              MeterValueStart       = null,
                                   Double?              MeterValueEnd         = null,
                                   IEnumerable<Double>  MeterValuesInBetween  = null,
                                   Double?              ConsumedEnergy        = null,
                                   String               MeteringSignature     = null,
                                   TimeSpan?            QueryTimeout          = null)

        {

            return await _OperatorRoamingService.SendChargeDetailRecord(Timestamp,
                                                                        CancellationToken,
                                                                        EventTrackingId,
                                                                        EVSEId,
                                                                        SessionId,
                                                                        PartnerProductId,
                                                                        SessionStart,
                                                                        SessionEnd,
                                                                        Identification,
                                                                        ChargingStart,
                                                                        ChargingEnd,
                                                                        MeterValueStart,
                                                                        MeterValueEnd,
                                                                        MeterValuesInBetween,
                                                                        ConsumedEnergy,
                                                                        MeteringSignature,
                                                                        QueryTimeout);

        }

        #endregion

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

