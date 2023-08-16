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
    /// A delegate for filtering CSO roaming provider identifications.
    /// </summary>
    /// <param name="CSORoamingProviderId">A CSO roaming provider identification to include.</param>
    public delegate Boolean IncludeCSORoamingProviderIdDelegate(CSORoamingProvider_Id CSORoamingProviderId);


    /// <summary>
    /// Extension methods for CSO roaming provider identifications.
    /// </summary>
    public static class CSORoamingProviderIdExtensions
    {

        /// <summary>
        /// Indicates whether this CSO roaming provider identification is null or empty.
        /// </summary>
        /// <param name="CSORoamingProviderId">A CSO roaming provider identification.</param>
        public static Boolean IsNullOrEmpty(this CSORoamingProvider_Id? CSORoamingProviderId)
            => !CSORoamingProviderId.HasValue || CSORoamingProviderId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this CSO roaming provider identification is NOT null or empty.
        /// </summary>
        /// <param name="CSORoamingProviderId">A CSO roaming provider identification.</param>
        public static Boolean IsNotNullOrEmpty(this CSORoamingProvider_Id? CSORoamingProviderId)
            => CSORoamingProviderId.HasValue && CSORoamingProviderId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a CSO roaming provider.
    /// </summary>
    public readonly struct CSORoamingProvider_Id : IId,
                                                   IEquatable <CSORoamingProvider_Id>,
                                                   IComparable<CSORoamingProvider_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this CSO roaming provider identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this CSO roaming provider identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the CSO roaming provider identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new CSO roaming provider identification based on the given string.
        /// </summary>
        /// <param name="Text">A text representation of a CSO roaming provider identification.</param>
        private CSORoamingProvider_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a CSO roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a CSO roaming provider identification.</param>
        public static CSORoamingProvider_Id Parse(String Text)
        {

            if (TryParse(Text, out var csoRoamingProviderId))
                return csoRoamingProviderId;

            throw new ArgumentException($"Invalid text representation of a CSO roaming provider identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a CSO roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a CSO roaming provider identification.</param>
        public static CSORoamingProvider_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var csoRoamingProviderId))
                return csoRoamingProviderId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out CSORoamingProviderId)

        /// <summary>
        /// Parse the given text as a CSO roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId">The parsed CSO roaming provider identification.</param>
        public static Boolean TryParse(String Text, out CSORoamingProvider_Id CSORoamingProviderId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                CSORoamingProviderId = new CSORoamingProvider_Id(Text);
                return true;
            }

            CSORoamingProviderId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this CSO roaming provider identification.
        /// </summary>
        public CSORoamingProvider_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another CSO roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CSORoamingProvider_Id CSORoamingProviderId1,
                                           CSORoamingProvider_Id CSORoamingProviderId2)

            => CSORoamingProviderId1.Equals(CSORoamingProviderId2);

        #endregion

        #region Operator != (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another CSO roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CSORoamingProvider_Id CSORoamingProviderId1,
                                           CSORoamingProvider_Id CSORoamingProviderId2)

            => !CSORoamingProviderId1.Equals(CSORoamingProviderId2);

        #endregion

        #region Operator <  (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another CSO roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CSORoamingProvider_Id CSORoamingProviderId1,
                                          CSORoamingProvider_Id CSORoamingProviderId2)

            => CSORoamingProviderId1.CompareTo(CSORoamingProviderId2) < 0;

        #endregion

        #region Operator <= (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another CSO roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CSORoamingProvider_Id CSORoamingProviderId1,
                                           CSORoamingProvider_Id CSORoamingProviderId2)

            => CSORoamingProviderId1.CompareTo(CSORoamingProviderId2) <= 0;

        #endregion

        #region Operator >  (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another CSO roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CSORoamingProvider_Id CSORoamingProviderId1,
                                          CSORoamingProvider_Id CSORoamingProviderId2)

            => CSORoamingProviderId1.CompareTo(CSORoamingProviderId2) > 0;

        #endregion

        #region Operator >= (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A CSO roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another CSO roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CSORoamingProvider_Id CSORoamingProviderId1,
                                           CSORoamingProvider_Id CSORoamingProviderId2)

            => CSORoamingProviderId1.CompareTo(CSORoamingProviderId2) >= 0;

        #endregion

        #endregion

        #region IComparable<CSORoamingProvider_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two CSO roaming provider identifications.
        /// </summary>
        /// <param name="Object">A CSO roaming provider identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is CSORoamingProvider_Id csoRoamingProviderId
                   ? CompareTo(csoRoamingProviderId)
                   : throw new ArgumentException("The given object is not a CSO roaming provider identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CSORoamingProviderId)

        /// <summary>
        /// Compares two CSO roaming provider identifications.
        /// </summary>
        /// <param name="CSORoamingProviderId">A CSO roaming provider identification to compare with.</param>
        public Int32 CompareTo(CSORoamingProvider_Id CSORoamingProviderId)

            => String.Compare(InternalId,
                              CSORoamingProviderId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<CSORoamingProvider_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two CSO roaming provider identifications for equality.
        /// </summary>
        /// <param name="Object">A CSO roaming provider identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is CSORoamingProvider_Id csoRoamingProviderId &&
                   Equals(csoRoamingProviderId);

        #endregion

        #region Equals(CSORoamingProviderId)

        /// <summary>
        /// Compares two CSO roaming provider identifications for equality.
        /// </summary>
        /// <param name="CSORoamingProviderId">A CSO roaming provider identification to compare with.</param>
        public Boolean Equals(CSORoamingProvider_Id CSORoamingProviderId)

            => String.Equals(InternalId,
                             CSORoamingProviderId.InternalId,
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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
