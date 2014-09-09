/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
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
    /// The unique identification of a playground.
    /// </summary>
    public class RoamingNetwork_Id : IId,
                                     IEquatable<RoamingNetwork_Id>,
                                     IComparable<RoamingNetwork_Id>

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

        #region RoamingNetwork_Id()

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment Operator (EVSE Op) identification.
        /// </summary>
        public RoamingNetwork_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region RoamingNetwork_Id(String)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment Operator (EVSE Op) identification.
        /// based on the given string.
        /// </summary>
        public RoamingNetwork_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new RoamingNetwork_Id.
        /// </summary>
        public static RoamingNetwork_Id New
        {
            get
            {
                return new RoamingNetwork_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Parse(RoamingNetworkId)

        /// <summary>
        /// Parse the given string as an Roaming Network identification.
        /// </summary>
        public static RoamingNetwork_Id Parse(String RoamingNetworkId)
        {
            return new RoamingNetwork_Id(RoamingNetworkId);
        }

        #endregion

        #region TryParse(Text, out RoamingNetworkId)

        /// <summary>
        /// Parse the given string as an Roaming Network identification.
        /// </summary>
        public static Boolean TryParse(String Text, out RoamingNetwork_Id RoamingNetworkId)
        {
            try
            {
                RoamingNetworkId = new RoamingNetwork_Id(Text);
                return true;
            }
            catch (Exception e)
            {
                RoamingNetworkId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an RoamingNetwork_Id.
        /// </summary>
        public RoamingNetwork_Id Clone
        {
            get
            {
                return new RoamingNetwork_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (RoamingNetwork_Id1, RoamingNetwork_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id1">A RoamingNetwork_Id.</param>
        /// <param name="RoamingNetwork_Id2">Another RoamingNetwork_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (RoamingNetwork_Id RoamingNetwork_Id1, RoamingNetwork_Id RoamingNetwork_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RoamingNetwork_Id1, RoamingNetwork_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingNetwork_Id1 == null) || ((Object) RoamingNetwork_Id2 == null))
                return false;

            return RoamingNetwork_Id1.Equals(RoamingNetwork_Id2);

        }

        #endregion

        #region Operator != (RoamingNetwork_Id1, RoamingNetwork_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id1">A RoamingNetwork_Id.</param>
        /// <param name="RoamingNetwork_Id2">Another RoamingNetwork_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (RoamingNetwork_Id RoamingNetwork_Id1, RoamingNetwork_Id RoamingNetwork_Id2)
        {
            return !(RoamingNetwork_Id1 == RoamingNetwork_Id2);
        }

        #endregion

        #region Operator <  (RoamingNetwork_Id1, RoamingNetwork_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id1">A RoamingNetwork_Id.</param>
        /// <param name="RoamingNetwork_Id2">Another RoamingNetwork_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (RoamingNetwork_Id RoamingNetwork_Id1, RoamingNetwork_Id RoamingNetwork_Id2)
        {

            if ((Object) RoamingNetwork_Id1 == null)
                throw new ArgumentNullException("The given RoamingNetwork_Id1 must not be null!");

            return RoamingNetwork_Id1.CompareTo(RoamingNetwork_Id2) < 0;

        }

        #endregion

        #region Operator <= (RoamingNetwork_Id1, RoamingNetwork_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id1">A RoamingNetwork_Id.</param>
        /// <param name="RoamingNetwork_Id2">Another RoamingNetwork_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (RoamingNetwork_Id RoamingNetwork_Id1, RoamingNetwork_Id RoamingNetwork_Id2)
        {
            return !(RoamingNetwork_Id1 > RoamingNetwork_Id2);
        }

        #endregion

        #region Operator >  (RoamingNetwork_Id1, RoamingNetwork_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id1">A RoamingNetwork_Id.</param>
        /// <param name="RoamingNetwork_Id2">Another RoamingNetwork_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (RoamingNetwork_Id RoamingNetwork_Id1, RoamingNetwork_Id RoamingNetwork_Id2)
        {

            if ((Object) RoamingNetwork_Id1 == null)
                throw new ArgumentNullException("The given RoamingNetwork_Id1 must not be null!");

            return RoamingNetwork_Id1.CompareTo(RoamingNetwork_Id2) > 0;

        }

        #endregion

        #region Operator >= (RoamingNetwork_Id1, RoamingNetwork_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id1">A RoamingNetwork_Id.</param>
        /// <param name="RoamingNetwork_Id2">Another RoamingNetwork_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (RoamingNetwork_Id RoamingNetwork_Id1, RoamingNetwork_Id RoamingNetwork_Id2)
        {
            return !(RoamingNetwork_Id1 < RoamingNetwork_Id2);
        }

        #endregion

        #endregion

        #region IComparable<RoamingNetwork_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an RoamingNetwork_Id.
            var RoamingNetwork_Id = Object as RoamingNetwork_Id;
            if ((Object) RoamingNetwork_Id == null)
                throw new ArgumentException("The given object is not a RoamingNetwork_Id!");

            return CompareTo(RoamingNetwork_Id);

        }

        #endregion

        #region CompareTo(RoamingNetwork_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="RoamingNetwork_Id">An object to compare with.</param>
        public Int32 CompareTo(RoamingNetwork_Id RoamingNetwork_Id)
        {

            if ((Object) RoamingNetwork_Id == null)
                throw new ArgumentNullException("The given RoamingNetwork_Id must not be null!");

            // Compare the length of the RoamingNetwork_Ids
            var _Result = this.Length.CompareTo(RoamingNetwork_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(RoamingNetwork_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<RoamingNetwork_Id> Members

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

            // Check if the given object is an RoamingNetwork_Id.
            var RoamingNetwork_Id = Object as RoamingNetwork_Id;
            if ((Object) RoamingNetwork_Id == null)
                return false;

            return this.Equals(RoamingNetwork_Id);

        }

        #endregion

        #region Equals(RoamingNetwork_Id)

        /// <summary>
        /// Compares two RoamingNetwork_Ids for equality.
        /// </summary>
        /// <param name="RoamingNetwork_Id">A RoamingNetwork_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingNetwork_Id RoamingNetwork_Id)
        {

            if ((Object) RoamingNetwork_Id == null)
                return false;

            return _Id.Equals(RoamingNetwork_Id._Id);

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
