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
    /// The unique identification of a Navigation Service Provider (NSP Id).
    /// </summary>
    public class NavigationServiceProvider_Id : IId,
                                                IEquatable<NavigationServiceProvider_Id>,
                                                IComparable<NavigationServiceProvider_Id>

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
        /// Generate a new unique identification of a Navigation Service Provider (NSP Id).
        /// </summary>
        public static NavigationServiceProvider_Id New
        {
            get
            {
                return new NavigationServiceProvider_Id(Guid.NewGuid().ToString());
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
        /// Generate a new Navigation Service Provider identification (NSP Id)
        /// based on the given string.
        /// </summary>
        private NavigationServiceProvider_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a Navigation Service Provider (NSP Id).
        /// </summary>
        /// <param name="Text">A text representation of a Navigation Service Provider identification.</param>
        public static NavigationServiceProvider_Id Parse(String Text)
        {
            return new NavigationServiceProvider_Id(Text);
        }

        #endregion

        #region TryParse(Text, out NSPId)

        /// <summary>
        /// Parse the given string as a Navigation Service Provider (NSP Id).
        /// </summary>
        /// <param name="Text">A text representation of a Navigation Service Provider identification.</param>
        /// <param name="NSPId">The parsed Electric Vehicle Charging Pool identification.</param>
        public static Boolean TryParse(String Text, out NavigationServiceProvider_Id NSPId)
        {
            try
            {
                NSPId = new NavigationServiceProvider_Id(Text);
                return true;
            }
            catch (Exception)
            {
                NSPId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Pool identification.
        /// </summary>
        public NavigationServiceProvider_Id Clone
        {
            get
            {
                return new NavigationServiceProvider_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (NSPId1, NSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId1">A NSPId.</param>
        /// <param name="NSPId2">Another NSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (NavigationServiceProvider_Id NSPId1, NavigationServiceProvider_Id NSPId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(NSPId1, NSPId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) NSPId1 == null) || ((Object) NSPId2 == null))
                return false;

            return NSPId1.Equals(NSPId2);

        }

        #endregion

        #region Operator != (NSPId1, NSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId1">A NSPId.</param>
        /// <param name="NSPId2">Another NSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (NavigationServiceProvider_Id NSPId1, NavigationServiceProvider_Id NSPId2)
        {
            return !(NSPId1 == NSPId2);
        }

        #endregion

        #region Operator <  (NSPId1, NSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId1">A NSPId.</param>
        /// <param name="NSPId2">Another NSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (NavigationServiceProvider_Id NSPId1, NavigationServiceProvider_Id NSPId2)
        {

            if ((Object) NSPId1 == null)
                throw new ArgumentNullException("The given NSPId1 must not be null!");

            return NSPId1.CompareTo(NSPId2) < 0;

        }

        #endregion

        #region Operator <= (NSPId1, NSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId1">A NSPId.</param>
        /// <param name="NSPId2">Another NSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (NavigationServiceProvider_Id NSPId1, NavigationServiceProvider_Id NSPId2)
        {
            return !(NSPId1 > NSPId2);
        }

        #endregion

        #region Operator >  (NSPId1, NSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId1">A NSPId.</param>
        /// <param name="NSPId2">Another NSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (NavigationServiceProvider_Id NSPId1, NavigationServiceProvider_Id NSPId2)
        {

            if ((Object) NSPId1 == null)
                throw new ArgumentNullException("The given NSPId1 must not be null!");

            return NSPId1.CompareTo(NSPId2) > 0;

        }

        #endregion

        #region Operator >= (NSPId1, NSPId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId1">A NSPId.</param>
        /// <param name="NSPId2">Another NSPId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (NavigationServiceProvider_Id NSPId1, NavigationServiceProvider_Id NSPId2)
        {
            return !(NSPId1 < NSPId2);
        }

        #endregion

        #endregion

        #region IComparable<NSPId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an NSPId.
            var NSPId = Object as NavigationServiceProvider_Id;
            if ((Object) NSPId == null)
                throw new ArgumentException("The given object is not a NSPId!");

            return CompareTo(NSPId);

        }

        #endregion

        #region CompareTo(NSPId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="NSPId">An object to compare with.</param>
        public Int32 CompareTo(NavigationServiceProvider_Id NSPId)
        {

            if ((Object) NSPId == null)
                throw new ArgumentNullException("The given NSPId must not be null!");

            // Compare the length of the NSPIds
            var _Result = this.Length.CompareTo(NSPId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(NSPId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<NSPId> Members

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

            // Check if the given object is an NSPId.
            var NSPId = Object as NavigationServiceProvider_Id;
            if ((Object) NSPId == null)
                return false;

            return this.Equals(NSPId);

        }

        #endregion

        #region Equals(NSPId)

        /// <summary>
        /// Compares two NSPIds for equality.
        /// </summary>
        /// <param name="NSPId">A NSPId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(NavigationServiceProvider_Id NSPId)
        {

            if ((Object) NSPId == null)
                return false;

            return _Id.Equals(NSPId._Id);

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
