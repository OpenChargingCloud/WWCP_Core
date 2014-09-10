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
    /// The unique identification of an Electric Vehicle Supply Equipment Socket Outlet (EVSESO_Id).
    /// </summary>
    public class SocketOutlet_Id : IId,
                                   IEquatable<SocketOutlet_Id>,
                                   IComparable<SocketOutlet_Id>

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
        /// Generate a new unique identification of an Electric Vehicle Supply Equipment Socket Outlet (EVSESO_Id).
        /// </summary>
        public static SocketOutlet_Id New
        {
            get
            {
                return SocketOutlet_Id.Parse(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Supply Equipment Socket Outlet (EVSESO_Id)
        /// based on the given string.
        /// </summary>
        private SocketOutlet_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Supply Equipment Socket Outlet (EVSESO_Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Supply Equipment Socket Outlet identification.</param>
        public static SocketOutlet_Id Parse(String Text)
        {
            return new SocketOutlet_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingPoolId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Supply Equipment Socket Outlet (EVSESO_Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Supply Equipment Socket Outlet identification.</param>
        /// <param name="ChargingPoolId">The parsed Electric Vehicle Supply Equipment Socket Outlet identification.</param>
        public static Boolean TryParse(String Text, out SocketOutlet_Id ChargingPoolId)
        {
            try
            {
                ChargingPoolId = new SocketOutlet_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingPoolId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Supply Equipment Socket Outlet identification.
        /// </summary>
        public SocketOutlet_Id Clone
        {
            get
            {
                return SocketOutlet_Id.Parse(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A SocketOutletId.</param>
        /// <param name="SocketOutletId2">Another SocketOutletId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SocketOutlet_Id SocketOutletId1, SocketOutlet_Id SocketOutletId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SocketOutletId1, SocketOutletId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SocketOutletId1 == null) || ((Object) SocketOutletId2 == null))
                return false;

            return SocketOutletId1.Equals(SocketOutletId2);

        }

        #endregion

        #region Operator != (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A SocketOutletId.</param>
        /// <param name="SocketOutletId2">Another SocketOutletId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SocketOutlet_Id SocketOutletId1, SocketOutlet_Id SocketOutletId2)
        {
            return !(SocketOutletId1 == SocketOutletId2);
        }

        #endregion

        #region Operator <  (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A SocketOutletId.</param>
        /// <param name="SocketOutletId2">Another SocketOutletId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SocketOutlet_Id SocketOutletId1, SocketOutlet_Id SocketOutletId2)
        {

            if ((Object) SocketOutletId1 == null)
                throw new ArgumentNullException("The given SocketOutletId1 must not be null!");

            return SocketOutletId1.CompareTo(SocketOutletId2) < 0;

        }

        #endregion

        #region Operator <= (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A SocketOutletId.</param>
        /// <param name="SocketOutletId2">Another SocketOutletId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SocketOutlet_Id SocketOutletId1, SocketOutlet_Id SocketOutletId2)
        {
            return !(SocketOutletId1 > SocketOutletId2);
        }

        #endregion

        #region Operator >  (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A SocketOutletId.</param>
        /// <param name="SocketOutletId2">Another SocketOutletId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SocketOutlet_Id SocketOutletId1, SocketOutlet_Id SocketOutletId2)
        {

            if ((Object) SocketOutletId1 == null)
                throw new ArgumentNullException("The given SocketOutletId1 must not be null!");

            return SocketOutletId1.CompareTo(SocketOutletId2) > 0;

        }

        #endregion

        #region Operator >= (SocketOutletId1, SocketOutletId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId1">A SocketOutletId.</param>
        /// <param name="SocketOutletId2">Another SocketOutletId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SocketOutlet_Id SocketOutletId1, SocketOutlet_Id SocketOutletId2)
        {
            return !(SocketOutletId1 < SocketOutletId2);
        }

        #endregion

        #endregion

        #region IComparable<SocketOutlet_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an SocketOutletId.
            var SocketOutletId = Object as SocketOutlet_Id;
            if ((Object) SocketOutletId == null)
                throw new ArgumentException("The given object is not a SocketOutletId!");

            return CompareTo(SocketOutletId);

        }

        #endregion

        #region CompareTo(SocketOutletId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SocketOutletId">An object to compare with.</param>
        public Int32 CompareTo(SocketOutlet_Id SocketOutletId)
        {

            if ((Object) SocketOutletId == null)
                throw new ArgumentNullException("The given SocketOutletId must not be null!");

            // Compare the length of the SocketOutletIds
            var _Result = this.Length.CompareTo(SocketOutletId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(SocketOutletId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<SocketOutlet_Id> Members

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

            // Check if the given object is an SocketOutletId.
            var SocketOutletId = Object as SocketOutlet_Id;
            if ((Object) SocketOutletId == null)
                return false;

            return this.Equals(SocketOutletId);

        }

        #endregion

        #region Equals(SocketOutletId)

        /// <summary>
        /// Compares two SocketOutletIds for equality.
        /// </summary>
        /// <param name="SocketOutletId">A SocketOutletId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SocketOutlet_Id SocketOutletId)
        {

            if ((Object) SocketOutletId == null)
                return false;

            return _Id.Equals(SocketOutletId._Id);

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
