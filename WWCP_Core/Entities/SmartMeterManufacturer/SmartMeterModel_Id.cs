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

namespace cloud.charging.open.protocols.WWCP.SMM
{

    /// <summary>
    /// Extension methods for smart meter model identifications.
    /// </summary>
    public static class SmartMeterModelIdExtensions
    {

        /// <summary>
        /// Indicates whether this smart meter model identification is null or empty.
        /// </summary>
        /// <param name="SmartMeterModelId">A smart meter model identification.</param>
        public static Boolean IsNullOrEmpty(this SmartMeterModel_Id? SmartMeterModelId)
            => !SmartMeterModelId.HasValue || SmartMeterModelId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this smart meter model identification is NOT null or empty.
        /// </summary>
        /// <param name="SmartMeterModelId">A smart meter model identification.</param>
        public static Boolean IsNotNullOrEmpty(this SmartMeterModel_Id? SmartMeterModelId)
            => SmartMeterModelId.HasValue && SmartMeterModelId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a smart meter model.
    /// </summary>
    public readonly struct SmartMeterModel_Id : IId<SmartMeterModel_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this smart meter model identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this smart meter model identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the smart meter model identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new smart meter model identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a smart meter model identification.</param>
        private SmartMeterModel_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random smart meter model identification.
        /// </summary>
        /// <param name="Length">The expected length of the smart meter model identification.</param>
        public static SmartMeterModel_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a smart meter model identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter model identification.</param>
        public static SmartMeterModel_Id Parse(String Text)
        {

            if (TryParse(Text, out var smartMeterModelId))
                return smartMeterModelId;

            throw new ArgumentException($"Invalid text representation of a smart meter model identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a smart meter model identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter model identification.</param>
        public static SmartMeterModel_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var smartMeterModelId))
                return smartMeterModelId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out SmartMeterModelId)

        /// <summary>
        /// Try to parse the given text as a smart meter model identification.
        /// </summary>
        /// <param name="Text">A text representation of a smart meter model identification.</param>
        /// <param name="SmartMeterModelId">The parsed smart meter model identification.</param>
        public static Boolean TryParse(String Text, out SmartMeterModel_Id SmartMeterModelId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    SmartMeterModelId = new SmartMeterModel_Id(Text);
                    return true;
                }
                catch
                { }
            }

            SmartMeterModelId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this smart meter model identification.
        /// </summary>
        public SmartMeterModel_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (SmartMeterModelId1, SmartMeterModelId2)

        /// <summary>
        /// Compares two smart meter model identifications for equality.
        /// </summary>
        /// <param name="SmartMeterModelId1">A smart meter model identification.</param>
        /// <param name="SmartMeterModelId2">Another smart meter model identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SmartMeterModel_Id SmartMeterModelId1,
                                           SmartMeterModel_Id SmartMeterModelId2)

            => SmartMeterModelId1.Equals(SmartMeterModelId2);

        #endregion

        #region Operator != (SmartMeterModelId1, SmartMeterModelId2)

        /// <summary>
        /// Compares two smart meter model identifications for inequality.
        /// </summary>
        /// <param name="SmartMeterModelId1">A smart meter model identification.</param>
        /// <param name="SmartMeterModelId2">Another smart meter model identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SmartMeterModel_Id SmartMeterModelId1,
                                           SmartMeterModel_Id SmartMeterModelId2)

            => !SmartMeterModelId1.Equals(SmartMeterModelId2);

        #endregion

        #region Operator <  (SmartMeterModelId1, SmartMeterModelId2)

        /// <summary>
        /// Compares two smart meter model identifications.
        /// </summary>
        /// <param name="SmartMeterModelId1">A smart meter model identification.</param>
        /// <param name="SmartMeterModelId2">Another smart meter model identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SmartMeterModel_Id SmartMeterModelId1,
                                          SmartMeterModel_Id SmartMeterModelId2)

            => SmartMeterModelId1.CompareTo(SmartMeterModelId2) < 0;

        #endregion

        #region Operator <= (SmartMeterModelId1, SmartMeterModelId2)

        /// <summary>
        /// Compares two smart meter model identifications.
        /// </summary>
        /// <param name="SmartMeterModelId1">A smart meter model identification.</param>
        /// <param name="SmartMeterModelId2">Another smart meter model identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SmartMeterModel_Id SmartMeterModelId1,
                                           SmartMeterModel_Id SmartMeterModelId2)

            => SmartMeterModelId1.CompareTo(SmartMeterModelId2) <= 0;

        #endregion

        #region Operator >  (SmartMeterModelId1, SmartMeterModelId2)

        /// <summary>
        /// Compares two smart meter model identifications.
        /// </summary>
        /// <param name="SmartMeterModelId1">A smart meter model identification.</param>
        /// <param name="SmartMeterModelId2">Another smart meter model identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SmartMeterModel_Id SmartMeterModelId1,
                                          SmartMeterModel_Id SmartMeterModelId2)

            => SmartMeterModelId1.CompareTo(SmartMeterModelId2) > 0;

        #endregion

        #region Operator >= (SmartMeterModelId1, SmartMeterModelId2)

        /// <summary>
        /// Compares two smart meter model identifications.
        /// </summary>
        /// <param name="SmartMeterModelId1">A smart meter model identification.</param>
        /// <param name="SmartMeterModelId2">Another smart meter model identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SmartMeterModel_Id SmartMeterModelId1,
                                           SmartMeterModel_Id SmartMeterModelId2)

            => SmartMeterModelId1.CompareTo(SmartMeterModelId2) >= 0;

        #endregion

        #endregion

        #region IComparable<SmartMeterModelId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two smart meter model identifications.
        /// </summary>
        /// <param name="Object">A smart meter model identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is SmartMeterModel_Id smartMeterModelId
                   ? CompareTo(smartMeterModelId)
                   : throw new ArgumentException("The given object is not a smart meter model identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(SmartMeterModelId)

        /// <summary>
        /// Compares two smart meter model identifications.
        /// </summary>
        /// <param name="SmartMeterModelId">A smart meter model identification to compare with.</param>
        public Int32 CompareTo(SmartMeterModel_Id SmartMeterModelId)

            => String.Compare(InternalId,
                              SmartMeterModelId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<SmartMeterModelId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two smart meter model identifications for equality.
        /// </summary>
        /// <param name="Object">A smart meter model identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SmartMeterModel_Id smartMeterModelId &&
                   Equals(smartMeterModelId);

        #endregion

        #region Equals(SmartMeterModelId)

        /// <summary>
        /// Compares two smart meter model identifications for equality.
        /// </summary>
        /// <param name="SmartMeterModelId">A smart meter model identification to compare with.</param>
        public Boolean Equals(SmartMeterModel_Id SmartMeterModelId)

            => String.Equals(InternalId,
                             SmartMeterModelId.InternalId,
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
