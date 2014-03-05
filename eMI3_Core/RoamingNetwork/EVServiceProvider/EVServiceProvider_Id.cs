/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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

namespace org.emi3group
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Service Provider (EVSProvider Id).
    /// </summary>
    public class EVServiceProvider_Id : IId, IEquatable<EVServiceProvider_Id>, IComparable<EVServiceProvider_Id>
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

        #region EVSProvider_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Service Provider identification (EVSP Id).
        /// </summary>
        public EVServiceProvider_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region EVSProvider_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Service Provider identification (EVSP Id)
        /// based on the given string.
        /// </summary>
        public EVServiceProvider_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new EVSP_Id.
        /// </summary>
        public static EVServiceProvider_Id New
        {
            get
            {
                return new EVServiceProvider_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Parse(EVServiceProviderId)

        /// <summary>
        /// Parse the given string as an EV Service Provider identification.
        /// </summary>
        public static EVServiceProvider_Id Parse(String EVServiceProviderId)
        {
            return new EVServiceProvider_Id(EVServiceProviderId);
        }

        #endregion

        #region TryParse(Text, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an EV Service Provider identification.
        /// </summary>
        public static Boolean TryParse(String Text, out EVServiceProvider_Id EVServiceProviderId)
        {
            try
            {
                EVServiceProviderId = new EVServiceProvider_Id(Text);
                return true;
            }
            catch (Exception e)
            {
                EVServiceProviderId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an EVSP_Id.
        /// </summary>
        public EVServiceProvider_Id Clone
        {
            get
            {
                return new EVServiceProvider_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVSP_Id1, EVSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id1">A EVSP_Id.</param>
        /// <param name="EVSP_Id2">Another EVSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVServiceProvider_Id EVSP_Id1, EVServiceProvider_Id EVSP_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSP_Id1, EVSP_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSP_Id1 == null) || ((Object) EVSP_Id2 == null))
                return false;

            return EVSP_Id1.Equals(EVSP_Id2);

        }

        #endregion

        #region Operator != (EVSP_Id1, EVSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id1">A EVSP_Id.</param>
        /// <param name="EVSP_Id2">Another EVSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVServiceProvider_Id EVSP_Id1, EVServiceProvider_Id EVSP_Id2)
        {
            return !(EVSP_Id1 == EVSP_Id2);
        }

        #endregion

        #region Operator <  (EVSP_Id1, EVSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id1">A EVSP_Id.</param>
        /// <param name="EVSP_Id2">Another EVSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVServiceProvider_Id EVSP_Id1, EVServiceProvider_Id EVSP_Id2)
        {

            if ((Object) EVSP_Id1 == null)
                throw new ArgumentNullException("The given EVSP_Id1 must not be null!");

            return EVSP_Id1.CompareTo(EVSP_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVSP_Id1, EVSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id1">A EVSP_Id.</param>
        /// <param name="EVSP_Id2">Another EVSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVServiceProvider_Id EVSP_Id1, EVServiceProvider_Id EVSP_Id2)
        {
            return !(EVSP_Id1 > EVSP_Id2);
        }

        #endregion

        #region Operator >  (EVSP_Id1, EVSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id1">A EVSP_Id.</param>
        /// <param name="EVSP_Id2">Another EVSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVServiceProvider_Id EVSP_Id1, EVServiceProvider_Id EVSP_Id2)
        {

            if ((Object) EVSP_Id1 == null)
                throw new ArgumentNullException("The given EVSP_Id1 must not be null!");

            return EVSP_Id1.CompareTo(EVSP_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVSP_Id1, EVSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id1">A EVSP_Id.</param>
        /// <param name="EVSP_Id2">Another EVSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVServiceProvider_Id EVSP_Id1, EVServiceProvider_Id EVSP_Id2)
        {
            return !(EVSP_Id1 < EVSP_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVSP_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVSP_Id.
            var EVSP_Id = Object as EVServiceProvider_Id;
            if ((Object) EVSP_Id == null)
                throw new ArgumentException("The given object is not a EVSP_Id!");

            return CompareTo(EVSP_Id);

        }

        #endregion

        #region CompareTo(EVSP_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSP_Id">An object to compare with.</param>
        public Int32 CompareTo(EVServiceProvider_Id EVSP_Id)
        {

            if ((Object) EVSP_Id == null)
                throw new ArgumentNullException("The given EVSP_Id must not be null!");

            // Compare the length of the EVSP_Ids
            var _Result = this.Length.CompareTo(EVSP_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVSP_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSP_Id> Members

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

            // Check if the given object is an EVSP_Id.
            var EVSP_Id = Object as EVServiceProvider_Id;
            if ((Object) EVSP_Id == null)
                return false;

            return this.Equals(EVSP_Id);

        }

        #endregion

        #region Equals(EVSP_Id)

        /// <summary>
        /// Compares two EVSP_Ids for equality.
        /// </summary>
        /// <param name="EVSP_Id">A EVSP_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVServiceProvider_Id EVSP_Id)
        {

            if ((Object) EVSP_Id == null)
                return false;

            return _Id.Equals(EVSP_Id._Id);

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
