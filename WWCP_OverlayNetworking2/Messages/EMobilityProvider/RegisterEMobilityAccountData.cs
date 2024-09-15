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
using System.Security.Cryptography;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Security;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Mail;

using cloud.charging.open.protocols.OCPPv2_1;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;

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

        public RegisterEMobilityAccountData_Id  Id                          { get; }
        public DateTime                         CreationTimestamp           { get; }
        public String                           Username                    { get; }
        public String                           Password                    { get; }
        public SimpleEMailAddress               EMailAddress                { get; }
        public IEnumerable<ECCPublicKey>        PublicKeys                  { get; }
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
                                            IEnumerable<ECCPublicKey>         PublicKeys,
                                            IEnumerable<Signature>?           Signatures                 = null,
                                            IEnumerable<SimpleEMailAddress>?  AdditionalEMailAddresses   = null,
                                            PhoneNumber?                      PhoneNumber                = null,
                                            RegisterEMobilityAccountData_Id?  Id                         = null,
                                            DateTime?                         CreationTimestamp          = null)
        {

            this.Id                        = Id                                   ?? RegisterEMobilityAccountData_Id.Random();
            this.CreationTimestamp         = CreationTimestamp                    ?? Timestamp.Now;
            this.Username                  = Username;
            this.Password                  = Password;
            this.EMailAddress              = EMailAddress;
            this.PublicKeys                = PublicKeys;
            this.Signatures                = Signatures?.              Distinct() ?? [];
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

                #region Id                          [mandatory]

                if (!JSON.ParseMandatory("@id",
                                         "RegisterEMobilityAccountData identification",
                                         RegisterEMobilityAccountData_Id.TryParse,
                                         out RegisterEMobilityAccountData_Id Id,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Context               [mandatory]

                if (!JSON.ParseMandatory("@context",
                                         "JSON-LinkedData context information",
                                         JSONLDContext.TryParse,
                                         out JSONLDContext Context,
                                         out ErrorResponse))
                {
                    ErrorResponse = $"The JSON-LD \"@context\" information is missing!";
                    return false;
                }

                if (Context != DefaultJSONLDContext)
                {
                    ErrorResponse = $"The given JSON-LD \"@context\" information '{Context}' is not supported!";
                    return false;
                }

                #endregion

                #region CreationTimestamp           [mandatory]

                if (!JSON.ParseMandatory("creationTimestamp",
                                         "creation timestamp",
                                         out DateTime CreationTimestamp,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

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

                #region Signatures                  [optional]

                if (JSON.ParseOptionalHashSet("signatures",
                                              "signatures",
                                              Signature.TryParse,
                                              out HashSet<Signature> Signatures,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
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
                                                   PhoneNumber,
                                                   Id,
                                                   CreationTimestamp
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
        public JObject ToJSON(Boolean                                                         Embedded                                       = false,
                              CustomJObjectSerializerDelegate<RegisterEMobilityAccountData>?  CustomRegisterEMobilityAccountDataSerializer   = null,
                              CustomJObjectSerializerDelegate<PublicKey>?                     CustomPublicKeySerializer                      = null,
                              CustomJObjectSerializerDelegate<Signature>?                     CustomSignatureSerializer                      = null,
                              CustomJObjectSerializerDelegate<CustomData>?                    CustomCustomDataSerializer                     = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("@id",                        Id.               ToString()),

                           Embedded
                               ? null
                               : new JProperty("@context",                   Context.          ToString()),

                                 new JProperty("creationTimestamp",          CreationTimestamp.ToIso8601()),
                                 new JProperty("username",                   Username),
                                 new JProperty("password",                   Password),
                                 new JProperty("eMailAddress",               EMailAddress.     ToString()),
                                 new JProperty("publicKeys",                 new JArray(PublicKeys.              Select(publicKey          => publicKey.         ToJSON  (CryptoSerialization.RAW,
                                                                                                                                                                          true,
                                                                                                                                                                          CustomPublicKeySerializer,
                                                                                                                                                                          CustomCustomDataSerializer)))),

                           Signatures.Any()
                               ? new JProperty("signatures",                 new JArray(Signatures.              Select(signature          => signature.         ToJSON  (CustomSignatureSerializer,
                                                                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           AdditionalEMailAddresses.Any()
                               ? new JProperty("additionalEMailAddresses",   new JArray(AdditionalEMailAddresses.Select(simpleEMailAddress => simpleEMailAddress.ToString())))
                               : null,

                           PhoneNumber.HasValue
                               ? new JProperty("phoneNumber",                PhoneNumber.Value.ToString())
                               : null

                       );

            return CustomRegisterEMobilityAccountDataSerializer is not null
                       ? CustomRegisterEMobilityAccountDataSerializer(this, json)
                       : json;

        }

        #endregion


        #region Sign(Keys)

        public RegisterEMobilityAccountData Sign(IEnumerable<ECCKeyPair> Keys)
        {

            var signatures    = new List<Signature>();
            var cryptoHashes  = new Dictionary<Int32, Byte[]>();

            var jsonCopy      = JObject.Parse(
                                    ToJSON(
                                        Embedded: false
                                    ).
                                    ToString(
                                        Formatting.None,
                                        SignableMessage.DefaultJSONConverters
                                    )
                                );
            jsonCopy.Remove("signatures");

            var plainText     = jsonCopy.ToString(Formatting.None, SignableMessage.DefaultJSONConverters);

            foreach (var key in Keys)
            {

                if (key.PrivateKey is not null)
                {

                    var blockSize = key.Algorithm switch {
                                        var s when s == CryptoAlgorithm.Secp256r1  => 32,
                                        var s when s == CryptoAlgorithm.Secp384r1  => 48,
                                        var s when s == CryptoAlgorithm.Secp521r1  => 64,
                                        _                                          => throw new Exception("Unknown key algorithm: " + key.Algorithm)
                                    };

                    if (!cryptoHashes.ContainsKey(blockSize))
                        cryptoHashes.TryAdd(
                            blockSize,
                            key.Algorithm switch {
                                var s when s == CryptoAlgorithm.Secp256r1  => SHA256.HashData(plainText.ToUTF8Bytes()),
                                var s when s == CryptoAlgorithm.Secp384r1  => SHA384.HashData(plainText.ToUTF8Bytes()),
                                var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(plainText.ToUTF8Bytes()),
                                _                                          => throw new Exception("Unknown key algorithm: " + key.Algorithm)
                            }
                        );

                    var signer = SignerUtilities.GetSigner("NONEwithECDSA");
                    signer.Init(true, key.PrivateKey);
                    signer.BlockUpdate(cryptoHashes[blockSize], 0, blockSize);

                    signatures.Add(
                        new Signature(
                            key.PublicKeyBytes,
                            signer.GenerateSignature(),
                            key.Algorithm,
                            CryptoSigningMethod.JSON,
                            CryptoEncoding.BASE64
                        )
                    );

                }

            }

            return new RegisterEMobilityAccountData(
                       Username,
                       Password,
                       EMailAddress,
                       PublicKeys,
                       signatures,
                       AdditionalEMailAddresses,
                       PhoneNumber,
                       Id,
                       CreationTimestamp
                   );

        }

        #endregion

        #region Verify(JSONData,   Context, out ErrorResponse, VerificationRuleAction = VerificationRuleActions.VerifyAll)

        /// <summary>
        /// Verify the given JSON data structure.
        /// </summary>
        /// <param name="JSONData">The JSON representation of the signable/verifiable data.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean Verify(JObject        JSONData,
                              JSONLDContext  Context,
                              out String?    ErrorResponse)
        {

            ErrorResponse = null;

            var signatures = JSONData["signatures"] as JArray;
            if (signatures is null || !signatures.Any())
            {
                ErrorResponse = "No digital signatures found!";
                return false;
            }

            try
            {

                //if (JSONData["@context"] is null)
                //    JSONData.AddFirst(new JProperty("@context", Context.ToString()));

                var jsonCopy      = JObject.Parse(
                                        JSONData.ToString(
                                            Formatting.None,
                                            SignableMessage.DefaultJSONConverters
                                        )
                                    );

                jsonCopy.Remove("signatures");

                var plainText     = jsonCopy.ToString(Formatting.None, SignableMessage.DefaultJSONConverters);
                var cryptoHashes  = new Dictionary<Int32, Byte[]>();

                foreach (var signature in Signatures)
                {

                    var ecp           = signature.Algorithm switch {
                                            var s when s == CryptoAlgorithm.Secp256r1  => SecNamedCurves.GetByName("secp256r1"),
                                            var s when s == CryptoAlgorithm.Secp384r1  => SecNamedCurves.GetByName("secp384r1"),
                                            var s when s == CryptoAlgorithm.Secp521r1  => SecNamedCurves.GetByName("secp521r1"),
                                            _                                          => throw new Exception("Unknown signature algorithm: " + signature.Algorithm)
                    };

                    var ecParams      = new ECDomainParameters   (ecp.Curve, ecp.G, ecp.N, ecp.H, ecp.GetSeed());
                    var pubKeyParams  = new ECPublicKeyParameters("ECDSA", ecParams.Curve.DecodePoint(signature.KeyId), ecParams);

                    var blockSize     = signature.Algorithm switch {
                                            var s when s == CryptoAlgorithm.Secp256r1  => 32,
                                            var s when s == CryptoAlgorithm.Secp384r1  => 48,
                                            var s when s == CryptoAlgorithm.Secp521r1  => 64,
                                            _                                          => throw new Exception("Unknown key algorithm: " + signature.Algorithm)
                                        };

                    if (!cryptoHashes.ContainsKey(blockSize))
                        cryptoHashes.TryAdd(
                            blockSize,
                            signature.Algorithm switch {
                                var s when s == CryptoAlgorithm.Secp256r1  => SHA256.HashData(plainText.ToUTF8Bytes()),
                                var s when s == CryptoAlgorithm.Secp384r1  => SHA384.HashData(plainText.ToUTF8Bytes()),
                                var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(plainText.ToUTF8Bytes()),
                                _                                          => throw new Exception("Unknown signature algorithm: " + signature.Algorithm)
                            }
                        );

                    var verifier      = SignerUtilities.GetSigner("NONEwithECDSA");
                    verifier.Init(false, pubKeyParams);
                    verifier.BlockUpdate(cryptoHashes[blockSize]);
                    signature.Status  = verifier.VerifySignature(signature.Value)
                                            ? VerificationStatus.ValidSignature
                                            : VerificationStatus.InvalidSignature;

                }

                return Signatures.All(signature => signature.Status == VerificationStatus.ValidSignature);

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

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
