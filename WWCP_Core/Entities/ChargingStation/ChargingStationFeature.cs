/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for charging station features.
    /// </summary>
    public static class ChargingStationFeaturesExtensions
    {

        /// <summary>
        /// Indicates whether this charging station feature is null or empty.
        /// </summary>
        /// <param name="ChargingStationFeatures">A charging station feature.</param>
        public static Boolean IsNullOrEmpty(this ChargingStationFeature? ChargingStationFeatures)
            => !ChargingStationFeatures.HasValue || ChargingStationFeatures.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging station feature is NOT null or empty.
        /// </summary>
        /// <param name="ChargingStationFeatures">A charging station feature.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingStationFeature? ChargingStationFeatures)
            => ChargingStationFeatures.HasValue && ChargingStationFeatures.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging charging station feature, e.g. reservability.
    /// </summary>
    public readonly struct ChargingStationFeature : IId<ChargingStationFeature>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this charging station feature is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging station feature is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging station feature.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station feature based on the given text.
        /// </summary>
        /// <param name="Text">The text representation of a charging station feature.</param>
        private ChargingStationFeature(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as a charging station feature.
        /// </summary>
        /// <param name="Text">A text representation of a charging station feature.</param>
        public static ChargingStationFeature Parse(String Text)
        {

            if (TryParse(Text, out var chargingStationFeature))
                return chargingStationFeature;

            throw new ArgumentException($"Invalid text representation of a charging station feature: '{Text}!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as a charging station feature.
        /// </summary>
        /// <param name="Text">A text representation of a charging station feature.</param>
        public static ChargingStationFeature? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingStationFeature))
                return chargingStationFeature;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out ChargingStationFeatures)

        /// <summary>
        /// Try to parse the given text as a charging station feature.
        /// </summary>
        /// <param name="Text">A text representation of a charging station feature.</param>
        /// <param name="ChargingStationFeatures">The parsed charging station feature.</param>
        public static Boolean TryParse(String Text, out ChargingStationFeature ChargingStationFeatures)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    ChargingStationFeatures = new ChargingStationFeature(Text);
                    return true;
                }
                catch
                { }
            }

            ChargingStationFeatures = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station feature.
        /// </summary>
        public ChargingStationFeature Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// Dynamic status information is available.
        /// </summary>
        public static ChargingStationFeature StatusInfoAvailable
            => new("StatusInfoAvailable");

        /// <summary>
        /// Dynamic energy information is available.
        /// </summary>
        public static ChargingStationFeature EnergyInfoAvailable
            => new("EnergyInfoAvailable");

        /// <summary>
        /// The German calibration law is supported.
        /// </summary>
        public static ChargingStationFeature GermanCalibrationLaw
            => new("GermanCalibrationLaw");

        /// <summary>
        /// The European calibration law is supported.
        /// </summary>
        public static ChargingStationFeature EuropeanCalibrationLaw
            => new("EuropeanCalibrationLaw");

        /// <summary>
        /// Is reservable.
        /// </summary>
        public static ChargingStationFeature Reservable
            => new("Reservable");

        /// <summary>
        /// The charging station requires an authentication before the
        /// charging cable can be plugged in.
        /// </summary>
        public static ChargingStationFeature AuthenticationBeforePlugIn
            => new("AuthenticationBeforePlugIn");

        /// <summary>
        /// OCPP charging profiles are supported.
        /// </summary>
        public static ChargingStationFeature ChargingProfilesSupported
            => new("ChargingProfilesSupported");

        /// <summary>
        /// OCPP charging preferences are supported.
        /// </summary>
        public static ChargingStationFeature ChargingPreferencesSupported
            => new("ChargingPreferencesSupported");

        /// <summary>
        /// OCPP / OCPI token groups are supported.
        /// </summary>
        public static ChargingStationFeature TokenGroupsSupported
            => new("TokenGroupsSupported");

        /// <summary>
        /// The charging station operator can unlock an EVSE remotely.
        /// </summary>
        public static ChargingStationFeature CSOUnlockSupported
            => new("CSOUnlockSupported");

        /// <summary>
        /// Hubject compatibility.
        /// </summary>
        public static ChargingStationFeature HubjectCompatible
            => new("HubjectCompatible");

        /// <summary>
        /// OCPI v2.2 requires the connector identification within START_SESSION commands.
        /// </summary>
        public static ChargingStationFeature START_SESSION_CONNECTOR_REQUIRED
            => new("START_SESSION_CONNECTOR_REQUIRED");

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationFeatures1, ChargingStationFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationFeatures1">A charging station feature.</param>
        /// <param name="ChargingStationFeatures2">Another charging station feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingStationFeature ChargingStationFeatures1,
                                           ChargingStationFeature ChargingStationFeatures2)

            => ChargingStationFeatures1.Equals(ChargingStationFeatures2);

        #endregion

        #region Operator != (ChargingStationFeatures1, ChargingStationFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationFeatures1">A charging station feature.</param>
        /// <param name="ChargingStationFeatures2">Another charging station feature.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingStationFeature ChargingStationFeatures1,
                                           ChargingStationFeature ChargingStationFeatures2)

            => !ChargingStationFeatures1.Equals(ChargingStationFeatures2);

        #endregion

        #region Operator <  (ChargingStationFeatures1, ChargingStationFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationFeatures1">A charging station feature.</param>
        /// <param name="ChargingStationFeatures2">Another charging station feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingStationFeature ChargingStationFeatures1,
                                          ChargingStationFeature ChargingStationFeatures2)

            => ChargingStationFeatures1.CompareTo(ChargingStationFeatures2) < 0;

        #endregion

        #region Operator <= (ChargingStationFeatures1, ChargingStationFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationFeatures1">A charging station feature.</param>
        /// <param name="ChargingStationFeatures2">Another charging station feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingStationFeature ChargingStationFeatures1,
                                           ChargingStationFeature ChargingStationFeatures2)

            => ChargingStationFeatures1.CompareTo(ChargingStationFeatures2) <= 0;

        #endregion

        #region Operator >  (ChargingStationFeatures1, ChargingStationFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationFeatures1">A charging station feature.</param>
        /// <param name="ChargingStationFeatures2">Another charging station feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingStationFeature ChargingStationFeatures1,
                                          ChargingStationFeature ChargingStationFeatures2)

            => ChargingStationFeatures1.CompareTo(ChargingStationFeatures2) > 0;

        #endregion

        #region Operator >= (ChargingStationFeatures1, ChargingStationFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationFeatures1">A charging station feature.</param>
        /// <param name="ChargingStationFeatures2">Another charging station feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingStationFeature ChargingStationFeatures1,
                                           ChargingStationFeature ChargingStationFeatures2)

            => ChargingStationFeatures1.CompareTo(ChargingStationFeatures2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingStationFeatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging station features.
        /// </summary>
        /// <param name="Object">A charging station feature to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingStationFeature chargingStationFeature
                   ? CompareTo(chargingStationFeature)
                   : throw new ArgumentException("The given object is not a charging station feature!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingStationFeatures)

        /// <summary>
        /// Compares two charging station features.
        /// </summary>
        /// <param name="ChargingStationFeatures">A charging station feature to compare with.</param>
        public Int32 CompareTo(ChargingStationFeature ChargingStationFeatures)

            => String.Compare(InternalId,
                              ChargingStationFeatures.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<ChargingStationFeatures> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging station features for equality.
        /// </summary>
        /// <param name="Object">A charging station feature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingStationFeature chargingStationFeature &&
                   Equals(chargingStationFeature);

        #endregion

        #region Equals(ChargingStationFeatures)

        /// <summary>
        /// Compares two charging station features for equality.
        /// </summary>
        /// <param name="ChargingStationFeatures">A charging station feature to compare with.</param>
        public Boolean Equals(ChargingStationFeature ChargingStationFeatures)

            => String.Equals(InternalId,
                             ChargingStationFeatures.InternalId,
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
