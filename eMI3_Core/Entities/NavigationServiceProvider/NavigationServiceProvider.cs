/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// The Navigation Service Provider provides services like searching,
    /// routing and navigation to ev drivers. In order to provide these
    /// services the NSP receives the locations, characteristics and
    /// real-time status information of the EVSEs from multiple contracted
    /// EVSE operators. By this NSPs are a natural aggregator of ev related
    /// data.
    /// For basic service delivery no contract between the NSP and ev driver
    /// is required. Additional services can be provided if the NSP can access
    /// required data from the ev drivers’ account located at its EV service
    /// provider (e.g. type of ev, battery capacity, special service offers,
    /// special pricing).
    /// </summary>
    public class NavigationServiceProvider : AEntity<NavigationServiceProvider_Id>,
                                             IEquatable<NavigationServiceProvider>, IComparable<NavigationServiceProvider>, IComparable,
                                             IEnumerable<ChargingPool>
    {

        #region Data

<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
        internal readonly RoamingNetwork                             RoamingNetwork;
        private  readonly ConcurrentDictionary<EVSPool_Id, EVSPool>  _RegisteredEVSPools;
=======
        private readonly ConcurrentDictionary<ChargingPool_Id, ChargingPool>  _RegisteredChargingPools;
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs

        #endregion

        #region Properties

        #region RoamingNetwork

        private readonly RoamingNetwork _RoamingNetwork;

        /// <summary>
        /// The associated EV Roaming Network of the Navigation Service Provider.
        /// </summary>
        public RoamingNetwork RoamingNetwork
        {
            get
            {
<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
                return EVSPoolAddition;
            }
        }

        #endregion


        // EVS pool events

        #region ChargingStationAddition

        internal readonly IVotingNotificator<EVSPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<EVSPool, ChargingStation, Boolean> OnChargingStationAddition
        {
            get
            {
                return ChargingStationAddition;
            }
        }

        #endregion


        // Charging station events

        #region EVSEAddition

        internal readonly IVotingNotificator<ChargingStation, EVSE, Boolean> EVSEAddition;

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
=======
                return _RoamingNetwork;
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs
            }
        }

        #endregion

        #region Name

        private I8NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the Navigation Service Provider.
        /// </summary>
        [Mandatory]
        public I8NString Name
        {

            get
            {
                return _Name;
            }

            set
            {
                SetProperty<I8NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I8NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the Navigation Service Provider.
        /// </summary>
        [Optional]
        public I8NString Description
        {

            get
            {
                return _Description;
            }

            set
            {
                SetProperty<I8NString>(ref _Description, value);
            }

        }

        #endregion


        #region ChargingPools

<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
        /// <summary>
        /// Return all EVS pools registered within this EVSE operator.
        /// </summary>
        public IEnumerable<EVSPool> EVSPools
=======
        public IEnumerable<ChargingPool> ChargingPools
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs
        {
            get
            {
                return _RegisteredChargingPools.Values;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (internal) NavigationServiceProvider()

        /// <summary>
<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSEOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs)
        /// and having the given EVSE operator identification.
        /// </summary>
        /// <param name="RoamingNetwork">The parent roaming network.</param>
        internal EVSEOperator(RoamingNetwork  RoamingNetwork)
            : this(EVSEOperator_Id.New, RoamingNetwork)
=======
        /// Create a new Navigation Service Provider (NSP).
        /// </summary>
        internal NavigationServiceProvider(RoamingNetwork  RoamingNetwork)
            : this(NavigationServiceProvider_Id.New, RoamingNetwork)
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs
        { }

        #endregion

        #region (internal) NavigationServiceProvider(Id)

        /// <summary>
<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSEOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs)
        /// and having the given EVSE operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE operator.</param>
        /// <param name="RoamingNetwork">The parent roaming network.</param>
        internal EVSEOperator(EVSEOperator_Id  Id,
                              RoamingNetwork   RoamingNetwork)
            : base(Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the roaming network must not be null!");

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The roaming network must not be null!");

            this.RoamingNetwork = RoamingNetwork;

            #endregion

            #region Init data and properties

            this._RegisteredEVSPools      = new ConcurrentDictionary<EVSPool_Id, EVSPool>();

            this.Name                     = new I8NString();

            #endregion

            #region Init and link events

            // EVSEOperator events
            this.EVSPoolAddition          = new VotingNotificator<EVSEOperator,    EVSPool,         Boolean>(() => new VetoVote(), true);

            this.OnEVSPoolAddition.        OnVoting       += (evseoperator, evspool, vote) => RoamingNetwork.EVSPoolAddition.SendVoting      (evseoperator, evspool, vote);
            this.OnEVSPoolAddition.        OnNotification += (evseoperator, evspool)       => RoamingNetwork.EVSPoolAddition.SendNotification(evseoperator, evspool);


            // EVS pool events
            this.ChargingStationAddition  = new VotingNotificator<EVSPool,         ChargingStation, Boolean>(() => new VetoVote(), true);

            this.OnChargingStationAddition.OnVoting       += (evseoperator, evspool, vote) => RoamingNetwork.ChargingStationAddition.SendVoting      (evseoperator, evspool, vote);
            this.OnChargingStationAddition.OnNotification += (evseoperator, evspool)       => RoamingNetwork.ChargingStationAddition.SendNotification(evseoperator, evspool);


            // Charging station events
            this.EVSEAddition             = new VotingNotificator<ChargingStation, EVSE,            Boolean>(() => new VetoVote(), true);

            this.OnEVSEAddition.           OnVoting       += (chargingstation, evse, vote) => RoamingNetwork.EVSEAddition.SendVoting      (chargingstation, evse, vote);
            this.OnEVSEAddition.           OnNotification += (chargingstation, evse)       => RoamingNetwork.EVSEAddition.SendNotification(chargingstation, evse);


            // EVSE events
            this.SocketOutletAddition     = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            this.SocketOutletAddition.OnVoting            += (evse, socketoutlet , vote)   => RoamingNetwork.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
            this.SocketOutletAddition.OnNotification      += (evse, socketoutlet)          => RoamingNetwork.SocketOutletAddition.SendNotification(evse, socketoutlet);

            #endregion

        }

        #endregion
=======
        /// Create a new Navigation Service Provider (NSP)
        /// having the given NSP_Id.
        /// </summary>
        /// <param name="Id">The Navigation Service Provider Identification.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal NavigationServiceProvider(NavigationServiceProvider_Id  Id,
                                           RoamingNetwork                RoamingNetwork)
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs

            : base(Id)

        {

<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
        #region CreateNewEVSPool(EVSPool_Id, Action)

        /// <summary>
        /// Create and register a new EVS pool having the given
        /// unique EVS pool identification.
        /// </summary>
        /// <param name="EVSPool_Id">The unique identification of the new EVS pool.</param>
        /// <param name="Action">An optional delegate to configure the new EVS pool after its creation.</param>
        public EVSPool CreateNewEVSPool(EVSPool_Id EVSPool_Id, Action<EVSPool> Action)
        {

            #region Initial checks

            if (EVSPool_Id == null)
                throw new ArgumentNullException("EVSPool_Id", "The given EVSPool_Id must not be null!");
=======
            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the navigation service provider must not be null!");

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The roaming network must not be null!");
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs

            #endregion

<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
            #endregion
=======
            #region Init data and properties
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs

            this._RoamingNetwork            = RoamingNetwork;

<<<<<<< HEAD:eMI3_Core/RoamingNetwork/EVSEOperator/EVSEOperator.cs
            Action.FailSafeRun(_EVSPool);
=======
            this._Name                      = new I8NString();
            this._Description               = new I8NString();
>>>>>>> eda9d1ffad4f5be4e2672bd1ed4b681a16a312d5:eMI3_Core/Entities/NavigationServiceProvider/NavigationServiceProvider.cs

            this._RegisteredChargingPools   = new ConcurrentDictionary<ChargingPool_Id, ChargingPool>();

            #endregion

        }

        #endregion

        #endregion


        #region IEnumerable<EVSPool> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _RegisteredChargingPools.Values.GetEnumerator();
        }

        public IEnumerator<ChargingPool> GetEnumerator()
        {
            return _RegisteredChargingPools.Values.GetEnumerator();
        }

        #endregion

        #region IComparable<EVSE_Operator> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as NavigationServiceProvider;
            if ((Object) EVSE_Operator == null)
                throw new ArgumentException("The given object is not an EVSE_Operator!");

            return CompareTo(EVSE_Operator);

        }

        #endregion

        #region CompareTo(EVSE_Operator)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSE_Operator">An EVSE_Operator object to compare with.</param>
        public Int32 CompareTo(NavigationServiceProvider EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                throw new ArgumentNullException("The given EVSE_Operator must not be null!");

            return Id.CompareTo(EVSE_Operator.Id);

        }

        #endregion

        #endregion

        #region IEquatable<EVSE_Operator> Members

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

            // Check if the given object is an EVSE_Operator.
            var EVSE_Operator = Object as NavigationServiceProvider;
            if ((Object) EVSE_Operator == null)
                return false;

            return this.Equals(EVSE_Operator);

        }

        #endregion

        #region Equals(EVSE_Operator)

        /// <summary>
        /// Compares two EVSE_Operator for equality.
        /// </summary>
        /// <param name="EVSE_Operator">An EVSE_Operator to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(NavigationServiceProvider EVSE_Operator)
        {

            if ((Object) EVSE_Operator == null)
                return false;

            return Id.Equals(EVSE_Operator.Id);

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
