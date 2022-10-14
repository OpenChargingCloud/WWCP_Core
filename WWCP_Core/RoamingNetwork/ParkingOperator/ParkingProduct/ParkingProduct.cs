/*
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A parking product.
    /// </summary>
    public class ParkingProduct : IEquatable <ParkingProduct>,
                                  IComparable<ParkingProduct>

    {

        #region Properties

        /// <summary>
        /// The unique identification of the parking product.
        /// </summary>
        public ParkingProduct_Id  Id                      { get; }


        /// <summary>
        /// The electric vehicle wants to charge at least for this amount of time.
        /// </summary>
        public TimeSpan?           MinDuration             { get; }

        /// <summary>
        /// Stop parking after this amount of time.
        /// </summary>
        public TimeSpan?           StopChargingAfterTime   { get; }


        /// <summary>
        /// The minimal parking power the electric vehicle accepts [kW].
        /// </summary>
        public kW?                 MinPower                { get; }

        /// <summary>
        /// The maximum parking power the electric vehicle consumes [kW].
        /// </summary>
        public kW?                 MaxPower                { get; }

        /// <summary>
        /// The electric vehicle wants to charge at least this amount of energy [kWh].
        /// </summary>
        public kWh?                MinEnergy               { get; }

        /// <summary>
        /// Stop parking after this amount of charged energy [kWh].
        /// </summary>
        public kWh?                StopChargingAfterKWh    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new parking product.
        /// </summary>
        /// <param name="ParkingProductId"></param>
        /// <param name="MinDuration">The electric vehicle wants to charge at least for this amount of time.</param>
        /// <param name="StopChargingAfterTime">Stop parking after this amount of time.</param>
        /// <param name="MinPower">The minimal parking power the electric vehicle accepts [kW].</param>
        /// <param name="MaxPower">The maximum parking power the electric vehicle consumes [kW].</param>
        /// <param name="MinEnergy">The electric vehicle wants to charge at least this amount of energy [kWh].</param>
        /// <param name="StopChargingAfterKWh">Stop parking after this amount of charged energy [kWh].</param>
        public ParkingProduct(ParkingProduct_Id  ParkingProductId,
                               TimeSpan?           MinDuration            = null,
                               TimeSpan?           StopChargingAfterTime  = null,
                               kW?                 MinPower               = null,
                               kW?                 MaxPower               = null,
                               kWh?                MinEnergy              = null,
                               kWh?                StopChargingAfterKWh   = null)
        {

            this.Id      = ParkingProductId;
            this.MinDuration            = MinDuration;
            this.StopChargingAfterTime  = StopChargingAfterTime;
            this.MinPower               = MinPower;
            this.MaxPower               = MaxPower;
            this.MinEnergy              = MinEnergy;
            this.StopChargingAfterKWh   = StopChargingAfterKWh;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ParkingProduct1, ParkingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct1">A parking product.</param>
        /// <param name="ParkingProduct2">Another parking product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ParkingProduct ParkingProduct1, ParkingProduct ParkingProduct2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ParkingProduct1, ParkingProduct2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ParkingProduct1 == null) || ((Object) ParkingProduct2 == null))
                return false;

            return ParkingProduct1.Equals(ParkingProduct2);

        }

        #endregion

        #region Operator != (ParkingProduct1, ParkingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct1">A parking product.</param>
        /// <param name="ParkingProduct2">Another parking product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ParkingProduct ParkingProduct1, ParkingProduct ParkingProduct2)
            => !(ParkingProduct1 == ParkingProduct2);

        #endregion

        #region Operator <  (ParkingProduct1, ParkingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct1">A parking product.</param>
        /// <param name="ParkingProduct2">Another parking product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ParkingProduct ParkingProduct1, ParkingProduct ParkingProduct2)
        {

            if ((Object) ParkingProduct1 == null)
                throw new ArgumentNullException(nameof(ParkingProduct1), "The given ParkingProduct1 must not be null!");

            return ParkingProduct1.CompareTo(ParkingProduct2) < 0;

        }

        #endregion

        #region Operator <= (ParkingProduct1, ParkingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct1">A parking product.</param>
        /// <param name="ParkingProduct2">Another parking product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ParkingProduct ParkingProduct1, ParkingProduct ParkingProduct2)
            => !(ParkingProduct1 > ParkingProduct2);

        #endregion

        #region Operator >  (ParkingProduct1, ParkingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct1">A parking product.</param>
        /// <param name="ParkingProduct2">Another parking product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ParkingProduct ParkingProduct1, ParkingProduct ParkingProduct2)
        {

            if ((Object) ParkingProduct1 == null)
                throw new ArgumentNullException(nameof(ParkingProduct1), "The given ParkingProduct1 must not be null!");

            return ParkingProduct1.CompareTo(ParkingProduct2) > 0;

        }

        #endregion

        #region Operator >= (ParkingProduct1, ParkingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct1">A parking product.</param>
        /// <param name="ParkingProduct2">Another parking product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ParkingProduct ParkingProduct1, ParkingProduct ParkingProduct2)
            => !(ParkingProduct1 < ParkingProduct2);

        #endregion

        #endregion

        #region IComparable<ParkingProduct> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var _ParkingProduct = Object as ParkingProduct;
            if (_ParkingProduct == null)
                throw new ArgumentException("The given object is not a parking product!",
                                            nameof(Object));

            return CompareTo(_ParkingProduct);

        }

        #endregion

        #region CompareTo(ParkingProduct)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingProduct">An object to compare with.</param>
        public Int32 CompareTo(ParkingProduct ParkingProduct)
        {

            if ((Object) ParkingProduct == null)
                throw new ArgumentNullException(nameof(ParkingProduct),  "The given parking product must not be null!");

            // Compare the ParkingProductIds
            var _Result = this.Id.CompareTo(ParkingProduct.Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ParkingProduct> Members

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

            var _ParkingProduct = Object as ParkingProduct;
            if (_ParkingProduct == null)
                return false;

            return Equals(_ParkingProduct);

        }

        #endregion

        #region Equals(ParkingProduct)

        /// <summary>
        /// Compares two parking products for equality.
        /// </summary>
        /// <param name="ParkingProduct">A parking product to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingProduct ParkingProduct)
        {

            if ((Object) Id == null)
                return false;

            return Id.Equals(ParkingProduct.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Id.ToString();

        #endregion

    }

}
