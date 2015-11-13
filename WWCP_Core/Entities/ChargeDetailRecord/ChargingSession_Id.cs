/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
    /// The unique identification of an Electric Vehicle Charging Session.
    /// </summary>
    public class ChargingSession_Id : IId,
                                      IEquatable<ChargingSession_Id>,
                                      IComparable<ChargingSession_Id>

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
        /// Returns a new Electric Vehicle Charging Session identification.
        /// </summary>
        public static ChargingSession_Id New
        {
            get
            {
                return ChargingSession_Id.Parse(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Charging Session identification.
        /// based on the given string.
        /// </summary>
        private ChargingSession_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Session identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Session identification.</param>
        public static ChargingSession_Id Parse(String Text)
        {
            return new ChargingSession_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingSessionId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Session identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Session identification.</param>
        /// <param name="ChargingSessionId">The parsed Electric Vehicle Charging Session identification.</param>
        public static Boolean TryParse(String Text, out ChargingSession_Id ChargingSessionId)
        {
            try
            {
                ChargingSessionId = new ChargingSession_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingSessionId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Session identification.
        /// </summary>
        public ChargingSession_Id Clone
        {
            get
            {
                return new ChargingSession_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingSession_Id ChargingSessionId1, ChargingSession_Id ChargingSessionId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingSessionId1, ChargingSessionId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingSessionId1 == null) || ((Object) ChargingSessionId2 == null))
                return false;

            return ChargingSessionId1.Equals(ChargingSessionId2);

        }

        #endregion

        #region Operator != (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingSession_Id ChargingSessionId1, ChargingSession_Id ChargingSessionId2)
        {
            return !(ChargingSessionId1 == ChargingSessionId2);
        }

        #endregion

        #region Operator <  (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingSession_Id ChargingSessionId1, ChargingSession_Id ChargingSessionId2)
        {

            if ((Object) ChargingSessionId1 == null)
                throw new ArgumentNullException("The given ChargingSessionId1 must not be null!");

            return ChargingSessionId1.CompareTo(ChargingSessionId2) < 0;

        }

        #endregion

        #region Operator <= (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingSession_Id ChargingSessionId1, ChargingSession_Id ChargingSessionId2)
        {
            return !(ChargingSessionId1 > ChargingSessionId2);
        }

        #endregion

        #region Operator >  (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingSession_Id ChargingSessionId1, ChargingSession_Id ChargingSessionId2)
        {

            if ((Object) ChargingSessionId1 == null)
                throw new ArgumentNullException("The given ChargingSessionId1 must not be null!");

            return ChargingSessionId1.CompareTo(ChargingSessionId2) > 0;

        }

        #endregion

        #region Operator >= (ChargingSessionId1, ChargingSessionId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId1">A ChargingSessionId.</param>
        /// <param name="ChargingSessionId2">Another ChargingSessionId.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingSession_Id ChargingSessionId1, ChargingSession_Id ChargingSessionId2)
        {
            return !(ChargingSessionId1 < ChargingSessionId2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingSessionId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingSessionId.
            var ChargingSessionId = Object as ChargingSession_Id;
            if ((Object) ChargingSessionId == null)
                throw new ArgumentException("The given object is not a ChargingSessionId!");

            return CompareTo(ChargingSessionId);

        }

        #endregion

        #region CompareTo(ChargingSessionId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingSessionId">An object to compare with.</param>
        public Int32 CompareTo(ChargingSession_Id ChargingSessionId)
        {

            if ((Object) ChargingSessionId == null)
                throw new ArgumentNullException("The given ChargingSessionId must not be null!");

            // Compare the length of the ChargingSessionIds
            var _Result = this.Length.CompareTo(ChargingSessionId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingSessionId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingSessionId> Members

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

            // Check if the given object is an ChargingSessionId.
            var ChargingSessionId = Object as ChargingSession_Id;
            if ((Object) ChargingSessionId == null)
                return false;

            return this.Equals(ChargingSessionId);

        }

        #endregion

        #region Equals(ChargingSessionId)

        /// <summary>
        /// Compares two ChargingSessionIds for equality.
        /// </summary>
        /// <param name="ChargingSessionId">A ChargingSessionId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingSession_Id ChargingSessionId)
        {

            if ((Object) ChargingSessionId == null)
                return false;

            return _Id.Equals(ChargingSessionId._Id);

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
