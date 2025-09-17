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
using cloud.charging.open.protocols.OCPPv2_1;
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The RevokeChargingTickets data.
    /// </summary>
    public class RevokeChargingTicketsData
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
        //public OCPPv2_1.EVSEKinds?              EVSEKind                    { get; }
        public WattHour?                        MaxEnergy                   { get; }
        public Ampere?                          MaxCurrent                  { get; }
        public Watt?                            MaxPower                    { get; }
        public TimeSpan?                        MaxTime                     { get; }
        //public OCPPv2_1.PaymentRecognition?     PaymentRecognition          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RevokeChargingTickets data.
        /// </summary>
        public RevokeChargingTicketsData(ChargingTicket_Id       ChargingTicketId,
                                         IEnumerable<Signature>  Signatures,
                                         DateTimeOffset?         RevocateForTimeStamp = null)

        {

            this.PublicKey                 = PublicKey;
            this.Signatures                = Signatures.Distinct();

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

        #region (static) Parse   (JSON, CustomRevokeChargingTicketsDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of RevokeChargingTickets data.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomRevokeChargingTicketsDataParser">A delegate to parse custom RevokeChargingTicketsData JSON objects.</param>
        public static RevokeChargingTicketsData Parse(JObject                                                     JSON,
                                                         CustomJObjectParserDelegate<RevokeChargingTicketsData>?  CustomRevokeChargingTicketsDataParser   = null)
        {

            if (TryParse(JSON,
                         out var registerEMobilityAccountData,
                         out var errorResponse,
                         CustomRevokeChargingTicketsDataParser))
            {
                return registerEMobilityAccountData;
            }

            throw new ArgumentException("The given JSON representation of a RevokeChargingTicketsData request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RevokeChargingTicketsData, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given JSON representation of RevokeChargingTicketsData.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RevokeChargingTicketsData">The parsed RevokeChargingTicketsData.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRevokeChargingTicketsDataParser">A delegate to parse custom RevokeChargingTicketsData requests.</param>
        public static Boolean TryParse(JObject                                                     JSON,
                                       [NotNullWhen(true)]  out RevokeChargingTicketsData?      RevokeChargingTicketsData,
                                       [NotNullWhen(false)] out String?                            ErrorResponse,
                                       CustomJObjectParserDelegate<RevokeChargingTicketsData>?  CustomRevokeChargingTicketsDataParser   = null)
        {

            try
            {

                RevokeChargingTicketsData = null;

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


                RevokeChargingTicketsData = new RevokeChargingTicketsData(
                                                   ChargingTicket_Id.NewRandom("EMPId"),
                                                   [],
                                                   Timestamp.Now + TimeSpan.FromHours(1)
                                               );

                if (CustomRevokeChargingTicketsDataParser is not null)
                    RevokeChargingTicketsData = CustomRevokeChargingTicketsDataParser(JSON,
                                                                                            RevokeChargingTicketsData);

                return true;

            }
            catch (Exception e)
            {
                RevokeChargingTicketsData  = null;
                ErrorResponse                 = "The given JSON representation of a RevokeChargingTicketsData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRevokeChargingTicketsDataSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRevokeChargingTicketsDataSerializer">A delegate to serialize custom RevokeChargingTicketsData requests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RevokeChargingTicketsData>?  CustomRevokeChargingTicketsDataSerializer   = null,
                              CustomJObjectSerializerDelegate<Signature>?                  CustomSignatureSerializer             = null)
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

            return CustomRevokeChargingTicketsDataSerializer is not null
                       ? CustomRevokeChargingTicketsDataSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RevokeChargingTicketsData1, RevokeChargingTicketsData2)

        /// <summary>
        /// Compares two RevokeChargingTicketsData requests for equality.
        /// </summary>
        /// <param name="RevokeChargingTicketsData1">A RevokeChargingTicketsData request.</param>
        /// <param name="RevokeChargingTicketsData2">Another RevokeChargingTicketsData request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RevokeChargingTicketsData? RevokeChargingTicketsData1,
                                           RevokeChargingTicketsData? RevokeChargingTicketsData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RevokeChargingTicketsData1, RevokeChargingTicketsData2))
                return true;

            // If one is null, but not both, return false.
            if (RevokeChargingTicketsData1 is null || RevokeChargingTicketsData2 is null)
                return false;

            return RevokeChargingTicketsData1.Equals(RevokeChargingTicketsData2);

        }

        #endregion

        #region Operator != (RevokeChargingTicketsData1, RevokeChargingTicketsData2)

        /// <summary>
        /// Compares two RevokeChargingTicketsData requests for inequality.
        /// </summary>
        /// <param name="RevokeChargingTicketsData1">A RevokeChargingTicketsData request.</param>
        /// <param name="RevokeChargingTicketsData2">Another RevokeChargingTicketsData request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RevokeChargingTicketsData? RevokeChargingTicketsData1,
                                           RevokeChargingTicketsData? RevokeChargingTicketsData2)

            => !(RevokeChargingTicketsData1 == RevokeChargingTicketsData2);

        #endregion

        #endregion

        #region IEquatable<RevokeChargingTicketsData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two RevokeChargingTicketsData requests for equality.
        /// </summary>
        /// <param name="Object">A RevokeChargingTicketsData request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RevokeChargingTicketsData registerEMobilityAccountData &&
                   Equals(registerEMobilityAccountData);

        #endregion

        #region Equals(RevokeChargingTicketsData)

        /// <summary>
        /// Compares two RevokeChargingTicketsData requests for equality.
        /// </summary>
        /// <param name="RevokeChargingTicketsData">A RevokeChargingTicketsData request to compare with.</param>
        public Boolean Equals(RevokeChargingTicketsData? RevokeChargingTicketsData)

            => RevokeChargingTicketsData is not null;

               //String.Equals(NotBefore, RevokeChargingTicketsData.NotBefore, StringComparison.OrdinalIgnoreCase);

             //((MessageId is     null && RevokeChargingTicketsData.MessageId is     null) ||
             // (MessageId is not null && RevokeChargingTicketsData.MessageId is not null && MessageId.Equals(RevokeChargingTicketsData.MessageId))) &&

             //((Data      is     null && RevokeChargingTicketsData.Data      is     null) ||
             // (Data      is not null && RevokeChargingTicketsData.Data      is not null && Data.     Equals(RevokeChargingTicketsData.Data)))      &&

             //  base.GenericEquals(RevokeChargingTicketsData);

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
