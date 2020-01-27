/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The unique identification of of a group of electric vehicle charging tariff group.
    /// </summary>
    public struct ChargingTariffGroup_Id : IId,
                                           IEquatable<ChargingTariffGroup_Id>,
                                           IComparable<ChargingTariffGroup_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random                       = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging tariff group identification.
        /// </summary>
        public  static readonly Regex  ChargingTariffGroupId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?TG([a-zA-Z0-9_][a-zA-Z0-9_\*\-\.€\$]{0,50})$",
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
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (OperatorId.ToString(OperatorIdFormats.ISO_STAR).Length + 2 + Suffix.Length);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric vehicle charging tariff group identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        private ChargingTariffGroup_Id(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging tariff group identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging tariff group.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging tariff group identification.</param>
        public static ChargingTariffGroup_Id Random(ChargingStationOperator_Id  OperatorId,
                                               Func<String, String>        Mapper  = null)


            => new ChargingTariffGroup_Id(OperatorId,
                                     Mapper != null ? Mapper(_Random.RandomString(30)) : _Random.RandomString(30));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging tariff group identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging tariff group identification.</param>
        public static ChargingTariffGroup_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging tariff group identification must not be null or empty!");

            #endregion

            var MatchCollection = ChargingTariffGroupId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging tariff group identification: '" + Text + "'!",
                                            nameof(Text));

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out ChargingStationOperator_Id _OperatorId))
                return new ChargingTariffGroup_Id(_OperatorId,
                                             MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging tariff group identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging tariff group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging tariff group identification.</param>
        public static ChargingTariffGroup_Id Parse(ChargingStationOperator_Id  OperatorId,
                                              String                      Suffix)

            => Parse(OperatorId.ToString(OperatorIdFormats.ISO_STAR) + "*TG" + Suffix);

        #endregion

        #region TryParse(Text, out ChargingTariffGroup_Id)

        /// <summary>
        /// Parse the given string as a charging tariff group identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingTariffGroup_Id ChargingTariffGroupId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingTariffGroupId = default(ChargingTariffGroup_Id);
                return false;
            }

            #endregion

            try
            {

                ChargingTariffGroupId = default(ChargingTariffGroup_Id);

                var _MatchCollection = ChargingTariffGroupId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id _OperatorId;

                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                {

                    ChargingTariffGroupId = new ChargingTariffGroup_Id(_OperatorId,
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

            ChargingTariffGroupId = default(ChargingTariffGroup_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging tariff group identification.
        /// </summary>
        public ChargingTariffGroup_Id Clone

            => new ChargingTariffGroup_Id(OperatorId.Clone,
                                     new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariffGroupId1, ChargingTariffGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroupId1">A charging tariff group identification.</param>
        /// <param name="ChargingTariffGroupId2">Another charging tariff group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariffGroup_Id ChargingTariffGroupId1, ChargingTariffGroup_Id ChargingTariffGroupId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingTariffGroupId1, ChargingTariffGroupId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingTariffGroupId1 == null) || ((Object) ChargingTariffGroupId2 == null))
                return false;

            return ChargingTariffGroupId1.Equals(ChargingTariffGroupId2);

        }

        #endregion

        #region Operator != (ChargingTariffGroupId1, ChargingTariffGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroupId1">A charging tariff group identification.</param>
        /// <param name="ChargingTariffGroupId2">Another charging tariff group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingTariffGroup_Id ChargingTariffGroupId1, ChargingTariffGroup_Id ChargingTariffGroupId2)
            => !(ChargingTariffGroupId1 == ChargingTariffGroupId2);

        #endregion

        #region Operator <  (ChargingTariffGroupId1, ChargingTariffGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroupId1">A charging tariff group identification.</param>
        /// <param name="ChargingTariffGroupId2">Another charging tariff group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariffGroup_Id ChargingTariffGroupId1, ChargingTariffGroup_Id ChargingTariffGroupId2)
        {

            if ((Object) ChargingTariffGroupId1 == null)
                throw new ArgumentNullException(nameof(ChargingTariffGroupId1), "The given ChargingTariffGroupId1 must not be null!");

            return ChargingTariffGroupId1.CompareTo(ChargingTariffGroupId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingTariffGroupId1, ChargingTariffGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroupId1">A charging tariff group identification.</param>
        /// <param name="ChargingTariffGroupId2">Another charging tariff group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingTariffGroup_Id ChargingTariffGroupId1, ChargingTariffGroup_Id ChargingTariffGroupId2)
            => !(ChargingTariffGroupId1 > ChargingTariffGroupId2);

        #endregion

        #region Operator >  (ChargingTariffGroupId1, ChargingTariffGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroupId1">A charging tariff group identification.</param>
        /// <param name="ChargingTariffGroupId2">Another charging tariff group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariffGroup_Id ChargingTariffGroupId1, ChargingTariffGroup_Id ChargingTariffGroupId2)
        {

            if ((Object) ChargingTariffGroupId1 == null)
                throw new ArgumentNullException(nameof(ChargingTariffGroupId1), "The given ChargingTariffGroupId1 must not be null!");

            return ChargingTariffGroupId1.CompareTo(ChargingTariffGroupId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingTariffGroupId1, ChargingTariffGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffGroupId1">A charging tariff group identification.</param>
        /// <param name="ChargingTariffGroupId2">Another charging tariff group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingTariffGroup_Id ChargingTariffGroupId1, ChargingTariffGroup_Id ChargingTariffGroupId2)
            => !(ChargingTariffGroupId1 < ChargingTariffGroupId2);

        #endregion

        #endregion

        #region IComparable<ChargingTariffGroup_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingTariffGroup_Id))
                throw new ArgumentException("The given object is not a charging tariff group identification!", nameof(Object));

            return CompareTo((ChargingTariffGroup_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingTariffGroup_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging tariff group identification must not be null!");

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

        #region IEquatable<ChargingTariffGroup_Id> Members

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

            if (!(Object is ChargingTariffGroup_Id))
                return false;

            return Equals((ChargingTariffGroup_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging tariff group identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging tariff group identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingTariffGroup_Id ChargingStationId)
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
            => String.Concat(OperatorId, "*T", Suffix);

        #endregion

    }

}
