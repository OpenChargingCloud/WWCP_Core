/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    public class EVSEDataPull
    {

        public IEnumerable<EVSE>                           EVSEs      { get; }
        public IEnumerable<KeyValuePair<EVSE_Id, String>>  Warnings   { get; }

        public EVSEDataPull(IEnumerable<EVSE>                           EVSEs,
                            IEnumerable<KeyValuePair<EVSE_Id, String>>  Warnings = null)
        {

            this.EVSEs     = EVSEs;
            this.Warnings  = Warnings;

        }

    }

    public interface IPullData
    {

        // Events

        #region OnEVSEDataPull/-Pulled

        ///// <summary>
        ///// An event fired whenever new EVSE data will be send upstream.
        ///// </summary>
        //event OnPushEVSEDataRequestDelegate OnPushEVSEDataRequest;

        ///// <summary>
        ///// An event fired whenever new EVSE data had been sent upstream.
        ///// </summary>
        //event OnPushEVSEDataResponseDelegate OnPushEVSEDataResponse;

        #endregion


        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        Boolean  DisablePullData   { get; set; }


        // Push data directly...

        #region SetStaticData   (EVSEs, ...)

        /// <summary>
        /// Upload the static data of the given EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<EVSEDataPull>

            PullEVSEData(DateTime?              LastCall            = null,
                         GeoCoordinate?         SearchCenter        = null,
                         Single                 DistanceKM          = 0f,
                         eMobilityProvider_Id?  ProviderId          = null,

                         DateTime?              Timestamp           = null,
                         CancellationToken?     CancellationToken   = null,
                         EventTracking_Id       EventTrackingId     = null,
                         TimeSpan?              RequestTimeout      = null);

        #endregion

    }

    public interface IReceivePullData
    {


    }

}
