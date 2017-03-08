/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// An EVSE status update.
    /// </summary>
    public struct EVSEStatusUpdate : IEquatable <EVSEStatusUpdate>,
                                     IComparable<EVSEStatusUpdate>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                      Id          { get; }

        /// <summary>
        /// The old timestamped status of the EVSE.
        /// </summary>
        public Timestamped<EVSEStatusType>  OldStatus   { get; }

        /// <summary>
        /// The new timestamped status of the EVSE.
        /// </summary>
        public Timestamped<EVSEStatusType>  NewStatus   { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new EVSE status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE.</param>
        public EVSEStatusUpdate(EVSE_Id                      Id,
                                Timestamped<EVSEStatusType>  OldStatus,
                                Timestamped<EVSEStatusType>  NewStatus)

        {

            this.Id         = Id;
            this.OldStatus  = OldStatus;
            this.NewStatus  = NewStatus;

        }

        #endregion

        #region EVSEStatusUpdate(Id, OldStatus, NewStatus)

        /// <summary>
        /// Create a new EVSE status update.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="OldStatus">The old timestamped status of the EVSE.</param>
        /// <param name="NewStatus">The new timestamped status of the EVSE.</param>
        public EVSEStatusUpdate(EVSE_Id     Id,
                                EVSEStatus  OldStatus,
                                EVSEStatus  NewStatus)

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
        public static EVSEStatusUpdate Snapshot(EVSE EVSE)

            => new EVSEStatusUpdate(EVSE.Id,
                                    EVSE.Status,
                                    EVSE.StatusSchedule().Skip(1).FirstOrDefault());

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatusUpdate EVSEStatusUpdate1, EVSEStatusUpdate EVSEStatusUpdate2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEStatusUpdate1, EVSEStatusUpdate2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatusUpdate1 == null) || ((Object) EVSEStatusUpdate2 == null))
                return false;

            return EVSEStatusUpdate1.Equals(EVSEStatusUpdate2);

        }

        #endregion

        #region Operator != (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatusUpdate EVSEStatusUpdate1, EVSEStatusUpdate EVSEStatusUpdate2)
            => !(EVSEStatusUpdate1 == EVSEStatusUpdate2);

        #endregion

        #region Operator <  (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatusUpdate EVSEStatusUpdate1, EVSEStatusUpdate EVSEStatusUpdate2)
        {

            if ((Object) EVSEStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdate1), "The given EVSEStatusUpdate1 must not be null!");

            return EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) < 0;

        }

        #endregion

        #region Operator <= (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatusUpdate EVSEStatusUpdate1, EVSEStatusUpdate EVSEStatusUpdate2)
            => !(EVSEStatusUpdate1 > EVSEStatusUpdate2);

        #endregion

        #region Operator >  (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatusUpdate EVSEStatusUpdate1, EVSEStatusUpdate EVSEStatusUpdate2)
        {

            if ((Object) EVSEStatusUpdate1 == null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdate1), "The given EVSEStatusUpdate1 must not be null!");

            return EVSEStatusUpdate1.CompareTo(EVSEStatusUpdate2) > 0;

        }

        #endregion

        #region Operator >= (EVSEStatusUpdate1, EVSEStatusUpdate2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate1">A EVSE status update.</param>
        /// <param name="EVSEStatusUpdate2">Another EVSE status update.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatusUpdate EVSEStatusUpdate1, EVSEStatusUpdate EVSEStatusUpdate2)
            => !(EVSEStatusUpdate1 < EVSEStatusUpdate2);

        #endregion

        #endregion

        #region IComparable<EVSEStatusUpdate> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEStatusUpdate))
                throw new ArgumentException("The given object is not a EVSEStatus!",
                                            nameof(Object));

            return CompareTo((EVSEStatusUpdate) Object);

        }

        #endregion

        #region CompareTo(EVSEStatusUpdate)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatusUpdate">An object to compare with.</param>
        public Int32 CompareTo(EVSEStatusUpdate EVSEStatusUpdate)
        {

            if ((Object) EVSEStatusUpdate == null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdate), "The given EVSE status update must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(EVSEStatusUpdate.Id);

            // If equal: Compare the new EVSE status
            if (_Result == 0)
                _Result = NewStatus.CompareTo(EVSEStatusUpdate.NewStatus);

            // If equal: Compare the old EVSE status
            if (_Result == 0)
                _Result = OldStatus.CompareTo(EVSEStatusUpdate.OldStatus);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatusUpdate> Members

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

            if (!(Object is EVSEStatusUpdate))
                return false;

            return Equals((EVSEStatusUpdate) Object);

        }

        #endregion

        #region Equals(EVSEStatusUpdate)

        /// <summary>
        /// Compares two EVSE status updates for equality.
        /// </summary>
        /// <param name="EVSEStatusUpdate">An EVSE status update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEStatusUpdate EVSEStatusUpdate)
        {

            if ((Object) EVSEStatusUpdate == null)
                return false;

            return Id.       Equals(EVSEStatusUpdate.Id)        &&
                   OldStatus.Equals(EVSEStatusUpdate.OldStatus) &&
                   NewStatus.Equals(EVSEStatusUpdate.NewStatus);

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
