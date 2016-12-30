﻿/*
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
    /// The result of a authorize start operation at a charging pool.
    /// </summary>
    public class AuthStartChargingPoolResult : AAuthStartResult<AuthStartChargingPoolResultType>
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new authorize start result.
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
        private AuthStartChargingPoolResult(IId                              AuthorizatorId,
                                            AuthStartChargingPoolResultType  Result,
                                            ChargingSession_Id?              SessionId              = null,
                                            Single?                          MaxkW                  = null,
                                            Single?                          MaxkWh                 = null,
                                            TimeSpan?                        MaxDuration            = null,
                                            IEnumerable<ChargingTariff>      ChargingTariffs        = null,
                                            IEnumerable<Auth_Token>          ListOfAuthStopTokens   = null,
                                            IEnumerable<UInt32>              ListOfAuthStopPINs     = null,

                                            eMobilityProvider_Id?            ProviderId             = null,
                                            String                           Description            = null,
                                            String                           AdditionalInfo         = null,
                                            TimeSpan?                        Runtime                = null)

            : base(AuthorizatorId,
                   Result,
                   SessionId,
                   MaxkW,
                   MaxkWh,
                   MaxDuration,
                   ChargingTariffs,
                   ListOfAuthStopTokens,
                   ListOfAuthStopPINs,

                   ProviderId,
                   Description,
                   AdditionalInfo,
                   Runtime)

        { }

        #endregion


        #region (static) Unspecified         (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static AuthStartChargingPoolResult Unspecified(IId                  AuthorizatorId,
                                                              ChargingSession_Id?  SessionId  = null,
                                                              TimeSpan?            Runtime    = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.Unspecified,
                                               SessionId,
                                               Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static AuthStartChargingPoolResult InvalidSessionId(IId                  AuthorizatorId,
                                                                   ChargingSession_Id?  SessionId  = null,
                                                                   TimeSpan?            Runtime    = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.InvalidSessionId,
                                               SessionId,
                                               Runtime: Runtime);

        #endregion

        #region (static) Reserved            (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The ChargingPool is reserved.
        /// </summary>
        public static AuthStartChargingPoolResult Reserved(IId                  AuthorizatorId,
                                                           ChargingSession_Id?  SessionId  = null,
                                                           TimeSpan?            Runtime    = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.Reserved,
                                               SessionId,
                                               Runtime: Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The ChargingPool or charging station is out of service.
        /// </summary>
        public static AuthStartChargingPoolResult OutOfService(IId                  AuthorizatorId,
                                                               ChargingSession_Id?  SessionId  = null,
                                                               TimeSpan?            Runtime    = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.OutOfService,
                                               SessionId,
                                               Runtime: Runtime);

        #endregion

        #region (static) Authorized          (AuthorizatorId, SessionId = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="MaxkW">The optional maximum allowed charging current.</param>
        /// <param name="MaxkWh">The optional maximum allowed charging energy.</param>
        /// <param name="MaxDuration">The optional maximum allowed charging duration.</param>
        /// <param name="ChargingTariffs">Optional charging tariff information.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        /// 
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Authorized(IId                          AuthorizatorId,
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


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   AuthStartChargingPoolResultType.Authorized,
                                                   SessionId,
                                                   MaxkW,
                                                   MaxkWh,
                                                   MaxDuration,
                                                   ChargingTariffs,
                                                   ListOfAuthStopTokens,
                                                   ListOfAuthStopPINs,

                                                   ProviderId,
                                                   Description,
                                                   AdditionalInfo,
                                                   Runtime);

        #endregion

        #region (static) NotAuthorized       (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            NotAuthorized(IId                    AuthorizatorId,
                          ChargingSession_Id?    SessionId        = null,
                          eMobilityProvider_Id?  ProviderId       = null,
                          String                 Description      = null,
                          String                 AdditionalInfo   = null,
                          TimeSpan?              Runtime          = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   AuthStartChargingPoolResultType.NotAuthorized,
                                                   SessionId,
                                                   ProviderId:      ProviderId,
                                                   Description:     Description,
                                                   AdditionalInfo:  AdditionalInfo,
                                                   Runtime:         Runtime);

        #endregion

        #region (static) Blocked             (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Blocked(IId                    AuthorizatorId,
                    ChargingSession_Id?    SessionId        = null,
                    eMobilityProvider_Id?  ProviderId       = null,
                    String                 Description      = null,
                    String                 AdditionalInfo   = null,
                    TimeSpan?              Runtime          = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   AuthStartChargingPoolResultType.Blocked,
                                                   SessionId,
                                                   ProviderId:      ProviderId,
                                                   Description:     Description,
                                                   AdditionalInfo:  AdditionalInfo,
                                                   Runtime:         Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult CommunicationTimeout(IId                  AuthorizatorId,
                                                                       ChargingSession_Id?  SessionId  = null,
                                                                       TimeSpan?            Runtime    = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.CommunicationTimeout,
                                               SessionId,
                                               Runtime: Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult StartChargingTimeout(IId                  AuthorizatorId,
                                                                       ChargingSession_Id?  SessionId  = null,
                                                                       TimeSpan?            Runtime    = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.StartChargingTimeout,
                                               SessionId,
                                               Runtime: Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, ErrorMessage = null, Runtime = null)

        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult Error(IId                  AuthorizatorId,
                                                        ChargingSession_Id?  SessionId     = null,
                                                        String               ErrorMessage  = null,
                                                        TimeSpan?            Runtime       = null)

            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               AuthStartChargingPoolResultType.Error,
                                               SessionId,
                                               Description:  ErrorMessage,
                                               Runtime:      Runtime);

        #endregion


    }


    /// <summary>
    /// The result of a authorize start operation at an EVSE.
    /// </summary>
    public enum AuthStartChargingPoolResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging station is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        Authorized,

        /// <summary>
        /// The authorize start was not successful (e.g. ev customer is unkown).
        /// </summary>
        NotAuthorized,

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        Blocked,

        /// <summary>
        /// The authorize start ran into a timeout between evse operator backend and the charging station.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The authorize start ran into a timeout between the charging station and the EV.
        /// </summary>
        StartChargingTimeout,

        /// <summary>
        /// The remote start operation led to an error.
        /// </summary>
        Error

    }

}