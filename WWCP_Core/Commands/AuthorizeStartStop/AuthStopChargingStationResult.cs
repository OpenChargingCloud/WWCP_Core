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
    /// The result of a authorize stop operation at a charging station.
    /// </summary>
    public class AuthStopChargingStationResult
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

        private readonly AuthStopChargingStationResultType _Result;

        /// <summary>
        /// The result of the authorize stop operation.
        /// </summary>
        public AuthStopChargingStationResultType AuthorizationResult
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

        #region (private) AuthStopChargingStationResult(AuthorizatorId, Result, ProviderId = null, Description = null, AdditionalInfo = null)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize stop result type.</param>
        /// <param name="ProviderId">An optional identification of the ev service provider.</param>
        /// <param name="Description">An optional description of the auth stop result.</param>
        /// <param name="AdditionalInfo">An optional additional message.</param>
        private AuthStopChargingStationResult(Authorizator_Id                    AuthorizatorId,
                                              AuthStopChargingStationResultType  Result,
                                              EVSP_Id                            ProviderId      = null,
                                              String                             Description     = null,
                                              String                             AdditionalInfo  = null)
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

        #region (private) AuthStopChargingStationResult(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// Create a new authorize stop result.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        private AuthStopChargingStationResult(Authorizator_Id  AuthorizatorId,
                                              String           ErrorMessage  = null)
        {

            #region Initial checks

            if (AuthorizatorId == null)
                throw new ArgumentNullException("AuthorizatorId", "The given parameter must not be null!");

            #endregion

            this._Result          = AuthStopChargingStationResultType.Error;
            this._AuthorizatorId  = AuthorizatorId;
            this._Description     = ErrorMessage != null ? ErrorMessage : String.Empty;

        }

        #endregion

        #endregion


        #region (static) Unspecified(AuthorizatorId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static AuthStopChargingStationResult Unspecified(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.Unspecified);

        }

        #endregion

        #region (static) UnknownChargingStation(AuthorizatorId)

        /// <summary>
        /// The charging station is unknown.
        /// </summary>
        public static AuthStopChargingStationResult UnknownChargingStation(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.UnknownChargingStation);

        }

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        public static AuthStopChargingStationResult InvalidSessionId(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.InvalidSessionId);

        }

        #endregion

        #region (static) OutOfService(AuthorizatorId)

        /// <summary>
        /// The charging station is out of service.
        /// </summary>
        public static AuthStopChargingStationResult OutOfService(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.OutOfService);

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
        public static AuthStopChargingStationResult Authorized(Authorizator_Id  AuthorizatorId,
                                                               EVSP_Id          ProviderId,
                                                               String           Description     = null,
                                                               String           AdditionalInfo  = null)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.Authorized,
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
        public static AuthStopChargingStationResult NotAuthorized(Authorizator_Id  AuthorizatorId,
                                                                  EVSP_Id          ProviderId,
                                                                  String           Description    = null,
                                                                  String           AdditionalInfo = null)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.NotAuthorized,
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
        public static AuthStopChargingStationResult Blocked(Authorizator_Id  AuthorizatorId,
                                                            EVSP_Id          ProviderId,
                                                            String           Description     = null,
                                                            String           AdditionalInfo  = null)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.Blocked,
                                                     ProviderId,
                                                     Description,
                                                     AdditionalInfo);

        }

        #endregion

        #region (static) ChargingStationCommunicationTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        public static AuthStopChargingStationResult ChargingStationCommunicationTimeout(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.ChargingStationCommunicationTimeout);

        }

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        public static AuthStopChargingStationResult StartChargingTimeout(Authorizator_Id AuthorizatorId)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
                                                     AuthStopChargingStationResultType.StopChargingTimeout);

        }

        #endregion

        #region (static) Error(AuthorizatorId, ErrorMessage = null)

        /// <summary>
        /// The authorize stop operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ErrorMessage">An error message.</param>
        public static AuthStopChargingStationResult Error(Authorizator_Id  AuthorizatorId,
                                                          String           ErrorMessage = null)
        {

            return new AuthStopChargingStationResult(AuthorizatorId,
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
    /// The result of a authorize stop operation at a charging station.
    /// </summary>
    public enum AuthStopChargingStationResultType
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
        ChargingStationCommunicationTimeout,

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
