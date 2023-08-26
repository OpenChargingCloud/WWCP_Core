/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A charging product.
    /// </summary>
    public class ChargingProduct : IEquatable<ChargingProduct>,
                                   IComparable<ChargingProduct>,
                                   IComparable
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
        /// The minimal charging power the electric vehicle accepts.
        /// </summary>
        public Watt?               MinPower                { get; }

        /// <summary>
        /// The maximum charging power the electric vehicle consumes.
        /// </summary>
        public Watt?               MaxPower                { get; }

        /// <summary>
        /// The electric vehicle wants to charge at least this amount of energy [kWh].
        /// </summary>
        public WattHour?           MinEnergy               { get; }

        /// <summary>
        /// Stop charging after this amount of charged energy [kWh].
        /// </summary>
        public WattHour?           StopChargingAfterKWh    { get; }

        public Decimal?            MaxB2BServiceCosts      { get; }

        public Boolean?            IntermediateCDRs        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging product.
        /// </summary>
        /// <param name="Id">The unqiue identification of this charging product.</param>
        /// <param name="MinDuration">The electric vehicle wants to charge at least for this amount of time.</param>
        /// <param name="StopChargingAfterTime">Stop charging after this amount of time.</param>
        /// <param name="MinPower">The minimal charging power the electric vehicle accepts.</param>
        /// <param name="MaxPower">The maximum charging power the electric vehicle consumes.</param>
        /// <param name="MinEnergy">The electric vehicle wants to charge at least this amount of energy [Wh].</param>
        /// <param name="StopChargingAfterKWh">Stop charging after this amount of charged energy [Wh].</param>
        public ChargingProduct(ChargingProduct_Id  Id,
                               TimeSpan?           MinDuration             = null,
                               TimeSpan?           StopChargingAfterTime   = null,
                               Watt?               MinPower                = null,
                               Watt?               MaxPower                = null,
                               WattHour?           MinEnergy               = null,
                               WattHour?           StopChargingAfterKWh    = null,
                               Decimal?            MaxB2BServiceCosts      = null,
                               Boolean?            IntermediateCDRs        = null)
        {

            this.Id                     = Id;
            this.MinDuration            = MinDuration;
            this.StopChargingAfterTime  = StopChargingAfterTime;
            this.MinPower               = MinPower;
            this.MaxPower               = MaxPower;
            this.MinEnergy              = MinEnergy;
            this.StopChargingAfterKWh   = StopChargingAfterKWh;
            this.MaxB2BServiceCosts     = MaxB2BServiceCosts;
            this.IntermediateCDRs       = IntermediateCDRs;

        }

        #endregion


        #region (static) FromId(ChargingProductId)

        /// <summary>
        /// Create a charging product based on the given charging product identification.
        /// </summary>
        /// <param name="ChargingProductId">A charging product identification.</param>
        public static ChargingProduct FromId(ChargingProduct_Id ChargingProductId)

            => new (ChargingProductId);

        #endregion

        #region Static defaults

        /// <summary>
        /// AC1
        /// </summary>
        public static ChargingProduct AC1
            => FromId(ChargingProduct_Id.AC1);

        /// <summary>
        /// AC3
        /// </summary>
        public static ChargingProduct AC3
            => FromId(ChargingProduct_Id.AC3);

        /// <summary>
        /// DC
        /// </summary>
        public static ChargingProduct DC
            => FromId(ChargingProduct_Id.DC);

        #endregion


        #region ToJSON(CustomChargingProductSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomChargingProductSerializer">A delegate to serialize custom ChargingProduct JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ChargingProduct>? CustomChargingProductSerializer = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("@id",                       Id.ToString()),

                           MinDuration.HasValue
                               ? new JProperty("minDuration",               MinDuration.          Value.TotalSeconds)
                               : null,

                           StopChargingAfterTime.HasValue
                               ? new JProperty("stopChargingAfterTime",     StopChargingAfterTime.Value.TotalSeconds)
                               : null,

                           MinPower.HasValue
                               ? new JProperty("minPower",                  MinPower.             Value.Value)
                               : null,

                           MaxPower.HasValue
                               ? new JProperty("maxPower",                  MaxPower.             Value.Value)
                               : null,

                           MinEnergy.HasValue
                               ? new JProperty("minEnergy",                 MinEnergy.            Value.Value)
                               : null,

                           StopChargingAfterKWh.HasValue
                               ? new JProperty("stopChargingAfterKWh",      StopChargingAfterKWh. Value.Value)
                               : null,

                           MaxB2BServiceCosts.HasValue
                               ? new JProperty("maxB2BServiceCosts",        MaxB2BServiceCosts.   Value)
                               : null,

                           IntermediateCDRs.HasValue
                               ? new JProperty("intermediateCDRs",          IntermediateCDRs.     Value)
                               : null

                       );

            return CustomChargingProductSerializer is not null
                       ? CustomChargingProductSerializer(this, json)
                       : json;

        }

        #endregion

        public static Boolean TryParse(JObject               JSON,
                                       out ChargingProduct?  ChargingProduct,
                                       out String?           ErrorResponse)
        {

            ErrorResponse    = "Not implemented!";
            ChargingProduct  = null;

            return false;

        }


        #region Operator overloading

        #region Operator == (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ChargingProduct ChargingProduct1,
                                           ChargingProduct ChargingProduct2)
        {

            if (Object.ReferenceEquals(ChargingProduct1, ChargingProduct2))
                return true;

            if (ChargingProduct1 is null || ChargingProduct2 is null)
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
        public static Boolean operator != (ChargingProduct ChargingProduct1,
                                           ChargingProduct ChargingProduct2)

            => !(ChargingProduct1 == ChargingProduct2);

        #endregion

        #region Operator <  (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (ChargingProduct ChargingProduct1,
                                          ChargingProduct ChargingProduct2)

            => ChargingProduct1 is null
                   ? throw new ArgumentNullException(nameof(ChargingProduct1), "The given charging product must not be null!")
                   : ChargingProduct1.CompareTo(ChargingProduct2) < 0;

        #endregion

        #region Operator <= (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (ChargingProduct ChargingProduct1,
                                           ChargingProduct ChargingProduct2)

            => !(ChargingProduct1 > ChargingProduct2);

        #endregion

        #region Operator >  (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (ChargingProduct ChargingProduct1,
                                          ChargingProduct ChargingProduct2)

            => ChargingProduct1 is null
                   ? throw new ArgumentNullException(nameof(ChargingProduct1), "The given charging product must not be null!")
                   : ChargingProduct1.CompareTo(ChargingProduct2) > 0;

        #endregion

        #region Operator >= (ChargingProduct1, ChargingProduct2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ChargingProduct1">A charging product.</param>
        /// <param name="ChargingProduct2">Another charging product.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (ChargingProduct ChargingProduct1,
                                           ChargingProduct ChargingProduct2)

            => !(ChargingProduct1 < ChargingProduct2);

        #endregion

        #endregion

        #region IComparable<ChargingProduct> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging products.
        /// </summary>
        /// <param name="Object">A charging product to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is ChargingProduct chargingProduct
                   ? CompareTo(chargingProduct)
                   : throw new ArgumentException("The given object is not a charging product!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(ChargingProduct)

        /// <summary>
        /// Compares two charging products.
        /// </summary>
        /// <param name="ChargingProduct">A charging product to compare with.</param>
        public Int32 CompareTo(ChargingProduct? ChargingProduct)
        {

            if (ChargingProduct is null)
                throw new ArgumentNullException(nameof(EVSE), "The given charging product must not be null!");

            var c = Id.CompareTo(ChargingProduct.Id);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<ChargingProduct> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging products for equality.
        /// </summary>
        /// <param name="Object">A charging product to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ChargingProduct chargingProduct &&
                   Equals(chargingProduct);

        #endregion

        #region Equals(ChargingProduct)

        /// <summary>
        /// Compares two charging products for equality.
        /// </summary>
        /// <param name="ChargingProduct">A charging product to compare with.</param>
        public Boolean Equals(ChargingProduct? ChargingProduct)

            => ChargingProduct is not null &&

               Id.Equals(ChargingProduct.Id);

        #endregion

        #endregion

        #region (override) GetHashCode()

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
