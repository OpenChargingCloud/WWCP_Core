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

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An OCPP CSE cryptographic signature policy.
    /// </summary>
    public partial class SignaturePolicy : ACustomSignableData,
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


        #region The protected header (white paper "Signed OCPP Messages and Signer Roles", chapter 4)

        #region Binding, time and format 0

        private readonly Object bindLock = new();

        /// <summary>
        /// The node this policy signs and verifies for: its OCPPSource when it signs.
        /// Set by the node's OCPP adapter (BindTo).
        /// </summary>
        public NetworkingNode_Id?     NodeId             { get; private set; }

        /// <summary>
        /// The protocol the node speaks, as its WebSocket subprotocol ("ocpp2.1", "ocpp1.6"):
        /// its OCPPVersion when it signs, and what a verified header must name.
        /// </summary>
        public String?                OCPPVersion        { get; private set; }

        /// <summary>
        /// Whether signatures of format 0 - which bind neither the frame nor a time - are
        /// checked at all. Even then they count as unverified, never as valid (section 4.2).
        /// </summary>
        public Boolean                AcceptFormat0      { get; set; }  = false;

        /// <summary>
        /// How far in the future a signature's "iat" may lie. Default 300 seconds.
        /// </summary>
        public TimeSpan               MaxClockSkew       { get; set; }  = TimeSpan.FromSeconds(300);

        /// <summary>
        /// How long a signature without "exp" is valid after its "iat". Default 900 seconds.
        /// </summary>
        public TimeSpan               MaxSignatureAge    { get; set; }  = TimeSpan.FromSeconds(900);

        /// <summary>
        /// The clock of signing and verifying. Replaceable for tests.
        /// </summary>
        public Func<DateTimeOffset>   Now                { get; set; }  = () => DateTimeOffset.UtcNow;

        /// <summary>
        /// Sign with OCPPMessageId "*" and a nonce of the signer's own (OCPPNonce) instead of
        /// the message id (section 4.3, Q3). For nodes whose messages pass a relay that sends
        /// them on under ids of its own, as a Local Controller does: the message id the
        /// signer knows is not the one the receiver sees. A response is then bound to its
        /// request by OCPPRequestHash alone, so this suits signed requests.
        /// </summary>
        public Boolean                NonceInsteadOfMessageId   { get; set; }  = false;

        /// <summary>
        /// Which signatures this node writes: the JWS over the JSON of a message, the
        /// COSE_Sign1 over its CBOR, or both (section 4.2). It verifies every signature
        /// it can read, whatever it writes itself. Default: JWS.
        /// </summary>
        /// <remarks>
        /// The two are worth different things on the way. The JWS signs the RFC 8785
        /// form of the JSON, which every node that reads JSON can rebuild. The COSE
        /// signs the deterministic CBOR of the message, which takes the class of the
        /// message to rebuild: a node without it carries the signature along and cannot
        /// verify it.
        /// </remarks>
        public SignatureFormats       Formats            { get; set; }  = SignatureFormats.JWS;

        /// <summary>
        /// Which signatures are written for the given message, where that is not the
        /// same for every message of this node: a node asks its connections, and the
        /// connection a message leaves over may say otherwise than <see cref="Formats"/>
        /// (white paper, section 6.3.1: "Formats", per connection). Null, or an answer
        /// of null, is <see cref="Formats"/>. Set by the node's OCPP adapter.
        /// </summary>
        public Func<ISignableMessage, SignatureFormats?>?  FormatsOf   { get; set; }

        /// <summary>
        /// What a COSE signature signs: the deterministic CBOR (RFC 8949, section 4.2.1)
        /// of the given message without its signatures - or null for a message without
        /// a CBOR form. Set by whoever knows the classes of the messages, the node's OCPP
        /// adapter; without it no COSE signature is written, and none is verified.
        /// </summary>
        public Func<ISignableMessage, Byte[]?>?  CBORPayloadOf   { get; set; }


        /// <summary>
        /// Bind this policy to its node. A policy belongs to one node: binding it to
        /// a second one would sign that node's messages in the first one's name.
        /// </summary>
        /// <param name="NodeId">The node's identification.</param>
        /// <param name="OCPPVersion">The protocol the node speaks, e.g. "ocpp2.1".</param>
        /// <exception cref="InvalidOperationException">The policy belongs to another node.</exception>
        public SignaturePolicy BindTo(NetworkingNode_Id  NodeId,
                                      String             OCPPVersion)
        {

            lock (bindLock)
            {

                if (this.NodeId.HasValue && this.NodeId.Value != NodeId)
                    throw new InvalidOperationException($"This signature policy belongs to the node '{this.NodeId}' already, not to '{NodeId}'!");

                this.NodeId       = NodeId;
                this.OCPPVersion  = OCPPVersion;

            }

            return this;

        }

        #endregion

        #region (private) Frame and FrameOf(Message)

        /// <summary>
        /// What a signature binds of the frame that carries the message.
        /// </summary>
        /// <param name="MessageTypeId">2 (CALL), 3 (CALLRESULT) or 6 (SEND).</param>
        /// <param name="Action">The action; for a response that of its request.</param>
        /// <param name="MessageId">The message id; for a response that of its request.</param>
        /// <param name="Destination">The node the message is for: of a request or SEND its destination, of a response the node that sent the request.</param>
        /// <param name="Request">For a response: the request it answers.</param>
        private sealed record Frame(Byte                MessageTypeId,
                                    String              Action,
                                    String              MessageId,
                                    NetworkingNode_Id?  Destination,
                                    IRequest?           Request);

        private static NetworkingNode_Id? Target(SourceRouting? Destination)
        {
            var target = Destination?.LastOrDefault();
            return target.HasValue && target.Value != NetworkingNode_Id.Zero ? target : null;
        }

        private static NetworkingNode_Id? Origin(NetworkPath? NetworkPath)
        {
            try
            {
                var source = NetworkPath?.Source;
                return source.HasValue && source.Value != NetworkingNode_Id.Zero ? source : null;
            }
            catch
            {
                return null;  // an empty network path
            }
        }

        private static Frame? FrameOf(ISignableMessage Message)

            => Message switch {

                   IRequest  request   => new Frame(2, request.Action, request.RequestId.ToString(), Target(request.Destination), null),

                   IResponse response when response.RequestMessage is IRequest request
                                       => new Frame(3, request.Action, request.RequestId.ToString(), Origin(request.NetworkPath), request),

                   IMessage  message   => new Frame(6, message.Action, message.MessageId.ToString(), Target(message.Destination), null),

                   _                   => null

               };

        #endregion

        #region (private) RequestHash(Request)

        /// <summary>
        /// OCPPRequestHash: SHA-256 (base64url) over the concatenated signature values of a request.
        /// </summary>
        private static String? RequestHash(IRequest? Request)
        {

            var signatures = Request?.Signatures.ToArray() ?? [];

            return signatures.Length > 0
                       ? JWS.Base64URL(SHA256.HashData(signatures.SelectMany(signature => signature.Value).ToArray()))
                       : null;

        }

        #endregion

        #region (private) JWSPayload(JSON, RemoveContext)

        /// <summary>
        /// The JWS payload of a message: base64url of the RFC 8785 form of its JSON
        /// without "signatures" - and without the "@context" a node adds for format 0.
        /// </summary>
        private static String JWSPayload(JObject JSON, Boolean RemoveContext)
        {

            var payload = (JObject) JSON.DeepClone();
            payload.Remove("signatures");

            if (RemoveContext)
                payload.Remove("@context");

            return JWS.Base64URL(JSONCanonicalization.CanonicalizeToUTF8(payload));

        }

        #endregion

        #region (private) ProtectedHeaderOf(Message, SignInfo, Frame, Algorithm, KeyIdentifier, out IssuedAt, out Name, out Description)

        /// <summary>
        /// The protected header of a signature (section 4.1): the same for a JWS and for
        /// a COSE signature, but for the key identifier, which is the thumbprint of the
        /// key in the form of each.
        /// </summary>
        /// <param name="Message">The message that is signed.</param>
        /// <param name="SignInfo">Who signs it.</param>
        /// <param name="frame">What of its frame the signature binds.</param>
        /// <param name="algorithm">ES256, ES384 or ES512.</param>
        /// <param name="KeyIdentifier">The key identifier, as the text the header carries.</param>
        /// <param name="IssuedAt">The signing time.</param>
        /// <param name="Name">The signer's name.</param>
        /// <param name="Description">The signer's description.</param>
        private JObject ProtectedHeaderOf(ISignableMessage    Message,
                                          SignInfo            SignInfo,
                                          Frame?              frame,
                                          String              algorithm,
                                          String              KeyIdentifier,
                                          out DateTimeOffset  IssuedAt,
                                          out String?         Name,
                                          out I18NString?     Description)
        {

            var iat         = SignInfo.Timestamp?.Invoke(Message) ?? Now();
            var source      = NodeId ?? (Message is IRequest request ? Origin(request.NetworkPath) : null);
            var name        = SignInfo.SignerName?. Invoke(Message);
            var description = SignInfo.Description?.Invoke(Message);
            var requestHash = RequestHash(frame?.Request);

            var header      = JSONObject.Create(

                                        new JProperty("alg",                    algorithm),
                                        new JProperty("kid",                    KeyIdentifier),

                                  frame is not null && OCPPVersion is not null
                                      ? new JProperty("OCPPVersion",            OCPPVersion)
                                      : null,

                                  frame is not null
                                      ? new JProperty("OCPPMessageTypeId",      frame.MessageTypeId)
                                      : null,

                                  frame is not null
                                      ? new JProperty("OCPPAction",             frame.Action)
                                      : null,

                                  frame is not null
                                      ? new JProperty("OCPPMessageId",          NonceInsteadOfMessageId ? "*" : frame.MessageId)
                                      : null,

                                  frame is not null && NonceInsteadOfMessageId
                                      ? new JProperty("OCPPNonce",              JWS.Base64URL(RandomNumberGenerator.GetBytes(16)))
                                      : null,

                                  frame is not null && source.HasValue
                                      ? new JProperty("OCPPSource",             source.Value.ToString())
                                      : null,

                                  frame?.Destination is not null
                                      ? new JProperty("OCPPDestination",        frame.Destination.Value.ToString())
                                      : null,

                                        new JProperty("iat",                    iat.ToUnixTimeSeconds()),

                                  requestHash is not null
                                      ? new JProperty("OCPPRequestHash",        requestHash)
                                      : null,

                                  name is not null && name.IsNotNullOrEmpty()
                                      ? new JProperty("OCPPSignerName",         name)
                                      : null,

                                  description is not null && description.IsNotNullOrEmpty()
                                      ? new JProperty("OCPPSignerDescription",  description.ToJSON())
                                      : null,

                                  frame is null
                                      ? new JProperty("typ",                    Message.Context.ToString())
                                      : null

                              );

            var crit = header.Properties().Select(property => property.Name).Where(OCPPHeaderFields.Contains).ToArray();

            if (crit.Length > 0)
                header.Add("crit", new JArray(crit));

            IssuedAt     = iat;
            Name         = name;
            Description  = description;

            return header;

        }

        #endregion

        #region (private) TrySignJWS(Message, JSONMessage, SignInfo, out Signature, out ErrorResponse)

        /// <summary>
        /// The OCPP fields a verifier must understand (RFC 7515 "crit").
        /// </summary>
        private static readonly HashSet<String> OCPPHeaderFields = [
            "OCPPVersion", "OCPPMessageTypeId", "OCPPAction", "OCPPMessageId", "OCPPSource", "OCPPDestination",
            "OCPPNonce", "OCPPRequestHash", "OCPPSignerName", "OCPPSignerDescription"
        ];

        private Boolean TrySignJWS(ISignableMessage                     Message,
                                   JObject                              JSONMessage,
                                   SignInfo                             SignInfo,
                                   [NotNullWhen(true)]  out Signature?  Signature,
                                   [NotNullWhen(false)] out String?     ErrorResponse)
        {

            Signature = null;

            var algorithm = JWS.AlgorithmFor(SignInfo.Algorithm);

            if (algorithm is null)
            {
                ErrorResponse = $"JWS has no algorithm for keys of '{SignInfo.Algorithm}'!";
                return false;
            }

            var frame = FrameOf(Message);

            if (frame is null && Message is IResponse)
            {
                ErrorResponse = "A response is signed only together with the request it answers!";
                return false;
            }

            var jwk         = JWS.PublicKeyJWK(SignInfo.PublicKey);
            var header      = ProtectedHeaderOf(Message, SignInfo, frame, algorithm, JWS.Thumbprint(jwk), out var iat, out var name, out var description);

            var protectedText  = JWS.Base64URL(header.ToString(Formatting.None));
            var signingInput   = System.Text.Encoding.ASCII.GetBytes(protectedText + "." + JWSPayload(JSONMessage, RemoveContext: true));

            Signature = new Signature(
                            protectedText,
                            header,
                            new JObject(new JProperty("jwk", jwk)),
                            JWS.Sign(algorithm, SignInfo.PrivateKey, signingInput),
                            SignInfo.PublicKey.Q.GetEncoded(false),
                            SignInfo.Algorithm,
                            name,
                            description,
                            // What the receiver reads: a NumericDate in whole seconds.
                            DateTimeOffset.FromUnixTimeSeconds(iat.ToUnixTimeSeconds())
                        );

            ErrorResponse = null;
            return true;

        }

        #endregion

        #region (private) TrySignCOSE(Message, SignInfo, out Signature, out ErrorResponse)

        /// <summary>
        /// Sign the given message with a COSE_Sign1 over its deterministic CBOR.
        /// </summary>
        private Boolean TrySignCOSE(ISignableMessage                     Message,
                                    SignInfo                             SignInfo,
                                    [NotNullWhen(true)]  out Signature?  Signature,
                                    [NotNullWhen(false)] out String?     ErrorResponse)
        {

            Signature = null;

            var algorithm = JWS.AlgorithmFor(SignInfo.Algorithm);

            if (algorithm is null)
            {
                ErrorResponse = $"COSE has no algorithm for keys of '{SignInfo.Algorithm}'!";
                return false;
            }

            var frame = FrameOf(Message);

            if (frame is null && Message is IResponse)
            {
                ErrorResponse = "A response is signed only together with the request it answers!";
                return false;
            }

            var payload = CBORPayloadOf?.Invoke(Message);

            if (payload is null)
            {
                ErrorResponse = $"The message '{Message.Context}' has no CBOR a COSE signature could sign!";
                return false;
            }

            var header = ProtectedHeaderOf(
                             Message,
                             SignInfo,
                             frame,
                             algorithm,
                             JWS.Base64URL(COSESignatures.KeyIdentifier(SignInfo.PublicKey)),
                             out _,
                             out _,
                             out _
                         );

            // Read back as whoever receives it reads it: the signature the message
            // carries is what was sent, and nothing that was only meant.
            return Signature.TryParseCOSE(
                       COSESignatures.Sign(header, SignInfo.PublicKey, SignInfo.PrivateKey, payload),
                       out Signature,
                       out ErrorResponse
                   );

        }

        #endregion

        #region (private) CheckJWS(Signature, Message, Payload, Rule, out Why)

        private readonly System.Collections.Concurrent.ConcurrentDictionary<String, DateTimeOffset> replayCache = new();
        private Int64 replayInserts;

        /// <summary>
        /// The replay cache key of a signature: its key, the message type, and its
        /// nonce - or its message id when it has none (section 4.1, "Freshness").
        /// </summary>
        private static String ReplayKey(JObject Header)

            => $"{Header["kid"]}|{Header["OCPPMessageTypeId"]}|{Header["OCPPNonce"] ?? Header["OCPPMessageId"]}";

        /// <summary>
        /// Check one JWS signature: the key, the mathematics, the header against the
        /// frame, and the time. The replay cache is consulted, not yet written.
        /// </summary>
        private VerificationStatus CheckJWS(Signature           Signature,
                                            ISignableMessage    Message,
                                            String              Payload,
                                            VerificationRule    Rule,
                                            out String          Why)
        {

            var header    = Signature.ProtectedHeader!;
            var algorithm = header["alg"]?.Value<String>();

            #region Critical fields this verifier understands

            if (header["crit"] is JToken crit &&
                (crit is not JArray critArray ||
                 critArray.Any(field => field.Type != JTokenType.String || !OCPPHeaderFields.Contains(field.Value<String>()!))))
            {
                Why = $"The signature names critical header fields this verifier does not understand: {crit.ToString(Formatting.None)}";
                return VerificationStatus.BrokenSignature;
            }

            #endregion

            #region The key: the rule's, or the one the signature carries

            ECPublicKeyParameters? publicKey = null;
            var kid = header["kid"]?.Value<String>();

            if (Rule.KeyPair?.PublicKeyBytes is Byte[] ruleKey)
            {

                var curve = Rule.KeyPair.Algorithm switch {
                                var a when a == CryptoAlgorithm.Secp521r1 => SecNamedCurves.GetByName("secp521r1"),
                                var a when a == CryptoAlgorithm.Secp384r1 => SecNamedCurves.GetByName("secp384r1"),
                                _                                         => SecNamedCurves.GetByName("secp256r1")
                            };

                publicKey = new ECPublicKeyParameters(curve.Curve.DecodePoint(ruleKey), new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed()));

                if (kid != JWS.Thumbprint(publicKey))
                {
                    Why = "The signature was made by another key than the one the signature policy names!";
                    return VerificationStatus.UnknownSigner;
                }

            }
            else
            {

                if (Signature.Header?["jwk"] is not JObject jwk ||
                    !JWS.TryParsePublicKey(jwk, out publicKey))
                {
                    Why = "The signature carries no public key, and the signature policy names none!";
                    return VerificationStatus.UnknownSigner;
                }

                if (kid != JWS.Thumbprint(publicKey!))
                {
                    Why = "The key id of the signature is not the thumbprint of the key it carries!";
                    return VerificationStatus.BrokenSignature;
                }

            }

            #endregion

            #region The mathematics

            if (!JWS.Verify(algorithm ?? "", publicKey!, System.Text.Encoding.ASCII.GetBytes(Signature.Protected + "." + Payload), Signature.Value))
            {
                Why = "The signature is not valid for the message as it was received!";
                return VerificationStatus.InvalidSignature;
            }

            #endregion

            return CheckFrameAndTime(header, Message, Rule, out Why);

        }

        #endregion

        #region (private) CheckCOSE(Signature, Message, Payload, Rule, out Why)

        /// <summary>
        /// Check one COSE signature: the key, the mathematics, the header against the
        /// frame, and the time. The replay cache is consulted, not yet written.
        /// </summary>
        /// <param name="Payload">The deterministic CBOR of the message without its signatures.</param>
        private VerificationStatus CheckCOSE(Signature           Signature,
                                             ISignableMessage    Message,
                                             Byte[]              Payload,
                                             VerificationRule    Rule,
                                             out String          Why)
        {

            if (!COSESignatures.TryRead(Signature.COSE!, out var sign1, out var header, out var carriedKey, out var errorResponse))
            {
                Why = errorResponse;
                return VerificationStatus.BrokenSignature;
            }

            #region Critical fields this verifier understands

            if (header["crit"] is JToken crit &&
                (crit is not JArray critArray ||
                 critArray.Any(field => field.Type != JTokenType.String || !OCPPHeaderFields.Contains(field.Value<String>()!))))
            {
                Why = $"The signature names critical header fields this verifier does not understand: {crit.ToString(Formatting.None)}";
                return VerificationStatus.BrokenSignature;
            }

            #endregion

            #region The key: the rule's, or the one the signature carries

            ECPublicKeyParameters? publicKey;
            var kid = header["kid"]?.Value<String>();

            if (Rule.KeyPair?.PublicKeyBytes is Byte[] ruleKey)
            {

                var curve = Rule.KeyPair.Algorithm switch {
                                var a when a == CryptoAlgorithm.Secp521r1 => SecNamedCurves.GetByName("secp521r1"),
                                var a when a == CryptoAlgorithm.Secp384r1 => SecNamedCurves.GetByName("secp384r1"),
                                _                                         => SecNamedCurves.GetByName("secp256r1")
                            };

                publicKey = new ECPublicKeyParameters("ECDSA", curve.Curve.DecodePoint(ruleKey), new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed()));

                if (kid != JWS.Base64URL(COSESignatures.KeyIdentifier(publicKey)))
                {
                    Why = "The signature was made by another key than the one the signature policy names!";
                    return VerificationStatus.UnknownSigner;
                }

            }
            else
            {

                if (carriedKey is null)
                {
                    Why = "The signature carries no public key, and the signature policy names none!";
                    return VerificationStatus.UnknownSigner;
                }

                publicKey = carriedKey;

                if (kid != JWS.Base64URL(COSESignatures.KeyIdentifier(publicKey)))
                {
                    Why = "The key id of the signature is not the thumbprint of the key it carries!";
                    return VerificationStatus.BrokenSignature;
                }

            }

            #endregion

            #region The mathematics

            if (!COSESignatures.Verify(sign1, header, publicKey, Payload))
            {
                Why = "The signature is not valid for the CBOR of the message!";
                return VerificationStatus.InvalidSignature;
            }

            #endregion

            return CheckFrameAndTime(header, Message, Rule, out Why);

        }

        #endregion

        #region (private) CheckFrameAndTime(header, Message, Rule, out Why)

        /// <summary>
        /// What every signature with a protected header is held to, whatever its
        /// encoding: the header against the frame, the time, and that it was not seen
        /// before (section 4.1).
        /// </summary>
        private VerificationStatus CheckFrameAndTime(JObject           header,
                                                     ISignableMessage  Message,
                                                     VerificationRule  Rule,
                                                     out String        Why)
        {

            #region The header against the frame

            var frame = FrameOf(Message);

            if (frame is not null)
            {

                if (OCPPVersion is not null &&
                    header["OCPPVersion"]?.Value<String>() != OCPPVersion)
                {
                    Why = $"The signature was made for '{header["OCPPVersion"]}', not for '{OCPPVersion}'!";
                    return VerificationStatus.WrongFrame;
                }

                if (header["OCPPMessageTypeId"]?.Type != JTokenType.Integer ||
                    header["OCPPMessageTypeId"]!.Value<Int32>() != frame.MessageTypeId)
                {
                    Why = $"The signature was made for message type '{header["OCPPMessageTypeId"]}', not for {frame.MessageTypeId}!";
                    return VerificationStatus.WrongFrame;
                }

                if (header["OCPPAction"]?.Value<String>() != frame.Action)
                {
                    Why = $"The signature was made for the action '{header["OCPPAction"]}', not for '{frame.Action}'!";
                    return VerificationStatus.WrongFrame;
                }

                var messageId = header["OCPPMessageId"]?.Value<String>();

                if (messageId != frame.MessageId &&
                    !(messageId == "*" && header["OCPPNonce"] is not null))
                {
                    Why = $"The signature was made for the message id '{messageId}', not for '{frame.MessageId}'!";
                    return VerificationStatus.WrongFrame;
                }

                if (frame.MessageTypeId != 3)
                {

                    // A request or SEND names the node that shall execute it: this node, or the
                    // destination the frame was delivered to (e.g. "CSMS" on a CSMS's connections).
                    var destination = header["OCPPDestination"]?.Value<String>();

                    if (destination is null ||
                        (destination != frame.Destination?.ToString() && destination != NodeId?.ToString()))
                    {
                        Why = $"The signature was made for the destination '{destination}', not for this node!";
                        return VerificationStatus.WrongFrame;
                    }

                }
                else
                {

                    // A response is bound to its request by the message id and, when the request
                    // was signed, by the hash of its signatures. Its destination is not compared:
                    // a node is known to its peers under aliases ("CSMS", "CentralSystem").
                    var requestHash = RequestHash(frame.Request);

                    if (requestHash is not null &&
                        header["OCPPRequestHash"]?.Value<String>() != requestHash)
                    {
                        Why = "The signature was made for a response to another request!";
                        return VerificationStatus.WrongFrame;
                    }

                }

            }

            #endregion

            #region The time

            // A signing time is part of the header even where it is not held to
            // the clock: a header without one is not one this verifier knows.
            if (header["iat"]?.Type != JTokenType.Integer)
            {
                Why = "The signature carries no signing time!";
                return VerificationStatus.BrokenSignature;
            }

            var now      = Now();
            var issued   = DateTimeOffset.FromUnixTimeSeconds(header["iat"]!.Value<Int64>());
            var expires  = header["exp"]?.Type == JTokenType.Integer
                               ? DateTimeOffset.FromUnixTimeSeconds(header["exp"]!.Value<Int64>())
                               : issued + MaxSignatureAge;

            // A rule for messages with a version of their own, a signed pin set,
            // is verified without the time window - see VerificationRule.WithoutTimeWindow.
            if (!Rule.WithoutTimeWindow)
            {

                if (issued > now + MaxClockSkew)
                {
                    Why = $"The signature was made at {issued:o}, further in the future than the clock skew allows!";
                    return VerificationStatus.OutOfTime;
                }

                if (now > expires)
                {
                    Why = $"The signature expired at {expires:o}!";
                    return VerificationStatus.OutOfTime;
                }

            }

            #endregion

            #region Freshness

            if (replayCache.TryGetValue(ReplayKey(header), out var seenUntil) && seenUntil >= now)
            {
                Why = "The signature was seen before: the message is a replay!";
                return VerificationStatus.Replayed;
            }

            #endregion

            Why = "";
            return VerificationStatus.ValidSignature;

        }

        #endregion

        #region (private) Remember(Signature, Rule)

        /// <summary>
        /// Remember an accepted signature until it expires, so that the same message is not accepted twice.
        /// </summary>
        /// <remarks>
        /// A signature taken by a rule without time window would be forgotten at
        /// once where its "iat" lies further back than MaxSignatureAge: it is
        /// remembered for MaxSignatureAge from now, by this node's clock, at least.
        /// </remarks>
        private void Remember(Signature         Signature,
                              VerificationRule  Rule)
        {

            // Format 0 (binary messages) names nothing a replay could be told by.
            if (Signature.ProtectedHeader is not JObject header)
                return;

            var issued   = DateTimeOffset.FromUnixTimeSeconds(header["iat"]!.Value<Int64>());
            var expires  = header["exp"]?.Type == JTokenType.Integer
                               ? DateTimeOffset.FromUnixTimeSeconds(header["exp"]!.Value<Int64>())
                               : issued + MaxSignatureAge;

            if (Rule.WithoutTimeWindow && expires < Now() + MaxSignatureAge)
                expires = Now() + MaxSignatureAge;

            replayCache[ReplayKey(header)] = expires;

            if (Interlocked.Increment(ref replayInserts) % 256 == 0)
            {
                var now = Now();
                foreach (var entry in replayCache)
                    if (entry.Value < now)
                        replayCache.TryRemove(entry.Key, out _);
            }

        }

        #endregion

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

                    // What the connection of the message says, or what this node says.
                    var formats = FormatsOf?.Invoke(SignableMessage) ?? Formats;

                    if (formats.HasFlag(SignatureFormats.JWS))
                    {

                        if (!TrySignJWS(SignableMessage, JSONMessage, signInfo, out var signature, out ErrorResponse))
                            return false;

                        SignableMessage.AddSignature(signature);

                    }

                    // Next to it, not instead of it: neither signs the other.
                    if (formats.HasFlag(SignatureFormats.COSE))
                    {

                        if (!TrySignCOSE(SignableMessage, signInfo, out var signature, out ErrorResponse))
                            return false;

                        SignableMessage.AddSignature(signature);

                    }

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
        /// A JWS signature counts as valid only when its protected header names
        /// this frame and its time has not passed, and only once (CheckJWS). A
        /// rule without time window (a signed pin set's) leaves out the time, and
        /// only the time. A signature of format 0 counts as unverified at best: it
        /// binds neither.
        /// </remarks>
        /// <param name="SignableMessage">A signable/verifiable message.</param>
        /// <param name="SignedData">What a signature of format 0 signs.</param>
        /// <param name="JWSPayload">What a JWS signature signs: base64url of the RFC 8785 form of the message as received, or null for a binary message.</param>
        /// <param name="ErrorResponse">Why the message is refused.</param>
        private Boolean VerifySignatures(ISignableMessage                  SignableMessage,
                                         Byte[]                            SignedData,
                                         String?                           JWSPayload,
                                         [NotNullWhen(false)] out String?  ErrorResponse)
        {

            var verificationRule = GetHighestVerificationRule(SignableMessage.Context);

            // The policy table (chapter 5) wants to know who signed a request, also where
            // no verification rule asks for signatures: what is there is then verified,
            // and none at all is for the table to judge.
            var forThePolicyTable = false;

            switch (verificationRule.Action)
            {

                case VerificationRuleActions.AcceptUnverified:

                    if (Mode == MessagePolicyModes.Off  ||
                        SignableMessage is IResponse    ||
                       !SignableMessage.Signatures.Any())
                    {
                        foreach (var signature in SignableMessage.Signatures)
                            signature.Status = VerificationStatus.Unverified;
                        ErrorResponse = null;
                        return true;
                    }

                    forThePolicyTable = true;
                    break;

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
                SignaturesDoNotHold(SignableMessage, ErrorResponse);
                return false;
            }

            var reasons = new List<String>();

            // What a COSE signature signs is built once, and only where one asks for it.
            Byte[]? cosePayload      = null;
            var     cosePayloadBuilt = false;

            foreach (var signature in SignableMessage.Signatures)
            {

                #region A COSE signature

                if (signature.IsCOSE)
                {

                    if (!cosePayloadBuilt)
                    {

                        try
                        {
                            cosePayload = CBORPayloadOf?.Invoke(SignableMessage);
                        }
                        catch
                        {
                            cosePayload = null;
                        }

                        cosePayloadBuilt = true;

                    }

                    if (cosePayload is null)
                    {
                        signature.Status = VerificationStatus.BrokenSignature;
                        reasons.Add("A COSE signature on a message this node has no CBOR for cannot be verified");
                    }
                    else
                    {
                        signature.Status = CheckCOSE(signature, SignableMessage, cosePayload, verificationRule, out var why);
                        if (why.Length > 0)
                            reasons.Add(why);
                    }

                    if (verificationRule.Action == VerificationRuleActions.VerifyAny &&
                        signature.Status        == VerificationStatus.ValidSignature)
                    {
                        Remember(signature, verificationRule);
                        ErrorResponse = null;
                        return true;
                    }

                    continue;

                }

                #endregion

                #region A JWS signature with a protected header

                if (signature.IsJWS)
                {

                    if (JWSPayload is null)
                    {
                        signature.Status = VerificationStatus.BrokenSignature;
                        reasons.Add("A JWS signature on a binary message cannot be verified");
                    }
                    else
                    {
                        signature.Status = CheckJWS(signature, SignableMessage, JWSPayload, verificationRule, out var why);
                        if (why.Length > 0)
                            reasons.Add(why);
                    }

                    if (verificationRule.Action == VerificationRuleActions.VerifyAny &&
                        signature.Status        == VerificationStatus.ValidSignature)
                    {
                        Remember(signature, verificationRule);
                        ErrorResponse = null;
                        return true;
                    }

                    continue;

                }

                #endregion

                #region Format 0: checked only when accepted, and unverified at best

                // A binary message has no form yet that binds the frame (finding F17): its
                // signatures stay of format 0, and a valid one counts as before phase 2.
                var binary = JWSPayload is null;

                if (!AcceptFormat0 && !binary)
                {
                    signature.Status = VerificationStatus.Format0NotAccepted;
                    reasons.Add("Signatures of format 0 are not accepted");
                    continue;
                }

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

                    // Valid mathematics, but no frame and no time bound: unverified at best.
                    signature.Status  = verifier.VerifySignature(signature.Value)
                                            ? binary
                                                  ? VerificationStatus.ValidSignature
                                                  : VerificationStatus.Unverified
                                            : VerificationStatus.InvalidSignature;

                    if (signature.Status == VerificationStatus.Unverified)
                        reasons.Add("A signature of format 0 binds neither the frame nor a time, and counts as unverified");

                    if (binary                                                 &&
                        verificationRule.Action == VerificationRuleActions.VerifyAny &&
                        signature.Status        == VerificationStatus.ValidSignature)
                    {
                        ErrorResponse = null;
                        return true;
                    }

                }
                catch
                {
                    // A key that is no point on the curve, or a value that is no signature.
                    signature.Status  = VerificationStatus.BrokenSignature;
                }

                #endregion

            }

            if (SignableMessage.Signatures.All(signature => signature.Status == VerificationStatus.ValidSignature))
            {

                foreach (var signature in SignableMessage.Signatures)
                    Remember(signature, verificationRule);

                ErrorResponse = null;
                return true;

            }

            // In the mode "Observe" an invalid signature is reported, not refused: its key
            // counts as no signer, and the policy table says what follows from that.
            if (forThePolicyTable && Mode == MessagePolicyModes.Observe)
            {
                ErrorResponse = null;
                return true;
            }

            ErrorResponse = (verificationRule.Action == VerificationRuleActions.VerifyAny
                                 ? "None of the message's signatures is valid"
                                 : "Not every signature of the message is valid") +
                            $": {String.Join(", ", SignableMessage.Signatures.Select(signature => signature.Status))}" +
                            (reasons.Count > 0 ? $" ({String.Join("; ", reasons.Distinct())})" : "") + "!";

            SignaturesDoNotHold(SignableMessage, ErrorResponse);

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
        /// <param name="ReceivedJSON">The payload exactly as it was received. A JWS signature signs it, so that fields the parser does not know are covered too. Without it, the re-serialization stands in for it.</param>
        public Boolean VerifySendMessage(ISignableMessage                  SignableMessage,
                                         JObject                           JSONMessage,
                                         [NotNullWhen(false)] out String?  ErrorResponse,
                                         JObject?                          ReceivedJSON   = null)
        {

            try
            {

                var jwsPayload = ReceivedJSON is not null
                                     ? JWSPayload(ReceivedJSON, RemoveContext: false)
                                     : JWSPayload(JSONMessage,  RemoveContext: true);

                if (JSONMessage["@context"] is null)
                    JSONMessage.AddFirst(new JProperty("@context", SignableMessage.Context.ToString()));

                var jsonMessageCopy  = JObject.Parse(JSONMessage.ToString(Formatting.None, WWCP.SignableMessage.DefaultJSONConverters));
                jsonMessageCopy.Remove("signatures");

                var plainText = jsonMessageCopy.ToString(Formatting.None, WWCP.SignableMessage.DefaultJSONConverters);

                return VerifySignatures(
                           SignableMessage,
                           plainText.ToUTF8Bytes(),
                           jwsPayload,
                           out ErrorResponse
                       ) &&
                       (SignableMessage is IResponse ||
                        Decide(
                            SignableMessage,
                            ReceivedJSON ?? JSONMessage,
                            out ErrorResponse
                        ));

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
        /// <param name="ReceivedJSON">The payload exactly as it was received.</param>
        /// <param name="Connection">What the connection the request came over may carry, whoever signed it: asked first. Null for a connection that may carry anything.</param>
        public Boolean VerifyRequestMessage(IRequest                          RequestMessage,
                                            JObject                           JSONMessage,
                                            [NotNullWhen(false)] out String?  ErrorResponse,
                                            JObject?                          ReceivedJSON   = null,
                                            ConnectionRights?                 Connection     = null)

            => (Connection is null ||
                ConnectionAllows(
                    RequestMessage,
                    ReceivedJSON ?? JSONMessage,
                    Connection,
                    out ErrorResponse
                )) &&
               VerifySendMessage(
                   RequestMessage,
                   JSONMessage,
                   out ErrorResponse,
                   ReceivedJSON
               );

        #endregion

        #region VerifyResponseMessage (ResponseMessage, JSONMessage,   out ErrorResponse)

        /// <summary>
        /// Verify the given request message.
        /// </summary>
        /// <param name="ResponseMessage">A response message.</param>
        /// <param name="JSONMessage">The JSON representation of the request message.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        /// <param name="ReceivedJSON">The payload exactly as it was received.</param>
        public Boolean VerifyResponseMessage(IResponse                         ResponseMessage,
                                             JObject                           JSONMessage,
                                             [NotNullWhen(false)] out String?  ErrorResponse,
                                             JObject?                          ReceivedJSON   = null)

            => VerifySendMessage(
                   ResponseMessage,
                   JSONMessage,
                   out ErrorResponse,
                   ReceivedJSON
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
                           null,
                           out ErrorResponse
                       ) &&
                       (SignableMessage is IResponse ||
                        Decide(
                            SignableMessage,
                            null,
                            out ErrorResponse
                        ));

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
                                            [NotNullWhen(false)] out String?  ErrorResponse,
                                            ConnectionRights?                 Connection   = null)

            => (Connection is null ||
                ConnectionAllows(
                    RequestMessage,
                    null,
                    Connection,
                    out ErrorResponse
                )) &&
               VerifySendMessage(
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
