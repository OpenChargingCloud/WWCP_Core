/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// An abstract generic response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public abstract class AResponse<TRequest, TResponse> : IResponse,
                                                           IEquatable<AResponse<TRequest, TResponse>>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse

    {

        #region Properties

        /// <summary>
        /// The request leading to this response.
        /// Might be null, when the request was not parsable!
        /// </summary>
        [Optional]
        public TRequest?             Request              { get; }

        /// <summary>
        /// The timestamp of the response creation.
        /// </summary>
        [Mandatory]
        public DateTime              ResponseTimestamp    { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this response with other events.
        /// </summary>
        [Mandatory]
        public EventTracking_Id      EventTrackingId      { get; }

        /// <summary>
        /// Optional warnings.
        /// </summary>
        [Optional]
        public IEnumerable<Warning>  Warnings             { get; }

        /// <summary>
        /// The runtime of the request/response.
        /// </summary>
        [Mandatory]
        public TimeSpan              Runtime              { get; }

        /// <summary>
        /// The HTTP response.
        /// </summary>
        [Optional]
        public HTTPResponse?         HTTPResponse         { get; }

        /// <summary>
        /// Optional custom data, e.g. in combination with custom parsers and serializers.
        /// </summary>
        [Optional]
        public JObject?              CustomData           { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic response.
        /// </summary>
        /// <param name="Request">The request leading to this result. Might be null, when the request e.g. was not parsable!</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// 
        /// <param name="Warnings">Optional warnings.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        protected AResponse(TRequest?              Request,
                            EventTracking_Id       EventTrackingId,
                            DateTime?              ResponseTimestamp,
                            TimeSpan?              Runtime,

                            IEnumerable<Warning>?  Warnings       = null,
                            JObject?               CustomData     = null,
                            HTTPResponse?          HTTPResponse   = null)
        {

            this.Request            = Request;
            this.ResponseTimestamp  = ResponseTimestamp ?? Timestamp.Now;
            this.EventTrackingId    = EventTrackingId;
            this.Runtime            = Runtime           ?? (this.ResponseTimestamp - (Request?.Timestamp ?? this.ResponseTimestamp));

            this.HTTPResponse       = HTTPResponse;
            this.Warnings           = Warnings          ?? Array.Empty<Warning>();
            this.CustomData         = CustomData;

        }

        #endregion


        #region IEquatable<AResponse<TRequest, TResponse>> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two abstract responses for equality.
        /// </summary>
        /// <param name="Object">An abstract response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AResponse<TRequest, TResponse> aResponse &&
                   Equals(aResponse);

        #endregion

        #region Equals(AResponse)

        /// <summary>
        /// Compare two abstract responses for equality.
        /// </summary>
        /// <param name="AResponse">Another abstract response.</param>
        public Boolean Equals(AResponse<TRequest, TResponse>? AResponse)

            => AResponse is not null &&

               ResponseTimestamp.Equals(AResponse.ResponseTimestamp) &&
               EventTrackingId.  Equals(AResponse.EventTrackingId)   &&
               Runtime.          Equals(AResponse.Runtime)           &&

             ((Request      is     null &&  AResponse.Request      is     null) ||
              (Request      is not null &&  AResponse.Request      is not null && Request.     Equals(AResponse.Request)))      &&

             ((HTTPResponse is     null &&  AResponse.HTTPResponse is     null) ||
              (HTTPResponse is not null &&  AResponse.HTTPResponse is not null && HTTPResponse.Equals(AResponse.HTTPResponse))) &&

             ((CustomData   is     null &&  AResponse.CustomData   is     null) ||
              (CustomData   is not null &&  AResponse.CustomData   is not null && CustomData.  Equals(AResponse.CustomData)))   &&

             (!Warnings.Any()           && !AResponse.Warnings.Any()               ||
               Warnings.Any()           &&  AResponse.Warnings.Any()              && Warnings.Count().Equals(AResponse.Warnings.Count()));

        #endregion

        #endregion


        #region ToJSON()

        ///// <summary>
        ///// Compare two abstract responses for equality.
        ///// </summary>
        //public abstract JObject ToJSON();

        #endregion


        #region (class) Builder

        /// <summary>
        /// An abstract generic response builder.
        /// </summary>
        public abstract class Builder
        {

            #region Properties

            /// <summary>
            /// The request leading to this response.
            /// </summary>
            [Mandatory]
            public TRequest?          Request              { get; set; }

            /// <summary>
            /// The timestamp of the response message creation.
            /// </summary>
            [Mandatory]
            public DateTime?          ResponseTimestamp    { get; set; }

            /// <summary>
            /// An optional event tracking identification for correlating this response with other events.
            /// </summary>
            public EventTracking_Id?  EventTrackingId      { get; set; }

            /// <summary>
            /// Optional warnings.
            /// </summary>
            [Optional]
            public HashSet<Warning>   Warnings             { get; }

            /// <summary>
            /// The runtime of the request/response.
            /// </summary>
            public TimeSpan?          Runtime              { get; set; }


            /// <summary>
            /// The HTTP response.
            /// </summary>
            [Optional]
            public HTTPResponse?      HTTPResponse         { get; set; }

            /// <summary>
            /// Optional custom data, e.g. in combination with custom parsers and serializers.
            /// </summary>
            [Optional]
            public JObject?           CustomData           { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new generic response.
            /// </summary>
            /// <param name="Request">The request leading to this result.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="Warnings">Optional warnings.</param>
            /// <param name="CustomData">Optional customer-specific data of the response.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            protected Builder(TRequest?              Request             = null,
                              EventTracking_Id?      EventTrackingId     = null,
                              DateTime?              ResponseTimestamp   = null,
                              TimeSpan?              Runtime             = null,
                              IEnumerable<Warning>?  Warnings            = null,
                              JObject?               CustomData          = null,
                              HTTPResponse?          HTTPResponse        = null)
            {

                this.Request            = Request;
                this.EventTrackingId    = EventTrackingId;
                this.ResponseTimestamp  = ResponseTimestamp;
                this.Runtime            = Runtime;
                this.Warnings           = Warnings is not null
                                              ? new HashSet<Warning>(Warnings)
                                              : new HashSet<Warning>();
                this.CustomData         = CustomData;
                this.HTTPResponse       = HTTPResponse;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable response.
            /// </summary>
            public abstract TResponse  ToImmutable();

            #endregion

        }

        #endregion

    }

}
