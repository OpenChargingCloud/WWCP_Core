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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a authorize stop operation at a charging pool.
    /// </summary>
    public class AuthStopChargingPoolResult : AAuthStopResult<AuthStopChargingPoolResultType>
    {

        #region Constructor(s)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize stop result type.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// 
        /// <param name="ProviderId">An optional identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private AuthStopChargingPoolResult(IId                                AuthorizatorId,
                                              AuthStopChargingPoolResultType  Result,
                                              ChargingSession_Id?                SessionId        = null,

                                              eMobilityProvider_Id?              ProviderId       = null,
                                              String                             Description      = null,
                                              String                             AdditionalInfo   = null,
                                              TimeSpan?                          Runtime          = null)

            : base(AuthorizatorId,
                   Result,
                   SessionId,
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
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult Unspecified(IId                  AuthorizatorId,
                                                             ChargingSession_Id?  SessionId  = null,
                                                             TimeSpan?            Runtime    = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.Unspecified,
                                              SessionId,
                                              Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult InvalidSessionId(IId                  AuthorizatorId,
                                                                  ChargingSession_Id?  SessionId  = null,
                                                                  TimeSpan?            Runtime    = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.InvalidSessionId,
                                              SessionId,
                                              Runtime: Runtime);

        #endregion

        #region (static) NotSupported        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        public static AuthStopChargingPoolResult NotSupported(IId                  AuthorizatorId,
                                                              ChargingSession_Id?  SessionId  = null,
                                                              TimeSpan?            Runtime    = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.NotSupported,
                                              SessionId,
                                              Description: "Operation not supported!",
                                              Runtime:     Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult OutOfService(IId                  AuthorizatorId,
                                                              ChargingSession_Id?  SessionId  = null,
                                                              TimeSpan?            Runtime    = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.OutOfService,
                                              SessionId,
                                              Description: "Out-of-service!",
                                              Runtime:     Runtime);

        #endregion

        #region (static) Authorized          (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult Authorized(IId                    AuthorizatorId,
                                                            ChargingSession_Id?    SessionId        = null,
                                                            eMobilityProvider_Id?  ProviderId       = null,
                                                            String                 Description      = "Success",
                                                            String                 AdditionalInfo   = null,
                                                            TimeSpan?              Runtime          = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.Authorized,
                                              SessionId,
                                              ProviderId,
                                              Description,
                                              AdditionalInfo,
                                              Runtime: Runtime);

        #endregion

        #region (static) NotAuthorized       (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult NotAuthorized(IId                    AuthorizatorId,
                                                               ChargingSession_Id?    SessionId        = null,
                                                               eMobilityProvider_Id?  ProviderId       = null,
                                                               String                 Description      = "NotAuthorized",
                                                               String                 AdditionalInfo   = null,
                                                               TimeSpan?              Runtime          = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.NotAuthorized,
                                              SessionId,
                                              ProviderId,
                                              Description,
                                              AdditionalInfo,
                                              Runtime: Runtime);

        #endregion

        #region (static) Blocked             (AuthorizatorId, SessionId = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ProviderId">The unique identification of the e-mobility provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult Blocked(IId                    AuthorizatorId,
                                                         ChargingSession_Id?    SessionId        = null,
                                                         eMobilityProvider_Id?  ProviderId       = null,
                                                         String                 Description      = null,
                                                         String                 AdditionalInfo   = null,
                                                         TimeSpan?              Runtime          = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.Blocked,
                                              SessionId,
                                              ProviderId,
                                              Description,
                                              AdditionalInfo,
                                              Runtime: Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult CommunicationTimeout(IId                  AuthorizatorId,
                                                                      ChargingSession_Id?  SessionId  = null,
                                                                      TimeSpan?            Runtime    = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.CommunicationTimeout,
                                              SessionId,
                                              Runtime: Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult StartChargingTimeout(IId                  AuthorizatorId,
                                                                      ChargingSession_Id?  SessionId  = null,
                                                                      TimeSpan?            Runtime    = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.StopChargingTimeout,
                                              SessionId,
                                              Runtime: Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, ErrorMessage = null, Runtime = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The optional charging session identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStopChargingPoolResult Error(IId                  AuthorizatorId,
                                                       ChargingSession_Id?  SessionId     = null,
                                                       String               ErrorMessage  = null,
                                                       TimeSpan?            Runtime       = null)

            => new AuthStopChargingPoolResult(AuthorizatorId,
                                              AuthStopChargingPoolResultType.Error,
                                              SessionId,
                                              Description: ErrorMessage,
                                              Runtime:     Runtime);

        #endregion


    }

    /// <summary>
    /// The result of a authorize stop operation at a charging pool.
    /// </summary>
    public enum AuthStopChargingPoolResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        UnknownChargingStation,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging pool does not support this operation.
        /// </summary>
        NotSupported,

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        Authorized,

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        NotAuthorized,

        /// <summary>
        /// The authorize stop operation is not allowed (ev customer is blocked).
        /// </summary>
        Blocked,

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        StopChargingTimeout,

        /// <summary>
        /// The remote stop operation led to an error.
        /// </summary>
        Error

    }

}
