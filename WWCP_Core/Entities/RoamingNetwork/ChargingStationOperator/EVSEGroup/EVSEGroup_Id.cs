/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The unique identification of a group of Electric Vehicle Supply Equipments (EVSEs).
    /// </summary>
    public struct EVSEGroup_Id : IId,
                                 IEquatable<EVSEGroup_Id>,
                                 IComparable<EVSEGroup_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random            = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing an EVSE group identification.
        /// </summary>
        public  static readonly Regex  EVSEGroupId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?GE([A-Z0-9][A-Z0-9\*]{0,30})$",
                                                                      RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                      Suffix       { get; }

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (OperatorId.ToFormat(OperatorIdFormats.ISO_STAR).Length + 3 + Suffix.Length);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric vehicle supply equipment group identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the EVSE group identification.</param>
        private EVSEGroup_Id(ChargingStationOperator_Id  OperatorId,
                             String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The EVSE group identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an EVSE group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated EVSE group identification.</param>
        public static EVSEGroup_Id Random(ChargingStationOperator_Id  OperatorId,
                                          Func<String, String>        Mapper  = null)


            => new EVSEGroup_Id(OperatorId,
                                Mapper != null ? Mapper(_Random.RandomString(30)) : _Random.RandomString(30));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an EVSE group identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE group identification.</param>
        public static EVSEGroup_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an EVSE group identification must not be null or empty!");

            #endregion

            var MatchCollection = EVSEGroupId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of an EVSE group identification: '" + Text + "'!",
                                            nameof(Text));

            ChargingStationOperator_Id _OperatorId;

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out _OperatorId))
                return new EVSEGroup_Id(_OperatorId,
                                        MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal EVSE group identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as an EVSE group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the EVSE group identification.</param>
        public static EVSEGroup_Id Parse(ChargingStationOperator_Id  OperatorId,
                                         String                      Suffix)

            => Parse(OperatorId.ToFormat(OperatorIdFormats.ISO_STAR) + "*GE" + Suffix);

        #endregion

        #region TryParse(Text, out EVSEGroupId)

        /// <summary>
        /// Parse the given string as an EVSE group identification.
        /// </summary>
        public static Boolean TryParse(String Text, out EVSEGroup_Id EVSEGroupId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                EVSEGroupId = default(EVSEGroup_Id);
                return false;
            }

            #endregion

            try
            {

                EVSEGroupId = default(EVSEGroup_Id);

                var _MatchCollection = EVSEGroupId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id _OperatorId;

                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                {

                    EVSEGroupId = new EVSEGroup_Id(_OperatorId,
                                                   _MatchCollection[0].Groups[2].Value);

                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception e)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            EVSEGroupId = default(EVSEGroup_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE group identification.
        /// </summary>
        public EVSEGroup_Id Clone

            => new EVSEGroup_Id(OperatorId.Clone,
                                new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (EVSEGroupId1, EVSEGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId1">A EVSE group identification.</param>
        /// <param name="EVSEGroupId2">Another EVSE group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEGroup_Id EVSEGroupId1, EVSEGroup_Id EVSEGroupId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEGroupId1, EVSEGroupId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEGroupId1 == null) || ((Object) EVSEGroupId2 == null))
                return false;

            return EVSEGroupId1.Equals(EVSEGroupId2);

        }

        #endregion

        #region Operator != (EVSEGroupId1, EVSEGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId1">A EVSE group identification.</param>
        /// <param name="EVSEGroupId2">Another EVSE group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEGroup_Id EVSEGroupId1, EVSEGroup_Id EVSEGroupId2)
            => !(EVSEGroupId1 == EVSEGroupId2);

        #endregion

        #region Operator <  (EVSEGroupId1, EVSEGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId1">A EVSE group identification.</param>
        /// <param name="EVSEGroupId2">Another EVSE group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEGroup_Id EVSEGroupId1, EVSEGroup_Id EVSEGroupId2)
        {

            if ((Object) EVSEGroupId1 == null)
                throw new ArgumentNullException(nameof(EVSEGroupId1), "The given EVSEGroupId1 must not be null!");

            return EVSEGroupId1.CompareTo(EVSEGroupId2) < 0;

        }

        #endregion

        #region Operator <= (EVSEGroupId1, EVSEGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId1">A EVSE group identification.</param>
        /// <param name="EVSEGroupId2">Another EVSE group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEGroup_Id EVSEGroupId1, EVSEGroup_Id EVSEGroupId2)
            => !(EVSEGroupId1 > EVSEGroupId2);

        #endregion

        #region Operator >  (EVSEGroupId1, EVSEGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId1">A EVSE group identification.</param>
        /// <param name="EVSEGroupId2">Another EVSE group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEGroup_Id EVSEGroupId1, EVSEGroup_Id EVSEGroupId2)
        {

            if ((Object) EVSEGroupId1 == null)
                throw new ArgumentNullException(nameof(EVSEGroupId1), "The given EVSEGroupId1 must not be null!");

            return EVSEGroupId1.CompareTo(EVSEGroupId2) > 0;

        }

        #endregion

        #region Operator >= (EVSEGroupId1, EVSEGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId1">A EVSE group identification.</param>
        /// <param name="EVSEGroupId2">Another EVSE group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEGroup_Id EVSEGroupId1, EVSEGroup_Id EVSEGroupId2)
            => !(EVSEGroupId1 < EVSEGroupId2);

        #endregion

        #endregion

        #region IComparable<EVSEGroupId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEGroup_Id))
                throw new ArgumentException("The given object is not an EVSE group identification!");

            return CompareTo((EVSEGroup_Id) Object);

        }

        #endregion

        #region CompareTo(EVSEGroupId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEGroupId">An object to compare with.</param>
        public Int32 CompareTo(EVSEGroup_Id EVSEGroupId)
        {

            if ((Object) EVSEGroupId == null)
                throw new ArgumentNullException(nameof(EVSEGroupId),  "The given EVSE group identification must not be null!");

            // Compare the length of the identifications
            var _Result = this.Length.CompareTo(EVSEGroupId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = OperatorId.CompareTo(EVSEGroupId.OperatorId);

            // If equal: Compare suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, EVSEGroupId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEGroupId> Members

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

            if (!(Object is EVSEGroup_Id))
                return false;

            return Equals((EVSEGroup_Id) Object);

        }

        #endregion

        #region Equals(EVSEGroupId)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEGroupId">An EVSE group identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEGroup_Id EVSEGroupId)
        {

            if ((Object) EVSEGroupId == null)
                return false;

            return OperatorId.Equals(EVSEGroupId.OperatorId) &&
                   Suffix.    Equals(EVSEGroupId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.GetHashCode() ^
               Suffix.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*GE", Suffix);

        #endregion

    }

}
