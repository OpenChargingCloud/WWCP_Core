/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;
using System.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An EVSE admin status update.
    /// </summary>
    public struct EVSEAdminStatusUpdate : IEquatable <EVSEAdminStatusUpdate>,
                                          IComparable<EVSEAdminStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                           Id          { get; }

        /// <summary>
        /// The old timestamped status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusType>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the EVSE.
        /// </summary>
        public Timestamped<EVSEAdminStatusType>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        #region EVSEAdminStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new EVSE admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="OldStatus">The old timestamped admin status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped admin status of the EVSE.</param>
        public EVSEAdminStatusUpdate(EVSE_Id                           Id,
                                     Timestamped<EVSEAdminStatusType>  OldStatus,
                                     Timestamped<EVSEAdminStatusType>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion

        #region EVSEAdminStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new EVSE admin status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="OldStatus">The old timestamped admin status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped admin status of the EVSE.</param>
        public EVSEAdminStatusUpdate(EVSE_Id          Id,
                                     EVSEAdminStatus  OldStatus,
                                     EVSEAdminStatus  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus.Combined;
            this.NewStatus  = NewStatus.Combined;

        }

        #endregion

        #endregion


        #region (static) Snapshot(EVSE)

        /// <summary>
        /// Take a snapshot of the current EVSE status.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public static EVSEAdminStatusUpdate Snapshot(EVSE EVSE)

            => new EVSEAdminStatusUpdate(EVSE.Id,
                                         EVSE.AdminStatus,
                                         EVSE.AdminStatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEAdminStatusUpdate1 == null) || ((Object) EVSEAdminStatusUpdate2 == null))
                return false;

            return EVSEAdminStatusUpdate1.Equals(EVSEAdminStatusUpdate2);

        }

        #endregion

        #region Operator != (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)
            => !(EVSEAdminStatusUpdate1 == EVSEAdminStatusUpdate2);

        #endregion

        #region Operator <  (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)
        {

            if ((Object) EVSEAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatusUpdate1), "The given EVSEAdminStatusUpdate1 must not be null!");

            return EVSEAdminStatusUpdate1.CompareTo(EVSEAdminStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)
            => !(EVSEAdminStatusUpdate1 > EVSEAdminStatusUpdate2);

        #endregion

        #region Operator >  (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)
        {

            if ((Object) EVSEAdminStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatusUpdate1), "The given EVSEAdminStatusUpdate1 must not be null!");

            return EVSEAdminStatusUpdate1.CompareTo(EVSEAdminStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEAdminStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEAdminStatusUpdate EVSEAdminStatusUpdate1, EVSEAdminStatusUpdate EVSEAdminStatusUpdate2)
            => !(EVSEAdminStatusUpdate1 < EVSEAdminStatusUpdate2);

        #endregion

        #endregion

        #region IComparable<EVSEAdminStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEAdminStatusUpdate))
                throw new ArgumentException("The given object is not a EVSEStatus!",
                                            nameof(Object));

            return CompareTo((EVSEAdminStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(EVSEAdminStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(EVSEAdminStatusUpdate EVSEAdminStatusUpdate)
        {

            if ((Object) EVSEAdminStatusUpdate == null)
                throw new ArgumentNullException(nameof(EVSEAdminStatusUpdate), "The given EVSE status update must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(EVSEAdminStatusUpdate.Id);

            // If equal: Compare the new EVSE status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(EVSEAdminStatusUpdate.NewStatus);

            // If equal: Compare the old EVSE status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(EVSEAdminStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEAdminStatusUpdate> Members

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

            if (!(Object is EVSEAdminStatusUpdate))
                return false;

            return this.Equals((EVSEAdminStatusUpdate) Object);

        }

        #endregion

        #region Equals(EVSEAdminStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="EVSEAdminStatusUpdate">An EVSE status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEAdminStatusUpdate EVSEAdminStatusUpdate)
        {

            if ((Object) EVSEAdminStatusUpdate == null)
                return false;

            return Id.       Equals(EVSEAdminStatusUpdate.Id)        &&
                   OldStatus.Equals(EVSEAdminStatusUpdate.OldStatus) &&
                   NewStatus.Equals(EVSEAdminStatusUpdate.NewStatus);

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
            unchecked
            {

                return Id.       GetHashCode() * 7 ^
                       OldStatus.GetHashCode() * 5 ^
                       NewStatus.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, ": ",
                             OldStatus,
                             " -> ",
                             NewStatus);

        #endregion

    }

}
