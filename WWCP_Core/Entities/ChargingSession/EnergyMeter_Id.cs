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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an energy meter.
    /// </summary>
    public class EnergyMeter_Id : IId,
                                  IEquatable<EnergyMeter_Id>,
                                  IComparable<EnergyMeter_Id>

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

        /// <summary>
        /// Generate a new energy meter identification based on the given string.
        /// </summary>
        private EnergyMeter_Id(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentException("The parameter must not be null or empty!", "Text");

            #endregion

            _Id = Text.Trim();

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an energy meter identification.
        /// </summary>
        /// <param name="Text">A text representation of an Electric Vehicle Roaming Network identification.</param>
        public static EnergyMeter_Id Parse(String Text)
        {

            EnergyMeter_Id _EnergyMeterId = null;

            if (TryParse(Text, out _EnergyMeterId))
                return _EnergyMeterId;

            return null;

        }

        #endregion

        #region TryParse(Text, out EnergyMeterId)

        /// <summary>
        /// Parse the given string as an energy meter identification.
        /// </summary>
        /// <param name="Text">A text representation of an energy meter identification.</param>
        /// <param name="EnergyMeterId">The parsed energy meter identification.</param>
        public static Boolean TryParse(String Text, out EnergyMeter_Id EnergyMeterId)
        {

             EnergyMeterId = new EnergyMeter_Id(Text);
             return true;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this energy meter identification.
        /// </summary>
        public EnergyMeter_Id Clone
        {
            get
            {
                return new EnergyMeter_Id(new String(_Id.ToCharArray()));
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">A energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EnergyMeterId1, EnergyMeterId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EnergyMeterId1 == null) || ((Object) EnergyMeterId2 == null))
                return false;

            return EnergyMeterId1.Equals(EnergyMeterId2);

        }

        #endregion

        #region Operator != (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">A energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {
            return !(EnergyMeterId1 == EnergyMeterId2);
        }

        #endregion

        #region Operator <  (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">A energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {

            if ((Object) EnergyMeterId1 == null)
                throw new ArgumentNullException("The given energy meter identification must not be null!");

            return EnergyMeterId1.CompareTo(EnergyMeterId2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">A energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {
            return !(EnergyMeterId1 > EnergyMeterId2);
        }

        #endregion

        #region Operator >  (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">A energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {

            if ((Object) EnergyMeterId1 == null)
                throw new ArgumentNullException("The given energy meter identification must not be null!");

            return EnergyMeterId1.CompareTo(EnergyMeterId2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">A energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {
            return !(EnergyMeterId1 < EnergyMeterId2);
        }

        #endregion

        #endregion

        #region IComparable<EnergyMeter_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an energy meter identification.
            var EnergyMeterId = Object as EnergyMeter_Id;
            if ((Object) EnergyMeterId == null)
                throw new ArgumentException("The given object is not a energy meter identification!");

            return CompareTo(EnergyMeterId);

        }

        #endregion

        #region CompareTo(EnergyMeterId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId">An object to compare with.</param>
        public Int32 CompareTo(EnergyMeter_Id EnergyMeterId)
        {

            if ((Object) EnergyMeterId == null)
                throw new ArgumentNullException("The given energy meter identification must not be null!");

            return _Id.CompareTo(EnergyMeterId._Id);

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeter_Id> Members

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

            // Check if the given object is an energy meter identification.
            var EnergyMeterId = Object as EnergyMeter_Id;
            if ((Object) EnergyMeterId == null)
                return false;

            return this.Equals(EnergyMeterId);

        }

        #endregion

        #region Equals(EnergyMeterId)

        /// <summary>
        /// Compares two energy meter identifications for equality.
        /// </summary>
        /// <param name="EnergyMeterId">Another energy meter identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EnergyMeter_Id EnergyMeterId)
        {

            if ((Object) EnergyMeterId == null)
                return false;

            return _Id.Equals(EnergyMeterId._Id);

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
