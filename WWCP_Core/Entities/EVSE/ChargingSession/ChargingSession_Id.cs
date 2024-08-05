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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extension methods for charging session identifications.
    /// </summary>
    public static class ChargingSessionIdExtensions
    {

        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        /// <param name="ChargingSessionId">A charging session identification.</param>
        public static Boolean IsNullOrEmpty(this ChargingSession_Id? ChargingSessionId)
            => !ChargingSessionId.HasValue || ChargingSessionId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        /// <param name="ChargingSessionId">A charging session identification.</param>
        public static Boolean IsNotNullOrEmpty(this ChargingSession_Id? ChargingSessionId)
            => ChargingSessionId.HasValue && ChargingSessionId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// The unique identification of a charging session.
    /// </summary>
    public readonly struct ChargingSession_Id : IId,
                                                IEquatable<ChargingSession_Id>,
                                                IComparable<ChargingSession_Id>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging session identification.
        /// </summary>
        public static readonly Regex ChargingSessionId_RegEx = new (@"^([A-Z]{2}[\*\-]?[A-Z0-9]{3})\*?N([A-Za-z0-9][A-Za-z0-9\*\-]{0,250})$",
                                                                    RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id?  OperatorId    { get; }

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public EMobilityProvider_Id?        ProviderId    { get; }

        /// <summary>
        /// The suffix of the charging session identification.
        /// </summary>
        public String                       Suffix        { get; }


        /// <summary>
        /// Indicates whether this charging session identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this charging session identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the charging session identificator.
        /// </summary>
        public UInt64 Length
            => (OperatorId?.Length ?? ProviderId?.Length ?? 0) + 2 + ((UInt64) (Suffix?.Length ?? 0));

        #endregion

        #region Constructor(s)

        #region (private) ChargingSession_Id(OperatorId, Suffix)

        /// <summary>
        /// Create a new charging session identification based on the
        /// given charging station operator identification and a suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging session identification.</param>
        private ChargingSession_Id(ChargingStationOperator_Id  OperatorId,
                                   String                      Suffix)

            : this(Suffix)

        {

            this.OperatorId = OperatorId;

        }

        #endregion

        #region (private) ChargingSession_Id(ProviderId, Suffix)

        /// <summary>
        /// Create a new charging session identification based on the
        /// given e-mobility provider identification and a suffix.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the charging session identification.</param>
        private ChargingSession_Id(EMobilityProvider_Id  ProviderId,
                                   String                Suffix)

            : this(Suffix)

        {

            this.ProviderId = ProviderId;

        }

        #endregion

        #region (private) ChargingSession_Id(SessionId)

        /// <summary>
        /// Create a new charging session identification
        /// based on the given e-mobility provider and identification suffix.
        /// </summary>
        /// <param name="SessionId">A charging session identification.</param>
        private ChargingSession_Id(String SessionId)
        {

            this.Suffix = SessionId;

        }

        #endregion

        #endregion


        #region (static) NewRandom(            Length = 30)

        /// <summary>
        /// Create a new random charging session identification.
        /// </summary>
        /// <param name="Length">The expected length of the charging session identification suffix.</param>
        public static ChargingSession_Id NewRandom(Byte Length  = 30)

            => new (RandomExtensions.RandomString(Length));

        #endregion

        #region (static) NewRandom(OperatorId, Length = 20)

        /// <summary>
        /// Create a new random charging session identification using the given charging station operator identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Length">The expected length of the charging session identification suffix.</param>
        public static ChargingSession_Id NewRandom(ChargingStationOperator_Id  OperatorId,
                                                   Byte                        Length  = 20)

            => new (OperatorId,
                    RandomExtensions.RandomString(Length));

        #endregion

        #region (static) NewRandom(ProviderId, Length = 20)

        /// <summary>
        /// Create a new random charging session identification using the given e-mobility provider identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Length">The expected length of the charging session identification suffix.</param>
        public static ChargingSession_Id NewRandom(EMobilityProvider_Id  ProviderId,
                                                   Byte                  Length  = 20)

            => new (ProviderId,
                    RandomExtensions.RandomString(Length));

        #endregion


        #region (static) Parse    (Text)

        /// <summary>
        /// Parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static ChargingSession_Id Parse(String Text)
        {

            if (TryParse(Text, out var chargingSessionId))
                return chargingSessionId;

            throw new ArgumentException($"Invalid text representation of a charging session identification: '{Text}'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) Parse    (OperatorId, Suffix)

        /// <summary>
        /// Parse the given charging station operator identification
        /// and the given suffix as a charging session identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging session identification.</param>
        public static ChargingSession_Id Parse(ChargingStationOperator_Id  OperatorId,
                                               String                      Suffix)
        {

            if (TryParse(OperatorId, Suffix, out var chargingSessionId))
                return chargingSessionId;

            throw new ArgumentException($"Invalid text representation of a charging session identification: '{OperatorId}*N{Suffix}'!",
                                        nameof(Suffix));

        }

        #endregion

        #region (static) Parse    (ProviderId, Suffix)

        /// <summary>
        /// Parse the given e-mobility provider identification
        /// and the given suffix as a charging session identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the charging session identification.</param>
        public static ChargingSession_Id Parse(EMobilityProvider_Id  ProviderId,
                                               String                Suffix)
        {

            if (TryParse(ProviderId, Suffix, out var chargingSessionId))
                return chargingSessionId;

            throw new ArgumentException($"Invalid text representation of a charging session identification: '{ProviderId}*N{Suffix}'!",
                                        nameof(Suffix));

        }

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        public static ChargingSession_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingSessionId))
                return chargingSessionId;

            return null;

        }

        #endregion

        #region (static) TryParse (Text,               out ChargingSessionId)

        /// <summary>
        /// Try to parse the given string as a charging session identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging session identification.</param>
        /// <param name="ChargingSessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(String Text, out ChargingSession_Id ChargingSessionId)
        {

            Text = Text.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {

                    var matchCollection = ChargingSessionId_RegEx.Matches(Text);

                    if (matchCollection.Count == 1)
                    {

                        // OperatorId (with '*' as separator)
                        if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value,
                                                                out var chargingStationOperatorId))
                        {

                            ChargingSessionId = new ChargingSession_Id(
                                                    chargingStationOperatorId,
                                                    matchCollection[0].Groups[2].Value
                                                );

                            return true;

                        }

                        // ProviderId (with '-' as separator)
                        if (EMobilityProvider_Id.TryParse(matchCollection[0].Groups[1].Value,
                                                          out var eMobilityProviderId))
                        {

                            ChargingSessionId = new ChargingSession_Id(
                                                    eMobilityProviderId,
                                                    matchCollection[0].Groups[2].Value
                                                );

                            return true;

                        }

                    }

                    // Use the whole string as charging session identification!
                    ChargingSessionId = new ChargingSession_Id(Text);
                    return true;

                }
                catch
                { }
            }

            ChargingSessionId = default;
            return false;

        }

        #endregion

        #region (static) TryParse (OperatorId, Suffix, out ChargingSessionId)

        /// <summary>
        /// Try to parse the given charging station operator identification
        /// and the given suffix as a charging session identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging session identification.</param>
        /// <param name="ChargingSessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix,
                                       out ChargingSession_Id      ChargingSessionId)

            => TryParse($"{OperatorId}*N{Suffix.Trim()}",
                        out ChargingSessionId);

        #endregion

        #region (static) TryParse (ProviderId, Suffix, out ChargingSessionId)

        /// <summary>
        /// Try to parse the given e-mobility provider identification
        /// and the given suffix as a charging session identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the charging session identification.</param>
        /// <param name="ChargingSessionId">The parsed charging session identification.</param>
        public static Boolean TryParse(EMobilityProvider_Id    ProviderId,
                                       String                  Suffix,
                                       out ChargingSession_Id  ChargingSessionId)

            => TryParse($"{ProviderId}*N{Suffix.Trim()}",
                        out ChargingSessionId);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging session identification.
        /// </summary>
        public ChargingSession_Id Clone
        {
            get
            {

                if (OperatorId.HasValue)
                    return new (OperatorId.Value.Clone,
                                new String(Suffix?.ToCharArray()));

                if (ProviderId.HasValue)
                    return new (ProviderId.Value.Clone,
                                new String(Suffix?.ToCharArray()));

                    return new (new String(Suffix?.ToCharArray()));

            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A charging session identification.</param>
        /// <param name="ChargingSessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ChargingSession_Id ChargingSessionId1,
                                           ChargingSession_Id ChargingSessionId2)

            => ChargingSessionId1.Equals(ChargingSessionId2);

        #endregion

        #region Operator != (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A charging session identification.</param>
        /// <param name="ChargingSessionId2">Another charging session identification.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ChargingSession_Id ChargingSessionId1,
                                           ChargingSession_Id ChargingSessionId2)

            => !ChargingSessionId1.Equals(ChargingSessionId2);

        #endregion

        #region Operator <  (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A charging session identification.</param>
        /// <param name="ChargingSessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ChargingSession_Id ChargingSessionId1,
                                          ChargingSession_Id ChargingSessionId2)

            => ChargingSessionId1.CompareTo(ChargingSessionId2) < 0;

        #endregion

        #region Operator <= (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A charging session identification.</param>
        /// <param name="ChargingSessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ChargingSession_Id ChargingSessionId1,
                                           ChargingSession_Id ChargingSessionId2)

            => ChargingSessionId1.CompareTo(ChargingSessionId2) <= 0;

        #endregion

        #region Operator >  (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A charging session identification.</param>
        /// <param name="ChargingSessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ChargingSession_Id ChargingSessionId1,
                                          ChargingSession_Id ChargingSessionId2)

            => ChargingSessionId1.CompareTo(ChargingSessionId2) > 0;

        #endregion

        #region Operator >= (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A charging session identification.</param>
        /// <param name="ChargingSessionId2">Another charging session identification.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ChargingSession_Id ChargingSessionId1,
                                           ChargingSession_Id ChargingSessionId2)

            => ChargingSessionId1.CompareTo(ChargingSessionId2) >= 0;

        #endregion

        #endregion

        #region IComparable<ChargingSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging session identifications.
        /// </summary>
        /// <param name="Object">A charging session identification to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingSession_Id chargingSessionId
                   ? CompareTo(chargingSessionId)
                   : throw new ArgumentException("The given object is not a charging session identification!");

        #endregion

        #region CompareTo(ChargingSessionId)

        /// <summary>
        /// Compares two charging session identifications.
        /// </summary>
        /// <param name="ChargingSessionId">A charging session identification to compare with.</param>
        public Int32 CompareTo(ChargingSession_Id ChargingSessionId)
        {

            var c = OperatorId.HasValue && ChargingSessionId.OperatorId.HasValue
                        ? OperatorId.Value.CompareTo(ChargingSessionId.OperatorId.Value)
                        : 0;

            if (c == 0)
                c = ProviderId.HasValue && ChargingSessionId.ProviderId.HasValue
                        ? ProviderId.Value.CompareTo(ChargingSessionId.ProviderId.Value)
                        : 0;

            if (c == 0)
                c = String.Compare(Suffix,
                                   ChargingSessionId.Suffix,
                                   StringComparison.Ordinal);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingSessionId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging session identifications for equality.
        /// </summary>
        /// <param name="Object">A charging session identification to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingSession_Id chargingSessionId &&
                   Equals(chargingSessionId);

        #endregion

        #region Equals(ChargingSessionId)

        /// <summary>
        /// Compares two charging session identifications for equality.
        /// </summary>
        /// <param name="ChargingSessionId">A charging session identification to compare with.</param>
        public Boolean Equals(ChargingSession_Id ChargingSessionId)
        {

            if (OperatorId.HasValue && ChargingSessionId.OperatorId.HasValue && OperatorId != ChargingSessionId.OperatorId)
                return false;

            if (ProviderId.HasValue && ChargingSessionId.ProviderId.HasValue && ProviderId != ChargingSessionId.ProviderId)
                return false;

            return String.Equals(Suffix,
                                 ChargingSessionId.Suffix,
                                 StringComparison.OrdinalIgnoreCase);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (OperatorId?.GetHashCode() ?? 0) * 5 ^
                       (ProviderId?.GetHashCode() ?? 0) * 3 ^
                       (Suffix?.    GetHashCode() ?? 0);
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (OperatorId.HasValue)
                return $"{OperatorId}*N{Suffix}";

            if (ProviderId.HasValue)
                return $"{ProviderId}-N{Suffix}";

            return Suffix;

        }

        #endregion

    }

}
