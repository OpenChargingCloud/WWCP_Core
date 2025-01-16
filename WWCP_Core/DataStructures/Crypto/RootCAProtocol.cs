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
    /// Extension methods for root CA protocols.
    /// </summary>
    public static class RootCAProtocolExtensions
    {

        /// <summary>
        /// Indicates whether this root CA protocol is null or empty.
        /// </summary>
        /// <param name="RootCAProtocol">A root CA protocol.</param>
        public static Boolean IsNullOrEmpty(this RootCAProtocol? RootCAProtocol)
            => !RootCAProtocol.HasValue || RootCAProtocol.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this root CA protocol is NOT null or empty.
        /// </summary>
        /// <param name="RootCAProtocol">A root CA protocol.</param>
        public static Boolean IsNotNullOrEmpty(this RootCAProtocol? RootCAProtocol)
            => RootCAProtocol.HasValue && RootCAProtocol.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The root CA protocol.
    /// </summary>
    public readonly struct RootCAProtocol : IId<RootCAProtocol>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this root CA protocol is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this root CA protocol is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the root CA protocol.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new root CA protocol based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a root CA protocol.</param>
        private RootCAProtocol(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a root CA protocol.
        /// </summary>
        /// <param name="Text">A text representation of a root CA protocol.</param>
        public static RootCAProtocol Parse(String Text)
        {

            if (TryParse(Text, out var vehicleType))
                return vehicleType;

            throw new ArgumentException($"Invalid text representation of a root CA protocol: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a root CA protocol.
        /// </summary>
        /// <param name="Text">A text representation of a root CA protocol.</param>
        public static RootCAProtocol? TryParse(String Text)
        {

            if (TryParse(Text, out var vehicleType))
                return vehicleType;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out RootCAProtocol)

        /// <summary>
        /// Try to parse the given text as a root CA protocol.
        /// </summary>
        /// <param name="Text">A text representation of a root CA protocol.</param>
        /// <param name="RootCAProtocol">The parsed root CA protocol.</param>
        public static Boolean TryParse(String Text, out RootCAProtocol RootCAProtocol)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    RootCAProtocol = new RootCAProtocol(Text);
                    return true;
                }
                catch
                { }
            }

            RootCAProtocol = default;
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this root CA protocol.
        /// </summary>
        public RootCAProtocol Clone()

            => new (
                   InternalId.CloneString()
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// ISO 15118-2
        /// </summary>
        public static RootCAProtocol  ISO15118_2     { get; }
            = new ("ISO 15118-2");

        /// <summary>
        /// ISO 15118-20
        /// </summary>
        public static RootCAProtocol  ISO15118_20    { get; }
            = new ("ISO 15118-20");

        #endregion


        #region Operator overloading

        #region Operator == (RootCAProtocol1, RootCAProtocol2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RootCAProtocol1">A root CA protocol.</param>
        /// <param name="RootCAProtocol2">Another root CA protocol.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RootCAProtocol RootCAProtocol1,
                                           RootCAProtocol RootCAProtocol2)

            => RootCAProtocol1.Equals(RootCAProtocol2);

        #endregion

        #region Operator != (RootCAProtocol1, RootCAProtocol2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RootCAProtocol1">A root CA protocol.</param>
        /// <param name="RootCAProtocol2">Another root CA protocol.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RootCAProtocol RootCAProtocol1,
                                           RootCAProtocol RootCAProtocol2)

            => !RootCAProtocol1.Equals(RootCAProtocol2);

        #endregion

        #region Operator <  (RootCAProtocol1, RootCAProtocol2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RootCAProtocol1">A root CA protocol.</param>
        /// <param name="RootCAProtocol2">Another root CA protocol.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RootCAProtocol RootCAProtocol1,
                                          RootCAProtocol RootCAProtocol2)

            => RootCAProtocol1.CompareTo(RootCAProtocol2) < 0;

        #endregion

        #region Operator <= (RootCAProtocol1, RootCAProtocol2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RootCAProtocol1">A root CA protocol.</param>
        /// <param name="RootCAProtocol2">Another root CA protocol.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RootCAProtocol RootCAProtocol1,
                                           RootCAProtocol RootCAProtocol2)

            => RootCAProtocol1.CompareTo(RootCAProtocol2) <= 0;

        #endregion

        #region Operator >  (RootCAProtocol1, RootCAProtocol2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RootCAProtocol1">A root CA protocol.</param>
        /// <param name="RootCAProtocol2">Another root CA protocol.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RootCAProtocol RootCAProtocol1,
                                          RootCAProtocol RootCAProtocol2)

            => RootCAProtocol1.CompareTo(RootCAProtocol2) > 0;

        #endregion

        #region Operator >= (RootCAProtocol1, RootCAProtocol2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RootCAProtocol1">A root CA protocol.</param>
        /// <param name="RootCAProtocol2">Another root CA protocol.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RootCAProtocol RootCAProtocol1,
                                           RootCAProtocol RootCAProtocol2)

            => RootCAProtocol1.CompareTo(RootCAProtocol2) >= 0;

        #endregion

        #endregion

        #region IComparable<RootCAProtocol> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two root CA protocols.
        /// </summary>
        /// <param name="Object">A root CA protocol to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is RootCAProtocol vehicleType
                   ? CompareTo(vehicleType)
                   : throw new ArgumentException("The given object is not a root CA protocol!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(RootCAProtocol)

        /// <summary>
        /// Compares two root CA protocols.
        /// </summary>
        /// <param name="RootCAProtocol">A root CA protocol to compare with.</param>
        public Int32 CompareTo(RootCAProtocol RootCAProtocol)

            => String.Compare(InternalId,
                              RootCAProtocol.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<RootCAProtocol> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two root CA protocols for equality.
        /// </summary>
        /// <param name="Object">A root CA protocol to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is RootCAProtocol vehicleType &&
                   Equals(vehicleType);

        #endregion

        #region Equals(RootCAProtocol)

        /// <summary>
        /// Compares two root CA protocols for equality.
        /// </summary>
        /// <param name="RootCAProtocol">A root CA protocol to compare with.</param>
        public Boolean Equals(RootCAProtocol RootCAProtocol)

            => String.Equals(InternalId,
                             RootCAProtocol.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToUpper().GetHashCode() ?? 0;

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
