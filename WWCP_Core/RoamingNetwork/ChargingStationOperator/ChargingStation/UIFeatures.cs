/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// User interface features, e.g. of a charging station.
    /// </summary>
    [Flags]
    public enum UIFeatures : long
    {

        /// <summary>
        /// Undefined interface features.
        /// </summary>
        Undefined           =  0,

        /// <summary>
        /// There is a (large) screen.
        /// </summary>
        Screen              =  1,

        /// <summary>
        /// The is a keypad, e.g. for entering a PIN.
        /// </summary>
        Pinpad              =  2,

        /// <summary>
        /// Audio feedback is available.
        /// </summary>
        Sound               =  4,

        /// <summary>
        /// Voice control supported.
        /// </summary>
        SpeechRecognition   =  8,

        /// <summary>
        /// RFID cards will be accepted.
        /// </summary>
        RFID                = 16,

        /// <summary>
        /// NFC control supported.
        /// </summary>
        NFC                 = 32,

        /// <summary>
        /// Bluetooth control supported.
        /// </summary>
        Bluetooth           = 64,

        /// <summary>
        /// Bluetooth low-energy beacons supported.
        /// </summary>
        BLEBeacons          = 128,

        /// <summary>
        /// WLAN control supported.
        /// </summary>
        WLAN                = 256

    }

}
