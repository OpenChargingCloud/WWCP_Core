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
    /// The priority of something.
    /// </summary>
    public readonly struct Priority : IId,
                                      IEquatable <Priority>,
                                      IComparable<Priority>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly Int32 InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => false;

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => true;

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64 Length
            => 1;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new priority based on the given number.
        /// </summary>
        public Priority(Int32 Number)
        {
            InternalId = Number;
        }

        #endregion


        #region Operator overloading

        #region Operator == (Priority1, Priority2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority1">A brand identification.</param>
        /// <param name="Priority2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Priority Priority1, Priority Priority2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(Priority1, Priority2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Priority1 == null) || ((Object) Priority2 == null))
                return false;

            return Priority1.Equals(Priority2);

        }

        #endregion

        #region Operator != (Priority1, Priority2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority1">A brand identification.</param>
        /// <param name="Priority2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Priority Priority1, Priority Priority2)
            => !(Priority1 == Priority2);

        #endregion

        #region Operator <  (Priority1, Priority2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority1">A brand identification.</param>
        /// <param name="Priority2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Priority Priority1, Priority Priority2)
        {

            if ((Object) Priority1 == null)
                throw new ArgumentNullException(nameof(Priority1), "The given Priority1 must not be null!");

            return Priority1.CompareTo(Priority2) < 0;

        }

        #endregion

        #region Operator <= (Priority1, Priority2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority1">A brand identification.</param>
        /// <param name="Priority2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Priority Priority1, Priority Priority2)
            => !(Priority1 > Priority2);

        #endregion

        #region Operator >  (Priority1, Priority2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority1">A brand identification.</param>
        /// <param name="Priority2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Priority Priority1, Priority Priority2)
        {

            if ((Object) Priority1 == null)
                throw new ArgumentNullException(nameof(Priority1), "The given Priority1 must not be null!");

            return Priority1.CompareTo(Priority2) > 0;

        }

        #endregion

        #region Operator >= (Priority1, Priority2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority1">A brand identification.</param>
        /// <param name="Priority2">Another brand identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Priority Priority1, Priority Priority2)
            => !(Priority1 < Priority2);

        #endregion

        #endregion

        #region IComparable<Priority> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Priority))
                throw new ArgumentException("The given object is not a brand identification!",
                                            nameof(Object));

            return CompareTo((Priority) Object);

        }

        #endregion

        #region CompareTo(Priority)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Priority">An object to compare with.</param>
        public Int32 CompareTo(Priority Priority)
        {

            if ((Object) Priority == null)
                throw new ArgumentNullException(nameof(Priority),  "The given brand identification must not be null!");

            return InternalId.CompareTo(Priority.InternalId);

        }

        #endregion

        #endregion

        #region IEquatable<Priority> Members

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

            if (!(Object is Priority))
                return false;

            return Equals((Priority) Object);

        }

        #endregion

        #region Equals(Priority)

        /// <summary>
        /// Compares two Prioritys for equality.
        /// </summary>
        /// <param name="Priority">A Priority to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Priority Priority)
        {

            if ((Object) Priority == null)
                return false;

            return InternalId.Equals(Priority.InternalId);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

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
            => InternalId.ToString();

        #endregion

    }

}
