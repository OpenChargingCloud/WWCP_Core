/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
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
    public class EVSE : AEntity<EVSE_Id>,
                        IEquatable<EVSE>, IComparable<EVSE>, IComparable,
                        IEnumerable<SocketOutlet>,
                        IStatus<EVSEStatusType>
    {

        #region Data

        /// <summary>
        /// The default max size of the EVSE status history.
        /// </summary>
        public const UInt16 DefaultEVSEStatusHistorySize = 50;

        #endregion

        #region Properties

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
                SetProperty<Double>(ref _MaxPower, value);
            }

        }

        #endregion

        #region MaxCapacity_kWh

        private Double _MaxCapacity_kWh;

        /// <summary>
        /// Max power at connector [Watt].
        /// </summary>
        [Mandatory]
        public Double MaxCapacity_kWh
        {

            get
            {
                return _MaxCapacity_kWh;
            }

            set
            {
                SetProperty<Double>(ref _MaxCapacity_kWh, value);
            }

        }

        #endregion

        #region AverageVoltage

        private Double _AverageVoltage;

        /// <summary>
        /// Average voltage at connector [Volt].
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
                SetProperty<Double>(ref _AverageVoltage, value);
            }

        }

        #endregion


        public IEnumerable<String> ChargingModes { get; set; }

        public IEnumerable<String> ChargingFacilities { get; set; }

        public I18NString AdditionalInfo { get; set; }



        #region Status

        /// <summary>
        /// The current EVSE status.
        /// </summary>
        [Mandatory, Not_eMI3defined]
        public Timestamped<EVSEStatusType> Status
        {

            get
            {
                return _StatusHistory.Peek();
            }

            set
            {
                SetStatus(DateTime.Now, value);
            }

        }

        #endregion

        #region StatusHistory

        private Stack<Timestamped<EVSEStatusType>> _StatusHistory;

        /// <summary>
        /// The EVSE status history.
        /// </summary>
        public IEnumerable<Timestamped<EVSEStatusType>> StatusHistory
        {
            get
            {
                return _StatusHistory.OrderByDescending(v => v.Timestamp);
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

        #region SocketOutlets

        private readonly ConcurrentDictionary<SocketOutlet_Id, SocketOutlet> _SocketOutlets;

        public IEnumerable<SocketOutlet> SocketOutlets
        {
            get
            {
                return _SocketOutlets.Select(KVP => KVP.Value);
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
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having the given EVSE_Id.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="ChargingStation">The parent EVS pool.</param>
        /// <param name="EVSEStatusHistorySize">The default size of the EVSE status history.</param>
        internal EVSE(EVSE_Id          Id,
                      ChargingStation  ChargingStation,
                      UInt16           EVSEStatusHistorySize = DefaultEVSEStatusHistorySize)

            : base(Id)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The charging station must not be null!");

            #endregion

            #region Init data and properties

            this._ChargingStation        = ChargingStation;

            this._StatusHistory          = new Stack<Timestamped<EVSEStatusType>>((Int32) EVSEStatusHistorySize);
            this._StatusHistory.Push(new Timestamped<EVSEStatusType>(EVSEStatusType.Unknown));

            this._SocketOutlets          = new ConcurrentDictionary<SocketOutlet_Id, SocketOutlet>();

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


        #region CreateNewSocketOutlet(SocketOutlet_Id, Action = null)

        /// <summary>
        /// Create and register a new socket outlet having the given
        /// unique socket outlet identification.
        /// </summary>
        /// <param name="SocketOutlet_Id">The unique identification of the new socket outlet.</param>
        /// <param name="Action">An optional delegate to configure the new socket outlet after its creation.</param>
        public SocketOutlet CreateNewSocketOutlet(SocketOutlet_Id SocketOutlet_Id, Action<SocketOutlet> Action = null)
        {

            #region Initial checks

            if (SocketOutlet_Id == null)
                throw new ArgumentNullException("SocketOutlet_Id", "The given socket outlet identification must not be null!");

            if (_SocketOutlets.ContainsKey(SocketOutlet_Id))
                throw new SocketOutletAlreadyExists(SocketOutlet_Id, this.Id);

            #endregion

            var _SocketOutlet = new SocketOutlet(SocketOutlet_Id, this);

            Action.FailSafeInvoke(_SocketOutlet);

            if (SocketOutletAddition.SendVoting(DateTime.Now, this, _SocketOutlet))
            {
                if (_SocketOutlets.TryAdd(SocketOutlet_Id, _SocketOutlet))
                {
                    SocketOutletAddition.SendNotification(DateTime.Now, this, _SocketOutlet);
                    return _SocketOutlet;
                }
            }

            throw new Exception();

        }

        #endregion

        #region SetSocketOutlets(SocketOutlets)

        public EVSE SetSocketOutlets(IEnumerable<SocketOutlet> SocketOutlets)
        {

            foreach (var SocketOutlet in SocketOutlets)
                this._SocketOutlets.TryAdd(SocketOutlet.Id, SocketOutlet);

            return this;

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

            if (_StatusHistory.Peek().Value != NewStatus.Value)
            {

                var OldStatus = _StatusHistory.Peek();

                _StatusHistory.Push(NewStatus);

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
                    if (_StatusHistory.Peek().Value != NewStatus.Value)
                    {

                        var OldStatus = _StatusHistory.Peek();

                        _StatusHistory.Push(NewStatus);

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


        #region IEnumerable<SocketOutlet> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _SocketOutlets.Values.GetEnumerator();
        }

        public IEnumerator<SocketOutlet> GetEnumerator()
        {
            return _SocketOutlets.Values.GetEnumerator();
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
        /// <param name="EVSE">An EVSE object to compare with.</param>
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
        /// Compares two EVSE for equality.
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
