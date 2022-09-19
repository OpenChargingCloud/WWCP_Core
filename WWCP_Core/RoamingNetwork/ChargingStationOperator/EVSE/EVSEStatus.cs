﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current timestamped status of an EVSE.
    /// </summary>
    public class EVSEStatus : AInternalData,
                              IEquatable <EVSEStatus>,
                              IComparable<EVSEStatus>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                       Id       { get; }

        /// <summary>
        /// The current status of the EVSE.
        /// </summary>
        public Timestamped<EVSEStatusTypes>  Status   { get; }

        #endregion

        #region Constructor(s)

        #region EVSEStatus(Id, Status,            CustomData = null)

        /// <summary>
        /// Create a new EVSE status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The current timestamped status of the EVSE.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EVSEStatus(EVSE_Id                                    Id,
                          Timestamped<EVSEStatusTypes>               Status,
                          IEnumerable<KeyValuePair<String, Object>>  CustomData  = null)

            : base(null,
                   CustomData)

        {

            this.Id      = Id;
            this.Status  = Status;

        }

        #endregion

        #region EVSEStatus(Id, Status, Timestamp, CustomData = null)

        /// <summary>
        /// Create a new EVSE status.
        /// </summary>
        /// <param name="Id">The unique identification of the EVSE.</param>
        /// <param name="Status">The current status of the EVSE.</param>
        /// <param name="Timestamp">The timestamp of the status change of the EVSE.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public EVSEStatus(EVSE_Id                                    Id,
                          EVSEStatusTypes                            Status,
                          DateTime                                   Timestamp,
                          IEnumerable<KeyValuePair<String, Object>>  CustomData  = null)

            : this(Id,
                   new Timestamped<EVSEStatusTypes>(Timestamp, Status),
                   CustomData)

        { }

        #endregion

        #endregion


        #region (static) Snapshot(EVSE)

        /// <summary>
        /// Take a snapshot of the current EVSE status.
        /// </summary>
        /// <param name="EVSE">An EVSE.</param>
        public static EVSEStatus Snapshot(EVSE EVSE)

            => new EVSEStatus(EVSE.Id,
                              EVSE.Status);

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatus1 == null) || ((Object) EVSEStatus2 == null))
                return false;

            return EVSEStatus1.Equals(EVSEStatus2);

        }

        #endregion

        #region Operator != (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #region Operator <  (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            if ((Object) EVSEStatus1 == null)
                throw new ArgumentNullException(nameof(EVSEStatus1), "The given EVSEStatus1 must not be null!");

            return EVSEStatus1.CompareTo(EVSEStatus2) < 0;

        }

        #endregion

        #region Operator <= (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
            => !(EVSEStatus1 > EVSEStatus2);

        #endregion

        #region Operator >  (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            if ((Object) EVSEStatus1 == null)
                throw new ArgumentNullException(nameof(EVSEStatus1), "The given EVSEStatus1 must not be null!");

            return EVSEStatus1.CompareTo(EVSEStatus2) > 0;

        }

        #endregion

        #region Operator >= (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
            => !(EVSEStatus1 < EVSEStatus2);

        #endregion

        #endregion

        #region IComparable<EVSEStatus> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EVSEStatus))
                throw new ArgumentException("The given object is not a EVSEStatus!",
                                            nameof(Object));

            return CompareTo((EVSEStatus) Object);

        }

        #endregion

        #region CompareTo(EVSEStatus)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEStatus">An object to compare with.</param>
        public Int32 CompareTo(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                throw new ArgumentNullException(nameof(EVSEStatus), "The given EVSEStatus must not be null!");

            // Compare EVSE Ids
            var _Result = Id.CompareTo(EVSEStatus.Id);

            // If equal: Compare EVSE status
            if (_Result == 0)
                _Result = Status.CompareTo(EVSEStatus.Status);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEStatus> Members

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

            if (!(Object is EVSEStatus))
                return false;

            return Equals((EVSEStatus) Object);

        }

        #endregion

        #region Equals(EVSEStatus)

        /// <summary>
        /// Compares two EVSE identifications for equality.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                return false;

            return Id.    Equals(EVSEStatus.Id) &&
                   Status.Equals(EVSEStatus.Status);

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

                return Id.    GetHashCode() * 5 ^
                       Status.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Id, " -> ",
                             Status.Value,
                             " since ",
                             Status.Timestamp.ToIso8601());

        #endregion

    }

}
