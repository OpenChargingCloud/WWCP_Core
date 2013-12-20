/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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
using System.Collections.Generic;
using System.Collections.Concurrent;

using eu.Vanaheimr.Illias.Commons;
using eu.Vanaheimr.Illias.Commons.Votes;
using eu.Vanaheimr.Styx.Arrows;

#endregion

namespace org.emi3group
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

        #region EVSEStatus

        private EVSEStatusType _Status = EVSEStatusType.OutOfService;

        [Mandatory, Not_eMI3defined]
        public EVSEStatusType Status
        {

            get
            {
                return _Status;
            }

            set
            {
                SetProperty<EVSEStatusType>(ref _Status, value);
            }

        }

        #endregion


        public IEnumerable<SocketOutlet> SocketOutlets
        {
            get
            {
                return _SocketOutlets.Values;
            }
        }

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

        #region (internal) EVSE(Pool)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having a random EVSE_Id.
        /// </summary>
        /// <param name="ChargingStation">The parent EVS pool.</param>
        public EVSE(ChargingStation ChargingStation)
            : this(EVSE_Id.New, ChargingStation)
        { }

        #endregion

        #region (internal) EVSE(Id, Pool)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment (EVSE)
        /// having the given EVSE_Id.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="ChargingStation">The parent EVS pool.</param>
        internal EVSE(EVSE_Id          Id,
                      ChargingStation  ChargingStation)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the EVSE must not be null!");

            if (ChargingStation == null)
                throw new ArgumentNullException("ChargingStation", "The charging station must not be null!");

            this.ChargingStation = ChargingStation;

            #endregion

            #region Init data and properties

            this._SocketOutlets          = new ConcurrentDictionary<SocketOutlet_Id, SocketOutlet>();

            #endregion

            #region Init and link events

            this.SocketOutletAddition = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            this.OnSocketOutletAddition.OnVoting       += (evse, socketoutlet, vote) => ChargingStation.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.OnSocketOutletAddition.OnNotification += (evse, socketoutlet)       => ChargingStation.SocketOutletAddition.SendNotification(evse, socketoutlet);

            #endregion

        }

        #endregion

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

            Action.FailSafeRun(_SocketOutlet);

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
