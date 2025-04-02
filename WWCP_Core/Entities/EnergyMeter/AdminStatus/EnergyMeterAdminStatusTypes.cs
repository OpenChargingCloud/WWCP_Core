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
    /// Extension methods for energy meter admin status types.
    /// </summary>
    public static class EnergyMeterAdminStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this energy meter admin status types is null or empty.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusTypes">A energy meter admin status type.</param>
        public static Boolean IsNullOrEmpty(this EnergyMeterAdminStatusTypes? EnergyMeterAdminStatusTypes)
            => !EnergyMeterAdminStatusTypes.HasValue || EnergyMeterAdminStatusTypes.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this energy meter admin status types is null or empty.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusTypes">A energy meter admin status type.</param>
        public static Boolean IsNotNullOrEmpty(this EnergyMeterAdminStatusTypes? EnergyMeterAdminStatusTypes)
            => EnergyMeterAdminStatusTypes.HasValue && EnergyMeterAdminStatusTypes.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The admin status of a energy meter.
    /// </summary>
    public readonly struct EnergyMeterAdminStatusTypes : IId,
                                                             IEquatable<EnergyMeterAdminStatusTypes>,
                                                             IComparable<EnergyMeterAdminStatusTypes>
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
        /// The length of the energy meter admin status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter admin status type based on the given string.
        /// </summary>
        private EnergyMeterAdminStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a energy meter admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a energy meter admin status type.</param>
        public static EnergyMeterAdminStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out EnergyMeterAdminStatusTypes energyMeterAdminStatusTypes))
                return energyMeterAdminStatusTypes;

            throw new ArgumentException($"Invalid text representation of a energy meter admin status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a energy meter admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a energy meter admin status type.</param>
        public static EnergyMeterAdminStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out EnergyMeterAdminStatusTypes energyMeterAdminStatusTypes))
                return energyMeterAdminStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EnergyMeterAdminStatusType)

        /// <summary>
        /// Parse the given string as a energy meter admin status type.
        /// </summary>
        /// <param name="Text">A text representation of a energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType">The parsed energy meter admin status type.</param>
        public static Boolean TryParse(String Text, out EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EnergyMeterAdminStatusType = new EnergyMeterAdminStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            EnergyMeterAdminStatusType = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this energy meter admin status type.
        /// </summary>
        public EnergyMeterAdminStatusTypes Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unclear admin status of the energy meter.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes Unspecified   = new("unspecified");

        /// <summary>
        /// Unknown admin status of the energy meter.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes Unknown        = new("unknown");

        /// <summary>
        /// The energy meter is planned for the future.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes Planned       = new("planned");

        /// <summary>
        /// The energy meter is currently in deployment, but not fully operational yet.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes InDeployment  = new("inDeployment");

        /// <summary>
        /// Private or internal use.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes InternalUse   = new("internalUse");

        /// <summary>
        /// The energy meter is under maintenance.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes OutOfService  = new("outOfService");

        /// <summary>
        /// The energy meter is operational.
        /// </summary>
        public static readonly EnergyMeterAdminStatusTypes Operational   = new("operational");

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterAdminStatusType1, EnergyMeterAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType1">A energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType2">Another energy meter admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType1,
                                           EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType2)

            => EnergyMeterAdminStatusType1.Equals(EnergyMeterAdminStatusType2);

        #endregion

        #region Operator != (EnergyMeterAdminStatusType1, EnergyMeterAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType1">A energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType2">Another energy meter admin status type.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType1,
                                           EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType2)

            => !EnergyMeterAdminStatusType1.Equals(EnergyMeterAdminStatusType2);

        #endregion

        #region Operator <  (EnergyMeterAdminStatusType1, EnergyMeterAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType1">A energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType2">Another energy meter admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType1,
                                          EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType2)

            => EnergyMeterAdminStatusType1.CompareTo(EnergyMeterAdminStatusType2) < 0;

        #endregion

        #region Operator <= (EnergyMeterAdminStatusType1, EnergyMeterAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType1">A energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType2">Another energy meter admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType1,
                                           EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType2)

            => EnergyMeterAdminStatusType1.CompareTo(EnergyMeterAdminStatusType2) <= 0;

        #endregion

        #region Operator >  (EnergyMeterAdminStatusType1, EnergyMeterAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType1">A energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType2">Another energy meter admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType1,
                                          EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType2)

            => EnergyMeterAdminStatusType1.CompareTo(EnergyMeterAdminStatusType2) > 0;

        #endregion

        #region Operator >= (EnergyMeterAdminStatusType1, EnergyMeterAdminStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType1">A energy meter admin status type.</param>
        /// <param name="EnergyMeterAdminStatusType2">Another energy meter admin status type.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType1,
                                           EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType2)

            => EnergyMeterAdminStatusType1.CompareTo(EnergyMeterAdminStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergyMeterAdminStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meter admin status types.
        /// </summary>
        /// <param name="Object">A energy meter admin status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterAdminStatusTypes energyMeterAdminStatusTypes
                   ? CompareTo(energyMeterAdminStatusTypes)
                   : throw new ArgumentException("The given object is not a energy meter admin status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterAdminStatusType)

        /// <summary>
        /// Compares two energy meter admin status types.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType">A energy meter admin status type to compare with.</param>
        public Int32 CompareTo(EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType)

            => String.Compare(InternalId,
                              EnergyMeterAdminStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EnergyMeterAdminStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meter admin status types for equality.
        /// </summary>
        /// <param name="Object">A energy meter admin status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterAdminStatusTypes energyMeterAdminStatusTypes &&
                   Equals(energyMeterAdminStatusTypes);

        #endregion

        #region Equals(EnergyMeterAdminStatusType)

        /// <summary>
        /// Compares two energy meter admin status types for equality.
        /// </summary>
        /// <param name="EnergyMeterAdminStatusType">A energy meter admin status type to compare with.</param>
        public Boolean Equals(EnergyMeterAdminStatusTypes EnergyMeterAdminStatusType)

            => String.Equals(InternalId,
                             EnergyMeterAdminStatusType.InternalId,
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
