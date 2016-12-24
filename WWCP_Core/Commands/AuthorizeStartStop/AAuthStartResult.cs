/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The abstract result of an authorize start operation.
    /// </summary>
    public abstract class AAuthStartResult<T> : IAAuthStartResult<T>
        where T : struct
    {

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public IId                          AuthorizatorId          { get; }

        /// <summary>
        /// The result of the authorize start operation.
        /// </summary>
        public T                            Result                  { get; }

        /// <summary>
        /// The optional charging session identification, when the authorize start operation was successful.
        /// </summary>
        public ChargingSession_Id?          SessionId               { get; }

        /// <summary>
        /// The optional maximum allowed charging current.
        /// </summary>
        public Single?                      MaxkW                   { get;}

        /// <summary>
        /// The optional maximum allowed charging energy.
        /// </summary>
        public Single?                      MaxkWh                  { get;}

        /// <summary>
        /// The optional maximum allowed charging duration.
        /// </summary>
        public TimeSpan?                    MaxDuration             { get; }

        /// <summary>
        /// Optional charging tariff information.
        /// </summary>
        public IEnumerable<ChargingTariff>  ChargingTariffs       { get; }

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        public IEnumerable<Auth_Token>      ListOfAuthStopTokens    { get; }

        /// <summary>
        /// An optional list of authorize stop PINs.
        /// </summary>
        public IEnumerable<UInt32>          ListOfAuthStopPINs      { get; }


        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public eMobilityProvider_Id?        ProviderId              { get; }

        /// <summary>
        /// A optional description of the authorize start result, e.g. in case of an error.
        /// </summary>
        public String                       Description             { get; }

        /// <summary>
        /// An optional additional message, e.g. in case of an error.
        /// </summary>
        public String                       AdditionalInfo          { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                    Runtime                 { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="MaxkW">The optional maximum allowed charging current.</param>
        /// <param name="MaxkWh">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        protected AAuthStartResult(IId                          AuthorizatorId,
                                   T                            Result,
                                   ChargingSession_Id?          SessionId              = null,
                                   Single?                      MaxkW                  = null,
                                   Single?                      MaxkWh                 = null,
                                   TimeSpan?                    MaxDuration            = null,
                                   IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                                   IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                                   IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                                   eMobilityProvider_Id?        ProviderId             = null,
                                   String                       Description            = null,
                                   String                       AdditionalInfo         = null,
                                   TimeSpan?                    Runtime                = null)

        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");

            #endregion

            this.AuthorizatorId        = AuthorizatorId;
            this.Result                = Result;
            this.SessionId             = SessionId;
            this.MaxkW                 = MaxkW;
            this.MaxkWh                = MaxkWh;
            this.MaxDuration           = MaxDuration;
            this.ChargingTariffs       = ChargingTariffs;
            this.ListOfAuthStopTokens  = ListOfAuthStopTokens;
            this.ListOfAuthStopPINs    = ListOfAuthStopPINs;

            this.ProviderId            = ProviderId     ?? new eMobilityProvider_Id?();
            this.Description           = Description;
            this.AdditionalInfo        = AdditionalInfo;
            this.Runtime               = Runtime        ?? TimeSpan.FromSeconds(0);

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (ProviderId != null)
                return String.Concat(Result.ToString(), ", ", ProviderId);

            return String.Concat(Result.ToString());

        }

        #endregion

    }

}
