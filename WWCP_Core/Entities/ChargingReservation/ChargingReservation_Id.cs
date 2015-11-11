/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Charging Reservation.
    /// </summary>
    public class ChargingReservation_Id : IId,
                                          IEquatable<ChargingReservation_Id>,
                                          IComparable<ChargingReservation_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String _Id;

        #endregion

        #region Properties

        #region New

        /// <summary>
        /// Returns a new Electric Vehicle Charging Session identification.
        /// </summary>
        public static ChargingReservation_Id New
        {
            get
            {
                return ChargingReservation_Id.Parse(Guid.NewGuid().ToString());
            }
        }

        #endregion

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
        /// Generate a new Electric Vehicle Charging Reservation identification.
        /// based on the given string.
        /// </summary>
        private ChargingReservation_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Session identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Session identification.</param>
        public static ChargingReservation_Id Parse(String Text)
        {
            return new ChargingReservation_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingReservationId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Session identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Session identification.</param>
        /// <param name="ChargingReservationId">The parsed Electric Vehicle Charging Session identification.</param>
        public static Boolean TryParse(String Text, out ChargingReservation_Id ChargingReservationId)
        {
            try
            {
                ChargingReservationId = new ChargingReservation_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingReservationId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Session identification.
        /// </summary>
        public ChargingReservation_Id Clone
        {
            get
            {
                return new ChargingReservation_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A ChargingReservationId.</param>
        /// <param name="ChargingReservationId2">Another ChargingReservationId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingReservation_Id ChargingReservationId1, ChargingReservation_Id ChargingReservationId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingReservationId1, ChargingReservationId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingReservationId1 == null) || ((Object) ChargingReservationId2 == null))
                return false;

            return ChargingReservationId1.Equals(ChargingReservationId2);

        }

        #endregion

        #region Operator != (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A ChargingReservationId.</param>
        /// <param name="ChargingReservationId2">Another ChargingReservationId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingReservation_Id ChargingReservationId1, ChargingReservation_Id ChargingReservationId2)
        {
            return !(ChargingReservationId1 == ChargingReservationId2);
        }

        #endregion

        #region Operator <  (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A ChargingReservationId.</param>
        /// <param name="ChargingReservationId2">Another ChargingReservationId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingReservation_Id ChargingReservationId1, ChargingReservation_Id ChargingReservationId2)
        {

            if ((Object) ChargingReservationId1 == null)
                throw new ArgumentNullException("The given ChargingReservationId1 must not be null!");

            return ChargingReservationId1.CompareTo(ChargingReservationId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A ChargingReservationId.</param>
        /// <param name="ChargingReservationId2">Another ChargingReservationId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingReservation_Id ChargingReservationId1, ChargingReservation_Id ChargingReservationId2)
        {
            return !(ChargingReservationId1 > ChargingReservationId2);
        }

        #endregion

        #region Operator >  (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A ChargingReservationId.</param>
        /// <param name="ChargingReservationId2">Another ChargingReservationId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingReservation_Id ChargingReservationId1, ChargingReservation_Id ChargingReservationId2)
        {

            if ((Object) ChargingReservationId1 == null)
                throw new ArgumentNullException("The given ChargingReservationId1 must not be null!");

            return ChargingReservationId1.CompareTo(ChargingReservationId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingReservationId1, ChargingReservationId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId1">A ChargingReservationId.</param>
        /// <param name="ChargingReservationId2">Another ChargingReservationId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingReservation_Id ChargingReservationId1, ChargingReservation_Id ChargingReservationId2)
        {
            return !(ChargingReservationId1 < ChargingReservationId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingReservationId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingReservationId.
            var ChargingReservationId = Object as ChargingReservation_Id;
            if ((Object) ChargingReservationId == null)
                throw new ArgumentException("The given object is not a ChargingReservationId!");

            return CompareTo(ChargingReservationId);

        }

        #endregion

        #region CompareTo(ChargingReservationId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingReservationId">An object to compare with.</param>
        public Int32 CompareTo(ChargingReservation_Id ChargingReservationId)
        {

            if ((Object) ChargingReservationId == null)
                throw new ArgumentNullException("The given ChargingReservationId must not be null!");

            // Compare the length of the ChargingReservationIds
            var _Result = this.Length.CompareTo(ChargingReservationId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingReservationId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingReservationId> Members

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

            // Check if the given object is an ChargingReservationId.
            var ChargingReservationId = Object as ChargingReservation_Id;
            if ((Object) ChargingReservationId == null)
                return false;

            return this.Equals(ChargingReservationId);

        }

        #endregion

        #region Equals(ChargingReservationId)

        /// <summary>
        /// Compares two ChargingReservationIds for equality.
        /// </summary>
        /// <param name="ChargingReservationId">A ChargingReservationId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingReservation_Id ChargingReservationId)
        {

            if ((Object) ChargingReservationId == null)
                return false;

            return _Id.Equals(ChargingReservationId._Id);

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
