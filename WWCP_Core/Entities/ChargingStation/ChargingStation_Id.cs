/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Charging Station (EVCS Id).
    /// </summary>
    public class ChargingStation_Id : IId,
                                      IEquatable<ChargingStation_Id>,
                                      IComparable<ChargingStation_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String _Id;

        #endregion

        #region Properties

        #region Length

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return (UInt64) _Id.Length;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Charging Station identification (EVCS Id)
        /// based on the given string.
        /// </summary>
        private ChargingStation_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Create(this EVSEIds)

        /// <summary>
        /// Create a ChargingStationId based on the given EVSEIds.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSEIds.</param>
        public static ChargingStation_Id Create(IEnumerable<EVSE_Id> EVSEIds)
        {

            var EVSEIdPrefixStrings = EVSEIds.
                                          Select(EVSEId => EVSEId.ToString().Split('*')).
                                          Select(EVSEIdElements => EVSEIdElements.
                                                                       Take(EVSEIdElements.Length - 1).
                                                                       AggregateWith("*")).
                                          Distinct().
                                          ToArray();

            if (EVSEIdPrefixStrings.Length == 1)
                return ChargingStation_Id.Parse(EVSEIdPrefixStrings.First());

            throw new ApplicationException("Could not create a common ChargingStationId based on the EVSEId prefixes " + EVSEIdPrefixStrings.Select(v => "'" + v + "'").AggregateWith(", ") + "!");

        }

        #endregion

        #region New(Mapper = null)

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle Charging Station (EVCS Id).
        /// </summary>
        /// <param name="Mapper">A delegate to modify the newly generated charging station identification.</param>
        public static ChargingStation_Id New(Func<String, String> Mapper = null)
        {
            return new ChargingStation_Id(Mapper != null ? Mapper(Guid.NewGuid().ToString()) : Guid.NewGuid().ToString());
        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Station identification.</param>
        public static ChargingStation_Id Parse(String Text)
        {
            return new ChargingStation_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingStationId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Station identification (EVCS Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Station identification.</param>
        /// <param name="ChargingStationId">The parsed Electric Vehicle Charging Station identification.</param>
        public static Boolean TryParse(String Text, out ChargingStation_Id ChargingStationId)
        {
            try
            {
                ChargingStationId = new ChargingStation_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingStationId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Station identification.
        /// </summary>
        public ChargingStation_Id Clone
        {
            get
            {
                return new ChargingStation_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingStation_Id1, ChargingStation_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingStation_Id1 == null) || ((Object) ChargingStation_Id2 == null))
                return false;

            return ChargingStation_Id1.Equals(ChargingStation_Id2);

        }

        #endregion

        #region Operator != (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {
            return !(ChargingStation_Id1 == ChargingStation_Id2);
        }

        #endregion

        #region Operator <  (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {

            if ((Object) ChargingStation_Id1 == null)
                throw new ArgumentNullException("The given ChargingStation_Id1 must not be null!");

            return ChargingStation_Id1.CompareTo(ChargingStation_Id2) < 0;

        }

        #endregion

        #region Operator <= (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {
            return !(ChargingStation_Id1 > ChargingStation_Id2);
        }

        #endregion

        #region Operator >  (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {

            if ((Object) ChargingStation_Id1 == null)
                throw new ArgumentNullException("The given ChargingStation_Id1 must not be null!");

            return ChargingStation_Id1.CompareTo(ChargingStation_Id2) > 0;

        }

        #endregion

        #region Operator >= (ChargingStation_Id1, ChargingStation_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStation_Id1">A ChargingStation_Id.</param>
        /// <param name="ChargingStation_Id2">Another ChargingStation_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingStation_Id ChargingStation_Id1, ChargingStation_Id ChargingStation_Id2)
        {
            return !(ChargingStation_Id1 < ChargingStation_Id2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingStation_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingStation_Id.
            var ChargingStationId = Object as ChargingStation_Id;
            if ((Object) ChargingStationId == null)
                throw new ArgumentException("The given object is not a ChargingStationId!");

            return CompareTo(ChargingStationId);

        }

        #endregion

        #region CompareTo(ChargingStationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingStationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingStation_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                throw new ArgumentNullException("The given ChargingStationId must not be null!");

            // Compare the length of the ChargingStationIds
            var _Result = this.Length.CompareTo(ChargingStationId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingStationId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingStation_Id> Members

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

            // Check if the given object is an ChargingStationId.
            var ChargingStationId = Object as ChargingStation_Id;
            if ((Object) ChargingStationId == null)
                return false;

            return this.Equals(ChargingStationId);

        }

        #endregion

        #region Equals(ChargingStation_Id)

        /// <summary>
        /// Compares two ChargingStationIds for equality.
        /// </summary>
        /// <param name="ChargingStationId">A ChargingStationId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingStation_Id ChargingStationId)
        {

            if ((Object) ChargingStationId == null)
                return false;

            return _Id.Equals(ChargingStationId._Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return _Id.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
