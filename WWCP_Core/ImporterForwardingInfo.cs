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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.WWCP;

#endregion

namespace org.GraphDefined.WWCP.Importer
{

    public class ImporterForwardingInfo
    {

        #region Data

        private Action<DateTime, ImporterForwardingInfo, RoamingNetwork_Id, RoamingNetwork_Id> _OnForwardingChangeCallback;

        public String         StationName;
        public String         StationServiceTag;
        public Address        StationAddress;
        public GeoCoordinate  StationGeoCoordinate;

        #endregion

        #region Properties

        #region StationId

        private readonly ChargingStation_Id _StationId;

        public ChargingStation_Id StationId
        {
            get
            {
                return _StationId;
            }
        }

        #endregion

        #region EVSEIds

        private readonly HashSet<EVSE_Id> _EVSEIds;

        //public IEnumerable<EVSE_Id> EVSEIds
        //{
        //    get
        //    {
        //        return _EVSEIds;
        //    }
        //}

        #endregion

        #region EVSEOperators

        private readonly IEnumerable<EVSEOperator>  _EVSEOperators;

        public IEnumerable<EVSEOperator> EVSEOperators
        {
            get
            {
                return _EVSEOperators;
            }
        }

        #endregion

        #region ForwardedToRoamingNetworkId

        public RoamingNetwork_Id ForwardedToRoamingNetworkId
        {
            get
            {
                return _ForwardedToEVSEOperator != null
                            ? _ForwardedToEVSEOperator.RoamingNetwork.Id
                            : Defaults.UnknownRoamingNetwork;
            }
        }

        #endregion

        #region ForwardedToEVSEOperator

        private EVSEOperator _ForwardedToEVSEOperator;

        public EVSEOperator ForwardedToEVSEOperator
        {

            get
            {
                return _ForwardedToEVSEOperator;
            }

            set
            {

                // Remove ChargingStation from old ChargingPool/EVSEOperator
                if (_ForwardedToEVSEOperator != null)
                {

                    ChargingStation _ChargingStationToMove = null;

                    // Do not fail if the charging station is not yet available/existing!
                    if (_ForwardedToEVSEOperator.TryGetChargingStationbyId(StationId, out _ChargingStationToMove))
                    {

                        _ChargingStationToMove.ChargingPool.RemoveChargingStation(StationId);

                        // Also remove empty charging pools
                        if (_ChargingStationToMove.ChargingPool.ChargingStations.Count() == 0)
                            _ChargingStationToMove.ChargingPool.EVSEOperator.RemoveChargingPool(_ChargingStationToMove.ChargingPool.Id);

                    }

                    // Add to new EVSE operator/charging pool will be done during the next import cycle!

                }

                var Old_ForwardedToEVSEOperator = _ForwardedToEVSEOperator;

                _ForwardedToEVSEOperator = value;

                this._OnForwardingChangeCallback(DateTime.Now,
                                                 this,
                                                 Old_ForwardedToEVSEOperator != null ? Old_ForwardedToEVSEOperator.RoamingNetwork.Id : null,
                                                 value                       != null ? value.RoamingNetwork.Id                       : null);

            }

        }

        #endregion

        #region ForwardedToEVSEOperatorId

        public EVSEOperator_Id ForwardedToEVSEOperatorId
        {
            get
            {

                return _ForwardedToEVSEOperator != null
                            ? _ForwardedToEVSEOperator.Id
                            : null;

            }
        }

        #endregion

        #region AdminStatus

        private Timestamped<ChargingStationAdminStatusType> _AdminStatus;

        public Timestamped<ChargingStationAdminStatusType> AdminStatus
        {

            get
            {
                return _AdminStatus;
            }

            set
            {
                _AdminStatus = value;
            }

        }

        #endregion

        #region Created

        private readonly DateTime _Created;

        public DateTime Created
        {
            get
            {
                return _Created;
            }
        }

        #endregion

        #region OutOfService

        private Boolean _OutOfService;

        public Boolean OutOfService
        {

            get
            {
                return _OutOfService;
            }

            set
            {
                _OutOfService = value;
            }

        }

        #endregion

        #region LastTimeSeen

        private DateTime _LastTimeSeen;

        public DateTime LastTimeSeen
        {
            get
            {
                return _LastTimeSeen;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public ImporterForwardingInfo(Action<DateTime, ImporterForwardingInfo, RoamingNetwork_Id, RoamingNetwork_Id> OnChangedCallback,
                                      IEnumerable<EVSEOperator>                     EVSEOperators,
                                      ChargingStation_Id                            StationId                 = null,
                                      String                                        StationName               = "",
                                      String                                        StationServiceTag         = "",
                                      Address                                       StationAddress            = null,
                                      GeoCoordinate                                 StationGeoCoordinate      = null,
                                      IEnumerable<EVSE_Id>                          EVSEIds                   = null,
                                      Timestamped<ChargingStationAdminStatusType>?  AdminStatus               = null,
                                      DateTime?                                     Created                   = null,
                                      Boolean                                       OutOfService              = false,
                                      EVSEOperator                                  ForwardedToEVSEOperator   = null)
        {

            this._OnForwardingChangeCallback  = OnChangedCallback;
            this._EVSEOperators               = EVSEOperators;
            this._EVSEIds                     = EVSEIds              != null ? new HashSet<EVSE_Id>(EVSEIds) : new HashSet<EVSE_Id>();
            this._StationId                   = StationId            != null ? StationId                     : ChargingStation_Id.Create(EVSEIds);
            this.StationName                  = StationName;
            this.StationServiceTag            = StationServiceTag;
            this.StationAddress               = StationAddress;
            this.StationGeoCoordinate         = StationGeoCoordinate != null ? StationGeoCoordinate          : new GeoCoordinate(new Latitude(0), new Longitude(0));
            this._AdminStatus                 = AdminStatus          != null ? AdminStatus.Value             : new Timestamped<ChargingStationAdminStatusType>(ChargingStationAdminStatusType.Operational);
            this._Created                     = Created              != null ? Created.Value                 : DateTime.Now;
            this._OutOfService                = OutOfService;
            this._LastTimeSeen                = _Created;
            this._ForwardedToEVSEOperator     = ForwardedToEVSEOperator;

        }

        #endregion


        #region AddEVSEId(EVSEId)

        public void AddEVSEId(EVSE_Id EVSEId)
        {
            _EVSEIds.Add(EVSEId);
        }

        #endregion

        #region UpdateTimestamp()

        public void UpdateTimestamp()
        {
            this._LastTimeSeen  = DateTime.Now;
            this._OutOfService  = false;
        }

        #endregion


        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return StationId.ToString();
        }

        #endregion


    }

}
