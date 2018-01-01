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
    /// The unique identification of an Charging Station Operator (CSO).
    /// </summary>
    public class ParkingOperator_Id : IId,
                                              IEquatable<ParkingOperator_Id>,
                                              IComparable<ParkingOperator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an EVSE Operator identification.
        /// </summary>
        public  const    String   OperatorId_RegEx            = @"^[A-Z0-9]{3}$";

        /// <summary>
        /// The regular expression for parsing an Alpha-2-CountryCode and an EVSE Operator identification.
        /// </summary>
        public  const    String   CountryAndOperatorId_RegEx  = @"^([A-Z]{2})\*?([A-Z0-9]{3})$ | ^\+?([0-9]{1,5})\*([0-9]{3})$ | ^([0-9]{3})$";

        #endregion

        #region Properties

        /// <summary>
        /// The internal Alpha-2-CountryCode.
        /// </summary>
        public Country       CountryCode  { get; }

        /// <summary>
        /// The internal EVSE Operator identification.
        /// </summary>
        public String        OperatorId   { get; }


        public OperatorIdFormats  Format       { get; }


        public String OriginId
            => ToFormat(Format);

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (CountryCode.Alpha2Code.Length + OperatorId.Length);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Charging Station Operator identification.
        /// </summary>
        /// <param name="CountryCode">The Alpha-2-CountryCode.</param>
        /// <param name="OperatorId">The EVSE Operator identification.</param>
        /// <param name="IdFormat">The format of the Charging Station Operator identification [old|new].</param>
        private ParkingOperator_Id(Country       CountryCode,
                                           String        OperatorId,
                                           OperatorIdFormats  IdFormat = OperatorIdFormats.ISO)
        {

            this.CountryCode  = CountryCode;
            this.OperatorId   = OperatorId;
            this.Format       = IdFormat;

        }

        #endregion


        #region Parse(CountryAndOperatorId)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryAndOperatorId">The country code and Charging Station Operator identification as a string.</param>
        public static ParkingOperator_Id Parse(String CountryAndOperatorId)
        {

            #region Initial checks

            if (CountryAndOperatorId.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "CountryAndOperatorId");

            #endregion

            var _MatchCollection = Regex.Matches(CountryAndOperatorId.Trim().ToUpper(),
                                                 CountryAndOperatorId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE Operator identification '" + CountryAndOperatorId + "'!", "CountryAndOperatorId");

            Country __CountryCode;

            if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
                return new ParkingOperator_Id(__CountryCode,
                                           _MatchCollection[0].Groups[2].Value,
                                           OperatorIdFormats.ISO);

            if (Country.TryParseTelefonCode(_MatchCollection[0].Groups[3].Value, out __CountryCode))
                return new ParkingOperator_Id(__CountryCode,
                                           _MatchCollection[0].Groups[4].Value,
                                           OperatorIdFormats.DIN);

            throw new ArgumentException("Illegal Charging Station Operator identification!", "EVSEId");

        }

        #endregion

        #region Parse(CountryCode, OperatorId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="OperatorId">An Charging Station Operator identification as a string.</param>
        /// <param name="IdFormat">The format of the Charging Station Operator identification [old|new].</param>
        public static ParkingOperator_Id Parse(Country       CountryCode,
                                            String        OperatorId,
                                            OperatorIdFormats  IdFormat = OperatorIdFormats.ISO)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentException("The parameter must not be null or empty!", "CountryCode");

            if (OperatorId.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "OperatorId");

            #endregion

            var _MatchCollection = Regex.Matches(OperatorId.Trim().ToUpper(),
                                                 OperatorId_RegEx,
                                                 RegexOptions.IgnorePatternWhitespace);

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE Operator identification '" + CountryCode + " / " + OperatorId + "'!", "OperatorId");

            return new ParkingOperator_Id(CountryCode, _MatchCollection[0].Value, IdFormat);

        }

        #endregion

        #region TryParse(CountryAndOperatorId, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryAndOperatorId">The country code and Charging Station Operator identification as a string.</param>
        /// <param name="EVSEOperatorId">The parsed EVSE Operator identification.</param>
        public static Boolean TryParse(String               CountryAndOperatorId,
                                       out ParkingOperator_Id  EVSEOperatorId)
        {

            #region Initial checks

            if (CountryAndOperatorId.IsNullOrEmpty())
            {
                EVSEOperatorId = null;
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(CountryAndOperatorId.Trim().ToUpper(),
                                                     CountryAndOperatorId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEOperatorId = null;
                    return false;
                }

                Country __CountryCode;

                if (Country.TryParseAlpha2Code(_MatchCollection[0].Groups[1].Value, out __CountryCode))
                {
                    EVSEOperatorId = new ParkingOperator_Id(__CountryCode,
                                                         _MatchCollection[0].Groups[2].Value,
                                                         OperatorIdFormats.ISO);
                    return true;
                }

                if (Country.TryParseTelefonCode(_MatchCollection[0].Groups[3].Value, out __CountryCode))
                {
                    EVSEOperatorId = new ParkingOperator_Id(__CountryCode,
                                                         _MatchCollection[0].Groups[4].Value,
                                                         OperatorIdFormats.DIN);
                    return true;
                }


                // Just e.g. "822"...
                EVSEOperatorId = ParkingOperator_Id.Parse(Country.Germany,
                                                       _MatchCollection[0].Groups[5].Value).
                                                 ChangeFormat(OperatorIdFormats.DIN);

                return true;

            }

            catch (Exception e)
            { }

            EVSEOperatorId = null;
            return false;

        }

        #endregion

        #region TryParse(CountryCode, OperatorId, out EVSEOperatorId, IdFormat = IdFormatType.NEW)

        /// <summary>
        /// Parse the given string as an EVSE Operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="OperatorId">An Charging Station Operator identification as a string.</param>
        /// <param name="EVSEOperatorId">The parsed EVSE Operator identification.</param>
        /// <param name="IdFormat">The format of the Charging Station Operator identification [old|new].</param>
        public static Boolean TryParse(Country              CountryCode,
                                       String               OperatorId,
                                       out ParkingOperator_Id  EVSEOperatorId,
                                       OperatorIdFormats         IdFormat = OperatorIdFormats.ISO)
        {

            #region Initial checks

            if (CountryCode == null || OperatorId.IsNullOrEmpty())
            {
                EVSEOperatorId = null;
                return false;
            }

            #endregion

            try
            {

                var _MatchCollection = Regex.Matches(OperatorId.Trim().ToUpper(),
                                                     OperatorId_RegEx,
                                                     RegexOptions.IgnorePatternWhitespace);

                if (_MatchCollection.Count != 1)
                {
                    EVSEOperatorId = null;
                    return false;
                }

                EVSEOperatorId = new ParkingOperator_Id(CountryCode, _MatchCollection[0].Value, IdFormat);
                return true;

            }

            catch (Exception e)
            {
                EVSEOperatorId = null;
                return false;
            }

        }

        #endregion

        #region ChangeFormat

        /// <summary>
        /// Return a new Charging Station Operator identification in the given format.
        /// </summary>
        /// <param name="Format">An Charging Station Operator identification format.</param>
        public ParkingOperator_Id ChangeFormat(OperatorIdFormats Format)
        {
            return new ParkingOperator_Id(this.CountryCode, this.OperatorId, Format);
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSEOperator_Id.
        /// </summary>
        public ParkingOperator_Id Clone
        {
            get
            {

                return new ParkingOperator_Id(CountryCode,
                                           new String(OperatorId.ToCharArray()),
                                           Format);

            }
        }

        #endregion


        #region ToFormat(IdFormat)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="IdFormat">The format.</param>
        public String ToFormat(OperatorIdFormats IdFormat)

            => IdFormat == OperatorIdFormats.ISO
                   ? String.Concat(     CountryCode.Alpha2Code,  "*", OperatorId)
                   : String.Concat("+", CountryCode.TelefonCode, "*", OperatorId);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ParkingOperator_Id EVSEOperator_Id1, ParkingOperator_Id EVSEOperator_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEOperator_Id1, EVSEOperator_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEOperator_Id1 == null) || ((Object) EVSEOperator_Id2 == null))
                return false;

            return EVSEOperator_Id1.Equals(EVSEOperator_Id2);

        }

        #endregion

        #region Operator != (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ParkingOperator_Id EVSEOperator_Id1, ParkingOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 == EVSEOperator_Id2);
        }

        #endregion

        #region Operator <  (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ParkingOperator_Id EVSEOperator_Id1, ParkingOperator_Id EVSEOperator_Id2)
        {

            if ((Object) EVSEOperator_Id1 == null)
                throw new ArgumentNullException("The given EVSEOperator_Id1 must not be null!");

            return EVSEOperator_Id1.CompareTo(EVSEOperator_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ParkingOperator_Id EVSEOperator_Id1, ParkingOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 > EVSEOperator_Id2);
        }

        #endregion

        #region Operator >  (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ParkingOperator_Id EVSEOperator_Id1, ParkingOperator_Id EVSEOperator_Id2)
        {

            if ((Object) EVSEOperator_Id1 == null)
                throw new ArgumentNullException("The given EVSEOperator_Id1 must not be null!");

            return EVSEOperator_Id1.CompareTo(EVSEOperator_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSEOperator_Id1, EVSEOperator_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id1">A EVSEOperator_Id.</param>
        /// <param name="EVSEOperator_Id2">Another EVSEOperator_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ParkingOperator_Id EVSEOperator_Id1, ParkingOperator_Id EVSEOperator_Id2)
        {
            return !(EVSEOperator_Id1 < EVSEOperator_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVSEOperator_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSEOperator_Id.
            var EVSEOperator_Id = Object as ParkingOperator_Id;
            if ((Object) EVSEOperator_Id == null)
                throw new ArgumentException("The given object is not a EVSEOperator_Id!");

            return CompareTo(EVSEOperator_Id);

        }

        #endregion

        #region CompareTo(EVSEOperator_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEOperator_Id">An object to compare with.</param>
        public Int32 CompareTo(ParkingOperator_Id EVSEOperator_Id)
        {

            if ((Object) EVSEOperator_Id == null)
                throw new ArgumentNullException("The given EVSEOperator_Id must not be null!");

            // Compare the length of the EVSEOperator_Ids
            var _Result = this.Length.CompareTo(EVSEOperator_Id.Length);

            // If equal: Compare CountryIds
            if (_Result == 0)
                _Result = CountryCode.CompareTo(EVSEOperator_Id.CountryCode);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = OperatorId.CompareTo(EVSEOperator_Id.OperatorId);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEOperator_Id> Members

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

            // Check if the given object is an EVSEOperator_Id.
            var EVSEOperator_Id = Object as ParkingOperator_Id;
            if ((Object) EVSEOperator_Id == null)
                return false;

            return this.Equals(EVSEOperator_Id);

        }

        #endregion

        #region Equals(EVSEOperator_Id)

        /// <summary>
        /// Compares two EVSEOperator_Ids for equality.
        /// </summary>
        /// <param name="EVSEOperatorId">A EVSEOperator_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingOperator_Id EVSEOperatorId)
        {

            if ((Object) EVSEOperatorId == null)
                return false;

            return CountryCode.Equals(EVSEOperatorId.CountryCode) &&
                   OperatorId. Equals(EVSEOperatorId.OperatorId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => CountryCode.Alpha2Code.GetHashCode() ^ OperatorId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => OriginId;

        #endregion

    }

}
