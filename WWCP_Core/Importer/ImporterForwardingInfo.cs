/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.Importer
{

    public class ImporterForwardingInfo
    {

        #region Data

        private Action<DateTimeOffset, ImporterForwardingInfo, RoamingNetwork_Id?, RoamingNetwork_Id?> _OnForwardingChanged;

        #endregion

        #region Properties

        public ChargingStation_Id   StationId               { get; }

        public String               StationName             { get; set; }

        public String               StationServiceTag       { get; set; }

        public Address              StationAddress          { get; set; }

        public GeoCoordinate?       StationGeoCoordinate    { get; set; }

        public String               PhoneNumber             { get; set; }

        #region EVSEIds

        public IEnumerable<EVSE_Id> EVSEIds
            => _EVSEIds;

        private readonly HashSet<EVSE_Id> _EVSEIds;

        #endregion

        public IEnumerable<ChargingStationOperator> ChargingStationOperators { get; }

        #region ForwardedToRoamingNetworkId

        public RoamingNetwork_Id ForwardedToRoamingNetworkId

            => _ForwardedToChargingStationOperator is not null
                ? _ForwardedToChargingStationOperator.RoamingNetwork.Id
                : Defaults.UnknownRoamingNetwork;

        #endregion

        #region ForwardedToChargingStationOperator

        private ChargingStationOperator _ForwardedToChargingStationOperator;

        public ChargingStationOperator ForwardedToChargingStationOperator
        {

            get
            {
                return _ForwardedToChargingStationOperator;
            }

            set
            {

                // Remove ChargingStation from old ChargingPool/EVSEOperator
                if (_ForwardedToChargingStationOperator is not null)
                {

                    // Do not fail if the charging station is not yet available/existing!
                    if (_ForwardedToChargingStationOperator.TryGetChargingStationById(StationId, out var chargingStationToMove))
                    {

                        chargingStationToMove.ChargingPool.RemoveChargingStation(StationId,
                                                                                 EventTrackingId:  EventTracking_Id.New,
                                                                                 CurrentUserId:    null);

                        // Also remove empty charging pools
                        if (!chargingStationToMove.ChargingPool.ChargingStations.Any())
                            chargingStationToMove.ChargingPool.Operator.RemoveChargingPool(chargingStationToMove.ChargingPool.Id);

                    }

                    // Add to new Charging Station Operator/charging pool will be done during the next import cycle!

                }

                var Old_ForwardedToChargingStationOperator = _ForwardedToChargingStationOperator;

                _ForwardedToChargingStationOperator = value;

                this._OnForwardingChanged(Timestamp.Now,
                                          this,
                                          Old_ForwardedToChargingStationOperator is not null ? Old_ForwardedToChargingStationOperator.RoamingNetwork.Id : new RoamingNetwork_Id?(),
                                          value                                  is not null ? value.RoamingNetwork.Id                                  : new RoamingNetwork_Id?());

            }

        }

        #endregion

        #region ForwardedToChargingStationOperatorId

        public ChargingStationOperator_Id? ForwardedToChargingStationOperatorId

            => _ForwardedToChargingStationOperator is not null
                ? _ForwardedToChargingStationOperator.Id
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

        public DateTimeOffset Created { get; }

        public Boolean OutOfService { get; set; }

        public DateTimeOffset LastTimeSeen { private set; get; }

        #endregion

        #region Constructor(s)

        public ImporterForwardingInfo(Action<DateTimeOffset, ImporterForwardingInfo, RoamingNetwork_Id?, RoamingNetwork_Id?>  OnChangedCallback,
                                      IEnumerable<ChargingStationOperator>                                                    ChargingStationOperators,
                                      ChargingStation_Id?                                                                     StationId                 = null,
                                      String                                                                                  StationName               = "",
                                      String                                                                                  StationServiceTag         = "",
                                      Address?                                                                                StationAddress            = null,
                                      GeoCoordinate?                                                                          StationGeoCoordinate      = null,
                                      IEnumerable<EVSE_Id>?                                                                   EVSEIds                   = null,
                                      String?                                                                                 PhoneNumber               = null,
                                      Timestamped<ChargingStationAdminStatusTypes>?                                           AdminStatus               = null,
                                      DateTime?                                                                               Created                   = null,
                                      Boolean                                                                                 OutOfService              = false,
                                      ChargingStationOperator?                                                                ForwardedToOperator       = null)
        {

            this._OnForwardingChanged                 = OnChangedCallback;
            this.ChargingStationOperators             = ChargingStationOperators;
            this._EVSEIds                             = EVSEIds               is not null ? new HashSet<EVSE_Id>(EVSEIds) : new HashSet<EVSE_Id>();
            this.StationId                            = StationId.HasValue            ? StationId.Value               : ChargingStation_Id.TryCreate(EVSEIds).Value;
            this.StationName                          = StationName;
            this.StationServiceTag                    = StationServiceTag;
            this.StationAddress                       = StationAddress;
            this.StationGeoCoordinate                 = StationGeoCoordinate;
            this._AdminStatus                         = AdminStatus           is not null ? AdminStatus.Value             : new Timestamped<ChargingStationAdminStatusTypes>(ChargingStationAdminStatusTypes.Operational);
            this.PhoneNumber                          = PhoneNumber;
            this.Created                              = Created               is not null ? Created.Value                 : Timestamp.Now;
            this.OutOfService                         = OutOfService;
            this.LastTimeSeen                         = this.Created;
            this._ForwardedToChargingStationOperator  = ForwardedToOperator;

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
            this.LastTimeSeen  = Timestamp.Now;
            this.OutOfService  = false;
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(StationId.ToString(),
                             " => ",
                             ForwardedToRoamingNetworkId,// ?? "<none>",
                             " / ",
                             ForwardedToChargingStationOperator?.Id.ToString() ?? "<none>");

        #endregion


    }

}
