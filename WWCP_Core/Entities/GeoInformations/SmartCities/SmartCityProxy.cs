/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The smart city is not only the main contract party of the EV driver,
    /// the smart city also takes care of the EV driver master data,
    /// the authentication and autorisation process before charging and for the
    /// billing process after charging.
    /// The smart city provides the EV drivere one or multiple methods for
    /// authentication (e.g. based on RFID cards, login/passwords, client certificates).
    /// The smart city takes care that none of the provided authentication
    /// methods can be misused by any entity in the ev charging process to track the
    /// ev driver or its behaviour.
    /// </summary>
    public class SmartCityProxy : ABaseEMobilityEntity<SmartCity_Id>,
                                  ISend2RemoteSmartCity,
                                  IEquatable <SmartCityProxy>,
                                  IComparable<SmartCityProxy>,
                                  IComparable
    {

        #region Data

        /// <summary>
        /// The default max size of the admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize   = 15;

        /// <summary>
        /// The default max size of the status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize        = 15;

        #endregion

        #region Properties

        //public Authorizator_Id AuthorizatorId { get; }

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the EVSE Operator.
        /// </summary>
        [Mandatory]
        public I18NString Name
        {

            get
            {
                return _Name;
            }

            set
            {

                if (value == null)
                    value = new I18NString();

                if (_Name != value)
                    SetProperty(ref _Name, value);

            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of the EVSE Operator.
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

                if (value == null)
                    value = new I18NString();

                if (_Description != value)
                    SetProperty(ref _Description, value);

            }

        }

        #endregion

        #region Logo

        private String _Logo;

        /// <summary>
        /// The logo of this evse operator.
        /// </summary>
        [Optional]
        public String Logo
        {

            get
            {
                return _Logo;
            }

            set
            {
                if (_Logo != value)
                    SetProperty(ref _Logo, value);
            }

        }

        #endregion

        #region Address

        private Address _Address;

        /// <summary>
        /// The address of the operators headquarter.
        /// </summary>
        [Optional]
        public Address Address
        {

            get
            {
                return _Address;
            }

            set
            {

                if (value == null)
                    _Address = value;

                if (_Address != value)
                    SetProperty(ref _Address, value);

            }

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this operator.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {
                return _GeoLocation;
            }

            set
            {

                if (value == null)
                    value = new GeoCoordinate(Latitude.Parse(0), Longitude.Parse(0));

                if (_GeoLocation != value)
                    SetProperty(ref _GeoLocation, value);

            }

        }

        #endregion

        #region Telephone

        private String _Telephone;

        /// <summary>
        /// The telephone number of the operator's (sales) office.
        /// </summary>
        [Optional]
        public String Telephone
        {

            get
            {
                return _Telephone;
            }

            set
            {
                if (_Telephone != value)
                    SetProperty(ref _Telephone, value);
            }

        }

        #endregion

        #region EMailAddress

        private String _EMailAddress;

        /// <summary>
        /// The e-mail address of the operator's (sales) office.
        /// </summary>
        [Optional]
        public String EMailAddress
        {

            get
            {
                return _EMailAddress;
            }

            set
            {
                if (_EMailAddress != value)
                    SetProperty(ref _EMailAddress, value);
            }

        }

        #endregion

        #region Homepage

        private String _Homepage;

        /// <summary>
        /// The homepage of this evse operator.
        /// </summary>
        [Optional]
        public String Homepage
        {

            get
            {
                return _Homepage;
            }

            set
            {
                if (_Homepage != value)
                    SetProperty(ref _Homepage, value);
            }

        }

        #endregion

        #region HotlinePhoneNumber

        private String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the Charging Station Operator hotline.
        /// </summary>
        [Optional]
        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber;
            }

            set
            {
                if (_HotlinePhoneNumber != value)
                    SetProperty(ref _HotlinePhoneNumber, value);
            }

        }

        #endregion


        #region DataLicense

        private List<DataLicense> _DataLicenses;

        /// <summary>
        /// The license of the charging station operator data.
        /// </summary>
        [Mandatory]
        public IEnumerable<DataLicense> DataLicenses
            => _DataLicenses;

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current admin status.
        /// </summary>
        [Optional]
        public Timestamped<SmartCityAdminStatusType> AdminStatus

            => _AdminStatusSchedule.CurrentStatus;

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<SmartCityAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<SmartCityAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current status.
        /// </summary>
        [Optional]
        public Timestamped<SmartCityStatusType> Status

            => _StatusSchedule.CurrentStatus;

        #endregion

        #region StatusSchedule

        private StatusSchedule<SmartCityStatusType> _StatusSchedule;

        /// <summary>
        /// The status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<SmartCityStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion


        public SmartCityPriority Priority { get; set; }


        //#region AllTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AllTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.AllTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        //#region AuthorizedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> AuthorizedTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.AuthorizedTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        //#region NotAuthorizedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> NotAuthorizedTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.NotAuthorizedTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        //#region BlockedTokens

        //public IEnumerable<KeyValuePair<Auth_Token, TokenAuthorizationResultType>> BlockedTokens

        //    => RemoteSmartCity != null
        //           ? RemoteSmartCity.BlockedTokens
        //           : new KeyValuePair<Auth_Token, TokenAuthorizationResultType>[0];

        //#endregion

        #endregion

        #region Links

        /// <summary>
        /// The remote smart city.
        /// </summary>
        public IRemoteSmartCity          RemoteSmartCity    { get; }

        #endregion

        #region Events

        #region OnEVSEDataPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE data will be send upstream.
        ///// </summary>
        //public event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE data had been sent upstream.
        ///// </summary>
        //public event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse;

        #endregion

        #region OnEVSEStatusPush/-Pushed

        ///// <summary>
        ///// An event fired whenever new EVSE status will be send upstream.
        ///// </summary>
        //public event OnPushEVSEStatusRequestDelegate OnPushEVSEStatusRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE status had been sent upstream.
        ///// </summary>
        //public event OnPushEVSEStatusResponseDelegate OnPushEVSEStatusResponse;

        #endregion


        #region OnReserve... / OnReserved...

        /// <summary>
        /// An event fired whenever an EVSE is being reserved.
        /// </summary>
        public event OnReserveEVSERequestDelegate              OnReserveEVSE;

        /// <summary>
        /// An event fired whenever an EVSE was reserved.
        /// </summary>
        public event OnReserveEVSEResponseDelegate             OnEVSEReserved;

        #endregion

        #region OnRemote...Start / OnRemote...Started

        /// <summary>
        /// An event fired whenever a remote start EVSE command was received.
        /// </summary>
        public event OnRemoteStartEVSERequestDelegate               OnRemoteEVSEStart;

        /// <summary>
        /// An event fired whenever a remote start EVSE command completed.
        /// </summary>
        public event OnRemoteStartEVSEResponseDelegate             OnRemoteEVSEStarted;

        #endregion

        #region OnRemote...Stop / OnRemote...Stopped

        /// <summary>
        /// An event fired whenever a remote stop EVSE command was received.
        /// </summary>
        public event OnRemoteStopEVSERequestDelegate                OnRemoteEVSEStop;

        /// <summary>
        /// An event fired whenever a remote stop EVSE command completed.
        /// </summary>
        public event OnRemoteStopEVSEResponseDelegate             OnRemoteEVSEStopped;

        #endregion

        // CancelReservation

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility (service) provider having the given
        /// unique identification.
        /// </summary>
        /// <param name="Id">The unique smart city identification.</param>
        /// <param name="Name">The offical (multi-language) name of the smart city.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal SmartCityProxy(SmartCity_Id                    Id,
                               I18NString                      Name,
                               RoamingNetwork                  RoamingNetwork,
                               I18NString                      Description              = null,
                               Action<SmartCityProxy>           Configurator             = null,
                               RemoteSmartCityCreatorDelegate  RemoteSmartCityCreator   = null,
                               SmartCityPriority               Priority                 = null,
                               SmartCityAdminStatusType        AdminStatus              = SmartCityAdminStatusType.Available,
                               SmartCityStatusType             Status                   = SmartCityStatusType.Available,
                               UInt16                          MaxAdminStatusListSize   = DefaultMaxAdminStatusListSize,
                               UInt16                          MaxStatusListSize        = DefaultMaxStatusListSize)

            : base(Id,
                   RoamingNetwork)

        {

            #region Initial checks

            if (IEnumerableExtensions.IsNullOrEmpty(Name))
                throw new ArgumentNullException(nameof(Name), "The given smart city name must not be null or empty!");

            #endregion

            #region Init data and properties

            this._Name                        = Name        ?? new I18NString();
            this._Description                 = Description ?? new I18NString();
            this._DataLicenses                = new List<DataLicense>();

            this.Priority                     = Priority    ?? new SmartCityPriority(0);

            this._AdminStatusSchedule         = new StatusSchedule<SmartCityAdminStatusType>();
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule              = new StatusSchedule<SmartCityStatusType>();
            this._StatusSchedule.Insert(Status);

            #endregion

            Configurator?.Invoke(this);

            this.RemoteSmartCity = RemoteSmartCityCreator?.Invoke(this);

        }

        #endregion


        #region PushEVSEData/-Status directly...

        #region DisablePushData

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushData                  { get; set; }

        #endregion

        #region DisablePushAdminStatus

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushAdminStatus           { get; set; }

        #endregion

        #region DisablePushStatus

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushStatus                { get; set; }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the given EVSE as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(EVSE                EVSE,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(EVSE                EVSE,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="OldValue">The old value of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(EVSE                EVSE,
                                             String              PropertyName,
                                             Object              OldValue,
                                             Object              NewValue,
                                             TransmissionTypes   TransmissionType,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(EVSE                EVSE,
                                             TransmissionTypes   TransmissionType,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion


        #region SetStaticData   (EVSEs, ...)

        /// <summary>
        /// Set the given enumeration of EVSEs as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region AddStaticData   (EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region UpdateStaticData(EVSEs, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region DeleteStaticData(EVSEs, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                   TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id                    EventTrackingId,
                                               TimeSpan?                           RequestTimeout)


        {

            return Task.FromResult(PushAdminStatusResult.OutOfService(Id,
                                                                      this,
                                                                      AdminStatusUpdates));

        }

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                     TransmissionTypes              TransmissionType,

                                     DateTime?                      Timestamp,
                                     CancellationToken?             CancellationToken,
                                     EventTracking_Id               EventTrackingId,
                                     TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return Task.FromResult(PushStatusResult.NoOperation(Id, this));

            #endregion

            return Task.FromResult(PushStatusResult.NoOperation(Id, this));

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="OldValue">The old value of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStation     ChargingStation,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStation     ChargingStation,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion


        #region SetStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region AddStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,


                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                              TransmissionType,

                                               DateTime?                                      Timestamp,
                                               CancellationToken?                             CancellationToken,
                                               EventTracking_Id                               EventTrackingId,
                                               TimeSpan?                                      RequestTimeout)


        {

            return Task.FromResult(PushAdminStatusResult.OutOfService(Id,
                                                                      this,
                                                                      AdminStatusUpdates));

        }

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                         TransmissionType,

                                     DateTime?                                 Timestamp,
                                     CancellationToken?                        CancellationToken,
                                     EventTracking_Id                          EventTrackingId,
                                     TimeSpan?                                 RequestTimeout)


                => Task.FromResult(PushStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="OldValue">The old value of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingPool        ChargingPool,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingPool        ChargingPool,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion


        #region SetStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region AddStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                           TransmissionType,

                                               DateTime?                                   Timestamp,
                                               CancellationToken?                          CancellationToken,
                                               EventTracking_Id                            EventTrackingId,
                                               TimeSpan?                                   RequestTimeout)
        {

            return Task.FromResult(PushAdminStatusResult.OutOfService(Id,
                                                                      this,
                                                                      AdminStatusUpdates));

        }

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType,

                                     DateTime?                              Timestamp,
                                     CancellationToken?                     CancellationToken,
                                     EventTracking_Id                       EventTrackingId,
                                     TimeSpan?                              RequestTimeout)


                => Task.FromResult(PushStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        #region SetStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

                                          DateTime?                Timestamp,
                                          CancellationToken?       CancellationToken,
                                          EventTracking_Id         EventTrackingId,
                                          TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

                                          DateTime?                Timestamp,
                                          CancellationToken?       CancellationToken,
                                          EventTracking_Id         EventTrackingId,
                                          TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion


        #region SetStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                          DateTime?                             Timestamp,
                                          CancellationToken?                    CancellationToken,
                                          EventTracking_Id                      EventTrackingId,
                                          TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                          DateTime?                             Timestamp,
                                          CancellationToken?                    CancellationToken,
                                          EventTracking_Id                      EventTrackingId,
                                          TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);


        }

        #endregion

        #region UpdateStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion


        #region UpdateChargingStationOperatorAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                                      TransmissionType,

                                               DateTime?                                              Timestamp,
                                               CancellationToken?                                     CancellationToken,
                                               EventTracking_Id                                       EventTrackingId,
                                               TimeSpan?                                              RequestTimeout)

        {

            return Task.FromResult(PushAdminStatusResult.OutOfService(Id,
                                                                      this,
                                                                      AdminStatusUpdates));

        }

        #endregion

        #region UpdateChargingStationOperatorStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                            TransmissionTypes                                 TransmissionType,

                                            DateTime?                                         Timestamp,
                                            CancellationToken?                                CancellationToken,
                                            EventTracking_Id                                  EventTrackingId,
                                            TimeSpan?                                         RequestTimeout)

        {

            return PushStatusResult.NoOperation(Id, this);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Roaming network...

        #region SetStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Set the EVSE data of the given roaming network as new static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(RoamingNetwork      RoamingNetwork,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region AddStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network to the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(RoamingNetwork      RoamingNetwork,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region UpdateStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion

        #region DeleteStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data at the OICP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return PushEVSEDataResult.NoOperation(Id, this);

        }

        #endregion


        #region UpdateRoamingNetworkAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                             TransmissionType,

                                               DateTime?                                     Timestamp,
                                               CancellationToken?                            CancellationToken,
                                               EventTracking_Id                              EventTrackingId,
                                               TimeSpan?                                     RequestTimeout)

        {

            return Task.FromResult(PushAdminStatusResult.OutOfService(Id,
                                                                      this,
                                                                      AdminStatusUpdates));

        }

        #endregion

        #region UpdateRoamingNetworkStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                            TransmissionTypes                        TransmissionType,

                                            DateTime?                                Timestamp,
                                            CancellationToken?                       CancellationToken,
                                            EventTracking_Id                         EventTrackingId,
                                            TimeSpan?                                RequestTimeout)

        {

            return PushStatusResult.NoOperation(Id, this);

        }

        #endregion

        #endregion

        #endregion


        #region Delayed upstream methods...

        #region EnqueueEVSEStatusUpdate(EVSE, OldStatus, NewStatus)

        /// <summary>
        /// Enqueue the given EVSE status for a delayed upload.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        /// <param name="OldStatus">The old status of the EVSE.</param>
        /// <param name="NewStatus">The new status of the EVSE.</param>
        public Task<PushEVSEDataResult>

            EnqueueEVSEStatusUpdate(EVSE                         EVSE,
                                    Timestamped<EVSEStatusTypes>  OldStatus,
                                    Timestamped<EVSEStatusTypes>  NewStatus)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion


        #region EnqueueChargingPoolDataUpdate(ChargingPool, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="ChargingPool">A charging station.</param>
        public Task<PushEVSEDataResult>

            EnqueueChargingPoolDataUpdate(ChargingPool  ChargingPool,
                                          String        PropertyName,
                                          Object        OldValue,
                                          Object        NewValue)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #region EnqueueChargingStationDataUpdate(ChargingStation, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Enqueue the given EVSE data for a delayed upload.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        public Task<PushEVSEDataResult>

            EnqueueChargingStationDataUpdate(ChargingStation  ChargingStation,
                                             String           PropertyName,
                                             Object           OldValue,
                                             Object           NewValue)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

        }

        #endregion

        #endregion









        #region Operator overloading

        #region Operator == (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two smart citys for equality.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SmartCityProxy SmartCity1, SmartCityProxy SmartCity2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SmartCity1, SmartCity2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SmartCity1 == null) || ((Object) SmartCity2 == null))
                return false;

            return SmartCity1.Equals(SmartCity2);

        }

        #endregion

        #region Operator != (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two smart citys for inequality.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SmartCityProxy SmartCity1, SmartCityProxy SmartCity2)

            => !(SmartCity1 == SmartCity2);

        #endregion

        #region Operator <  (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SmartCityProxy  SmartCity1,
                                          SmartCityProxy  SmartCity2)
        {

            if ((Object) SmartCity1 == null)
                throw new ArgumentNullException(nameof(SmartCity1),  "The given smart city must not be null!");

            return SmartCity1.CompareTo(SmartCity2) < 0;

        }

        #endregion

        #region Operator <= (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SmartCityProxy SmartCity1,
                                           SmartCityProxy SmartCity2)

            => !(SmartCity1 > SmartCity2);

        #endregion

        #region Operator >  (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SmartCityProxy SmartCity1,
                                          SmartCityProxy SmartCity2)
        {

            if ((Object) SmartCity1 == null)
                throw new ArgumentNullException(nameof(SmartCity1),  "The given smart city must not be null!");

            return SmartCity1.CompareTo(SmartCity2) > 0;

        }

        #endregion

        #region Operator >= (SmartCity1, SmartCity2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCity1">A smart city.</param>
        /// <param name="SmartCity2">Another smart city.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SmartCityProxy SmartCity1,
                                           SmartCityProxy SmartCity2)

            => !(SmartCity1 < SmartCity2);

        #endregion

        #endregion

        #region IComparable<SmartCityStub> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var SmartCityStub = Object as SmartCityProxy;
            if ((Object) SmartCityStub == null)
                throw new ArgumentException("The given object is not an SmartCityStub!", nameof(Object));

            return CompareTo(SmartCityStub);

        }

        #endregion

        #region CompareTo(SmartCityStub)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SmartCityStub">An SmartCityStub object to compare with.</param>
        public Int32 CompareTo(SmartCityProxy SmartCityStub)
        {

            if ((Object) SmartCityStub == null)
                throw new ArgumentNullException(nameof(SmartCityStub), "The given SmartCityStub must not be null!");

            return Id.CompareTo(SmartCityStub.Id);

        }

        #endregion

        #endregion

        #region IEquatable<SmartCityStub> Members

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

            var SmartCityStub = Object as SmartCityProxy;
            if ((Object) SmartCityStub == null)
                return false;

            return this.Equals(SmartCityStub);

        }

        #endregion

        #region Equals(SmartCityStub)

        /// <summary>
        /// Compares two SmartCityStub for equality.
        /// </summary>
        /// <param name="SmartCityStub">An SmartCityStub to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SmartCityProxy SmartCityStub)
        {

            if ((Object) SmartCityStub == null)
                return false;

            return Id.Equals(SmartCityStub.Id);

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

            => "Smart City " + Id;

        public Task<PushEVSEDataResult> UpdateStaticData(ChargingStation ChargingStation, string PropertyName = null, object OldValue = null, object NewValue = null, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> UpdateStaticData(ChargingPool ChargingPool, string PropertyName = null, object OldValue = null, object NewValue = null, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(ChargingStation ChargingStation, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(ChargingStation ChargingStation, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> SetStaticData(ChargingPool ChargingPool, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public Task<PushEVSEDataResult> AddStaticData(ChargingPool ChargingPool, TransmissionTypes TransmissionType = TransmissionTypes.Enqueue, DateTime? Timestamp = default(DateTime?), CancellationToken? CancellationToken = default(CancellationToken?), EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        #endregion



    }

}
