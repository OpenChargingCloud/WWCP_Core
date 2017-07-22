/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region (private) AuthStartChargingPoolResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
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
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStartChargingPoolResult(IId                              AuthorizatorId,
                                            ISendAuthorizeStartStop          ISendAuthorizeStartStop,
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
                                            Byte                             NumberOfRetries        = 0,
                                            TimeSpan?                        Runtime                = null)

            : base(AuthorizatorId,
                   ISendAuthorizeStartStop,
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
                   NumberOfRetries,
                   Runtime)

        { }

        #endregion

        #region (private) AuthStartChargingPoolResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
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
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStartChargingPoolResult(IId                              AuthorizatorId,
                                            IReceiveAuthorizeStartStop       IReceiveAuthorizeStartStop,
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
                                            Byte                             NumberOfRetries        = 0,
                                            TimeSpan?                        Runtime                = null)

            : base(AuthorizatorId,
                   IReceiveAuthorizeStartStop,
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
                   NumberOfRetries,
                   Runtime)

        { }

        #endregion

        #endregion


        #region (static) Unspecified         (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        ChargingSession_Id?      SessionId   = null,
                        TimeSpan?                Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Unspecified,
                                                   SessionId,
                                                   Runtime: Runtime);




        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        ChargingSession_Id?         SessionId   = null,
                        TimeSpan?                   Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Unspecified,
                                                   SessionId,
                                                   Runtime: Runtime);

        #endregion

        #region (static) AdminDown           (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      ChargingSession_Id?      SessionId   = null,
                      TimeSpan?                Runtime     = null)


            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               ISendAuthorizeStartStop,
                                               AuthStartChargingPoolResultType.AdminDown,
                                               SessionId,
                                               Description: "The authentication service was disabled by the administrator!",
                                               Runtime:     Runtime);



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      ChargingSession_Id?         SessionId   = null,
                      TimeSpan?                   Runtime     = null)


            => new AuthStartChargingPoolResult(AuthorizatorId,
                                               IReceiveAuthorizeStartStop,
                                               AuthStartChargingPoolResultType.AdminDown,
                                               SessionId,
                                               Description: "The authentication service was disabled by the administrator!",
                                               Runtime:     Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id?      SessionId   = null,
                             TimeSpan?                Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.InvalidSessionId,
                                                   SessionId,
                                                   Runtime: Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id?         SessionId   = null,
                             TimeSpan?                   Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.InvalidSessionId,
                                                   SessionId,
                                                   Runtime: Runtime);

        #endregion

        #region (static) Reserved            (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The charging pool is reserved.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Reserved(IId                      AuthorizatorId,
                     ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                     ChargingSession_Id?      SessionId   = null,
                     TimeSpan?                Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Reserved,
                                                   SessionId,
                                                   Runtime: Runtime);



        /// <summary>
        /// The charging pool is reserved.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Reserved(IId                         AuthorizatorId,
                     IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                     ChargingSession_Id?         SessionId   = null,
                     TimeSpan?                   Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Reserved,
                                                   SessionId,
                                                   Runtime: Runtime);

        #endregion

        #region (static) NotSupported        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            NotSupported(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId   = null,
                         TimeSpan?                Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.NotSupported,
                                                   SessionId,
                                                   Description: "Operation not supported!",
                                                   Runtime:     Runtime);



        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            NotSupported(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId   = null,
                         TimeSpan?                   Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.NotSupported,
                                                   SessionId,
                                                   Description: "Operation not supported!",
                                                   Runtime:     Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The ChargingPool or charging pool is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId   = null,
                         TimeSpan?                Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.OutOfService,
                                                   SessionId,
                                                   Description: "Out-of-service!",
                                                   Runtime:     Runtime);



        /// <summary>
        /// The ChargingPool or charging pool is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId   = null,
                         TimeSpan?                   Runtime     = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.OutOfService,
                                                   SessionId,
                                                   Description: "Out-of-service!",
                                                   Runtime:     Runtime);

        #endregion

        #region (static) Authorized          (AuthorizatorId, SessionId = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
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
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Authorized(IId                          AuthorizatorId,
                       ISendAuthorizeStartStop      ISendAuthorizeStartStop,
                       ChargingSession_Id?          SessionId              = null,
                       Single?                      MaxkW                  = null,
                       Single?                      MaxkWh                 = null,
                       TimeSpan?                    MaxDuration            = null,
                       IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                       IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                       IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                       eMobilityProvider_Id?        ProviderId             = null,
                       String                       Description            = "Success",
                       String                       AdditionalInfo         = null,
                       Byte                         NumberOfRetries        = 0,
                       TimeSpan?                    Runtime                = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
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
                                                   NumberOfRetries,
                                                   Runtime);



        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
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
        /// <param name="NumberOfRetries">Number of transmission retries.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Authorized(IId                          AuthorizatorId,
                       IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop,
                       ChargingSession_Id?          SessionId              = null,
                       Single?                      MaxkW                  = null,
                       Single?                      MaxkWh                 = null,
                       TimeSpan?                    MaxDuration            = null,
                       IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                       IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                       IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                       eMobilityProvider_Id?        ProviderId             = null,
                       String                       Description            = "Success",
                       String                       AdditionalInfo         = null,
                       Byte                         NumberOfRetries        = 0,
                       TimeSpan?                    Runtime                = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
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
                                                   NumberOfRetries,
                                                   Runtime);

        #endregion

        #region (static) NotAuthorized       (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          ChargingSession_Id?      SessionId        = null,
                          eMobilityProvider_Id?    ProviderId       = null,
                          String                   Description      = "NotAuthorized",
                          String                   AdditionalInfo   = null,
                          TimeSpan?                Runtime          = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.NotAuthorized,
                                                   SessionId,
                                                   ProviderId:      ProviderId,
                                                   Description:     Description,
                                                   AdditionalInfo:  AdditionalInfo,
                                                   Runtime:         Runtime);



        /// <summary>
        /// The authorize start was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          ChargingSession_Id?         SessionId        = null,
                          eMobilityProvider_Id?       ProviderId       = null,
                          String                      Description      = "NotAuthorized",
                          String                      AdditionalInfo   = null,
                          TimeSpan?                   Runtime          = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
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
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    ChargingSession_Id?      SessionId        = null,
                    eMobilityProvider_Id?    ProviderId       = null,
                    String                   Description      = null,
                    String                   AdditionalInfo   = null,
                    TimeSpan?                Runtime          = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Blocked,
                                                   SessionId,
                                                   ProviderId:      ProviderId,
                                                   Description:     Description,
                                                   AdditionalInfo:  AdditionalInfo,
                                                   Runtime:         Runtime);



        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    ChargingSession_Id?         SessionId        = null,
                    eMobilityProvider_Id?       ProviderId       = null,
                    String                      Description      = null,
                    String                      AdditionalInfo   = null,
                    TimeSpan?                   Runtime          = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Blocked,
                                                   SessionId,
                                                   ProviderId:      ProviderId,
                                                   Description:     Description,
                                                   AdditionalInfo:  AdditionalInfo,
                                                   Runtime:         Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging pool.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.CommunicationTimeout,
                                                   SessionId,
                                                   Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging pool.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.CommunicationTimeout,
                                                   SessionId,
                                                   Runtime: Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging pool and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            StartChargingTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId  = null,
                                 TimeSpan?                Runtime    = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.StartChargingTimeout,
                                                   SessionId,
                                                   Runtime: Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging pool and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            StartChargingTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId  = null,
                                 TimeSpan?                   Runtime    = null)


                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.StartChargingTimeout,
                                                   SessionId,
                                                   Runtime: Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, ErrorMessage = null, Runtime = null)

        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  ChargingSession_Id?      SessionId     = null,
                  String                   ErrorMessage  = null,
                  TimeSpan?                Runtime       = null)

                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   ISendAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Error,
                                                   SessionId,
                                                   Description:  ErrorMessage,
                                                   Runtime:      Runtime);



        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartChargingPoolResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  ChargingSession_Id?         SessionId     = null,
                  String                      ErrorMessage  = null,
                  TimeSpan?                   Runtime       = null)

                => new AuthStartChargingPoolResult(AuthorizatorId,
                                                   IReceiveAuthorizeStartStop,
                                                   AuthStartChargingPoolResultType.Error,
                                                   SessionId,
                                                   Description:  ErrorMessage,
                                                   Runtime:      Runtime);

        #endregion


    }


    /// <summary>
    /// The result of a authorize start operation at a charging pool.
    /// </summary>
    public enum AuthStartChargingPoolResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        AdminDown,

        /// <summary>
        /// The charging pool is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging pool is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        NotSupported,

        /// <summary>
        /// The charging pool is out of service.
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
        /// The authorize start ran into a timeout between evse operator backend and the charging pool.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The authorize start ran into a timeout between the charging pool and the EV.
        /// </summary>
        StartChargingTimeout,

        /// <summary>
        /// The remote start operation led to an error.
        /// </summary>
        Error

    }

}
