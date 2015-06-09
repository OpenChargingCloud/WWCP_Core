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
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The Electric Vehicle Supply Equipment Operator is responsible for
    /// operating EV Charging Pools and with them also EV Charging Stations
    /// and EVSEs. The EVSE operator is not neccessarily also the owner of
    /// these charging stations. For the delivered service (energy, parking,
    /// etc.) the EVSE operator will either be payed directly by the ev driver
    /// or by an contracted EVSP of the ev driver.
    /// The EVSE operator delivers the locations, characteristics and real-time
    /// status information of its EV Charging Pools as linked open data to EVSPs,
    /// NSPs or the public. Pricing information might be either public
    /// information or part of business-to-business contracts.
    /// </summary>
    public class EVSEOperator : AEntity<EVSEOperator_Id>,
                                IEquatable<EVSEOperator>, IComparable<EVSEOperator>, IComparable,
                                IEnumerable<ChargingPool>
    {

        #region Events

        #region ChargingPoolAddition

        private readonly IVotingNotificator<EVSEOperator, ChargingPool, Boolean> ChargingPoolAddition;

        /// <summary>
        /// Called whenever a charging pool will be or was added.
        /// </summary>
        public IVotingSender<EVSEOperator, ChargingPool, Boolean> OnChargingPoolAddition
        {
            get
            {
                return ChargingPoolAddition;
            }
        }

        #endregion


        // EVS pool events

        #region ChargingStationAddition

        internal readonly IVotingNotificator<ChargingPool, ChargingStation, Boolean> ChargingStationAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<ChargingPool, ChargingStation, Boolean> OnChargingStationAddition
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
            }
        }

        #endregion

        #endregion

        #region Properties

        #region RoamingNetwork

        private readonly RoamingNetwork _RoamingNetwork;

        /// <summary>
        /// The associated EV Roaming Network of the Electric Vehicle Supply Equipment Operator.
        /// </summary>
        public RoamingNetwork RoamingNetwork
        {
            get
            {
                return _RoamingNetwork;
            }
        }

        #endregion

        #region Name

        private readonly I18NString _Name;

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
        }

        #endregion

        #region Description

        private readonly I18NString _Description;

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
        }

        #endregion


        #region ValidEVSEIds

        private List<EVSE_Id> _ValidEVSEIds;

        /// <summary>
        /// A list of valid EVSE Ids. All others will be filtered.
        /// </summary>
        public List<EVSE_Id> ValidEVSEIds
        {
            get
            {
                return _ValidEVSEIds;
            }
        }

        #endregion

        #region InvalidEVSEIds

        private List<EVSE_Id> _InvalidEVSEIds;

        /// <summary>
        /// A list of invalid EVSE Ids.
        /// </summary>
        public List<EVSE_Id> InvalidEVSEIds
        {
            get
            {
                return _InvalidEVSEIds;
            }
        }

        #endregion

        #region ManualEVSEIds

        private List<EVSE_Id> _ManualEVSEIds;

        /// <summary>
        /// A list of manual EVSE Ids which will not be touched automagically.
        /// </summary>
        public List<EVSE_Id> ManualEVSEIds
        {
            get
            {
                return _ManualEVSEIds;
            }
        }

        #endregion


        #region AllEVSEIds

        public IEnumerable<EVSE_Id> AllEVSEIds
        {
            get
            {
                return _ChargingPools.Values.
                           SelectMany(v => v.ChargingStations).
                           SelectMany(v => v.EVSEs).
                           Select    (v => v.Id);
            }
        }

        #endregion

        #region AllEVSEStatus

        public IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> AllEVSEStatus
        {
            get
            {
                return _ChargingPools.Values.
                           SelectMany(v => v.ChargingStations).
                           SelectMany(v => v.EVSEs).
                           Select    (v => new KeyValuePair<EVSE_Id, EVSEStatusType>(v.Id, v.Status.Value));
            }
        }

        #endregion


        #region ChargingPools

        private readonly ConcurrentDictionary<ChargingPool_Id, ChargingPool> _ChargingPools;

        /// <summary>
        /// Return all EV Charging Pools registered within this EVSE operator.
        /// </summary>
        public IEnumerable<ChargingPool> ChargingPools
        {
            get
            {
                return _ChargingPools.Values;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Electric Vehicle Supply Equipment Operator (EVSEOP) to manage
        /// multiple Electric Vehicle Supply Equipments (EVSEs)
        /// and having the given EVSE operator identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE operator.</param>
        /// <param name="Name">The offical (multi-language) name of the EVSE Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the EVSE Operator.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal EVSEOperator(EVSEOperator_Id  Id,
                              I18NString        Name           = null,
                              I18NString        Description    = null,
                              RoamingNetwork   RoamingNetwork = null)

            : base(Id)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the roaming network must not be null!");

            //if (RoamingNetwork == null)
            //    throw new ArgumentNullException("RoamingNetwork", "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this._RoamingNetwork            = RoamingNetwork;

            this._Name                      = Name        != null ? Name        : new I18NString();
            this._Description               = Description != null ? Description : new I18NString();
            this._ValidEVSEIds              = new List<EVSE_Id>();
            this._InvalidEVSEIds            = new List<EVSE_Id>();
            this._ManualEVSEIds             = new List<EVSE_Id>();

            this._ChargingPools   = new ConcurrentDictionary<ChargingPool_Id, ChargingPool>();

            #endregion

            #region Init and link events

            // EVSEOperator events
            this.ChargingPoolAddition          = new VotingNotificator<EVSEOperator,    ChargingPool,         Boolean>(() => new VetoVote(), true);

            if (RoamingNetwork != null)
            {
                this.OnChargingPoolAddition.OnVoting       += (evseoperator, evspool, vote) => RoamingNetwork.EVSPoolAddition.SendVoting      (evseoperator, evspool, vote);
                this.OnChargingPoolAddition.OnNotification += (evseoperator, evspool)       => RoamingNetwork.EVSPoolAddition.SendNotification(evseoperator, evspool);
            }


            // EVS pool events
            this.ChargingStationAddition  = new VotingNotificator<ChargingPool,         ChargingStation, Boolean>(() => new VetoVote(), true);

            if (RoamingNetwork != null)
            {
                this.OnChargingStationAddition.OnVoting       += (evseoperator, evspool, vote) => RoamingNetwork.ChargingStationAddition.SendVoting      (evseoperator, evspool, vote);
                this.OnChargingStationAddition.OnNotification += (evseoperator, evspool)       => RoamingNetwork.ChargingStationAddition.SendNotification(evseoperator, evspool);
            }


            // Charging station events
            this.EVSEAddition             = new VotingNotificator<ChargingStation, EVSE,            Boolean>(() => new VetoVote(), true);

            if (RoamingNetwork != null)
            {
                this.OnEVSEAddition.OnVoting       += (chargingstation, evse, vote) => RoamingNetwork.EVSEAddition.SendVoting      (chargingstation, evse, vote);
                this.OnEVSEAddition.OnNotification += (chargingstation, evse)       => RoamingNetwork.EVSEAddition.SendNotification(chargingstation, evse);
            }


            // EVSE events
            this.SocketOutletAddition     = new VotingNotificator<EVSE, SocketOutlet, Boolean>(() => new VetoVote(), true);

            if (RoamingNetwork != null)
            {
                this.SocketOutletAddition.OnVoting            += (evse, socketoutlet , vote)   => RoamingNetwork.SocketOutletAddition.SendVoting      (evse, socketoutlet, vote);
                this.SocketOutletAddition.OnNotification      += (evse, socketoutlet)          => RoamingNetwork.SocketOutletAddition.SendNotification(evse, socketoutlet);
            }

            #endregion

        }

        #endregion


        #region CreateNew(Id, Name = null, Description = null, RoamingNetwork = null)

        /// <summary>
        /// Create a new EVSE operator having a random identification.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE operator.</param>
        /// <param name="Name">The offical (multi-language) name of the EVSE Operator.</param>
        /// <param name="Description">An optional (multi-language) description of the EVSE Operator.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        public static EVSEOperator CreateNew(EVSEOperator_Id  Id,
                                             I18NString        Name           = null,
                                             I18NString        Description    = null,
                                             RoamingNetwork   RoamingNetwork = null)
        {

            return new EVSEOperator(Id, Name, Description, RoamingNetwork);

        }

        #endregion

        #region CreateNewChargingPool(ChargingPoolId = null, Configurator = null, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging pool having the given
        /// unique charging pool identification.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the new charging pool.</param>
        /// <param name="Configurator">An optional delegate to configure the new charging pool before its successful creation.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging pool after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging pool failed.</param>
        public ChargingPool CreateNewChargingPool(ChargingPool_Id                        ChargingPoolId  = null,
                                                  Action<ChargingPool>                   Configurator    = null,
                                                  Action<ChargingPool>                   OnSuccess       = null,
                                                  Action<EVSEOperator, ChargingPool_Id>  OnError         = null)
        {

            #region Initial checks

            if (ChargingPoolId == null)
                ChargingPoolId = ChargingPool_Id.New();

            // Do not throw an exception when an OnError delegate was given!
            if (_ChargingPools.ContainsKey(ChargingPoolId))
            {
                if (OnError == null)
                    throw new EVSPoolAlreadyExists(ChargingPoolId, this.Id);
                else
                    OnError.FailSafeInvoke(this, ChargingPoolId);
            }

            #endregion

            var _ChargingPool = new ChargingPool(ChargingPoolId, this);

            if (Configurator != null)
                Configurator(_ChargingPool);

            if (ChargingPoolAddition.SendVoting(this, _ChargingPool))
            {
                if (_ChargingPools.TryAdd(ChargingPoolId, _ChargingPool))
                {
                    OnSuccess.FailSafeInvoke(_ChargingPool);
                    ChargingPoolAddition.SendNotification(this, _ChargingPool);
                    return _ChargingPool;
                }
            }

            return null;

        }

        #endregion

        #region ContainsChargingPoolId(ChargingPoolId)

        /// <summary>
        /// Check if the given ChargingPool identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="ChargingPoolId">The unique identification of the charging pool.</param>
        public Boolean ContainsChargingPoolId(ChargingPool_Id ChargingPoolId)
        {
            return _ChargingPools.ContainsKey(ChargingPoolId);
        }

        #endregion

        #region GetChargingPoolbyId(ChargingPoolId)

        public ChargingPool GetChargingPoolbyId(ChargingPool_Id ChargingPoolId)
        {

            ChargingPool _ChargingPool = null;

            if (_ChargingPools.TryGetValue(ChargingPoolId, out _ChargingPool))
                return _ChargingPool;

            return null;

        }

        #endregion

        #region TryGetChargingPoolbyId(ChargingPoolId, out ChargingPool)

        public Boolean TryGetChargingPoolbyId(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {
            return _ChargingPools.TryGetValue(ChargingPoolId, out ChargingPool);
        }

        #endregion

        public Boolean TryRemoveChargingPool(ChargingPool_Id ChargingPoolId, out ChargingPool ChargingPool)
        {
            return _ChargingPools.TryRemove(ChargingPoolId, out ChargingPool);
        }



        #region GetChargingStationbyId(ChargingStationId)

        public ChargingStation GetChargingStationbyId(ChargingStation_Id ChargingStationId)
        {
            return _ChargingPools.Values.
                       SelectMany(pool    => pool.ChargingStations).
                       Where     (station => station.Id == ChargingStationId).
                       FirstOrDefault();
        }

        #endregion

        #region TryGetChargingStationbyId(ChargingStationId, out ChargingStation ChargingStation)

        public Boolean TryGetChargingStationbyId(ChargingStation_Id ChargingStationId, out ChargingStation ChargingStation)
        {

            ChargingStation = _ChargingPools.Values.
                                  SelectMany(pool    => pool.ChargingStations).
                                  Where     (station => station.Id == ChargingStationId).
                                  FirstOrDefault();

            return ChargingStation != null;

        }

        #endregion

        #region ContainsChargingStationId(ChargingStationId)

        /// <summary>
        /// Check if the given ChargingStation identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="ChargingStationId">The unique identification of the charging station.</param>
        public Boolean ContainsChargingStationId(ChargingStation_Id ChargingStationId)
        {
            return _ChargingPools.Any(v => v.Value.ContainsChargingStationId(ChargingStationId));
        }

        #endregion


        #region GetEVSEbyId(EVSEId)

        public EVSE GetEVSEbyId(EVSE_Id EVSEId)
        {
            return _ChargingPools.Values.
                       SelectMany(pool    => pool.   ChargingStations).
                       SelectMany(station => station.EVSEs).
                       Where     (evse    => evse.Id == EVSEId).
                       FirstOrDefault();
        }

        #endregion

        #region TryGetEVSEbyId(EVSEId, out EVSE)

        public Boolean TryGetEVSEbyId(EVSE_Id EVSEId, out EVSE EVSE)
        {

            EVSE = _ChargingPools.Values.
                       SelectMany(pool    => pool.   ChargingStations).
                       SelectMany(station => station.EVSEs).
                       Where     (evse    => evse.Id == EVSEId).
                       FirstOrDefault();

            return EVSE != null;

        }

        #endregion

        #region ContainsEVSEId(EVSEId)

        /// <summary>
        /// Check if the given EVSE identification is already present within the EVSE operator.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        public Boolean ContainsEVSEId(EVSE_Id EVSEId)
        {
            return _ChargingPools.Any(v => v.Value.ContainsEVSEId(EVSEId));
        }

        #endregion



        #region SetEVSEStatus(EVSEId, NewStatus, SendUpstream = false)

        public void SetEVSEStatus(EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusType>  NewStatus,
                                  Boolean                      SendUpstream = false)
        {
            SetEVSEStatus(DateTime.Now, EVSEId, NewStatus);
        }

        public void SetEVSEStatus(DateTime                     Timestamp,
                                  EVSE_Id                      EVSEId,
                                  Timestamped<EVSEStatusType>  NewStatus,
                                  Boolean                      SendUpstream = false)
        {

            if (InvalidEVSEIds.Contains(EVSEId))
                return;

            EVSE _EVSE = null;
            if (TryGetEVSEbyId(EVSEId, out _EVSE))
            {

                _EVSE.SetStatus(Timestamp, NewStatus);

                if (SendUpstream)
                {
                    RoamingNetwork.
                        RequestRouter.
                        SendEVSEStatusDiff(new EVSEStatusDiff(new List<KeyValuePair<EVSE_Id, EVSEStatusType>>(),
                                                               new List<KeyValuePair<EVSE_Id, EVSEStatusType>>() {
                                                                   new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSEId, NewStatus.Value)
                                                               },
                                                               new List<EVSE_Id>()));
                }

            }

            else
                DebugX.Log("Could not set status for EVSE '" + EVSEId.OriginEVSEId.ToString() + "'!");

        }

        #endregion

        #region CalcEVSEStatusDiff(EVSEStatus, AutoApply = false)

        public EVSEStatusDiff CalcEVSEStatusDiff(Dictionary<EVSE_Id, EVSEStatusType>  EVSEStatus,
                                                 Boolean                              AutoApply = false)
        {

            #region Get data...

            var EVSEStatusDiff     = new EVSEStatusDiff();

            // Only ValidEVSEIds!
            // Do nothing with manual EVSE Ids!
            var CurrentEVSEStates  = AllEVSEStatus.
                                     Where(KVP => ValidEVSEIds. Contains(KVP.Key) &&
                                                 !ManualEVSEIds.Contains(KVP.Key)).
                                     ToDictionary(v => v.Key, v => v.Value);

            var OldEVSEIds         = new List<EVSE_Id>(CurrentEVSEStates.Keys);

            #endregion

            try
            {

                #region Find new and changed EVSE states

                // Only for ValidEVSEIds!
                // Do nothing with manual EVSE Ids!
                foreach (var NewEVSEStatus in EVSEStatus.
                                                  Where(KVP => ValidEVSEIds. Contains(KVP.Key) &&
                                                              !ManualEVSEIds.Contains(KVP.Key)))
                {

                    // Add to NewEVSEStates, if new EVSE was found!
                    if (!CurrentEVSEStates.ContainsKey(NewEVSEStatus.Key))
                        EVSEStatusDiff.NewEVSEStates.Add(NewEVSEStatus);

                    else
                    {

                        // Add to CHANGED, if state of known EVSE changed!
                        if (CurrentEVSEStates[NewEVSEStatus.Key] != NewEVSEStatus.Value)
                            EVSEStatusDiff.ChangedEVSEStates.Add(NewEVSEStatus);

                        // Remove EVSEId, as it was processed...
                        OldEVSEIds.Remove(NewEVSEStatus.Key);

                    }

                }

                #endregion

                #region Delete what is left in OldEVSEIds!

                EVSEStatusDiff.RemovedEVSEIds.AddRange(OldEVSEIds);

                #endregion

                if (AutoApply)
                    ApplyEVSEStatusDiff(EVSEStatusDiff);

                return EVSEStatusDiff;

            }

            catch (Exception e)
            {
                DebugX.Log("GetEVSEStatusDiff lead to an exception: " + e.Message);
            }

            // empty!
            return new EVSEStatusDiff();

        }

        #endregion

        #region ApplyEVSEStatusDiff(StatusDiff)

        public EVSEStatusDiff ApplyEVSEStatusDiff(EVSEStatusDiff StatusDiff)
        {

            foreach (var EVSEState in StatusDiff.NewEVSEStates)
                SetEVSEStatus(EVSEState.Key, EVSEState.Value);

            foreach (var EVSEState in StatusDiff.ChangedEVSEStates)
                SetEVSEStatus(EVSEState.Key, EVSEState.Value);

            RoamingNetwork.RequestRouter.SendEVSEStatusDiff(StatusDiff);

            return StatusDiff;

        }

        #endregion


        #region IEnumerable<EVSPool> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _ChargingPools.Values.GetEnumerator();
        }

        public IEnumerator<ChargingPool> GetEnumerator()
        {
            return _ChargingPools.Values.GetEnumerator();
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
            var EVSE_Operator = Object as EVSEOperator;
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
        public Int32 CompareTo(EVSEOperator EVSE_Operator)
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
            var EVSE_Operator = Object as EVSEOperator;
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
        public Boolean Equals(EVSEOperator EVSE_Operator)
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
