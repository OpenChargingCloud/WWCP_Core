/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for certificate identifications.
    /// </summary>
    public static class CertificateIdExtensions
    {

        /// <summary>
        /// Indicates whether this certificate identification is null or empty.
        /// </summary>
        /// <param name="CertificateId">A certificate identification.</param>
        public static Boolean IsNullOrEmpty(this Certificate_Id? CertificateId)
            => !CertificateId.HasValue || CertificateId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this certificate identification is null or empty.
        /// </summary>
        /// <param name="CertificateId">A certificate identification.</param>
        public static Boolean IsNotNullOrEmpty(this Certificate_Id? CertificateId)
            => CertificateId.HasValue && CertificateId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a certificate.
    /// </summary>
    public readonly struct Certificate_Id : IId<Certificate_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this certificate identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this certificate identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the certificate identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new certificate identification based on the given string.
        /// </summary>
        private Certificate_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Mapper = null)

        /// <summary>
        /// Generate a new random certificate identification.
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated certificate identification.</param>
        public static Certificate_Id NewRandom(Func<String, String>? Mapper = null)

            => new(Mapper is not null
                        ? Mapper(Guid.NewGuid().ToString())
                        : Guid.NewGuid().ToString());

        #endregion

        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a certificate identification.
        /// </summary>
        /// <param name="Text">A text-representation of a certificate identification.</param>
        public static Certificate_Id Parse(String Text)
        {

            if (TryParse(Text, out var certificateId))
                return certificateId;

            throw new ArgumentException("Invalid text representation of a certificate identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a certificate identification.
        /// </summary>
        /// <param name="Text">A text-representation of a certificate identification.</param>
        public static Certificate_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var certificateId))
                return certificateId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text, out CertificateId)

        /// <summary>
        /// Try to parse the given string as a certificate identification.
        /// </summary>
        /// <param name="Text">A text-representation of a certificate identification.</param>
        /// <param name="CertificateId">The parsed certificate identification.</param>
        public static Boolean TryParse(String Text, out Certificate_Id CertificateId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    CertificateId = new Certificate_Id(Text.Trim());
                    return true;
                }
                catch
                { }
            }

            CertificateId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this certificate identification.
        /// </summary>
        public Certificate_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CertificateIdId1, CertificateIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateIdId1">A certificate identification.</param>
        /// <param name="CertificateIdId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Certificate_Id CertificateIdId1,
                                           Certificate_Id CertificateIdId2)

            => CertificateIdId1.Equals(CertificateIdId2);

        #endregion

        #region Operator != (CertificateIdId1, CertificateIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateIdId1">A certificate identification.</param>
        /// <param name="CertificateIdId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Certificate_Id CertificateIdId1,
                                           Certificate_Id CertificateIdId2)

            => !CertificateIdId1.Equals(CertificateIdId2);

        #endregion

        #region Operator <  (CertificateIdId1, CertificateIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateIdId1">A certificate identification.</param>
        /// <param name="CertificateIdId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Certificate_Id CertificateIdId1,
                                          Certificate_Id CertificateIdId2)

            => CertificateIdId1.CompareTo(CertificateIdId2) < 0;

        #endregion

        #region Operator <= (CertificateIdId1, CertificateIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateIdId1">A certificate identification.</param>
        /// <param name="CertificateIdId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Certificate_Id CertificateIdId1,
                                           Certificate_Id CertificateIdId2)

            => CertificateIdId1.CompareTo(CertificateIdId2) <= 0;

        #endregion

        #region Operator >  (CertificateIdId1, CertificateIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateIdId1">A certificate identification.</param>
        /// <param name="CertificateIdId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Certificate_Id CertificateIdId1,
                                          Certificate_Id CertificateIdId2)

            => CertificateIdId1.CompareTo(CertificateIdId2) > 0;

        #endregion

        #region Operator >= (CertificateIdId1, CertificateIdId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateIdId1">A certificate identification.</param>
        /// <param name="CertificateIdId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Certificate_Id CertificateIdId1,
                                           Certificate_Id CertificateIdId2)

            => CertificateIdId1.CompareTo(CertificateIdId2) >= 0;

        #endregion

        #endregion

        #region IComparable<CertificateId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two certificate identifications.
        /// </summary>
        /// <param name="Object">A certificate identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Certificate_Id certificateId
                   ? CompareTo(certificateId)
                   : throw new ArgumentException("The given object is not a certificate identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CertificateId)

        /// <summary>
        /// Compares two certificate identifications.
        /// </summary>
        /// <param name="CertificateId">A certificate identification to compare with.</param>
        public Int32 CompareTo(Certificate_Id CertificateId)

            => String.Compare(InternalId,
                              CertificateId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<CertificateId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two certificate identifications for equality.
        /// </summary>
        /// <param name="Object">A certificate identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Certificate_Id certificateId &&
                   Equals(certificateId);

        #endregion

        #region Equals(CertificateId)

        /// <summary>
        /// Compares two certificate identifications for equality.
        /// </summary>
        /// <param name="CertificateId">A certificate identification to compare with.</param>
        public Boolean Equals(Certificate_Id CertificateId)

            => String.Equals(InternalId,
                             CertificateId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text-representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
