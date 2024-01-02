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
    /// Extension methods for energy meter status typess.
    /// </summary>
    public static class EnergyMeterStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this energy meter status types is null or empty.
        /// </summary>
        /// <param name="EnergyMeterStatusType">An energy meter status type.</param>
        public static Boolean IsNullOrEmpty(this EnergyMeterStatusTypes? EnergyMeterStatusType)
            => !EnergyMeterStatusType.HasValue || EnergyMeterStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this energy meter status types is null or empty.
        /// </summary>
        /// <param name="EnergyMeterStatusType">An energy meter status type.</param>
        public static Boolean IsNotNullOrEmpty(this EnergyMeterStatusTypes? EnergyMeterStatusType)
            => EnergyMeterStatusType.HasValue && EnergyMeterStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status type of an energy meter.
    /// </summary>
    public readonly struct EnergyMeterStatusTypes : IId,
                                                    IEquatable<EnergyMeterStatusTypes>,
                                                    IComparable<EnergyMeterStatusTypes>
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
        /// The length of the energy meter status.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter status types based on the given string.
        /// </summary>
        private EnergyMeterStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an energy meter status type.
        /// </summary>
        /// <param name="Text">A text representation of an energy meter status type.</param>
        public static EnergyMeterStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out EnergyMeterStatusTypes energyMeterStatusTypes))
                return energyMeterStatusTypes;

            throw new ArgumentException($"Invalid text representation of an energy meter status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an energy meter status type.
        /// </summary>
        /// <param name="Text">A text representation of an energy meter status type.</param>
        public static EnergyMeterStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out EnergyMeterStatusTypes energyMeterStatusTypes))
                return energyMeterStatusTypes;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EnergyMeterStatusType)

        /// <summary>
        /// Parse the given string as an energy meter status type.
        /// </summary>
        /// <param name="Text">A text representation of an energy meter status type.</param>
        /// <param name="EnergyMeterStatusType">The parsed energy meter status type.</param>
        public static Boolean TryParse(String Text, out EnergyMeterStatusTypes EnergyMeterStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EnergyMeterStatusType = new EnergyMeterStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            EnergyMeterStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this energy meter status type.
        /// </summary>
        public EnergyMeterStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the EnergyMeter.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Unknown           = new("unknown");

        /// <summary>
        /// Unclear status of the EnergyMeter.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Unspecified       = new("unspecified");

        /// <summary>
        /// Currently no communication with the EnergyMeter possible,
        /// but charging in offline mode might be available.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Offline           = new("offline");

        /// <summary>
        /// The EnergyMeter is not fully operational yet.
        /// </summary>
        public static readonly EnergyMeterStatusTypes InDeployment      = new("inDeployment");

        /// <summary>
        /// The EnergyMeter is reserved for future charging.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Reserved          = new("reserved");

        /// <summary>
        /// The EnergyMeter is available for charging.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Available         = new("available");

        /// <summary>
        /// Some ongoing charging sessions or reservations, but still ready to charge.
        /// </summary>
        public static readonly EnergyMeterStatusTypes PartialAvailable  = new("partialAvailable");

        /// <summary>
        /// The door of a charging locker is open, the EnergyMeter is unlocked
        /// and is waiting for the customer to plugin.
        /// </summary>
        public static readonly EnergyMeterStatusTypes WaitingForPlugin  = new("waitingForPlugin");

        /// <summary>
        /// A cable is plugged into the socket or a vehicle is connected
        /// to the cable, but both without any further action.
        /// </summary>
        public static readonly EnergyMeterStatusTypes PluggedIn         = new("pluggedIn");

        /// <summary>
        /// An ongoing charging process.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Charging          = new("charging");

        /// <summary>
        /// The EnergyMeter has a mechanical door, e.g. an e-bike charging locker,
        /// which was not closed after the customer took the battery out.
        /// </summary>
        public static readonly EnergyMeterStatusTypes DoorNotClosed     = new("doorNotClosed");

        /// <summary>
        /// A fatal error has occured within the EnergyMeter.
        /// </summary>
        public static readonly EnergyMeterStatusTypes Error             = new("error");

        /// <summary>
        /// The EnergyMeter is not ready for charging because it is under maintenance
        /// or was disabled by the energy meter operator.
        /// </summary>
        public static readonly EnergyMeterStatusTypes OutOfService      = new("outOfService");

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterStatusType1, EnergyMeterStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusType1">An energy meter status type.</param>
        /// <param name="EnergyMeterStatusType2">Another energy meter status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergyMeterStatusTypes EnergyMeterStatusType1,
                                           EnergyMeterStatusTypes EnergyMeterStatusType2)

            => EnergyMeterStatusType1.Equals(EnergyMeterStatusType2);

        #endregion

        #region Operator != (EnergyMeterStatusType1, EnergyMeterStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusType1">An energy meter status type.</param>
        /// <param name="EnergyMeterStatusType2">Another energy meter status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergyMeterStatusTypes EnergyMeterStatusType1,
                                           EnergyMeterStatusTypes EnergyMeterStatusType2)

            => !EnergyMeterStatusType1.Equals(EnergyMeterStatusType2);

        #endregion

        #region Operator <  (EnergyMeterStatusType1, EnergyMeterStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusType1">An energy meter status type.</param>
        /// <param name="EnergyMeterStatusType2">Another energy meter status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergyMeterStatusTypes EnergyMeterStatusType1,
                                          EnergyMeterStatusTypes EnergyMeterStatusType2)

            => EnergyMeterStatusType1.CompareTo(EnergyMeterStatusType2) < 0;

        #endregion

        #region Operator <= (EnergyMeterStatusType1, EnergyMeterStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusType1">An energy meter status type.</param>
        /// <param name="EnergyMeterStatusType2">Another energy meter status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergyMeterStatusTypes EnergyMeterStatusType1,
                                           EnergyMeterStatusTypes EnergyMeterStatusType2)

            => EnergyMeterStatusType1.CompareTo(EnergyMeterStatusType2) <= 0;

        #endregion

        #region Operator >  (EnergyMeterStatusType1, EnergyMeterStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusType1">An energy meter status type.</param>
        /// <param name="EnergyMeterStatusType2">Another energy meter status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergyMeterStatusTypes EnergyMeterStatusType1,
                                          EnergyMeterStatusTypes EnergyMeterStatusType2)

            => EnergyMeterStatusType1.CompareTo(EnergyMeterStatusType2) > 0;

        #endregion

        #region Operator >= (EnergyMeterStatusType1, EnergyMeterStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterStatusType1">An energy meter status type.</param>
        /// <param name="EnergyMeterStatusType2">Another energy meter status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergyMeterStatusTypes EnergyMeterStatusType1,
                                           EnergyMeterStatusTypes EnergyMeterStatusType2)

            => EnergyMeterStatusType1.CompareTo(EnergyMeterStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<EnergyMeterStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two energy meter status types.
        /// </summary>
        /// <param name="Object">An energy meter status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EnergyMeterStatusTypes energyMeterStatusTypes
                   ? CompareTo(energyMeterStatusTypes)
                   : throw new ArgumentException("The given object is not an energy meter status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EnergyMeterStatusType)

        /// <summary>
        /// Compares two energy meter status types.
        /// </summary>
        /// <param name="EnergyMeterStatusType">An energy meter status type to compare with.</param>
        public Int32 CompareTo(EnergyMeterStatusTypes EnergyMeterStatusType)

            => String.Compare(InternalId,
                              EnergyMeterStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EnergyMeterStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two energy meter status types for equality.
        /// </summary>
        /// <param name="Object">An energy meter status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EnergyMeterStatusTypes energyMeterStatusTypes &&
                   Equals(energyMeterStatusTypes);

        #endregion

        #region Equals(EnergyMeterStatusType)

        /// <summary>
        /// Compares two energy meter status types for equality.
        /// </summary>
        /// <param name="EnergyMeterStatusType">An energy meter status type to compare with.</param>
        public Boolean Equals(EnergyMeterStatusTypes EnergyMeterStatusType)

            => String.Equals(InternalId,
                             EnergyMeterStatusType.InternalId,
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
