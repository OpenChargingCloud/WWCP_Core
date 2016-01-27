/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a authorize stop operation at an EVSE.
    /// </summary>
    public class AuthStopEVSEResult
    {

        #region Properties

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region Result

        private readonly AuthStopEVSEResultType _Result;

        /// <summary>
        /// The result of the authorize stop operation.
        /// </summary>
        public AuthStopEVSEResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        /// <summary>
        /// The charging session identification for a successful authorize stop operation.
        /// </summary>
        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #region ProviderId

        private readonly EVSP_Id _ProviderId;

        /// <summary>
        /// The unique identification of the ev service provider.
        /// </summary>
        public EVSP_Id ProviderId
        {
            get
            {
                return _ProviderId;
            }
        }

        #endregion

        #region Description

        private readonly String _Description;

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #region AdditionalInfo

        private readonly String _AdditionalInfo;

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public String AdditionalInfo
        {
            get
            {
                return _AdditionalInfo;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) AuthStopEVSEResult(AuthorizatorId, Result, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize stop result type.</param>
        /// <param name="ProviderId">An optional identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth stop result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        private AuthStopEVSEResult(Authorizator_Id         AuthorizatorId,
                                   AuthStopEVSEResultType  Result,
                                   EVSP_Id                 ProviderId      = null,
                                   String                  Description     = null,
                                   String                  AdditionalInfo  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException("AuthorizatorId", "The given parameter must not be null!");

            #endregion

            this._AuthorizatorId  = AuthorizatorId;
            this._Result          = Result;
            this._ProviderId      = ProviderId;
            this._Description     = Description    != null ? Description    : String.Empty;
            this._AdditionalInfo  = AdditionalInfo != null ? AdditionalInfo : String.Empty;

        }

        #endregion

        #region (private) AuthStopEVSEResult(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        private AuthStopEVSEResult(Authorizator_Id  AuthorizatorId,
                                   String           ErrorMessage  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException("AuthorizatorId", "The given parameter must not be null!");

            #endregion

            this._Result          = AuthStopEVSEResultType.Error;
            this._AuthorizatorId  = AuthorizatorId;
            this._Description     = ErrorMessage != null ? ErrorMessage : String.Empty;

        }

        #endregion

        #endregion


        #region (static) Unspecified(AuthorizatorId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static AuthStopEVSEResult Unspecified(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.Unspecified);

        }

        #endregion

        #region (static) UnknownEVSE(AuthorizatorId)

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        public static AuthStopEVSEResult UnknownEVSE(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.UnknownEVSE);

        }

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static AuthStopEVSEResult InvalidSessionId(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.InvalidSessionId);

        }

        #endregion

        #region (static) OutOfService(AuthorizatorId)

        /// <summary>
        /// The EVSE is out of service.
        /// </summary>
        public static AuthStopEVSEResult OutOfService(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.OutOfService);

        }

        #endregion

        #region (static) Authorized(AuthorizatorId, ProviderId, Description = null, AdditionalInfo = null)

        /// <summary>
        /// The authorize stop was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopEVSEResult Authorized(Authorizator_Id  AuthorizatorId,
                                                    EVSP_Id          ProviderId,
                                                    String           Description     = null,
                                                    String           AdditionalInfo  = null)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.Authorized,
                                          ProviderId,
                                          Description,
                                          AdditionalInfo);

        }

        #endregion

        #region (static) NotAuthorized(AuthorizatorId, SessionId)

        /// <summary>
        /// The authorize stop was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopEVSEResult NotAuthorized(Authorizator_Id  AuthorizatorId,
                                                       EVSP_Id          ProviderId,
                                                       String           Description    = null,
                                                       String           AdditionalInfo = null)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.NotAuthorized,
                                          ProviderId,
                                          Description,
                                          AdditionalInfo);

        }

        #endregion

        #region (static) Blocked(AuthorizatorId, SessionId)

        /// <summary>
        /// The authorize start operation is not allowed (ev customer is blocked).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStopEVSEResult Blocked(Authorizator_Id  AuthorizatorId,
                                                 EVSP_Id          ProviderId,
                                                 String           Description     = null,
                                                 String           AdditionalInfo  = null)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.Blocked,
                                          ProviderId,
                                          Description,
                                          AdditionalInfo);

        }

        #endregion

        #region (static) EVSECommunicationTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and evse.
        /// </summary>
        public static AuthStopEVSEResult EVSECommunicationTimeout(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.EVSECommunicationTimeout);

        }

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between evse and ev.
        /// </summary>
        public static AuthStopEVSEResult StartChargingTimeout(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          AuthStopEVSEResultType.StopChargingTimeout);

        }

        #endregion

        #region (static) Error(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        public static AuthStopEVSEResult Error(Authorizator_Id  AuthorizatorId,
                                               String           ErrorMessage = null)
        {

            return new AuthStopEVSEResult(AuthorizatorId,
                                          ErrorMessage);

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

    /// <summary>
    /// The result of a authorize stop operation at an EVSE.
    /// </summary>
    public enum AuthStopEVSEResultType
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The EVSE is unknown.
        /// </summary>
        UnknownEVSE,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The EVSE is out of service.
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
        /// The authorize stop ran into a timeout between evse operator backend and evse.
        /// </summary>
        EVSECommunicationTimeout,

        /// <summary>
        /// The authorize stop ran into a timeout between evse and ev.
        /// </summary>
        StopChargingTimeout,

        /// <summary>
        /// The remote stop operation led to an error.
        /// </summary>
        Error

    }

}
