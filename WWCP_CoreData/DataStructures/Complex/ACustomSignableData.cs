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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An abstract custom signable data container.
    /// </summary>
    public abstract class ACustomSignableData : ACustomData,
                                                IEquatable<ACustomSignableData>
    {

        #region Data

        private readonly HashSet<Signature> signatures;

        #endregion

        #region Properties

        /// <summary>
        /// The optional enumeration of keys to be used for signing this message.
        /// </summary>
        public IEnumerable<KeyPair>    SignKeys     { get; }

        /// <summary>
        /// The optional enumeration of information to be used for signing this message.
        /// </summary>
        public IEnumerable<SignInfo>   SignInfos    { get; }

        /// <summary>
        /// The optional enumeration of cryptographic signatures of this message.
        /// </summary>
        [Optional]
        public IEnumerable<Signature>  Signatures
            => signatures;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new custom signable data.
        /// </summary>
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this message.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this message.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public ACustomSignableData(IEnumerable<KeyPair>?    SignKeys     = null,
                                   IEnumerable<SignInfo>?   SignInfos    = null,
                                   IEnumerable<Signature>?  Signatures   = null,
                                   CustomData?              CustomData   = null)

            : base(CustomData)

        {

            this.SignKeys    = SignKeys  ?? [];
            this.SignInfos   = SignInfos ?? [];
            this.signatures  = Signatures is not null && Signatures.Any()
                                   ? new HashSet<Signature>(Signatures)
                                   : [];

            unchecked
            {

                hashCode = this.Signatures.CalcHashCode() * 3 ^
                           base.           GetHashCode();

            }

        }

        #endregion


        #region AddSignature   (Signature)

        /// <summary>
        /// Add a cryptographic signature to this message.
        /// </summary>
        /// <param name="Signature">A cryptographic signature.</param>
        public void AddSignature(Signature Signature)
        {
            lock (signatures)
            {

                if (signatures.Add(Signature))
                    hashCode = Signatures.CalcHashCode() * 3 ^
                               base.      GetHashCode();

            }
        }

        #endregion

        #region RemoveSignature(Signature)

        /// <summary>
        /// Remove a cryptographic signature from this message.
        /// </summary>
        /// <param name="Signature">A cryptographic signature.</param>
        public void RemoveSignature(Signature Signature)
        {
            lock (signatures)
            {

                if (signatures.Remove(Signature))
                    hashCode = Signatures.CalcHashCode() * 3 ^
                               base.GetHashCode();

            }
        }

        #endregion


        #region Sign  (JSONData,   Context, KeyPair, out ErrorResponse, SignerName = null, Description = null, Timestamp = null)

        /// <summary>
        /// Sign the given JSON data structure, as a signed data object: the
        /// embedded JWS of section 4.4 of the white paper "Signed OCPP
        /// Messages and Signer Roles". Until phase 4 this was a signature of
        /// format 0, over Newtonsoft's own serialization of the object.
        /// </summary>
        /// <remarks>
        /// The name and the description of the signer are in the protected
        /// header, under the names a signature of a message has for them.
        /// </remarks>
        /// <param name="JSONData">A JSON object to be signed.</param>
        /// <param name="Context">The JSON-LD context of the JSON data structure.</param>
        /// <param name="KeyPair">A cryptographic key pair for signing.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        /// <param name="SignerName">An optional name of the signer.</param>
        /// <param name="Description">An optional description of the signature.</param>
        /// <param name="Timestamp">An optional timestamp of the signature.</param>
        public Boolean Sign(JObject          JSONData,
                            JSONLDContext    Context,
                            ECCKeyPair       KeyPair,
                            out String?      ErrorResponse,
                            String?          SignerName    = null,
                            I18NString?      Description   = null,
                            DateTimeOffset?  Timestamp     = null)
        {

            try
            {

                #region Initial checks

                if (KeyPair.PrivateKeyBytes is null || KeyPair.PrivateKeyBytes.IsNullOrEmpty())
                {
                    ErrorResponse = "The given key pair must contain a serialized private key!";
                    return false;
                }

                if (KeyPair.PublicKeyBytes  is null || KeyPair.PublicKeyBytes. IsNullOrEmpty())
                {
                    ErrorResponse = "The given key pair must contain a serialized public key!";
                    return false;
                }


                if (KeyPair.PrivateKey is null)
                {
                    ErrorResponse = "The given key pair must contain a private key!";
                    return false;
                }

                if (KeyPair.PublicKey is null)
                {
                    ErrorResponse = "The given key pair must contain a public key!";
                    return false;
                }

                #endregion

                // The embedded JWS of a signed data object (white paper "Signed OCPP
                // Messages and Signer Roles", section 4.4): the protected header says
                // what kind of object was signed, by its JSON-LD context, and when;
                // the payload is the RFC 8785 form of the object without its signatures.
                var unsigned = (JObject) JSONData.DeepClone();
                unsigned.Remove(SignedDataObject.SignaturesMember);

                SignedDataObject.Sign(unsigned, Context.ToString(), KeyPair, Timestamp, null, SignerName, Description);

                if (unsigned[SignedDataObject.SignaturesMember]?[0] is not JObject signatureJSON)
                {
                    ErrorResponse = "The signature could not be made!";
                    return false;
                }

                if (!Signature.TryParse(signatureJSON, out var signature, out ErrorResponse))
                    return false;

                AddSignature(signature);

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

        #region Sign  (BinaryData, Context, KeyPair, out ErrorResponse, SignerName = null, Description = null, Timestamp = null)

        /// <summary>
        /// Sign the given binary data structure.
        /// </summary>
        /// <param name="BinaryData">Binary data to be signed.</param>
        /// <param name="Context">The JSON-LD context of the JSON data structure.</param>
        /// <param name="KeyPair">A cryptographic key pair for signing.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        /// <param name="SignerName">An optional name of the signer.</param>
        /// <param name="Description">An optional description of the signature.</param>
        /// <param name="Timestamp">An optional timestamp of the signature.</param>
        public Boolean Sign(Byte[]           BinaryData,
                            //JSONLDContext    Context,
                            ECCKeyPair       KeyPair,
                            out String?      ErrorResponse,
                            String?          SignerName    = null,
                            I18NString?      Description   = null,
                            DateTimeOffset?  Timestamp     = null)
        {

            try
            {

                //if (JSONMessage["@context"] is null)
                //    JSONMessage.AddFirst(new JProperty("@context", Context.ToString()));

                #region Initial checks

                if (KeyPair.PrivateKeyBytes is null || KeyPair.PrivateKeyBytes.IsNullOrEmpty())
                {
                    ErrorResponse = "The given key pair must contain a serialized private key!";
                    return false;
                }

                if (KeyPair.PublicKeyBytes  is null || KeyPair.PublicKeyBytes. IsNullOrEmpty())
                {
                    ErrorResponse = "The given key pair must contain a serialized public key!";
                    return false;
                }


                if (KeyPair.PrivateKey is null)
                {
                    ErrorResponse = "The given key pair must contain a private key!";
                    return false;
                }

                if (KeyPair.PublicKey is null)
                {
                    ErrorResponse = "The given key pair must contain a public key!";
                    return false;
                }

                #endregion

                var cryptoHash  = KeyPair.Algorithm switch {
                                      var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(BinaryData),
                                      var s when s == CryptoAlgorithm.Secp384r1  => SHA512.HashData(BinaryData),
                                      _                                          => SHA256.HashData(BinaryData),
                                  };

                var signer       = SignerUtilities.GetSigner("NONEwithECDSA");
                signer.Init(true, KeyPair.PrivateKey);
                signer.BlockUpdate(cryptoHash);
                var signature    = signer.GenerateSignature();

                AddSignature(new Signature(
                                 KeyId:           SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(KeyPair.PublicKey).PublicKeyData.GetBytes(),
                                 Value:           signature,
                                 Algorithm:       KeyPair.Algorithm,
                                 SigningMethod:   null,
                                 Encoding:  KeyPair.Encoding,
                                 Name:            SignerName,
                                 Description:     Description,
                                 Timestamp:       Timestamp
                             ));

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


        #region Verify(JSONData,   Context, out ErrorResponse, VerificationRuleAction = VerificationRuleActions.VerifyAll)

        /// <summary>
        /// Verify the given JSON data structure, as a signed data object: its
        /// signatures are the embedded JWS of section 4.4 of the white paper
        /// "Signed OCPP Messages and Signer Roles", made for an object of the
        /// given JSON-LD context. A signature of format 0 is not counted.
        /// </summary>
        /// <param name="JSONData">The JSON representation of the signable/verifiable data.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean Verify(JObject                   JSONData,
                              JSONLDContext             Context,
                              out String?               ErrorResponse,
                              VerificationRuleActions?  VerificationRuleAction = VerificationRuleActions.VerifyAll)
        {

            ErrorResponse = null;

            if (!Signatures.Any())
            {

                if (VerificationRuleAction == VerificationRuleActions.AcceptUnverified)
                    return true;

                ErrorResponse = "The given message does not contain any digital signatures!";
                return false;

            }

            try
            {

                var type = Context.ToString();

                foreach (var signature in Signatures)
                {

                    if (signature.IsJWS)
                    {

                        // Each signature by itself, so that each has its own status.
                        var withThisOne = (JObject) JSONData.DeepClone();
                        withThisOne[SignedDataObject.SignaturesMember] = new JArray(signature.ToJSON());

                        signature.Status = SignedDataObject.Verify(withThisOne, type, out var signers, out var doesNotHold) && signers.Count == 1
                                               ? VerificationStatus.ValidSignature
                                               : VerificationStatus.InvalidSignature;

                        if (signature.Status != VerificationStatus.ValidSignature)
                            ErrorResponse ??= doesNotHold ?? "A signature of the object does not hold!";

                    }

                    // A signature of format 0 covers Newtonsoft's own serialization of the
                    // object and binds neither its kind nor its time: it is not counted (Q7, Q21).
                    else
                    {
                        signature.Status  = VerificationStatus.Format0NotAccepted;
                        ErrorResponse   ??= "A signature of the object is of format 0, which signed data objects no longer have!";
                    }

                    if (VerificationRuleAction == VerificationRuleActions.VerifyAny &&
                        signature.Status       == VerificationStatus.ValidSignature)
                    {
                        ErrorResponse = null;
                        return true;
                    }

                }

                // Default, and when there is no signature policy (entry) defined!
                return Signatures.All(signature => signature.Status == VerificationStatus.ValidSignature);

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion

        #region Verify(BinaryData, Context, out ErrorResponse, VerificationRuleAction = VerificationRuleActions.VerifyAll)

        /// <summary>
        /// Verify the given binary data structure.
        /// </summary>
        /// <param name="BinaryData">The binary representation of the signable/verifiable data.</param>
        /// <param name="ErrorResponse">An optional error response in case of validation errors.</param>
        public Boolean Verify(Byte[]                    BinaryData,
                              //JSONLDContext             Context,
                              out String?               ErrorResponse,
                              VerificationRuleActions?  VerificationRuleAction = VerificationRuleActions.VerifyAll)
        {

            ErrorResponse = null;

            if (!Signatures.Any())
            {

                if (VerificationRuleAction == VerificationRuleActions.AcceptUnverified)
                    return true;

                ErrorResponse = "The given message does not contain any digital signatures!";
                return false;

            }

            try
            {

                //if (JSONData["@context"] is null)
                //    JSONData.AddFirst(new JProperty("@context", Context.ToString()));

                //var jsonMessageCopy  = JObject.Parse(JSONData.ToString(Formatting.None, SignableMessage.DefaultJSONConverters));
                //jsonMessageCopy.Remove("signatures");

                var plainText = BinaryData; //jsonMessageCopy.ToString(Formatting.None, SignableMessage.DefaultJSONConverters);

                foreach (var signature in Signatures)
                {

                    var ecp           = signature.Algorithm switch {
                                            var s when s == CryptoAlgorithm.Secp521r1  => SecNamedCurves.GetByName("secp521r1"),
                                            var s when s == CryptoAlgorithm.Secp384r1  => SecNamedCurves.GetByName("secp384r1"),
                                            _                                          => SecNamedCurves.GetByName("secp256r1"),
                                        };
                    var ecParams      = new ECDomainParameters(ecp.Curve, ecp.G, ecp.N, ecp.H, ecp.GetSeed());
                    var pubKeyParams  = new ECPublicKeyParameters("ECDSA", ecParams.Curve.DecodePoint(signature.KeyId), ecParams);

                    var cryptoHash    = signature.Algorithm switch {
                                            var s when s == CryptoAlgorithm.Secp521r1  => SHA512.HashData(plainText),
                                            var s when s == CryptoAlgorithm.Secp384r1  => SHA512.HashData(plainText),
                                            _                                          => SHA256.HashData(plainText),
                                        };

                    var verifier      = SignerUtilities.GetSigner("NONEwithECDSA");
                    verifier.Init(false, pubKeyParams);
                    verifier.BlockUpdate(cryptoHash);
                    signature.Status  = verifier.VerifySignature(signature.Value)
                                            ? VerificationStatus.ValidSignature
                                            : VerificationStatus.InvalidSignature;

                    if (VerificationRuleAction == VerificationRuleActions.VerifyAny &&
                        signature.Status       == VerificationStatus.ValidSignature)
                    {
                        return true;
                    }

                }

                // Default, and when there is no signature policy (entry) defined!
                return Signatures.All(signature => signature.Status == VerificationStatus.ValidSignature);

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion


        #region IEquatable<ACustomSignableData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two abstract custom signable data containers for equality.
        /// </summary>
        /// <param name="Object">An abstract custom signable data container to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ACustomSignableData aCustomSignableData &&
                   Equals(aCustomSignableData);

        #endregion

        #region Equals(ACustomSignableData)

        /// <summary>
        /// Compares two abstract custom signable data containers for equality.
        /// </summary>
        /// <param name="ACustomSignableData">An abstract custom signable data container to compare with.</param>
        public Boolean Equals(ACustomSignableData? ACustomSignableData)

            => ACustomSignableData is not null &&

             ((Signatures is     null && ACustomSignableData.Signatures is     null) ||
              (Signatures is not null && ACustomSignableData.Signatures is not null && Signatures.Equals(ACustomSignableData.Signatures)))&&

             ((CustomData is     null && ACustomSignableData.CustomData is     null) ||
              (CustomData is not null && ACustomSignableData.CustomData is not null && CustomData.Equals(ACustomSignableData.CustomData)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        private Int32 hashCode;

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

            => CustomData?.ToString() ?? "";

        #endregion

    }

}
