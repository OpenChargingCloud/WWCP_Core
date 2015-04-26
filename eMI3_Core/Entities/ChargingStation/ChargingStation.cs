/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3_Core>
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// </summary>
    public class ChargingStation : AEntity<ChargingStation_Id>,
                                   IEquatable<ChargingStation>, IComparable<ChargingStation>, IComparable,
                                   IEnumerable<EVSE>
    {

        #region Data

        /// <summary>
        /// The default max size of the aggregated EVSE status history.
        /// </summary>
        public const UInt16 DefaultStatusHistorySize = 50;

        #endregion

        #region Properties

        #region ServiceIdentification

        private String _ServiceIdentification;

        /// <summary>
        /// The internal service identification of the charging station maintained by the EVSE operator.
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
                SetProperty<String>(ref _ServiceIdentification, value);
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

            //set
            //{
            //    SetProperty<I8NString>(ref _UserComment, value);
            //}

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

            //set
            //{
            //    SetProperty<I8NString>(ref _ServiceProviderComment, value);
            //}

        }

        #endregion

        #region GeoLocation

        private GeoCoordinate _GeoLocation;

        /// <summary>
        /// The geographical location of the charging station.
        /// If it is not set return the geographical location of its EVSPool.
        /// </summary>
        [Optional]
        public GeoCoordinate GeoLocation
        {

            get
            {

                if (_GeoLocation.IsValid)
                    return _GeoLocation;

                else
                    return ChargingPool.PoolLocation;

            }

            set
            {
                SetProperty<GeoCoordinate>(ref _GeoLocation, value);
            }

        }

        #endregion

        #region GridConnection

        private GridConnection _GridConnection;

        /// <summary>
        /// The grid connection of the charging station.
        /// </summary>
        [Optional]
        public GridConnection GridConnection
        {

            get
            {
                return _GridConnection;
            }

            set
            {
                SetProperty<GridConnection>(ref _GridConnection, value);
            }

        }

        #endregion

        #region Features

        private ChargingStationUIFeatures _Features;

        /// <summary>
        /// The features of the charging station.
        /// </summary>
        [Optional]
        public ChargingStationUIFeatures Features
        {

            get
            {
                return _Features;
            }

            set
            {
                SetProperty<ChargingStationUIFeatures>(ref _Features, value);
            }

        }

        #endregion

        #region AuthorizationOptions

        private AuthorizationOptions _AuthorizationOptions;

        /// <summary>
        /// The authorization options of the charging station.
        /// </summary>
        [Optional]
        public AuthorizationOptions AuthorizationOptions
        {

            get
            {
                return _AuthorizationOptions;
            }

            set
            {
                SetProperty<AuthorizationOptions>(ref _AuthorizationOptions, value);
            }

        }

        #endregion

        #region PhotoURIs

        private readonly List<String> _PhotoURIs;

        /// <summary>
        /// URIs of photos of this charging station.
        /// </summary>
        [Optional, Not_eMI3defined]
        public List<String> PhotoURIs
        {
            get
            {
                return _PhotoURIs;
            }
        }

        #endregion

        #region PointOfDelivery // MeterId

        private readonly String _PointOfDelivery;

        /// <summary>
        /// Point of delivery or meter identification.
        /// </summary>
        [Optional]
        public String PointOfDelivery
        {
            get
            {
                return _PointOfDelivery;
            }
        }

        #endregion


        public I18NString           Name                    { get; set; }

        public String               EVSEIdPrefix            { get; set; }


        #region AuthenticationModes

        private IEnumerable<String> _AuthenticationModes;

        public IEnumerable<String>  AuthenticationModes
        {

            get
            {

                if (_AuthenticationModes == null)
                    return ChargingPool.DefaultAuthenticationModes;

                return _AuthenticationModes;

            }

            set
            {

                if (value == null)
                    _AuthenticationModes = ChargingPool.DefaultAuthenticationModes;

                else
                {

                    if (ChargingPool.DefaultAuthenticationModes.Count() != value.Count())
                        _AuthenticationModes = value;

                    else
                    {
                        foreach (var AuthenticationMode in value)
                        {
                            if (!ChargingPool.DefaultAuthenticationModes.Contains(AuthenticationMode))
                                _AuthenticationModes = value;
                        }
                    }

                }

            }

        }

        #endregion


        public IEnumerable<String>  PaymentOptions          { get; set; }

        public String               Accessibility           { get; set; }

        public String               HotlinePhoneNum         { get; set; }

        public I18NString           AdditionalInfo          { get; set; }

        public Boolean?             IsHubjectCompatible     { get; set; }

        public Boolean              DynamicInfoAvailable    { get; set; }


        #region Status

        /// <summary>
        /// The current charging station status.
        /// </summary>
        [Optional, Not_eMI3defined]
        public Timestamped<AggregatedStatusType> Status
        {
            get
            {
                return _StatusHistory.Peek();
            }
        }

        #endregion

        #region StatusHistory

        private Stack<Timestamped<AggregatedStatusType>> _StatusHistory;

        /// <summary>
        /// The charging station status history.
        /// </summary>
        [Optional, Not_eMI3defined]
        public IEnumerable<Timestamped<AggregatedStatusType>> StatusHistory
        {
            get
            {
                return _StatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region StatusAggregationDelegate

        private Func<EVSEStatusReport, AggregatedStatusType> _StatusAggregationDelegate;

        /// <summary>
        /// A delegate called to aggregate the dynamic status of all subordinated EVSEs.
        /// </summary>
        public Func<EVSEStatusReport, AggregatedStatusType> StatusAggregationDelegate
        {

            get
            {
                return _StatusAggregationDelegate;
            }

            set
            {
                _StatusAggregationDelegate = value;
            }

        }

        #endregion


        #region ChargingPool

        private readonly ChargingPool _ChargingPool;

        /// <summary>
        /// The charging pool.
        /// </summary>
        public ChargingPool ChargingPool
        {
            get
            {
                return _ChargingPool;
            }
        }

        #endregion

        #region EVSEs

        private readonly ConcurrentDictionary<EVSE_Id, EVSE> _EVSEs;

        /// <summary>
        /// All Electric Vehicle Supply Equipments (EVSE) present
        /// within this charging station.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {
                return _EVSEs.Values;
            }
        }

        #endregion

        #region EVSEIds

        /// <summary>
        /// The unique identifications of all Electric Vehicle Supply Equipment (EVSEs)
        /// present within this charging station.
        /// </summary>
        public IEnumerable<EVSE_Id> EVSEIds
        {
            get
            {
                return _EVSEs.Values.Select(v => v.Id);
            }
        }

        #endregion

        #endregion

        #region Events

        #region EVSEAddition

        private readonly IVotingNotificator<ChargingStation, EVSE, Boolean> EVSEAddition;

        /// <summary>
        /// Called whenever an EVSE will be or was added.
        /// </summary>
        public IVotingSender<ChargingStation, EVSE, Boolean> OnEVSEAddition
        {
            get
            {
                return EVSEAddition;
            }
        }

        #endregion

        // EVSE events

        #region SocketOutletAddition

        internal readonly IVotingNotificator<EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion


        #region OnAggregatedStatusChanged

        /// <summary>
        /// A delegate called whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="ChargingStation">The charging station.</param>
        /// <param name="OldChargingStationStatus">The old timestamped status of the charging station.</param>
        /// <param name="NewChargingStationStatus">The new timestamped status of the charging station.</param>
        public delegate void OnAggregatedStatusChangedDelegate(DateTime Timestamp, ChargingStation ChargingStation, Timestamped<AggregatedStatusType> OldChargingStationStatus, Timestamped<AggregatedStatusType> NewChargingStationStatus);

        /// <summary>
        /// An event fired whenever the aggregated dynamic status of all subordinated EVSEs changed.
        /// </summary>
        public event OnAggregatedStatusChangedDelegate OnAggregatedStatusChanged;

        #endregion

        #endregion

        #region Constructor(s)

        #region (internal) ChargingStation()

        /// <summary>
        /// Create a new charging station having a random identification.
        /// </summary>
        internal ChargingStation()
            : this(ChargingStation_Id.New())
        { }

        #endregion

        #region (internal) ChargingStation(ChargingPool)

        /// <summary>
        /// Create a new charging station having a random identification
        /// and the given charging pool.
        /// </summary>
        /// <param name="ChargingPool">The parent charging pool.</param>
        internal ChargingStation(ChargingPool ChargingPool)
            : this(ChargingStation_Id.New(), ChargingPool)
        { }

        #endregion

        #region (internal) ChargingStation(Id, StatusHistorySize = DefaultStatusHistorySize)  // Main Constructor

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="StatusHistorySize">The default size of the aggregated EVSE status history.</param>
        internal ChargingStation(ChargingStation_Id  Id,
                                 UInt16              StatusHistorySize = DefaultStatusHistorySize)

            : base(Id)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the charging station must not be null!");

            #endregion

            #region Init data and properties

            this._EVSEs                   = new ConcurrentDictionary<EVSE_Id, EVSE>();

            this.Name                     = new I18NString();
            this.AdditionalInfo           = new I18NString();

            this._PhotoURIs               = new List<String>();
            this._UserComment             = new I18NString();
            this._ServiceProviderComment  = new I18NString();
            //this.GeoLocation             = new GeoCoordinate();

            this._StatusHistory           = new Stack<Timestamped<AggregatedStatusType>>((Int32) StatusHistorySize);
            this._StatusHistory.Push(new Timestamped<AggregatedStatusType>(AggregatedStatusType.Unknown));

            #endregion

            #region Init and link events

            this.EVSEAddition          = new VotingNotificator<ChargingStation, EVSE, Boolean>(() => new VetoVote(), true);

            if (ChargingPool != null)
            {
                this.OnEVSEAddition.OnVoting        += (chargingstation, evse, vote) => ChargingPool.EVSEAddition.SendVoting      (chargingstation, evse, vote);
                this.OnEVSEAddition.OnNotification  += (chargingstation, evse)       => ChargingPool.EVSEAddition.SendNotification(chargingstation, evse);
            }

            // EVSE events
            this.SocketOutletAddition  = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #region (internal) ChargingStation(Id, ChargingPool, StatusHistorySize = DefaultStatusHistorySize)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station pool.</param>
        /// <param name="ChargingPool">The parent charging pool.</param>
        /// <param name="StatusHistorySize">The default size of the aggregated EVSE status history.</param>
        internal ChargingStation(ChargingStation_Id  Id,
                                 ChargingPool        ChargingPool,
                                 UInt16              StatusHistorySize = DefaultStatusHistorySize)

            : this(Id, StatusHistorySize)

        {

            if (ChargingPool == null)
                throw new ArgumentNullException("EVSPool", "The EVS pool must not be null!");

            this._ChargingPool = ChargingPool;

            this.SocketOutletAddition.OnVoting        += (evse, socketoutlet , vote) => ChargingPool.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.SocketOutletAddition.OnNotification  += (evse, socketoutlet)        => ChargingPool.SocketOutletAddition.SendNotification(evse, socketoutlet);

        }

        #endregion

        #endregion


        #region CreateNew()

        /// <summary>
        /// Create a new charging station having a random identification.
        /// </summary>
        public static ChargingStation CreateNew()
        {
            return new ChargingStation();
        }

        #endregion

        #region CreateNew(Id)

        /// <summary>
        /// Create a new charging station having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        public static ChargingStation CreateNew(ChargingStation_Id Id)
        {
            return new ChargingStation(Id);
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
        public EVSE CreateNewEVSE(EVSE_Id                           EVSEId,
                                  Action<EVSE>                      Configurator  = null,
                                  Action<EVSE>                      OnSuccess     = null,
                                  Action<ChargingStation, EVSE_Id>  OnError       = null)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException("EVSE_Id", "The given EVSE identification must not be null!");

            if (_EVSEs.ContainsKey(EVSEId))
            {
                if (OnError == null)
                    throw new EVSEAlreadyExistsInStation(EVSEId, this.Id);
                else
                    OnError.FailSafeInvoke(this, EVSEId);
            }

            #endregion

            var _EVSE = new EVSE(EVSEId, this);

            Configurator.FailSafeInvoke(_EVSE);

            if (EVSEAddition.SendVoting(this, _EVSE))
            {
                if (_EVSEs.TryAdd(EVSEId, _EVSE))
                {

                    // Subscribe to EVSE status changes for aggregated status creation!
                    _EVSE.OnStatusChanged += (Timestamp, EVSE, OldEVSEStatus, NewEVSEStatus) => UpdateStatus(Timestamp);

                    OnSuccess.FailSafeInvoke(_EVSE);
                    EVSEAddition.SendNotification(this, _EVSE);
                    return _EVSE;

                }
            }

            Debug.WriteLine("EVSE '" + EVSEId + "' was not created!");
            return null;

        }

        #endregion


        #region (internal) UpdateStatus(Timestamp)

        /// <summary>
        /// Update the current charging station status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        internal void UpdateStatus(DateTime Timestamp)
        {

            if (StatusAggregationDelegate != null)
            {

                var NewStatus = new Timestamped<AggregatedStatusType>(StatusAggregationDelegate(new EVSEStatusReport(EVSEs)));

                if (NewStatus.Value != _StatusHistory.Peek().Value)
                {

                    var OldStatus = _StatusHistory.Peek();

                    _StatusHistory.Push(NewStatus);

                    var OnAggregatedStatusChangedLocal = OnAggregatedStatusChanged;
                    if (OnAggregatedStatusChangedLocal != null)
                        OnAggregatedStatusChangedLocal(Timestamp, this, OldStatus, NewStatus);

                }

            }

        }

        #endregion


        #region IEnumerable<EVSE> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _EVSEs.Values.GetEnumerator();
        }

        public IEnumerator<EVSE> GetEnumerator()
        {
            return _EVSEs.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<ChargingStation> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingStation.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                throw new ArgumentException("The given object is not an ChargingStation!");

            return CompareTo(ChargingStation);

        }

        #endregion

        #region CompareTo(ChargingStation)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation">An ChargingStation object to compare with.</param>
        public Int32 CompareTo(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                throw new ArgumentNullException("The given ChargingStation must not be null!");

            return Id.CompareTo(ChargingStation.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation> Members

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

            // Check if the given object is an ChargingStation.
            var ChargingStation = Object as ChargingStation;
            if ((Object) ChargingStation == null)
                return false;

            return this.Equals(ChargingStation);

        }

        #endregion

        #region Equals(ChargingStation)

        /// <summary>
        /// Compares two ChargingStation for equality.
        /// </summary>
        /// <param name="ChargingStation">An ChargingStation to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation ChargingStation)
        {

            if ((Object) ChargingStation == null)
                return false;

            return Id.Equals(ChargingStation.Id);

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

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "eMI3 ChargingStation: " + Id.ToString();
        }

        #endregion

    }

}
