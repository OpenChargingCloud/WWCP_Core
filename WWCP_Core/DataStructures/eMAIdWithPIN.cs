/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OICP <https://github.com/OpenChargingCloud/WWCP_OICP>
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

    public readonly struct eMAIdWithPIN2 : IEquatable<eMAIdWithPIN2>,
                                           IComparable<eMAIdWithPIN2>,
                                           IComparable
    {

        #region Properties

        public EMobilityAccount_Id  eMAId           { get; }

        public String               PIN             { get; }

        public PINCrypto            Function        { get; }

        public String               Salt            { get; }

        #endregion

        #region Constructor(s)

        #region eMAIdWithPIN(eMAId, PIN)

        public eMAIdWithPIN2(EMobilityAccount_Id  eMAId,
                             String               PIN)
        {

            this.eMAId     = eMAId;
            this.PIN       = PIN?.Trim() ?? "";
            this.Function  = PINCrypto.None;
            this.Salt      = "";

        }

        #endregion

        #region eMAIdWithPIN(eMAId, PIN, Function, Salt = "")

        public eMAIdWithPIN2(EMobilityAccount_Id  eMAId,
                             String               PIN,
                             PINCrypto            Function,
                             String               Salt   = "")
        {

            this.eMAId     = eMAId;
            this.PIN       = PIN?. Trim() ?? "";
            this.Function  = Function;
            this.Salt      = Salt?.Trim() ?? "";

        }

        #endregion

        #endregion


        #region ToJSON(CustomEMAIdWithPIN2Serializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEMAIdWithPIN2Serializer">A delegate to serialize custom eMAIdWithPIN JSON objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<eMAIdWithPIN2>? CustomEMAIdWithPIN2Serializer = null)
        {

            var json = JSONObject.Create(

                           new JProperty("eMAId", eMAId.ToString()),

                           PIN.IsNotNullOrEmpty()

                               ? Function == PINCrypto.None

                                     ? new JProperty("PIN", PIN)

                                     : new JProperty("hashedPIN", JSONObject.Create(
                                             new JProperty("value",     PIN),
                                             new JProperty("function",  Function.AsString()),
                                             new JProperty("salt",      Salt)
                                       ))

                               : null

                       );

            return CustomEMAIdWithPIN2Serializer is not null
                       ? CustomEMAIdWithPIN2Serializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two eMAId with PINs for equality.
        /// </summary>
        /// <param name="eMAIdWithPIN21">An eMAId with PIN.</param>
        /// <param name="eMAIdWithPIN22">Another eMAId with PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (eMAIdWithPIN2 eMAIdWithPIN21,
                                           eMAIdWithPIN2 eMAIdWithPIN22)

            => eMAIdWithPIN21.Equals(eMAIdWithPIN22);

        #endregion

        #region Operator != (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two eMAId with PINs for inequality.
        /// </summary>
        /// <param name="eMAIdWithPIN21">An eMAId with PIN.</param>
        /// <param name="eMAIdWithPIN22">Another eMAId with PIN.</param>
            /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (eMAIdWithPIN2 eMAIdWithPIN21,
                                           eMAIdWithPIN2 eMAIdWithPIN22)

            => !eMAIdWithPIN21.Equals(eMAIdWithPIN22);

        #endregion

        #region Operator <  (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">An eMAId with PIN.</param>
        /// <param name="eMAIdWithPIN22">Another eMAId with PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator < (eMAIdWithPIN2 eMAIdWithPIN21,
                                          eMAIdWithPIN2 eMAIdWithPIN22)

            => eMAIdWithPIN21.CompareTo(eMAIdWithPIN22) < 0;

        #endregion

        #region Operator <= (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">An eMAId with PIN.</param>
        /// <param name="eMAIdWithPIN22">Another eMAId with PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator <= (eMAIdWithPIN2 eMAIdWithPIN21,
                                           eMAIdWithPIN2 eMAIdWithPIN22)

            => eMAIdWithPIN21.CompareTo(eMAIdWithPIN22) <= 0;

        #endregion

        #region Operator >  (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">An eMAId with PIN.</param>
        /// <param name="eMAIdWithPIN22">Another eMAId with PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator > (eMAIdWithPIN2 eMAIdWithPIN21,
                                          eMAIdWithPIN2 eMAIdWithPIN22)

            => eMAIdWithPIN21.CompareTo(eMAIdWithPIN22) > 0;

        #endregion

        #region Operator >= (eMAIdWithPIN21, eMAIdWithPIN22)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="eMAIdWithPIN21">An eMAId with PIN.</param>
        /// <param name="eMAIdWithPIN22">Another eMAId with PIN.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator >= (eMAIdWithPIN2 eMAIdWithPIN21,
                                           eMAIdWithPIN2 eMAIdWithPIN22)

            => eMAIdWithPIN21.CompareTo(eMAIdWithPIN22) >= 0;

        #endregion

        #endregion

        #region IComparable<eMAIdWithPIN2> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two eMAIds with (hashed) pins.
        /// </summary>
        /// <param name="Object">An eMAId with (hashed) pin to compare with.</param>
        public Int32 CompareTo(Object? Object)

            => Object is eMAIdWithPIN2 eMAIdWithPIN2
                   ? CompareTo(eMAIdWithPIN2)
                   : throw new ArgumentException("The given object is not an eMAId with PIN!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(eMAIdWithPIN2)

        /// <summary>
        /// Compares two eMAIds with (hashed) pins.
        /// </summary>
        /// <param name="eMAIdWithPIN2">An eMAId with (hashed) pin to compare with.</param>
        public Int32 CompareTo(eMAIdWithPIN2 eMAIdWithPIN2)
        {

            var result = eMAId.   CompareTo(eMAIdWithPIN2.eMAId);

            if (result == 0)
                result = PIN.     CompareTo(eMAIdWithPIN2.PIN);

            if (result == 0)
                result = Function.CompareTo(eMAIdWithPIN2.Function);

            if (result == 0)
                result = Salt.    CompareTo(eMAIdWithPIN2.Salt);

            return result;

        }

        #endregion

        #endregion

        #region IEquatable<eMAIdWithPIN2> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two eMAIds with (hashed) pins for equality.
        /// </summary>
        /// <param name="Object">An eMAId with (hashed) pin to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is eMAIdWithPIN2 eMAIdWithPIN2 &&
                   Equals(eMAIdWithPIN2);

        #endregion

        #region Equals(eMAIdWithPIN2)

        /// <summary>
        /// Compares two eMAIds with (hashed) pins for equality.
        /// </summary>
        /// <param name="eMAIdWithPIN2">An eMAId with (hashed) pin to compare with.</param>
        public Boolean Equals(eMAIdWithPIN2 eMAIdWithPIN2)

            => eMAId.   Equals(eMAIdWithPIN2.eMAId)    &&
               PIN.     Equals(eMAIdWithPIN2.PIN)      &&
               Function.Equals(eMAIdWithPIN2.Function) &&
               Salt.    Equals(eMAIdWithPIN2.Salt);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                return eMAId.   GetHashCode() * 7 ^
                       PIN.     GetHashCode() * 5 ^
                       Function.GetHashCode() * 3 ^
                       Salt.    GetHashCode();
            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   eMAId.ToString(),

                   Function is not PINCrypto.None

                       ? String.Concat(

                             $" -{Function.AsString()}-> {PIN}",

                             Salt.IsNotNullOrEmpty()
                                 ? " (" + Salt + ")"
                                 : ""

                         )

                       : PIN

               );

        #endregion

    }

}
