/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Diagnostics.CodeAnalysis;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The price of e.g. a charging session.
    /// </summary>
    public readonly struct Price : IEquatable<Price>,
                                   IComparable<Price>,
                                   IComparable
    {

        #region Properties

        /// <summary>
        /// The base price (e.g. excluding VAT)
        /// </summary>
        [Mandatory]
        public Decimal    Base        { get; }

        /// <summary>
        /// The additional VAT.
        /// </summary>
        [Optional]
        public Decimal?   VAT         { get; }

        /// <summary>
        /// The ISO 4217 code of the currency used for this charge detail record.
        /// </summary>
        public Currency?  Currency    { get; }


        /// <summary>
        /// The total price.
        /// </summary>
        public Decimal    Total
            => Base + (VAT ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new price e.g. for a charging session.
        /// </summary>
        /// <param name="Base">The base price (e.g. excluding VAT)</param>
        /// <param name="VAT">The optional additional VAT.</param>
        /// <param name="Currency">The optional currency.</param>
        public Price(Decimal    Base,
                     Decimal?   VAT        = null,
                     Currency?  Currency   = null)
        {

            this.Base      = Base;
            this.VAT       = VAT;
            this.Currency  = Currency;

        }

        #endregion


        #region (static) Parse   (JSON, CustomPriceParser = null)

        /// <summary>
        /// Parse the given JSON representation of a price.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPriceParser">An optional delegate to parse custom price JSON objects.</param>
        public static Price Parse(JObject                              JSON,
                                  CustomJObjectParserDelegate<Price>?  CustomPriceParser   = null)
        {

            if (TryParse(JSON,
                         out var price,
                         out var errorResponse,
                         CustomPriceParser))
            {
                return price;
            }

            throw new ArgumentException("The given JSON representation of a price is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, CustomPriceParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a price.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomPriceParser">An optional delegate to parse custom price JSON objects.</param>
        public static Price? TryParse(JObject                              JSON,
                                      CustomJObjectParserDelegate<Price>?  CustomPriceParser   = null)
        {

            if (TryParse(JSON,
                         out var price,
                         out var errorResponse,
                         CustomPriceParser))
            {
                return price;
            }

            return default;

        }

        #endregion

        #region (static) TryParse(JSON, out Price, out ErrorResponse, CustomPriceParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a price.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Price">The parsed price.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                           JSON,
                                       out Price                         Price,
                                       [NotNullWhen(false)] out String?  ErrorResponse)

            => TryParse(JSON,
                        out Price,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a price.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="Price">The parsed price.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPriceParser">An optional delegate to parse custom price JSON objects.</param>
        public static Boolean TryParse(JObject                              JSON,
                                       out Price                            Price,
                                       [NotNullWhen(false)] out String?     ErrorResponse,
                                       CustomJObjectParserDelegate<Price>?  CustomPriceParser   = null)
        {

            try
            {

                Price = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse Base            [mandatory]

                if (!JSON.ParseMandatory("base",
                                         "base price",
                                         out Decimal Base,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse IncludingVAT    [optional]

                if (JSON.ParseOptional("vat",
                                       "valued added tax",
                                       out Decimal? VAT,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse Curreny         [optional]

                if (JSON.ParseOptional("curreny",
                                       "curreny",
                                       org.GraphDefined.Vanaheimr.Illias.Currency.TryParse,
                                       out Currency? Currency,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                Price = new Price(
                            Base,
                            VAT,
                            Currency
                        );


                if (CustomPriceParser is not null)
                    Price = CustomPriceParser(JSON,
                                              Price);

                return true;

            }
            catch (Exception e)
            {
                Price          = default;
                ErrorResponse  = "The given JSON representation of a price is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPriceSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPriceSerializer">A delegate to serialize custom price JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<Price>? CustomPriceSerializer = null)
        {

            var JSON = JSONObject.Create(

                                 new JProperty("base",       Base),

                           VAT.HasValue
                               ? new JProperty("vat",        VAT)
                               : null,

                           Currency is not null
                               ? new JProperty("currency",   Currency.ISOCode)
                               : null

                       );

            return CustomPriceSerializer is not null
                       ? CustomPriceSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Static Definitions

        public static Price EURO(Decimal   Base,
                                Decimal?  VAT = null)

            => new (Base,
                    VAT,
                    Currency.EUR);

        #endregion


        #region Operator overloading

        #region Operator == (Price1, Price2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Price1">A price.</param>
        /// <param name="Price2">Another price.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Price Price1,
                                           Price Price2)

            => Price1.Equals(Price2);

        #endregion

        #region Operator != (Price1, Price2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Price1">A price.</param>
        /// <param name="Price2">Another price.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Price Price1,
                                           Price Price2)

            => !Price1.Equals(Price2);

        #endregion

        #region Operator <  (Price1, Price2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Price1">A price.</param>
        /// <param name="Price2">Another price.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (Price Price1,
                                          Price Price2)

            => Price1.CompareTo(Price2) < 0;

        #endregion

        #region Operator <= (Price1, Price2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Price1">A price.</param>
        /// <param name="Price2">Another price.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (Price Price1,
                                           Price Price2)

            => Price1.CompareTo(Price2) <= 0;

        #endregion

        #region Operator >  (Price1, Price2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Price1">A price.</param>
        /// <param name="Price2">Another price.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (Price Price1,
                                          Price Price2)

            => Price1.CompareTo(Price2) > 0;

        #endregion

        #region Operator >= (Price1, Price2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Price1">A price.</param>
        /// <param name="Price2">Another price.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (Price Price1,
                                           Price Price2)

            => Price1.CompareTo(Price2) >= 0;

        #endregion

        #endregion

        #region IComparable<Price> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two prices.
        /// </summary>
        /// <param name="Object">A price to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is Price price
                   ? CompareTo(price)
                   : throw new ArgumentException("The given object is not a price!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(Price)

        /// <summary>
        /// Compares two prices.
        /// </summary>
        /// <param name="Price">A price to compare with.</param>
        public Int32 CompareTo(Price Price)
        {

            var c = Base.CompareTo(Price.Base);

            if (c == 0 && VAT.HasValue && Price.VAT.HasValue)
                c = VAT.Value.CompareTo(Price.VAT.Value);

            return c;

        }

        #endregion

        #endregion

        #region IEquatable<Price> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two prices for equality.
        /// </summary>
        /// <param name="Object">A price to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Price price &&
                   Equals(price);

        #endregion

        #region Equals(Price)

        /// <summary>
        /// Compares two prices for equality.
        /// </summary>
        /// <param name="Price">A price to compare with.</param>
        public Boolean Equals(Price Price)

            => Base.Equals(Price.Base) &&

            ((!VAT.HasValue && !Price.VAT.HasValue) ||
              (VAT.HasValue &&  Price.VAT.HasValue && VAT.Value.Equals(Price.VAT.Value)));

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Base.GetHashCode() * 3 ^
                      (VAT?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   Base,

                   VAT.HasValue
                       ? ", VAT: " + VAT
                       : ""

               );

        #endregion

    }

}
