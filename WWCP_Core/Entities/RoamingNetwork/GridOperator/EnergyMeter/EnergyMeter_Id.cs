/*
 * Copyright (c) 2014-2019 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The unique identification of an energy meter.
    /// </summary>
    public struct EnergyMeter_Id : IId,
                                   IEquatable <EnergyMeter_Id>,
                                   IComparable<EnergyMeter_Id>

    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        #endregion

        #region Properties

        #region New

        /// <summary>
        /// Returns a new energy meter identification.
        /// </summary>
        public static EnergyMeter_Id New
            => EnergyMeter_Id.Parse(Guid.NewGuid().ToString());

        #endregion

        #region Length

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length

            => (UInt64) InternalId.Length;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new energy meter identification.
        /// based on the given string.
        /// </summary>
        private EnergyMeter_Id(String Text)
        {
            InternalId = Text;
        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as an energy meter identification.
        /// </summary>
        /// <param name="Text">A text representation of an energy meter identification.</param>
        public static EnergyMeter_Id Parse(String Text)

            => new EnergyMeter_Id(Text);

        #endregion

        #region TryParse(Text, out EnergyMeterId)

        /// <summary>
        /// Parse the given string as an energy meter identification.
        /// </summary>
        /// <param name="Text">A text representation of an energy meter identification.</param>
        /// <param name="EnergyMeterId">The parsed energy meter identification.</param>
        public static Boolean TryParse(String Text, out EnergyMeter_Id EnergyMeterId)
        {
            try
            {

                EnergyMeterId = new EnergyMeter_Id(Text);

                return true;

            }
            catch (Exception)
            {
                EnergyMeterId = default(EnergyMeter_Id);
                return false;
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this energy meter identification.
        /// </summary>
        public EnergyMeter_Id Clone

            => new EnergyMeter_Id(
                   new String(InternalId.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">An energy meter identification.</param>
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
        /// <param name="EnergyMeterId1">An energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
            => !(EnergyMeterId1 == EnergyMeterId2);

        #endregion

        #region Operator <  (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">An energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {

            if ((Object) EnergyMeterId1 == null)
                throw new ArgumentNullException(nameof(EnergyMeterId1), "The given EnergyMeterId1 must not be null!");

            return EnergyMeterId1.CompareTo(EnergyMeterId2) < 0;

        }

        #endregion

        #region Operator <= (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">An energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
            => !(EnergyMeterId1 > EnergyMeterId2);

        #endregion

        #region Operator >  (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">An energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
        {

            if ((Object) EnergyMeterId1 == null)
                throw new ArgumentNullException(nameof(EnergyMeterId1), "The given EnergyMeterId1 must not be null!");

            return EnergyMeterId1.CompareTo(EnergyMeterId2) > 0;

        }

        #endregion

        #region Operator >= (EnergyMeterId1, EnergyMeterId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EnergyMeterId1">An energy meter identification.</param>
        /// <param name="EnergyMeterId2">Another energy meter identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EnergyMeter_Id EnergyMeterId1, EnergyMeter_Id EnergyMeterId2)
            => !(EnergyMeterId1 < EnergyMeterId2);

        #endregion

        #endregion

        #region IComparable<EnergyMeterId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is EnergyMeter_Id))
                throw new ArgumentException("The given object is not an energy meter identification!",
                                            nameof(Object));

            return CompareTo((EnergyMeter_Id) Object);

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
                throw new ArgumentNullException(nameof(EnergyMeterId),  "The given energy meter identification must not be null!");

            // Compare the length of the EnergyMeterIds
            var _Result = this.Length.CompareTo(EnergyMeterId.Length);

            if (_Result == 0)
                _Result = String.Compare(InternalId, EnergyMeterId.InternalId, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EnergyMeterId> Members

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

            if (!(Object is EnergyMeter_Id))
                return false;

            return Equals((EnergyMeter_Id) Object);

        }

        #endregion

        #region Equals(EnergyMeterId)

        /// <summary>
        /// Compares two EnergyMeterIds for equality.
        /// </summary>
        /// <param name="EnergyMeterId">A EnergyMeterId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EnergyMeter_Id EnergyMeterId)
        {

            if ((Object) EnergyMeterId == null)
                return false;

            return InternalId.Equals(EnergyMeterId.InternalId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => InternalId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => InternalId;

        #endregion

    }

}
