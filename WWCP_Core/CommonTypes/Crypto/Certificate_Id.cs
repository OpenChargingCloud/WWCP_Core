/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of a certificate.
    /// </summary>
    public struct Certificate_Id : IId,
                                   IEquatable <Certificate_Id>,
                                   IComparable<Certificate_Id>

    {

        #region Data

        private readonly static Random _Random = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the certificate identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new certificate identification based on the given string.
        /// </summary>
        /// <param name="Text">The text representation of a certificate identification.</param>
        private Certificate_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Random(Length = 20)

        public static Certificate_Id Random(Byte Length = 42)
            => new Certificate_Id(_Random.RandomString(Length));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a certificate identification.
        /// </summary>
        /// <param name="Text">A text representation of a certificate identification.</param>
        public static Certificate_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a certificate identification must not be null or empty!");

            #endregion

            if (TryParse(Text, out Certificate_Id CertificateId))
                return CertificateId;

            throw new ArgumentNullException(nameof(Text), "The given text representation of a certificate identification is invalid!");

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a certificate identification.
        /// </summary>
        /// <param name="Text">A text representation of a certificate identification.</param>
        public static Certificate_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Certificate_Id CertificateId))
                return CertificateId;

            return new Certificate_Id?();

        }

        #endregion

        #region (static) TryParse(Text, out CertificateId)

        /// <summary>
        /// Try to parse the given string as a certificate identification.
        /// </summary>
        /// <param name="Text">A text representation of a certificate identification.</param>
        /// <param name="CertificateId">The parsed certificate identification.</param>
        public static Boolean TryParse(String Text, out Certificate_Id CertificateId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                CertificateId = default;
                return false;
            }

            #endregion

            try
            {
                CertificateId = new Certificate_Id(Text);
                return true;
            }
            catch (Exception)
            { }

            CertificateId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this certificate identification.
        /// </summary>
        public Certificate_Id Clone

            => new Certificate_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CertificateId1, CertificateId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId1">A certificate identification.</param>
        /// <param name="CertificateId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Certificate_Id CertificateId1, Certificate_Id CertificateId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(CertificateId1, CertificateId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CertificateId1 == null) || ((Object) CertificateId2 == null))
                return false;

            return CertificateId1.Equals(CertificateId2);

        }

        #endregion

        #region Operator != (CertificateId1, CertificateId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId1">A certificate identification.</param>
        /// <param name="CertificateId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Certificate_Id CertificateId1, Certificate_Id CertificateId2)
            => !(CertificateId1 == CertificateId2);

        #endregion

        #region Operator <  (CertificateId1, CertificateId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId1">A certificate identification.</param>
        /// <param name="CertificateId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Certificate_Id CertificateId1, Certificate_Id CertificateId2)
        {

            if ((Object) CertificateId1 == null)
                throw new ArgumentNullException(nameof(CertificateId1), "The given CertificateId1 must not be null!");

            return CertificateId1.CompareTo(CertificateId2) < 0;

        }

        #endregion

        #region Operator <= (CertificateId1, CertificateId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId1">A certificate identification.</param>
        /// <param name="CertificateId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Certificate_Id CertificateId1, Certificate_Id CertificateId2)
            => !(CertificateId1 > CertificateId2);

        #endregion

        #region Operator >  (CertificateId1, CertificateId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId1">A certificate identification.</param>
        /// <param name="CertificateId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Certificate_Id CertificateId1, Certificate_Id CertificateId2)
        {

            if ((Object) CertificateId1 == null)
                throw new ArgumentNullException(nameof(CertificateId1), "The given CertificateId1 must not be null!");

            return CertificateId1.CompareTo(CertificateId2) > 0;

        }

        #endregion

        #region Operator >= (CertificateId1, CertificateId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId1">A certificate identification.</param>
        /// <param name="CertificateId2">Another certificate identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Certificate_Id CertificateId1, Certificate_Id CertificateId2)
            => !(CertificateId1 < CertificateId2);

        #endregion

        #endregion

        #region IComparable<CertificateId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Certificate_Id CertificateId))
                throw new ArgumentException("The given object is not a certificate identification!",
                                            nameof(Object));

            return CompareTo(CertificateId);

        }

        #endregion

        #region CompareTo(CertificateId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CertificateId">An object to compare with.</param>
        public Int32 CompareTo(Certificate_Id CertificateId)
        {

            if ((Object) CertificateId == null)
                throw new ArgumentNullException(nameof(CertificateId),  "The given certificate identification must not be null!");

            return String.Compare(InternalId, CertificateId.InternalId, StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region IEquatable<CertificateId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is Certificate_Id CertificateId))
                return false;

            return Equals(CertificateId);

        }

        #endregion

        #region Equals(CertificateId)

        /// <summary>
        /// Compares two CertificateIds for equality.
        /// </summary>
        /// <param name="CertificateId">A certificate identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Certificate_Id CertificateId)
        {

            if ((Object) CertificateId == null)
                return false;

            return InternalId.ToLower().Equals(CertificateId.InternalId.ToLower());

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
