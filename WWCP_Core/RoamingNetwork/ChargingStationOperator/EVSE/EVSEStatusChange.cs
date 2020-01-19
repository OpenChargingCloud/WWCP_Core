///*
// * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace org.GraphDefined.WWCP
//{

//    /// <summary>
//    /// An EVSE status change.
//    /// </summary>
//    public class EVSEStatusChange : IEquatable<EVSEStatusChange>,
//                                    IComparable<EVSEStatusChange>,
//                                    IComparable
//    {

//        #region Properties

//        /// <summary>
//        /// The unique identification of the EVSE.
//        /// </summary>
//        public EVSE_Id                       Id           { get; }

//        /// <summary>
//        /// The old status of the EVSE.
//        /// </summary>
//        public Timestamped<EVSEStatusTypes>  OldStatus    { get; }

//        /// <summary>
//        /// The new status of the EVSE.
//        /// </summary>
//        public Timestamped<EVSEStatusTypes>  NewStatus    { get; }


//        /// <summary>
//        /// The current status of the EVSE.
//        /// </summary>
//        public EVSEStatus CurrentStatus

//            => new EVSEStatus(Id,
//                              NewStatus.Value,
//                              NewStatus.Timestamp);

//        #endregion

//        #region Constructor(s)

//        /// <summary>
//        /// Create a new EVSE status change.
//        /// </summary>
//        /// <param name="Id">The unique identification of the EVSE.</param>
//        /// <param name="OldStatus">The old status of the EVSE.</param>
//        /// <param name="NewStatus">The new status of the EVSE.</param>
//        public EVSEStatusChange(EVSE_Id                       Id,
//                                Timestamped<EVSEStatusTypes>  OldStatus,
//                                Timestamped<EVSEStatusTypes>  NewStatus)

//        {

//            #region Initial checks

//            if (Id == null)
//                throw new ArgumentNullException(nameof(Id), "The given unique identification of an EVSE must not be null!");

//            #endregion

//            this.Id         = Id;
//            this.OldStatus  = OldStatus;
//            this.NewStatus  = NewStatus;

//        }

//        #endregion


//        #region Operator overloading

//        #region Operator == (EVSEStatusChange1, EVSEStatusChange2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange1">A EVSEStatusChange.</param>
//        /// <param name="EVSEStatusChange2">Another EVSEStatusChange.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator == (EVSEStatusChange EVSEStatusChange1, EVSEStatusChange EVSEStatusChange2)
//        {

//            // If both are null, or both are same instance, return true.
//            if (Object.ReferenceEquals(EVSEStatusChange1, EVSEStatusChange2))
//                return true;

//            // If one is null, but not both, return false.
//            if (((Object) EVSEStatusChange1 == null) || ((Object) EVSEStatusChange2 == null))
//                return false;

//            return EVSEStatusChange1.Equals(EVSEStatusChange2);

//        }

//        #endregion

//        #region Operator != (EVSEStatusChange1, EVSEStatusChange2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange1">A EVSEStatusChange.</param>
//        /// <param name="EVSEStatusChange2">Another EVSEStatusChange.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator != (EVSEStatusChange EVSEStatusChange1, EVSEStatusChange EVSEStatusChange2)
//        {
//            return !(EVSEStatusChange1 == EVSEStatusChange2);
//        }

//        #endregion

//        #region Operator <  (EVSEStatusChange1, EVSEStatusChange2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange1">A EVSEStatusChange.</param>
//        /// <param name="EVSEStatusChange2">Another EVSEStatusChange.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator < (EVSEStatusChange EVSEStatusChange1, EVSEStatusChange EVSEStatusChange2)
//        {

//            if ((Object) EVSEStatusChange1 == null)
//                throw new ArgumentNullException("The given EVSEStatusChange1 must not be null!");

//            return EVSEStatusChange1.CompareTo(EVSEStatusChange2) < 0;

//        }

//        #endregion

//        #region Operator <= (EVSEStatusChange1, EVSEStatusChange2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange1">A EVSEStatusChange.</param>
//        /// <param name="EVSEStatusChange2">Another EVSEStatusChange.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator <= (EVSEStatusChange EVSEStatusChange1, EVSEStatusChange EVSEStatusChange2)
//        {
//            return !(EVSEStatusChange1 > EVSEStatusChange2);
//        }

//        #endregion

//        #region Operator >  (EVSEStatusChange1, EVSEStatusChange2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange1">A EVSEStatusChange.</param>
//        /// <param name="EVSEStatusChange2">Another EVSEStatusChange.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator > (EVSEStatusChange EVSEStatusChange1, EVSEStatusChange EVSEStatusChange2)
//        {

//            if ((Object) EVSEStatusChange1 == null)
//                throw new ArgumentNullException("The given EVSEStatusChange1 must not be null!");

//            return EVSEStatusChange1.CompareTo(EVSEStatusChange2) > 0;

//        }

//        #endregion

//        #region Operator >= (EVSEStatusChange1, EVSEStatusChange2)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange1">A EVSEStatusChange.</param>
//        /// <param name="EVSEStatusChange2">Another EVSEStatusChange.</param>
//        /// <returns>true|false</returns>
//        public static Boolean operator >= (EVSEStatusChange EVSEStatusChange1, EVSEStatusChange EVSEStatusChange2)
//        {
//            return !(EVSEStatusChange1 < EVSEStatusChange2);
//        }

//        #endregion

//        #endregion

//        #region IComparable<EVSEStatusChange> Members

//        #region CompareTo(Object)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="Object">An object to compare with.</param>
//        public Int32 CompareTo(Object Object)
//        {

//            if (Object == null)
//                throw new ArgumentNullException("The given object must not be null!");

//            // Check if the given object is an EVSEStatusChange.
//            var EVSEStatusChange = Object as EVSEStatusChange;
//            if ((Object) EVSEStatusChange == null)
//                throw new ArgumentException("The given object is not a EVSEStatusChange!");

//            return CompareTo(EVSEStatusChange);

//        }

//        #endregion

//        #region CompareTo(EVSEStatusChange)

//        /// <summary>
//        /// Compares two instances of this object.
//        /// </summary>
//        /// <param name="EVSEStatusChange">An object to compare with.</param>
//        public Int32 CompareTo(EVSEStatusChange EVSEStatusChange)
//        {

//            if ((Object) EVSEStatusChange == null)
//                throw new ArgumentNullException("The given EVSEStatusChange must not be null!");

//            // Compare EVSE Ids
//            var _Result = Id.CompareTo(EVSEStatusChange.Id);

//            // If equal: Compare EVSE status
//            if (_Result == 0)
//                _Result = OldStatus.CompareTo(EVSEStatusChange.OldStatus);

//            if (_Result == 0)
//                _Result = NewStatus.CompareTo(EVSEStatusChange.NewStatus);

//            return _Result;

//        }

//        #endregion

//        #endregion

//        #region IEquatable<EVSEStatusChange> Members

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

//            // Check if the given object is an EVSEStatusChange.
//            var EVSEStatusChange = Object as EVSEStatusChange;
//            if ((Object) EVSEStatusChange == null)
//                return false;

//            return this.Equals(EVSEStatusChange);

//        }

//        #endregion

//        #region Equals(EVSEStatusChange)

//        /// <summary>
//        /// Compares two EVSE identifications for equality.
//        /// </summary>
//        /// <param name="EVSEStatusChange">An EVSE identification to compare with.</param>
//        /// <returns>True if both match; False otherwise.</returns>
//        public Boolean Equals(EVSEStatusChange EVSEStatusChange)
//        {

//            if ((Object) EVSEStatusChange == null)
//                return false;

//            return Id.       Equals(EVSEStatusChange.Id)        &&
//                   OldStatus.Equals(EVSEStatusChange.OldStatus) &&
//                   NewStatus.Equals(EVSEStatusChange.NewStatus);

//        }

//        #endregion

//        #endregion

//        #region (override) GetHashCode()

//        /// <summary>
//        /// Return the HashCode of this object.
//        /// </summary>
//        /// <returns>The HashCode of this object.</returns>
//        public override Int32 GetHashCode()
//        {
//            unchecked
//            {
//                return Id.GetHashCode() * 17 ^ OldStatus.GetHashCode() * 23 ^ NewStatus.GetHashCode();
//            }
//        }

//        #endregion

//        #region (override) ToString()

//        /// <summary>
//        /// Return a text representation of this object.
//        /// ISO-IEC-15118 – Annex H "Specification of Identifiers"
//        /// </summary>
//        public override String ToString()
//        {

//            return String.Concat(Id, ": ", OldStatus.ToString(), " -> ", NewStatus.ToString());

//        }

//        #endregion

//    }

//}
