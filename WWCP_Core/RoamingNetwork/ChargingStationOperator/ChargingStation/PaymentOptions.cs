﻿/*
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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this PaymentOptions)

        public static JArray? ToJSON(this IEnumerable<PaymentOptions> PaymentOptions)

            => PaymentOptions is not null
                   ? new JArray(PaymentOptions.Select(paymentOption => paymentOption.ToString()))
                   : null;

        #endregion

    }


    public enum PaymentOptions
    {

        Unspecified,
        Free,
        Direct,         // e.g. Cash, Card, SMS
        SMS,
        Cash,
        CreditCard,
        Contract

    }

}
