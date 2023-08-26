/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The RemoteStart response.
    /// </summary>
    public class RemoteStartResponse : AResponse<RemoteStartRequest,
                                                 RemoteStartResponse>
    {

        #region Properties

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStartResultTypes  Result             { get; }

        /// <summary>
        /// The charging session for the remote start operation.
        /// </summary>
        public ChargingSession?        ChargingSession    { get; }

        /// <summary>
        /// A optional description of the remote start result.
        /// </summary>
        public I18NString              Description        { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public String?                 AdditionalInfo     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RemoteStart response.
        /// </summary>
        /// <param name="Request">An optional RemoteStart request.</param>
        /// <param name="Result"></param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this response with other events.</param>
        /// <param name="ResponseTimestamp">An timestamp of the response creation.</param>
        /// 
        /// <param name="ChargingSession"></param>
        /// 
        /// <param name="Description"></param>
        /// <param name="AdditionalInfo"></param>
        /// <param name="Warnings">Optional warnings.</param>
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// 
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        public RemoteStartResponse(RemoteStartRequest      Request,
                                   RemoteStartResultTypes  Result,
                                   EventTracking_Id        EventTrackingId,
                                   DateTime                ResponseTimestamp,

                                   ChargingSession?        ChargingSession   = null,

                                   I18NString?             Description       = null,
                                   String?                 AdditionalInfo    = null,
                                   IEnumerable<Warning>?   Warnings          = null,
                                   JObject?                CustomData        = null,

                                   TimeSpan?               Runtime           = null,
                                   HTTPResponse?           HTTPResponse      = null)

            : base(Request,
                   EventTrackingId,
                   ResponseTimestamp,
                   Runtime,
                   Warnings,
                   CustomData,
                   HTTPResponse)

        {

            this.Result           = Result;
            this.ChargingSession  = ChargingSession;
            this.Description      = Description ?? I18NString.Empty;
            this.AdditionalInfo   = AdditionalInfo;

            unchecked
            {

                hashCode = (this.Request?.         GetHashCode() ?? 0) * 31 ^
                            this.Result.           GetHashCode()       * 29 ^
                            this.EventTrackingId.  GetHashCode()       * 23 ^
                            this.ResponseTimestamp.GetHashCode()       * 19 ^

                           (this.ChargingSession?. GetHashCode() ?? 0) * 17 ^
                            this.Description.      GetHashCode()       * 13 ^
                           (this.AdditionalInfo?.  GetHashCode() ?? 0) * 11 ^
                            this.Warnings.         CalcHashCode()      *  7 ^
                           (this.CustomData?.      GetHashCode() ?? 0) *  5 ^

                            this.Runtime.          GetHashCode()       *  3 ^
                           (this.HTTPResponse?.    GetHashCode() ?? 0);

            }

        }

        #endregion


        #region (static) Parse   (JSON, CustomRemoteStartResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a RemoteStart response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomRemoteStartResponseParser">A delegate to parse custom RemoteStart JSON objects.</param>
        public static RemoteStartResponse Parse(RemoteStartRequest                                 Request,
                                                JObject                                            JSON,
                                                DateTime                                           ResponseTimestamp,
                                                TimeSpan                                           Runtime,
                                                HTTPResponse?                                      HTTPResponse                      = null,
                                                CustomJObjectParserDelegate<RemoteStartResponse>?  CustomRemoteStartResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         ResponseTimestamp,
                         Runtime,
                         out var pullEVSEDataResponse,
                         out var errorResponse,
                         HTTPResponse,
                         CustomRemoteStartResponseParser))
            {
                return pullEVSEDataResponse!;
            }

            throw new ArgumentException("The given JSON representation of a RemoteStart response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RemoteStartResponse, out ErrorResponse, CustomRemoteStartResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a RemoteStart response.
        /// </summary>
        /// <param name="Request">The request leading to this response.</param>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
        /// <param name="Runtime">The runtime of the request/response.</param>
        /// <param name="RemoteStartResponse">The parsed RemoteStart response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="HTTPResponse">The optional HTTP response.</param>
        /// <param name="CustomRemoteStartResponseParser">A delegate to parse custom RemoteStart response JSON objects.</param>
        public static Boolean TryParse(RemoteStartRequest                                 Request,
                                       JObject                                            JSON,
                                       DateTime                                           ResponseTimestamp,
                                       TimeSpan                                           Runtime,
                                       out RemoteStartResponse?                           RemoteStartResponse,
                                       out String?                                        ErrorResponse,
                                       HTTPResponse?                                      HTTPResponse                      = null,
                                       CustomJObjectParserDelegate<RemoteStartResponse>?  CustomRemoteStartResponseParser   = null)
        {

            try
            {

                RemoteStartResponse = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Result             [mandatory]

                if (!JSON.ParseMandatory("result",
                                         "remote start result",
                                         RemoteStartResultTypesExtensions.TryParse,
                                         out RemoteStartResultTypes Result,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse EventTrackingId    [mandatory]

                if (!JSON.ParseMandatory("eventTrackingId",
                                         "event tracking identification",
                                         EventTracking_Id.TryParse,
                                         out EventTracking_Id EventTrackingId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Parse ChargingSession    [optional]

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


                #region Parse Description        [optional]

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

                #region Parse AdditionalInfo     [optional]

                var AdditionalInfo = JSON.GetString("additionalInfo");

                #endregion


                #region Parse Warnings           [optional]

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

                #region Parse CustomData         [optional]

                var customData = JSON[nameof(CustomData)] as JObject;

                #endregion


                RemoteStartResponse = new RemoteStartResponse(Request,
                                                              Result,
                                                              EventTrackingId,
                                                              ResponseTimestamp,

                                                              ChargingSession,
                                                              Description,
                                                              AdditionalInfo,

                                                              Warnings,
                                                              customData,
                                                              Runtime,
                                                              HTTPResponse);

                if (CustomRemoteStartResponseParser is not null)
                    RemoteStartResponse = CustomRemoteStartResponseParser(JSON,
                                                                          RemoteStartResponse);

                return true;

            }
            catch (Exception e)
            {
                RemoteStartResponse  = default;
                ErrorResponse        = "The given JSON representation of a RemoteStart response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRemoteStartResponseSerializer = null, CustomChargeDetailRecordSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomRemoteStartResponseSerializer">A delegate to customize the serialization of RemoteStartResponse responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RemoteStartResponse>?  CustomRemoteStartResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>?   CustomChargeDetailRecordSerializer    = null,
                              CustomJObjectSerializerDelegate<SendCDRResult>?        CustomSendCDRResultSerializer         = null,
                              CustomJObjectSerializerDelegate<ChargingSession>?      CustomChargingSessionSerializer       = null,
                              CustomJObjectSerializerDelegate<Warning>?              CustomWarningSerializer               = null)
        {

            var json = JSONObject.Create(

                                   new JProperty("result",            Result.         ToString()),
                                   new JProperty("eventTrackingId",   EventTrackingId.ToString()),

                           ChargingSession is not null
                               ? new JProperty("chargingSession",     ChargingSession.ToJSON(Embedded: true,
                                                                                             CustomChargeDetailRecordSerializer,
                                                                                             CustomSendCDRResultSerializer,
                                                                                             CustomChargingSessionSerializer))
                               : null,

                           Description is not null
                               ? new JProperty("description",         Description.    ToJSON())
                               : null,

                           AdditionalInfo is not null
                               ? new JProperty("additionalInfo",      AdditionalInfo)
                               : null,

                           Warnings is not null
                               ? new JProperty("warnings",            new JArray(Warnings.Select(warning => warning.ToJSON(CustomWarningSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",          CustomData)
                               : null

                       );

            return CustomRemoteStartResponseSerializer is not null
                       ? CustomRemoteStartResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RemoteStartResponse1, RemoteStartResponse2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="RemoteStartResponse1">A RemoteStart response.</param>
        /// <param name="RemoteStartResponse2">Another RemoteStart response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RemoteStartResponse RemoteStartResponse1,
                                           RemoteStartResponse RemoteStartResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RemoteStartResponse1, RemoteStartResponse2))
                return true;

            // If one is null, but not both, return false.
            if (RemoteStartResponse1 is null || RemoteStartResponse2 is null)
                return false;

            return RemoteStartResponse1.Equals(RemoteStartResponse2);

        }

        #endregion

        #region Operator != (RemoteStartResponse1, RemoteStartResponse2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="RemoteStartResponse1">A RemoteStart response.</param>
        /// <param name="RemoteStartResponse2">Another RemoteStart response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RemoteStartResponse RemoteStartResponse1,
                                           RemoteStartResponse RemoteStartResponse2)

            => !(RemoteStartResponse1 == RemoteStartResponse2);

        #endregion

        #endregion

        #region IEquatable<RemoteStartResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two remote start responses for equality.
        /// </summary>
        /// <param name="Object">A remote start response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RemoteStartResponse remoteStartResponse &&
                   Equals(remoteStartResponse);

        #endregion

        #region Equals(RemoteStartResponse)

        /// <summary>
        /// Compares two remote start responses for equality.
        /// </summary>
        /// <param name="RemoteStartResponse">A remote start response to compare with.</param>
        public Boolean Equals(RemoteStartResponse? RemoteStartResponse)

            => RemoteStartResponse is not null &&

               base.Equals(RemoteStartResponse) &&

               Result.         Equals(RemoteStartResponse.Result)          &&

             ((ChargingSession is     null &&  RemoteStartResponse.ChargingSession is     null) ||
              (ChargingSession is not null &&  RemoteStartResponse.ChargingSession is not null && ChargingSession. Equals(RemoteStartResponse.ChargingSession))) &&

             ((Description     is     null &&  RemoteStartResponse.Description     is     null) ||
              (Description     is not null &&  RemoteStartResponse.Description     is not null && Description.     Equals(RemoteStartResponse.Description)))     &&

             ((AdditionalInfo  is     null &&  RemoteStartResponse.AdditionalInfo  is     null) ||
              (AdditionalInfo  is not null &&  RemoteStartResponse.AdditionalInfo  is not null && AdditionalInfo.  Equals(RemoteStartResponse.AdditionalInfo)));

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

            => $"{Result}";

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

                    ChargingSession,
                    Description,
                    AdditionalInfo,

                    Warnings,
                    CustomData,
                    Runtime,
                    HTTPResponse);

        #endregion

        #region (class) Builder

        /// <summary>
        /// The RemoteStart response builder.
        /// </summary>
        public new class Builder : AResponse<RemoteStartRequest,
                                             RemoteStartResponse>.Builder
        {

            #region Properties

            /// <summary>
            /// The result of a remote start operation.
            /// </summary>
            public RemoteStartResultTypes?  Result             { get; set; }

            /// <summary>
            /// The charging session for the remote start operation.
            /// </summary>
            public ChargingSession?         ChargingSession    { get; set; }

            /// <summary>
            /// A optional description of the remote start result.
            /// </summary>
            public I18NString               Description        { get; }

            /// <summary>
            /// An optional additional information on this error,
            /// e.g. the HTTP error response.
            /// </summary>
            public String?                  AdditionalInfo     { get; set; }

            #endregion

            #region Constructor(s)

            /// <summary>
            /// Create a new RemoteStart response builder.
            /// </summary>
            /// <param name="Request">A RemoteStart request.</param>
            /// <param name="Result"></param>
            /// <param name="EventTrackingId">An optional event tracking identification for correlating this response with other events.</param>
            /// <param name="ResponseTimestamp">The timestamp of the response creation.</param>
            /// 
            /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
            /// 
            /// <param name="Runtime">The runtime of the request/response.</param>
            /// <param name="HTTPResponse">The optional HTTP response.</param>
            public Builder(RemoteStartRequest?      Request             = null,
                           RemoteStartResultTypes?  Result              = null,
                           EventTracking_Id?        EventTrackingId     = null,
                           DateTime?                ResponseTimestamp   = null,

                           ChargingSession?         ChargingSession     = null,
                           I18NString?              Description         = null,
                           String?                  AdditionalInfo      = null,
                           IEnumerable<Warning>?    Warnings            = null,
                           JObject?                 CustomData          = null,

                           TimeSpan?                Runtime             = null,
                           HTTPResponse?            HTTPResponse        = null)

                : base(Request,
                       EventTrackingId,
                       ResponseTimestamp,
                       Runtime,
                       Warnings,
                       CustomData,
                       HTTPResponse)

            {

                this.Result           = Result;
                this.ChargingSession  = ChargingSession;
                this.Description      = Description ?? I18NString.Empty;
                this.AdditionalInfo   = AdditionalInfo;

            }

            #endregion

            #region ToImmutable()

            /// <summary>
            /// Return an immutable version of the RemoteStart response.
            /// </summary>
            /// <param name="Builder">A RemoteStartResponse builder.</param>
            public static implicit operator RemoteStartResponse(Builder Builder)

                => Builder.ToImmutable();


            /// <summary>
            /// Return an immutable version of the RemoteStart response.
            /// </summary>
            public override RemoteStartResponse ToImmutable()
            {

                if (Request is null)
                    throw new ArgumentNullException(nameof(Request), "The given request must not be null!");

                if (!Result.HasValue)
                    throw new ArgumentNullException(nameof(Request), "The given result must not be null!");

                var now = Timestamp.Now;

                return new (Request,
                            Result.Value,
                            EventTrackingId   ?? EventTracking_Id.New,
                            ResponseTimestamp ?? now,

                            ChargingSession,
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
