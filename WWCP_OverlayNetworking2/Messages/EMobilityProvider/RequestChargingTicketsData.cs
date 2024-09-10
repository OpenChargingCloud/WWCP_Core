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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The RequestChargingTickets data.
    /// </summary>
    public class RequestChargingTicketsData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/wwcp/registerEMobilityAccountData");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext  Context
            => DefaultJSONLDContext;

        public PublicKey                        PublicKey                   { get; }
        public IEnumerable<Signature>           Signatures                  { get; }

        public DateTime                         NotBefore                   { get; }
        public DateTime?                        NotAfter                    { get; }
        public TimeSpan?                        Lifetime                    { get; }
        public IEnumerable<DayOfWeek>           DaysOfWeek                  { get; }
        public Time?                            StartTimeOfDay              { get; }
        public Time?                            EndTimeOfDay                { get; }
        public OCPPv2_1.EVSEKinds?              EVSEKind                    { get; }
        public WattHour?                        MaxEnergy                   { get; }
        public Ampere?                          MaxCurrent                  { get; }
        public Watt?                            MaxPower                    { get; }
        public TimeSpan?                        MaxTime                     { get; }
        public OCPPv2_1.PaymentRecognition?     PaymentRecognition          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RequestChargingTickets data.
        /// </summary>
        public RequestChargingTicketsData(PublicKey                        PublicKey,
                                          IEnumerable<Signature>           Signatures,

                                          DateTime?                        NotBefore            = null,
                                          DateTime?                        NotAfter             = null,
                                          TimeSpan?                        Lifetime             = null,
                                          IEnumerable<DayOfWeek>?          DaysOfWeek           = null,
                                          Time?                            StartTimeOfDay       = null,
                                          Time?                            EndTimeOfDay         = null,
                                          OCPPv2_1.EVSEKinds?              EVSEKind             = null,
                                          WattHour?                        MaxEnergy            = null,
                                          Ampere?                          MaxCurrent           = null,
                                          Watt?                            MaxPower             = null,
                                          TimeSpan?                        MaxTime              = null,
                                          OCPPv2_1.PaymentRecognition?     PaymentRecognition   = null)

        {

            this.PublicKey                 = PublicKey;
            this.Signatures                = Signatures.Distinct();
            this.NotBefore                 = NotBefore ?? Timestamp.Now;
            this.NotAfter                  = NotAfter  ?? this.NotBefore + Lifetime;

            unchecked
            {

                hashCode = this.NotBefore.GetHashCode() * 17 ^
                           this.NotAfter. GetHashCode() * 13;

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse   (JSON, CustomRequestChargingTicketsDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of RequestChargingTickets data.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomRequestChargingTicketsDataParser">A delegate to parse custom RequestChargingTicketsData JSON objects.</param>
        public static RequestChargingTicketsData Parse(JObject                                                     JSON,
                                                         CustomJObjectParserDelegate<RequestChargingTicketsData>?  CustomRequestChargingTicketsDataParser   = null)
        {

            if (TryParse(JSON,
                         out var registerEMobilityAccountData,
                         out var errorResponse,
                         CustomRequestChargingTicketsDataParser))
            {
                return registerEMobilityAccountData;
            }

            throw new ArgumentException("The given JSON representation of a RequestChargingTicketsData request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RequestChargingTicketsData, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given JSON representation of RequestChargingTicketsData.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestChargingTicketsData">The parsed RequestChargingTicketsData.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRequestChargingTicketsDataParser">A delegate to parse custom RequestChargingTicketsData requests.</param>
        public static Boolean TryParse(JObject                                                     JSON,
                                       [NotNullWhen(true)]  out RequestChargingTicketsData?      RequestChargingTicketsData,
                                       [NotNullWhen(false)] out String?                            ErrorResponse,
                                       CustomJObjectParserDelegate<RequestChargingTicketsData>?  CustomRequestChargingTicketsDataParser   = null)
        {

            try
            {

                RequestChargingTicketsData = null;

                #region Username                    [mandatory]

                if (!JSON.ParseMandatoryText("username",
                                             "username",
                                             out String? Username,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Password                    [mandatory]

                if (!JSON.ParseMandatoryText("password",
                                             "password",
                                             out String? Password,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region EMailAddress                [mandatory]

                if (!JSON.ParseMandatory("eMailAddress",
                                         "eMail address",
                                         SimpleEMailAddress.TryParse,
                                         out SimpleEMailAddress EMailAddress,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region PublicKeys                  [mandatory]

                if (!JSON.ParseMandatoryHashSet("publicKeys",
                                                "public keys",
                                                ECCPublicKey.TryParse,
                                                out HashSet<ECCPublicKey> PublicKeys,
                                                out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Signatures                  [mandatory]

                if (!JSON.ParseMandatoryHashSet("signatures",
                                                "signatures",
                                                Signature.TryParse,
                                                out HashSet<Signature> Signatures,
                                                out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region AdditionalEMailAddresses    [optional]

                if (JSON.ParseOptionalHashSet("additionalEMailAddresses",
                                              "additional e-mail addresses",
                                              SimpleEMailAddress.TryParse,
                                              out HashSet<SimpleEMailAddress> AdditionalEMailAddresses,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region PhoneNumber                 [optional]

                if (JSON.ParseOptional("phoneNumber",
                                       "phoneNumber",
                                       org.GraphDefined.Vanaheimr.Illias.PhoneNumber.TryParse,
                                       out PhoneNumber PhoneNumber,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                RequestChargingTicketsData = new RequestChargingTicketsData(
                                                   ECCPublicKey.Parse(""),
                                                   [],
                                                   Timestamp.Now,
                                                   null,
                                                   TimeSpan.FromDays(7)
                                               );

                if (CustomRequestChargingTicketsDataParser is not null)
                    RequestChargingTicketsData = CustomRequestChargingTicketsDataParser(JSON,
                                                                                            RequestChargingTicketsData);

                return true;

            }
            catch (Exception e)
            {
                RequestChargingTicketsData  = null;
                ErrorResponse                 = "The given JSON representation of a RequestChargingTicketsData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRequestChargingTicketsDataSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRequestChargingTicketsDataSerializer">A delegate to serialize custom RequestChargingTicketsData requests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RequestChargingTicketsData>?  CustomRequestChargingTicketsDataSerializer   = null,
                              CustomJObjectSerializerDelegate<Signature>?            CustomSignatureSerializer             = null)
        {

            var json = JSONObject.Create(

                           //      new JProperty("vendorId",     VendorId.       TextId),

                           //MessageId.HasValue
                           //    ? new JProperty("messageId",    MessageId.Value.TextId)
                           //    : null,

                           //Data is not null
                           //    ? new JProperty("data",         Data)
                           //    : null,

                           //Signatures.Any()
                           //    ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                           //                                                                                               CustomCustomDataSerializer))))
                           //    : null,

                           //CustomData is not null
                           //    ? new JProperty("customData",   CustomData.ToJSON(CustomCustomDataSerializer))
                           //    : null

                       );

            return CustomRequestChargingTicketsDataSerializer is not null
                       ? CustomRequestChargingTicketsDataSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RequestChargingTicketsData1, RequestChargingTicketsData2)

        /// <summary>
        /// Compares two RequestChargingTicketsData requests for equality.
        /// </summary>
        /// <param name="RequestChargingTicketsData1">A RequestChargingTicketsData request.</param>
        /// <param name="RequestChargingTicketsData2">Another RequestChargingTicketsData request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RequestChargingTicketsData? RequestChargingTicketsData1,
                                           RequestChargingTicketsData? RequestChargingTicketsData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RequestChargingTicketsData1, RequestChargingTicketsData2))
                return true;

            // If one is null, but not both, return false.
            if (RequestChargingTicketsData1 is null || RequestChargingTicketsData2 is null)
                return false;

            return RequestChargingTicketsData1.Equals(RequestChargingTicketsData2);

        }

        #endregion

        #region Operator != (RequestChargingTicketsData1, RequestChargingTicketsData2)

        /// <summary>
        /// Compares two RequestChargingTicketsData requests for inequality.
        /// </summary>
        /// <param name="RequestChargingTicketsData1">A RequestChargingTicketsData request.</param>
        /// <param name="RequestChargingTicketsData2">Another RequestChargingTicketsData request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RequestChargingTicketsData? RequestChargingTicketsData1,
                                           RequestChargingTicketsData? RequestChargingTicketsData2)

            => !(RequestChargingTicketsData1 == RequestChargingTicketsData2);

        #endregion

        #endregion

        #region IEquatable<RequestChargingTicketsData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two RequestChargingTicketsData requests for equality.
        /// </summary>
        /// <param name="Object">A RequestChargingTicketsData request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RequestChargingTicketsData registerEMobilityAccountData &&
                   Equals(registerEMobilityAccountData);

        #endregion

        #region Equals(RequestChargingTicketsData)

        /// <summary>
        /// Compares two RequestChargingTicketsData requests for equality.
        /// </summary>
        /// <param name="RequestChargingTicketsData">A RequestChargingTicketsData request to compare with.</param>
        public Boolean Equals(RequestChargingTicketsData? RequestChargingTicketsData)

            => RequestChargingTicketsData is not null;

               //String.Equals(NotBefore, RequestChargingTicketsData.NotBefore, StringComparison.OrdinalIgnoreCase);

             //((MessageId is     null && RequestChargingTicketsData.MessageId is     null) ||
             // (MessageId is not null && RequestChargingTicketsData.MessageId is not null && MessageId.Equals(RequestChargingTicketsData.MessageId))) &&

             //((Data      is     null && RequestChargingTicketsData.Data      is     null) ||
             // (Data      is not null && RequestChargingTicketsData.Data      is not null && Data.     Equals(RequestChargingTicketsData.Data)))      &&

             //  base.GenericEquals(RequestChargingTicketsData);

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

            => "";// $"{VendorId}: {MessageId?.ToString() ?? "-"} => {Data?.ToString() ?? "-"}";

        #endregion


    }


}
