/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// Extension methods for certificates.
    /// </summary>
    public static class CertificateExtensions
    {

        /// <summary>
        /// Indicates whether this certificate is null or empty.
        /// </summary>
        /// <param name="Certificate">A certificate.</param>
        public static Boolean IsNullOrEmpty(this Certificate? Certificate)
            => !Certificate.HasValue || Certificate.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this certificate is NOT null or empty.
        /// </summary>
        /// <param name="Certificate">A certificate.</param>
        public static Boolean IsNotNullOrEmpty(this Certificate? Certificate)
            => Certificate.HasValue && Certificate.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a certificate.
    /// </summary>
    public readonly struct Certificate : IId<Certificate>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this certificate is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this certificate is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the certificate.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new certificate based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a certificate.</param>
        private Certificate(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a certificate.
        /// </summary>
        /// <param name="Text">A text representation of a certificate.</param>
        public static Certificate Parse(String Text)
        {

            if (TryParse(Text, out var certificate))
                return certificate;

            throw new ArgumentException($"Invalid text representation of a certificate: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a certificate.
        /// </summary>
        /// <param name="Text">A text representation of a certificate.</param>
        public static Certificate? TryParse(String Text)
        {

            if (TryParse(Text, out var certificate))
                return certificate;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out Certificate)

        /// <summary>
        /// Try to parse the given text as a certificate.
        /// </summary>
        /// <param name="Text">A text representation of a certificate.</param>
        /// <param name="Certificate">The parsed certificate.</param>
        public static Boolean TryParse(String Text, out Certificate Certificate)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    Certificate = new Certificate(Text);
                    return true;
                }
                catch
                { }
            }

            Certificate = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this certificate.
        /// </summary>
        public Certificate Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (Certificate1, Certificate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Certificate1">A certificate.</param>
        /// <param name="Certificate2">Another certificate.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Certificate Certificate1,
                                           Certificate Certificate2)

            => Certificate1.Equals(Certificate2);

        #endregion

        #region Operator != (Certificate1, Certificate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Certificate1">A certificate.</param>
        /// <param name="Certificate2">Another certificate.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Certificate Certificate1,
                                           Certificate Certificate2)

            => !Certificate1.Equals(Certificate2);

        #endregion

        #region Operator <  (Certificate1, Certificate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Certificate1">A certificate.</param>
        /// <param name="Certificate2">Another certificate.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Certificate Certificate1,
                                          Certificate Certificate2)

            => Certificate1.CompareTo(Certificate2) < 0;

        #endregion

        #region Operator <= (Certificate1, Certificate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Certificate1">A certificate.</param>
        /// <param name="Certificate2">Another certificate.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Certificate Certificate1,
                                           Certificate Certificate2)

            => Certificate1.CompareTo(Certificate2) <= 0;

        #endregion

        #region Operator >  (Certificate1, Certificate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Certificate1">A certificate.</param>
        /// <param name="Certificate2">Another certificate.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Certificate Certificate1,
                                          Certificate Certificate2)

            => Certificate1.CompareTo(Certificate2) > 0;

        #endregion

        #region Operator >= (Certificate1, Certificate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Certificate1">A certificate.</param>
        /// <param name="Certificate2">Another certificate.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Certificate Certificate1,
                                           Certificate Certificate2)

            => Certificate1.CompareTo(Certificate2) >= 0;

        #endregion

        #endregion

        #region IComparable<Certificate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two certificates.
        /// </summary>
        /// <param name="Object">A certificate to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Certificate certificate
                   ? CompareTo(certificate)
                   : throw new ArgumentException("The given object is not a certificate!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Certificate)

        /// <summary>
        /// Compares two certificates.
        /// </summary>
        /// <param name="Certificate">A certificate to compare with.</param>
        public Int32 CompareTo(Certificate Certificate)

            => String.Compare(InternalId,
                              Certificate.InternalId,
                              StringComparison.Ordinal);

        #endregion

        #endregion

        #region IEquatable<Certificate> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two certificates for equality.
        /// </summary>
        /// <param name="Object">A certificate to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Certificate certificate &&
                   Equals(certificate);

        #endregion

        #region Equals(Certificate)

        /// <summary>
        /// Compares two certificates for equality.
        /// </summary>
        /// <param name="Certificate">A certificate to compare with.</param>
        public Boolean Equals(Certificate Certificate)

            => String.Equals(InternalId,
                             Certificate.InternalId,
                             StringComparison.Ordinal);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
