/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;
using static System.Net.Mime.MediaTypeNames;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of an electric vehicle.
    /// </summary>
    public class eVehicle_Id : IId,
                               IEquatable<eVehicle_Id>,
                               IComparable<eVehicle_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an e-vehicle identification.
        /// </summary>
        public    const    String  eVehicleId_RegEx  = @"^([A-Za-z]{2}\-?[A-Za-z0-9]{3})\-?V([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing an e-vehicle identification.
        /// </summary>
        public    const    String  IdSuffix_RegEx    = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

        #endregion

        #region Properties

        /// <summary>
        /// The the e-mobility provider identification.
        /// </summary>
        public EMobilityProvider_Id  ProviderId  { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                Suffix      { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => ProviderId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric vehicle identification
        /// based on the given string.
        /// </summary>
        private eVehicle_Id(EMobilityProvider_Id  ProviderId,
                            String                Suffix)
        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The unique provider identification must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),      "The suffix must not be null or empty!");

            #endregion

            var _MatchCollection = Regex.Matches(Suffix.Trim().ToUpper(),
                                                 IdSuffix_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal e-vehicle identification suffix '" + Suffix + "'!", nameof(Suffix));

            this.ProviderId  = ProviderId;
            this.Suffix      = _MatchCollection[0].Value;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an electric vehicle.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an Charging Station Operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static eVehicle_Id Random(EMobilityProvider_Id   ProviderId,
                                         Func<String, String>?  Mapper  = null)

            => new (ProviderId,
                    Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(12))
                        :        RandomExtensions.RandomString(12));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static eVehicle_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text must not be null or empty!");

            #endregion

            var matchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                eVehicleId_RegEx,
                                                RegexOptions.IgnorePatternWhitespace);

            if (matchCollection.Count != 1)
                throw new ArgumentException("Illegal e-vehicle identification '" + Text + "'!", nameof(Text));

            if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[1].Value, out EMobilityProvider_Id eMobilityProviderId))
                return new eVehicle_Id(eMobilityProviderId,
                                       matchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal e-vehicle identification '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(ProviderId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">A suffix of an electric vehicle identification.</param>
        public static eVehicle_Id Parse(EMobilityProvider_Id  ProviderId,
                                        String                Suffix)
        {

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The suffix of the electric vehicle identification must not be null or empty!");

            if (TryParse(ProviderId, Suffix, out eVehicle_Id eVehicleId))
                return eVehicleId;

            throw new ArgumentException("Illegal e-vehicle identification '" + Suffix + "'!", nameof(Suffix));

        }

        #endregion

        #region TryParse(Text, out eVehicleId)

        /// <summary>
        /// Parse the given string as an electric vehicle identification.
        /// </summary>
        /// <param name="Text">A text representation of an electric vehicle identification.</param>
        /// <param name="eVehicleId">The parsed electric vehicle identification.</param>
        public static Boolean TryParse(String Text, out eVehicle_Id? eVehicleId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                eVehicleId = null;
                return false;
            }

            #endregion

            try
            {

                eVehicleId = null;

                var matchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                    eVehicleId_RegEx,
                                                    RegexOptions.IgnorePatternWhitespace);

                if (matchCollection.Count != 1)
                    return false;

                // New format...
                if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[1].Value, out EMobilityProvider_Id eMobilityProviderId))
                {

                    eVehicleId = new eVehicle_Id(eMobilityProviderId,
                                                 matchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[3].Value, out eMobilityProviderId))
                {

                    eVehicleId = new eVehicle_Id(eMobilityProviderId,
                                                 matchCollection[0].Groups[4].Value);

                    return true;

                }

                // New format without the 'S'...
                else if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[5].Value, out eMobilityProviderId))
                {

                    eVehicleId = new eVehicle_Id(eMobilityProviderId,
                                                 matchCollection[0].Groups[6].Value);

                    return true;

                }

                // Old format without the 'S'...
                else if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[7].Value, out eMobilityProviderId))
                {

                    eVehicleId = new eVehicle_Id(eMobilityProviderId,
                                                 matchCollection[0].Groups[8].Value);

                    return true;

                }

            }
            catch (Exception)
            { }

            eVehicleId = null;
            return false;

        }

        #endregion

        #region TryParse(ProviderId, Suffix, out eVehicleId)

        /// <summary>
        /// Parse the given string as an electric vehicle identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">A text representation of an electric vehicle identification.</param>
        /// <param name="eVehicleId">The parsed electric vehicle identification.</param>
        public static Boolean TryParse(EMobilityProvider_Id  ProviderId,
                                       String                Suffix,
                                       out eVehicle_Id?      eVehicleId)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
            {
                eVehicleId = null;
                return false;
            }

            #endregion

            try
            {

                eVehicleId = null;

                var matchCollection = Regex.Matches(Suffix.Trim().ToUpper(),
                                                    IdSuffix_RegEx,
                                                    RegexOptions.IgnorePatternWhitespace);

                if (matchCollection.Count != 1)
                    return false;

                eVehicleId = new eVehicle_Id(ProviderId,
                                             matchCollection[0].Groups[0].Value);

                return true;

            }
            catch (Exception)
            {
                eVehicleId = null;
                return false;
            }

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this electric vehicle identification.
        /// </summary>
        public eVehicle_Id Clone

            => new (ProviderId,
                    new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (eVehicleId1, eVehicleId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId1">A electric vehicle identification.</param>
        /// <param name="eVehicleId2">Another electric vehicle identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (eVehicle_Id  eVehicleId1,
                                           eVehicle_Id  eVehicleId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(eVehicleId1, eVehicleId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eVehicleId1 == null) || ((Object) eVehicleId2 == null))
                return false;

            return eVehicleId1.Equals(eVehicleId2);

        }

        #endregion

        #region Operator != (eVehicleId1, eVehicleId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId1">A electric vehicle identification.</param>
        /// <param name="eVehicleId2">Another electric vehicle identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eVehicle_Id  eVehicleId1,
                                           eVehicle_Id  eVehicleId2)

            => !(eVehicleId1 == eVehicleId2);

        #endregion

        #region Operator <  (eVehicleId1, eVehicleId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId1">A electric vehicle identification.</param>
        /// <param name="eVehicleId2">Another electric vehicle identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eVehicle_Id  eVehicleId1,
                                          eVehicle_Id  eVehicleId2)
        {

            if ((Object) eVehicleId1 == null)
                throw new ArgumentNullException(nameof(eVehicleId1),  "The given electric vehicle identification must not be null!");

            return eVehicleId1.CompareTo(eVehicleId2) < 0;

        }

        #endregion

        #region Operator <= (eVehicleId1, eVehicleId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId1">A electric vehicle identification.</param>
        /// <param name="eVehicleId2">Another electric vehicle identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eVehicle_Id  eVehicleId1,
                                           eVehicle_Id  eVehicleId2)

            => !(eVehicleId1 > eVehicleId2);

        #endregion

        #region Operator >  (eVehicleId1, eVehicleId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId1">A electric vehicle identification.</param>
        /// <param name="eVehicleId2">Another electric vehicle identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eVehicle_Id  eVehicleId1,
                                          eVehicle_Id  eVehicleId2)
        {

            if ((Object) eVehicleId1 == null)
                throw new ArgumentNullException(nameof(eVehicleId1),  "The given electric vehicle identification must not be null!");

            return eVehicleId1.CompareTo(eVehicleId2) > 0;

        }

        #endregion

        #region Operator >= (eVehicleId1, eVehicleId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId1">A electric vehicle identification.</param>
        /// <param name="eVehicleId2">Another electric vehicle identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eVehicle_Id  eVehicleId1,
                                           eVehicle_Id  eVehicleId2)

            => !(eVehicleId1 < eVehicleId2);

        #endregion

        #endregion

        #region IComparable<eVehicle_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eVehicleId.
            var eVehicleId = Object as eVehicle_Id;
            if ((Object) eVehicleId == null)
                throw new ArgumentException("The given object is not a eVehicleId!");

            return CompareTo(eVehicleId);

        }

        #endregion

        #region CompareTo(eVehicleId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eVehicleId">An object to compare with.</param>
        public Int32 CompareTo(eVehicle_Id eVehicleId)
        {

            if ((Object) eVehicleId == null)
                throw new ArgumentNullException(nameof(eVehicleId),  "The given electric vehicle identification must not be null!");

            // Compare the length of the eVehicleIds
            var _Result = this.Length.CompareTo(eVehicleId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = ProviderId.CompareTo(eVehicleId.ProviderId);

            // If equal: Compare eVehicleId suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, eVehicleId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eVehicleId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an eVehicleId.
            var eVehicleId = Object as eVehicle_Id;
            if ((Object) eVehicleId == null)
                return false;

            return this.Equals(eVehicleId);

        }

        #endregion

        #region Equals(eVehicleId)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="eVehicleId">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eVehicle_Id eVehicleId)
        {

            if ((Object) eVehicleId == null)
                return false;

            if (!ProviderId.Equals(eVehicleId.ProviderId))
                return false;

            return Suffix.Equals(eVehicleId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ProviderId.GetHashCode() * 17 ^ Suffix.GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(ProviderId, "*V", Suffix);

        #endregion

    }

}
