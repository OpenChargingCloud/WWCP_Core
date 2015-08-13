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
using System.Collections.Concurrent;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
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
    public class NavigationServiceProvider : AEMobilityEntity<NavigationServiceProvider_Id>,
                                             IEquatable<NavigationServiceProvider>, IComparable<NavigationServiceProvider>, IComparable,
                                             IEnumerable<ChargingPool>
    {

        #region Data

        private readonly ConcurrentDictionary<ChargingPool_Id, ChargingPool>  _RegisteredChargingPools;

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
                return _RoamingNetwork;
            }
        }

        #endregion

        #region Name

        private I18NString _Name;

        /// <summary>
        /// The offical (multi-language) name of the Navigation Service Provider.
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
                SetProperty<I18NString>(ref _Name, value);
            }

        }

        #endregion

        #region Description

        private I18NString _Description;

        /// <summary>
        /// An optional additional (multi-language) description of the Navigation Service Provider.
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
                SetProperty<I18NString>(ref _Description, value);
            }

        }

        #endregion


        #region ChargingPools

        public IEnumerable<ChargingPool> ChargingPools
        {
            get
            {
                return _RegisteredChargingPools.Select(KVP => KVP.Value);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (internal) NavigationServiceProvider()

        /// <summary>
        /// Create a new Navigation Service Provider (NSP).
        /// </summary>
        internal NavigationServiceProvider(RoamingNetwork  RoamingNetwork)
            : this(NavigationServiceProvider_Id.New, RoamingNetwork)
        { }

        #endregion

        #region (internal) NavigationServiceProvider(Id)

        /// <summary>
        /// Create a new Navigation Service Provider (NSP)
        /// having the given NSP_Id.
        /// </summary>
        /// <param name="Id">The Navigation Service Provider Identification.</param>
        /// <param name="RoamingNetwork">The associated roaming network.</param>
        internal NavigationServiceProvider(NavigationServiceProvider_Id  Id,
                                           RoamingNetwork                RoamingNetwork)

            : base(Id)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The unique identification of the navigation service provider must not be null!");

            if (RoamingNetwork == null)
                throw new ArgumentNullException("RoamingNetwork", "The roaming network must not be null!");

            #endregion

            #region Init data and properties

            this._RoamingNetwork            = RoamingNetwork;

            this._Name                      = new I18NString();
            this._Description               = new I18NString();

            this._RegisteredChargingPools   = new ConcurrentDictionary<ChargingPool_Id, ChargingPool>();

            #endregion

        }

        #endregion

        #endregion


        #region IEnumerable<ChargingPool> Members

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
