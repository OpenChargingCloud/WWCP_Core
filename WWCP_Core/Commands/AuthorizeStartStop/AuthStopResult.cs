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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a authorize stop operation.
    /// </summary>
    public class AuthStopResult
    {

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public Authorizator_Id        AuthorizatorId        { get; }

        /// <summary>
        /// The result of the authorize stop operation.
        /// </summary>
        public AuthStopResultType     AuthorizationResult   { get; }

        /// <summary>
        /// The unique identification of the ev service provider.
        /// </summary>
        public eMobilityProvider_Id?  ProviderId            { get; }

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public String                 Description           { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public String                 AdditionalInfo        { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStopResult(AuthorizatorId, Result, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize stop result type.</param>
        /// <param name="ProviderId">An optional identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth stop result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        private AuthStopResult(Authorizator_Id        AuthorizatorId,
                               AuthStopResultType     Result,
                               eMobilityProvider_Id?  ProviderId      = null,
                               String                 Description     = null,
                               String                 AdditionalInfo  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException(nameof(AuthorizatorId), "The given parameter must not be null!");

            #endregion

            this.AuthorizatorId       = AuthorizatorId;
            this.AuthorizationResult  = Result;
            this.ProviderId           = ProviderId     ?? new eMobilityProvider_Id?();
            this.Description          = Description    ?? "";
            this.AdditionalInfo       = AdditionalInfo ?? "";

        }

        #endregion

        #region (private) AuthStopResult(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        private AuthStopResult(Authorizator_Id  AuthorizatorId,
                               String           ErrorMessage  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException(nameof(AuthorizatorId), "The given parameter must not be null!");

            #endregion

            this.AuthorizationResult  = AuthStopResultType.Error;
            this.AuthorizatorId       = AuthorizatorId;
            this.Description          = ErrorMessage;

        }

        #endregion

        #endregion


        #region (static) Unspecified(AuthorizatorId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static AuthStopResult Unspecified(Authorizator_Id AuthorizatorId)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.Unspecified);

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static AuthStopResult InvalidSessionId(Authorizator_Id AuthorizatorId)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.InvalidSessionId);

        #endregion

        #region (static) OutOfService (AuthorizatorId)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        public static AuthStopResult OutOfService(Authorizator_Id AuthorizatorId)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.OutOfService);

        #endregion

        #region (static) Authorized   (AuthorizatorId, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult Authorized(Authorizator_Id        AuthorizatorId,
                                                eMobilityProvider_Id?  ProviderId      = null,
                                                String                 Description     = null,
                                                String                 AdditionalInfo  = null)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.Authorized,
                                  ProviderId,
                                  Description,
                                  AdditionalInfo);

        #endregion

        #region (static) NotAuthorized(AuthorizatorId, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult NotAuthorized(Authorizator_Id        AuthorizatorId,
                                                   eMobilityProvider_Id?  ProviderId      = null,
                                                   String                 Description     = null,
                                                   String                 AdditionalInfo  = null)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.NotAuthorized,
                                  ProviderId,
                                  Description,
                                  AdditionalInfo);

        #endregion

        #region (static) Blocked      (AuthorizatorId, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopResult Blocked(Authorizator_Id        AuthorizatorId,
                                             eMobilityProvider_Id?  ProviderId      = null,
                                             String                 Description     = null,
                                             String                 AdditionalInfo  = null)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.Blocked,
                                  ProviderId,
                                  Description,
                                  AdditionalInfo);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        public static AuthStopResult CommunicationTimeout(Authorizator_Id AuthorizatorId)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.CommunicationTimeout);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        public static AuthStopResult StartChargingTimeout(Authorizator_Id AuthorizatorId)

            => new AuthStopResult(AuthorizatorId,
                                  AuthStopResultType.StopChargingTimeout);

        #endregion

        #region (static) Error(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        public static AuthStopResult Error(Authorizator_Id  AuthorizatorId,
                                           String           ErrorMessage = null)

            => new AuthStopResult(AuthorizatorId,
                                  ErrorMessage);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (ProviderId != null)
                return String.Concat(AuthorizationResult.ToString(), ", ", ProviderId);

            return String.Concat(AuthorizationResult.ToString());

        }

        #endregion

    }

    /// <summary>
    /// The result of a authorize stop operation.
    /// </summary>
    public enum AuthStopResultType
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
        /// The EVSE or charging station is out of service.
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
