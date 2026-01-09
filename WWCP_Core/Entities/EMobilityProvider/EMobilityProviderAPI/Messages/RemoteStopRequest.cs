/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.WWCP.MobilityProvider
{

    /// <summary>
    /// The RemoteStop request.
    /// </summary>
    public class RemoteStopRequest : ARequest<RemoteStopRequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of a charging session to stop.
        /// </summary>
        [Mandatory]
        public ChargingSession_Id       ChargingSessionId       { get; }

        /// <summary>
        /// The optional unique identification of a e-mobility account.
        /// </summary>
        [Optional]
        public RemoteAuthentication?    RemoteAuthentication    { get; }

        [Optional]
        public Auth_Path?               AuthenticationPath      { get; }

        /// <summary>
        /// The optional charging location.
        /// </summary>
        [Optional]
        public ChargingLocation?        ChargingLocation        { get; }

        /// <summary>
        /// The optional charging product.
        /// </summary>
        [Optional]
        public ChargingProduct?         ChargingProduct         { get; }

        /// <summary>
        /// The optional unique identification of a charging reservation.
        /// </summary>
        [Optional]
        public ChargingReservation_Id?  ReservationId           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RemoteStop request.
        /// </summary>
        /// <param name="ChargingSessionId">An unique identification of a charging session to stop.</param>
        /// <param name="RemoteAuthentication">The unique identification of the e-mobility account.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ReservationId">An optional unique identification of a charging reservation.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        public RemoteStopRequest(ChargingSession_Id       ChargingSessionId,
                                 RemoteAuthentication?    RemoteAuthentication   = null,
                                 Auth_Path?               AuthenticationPath     = null,
                                 ChargingLocation?        ChargingLocation       = null,
                                 ChargingProduct?         ChargingProduct        = null,
                                 ChargingReservation_Id?  ReservationId          = null,
                                 JObject?                 CustomData             = null,

                                 DateTime?                Timestamp              = null,
                                 EventTracking_Id?        EventTrackingId        = null,
                                 TimeSpan?                RequestTimeout         = null,
                                 CancellationToken        CancellationToken      = default)

            : base(CustomData,
                   Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.ChargingSessionId     = ChargingSessionId;
            this.RemoteAuthentication  = RemoteAuthentication;
            this.AuthenticationPath    = AuthenticationPath;
            this.ChargingLocation      = ChargingLocation;
            this.ChargingProduct       = ChargingProduct;
            this.ReservationId         = ReservationId;

            unchecked
            {

                hashCode = this.ChargingSessionId.    GetHashCode()       * 13 ^
                          (this.RemoteAuthentication?.GetHashCode() ?? 0) * 11 ^
                          (this.AuthenticationPath?.  GetHashCode() ?? 0) *  7 ^
                          (this.ChargingLocation?.    GetHashCode() ?? 0) *  5 ^
                          (this.ChargingProduct?.     GetHashCode() ?? 0) *  3 ^
                          (this.ReservationId?.       GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Parse   (JSON, ..., CustomRemoteStopRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a RemoteStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomRemoteStopRequestParser">An optional delegate to parse custom RemoteStop JSON objects.</param>
        public static RemoteStopRequest Parse(JObject                                           JSON,

                                               DateTime?                                        Timestamp                       = null,
                                               EventTracking_Id?                                EventTrackingId                 = null,
                                               TimeSpan?                                        RequestTimeout                  = null,
                                               CancellationToken                                CancellationToken               = default,

                                               CustomJObjectParserDelegate<RemoteStopRequest>?  CustomRemoteStopRequestParser   = null)
        {

            if (TryParse(JSON,
                         out var remoteStopRequest,
                         out var errorResponse,
                         Timestamp,
                         CancellationToken,
                         EventTrackingId,
                         RequestTimeout,
                         CustomRemoteStopRequestParser))
            {
                return remoteStopRequest!;
            }

            throw new ArgumentException("The given JSON representation of a RemoteStop request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RemoteStopRequest, out ErrorResponse, ..., CustomRemoteStopRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a RemoteStop request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="RemoteStopRequest">The parsed RemoteStop request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomRemoteStopRequestParser">An optional delegate to parse custom RemoteStop request JSON objects.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       out RemoteStopRequest?                           RemoteStopRequest,
                                       out String?                                      ErrorResponse,

                                       DateTime?                                        Timestamp                       = null,
                                       CancellationToken                                CancellationToken               = default,
                                       EventTracking_Id?                                EventTrackingId                 = null,
                                       TimeSpan?                                        RequestTimeout                  = null,

                                       CustomJObjectParserDelegate<RemoteStopRequest>?  CustomRemoteStopRequestParser   = null)
        {

            try
            {

                RemoteStopRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse ChargingSessionId       [mandatory]

                if (!JSON.ParseMandatory("chargingSessionId",
                                         "charging session identification",
                                         ChargingSession_Id.TryParse,
                                         out ChargingSession_Id ChargingSessionId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse RemoteAuthentication    [optional]

                if (JSON.ParseOptionalJSON("remoteAuthentication",
                                           "remote authentication",
                                           WWCP.RemoteAuthentication.TryParse,
                                           out RemoteAuthentication? RemoteAuthentication,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AuthenticationPath      [optional]

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

                #region Parse ChargingLocation        [optional]

                if (JSON.ParseOptionalJSON("chargingLocation",
                                           "charging location",
                                           WWCP.ChargingLocation.TryParse,
                                           out ChargingLocation? ChargingLocation,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargingProduct         [optional]

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

                #region Parse ReservationId           [optional]

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

                #region Parse CustomData              [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                RemoteStopRequest = new RemoteStopRequest(
                                        ChargingSessionId,
                                        RemoteAuthentication,
                                        AuthenticationPath,
                                        ChargingLocation,
                                        ChargingProduct,
                                        ReservationId,
                                        customData,

                                        Timestamp,
                                        EventTrackingId,
                                        RequestTimeout,
                                        CancellationToken
                                    );

                if (CustomRemoteStopRequestParser is not null)
                    RemoteStopRequest = CustomRemoteStopRequestParser(JSON,
                                                                        RemoteStopRequest);

                return true;

            }
            catch (Exception e)
            {
                RemoteStopRequest  = default;
                ErrorResponse       = "The given JSON representation of a RemoteStop request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRemoteStopRequestSerializer = null, CustomChargingProductSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomRemoteStopRequestSerializer">A delegate to customize the serialization of RemoteStopRequest responses.</param>
        /// <param name="CustomChargingProductSerializer">A delegate to serialize custom ChargingProduct JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RemoteStopRequest>?  CustomRemoteStopRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<ChargingProduct>?    CustomChargingProductSerializer     = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("chargingSessionId",       ChargingSessionId.       ToString()),

                           RemoteAuthentication is not null
                               ? new JProperty("remoteAuthentication",    RemoteAuthentication.    ToJSON())
                               : null,

                           AuthenticationPath.HasValue
                               ? new JProperty("authenticationPath",      AuthenticationPath.Value.ToString())
                               : null,

                           ChargingLocation is not null
                               ? new JProperty("chargingLocation",        ChargingLocation.        ToJSON())
                               : null,

                           ChargingProduct is not null
                               ? new JProperty("chargingProduct",         ChargingProduct.         ToJSON(Embedded: true,
                                                                                                          CustomChargingProductSerializer))
                               : null,

                           ReservationId.HasValue
                               ? new JProperty("chargingReservationId",   ReservationId.     Value.ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",              CustomData)
                               : null

                       );

            return CustomRemoteStopRequestSerializer is not null
                       ? CustomRemoteStopRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RemoteStopRequest1, RemoteStopRequest2)

        /// <summary>
        /// Compares two remote stop requests for equality.
        /// </summary>
        /// <param name="RemoteStopRequest1">A remote stop request.</param>
        /// <param name="RemoteStopRequest2">Another remote stop request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RemoteStopRequest RemoteStopRequest1,
                                           RemoteStopRequest RemoteStopRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RemoteStopRequest1, RemoteStopRequest2))
                return true;

            // If one is null, but not both, return false.
            if (RemoteStopRequest1 is null || RemoteStopRequest2 is null)
                return false;

            return RemoteStopRequest1.Equals(RemoteStopRequest2);

        }

        #endregion

        #region Operator != (RemoteStopRequest1, RemoteStopRequest2)

        /// <summary>
        /// Compares two remote stop requests for inequality.
        /// </summary>
        /// <param name="RemoteStopRequest1">A remote stop request.</param>
        /// <param name="RemoteStopRequest2">Another remote stop request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RemoteStopRequest RemoteStopRequest1,
                                           RemoteStopRequest RemoteStopRequest2)

            => !(RemoteStopRequest1 == RemoteStopRequest2);

        #endregion

        #endregion

        #region IEquatable<RemoteStopRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two remote stop requests for equality.
        /// </summary>
        /// <param name="Object">A remote stop request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RemoteStopRequest remoteStopRequest &&
                   Equals(remoteStopRequest);

        #endregion

        #region Equals(RemoteStopRequest)

        /// <summary>
        /// Compares two remote stop requests for equality.
        /// </summary>
        /// <param name="RemoteStopRequest">A remote stop request to compare with.</param>
        public override Boolean Equals(RemoteStopRequest? RemoteStopRequest)

            => RemoteStopRequest is not null &&

               ChargingSessionId.Equals(RemoteStopRequest.ChargingSessionId) &&

             ((RemoteAuthentication is null     &&  RemoteStopRequest.RemoteAuthentication is null)  ||
              (RemoteAuthentication is not null &&  RemoteStopRequest.RemoteAuthentication is not null && RemoteAuthentication.    Equals(RemoteStopRequest.RemoteAuthentication)))     &&

            ((!AuthenticationPath.  HasValue    && !RemoteStopRequest.AuthenticationPath.  HasValue) ||
              (AuthenticationPath.  HasValue    &&  RemoteStopRequest.AuthenticationPath.  HasValue    && AuthenticationPath.Value.Equals(RemoteStopRequest.AuthenticationPath.Value))) &&

             ((ChargingLocation     is null     &&  RemoteStopRequest.ChargingLocation     is null)  ||
              (ChargingLocation     is not null &&  RemoteStopRequest.ChargingLocation     is not null && ChargingLocation.        Equals(RemoteStopRequest.ChargingLocation)))         &&

             ((ChargingProduct      is null     &&  RemoteStopRequest.ChargingProduct      is null)  ||
              (ChargingProduct      is not null &&  RemoteStopRequest.ChargingProduct      is not null && ChargingProduct.         Equals(RemoteStopRequest.ChargingProduct)))          &&

            ((!ReservationId.       HasValue    && !RemoteStopRequest.ReservationId.       HasValue) ||
              (ReservationId.       HasValue    &&  RemoteStopRequest.ReservationId.       HasValue    && ReservationId.     Value.Equals(RemoteStopRequest.ReservationId.Value)));

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

            => $"{ChargingSessionId} {(ChargingLocation is not null ? $", at '{ChargingLocation}'" : "")} {(RemoteAuthentication is not null ? $", authentication '{RemoteAuthentication}'" : "")}";

        #endregion

    }

}
