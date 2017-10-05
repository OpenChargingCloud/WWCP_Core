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

    public class EVSEStatusPull
    {

        public IEnumerable<EVSEStatus>  EVSEStatus   { get; }
        public IEnumerable<String>      Warnings     { get; }

        public EVSEStatusPull(IEnumerable<EVSEStatus>  EVSEStatus,
                              IEnumerable<String>      Warnings = null)
        {

            this.EVSEStatus  = EVSEStatus ?? new EVSEStatus[0];
            this.Warnings    = Warnings != null
                                   ? Warnings.Where     (warning => warning != null).
                                              SafeSelect(warning => warning.Trim()).
                                              Where     (warning => warning.IsNotNullOrEmpty())
                                   : new String[0];

        }

    }

    /// <summary>
    /// The EV Roaming Provider provided EVSE Operator services interface.
    /// </summary>
    public interface IPullStatus
    {

        Task<EVSEStatusPull>

            PullEVSEStatus(DateTime?              LastCall            = null,
                           GeoCoordinate?         SearchCenter        = null,
                           Single                 DistanceKM          = 0f,
                           EVSEStatusTypes?       EVSEStatusFilter    = null,
                           eMobilityProvider_Id?  ProviderId          = null,

                           DateTime?              Timestamp           = null,
                           CancellationToken?     CancellationToken   = null,
                           EventTracking_Id       EventTrackingId     = null,
                           TimeSpan?              RequestTimeout      = null);

    }


}
