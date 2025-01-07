/*
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An asymmetric cryptographic key pair.
    /// </summary>
    public class KeyPair : ACustomData,
                           IEquatable<KeyPair>
    {

        #region Data

        public const String Context = "https://open.charging.cloud/context//context/cryptography/keyPair";

        #endregion

        #region Properties

        /// <summary>
        /// The cryptographic public key.
        /// </summary>
        public PublicKey             PublicKey
            => PublicKey.Parse(PublicKeyBytes);

        /// <summary>
        /// The cryptographic public key.
        /// </summary>
        [Mandatory]
        public  Byte[]               PublicKeyBytes     { get; }

        /// <summary>
        /// The optional cryptographic private key.
        /// </summary>
        [Optional]
        public  Byte[]               PrivateKeyBytes    { get; }

        /// <summary>
        /// The optional cryptographic algorithm of the keys. Default is 'secp256r1'.
        /// </summary>
        [Optional]
        public  CryptoAlgorithm      Algorithm          { get; }

        /// <summary>
        /// The optional serialization of the cryptographic keys. Default is 'raw'.
        /// </summary>
        [Optional]
        public  CryptoSerialization  Serialization      { get; }

        /// <summary>
        /// The optional encoding of the cryptographic keys. Default is 'base64'.
        /// </summary>
        [Optional]
        public  CryptoEncoding       Encoding           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new asymmetric cryptographic key pair.
        /// </summary>
        /// <param name="Private">The private key.</param>
        /// <param name="Public">The public key.</param>
        /// <param name="Algorithm">The optional cryptographic algorithm of the keys. Default is 'secp256r1'.</param>
        /// <param name="Serialization">The optional serialization of the cryptographic keys. Default is 'raw'.</param>
        /// <param name="Encoding">The optional encoding of the cryptographic keys. Default is 'base64'.</param>
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public KeyPair(Byte[]                Public,
                       Byte[]?               Private         = null,
                       CryptoAlgorithm?      Algorithm       = null,
                       CryptoSerialization?  Serialization   = null,
                       CryptoEncoding?       Encoding        = null,
                       CustomData?           CustomData      = null)

            : base(CustomData)

        {

            this.PublicKeyBytes   = Public;
            this.PrivateKeyBytes  = Private       ?? [];
            this.Algorithm        = Algorithm     ?? CryptoAlgorithm.    Secp256r1;
            this.Serialization    = Serialization ?? CryptoSerialization.RAW;
            this.Encoding         = Encoding      ?? CryptoEncoding.     BASE64;

            unchecked
            {

                hashCode = this.PublicKeyBytes. GetHashCode() * 13 ^
                           this.PrivateKeyBytes.GetHashCode() * 11 ^
                           this.Algorithm.      GetHashCode() *  7 ^
                           this.Serialization.  GetHashCode() *  5 ^
                           this.Encoding.       GetHashCode() *  3 ^
                           base.                GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        public static KeyPair Parse(String                PublicKey,
                                    String?               PrivateKey      = null,
                                    CryptoAlgorithm?      Algorithm       = null,
                                    CryptoSerialization?  Serialization   = null,
                                    CryptoEncoding?       Encoding        = null,
                                    CustomData?           CustomData      = null)
        {

            if (TryParse(PublicKey,
                         out var keyPair,
                         out var errorResponse,
                         PrivateKey,
                         Algorithm,
                         Serialization,
                         Encoding,
                         CustomData))
            {
                return keyPair;
            }

            throw new ArgumentException("The given public key is invalid!");

        }


        public static Boolean TryParse(String                             PublicKey,
                                       [NotNullWhen(true)]  out KeyPair?  KeyPair,
                                       [NotNullWhen(false)] out String?   ErrorResponse,
                                       String?                            PrivateKey      = null,
                                       CryptoAlgorithm?                   Algorithm       = null,
                                       CryptoSerialization?               Serialization   = null,
                                       CryptoEncoding?                    Encoding        = null,
                                       CustomData?                        CustomData      = null)
        {

            KeyPair        = null;
            ErrorResponse  = null;

            if (Encoding is null ||
                Encoding == CryptoEncoding.HEX)
            {

                KeyPair = new KeyPair(
                              PublicKey.  FromHEX(),
                              PrivateKey?.FromHEX(),
                              Algorithm,
                              Serialization,
                              CryptoEncoding.HEX,
                              CustomData
                          );

                return true;

            }

            else if (Encoding == CryptoEncoding.BASE64)
            {

                KeyPair = new KeyPair(
                              PublicKey.  FromBASE64(),
                              PrivateKey?.FromBASE64(),
                              Algorithm,
                              Serialization,
                              CryptoEncoding.BASE64,
                              CustomData
                          );

                return true;

            }


            ErrorResponse = "The given encoding is not supported!";
            return false;

        }




        public static Boolean TryParse(JObject                            JSON,
                                       [NotNullWhen(true)]  out KeyPair?  KeyPair,
                                       [NotNullWhen(false)] out String?   ErrorResponse)

            => TryParse(JSON,
                        out KeyPair,
                        out ErrorResponse,
                        null,
                        null,
                        null,
                        null,
                        null);


        public static Boolean TryParse(JObject                            JSON,
                                       [NotNullWhen(true)]  out KeyPair?  KeyPair,
                                       [NotNullWhen(false)] out String?   ErrorResponse,
                                       String?                            PrivateKey      = null,
                                       CryptoAlgorithm?                   Algorithm       = null,
                                       CryptoSerialization?               Serialization   = null,
                                       CryptoEncoding?                    Encoding        = null,
                                       CustomData?                        CustomData      = null)
        {

            KeyPair        = null;
            ErrorResponse  = null;

            return false;

        }





        #region ToJSON(CustomKeyPairSerializer = null, CustomCustomDataSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomKeyPairSerializer">A delegate to serialize cryptographic key pairs.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<KeyPair>?     CustomKeyPairSerializer      = null,
                              CustomJObjectSerializerDelegate<CustomData>?  CustomCustomDataSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("private",         Encoding.Encode(PrivateKeyBytes)),
                                 new JProperty("public",          Encoding.Encode(PublicKeyBytes)),

                           Algorithm     != CryptoAlgorithm.Secp256r1
                               ? new JProperty("algorithm",       Algorithm.    ToString())
                               : null,

                           Serialization != CryptoSerialization.RAW
                               ? new JProperty("serialization",   Serialization.ToString())
                               : null,

                           Encoding      != CryptoEncoding.BASE64
                               ? new JProperty("encoding",        Encoding.     ToString())
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",      CustomData.   ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomKeyPairSerializer is not null
                       ? CustomKeyPairSerializer(this, json)
                       : json;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this object.
        /// </summary>
        public KeyPair Clone()

            => new (

                   (Byte[]) PrivateKeyBytes.Clone(),
                   (Byte[]) PublicKeyBytes. Clone(),

                   Algorithm.    Clone(),
                   Serialization.Clone(),
                   Encoding.     Clone(),

                   CustomData

               );

        #endregion


        #region Operator overloading

        #region Operator == (KeyPair1, KeyPair2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="KeyPair1">A key pair.</param>
        /// <param name="KeyPair2">Another key pair.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (KeyPair? KeyPair1,
                                           KeyPair? KeyPair2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(KeyPair1, KeyPair2))
                return true;

            // If one is null, but not both, return false.
            if (KeyPair1 is null || KeyPair2 is null)
                return false;

            return KeyPair1.Equals(KeyPair2);

        }

        #endregion

        #region Operator != (KeyPair1, KeyPair2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="KeyPair1">A key pair.</param>
        /// <param name="KeyPair2">Another key pair.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (KeyPair? KeyPair1,
                                           KeyPair? KeyPair2)

            => !(KeyPair1 == KeyPair2);

        #endregion

        #endregion

        #region IEquatable<KeyPair> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two key pairs for equality.
        /// </summary>
        /// <param name="Object">A key pair to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is KeyPair keyPair &&
                   Equals(keyPair);

        #endregion

        #region Equals(KeyPair)

        /// <summary>
        /// Compares two key pairs for equality.
        /// </summary>
        /// <param name="KeyPair">A key pair to compare with.</param>
        public Boolean Equals(KeyPair? KeyPair)

            => KeyPair is not null &&

               PrivateKeyBytes.SequenceEqual(KeyPair.PrivateKeyBytes) &&
               PublicKeyBytes. SequenceEqual(KeyPair.PublicKeyBytes)  &&
               Algorithm.      Equals       (KeyPair.Algorithm)       &&
               Serialization.  Equals       (KeyPair.Serialization)   &&
               Encoding.       Equals       (KeyPair.Encoding)        &&
               base.           Equals       (KeyPair);

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

            => String.Concat(

                   PrivateKeyBytes is not null
                       ? $"private: {PrivateKeyBytes.ToBase64()}, "
                       : "",

                   $"public: {PublicKeyBytes.ToBase64()}"

               );

        #endregion

    }

}
