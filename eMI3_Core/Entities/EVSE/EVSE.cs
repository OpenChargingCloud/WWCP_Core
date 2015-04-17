/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// An Electric Vehicle Supply Equipment (EVSE) to charge an electric vehicle (EV).
    /// This is meant to be one electrical circuit which can charge a electric vehicle
    /// independently. Thus there could be multiple interdependent power sockets.
    /// </summary>
    public class EVSE : AEntity<EVSE_Id>,
                        IEquatable<EVSE>, IComparable<EVSE>, IComparable,
                        IEnumerable<SocketOutlet>
    {

        #region Data

        public  const    UInt32                                               DefaultStatusHistorySize = 50;
        public  readonly ChargingStation                                      ChargingStation;
        private readonly ConcurrentDictionary<SocketOutlet_Id, SocketOutlet>  _SocketOutlets;

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

        #region Status

        /// <summary>
        /// The current EVSE status.
        /// </summary>
        [Mandatory, Not_eMI3defined]
        public Timestamped<EVSEStatusType> Status
        {
            get
            {

                if (_StatusHistory.Count == 0)
                    return new Timestamped<EVSEStatusType>(EVSEStatusType.EvseNotFound);

                return _StatusHistory.Peek();

            }
        }

        #endregion

        #region StatusHistory

        private Queue<Timestamped<EVSEStatusType>> _StatusHistory;

        public IEnumerable<Timestamped<EVSEStatusType>> StatusHistory
        {
            get
            {
                return _StatusHistory.OrderByDescending(v => v.Timestamp);
            }
        }

        #endregion

        public IEnumerable<String> ChargingFacilities   { get; set; }
        public IEnumerable<String> ChargingModes        { get; set; }
        public IEnumerable<String> AuthenticationModes  { get; set; }
        public Double              MaxCapacity_kWh      { get; set; }


        #region SocketOutlets

        public IEnumerable<SocketOutlet> SocketOutlets
        {
            get
            {
                return _SocketOutlets.Values;
            }
        }

        #endregion

        #endregion

        #region Events

        #region SocketOutletAddition

        private readonly IVotingNotificator<EVSE, SocketOutlet, Boolean> SocketOutletAddition;

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

        #endregion

        #region Constructor(s)

        #region (internal) EVSE(Id)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having the given EVSE_Id.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        internal EVSE(EVSE_Id  Id)

            : base(Id)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the EVSE must not be null!");

            #endregion

            #region Init data and properties

            this._SocketOutlets          = new ConcurrentDictionary<SocketOutlet_Id, SocketOutlet>();

            #endregion

            #region Init and link events

            this.SocketOutletAddition = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            #endregion

        }

        #endregion

        #region (internal) EVSE(Id, ChargingStation, StatusHistorySize = DefaultStatusHistorySize)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having the given EVSE_Id.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="ChargingStation">The parent EVS pool.</param>
        /// <param name="StatusHistorySize">The default size of the EVSE status history.</param>
        internal EVSE(EVSE_Id          Id,
                      ChargingStation  ChargingStation,
                      UInt32           StatusHistorySize = DefaultStatusHistorySize)

            : this(Id)

        {

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The charging station must not be null!");

            this.ChargingStation = ChargingStation;

            this._StatusHistory = new Queue<Timestamped<EVSEStatusType>>((Int32) StatusHistorySize);

            this.OnSocketOutletAddition.OnVoting       += (evse, socketoutlet, vote) => ChargingStation.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.OnSocketOutletAddition.OnNotification += (evse, socketoutlet)       => ChargingStation.SocketOutletAddition.SendNotification(evse, socketoutlet);

        }

        #endregion

        #endregion


        #region CreateNew(Id)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE) having the given identification.
        /// </summary>
        /// <param name="Id">The unique identification of the Electric Vehicle Supply Equipment (EVSE).</param>
        public static EVSE CreateNew(EVSE_Id Id)
        {
            return new EVSE(Id);
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

            if (SocketOutletAddition.SendVoting(this, _SocketOutlet))
            {
                if (_SocketOutlets.TryAdd(SocketOutlet_Id, _SocketOutlet))
                {
                    SocketOutletAddition.SendNotification(this, _SocketOutlet);
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


        public Boolean StatusIs(EVSEStatusType Status)
        {

            if (_StatusHistory.Count == 0)
                return false;

            if (_StatusHistory.Peek().Value == Status)
                return true;

            return false;

        }

        public Boolean StatusIsNot(EVSEStatusType Status)
        {

            if (_StatusHistory.Count == 0)
                return true;

            if (_StatusHistory.Peek().Value != Status)
                return true;

            return false;

        }

        #region SetStatus(Status)

        /// <summary>
        /// Set the current EVSE status.
        /// </summary>
        /// <param name="Status">The EVSE status.</param>
        public EVSE SetStatus(Timestamped<EVSEStatusType> Status)
        {
            _StatusHistory.Enqueue(Status);
            return this;
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
            return "eMI3 EVSE: " + Id.ToString();
        }

        #endregion

    }

}
