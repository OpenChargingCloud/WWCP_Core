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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of a authorize start operation at an EVSE.
    /// </summary>
    public class AuthStartChargingStationResult
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

        #region AuthorizationResult

        private readonly AuthStartChargingStationResultType _Result;

        /// <summary>
        /// The result of the authorize start operation.
        /// </summary>
        public AuthStartChargingStationResultType  AuthorizationResult
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
        /// The charging session identification for a successful authorize start operation.
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
        /// A optional description of the authorize start result.
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

        #region ListOfAuthStopTokens

        private readonly IEnumerable<Auth_Token> _ListOfAuthStopTokens;

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        public IEnumerable<Auth_Token> ListOfAuthStopTokens
        {
            get
            {
                return _ListOfAuthStopTokens;
            }
        }

        #endregion

        #region ListOfAuthStopPINs

        private readonly IEnumerable<UInt32> _ListOfAuthStopPINs;

        /// <summary>
        /// An optional list of authorize stop PINs.
        /// </summary>
        public IEnumerable<UInt32> ListOfAuthStopPINs
        {
            get
            {
                return _ListOfAuthStopPINs;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) AuthStartChargingStationResult(AuthorizatorId, Result, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="ProviderId">An optional identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        private AuthStartChargingStationResult(Authorizator_Id                     AuthorizatorId,
                                               AuthStartChargingStationResultType  Result,
                                               EVSP_Id                             ProviderId      = null,
                                               String                              Description     = null,
                                               String                              AdditionalInfo  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException("AuthorizatorId", "The given parameter must not be null!");

            #endregion

            this._AuthorizatorId        = AuthorizatorId;
            this._Result                = Result;
            this._ProviderId            = ProviderId;
            this._Description           = Description    != null ? Description    : String.Empty;
            this._AdditionalInfo        = AdditionalInfo != null ? AdditionalInfo : String.Empty;
            this._ListOfAuthStopTokens  = new Auth_Token[0];
            this._ListOfAuthStopPINs    = new UInt32[0];

        }

        #endregion

        #region (private) AuthStartChargingStationResult(AuthorizatorId, SessionId, ProviderId, Description = null, AdditionalInfo = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null)

        /// <summary>
        /// Create a new successful authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="SessionId">The charging session identification for the authorize start operation.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        private AuthStartChargingStationResult(Authorizator_Id          AuthorizatorId,
                                               ChargingSession_Id       SessionId,
                                               EVSP_Id                  ProviderId,
                                               String                   Description           = null,
                                               String                   AdditionalInfo        = null,
                                               IEnumerable<Auth_Token>  ListOfAuthStopTokens  = null,
                                               IEnumerable<UInt32>      ListOfAuthStopPINs    = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException("AuthorizatorId",  "The given parameter must not be null!");

            if (SessionId == null)
                throw new ArgumentNullException(nameof(SessionId),       "The given parameter must not be null!");

            if (ProviderId == null)
                throw new ArgumentNullException("ProviderId",      "The given parameter must not be null!");

            #endregion

            this._Result                = AuthStartChargingStationResultType.Authorized;
            this._AuthorizatorId        = AuthorizatorId;
            this._SessionId             = SessionId;
            this._ProviderId            = ProviderId;
            this._Description           = Description          != null ? Description          : String.Empty;
            this._AdditionalInfo        = AdditionalInfo       != null ? AdditionalInfo       : String.Empty;
            this._ListOfAuthStopTokens  = ListOfAuthStopTokens != null ? ListOfAuthStopTokens : new Auth_Token[0];
            this._ListOfAuthStopPINs    = ListOfAuthStopPINs   != null ? ListOfAuthStopPINs   : new UInt32[0];

        }

        #endregion

        #region (private) AuthStartChargingStationResult(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// Create a new remote start result.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        private AuthStartChargingStationResult(Authorizator_Id  AuthorizatorId,
                                               String           ErrorMessage  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException("AuthorizatorId", "The given parameter must not be null!");

            #endregion

            this._Result                = AuthStartChargingStationResultType.Error;
            this._AuthorizatorId        = AuthorizatorId;
            this._Description           = ErrorMessage         != null ? ErrorMessage         : String.Empty;
            this._ListOfAuthStopTokens  = ListOfAuthStopTokens != null ? ListOfAuthStopTokens : new Auth_Token[0];
            this._ListOfAuthStopPINs    = ListOfAuthStopPINs   != null ? ListOfAuthStopPINs   : new UInt32[0];

        }

        #endregion

        #endregion


        #region (static) Unspecified(AuthorizatorId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static AuthStartChargingStationResult Unspecified(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.Unspecified);

        }

        #endregion

        #region (static) UnknownEVSE(AuthorizatorId)

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        public static AuthStartChargingStationResult UnknownEVSE(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.UnknownChargingStation);

        }

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static AuthStartChargingStationResult InvalidSessionId(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.InvalidSessionId);

        }

        #endregion

        #region (static) Reserved(AuthorizatorId)

        /// <summary>
        /// The charging station is reserved.
        /// </summary>
        public static AuthStartChargingStationResult Reserved(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.Reserved);

        }

        #endregion

        #region (static) OutOfService(AuthorizatorId)

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        public static AuthStartChargingStationResult OutOfService(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.OutOfService);

        }

        #endregion

        #region (static) Authorized(AuthorizatorId, SessionId, ProviderId, Description = null, AdditionalInfo = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null, ListOfAuthStopPINs = null)

        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="SessionId">The charging session identification for the authorize start operation.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        /// <param name="ListOfAuthStopTokens">An optional enumeration of authorize stop tokens.</param>
        /// <param name="ListOfAuthStopPINs">An optional enumeration of authorize stop PINs.</param>
        public static AuthStartChargingStationResult Authorized(Authorizator_Id          AuthorizatorId,
                                                                ChargingSession_Id       SessionId,
                                                                EVSP_Id                  ProviderId,
                                                                String                   Description           = null,
                                                                String                   AdditionalInfo        = null,
                                                                IEnumerable<Auth_Token>  ListOfAuthStopTokens  = null,
                                                                IEnumerable<UInt32>      ListOfAuthStopPINs    = null)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      SessionId,
                                                      ProviderId,
                                                      Description,
                                                      AdditionalInfo,
                                                      ListOfAuthStopTokens,
                                                      ListOfAuthStopPINs);

        }

        #endregion

        #region (static) NotAuthorized(AuthorizatorId, SessionId)

        /// <summary>
        /// The authorize start was not successful (e.g. ev customer is unkown).
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ProviderId">The unique identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        public static AuthStartChargingStationResult NotAuthorized(Authorizator_Id  AuthorizatorId,
                                                                   EVSP_Id          ProviderId,
                                                                   String           Description     = null,
                                                                   String           AdditionalInfo  = null)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.NotAuthorized,
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
        public static AuthStartChargingStationResult Blocked(Authorizator_Id  AuthorizatorId,
                                                             EVSP_Id          ProviderId,
                                                             String           Description     = null,
                                                             String           AdditionalInfo  = null)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.Blocked,
                                                      ProviderId,
                                                      Description,
                                                      AdditionalInfo);

        }

        #endregion

        #region (static) EVSECommunicationTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        public static AuthStartChargingStationResult EVSECommunicationTimeout(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.ChargingStationCommunicationTimeout);

        }

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        public static AuthStartChargingStationResult StartChargingTimeout(Authorizator_Id AuthorizatorId)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
                                                      AuthStartChargingStationResultType.StartChargingTimeout);

        }

        #endregion

        #region (static) Error(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        public static AuthStartChargingStationResult Error(Authorizator_Id  AuthorizatorId,
                                                           String           ErrorMessage = null)
        {

            return new AuthStartChargingStationResult(AuthorizatorId,
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
                return String.Concat(AuthorizationResult.ToString(), ", ", ProviderId);

            return String.Concat(AuthorizationResult.ToString());

        }

        #endregion

    }


    /// <summary>
    /// The result of a authorize start operation at an EVSE.
    /// </summary>
    public enum AuthStartChargingStationResultType
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
        /// The authorize start ran into a timeout between evse operator backend and charging station.
        /// </summary>
        ChargingStationCommunicationTimeout,

        /// <summary>
        /// The authorize start ran into a timeout between charging station and ev.
        /// </summary>
        StartChargingTimeout,

        /// <summary>
        /// The remote start operation led to an error.
        /// </summary>
        Error

    }

}
