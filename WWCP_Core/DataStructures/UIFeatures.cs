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
    /// Extension methods for features.
    /// </summary>
    public static class UIFeaturesExtensions
    {

        /// <summary>
        /// Indicates whether this user interface feature is null or empty.
        /// </summary>
        /// <param name="UIFeatures">An user interface feature.</param>
        public static Boolean IsNullOrEmpty(this UIFeatures? UIFeatures)
            => !UIFeatures.HasValue || UIFeatures.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this user interface feature is NOT null or empty.
        /// </summary>
        /// <param name="UIFeatures">An user interface feature.</param>
        public static Boolean IsNotNullOrEmpty(this UIFeatures? UIFeatures)
            => UIFeatures.HasValue && UIFeatures.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging feature, e.g. reservability.
    /// </summary>
    public readonly struct UIFeatures : IId<UIFeatures>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this user interface feature is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this user interface feature is NOT null or empty.
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
        /// <param name="Text">The text representation of an user interface feature.</param>
        private UIFeatures(String Text)
        {
            this.InternalId = Text;
        }

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given text as an user interface feature.
        /// </summary>
        /// <param name="Text">A text representation of an user interface feature.</param>
        public static UIFeatures Parse(String Text)
        {

            if (TryParse(Text, out var featureId))
                return featureId;

            throw new ArgumentException($"Invalid text representation of an user interface feature: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as an user interface feature.
        /// </summary>
        /// <param name="Text">A text representation of an user interface feature.</param>
        public static UIFeatures? TryParse(String Text)
        {

            if (TryParse(Text, out var featureId))
                return featureId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out UIFeatures)

        /// <summary>
        /// Try to parse the given text as an user interface feature.
        /// </summary>
        /// <param name="Text">A text representation of an user interface feature.</param>
        /// <param name="UIFeatures">The parsed feature.</param>
        public static Boolean TryParse(String Text, out UIFeatures UIFeatures)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    UIFeatures = new UIFeatures(Text);
                    return true;
                }
                catch
                { }
            }

            UIFeatures = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this user interface feature.
        /// </summary>
        public UIFeatures Clone

            => new (
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Static definitions

        /// <summary>
        /// There is a (large) screen.
        /// </summary>
        public static UIFeatures Screen
            => new("Screen");

        public static UIFeatures CreditCard
            => new("CreditCard");

        public static UIFeatures DebitCard
            => new("DebitCard");

        /// <summary>
        /// The is a keypad, e.g. for entering a PIN.
        /// </summary>
        public static UIFeatures Pinpad
            => new("Pinpad");

        /// <summary>
        /// Audio feedback is available.
        /// </summary>
        public static UIFeatures Sound
            => new("Sound");

        /// <summary>
        /// Voice control supported.
        /// </summary>
        public static UIFeatures SpeechRecognition
            => new("SpeechRecognition");

        /// <summary>
        /// RFID cards will be accepted.
        /// </summary>
        public static UIFeatures RFID
            => new("RFID");

        /// <summary>
        /// NFC control supported.
        /// </summary>
        public static UIFeatures NFC
            => new("NFC");

        /// <summary>
        /// Bluetooth control supported.
        /// </summary>
        public static UIFeatures Bluetooth
            => new("Bluetooth");

        /// <summary>
        /// Information via Bluetooth low-energy beacons supported.
        /// </summary>
        public static UIFeatures BLEBeacons
            => new("BLEBeacons");

        /// <summary>
        /// WLAN control supported.
        /// </summary>
        public static UIFeatures WLAN
            => new("WLAN");

        #endregion


        #region Operator overloading

        #region Operator == (UIFeatures1, UIFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UIFeatures1">An user interface feature.</param>
        /// <param name="UIFeatures2">Another user interface feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UIFeatures UIFeatures1,
                                           UIFeatures UIFeatures2)

            => UIFeatures1.Equals(UIFeatures2);

        #endregion

        #region Operator != (UIFeatures1, UIFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UIFeatures1">An user interface feature.</param>
        /// <param name="UIFeatures2">Another user interface feature.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UIFeatures UIFeatures1,
                                           UIFeatures UIFeatures2)

            => !UIFeatures1.Equals(UIFeatures2);

        #endregion

        #region Operator <  (UIFeatures1, UIFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UIFeatures1">An user interface feature.</param>
        /// <param name="UIFeatures2">Another user interface feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (UIFeatures UIFeatures1,
                                          UIFeatures UIFeatures2)

            => UIFeatures1.CompareTo(UIFeatures2) < 0;

        #endregion

        #region Operator <= (UIFeatures1, UIFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UIFeatures1">An user interface feature.</param>
        /// <param name="UIFeatures2">Another user interface feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (UIFeatures UIFeatures1,
                                           UIFeatures UIFeatures2)

            => UIFeatures1.CompareTo(UIFeatures2) <= 0;

        #endregion

        #region Operator >  (UIFeatures1, UIFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UIFeatures1">An user interface feature.</param>
        /// <param name="UIFeatures2">Another user interface feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (UIFeatures UIFeatures1,
                                          UIFeatures UIFeatures2)

            => UIFeatures1.CompareTo(UIFeatures2) > 0;

        #endregion

        #region Operator >= (UIFeatures1, UIFeatures2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="UIFeatures1">An user interface feature.</param>
        /// <param name="UIFeatures2">Another user interface feature.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (UIFeatures UIFeatures1,
                                           UIFeatures UIFeatures2)

            => UIFeatures1.CompareTo(UIFeatures2) >= 0;

        #endregion

        #endregion

        #region IComparable<UIFeatures> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two features.
        /// </summary>
        /// <param name="Object">An user interface feature to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is UIFeatures featureId
                   ? CompareTo(featureId)
                   : throw new ArgumentException("The given object is not an user interface feature!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(UIFeatures)

        /// <summary>
        /// Compares two features.
        /// </summary>
        /// <param name="UIFeatures">An user interface feature to compare with.</param>
        public Int32 CompareTo(UIFeatures UIFeatures)

            => String.Compare(InternalId,
                              UIFeatures.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<UIFeatures> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two features for equality.
        /// </summary>
        /// <param name="Object">An user interface feature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is UIFeatures featureId &&
                   Equals(featureId);

        #endregion

        #region Equals(UIFeatures)

        /// <summary>
        /// Compares two features for equality.
        /// </summary>
        /// <param name="UIFeatures">An user interface feature to compare with.</param>
        public Boolean Equals(UIFeatures UIFeatures)

            => String.Equals(InternalId,
                             UIFeatures.InternalId,
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
