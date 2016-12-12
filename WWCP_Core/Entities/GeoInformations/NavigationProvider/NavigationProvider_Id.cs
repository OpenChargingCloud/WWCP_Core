/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Service Provider (EVSP Id).
    /// </summary>
    public class NavigationProvider_Id : IId,
                                        IEquatable <NavigationProvider_Id>,
                                        IComparable<NavigationProvider_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE Service Provider identification.
        /// </summary>
        public  const    String   ProviderId_RegEx            = @"^[A-Z0-9]{3}$";

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an EV Service Provider identification.
        /// The ISO format onyl allows '-' as a separator!
        /// </summary>
        public  const    String   CountryAndProviderId_RegEx  = @"^([A-Z]{2})([\*|\-]?)([A-Z0-9]{3})$";

        #endregion

        #region Properties

        #region Length

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return (UInt64) _ProviderId.Length;
            }
        }

        #endregion

        #region CountryCode

        private readonly Country _CountryCode;

        /// <summary>
        /// The internal Alpha-2-CountryCode.
        /// </summary>
        public Country CountryCode
        {
            get
            {
                return _CountryCode;
            }
        }

        #endregion

        #region IdFormat

        private readonly ProviderIdFormats _IdFormat;

        /// <summary>
        /// The EVSP Id format.
        /// </summary>
        public ProviderIdFormats IdFormat
        {
            get
            {
                return _IdFormat;
            }
        }

        #endregion

        #region ProviderId

        private readonly String _ProviderId;

        /// <summary>
        /// The internal EV Service Provider identification.
        /// </summary>
        public String ProviderId
        {
            get
            {
                return _ProviderId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Service Provider (EVSP Id)
        /// based on the given string.
        /// </summary>
        /// <param name="CountryCode">The Alpha-2-CountryCode.</param>
        /// <param name="IdFormat">The id format '-' (ISO) or '*|-' DIN to use.</param>
        /// <param name="ProviderId">The EV Service Provider identification.</param>
        private NavigationProvider_Id(Country            CountryCode,
                                     ProviderIdFormats  IdFormat,
                                     String             ProviderId)
        {

            this._CountryCode  = CountryCode;
            this._IdFormat     = IdFormat;
            this._ProviderId   = ProviderId;

        }

        #endregion


        #region Parse(CountryAndProviderId)

        /// <summary>
        /// Parse the given string as an EV Service Provider identification.
        /// </summary>
        /// <param name="CountryAndProviderId">The country code and EV Service Provider identification as a string.</param>
        public static NavigationProvider_Id Parse(String CountryAndProviderId)
        {

            #region Initial checks

            if (CountryAndProviderId.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "CountryAndProviderId");

            #endregion

            var _MatchCollection = Regex.Matches(CountryAndProviderId.Trim().ToUpper(),
                                                 CountryAndProviderId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EV Service Provider identification '" + CountryAndProviderId + "'!", "CountryAndProviderId");

            Country __CountryCode;

            if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
            {

                ProviderIdFormats Separator = ProviderIdFormats.ISO_HYPHEN;

                switch (_MatchCollection[0].Groups[2].Value)
                {

                    case ""  : Separator = ProviderIdFormats.DIN|ProviderIdFormats.ISO; break;
                    case "-" : Separator = ProviderIdFormats.DIN_HYPHEN|ProviderIdFormats.ISO_HYPHEN; break;
                    case "*" : Separator = ProviderIdFormats.DIN_STAR; break;

                    default: throw new ArgumentException("Illegal EV Service Provider identification!", "CountryAndProviderId");

                }

                return new NavigationProvider_Id(__CountryCode,
                                   Separator,
                                   _MatchCollection[0].Groups[3].Value);
            }

            throw new ArgumentException("Illegal EV Service Provider identification!", "CountryAndProviderId");

        }

        #endregion

        #region Parse(CountryCode, ProviderId, ProviderIdFormat = ProviderIdFormats.ISO_HYPHEN)

        /// <summary>
        /// Parse the given string as an EV Service Provider identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="ProviderId">An EV Service Provider identification as a string.</param>
        /// <param name="ProviderIdFormat">The optional format of the provider identification.</param>
        public static NavigationProvider_Id Parse(Country            CountryCode,
                                    String             ProviderId,
                                    ProviderIdFormats  ProviderIdFormat = ProviderIdFormats.ISO_HYPHEN)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The country code must not be null!");

            if (ProviderId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ProviderId),   "The provider identification must not be null or empty!");

            #endregion

            var _MatchCollection = Regex.Matches(ProviderId.Trim().ToUpper(),
                                                 ProviderId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EV Service Provider identification '" + CountryCode + " / " + ProviderId + "'!", nameof(ProviderId));

            return new NavigationProvider_Id(CountryCode,
                               ProviderIdFormat,
                               _MatchCollection[0].Value);

        }

        #endregion

        #region TryParse(CountryAndProviderId, out EVSEProviderId)

        /// <summary>
        /// Parse the given string as an EV Service Provider identification.
        /// </summary>
        /// <param name="CountryAndProviderId">The country code and EV Service Provider identification as a string.</param>
        /// <param name="EVSEProviderId">The parsed EV Service Provider identification.</param>
        public static Boolean TryParse(String       CountryAndProviderId,
                                       out NavigationProvider_Id  EVSEProviderId)
        {

            #region Initial checks

            if (CountryAndProviderId.IsNullOrEmpty())
            {
                EVSEProviderId = null;
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(CountryAndProviderId.Trim().ToUpper(),
                                                     CountryAndProviderId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEProviderId = null;
                    return false;
                }

                Country __CountryCode;

                if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
                {

                    ProviderIdFormats Separator = ProviderIdFormats.ISO_HYPHEN;

                    switch (_MatchCollection[0].Groups[2].Value)
                    {

                        case ""  : Separator = ProviderIdFormats.DIN|ProviderIdFormats.ISO; break;
                        case "-" : Separator = ProviderIdFormats.ISO_HYPHEN;                break;
                        case "*" : Separator = ProviderIdFormats.DIN_STAR;                  break;

                        default: throw new ArgumentException("Illegal EV Service Provider identification!", nameof(CountryAndProviderId));

                    }

                    EVSEProviderId = new NavigationProvider_Id(__CountryCode,
                                                 Separator,
                                                 _MatchCollection[0].Groups[3].Value);

                    return true;

                }

            }

            catch (Exception e)
            { }

            EVSEProviderId = null;
            return false;

        }

        #endregion

        #region TryParse(CountryCode, ProviderId, out EVSEProviderId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="ProviderId">An Charging Station Operator identification as a string.</param>
        /// <param name="EVSEProviderId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(Country      CountryCode,
                                       String       ProviderId,
                                       out NavigationProvider_Id  EVSEProviderId)
        {

            #region Initial checks

            if (CountryCode == null || ProviderId.IsNullOrEmpty())
            {
                EVSEProviderId = null;
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(ProviderId.Trim().ToUpper(),
                                                     ProviderId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEProviderId = null;
                    return false;
                }

                EVSEProviderId = new NavigationProvider_Id(CountryCode,
                                             ProviderIdFormats.DIN | ProviderIdFormats.ISO,
                                             _MatchCollection[0].Value);

                return true;

            }

            catch (Exception)
            {
                EVSEProviderId = null;
                return false;
            }

        }

        #endregion

        #region ChangeIdFormat(NewIdFormat)

        /// <summary>
        /// Change the EVSP Id format.
        /// </summary>
        /// <param name="NewIdFormat">The new EVSP Id format.</param>
        /// <returns>A new EVSPId object.</returns>
        public NavigationProvider_Id ChangeIdFormat(ProviderIdFormats NewIdFormat)
        {
            return new NavigationProvider_Id(this._CountryCode, NewIdFormat, this.ProviderId);
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Service Provider identification.
        /// </summary>
        public NavigationProvider_Id Clone
        {
            get
            {

                return new NavigationProvider_Id(_CountryCode,
                                   _IdFormat,
                                   new String(_ProviderId.ToCharArray()));

            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (NavigationProvider_Id EVSPId1, NavigationProvider_Id EVSPId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSPId1, EVSPId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSPId1 == null) || ((Object) EVSPId2 == null))
                return false;

            return EVSPId1.Equals(EVSPId2);

        }

        #endregion

        #region Operator != (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (NavigationProvider_Id EVSPId1, NavigationProvider_Id EVSPId2)
        {
            return !(EVSPId1 == EVSPId2);
        }

        #endregion

        #region Operator <  (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (NavigationProvider_Id EVSPId1, NavigationProvider_Id EVSPId2)
        {

            if ((Object) EVSPId1 == null)
                throw new ArgumentNullException("The given EVSPId1 must not be null!");

            return EVSPId1.CompareTo(EVSPId2) < 0;

        }

        #endregion

        #region Operator <= (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (NavigationProvider_Id EVSPId1, NavigationProvider_Id EVSPId2)
        {
            return !(EVSPId1 > EVSPId2);
        }

        #endregion

        #region Operator >  (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (NavigationProvider_Id EVSPId1, NavigationProvider_Id EVSPId2)
        {

            if ((Object) EVSPId1 == null)
                throw new ArgumentNullException("The given EVSPId1 must not be null!");

            return EVSPId1.CompareTo(EVSPId2) > 0;

        }

        #endregion

        #region Operator >= (EVSPId1, EVSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId1">A EVSPId.</param>
        /// <param name="EVSPId2">Another EVSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (NavigationProvider_Id EVSPId1, NavigationProvider_Id EVSPId2)
        {
            return !(EVSPId1 < EVSPId2);
        }

        #endregion

        #endregion

        #region IComparable<EVSP_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSPId.
            var EVSPId = Object as NavigationProvider_Id;
            if ((Object) EVSPId == null)
                throw new ArgumentException("The given object is not a EVSPId!");

            return CompareTo(EVSPId);

        }

        #endregion

        #region CompareTo(EVSPId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSPId">An object to compare with.</param>
        public Int32 CompareTo(NavigationProvider_Id EVSPId)
        {

            if ((Object) EVSPId == null)
                throw new ArgumentNullException("The given EVSPId must not be null!");

            // Compare the length of the EVSPIds
            var _Result = this.Length.CompareTo(EVSPId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _ProviderId.CompareTo(EVSPId._ProviderId);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSP_Id> Members

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

            // Check if the given object is an EVSPId.
            var EVSPId = Object as NavigationProvider_Id;
            if ((Object) EVSPId == null)
                return false;

            return this.Equals(EVSPId);

        }

        #endregion

        #region Equals(EVSPId)

        /// <summary>
        /// Compares two EVSPIds for equality.
        /// </summary>
        /// <param name="EVSPId">A EVSPId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(NavigationProvider_Id EVSPId)
        {

            if ((Object) EVSPId == null)
                return false;

            return _ProviderId.Equals(EVSPId._ProviderId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return _ProviderId.GetHashCode();
        }

        #endregion

        #region ToString(IdFormat)

        /// <summary>
        /// Return a string representation of this object
        /// in the given Id format.
        /// </summary>
        public String ToString(ProviderIdFormats IdFormat)
        {

            switch (IdFormat)
            {

                case ProviderIdFormats.DIN_HYPHEN:
                case ProviderIdFormats.ISO_HYPHEN:
                    return String.Concat(CountryCode.Alpha2Code.ToUpper(), "-", _ProviderId.ToString());

                case ProviderIdFormats.DIN_STAR:
                    return String.Concat(CountryCode.Alpha2Code.ToUpper(), "*", _ProviderId.ToString());

                default:
                    return String.Concat(CountryCode.Alpha2Code.ToUpper(), _ProviderId.ToString());

            }

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return ToString(_IdFormat);
        }

        #endregion

    }

}
