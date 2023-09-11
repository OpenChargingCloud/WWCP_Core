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

using System;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of of a group of electric vehicle charging stations.
    /// </summary>
    public readonly struct ChargingStationGroup_Id : IId,
                                                     IEquatable<ChargingStationGroup_Id>,
                                                     IComparable<ChargingStationGroup_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging station group identification.
        /// </summary>
        public static readonly Regex ChargingStationGroupId_RegEx  = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?GS([a-zA-Z0-9_][a-zA-Z0-9_\*\-\.€\$]{0,50})$",
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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (OperatorId.ToString(OperatorIdFormats.ISO_STAR).Length + 3 + Suffix.Length);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric vehicle charging station group identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        private ChargingStationGroup_Id(ChargingStationOperator_Id  OperatorId,
                                        String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging station group identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging station group.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging station group identification.</param>
        public static ChargingStationGroup_Id Random(ChargingStationOperator_Id  OperatorId,
                                                     Func<String, String>?       Mapper   = null)


            => new (OperatorId,
                    Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(30))
                        :        RandomExtensions.RandomString(30));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging station group identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station group identification.</param>
        public static ChargingStationGroup_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station group identification must not be null or empty!");

            #endregion

            var matchCollection = ChargingStationGroupId_RegEx.Matches(Text);

            if (matchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging station group identification: '{Text}'!",
                                            nameof(Text));

            if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out ChargingStationOperator_Id chargingStationOperatorId))
                return new ChargingStationGroup_Id(chargingStationOperatorId,
                                                   matchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging station group identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station group identification.</param>
        public static ChargingStationGroup_Id Parse(ChargingStationOperator_Id  OperatorId,
                                                    String                      Suffix)

            => Parse(OperatorId.ToString(OperatorIdFormats.ISO_STAR) + "*GS" + Suffix);

        #endregion

        #region Parse(OperatorId, ChargingTariffGroupId, Suffix)

        /// <summary>
        /// Parse the given string as a charging station group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging station group identification.</param>
        public static ChargingStationGroup_Id Parse(ChargingStationOperator_Id  OperatorId,
                                                    ChargingTariffGroup_Id      ChargingTariffGroupId,
                                                    String                      Suffix)

            => Parse(OperatorId.ToString(OperatorIdFormats.ISO_STAR) + "*GS_" + ChargingTariffGroupId + "_" + Suffix);

        #endregion

        #region TryParse(Text, out ChargingStationGroup_Id)

        /// <summary>
        /// Parse the given string as a charging station group identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingStationGroup_Id ChargingStationGroupId)
        {

            #region Initial checks

            ChargingStationGroupId = default;

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = ChargingStationGroupId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;

                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out ChargingStationOperator_Id chargingStationOperatorId))
                {

                    ChargingStationGroupId = new ChargingStationGroup_Id(chargingStationOperatorId,
                                                                         matchCollection[0].Groups[2].Value);

                    return true;

                }

            }
            catch
            { }

            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging station group identification.
        /// </summary>
        public ChargingStationGroup_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A charging station group identification.</param>
        /// <param name="ChargingStationGroupId2">Another charging station group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ChargingStationGroupId1, ChargingStationGroupId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStationGroupId1 == null) || ((Object) ChargingStationGroupId2 == null))
                return false;

            return ChargingStationGroupId1.Equals(ChargingStationGroupId2);

        }

        #endregion

        #region Operator != (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A charging station group identification.</param>
        /// <param name="ChargingStationGroupId2">Another charging station group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
            => !(ChargingStationGroupId1 == ChargingStationGroupId2);

        #endregion

        #region Operator <  (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A charging station group identification.</param>
        /// <param name="ChargingStationGroupId2">Another charging station group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {

            if ((Object) ChargingStationGroupId1 == null)
                throw new ArgumentNullException(nameof(ChargingStationGroupId1), "The given ChargingStationGroupId1 must not be null!");

            return ChargingStationGroupId1.CompareTo(ChargingStationGroupId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A charging station group identification.</param>
        /// <param name="ChargingStationGroupId2">Another charging station group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
            => !(ChargingStationGroupId1 > ChargingStationGroupId2);

        #endregion

        #region Operator >  (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A charging station group identification.</param>
        /// <param name="ChargingStationGroupId2">Another charging station group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
        {

            if ((Object) ChargingStationGroupId1 == null)
                throw new ArgumentNullException(nameof(ChargingStationGroupId1), "The given ChargingStationGroupId1 must not be null!");

            return ChargingStationGroupId1.CompareTo(ChargingStationGroupId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStationGroupId1, ChargingStationGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationGroupId1">A charging station group identification.</param>
        /// <param name="ChargingStationGroupId2">Another charging station group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStationGroup_Id ChargingStationGroupId1, ChargingStationGroup_Id ChargingStationGroupId2)
            => !(ChargingStationGroupId1 < ChargingStationGroupId2);

        #endregion

        #endregion

        #region IComparable<ChargingStationGroup_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingStationGroup_Id))
                throw new ArgumentException("The given object is not a charging station group identification!", nameof(Object));

            return CompareTo((ChargingStationGroup_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingStationGroup_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging station group identification must not be null!");

            // Compare the length of the identifications
            var _Result = Length.CompareTo(ChargingStationId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = OperatorId.CompareTo(ChargingStationId.OperatorId);

            // If equal: Compare suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ChargingStationId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStationGroup_Id> Members

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

            if (!(Object is ChargingStationGroup_Id))
                return false;

            return Equals((ChargingStationGroup_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging station group identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging station group identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStationGroup_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                return false;

            return OperatorId.Equals(ChargingStationId.OperatorId) &&
                   Suffix.    Equals(ChargingStationId.Suffix);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*GS", Suffix);

        #endregion

    }

}
