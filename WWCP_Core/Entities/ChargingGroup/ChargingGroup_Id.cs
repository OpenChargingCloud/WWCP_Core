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
    /// The unique identification of a Electric Vehicle Charging Group Identification (EVCGId).
    /// </summary>
    public class ChargingGroup_Id : IId,
                                    IEquatable<ChargingGroup_Id>,
                                    IComparable<ChargingGroup_Id>

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
        /// Generate a new unique identification of an Electric Vehicle Charging Group Identification (EVCGId).
        /// </summary>
        public static ChargingGroup_Id New
        {
            get
            {
                return new ChargingGroup_Id(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Charging Group Identification (EVCG Id)
        /// based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the charging group identification.</param>
        private ChargingGroup_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Group Identification (EVCG Id)
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Group identification.</param>
        public static ChargingGroup_Id Parse(String Text)
        {
            return new ChargingGroup_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingGroupId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Group Identification (EVCG Id)
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Group identification.</param>
        /// <param name="ChargingGroupId">The parsed Electric Vehicle Charging Group identification.</param>
        public static Boolean TryParse(String Text, out ChargingGroup_Id ChargingGroupId)
        {
            try
            {
                ChargingGroupId = new ChargingGroup_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingGroupId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Group identification.
        /// </summary>
        public ChargingGroup_Id Clone
        {
            get
            {
                return new ChargingGroup_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (ChargingGroup_Id1, ChargingGroup_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id1">A ChargingGroup_Id.</param>
        /// <param name="ChargingGroup_Id2">Another ChargingGroup_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingGroup_Id ChargingGroup_Id1, ChargingGroup_Id ChargingGroup_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingGroup_Id1, ChargingGroup_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingGroup_Id1 == null) || ((Object) ChargingGroup_Id2 == null))
                return false;

            return ChargingGroup_Id1.Equals(ChargingGroup_Id2);

        }

        #endregion

        #region Operator != (ChargingGroup_Id1, ChargingGroup_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id1">A ChargingGroup_Id.</param>
        /// <param name="ChargingGroup_Id2">Another ChargingGroup_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingGroup_Id ChargingGroup_Id1, ChargingGroup_Id ChargingGroup_Id2)
        {
            return !(ChargingGroup_Id1 == ChargingGroup_Id2);
        }

        #endregion

        #region Operator <  (ChargingGroup_Id1, ChargingGroup_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id1">A ChargingGroup_Id.</param>
        /// <param name="ChargingGroup_Id2">Another ChargingGroup_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingGroup_Id ChargingGroup_Id1, ChargingGroup_Id ChargingGroup_Id2)
        {

            if ((Object) ChargingGroup_Id1 == null)
                throw new ArgumentNullException("The given ChargingGroup_Id1 must not be null!");

            return ChargingGroup_Id1.CompareTo(ChargingGroup_Id2) < 0;

        }

        #endregion

        #region Operator <= (ChargingGroup_Id1, ChargingGroup_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id1">A ChargingGroup_Id.</param>
        /// <param name="ChargingGroup_Id2">Another ChargingGroup_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingGroup_Id ChargingGroup_Id1, ChargingGroup_Id ChargingGroup_Id2)
        {
            return !(ChargingGroup_Id1 > ChargingGroup_Id2);
        }

        #endregion

        #region Operator >  (ChargingGroup_Id1, ChargingGroup_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id1">A ChargingGroup_Id.</param>
        /// <param name="ChargingGroup_Id2">Another ChargingGroup_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingGroup_Id ChargingGroup_Id1, ChargingGroup_Id ChargingGroup_Id2)
        {

            if ((Object) ChargingGroup_Id1 == null)
                throw new ArgumentNullException("The given ChargingGroup_Id1 must not be null!");

            return ChargingGroup_Id1.CompareTo(ChargingGroup_Id2) > 0;

        }

        #endregion

        #region Operator >= (ChargingGroup_Id1, ChargingGroup_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id1">A ChargingGroup_Id.</param>
        /// <param name="ChargingGroup_Id2">Another ChargingGroup_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingGroup_Id ChargingGroup_Id1, ChargingGroup_Id ChargingGroup_Id2)
        {
            return !(ChargingGroup_Id1 < ChargingGroup_Id2);
        }

        #endregion

        #endregion

        #region IComparable<ChargingGroup_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an ChargingGroup_Id.
            var ChargingGroupId = Object as ChargingGroup_Id;
            if ((Object) ChargingGroupId == null)
                throw new ArgumentException("The given object is not a ChargingGroup_Id!");

            return CompareTo(ChargingGroupId);

        }

        #endregion

        #region CompareTo(ChargingGroupId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingGroup_Id">An object to compare with.</param>
        public Int32 CompareTo(ChargingGroup_Id ChargingGroupId)
        {

            if ((Object) ChargingGroupId == null)
                throw new ArgumentNullException("The given ChargingGroupId must not be null!");

            // Compare the length of the ChargingGroupIds
            var _Result = this.Length.CompareTo(ChargingGroupId.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(ChargingGroupId._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingGroup_Id> Members

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

            // Check if the given object is an ChargingGroupId.
            var ChargingGroupId = Object as ChargingGroup_Id;
            if ((Object) ChargingGroupId == null)
                return false;

            return this.Equals(ChargingGroupId);

        }

        #endregion

        #region Equals(ChargingGroupId)

        /// <summary>
        /// Compares two ChargingGroup_Ids for equality.
        /// </summary>
        /// <param name="ChargingGroupId">A ChargingGroup_Id to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingGroup_Id ChargingGroupId)
        {

            if ((Object) ChargingGroupId == null)
                return false;

            return _Id.Equals(ChargingGroupId._Id);

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
