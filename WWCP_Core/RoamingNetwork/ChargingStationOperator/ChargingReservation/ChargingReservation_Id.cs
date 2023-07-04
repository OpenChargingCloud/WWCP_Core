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
    /// The unique identification of a charging reservation.
    /// </summary>
    public readonly struct ChargingReservation_Id : IId,
                                                    IEquatable<ChargingReservation_Id>,
                                                    IComparable<ChargingReservation_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charging reservation identification.
        /// </summary>
        public static readonly Regex  ReservationId_RegEx  = new (@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?R([A-Za-z0-9][A-Za-z0-9\*\-]{0,50})$", // The GUID in OICP is atleast 36 characters long!
                                                                  RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The internal identification.
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

            => OperatorId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging reservation identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging reservation identification.</param>
        private ChargingReservation_Id(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),  "The charging reservation identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region (static) Random(OperatorId, Length = 20)

        public static ChargingReservation_Id Random(ChargingStationOperator_Id  OperatorId,
                                                    Byte                        Length  = 20)

            => new (OperatorId,
                    RandomExtensions.RandomString(Length));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging reservation identification.</param>
        public static ChargingReservation_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text representation of a charging reservation identification must not be null or empty!");

            #endregion

            var matchCollection = ReservationId_RegEx.Matches(Text);

            if (matchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging session identification: '{Text}'!", nameof(Text));


            if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out var chargingStationOperatorId))
                return new ChargingReservation_Id(chargingStationOperatorId,
                                                  matchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging reservation identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging reservation identification.</param>
        public static ChargingReservation_Id Parse(ChargingStationOperator_Id  OperatorId,
                                                   String                      Suffix)

            => Parse(OperatorId.ToString() + "*R" + Suffix);

        #endregion

        #region (static) TryParse (Text)

        /// <summary>
        /// Try to parse the given string as a charging reservation identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging reservation identification..</param>
        public static ChargingReservation_Id? TryParse(String Text)
        {

            if (TryParse(Text, out var chargingReservationId))
                return chargingReservationId;

            return null;

        }

        #endregion

        #region TryParse(Text, out ChargingReservation_Id)

        /// <summary>
        /// Parse the given string as a charging reservation identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingReservation_Id ReservationId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ReservationId = default(ChargingReservation_Id);
                return false;
            }

            #endregion

            try
            {

                ReservationId = default;

                var matchCollection = ReservationId_RegEx.Matches(Text);

                if (matchCollection.Count != 1)
                    return false;


                // New format...
                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[1].Value, out ChargingStationOperator_Id chargingStationOperatorId))
                {

                    ReservationId = new ChargingReservation_Id(chargingStationOperatorId,
                                                               matchCollection[0].Groups[2].Value);

                    return true;

                }

                if (ChargingStationOperator_Id.TryParse(matchCollection[0].Groups[3].Value, out chargingStationOperatorId))
                {

                    ReservationId = new ChargingReservation_Id(chargingStationOperatorId,
                                                               matchCollection[0].Groups[4].Value);

                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception e)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            ReservationId = default(ChargingReservation_Id);
            return false;

        }

        #endregion

        #region TryParse(OperatorId, Suffix, out ChargingReservation_Id)

        /// <summary>
        /// Parse the given string as a charging reservation identification.
        /// </summary>
        public static Boolean TryParse(ChargingStationOperator_Id  OperatorId,
                                       String                      Suffix,
                                       out ChargingReservation_Id  ReservationId)

            => TryParse(OperatorId.ToString() + "*R"  + Suffix, out ReservationId);

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging reservation identification.
        /// </summary>
        public ChargingReservation_Id Clone

            => new (OperatorId.Clone,
                    new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ReservationId1, ReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId1">A charging reservation identification.</param>
        /// <param name="ReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingReservation_Id ReservationId1, ChargingReservation_Id ReservationId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ReservationId1, ReservationId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ReservationId1 == null) || ((Object) ReservationId2 == null))
                return false;

            return ReservationId1.Equals(ReservationId2);

        }

        #endregion

        #region Operator != (ReservationId1, ReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId1">A charging reservation identification.</param>
        /// <param name="ReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingReservation_Id ReservationId1, ChargingReservation_Id ReservationId2)
            => !(ReservationId1 == ReservationId2);

        #endregion

        #region Operator <  (ReservationId1, ReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId1">A charging reservation identification.</param>
        /// <param name="ReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingReservation_Id ReservationId1, ChargingReservation_Id ReservationId2)
        {

            if ((Object) ReservationId1 == null)
                throw new ArgumentNullException(nameof(ReservationId1), "The given charging ReservationId1 must not be null!");

            return ReservationId1.CompareTo(ReservationId2) < 0;

        }

        #endregion

        #region Operator <= (ReservationId1, ReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId1">A charging reservation identification.</param>
        /// <param name="ReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingReservation_Id ReservationId1, ChargingReservation_Id ReservationId2)
            => !(ReservationId1 > ReservationId2);

        #endregion

        #region Operator >  (ReservationId1, ReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId1">A charging reservation identification.</param>
        /// <param name="ReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingReservation_Id ReservationId1, ChargingReservation_Id ReservationId2)
        {

            if ((Object) ReservationId1 == null)
                throw new ArgumentNullException(nameof(ReservationId1), "The given charging ReservationId1 must not be null!");

            return ReservationId1.CompareTo(ReservationId2) > 0;

        }

        #endregion

        #region Operator >= (ReservationId1, ReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId1">A charging reservation identification.</param>
        /// <param name="ReservationId2">Another charging reservation identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingReservation_Id ReservationId1, ChargingReservation_Id ReservationId2)
            => !(ReservationId1 < ReservationId2);

        #endregion

        #endregion

        #region IComparable<charging reservationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingReservation_Id))
                throw new ArgumentException("The given object is not a charging reservation identification!",
                                            nameof(Object));

            return CompareTo((ChargingReservation_Id) Object);

        }

        #endregion

        #region CompareTo(ReservationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReservationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingReservation_Id ReservationId)
        {

            if ((Object) ReservationId == null)
                throw new ArgumentNullException(nameof(ReservationId),  "The given charging reservation identification must not be null!");

            // If equal: Compare charging operator identifications
            var _Result = OperatorId.CompareTo(ReservationId.OperatorId);

            // If equal: Compare charging reservationId suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ReservationId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<charging reservationId> Members

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

            if (!(Object is ChargingReservation_Id))
                return false;

            return this.Equals((ChargingReservation_Id) Object);

        }

        #endregion

        #region Equals(ReservationId)

        /// <summary>
        /// Compares two charging reservation identifications for equality.
        /// </summary>
        /// <param name="ReservationId">A charging reservation identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingReservation_Id ReservationId)
        {

            if ((Object) ReservationId == null)
                return false;

            return OperatorId.Equals(ReservationId.OperatorId) &&
                   Suffix.    Equals(ReservationId.Suffix);

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
        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorId,
                             "*R",
                             Suffix);

        #endregion

    }

}
