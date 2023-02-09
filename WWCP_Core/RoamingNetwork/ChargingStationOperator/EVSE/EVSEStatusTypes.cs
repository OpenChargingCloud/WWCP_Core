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
    /// Extension methods for EVSE status typess.
    /// </summary>
    public static class EVSEStatusTypesExtensions
    {

        /// <summary>
        /// Indicates whether this EVSE status types is null or empty.
        /// </summary>
        /// <param name="EVSEStatusType">A EVSE status type.</param>
        public static Boolean IsNullOrEmpty(this EVSEStatusTypes? EVSEStatusType)
            => !EVSEStatusType.HasValue || EVSEStatusType.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this EVSE status types is null or empty.
        /// </summary>
        /// <param name="EVSEStatusType">A EVSE status type.</param>
        public static Boolean IsNotNullOrEmpty(this EVSEStatusTypes? EVSEStatusType)
            => EVSEStatusType.HasValue && EVSEStatusType.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The status type of a EVSE.
    /// </summary>
    public readonly struct EVSEStatusTypes : IId,
                                             IEquatable<EVSEStatusTypes>,
                                             IComparable<EVSEStatusTypes>
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
        /// The length of the EVSE status.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE status types based on the given string.
        /// </summary>
        private EVSEStatusTypes(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of a EVSE status type.</param>
        public static EVSEStatusTypes Parse(String Text)
        {

            if (TryParse(Text, out EVSEStatusTypes evseStatusType))
                return evseStatusType;

            throw new ArgumentException("Invalid text representation of a EVSE status type: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as a EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of a EVSE status type.</param>
        public static EVSEStatusTypes? TryParse(String Text)
        {

            if (TryParse(Text, out EVSEStatusTypes evseStatusType))
                return evseStatusType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out EVSEStatusType)

        /// <summary>
        /// Parse the given string as a EVSE status type.
        /// </summary>
        /// <param name="Text">A text representation of a EVSE status type.</param>
        /// <param name="EVSEStatusType">The parsed EVSE status type.</param>
        public static Boolean TryParse(String Text, out EVSEStatusTypes EVSEStatusType)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    EVSEStatusType = new EVSEStatusTypes(Text);
                    return true;
                }
                catch
                { }
            }

            EVSEStatusType = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE status type.
        /// </summary>
        public EVSEStatusTypes Clone

            => new(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static members

        /// <summary>
        /// Unknown status of the EVSE.
        /// </summary>
        public static readonly EVSEStatusTypes Unknown           = new("unknown");

        /// <summary>
        /// Unclear status of the EVSE.
        /// </summary>
        public static readonly EVSEStatusTypes Unspecified       = new("unspecified");

        /// <summary>
        /// Currently no communication with the EVSE possible,
        /// but charging in offline mode might be available.
        /// </summary>
        public static readonly EVSEStatusTypes Offline           = new("offline");

        /// <summary>
        /// The EVSE is not fully operational yet.
        /// </summary>
        public static readonly EVSEStatusTypes InDeployment      = new("inDeployment");

        /// <summary>
        /// The EVSE is reserved for future charging.
        /// </summary>
        public static readonly EVSEStatusTypes Reserved          = new("reserved");

        /// <summary>
        /// The EVSE is available for charging.
        /// </summary>
        public static readonly EVSEStatusTypes Available         = new("available");

        /// <summary>
        /// The door of a charging locker is open, the EVSE is unlocked
        /// and is waiting for the customer to plugin.
        /// </summary>
        public static readonly EVSEStatusTypes WaitingForPlugin  = new("waitingForPlugin");

        /// <summary>
        /// A cable is plugged into the socket or a vehicle is connected
        /// to the cable, but both without any further action.
        /// </summary>
        public static readonly EVSEStatusTypes PluggedIn         = new("pluggedIn");

        /// <summary>
        /// An ongoing charging process.
        /// </summary>
        public static readonly EVSEStatusTypes Charging          = new("charging");

        /// <summary>
        /// The EVSE has a mechanical door, e.g. an e-bike charging locker,
        /// which was not closed after the customer took the battery out.
        /// </summary>
        public static readonly EVSEStatusTypes DoorNotClosed     = new("doorNotClosed");

        /// <summary>
        /// A fatal error has occured within the EVSE.
        /// </summary>
        public static readonly EVSEStatusTypes Error             = new("error");

        /// <summary>
        /// The EVSE is not ready for charging because it is under maintenance
        /// or was disabled by the charging station operator.
        /// </summary>
        public static readonly EVSEStatusTypes OutOfService      = new("outOfService");

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">A EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatusTypes EVSEStatusType1,
                                           EVSEStatusTypes EVSEStatusType2)

            => EVSEStatusType1.Equals(EVSEStatusType2);

        #endregion

        #region Operator != (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">A EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatusTypes EVSEStatusType1,
                                           EVSEStatusTypes EVSEStatusType2)

            => !EVSEStatusType1.Equals(EVSEStatusType2);

        #endregion

        #region Operator <  (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">A EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatusTypes EVSEStatusType1,
                                          EVSEStatusTypes EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) < 0;

        #endregion

        #region Operator <= (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">A EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatusTypes EVSEStatusType1,
                                           EVSEStatusTypes EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) <= 0;

        #endregion

        #region Operator >  (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">A EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatusTypes EVSEStatusType1,
                                          EVSEStatusTypes EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) > 0;

        #endregion

        #region Operator >= (EVSEStatusType1, EVSEStatusType2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusType1">A EVSE status type.</param>
        /// <param name="EVSEStatusType2">Another EVSE status type.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatusTypes EVSEStatusType1,
                                           EVSEStatusTypes EVSEStatusType2)

            => EVSEStatusType1.CompareTo(EVSEStatusType2) >= 0;

        #endregion

        #endregion

        #region IComparable<EVSEStatusTypes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two EVSE status types.
        /// </summary>
        /// <param name="Object">A EVSE status type to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is EVSEStatusTypes evseStatusType
                   ? CompareTo(evseStatusType)
                   : throw new ArgumentException("The given object is not a EVSE status type!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(EVSEStatusType)

        /// <summary>
        /// Compares two EVSE status types.
        /// </summary>
        /// <param name="EVSEStatusType">A EVSE status type to compare with.</param>
        public Int32 CompareTo(EVSEStatusTypes EVSEStatusType)

            => String.Compare(InternalId,
                              EVSEStatusType.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<EVSEStatusTypes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two EVSE status types for equality.
        /// </summary>
        /// <param name="Object">A EVSE status type to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is EVSEStatusTypes evseStatusType &&
                   Equals(evseStatusType);

        #endregion

        #region Equals(EVSEStatusType)

        /// <summary>
        /// Compares two EVSE status types for equality.
        /// </summary>
        /// <param name="EVSEStatusType">A EVSE status type to compare with.</param>
        public Boolean Equals(EVSEStatusTypes EVSEStatusType)

            => String.Equals(InternalId,
                             EVSEStatusType.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

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
