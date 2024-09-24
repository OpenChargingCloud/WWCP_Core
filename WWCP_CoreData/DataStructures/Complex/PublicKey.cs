/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region (static) TryParseASN1 (ASN1, out PublicKey, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given ASN.1 DER representation of an asymmetric public key.
        /// </summary>
        /// <param name="ASN1">The ASN.1 DER representation of the public key.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public static Boolean TryParseASN1(Byte[]                               ASN1,
                                           [NotNullWhen(true)]  out PublicKey?  PublicKey,
                                           [NotNullWhen(false)] out String?     ErrorResponse,
                                           CustomData?                          CustomData   = null)
        {

            PublicKey = null;

            try
            {

                // Most likely an ASN.1 DER encoding sequence of a public key
                if (ASN1[0] != 0x30)
                {
                    PublicKey      = null;
                    ErrorResponse  = $"The given ASN.1 DER representation '{ASN1.ToHexString()}' of a public key is invalid!";
                    return false;
                }

                if (ECCPublicKey.TryParseASN1(ASN1,
                                              out var eccPublicKey,
                                              out ErrorResponse,
                                              CustomData))
                {
                    PublicKey = eccPublicKey;
                    return true;
                }


            } catch (Exception e)
            {
                ErrorResponse  = $"The given ASN.1 DER representation '{ASN1.ToHexString()}' of a public key is invalid: " + e.Message;
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
                               : new JProperty("serialization",   Serialization),

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
        /// Clone this object.
        /// </summary>
        public PublicKey Clone()

            => new (

                   (Byte[]) Value.Clone(),

                   Algorithm?.   Clone,
                   Serialization.Clone,
                   Encoding.     Clone,

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
