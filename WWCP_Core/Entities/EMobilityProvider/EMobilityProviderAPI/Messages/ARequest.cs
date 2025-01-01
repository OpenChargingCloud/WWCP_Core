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

using System.Net.Security;
using System.Security.Authentication;
using System.Collections.Concurrent;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

#endregion

namespace cloud.charging.open.protocols.WWCP.MobilityProvider
{

    /// <summary>
    /// An abstract generic request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public abstract class ARequest<TRequest> : IRequest,
                                               IEquatable<TRequest>

        where TRequest : class, IRequest

    {

        #region Properties

        /// <summary>
        /// The optional timestamp of the request.
        /// </summary>
        public DateTime                  Timestamp                  { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        public EventTracking_Id          EventTrackingId            { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        public TimeSpan?                 RequestTimeout             { get; set; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?                  CustomData                 { get; set; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken         CancellationToken          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic request message.
        /// </summary>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public ARequest(JObject?           CustomData          = null,
                        DateTime?          Timestamp           = null,
                        EventTracking_Id?  EventTrackingId     = null,
                        TimeSpan?          RequestTimeout      = null,
                        CancellationToken  CancellationToken   = default)
        {

            this.CustomData         = CustomData;
            this.Timestamp          = Timestamp       ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.EventTrackingId    = EventTrackingId ?? EventTracking_Id.New;
            this.RequestTimeout     = RequestTimeout;
            this.CancellationToken  = CancellationToken;

        }

        #endregion


        #region IEquatable<ARequest> Members

        /// <summary>
        /// Compare two abstract generic requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract generic request.</param>
        public abstract Boolean Equals(TRequest? ARequest);

        #endregion


    }

}
