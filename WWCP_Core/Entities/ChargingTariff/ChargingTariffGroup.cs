/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging tariff group.
    /// </summary>
    public class ChargingTariffGroup : AEMobilityEntity<ChargingTariffGroup_Id,
                                                        ChargingTariffGroupAdminStatusTypes,
                                                        ChargingTariffGroupStatusTypes>,
                                       IEquatable<ChargingTariffGroup>, IComparable<ChargingTariffGroup>, IComparable,
                                       IEnumerable<ChargingTariff>
    {

        #region Data

        private readonly Dictionary<ChargingTariff_Id, ChargingTariff> _ChargingTariffs;

        #endregion

        #region Properties

        /// <summary>
        /// An optional (multi-language) description of this group.
        /// </summary>
        public I18NString  Description   { get; }

        /// <summary>
        /// Return all charging stations registered within this charing station group.
        /// </summary>
        public IEnumerable<ChargingTariff> ChargingTariffs
            => _ChargingTariffs.Values;


        /// <summary>
        /// Return all charging station identifications registered within this charing station group.
        /// </summary>
        public IEnumerable<ChargingTariff_Id> ChargingTariffIds
            => ChargingTariffs.SafeSelect(station => station.Id);

        #endregion

        #region Links

        /// <summary>
        /// The Charging Station Operator of this charging pool.
        /// </summary>
        [Mandatory]
        public ChargingStationOperator Operator { get; }

        /// <summary>
        /// The roaming network of this charging station.
        /// </summary>
        [InternalUseOnly]
        public IRoamingNetwork RoamingNetwork
            => Operator?.RoamingNetwork;

        #endregion

        #region Events

        #region ChargingTariffAddition

        internal readonly IVotingNotificator<DateTime, ChargingTariffGroup, ChargingTariff, Boolean> ChargingTariffAddition;

        /// <summary>
        /// Called whenever a charging station will be or was added.
        /// </summary>
        public IVotingSender<DateTime, ChargingTariffGroup, ChargingTariff, Boolean> OnChargingTariffAddition
        {
            get
            {
                return ChargingTariffAddition;
            }
        }

        #endregion

        #region ChargingTariffRemoval

        internal readonly IVotingNotificator<DateTime, ChargingTariffGroup, ChargingTariff, Boolean> ChargingTariffRemoval;

        /// <summary>
        /// Called whenever a charging station will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, ChargingTariffGroup, ChargingTariff, Boolean> OnChargingTariffRemoval
        {
            get
            {
                return ChargingTariffRemoval;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station group.
        /// </summary>
        /// <param name="Id">The unique identification of the charing station group.</param>
        /// <param name="Operator">The charging station operator of this charging station group.</param>
        /// <param name="Description">An optional (multi-language) description of this charging station group.</param>
        internal ChargingTariffGroup(ChargingTariffGroup_Id   Id,
                                     ChargingStationOperator  Operator,
                                     I18NString               Description  = null)

            : base(Id)

        {

            this.Operator                = Operator ?? throw new ArgumentNullException(nameof(Operator), "The charging station operator must not be null!");
            this.Description             = Description ?? new I18NString();

            this._ChargingTariffs        = new Dictionary<ChargingTariff_Id, ChargingTariff>();


            this.ChargingTariffAddition  = new VotingNotificator<DateTime, ChargingTariffGroup, ChargingTariff, Boolean>(() => new VetoVote(), true);
            this.ChargingTariffRemoval   = new VotingNotificator<DateTime, ChargingTariffGroup, ChargingTariff, Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region CreateChargingTariff     (Id,       Description, OnSuccess = null, OnError = null)

        /// <summary>
        /// Create and register a new charging tariff having the given
        /// unique charging tariff identification.
        /// </summary>
        /// <param name="Id">The unique identification of the charing tariff.</param>
        /// <param name="Name">The offical (multi-language) name of this charging tariff.</param>
        /// <param name="Description">An optional (multi-language) description of this charging tariff.</param>
        /// <param name="OnSuccess">An optional delegate to configure the new charging tariff after its successful creation.</param>
        /// <param name="OnError">An optional delegate to be called whenever the creation of the charging tariff failed.</param>
        public ChargingTariff CreateChargingTariff(ChargingTariff_Id                                    Id,
                                                   I18NString                                           Name,
                                                   I18NString                                           Description,
                                                   Brand?                                               Brand,
                                                   URL?                                                 TariffURL,
                                                   Currency                                             Currency,
                                                   EnergyMix?                                           EnergyMix,
                                                   IEnumerable<ChargingTariffElement>                   TariffElements,

                                                   Action<ChargingTariff>?                              OnSuccess   = null,
                                                   Action<ChargingStationOperator, ChargingTariff_Id>?  OnError     = null)

        {

            lock (_ChargingTariffs)
            {

                #region Initial checks

                if (_ChargingTariffs.ContainsKey(Id))
                {

                    //if (OnError != null)
                    //    OnError?.Invoke(this, Id);

                    throw new ArgumentException("Invalid tariff!");

                }

                #endregion

                var _ChargingTariff = new ChargingTariff(Id,
                                                         null,
                                                         Name,
                                                         Description,
                                                         TariffElements,
                                                         Currency,
                                                         Brand,
                                                         TariffURL,
                                                         EnergyMix);


                if (ChargingTariffAddition.SendVoting(Timestamp.Now, this, _ChargingTariff))
                {

                    _ChargingTariffs.Add(_ChargingTariff.Id, _ChargingTariff);

                    //_ChargingTariff.OnEVSEDataChanged                             += UpdateEVSEData;
                    //_ChargingTariff.OnEVSEStatusChanged                           += UpdateEVSEStatus;
                    //_ChargingTariff.OnEVSEAdminStatusChanged                      += UpdateEVSEAdminStatus;

                    //_ChargingTariff.OnChargingStationDataChanged                  += UpdateChargingStationData;
                    //_ChargingTariff.OnChargingStationStatusChanged                += UpdateChargingStationStatus;
                    //_ChargingTariff.OnChargingStationAdminStatusChanged           += UpdateChargingStationAdminStatus;

                    ////_ChargingTariff.OnDataChanged                                 += UpdateChargingTariffData;
                    ////_ChargingTariff.OnAdminStatusChanged                          += UpdateChargingTariffAdminStatus;

                    OnSuccess?.Invoke(_ChargingTariff);

                    ChargingTariffAddition.SendNotification(Timestamp.Now,
                                                            this,
                                                            _ChargingTariff);

                    return _ChargingTariff;

                }

                return null;

            }

        }

        #endregion


        public ChargingTariffGroup Add(ChargingTariff Tariff)
        {

            lock (_ChargingTariffs)
            {
                _ChargingTariffs.Add(Tariff.Id, Tariff);
            }

            return this;

        }


        #region ContainsId(ChargingTariffId)

        /// <summary>
        /// Check if the given charging tariff identification is member of this charging tariff group.
        /// </summary>
        /// <param name="ChargingTariffId">The unique identification of the charging tariff.</param>
        public Boolean ContainsId(ChargingTariff_Id ChargingTariffId)
            => _ChargingTariffs.ContainsKey(ChargingTariffId);

        #endregion


        #region IEnumerable<ChargingTariff> Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ChargingTariffs.GetEnumerator();
        }

        public IEnumerator<ChargingTariff> GetEnumerator()
        {
            return ChargingTariffs.GetEnumerator();
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariffGroup1, ChargingTariffGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup1">A charging station group.</param>
        /// <param name="ChargingTariffGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariffGroup ChargingTariffGroup1, ChargingTariffGroup ChargingTariffGroup2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingTariffGroup1, ChargingTariffGroup2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingTariffGroup1 == null) || ((Object) ChargingTariffGroup2 == null))
                return false;

            return ChargingTariffGroup1.Equals(ChargingTariffGroup2);

        }

        #endregion

        #region Operator != (ChargingTariffGroup1, ChargingTariffGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup1">A charging station group.</param>
        /// <param name="ChargingTariffGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingTariffGroup ChargingTariffGroup1, ChargingTariffGroup ChargingTariffGroup2)
            => !(ChargingTariffGroup1 == ChargingTariffGroup2);

        #endregion

        #region Operator <  (ChargingTariffGroup1, ChargingTariffGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup1">A charging station group.</param>
        /// <param name="ChargingTariffGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariffGroup ChargingTariffGroup1, ChargingTariffGroup ChargingTariffGroup2)
        {

            if ((Object) ChargingTariffGroup1 == null)
                throw new ArgumentNullException(nameof(ChargingTariffGroup1), "The given ChargingTariffGroup1 must not be null!");

            return ChargingTariffGroup1.CompareTo(ChargingTariffGroup2) < 0;

        }

        #endregion

        #region Operator <= (ChargingTariffGroup1, ChargingTariffGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup1">A charging station group.</param>
        /// <param name="ChargingTariffGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingTariffGroup ChargingTariffGroup1, ChargingTariffGroup ChargingTariffGroup2)
            => !(ChargingTariffGroup1 > ChargingTariffGroup2);

        #endregion

        #region Operator >  (ChargingTariffGroup1, ChargingTariffGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup1">A charging station group.</param>
        /// <param name="ChargingTariffGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariffGroup ChargingTariffGroup1, ChargingTariffGroup ChargingTariffGroup2)
        {

            if ((Object) ChargingTariffGroup1 == null)
                throw new ArgumentNullException(nameof(ChargingTariffGroup1), "The given ChargingTariffGroup1 must not be null!");

            return ChargingTariffGroup1.CompareTo(ChargingTariffGroup2) > 0;

        }

        #endregion

        #region Operator >= (ChargingTariffGroup1, ChargingTariffGroup2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup1">A charging station group.</param>
        /// <param name="ChargingTariffGroup2">Another charging station group.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingTariffGroup ChargingTariffGroup1, ChargingTariffGroup ChargingTariffGroup2)
            => !(ChargingTariffGroup1 < ChargingTariffGroup2);

        #endregion

        #endregion

        #region IComparable<ChargingTariffGroup> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var ChargingTariffGroup = Object as ChargingTariffGroup;
            if ((Object) ChargingTariffGroup == null)
                throw new ArgumentException("The given object is not a charging pool!", nameof(Object));

            return CompareTo(ChargingTariffGroup);

        }

        #endregion

        #region CompareTo(ChargingTariffGroup)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroup">A charging station group object to compare with.</param>
        public Int32 CompareTo(ChargingTariffGroup ChargingTariffGroup)
        {

            if ((Object) ChargingTariffGroup == null)
                throw new ArgumentNullException(nameof(ChargingTariffGroup), "The given charging station group must not be null!");

            return Id.CompareTo(ChargingTariffGroup.Id);

        }

        #endregion

        #endregion

        #region IEquatable<ChargingTariffGroup> Members

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

            var ChargingTariffGroup = Object as ChargingTariffGroup;
            if ((Object) ChargingTariffGroup == null)
                return false;

            return Equals(ChargingTariffGroup);

        }

        #endregion

        #region Equals(ChargingTariffGroup)

        /// <summary>
        /// Compares two charging pools for equality.
        /// </summary>
        /// <param name="ChargingTariffGroup">A charging station group to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingTariffGroup ChargingTariffGroup)
        {

            if ((Object) ChargingTariffGroup == null)
                return false;

            return Id.Equals(ChargingTariffGroup.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

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
            => String.Concat(Id, ", ", Description.FirstText());

        #endregion

    }

}
