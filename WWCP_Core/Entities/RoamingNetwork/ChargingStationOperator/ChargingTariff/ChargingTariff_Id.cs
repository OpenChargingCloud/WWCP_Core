/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The unique identification of of a group of electric vehicle charging tariff.
    /// </summary>
    public struct ChargingTariff_Id : IId,
                                      IEquatable<ChargingTariff_Id>,
                                      IComparable<ChargingTariff_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random                       = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging tariff identification.
        /// </summary>
        public  static readonly Regex  ChargingTariffId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?T([a-zA-Z0-9_][a-zA-Z0-9_\*\-\.€\$]{0,50})$",
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
            => (UInt64) (OperatorId.ToString(OperatorIdFormats.ISO_STAR).Length + 2 + Suffix.Length);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new electric vehicle charging tariff identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        private ChargingTariff_Id(ChargingStationOperator_Id  OperatorId,
                                  String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging tariff identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging tariff.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging tariff identification.</param>
        public static ChargingTariff_Id Random(ChargingStationOperator_Id  OperatorId,
                                               Func<String, String>        Mapper  = null)


            => new ChargingTariff_Id(OperatorId,
                                     Mapper != null ? Mapper(_Random.RandomString(30)) : _Random.RandomString(30));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging tariff identification.</param>
        public static ChargingTariff_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging tariff identification must not be null or empty!");

            #endregion

            var MatchCollection = ChargingTariffId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging tariff identification: '" + Text + "'!",
                                            nameof(Text));

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out ChargingStationOperator_Id _OperatorId))
                return new ChargingTariff_Id(_OperatorId,
                                             MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging tariff identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        public static ChargingTariff_Id Parse(ChargingStationOperator_Id  OperatorId,
                                              String                      Suffix)

            => Parse(OperatorId.ToString(OperatorIdFormats.ISO_STAR) + "*T" + Suffix);

        #endregion

        #region TryParse(Text, out ChargingTariff_Id)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingTariff_Id ChargingTariffId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingTariffId = default(ChargingTariff_Id);
                return false;
            }

            #endregion

            try
            {

                ChargingTariffId = default(ChargingTariff_Id);

                var _MatchCollection = ChargingTariffId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id _OperatorId;

                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                {

                    ChargingTariffId = new ChargingTariff_Id(_OperatorId,
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

            ChargingTariffId = default(ChargingTariff_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging tariff identification.
        /// </summary>
        public ChargingTariff_Id Clone

            => new ChargingTariff_Id(OperatorId.Clone,
                                     new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariff_Id ChargingTariffId1, ChargingTariff_Id ChargingTariffId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingTariffId1, ChargingTariffId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingTariffId1 == null) || ((Object) ChargingTariffId2 == null))
                return false;

            return ChargingTariffId1.Equals(ChargingTariffId2);

        }

        #endregion

        #region Operator != (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingTariff_Id ChargingTariffId1, ChargingTariff_Id ChargingTariffId2)
            => !(ChargingTariffId1 == ChargingTariffId2);

        #endregion

        #region Operator <  (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariff_Id ChargingTariffId1, ChargingTariff_Id ChargingTariffId2)
        {

            if ((Object) ChargingTariffId1 == null)
                throw new ArgumentNullException(nameof(ChargingTariffId1), "The given ChargingTariffId1 must not be null!");

            return ChargingTariffId1.CompareTo(ChargingTariffId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingTariff_Id ChargingTariffId1, ChargingTariff_Id ChargingTariffId2)
            => !(ChargingTariffId1 > ChargingTariffId2);

        #endregion

        #region Operator >  (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariff_Id ChargingTariffId1, ChargingTariff_Id ChargingTariffId2)
        {

            if ((Object) ChargingTariffId1 == null)
                throw new ArgumentNullException(nameof(ChargingTariffId1), "The given ChargingTariffId1 must not be null!");

            return ChargingTariffId1.CompareTo(ChargingTariffId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingTariff_Id ChargingTariffId1, ChargingTariff_Id ChargingTariffId2)
            => !(ChargingTariffId1 < ChargingTariffId2);

        #endregion

        #endregion

        #region IComparable<ChargingTariff_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingTariff_Id))
                throw new ArgumentException("The given object is not a charging tariff identification!", nameof(Object));

            return CompareTo((ChargingTariff_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingTariff_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging tariff identification must not be null!");

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

        #region IEquatable<ChargingTariff_Id> Members

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

            if (!(Object is ChargingTariff_Id))
                return false;

            return Equals((ChargingTariff_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging tariff identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging tariff identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingTariff_Id ChargingStationId)
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
