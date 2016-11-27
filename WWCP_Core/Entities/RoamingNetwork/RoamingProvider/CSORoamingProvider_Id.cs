/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of a charging station roaming provider.
    /// </summary>
    public class CSORoamingProvider_Id : IId,
                                         IEquatable <CSORoamingProvider_Id>,
                                         IComparable<CSORoamingProvider_Id>,
                                         IComparable

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// The length of the charging station roaming provider identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station roaming provider identification.
        /// based on the given string.
        /// </summary>
        private CSORoamingProvider_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station roaming provider identification.</param>
        public static CSORoamingProvider_Id Parse(String Text)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station roaming provider identification must not be null or empty!");

            #endregion

            return new CSORoamingProvider_Id(Text);

        }

        #endregion

        #region TryParse(Text, out CSORoamingProviderId)

        /// <summary>
        /// Parse the given string as a charging station roaming provider identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId">The parsed charging station roaming provider identification.</param>
        public static Boolean TryParse(String Text, out CSORoamingProvider_Id CSORoamingProviderId)
        {

            #region Initial checks

            if (Text != null)
                Text = Text.Trim();

            if (Text.IsNullOrEmpty())
            {
                CSORoamingProviderId = default(CSORoamingProvider_Id);
                return false;
            }

            #endregion

            try
            {

                CSORoamingProviderId = new CSORoamingProvider_Id(Text);

                return true;

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            CSORoamingProviderId = default(CSORoamingProvider_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station roaming provider identification.
        /// </summary>
        public CSORoamingProvider_Id Clone

            => new CSORoamingProvider_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another charging station roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CSORoamingProvider_Id CSORoamingProviderId1, CSORoamingProvider_Id CSORoamingProviderId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(CSORoamingProviderId1, CSORoamingProviderId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CSORoamingProviderId1 == null) || ((Object) CSORoamingProviderId2 == null))
                return false;

            return CSORoamingProviderId1.Equals(CSORoamingProviderId2);

        }

        #endregion

        #region Operator != (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another charging station roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CSORoamingProvider_Id CSORoamingProviderId1, CSORoamingProvider_Id CSORoamingProviderId2)
            => !(CSORoamingProviderId1 == CSORoamingProviderId2);

        #endregion

        #region Operator <  (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another charging station roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CSORoamingProvider_Id CSORoamingProviderId1, CSORoamingProvider_Id CSORoamingProviderId2)
        {

            if ((Object) CSORoamingProviderId1 == null)
                throw new ArgumentNullException(nameof(CSORoamingProviderId1), "The given CSORoamingProviderId1 must not be null!");

            return CSORoamingProviderId1.CompareTo(CSORoamingProviderId2) < 0;

        }

        #endregion

        #region Operator <= (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another charging station roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CSORoamingProvider_Id CSORoamingProviderId1, CSORoamingProvider_Id CSORoamingProviderId2)
            => !(CSORoamingProviderId1 > CSORoamingProviderId2);

        #endregion

        #region Operator >  (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another charging station roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CSORoamingProvider_Id CSORoamingProviderId1, CSORoamingProvider_Id CSORoamingProviderId2)
        {

            if ((Object) CSORoamingProviderId1 == null)
                throw new ArgumentNullException(nameof(CSORoamingProviderId1), "The given CSORoamingProviderId1 must not be null!");

            return CSORoamingProviderId1.CompareTo(CSORoamingProviderId2) > 0;

        }

        #endregion

        #region Operator >= (CSORoamingProviderId1, CSORoamingProviderId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId1">A charging station roaming provider identification.</param>
        /// <param name="CSORoamingProviderId2">Another charging station roaming provider identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CSORoamingProvider_Id CSORoamingProviderId1, CSORoamingProvider_Id CSORoamingProviderId2)
            => !(CSORoamingProviderId1 < CSORoamingProviderId2);

        #endregion

        #endregion

        #region IComparable<CSORoamingProviderId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is CSORoamingProvider_Id))
                throw new ArgumentException("The given object is not a charging station roaming provider identification!",
                                            nameof(Object));

            return CompareTo((CSORoamingProvider_Id) Object);

        }

        #endregion

        #region CompareTo(CSORoamingProviderId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CSORoamingProviderId">An object to compare with.</param>
        public Int32 CompareTo(CSORoamingProvider_Id CSORoamingProviderId)
        {

            if ((Object) CSORoamingProviderId == null)
                throw new ArgumentNullException(nameof(CSORoamingProviderId),  "The given charging station roaming provider identification must not be null!");

            // Compare the length of the CSORoamingProviderIds
            var _Result = this.Length.CompareTo(CSORoamingProviderId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, CSORoamingProviderId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<CSORoamingProviderId> Members

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

            if (!(Object is CSORoamingProvider_Id))
                return false;

            return Equals((CSORoamingProvider_Id) Object);

        }

        #endregion

        #region Equals(CSORoamingProviderId)

        /// <summary>
        /// Compares two CSORoamingProviderIds for equality.
        /// </summary>
        /// <param name="CSORoamingProviderId">A charging station roaming provider identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CSORoamingProvider_Id CSORoamingProviderId)
        {

            if ((Object) CSORoamingProviderId == null)
                return false;

            return InternalId.Equals(CSORoamingProviderId.InternalId);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
