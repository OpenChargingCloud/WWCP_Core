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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.WWCP;

#endregion

namespace org.GraphDefined.WWCP.Importer
{

    public class ImporterForwardingInfo
    {

        #region Properties

        public           String                     StationName;
        public           String                     StationServiceTag;
        public           Address                    StationAddress;
        public           GeoCoordinate              StationGeoCoordinate;
        public  readonly ChargingStation_Id         StationId;
        public  readonly HashSet<EVSE_Id>           EVSEIds;

        private readonly IEnumerable<EVSEOperator>  EVSEOperators;

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

                if (value != null)
                {

                    //if (_ForwardedToRoamingNetwork != value)
                    //{

                    //    var OldEVSEOp = this.EVSEOperators.Where(EVSEOp => EVSEOp.RoamingNetwork.Id == _ForwardedToRoamingNetwork).FirstOrDefault();
                    //    if (OldEVSEOp != null)
                    //    {
                    //        foreach (var EVSEId in EVSEIds)
                    //            OldEVSEOp.ValidEVSEIds.Remove(EVSEId);
                    //    }

                    //}

                    _ForwardedToEVSEOperator = value;

                    //var NewEVSEOp = this.EVSEOperators.Where(EVSEOp => EVSEOp.RoamingNetwork.Id == _ForwardedToRoamingNetwork).FirstOrDefault();
                    //if (NewEVSEOp != null)
                    //{
                    //    foreach (var EVSEId in EVSEIds)
                    //        NewEVSEOp.ValidEVSEIds.Add(EVSEId);
                    //}

                }

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

        private Nullable<EVSEStatusType> _AdminStatus;

        public Nullable<EVSEStatusType> AdminStatus
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

        public ImporterForwardingInfo(IEnumerable<EVSEOperator>  EVSEOperators,
                                      IEnumerable<EVSE_Id>       EVSEIds,
                                      ChargingStation_Id         StationId                 = null,
                                      String                     StationName               = "",
                                      String                     StationServiceTag         = "",
                                      Address                    StationAddress            = null,
                                      GeoCoordinate              StationGeoCoordinate      = null,
                                      DateTime?                  Created                   = null,
                                      Boolean                    OutOfService              = false,
                                      EVSEOperator               ForwardedToEVSEOperator   = null)
        {

            this.EVSEOperators             = EVSEOperators;
            this.StationName               = StationName;
            this.StationServiceTag         = StationServiceTag;
            this.EVSEIds                   = new HashSet<EVSE_Id>(EVSEIds);
            this.StationAddress            = StationAddress;
            this.StationGeoCoordinate      = StationGeoCoordinate != null ? StationGeoCoordinate : new GeoCoordinate(new Latitude(0), new Longitude(0));
            this.StationId                 = StationId != null ? StationId     : ChargingStation_Id.Create(EVSEIds);
            this._Created                  = Created   != null ? Created.Value : DateTime.Now;
            this._OutOfService             = OutOfService;
            this._LastTimeSeen             = _Created;

            this._ForwardedToEVSEOperator  = ForwardedToEVSEOperator;

        }

        #endregion


        #region AddEVSEId(EVSEId)

        public void AddEVSEId(EVSE_Id EVSEId)
        {
            EVSEIds.Add(EVSEId);
        }

        #endregion

        #region UpdateTimestamp()

        public void UpdateTimestamp()
        {
            this._LastTimeSeen  = DateTime.Now;
            this._OutOfService  = false;
        }

        #endregion


    }

}
