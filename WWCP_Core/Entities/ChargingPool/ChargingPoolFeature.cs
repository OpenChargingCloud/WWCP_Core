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
    /// Extension methods for charging pool features.
    /// </summary>
    public static class ChargingPoolFeaturesExtensions
    {

        /// <summary>
        /// Indicates whether this charging pool feature is null or empty.
        /// </summary>
        /// <param name="ChargingPoolFeatures">A charging pool feature.</param>
        public static Boolean IsNullOrEmpty(this ChargingPoolFeature? ChargingPoolFeatures)
            => !ChargingPoolFeatures.HasValue || ChargingPoolFeatures.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging pool feature is NOT null or empty.
        /// </summary>
        /// <param name="ChargingPoolFeatures">A charging pool feature.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingPoolFeature? ChargingPoolFeatures)
            => ChargingPoolFeatures.HasValue && ChargingPoolFeatures.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging charging pool feature, e.g. reservability.
    /// </summary>
    public readonly struct ChargingPoolFeature : IId<ChargingPoolFeature>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging pool feature is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging pool feature is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging pool feature.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging pool feature based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a charging pool feature.</param>
        private ChargingPoolFeature(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a charging pool feature.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool feature.</param>
        public static ChargingPoolFeature Parse(String Text)
        {

            if (TryParse(Text, out var chargingPoolFeature))
                return chargingPoolFeature;

            throw new ArgumentException($"Invalid text representation of a charging pool feature: '{Text}!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a charging pool feature.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool feature.</param>
        public static ChargingPoolFeature? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingPoolFeature))
                return chargingPoolFeature;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingPoolFeatures)

        /// <summary>
        /// Try to parse the given text as a charging pool feature.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool feature.</param>
        /// <param name="ChargingPoolFeatures">The parsed charging pool feature.</param>
        public static Boolean TryParse(String Text, out ChargingPoolFeature ChargingPoolFeatures)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingPoolFeatures = new ChargingPoolFeature(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingPoolFeatures = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging pool feature.
        /// </summary>
        public ChargingPoolFeature Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Dynamic status information is available.
        /// </summary>
        public static ChargingPoolFeature StatusInfoAvailable
            => new("StatusInfoAvailable");

        /// <summary>
        /// Dynamic energy information is available.
        /// </summary>
        public static ChargingPoolFeature EnergyInfoAvailable
            => new("EnergyInfoAvailable");

        /// <summary>
        /// The German calibration law is supported.
        /// </summary>
        public static ChargingPoolFeature GermanCalibrationLaw
            => new("GermanCalibrationLaw");

        /// <summary>
        /// The European calibration law is supported.
        /// </summary>
        public static ChargingPoolFeature EuropeanCalibrationLaw
            => new("EuropeanCalibrationLaw");

        /// <summary>
        /// Is reservable.
        /// </summary>
        public static ChargingPoolFeature Reservable
            => new("Reservable");

        /// <summary>
        /// The charging pool requires an authentication before the
        /// charging cable can be plugged in.
        /// </summary>
        public static ChargingPoolFeature AuthenticationBeforePlugIn
            => new("AuthenticationBeforePlugIn");

        /// <summary>
        /// OCPP charging profiles are supported.
        /// </summary>
        public static ChargingPoolFeature ChargingProfilesSupported
            => new("ChargingProfilesSupported");

        /// <summary>
        /// OCPP charging preferences are supported.
        /// </summary>
        public static ChargingPoolFeature ChargingPreferencesSupported
            => new("ChargingPreferencesSupported");

        /// <summary>
        /// OCPP / OCPI token groups are supported.
        /// </summary>
        public static ChargingPoolFeature TokenGroupsSupported
            => new("TokenGroupsSupported");

        /// <summary>
        /// The charging pool operator can unlock an EVSE remotely.
        /// </summary>
        public static ChargingPoolFeature CSOUnlockSupported
            => new("CSOUnlockSupported");

        /// <summary>
        /// Hubject compatibility.
        /// </summary>
        public static ChargingPoolFeature HubjectCompatible
            => new("HubjectCompatible");

        /// <summary>
        /// OCPI v2.2 requires the connector identification within START_SESSION commands.
        /// </summary>
        public static ChargingPoolFeature START_SESSION_CONNECTOR_REQUIRED
            => new("START_SESSION_CONNECTOR_REQUIRED");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolFeatures1, ChargingPoolFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolFeatures1">A charging pool feature.</param>
        /// <param name="ChargingPoolFeatures2">Another charging pool feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolFeature ChargingPoolFeatures1,
                                           ChargingPoolFeature ChargingPoolFeatures2)

            => ChargingPoolFeatures1.Equals(ChargingPoolFeatures2);

        #endregion

        #region Operator != (ChargingPoolFeatures1, ChargingPoolFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolFeatures1">A charging pool feature.</param>
        /// <param name="ChargingPoolFeatures2">Another charging pool feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolFeature ChargingPoolFeatures1,
                                           ChargingPoolFeature ChargingPoolFeatures2)

            => !ChargingPoolFeatures1.Equals(ChargingPoolFeatures2);

        #endregion

        #region Operator <  (ChargingPoolFeatures1, ChargingPoolFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolFeatures1">A charging pool feature.</param>
        /// <param name="ChargingPoolFeatures2">Another charging pool feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolFeature ChargingPoolFeatures1,
                                          ChargingPoolFeature ChargingPoolFeatures2)

            => ChargingPoolFeatures1.CompareTo(ChargingPoolFeatures2) < 0;

        #endregion

        #region Operator <= (ChargingPoolFeatures1, ChargingPoolFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolFeatures1">A charging pool feature.</param>
        /// <param name="ChargingPoolFeatures2">Another charging pool feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolFeature ChargingPoolFeatures1,
                                           ChargingPoolFeature ChargingPoolFeatures2)

            => ChargingPoolFeatures1.CompareTo(ChargingPoolFeatures2) <= 0;

        #endregion

        #region Operator >  (ChargingPoolFeatures1, ChargingPoolFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolFeatures1">A charging pool feature.</param>
        /// <param name="ChargingPoolFeatures2">Another charging pool feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolFeature ChargingPoolFeatures1,
                                          ChargingPoolFeature ChargingPoolFeatures2)

            => ChargingPoolFeatures1.CompareTo(ChargingPoolFeatures2) > 0;

        #endregion

        #region Operator >= (ChargingPoolFeatures1, ChargingPoolFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolFeatures1">A charging pool feature.</param>
        /// <param name="ChargingPoolFeatures2">Another charging pool feature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolFeature ChargingPoolFeatures1,
                                           ChargingPoolFeature ChargingPoolFeatures2)

            => ChargingPoolFeatures1.CompareTo(ChargingPoolFeatures2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingPoolFeatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging pool features.
        /// </summary>
        /// <param name="Object">A charging pool feature to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingPoolFeature chargingPoolFeature
                   ? CompareTo(chargingPoolFeature)
                   : throw new ArgumentException("The given object is not a charging pool feature!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingPoolFeatures)

        /// <summary>
        /// Compares two charging pool features.
        /// </summary>
        /// <param name="ChargingPoolFeatures">A charging pool feature to compare with.</param>
        public Int32 CompareTo(ChargingPoolFeature ChargingPoolFeatures)

            => String.Compare(InternalId,
                              ChargingPoolFeatures.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingPoolFeatures> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging pool features for equality.
        /// </summary>
        /// <param name="Object">A charging pool feature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingPoolFeature chargingPoolFeature &&
                   Equals(chargingPoolFeature);

        #endregion

        #region Equals(ChargingPoolFeatures)

        /// <summary>
        /// Compares two charging pool features for equality.
        /// </summary>
        /// <param name="ChargingPoolFeatures">A charging pool feature to compare with.</param>
        public Boolean Equals(ChargingPoolFeature ChargingPoolFeatures)

            => String.Equals(InternalId,
                             ChargingPoolFeatures.InternalId,
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
