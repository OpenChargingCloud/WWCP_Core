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
    /// The unique identification of a parking space.
    /// </summary>
    public readonly struct ParkingSpace_Id : IId,
                                             IEquatable <ParkingSpace_Id>,
                                             IComparable<ParkingSpace_Id>

{

        #region Data

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
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new parking space identification.
        /// based on the given string.
        /// </summary>
        private ParkingSpace_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region New

        /// <summary>
        /// Returns a new parking space identification.
        /// </summary>
        public static ParkingSpace_Id New
            => ParkingSpace_Id.Parse(Guid.NewGuid().ToString());

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a parking space identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking space identification.</param>
        public static ParkingSpace_Id Parse(String Text)

            => new ParkingSpace_Id(Text);

        #endregion

        #region TryParse(Text, out ParkingSpaceId)

        /// <summary>
        /// Parse the given string as a parking space identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking space identification.</param>
        /// <param name="ParkingSpaceId">The parsed parking space identification.</param>
        public static Boolean TryParse(String Text, out ParkingSpace_Id ParkingSpaceId)
        {
            try
            {

                ParkingSpaceId = new ParkingSpace_Id(Text);

                return true;

            }
            catch
            {
                ParkingSpaceId = default(ParkingSpace_Id);
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this parking space identification.
        /// </summary>
        public ParkingSpace_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ParkingSpaceId1, ParkingSpaceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId1">A parking space identification.</param>
        /// <param name="ParkingSpaceId2">Another parking space identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ParkingSpace_Id ParkingSpaceId1, ParkingSpace_Id ParkingSpaceId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ParkingSpaceId1, ParkingSpaceId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ParkingSpaceId1 == null) || ((Object) ParkingSpaceId2 == null))
                return false;

            return ParkingSpaceId1.Equals(ParkingSpaceId2);

        }

        #endregion

        #region Operator != (ParkingSpaceId1, ParkingSpaceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId1">A parking space identification.</param>
        /// <param name="ParkingSpaceId2">Another parking space identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ParkingSpace_Id ParkingSpaceId1, ParkingSpace_Id ParkingSpaceId2)
            => !(ParkingSpaceId1 == ParkingSpaceId2);

        #endregion

        #region Operator <  (ParkingSpaceId1, ParkingSpaceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId1">A parking space identification.</param>
        /// <param name="ParkingSpaceId2">Another parking space identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ParkingSpace_Id ParkingSpaceId1, ParkingSpace_Id ParkingSpaceId2)
        {

            if ((Object) ParkingSpaceId1 == null)
                throw new ArgumentNullException(nameof(ParkingSpaceId1), "The given ParkingSpaceId1 must not be null!");

            return ParkingSpaceId1.CompareTo(ParkingSpaceId2) < 0;

        }

        #endregion

        #region Operator <= (ParkingSpaceId1, ParkingSpaceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId1">A parking space identification.</param>
        /// <param name="ParkingSpaceId2">Another parking space identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ParkingSpace_Id ParkingSpaceId1, ParkingSpace_Id ParkingSpaceId2)
            => !(ParkingSpaceId1 > ParkingSpaceId2);

        #endregion

        #region Operator >  (ParkingSpaceId1, ParkingSpaceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId1">A parking space identification.</param>
        /// <param name="ParkingSpaceId2">Another parking space identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ParkingSpace_Id ParkingSpaceId1, ParkingSpace_Id ParkingSpaceId2)
        {

            if ((Object) ParkingSpaceId1 == null)
                throw new ArgumentNullException(nameof(ParkingSpaceId1), "The given ParkingSpaceId1 must not be null!");

            return ParkingSpaceId1.CompareTo(ParkingSpaceId2) > 0;

        }

        #endregion

        #region Operator >= (ParkingSpaceId1, ParkingSpaceId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId1">A parking space identification.</param>
        /// <param name="ParkingSpaceId2">Another parking space identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ParkingSpace_Id ParkingSpaceId1, ParkingSpace_Id ParkingSpaceId2)
            => !(ParkingSpaceId1 < ParkingSpaceId2);

        #endregion

        #endregion

        #region IComparable<ParkingSpaceId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ParkingSpace_Id))
                throw new ArgumentException("The given object is not a parking space identification!",
                                            nameof(Object));

            return CompareTo((ParkingSpace_Id) Object);

        }

        #endregion

        #region CompareTo(ParkingSpaceId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingSpaceId">An object to compare with.</param>
        public Int32 CompareTo(ParkingSpace_Id ParkingSpaceId)
        {

            if ((Object) ParkingSpaceId == null)
                throw new ArgumentNullException(nameof(ParkingSpaceId),  "The given parking space identification must not be null!");

            // Compare the length of the ParkingSpaceIds
            var _Result = this.Length.CompareTo(ParkingSpaceId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, ParkingSpaceId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ParkingSpaceId> Members

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

            if (!(Object is ParkingSpace_Id))
                return false;

            return Equals((ParkingSpace_Id) Object);

        }

        #endregion

        #region Equals(ParkingSpaceId)

        /// <summary>
        /// Compares two ParkingSpaceIds for equality.
        /// </summary>
        /// <param name="ParkingSpaceId">A ParkingSpaceId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingSpace_Id ParkingSpaceId)
        {

            if ((Object) ParkingSpaceId == null)
                return false;

            return InternalId.Equals(ParkingSpaceId.InternalId);

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
            => InternalId ?? "";

        #endregion

    }

}
