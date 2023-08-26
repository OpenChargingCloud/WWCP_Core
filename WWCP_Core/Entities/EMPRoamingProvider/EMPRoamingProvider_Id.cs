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
    /// A delegate for filtering EMP roaming provider identifications.
    /// </summary>
    /// <param name="EMPRoamingProviderId">An EMP roaming provider identification to include.</param>
    public delegate Boolean IncludeEMPRoamingProviderIdDelegate(EMPRoamingProvider_Id EMPRoamingProviderId);


    /// <summary>
    /// Extension methods for EMP roaming provider identifications.
    /// </summary>
    public static class EMPRoamingProviderIdExtensions
    {

        /// <summary>
        /// Indicates whether this EMP roaming provider identification is null or empty.
        /// </summary>
        /// <param name="EMPRoamingProviderId">An EMP roaming provider identification.</param>
        public static Boolean IsNullOrEmpty(this EMPRoamingProvider_Id? EMPRoamingProviderId)
            => !EMPRoamingProviderId.HasValue || EMPRoamingProviderId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EMP roaming provider identification is NOT null or empty.
        /// </summary>
        /// <param name="EMPRoamingProviderId">An EMP roaming provider identification.</param>
        public static Boolean IsNotNullOrEmpty(this EMPRoamingProvider_Id? EMPRoamingProviderId)
            => EMPRoamingProviderId.HasValue && EMPRoamingProviderId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of an EMP roaming provider.
    /// </summary>
    public readonly struct EMPRoamingProvider_Id : IId,
                                                   IEquatable <EMPRoamingProvider_Id>,
                                                   IComparable<EMPRoamingProvider_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this EMP roaming provider identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this EMP roaming provider identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the EMP roaming provider identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EMP roaming provider identification based on the given string.
        /// </summary>
        /// <param name="Text">A text representation of an EMP roaming provider identification.</param>
        private EMPRoamingProvider_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an EMP roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an EMP roaming provider identification.</param>
        public static EMPRoamingProvider_Id Parse(String Text)
        {

            if (TryParse(Text, out var empRoamingProviderId))
                return empRoamingProviderId;

            throw new ArgumentException($"Invalid text representation of an EMP roaming provider identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an EMP roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an EMP roaming provider identification.</param>
        public static EMPRoamingProvider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var empRoamingProviderId))
                return empRoamingProviderId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EMPRoamingProviderId)

        /// <summary>
        /// Parse the given text as an EMP roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of an EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId">The parsed EMP roaming provider identification.</param>
        public static Boolean TryParse(String Text, out EMPRoamingProvider_Id EMPRoamingProviderId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                EMPRoamingProviderId = new EMPRoamingProvider_Id(Text);
                return true;
            }

            EMPRoamingProviderId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EMP roaming provider identification.
        /// </summary>
        public EMPRoamingProvider_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (EMPRoamingProviderId1, EMPRoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPRoamingProviderId1">An EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId2">Another EMP roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EMPRoamingProvider_Id EMPRoamingProviderId1,
                                           EMPRoamingProvider_Id EMPRoamingProviderId2)

            => EMPRoamingProviderId1.Equals(EMPRoamingProviderId2);

        #endregion

        #region Operator != (EMPRoamingProviderId1, EMPRoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPRoamingProviderId1">An EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId2">Another EMP roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EMPRoamingProvider_Id EMPRoamingProviderId1,
                                           EMPRoamingProvider_Id EMPRoamingProviderId2)

            => !EMPRoamingProviderId1.Equals(EMPRoamingProviderId2);

        #endregion

        #region Operator <  (EMPRoamingProviderId1, EMPRoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPRoamingProviderId1">An EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId2">Another EMP roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EMPRoamingProvider_Id EMPRoamingProviderId1,
                                          EMPRoamingProvider_Id EMPRoamingProviderId2)

            => EMPRoamingProviderId1.CompareTo(EMPRoamingProviderId2) < 0;

        #endregion

        #region Operator <= (EMPRoamingProviderId1, EMPRoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPRoamingProviderId1">An EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId2">Another EMP roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EMPRoamingProvider_Id EMPRoamingProviderId1,
                                           EMPRoamingProvider_Id EMPRoamingProviderId2)

            => EMPRoamingProviderId1.CompareTo(EMPRoamingProviderId2) <= 0;

        #endregion

        #region Operator >  (EMPRoamingProviderId1, EMPRoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPRoamingProviderId1">An EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId2">Another EMP roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EMPRoamingProvider_Id EMPRoamingProviderId1,
                                          EMPRoamingProvider_Id EMPRoamingProviderId2)

            => EMPRoamingProviderId1.CompareTo(EMPRoamingProviderId2) > 0;

        #endregion

        #region Operator >= (EMPRoamingProviderId1, EMPRoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMPRoamingProviderId1">An EMP roaming provider identification.</param>
        /// <param name="EMPRoamingProviderId2">Another EMP roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EMPRoamingProvider_Id EMPRoamingProviderId1,
                                           EMPRoamingProvider_Id EMPRoamingProviderId2)

            => EMPRoamingProviderId1.CompareTo(EMPRoamingProviderId2) >= 0;

        #endregion

        #endregion

        #region IComparable<EMPRoamingProvider_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EMP roaming provider identifications.
        /// </summary>
        /// <param name="Object">An EMP roaming provider identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EMPRoamingProvider_Id empRoamingProviderId
                   ? CompareTo(empRoamingProviderId)
                   : throw new ArgumentException("The given object is not an EMP roaming provider identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EMPRoamingProviderId)

        /// <summary>
        /// Compares two EMP roaming provider identifications.
        /// </summary>
        /// <param name="EMPRoamingProviderId">An EMP roaming provider identification to compare with.</param>
        public Int32 CompareTo(EMPRoamingProvider_Id EMPRoamingProviderId)

            => String.Compare(InternalId,
                              EMPRoamingProviderId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EMPRoamingProvider_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EMP roaming provider identifications for equality.
        /// </summary>
        /// <param name="Object">An EMP roaming provider identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EMPRoamingProvider_Id empRoamingProviderId &&
                   Equals(empRoamingProviderId);

        #endregion

        #region Equals(EMPRoamingProviderId)

        /// <summary>
        /// Compares two EMP roaming provider identifications for equality.
        /// </summary>
        /// <param name="EMPRoamingProviderId">An EMP roaming provider identification to compare with.</param>
        public Boolean Equals(EMPRoamingProvider_Id EMPRoamingProviderId)

            => String.Equals(InternalId,
                             EMPRoamingProviderId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

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
