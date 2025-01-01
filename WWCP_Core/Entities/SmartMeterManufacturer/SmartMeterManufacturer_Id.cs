/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.SMM
{

    /// <summary>
    /// Extension methods for smart meter manufacturer identifications.
    /// </summary>
    public static class SmartMeterManufacturerIdExtensions
    {

        /// <summary>
        /// Indicates whether this smart meter manufacturer identification is null or empty.
        /// </summary>
        /// <param name="SmartMeterManufacturerId">A smart meter manufacturer identification.</param>
        public static Boolean IsNullOrEmpty(this SmartMeterManufacturer_Id? SmartMeterManufacturerId)
            => !SmartMeterManufacturerId.HasValue || SmartMeterManufacturerId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this smart meter manufacturer identification is NOT null or empty.
        /// </summary>
        /// <param name="SmartMeterManufacturerId">A smart meter manufacturer identification.</param>
        public static Boolean IsNotNullOrEmpty(this SmartMeterManufacturer_Id? SmartMeterManufacturerId)
            => SmartMeterManufacturerId.HasValue && SmartMeterManufacturerId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a smart meter manufacturer.
    /// </summary>
    public readonly struct SmartMeterManufacturer_Id : IId<SmartMeterManufacturer_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this smart meter manufacturer identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this smart meter manufacturer identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the smart meter manufacturer identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart meter manufacturer identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a smart meter manufacturer identification.</param>
        private SmartMeterManufacturer_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random smart meter manufacturer identification.
        /// </summary>
        /// <param name="Length">The expected length of the smart meter manufacturer identification.</param>
        public static SmartMeterManufacturer_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a smart meter manufacturer identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter manufacturer identification.</param>
        public static SmartMeterManufacturer_Id Parse(String Text)
        {

            if (TryParse(Text, out var smartMeterManufacturerId))
                return smartMeterManufacturerId;

            throw new ArgumentException($"Invalid text representation of a smart meter manufacturer identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a smart meter manufacturer identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter manufacturer identification.</param>
        public static SmartMeterManufacturer_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var smartMeterManufacturerId))
                return smartMeterManufacturerId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out SmartMeterManufacturerId)

        /// <summary>
        /// Try to parse the given text as a smart meter manufacturer identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId">The parsed smart meter manufacturer identification.</param>
        public static Boolean TryParse(String Text, out SmartMeterManufacturer_Id SmartMeterManufacturerId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    SmartMeterManufacturerId = new SmartMeterManufacturer_Id(Text);
                    return true;
                }
                catch
                { }
            }

            SmartMeterManufacturerId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this smart meter manufacturer identification.
        /// </summary>
        public SmartMeterManufacturer_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SmartMeterManufacturerId1, SmartMeterManufacturerId2)

        /// <summary>
        /// Compares two smart meter manufacturer identifications for equality.
        /// </summary>
        /// <param name="SmartMeterManufacturerId1">A smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId2">Another smart meter manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SmartMeterManufacturer_Id SmartMeterManufacturerId1,
                                           SmartMeterManufacturer_Id SmartMeterManufacturerId2)

            => SmartMeterManufacturerId1.Equals(SmartMeterManufacturerId2);

        #endregion

        #region Operator != (SmartMeterManufacturerId1, SmartMeterManufacturerId2)

        /// <summary>
        /// Compares two smart meter manufacturer identifications for inequality.
        /// </summary>
        /// <param name="SmartMeterManufacturerId1">A smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId2">Another smart meter manufacturer identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SmartMeterManufacturer_Id SmartMeterManufacturerId1,
                                           SmartMeterManufacturer_Id SmartMeterManufacturerId2)

            => !SmartMeterManufacturerId1.Equals(SmartMeterManufacturerId2);

        #endregion

        #region Operator <  (SmartMeterManufacturerId1, SmartMeterManufacturerId2)

        /// <summary>
        /// Compares two smart meter manufacturer identifications.
        /// </summary>
        /// <param name="SmartMeterManufacturerId1">A smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId2">Another smart meter manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (SmartMeterManufacturer_Id SmartMeterManufacturerId1,
                                          SmartMeterManufacturer_Id SmartMeterManufacturerId2)

            => SmartMeterManufacturerId1.CompareTo(SmartMeterManufacturerId2) < 0;

        #endregion

        #region Operator <= (SmartMeterManufacturerId1, SmartMeterManufacturerId2)

        /// <summary>
        /// Compares two smart meter manufacturer identifications.
        /// </summary>
        /// <param name="SmartMeterManufacturerId1">A smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId2">Another smart meter manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (SmartMeterManufacturer_Id SmartMeterManufacturerId1,
                                           SmartMeterManufacturer_Id SmartMeterManufacturerId2)

            => SmartMeterManufacturerId1.CompareTo(SmartMeterManufacturerId2) <= 0;

        #endregion

        #region Operator >  (SmartMeterManufacturerId1, SmartMeterManufacturerId2)

        /// <summary>
        /// Compares two smart meter manufacturer identifications.
        /// </summary>
        /// <param name="SmartMeterManufacturerId1">A smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId2">Another smart meter manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (SmartMeterManufacturer_Id SmartMeterManufacturerId1,
                                          SmartMeterManufacturer_Id SmartMeterManufacturerId2)

            => SmartMeterManufacturerId1.CompareTo(SmartMeterManufacturerId2) > 0;

        #endregion

        #region Operator >= (SmartMeterManufacturerId1, SmartMeterManufacturerId2)

        /// <summary>
        /// Compares two smart meter manufacturer identifications.
        /// </summary>
        /// <param name="SmartMeterManufacturerId1">A smart meter manufacturer identification.</param>
        /// <param name="SmartMeterManufacturerId2">Another smart meter manufacturer identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (SmartMeterManufacturer_Id SmartMeterManufacturerId1,
                                           SmartMeterManufacturer_Id SmartMeterManufacturerId2)

            => SmartMeterManufacturerId1.CompareTo(SmartMeterManufacturerId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SmartMeterManufacturerId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two smart meter manufacturer identifications.
        /// </summary>
        /// <param name="Object">A smart meter manufacturer identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SmartMeterManufacturer_Id smartMeterManufacturerId
                   ? CompareTo(smartMeterManufacturerId)
                   : throw new ArgumentException("The given object is not a smart meter manufacturer identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SmartMeterManufacturerId)

        /// <summary>
        /// Compares two smart meter manufacturer identifications.
        /// </summary>
        /// <param name="SmartMeterManufacturerId">A smart meter manufacturer identification to compare with.</param>
        public Int32 CompareTo(SmartMeterManufacturer_Id SmartMeterManufacturerId)

            => String.Compare(InternalId,
                              SmartMeterManufacturerId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SmartMeterManufacturerId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two smart meter manufacturer identifications for equality.
        /// </summary>
        /// <param name="Object">A smart meter manufacturer identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SmartMeterManufacturer_Id smartMeterManufacturerId &&
                   Equals(smartMeterManufacturerId);

        #endregion

        #region Equals(SmartMeterManufacturerId)

        /// <summary>
        /// Compares two smart meter manufacturer identifications for equality.
        /// </summary>
        /// <param name="SmartMeterManufacturerId">A smart meter manufacturer identification to compare with.</param>
        public Boolean Equals(SmartMeterManufacturer_Id SmartMeterManufacturerId)

            => String.Equals(InternalId,
                             SmartMeterManufacturerId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

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
