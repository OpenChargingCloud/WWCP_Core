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
    /// Extension methods for features.
    /// </summary>
    public static class FeaturesExtensions
    {

        /// <summary>
        /// Indicates whether this feature is null or empty.
        /// </summary>
        /// <param name="Features">A feature.</param>
        public static Boolean IsNullOrEmpty(this Features? Features)
            => !Features.HasValue || Features.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this feature is NOT null or empty.
        /// </summary>
        /// <param name="Features">A feature.</param>
        public static Boolean IsNotNullOrEmpty(this Features? Features)
            => Features.HasValue && Features.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging feature, e.g. reservability.
    /// </summary>
    public readonly struct Features : IId<Features>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this feature is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this feature is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the feature.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new feature based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a feature.</param>
        private Features(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a feature.
        /// </summary>
        /// <param name="Text">A text representation of a feature.</param>
        public static Features Parse(String Text)
        {

            if (TryParse(Text, out var featureId))
                return featureId;

            throw new ArgumentException($"Invalid text representation of a feature: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a feature.
        /// </summary>
        /// <param name="Text">A text representation of a feature.</param>
        public static Features? TryParse(String Text)
        {

            if (TryParse(Text, out var featureId))
                return featureId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out Features)

        /// <summary>
        /// Try to parse the given text as a feature.
        /// </summary>
        /// <param name="Text">A text representation of a feature.</param>
        /// <param name="Features">The parsed feature.</param>
        public static Boolean TryParse(String Text, out Features Features)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    Features = new Features(Text);
                    return true;
                }
                catch
                { }
            }

            Features = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this feature.
        /// </summary>
        public Features Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Dynamic status information is available.
        /// </summary>
        public static Features StatusInfoAvailable
            => new("StatusInfoAvailable");

        /// <summary>
        /// Dynamic energy information is available.
        /// </summary>
        public static Features EnergyInfoAvailable
            => new("EnergyInfoAvailable");

        /// <summary>
        /// The German calibration law is supported.
        /// </summary>
        public static Features GermanCalibrationLaw
            => new("GermanCalibrationLaw");

        /// <summary>
        /// The European calibration law is supported.
        /// </summary>
        public static Features EuropeanCalibrationLaw
            => new("EuropeanCalibrationLaw");

        /// <summary>
        /// Is reservable.
        /// </summary>
        public static Features Reservable
            => new("Reservable");

        /// <summary>
        /// The charging station requires an authentication before the
        /// charging cable can be plugged in.
        /// </summary>
        public static Features AuthenticationBeforePlugIn
            => new("AuthenticationBeforePlugIn");

        /// <summary>
        /// OCPP charging profiles are supported.
        /// </summary>
        public static Features ChargingProfilesSupported
            => new("ChargingProfilesSupported");

        /// <summary>
        /// OCPP charging preferences are supported.
        /// </summary>
        public static Features ChargingPreferencesSupported
            => new("ChargingPreferencesSupported");

        /// <summary>
        /// OCPP / OCPI token groups are supported.
        /// </summary>
        public static Features TokenGroupsSupported
            => new("TokenGroupsSupported");

        /// <summary>
        /// The charging station operator can unlock an EVSE remotely.
        /// </summary>
        public static Features CSOUnlockSupported
            => new("CSOUnlockSupported");

        /// <summary>
        /// Hubject compatibility.
        /// </summary>
        public static Features HubjectCompatible
            => new("HubjectCompatible");

        /// <summary>
        /// OCPI v2.2 requires the connector identification within START_SESSION commands.
        /// </summary>
        public static Features START_SESSION_CONNECTOR_REQUIRED
            => new("START_SESSION_CONNECTOR_REQUIRED");

        #endregion


        #region Operator overloading

        #region Operator == (Features1, Features2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Features1">A feature.</param>
        /// <param name="Features2">Another feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Features Features1,
                                           Features Features2)

            => Features1.Equals(Features2);

        #endregion

        #region Operator != (Features1, Features2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Features1">A feature.</param>
        /// <param name="Features2">Another feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Features Features1,
                                           Features Features2)

            => !Features1.Equals(Features2);

        #endregion

        #region Operator <  (Features1, Features2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Features1">A feature.</param>
        /// <param name="Features2">Another feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Features Features1,
                                          Features Features2)

            => Features1.CompareTo(Features2) < 0;

        #endregion

        #region Operator <= (Features1, Features2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Features1">A feature.</param>
        /// <param name="Features2">Another feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Features Features1,
                                           Features Features2)

            => Features1.CompareTo(Features2) <= 0;

        #endregion

        #region Operator >  (Features1, Features2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Features1">A feature.</param>
        /// <param name="Features2">Another feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Features Features1,
                                          Features Features2)

            => Features1.CompareTo(Features2) > 0;

        #endregion

        #region Operator >= (Features1, Features2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Features1">A feature.</param>
        /// <param name="Features2">Another feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Features Features1,
                                           Features Features2)

            => Features1.CompareTo(Features2) >= 0;

        #endregion

        #endregion

        #region IComparable<Features> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two features.
        /// </summary>
        /// <param name="Object">A feature to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Features featureId
                   ? CompareTo(featureId)
                   : throw new ArgumentException("The given object is not a feature!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Features)

        /// <summary>
        /// Compares two features.
        /// </summary>
        /// <param name="Features">A feature to compare with.</param>
        public Int32 CompareTo(Features Features)

            => String.Compare(InternalId,
                              Features.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<Features> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two features for equality.
        /// </summary>
        /// <param name="Object">A feature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Features featureId &&
                   Equals(featureId);

        #endregion

        #region Equals(Features)

        /// <summary>
        /// Compares two features for equality.
        /// </summary>
        /// <param name="Features">A feature to compare with.</param>
        public Boolean Equals(Features Features)

            => String.Equals(InternalId,
                             Features.InternalId,
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
