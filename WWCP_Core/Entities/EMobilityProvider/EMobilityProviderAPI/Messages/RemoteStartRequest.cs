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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.MobilityProvider
{

    /// <summary>
    /// The RemoteStart request.
    /// </summary>
    public class RemoteStartRequest : ARequest<RemoteStartRequest>
    {

        #region Properties

        /// <summary>
        /// The charging location.
        /// </summary>
        [Mandatory]
        public ChargingLocation         ChargingLocation          { get; }

        /// <summary>
        /// The unique identification of the e-mobility account.
        /// </summary>
        [Mandatory]
        public RemoteAuthentication     RemoteAuthentication      { get; }


        [Optional]
        public JObject?                 AdditionalSessionInfos    { get; }


        [Optional]
        public Auth_Path?               AuthenticationPath        { get; }

        /// <summary>
        /// The optional unique identification of a charging reservation.
        /// </summary>
        [Optional]
        public ChargingReservation_Id?  ReservationId             { get; }

        /// <summary>
        /// The optional charging product.
        /// </summary>
        [Optional]
        public ChargingProduct?         ChargingProduct           { get; }

        /// <summary>
        /// The optional unique identification for this charging session.
        /// </summary>
        [Optional]
        public ChargingSession_Id?      ChargingSessionId         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RemoteStart request.
        /// </summary>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// <param name="ReservationId">An optional unique identification of a charging reservation.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="ChargingSessionId">An optional unique identification for this charging session.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public RemoteStartRequest(ChargingLocation         ChargingLocation,
                                  RemoteAuthentication     RemoteAuthentication,
                                  JObject?                 AdditionalSessionInfos   = null,
                                  Auth_Path?               AuthenticationPath       = null,
                                  ChargingReservation_Id?  ReservationId            = null,
                                  ChargingProduct?         ChargingProduct          = null,
                                  ChargingSession_Id?      ChargingSessionId        = null,
                                  JObject?                 CustomData               = null,

                                  DateTime?                Timestamp                = null,
                                  EventTracking_Id?        EventTrackingId          = null,
                                  TimeSpan?                RequestTimeout           = null,
                                  CancellationToken        CancellationToken        = default)

            : base(CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ChargingLocation        = ChargingLocation;
            this.RemoteAuthentication    = RemoteAuthentication;
            this.AdditionalSessionInfos  = AdditionalSessionInfos;
            this.AuthenticationPath      = AuthenticationPath;
            this.ReservationId           = ReservationId;
            this.ChargingProduct         = ChargingProduct;
            this.ChargingSessionId       = ChargingSessionId;

            unchecked
            {

                hashCode = this.ChargingLocation.       GetHashCode()       * 17 ^
                           this.RemoteAuthentication.   GetHashCode()       * 13 ^
                          (this.AdditionalSessionInfos?.GetHashCode() ?? 0) * 11 ^
                          (this.AuthenticationPath?.    GetHashCode() ?? 0) *  7 ^
                          (this.ReservationId?.         GetHashCode() ?? 0) *  5 ^
                          (this.ChargingProduct?.       GetHashCode() ?? 0) *  3 ^
                          (this.ChargingSessionId?.     GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Parse   (JSON, ..., CustomRemoteStartRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a RemoteStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomRemoteStartRequestParser">An optional delegate to parse custom RemoteStart JSON objects.</param>
        public static RemoteStartRequest Parse(JObject                                           JSON,

                                               DateTime?                                         Timestamp                        = null,
                                               EventTracking_Id?                                 EventTrackingId                  = null,
                                               TimeSpan?                                         RequestTimeout                   = null,
                                               CancellationToken                                 CancellationToken                = default,

                                               CustomJObjectParserDelegate<RemoteStartRequest>?  CustomRemoteStartRequestParser   = null)
        {

            if (TryParse(JSON,
                         out var remoteStartRequest,
                         out var errorResponse,
                         CustomRemoteStartRequestParser,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CancellationToken))
            {
                return remoteStartRequest;
            }

            throw new ArgumentException("The given JSON representation of a RemoteStart request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RemoteStartRequest, out ErrorResponse, ..., CustomRemoteStartRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a RemoteStart request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="RemoteStartRequest">The parsed RemoteStart request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomRemoteStartRequestParser">An optional delegate to parse custom RemoteStart request JSON objects.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       [NotNullWhen(true)]  out RemoteStartRequest?      RemoteStartRequest,
                                       [NotNullWhen(false)] out String?                  ErrorResponse,
                                       CustomJObjectParserDelegate<RemoteStartRequest>?  CustomRemoteStartRequestParser   = null,

                                       DateTime?                                         Timestamp                        = null,
                                       EventTracking_Id?                                 EventTrackingId                  = null,
                                       TimeSpan?                                         RequestTimeout                   = null,
                                       CancellationToken                                 CancellationToken                = default)
        {

            try
            {

                RemoteStartRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ChargingLocation          [mandatory]

                if (!JSON.ParseMandatoryJSON("chargingLocation",
                                             "charging location",
                                             WWCP.ChargingLocation.TryParse,
                                             out ChargingLocation? ChargingLocation,
                                             out ErrorResponse) ||
                     ChargingLocation is null)
                {
                    return false;
                }

                #endregion

                #region Parse RemoteAuthentication      [mandatory]

                if (!JSON.ParseMandatoryJSON("remoteAuthentication",
                                             "remote authentication",
                                             WWCP.RemoteAuthentication.TryParse,
                                             out RemoteAuthentication? RemoteAuthentication,
                                             out ErrorResponse) ||
                     RemoteAuthentication is null)
                {
                    return false;
                }

                #endregion

                #region Parse AdditionalSessionInfos    [optional]

                var AdditionalSessionInfos = JSON["additionalSessionInfos"] as JObject;

                #endregion

                #region Parse AuthenticationPath        [optional]

                if (JSON.ParseOptional("authenticationPath",
                                       "authentication path",
                                       Auth_Path.TryParse,
                                       out Auth_Path? AuthenticationPath,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ReservationId             [optional]

                if (JSON.ParseOptional("chargingReservationId",
                                       "charging reservation identification",
                                       ChargingReservation_Id.TryParse,
                                       out ChargingReservation_Id? ReservationId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingProduct           [optional]

                if (JSON.ParseOptionalJSON("chargingProduct",
                                           "charging product",
                                           WWCP.ChargingProduct.TryParse,
                                           out ChargingProduct? ChargingProduct,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingSessionId         [optional]

                if (JSON.ParseOptional("chargingSessionId",
                                       "charging session identification",
                                       ChargingSession_Id.TryParse,
                                       out ChargingSession_Id? ChargingSessionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData                [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                RemoteStartRequest = new RemoteStartRequest(
                                         ChargingLocation,
                                         RemoteAuthentication,
                                         AdditionalSessionInfos,
                                         AuthenticationPath,
                                         ReservationId,
                                         ChargingProduct,
                                         ChargingSessionId,
                                         customData,

                                         Timestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     );

                if (CustomRemoteStartRequestParser is not null)
                    RemoteStartRequest = CustomRemoteStartRequestParser(JSON,
                                                                        RemoteStartRequest);

                return true;

            }
            catch (Exception e)
            {
                RemoteStartRequest  = default;
                ErrorResponse       = "The given JSON representation of a RemoteStart request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRemoteStartRequestSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomRemoteStartRequestSerializer">A delegate to customize the serialization of RemoteStartRequest responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RemoteStartRequest>?  CustomRemoteStartRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("chargingLocation",         ChargingLocation.        ToJSON()),
                                 new JProperty("remoteAuthentication",     RemoteAuthentication.    ToJSON()),

                           AdditionalSessionInfos is not null
                               ? new JProperty("additionalSessionInfos",   AdditionalSessionInfos)
                               : null,

                           AuthenticationPath.HasValue
                               ? new JProperty("authenticationPath",       AuthenticationPath.Value.ToString())
                               : null,

                           ReservationId.HasValue
                               ? new JProperty("chargingReservationId",    ReservationId.     Value.ToString())
                               : null,

                           ChargingProduct is not null
                               ? new JProperty("chargingProduct",          ChargingProduct.         ToJSON())
                               : null,

                           ChargingSessionId.HasValue
                               ? new JProperty("chargingSessionId",        ChargingSessionId. Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",               CustomData)
                               : null

                       );

            return CustomRemoteStartRequestSerializer is not null
                       ? CustomRemoteStartRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RemoteStartRequest1, RemoteStartRequest2)

        /// <summary>
        /// Compares two remote start requests for equality.
        /// </summary>
        /// <param name="RemoteStartRequest1">A remote start request.</param>
        /// <param name="RemoteStartRequest2">Another remote start request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RemoteStartRequest RemoteStartRequest1,
                                           RemoteStartRequest RemoteStartRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RemoteStartRequest1, RemoteStartRequest2))
                return true;

            // If one is null, but not both, return false.
            if (RemoteStartRequest1 is null || RemoteStartRequest2 is null)
                return false;

            return RemoteStartRequest1.Equals(RemoteStartRequest2);

        }

        #endregion

        #region Operator != (RemoteStartRequest1, RemoteStartRequest2)

        /// <summary>
        /// Compares two remote start requests for inequality.
        /// </summary>
        /// <param name="RemoteStartRequest1">A remote start request.</param>
        /// <param name="RemoteStartRequest2">Another remote start request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RemoteStartRequest RemoteStartRequest1,
                                           RemoteStartRequest RemoteStartRequest2)

            => !(RemoteStartRequest1 == RemoteStartRequest2);

        #endregion

        #endregion

        #region IEquatable<RemoteStartRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two remote start requests for equality.
        /// </summary>
        /// <param name="Object">A remote start request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RemoteStartRequest remoteStartRequest &&
                   Equals(remoteStartRequest);

        #endregion

        #region Equals(RemoteStartRequest)

        /// <summary>
        /// Compares two remote start requests for equality.
        /// </summary>
        /// <param name="RemoteStartRequest">A remote start request to compare with.</param>
        public override Boolean Equals(RemoteStartRequest? RemoteStartRequest)

            => RemoteStartRequest is not null &&

               ChargingLocation.    Equals(RemoteStartRequest.ChargingLocation)     &&
               RemoteAuthentication.Equals(RemoteStartRequest.RemoteAuthentication) &&

            ((!AuthenticationPath. HasValue && !RemoteStartRequest.AuthenticationPath. HasValue) ||
              (AuthenticationPath. HasValue &&  RemoteStartRequest.AuthenticationPath. HasValue && AuthenticationPath. Value.Equals(RemoteStartRequest.AuthenticationPath. Value))) &&

            ((!ReservationId.      HasValue && !RemoteStartRequest.ReservationId.      HasValue) ||
              (ReservationId.      HasValue &&  RemoteStartRequest.ReservationId.      HasValue && ReservationId.      Value.Equals(RemoteStartRequest.ReservationId.      Value))) &&

             ((ChargingProduct is null     &&   RemoteStartRequest.ChargingProduct is null)      ||
              (ChargingProduct is not null &&   RemoteStartRequest.ChargingProduct is not null  && ChargingProduct.          Equals(RemoteStartRequest.ChargingProduct)))           &&

            ((!ChargingSessionId.  HasValue && !RemoteStartRequest.ChargingSessionId.  HasValue) ||
              (ChargingSessionId.  HasValue &&  RemoteStartRequest.ChargingSessionId.  HasValue && ChargingSessionId.  Value.Equals(RemoteStartRequest.ChargingSessionId.  Value)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{RemoteAuthentication} at {ChargingLocation}";

        #endregion

    }

}
