/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An OCPP CSE cryptographic signature policy.
    /// </summary>
    public class SignaturePolicy : ACustomSignableData,
                                   ISignableMessage,
                                   IEquatable<SignaturePolicy>
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this data structure.
        /// </summary>
        public  static readonly JSONLDContext              DefaultJSONLDContext   = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/signaturePolicy");

        private        readonly HashSet<SigningRule>       signingRules           = new();
        private        readonly HashSet<VerificationRule>  verificationRules      = new();
        private        readonly HashSet<KeyPair>           keyPairs               = new();

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of this signature policy.
        /// </summary>
        [Mandatory]
        public SignaturePolicy_Id?            Id                            { get; }

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        [Mandatory]
        public JSONLDContext                  Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The priority of this signature policy.
        /// </summary>
        [Mandatory]
        public Byte                           Priority                      { get; }

        /// <summary>
        /// The timestamp before which the signature policy should not be used.
        /// </summary>
        [Mandatory]
        public DateTimeOffset                 NotBefore                     { get; }

        /// <summary>
        /// The optional timestamp after which the signature policy should not be used.
        /// </summary>
        [Optional]
        public DateTimeOffset?                NotAfter                      { get; }


        /// <summary>
        /// The enumeration of signature policy entries.
        /// </summary>
        [Mandatory]
        public IEnumerable<SigningRule>       SignaturePolicyEntries
            => signingRules;

        /// <summary>
        /// The enumeration of signature verification policy entries.
        /// </summary>
        [Mandatory]
        public IEnumerable<VerificationRule>  VerificationPolicyEntries
            => verificationRules;

        /// <summary>
        /// The enumeration of cryptographic key pairs.
        /// </summary>
        [Mandatory]
        public IEnumerable<KeyPair>           KeyPairs
            => keyPairs;


        /// <summary>
        /// The default signature action.
        /// </summary>
        [Mandatory]
        public SigningRuleActions             DefaultSigningAction          { get; }

        /// <summary>
        /// The optional default cryptographic signing key pair.
        /// </summary>
        [Optional]
        public KeyPair?                       DefaultSigningKeyPair         { get; }


        /// <summary>
        /// The default verification action.
        /// </summary>
        [Mandatory]
        public VerificationRuleActions        DefaultVerificationAction     { get; }

        /// <summary>
        /// The optional default cryptographic verification key pair.
        /// </summary>
        [Optional]
        public KeyPair?                       DefaultVerificationKeyPair    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCPP CSE cryptographic signature policy.
        /// </summary>
        /// <param name="Id">An optional unique identification for this signature policy.</param>
        /// <param name="Priority">An optional priority of this signature policy.</param>
        /// <param name="NotBefore">An optional timestamp before which the signature policy should not be used.</param>
        /// <param name="NotAfter">An optional timestamp after which the signature policy should not be used.</param>
        /// 
        /// <param name="SigningRules">An optional enumeration of cryptographic signing rules.</param>
        /// <param name="DefaultSigningAction">An optional default signing action for this policy.</param>
        /// <param name="DefaultSigningKeyPair">An optional default cryptographic signing key pair.</param>
        /// 
        /// <param name="VerificationRules">An optional enumeration of cryptographic verification rules.</param>
        /// <param name="DefaultVerificationAction">An optional default verification action for this policy.</param>
        /// <param name="DefaultSigningKeyPair">An optional default cryptographic signing key pair.</param>
        /// 
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this message.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this message.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures.</param>
        /// 
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public SignaturePolicy(SignaturePolicy_Id?             Id                           = null,
                               Byte?                           Priority                     = null,
                               DateTimeOffset?                 NotBefore                    = null,
                               DateTimeOffset?                 NotAfter                     = null,

                               IEnumerable<SigningRule>?       SigningRules                 = null,
                               SigningRuleActions?             DefaultSigningAction         = null,
                               KeyPair?                        DefaultSigningKeyPair        = null,

                               IEnumerable<VerificationRule>?  VerificationRules            = null,
                               VerificationRuleActions?        DefaultVerificationAction    = null,
                               KeyPair?                        DefaultVerificationKeyPair   = null,

                               IEnumerable<KeyPair>?           SignKeys                     = null,
                               IEnumerable<SignInfo>?          SignInfos                    = null,
                               IEnumerable<Signature>?         Signatures                   = null,

                               CustomData?                     CustomData                   = null)

            : base(SignKeys,
                   SignInfos,
                   Signatures,
                   CustomData)

        {

            this.Id                          = Id        ?? SignaturePolicy_Id.NewRandom();
            this.Priority                    = Priority  ?? 1;
            this.NotBefore                   = NotBefore ?? Timestamp.Now;
            this.NotAfter                    = NotAfter;

            // Signing
            if (SigningRules is not null)
                foreach (var signingRule      in SigningRules)
                    signingRules.   Add(signingRule);

            this.DefaultSigningAction        = DefaultSigningAction      ?? SigningRuleActions.ForwardUnsigned;
            this.DefaultSigningKeyPair       = DefaultSigningKeyPair;

            if (this.DefaultSigningAction == SigningRuleActions.Sign &&
                this.DefaultSigningKeyPair is null)
            {
                throw new ArgumentException("If the default action is 'sign', a default signing key pair must be provided!");
            }


            // Verification
            if (VerificationRules is not null)
                foreach (var verificationRule in VerificationRules)
                    verificationRules.Add(verificationRule);

            this.DefaultVerificationAction   = DefaultVerificationAction ?? VerificationRuleActions.AcceptUnverified;
            this.DefaultVerificationKeyPair  = DefaultVerificationKeyPair;

            if ((this.DefaultVerificationAction == VerificationRuleActions.VerifyAny ||
                 this.DefaultVerificationAction == VerificationRuleActions.VerifyAll) &&
                 this.DefaultVerificationKeyPair is null)
            {
                throw new ArgumentException("If the default action is 'VerifyAny' or 'VerifyAll', a default verification key pair must be provided!");
            }


            unchecked
            {

                hashCode = //KeyId.          GetHashCode()       * 11 ^
                //           Value.          GetHashCode()       *  7 ^
                //          (SigningMethod?. GetHashCode() ?? 0) *  5 ^
                //          (EncodingMethod?.GetHashCode() ?? 0) *  3 ^
                           base.           GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse   (JSON, CustomSignaturePolicyParser = null)

        /// <summary>
        /// Parse the given JSON representation of a signature policy.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomSignaturePolicyParser">An optional delegate to parse custom signature policies.</param>
        public static SignaturePolicy Parse(JObject                                        JSON,
                                            CustomJObjectParserDelegate<SignaturePolicy>?  CustomSignaturePolicyParser   = null)
        {

            if (TryParse(JSON,
                         out var signaturePolicy,
                         out var errorResponse,
                         CustomSignaturePolicyParser) &&
                signaturePolicy is not null)
            {
                return signaturePolicy;
            }

            throw new ArgumentException("The given JSON representation of a signature policy is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out SignaturePolicy, out ErrorResponse, CustomSignaturePolicyParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a signature policy.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="SignaturePolicy">The parsed connector type.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out SignaturePolicy?  SignaturePolicy,
                                       [NotNullWhen(false)] out String?           ErrorResponse)

            => TryParse(JSON,
                        out SignaturePolicy,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a signature policy.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="SignaturePolicy">The parsed connector type.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomSignaturePolicyParser">An optional delegate to parse custom signature policies.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out SignaturePolicy?      SignaturePolicy,
                                       [NotNullWhen(false)] out String?               ErrorResponse,
                                       CustomJObjectParserDelegate<SignaturePolicy>?  CustomSignaturePolicyParser)
        {

            try
            {

                SignaturePolicy = default;

                #region Id                   [mandatory]

                if (!JSON.ParseMandatory("id",
                                         "signature policy identification",
                                         SignaturePolicy_Id.TryParse,
                                         out SignaturePolicy_Id Id,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Priority             [mandatory]

                if (!JSON.ParseMandatory("priority",
                                         "signature policy priority",
                                         out Byte Priority,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region NotBefore            [mandatory]

                if (!JSON.ParseMandatory("notBefore",
                                         "start schedule",
                                         out DateTimeOffset NotBefore,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region NotAfter             [optional]

                if (JSON.ParseOptional("notAfter",
                                       "start schedule",
                                       out DateTimeOffset? NotAfter,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                       return false;
                }

                #endregion


                #region SigningRules         [optional]

                if (JSON.ParseOptionalHashSet("signingRules",
                                              "signing rules",
                                              SigningRule.TryParse,
                                              out HashSet<SigningRule> SigningRules,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region VerificationRules    [optional]

                if (JSON.ParseOptionalHashSet("verificationRules",
                                              "verification rules",
                                              VerificationRule.TryParse,
                                              out HashSet<VerificationRule> VerificationRules,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Signatures           [optional, OCPP_CSE]

                if (JSON.ParseOptionalHashSet("signatures",
                                              "cryptographic signatures",
                                              Signature.TryParse,
                                              out HashSet<Signature> Signatures,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CustomData           [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           WWCP.CustomData.TryParse,
                                           out CustomData CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                SignaturePolicy = new SignaturePolicy(

                                      Id,
                                      Priority,
                                      NotBefore,
                                      NotAfter,

                                      SigningRules,
                                      null,  // DefaultSigningAction
                                      null,  // DefaultSigningKeyPair

                                      VerificationRules,
                                      null,  // DefaultVerificationAction
                                      null,  // DefaultVerificationKeyPair

                                      null,
                                      null,
                                      Signatures,

                                      CustomData

                                  );

                if (CustomSignaturePolicyParser is not null)
                    SignaturePolicy = CustomSignaturePolicyParser(JSON,
                                                                  SignaturePolicy);

                return true;

            }
            catch (Exception e)
            {
                SignaturePolicy  = default;
                ErrorResponse    = "The given JSON representation of a signature policy is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSignaturePolicySerializer = null, CustomEventDataSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomSignaturePolicySerializer">A delegate to serialize custom signature policies.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<SignaturePolicy>?  CustomSignaturePolicySerializer   = null,
                              CustomJObjectSerializerDelegate<Signature>?        CustomSignatureSerializer         = null,
                              CustomJObjectSerializerDelegate<CustomData>?       CustomCustomDataSerializer        = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("id",            Id.ToString()),
                                 new JProperty("priority",      Priority),
                                 new JProperty("notBefore",     NotBefore.ToISO8601()),

                           NotAfter.HasValue
                               ? new JProperty("notAfter",      NotAfter.Value.ToISO8601())
                               : null,



                           Signatures.Any()
                               ? new JProperty("signatures",    new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                           CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",    CustomData.ToJSON(CustomCustomDataSerializer))
                               : null);

            return CustomSignaturePolicySerializer is not null
                       ? CustomSignaturePolicySerializer(this, json)
                       : json;

        }

        #endregion


        #region Signing...

        #region AddSigningRule (...)

        public SignaturePolicy AddSigningRule(JSONLDContext                            Context,
                                              KeyPair                                  KeyPair,
                                              Func<ISignableMessage, String>?          UserIdGenerator        = null,
                                              Func<ISignableMessage, I18NString>?      DescriptionGenerator   = null,
                                              Func<ISignableMessage, DateTimeOffset>?  TimestampGenerator     = null)
        {

            lock (signingRules)
            {

                signingRules.Add(new SigningRule(
                                     Context,
                                     signingRules.Count > 0
                                         ? signingRules.Max(entry => entry.Priority) + 1
                                         : 1,
                                     SigningRuleActions.Sign,
                                     KeyPair,
                                     UserIdGenerator,
                                     DescriptionGenerator,
                                     TimestampGenerator
                                 ));

                return this;

            }

        }

        #endregion

        #region AddSigningRule (Priority, ...)

        public SignaturePolicy AddSigningRule(UInt32                                   Priority,
                                              JSONLDContext                            Context,
                                              KeyPair                                  KeyPair,
                                              Func<ISignableMessage, String>?          UserIdGenerator        = null,
                                              Func<ISignableMessage, I18NString>?      DescriptionGenerator   = null,
                                              Func<ISignableMessage, DateTimeOffset>?  TimestampGenerator     = null)
        {

            lock (signingRules)
            {

                signingRules.Add(new SigningRule(
                                     Context,
                                     Priority,
                                     SigningRuleActions.Sign,
                                     KeyPair,
                                     UserIdGenerator,
                                     DescriptionGenerator,
                                     TimestampGenerator
                                 ));

                return this;

            }

        }

        #endregion

        #region AddSigningRules(...)

        public SignaturePolicy AddSigningRules(IEnumerable<SigningRule> SigningRules)
        {

            lock (signingRules)
            {

                foreach (var signingRule in SigningRules)
                    signingRules.Add(signingRule);

                return this;

            }

        }

        #endregion


        #region (private) MatchingSigningRules(Context)

        /// <summary>
        /// The signing rules for messages of the given JSON-LD context, exactly
        /// or by a pattern ending in "...", the rule of highest priority first.
        /// </summary>
        /// <param name="Context">The JSON-LD context of a message.</param>
        private List<SigningRule> MatchingSigningRules(JSONLDContext Context)
        {

            lock (signingRules)
            {
                return [.. signingRules.Where            (signingRule => Context.Matches(signingRule.Context)).
                                        OrderByDescending(signingRule => signingRule.Priority)];
            }

        }

        #endregion

        #region GetSigningRules(...)

        /// <summary>
        /// The signing rules for messages of the given JSON-LD context, the rule
        /// of highest priority first, or the default rule when none matches.
        /// </summary>
        /// <param name="Context">The JSON-LD context of a message.</param>
        public IEnumerable<SigningRule> GetSigningRules(JSONLDContext Context)
        {

            var rules = MatchingSigningRules(Context);

            if (rules.Count > 0)
                return rules;

            return [
                       new SigningRule(
                           JSONLDContext.Parse("default"),
                           0,
                           DefaultSigningAction,
                           DefaultSigningKeyPair
                       )
                   ];

        }

        #endregion

        #region HasSigningRules(Context, out SigningRules)

        public Boolean HasSigningRules(JSONLDContext             Context,
                                       out HashSet<SigningRule>  SigningRules)
        {

            SigningRules = [.. MatchingSigningRules(Context)];

            return SigningRules.Count > 0;

        }

        #endregion

        #region (private) TryGetSignInfos(SignableMessage, SignInfos, out AllSignInfos, out ErrorResponse)

        /// <summary>
        /// Which keys sign the given message, or why it must not be sent at all.
        /// </summary>
        /// <remarks>
        /// The matching signing rule of highest priority decides what the policy
        /// does with the message, and the default signing action does when no
        /// rule matches. "Sign" signs it with the key of every matching rule that
        /// says "Sign", or with the default key. "ForwardUnsigned" adds no key of
        /// the policy. "Drop" and "Reject" refuse the message, also one whose
        /// caller named keys of its own. In every other case the keys that the
        /// caller and the message name sign as well.
        /// </remarks>
        /// <param name="SignableMessage">A signable message.</param>
        /// <param name="SignInfos">The keys the caller named.</param>
        /// <param name="AllSignInfos">Every key that signs the message.</param>
        /// <param name="ErrorResponse">Why the message must not be sent.</param>
        private Boolean TryGetSignInfos(ISignableMessage                  SignableMessage,
                                        SignInfo[]?                       SignInfos,
                                        out List<SignInfo>                AllSignInfos,
                                        [NotNullWhen(false)] out String?  ErrorResponse)
        {

            AllSignInfos  = [];

            var rules     = MatchingSigningRules(SignableMessage.Context);
            var action    = rules.Count > 0
                                ? rules[0].Action
                                : DefaultSigningAction;

            if (action == SigningRuleActions.Drop ||
                action == SigningRuleActions.Reject)
            {
                ErrorResponse = $"The signature policy {(action == SigningRuleActions.Drop ? "drops" : "rejects")} '{SignableMessage.Context}' messages!";
                return false;
            }

            if (SignInfos                 is not null)
                AllSignInfos.AddRange(SignInfos);

            if (SignableMessage.SignKeys  is not null)
                AllSignInfos.AddRange(SignableMessage.SignKeys.Select(keyPair => keyPair.ToSignInfo1()));

            if (SignableMessage.SignInfos is not null)
                AllSignInfos.AddRange(SignableMessage.SignInfos);

            if (action == SigningRuleActions.Sign)
            {

                if (rules.Count == 0)
                {

                    // The constructor refuses a default action "Sign" without a default key.
                    if (DefaultSigningKeyPair is not null)
                        AllSignInfos.Add(DefaultSigningKeyPair.ToSignInfo1());

                }

                else
                    foreach (var rule in rules.Where(rule => rule.Action == SigningRuleActions.Sign))
                    {

                        var signInfo = rule.ToSignInfo();

                        if (signInfo is null)
                        {
                            ErrorResponse = $"The signing rule for '{rule.Context}' has no key pair to sign with!";
                            return false;
                        }

                        AllSignInfos.Add(signInfo);

                    }

            }

            ErrorResponse = null;
            return true;

        }

        #endregion

        #region ClearSigningRules  ()

        /// <summary>
        /// Remove all signing rules, e.g. to reset the signature policy within tests.
        /// </summary>
        public SignaturePolicy ClearSigningRules()
        {

            lock (signingRules)
            {
                signingRules.Clear();
                return this;
            }

        }

        #endregion


        #region SignMessage         (SignableMessage, JSONMessage,   out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given message.
        /// </summary>
        /// <param name="SignableMessage">A signable message.</param>
        /// <param name="JSONMessage">The JSON representation of the signable message.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="SignInfos">An enumeration of cryptographic signature information or key pairs to sign the given message.</param>
        public Boolean SignMessage(ISignableMessage                  SignableMessage,
                                   JObject                           JSONMessage,
                                   [NotNullWhen(false)] out String?  ErrorResponse,
                                   params SignInfo[]                 SignInfos)
        {

            #region Initial checks

            if (JSONMessage is null)
            {
                ErrorResponse = "The given JSON message must not be null!";
                return false;
            }

            //if (SignInfos is null || !SignInfos.Any())
            //{
            //    ErrorResponse = "The given key pairs must not be null or empty!";
            //    return false;
            //}

            #endregion

            try
            {

                if (JSONMessage["@context"] is null)
                    JSONMessage.AddFirst(new JProperty("@context", SignableMessage.Context.ToString()));

                if (!TryGetSignInfos(SignableMessage, SignInfos, out var signInfos, out ErrorResponse))
                    return false;

                foreach (var signInfo in signInfos)
                {

                    #region Initial checks

                    if (signInfo is null)
                    {
                        ErrorResponse = "The given key pair must not be null!";
                        return false;
                    }


                    if (signInfo.PrivateKeyBytes is null || signInfo.PrivateKeyBytes.IsNullOrEmpty())
                    {
                        ErrorResponse = "The given key pair must contain a serialized private key!";
                        return false;
                    }

                    if (signInfo.PublicKeyBytes  is null || signInfo.PublicKeyBytes. IsNullOrEmpty())
                    {
                        ErrorResponse = "The given key pair must contain a serialized public key!";
                        return false;
                    }


                    if (signInfo.PrivateKey is null)
                    {
                        ErrorResponse = "The given key pair must contain a private key!";
                        return false;
                    }

                    if (signInfo.PublicKey is null)
                    {
                        ErrorResponse = "The given key pair must contain a public key!";
                        return false;
                    }

                    #endregion

                    var plainText   = JSONMessage.ToString(Formatting.None, WWCP.SignableMessage.DefaultJSONConverters);

                    var cryptoHash  = signInfo.Algorithm switch {
                                          var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(plainText.ToUTF8Bytes()),
                                          var s when s == CryptoAlgorithm.Secp384r1  => SHA512.HashData(plainText.ToUTF8Bytes()),
                                          _                                          => SHA256.HashData(plainText.ToUTF8Bytes()),
                                      };

                    var signer       = SignerUtilities.GetSigner("NONEwithECDSA");
                    signer.Init(true, signInfo.PrivateKey);
                    signer.BlockUpdate(cryptoHash);
                    var signature    = signer.GenerateSignature();

                    SignableMessage.AddSignature(
                        new Signature(
                            //KeyId:           SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(signInfo.PublicKey).PublicKey.GetBytes(),
                            KeyId:           signInfo.PublicKey.Q.GetEncoded(),
                            Value:           signature,
                            Algorithm:       signInfo.Algorithm,
                            SigningMethod:   null,
                            Encoding:        signInfo.Encoding,
                            Name:            signInfo.SignerName?. Invoke(SignableMessage),
                            Description:     signInfo.Description?.Invoke(SignableMessage),
                            Timestamp:       signInfo.Timestamp?.  Invoke(SignableMessage)
                        )
                    );

                }

                ErrorResponse = null;
                return true;

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion

        #region SignRequestMessage  (RequestMessage,  JSONMessage,   out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given request message.
        /// </summary>
        /// <param name="RequestMessage">The request message.</param>
        /// <param name="JSONMessage">The JSON representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of signing errors.</param>
        /// <param name="SignInfos">One or multiple cryptographic signature information or key pairs to sign the request message.</param>
        public Boolean SignRequestMessage(IRequest                          RequestMessage,
                                          JObject                           JSONMessage,
                                          [NotNullWhen(false)] out String?  ErrorResponse,
                                          params SignInfo[]                 SignInfos)

            => SignMessage(
                   RequestMessage,
                   JSONMessage,
                   out ErrorResponse,
                   SignInfos
               );

        #endregion

        #region SignResponseMessage (ResponseMessage, JSONMessage,   out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given response message.
        /// </summary>
        /// <param name="ResponseMessage">A response message.</param>
        /// <param name="JSONMessage">The JSON representation of the response message.</param>
        /// <param name="ErrorResponse">An optional error response in case of signing errors.</param>
        /// <param name="SignInfos">One or multiple cryptographic signature information or key pairs to sign the response message.</param>
        public Boolean SignResponseMessage(IResponse                         ResponseMessage,
                                           JObject                           JSONMessage,
                                           [NotNullWhen(false)] out String?  ErrorResponse,
                                           params SignInfo[]                 SignInfos)

            => SignMessage(
                   ResponseMessage,
                   JSONMessage,
                   out ErrorResponse,
                   SignInfos
               );

        #endregion

        #region SignSendMessage     (SendMessage,     JSONMessage,   out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given send message.
        /// </summary>
        /// <param name="SendMessage">The send message.</param>
        /// <param name="JSONMessage">The JSON representation of the send message.</param>
        /// <param name="ErrorResponse">An optional error response in case of signing errors.</param>
        /// <param name="SignInfos">One or multiple cryptographic signature information or key pairs to sign the send message.</param>
        public Boolean SignSendMessage(IMessage                          SendMessage,
                                       JObject                           JSONMessage,
                                       [NotNullWhen(false)] out String?  ErrorResponse,
                                       params SignInfo[]                 SignInfos)

            => SignMessage(
                   SendMessage,
                   JSONMessage,
                   out ErrorResponse,
                   SignInfos
               );

        #endregion


        #region SignMessage         (SignableMessage, BinaryMessage, out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given message.
        /// </summary>
        /// <param name="SignableMessage">A signable message.</param>
        /// <param name="BinaryMessage">The binary representation of the signable message.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="SignInfos">An enumeration of cryptographic signature information or key pairs to sign the given message.</param>
        public Boolean SignMessage(ISignableMessage                  SignableMessage,
                                   Byte[]                            BinaryMessage,
                                   [NotNullWhen(false)] out String?  ErrorResponse,
                                   params SignInfo[]                 SignInfos)
        {

            #region Initial checks

            if (BinaryMessage is null)
            {
                ErrorResponse = "The given binary message must not be null!";
                return false;
            }

            //if (SignInfos is null || !SignInfos.Any())
            //{
            //    ErrorResponse = "The given key pairs must not be null or empty!";
            //    return false;
            //}

            #endregion

            try
            {

                //if (BinaryMessage["@context"] is null)
                //    BinaryMessage.AddFirst(new JProperty("@context", SignableMessage.Context.ToString()));

                if (!TryGetSignInfos(SignableMessage, SignInfos, out var signInfos, out ErrorResponse))
                    return false;

                foreach (var signInfo in signInfos)
                {

                    #region Initial checks

                    if (signInfo is null)
                    {
                        ErrorResponse = "The given key pair must not be null!";
                        return false;
                    }


                    if (signInfo.PrivateKeyBytes is null || signInfo.PrivateKeyBytes.IsNullOrEmpty())
                    {
                        ErrorResponse = "The given key pair must contain a serialized private key!";
                        return false;
                    }

                    if (signInfo.PublicKeyBytes  is null || signInfo.PublicKeyBytes. IsNullOrEmpty())
                    {
                        ErrorResponse = "The given key pair must contain a serialized public key!";
                        return false;
                    }


                    if (signInfo.PrivateKey is null)
                    {
                        ErrorResponse = "The given key pair must contain a private key!";
                        return false;
                    }

                    if (signInfo.PublicKey is null)
                    {
                        ErrorResponse = "The given key pair must contain a public key!";
                        return false;
                    }

                    #endregion

                    var cryptoHash  = signInfo.Algorithm switch {
                                          var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(BinaryMessage),
                                          var s when s == CryptoAlgorithm.Secp384r1  => SHA512.HashData(BinaryMessage),
                                          _                                          => SHA256.HashData(BinaryMessage),
                                      };

                    var signer       = SignerUtilities.GetSigner("NONEwithECDSA");
                    signer.Init(true, signInfo.PrivateKey);
                    signer.BlockUpdate(cryptoHash);
                    var signature    = signer.GenerateSignature();

                    SignableMessage.AddSignature(
                                        new Signature(
                                            KeyId:           SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(signInfo.PublicKey).PublicKeyData.GetBytes(),
                                            Value:           signature,
                                            Algorithm:       signInfo.Algorithm,
                                            SigningMethod:   null,
                                            Encoding:  signInfo.Encoding,
                                            Name:            signInfo.SignerName?. Invoke(SignableMessage),
                                            Description:     signInfo.Description?.Invoke(SignableMessage),
                                            Timestamp:       signInfo.Timestamp?.  Invoke(SignableMessage)
                                        ));

                }

                ErrorResponse = null;
                return true;

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion

        #region SignRequestMessage  (RequestMessage,  BinaryMessage, out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given request message.
        /// </summary>
        /// <param name="RequestMessage">The request message.</param>
        /// <param name="BinaryMessage">The binary representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of signing errors.</param>
        /// <param name="SignInfos">One or multiple cryptographic signature information or key pairs to sign the request message.</param>
        public Boolean SignRequestMessage(IRequest                          RequestMessage,
                                          Byte[]                            BinaryMessage,
                                          [NotNullWhen(false)] out String?  ErrorResponse,
                                          params SignInfo[]                 SignInfos)

            => SignMessage(
                   RequestMessage,
                   BinaryMessage,
                   out ErrorResponse,
                   SignInfos
               );

        #endregion

        #region SignResponseMessage (ResponseMessage, BinaryMessage, out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given response message.
        /// </summary>
        /// <param name="ResponseMessage">A response message.</param>
        /// <param name="BinaryMessage">The binary representation of the response message.</param>
        /// <param name="ErrorResponse">An optional error response in case of signing errors.</param>
        /// <param name="SignInfos">One or multiple cryptographic signature information or key pairs to sign the response message.</param>
        public Boolean SignResponseMessage(IResponse                         ResponseMessage,
                                           Byte[]                            BinaryMessage,
                                           [NotNullWhen(false)] out String?  ErrorResponse,
                                           params SignInfo[]                 SignInfos)

            => SignMessage(
                   ResponseMessage,
                   BinaryMessage,
                   out ErrorResponse,
                   SignInfos
               );

        #endregion

        #region SignSendMessage     (SendMessage,     BinaryMessage, out ErrorResponse, params SignInfos)

        /// <summary>
        /// Sign the given request message.
        /// </summary>
        /// <param name="SendMessage">The request message.</param>
        /// <param name="BinaryMessage">The binary representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of signing errors.</param>
        /// <param name="SignInfos">One or multiple cryptographic signature information or key pairs to sign the request message.</param>
        public Boolean SignSendMessage(IMessage                          SendMessage,
                                       Byte[]                            BinaryMessage,
                                       [NotNullWhen(false)] out String?  ErrorResponse,
                                       params SignInfo[]                 SignInfos)

            => SignMessage(
                   SendMessage,
                   BinaryMessage,
                   out ErrorResponse,
                   SignInfos
               );

        #endregion

        #endregion

        #region Verification...

        #region AddVerificationRule (...)

        public SignaturePolicy AddVerificationRule(JSONLDContext           Context,
                                                   VerificationRuleActions  Action   = VerificationRuleActions.VerifyAll)
        {

            lock (verificationRules)
            {

                verificationRules.Add(new VerificationRule(
                                          Context,
                                          verificationRules.Any() ? verificationRules.Max(entry => entry.Priority) + 1 : 1,
                                          Action
                                      ));

                return this;

            }

        }

        #endregion

        #region AddVerificationRule (...)

        public SignaturePolicy AddVerificationRule(UInt32                  Priority,
                                                   JSONLDContext           Context,
                                                   VerificationRuleActions  Action   = VerificationRuleActions.VerifyAll)
        {

            lock (verificationRules)
            {

                verificationRules.Add(new VerificationRule(
                                          Context,
                                          Priority,
                                          Action
                                      ));

                return this;

            }

        }

        #endregion

        #region ClearVerificationRules()

        /// <summary>
        /// Remove all verification rules, e.g. to reset the signature policy within tests.
        /// </summary>
        public SignaturePolicy ClearVerificationRules()
        {

            lock (verificationRules)
            {
                verificationRules.Clear();
                return this;
            }

        }

        #endregion

        #region AddVerificationRules(...)

        public SignaturePolicy AddVerificationRules(IEnumerable<VerificationRule> VerificationRules)
        {

            lock (verificationRules)
            {

                foreach (var verificationRule in VerificationRules)
                    verificationRules.Add(verificationRule);

                return this;

            }

        }

        #endregion


        #region GetVerificationRules      (...)

        public IEnumerable<VerificationRule> GetVerificationRules(JSONLDContext Context)
        {

            var rules = verificationRules.Where(verificationRule => Context.Matches(verificationRule.Context));

            if (rules.Any())
                return rules;

            return [
                       new VerificationRule(
                           JSONLDContext.Parse("default"),
                           0,
                           DefaultVerificationAction,
                           DefaultVerificationKeyPair
                       )
                   ];

        }

        #endregion

        #region GetHighestVerificationRule(...)

        /// <summary>
        /// The verification rule of highest priority for messages of the given
        /// JSON-LD context, or the default rule, with the default action and the
        /// default verification key, when none matches.
        /// </summary>
        /// <param name="Context">The JSON-LD context of a message.</param>
        public VerificationRule GetHighestVerificationRule(JSONLDContext Context)
        {

            var verificationRule = verificationRules.
                                       Where(verificationRule => Context.Matches(verificationRule.Context)).
                                       MaxBy(verificationRule => verificationRule.Priority);

            if (verificationRule is not null)
                return verificationRule;

            return new VerificationRule(
                       JSONLDContext.Parse("default"),
                       0,
                       DefaultVerificationAction,
                       DefaultVerificationKeyPair
                   );

        }

        #endregion


        #region (private) VerifySignatures(SignableMessage, SignedData, out ErrorResponse)

        /// <summary>
        /// Verify the signatures of the given message over the given data, as the
        /// verification rule of highest priority for its JSON-LD context says.
        /// </summary>
        /// <remarks>
        /// "AcceptUnverified" accepts the message whatever it carries, and "Drop"
        /// and "Reject" refuse it. "VerifyAny" and "VerifyAll" refuse a message
        /// that carries no signature at all, so that taking the signatures off a
        /// message is no way past them. A rule that names a key pair accepts
        /// signatures by that key only, and any other signer is "UnknownSigner".
        /// A rule without a key pair checks that each signature is valid for the
        /// key it names, and leaves it to the node's user roles to say who that
        /// key belongs to.
        /// </remarks>
        /// <param name="SignableMessage">A signable/verifiable message.</param>
        /// <param name="SignedData">What its signatures sign.</param>
        /// <param name="ErrorResponse">Why the message is refused.</param>
        private Boolean VerifySignatures(ISignableMessage                  SignableMessage,
                                         Byte[]                            SignedData,
                                         [NotNullWhen(false)] out String?  ErrorResponse)
        {

            var verificationRule = GetHighestVerificationRule(SignableMessage.Context);

            switch (verificationRule.Action)
            {

                case VerificationRuleActions.AcceptUnverified:
                    foreach (var signature in SignableMessage.Signatures)
                        signature.Status = VerificationStatus.Unverified;
                    ErrorResponse = null;
                    return true;

                case VerificationRuleActions.Drop:
                    foreach (var signature in SignableMessage.Signatures)
                        signature.Status = VerificationStatus.DropMessage;
                    ErrorResponse = $"The signature policy drops '{SignableMessage.Context}' messages!";
                    return false;

                case VerificationRuleActions.Reject:
                    foreach (var signature in SignableMessage.Signatures)
                        signature.Status = VerificationStatus.RejectMessage;
                    ErrorResponse = $"The signature policy rejects '{SignableMessage.Context}' messages!";
                    return false;

            }

            if (!SignableMessage.Signatures.Any())
            {
                ErrorResponse = $"The signature policy wants '{SignableMessage.Context}' messages signed, and this one carries no signature!";
                return false;
            }

            foreach (var signature in SignableMessage.Signatures)
            {

                if (verificationRule.KeyPair?.PublicKeyBytes is Byte[] expectedKey &&
                   !signature.KeyId.SequenceEqual(expectedKey))
                {
                    signature.Status = VerificationStatus.UnknownSigner;
                    continue;
                }

                try
                {

                    var ecp           = signature.Algorithm switch {
                                            var s when s == CryptoAlgorithm.Secp521r1  => SecNamedCurves.GetByName("secp521r1"),
                                            var s when s == CryptoAlgorithm.Secp384r1  => SecNamedCurves.GetByName("secp384r1"),
                                            _                                          => SecNamedCurves.GetByName("secp256r1"),
                                        };
                    var ecParams      = new ECDomainParameters(ecp.Curve, ecp.G, ecp.N, ecp.H, ecp.GetSeed());
                    var pubKeyParams  = new ECPublicKeyParameters("ECDSA", ecParams.Curve.DecodePoint(signature.KeyId), ecParams);

                    var cryptoHash    = signature.Algorithm switch {
                                            var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(SignedData),
                                            var s when s == CryptoAlgorithm.Secp384r1  => SHA512.HashData(SignedData),
                                            _                                          => SHA256.HashData(SignedData),
                                        };

                    var verifier      = SignerUtilities.GetSigner("NONEwithECDSA");
                    verifier.Init(false, pubKeyParams);
                    verifier.BlockUpdate(cryptoHash);
                    signature.Status  = verifier.VerifySignature(signature.Value)
                                            ? VerificationStatus.ValidSignature
                                            : VerificationStatus.InvalidSignature;

                }
                catch
                {
                    // A key that is no point on the curve, or a value that is no signature.
                    signature.Status  = VerificationStatus.BrokenSignature;
                }

                if (verificationRule.Action == VerificationRuleActions.VerifyAny &&
                    signature.Status        == VerificationStatus.ValidSignature)
                {
                    ErrorResponse = null;
                    return true;
                }

            }

            if (SignableMessage.Signatures.All(signature => signature.Status == VerificationStatus.ValidSignature))
            {
                ErrorResponse = null;
                return true;
            }

            ErrorResponse = (verificationRule.Action == VerificationRuleActions.VerifyAny
                                 ? "None of the message's signatures is valid"
                                 : "Not every signature of the message is valid") +
                            $": {String.Join(", ", SignableMessage.Signatures.Select(signature => signature.Status))}!";

            return false;

        }

        #endregion

        #region VerifySendMessage     (SignableMessage, JSONMessage,   out ErrorResponse)

        /// <summary>
        /// Verify the given message.
        /// </summary>
        /// <param name="SignableMessage">A signable/verifiable message.</param>
        /// <param name="JSONMessage">The JSON representation of the signable/verifiable message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean VerifySendMessage(ISignableMessage                  SignableMessage,
                                         JObject                           JSONMessage,
                                         [NotNullWhen(false)] out String?  ErrorResponse)
        {

            try
            {

                if (JSONMessage["@context"] is null)
                    JSONMessage.AddFirst(new JProperty("@context", SignableMessage.Context.ToString()));

                var jsonMessageCopy  = JObject.Parse(JSONMessage.ToString(Formatting.None, WWCP.SignableMessage.DefaultJSONConverters));
                jsonMessageCopy.Remove("signatures");

                var plainText = jsonMessageCopy.ToString(Formatting.None, WWCP.SignableMessage.DefaultJSONConverters);

                return VerifySignatures(
                           SignableMessage,
                           plainText.ToUTF8Bytes(),
                           out ErrorResponse
                       );

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion

        #region VerifyRequestMessage  (RequestMessage,  JSONMessage,   out ErrorResponse)

        /// <summary>
        /// Verify the given request message.
        /// </summary>
        /// <param name="RequestMessage">The request message.</param>
        /// <param name="JSONMessage">The JSON representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean VerifyRequestMessage(IRequest                          RequestMessage,
                                            JObject                           JSONMessage,
                                            [NotNullWhen(false)] out String?  ErrorResponse)

            => VerifySendMessage(
                   RequestMessage,
                   JSONMessage,
                   out ErrorResponse
               );

        #endregion

        #region VerifyResponseMessage (ResponseMessage, JSONMessage,   out ErrorResponse)

        /// <summary>
        /// Verify the given request message.
        /// </summary>
        /// <param name="ResponseMessage">A response message.</param>
        /// <param name="JSONMessage">The JSON representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean VerifyResponseMessage(IResponse                         ResponseMessage,
                                             JObject                           JSONMessage,
                                             [NotNullWhen(false)] out String?  ErrorResponse)

            => VerifySendMessage(
                   ResponseMessage,
                   JSONMessage,
                   out ErrorResponse
               );

        #endregion


        #region VerifySendMessage     (SignableMessage, BinaryMessage, out ErrorResponse)

        /// <summary>
        /// Verify the given message.
        /// </summary>
        /// <param name="SignableMessage">A signable/verifiable message.</param>
        /// <param name="BinaryMessage">The binary representation of the signable/verifiable message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean VerifySendMessage(ISignableMessage                  SignableMessage,
                                         Byte[]                            BinaryMessage,
                                         [NotNullWhen(false)] out String?  ErrorResponse)
        {

            try
            {

                return VerifySignatures(
                           SignableMessage,
                           BinaryMessage,
                           out ErrorResponse
                       );

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion

        #region VerifyRequestMessage  (RequestMessage,  BinaryMessage, out ErrorResponse)

        /// <summary>
        /// Verify the given request message.
        /// </summary>
        /// <param name="RequestMessage">The request message.</param>
        /// <param name="BinaryMessage">The binary representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean VerifyRequestMessage(IRequest                          RequestMessage,
                                            Byte[]                            BinaryMessage,
                                            [NotNullWhen(false)] out String?  ErrorResponse)

            => VerifySendMessage(
                   RequestMessage,
                   BinaryMessage,
                   out ErrorResponse
               );

        #endregion

        #region VerifyResponseMessage (RequestMessage,  BinaryMessage, out ErrorResponse)

        /// <summary>
        /// Verify the given request message.
        /// </summary>
        /// <param name="ResponseMessage">A response message.</param>
        /// <param name="BinaryMessage">The binary representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean VerifyResponseMessage(IResponse                         ResponseMessage,
                                             Byte[]                            BinaryMessage,
                                             [NotNullWhen(false)] out String?  ErrorResponse)

            => VerifySendMessage(
                   ResponseMessage,
                   BinaryMessage,
                   out ErrorResponse
               );

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (SignaturePolicy1, SignaturePolicy2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignaturePolicy1">A signature policy.</param>
        /// <param name="SignaturePolicy2">Another signature policy.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SignaturePolicy? SignaturePolicy1,
                                           SignaturePolicy? SignaturePolicy2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SignaturePolicy1, SignaturePolicy2))
                return true;

            // If one is null, but not both, return false.
            if (SignaturePolicy1 is null || SignaturePolicy2 is null)
                return false;

            return SignaturePolicy1.Equals(SignaturePolicy2);

        }

        #endregion

        #region Operator != (SignaturePolicy1, SignaturePolicy2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SignaturePolicy1">A signature policy.</param>
        /// <param name="SignaturePolicy2">Another signature policy.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SignaturePolicy? SignaturePolicy1,
                                           SignaturePolicy? SignaturePolicy2)

            => !(SignaturePolicy1 == SignaturePolicy2);

        #endregion

        #endregion

        #region IEquatable<SignaturePolicy> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two signature policies for equality.
        /// </summary>
        /// <param name="Object">A signature policy to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SignaturePolicy signaturePolicy &&
                   Equals(signaturePolicy);

        #endregion

        #region Equals(SignaturePolicy)

        /// <summary>
        /// Compares two signature policies for equality.
        /// </summary>
        /// <param name="SignaturePolicy">A signature policy to compare with.</param>
        public Boolean Equals(SignaturePolicy? SignaturePolicy)

            => SignaturePolicy is not null &&

               //String.Equals(KeyId,           SignaturePolicy.KeyId,           StringComparison.Ordinal) &&
               //String.Equals(Value,           SignaturePolicy.EncodingMethod,  StringComparison.Ordinal) &&
               //String.Equals(SigningMethod,   SignaturePolicy.SigningMethod,   StringComparison.Ordinal) &&
               //String.Equals(EncodingMethod,  SignaturePolicy.EncodingMethod,  StringComparison.Ordinal) &&

               base.  Equals(SignaturePolicy);

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

            => $"{SignaturePolicyEntries.Count()} / {KeyPairs.Count()} => Default: '{DefaultSigningAction}'{(DefaultSigningKeyPair is not null ? " / defaultKey" : "")}";

        #endregion

    }

}
