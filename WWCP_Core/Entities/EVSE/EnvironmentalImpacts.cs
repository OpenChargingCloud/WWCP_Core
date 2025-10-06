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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for environmental impacts.
    /// </summary>
    public static class EnvironmentalImpactsExtensions
    {

        /// <summary>
        /// Indicates whether this environmental impacts is null or empty.
        /// </summary>
        /// <param name="EnvironmentalImpact">A environmental impact.</param>
        public static Boolean IsNullOrEmpty(this EnvironmentalImpacts? EnvironmentalImpact)
            => !EnvironmentalImpact.HasValue || EnvironmentalImpact.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this environmental impacts is null or empty.
        /// </summary>
        /// <param name="EnvironmentalImpact">A environmental impact.</param>
        public static Boolean IsNotNullOrEmpty(this EnvironmentalImpacts? EnvironmentalImpact)
            => EnvironmentalImpact.HasValue && EnvironmentalImpact.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status type of an EVSE.
    /// </summary>
    public readonly struct EnvironmentalImpacts : IId,
                                                  IEquatable <EnvironmentalImpacts>,
                                                  IComparable<EnvironmentalImpacts>
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
        /// The length of the EVSE admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new environmental impact based on the given string.
        /// </summary>
        private EnvironmentalImpacts(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an environmental impact.
        /// </summary>
        /// <param name="Text">A text representation of an environmental impact.</param>
        public static EnvironmentalImpacts Parse(String Text)
        {

            if (TryParse(Text, out EnvironmentalImpacts environmentalImpacts))
                return environmentalImpacts;

            throw new ArgumentException($"Invalid text representation of an environmental impact: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an environmental impact.
        /// </summary>
        /// <param name="Text">A text representation of an environmental impact.</param>
        public static EnvironmentalImpacts? TryParse(String Text)
        {

            if (TryParse(Text, out EnvironmentalImpacts environmentalImpacts))
                return environmentalImpacts;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EnvironmentalImpact)

        /// <summary>
        /// Parse the given string as an environmental impact.
        /// </summary>
        /// <param name="Text">A text representation of an environmental impact.</param>
        /// <param name="EnvironmentalImpact">The parsed environmental impact.</param>
        public static Boolean TryParse(String Text, out EnvironmentalImpacts EnvironmentalImpact)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EnvironmentalImpact = new EnvironmentalImpacts(Text);
                    return true;
                }
                catch
                { }
            }

            EnvironmentalImpact = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this environmental impact.
        /// </summary>
        public EnvironmentalImpacts Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static members

        /// <summary>
        /// Nuclear waste.
        /// </summary>
        public static readonly EnvironmentalImpacts  NuclearWaste    = new("NuclearWaste");

        /// <summary>
        /// Carbon dioxide.
        /// </summary>
        public static readonly EnvironmentalImpacts  CarbonDioxide   = new("CarbonDioxide");

        #endregion


        #region Operator overloading

        #region Operator == (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">A environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnvironmentalImpacts EnvironmentalImpact1,
                                           EnvironmentalImpacts EnvironmentalImpact2)

            => EnvironmentalImpact1.Equals(EnvironmentalImpact2);

        #endregion

        #region Operator != (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">A environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnvironmentalImpacts EnvironmentalImpact1,
                                           EnvironmentalImpacts EnvironmentalImpact2)

            => !EnvironmentalImpact1.Equals(EnvironmentalImpact2);

        #endregion

        #region Operator <  (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">A environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnvironmentalImpacts EnvironmentalImpact1,
                                          EnvironmentalImpacts EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) < 0;

        #endregion

        #region Operator <= (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">A environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnvironmentalImpacts EnvironmentalImpact1,
                                           EnvironmentalImpacts EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) <= 0;

        #endregion

        #region Operator >  (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">A environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnvironmentalImpacts EnvironmentalImpact1,
                                          EnvironmentalImpacts EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) > 0;

        #endregion

        #region Operator >= (EnvironmentalImpact1, EnvironmentalImpact2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnvironmentalImpact1">A environmental impact.</param>
        /// <param name="EnvironmentalImpact2">Another environmental impact.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnvironmentalImpacts EnvironmentalImpact1,
                                           EnvironmentalImpacts EnvironmentalImpact2)

            => EnvironmentalImpact1.CompareTo(EnvironmentalImpact2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnvironmentalImpacts> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two environmental impacts.
        /// </summary>
        /// <param name="Object">A environmental impact to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnvironmentalImpacts environmentalImpacts
                   ? CompareTo(environmentalImpacts)
                   : throw new ArgumentException("The given object is not an environmental impact!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnvironmentalImpact)

        /// <summary>
        /// Compares two environmental impacts.
        /// </summary>
        /// <param name="EnvironmentalImpact">A environmental impact to compare with.</param>
        public Int32 CompareTo(EnvironmentalImpacts EnvironmentalImpact)

            => String.Compare(InternalId,
                              EnvironmentalImpact.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EnvironmentalImpacts> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two environmental impacts for equality.
        /// </summary>
        /// <param name="Object">A environmental impact to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnvironmentalImpacts environmentalImpacts &&
                   Equals(environmentalImpacts);

        #endregion

        #region Equals(EnvironmentalImpact)

        /// <summary>
        /// Compares two environmental impacts for equality.
        /// </summary>
        /// <param name="EnvironmentalImpact">A environmental impact to compare with.</param>
        public Boolean Equals(EnvironmentalImpacts EnvironmentalImpact)

            => String.Equals(InternalId,
                             EnvironmentalImpact.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
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
