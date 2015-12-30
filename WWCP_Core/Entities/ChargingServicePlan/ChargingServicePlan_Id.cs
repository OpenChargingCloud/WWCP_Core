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
    /// The unique identification of an Electric Vehicle Charging Service Plan (EVCSP Id).
    /// </summary>
    public class ChargingServicePlan_Id : IId,
                                          IEquatable<ChargingServicePlan_Id>,
                                          IComparable<ChargingServicePlan_Id>

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
        /// Generate a new unique identification of an Electric Vehicle Charging Service Plan (EVCSP Id).
        /// </summary>
        public static ChargingServicePlan_Id New
        {
            get
            {
                return new ChargingServicePlan_Id(Guid.NewGuid().ToString());
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
        /// Generate a new Electric Vehicle Charging Service Plan identification (EVSP Id)
        /// based on the given string.
        /// </summary>
        private ChargingServicePlan_Id(String String)
        {
            _Id = String.Trim();
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Service Plan (EVCSP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Group identification.</param>
        public static ChargingServicePlan_Id Parse(String Text)
        {
            return new ChargingServicePlan_Id(Text);
        }

        #endregion

        #region TryParse(Text, out ChargingServicePlanId)

        /// <summary>
        /// Parse the given string as an Electric Vehicle Charging Service Plan (EVCSP Id).
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Charging Service Plan identification.</param>
        /// <param name="ChargingServicePlanId">The parsed Electric Vehicle Charging Service Plan identification.</param>
        public static Boolean TryParse(String Text, out ChargingServicePlan_Id ChargingServicePlanId)
        {
            try
            {
                ChargingServicePlanId = new ChargingServicePlan_Id(Text);
                return true;
            }
            catch (Exception)
            {
                ChargingServicePlanId = null;
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this Electric Vehicle Charging Service Plan identification.
        /// </summary>
        public ChargingServicePlan_Id Clone
        {
            get
            {
                return new ChargingServicePlan_Id(_Id);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EVCSP_Id1, EVCSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id1">A EVCSP_Id.</param>
        /// <param name="EVCSP_Id2">Another EVCSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingServicePlan_Id EVCSP_Id1, ChargingServicePlan_Id EVCSP_Id2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVCSP_Id1, EVCSP_Id2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVCSP_Id1 == null) || ((Object) EVCSP_Id2 == null))
                return false;

            return EVCSP_Id1.Equals(EVCSP_Id2);

        }

        #endregion

        #region Operator != (EVCSP_Id1, EVCSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id1">A EVCSP_Id.</param>
        /// <param name="EVCSP_Id2">Another EVCSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingServicePlan_Id EVCSP_Id1, ChargingServicePlan_Id EVCSP_Id2)
        {
            return !(EVCSP_Id1 == EVCSP_Id2);
        }

        #endregion

        #region Operator <  (EVCSP_Id1, EVCSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id1">A EVCSP_Id.</param>
        /// <param name="EVCSP_Id2">Another EVCSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingServicePlan_Id EVCSP_Id1, ChargingServicePlan_Id EVCSP_Id2)
        {

            if ((Object) EVCSP_Id1 == null)
                throw new ArgumentNullException("The given EVCSP_Id1 must not be null!");

            return EVCSP_Id1.CompareTo(EVCSP_Id2) < 0;

        }

        #endregion

        #region Operator <= (EVCSP_Id1, EVCSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id1">A EVCSP_Id.</param>
        /// <param name="EVCSP_Id2">Another EVCSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingServicePlan_Id EVCSP_Id1, ChargingServicePlan_Id EVCSP_Id2)
        {
            return !(EVCSP_Id1 > EVCSP_Id2);
        }

        #endregion

        #region Operator >  (EVCSP_Id1, EVCSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id1">A EVCSP_Id.</param>
        /// <param name="EVCSP_Id2">Another EVCSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingServicePlan_Id EVCSP_Id1, ChargingServicePlan_Id EVCSP_Id2)
        {

            if ((Object) EVCSP_Id1 == null)
                throw new ArgumentNullException("The given EVCSP_Id1 must not be null!");

            return EVCSP_Id1.CompareTo(EVCSP_Id2) > 0;

        }

        #endregion

        #region Operator >= (EVCSP_Id1, EVCSP_Id2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id1">A EVCSP_Id.</param>
        /// <param name="EVCSP_Id2">Another EVCSP_Id.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingServicePlan_Id EVCSP_Id1, ChargingServicePlan_Id EVCSP_Id2)
        {
            return !(EVCSP_Id1 < EVCSP_Id2);
        }

        #endregion

        #endregion

        #region IComparable<EVCSP_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an EVCSP_Id.
            var EVCSP_Id = Object as ChargingServicePlan_Id;
            if ((Object) EVCSP_Id == null)
                throw new ArgumentException("The given object is not a EVCSP_Id!");

            return CompareTo(EVCSP_Id);

        }

        #endregion

        #region CompareTo(EVCSP_Id)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVCSP_Id">An object to compare with.</param>
        public Int32 CompareTo(ChargingServicePlan_Id EVCSP_Id)
        {

            if ((Object) EVCSP_Id == null)
                throw new ArgumentNullException("The given EVCSP_Id must not be null!");

            // Compare the length of the EVP_Ids
            var _Result = this.Length.CompareTo(EVCSP_Id.Length);

            // If equal: Compare Ids
            if (_Result == 0)
                _Result = _Id.CompareTo(EVCSP_Id._Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVCSP_Id> Members

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

            // Check if the given object is an EVCSP_Id.
            var EVCSP_Id = Object as ChargingServicePlan_Id;
            if ((Object) EVCSP_Id == null)
                return false;

            return this.Equals(EVCSP_Id);

        }

        #endregion

        #region Equals(EVCSP_Id)

        /// <summary>
        /// Compares two electric vehicle charging service plan identifications for equality.
        /// </summary>
        /// <param name="EVCSP_Id">An electric vehicle charging service plan identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingServicePlan_Id EVCSP_Id)
        {

            if ((Object) EVCSP_Id == null)
                return false;

            return _Id.Equals(EVCSP_Id._Id);

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
