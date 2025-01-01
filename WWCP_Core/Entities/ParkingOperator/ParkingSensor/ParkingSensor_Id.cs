﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The unique identification of a parking sensor.
    /// </summary>
    public class ParkingSensor_Id : IId,
                                    IEquatable<ParkingSensor_Id>,
                                    IComparable<ParkingSensor_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        protected readonly String InternalId;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new parking sensor based on the given string.
        /// </summary>
        private ParkingSensor_Id(String String)
        {
            InternalId = String.Trim();
        }

        #endregion


        #region New

        /// <summary>
        /// Generate a new unique identification of an Electric Vehicle parking space (EVPS Id).
        /// </summary>
        public static ParkingSensor_Id New
        {
            get
            {
                return new ParkingSensor_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a parking sensor identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking sensor identification.</param>
        public static ParkingSensor_Id Parse(String Text)
        {
            return new ParkingSensor_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as a parking sensor identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking sensor identification.</param>
        /// <param name="ParkingSpaceId">The parsed parking sensor identification.</param>
        public static Boolean TryParse(String Text, out ParkingSensor_Id ParkingSpaceId)
        {
            try
            {
                ParkingSpaceId = new ParkingSensor_Id(Text);
                return true;
            }
            catch
            {
                ParkingSpaceId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this parking sensor identification.
        /// </summary>
        public ParkingSensor_Id Clone
        {
            get
            {
                return new ParkingSensor_Id(InternalId);
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ParkingSensor_Id EVPS_Id1, ParkingSensor_Id EVPS_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVPS_Id1, EVPS_Id2))
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
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ParkingSensor_Id EVPS_Id1, ParkingSensor_Id EVPS_Id2)
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (ParkingSensor_Id EVPS_Id1, ParkingSensor_Id EVPS_Id2)
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (ParkingSensor_Id EVPS_Id1, ParkingSensor_Id EVPS_Id2)
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (ParkingSensor_Id EVPS_Id1, ParkingSensor_Id EVPS_Id2)
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
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (ParkingSensor_Id EVPS_Id1, ParkingSensor_Id EVPS_Id2)
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
            var EVPS_Id = Object as ParkingSensor_Id;
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
        public Int32 CompareTo(ParkingSensor_Id EVPS_Id)
        {

            if ((Object) EVPS_Id == null)
                throw new ArgumentNullException("The given EVPS_Id must not be null!");

            // Compare the length of the EVP_Ids
            var _Result = this.Length.CompareTo(EVPS_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = InternalId.CompareTo(EVPS_Id.InternalId);

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
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an EVPS_Id.
            var EVPS_Id = Object as ParkingSensor_Id;
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
        public Boolean Equals(ParkingSensor_Id EVPS_Id)
        {

            if ((Object) EVPS_Id == null)
                return false;

            return InternalId.Equals(EVPS_Id.InternalId);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return InternalId.GetHashCode();
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return InternalId.ToString();
        }

        #endregion

    }

}
