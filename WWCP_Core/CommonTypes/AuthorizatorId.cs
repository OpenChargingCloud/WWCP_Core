/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an authorizator.
    /// </summary>
    public class Authorizator_Id : IId,
                                   IEquatable<Authorizator_Id>,
                                   IComparable<Authorizator_Id>

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

        #region AuthorizatorId()

        /// <summary>
        /// Generate a new authorizator identification.
        /// </summary>
        public Authorizator_Id()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region AuthorizatorId(String)

        /// <summary>
        /// Generate a new authorizator identification.
        /// based on the given string.
        /// </summary>
        public Authorizator_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new authorizator identification.
        /// </summary>
        public static Authorizator_Id New
        {
            get
            {
                return new Authorizator_Id(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Parse(EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an authorizator identification.
        /// </summary>
        public static Authorizator_Id Parse(String EVSEOperatorId)
        {
            return new Authorizator_Id(EVSEOperatorId);
        }

        #endregion

        #region TryParse(Text, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an authorizator identification.
        /// </summary>
        public static Boolean TryParse(String Text, out Authorizator_Id EVSEOperatorId)
        {
            try
            {
                EVSEOperatorId = new Authorizator_Id(Text);
                return true;
            }
            catch (Exception e)
            {
                EVSEOperatorId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone an authorizator identification.
        /// </summary>
        public Authorizator_Id Clone
        {
            get
            {
                return new Authorizator_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">A AuthorizatorId.</param>
        /// <param name="AuthorizatorId2">Another AuthorizatorId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Authorizator_Id AuthorizatorId1, Authorizator_Id AuthorizatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AuthorizatorId1, AuthorizatorId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AuthorizatorId1 == null) || ((Object) AuthorizatorId2 == null))
                return false;

            return AuthorizatorId1.Equals(AuthorizatorId2);

        }

        #endregion

        #region Operator != (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">A AuthorizatorId.</param>
        /// <param name="AuthorizatorId2">Another AuthorizatorId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Authorizator_Id AuthorizatorId1, Authorizator_Id AuthorizatorId2)
        {
            return !(AuthorizatorId1 == AuthorizatorId2);
        }

        #endregion

        #region Operator <  (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">A AuthorizatorId.</param>
        /// <param name="AuthorizatorId2">Another AuthorizatorId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Authorizator_Id AuthorizatorId1, Authorizator_Id AuthorizatorId2)
        {

            if ((Object) AuthorizatorId1 == null)
                throw new ArgumentNullException("The given AuthorizatorId1 must not be null!");

            return AuthorizatorId1.CompareTo(AuthorizatorId2) < 0;

        }

        #endregion

        #region Operator <= (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">A AuthorizatorId.</param>
        /// <param name="AuthorizatorId2">Another AuthorizatorId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Authorizator_Id AuthorizatorId1, Authorizator_Id AuthorizatorId2)
        {
            return !(AuthorizatorId1 > AuthorizatorId2);
        }

        #endregion

        #region Operator >  (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">A AuthorizatorId.</param>
        /// <param name="AuthorizatorId2">Another AuthorizatorId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Authorizator_Id AuthorizatorId1, Authorizator_Id AuthorizatorId2)
        {

            if ((Object) AuthorizatorId1 == null)
                throw new ArgumentNullException("The given AuthorizatorId1 must not be null!");

            return AuthorizatorId1.CompareTo(AuthorizatorId2) > 0;

        }

        #endregion

        #region Operator >= (AuthorizatorId1, AuthorizatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId1">A AuthorizatorId.</param>
        /// <param name="AuthorizatorId2">Another AuthorizatorId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Authorizator_Id AuthorizatorId1, Authorizator_Id AuthorizatorId2)
        {
            return !(AuthorizatorId1 < AuthorizatorId2);
        }

        #endregion

        #endregion

        #region IComparable<AuthorizatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an AuthorizatorId.
            var AuthorizatorId = Object as Authorizator_Id;
            if ((Object) AuthorizatorId == null)
                throw new ArgumentException("The given object is not a AuthorizatorId!");

            return CompareTo(AuthorizatorId);

        }

        #endregion

        #region CompareTo(AuthorizatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthorizatorId">An object to compare with.</param>
        public Int32 CompareTo(Authorizator_Id AuthorizatorId)
        {

            if ((Object) AuthorizatorId == null)
                throw new ArgumentNullException("The given AuthorizatorId must not be null!");

            // Compare the length of the AuthorizatorIds
            var _Result = this.Length.CompareTo(AuthorizatorId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(AuthorizatorId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<AuthorizatorId> Members

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

            // Check if the given object is an AuthorizatorId.
            var AuthorizatorId = Object as Authorizator_Id;
            if ((Object) AuthorizatorId == null)
                return false;

            return this.Equals(AuthorizatorId);

        }

        #endregion

        #region Equals(AuthorizatorId)

        /// <summary>
        /// Compares two AuthorizatorIds for equality.
        /// </summary>
        /// <param name="AuthorizatorId">A AuthorizatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Authorizator_Id AuthorizatorId)
        {

            if ((Object) AuthorizatorId == null)
                return false;

            return _Id.Equals(AuthorizatorId._Id);

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
