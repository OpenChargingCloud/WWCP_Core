/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

namespace com.graphdefined.eMI3
{

    /// <summary>
    /// The unique identification of an Electric Vehicle Charging Session.
    /// </summary>
    public class ChargingSessionId : IId,
                                     IEquatable<ChargingSessionId>,
                                     IComparable<ChargingSessionId>

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

        #region ChargingSessionId()

        /// <summary>
        /// Generate a new Electric Vehicle Charging Session identification.
        /// </summary>
        public ChargingSessionId()
        {
            _Id = Guid.NewGuid().ToString();
        }

        #endregion

        #region ChargingSessionId(String)

        /// <summary>
        /// Generate a new Electric Vehicle Charging Session identification.
        /// based on the given string.
        /// </summary>
        public ChargingSessionId(String String)
        {
            _Id = String.Trim();
        }

        #endregion

        #endregion


        #region New

        /// <summary>
        /// Generate a new SessionId.
        /// </summary>
        public static ChargingSessionId New
        {
            get
            {
                return new ChargingSessionId(Guid.NewGuid().ToString());
            }
        }

        #endregion

        #region Parse(EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Session identification.
        /// </summary>
        public static ChargingSessionId Parse(String EVSEOperatorId)
        {
            return new ChargingSessionId(EVSEOperatorId);
        }

        #endregion

        #region TryParse(Text, out EVSEOperatorId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Session identification.
        /// </summary>
        public static Boolean TryParse(String Text, out ChargingSessionId EVSEOperatorId)
        {
            try
            {
                EVSEOperatorId = new ChargingSessionId(Text);
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
        /// Clone an SessionId.
        /// </summary>
        public ChargingSessionId Clone
        {
            get
            {
                return new ChargingSessionId(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A SessionId.</param>
        /// <param name="SessionId2">Another SessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingSessionId SessionId1, ChargingSessionId SessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SessionId1, SessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SessionId1 == null) || ((Object) SessionId2 == null))
                return false;

            return SessionId1.Equals(SessionId2);

        }

        #endregion

        #region Operator != (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A SessionId.</param>
        /// <param name="SessionId2">Another SessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingSessionId SessionId1, ChargingSessionId SessionId2)
        {
            return !(SessionId1 == SessionId2);
        }

        #endregion

        #region Operator <  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A SessionId.</param>
        /// <param name="SessionId2">Another SessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingSessionId SessionId1, ChargingSessionId SessionId2)
        {

            if ((Object) SessionId1 == null)
                throw new ArgumentNullException("The given SessionId1 must not be null!");

            return SessionId1.CompareTo(SessionId2) < 0;

        }

        #endregion

        #region Operator <= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A SessionId.</param>
        /// <param name="SessionId2">Another SessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingSessionId SessionId1, ChargingSessionId SessionId2)
        {
            return !(SessionId1 > SessionId2);
        }

        #endregion

        #region Operator >  (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A SessionId.</param>
        /// <param name="SessionId2">Another SessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingSessionId SessionId1, ChargingSessionId SessionId2)
        {

            if ((Object) SessionId1 == null)
                throw new ArgumentNullException("The given SessionId1 must not be null!");

            return SessionId1.CompareTo(SessionId2) > 0;

        }

        #endregion

        #region Operator >= (SessionId1, SessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId1">A SessionId.</param>
        /// <param name="SessionId2">Another SessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingSessionId SessionId1, ChargingSessionId SessionId2)
        {
            return !(SessionId1 < SessionId2);
        }

        #endregion

        #endregion

        #region IComparable<SessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an SessionId.
            var SessionId = Object as ChargingSessionId;
            if ((Object) SessionId == null)
                throw new ArgumentException("The given object is not a SessionId!");

            return CompareTo(SessionId);

        }

        #endregion

        #region CompareTo(SessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="SessionId">An object to compare with.</param>
        public Int32 CompareTo(ChargingSessionId SessionId)
        {

            if ((Object) SessionId == null)
                throw new ArgumentNullException("The given SessionId must not be null!");

            // Compare the length of the SessionIds
            var _Result = this.Length.CompareTo(SessionId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(SessionId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<SessionId> Members

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

            // Check if the given object is an SessionId.
            var SessionId = Object as ChargingSessionId;
            if ((Object) SessionId == null)
                return false;

            return this.Equals(SessionId);

        }

        #endregion

        #region Equals(SessionId)

        /// <summary>
        /// Compares two SessionIds for equality.
        /// </summary>
        /// <param name="SessionId">A SessionId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingSessionId SessionId)
        {

            if ((Object) SessionId == null)
                return false;

            return _Id.Equals(SessionId._Id);

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
