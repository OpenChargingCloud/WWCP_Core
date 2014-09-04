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
    /// The unique identification of an Electric Vehicle Supply Equipment Connector (EVSEConnector_Id)
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

        #region EVSEConnector_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment Connector identification (EVSEConnector_Id).
        /// </summary>
        public SocketOutlet_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region EVSEConnector_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment Connector identification (EVSEConnector_Id)
        /// based on the given string.
        /// </summary>
        public SocketOutlet_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new EVSEConnector_Id.
        /// </summary>
        public static SocketOutlet_Id New
        {
            get
            {
                return new SocketOutlet_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSEConnector_Id.
        /// </summary>
        public SocketOutlet_Id Clone
        {
            get
            {
                return new SocketOutlet_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSEConnector_Id1, EVSEConnector_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id1">A EVSEConnector_Id.</param>
        /// <param name="EVSEConnector_Id2">Another EVSEConnector_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (SocketOutlet_Id EVSEConnector_Id1, SocketOutlet_Id EVSEConnector_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEConnector_Id1, EVSEConnector_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEConnector_Id1 == null) || ((Object) EVSEConnector_Id2 == null))
                return false;

            return EVSEConnector_Id1.Equals(EVSEConnector_Id2);

        }

        #endregion

        #region Operator != (EVSEConnector_Id1, EVSEConnector_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id1">A EVSEConnector_Id.</param>
        /// <param name="EVSEConnector_Id2">Another EVSEConnector_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (SocketOutlet_Id EVSEConnector_Id1, SocketOutlet_Id EVSEConnector_Id2)
        {
            return !(EVSEConnector_Id1 == EVSEConnector_Id2);
        }

        #endregion

        #region Operator <  (EVSEConnector_Id1, EVSEConnector_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id1">A EVSEConnector_Id.</param>
        /// <param name="EVSEConnector_Id2">Another EVSEConnector_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (SocketOutlet_Id EVSEConnector_Id1, SocketOutlet_Id EVSEConnector_Id2)
        {

            if ((Object) EVSEConnector_Id1 == null)
                throw new ArgumentNullException("The given EVSEConnector_Id1 must not be null!");

            return EVSEConnector_Id1.CompareTo(EVSEConnector_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSEConnector_Id1, EVSEConnector_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id1">A EVSEConnector_Id.</param>
        /// <param name="EVSEConnector_Id2">Another EVSEConnector_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (SocketOutlet_Id EVSEConnector_Id1, SocketOutlet_Id EVSEConnector_Id2)
        {
            return !(EVSEConnector_Id1 > EVSEConnector_Id2);
        }

        #endregion

        #region Operator >  (EVSEConnector_Id1, EVSEConnector_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id1">A EVSEConnector_Id.</param>
        /// <param name="EVSEConnector_Id2">Another EVSEConnector_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (SocketOutlet_Id EVSEConnector_Id1, SocketOutlet_Id EVSEConnector_Id2)
        {

            if ((Object) EVSEConnector_Id1 == null)
                throw new ArgumentNullException("The given EVSEConnector_Id1 must not be null!");

            return EVSEConnector_Id1.CompareTo(EVSEConnector_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSEConnector_Id1, EVSEConnector_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id1">A EVSEConnector_Id.</param>
        /// <param name="EVSEConnector_Id2">Another EVSEConnector_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (SocketOutlet_Id EVSEConnector_Id1, SocketOutlet_Id EVSEConnector_Id2)
        {
            return !(EVSEConnector_Id1 < EVSEConnector_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVSEConnector_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSEConnector_Id.
            var EVSEConnector_Id = Object as SocketOutlet_Id;
            if ((Object) EVSEConnector_Id == null)
                throw new ArgumentException("The given object is not a EVSEConnector_Id!");

            return CompareTo(EVSEConnector_Id);

        }

        #endregion

        #region CompareTo(EVSEConnector_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEConnector_Id">An object to compare with.</param>
        public Int32 CompareTo(SocketOutlet_Id EVSEConnector_Id)
        {

            if ((Object) EVSEConnector_Id == null)
                throw new ArgumentNullException("The given EVSEConnector_Id must not be null!");

            // Compare the length of the EVSEConnector_Ids
            var _Result = this.Length.CompareTo(EVSEConnector_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVSEConnector_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEConnector_Id> Members

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

            // Check if the given object is an EVSEConnector_Id.
            var EVSEConnector_Id = Object as SocketOutlet_Id;
            if ((Object) EVSEConnector_Id == null)
                return false;

            return this.Equals(EVSEConnector_Id);

        }

        #endregion

        #region Equals(EVSEConnector_Id)

        /// <summary>
        /// Compares two EVSEConnector_Ids for equality.
        /// </summary>
        /// <param name="EVSEConnector_Id">A EVSEConnector_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SocketOutlet_Id EVSEConnector_Id)
        {

            if ((Object) EVSEConnector_Id == null)
                return false;

            return _Id.Equals(EVSEConnector_Id._Id);

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
