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

namespace cloud.charging.open.protocols.WWCP.MCL
{

    /// <summary>
    /// Extension methods for metering calibration law agency identifications.
    /// </summary>
    public static class MeteringCalibrationLawAgencyIdExtensions
    {

        /// <summary>
        /// Indicates whether this metering calibration law agency identification is null or empty.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId">A metering calibration law agency identification.</param>
        public static Boolean IsNullOrEmpty(this MeteringCalibrationLawAgency_Id? MeteringCalibrationLawAgencyId)
            => !MeteringCalibrationLawAgencyId.HasValue || MeteringCalibrationLawAgencyId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this metering calibration law agency identification is NOT null or empty.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId">A metering calibration law agency identification.</param>
        public static Boolean IsNotNullOrEmpty(this MeteringCalibrationLawAgency_Id? MeteringCalibrationLawAgencyId)
            => MeteringCalibrationLawAgencyId.HasValue && MeteringCalibrationLawAgencyId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a metering calibration law agency.
    /// </summary>
    public readonly struct MeteringCalibrationLawAgency_Id : IId<MeteringCalibrationLawAgency_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this metering calibration law agency identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this metering calibration law agency identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the metering calibration law agency identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new metering calibration law agency identification based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a metering calibration law agency identification.</param>
        private MeteringCalibrationLawAgency_Id(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) NewRandom(Length = 50)

        /// <summary>
        /// Create a new random metering calibration law agency identification.
        /// </summary>
        /// <param name="Length">The expected length of the metering calibration law agency identification.</param>
        public static MeteringCalibrationLawAgency_Id NewRandom(Byte Length = 50)

            => new(RandomExtensions.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a metering calibration law agency identification.
        /// </summary>
        /// <param name="Text">A text representation of a metering calibration law agency identification.</param>
        public static MeteringCalibrationLawAgency_Id Parse(String Text)
        {

            if (TryParse(Text, out var meteringCalibrationLawAgencyId))
                return meteringCalibrationLawAgencyId;

            throw new ArgumentException($"Invalid text representation of a metering calibration law agency identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a metering calibration law agency identification.
        /// </summary>
        /// <param name="Text">A text representation of a metering calibration law agency identification.</param>
        public static MeteringCalibrationLawAgency_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var meteringCalibrationLawAgencyId))
                return meteringCalibrationLawAgencyId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out MeteringCalibrationLawAgencyId)

        /// <summary>
        /// Try to parse the given text as a metering calibration law agency identification.
        /// </summary>
        /// <param name="Text">A text representation of a metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId">The parsed metering calibration law agency identification.</param>
        public static Boolean TryParse(String Text, out MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    MeteringCalibrationLawAgencyId = new MeteringCalibrationLawAgency_Id(Text);
                    return true;
                }
                catch
                { }
            }

            MeteringCalibrationLawAgencyId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this metering calibration law agency identification.
        /// </summary>
        public MeteringCalibrationLawAgency_Id Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (MeteringCalibrationLawAgencyId1, MeteringCalibrationLawAgencyId2)

        /// <summary>
        /// Compares two metering calibration law agency identifications for equality.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId1">A metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId2">Another metering calibration law agency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId1,
                                           MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId2)

            => MeteringCalibrationLawAgencyId1.Equals(MeteringCalibrationLawAgencyId2);

        #endregion

        #region Operator != (MeteringCalibrationLawAgencyId1, MeteringCalibrationLawAgencyId2)

        /// <summary>
        /// Compares two metering calibration law agency identifications for inequality.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId1">A metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId2">Another metering calibration law agency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId1,
                                           MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId2)

            => !MeteringCalibrationLawAgencyId1.Equals(MeteringCalibrationLawAgencyId2);

        #endregion

        #region Operator <  (MeteringCalibrationLawAgencyId1, MeteringCalibrationLawAgencyId2)

        /// <summary>
        /// Compares two metering calibration law agency identifications.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId1">A metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId2">Another metering calibration law agency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId1,
                                          MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId2)

            => MeteringCalibrationLawAgencyId1.CompareTo(MeteringCalibrationLawAgencyId2) < 0;

        #endregion

        #region Operator <= (MeteringCalibrationLawAgencyId1, MeteringCalibrationLawAgencyId2)

        /// <summary>
        /// Compares two metering calibration law agency identifications.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId1">A metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId2">Another metering calibration law agency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId1,
                                           MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId2)

            => MeteringCalibrationLawAgencyId1.CompareTo(MeteringCalibrationLawAgencyId2) <= 0;

        #endregion

        #region Operator >  (MeteringCalibrationLawAgencyId1, MeteringCalibrationLawAgencyId2)

        /// <summary>
        /// Compares two metering calibration law agency identifications.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId1">A metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId2">Another metering calibration law agency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId1,
                                          MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId2)

            => MeteringCalibrationLawAgencyId1.CompareTo(MeteringCalibrationLawAgencyId2) > 0;

        #endregion

        #region Operator >= (MeteringCalibrationLawAgencyId1, MeteringCalibrationLawAgencyId2)

        /// <summary>
        /// Compares two metering calibration law agency identifications.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId1">A metering calibration law agency identification.</param>
        /// <param name="MeteringCalibrationLawAgencyId2">Another metering calibration law agency identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId1,
                                           MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId2)

            => MeteringCalibrationLawAgencyId1.CompareTo(MeteringCalibrationLawAgencyId2) >= 0;

        #endregion

        #endregion

        #region IComparable<MeteringCalibrationLawAgencyId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two metering calibration law agency identifications.
        /// </summary>
        /// <param name="Object">A metering calibration law agency identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is MeteringCalibrationLawAgency_Id meteringCalibrationLawAgencyId
                   ? CompareTo(meteringCalibrationLawAgencyId)
                   : throw new ArgumentException("The given object is not a metering calibration law agency identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(MeteringCalibrationLawAgencyId)

        /// <summary>
        /// Compares two metering calibration law agency identifications.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId">A metering calibration law agency identification to compare with.</param>
        public Int32 CompareTo(MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId)

            => String.Compare(InternalId,
                              MeteringCalibrationLawAgencyId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<MeteringCalibrationLawAgencyId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two metering calibration law agency identifications for equality.
        /// </summary>
        /// <param name="Object">A metering calibration law agency identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is MeteringCalibrationLawAgency_Id meteringCalibrationLawAgencyId &&
                   Equals(meteringCalibrationLawAgencyId);

        #endregion

        #region Equals(MeteringCalibrationLawAgencyId)

        /// <summary>
        /// Compares two metering calibration law agency identifications for equality.
        /// </summary>
        /// <param name="MeteringCalibrationLawAgencyId">A metering calibration law agency identification to compare with.</param>
        public Boolean Equals(MeteringCalibrationLawAgency_Id MeteringCalibrationLawAgencyId)

            => String.Equals(InternalId,
                             MeteringCalibrationLawAgencyId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

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
