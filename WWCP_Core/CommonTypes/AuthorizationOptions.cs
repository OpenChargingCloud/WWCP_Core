/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP
{

    public enum AuthorizationOptions
    {

        [FreeCharge]
        UnlimitedAccess             =    0,

        Private                     =    1,


        [ChargeWithContract]
        RFIDMifareClassic           =    2,

        [ChargeWithContract]
        RFIDMifareDesfire           =    4,

        [ChargeWithContract]
        RFIDCalypso                 =    8,

        [ChargeWithContract]
        PINPAD                      =   16,

        [ChargeWithContract]
        Apps                        =   32,

        [ChargeWithContract]
        PhoneActiveRFIDChip         =   64,

        [ChargeWithContract]
        IEC15118PLC                 =  128,

        [ChargeWithContract]
        IEC15118OverTheAir          =  256,


        [ChargeWithoutContract]
        PhoneDialogWithPlatform     =  512,

        [ChargeWithoutContract]
        PhoneSMS                    = 1024,

        [ChargeWithoutContract]
        CreditCard                  = 2048,

        [ChargeWithoutContract]
        LocalCurrencyCoin           = 4096,

        [ChargeWithoutContract]
        PrePaidCard                 = 8192

    }

}
