﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An asymmetric cryptographic public key.
    /// </summary>
    public class PublicKey : ACustomData,
                             IEquatable<PublicKey>
    {

        #region Properties

        /// <summary>
        /// The cryptographic public key.
        /// </summary>
        [Mandatory]
        public   Byte[]               Value            { get; protected set; }

        /// <summary>
        /// The optional cryptographic algorithm of the public key.
        /// The algorithm is optional as in some situations we just do not know it!
        /// </summary>
        [Optional]
        public   CryptoAlgorithm?     Algorithm        { get; }

        /// <summary>
        /// The serialization of the cryptographic public key. Default is 'raw'.
        /// </summary>
        [Mandatory]
        public   CryptoSerialization  Serialization    { get; }

        /// <summary>
        /// The encoding of the cryptographic public key. Default is 'base64'.
        /// </summary>
        [Mandatory]
        public   CryptoEncoding       Encoding         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new asymmetric cryptographic public key.
        /// </summary>
        /// <param name="Value">The public key.</param>
        /// <param name="Algorithm">An optional cryptographic algorithm of the public key. Default is 'secp256r1'.</param>
        /// <param name="Serialization">An optional serialization of the public key. Default is 'raw'.</param>
        /// <param name="Encoding">An optional encoding of the public key. Default is 'base64'.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public PublicKey(Byte[]                Value,
                         CryptoAlgorithm?      Algorithm       = null,
                         CryptoSerialization?  Serialization   = null,
                         CryptoEncoding?       Encoding        = null,
                         CustomData?           CustomData      = null)

            : base(CustomData)

        {

            this.Value          = Value;
            this.Algorithm      = Algorithm;
            this.Serialization  = Serialization ?? CryptoSerialization.RAW;
            this.Encoding       = Encoding      ?? CryptoEncoding.     NONE;

            unchecked
            {

                hashCode = this.Value.        GetHashCode()       * 11 ^
                          (this.Algorithm?.   GetHashCode() ?? 0) *  7 ^
                           this.Serialization.GetHashCode()       *  5 ^
                           this.Encoding.     GetHashCode()       *  3 ^
                           base.              GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion


        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given text representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="Text">The text to be parsed.</param>
        public static PublicKey Parse(String                Text,
                                      CryptoAlgorithm?      Algorithm       = null,
                                      CryptoSerialization?  Serialization   = null,
                                      CryptoEncoding?       Encoding        = null,
                                      CustomData?           CustomData      = null)
        {

            if (TryParse(Text,
                         out var eccPublicKey,
                         out var errorResponse,
                         Algorithm,
                         Serialization,
                         Encoding,
                         CustomData))
            {
                return eccPublicKey;
            }

            throw new ArgumentException("The given text representation of a public key is invalid: " + errorResponse,
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text, out PublicKey, out ErrorResponse, Encoding = null, ...)

        /// <summary>
        /// Try to parse the given text representation of an asymmetric public key.
        /// </summary>
        /// <param name="Text">The text representation of the public key.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParse(String                               Text,
                                       [NotNullWhen(true)]  out PublicKey?  PublicKey,
                                       [NotNullWhen(false)] out String?     ErrorResponse,
                                       CryptoAlgorithm?                     Algorithm       = null,
                                       CryptoSerialization?                 Serialization   = null,
                                       CryptoEncoding?                      Encoding        = null,
                                       CustomData?                          CustomData      = null)
        {

            PublicKey      = null;
            ErrorResponse  = null;

            var text       = Text.Trim().Replace(" ", "");

            try
            {

                #region HEX    encoding

                if (Encoding == CryptoEncoding.HEX)
                {

                    if (!text.TryParseHEX(out var hexByteArray1, out var errorResponse1))
                    {
                        ErrorResponse = $"The given HEX encoding of a public key '{Text}' is invalid: " + errorResponse1;
                        return false;
                    }

                    if (TryParse(hexByteArray1,
                                 out var publicKey,
                                 out ErrorResponse,
                                 Algorithm,
                                 Serialization,
                                 Encoding,
                                 CustomData))
                    {
                        PublicKey = publicKey;
                    }

                }

                #endregion

                #region BASE32 encoding

                else if (Encoding == CryptoEncoding.BASE32)
                {
                    if (!text.TryParseBASE32(out var base32ByteArray1, out var errorResponse1))
                    {
                        ErrorResponse = $"The given base32 encoding of a public key '{Text}' is invalid: " + errorResponse1;
                        return false;
                    }
                }

                #endregion

                #region BASE64 encoding

                else if (Encoding == CryptoEncoding.BASE64)
                {
                    if (!text.TryParseBASE64(out var base64ByteArray1, out var errorResponse1))
                    {
                        ErrorResponse = $"The given base64 encoding of a public key '{Text}' is invalid: " + errorResponse1;
                        return false;
                    }
                }

                #endregion

                #region ...or an unknown encoding!

                else
                {

                    var publicKeys = new List<PublicKey>();

                    #region Try to parse HEX (0-9, A-F, a-f)  // same as Base16

                    if (text.TryParseHEX(out var hexByteArray, out var errorResponse))
                    {
                        try
                        {

                            if (hexByteArray[0] == 0x30 &&
                                TryParse(hexByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         Algorithm,
                                         Serialization,
                                         CryptoEncoding.HEX,
                                         CustomData))
                            {
                                publicKeys.Add(
                                    publicKey
                                );
                            }

                            else
                                publicKeys.Add(
                                    new ECCPublicKey(
                                        hexByteArray,
                                        Algorithm,
                                        Serialization,
                                        CryptoEncoding.HEX,
                                        CustomData
                                    )
                                );

                        }
                        catch (Exception)
                        {
                            ErrorResponse = $"The given HEX encoding of a public key '{Text}' is invalid: " + errorResponse;
                            return false;
                        }
                    }

                    #endregion

                    #region Try to parse Base32

                    if (text.TryParseBASE32(out var base32ByteArray, out errorResponse))
                    {
                        try
                        {

                            if (TryParse(base32ByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         Algorithm,
                                         Serialization,
                                         CryptoEncoding.BASE32,
                                         CustomData))
                            {
                                publicKeys.Add(
                                    publicKey
                                );
                            }

                            else
                                publicKeys.Add(
                                    new ECCPublicKey(
                                        base32ByteArray,
                                        Algorithm,
                                        Serialization,
                                        CryptoEncoding.BASE32,
                                        CustomData
                                    )
                                );

                        }
                        catch (Exception)
                        {
                            ErrorResponse = $"The given base32 encoding of a public key '{Text}' is invalid: " + errorResponse;
                            return false;
                        }
                    }

                    #endregion

                    // Base36: 0-9, A-Z
                    // Base58: Which is Base64 without 0, O, I and l
                    // Base62: Which is Base64 without + and /

                    #region Try to parse Base64

                    if (text.TryParseBASE64(out var base64ByteArray, out errorResponse))
                    {
                        try
                        {

                            if (TryParse(base64ByteArray,
                                         out var publicKey,
                                         out var errorResponse2,
                                         Algorithm,
                                         Serialization,
                                         CryptoEncoding.BASE64,
                                         CustomData))
                            {
                                publicKeys.Add(
                                    publicKey
                                );
                            }

                            else
                                publicKeys.Add(
                                    new ECCPublicKey(
                                        base64ByteArray,
                                        Algorithm,
                                        Serialization,
                                        CryptoEncoding.BASE64,
                                        CustomData
                                    )
                                );

                        }
                        catch (Exception)
                        {
                            ErrorResponse = $"The given base64 encoding of a public key '{Text}' is invalid: " + errorResponse;
                            return false;
                        }
                    }

                    #endregion

                    // Base85
                    // Base91
                    // Base92
                    // Base100


                    #region ...or not matches any encoding!

                    if (publicKeys.Count == 0)
                    {
                        ErrorResponse = $"The given public key '{Text}' could not be parsed!";
                        return false;
                    }

                    #endregion

                    #region ...or ambiguous encodings!

                    if (publicKeys.Count > 1)
                    {

                        var hashSet = publicKeys.Select(pk => pk.Encoding).ToHashSet();

                        if (hashSet.Count == 2 &&
                            hashSet.Contains(CryptoEncoding.HEX) &&
                            hashSet.Contains(CryptoEncoding.BASE64))
                        {
                            PublicKey = publicKeys.First(publicKey => publicKey.Encoding == CryptoEncoding.HEX);
                            return true;
                        }

                        else if (hashSet.Count == 2 &&
                            hashSet.Contains(CryptoEncoding.BASE32) &&
                            hashSet.Contains(CryptoEncoding.BASE64))
                        {
                            PublicKey = publicKeys.First(publicKey => publicKey.Encoding == CryptoEncoding.BASE64);
                            return true;
                        }

                        else
                        {
                            ErrorResponse = $"The given public key '{Text}' could be parsed as multiple different encodings: {publicKeys.Select(publicKey => publicKey.Encoding.ToString()).AggregateWith(", ")}!";
                            return false;
                        }

                    }

                    #endregion

                    else
                    {
                        PublicKey = publicKeys.First();
                        return true;
                    }

                }

                #endregion



                //// Most likely an ASN.1 DER encoding sequence of a public key
                //if (ASN1[0] != 0x30)
                //{
                //    PublicKey      = null;
                //    ErrorResponse  = $"The given ASN.1 DER representation '{ASN1.ToHexString()}' of a public key is invalid!";
                //    return false;
                //}

                //if (ECCPublicKey.TryParseASN1(ASN1,
                //                              out ECCPublicKey eccPublicKey,
                //                              out ErrorResponse,
                //                              CustomData))
                //{
                //    PublicKey = eccPublicKey;
                //    return true;
                //}


            }
            catch (Exception e)
            {
                ErrorResponse = $"The given text representation '{Text}' of a public key is invalid: " + e.Message;
            }

            ErrorResponse = "Unknown error!";
            return false;

        }

        #endregion


        #region (static) Parse    (ByteArray)

        /// <summary>
        /// Parse the given binary representation of an asymmetric elliptic curve cryptographic public key.
        /// </summary>
        /// <param name="ByteArray">The byte array to be parsed.</param>
        public static PublicKey Parse(Byte[]                ByteArray,
                                      CryptoAlgorithm?      Algorithm       = null,
                                      CryptoSerialization?  Serialization   = null,
                                      CryptoEncoding?       Encoding        = null,
                                      CustomData?           CustomData      = null)
        {

            if (TryParse(ByteArray,
                         out var eccPublicKey,
                         out var errorResponse,
                         Algorithm,
                         Serialization,
                         Encoding,
                         CustomData))
            {
                return eccPublicKey;
            }

            throw new ArgumentException("The given binary representation of a public key is invalid: " + errorResponse,
                                        nameof(ByteArray));

        }

        #endregion

        #region (static) TryParse (ByteArray, out PublicKey, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given ASN.1 DER representation of an asymmetric public key.
        /// </summary>
        /// <param name="ByteArray">The ASN.1 DER representation of the public key.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParse(Byte[]                               ByteArray,
                                       [NotNullWhen(true)]  out PublicKey?  PublicKey,
                                       [NotNullWhen(false)] out String?     ErrorResponse,
                                       CryptoAlgorithm?                     Algorithm       = null,
                                       CryptoSerialization?                 Serialization   = null,
                                       CryptoEncoding?                      Encoding        = null,
                                       CustomData?                          CustomData      = null)
        {

            PublicKey      = null;
            ErrorResponse  = null;

            try
            {

                // Most likely an ASN.1 DER encoding sequence of a public key
                //if (ByteArray[0] != 0x30)
                //{
                //    PublicKey      = null;
                //    ErrorResponse  = $"The given ASN.1 DER representation '{ByteArray.ToHexString()}' of a public key is invalid!";
                //    return false;
                //}

                if (ByteArray[0] == 0x30 &&
                    ECCPublicKey.TryParse(ByteArray,
                                          out var eccPublicKey,
                                          out ErrorResponse,
                                          Algorithm,
                                          Serialization,
                                          Encoding,
                                          CustomData))
                {
                    PublicKey = eccPublicKey;
                    return true;
                }

                else if (ByteArray[0] == 0x30 &&
                         ECCPublicKey.TryParseASN1(ByteArray,
                                                   out eccPublicKey,
                                                   out ErrorResponse,
                                                   Encoding,
                                                   CustomData))
                {
                    PublicKey = eccPublicKey;
                    return true;
                }


            } catch (Exception e)
            {
                ErrorResponse  = $"The given ASN.1 DER representation '{ByteArray.ToHexString()}' of a public key is invalid: " + e.Message;
            }

            return false;

        }

        #endregion


        #region ToJSON(CustomPublicKeySerializer = null, CustomCustomDataSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPublicKeySerializer">A delegate to serialize cryptographic public keys.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(Boolean                                       Embedded                     = true,
                              CustomJObjectSerializerDelegate<PublicKey>?   CustomPublicKeySerializer    = null,
                              CustomJObjectSerializerDelegate<CustomData>?  CustomCustomDataSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("value",           Encoding.       Encode(Value)),

                           Algorithm.HasValue
                               ? new JProperty("algorithm",       Algorithm.Value.ToString())
                               : null,

                           Serialization == CryptoSerialization.RAW
                               ? null
                               : new JProperty("serialization",   Serialization.  ToString()),

                           Encoding      == CryptoEncoding.     BASE64
                               ? null
                               : new JProperty("encoding",        Encoding.       ToString()),

                           CustomData is not null
                               ? new JProperty("customData",      CustomData.     ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomPublicKeySerializer is not null
                       ? CustomPublicKeySerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this public key.
        /// </summary>
        public PublicKey Clone()

            => new (

                   (Byte[]) Value.Clone(),

                   Algorithm?.   Clone(),
                   Serialization.Clone(),
                   Encoding.     Clone(),

                   CustomData

               );

        #endregion


        #region Operator overloading

        #region Operator == (PublicKey1, PublicKey2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PublicKey1">A public key.</param>
        /// <param name="PublicKey2">Another public key.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (PublicKey? PublicKey1,
                                           PublicKey? PublicKey2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(PublicKey1, PublicKey2))
                return true;

            // If one is null, but not both, return false.
            if (PublicKey1 is null || PublicKey2 is null)
                return false;

            return PublicKey1.Equals(PublicKey2);

        }

        #endregion

        #region Operator != (PublicKey1, PublicKey2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="PublicKey1">A public key.</param>
        /// <param name="PublicKey2">Another public key.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (PublicKey? PublicKey1,
                                           PublicKey? PublicKey2)

            => !(PublicKey1 == PublicKey2);

        #endregion

        #endregion

        #region IEquatable<PublicKey> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two public keys for equality.
        /// </summary>
        /// <param name="Object">A public key to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is PublicKey publicKey &&
                   Equals(publicKey);

        #endregion

        #region Equals(PublicKey)

        /// <summary>
        /// Compares two public keys for equality.
        /// </summary>
        /// <param name="PublicKey">A public key to compare with.</param>
        public Boolean Equals(PublicKey? PublicKey)

            => PublicKey is not null &&

               Value.        SequenceEqual(PublicKey.Value)         &&

            ((!Algorithm.HasValue && !PublicKey.Algorithm.HasValue) ||
              (Algorithm.HasValue &&  PublicKey.Algorithm.HasValue && Algorithm.Value.Equals(PublicKey.Algorithm.Value))) &&

               Serialization.Equals       (PublicKey.Serialization) &&
               Encoding.     Equals       (PublicKey.Encoding)      &&
               base.         Equals       (PublicKey);

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
        {

            var parameters = new List<String>();

            if (Encoding      != CryptoEncoding.     NONE)
                parameters.Add(Encoding.       ToString());

            if (Serialization != CryptoSerialization.RAW)
                parameters.Add(Serialization.  ToString());

            if (Algorithm.HasValue)
                parameters.Add(Algorithm.Value.ToString());

            return String.Concat(

                       Encoding.Encode(Value),

                       parameters.Count > 0
                           ? $" [{parameters.AggregateWith(", ")}]"
                           : ""

                   );

        }

        #endregion


    }

}
