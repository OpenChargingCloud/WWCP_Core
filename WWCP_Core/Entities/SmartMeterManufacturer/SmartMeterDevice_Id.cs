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
    /// Extension methods for smart meter device identifications.
    /// </summary>
    public static class SmartMeterDeviceIdExtensions
    {

        /// <summary>
        /// Indicates whether this smart meter device identification is null or empty.
        /// </summary>
        /// <param name="SmartMeterDeviceId">A smart meter device identification.</param>
        public static Boolean IsNullOrEmpty(this SmartMeterDevice_Id? SmartMeterDeviceId)
            => !SmartMeterDeviceId.HasValue || SmartMeterDeviceId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this smart meter device identification is NOT null or empty.
        /// </summary>
        /// <param name="SmartMeterDeviceId">A smart meter device identification.</param>
        public static Boolean IsNotNullOrEmpty(this SmartMeterDevice_Id? SmartMeterDeviceId)
            => SmartMeterDeviceId.HasValue && SmartMeterDeviceId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a smart meter.
    /// </summary>
    public readonly struct SmartMeterDevice_Id : IId<SmartMeterDevice_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this smart meter device identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this smart meter device identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the smart meter device identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart meter device identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a smart meter device identification.</param>
        private SmartMeterDevice_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random smart meter device identification.
        /// </summary>
        /// <param name="Length">The expected length of the smart meter device identification.</param>
        public static SmartMeterDevice_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a smart meter device identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter device identification.</param>
        public static SmartMeterDevice_Id Parse(String Text)
        {

            if (TryParse(Text, out var smartMeterDeviceId))
                return smartMeterDeviceId;

            throw new ArgumentException($"Invalid text representation of a smart meter device identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a smart meter device identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter device identification.</param>
        public static SmartMeterDevice_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var smartMeterDeviceId))
                return smartMeterDeviceId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out SmartMeterDeviceId)

        /// <summary>
        /// Try to parse the given text as a smart meter device identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId">The parsed smart meter device identification.</param>
        public static Boolean TryParse(String Text, out SmartMeterDevice_Id SmartMeterDeviceId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    SmartMeterDeviceId = new SmartMeterDevice_Id(Text);
                    return true;
                }
                catch
                { }
            }

            SmartMeterDeviceId = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this smart meter device identification.
        /// </summary>
        public SmartMeterDevice_Id Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (SmartMeterDeviceId1, SmartMeterDeviceId2)

        /// <summary>
        /// Compares two smart meter device identifications for equality.
        /// </summary>
        /// <param name="SmartMeterDeviceId1">A smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId2">Another smart meter device identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SmartMeterDevice_Id SmartMeterDeviceId1,
                                           SmartMeterDevice_Id SmartMeterDeviceId2)

            => SmartMeterDeviceId1.Equals(SmartMeterDeviceId2);

        #endregion

        #region Operator != (SmartMeterDeviceId1, SmartMeterDeviceId2)

        /// <summary>
        /// Compares two smart meter device identifications for inequality.
        /// </summary>
        /// <param name="SmartMeterDeviceId1">A smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId2">Another smart meter device identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SmartMeterDevice_Id SmartMeterDeviceId1,
                                           SmartMeterDevice_Id SmartMeterDeviceId2)

            => !SmartMeterDeviceId1.Equals(SmartMeterDeviceId2);

        #endregion

        #region Operator <  (SmartMeterDeviceId1, SmartMeterDeviceId2)

        /// <summary>
        /// Compares two smart meter device identifications.
        /// </summary>
        /// <param name="SmartMeterDeviceId1">A smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId2">Another smart meter device identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (SmartMeterDevice_Id SmartMeterDeviceId1,
                                          SmartMeterDevice_Id SmartMeterDeviceId2)

            => SmartMeterDeviceId1.CompareTo(SmartMeterDeviceId2) < 0;

        #endregion

        #region Operator <= (SmartMeterDeviceId1, SmartMeterDeviceId2)

        /// <summary>
        /// Compares two smart meter device identifications.
        /// </summary>
        /// <param name="SmartMeterDeviceId1">A smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId2">Another smart meter device identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (SmartMeterDevice_Id SmartMeterDeviceId1,
                                           SmartMeterDevice_Id SmartMeterDeviceId2)

            => SmartMeterDeviceId1.CompareTo(SmartMeterDeviceId2) <= 0;

        #endregion

        #region Operator >  (SmartMeterDeviceId1, SmartMeterDeviceId2)

        /// <summary>
        /// Compares two smart meter device identifications.
        /// </summary>
        /// <param name="SmartMeterDeviceId1">A smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId2">Another smart meter device identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (SmartMeterDevice_Id SmartMeterDeviceId1,
                                          SmartMeterDevice_Id SmartMeterDeviceId2)

            => SmartMeterDeviceId1.CompareTo(SmartMeterDeviceId2) > 0;

        #endregion

        #region Operator >= (SmartMeterDeviceId1, SmartMeterDeviceId2)

        /// <summary>
        /// Compares two smart meter device identifications.
        /// </summary>
        /// <param name="SmartMeterDeviceId1">A smart meter device identification.</param>
        /// <param name="SmartMeterDeviceId2">Another smart meter device identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (SmartMeterDevice_Id SmartMeterDeviceId1,
                                           SmartMeterDevice_Id SmartMeterDeviceId2)

            => SmartMeterDeviceId1.CompareTo(SmartMeterDeviceId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SmartMeterDeviceId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two smart meter device identifications.
        /// </summary>
        /// <param name="Object">A smart meter device identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SmartMeterDevice_Id smartMeterDeviceId
                   ? CompareTo(smartMeterDeviceId)
                   : throw new ArgumentException("The given object is not a smart meter device identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SmartMeterDeviceId)

        /// <summary>
        /// Compares two smart meter device identifications.
        /// </summary>
        /// <param name="SmartMeterDeviceId">A smart meter device identification to compare with.</param>
        public Int32 CompareTo(SmartMeterDevice_Id SmartMeterDeviceId)

            => String.Compare(InternalId,
                              SmartMeterDeviceId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SmartMeterDeviceId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two smart meter device identifications for equality.
        /// </summary>
        /// <param name="Object">A smart meter device identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SmartMeterDevice_Id smartMeterDeviceId &&
                   Equals(smartMeterDeviceId);

        #endregion

        #region Equals(SmartMeterDeviceId)

        /// <summary>
        /// Compares two smart meter device identifications for equality.
        /// </summary>
        /// <param name="SmartMeterDeviceId">A smart meter device identification to compare with.</param>
        public Boolean Equals(SmartMeterDevice_Id SmartMeterDeviceId)

            => String.Equals(InternalId,
                             SmartMeterDeviceId.InternalId,
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
