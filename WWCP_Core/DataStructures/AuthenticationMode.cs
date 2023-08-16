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

    public enum RFIDCardTypes
    {

        MifareClassic,
        MifareDESFire,
        Calypso

    }


    /// <summary>
    /// Extension methods for authentication modes.
    /// </summary>
    public static class AuthenticationModesExtensions
    {

        #region ToJSON(this AuthenticationModes)

        public static JArray? ToJSON(this IEnumerable<AuthenticationModes> AuthenticationModes)

            => AuthenticationModes is not null
                   ? new JArray(AuthenticationModes.SafeSelect(authenticationMode => authenticationMode.ToJSON()))
                   : null;

        #endregion

    }


    public class AuthenticationModes : IEquatable<AuthenticationModes>,
                                       IComparable<AuthenticationModes>,
                                       IComparable
    {

        public class FreeCharging : AuthenticationModes
        {

            public FreeCharging()

                : base("Free Charging")

            { }

        }

        public class RFID : AuthenticationModes
        {

            #region Properties

            public IEnumerable<RFIDCardTypes>  CardTypes    { get; }

            public IEnumerable<Brand_Id>       BrandIds     { get; }

            #endregion

            #region Constructor(s)

            public RFID(RFIDCardTypes  CardType)

                : base("RFID")

            {

                this.CardTypes  = CardTypes ?? new RFIDCardTypes[] { CardType };
                this.BrandIds   = BrandIds  ?? new Brand_Id[0];

            }

            public RFID(IEnumerable<RFIDCardTypes>  CardTypes,
                        IEnumerable<Brand_Id>       BrandIds = null)

                : base("RFID")

            {

                this.CardTypes  = CardTypes ?? new RFIDCardTypes[0];
                this.BrandIds   = BrandIds  ?? new Brand_Id[0];

            }

            #endregion

            public override JObject ToJSON()

                => new (
                       new JProperty("type",       Type),
                       new JProperty("cardTypes",  new JArray(CardTypes.Select(cardType => cardType.ToString()))),
                       new JProperty("brandIds",   new JArray(BrandIds. Select(brandId  => brandId. ToString())))
                   );

            public override String ToString()

                => String.Concat("RFID: ",
                                 CardTypes.AggregateWith(", "),
                                 " / ",
                                 BrandIds. AggregateWith(", "));

        }

        public class PINPAD : AuthenticationModes
        {

            public PINPAD()

                : base("PINPAD")

            { }

        }

        public class ISO15118_PLC : AuthenticationModes
        {

            public ISO15118_PLC()    // ISO/IEC 15118 PLC

                : base("ISO/IEC 15118 PLC")

            { }

        }

        public class ISO15118_Air : AuthenticationModes
        {

            public ISO15118_Air()    // ISO/IEC 15118 Over-the-Air

                : base("ISO/IEC 15118 Over-the-Air")

            { }

        }

        public class REMOTE : AuthenticationModes
        {

            public REMOTE()

                : base("REMOTE")  // App, QR-Code, Phone

            { }

        }

        public class CreditCard : AuthenticationModes
        {

            public CreditCard()

                : base("CreditCard")

            { }

        }

        public class DebitCard : AuthenticationModes
        {

            public DebitCard()

                : base("DebitCard")

            { }

        }

        public class PrepaidCard : AuthenticationModes
        {

            public PrepaidCard()

                : base("PrepaidCard")

            { }

        }

        public class NFC : AuthenticationModes
        {

            public NFC()

                : base("NFC")

            { }

        }

        public class Bluetooth : AuthenticationModes
        {

            public Bluetooth()

                : base("Bluetooth")

            { }

        }


        public class WLAN : AuthenticationModes
        {

            public WLAN()

                : base("WLAN")

            { }

        }

        public class NoAuthenticationRequired : AuthenticationModes
        {

            public NoAuthenticationRequired()

                : base("No authentication required")

            { }

        }


        /// <summary>
        /// Only for OICP compatibility! Do not use!
        /// </summary>
        [Obsolete]
        public class DirectPayment : AuthenticationModes
        {

            public DirectPayment()

                : base("DirectPayment")

            { }

        }


        public class SMS : AuthenticationModes
        {

            #region Properties

            public String  Number         { get; }

            public String  StationCode    { get; }

            #endregion

            #region Constructor(s)

            public SMS(String  Number,
                       String  StationCode = null)

                : base("SMS")

            {

                if (Number.IsNullOrEmpty())
                    throw new ArgumentNullException("Number", "The given SMS telephone number must not be null or empty!");

                this.Number       = Number;
                this.StationCode  = StationCode;

            }

            #endregion


            public override JObject ToJSON()

                => JSONObject.Create(

                       new JProperty("type",    Type),
                       new JProperty("Number",  Number),

                       StationCode is not null && StationCode.IsNotNullOrEmpty()
                           ? new JProperty("StationCode",  StationCode)
                           : null

                   );

            public override String ToString()

                => String.Concat("SMS: ", Number,
                                 StationCode.IsNotNullOrEmpty()
                                     ? ", " + StationCode
                                     : "");

        }

        public class PhoneCall : AuthenticationModes
        {

            #region Properties

            public String  Number    { get; }

            #endregion

            #region Constructor(s)

            public PhoneCall(String Number)

                : base("PhoneCall")

            {

                this.Number = Number;

            }

            #endregion


            public override JObject ToJSON()

                => new JObject(
                       new JProperty("type",    Type),
                       new JProperty("Number",  Number)
                   );

            public override String ToString()

                => String.Concat("Phone call: ", Number);

        }



        #region Properties

        public String  Type    { get; }

        #endregion

        #region Constructor(s)

        public AuthenticationModes(String Type)
        {
            this.Type = Type;
        }

        #endregion



        //public static AuthenticationModes FreeCharging
        //    => new FreeCharging();

        //public static AuthenticationModes RFID(RFIDCardTypes CardType)

        //    => new RFID(
        //           new RFIDCardTypes[] {
        //               CardType
        //           },
        //           new Brand_Id[0]
        //       );

        //public static AuthenticationModes RFID(IEnumerable<RFIDCardTypes>  CardTypes,
        //                                       IEnumerable<Brand_Id>       BrandIds = null)

        //    => new RFID(CardTypes,
        //                BrandIds);

        //public static AuthenticationModes NFC
        //    => new NFC();

        //public static AuthenticationModes ISO15118_PLC
        //    => new ISO15118_PLC();

        //public static AuthenticationModes REMOTE
        //    => new REMOTE();

        //public static AuthenticationModes DirectPayment
        //    => new DirectPayment();

        //public static AuthenticationModes NoAuthenticationRequired
        //    => new NoAuthenticationRequired();


        //public static AuthenticationModes SMS(String  Number,
        //                                      String  StationCode = null)
        //{
        //    return new SMS(Number, StationCode);
        //}

        //public static AuthenticationModes PhoneCall(String Number)
        //{
        //    return new PhoneCall(Number);
        //}



        public virtual JObject ToJSON()
            => new JObject(new JProperty("type", Type));


        #region IComparable<AuthenticationModes> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is AuthenticationModes authenticationModes
                   ? CompareTo(authenticationModes)
                   : throw new ArgumentException("The given object is not an authentication mode!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(AuthenticationMode)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode to compare with.</param>
        public Int32 CompareTo(AuthenticationModes AuthenticationMode)
        {

            if (AuthenticationMode is null)
                throw new ArgumentNullException("The given authentication mode must not be null!");

            return Type.CompareTo(AuthenticationMode.Type);

        }

        #endregion

        #endregion

        #region IEquatable<AuthenticationModes> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is AuthenticationModes authenticationMode &&
                   Equals(authenticationMode);

        #endregion

        #region Equals(AuthenticationMode)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AuthenticationModes AuthenticationMode)

            => !(AuthenticationMode is null) &&

                 String.Equals(Type, AuthenticationMode.Type, StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => Type.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => Type;

        #endregion

    }


    //public class Unkown : AuthenticationModes
    //{

    //    public Unkown()

    //        : base("Unkown")

    //    { }

    //}

    

}
