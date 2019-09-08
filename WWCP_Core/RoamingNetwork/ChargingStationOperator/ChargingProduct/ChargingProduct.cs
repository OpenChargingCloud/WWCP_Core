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
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging product.
    /// </summary>
    public class ChargingProduct : IEquatable <ChargingProduct>,
                                   IComparable<ChargingProduct>

    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging product.
        /// </summary>
        public ChargingProduct_Id  Id                      { get; }


        /// <summary>
        /// The electric vehicle wants to charge at least for this amount of time.
        /// </summary>
        public TimeSpan?           MinDuration             { get; }

        /// <summary>
        /// Stop charging after this amount of time.
        /// </summary>
        public TimeSpan?           StopChargingAfterTime   { get; }


        /// <summary>
        /// The minimal charging power the electric vehicle accepts [kW].
        /// </summary>
        public kW?                 MinPower                { get; }

        /// <summary>
        /// The maximum charging power the electric vehicle consumes [kW].
        /// </summary>
        public kW?                 MaxPower                { get; }

        /// <summary>
        /// The electric vehicle wants to charge at least this amount of energy [kWh].
        /// </summary>
        public kWh?                MinEnergy               { get; }

        /// <summary>
        /// Stop charging after this amount of charged energy [kWh].
        /// </summary>
        public kWh?                StopChargingAfterKWh    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging product.
        /// </summary>
        /// <param name="Id">The unqiue identification of this charging product.</param>
        /// <param name="MinDuration">The electric vehicle wants to charge at least for this amount of time.</param>
        /// <param name="StopChargingAfterTime">Stop charging after this amount of time.</param>
        /// <param name="MinPower">The minimal charging power the electric vehicle accepts [kW].</param>
        /// <param name="MaxPower">The maximum charging power the electric vehicle consumes [kW].</param>
        /// <param name="MinEnergy">The electric vehicle wants to charge at least this amount of energy [kWh].</param>
        /// <param name="StopChargingAfterKWh">Stop charging after this amount of charged energy [kWh].</param>
        public ChargingProduct(ChargingProduct_Id  Id,
                               TimeSpan?           MinDuration            = null,
                               TimeSpan?           StopChargingAfterTime  = null,
                               kW?                 MinPower               = null,
                               kW?                 MaxPower               = null,
                               kWh?                MinEnergy              = null,
                               kWh?                StopChargingAfterKWh   = null)
        {

            this.Id                     = Id;
            this.MinDuration            = MinDuration;
            this.StopChargingAfterTime  = StopChargingAfterTime;
            this.MinPower               = MinPower;
            this.MaxPower               = MaxPower;
            this.MinEnergy              = MinEnergy;
            this.StopChargingAfterKWh   = StopChargingAfterKWh;

        }

        #endregion


        #region Parse(ChargingProductId)

        /// <summary>
        /// Parse the given string as a charging product identification.
        /// </summary>
        /// <param name="ChargingProductId">A text representation of a charging product identification.</param>
        public static ChargingProduct Parse(String ChargingProductId)
        {

            if (ChargingProductId?.Trim().IsNullOrEmpty() != false)
                throw new ArgumentNullException(nameof(ChargingProductId), "The given charging product identification must not be null or empty!");

            return new ChargingProduct(ChargingProduct_Id.Parse(ChargingProductId));

        }

        #endregion



        public JObject ToJSON()

            => JSONObject.Create(
                           new JProperty("@id",                   Id.ToString()),
                           MinDuration.HasValue
                               ? new JProperty("minDuration",               MinDuration.Value.TotalSeconds)
                               : null,
                           StopChargingAfterTime.HasValue
                               ? new JProperty("stopChargingAfterTime",     StopChargingAfterTime.Value.TotalSeconds)
                               : null,
                           MinPower.HasValue
                               ? new JProperty("minPower",                  MinPower.Value)
                               : null,
                           MaxPower.HasValue
                               ? new JProperty("maxPower",                  MaxPower.Value)
                               : null,
                           MinEnergy.HasValue
                               ? new JProperty("minEnergy",                 MinEnergy.Value)
                               : null,
                           StopChargingAfterKWh.HasValue
                               ? new JProperty("stopChargingAfterKWh",      StopChargingAfterKWh.Value)
                               : null
                       );


        #region Operator overloading

        #region Operator == (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingProduct ChargingProduct1, ChargingProduct ChargingProduct2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ChargingProduct1, ChargingProduct2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ChargingProduct1 == null) || ((Object) ChargingProduct2 == null))
                return false;

            return ChargingProduct1.Equals(ChargingProduct2);

        }

        #endregion

        #region Operator != (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ChargingProduct ChargingProduct1, ChargingProduct ChargingProduct2)
            => !(ChargingProduct1 == ChargingProduct2);

        #endregion

        #region Operator <  (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingProduct ChargingProduct1, ChargingProduct ChargingProduct2)
        {

            if ((Object) ChargingProduct1 == null)
                throw new ArgumentNullException(nameof(ChargingProduct1), "The given ChargingProduct1 must not be null!");

            return ChargingProduct1.CompareTo(ChargingProduct2) < 0;

        }

        #endregion

        #region Operator <= (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingProduct ChargingProduct1, ChargingProduct ChargingProduct2)
            => !(ChargingProduct1 > ChargingProduct2);

        #endregion

        #region Operator >  (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingProduct ChargingProduct1, ChargingProduct ChargingProduct2)
        {

            if ((Object) ChargingProduct1 == null)
                throw new ArgumentNullException(nameof(ChargingProduct1), "The given ChargingProduct1 must not be null!");

            return ChargingProduct1.CompareTo(ChargingProduct2) > 0;

        }

        #endregion

        #region Operator >= (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingProduct ChargingProduct1, ChargingProduct ChargingProduct2)
            => !(ChargingProduct1 < ChargingProduct2);

        #endregion

        #endregion

        #region IComparable<ChargingProduct> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var _ChargingProduct = Object as ChargingProduct;
            if (_ChargingProduct == null)
                throw new ArgumentException("The given object is not a charging product!",
                                            nameof(Object));

            return CompareTo(_ChargingProduct);

        }

        #endregion

        #region CompareTo(ChargingProduct)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct">An object to compare with.</param>
        public Int32 CompareTo(ChargingProduct ChargingProduct)
        {

            if ((Object) ChargingProduct == null)
                throw new ArgumentNullException(nameof(ChargingProduct),  "The given charging product must not be null!");

            // Compare the ChargingProductIds
            var _Result = this.Id.CompareTo(ChargingProduct.Id);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingProduct> Members

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

            var _ChargingProduct = Object as ChargingProduct;
            if (_ChargingProduct == null)
                return false;

            return Equals(_ChargingProduct);

        }

        #endregion

        #region Equals(ChargingProduct)

        /// <summary>
        /// Compares two charging products for equality.
        /// </summary>
        /// <param name="ChargingProduct">A charging product to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ChargingProduct ChargingProduct)
        {

            if ((Object) Id == null)
                return false;

            return Id.Equals(ChargingProduct.Id);

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
