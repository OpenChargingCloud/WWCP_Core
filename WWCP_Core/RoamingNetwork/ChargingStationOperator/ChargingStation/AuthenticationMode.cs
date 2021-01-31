﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    public enum RFIDAuthenticationModes
    {

        MifareClassic,
        MifareDESFire,
        Calypso

    }

    public class AuthenticationModes : IEquatable<AuthenticationModes>,
                                       IComparable<AuthenticationModes>,
                                       IComparable
    {

        #region Properties

        #region Type

        protected readonly String _Type;

        public String Type
        {
            get
            {
                return _Type;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public AuthenticationModes(String Type)
        {

            this._Type = Type;

        }

        #endregion



        public static AuthenticationModes Unkown
        {
            get
            {
                return new Unkown();
            }
        }

        public static AuthenticationModes FreeCharging
        {
            get
            {
                return new FreeCharging();
            }
        }

        public static AuthenticationModes RFID(RFIDAuthenticationModes  RFIDAuthModes)
        {
            return new RFID(new RFIDAuthenticationModes[] { RFIDAuthModes }, new String[0]);
        }

        public static AuthenticationModes RFID(IEnumerable<RFIDAuthenticationModes>  RFIDAuthModes,
                                              IEnumerable<String>                   Brands)
        {
            return new RFID(RFIDAuthModes, Brands);
        }

        public static AuthenticationModes NFC
        {
            get
            {
                return new NFC();
            }
        }

        public static AuthenticationModes ISO15118_PLC
        {
            get
            {
                return new ISO15118_PLC();
            }
        }

        public static AuthenticationModes REMOTE
        {
            get
            {
                return new REMOTE();
            }
        }

        public static AuthenticationModes DirectPayment
        {
            get
            {
                return new DirectPayment();
            }
        }

        public static AuthenticationModes SMS(String  Number,
                                             String  StationCode = null)
        {
            return new SMS(Number, StationCode);
        }

        public static AuthenticationModes PhoneCall(String Number)
        {
            return new PhoneCall(Number);
        }





        #region IComparable<AuthenticationMode> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an authentication mode.
            var AuthenticationMode = Object as AuthenticationModes;
            if ((Object) AuthenticationMode == null)
                throw new ArgumentException("The given object is not an authentication mode!");

            return CompareTo(AuthenticationMode);

        }

        #endregion

        #region CompareTo(AuthenticationMode)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode to compare with.</param>
        public Int32 CompareTo(AuthenticationModes AuthenticationMode)
        {

            if ((Object) AuthenticationMode == null)
                throw new ArgumentNullException("The given authentication mode must not be null!");

            return _Type.CompareTo(AuthenticationMode._Type);

        }

        #endregion

        #endregion

        #region IEquatable<AuthenticationMode> Members

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

            // Check if the given object is an authentication mode.
            var AuthenticationMode = Object as AuthenticationModes;
            if ((Object) AuthenticationMode == null)
                return false;

            return this.Equals(AuthenticationMode);

        }

        #endregion

        #region Equals(AuthenticationMode)

        /// <summary>
        /// Compares two EVSEs for equality.
        /// </summary>
        /// <param name="AuthenticationMode">An authentication mode to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AuthenticationModes AuthenticationMode)
        {

            if ((Object) AuthenticationMode == null)
                return false;

            return _Type.Equals(AuthenticationMode._Type);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return _Type.GetHashCode();
        }

        #endregion


        public virtual JObject ToJSON()
        {
            return new JObject(new JProperty("Type", _Type));
        }

        public override String ToString()
        {
            return _Type;
        }

    }


    public class Unkown : AuthenticationModes
    {

        public Unkown()

            : base("Unkown")

        { }

    }

    public class FreeCharging : AuthenticationModes
    {

        public FreeCharging()

            : base("Free Charging")

        { }

    }

    public class RFID : AuthenticationModes
    {

        #region Properties

        #region Cards

        private readonly IEnumerable<RFIDAuthenticationModes> _Cards;

        public IEnumerable<RFIDAuthenticationModes> Cards
        {
            get
            {
                return _Cards;
            }
        }

        #endregion

        #region Brands

        private readonly IEnumerable<String> _Brands;

        public IEnumerable<String> Brands
        {
            get
            {
                return _Brands;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public RFID(IEnumerable<RFIDAuthenticationModes>  Cards,
                    IEnumerable<String>                   Brands)

            : base("RFID")

        {

            this._Cards   = Cards;
            this._Brands  = Brands;

        }

        #endregion

        public override JObject ToJSON()
        {
            return new JObject(new JProperty("Type",    _Type),
                               new JProperty("Cards",   new JArray(_Cards. Select(Card  => Card. ToString()))),
                               new JProperty("Brands",  new JArray(_Brands.Select(Brand => Brand.ToString()))));
        }

        public override String ToString()
        {
            return String.Concat("RFID: ", _Cards.AggregateWith(", "), " / ", _Brands.AggregateWith(", "));
        }

    }

    public class NFC : AuthenticationModes
    {

        public NFC()

            : base("NFC")

        { }

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

            : base("Credit Card")

        { }

    }

    public class PrepaidCard : AuthenticationModes
    {

        public PrepaidCard()

            : base("Prepaid Card")

        { }

    }

    public class LocalCurrency : AuthenticationModes
    {

        public LocalCurrency()

            : base("Local currency")

        { }

    }

    public class DirectPayment : AuthenticationModes
    {

        public DirectPayment()

            : base("Direct payment")

        { }

    }

    public class SMS : AuthenticationModes
    {

        #region Properties

        #region Number

        private readonly String _Number;

        public String Number
        {
            get
            {
                return _Number;
            }
        }

        #endregion

        #region StationCode

        private readonly String _StationCode;

        public String StationCode
        {
            get
            {
                return _StationCode;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public SMS(String  Number,
                   String  StationCode = null)

            : base("SMS")

        {

            #region Initial checks

            if (Number.IsNullOrEmpty())
                throw new ArgumentNullException("Number", "The given SMS telephone number must not be null or empty!");

            #endregion

            this._Number       = Number;
            this._StationCode  = StationCode != null ? StationCode : String.Empty;

        }

        #endregion


        public override JObject ToJSON()
        {
            return new JObject(new JProperty("Type",        _Type),
                               new JProperty("Number",      _Number),
                               new JProperty("StationCode", _StationCode));
        }

        public override String ToString()
        {
            return String.Concat("SMS: ", _Number, ", ", _StationCode);
        }

    }

    public class PhoneCall : AuthenticationModes
    {

        #region Properties

        #region Number

        private readonly String _Number;

        public String Number
        {
            get
            {
                return _Number;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public PhoneCall(String Number)

            : base("PhoneCall")

        {

            this._Number = Number;

        }

        #endregion


        public override JObject ToJSON()
        {
            return new JObject(new JProperty("Type",    _Type),
                               new JProperty("Number",  _Number));
        }

        public override String ToString()
        {
            return String.Concat("Phone call: ", _Number);
        }

    }

}
