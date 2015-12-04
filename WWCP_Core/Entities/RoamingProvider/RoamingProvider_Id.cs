/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
    /// The unique identification of an Electric Vehicle Roaming Provider (EVRP Id).
    /// </summary>
    public class RoamingProvider_Id : IId,
                                      IEquatable<RoamingProvider_Id>,
                                      IComparable<RoamingProvider_Id>

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
        /// Generate a new unique identification of an Electric Vehicle Roaming Provider (EVCP Id).
        /// </summary>
        public static RoamingProvider_Id New
        {
            get
            {
                return new RoamingProvider_Id(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Roaming Provider identification (EVRP Id)
        /// based on the given string.
        /// </summary>
        private RoamingProvider_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Roaming Provider (EVRP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Roaming Provider identification.</param>
        public static RoamingProvider_Id Parse(String Text)
        {
            return new RoamingProvider_Id(Text);
        }

        #endregion

        #region TryParse(Text, out RoamingProviderId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Roaming Provider (EVRP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Roaming Provider identification.</param>
        /// <param name="RoamingProviderId">The parsed Electric Vehicle Roaming Provider identification.</param>
        public static Boolean TryParse(String Text, out RoamingProvider_Id RoamingProviderId)
        {
            try
            {
                RoamingProviderId = new RoamingProvider_Id(Text);
                return true;
            }
            catch (Exception)
            {
                RoamingProviderId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Roaming Provider identification.
        /// </summary>
        public RoamingProvider_Id Clone
        {
            get
            {
                return new RoamingProvider_Id(_Id);
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
        public static Boolean operator == (RoamingProvider_Id EVSP_Id1, RoamingProvider_Id EVSP_Id2)
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
        public static Boolean operator != (RoamingProvider_Id EVSP_Id1, RoamingProvider_Id EVSP_Id2)
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
        public static Boolean operator < (RoamingProvider_Id EVSP_Id1, RoamingProvider_Id EVSP_Id2)
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
        public static Boolean operator <= (RoamingProvider_Id EVSP_Id1, RoamingProvider_Id EVSP_Id2)
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
        public static Boolean operator > (RoamingProvider_Id EVSP_Id1, RoamingProvider_Id EVSP_Id2)
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
        public static Boolean operator >= (RoamingProvider_Id EVSP_Id1, RoamingProvider_Id EVSP_Id2)
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
            var EVSP_Id = Object as RoamingProvider_Id;
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
        public Int32 CompareTo(RoamingProvider_Id EVSP_Id)
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
            var EVSP_Id = Object as RoamingProvider_Id;
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
        public Boolean Equals(RoamingProvider_Id EVSP_Id)
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

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _Id.ToString();
        }

        #endregion

    }

}
