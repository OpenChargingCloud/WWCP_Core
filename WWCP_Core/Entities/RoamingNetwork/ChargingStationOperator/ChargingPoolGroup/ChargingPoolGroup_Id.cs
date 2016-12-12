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
    /// The unique identification of of a group of electric vehicle charging pools.
    /// </summary>
    public struct ChargingPoolGroup_Id : IId,
                                         IEquatable<ChargingPoolGroup_Id>,
                                         IComparable<ChargingPoolGroup_Id>

    {

        #region Data

        //ToDo: Replace with better randomness!
        private static readonly Random _Random                       = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// The regular expression for parsing a charging pool group identification.
        /// </summary>
        public  static readonly Regex  ChargingPoolGroupId_RegEx  = new Regex(@"^([A-Z]{2}\*?[A-Z0-9]{3})\*?GP([A-Z0-9][A-Z0-9\*]{0,30})$",
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
        /// Generate a new electric vehicle charging pool group identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        private ChargingPoolGroup_Id(ChargingStationOperator_Id  OperatorId,
                                     String                      Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charging pool group identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Random(OperatorId, Mapper = null)

        /// <summary>
        /// Generate a new unique identification of a charging pool group.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Mapper">A delegate to modify the newly generated charging pool group identification.</param>
        public static ChargingPoolGroup_Id Random(ChargingStationOperator_Id  OperatorId,
                                                  Func<String, String>        Mapper  = null)


            => new ChargingPoolGroup_Id(OperatorId,
                                        Mapper != null ? Mapper(_Random.RandomString(30)) : _Random.RandomString(30));

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charging pool group identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging pool group identification.</param>
        public static ChargingPoolGroup_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging pool group identification must not be null or empty!");

            #endregion

            var MatchCollection = ChargingPoolGroupId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal text representation of a charging pool group identification: '" + Text + "'!",
                                            nameof(Text));

            ChargingStationOperator_Id _OperatorId;

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out _OperatorId))
                return new ChargingPoolGroup_Id(_OperatorId,
                                                MatchCollection[0].Groups[2].Value);

            throw new ArgumentException("Illegal charging pool group identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charging pool group identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charging pool group identification.</param>
        public static ChargingPoolGroup_Id Parse(ChargingStationOperator_Id  OperatorId,
                                                 String                      Suffix)

            => Parse(OperatorId.ToFormat(OperatorIdFormats.ISO_STAR) + "*GS" + Suffix);

        #endregion

        #region TryParse(Text, out ChargingPoolGroup_Id)

        /// <summary>
        /// Parse the given string as a charging pool group identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingPoolGroup_Id ChargingPoolGroupId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ChargingPoolGroupId = default(ChargingPoolGroup_Id);
                return false;
            }

            #endregion

            try
            {

                ChargingPoolGroupId = default(ChargingPoolGroup_Id);

                var _MatchCollection = ChargingPoolGroupId_RegEx.Matches(Text);

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id _OperatorId;

                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                {

                    ChargingPoolGroupId = new ChargingPoolGroup_Id(_OperatorId,
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

            ChargingPoolGroupId = default(ChargingPoolGroup_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging pool group identification.
        /// </summary>
        public ChargingPoolGroup_Id Clone

            => new ChargingPoolGroup_Id(OperatorId.Clone,
                                        new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ChargingPoolGroupId1, ChargingPoolGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroupId1">A charging pool group identification.</param>
        /// <param name="ChargingPoolGroupId2">Another charging pool group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingPoolGroup_Id ChargingPoolGroupId1, ChargingPoolGroup_Id ChargingPoolGroupId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingPoolGroupId1, ChargingPoolGroupId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingPoolGroupId1 == null) || ((Object) ChargingPoolGroupId2 == null))
                return false;

            return ChargingPoolGroupId1.Equals(ChargingPoolGroupId2);

        }

        #endregion

        #region Operator != (ChargingPoolGroupId1, ChargingPoolGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroupId1">A charging pool group identification.</param>
        /// <param name="ChargingPoolGroupId2">Another charging pool group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingPoolGroup_Id ChargingPoolGroupId1, ChargingPoolGroup_Id ChargingPoolGroupId2)
            => !(ChargingPoolGroupId1 == ChargingPoolGroupId2);

        #endregion

        #region Operator <  (ChargingPoolGroupId1, ChargingPoolGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroupId1">A charging pool group identification.</param>
        /// <param name="ChargingPoolGroupId2">Another charging pool group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingPoolGroup_Id ChargingPoolGroupId1, ChargingPoolGroup_Id ChargingPoolGroupId2)
        {

            if ((Object) ChargingPoolGroupId1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolGroupId1), "The given ChargingPoolGroupId1 must not be null!");

            return ChargingPoolGroupId1.CompareTo(ChargingPoolGroupId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingPoolGroupId1, ChargingPoolGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroupId1">A charging pool group identification.</param>
        /// <param name="ChargingPoolGroupId2">Another charging pool group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingPoolGroup_Id ChargingPoolGroupId1, ChargingPoolGroup_Id ChargingPoolGroupId2)
            => !(ChargingPoolGroupId1 > ChargingPoolGroupId2);

        #endregion

        #region Operator >  (ChargingPoolGroupId1, ChargingPoolGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroupId1">A charging pool group identification.</param>
        /// <param name="ChargingPoolGroupId2">Another charging pool group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingPoolGroup_Id ChargingPoolGroupId1, ChargingPoolGroup_Id ChargingPoolGroupId2)
        {

            if ((Object) ChargingPoolGroupId1 == null)
                throw new ArgumentNullException(nameof(ChargingPoolGroupId1), "The given ChargingPoolGroupId1 must not be null!");

            return ChargingPoolGroupId1.CompareTo(ChargingPoolGroupId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingPoolGroupId1, ChargingPoolGroupId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingPoolGroupId1">A charging pool group identification.</param>
        /// <param name="ChargingPoolGroupId2">Another charging pool group identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingPoolGroup_Id ChargingPoolGroupId1, ChargingPoolGroup_Id ChargingPoolGroupId2)
            => !(ChargingPoolGroupId1 < ChargingPoolGroupId2);

        #endregion

        #endregion

        #region IComparable<ChargingPoolGroup_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is ChargingPoolGroup_Id))
                throw new ArgumentException("The given object is not a charging pool group identification!", nameof(Object));

            return CompareTo((ChargingPoolGroup_Id) Object);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingPoolGroup_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException(nameof(ChargingStationId), "The given charging pool group identification must not be null!");

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

        #region IEquatable<ChargingPoolGroup_Id> Members

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

            if (!(Object is ChargingPoolGroup_Id))
                return false;

            return Equals((ChargingPoolGroup_Id) Object);

        }

        #endregion

        #region Equals(ChargingStationId)

        /// <summary>
        /// Compares two charging pool group identifications for equality.
        /// </summary>
        /// <param name="ChargingStationId">A charging pool group identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingPoolGroup_Id ChargingStationId)
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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*GS", Suffix);

        #endregion

    }

}
