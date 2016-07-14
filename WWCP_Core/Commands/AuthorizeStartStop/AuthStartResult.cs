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
    /// The result of a authorize start operation.
    /// </summary>
    public class AuthStartResult
    {

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public Authorizator_Id          AuthorizatorId          { get; }

        /// <summary>
        /// The result of the authorize start operation.
        /// </summary>
        public AuthStartResultType      Result                  { get; }

        /// <summary>
        /// The charging session identification for a successful authorize start operation.
        /// </summary>
        public ChargingSession_Id       SessionId               { get; }

        /// <summary>
        /// The unique identification of the ev service provider.
        /// </summary>
        public EVSP_Id                  ProviderId              { get; }

        /// <summary>
        /// A optional description of the authorize start result.
        /// </summary>
        public String                   Description             { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public String                   AdditionalInfo          { get; }

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        public IEnumerable<Auth_Token>  ListOfAuthStopTokens    { get; }

        /// <summary>
        /// An optional list of authorize stop PINs.
        /// </summary>
        public IEnumerable<UInt32>      ListOfAuthStopPINs      { get; }

        /// <summary>
        /// The run time of the request.
        /// </summary>
        public TimeSpan?                Runtime                 { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStartResult(AuthorizatorId, Result, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="ProviderId">An optional identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        private AuthStartResult(Authorizator_Id      AuthorizatorId,
                                AuthStartResultType  Result,
                                EVSP_Id              ProviderId      = null,
                                String               Description     = null,
                                String               AdditionalInfo  = null,
                                TimeSpan?            Runtime         = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException(nameof(AuthorizatorId),  "The given identification of the authorizator must not be null!");

            #endregion

            this.AuthorizatorId        = AuthorizatorId;
            this.Result                = Result;
            this.ProviderId            = ProviderId;
            this.Description           = Description    ?? String.Empty;
            this.AdditionalInfo        = AdditionalInfo ?? String.Empty;
            this.Runtime               = Runtime        ?? TimeSpan.FromSeconds(0);
            this.ListOfAuthStopTokens  = new Auth_Token[0];
            this.ListOfAuthStopPINs    = new UInt32[0];

        }

        #endregion

        #region (private) AuthStartResult(AuthorizatorId, SessionId, ProviderId, Description = null, AdditionalInfo = null, Runtime = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null)

        /// <summary>
        /// Create a new successful authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="SessionId">The charging session identification for the authorize start operation.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        private AuthStartResult(Authorizator_Id          AuthorizatorId,
                                ChargingSession_Id       SessionId,
                                EVSP_Id                  ProviderId,
                                String                   Description           = null,
                                String                   AdditionalInfo        = null,
                                TimeSpan?                Runtime               = null,
                                IEnumerable<Auth_Token>  ListOfAuthStopTokens  = null,
                                IEnumerable<UInt32>      ListOfAuthStopPINs    = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException(nameof(AuthorizatorId),  "The given identification of the authorizator must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),       "The given charging session identification must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException(nameof(ProviderId),      "The given e-mobility provider identification must not be null!");

            #endregion

            this.Result                = AuthStartResultType.Authorized;
            this.AuthorizatorId        = AuthorizatorId;
            this.SessionId             = SessionId;
            this.ProviderId            = ProviderId;
            this.Description           = Description          != null ? Description          : String.Empty;
            this.AdditionalInfo        = AdditionalInfo       != null ? AdditionalInfo       : String.Empty;
            this.Runtime               = Runtime ?? TimeSpan.FromSeconds(0);
            this.ListOfAuthStopTokens  = ListOfAuthStopTokens != null ? ListOfAuthStopTokens : new Auth_Token[0];
            this.ListOfAuthStopPINs    = ListOfAuthStopPINs   != null ? ListOfAuthStopPINs   : new UInt32[0];

        }

        #endregion

        #region (private) AuthStartResult(AuthorizatorId, ErrorMessage = null, Runtime = null)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        private AuthStartResult(Authorizator_Id  AuthorizatorId,
                                String           ErrorMessage  = null,
                                TimeSpan?        Runtime       = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException(nameof(AuthorizatorId),  "The given identification of the authorizator must not be null!");

            #endregion

            this.Result                = AuthStartResultType.Error;
            this.AuthorizatorId        = AuthorizatorId;
            this.Runtime               = Runtime;
            this.Description           = ErrorMessage ?? String.Empty;
            this.ListOfAuthStopTokens  = new Auth_Token[0];
            this.ListOfAuthStopPINs    = new UInt32[0];

        }

        #endregion

        #endregion


        #region (static) Unspecified(AuthorizatorId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>

        public static AuthStartResult Unspecified(Authorizator_Id AuthorizatorId)

            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.Unspecified);

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>

        public static AuthStartResult InvalidSessionId(Authorizator_Id AuthorizatorId)

            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.InvalidSessionId);

        #endregion

        #region (static) Reserved(AuthorizatorId)

        /// <summary>
        /// The EVSE is reserved.
        /// </summary>

        public static AuthStartResult Reserved(Authorizator_Id AuthorizatorId)

            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.Reserved);

        #endregion

        #region (static) OutOfService(AuthorizatorId)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>

        public static AuthStartResult OutOfService(Authorizator_Id AuthorizatorId)

            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.OutOfService);

        #endregion

        #region (static) Authorized(AuthorizatorId, SessionId, ProviderId, Description = null, AdditionalInfo = null, Runtime = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null)

        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The charging session identification for the authorize start operation.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        public static AuthStartResult

            Authorized(Authorizator_Id          AuthorizatorId,
                       ChargingSession_Id       SessionId,
                       EVSP_Id                  ProviderId,
                       String                   Description           = null,
                       String                   AdditionalInfo        = null,
                       TimeSpan?                Runtime               = null,
                       IEnumerable<Auth_Token>  ListOfAuthStopTokens  = null,
                       IEnumerable<UInt32>      ListOfAuthStopPINs    = null)


            => new AuthStartResult(AuthorizatorId,
                                   SessionId,
                                   ProviderId,
                                   Description,
                                   AdditionalInfo,
                                   Runtime,
                                   ListOfAuthStopTokens,
                                   ListOfAuthStopPINs);

        #endregion

        #region (static) NotAuthorized(AuthorizatorId, ProviderId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        public static AuthStartResult

            NotAuthorized(Authorizator_Id  AuthorizatorId,
                          EVSP_Id          ProviderId,
                          String           Description     = null,
                          String           AdditionalInfo  = null,
                          TimeSpan?        Runtime         = null)


            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.NotAuthorized,
                                   ProviderId,
                                   Description,
                                   AdditionalInfo,
                                   Runtime);

        #endregion

        #region (static) Blocked(AuthorizatorId, ProviderId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        public static AuthStartResult

            Blocked(Authorizator_Id  AuthorizatorId,
                    EVSP_Id          ProviderId,
                    String           Description     = null,
                    String           AdditionalInfo  = null,
                    TimeSpan?        Runtime         = null)


            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.Blocked,
                                   ProviderId,
                                   Description,
                                   AdditionalInfo,
                                   Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, Runtime)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The run time of the request.</param>
        public static AuthStartResult CommunicationTimeout(Authorizator_Id  AuthorizatorId,
                                                           TimeSpan         Runtime)

            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.CommunicationTimeout,
                                   Runtime: Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, Runtime)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="Runtime">The run time of the request.</param>
        public static AuthStartResult StartChargingTimeout(Authorizator_Id  AuthorizatorId,
                                                           TimeSpan         Runtime)

            => new AuthStartResult(AuthorizatorId,
                                   AuthStartResultType.StartChargingTimeout,
                                   Runtime: Runtime);

        #endregion

        #region (static) Error(AuthorizatorId, ErrorMessage = null, Runtime = null)

        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        /// <param name="Runtime">The run time of the request.</param>
        public static AuthStartResult Error(Authorizator_Id  AuthorizatorId,
                                            String           ErrorMessage  = null,
                                            TimeSpan?        Runtime       = null)

            => new AuthStartResult(AuthorizatorId,
                                   ErrorMessage,
                                   Runtime);

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


    /// <summary>
    /// The result of a authorize start operation at an EVSE.
    /// </summary>
    public enum AuthStartResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The EVSE or charging station is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The EVSE or charging station or charging station is out of service.
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
