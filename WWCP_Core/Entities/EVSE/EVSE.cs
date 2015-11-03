/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System.Collections.Generic;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class EVSE : AEMobilityEntity<EVSE_Id>,
                        IEquatable<EVSE>, IComparable<EVSE>, IComparable,
                        IEnumerable<SocketOutlet>,
                        IStatus<EVSEStatusType>

    {

        #region Data

        /// <summary>
        /// The default max size of the EVSE status history.
        /// </summary>
        public const UInt16 DefaultEVSEStatusHistorySize = 50;

        /// <summary>
        /// The default max size of the EVSE admin status history.
        /// </summary>
        public const UInt16 DefaultEVSEAdminStatusHistorySize = 50;

        #endregion

        #region Properties

        #region Description

        private I18NString _Description;

        [Mandatory]
        public I18NString Description
        {

            get
            {

                return _Description != null
                    ? _Description
                    : ChargingStation.Description;

            }

            set
            {

                if (value == ChargingStation.Description)
                    value = null;

                if (_Description != value)
                    SetProperty<I18NString>(ref _Description, value);

            }

        }

        #endregion

        #region AverageVoltage

        private Double _AverageVoltage;

        /// <summary>
        /// Average voltage at the connector [Volt].
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

        #region MaxPower

        private Double _MaxPower;

        /// <summary>
        /// Max power at connector [Watt].
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
        /// Real-time power at connector [Watt].
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

        #region GuranteedMinPower

        private Double _GuranteedMinPower;

        /// <summary>
        /// Guranteed min power at connector [Watt].
        /// </summary>
        [Mandatory]
        public Double GuranteedMinPower
        {

            get
            {
                return _GuranteedMinPower;
            }

            set
            {

                if (_MaxPower != value)
                    SetProperty(ref _GuranteedMinPower, value);

            }

        }

        #endregion

        #region MaxCapacity_kWh

        private Double? _MaxCapacity_kWh;

        /// <summary>
        /// Max power capacity at the connector [kWh].
        /// </summary>
        [Mandatory]
        public Double? MaxCapacity_kWh
        {

            get
            {
                return _MaxCapacity_kWh;
            }

            set
            {

                if (_MaxCapacity_kWh != value)
                    SetProperty(ref _MaxCapacity_kWh, value);

            }

        }

        #endregion

        #region ChargingModes

        private ReactiveSet<ChargingModes> _ChargingModes;

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

        #region ChargingFacilities

        private ReactiveSet<ChargingFacilities> _ChargingFacilities;

        [Mandatory]
        public ReactiveSet<ChargingFacilities> ChargingFacilities
        {

            get
            {
                return _ChargingFacilities;
            }

            set
            {

                if (_ChargingFacilities != value)
                    SetProperty(ref _ChargingFacilities, value);

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


        #region PointOfDelivery // MeterId

        private String _PointOfDelivery;

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

            set
            {

                if (_PointOfDelivery != value)
                    SetProperty<String>(ref _PointOfDelivery, value);

            }

        }

        #endregion


        #region Status

        /// <summary>
        /// The current EVSE status.
        /// </summary>
        [Mandatory]
        public Timestamped<EVSEStatusType> Status
        {

            get
            {
                return _StatusSchedule.Peek();
            }

            set
            {
                SetStatus(DateTime.Now, value);
            }

        }

        #endregion

        #region StatusSchedule

        private Stack<Timestamped<EVSEStatusType>> _StatusSchedule;

        /// <summary>
        /// The EVSE status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusType>> StatusSchedule
        {
            get
            {
                return _StatusSchedule.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region PlannedStatusChanges

        private List<Timestamped<EVSEStatusType>> _PlannedStatusChanges;

        /// <summary>
        /// A list of planned future EVSE status changes.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusType>> PlannedStatusChanges
        {
            get
            {
                return _PlannedStatusChanges.OrderBy(v => v.Timestamp);
            }
        }

        #endregion


        #region AdminStatus

        /// <summary>
        /// The current EVSE admin status.
        /// </summary>
        [Optional]
        public Timestamped<EVSEAdminStatusType> AdminStatus
        {

            get
            {
                return _AdminStatusSchedule.Peek();
            }

            set
            {
                SetAdminStatus(DateTime.Now, value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        private Stack<Timestamped<EVSEAdminStatusType>> _AdminStatusSchedule;

        /// <summary>
        /// The EVSE admin status schedule.
        /// </summary>
        public IEnumerable<Timestamped<EVSEAdminStatusType>> AdminStatusSchedule
        {
            get
            {
                return _AdminStatusSchedule.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        #region PlannedAdminStatusChanges

        private List<Timestamped<EVSEAdminStatusType>> _PlannedAdminStatusChanges;

        /// <summary>
        /// A list of planned future EVSE admin status changes.
        /// </summary>
        public IEnumerable<Timestamped<EVSEAdminStatusType>> PlannedAdminStatusChanges
        {
            get
            {
                return _PlannedAdminStatusChanges.OrderBy(v => v.Timestamp);
            }
        }

        #endregion


        #region ChargingStation

        private readonly ChargingStation _ChargingStation;

        /// <summary>
        /// The charging station of this EVSE.
        /// </summary>
        public ChargingStation ChargingStation
        {
            get
            {
                return _ChargingStation;
            }
        }

        #endregion

        #endregion

        #region Events

        // EVSE events

        #region OnStatusChanged

        /// <summary>
        /// A delegate called whenever the dynamic status of the EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="OldEVSEStatus">The old timestamped status of the EVSE.</param>
        /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
        public delegate void OnStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEStatusType> OldEVSEStatus, Timestamped<EVSEStatusType> NewEVSEStatus);

        /// <summary>
        /// An event fired whenever the dynamic status of the EVSE changed.
        /// </summary>
        public event OnStatusChangedDelegate OnStatusChanged;

        #endregion

        #region OnAdminStatusChanged

        /// <summary>
        /// A delegate called whenever the admin status of the EVSE changed.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="EVSE">The EVSE.</param>
        /// <param name="OldEVSEStatus">The old timestamped status of the EVSE.</param>
        /// <param name="NewEVSEStatus">The new timestamped status of the EVSE.</param>
        public delegate void OnAdminStatusChangedDelegate(DateTime Timestamp, EVSE EVSE, Timestamped<EVSEAdminStatusType> OldEVSEStatus, Timestamped<EVSEAdminStatusType> NewEVSEStatus);

        /// <summary>
        /// An event fired whenever the admin status of the EVSE changed.
        /// </summary>
        public event OnAdminStatusChangedDelegate OnAdminStatusChanged;

        #endregion

        #region SocketOutletAddition

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletAddition;

        /// <summary>
        /// Called whenever a socket outlet will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletAddition
        {
            get
            {
                return SocketOutletAddition;
            }
        }

        #endregion

        #region SocketOutletRemoval

        internal readonly IVotingNotificator<DateTime, EVSE, SocketOutlet, Boolean> SocketOutletRemoval;

        /// <summary>
        /// Called whenever a socket outlet will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, EVSE, SocketOutlet, Boolean> OnSocketOutletRemoval
        {
            get
            {
                return SocketOutletRemoval;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given EVSE_Id.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="ChargingStation">The parent EVS pool.</param>
        /// <param name="EVSEStatusHistorySize">The default size of the EVSE status history.</param>
        /// <param name="EVSEAdminStatusHistorySize">The default size of the EVSE admin status history.</param>
        internal EVSE(EVSE_Id          Id,
                      ChargingStation  ChargingStation,
                      UInt16           EVSEStatusHistorySize      = DefaultEVSEStatusHistorySize,
                      UInt16           EVSEAdminStatusHistorySize = DefaultEVSEAdminStatusHistorySize)

            : base(Id)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The charging station must not be null!");

            #endregion

            #region Init data and properties

            this._ChargingStation       = ChargingStation;

            this._Description           = new I18NString();
            this._ChargingModes         = new ReactiveSet<ChargingModes>();
            this._ChargingFacilities    = new ReactiveSet<ChargingFacilities>();
            this._SocketOutlets         = new ReactiveSet<SocketOutlet>();

            this._StatusSchedule        = new Stack<Timestamped<EVSEStatusType>>((Int32) EVSEStatusHistorySize);
            this._StatusSchedule.Push(new Timestamped<EVSEStatusType>(EVSEStatusType.Unknown));

            this._AdminStatusSchedule   = new Stack<Timestamped<EVSEAdminStatusType>>((Int32) EVSEStatusHistorySize);

            #endregion

            #region Init events

            // EVSE events
            this.SocketOutletAddition     = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);
            this.SocketOutletRemoval      = new VotingNotificator<DateTime, EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

            #region Link events

            // EVSE events
            this.SocketOutletAddition.     OnVoting       += (timestamp, evse, outlet, vote)       => ChargingStation.SocketOutletAddition.   SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletAddition.     OnNotification += (timestamp, evse, outlet)             => ChargingStation.SocketOutletAddition.   SendNotification(timestamp, evse, outlet);

            this.SocketOutletRemoval.      OnVoting       += (timestamp, evse, outlet, vote)       => ChargingStation.SocketOutletRemoval.    SendVoting      (timestamp, evse, outlet, vote);
            this.SocketOutletRemoval.      OnNotification += (timestamp, evse, outlet)             => ChargingStation.SocketOutletRemoval.    SendNotification(timestamp, evse, outlet);

            #endregion

        }

        #endregion


        #region SetStatus(Timestamp, NewStatus)

        /// <summary>
        /// Set the timestamped status of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewStatus">A list of new timestamped status for this EVSE.</param>
        public void SetStatus(DateTime                     Timestamp,
                              Timestamped<EVSEStatusType>  NewStatus)
        {

            if (_StatusSchedule.Peek().Value != NewStatus.Value)
            {

                var OldStatus = _StatusSchedule.Peek();

                _StatusSchedule.Push(NewStatus);

                var OnStatusChangedLocal = OnStatusChanged;
                if (OnStatusChangedLocal != null)
                    OnStatusChanged(Timestamp, this, OldStatus, NewStatus);

            }

        }

        #endregion

        #region SetStatus(Timestamp, NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped status of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewStatusList">A list of new timestamped status for this EVSE.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetStatus(DateTime                                  Timestamp,
                              IEnumerable<Timestamped<EVSEStatusType>>  NewStatusList,
                              ChangeMethods                             ChangeMethod = ChangeMethods.Replace)
        {

            foreach (var NewStatus in NewStatusList)
            {

                if (NewStatus.Timestamp <= DateTime.Now)
                {
                    if (_StatusSchedule.Peek().Value != NewStatus.Value)
                    {

                        var OldStatus = _StatusSchedule.Peek();

                        _StatusSchedule.Push(NewStatus);

                        var OnStatusChangedLocal = OnStatusChanged;
                        if (OnStatusChangedLocal != null)
                            OnStatusChanged(Timestamp, this, OldStatus, NewStatus);

                    }
                }

                else
                {

                    if (ChangeMethod == ChangeMethods.Replace)
                        _PlannedStatusChanges = NewStatusList.
                                                    Where(TVP => TVP.Timestamp > DateTime.Now).
                                                    ToList();

                    else
                        _PlannedStatusChanges = _PlannedStatusChanges.
                                                    Concat(NewStatusList.Where(TVP => TVP.Timestamp > DateTime.Now)).
                                                    Deduplicate().
                                                    ToList();

                }

            }

        }

        #endregion


        #region SetAdminStatus(Timestamp, NewStatus)

        /// <summary>
        /// Set the timestamped admin status of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewStatus">A list of new timestamped status for this EVSE.</param>
        public void SetAdminStatus(DateTime                          Timestamp,
                                   Timestamped<EVSEAdminStatusType>  NewStatus)
        {

            if (_AdminStatusSchedule.Peek().Value != NewStatus.Value)
            {

                var OldStatus = _AdminStatusSchedule.Peek();

                _AdminStatusSchedule.Push(NewStatus);

                var OnAdminStatusChangedLocal = OnAdminStatusChanged;
                if (OnAdminStatusChangedLocal != null)
                    OnAdminStatusChanged(Timestamp, this, OldStatus, NewStatus);

            }

        }

        #endregion

        #region SetAdminStatus(Timestamp, NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status of the EVSE.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewStatusList">A list of new timestamped status for this EVSE.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(DateTime                                       Timestamp,
                                   IEnumerable<Timestamped<EVSEAdminStatusType>>  NewStatusList,
                                   ChangeMethods                                  ChangeMethod = ChangeMethods.Replace)
        {

            foreach (var NewStatus in NewStatusList)
            {

                if (NewStatus.Timestamp <= DateTime.Now)
                {
                    if (_AdminStatusSchedule.Peek().Value != NewStatus.Value)
                    {

                        var OldStatus = _AdminStatusSchedule.Peek();

                        _AdminStatusSchedule.Push(NewStatus);

                        var OnAdminStatusChangedLocal = OnAdminStatusChanged;
                        if (OnAdminStatusChangedLocal != null)
                            OnAdminStatusChanged(Timestamp, this, OldStatus, NewStatus);

                    }
                }

                else
                {

                    if (ChangeMethod == ChangeMethods.Replace)
                        _PlannedAdminStatusChanges = NewStatusList.
                                                         Where(TVP => TVP.Timestamp > DateTime.Now).
                                                         ToList();

                    else
                        _PlannedAdminStatusChanges = _PlannedAdminStatusChanges.
                                                         Concat(NewStatusList.Where(TVP => TVP.Timestamp > DateTime.Now)).
                                                         Deduplicate().
                                                         ToList();

                }

            }

        }

        #endregion


        #region IEnumerable<SocketOutlet> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _SocketOutlets.GetEnumerator();
        }

        public IEnumerator<SocketOutlet> GetEnumerator()
        {
            return _SocketOutlets.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSE> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE.
            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                throw new ArgumentException("The given object is not an EVSE!");

            return CompareTo(EVSE);

        }

        #endregion

        #region CompareTo(EVSE)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        public Int32 CompareTo(EVSE EVSE)
        {

            if ((Object) EVSE == null)
                throw new ArgumentNullException("The given EVSE must not be null!");

            return Id.CompareTo(EVSE.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSE> Members

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

            // Check if the given object is an EVSE.
            var EVSE = Object as EVSE;
            if ((Object) EVSE == null)
                return false;

            return this.Equals(EVSE);

        }

        #endregion

        #region Equals(EVSE)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="EVSE">An EVSE to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE EVSE)
        {

            if ((Object) EVSE == null)
                return false;

            return Id.Equals(EVSE.Id);

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
            return Id.ToString();
        }

        #endregion

    }

}
