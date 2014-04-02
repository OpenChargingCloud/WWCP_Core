///*
// * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
// * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;

//#endregion

//namespace org.emi3group
//{

//    /// <summary>
//    /// The unique identification of a RFID card (eMT_Id).
//    /// (7 digits)
//    /// </summary>
//    public class eMT_Id : IEquatable<eMT_Id>, IComparable<eMT_Id>, IComparable
//    {

//        #region Data

//        /// <summary>
//        /// The internal identification.
//        /// </summary>
//        protected readonly String _Id;

//        #endregion

//        #region Properties

//        #region Length

//        /// <summary>
//        /// Returns the length of the identificator.
//        /// </summary>
//        public UInt64 Length
//        {
//            get
//            {
//                return (UInt64) _Id.Length;
//            }
//        }

//        #endregion

//        #endregion

//        #region Constructor(s)

//        #region eMT_Id()

//        /// <summary>
//        /// Generate a new RFID card identification (eMT_Id).
//        /// </summary>
//        public eMT_Id()
//        {
//            _Id = Guid.NewGuid().ToString();
//        }

//        #endregion

//        #region eMT_Id(String)

//        /// <summary>
//        /// Generate a new RFID card identification (eMT_Id)
//        /// based on the given string.
//        /// </summary>
//        public eMT_Id(String String)
//        {
//            _Id = String.Trim();
//        }

//        #endregion

//        #endregion


//        #region New

//        /// <summary>
//        /// Generate a new eMT_Id.
//        /// </summary>
//        public static eMT_Id New
//        {
//            get
//            {
//                return new eMT_Id(Guid.NewGuid().ToString());
//            }
//        }

//        #endregion

//        #region Clone

//        /// <summary>
//        /// Clone an eMT_Id.
//        /// </summary>
//        public eMT_Id Clone
//        {
//            get
//            {
//                return new eMT_Id(_Id);
//            }
//        }

//        #endregion


//        #region Operator overloading

//        #region Operator == (eMT_Id1, eMT_Id2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id1">A eMT_Id.</param>
//        /// <param name="eMT_Id2">Another eMT_Id.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator == (eMT_Id eMT_Id1, eMT_Id eMT_Id2)
//        {

//            // If both are null, or both are same instance, return true.
//            if (Object.ReferenceEquals(eMT_Id1, eMT_Id2))
//                return true;

//            // If one is null, but not both, return false.
//            if (((Object) eMT_Id1 == null) || ((Object) eMT_Id2 == null))
//                return false;

//            return eMT_Id1.Equals(eMT_Id2);

//        }

//        #endregion

//        #region Operator != (eMT_Id1, eMT_Id2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id1">A eMT_Id.</param>
//        /// <param name="eMT_Id2">Another eMT_Id.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator != (eMT_Id eMT_Id1, eMT_Id eMT_Id2)
//        {
//            return !(eMT_Id1 == eMT_Id2);
//        }

//        #endregion

//        #region Operator <  (eMT_Id1, eMT_Id2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id1">A eMT_Id.</param>
//        /// <param name="eMT_Id2">Another eMT_Id.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator < (eMT_Id eMT_Id1, eMT_Id eMT_Id2)
//        {

//            if ((Object) eMT_Id1 == null)
//                throw new ArgumentNullException("The given eMT_Id1 must not be null!");

//            return eMT_Id1.CompareTo(eMT_Id2) < 0;

//        }

//        #endregion

//        #region Operator <= (eMT_Id1, eMT_Id2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id1">A eMT_Id.</param>
//        /// <param name="eMT_Id2">Another eMT_Id.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator <= (eMT_Id eMT_Id1, eMT_Id eMT_Id2)
//        {
//            return !(eMT_Id1 > eMT_Id2);
//        }

//        #endregion

//        #region Operator >  (eMT_Id1, eMT_Id2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id1">A eMT_Id.</param>
//        /// <param name="eMT_Id2">Another eMT_Id.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator > (eMT_Id eMT_Id1, eMT_Id eMT_Id2)
//        {

//            if ((Object) eMT_Id1 == null)
//                throw new ArgumentNullException("The given eMT_Id1 must not be null!");

//            return eMT_Id1.CompareTo(eMT_Id2) > 0;

//        }

//        #endregion

//        #region Operator >= (eMT_Id1, eMT_Id2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id1">A eMT_Id.</param>
//        /// <param name="eMT_Id2">Another eMT_Id.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator >= (eMT_Id eMT_Id1, eMT_Id eMT_Id2)
//        {
//            return !(eMT_Id1 < eMT_Id2);
//        }

//        #endregion

//        #endregion

//        #region IComparable<eMT_Id> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException("The given object must not be null!");

//            // Check if the given object is an eMT_Id.
//            var eMT_Id = Object as eMT_Id;
//            if ((Object) eMT_Id == null)
//                throw new ArgumentException("The given object is not a eMT_Id!");

//            return CompareTo(eMT_Id);

//        }

//        #endregion

//        #region CompareTo(eMT_Id)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="eMT_Id">An object to compare with.</param>
//        public Int32 CompareTo(eMT_Id eMT_Id)
//        {

//            if ((Object) eMT_Id == null)
//                throw new ArgumentNullException("The given eMT_Id must not be null!");

//            // Compare the length of the eMT_Ids
//            var _Result = this.Length.CompareTo(eMT_Id.Length);

//            // If equal: Compare Ids
//            if (_Result == 0)
//                _Result = _Id.CompareTo(eMT_Id._Id);

//            return _Result;

//        }

//        #endregion

//        #endregion

//        #region IEquatable<eMT_Id> Members

//        #region Equals(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        /// <returns>true|false</returns>
//        public override Boolean Equals(Object Object)
//        {

//            if (Object == null)
//                return false;

//            // Check if the given object is an eMT_Id.
//            var eMT_Id = Object as eMT_Id;
//            if ((Object) eMT_Id == null)
//                return false;

//            return this.Equals(eMT_Id);

//        }

//        #endregion

//        #region Equals(eMT_Id)

//        /// <summary>
//        /// Compares two eMT_Ids for equality.
//        /// </summary>
//        /// <param name="eMT_Id">A eMT_Id to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(eMT_Id eMT_Id)
//        {

//            if ((Object) eMT_Id == null)
//                return false;

//            return _Id.Equals(eMT_Id._Id);

//        }

//        #endregion

//        #endregion

//        #region GetHashCode()

//        /// <summary>
//        /// Return the HashCode of this object.
//        /// </summary>
//        /// <returns>The HashCode of this object.</returns>
//        public override Int32 GetHashCode()
//        {
//            return _Id.GetHashCode();
//        }

//        #endregion

//        #region ToString()

//        /// <summary>
//        /// Return a string represtentation of this object.
//        /// </summary>
//        public override String ToString()
//        {
//            return _Id.ToString();
//        }

//        #endregion

//    }

//}
