/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The RemoteStop response.
    /// </summary>
    public class RemoteStopResponse : AResponse<RemoteStopRequest,
                                                 RemoteStopResponse>
    {

        #region Properties

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStopResultTypes    Result                 { get; }

        /// <summary>
        /// The charging session identification, e.g. in case of an unknown/invalid remote stop request.
        /// </summary>
        public ChargingSession_Id       ChargingSessionId      { get; }

        /// <summary>
        /// The charging session identification for an invalid remote stop operation.
        /// </summary>
        public ChargingSession?         ChargingSession        { get; }

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public I18NString               Description            { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public String?                  AdditionalInfo         { get; }

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        public ChargingReservation_Id?  ReservationId          { get; }

        /// <summary>
        /// The handling of the charging reservation after the charging session stopped.
        /// </summary>
        public ReservationHandling      ReservationHandling    { get; }

        /// <summary>
        /// The charge detail record for a successfully stopped charging process.
        /// </summary>
        public ChargeDetailRecord?      ChargeDetailRecord     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RemoteStop response.
        /// </summary>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// 
        /// <param name="Request">An optional RemoteStop request.</param>
        /// 
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="Warnings">Optional warnings.</param>
        public RemoteStopResponse(RemoteStopRequest        Request,
                                  RemoteStopResultTypes    Result,
                                  EventTracking_Id         EventTrackingId,
                                  DateTime                 ResponseTimestamp,

                                  ChargingSession_Id       ChargingSessionId,
                                  ChargingSession?         ChargingSession       = null,
                                  ChargingReservation_Id?  ReservationId         = null,
                                  ReservationHandling?     ReservationHandling   = null,
                                  ChargeDetailRecord?      ChargeDetailRecord    = null,

                                  I18NString?              Description           = null,
                                  String?                  AdditionalInfo        = null,
                                  IEnumerable<Warning>?    Warnings              = null,
                                  JObject?                 CustomData            = null,

                                  TimeSpan?                Runtime               = null,
                                  HTTPResponse?            HTTPResponse          = null)


            : base(Request,
                   EventTrackingId,
                   ResponseTimestamp,
                   Runtime,
                   Warnings,
                   CustomData,
                   HTTPResponse)

        {

            this.Result               = Result;
            this.ChargingSessionId    = ChargingSessionId;
            this.ChargingSession      = ChargingSession;
            this.ReservationId        = ReservationId;
            this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;
            this.ChargeDetailRecord   = ChargeDetailRecord;

            this.Description          = Description         ?? I18NString.Empty;
            this.AdditionalInfo       = AdditionalInfo;

            unchecked
            {

                hashCode = (this.Request?.           GetHashCode() ?? 0) * 53 ^
                            this.Result.             GetHashCode()       * 47 ^
                            this.EventTrackingId.    GetHashCode()       * 43 ^
                            this.ResponseTimestamp.  GetHashCode()       * 41 ^
                            this.Runtime.            GetHashCode()       * 37 ^

                            this.ChargingSessionId.  GetHashCode()       * 31 ^
                           (this.ChargingSession?.   GetHashCode() ?? 0) * 29 ^
                           (this.ReservationId?.     GetHashCode() ?? 0) * 23 ^
                            this.ReservationHandling.GetHashCode()       * 19 ^
                           (this.ChargeDetailRecord?.GetHashCode() ?? 0) * 17 ^

                            this.Description.        GetHashCode()       * 13 ^
                           (this.AdditionalInfo?.    GetHashCode() ?? 0) * 11 ^
                            this.Warnings.           CalcHashCode()      *  7 ^
                           (this.CustomData?.        GetHashCode() ?? 0) *  5 ^

                            this.Runtime.            GetHashCode()       *  3 ^
                           (this.HTTPResponse?.      GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Parse   (JSON, CustomRemoteStopResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a RemoteStop response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomRemoteStopResponseParser">A delegate to parse custom RemoteStop JSON objects.</param>
        public static RemoteStopResponse Parse(RemoteStopRequest                                 Request,
                                               JObject                                           JSON,
                                               DateTime                                          ResponseTimestamp,
                                               TimeSpan                                          Runtime,
                                               HTTPResponse?                                     HTTPResponse                     = null,
                                               CustomJObjectParserDelegate<RemoteStopResponse>?  CustomRemoteStopResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         Runtime,
                         out var remoteStopResponse,
                         out var errorResponse,
                         HTTPResponse,
                         CustomRemoteStopResponseParser))
            {
                return remoteStopResponse!;
            }

            throw new ArgumentException("The given JSON representation of a RemoteStop response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RemoteStopResponse, out ErrorResponse, CustomRemoteStopResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a RemoteStop response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="RemoteStopResponse">The parsed RemoteStop response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomRemoteStopResponseParser">A delegate to parse custom RemoteStop response JSON objects.</param>
        public static Boolean TryParse(RemoteStopRequest                                 Request,
                                       JObject                                           JSON,
                                       DateTime                                          ResponseTimestamp,
                                       TimeSpan                                          Runtime,
                                       out RemoteStopResponse?                           RemoteStopResponse,
                                       out String?                                       ErrorResponse,
                                       HTTPResponse?                                     HTTPResponse                     = null,
                                       CustomJObjectParserDelegate<RemoteStopResponse>?  CustomRemoteStopResponseParser   = null)
        {

            try
            {

                RemoteStopResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Result                 [mandatory]

                if (!JSON.ParseMandatory("result",
                                         "remote stop result",
                                         RemoteStopResultTypesExtensions.TryParse,
                                         out RemoteStopResultTypes Result,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EventTrackingId        [mandatory]

                if (!JSON.ParseMandatory("eventTrackingId",
                                         "event tracking identification",
                                         EventTracking_Id.TryParse,
                                         out EventTracking_Id EventTrackingId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse ChargingSession        [mandatory]

                if (!JSON.ParseMandatory("chargingSessionId",
                                         "charging session identification",
                                         ChargingSession_Id.TryParse,
                                         out ChargingSession_Id ChargingSessionId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse ChargingSession        [optional]

                if (JSON.ParseOptionalJSON("chargingSession",
                                           "remote start result",
                                           WWCP.ChargingSession.TryParse,
                                           out ChargingSession? ChargingSession,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ReservationId          [optional]

                if (JSON.ParseOptional("reservationId",
                                       "charging reservation identification",
                                       ChargingReservation_Id.TryParse,
                                       out ChargingReservation_Id? ReservationId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ReservationHandling    [optional]

                if (JSON.ParseOptionalJSON("reservationHandling",
                                           "charging reservation handling",
                                           WWCP.ReservationHandling.TryParse,
                                           out ReservationHandling? ReservationHandling,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse ChargeDetailRecord     [optional]

                if (JSON.ParseOptionalJSON("chargeDetailRecord",
                                           "charge detail record",
                                           WWCP.ChargeDetailRecord.TryParse,
                                           out ChargeDetailRecord? ChargeDetailRecord,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Parse Description            [optional]

                if (JSON.ParseOptionalJSON("description",
                                           "description",
                                           I18NString.TryParse,
                                           out I18NString? Description,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse AdditionalInfo         [optional]

                var AdditionalInfo = JSON.GetString("additionalInfo");

                #endregion

                #region Parse Warnings               [optional]

                if (JSON.ParseOptionalJSON("warnings",
                                           "warnings",
                                           Warning.TryParse,
                                           out IEnumerable<Warning> Warnings,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse CustomData             [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                RemoteStopResponse = new RemoteStopResponse(Request,
                                                            Result,
                                                            EventTrackingId,
                                                            ResponseTimestamp,

                                                            ChargingSessionId,
                                                            ChargingSession,
                                                            ReservationId,
                                                            ReservationHandling,
                                                            ChargeDetailRecord,

                                                            Description,
                                                            AdditionalInfo,
                                                            Warnings,
                                                            customData,

                                                            Runtime,
                                                            HTTPResponse);

                if (CustomRemoteStopResponseParser is not null)
                    RemoteStopResponse = CustomRemoteStopResponseParser(JSON,
                                                                          RemoteStopResponse);

                return true;

            }
            catch (Exception e)
            {
                RemoteStopResponse  = default;
                ErrorResponse        = "The given JSON representation of a RemoteStop response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRemoteStopResponseSerializer = null, CustomEVSEDataRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomRemoteStopResponseSerializer">A delegate to customize the serialization of RemoteStopResponse responses.</param>
        /// <param name="CustomEVSEDataRecordSerializer">A delegate to serialize custom EVSE data record JSON objects.</param>
        /// <param name="CustomAddressSerializer">A delegate to serialize custom address JSON objects.</param>
        /// <param name="CustomChargingFacilitySerializer">A delegate to serialize custom charging facility JSON objects.</param>
        /// <param name="CustomGeoCoordinatesSerializer">A delegate to serialize custom geo coordinates JSON objects.</param>
        /// <param name="CustomEnergyMeterSerializer">A delegate to serialize custom energy meter JSON objects.</param>
        /// <param name="CustomTransparencySoftwareStatusSerializer">A delegate to serialize custom transparency software status JSON objects.</param>
        /// <param name="CustomTransparencySoftwareSerializer">A delegate to serialize custom transparency software JSON objects.</param>
        /// <param name="CustomEnergySourceSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomEnvironmentalImpactSerializer">A delegate to serialize custom time period JSON objects.</param>
        /// <param name="CustomOpeningTimesSerializer">A delegate to serialize custom opening time JSON objects.</param>
        /// <param name="CustomStatusCodeSerializer">A delegate to serialize custom StatusCode JSON elements.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RemoteStopResponse>?        CustomRemoteStopResponseSerializer         = null)
        {

            var json = JSONObject.Create(

                           //new JProperty("content",           new JArray(EVSEDataRecords.Select(evseDataRecord => evseDataRecord.ToJSON(CustomEVSEDataRecordSerializer,
                           //                                                                                                             CustomAddressSerializer,
                           //                                                                                                             CustomChargingFacilitySerializer,
                           //                                                                                                             CustomGeoCoordinatesSerializer,
                           //                                                                                                             CustomEnergyMeterSerializer,
                           //                                                                                                             CustomTransparencySoftwareStatusSerializer,
                           //                                                                                                             CustomTransparencySoftwareSerializer,
                           //                                                                                                             CustomEnergySourceSerializer,
                           //                                                                                                             CustomEnvironmentalImpactSerializer,
                           //                                                                                                             CustomOpeningTimesSerializer)))),

                           //StatusCode is not null
                           //    ? new JProperty("StatusCode",  StatusCode.ToJSON(CustomStatusCodeSerializer))
                           //    : null,

                           CustomData is not null
                               ? new JProperty("CustomData",  CustomData)
                               : null

                       );

            return CustomRemoteStopResponseSerializer is not null
                       ? CustomRemoteStopResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RemoteStopResponse1, RemoteStopResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="RemoteStopResponse1">A RemoteStop response.</param>
        /// <param name="RemoteStopResponse2">Another RemoteStop response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RemoteStopResponse RemoteStopResponse1,
                                           RemoteStopResponse RemoteStopResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RemoteStopResponse1, RemoteStopResponse2))
                return true;

            // If one is null, but not both, return false.
            if (RemoteStopResponse1 is null || RemoteStopResponse2 is null)
                return false;

            return RemoteStopResponse1.Equals(RemoteStopResponse2);

        }

        #endregion

        #region Operator != (RemoteStopResponse1, RemoteStopResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="RemoteStopResponse1">A RemoteStop response.</param>
        /// <param name="RemoteStopResponse2">Another RemoteStop response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RemoteStopResponse RemoteStopResponse1,
                                           RemoteStopResponse RemoteStopResponse2)

            => !(RemoteStopResponse1 == RemoteStopResponse2);

        #endregion

        #endregion

        #region IEquatable<RemoteStopResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two remote stop responses for equality.
        /// </summary>
        /// <param name="Object">A remote stop response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RemoteStopResponse remoteStopResponse &&
                   Equals(remoteStopResponse);

        #endregion

        #region Equals(RemoteStopResponse)

        /// <summary>
        /// Compares two remote stop responses for equality.
        /// </summary>
        /// <param name="RemoteStopResponse">A remote stop response to compare with.</param>
        public Boolean Equals(RemoteStopResponse? RemoteStopResponse)

            => RemoteStopResponse is not null &&

               base.Equals(RemoteStopResponse) &&

               Result.             Equals(RemoteStopResponse.Result)              &&
               ChargingSessionId.  Equals(RemoteStopResponse.ChargingSessionId)   &&
               ReservationHandling.Equals(RemoteStopResponse.ReservationHandling) &&

             ((Request            is     null &&  RemoteStopResponse.Request            is     null) ||
              (Request            is not null &&  RemoteStopResponse.Request            is not null && Request.            Equals(RemoteStopResponse.Request)))             &&

             ((ChargingSession    is     null &&  RemoteStopResponse.ChargingSession    is     null) ||
              (ChargingSession    is not null &&  RemoteStopResponse.ChargingSession    is not null && ChargingSession.    Equals(RemoteStopResponse.ChargingSession)))     &&

            ((!ReservationId.     HasValue    && !RemoteStopResponse.ReservationId.     HasValue)    ||
              (ReservationId.     HasValue    &&  RemoteStopResponse.ReservationId.     HasValue    && ReservationId.Value.Equals(RemoteStopResponse.ReservationId.Value))) &&

             ((ChargeDetailRecord is     null &&  RemoteStopResponse.ChargeDetailRecord is     null) ||
              (ChargeDetailRecord is not null &&  RemoteStopResponse.ChargeDetailRecord is not null && ChargeDetailRecord. Equals(RemoteStopResponse.ChargeDetailRecord)))  &&

             ((Description        is     null &&  RemoteStopResponse.Description        is     null) ||
              (Description        is not null &&  RemoteStopResponse.Description        is not null && Description.        Equals(RemoteStopResponse.Description)))         &&

             ((AdditionalInfo     is     null &&  RemoteStopResponse.AdditionalInfo     is     null) ||
              (AdditionalInfo     is not null &&  RemoteStopResponse.AdditionalInfo     is not null && AdditionalInfo.     Equals(RemoteStopResponse.AdditionalInfo)));

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

            => $"{ChargingSessionId} => '{Result}' in {Runtime.TotalMilliseconds} msec!";

        #endregion


        #region ToBuilder

        /// <summary>
        /// Return a response builder.
        /// </summary>
        public Builder ToBuilder

            => new (Request,
                    Result,
                    EventTrackingId,
                    ResponseTimestamp,

                    ChargingSessionId,
                    ChargingSession,
                    ReservationId,
                    ReservationHandling,
                    ChargeDetailRecord,

                    Description,
                    AdditionalInfo,
                    Warnings,
                    CustomData,

                    Runtime,
                    HTTPResponse);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The RemoteStop response builder.
        /// </summary>
        public new class Builder : AResponse<RemoteStopRequest,
                                             RemoteStopResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// The result of a remote start operation.
            /// </summary>
            public RemoteStopResultTypes?   Result                 { get; set; }

            /// <summary>
            /// The charging session identification, e.g. in case of an unknown/invalid remote stop request.
            /// </summary>
            public ChargingSession_Id?      ChargingSessionId      { get; set; }

            /// <summary>
            /// The charging session identification for an invalid remote stop operation.
            /// </summary>
            public ChargingSession?         ChargingSession        { get; set; }

            /// <summary>
            /// A optional description of the authorize stop result.
            /// </summary>
            public I18NString               Description            { get; }

            /// <summary>
            /// An optional additional message.
            /// </summary>
            public String?                  AdditionalInfo         { get; set; }

            /// <summary>
            /// The charging reservation identification.
            /// </summary>
            public ChargingReservation_Id?  ReservationId          { get; set; }

            /// <summary>
            /// The handling of the charging reservation after the charging session stopped.
            /// </summary>
            public ReservationHandling      ReservationHandling    { get; set; }

            /// <summary>
            /// The charge detail record for a successfully stopped charging process.
            /// </summary>
            public ChargeDetailRecord?      ChargeDetailRecord     { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new RemoteStop response builder.
            /// </summary>
            /// <param name="Request">A RemoteStop request.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// 
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            public Builder(RemoteStopRequest?       Request               = null,
                           RemoteStopResultTypes?   Result                = null,
                           EventTracking_Id?        EventTrackingId       = null,
                           DateTime?                ResponseTimestamp     = null,

                           ChargingSession_Id?      ChargingSessionId     = null,
                           ChargingSession?         ChargingSession       = null,
                           ChargingReservation_Id?  ReservationId         = null,
                           ReservationHandling?     ReservationHandling   = null,
                           ChargeDetailRecord?      ChargeDetailRecord    = null,

                           I18NString?              Description           = null,
                           String?                  AdditionalInfo        = null,
                           IEnumerable<Warning>?    Warnings              = null,
                           JObject?                 CustomData            = null,

                           TimeSpan?                Runtime               = null,
                           HTTPResponse?            HTTPResponse          = null)

                : base(Request,
                       EventTrackingId,
                       ResponseTimestamp,
                       Runtime,
                       Warnings,
                       CustomData,
                       HTTPResponse)

            {

                this.Result               = Result;
                this.ChargingSessionId    = ChargingSessionId;
                this.ChargingSession      = ChargingSession;
                this.ReservationId        = ReservationId;
                this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;
                this.ChargeDetailRecord   = ChargeDetailRecord;

                this.Description          = Description         ?? I18NString.Empty;
                this.AdditionalInfo       = AdditionalInfo;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the RemoteStop response.
            /// </summary>
            /// <param name="Builder">A RemoteStopResponse builder.</param>
            public static implicit operator RemoteStopResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the RemoteStop response.
            /// </summary>
            public override RemoteStopResponse ToImmutable()
            {

                if (Request is null)
                    throw new ArgumentNullException(nameof(Request),            "The given request must not be null!");

                if (!Result.HasValue)
                    throw new ArgumentNullException(nameof(Result),             "The given result must not be null!");

                if (!ChargingSessionId.HasValue)
                    throw new ArgumentNullException(nameof(ChargingSessionId),  "The given charging session identification must not be null!");


                var now = Timestamp.Now;

                return new (Request,
                            Result.Value,
                            EventTrackingId   ?? EventTracking_Id.New,
                            ResponseTimestamp ?? now,

                            ChargingSessionId.Value,
                            ChargingSession,
                            ReservationId,
                            ReservationHandling,
                            ChargeDetailRecord,

                            Description,
                            AdditionalInfo,
                            Warnings,
                            CustomData,

                            Runtime           ?? (now - (Request?.Timestamp ?? now)),
                            HTTPResponse);

            }

            #endregion

        }

        #endregion

    }

}
