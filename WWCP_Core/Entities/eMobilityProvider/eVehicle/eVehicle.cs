/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An electric vehicle.
    /// </summary>
    public class eVehicle : AEMobilityEntity<eVehicle_Id>,
                            IEquatable<eVehicle>, IComparable<eVehicle>, IComparable

    {

        #region Data

        /// <summary>
        /// The default max size of the e-vehicle (aggregated EVSE) status list.
        /// </summary>
        public const UInt16 DefaultMaxStatusListSize         = 15;

        /// <summary>
        /// The default max size of the e-vehicle admin status list.
        /// </summary>
        public const UInt16 DefaultMaxAdminStatusListSize    = 15;

        #endregion

        #region Properties

        #region ServiceIdentification

        private String _ServiceIdentification;

        /// <summary>
        /// The internal service identification of the e-vehicle maintained by the e-vehicle Operator.
        /// </summary>
        [Optional]
        public String ServiceIdentification
        {

            get
            {
                return _ServiceIdentification;
            }

            set
            {

                if (ServiceIdentification != value)
                    SetProperty<String>(ref _ServiceIdentification, value);

            }

        }

        #endregion

        #region Description

        internal I18NString _Description;

        /// <summary>
        /// An optional (multi-language) description of this e-vehicle.
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

                if (Description != value)
                    SetProperty(ref _Description, value);

            }

        }

        #endregion

        #region Brand

        private Brand _Brand;

        /// <summary>
        /// A brand for this e-vehicle
        /// is this is different from the e-vehicle Operator.
        /// </summary>
        [Optional]
        public Brand Brand
        {

            get
            {
                return _Brand;
            }

            set
            {

                if (_Brand != value)
                    SetProperty(ref _Brand, value);

            }

        }

        #endregion

        #region Address

        internal Address _Address;

        /// <summary>
        /// The address of this e-vehicle.
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

                if (Address != value)
                {

                    if (value == null)
                        DeleteProperty(ref _Address);

                    else
                        SetProperty(ref _Address, value);

                }

            }

        }

        #endregion

        #region GeoLocation

        internal GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of this e-vehicle.
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

                if (GeoLocation != value)
                {

                    if (value == null)
                        DeleteProperty(ref _GeoLocation);

                    else
                        SetProperty(ref _GeoLocation, value);

                }

            }

        }

        #endregion

        #region AuthenticationModes

        internal ReactiveSet<AuthenticationModes> _AuthenticationModes;

        public ReactiveSet<AuthenticationModes> AuthenticationModes
        {

            get
            {
                return _AuthenticationModes;
            }

            set
            {

                if (AuthenticationModes != value)
                {

                    if (_AuthenticationModes == null)
                        _AuthenticationModes = new ReactiveSet<AuthenticationModes>();

                    if (value == null)
                        DeleteProperty(ref _AuthenticationModes);

                    else
                        SetProperty(ref _AuthenticationModes, value);

                }

            }

        }

        #endregion

        #region PaymentOptions

        internal ReactiveSet<PaymentOptions> _PaymentOptions;

        [Mandatory]
        public ReactiveSet<PaymentOptions> PaymentOptions
        {

            get
            {
                return _PaymentOptions;
            }

            set
            {

                if (PaymentOptions != value)
                {

                    if (_PaymentOptions == null)
                        _PaymentOptions = new ReactiveSet<PaymentOptions>();

                    if (value == null)
                        DeleteProperty(ref _PaymentOptions);

                    else
                        SetProperty(ref _PaymentOptions, value);

                }

            }

        }

        #endregion

        #region Accessibility

        internal AccessibilityTypes _Accessibility;

        [Optional]
        public AccessibilityTypes Accessibility
        {

            get
            {

                return _Accessibility;

            }

            set
            {

                if (Accessibility != value)
                {

                    SetProperty(ref _Accessibility, value);

                }

            }

        }

        #endregion

        #region HotlinePhoneNumber

        internal String _HotlinePhoneNumber;

        /// <summary>
        /// The telephone number of the e-vehicle Operator hotline.
        /// </summary>
        [Optional]
        public String HotlinePhoneNumber
        {

            get
            {
                return _HotlinePhoneNumber ?? Provider.HotlinePhoneNumber;
            }

            set
            {

                if (value == Provider.HotlinePhoneNumber)
                    return;

                if (HotlinePhoneNumber != value)
                {

                    if (value == null)
                        DeleteProperty(ref _HotlinePhoneNumber);

                    else
                        SetProperty(ref _HotlinePhoneNumber, value);

                }

            }

        }

        #endregion



        #region UserComment

        private I18NString _UserComment;

        /// <summary>
        /// A comment from the users.
        /// </summary>
        [Optional]
        public I18NString UserComment
        {

            get
            {
                return _UserComment;
            }

            set
            {
                SetProperty<I18NString>(ref _UserComment, value);
            }

        }

        #endregion

        #region ServiceProviderComment

        private I18NString _ServiceProviderComment;

        /// <summary>
        /// A comment from the service provider.
        /// </summary>
        [Optional]
        public I18NString ServiceProviderComment
        {

            get
            {
                return _ServiceProviderComment;
            }

            set
            {
                SetProperty<I18NString>(ref _ServiceProviderComment, value);
            }

        }

        #endregion

        #region PhotoURIs

        private ReactiveSet<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this e-vehicle.
        /// </summary>
        [Optional]
        public ReactiveSet<String> PhotoURIs
        {

            get
            {
                return _PhotoURIs;
            }

            set
            {
                SetProperty(ref _PhotoURIs, value);
            }

        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current e-vehicle admin status.
        /// </summary>
        [Optional]
        public Timestamped<eVehicleAdminStatusType> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.CurrentStatus;
            }

            set
            {
                SetAdminStatus(value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<eVehicleAdminStatusType> _AdminStatusSchedule;

        /// <summary>
        /// The e-vehicle admin status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<eVehicleAdminStatusType>> AdminStatusSchedule

            => _AdminStatusSchedule;

        #endregion


        #region Status

        /// <summary>
        /// The current e-vehicle status.
        /// </summary>
        [Optional]
        public Timestamped<eVehicleStatusType> Status
        {

            get
            {
                return _StatusSchedule.CurrentStatus;
            }

            set
            {
                SetStatus(value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        private StatusSchedule<eVehicleStatusType> _StatusSchedule;

        /// <summary>
        /// The e-vehicle status schedule.
        /// </summary>
        [Optional]
        public IEnumerable<Timestamped<eVehicleStatusType>> StatusSchedule

            => _StatusSchedule;

        #endregion

        #endregion

        #region Links

        /// <summary>
        /// An optional remote e-vehicle.
        /// </summary>
        public IRemoteEVehicle  RemoteEVehicle  { get; }


        /// <summary>
        /// The e-vehicle Operator of this EVSE.
        /// </summary>
        [InternalUseOnly]
        public eMobilityProvider Provider       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-vehicle having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the e-vehicle pool.</param>
        /// <param name="MaxAdminStatusListSize">The default size of the admin status list.</param>
        internal eVehicle(eVehicle_Id                    Id,
                          eMobilityProvider              Provider,
                          Action<eVehicle>               Configurator            = null,
                          RemoteEVehicleCreatorDelegate  RemoteeVehicleCreator   = null,
                          eVehicleAdminStatusType        AdminStatus             = eVehicleAdminStatusType.Operational,
                          eVehicleStatusType             Status                  = eVehicleStatusType.Available,
                          UInt16                         MaxAdminStatusListSize  = DefaultMaxAdminStatusListSize,
                          UInt16                         MaxStatusListSize       = DefaultMaxStatusListSize)

            : base(Id)

        {

            #region Initial checks

            if (Provider == null)
                throw new ArgumentNullException(nameof(Provider),  "The e-mobility provider must not be null!");

            #endregion

            #region Init data and properties

            this.Provider                    = Provider;

            this.Description                 = new I18NString();

            this._UserComment                = new I18NString();
            this._ServiceProviderComment     = new I18NString();
            this.GeoLocation                 = null;

            this._PaymentOptions             = new ReactiveSet<PaymentOptions>();

            this._AdminStatusSchedule        = new StatusSchedule<eVehicleAdminStatusType>(MaxAdminStatusListSize);
            this._AdminStatusSchedule.Insert(AdminStatus);

            this._StatusSchedule             = new StatusSchedule<eVehicleStatusType>(MaxStatusListSize);
            this._StatusSchedule.Insert(Status);

            #endregion

            #region Init events


            #endregion

            #region Link events

            this._AdminStatusSchedule.OnStatusChanged += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateAdminStatus(Timestamp, OldStatus, NewStatus);

            this._StatusSchedule.OnStatusChanged      += (Timestamp, StatusSchedule, OldStatus, NewStatus)
                                                          => UpdateStatus(Timestamp, OldStatus, NewStatus);

            #endregion

            this.OnPropertyChanged += UpdateData;

            Configurator?.Invoke(this);

            this.RemoteEVehicle = RemoteeVehicleCreator?.Invoke(this);

        }

        #endregion


        #region Data/(Admin-)Status management

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        public event OnEVehicleDataChangedDelegate         OnDataChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        public event OnEVehicleStatusChangedDelegate       OnStatusChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        public event OnEVehicleAdminStatusChangedDelegate  OnAdminStatusChanged;

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<eVehicleStatusType>  NewStatus)
        {

            _StatusSchedule.Insert(NewStatus);

        }

        #endregion


        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(eVehicleAdminStatusType  NewAdminStatus)
        {

            _AdminStatusSchedule.Insert(NewAdminStatus);

        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<eVehicleAdminStatusType> NewTimestampedAdminStatus)
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
        public void SetAdminStatus(eVehicleAdminStatusType  NewAdminStatus,
                                   DateTime                        Timestamp)
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
        public void SetAdminStatus(IEnumerable<Timestamped<eVehicleAdminStatusType>>  NewAdminStatusList,
                                   ChangeMethods                                             ChangeMethod = ChangeMethods.Replace)
        {

            _AdminStatusSchedule.Insert(NewAdminStatusList, ChangeMethod);

        }

        #endregion


        #region (internal) UpdateData(Timestamp, Sender, PropertyName, OldValue, NewValue)

        /// <summary>
        /// Update the static data.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="Sender">The changed charging station.</param>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        internal async Task UpdateData(DateTime  Timestamp,
                                       Object    Sender,
                                       String    PropertyName,
                                       Object    OldValue,
                                       Object    NewValue)
        {

            var OnDataChangedLocal = OnDataChanged;
            if (OnDataChangedLocal != null)
                await OnDataChangedLocal(Timestamp, Sender as eVehicle, PropertyName, OldValue, NewValue);

        }

        #endregion

        #region (internal) UpdateStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old EVSE status.</param>
        /// <param name="NewStatus">The new EVSE status.</param>
        internal void UpdateStatus(DateTime                         Timestamp,
                                   Timestamped<eVehicleStatusType>  OldStatus,
                                   Timestamped<eVehicleStatusType>  NewStatus)
        {

            OnStatusChanged?.Invoke(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #region (internal) UpdateAdminStatus(Timestamp, OldStatus, NewStatus)

        /// <summary>
        /// Update the current admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldStatus">The old charging station admin status.</param>
        /// <param name="NewStatus">The new charging station admin status.</param>
        internal void UpdateAdminStatus(DateTime                              Timestamp,
                                        Timestamped<eVehicleAdminStatusType>  OldStatus,
                                        Timestamped<eVehicleAdminStatusType>  NewStatus)
        {

            OnAdminStatusChanged?.Invoke(Timestamp, this, OldStatus, NewStatus);

        }

        #endregion

        #endregion

        #region Geo location management

        #region OnGeoLocationChanged

        /// <summary>
        /// An event fired whenever the geo coordinate changed.
        /// </summary>
        public event OnEVehicleGeoLocationChangedDelegate  OnGeoLocationChanged;

        #endregion

        #region (internal) UpdateGeoLocation(Timestamp, OldGeoCoordinate, NewGeoCoordinate)

        /// <summary>
        /// Update the current geo location.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="OldGeoCoordinate">The old geo coordinate.</param>
        /// <param name="NewGeoCoordinate">The new geo coordinate.</param>
        internal void UpdateGeoLocation(DateTime                    Timestamp,
                                        Timestamped<GeoCoordinate>  OldGeoCoordinate,
                                        Timestamped<GeoCoordinate>  NewGeoCoordinate)
        {

            OnGeoLocationChanged?.Invoke(Timestamp, this, OldGeoCoordinate, NewGeoCoordinate);

        }

        #endregion

        #endregion



        #region IComparable<eVehicle> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is a e-vehicle.
            var eVehicle = Object as eVehicle;
            if ((Object) eVehicle == null)
                throw new ArgumentException("The given object is not a e-vehicle!");

            return CompareTo(eVehicle);

        }

        #endregion

        #region CompareTo(eVehicle)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicle">A e-vehicle object to compare with.</param>
        public Int32 CompareTo(eVehicle eVehicle)
        {

            if ((Object) eVehicle == null)
                throw new ArgumentNullException("The given e-vehicle must not be null!");

            return Id.CompareTo(eVehicle.Id);

        }

        #endregion

        #endregion

        #region IEquatable<eVehicle> Members

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

            // Check if the given object is a e-vehicle.
            var eVehicle = Object as eVehicle;
            if ((Object) eVehicle == null)
                return false;

            return this.Equals(eVehicle);

        }

        #endregion

        #region Equals(eVehicle)

        /// <summary>
        /// Compares two e-vehicles for equality.
        /// </summary>
        /// <param name="eVehicle">A e-vehicle to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eVehicle eVehicle)
        {

            if ((Object) eVehicle == null)
                return false;

            return Id.Equals(eVehicle.Id);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
