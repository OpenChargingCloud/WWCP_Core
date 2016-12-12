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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an e-mobility station.
    /// </summary>
    public class eMobilityStation_Id : IId,
                                       IEquatable<eMobilityStation_Id>,
                                       IComparable<eMobilityStation_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing an e-mobility station identification.
        /// </summary>
        public    const    String  eMobilityStationId_RegEx  = @"^([A-Za-z]{2}\*?[A-Za-z0-9]{3})\*?M([A-Z0-9][A-Z0-9\*]{0,30})$";

        /// <summary>
        /// The regular expression for parsing an e-mobility station identification.
        /// </summary>
        public    const    String  IdSuffix_RegEx           = @"^[A-Z0-9][A-Z0-9\*]{0,30}$";

        #endregion

        #region Properties

        /// <summary>
        /// The the e-mobility provider identification.
        /// </summary>
        public eMobilityProvider_Id  ProviderId  { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                Suffix      { get; }

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => ProviderId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new e-mobility station identification
        /// based on the given string.
        /// </summary>
        private eMobilityStation_Id(eMobilityProvider_Id  ProviderId,
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
                throw new ArgumentException("Illegal e-mobility station identification suffix '" + Suffix + "'!", nameof(Suffix));

            this.ProviderId  = ProviderId;
            this.Suffix      = _MatchCollection[0].Value;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle Charging Station (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an Charging Station Operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static eMobilityStation_Id Random(eMobilityProvider_Id  ProviderId,
                                                Func<String, String>   Mapper    = null)
        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The parameter must not be null!");

            #endregion

            return new eMobilityStation_Id(ProviderId,
                                           Mapper != null ? Mapper(_Random.RandomString(12)) : _Random.RandomString(12));

        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        public static eMobilityStation_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", nameof(Text));

            #endregion

            var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                 eMobilityStationId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal e-mobility station identification '" + Text + "'!", nameof(Text));

            eMobilityProvider_Id __EVSEOperatorId;

            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new eMobilityStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[2].Value);

            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new eMobilityStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[4].Value);

            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[5].Value, out __EVSEOperatorId))
                return new eMobilityStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[6].Value);

            if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[7].Value, out __EVSEOperatorId))
                return new eMobilityStation_Id(__EVSEOperatorId,
                                              _MatchCollection[0].Groups[8].Value);

            throw new ArgumentException("Illegal e-mobility station identification '" + Text + "'!", nameof(Text));

        }

        #endregion

        #region Parse(ProviderId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an Charging Station Operator.</param>
        /// <param name="Suffix">A text representation of a charging station identification.</param>
        public static eMobilityStation_Id Parse(eMobilityProvider_Id  ProviderId,
                                                String                Suffix)
        {

            #region Initial checks

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),  "The unique provider identification must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),      "The suffix must not be null or empty!");

            #endregion

            eMobilityStation_Id _eMobilityStationId = null;

            if (TryParse(ProviderId, Suffix, out _eMobilityStationId))
                return _eMobilityStationId;

            return null;

        }

        #endregion

        #region TryParse(Text, out eMobilityStationId)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of a charging station identification.</param>
        /// <param name="eMobilityStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(String Text, out eMobilityStation_Id eMobilityStationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                eMobilityStationId = null;
                return false;
            }

            #endregion

            try
            {

                eMobilityStationId = null;

                var _MatchCollection = Regex.Matches(Text.Trim().ToUpper(),
                                                     eMobilityStationId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                eMobilityProvider_Id __EVSEOperatorId;

                // New format...
                if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    eMobilityStationId = new eMobilityStation_Id(__EVSEOperatorId,
                                                                 _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    eMobilityStationId = new eMobilityStation_Id(__EVSEOperatorId,
                                                                 _MatchCollection[0].Groups[4].Value);

                    return true;

                }

                // New format without the 'S'...
                else if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[5].Value, out __EVSEOperatorId))
                {

                    eMobilityStationId = new eMobilityStation_Id(__EVSEOperatorId,
                                                                 _MatchCollection[0].Groups[6].Value);

                    return true;

                }

                // Old format without the 'S'...
                else if (eMobilityProvider_Id.TryParse(_MatchCollection[0].Groups[7].Value, out __EVSEOperatorId))
                {

                    eMobilityStationId = new eMobilityStation_Id(__EVSEOperatorId,
                                                                 _MatchCollection[0].Groups[8].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            eMobilityStationId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, Suffix, out eMobilityStationId)

        /// <summary>
        /// Parse the given string as a charging station identification (EVCS Id).
        /// </summary>
        /// <param name="OperatorId">The unique identification of an Charging Station Operator.</param>
        /// <param name="Suffix">A text representation of a charging station identification.</param>
        /// <param name="eMobilityStationId">The parsed charging station identification.</param>
        public static Boolean TryParse(eMobilityProvider_Id         OperatorId,
                                       String                  Suffix,
                                       out eMobilityStation_Id  eMobilityStationId)
        {

            #region Initial checks

            if (OperatorId == null || Suffix.IsNullOrEmpty())
            {
                eMobilityStationId = null;
                return false;
            }

            #endregion

            try
            {

                eMobilityStationId = null;

                var _MatchCollection = Regex.Matches(Suffix.Trim().ToUpper(),
                                                     IdSuffix_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                    return false;

                eMobilityStationId = new eMobilityStation_Id(OperatorId,
                                                             _MatchCollection[0].Groups[0].Value);

                return true;

            }
            catch (Exception e)
            { }

            eMobilityStationId = null;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public eMobilityStation_Id Clone
        {
            get
            {
                return new eMobilityStation_Id(ProviderId,
                                              new String(Suffix.ToCharArray()));
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (eMobilityStationId1, eMobilityStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId1">A eMobilityStation_Id.</param>
        /// <param name="eMobilityStationId2">Another eMobilityStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (eMobilityStation_Id eMobilityStationId1, eMobilityStation_Id eMobilityStationId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(eMobilityStationId1, eMobilityStationId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) eMobilityStationId1 == null) || ((Object) eMobilityStationId2 == null))
                return false;

            return eMobilityStationId1.Equals(eMobilityStationId2);

        }

        #endregion

        #region Operator != (eMobilityStationId1, eMobilityStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId1">A eMobilityStation_Id.</param>
        /// <param name="eMobilityStationId2">Another eMobilityStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (eMobilityStation_Id eMobilityStationId1, eMobilityStation_Id eMobilityStationId2)
        {
            return !(eMobilityStationId1 == eMobilityStationId2);
        }

        #endregion

        #region Operator <  (eMobilityStationId1, eMobilityStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId1">A eMobilityStation_Id.</param>
        /// <param name="eMobilityStationId2">Another eMobilityStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (eMobilityStation_Id eMobilityStationId1, eMobilityStation_Id eMobilityStationId2)
        {

            if ((Object) eMobilityStationId1 == null)
                throw new ArgumentNullException("The given eMobilityStationId1 must not be null!");

            return eMobilityStationId1.CompareTo(eMobilityStationId2) < 0;

        }

        #endregion

        #region Operator <= (eMobilityStationId1, eMobilityStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId1">A eMobilityStation_Id.</param>
        /// <param name="eMobilityStationId2">Another eMobilityStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (eMobilityStation_Id eMobilityStationId1, eMobilityStation_Id eMobilityStationId2)
        {
            return !(eMobilityStationId1 > eMobilityStationId2);
        }

        #endregion

        #region Operator >  (eMobilityStationId1, eMobilityStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId1">A eMobilityStation_Id.</param>
        /// <param name="eMobilityStationId2">Another eMobilityStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (eMobilityStation_Id eMobilityStationId1, eMobilityStation_Id eMobilityStationId2)
        {

            if ((Object) eMobilityStationId1 == null)
                throw new ArgumentNullException("The given eMobilityStationId1 must not be null!");

            return eMobilityStationId1.CompareTo(eMobilityStationId2) > 0;

        }

        #endregion

        #region Operator >= (eMobilityStationId1, eMobilityStationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId1">A eMobilityStation_Id.</param>
        /// <param name="eMobilityStationId2">Another eMobilityStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (eMobilityStation_Id eMobilityStationId1, eMobilityStation_Id eMobilityStationId2)
        {
            return !(eMobilityStationId1 < eMobilityStationId2);
        }

        #endregion

        #endregion

        #region IComparable<eMobilityStation_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an eMobilityStationId.
            var eMobilityStationId = Object as eMobilityStation_Id;
            if ((Object) eMobilityStationId == null)
                throw new ArgumentException("The given object is not a eMobilityStationId!");

            return CompareTo(eMobilityStationId);

        }

        #endregion

        #region CompareTo(eMobilityStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMobilityStationId">An object to compare with.</param>
        public Int32 CompareTo(eMobilityStation_Id eMobilityStationId)
        {

            if ((Object) eMobilityStationId == null)
                throw new ArgumentNullException("The given eMobilityStationId must not be null!");

            // Compare the length of the eMobilityStationIds
            var _Result = this.Length.CompareTo(eMobilityStationId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = ProviderId.CompareTo(eMobilityStationId.ProviderId);

            // If equal: Compare eMobilityStationId suffix
            if (_Result == 0)
                _Result = Suffix.CompareTo(eMobilityStationId.Suffix);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<eMobilityStation_Id> Members

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

            // Check if the given object is an eMobilityStationId.
            var eMobilityStationId = Object as eMobilityStation_Id;
            if ((Object) eMobilityStationId == null)
                return false;

            return this.Equals(eMobilityStationId);

        }

        #endregion

        #region Equals(eMobilityStationId)

        /// <summary>
        /// Compares two charging station identifications for equality.
        /// </summary>
        /// <param name="eMobilityStationId">A charging station identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(eMobilityStation_Id eMobilityStationId)
        {

            if ((Object) eMobilityStationId == null)
                return false;

            return ProviderId.Equals(eMobilityStationId.ProviderId) &&
                   Suffix.  Equals(eMobilityStationId.Suffix);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(ProviderId, "*M", Suffix);

        #endregion

    }

}
