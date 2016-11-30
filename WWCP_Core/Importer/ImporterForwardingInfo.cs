/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.Importer
{

    public class ImporterForwardingInfo
    {

        #region Data

        private Action<DateTime, ImporterForwardingInfo, RoamingNetwork_Id?, RoamingNetwork_Id?> _OnForwardingChanged;

        #endregion

        #region Properties

        public ChargingStation_Id   StationId               { get; }

        public String               StationName             { get; set; }

        public String               StationServiceTag       { get; set; }

        public Address              StationAddress          { get; set; }

        public GeoCoordinate        StationGeoCoordinate    { get; set; }

        public String               PhoneNumber             { get; set; }

        #region EVSEIds

        public IEnumerable<EVSE_Id> EVSEIds
            => _EVSEIds;

        private readonly HashSet<EVSE_Id> _EVSEIds;

        #endregion

        public IEnumerable<ChargingStationOperator> EVSEOperators { get; }

        #region ForwardedToRoamingNetworkId

        public RoamingNetwork_Id ForwardedToRoamingNetworkId

            => _ForwardedToEVSEOperator != null
                ? _ForwardedToEVSEOperator.RoamingNetwork.Id
                : Defaults.UnknownRoamingNetwork;

        #endregion

        #region ForwardedToEVSEOperator

        private ChargingStationOperator _ForwardedToEVSEOperator;

        public ChargingStationOperator ForwardedToEVSEOperator
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
                        if (!_ChargingStationToMove.ChargingPool.ChargingStations.Any())
                            _ChargingStationToMove.ChargingPool.Operator.RemoveChargingPool(_ChargingStationToMove.ChargingPool.Id);

                    }

                    // Add to new Charging Station Operator/charging pool will be done during the next import cycle!

                }

                var Old_ForwardedToEVSEOperator = _ForwardedToEVSEOperator;

                _ForwardedToEVSEOperator = value;

                this._OnForwardingChanged(DateTime.Now,
                                          this,
                                          Old_ForwardedToEVSEOperator != null ? Old_ForwardedToEVSEOperator.RoamingNetwork.Id : new RoamingNetwork_Id?(),
                                          value                       != null ? value.RoamingNetwork.Id                       : new RoamingNetwork_Id?());

            }

        }

        #endregion

        #region ForwardedToEVSEOperatorId

        public ChargingStationOperator_Id? ForwardedToEVSEOperatorId

            => _ForwardedToEVSEOperator != null
                ? _ForwardedToEVSEOperator.Id
                : new ChargingStationOperator_Id?();

        #endregion


        #region AdminStatus

        private Timestamped<ChargingStationAdminStatusTypes> _AdminStatus;

        public Timestamped<ChargingStationAdminStatusTypes> AdminStatus
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

        public DateTime Created { get; }

        public Boolean OutOfService { get; set; }

        public DateTime LastTimeSeen { private set; get; }

        #endregion

        #region Constructor(s)

        public ImporterForwardingInfo(Action<DateTime, ImporterForwardingInfo, RoamingNetwork_Id?, RoamingNetwork_Id?>  OnChangedCallback,
                                      IEnumerable<ChargingStationOperator>                                              EVSEOperators,
                                      ChargingStation_Id                                                                StationId                 = null,
                                      String                                                                            StationName               = "",
                                      String                                                                            StationServiceTag         = "",
                                      Address                                                                           StationAddress            = null,
                                      GeoCoordinate                                                                     StationGeoCoordinate      = null,
                                      IEnumerable<EVSE_Id>                                                              EVSEIds                   = null,
                                      String                                                                            PhoneNumber               = null,
                                      Timestamped<ChargingStationAdminStatusTypes>?                                      AdminStatus               = null,
                                      DateTime?                                                                         Created                   = null,
                                      Boolean                                                                           OutOfService              = false,
                                      ChargingStationOperator                                                           ForwardedToEVSEOperator   = null)
        {

            this._OnForwardingChanged        = OnChangedCallback;
            this.EVSEOperators               = EVSEOperators;
            this._EVSEIds                    = EVSEIds              != null ? new HashSet<EVSE_Id>(EVSEIds) : new HashSet<EVSE_Id>();
            this.StationId                   = StationId            != null ? StationId                     : ChargingStation_Id.Create(EVSEIds);
            this.StationName                 = StationName;
            this.StationServiceTag           = StationServiceTag;
            this.StationAddress              = StationAddress;
            this.StationGeoCoordinate        = StationGeoCoordinate != null ? StationGeoCoordinate          : new GeoCoordinate(new Latitude(0), new Longitude(0));
            this._AdminStatus                = AdminStatus          != null ? AdminStatus.Value             : new Timestamped<ChargingStationAdminStatusTypes>(ChargingStationAdminStatusTypes.Operational);
            this.PhoneNumber                 = PhoneNumber;
            this.Created                     = Created              != null ? Created.Value                 : DateTime.Now;
            this.OutOfService                = OutOfService;
            this.LastTimeSeen                = this.Created;
            this._ForwardedToEVSEOperator    = ForwardedToEVSEOperator;

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
            this.LastTimeSeen  = DateTime.Now;
            this.OutOfService  = false;
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => StationId.ToString() + " => " + ForwardedToRoamingNetworkId;

        #endregion


    }

}
