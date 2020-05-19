/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The result of an authorize start request.
    /// </summary>
    public class AuthStartResult
    {

        #region Properties

        /// <summary>
        /// The identification of the authorizing entity.
        /// </summary>
        public IId                          AuthorizatorId                { get; }

        /// <summary>
        /// The entity asking for an authorization.
        /// </summary>
        public ISendAuthorizeStartStop      ISendAuthorizeStartStop       { get; }

        /// <summary>
        /// The entity giving an authorization.
        /// </summary>
        public IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop    { get; }

        /// <summary>
        /// The result of the authorize start request.
        /// </summary>
        public AuthStartResultTypes         Result                        { get; }

        /// <summary>
        /// The optional charging session identification, when the authorize start operation was successful.
        /// </summary>
        public ChargingSession_Id?          SessionId                     { get; }

        /// <summary>
        /// An optional contract identification.
        /// </summary>
        public String                       ContractId                    { get; }

        /// <summary>
        /// An optional printed number.
        /// </summary>
        public String                       PrintedNumber                 { get; }

        /// <summary>
        /// The timestamp when this authorization expires.
        /// </summary>
        public DateTime?                    ExpiryDate                    { get; }

        /// <summary>
        /// The optional maximum allowed charging current.
        /// </summary>
        public Single?                      MaxkW                         { get;}

        /// <summary>
        /// The optional maximum allowed charging energy.
        /// </summary>
        public Single?                      MaxkWh                        { get;}

        /// <summary>
        /// The optional maximum allowed charging duration.
        /// </summary>
        public TimeSpan?                    MaxDuration                   { get; }

        /// <summary>
        /// Optional charging tariff information.
        /// </summary>
        public IEnumerable<ChargingTariff>  ChargingTariffs               { get; }

        /// <summary>
        /// An optional list of authorize stop tokens.
        /// </summary>
        public IEnumerable<Auth_Token>      ListOfAuthStopTokens          { get; }

        /// <summary>
        /// An optional list of authorize stop PINs.
        /// </summary>
        public IEnumerable<UInt32>          ListOfAuthStopPINs            { get; }


        /// <summary>
        /// The unique identification of the e-mobility provider.
        /// </summary>
        public eMobilityProvider_Id?        ProviderId                    { get; }

        /// <summary>
        /// A optional description of the authorize start result, e.g. in case of an error.
        /// </summary>
        public I18NString                   Description                   { get; }

        /// <summary>
        /// An optional additional message, e.g. in case of an error.
        /// </summary>
        public I18NString                   AdditionalInfo                { get; }

        /// <summary>
        /// Number of transmission retries.
        /// </summary>
        public Byte                         NumberOfRetries               { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?                    Runtime                       { get; }

        #endregion

        #region Constructor(s)

        #region (private) AuthStartResult(AuthorizatorId,                             Result, ...)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity asking for an authorization.</param>
        /// 
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
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
        private AuthStartResult(IId                          AuthorizatorId,
                                AuthStartResultTypes         Result,
                                ISendAuthorizeStartStop      ISendAuthorizeStartStop      = null,
                                IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop   = null,

                                ChargingSession_Id?          SessionId                    = null,
                                String                       ContractId                   = null,
                                String                       PrintedNumber                = null,
                                DateTime?                    ExpiryDate                   = null,
                                Single?                      MaxkW                        = null,
                                Single?                      MaxkWh                       = null,
                                TimeSpan?                    MaxDuration                  = null,
                                IEnumerable<ChargingTariff>  ChargingTariffs              = null,
                                IEnumerable<Auth_Token>      ListOfAuthStopTokens         = null,
                                IEnumerable<UInt32>          ListOfAuthStopPINs           = null,

                                eMobilityProvider_Id?        ProviderId                   = null,
                                I18NString                   Description                  = null,
                                I18NString                   AdditionalInfo               = null,
                                Byte                         NumberOfRetries              = 0,
                                TimeSpan?                    Runtime                      = null)

        {

            this.AuthorizatorId              = AuthorizatorId ?? throw new ArgumentNullException(nameof(AuthorizatorId), "The given identification of the authorizator must not be null!");
            this.ISendAuthorizeStartStop     = ISendAuthorizeStartStop;
            this.IReceiveAuthorizeStartStop  = IReceiveAuthorizeStartStop;
            this.Result                      = Result;
            this.SessionId                   = SessionId;
            this.ContractId                  = ContractId;
            this.PrintedNumber               = PrintedNumber;
            this.ExpiryDate                  = ExpiryDate;
            this.MaxkW                       = MaxkW;
            this.MaxkWh                      = MaxkWh;
            this.MaxDuration                 = MaxDuration;
            this.ChargingTariffs             = ChargingTariffs;
            this.ListOfAuthStopTokens        = ListOfAuthStopTokens;
            this.ListOfAuthStopPINs          = ListOfAuthStopPINs;

            this.ProviderId                  = ProviderId     ?? new eMobilityProvider_Id?();
            this.Description                 = Description;
            this.AdditionalInfo              = AdditionalInfo;
            this.NumberOfRetries             = NumberOfRetries;
            this.Runtime                     = Runtime        ?? TimeSpan.FromSeconds(0);

        }

        #endregion

        #region (private) AuthStartResult(AuthorizatorId, ISendAuthorizeStartStop,    Result, ...)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
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
        protected AuthStartResult(IId                          AuthorizatorId,
                                  ISendAuthorizeStartStop      ISendAuthorizeStartStop,
                                  AuthStartResultTypes         Result,
                                  ChargingSession_Id?          SessionId              = null,
                                  String                       ContractId             = null,
                                  String                       PrintedNumber          = null,
                                  DateTime?                    ExpiryDate             = null,
                                  Single?                      MaxkW                  = null,
                                  Single?                      MaxkWh                 = null,
                                  TimeSpan?                    MaxDuration            = null,
                                  IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                                  IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                                  IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                                  eMobilityProvider_Id?        ProviderId             = null,
                                  I18NString                   Description            = null,
                                  I18NString                   AdditionalInfo         = null,
                                  Byte                         NumberOfRetries        = 0,
                                  TimeSpan?                    Runtime                = null)

            : this(AuthorizatorId,
                   Result,
                   ISendAuthorizeStartStop,
                   null,
                   SessionId,
                   ContractId,
                   PrintedNumber,
                   ExpiryDate,
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

        #region (private) AuthStartResult(AuthorizatorId, IReceiveAuthorizeStartStop, Result, ...)

        /// <summary>
        /// Create a new authorize start result.
        /// </summary>
        /// <param name="AuthorizatorId">The identification of the authorizing entity.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="Result">The authorize start result type.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
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
        protected AuthStartResult(IId                          AuthorizatorId,
                                  IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop,
                                  AuthStartResultTypes         Result,
                                  ChargingSession_Id?          SessionId              = null,
                                  String                       ContractId             = null,
                                  String                       PrintedNumber          = null,
                                  DateTime?                    ExpiryDate             = null,
                                  Single?                      MaxkW                  = null,
                                  Single?                      MaxkWh                 = null,
                                  TimeSpan?                    MaxDuration            = null,
                                  IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                                  IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                                  IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                                  eMobilityProvider_Id?        ProviderId             = null,
                                  I18NString                   Description            = null,
                                  I18NString                   AdditionalInfo         = null,
                                  Byte                         NumberOfRetries        = 0,
                                  TimeSpan?                    Runtime                = null)

            : this(AuthorizatorId,
                   Result,
                   null,
                   IReceiveAuthorizeStartStop,
                   SessionId,
                   ContractId,
                   PrintedNumber,
                   ExpiryDate,
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

        {

            this.IReceiveAuthorizeStartStop = IReceiveAuthorizeStartStop;

        }

        #endregion

        #endregion


        #region (static) Unspecified         (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Unspecified(IId                      AuthorizatorId,
                        ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                        ChargingSession_Id?      SessionId     = null,
                        I18NString               Description   = null,
                        TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.Unspecified,
                                       SessionId,
                                       Description:  Description,
                                       Runtime:      Runtime);



        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Unspecified(IId                         AuthorizatorId,
                        IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                        ChargingSession_Id?         SessionId     = null,
                        I18NString                  Description   = null,
                        TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.Unspecified,
                                       SessionId,
                                       Description:  Description,
                                       Runtime:      Runtime);

        #endregion

        #region (static) AdminDown           (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            AdminDown(IId                      AuthorizatorId,
                      ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                      ChargingSession_Id?      SessionId     = null,
                      I18NString               Description   = null,
                      TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.AdminDown,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "The authentication service was disabled by the administrator!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The authentication service was disabled by the administrator.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            AdminDown(IId                         AuthorizatorId,
                      IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                      ChargingSession_Id?         SessionId     = null,
                      I18NString                  Description   = null,
                      TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.AdminDown,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "The authentication service was disabled by the administrator!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) UnknownLocation     (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            UnknownLocation(IId                      AuthorizatorId,
                            ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                            ChargingSession_Id?      SessionId     = null,
                            I18NString               Description   = null,
                            TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.UnknownLocation,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Unknown location!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The given charging location is unknown.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            UnknownLocation(IId                         AuthorizatorId,
                            IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                            ChargingSession_Id?         SessionId     = null,
                            I18NString                  Description   = null,
                            TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.UnknownLocation,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Unknown location!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) InvalidToken        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            InvalidToken(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId     = null,
                         I18NString               Description   = null,
                         TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.InvalidToken,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Invalid token!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            InvalidToken(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId     = null,
                         I18NString                  Description   = null,
                         TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.InvalidToken,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Invalid token!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) InvalidSessionId    (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            InvalidSessionId(IId                      AuthorizatorId,
                             ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                             ChargingSession_Id?      SessionId     = null,
                             I18NString               Description   = null,
                             TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.InvalidSessionId,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Invalid session identification!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            InvalidSessionId(IId                         AuthorizatorId,
                             IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                             ChargingSession_Id?         SessionId     = null,
                             I18NString                  Description   = null,
                             TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.InvalidSessionId,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Invalid session identification!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) Reserved            (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Reserved(IId                      AuthorizatorId,
                     ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                     ChargingSession_Id?      SessionId     = null,
                     I18NString               Description   = null,
                     TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.Reserved,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Reserved!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The EVSE is already reserved.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Reserved(IId                         AuthorizatorId,
                     IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                     ChargingSession_Id?         SessionId     = null,
                     I18NString                  Description   = null,
                     TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.Reserved,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Reserved!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) OutOfService        (AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            OutOfService(IId                      AuthorizatorId,
                         ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                         ChargingSession_Id?      SessionId     = null,
                         I18NString               Description   = null,
                         TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.OutOfService,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Out of service!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            OutOfService(IId                         AuthorizatorId,
                         IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                         ChargingSession_Id?         SessionId     = null,
                         I18NString                  Description   = null,
                         TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.OutOfService,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Out of service!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) Authorized          (AuthorizatorId, SessionId = null, ListOfAuthStopTokens = null, ListOfAuthStopPINs = null, ProviderId = null, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
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
        public static AuthStartResult

            Authorized(IId                          AuthorizatorId,
                       ISendAuthorizeStartStop      ISendAuthorizeStartStop,
                       ChargingSession_Id?          SessionId              = null,
                       String                       ContractId             = null,
                       String                       PrintedNumber          = null,
                       DateTime?                    ExpiryDate             = null,
                       Single?                      MaxkW                  = null,
                       Single?                      MaxkWh                 = null,
                       TimeSpan?                    MaxDuration            = null,
                       IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                       IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                       IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                       eMobilityProvider_Id?        ProviderId             = null,
                       I18NString                   Description            = null,
                       I18NString                   AdditionalInfo         = null,
                       Byte                         NumberOfRetries        = 0,
                       TimeSpan?                    Runtime                = null)


                => new AuthStartResult(AuthorizatorId,
                                       AuthStartResultTypes.Authorized,
                                       ISendAuthorizeStartStop,
                                       null,

                                       SessionId,
                                       ContractId,
                                       PrintedNumber,
                                       ExpiryDate,
                                       MaxkW,
                                       MaxkWh,
                                       MaxDuration,
                                       ChargingTariffs,
                                       ListOfAuthStopTokens,
                                       ListOfAuthStopPINs,

                                       ProviderId,
                                       Description ?? I18NString.Create(Languages.eng, "Success!"),
                                       AdditionalInfo,
                                       NumberOfRetries,
                                       Runtime);



        /// <summary>
        /// The authorize start was successful.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification, when the authorize start operation was successful.</param>
        /// <param name="ContractId">An optional contract identification.</param>
        /// <param name="PrintedNumber">An optional printed number.</param>
        /// <param name="ExpiryDate">The timestamp when this authorization expires.</param>
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
        public static AuthStartResult

            Authorized(IId                          AuthorizatorId,
                       IReceiveAuthorizeStartStop   IReceiveAuthorizeStartStop,
                       ChargingSession_Id?          SessionId              = null,
                       String                       ContractId             = null,
                       String                       PrintedNumber          = null,
                       DateTime?                    ExpiryDate             = null,
                       Single?                      MaxkW                  = null,
                       Single?                      MaxkWh                 = null,
                       TimeSpan?                    MaxDuration            = null,
                       IEnumerable<ChargingTariff>  ChargingTariffs        = null,
                       IEnumerable<Auth_Token>      ListOfAuthStopTokens   = null,
                       IEnumerable<UInt32>          ListOfAuthStopPINs     = null,

                       eMobilityProvider_Id?        ProviderId             = null,
                       I18NString                   Description            = null,
                       I18NString                   AdditionalInfo         = null,
                       Byte                         NumberOfRetries        = 0,
                       TimeSpan?                    Runtime                = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.Authorized,
                                       SessionId,
                                       ContractId,
                                       PrintedNumber,
                                       ExpiryDate,
                                       MaxkW,
                                       MaxkWh,
                                       MaxDuration,
                                       ChargingTariffs,
                                       ListOfAuthStopTokens,
                                       ListOfAuthStopPINs,

                                       ProviderId,
                                       Description ?? I18NString.Create(Languages.eng, "Success!"),
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
        public static AuthStartResult

            NotAuthorized(IId                      AuthorizatorId,
                          ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                          ChargingSession_Id?      SessionId        = null,
                          eMobilityProvider_Id?    ProviderId       = null,
                          I18NString               Description      = null,
                          I18NString               AdditionalInfo   = null,
                          TimeSpan?                Runtime          = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.NotAuthorized,
                                       SessionId,
                                       ProviderId:      ProviderId,
                                       Description:     Description ?? I18NString.Create(Languages.eng, "Not authorized!"),
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
        public static AuthStartResult

            NotAuthorized(IId                         AuthorizatorId,
                          IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                          ChargingSession_Id?         SessionId        = null,
                          eMobilityProvider_Id?       ProviderId       = null,
                          I18NString                  Description      = null,
                          I18NString                  AdditionalInfo   = null,
                          TimeSpan?                   Runtime          = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.NotAuthorized,
                                       SessionId,
                                       ProviderId:      ProviderId,
                                       Description:     Description ?? I18NString.Create(Languages.eng, "Not authorized!"),
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
        public static AuthStartResult

            Blocked(IId                      AuthorizatorId,
                    ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                    ChargingSession_Id?      SessionId        = null,
                    eMobilityProvider_Id?    ProviderId       = null,
                    I18NString               Description      = null,
                    I18NString               AdditionalInfo   = null,
                    TimeSpan?                Runtime          = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.Blocked,
                                       SessionId,
                                       ProviderId:      ProviderId,
                                       Description:     Description ?? I18NString.Create(Languages.eng, "Blocked!"),
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
        public static AuthStartResult

            Blocked(IId                         AuthorizatorId,
                    IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                    ChargingSession_Id?         SessionId        = null,
                    eMobilityProvider_Id?       ProviderId       = null,
                    I18NString                  Description      = null,
                    I18NString                  AdditionalInfo   = null,
                    TimeSpan?                   Runtime          = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.Blocked,
                                       SessionId,
                                       ProviderId:      ProviderId,
                                       Description:     Description ?? I18NString.Create(Languages.eng, "Blocked!"),
                                       AdditionalInfo:  AdditionalInfo,
                                       Runtime:         Runtime);

        #endregion

        #region (static) CommunicationTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            CommunicationTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId     = null,
                                 I18NString               Description   = null,
                                 TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.CommunicationTimeout,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Communication timeout!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between evse operator backend and charging station.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            CommunicationTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId     = null,
                                 I18NString                  Description   = null,
                                 TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.CommunicationTimeout,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Communication timeout!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) StartChargingTimeout(AuthorizatorId, SessionId = null, Runtime = null)

        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            StartChargingTimeout(IId                      AuthorizatorId,
                                 ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                                 ChargingSession_Id?      SessionId     = null,
                                 I18NString               Description   = null,
                                 TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.StartChargingTimeout,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Start charging timeout!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The authorize stop ran into a timeout between charging station and ev.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            StartChargingTimeout(IId                         AuthorizatorId,
                                 IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                                 ChargingSession_Id?         SessionId     = null,
                                 I18NString                  Description   = null,
                                 TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.StartChargingTimeout,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Start charging timeout!"),
                                       Runtime:      Runtime);

        #endregion

        #region (static) Error               (AuthorizatorId, SessionId = null, Description = null, Runtime = null)

        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="ISendAuthorizeStartStop">The entity asking for an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Error(IId                      AuthorizatorId,
                  ISendAuthorizeStartStop  ISendAuthorizeStartStop,
                  ChargingSession_Id?      SessionId     = null,
                  I18NString               Description   = null,
                  TimeSpan?                Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       ISendAuthorizeStartStop,
                                       AuthStartResultTypes.Error,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Error!"),
                                       Runtime:      Runtime);



        /// <summary>
        /// The authorize start operation led to an error.
        /// </summary>
        /// <param name="AuthorizatorId">An authorizator identification.</param>
        /// <param name="IReceiveAuthorizeStartStop">The entity giving an authorization.</param>
        /// <param name="SessionId">The optional charging session identification from the authorization request.</param>
        /// <param name="Description">An optional description of the auth start result.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static AuthStartResult

            Error(IId                         AuthorizatorId,
                  IReceiveAuthorizeStartStop  IReceiveAuthorizeStartStop,
                  ChargingSession_Id?         SessionId     = null,
                  I18NString                  Description   = null,
                  TimeSpan?                   Runtime       = null)


                => new AuthStartResult(AuthorizatorId,
                                       IReceiveAuthorizeStartStop,
                                       AuthStartResultTypes.Error,
                                       SessionId,
                                       Description:  Description ?? I18NString.Create(Languages.eng, "Error!"),
                                       Runtime:      Runtime);

        #endregion


        #region ToJSON(ResponseMapper = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="ResponseMapper">An optional response mapper delegate.</param>
        public JObject ToJSON(Func<JObject, JObject> ResponseMapper = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("result",                Result.        ToString()),

                           SessionId.HasValue
                               ? new JProperty("sessionId",       SessionId.     ToString())
                               : null,

                           ProviderId.HasValue
                               ? new JProperty("providerId",      ProviderId.    ToString())
                               : null,

                                 new JProperty("authorizatorId",  AuthorizatorId.ToString()),

                           Description.IsNeitherNullNorEmpty()
                               ? new JProperty("description",     Description.   ToJSON())
                               : null,

                           Runtime.HasValue
                               ? new JProperty("runtime",         Runtime.Value.TotalMilliseconds)
                               : null

                       );

            return ResponseMapper != null
                       ? ResponseMapper(JSON)
                       : JSON;

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Result + (ProviderId != null ? ", " + ProviderId : "");

        #endregion

    }


    /// <summary>
    /// The result of an authorize start request.
    /// </summary>
    public enum AuthStartResultTypes
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
        /// The given charging location is unknown.
        /// </summary>
        UnknownLocation,

        /// <summary>
        /// The given token is unknown or invalid.
        /// </summary>
        InvalidToken,

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// The charging location is already reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The charging location is out of service.
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
        /// The authorize start ran into a timeout between evse operator backend and the charging location.
        /// </summary>
        CommunicationTimeout,

        /// <summary>
        /// The authorize start ran into a timeout between the charging location and the EV.
        /// </summary>
        StartChargingTimeout,

        /// <summary>
        /// The remote start operation led to an error.
        /// </summary>
        Error

    }

}
