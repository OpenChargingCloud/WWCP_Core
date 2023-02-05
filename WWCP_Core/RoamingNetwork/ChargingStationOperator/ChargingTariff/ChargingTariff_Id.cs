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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for filtering charging tariff identifications.
    /// </summary>
    /// <param name="ChargingTariffId">A charging tariff identification to include.</param>
    public delegate Boolean IncludeChargingTariffIdDelegate(ChargingTariff_Id ChargingTariffId);


    /// <summary>
    /// Extension methods for charging tariff identifications.
    /// </summary>
    public static class ChargingTariffIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging tariff identification is null or empty.
        /// </summary>
        /// <param name="ChargingTariffId">A charging tariff identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingTariff_Id? ChargingTariffId)
            => !ChargingTariffId.HasValue || ChargingTariffId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging tariff identification is null or empty.
        /// </summary>
        /// <param name="ChargingTariffId">A charging tariff identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingTariff_Id? ChargingTariffId)
            => ChargingTariffId.HasValue && ChargingTariffId.Value.IsNotNullOrEmpty;

    }

    /// <summary>
    /// The unique identification of a charging tariff.
    /// </summary>
    public readonly struct ChargingTariff_Id : IId,
                                               IEquatable<ChargingTariff_Id>,
                                               IComparable<ChargingTariff_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging pool identification.
        /// </summary>
        public static readonly Regex ChargingTariffId_RegEx = new(@"^([a-zA-Z]{2}\*?[a-zA-Z0-9]{3})\*?T([a-zA-Z0-9_][a-zA-Z0-9_\*\-\.€\$]{0,50})$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId    { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                      Suffix        { get; }


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
        /// The length of the EVSE admin status.
        /// </summary>
        public UInt64 Length

            => OperatorId.Format switch {
                   OperatorIdFormats.DIN       => OperatorId.Length + 1 + (UInt64) Suffix.Length,
                   OperatorIdFormats.ISO_STAR  => OperatorId.Length + 2 + (UInt64) Suffix.Length,
                   _                           => OperatorId.Length + 1 + (UInt64) Suffix.Length,  // ISO
               };

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging tariff identification based on the given
        /// charging station operator and charging tariff identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        private ChargingTariff_Id(ChargingStationOperator_Id  OperatorId,
                                  String                      Suffix)
        {
            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;
        }

        #endregion


        #region (static) NewRandom(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging tariff identification.</param>
        public static ChargingTariff_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                  Func<String, String>?       Mapper   = null)

            => new (OperatorId,
                    Mapper is not null
                        ? Mapper(RandomExtensions.RandomString(50))
                        :        RandomExtensions.RandomString(50));

        #endregion


        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging tariff identification.</param>
        public static ChargingTariff_Id Parse(String Text)
        {

            if (TryParse(Text, out ChargingTariff_Id chargingTariffId))
                return chargingTariffId;

            throw new ArgumentException("Invalid text representation of a charging tariff identification: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse   (OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        public static ChargingTariff_Id Parse(ChargingStationOperator_Id  OperatorId,
                                              String                      Suffix)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => Parse(String.Concat(OperatorId,  "T", Suffix)),
                   OperatorIdFormats.ISO_STAR  => Parse(String.Concat(OperatorId, "*T", Suffix)),
                   _                           => Parse(String.Concat(OperatorId, "*T", Suffix))
               };

        #endregion

        #region (static) Parse   (OperatorId, TariffGroupId, Suffix)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="TariffGroupId">The unique identification of a charging tariff group.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        public static ChargingTariff_Id Parse(ChargingStationOperator_Id  OperatorId,
                                              ChargingTariffGroup_Id      TariffGroupId,
                                              String                      Suffix)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => Parse(String.Concat(OperatorId,  "T", TariffGroupId, "_", Suffix)),
                   OperatorIdFormats.ISO_STAR  => Parse(String.Concat(OperatorId, "*T", TariffGroupId, "_", Suffix)),
                   _                           => Parse(String.Concat(OperatorId, "*T", TariffGroupId, "_", Suffix))
               };

        #endregion


        #region (static) TryParse(Text)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        public static ChargingTariff_Id? TryParse(String Text)
        {

            if (TryParse(Text, out ChargingTariff_Id chargingTariffId))
                return chargingTariffId;

            return null;

        }

        #endregion

        #region (static) TryParse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        public static ChargingTariff_Id? TryParse(ChargingStationOperator_Id  OperatorId,
                                                  String                      Suffix)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => TryParse(String.Concat(OperatorId,  "T", Suffix)),
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(OperatorId, "*T", Suffix)),
                   _                           => TryParse(String.Concat(OperatorId, "*T", Suffix))
               };

        #endregion

        #region (static) TryParse(OperatorId, TariffGroupId, Suffix)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="TariffGroupId">The unique identification of a charging tariff group.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        public static ChargingTariff_Id? TryParse(ChargingStationOperator_Id  OperatorId,
                                                  ChargingTariffGroup_Id      TariffGroupId,
                                                  String                      Suffix)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => TryParse(String.Concat(OperatorId,  "T", TariffGroupId, "_", Suffix)),
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(OperatorId, "*T", TariffGroupId, "_", Suffix)),
                   _                           => TryParse(String.Concat(OperatorId, "*T", TariffGroupId, "_", Suffix))
               };

        #endregion


        #region (static) TryParse(Text,                              out ChargingTariffId)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging tariff identification.</param>
        /// <param name="ChargingTariffId">The parsed charging tariff identification.</param>
        public static Boolean TryParse(String Text, out ChargingTariff_Id ChargingTariffId)
        {

            #region Initial checks

            ChargingTariffId = default;

            if (Text is null)
                return false;

            Text = Text.Trim();

            if (Text.IsNullOrEmpty())
                return false;

            #endregion

            try
            {

                var matchCollection = ChargingTariffId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;

                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out var chargingStationOperatorId))
                {

                    ChargingTariffId = new ChargingTariff_Id(chargingStationOperatorId,
                                                             matchCollection[0].Groups[2].Value);

                    return true;

                }

            }
            catch (Exception)
            { }

            return false;

        }

        #endregion

        #region (static) TryParse(OperatorId, Suffix,                out ChargingTariffId)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        /// <param name="ChargingTariffId">The parsed charging tariff identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix,
                                       out ChargingTariff_Id       ChargingTariffId)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => TryParse(String.Concat(OperatorId,  "T", Suffix), out ChargingTariffId),
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(OperatorId, "*T", Suffix), out ChargingTariffId),
                   _                           => TryParse(String.Concat(OperatorId, "*T", Suffix), out ChargingTariffId)
               };

        #endregion

        #region (static) TryParse(OperatorId, TariffGroupId, Suffix, out ChargingTariffId)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="TariffGroupId">The unique identification of a charging tariff group.</param>
        /// <param name="Suffix">The suffix of the charging tariff identification.</param>
        /// <param name="ChargingTariffId">The parsed charging tariff identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  OperatorId,
                                       ChargingTariffGroup_Id      TariffGroupId,
                                       String                      Suffix,
                                       out ChargingTariff_Id       ChargingTariffId)

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => TryParse(String.Concat(OperatorId,  "T", TariffGroupId, "_", Suffix), out ChargingTariffId),
                   OperatorIdFormats.ISO_STAR  => TryParse(String.Concat(OperatorId, "*T", TariffGroupId, "_", Suffix), out ChargingTariffId),
                   _                           => TryParse(String.Concat(OperatorId, "*T", TariffGroupId, "_", Suffix), out ChargingTariffId)
               };

        #endregion


        #region Clone

        /// <summary>
        /// Clone this charging tariff identification.
        /// </summary>
        public ChargingTariff_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix?.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingTariff_Id ChargingTariffId1,
                                           ChargingTariff_Id ChargingTariffId2)

            => ChargingTariffId1.Equals(ChargingTariffId2);

        #endregion

        #region Operator != (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingTariff_Id ChargingTariffId1,
                                           ChargingTariff_Id ChargingTariffId2)

            => !ChargingTariffId1.Equals(ChargingTariffId2);

        #endregion

        #region Operator <  (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingTariff_Id ChargingTariffId1,
                                          ChargingTariff_Id ChargingTariffId2)

            => ChargingTariffId1.CompareTo(ChargingTariffId2) < 0;

        #endregion

        #region Operator <= (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingTariff_Id ChargingTariffId1,
                                           ChargingTariff_Id ChargingTariffId2)

            => ChargingTariffId1.CompareTo(ChargingTariffId2) <= 0;

        #endregion

        #region Operator >  (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingTariff_Id ChargingTariffId1,
                                          ChargingTariff_Id ChargingTariffId2)

            => ChargingTariffId1.CompareTo(ChargingTariffId2) > 0;

        #endregion

        #region Operator >= (ChargingTariffId1, ChargingTariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingTariffId1">A charging tariff identification.</param>
        /// <param name="ChargingTariffId2">Another charging tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingTariff_Id ChargingTariffId1,
                                           ChargingTariff_Id ChargingTariffId2)

            => ChargingTariffId1.CompareTo(ChargingTariffId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingTariffId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging tariff identifications.
        /// </summary>
        /// <param name="Object">A charging tariff identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingTariff_Id chargingTariffId
                   ? CompareTo(chargingTariffId)
                   : throw new ArgumentException("The given object is not a charging tariff identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingTariffId)

        /// <summary>
        /// Compares two charging tariff identifications.
        /// </summary>
        /// <param name="ChargingTariffId">A charging tariff identification to compare with.</param>
        public Int32 CompareTo(ChargingTariff_Id ChargingTariffId)
        {

            var c = OperatorId.CompareTo(ChargingTariffId.OperatorId);

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargingTariffId.Suffix,
                                   StringComparison.OrdinalIgnoreCase);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingTariffId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging tariff identifications for equality.
        /// </summary>
        /// <param name="Object">A charging tariff identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingTariff_Id chargingTariffId &&
                   Equals(chargingTariffId);

        #endregion

        #region Equals(ChargingTariffId)

        /// <summary>
        /// Compares two charging tariff identifications for equality.
        /// </summary>
        /// <param name="ChargingTariffId">A charging tariff identification to compare with.</param>
        public Boolean Equals(ChargingTariff_Id ChargingTariffId)

            => OperatorId.Equals(ChargingTariffId.OperatorId) &&

               String.Equals(Suffix.                 Replace("*", ""),
                             ChargingTariffId.Suffix.Replace("*", ""),
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => OperatorId.               GetHashCode() ^
               Suffix?.Replace("*", "")?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => OperatorId.Format switch {
                   OperatorIdFormats.ISO       => String.Concat(OperatorId,  "T", Suffix),
                   OperatorIdFormats.ISO_STAR  => String.Concat(OperatorId, "*T", Suffix),
                   _                           => String.Concat(OperatorId, "*T", Suffix)
               };

        #endregion

    }

}
