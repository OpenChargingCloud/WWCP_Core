/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The interface of a result of an authorize start operation.
    /// </summary>
    public interface IAAuthStartResult
    {

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        IId                          AuthorizatorId          { get; }

        /// <summary>
        /// The optional charging session identification, when the authorize start operation was successful.
        /// </summary>
        ChargingSession_Id?          SessionId               { get; }

        /// <summary>
        /// The optional maximum allowed charging current.
        /// </summary>
        Single?                      MaxkW                   { get;}

        /// <summary>
        /// The optional maximum allowed charging energy.
        /// </summary>
        Single?                      MaxkWh                  { get;}

        /// <summary>
        /// The optional maximum allowed charging duration.
        /// </summary>
        TimeSpan?                    MaxDuration             { get; }

        /// <summary>
        /// Optional charging tariff information.
        /// </summary>
        IEnumerable<ChargingTariff>  ChargingTariffs       { get; }

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        IEnumerable<Auth_Token>      ListOfAuthStopTokens    { get; }

        /// <summary>
        /// An optional list of authorize stop PINs.
        /// </summary>
        IEnumerable<UInt32>          ListOfAuthStopPINs      { get; }


        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        eMobilityProvider_Id?        ProviderId              { get; }

        /// <summary>
        /// A optional description of the authorize start result, e.g. in case of an error.
        /// </summary>
        String                       Description             { get; }

        /// <summary>
        /// An optional additional message, e.g. in case of an error.
        /// </summary>
        String                       AdditionalInfo          { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        TimeSpan?                    Runtime                 { get; }

        String ToString();

    }

    /// <summary>
    /// The interface of a result of an authorize start operation.
    /// </summary>
    public interface IAAuthStartResult<T> : IAAuthStartResult
        where T : struct
    {


        /// <summary>
        /// The result of the authorize start operation.
        /// </summary>
        T                            Result                  { get; }

    }


}
