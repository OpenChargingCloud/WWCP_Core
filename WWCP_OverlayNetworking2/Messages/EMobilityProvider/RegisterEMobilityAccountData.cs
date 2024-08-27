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
    /// The RegisterEMobilityAccount data.
    /// </summary>
    public class RegisterEMobilityAccountData
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

        public String                           Username                    { get; }
        public String                           Password                    { get; }
        public SimpleEMailAddress               EMailAddress                { get; }
        public IEnumerable<PublicKey>           PublicKeys                  { get; }
        public IEnumerable<Signature>           Signatures                  { get; }
        public IEnumerable<SimpleEMailAddress>  AdditionalEMailAddresses    { get; }
        public PhoneNumber?                     PhoneNumber                 { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RegisterEMobilityAccount data.
        /// </summary>
        public RegisterEMobilityAccountData(String                            Username,
                                            String                            Password,
                                            SimpleEMailAddress                EMailAddress,
                                            IEnumerable<PublicKey>            PublicKeys,
                                            IEnumerable<Signature>            Signatures,
                                            IEnumerable<SimpleEMailAddress>?  AdditionalEMailAddresses   = null,
                                            PhoneNumber?                      PhoneNumber                = null)
        {

            this.Username                  = Username;
            this.Password                  = Password;
            this.EMailAddress              = EMailAddress;
            this.PublicKeys                = PublicKeys;
            this.Signatures                = Signatures;
            this.AdditionalEMailAddresses  = AdditionalEMailAddresses?.Distinct() ?? [];
            this.PhoneNumber               = PhoneNumber;

            unchecked
            {

                hashCode = this.Username.                GetHashCode()  * 17 ^
                           this.Password.                GetHashCode()  * 13 ^
                           this.EMailAddress.            GetHashCode()  * 11 ^
                           this.PublicKeys.              CalcHashCode() *  7 ^
                           this.Signatures.              CalcHashCode() *  5 ^
                           this.AdditionalEMailAddresses.CalcHashCode() *  3 ^
                           this.PhoneNumber?.            GetHashCode() ?? 0;

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse   (JSON, CustomRegisterEMobilityAccountDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of RegisterEMobilityAccount data.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomRegisterEMobilityAccountDataParser">A delegate to parse custom RegisterEMobilityAccountData JSON objects.</param>
        public static RegisterEMobilityAccountData Parse(JObject                                                     JSON,
                                                         CustomJObjectParserDelegate<RegisterEMobilityAccountData>?  CustomRegisterEMobilityAccountDataParser   = null)
        {

            if (TryParse(JSON,
                         out var registerEMobilityAccountData,
                         out var errorResponse,
                         CustomRegisterEMobilityAccountDataParser))
            {
                return registerEMobilityAccountData;
            }

            throw new ArgumentException("The given JSON representation of a RegisterEMobilityAccountData request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out RegisterEMobilityAccountData, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given JSON representation of RegisterEMobilityAccountData.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RegisterEMobilityAccountData">The parsed RegisterEMobilityAccountData.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRegisterEMobilityAccountDataParser">A delegate to parse custom RegisterEMobilityAccountData requests.</param>
        public static Boolean TryParse(JObject                                                     JSON,
                                       [NotNullWhen(true)]  out RegisterEMobilityAccountData?      RegisterEMobilityAccountData,
                                       [NotNullWhen(false)] out String?                            ErrorResponse,
                                       CustomJObjectParserDelegate<RegisterEMobilityAccountData>?  CustomRegisterEMobilityAccountDataParser   = null)
        {

            try
            {

                RegisterEMobilityAccountData = null;

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
                                                PublicKey.TryParse,
                                                out HashSet<PublicKey> PublicKeys,
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


                RegisterEMobilityAccountData = new RegisterEMobilityAccountData(
                                                   Username,
                                                   Password,
                                                   EMailAddress,
                                                   PublicKeys,
                                                   Signatures,
                                                   AdditionalEMailAddresses,
                                                   PhoneNumber
                                               );

                if (CustomRegisterEMobilityAccountDataParser is not null)
                    RegisterEMobilityAccountData = CustomRegisterEMobilityAccountDataParser(JSON,
                                                                                            RegisterEMobilityAccountData);

                return true;

            }
            catch (Exception e)
            {
                RegisterEMobilityAccountData  = null;
                ErrorResponse                 = "The given JSON representation of a RegisterEMobilityAccountData request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRegisterEMobilityAccountDataSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRegisterEMobilityAccountDataSerializer">A delegate to serialize custom RegisterEMobilityAccountData requests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RegisterEMobilityAccountData>?  CustomRegisterEMobilityAccountDataSerializer   = null,
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

            return CustomRegisterEMobilityAccountDataSerializer is not null
                       ? CustomRegisterEMobilityAccountDataSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RegisterEMobilityAccountData1, RegisterEMobilityAccountData2)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData requests for equality.
        /// </summary>
        /// <param name="RegisterEMobilityAccountData1">A RegisterEMobilityAccountData request.</param>
        /// <param name="RegisterEMobilityAccountData2">Another RegisterEMobilityAccountData request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RegisterEMobilityAccountData? RegisterEMobilityAccountData1,
                                           RegisterEMobilityAccountData? RegisterEMobilityAccountData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RegisterEMobilityAccountData1, RegisterEMobilityAccountData2))
                return true;

            // If one is null, but not both, return false.
            if (RegisterEMobilityAccountData1 is null || RegisterEMobilityAccountData2 is null)
                return false;

            return RegisterEMobilityAccountData1.Equals(RegisterEMobilityAccountData2);

        }

        #endregion

        #region Operator != (RegisterEMobilityAccountData1, RegisterEMobilityAccountData2)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData requests for inequality.
        /// </summary>
        /// <param name="RegisterEMobilityAccountData1">A RegisterEMobilityAccountData request.</param>
        /// <param name="RegisterEMobilityAccountData2">Another RegisterEMobilityAccountData request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RegisterEMobilityAccountData? RegisterEMobilityAccountData1,
                                           RegisterEMobilityAccountData? RegisterEMobilityAccountData2)

            => !(RegisterEMobilityAccountData1 == RegisterEMobilityAccountData2);

        #endregion

        #endregion

        #region IEquatable<RegisterEMobilityAccountData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData requests for equality.
        /// </summary>
        /// <param name="Object">A RegisterEMobilityAccountData request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RegisterEMobilityAccountData registerEMobilityAccountData &&
                   Equals(registerEMobilityAccountData);

        #endregion

        #region Equals(RegisterEMobilityAccountData)

        /// <summary>
        /// Compares two RegisterEMobilityAccountData requests for equality.
        /// </summary>
        /// <param name="RegisterEMobilityAccountData">A RegisterEMobilityAccountData request to compare with.</param>
        public Boolean Equals(RegisterEMobilityAccountData? RegisterEMobilityAccountData)

            => RegisterEMobilityAccountData is not null &&

               String.Equals(Username, RegisterEMobilityAccountData.Username, StringComparison.OrdinalIgnoreCase);

             //((MessageId is     null && RegisterEMobilityAccountData.MessageId is     null) ||
             // (MessageId is not null && RegisterEMobilityAccountData.MessageId is not null && MessageId.Equals(RegisterEMobilityAccountData.MessageId))) &&

             //((Data      is     null && RegisterEMobilityAccountData.Data      is     null) ||
             // (Data      is not null && RegisterEMobilityAccountData.Data      is not null && Data.     Equals(RegisterEMobilityAccountData.Data)))      &&

             //  base.GenericEquals(RegisterEMobilityAccountData);

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

    public class RegisterEMobilityAccountData2 : RegisterEMobilityAccountData
    {

        public User_Id UserId { get; }

        public RegisterEMobilityAccountData2(User_Id                       UserId,
                                             RegisterEMobilityAccountData  RegisterEMobilityAccountData)

            : base(RegisterEMobilityAccountData.Username,
                   RegisterEMobilityAccountData.Password,
                   RegisterEMobilityAccountData.EMailAddress,
                   RegisterEMobilityAccountData.PublicKeys,
                   RegisterEMobilityAccountData.Signatures,
                   RegisterEMobilityAccountData.AdditionalEMailAddresses,
                   RegisterEMobilityAccountData.PhoneNumber)

        {

            this.UserId = UserId;

        }


    }



}
