/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using System.Text;
using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The JSON Canonicalization Scheme (JCS) of RFC 8785.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A signer and a verifier that canonicalize the same JSON value get the same
    /// bytes, whatever whitespace, member order or number spelling the text had
    /// on the wire. That lets a verifier check the payload it received rather
    /// than a re-serialization of what its parser understood.
    /// </para>
    /// <para>
    /// The rules: members sorted by the UTF-16 code units of their names, no
    /// whitespace, strings escaped as ECMAScript's JSON.stringify escapes them,
    /// and every number written as ECMAScript writes an IEEE 754 double.
    /// </para>
    /// <para>
    /// A value that Newtonsoft holds in a type JSON does not have - a date, a
    /// GUID, a URI, a time span, bytes - is canonicalized as the string
    /// Newtonsoft writes for it, which is the string a peer receives. A peer
    /// reads it back as a string (see JSONMessages.ParseArray), so both sides
    /// canonicalize the same text.
    /// </para>
    /// </remarks>
    public static class JSONCanonicalization
    {

        #region Canonicalize       (JSON)

        /// <summary>
        /// The canonical form of the given JSON value.
        /// </summary>
        /// <param name="JSON">A JSON value.</param>
        /// <exception cref="ArgumentException">The value holds a number JSON cannot express (NaN, an infinity).</exception>
        public static String Canonicalize(JToken JSON)
        {
            var builder = new StringBuilder();
            Write(JSON, builder);
            return builder.ToString();
        }

        #endregion

        #region CanonicalizeToUTF8 (JSON)

        /// <summary>
        /// The UTF-8 bytes of the canonical form of the given JSON value.
        /// </summary>
        /// <param name="JSON">A JSON value.</param>
        public static Byte[] CanonicalizeToUTF8(JToken JSON)

            => Encoding.UTF8.GetBytes(Canonicalize(JSON));

        #endregion


        #region (private) Write        (JSON, Builder)

        private static void Write(JToken JSON, StringBuilder Builder)
        {

            switch (JSON.Type)
            {

                case JTokenType.Object:

                    Builder.Append('{');

                    var first = true;

                    foreach (var property in ((JObject) JSON).Properties().OrderBy(property => property.Name, StringComparer.Ordinal))
                    {

                        if (!first)
                            Builder.Append(',');

                        first = false;

                        WriteString(property.Name, Builder);
                        Builder.Append(':');
                        Write(property.Value, Builder);

                    }

                    Builder.Append('}');
                    break;


                case JTokenType.Array:

                    Builder.Append('[');

                    var firstElement = true;

                    foreach (var element in (JArray) JSON)
                    {

                        if (!firstElement)
                            Builder.Append(',');

                        firstElement = false;

                        Write(element, Builder);

                    }

                    Builder.Append(']');
                    break;


                case JTokenType.Property:
                    throw new ArgumentException("A JSON property is no JSON value!", nameof(JSON));


                case JTokenType.Null:
                case JTokenType.Undefined:
                    Builder.Append("null");
                    break;


                case JTokenType.Boolean:
                    Builder.Append(((JValue) JSON).Value<Boolean>() ? "true" : "false");
                    break;


                case JTokenType.String:
                    WriteString(((JValue) JSON).Value<String>() ?? "", Builder);
                    break;


                case JTokenType.Integer:
                case JTokenType.Float:
                    Builder.Append(Number(ToDouble((JValue) JSON)));
                    break;


                default:
                    // Date, Guid, Uri, TimeSpan, Bytes, Raw...: the string Newtonsoft writes for it.
                    var written = JSON.ToString(Formatting.None);
                    using (var reader = new JsonTextReader(new StringReader(written)) { DateParseHandling = DateParseHandling.None })
                        Write(JToken.Load(reader), Builder);
                    break;

            }

        }

        #endregion

        #region (private) ToDouble     (Value)

        private static Double ToDouble(JValue Value)

            => Value.Value switch {
                   Double     d  => d,
                   Single     f  => f,
                   Decimal    m  => (Double) m,
                   Int64      l  => l,
                   Int32      i  => i,
                   UInt64     ul => ul,
                   System.Numerics.BigInteger bi => (Double) bi,
                   IConvertible c => c.ToDouble(CultureInfo.InvariantCulture),
                   _              => throw new ArgumentException($"The JSON number '{Value}' cannot be canonicalized!")
               };

        #endregion

        #region (private) WriteString  (Text, Builder)

        private static void WriteString(String Text, StringBuilder Builder)
        {

            Builder.Append('"');

            foreach (var c in Text)
            {
                switch (c)
                {

                    case '"':  Builder.Append("\\\""); break;
                    case '\\': Builder.Append("\\\\"); break;
                    case '\b': Builder.Append("\\b");  break;
                    case '\f': Builder.Append("\\f");  break;
                    case '\n': Builder.Append("\\n");  break;
                    case '\r': Builder.Append("\\r");  break;
                    case '\t': Builder.Append("\\t");  break;

                    default:
                        if (c < 0x20)
                            Builder.Append("\\u").Append(((Int32) c).ToString("x4", CultureInfo.InvariantCulture));
                        else
                            Builder.Append(c);
                        break;

                }
            }

            Builder.Append('"');

        }

        #endregion

        #region Number             (Value)

        /// <summary>
        /// The given number as ECMAScript writes it (ECMA-262, Number::toString),
        /// which is how RFC 8785 writes every JSON number.
        /// </summary>
        /// <param name="Value">A finite number.</param>
        /// <exception cref="ArgumentException">The number is NaN or an infinity.</exception>
        public static String Number(Double Value)
        {

            if (Double.IsNaN(Value) || Double.IsInfinity(Value))
                throw new ArgumentException("JSON has no NaN and no infinity!", nameof(Value));

            if (Value == 0)
                return "0";  // also -0

            if (Value < 0)
                return "-" + Number(-Value);

            // The shortest digits that read back as the same double, and where the point goes.
            var roundTrip  = Value.ToString("R", CultureInfo.InvariantCulture);
            var exponent   = 0;
            var e          = roundTrip.IndexOfAny([ 'E', 'e' ]);

            if (e >= 0)
            {
                exponent   = Int32.Parse(roundTrip[(e + 1)..], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
                roundTrip  = roundTrip[..e];
            }

            var point      = roundTrip.IndexOf('.');
            var digits     = point >= 0 ? roundTrip.Remove(point, 1) : roundTrip;
            var intLength  = point >= 0 ? point                      : roundTrip.Length;

            // Value = 0.digits * 10^n
            var leading    = 0;
            while (leading < digits.Length - 1 && digits[leading] == '0')
                leading++;

            digits         = digits[leading..].TrimEnd('0');
            var n          = intLength - leading + exponent;
            var k          = digits.Length;

            if (k <= n && n <= 21)
                return digits + new String('0', n - k);

            if (0 < n && n <= 21)
                return digits[..n] + "." + digits[n..];

            if (-6 < n && n <= 0)
                return "0." + new String('0', -n) + digits;

            var exp = n - 1;
            var sign = exp < 0 ? "-" : "+";

            return k == 1
                       ? digits + "e" + sign + Math.Abs(exp).ToString(CultureInfo.InvariantCulture)
                       : digits[0] + "." + digits[1..] + "e" + sign + Math.Abs(exp).ToString(CultureInfo.InvariantCulture);

        }

        #endregion

    }

}
