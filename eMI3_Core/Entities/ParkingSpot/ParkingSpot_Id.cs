/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

namespace com.graphdefined.eMI3
{

    /// <summary>
    /// The unique identification of an electric vehicle parking sport (EVPS Id).
    /// </summary>
    public class ParkingSpot_Id : IId,
                                  IEquatable<ParkingSpot_Id>,
                                  IComparable<ParkingSpot_Id>

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

        #region ServicePlan_Id()

        /// <summary>
        /// Generate a new electric vehicle service plan identification (EVSP Id).
        /// </summary>
        public ParkingSpot_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region ServicePlan_Id(String)

        /// <summary>
        /// Generate a new electric vehicle service plan identification (EVSP Id)
        /// based on the given string.
        /// </summary>
        public ParkingSpot_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new EVPS_Id.
        /// </summary>
        public static ParkingSpot_Id New
        {
            get
            {
                return new ParkingSpot_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVPS_Id.
        /// </summary>
        public ParkingSpot_Id Clone
        {
            get
            {
                return new ParkingSpot_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVPS_Id1, EVPS_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id1">A EVPS_Id.</param>
        /// <param name="EVPS_Id2">Another EVPS_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ParkingSpot_Id EVPS_Id1, ParkingSpot_Id EVPS_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVPS_Id1, EVPS_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVPS_Id1 == null) || ((Object) EVPS_Id2 == null))
                return false;

            return EVPS_Id1.Equals(EVPS_Id2);

        }

        #endregion

        #region Operator != (EVPS_Id1, EVPS_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id1">A EVPS_Id.</param>
        /// <param name="EVPS_Id2">Another EVPS_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ParkingSpot_Id EVPS_Id1, ParkingSpot_Id EVPS_Id2)
        {
            return !(EVPS_Id1 == EVPS_Id2);
        }

        #endregion

        #region Operator <  (EVPS_Id1, EVPS_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id1">A EVPS_Id.</param>
        /// <param name="EVPS_Id2">Another EVPS_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ParkingSpot_Id EVPS_Id1, ParkingSpot_Id EVPS_Id2)
        {

            if ((Object) EVPS_Id1 == null)
                throw new ArgumentNullException("The given EVPS_Id1 must not be null!");

            return EVPS_Id1.CompareTo(EVPS_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVPS_Id1, EVPS_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id1">A EVPS_Id.</param>
        /// <param name="EVPS_Id2">Another EVPS_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ParkingSpot_Id EVPS_Id1, ParkingSpot_Id EVPS_Id2)
        {
            return !(EVPS_Id1 > EVPS_Id2);
        }

        #endregion

        #region Operator >  (EVPS_Id1, EVPS_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id1">A EVPS_Id.</param>
        /// <param name="EVPS_Id2">Another EVPS_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ParkingSpot_Id EVPS_Id1, ParkingSpot_Id EVPS_Id2)
        {

            if ((Object) EVPS_Id1 == null)
                throw new ArgumentNullException("The given EVPS_Id1 must not be null!");

            return EVPS_Id1.CompareTo(EVPS_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVPS_Id1, EVPS_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id1">A EVPS_Id.</param>
        /// <param name="EVPS_Id2">Another EVPS_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ParkingSpot_Id EVPS_Id1, ParkingSpot_Id EVPS_Id2)
        {
            return !(EVPS_Id1 < EVPS_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVPS_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVPS_Id.
            var EVPS_Id = Object as ParkingSpot_Id;
            if ((Object) EVPS_Id == null)
                throw new ArgumentException("The given object is not a EVPS_Id!");

            return CompareTo(EVPS_Id);

        }

        #endregion

        #region CompareTo(EVPS_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVPS_Id">An object to compare with.</param>
        public Int32 CompareTo(ParkingSpot_Id EVPS_Id)
        {

            if ((Object) EVPS_Id == null)
                throw new ArgumentNullException("The given EVPS_Id must not be null!");

            // Compare the length of the EVP_Ids
            var _Result = this.Length.CompareTo(EVPS_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVPS_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVPS_Id> Members

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

            // Check if the given object is an EVPS_Id.
            var EVPS_Id = Object as ParkingSpot_Id;
            if ((Object) EVPS_Id == null)
                return false;

            return this.Equals(EVPS_Id);

        }

        #endregion

        #region Equals(EVPS_Id)

        /// <summary>
        /// Compares two electric vehicle charging service plan identifications for equality.
        /// </summary>
        /// <param name="EVPS_Id">An electric vehicle charging service plan identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingSpot_Id EVPS_Id)
        {

            if ((Object) EVPS_Id == null)
                return false;

            return _Id.Equals(EVPS_Id._Id);

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
