/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for parking operator identifications.
    /// </summary>
    public static class ParkingOperatorIdExtensions
    {

        /// <summary>
        /// Indicates whether this parking operator identification is null or empty.
        /// </summary>
        /// <param name="ParkingOperatorId">A parking operator identification.</param>
        public static Boolean IsNullOrEmpty(this ParkingOperator_Id? ParkingOperatorId)
            => !ParkingOperatorId.HasValue || ParkingOperatorId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this parking operator identification is null or empty.
        /// </summary>
        /// <param name="ParkingOperatorId">A parking operator identification.</param>
        public static Boolean IsNotNullOrEmpty(this ParkingOperator_Id? ParkingOperatorId)
            => ParkingOperatorId.HasValue && ParkingOperatorId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a parking operator.
    /// </summary>
    public readonly struct ParkingOperator_Id : IId,
                                                IEquatable <ParkingOperator_Id>,
                                                IComparable<ParkingOperator_Id>

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
        /// The length of the parking operator identificator.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new parking operator identification.
        /// based on the given string.
        /// </summary>
        private ParkingOperator_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a parking operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking operator identification.</param>
        public static ParkingOperator_Id Parse(String Text)
        {

            if (TryParse(Text, out ParkingOperator_Id parkingOperatorId))
                return parkingOperatorId;

            throw new ArgumentException($"Invalid text representation of a parking operator identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a parking operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking operator identification.</param>
        public static ParkingOperator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ParkingOperator_Id parkingOperatorId))
                return parkingOperatorId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ParkingOperatorId)

        /// <summary>
        /// Parse the given string as a parking operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking operator identification.</param>
        /// <param name="ParkingOperatorId">The parsed parking operator identification.</param>
        public static Boolean TryParse(String Text, out ParkingOperator_Id ParkingOperatorId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ParkingOperatorId = new ParkingOperator_Id(Text);
                    return true;
                }
                catch
                { }
            }

            ParkingOperatorId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this parking operator identification.
        /// </summary>
        public ParkingOperator_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ParkingOperatorId1, ParkingOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId1">A parking operator identification.</param>
        /// <param name="ParkingOperatorId2">Another parking operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ParkingOperator_Id ParkingOperatorId1,
                                           ParkingOperator_Id ParkingOperatorId2)

            => ParkingOperatorId1.Equals(ParkingOperatorId2);

        #endregion

        #region Operator != (ParkingOperatorId1, ParkingOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId1">A parking operator identification.</param>
        /// <param name="ParkingOperatorId2">Another parking operator identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ParkingOperator_Id ParkingOperatorId1,
                                           ParkingOperator_Id ParkingOperatorId2)

            => !ParkingOperatorId1.Equals(ParkingOperatorId2);

        #endregion

        #region Operator <  (ParkingOperatorId1, ParkingOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId1">A parking operator identification.</param>
        /// <param name="ParkingOperatorId2">Another parking operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ParkingOperator_Id ParkingOperatorId1,
                                          ParkingOperator_Id ParkingOperatorId2)

            => ParkingOperatorId1.CompareTo(ParkingOperatorId2) < 0;

        #endregion

        #region Operator <= (ParkingOperatorId1, ParkingOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId1">A parking operator identification.</param>
        /// <param name="ParkingOperatorId2">Another parking operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ParkingOperator_Id ParkingOperatorId1,
                                           ParkingOperator_Id ParkingOperatorId2)

            => ParkingOperatorId1.CompareTo(ParkingOperatorId2) <= 0;

        #endregion

        #region Operator >  (ParkingOperatorId1, ParkingOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId1">A parking operator identification.</param>
        /// <param name="ParkingOperatorId2">Another parking operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ParkingOperator_Id ParkingOperatorId1,
                                          ParkingOperator_Id ParkingOperatorId2)

            => ParkingOperatorId1.CompareTo(ParkingOperatorId2) > 0;

        #endregion

        #region Operator >= (ParkingOperatorId1, ParkingOperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId1">A parking operator identification.</param>
        /// <param name="ParkingOperatorId2">Another parking operator identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ParkingOperator_Id ParkingOperatorId1,
                                           ParkingOperator_Id ParkingOperatorId2)

            => ParkingOperatorId1.CompareTo(ParkingOperatorId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ParkingOperator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ParkingOperator_Id parkingOperatorId
                   ? CompareTo(parkingOperatorId)
                   : throw new ArgumentException("The given object is not a parking operator identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ParkingOperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingOperatorId">An object to compare with.</param>
        public Int32 CompareTo(ParkingOperator_Id ParkingOperatorId)

            => String.Compare(InternalId,
                              ParkingOperatorId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ParkingOperator_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object? Object)

            => Object is ParkingOperator_Id parkingOperatorId &&
                   Equals(parkingOperatorId);

        #endregion

        #region Equals(ParkingOperatorId)

        /// <summary>
        /// Compares two parking operator identifications for equality.
        /// </summary>
        /// <param name="ParkingOperatorId">A parking operator identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingOperator_Id ParkingOperatorId)

            => String.Equals(InternalId,
                             ParkingOperatorId.InternalId,
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
