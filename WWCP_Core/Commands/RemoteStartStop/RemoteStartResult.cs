/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The result of a RemoteStart operation.
    /// </summary>
    public class RemoteStartResult
    {

        #region Properties

        /// <summary>
        /// The result of a RemoteStart operation.
        /// </summary>
        public RemoteStartResultTypes  Result            { get; }

        /// <summary>
        /// The sender of the result.
        /// </summary>
        public System_Id               Sender            { get; }

        /// <summary>
        /// The charging session for the RemoteStart operation.
        /// </summary>
        public ChargingSession?        Session           { get; internal set; }

        /// <summary>
        /// A optional description of the RemoteStart result.
        /// </summary>
        public I18NString              Description       { get; }

        /// <summary>
        /// An optional additional information on this error,
        /// e.g. the HTTP error response.
        /// </summary>
        public String?                 AdditionalInfo    { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                Runtime           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new RemoteStart result.
        /// </summary>
        /// <param name="Result">The result of the RemoteStart operation.</param>
        /// <param name="Sender">The sender of the result.</param>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStartResult(RemoteStartResultTypes  Result,
                                  System_Id               Sender,
                                  I18NString?             Description      = null,
                                  String?                 AdditionalInfo   = null,
                                  ChargingSession?        Session          = null,
                                  TimeSpan?               Runtime          = null)
        {

            this.Result          = Result;
            this.Sender          = Sender;
            this.Session         = Session;
            this.Description     = Description ?? I18NString.Empty;
            this.AdditionalInfo  = AdditionalInfo;
            this.Runtime         = Runtime     ?? TimeSpan.Zero;

        }

        #endregion


        #region (static) Unspecified                       (Sender)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStartResult Unspecified(System_Id Sender)

            => new (
                   RemoteStartResultTypes.Unspecified,
                   Sender
               );

        #endregion

        #region (static) UnknownOperator                   (Sender, Runtime = null)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult UnknownOperator(System_Id  Sender,
                                                        TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.UnknownOperator,
                   Sender,
                   I18NString.Create("The EVSE or charging station operator is unknown!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) UnknownLocation                   (Sender, Runtime = null)

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult UnknownLocation(System_Id  Sender,
                                                        TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.UnknownLocation,
                   Sender,
                   I18NString.Create("The charging location is unknown!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) InvalidSessionId                  (Sender, Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InvalidSessionId(System_Id  Sender,
                                                         TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.InvalidSessionId,
                   Sender,
                   I18NString.Create("The session identification is unknown or invalid!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) InvalidCredentials                (Sender, Runtime = null)

        /// <summary>
        /// Unauthorized RemoteStart or invalid credentials.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InvalidCredentials(System_Id  Sender,
                                                           TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.InvalidCredentials,
                   Sender,
                   I18NString.Create("Unauthorized RemoteStart or invalid credentials!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) NoEVConnectedToEVSE               (Sender, Runtime = null)

        /// <summary>
        /// No electric vehicle connected to EVSE.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult NoEVConnectedToEVSE(System_Id  Sender,
                                                            TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.NoEVConnectedToEVSE,
                   Sender,
                   I18NString.Create("No electric vehicle connected to EVSE!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) AlreadyInUse                      (Sender, Runtime = null)

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult AlreadyInUse(System_Id  Sender,
                                                     TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.AlreadyInUse,
                   Sender,
                   I18NString.Create("The EVSE is already in use!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) InternalUse                       (Sender, Runtime = null)

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InternalUse(System_Id  Sender,
                                                    TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.InternalUse,
                   Sender,
                   I18NString.Create("Reserved for internal use!"), 
                   Runtime: Runtime
               );

        #endregion

        #region (static) OutOfService                      (Sender, Runtime = null)

        /// <summary>
        /// The EVSE is out-of-service.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult OutOfService(System_Id  Sender,
                                                     TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.OutOfService,
                   Sender,
                   I18NString.Create("The EVSE or charging station is out of service!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) Offline                           (Sender, Runtime = null)

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Offline(System_Id  Sender,
                                                TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.Offline,
                   Sender,
                   I18NString.Create("The EVSE or charging station is offline!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) Reserved                          (Sender,          Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is reserved.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Reserved(System_Id    Sender,
                                                 I18NString?  Description      = null,
                                                 String?      AdditionalInfo   = null,
                                                 TimeSpan?    Runtime          = null)

            => new (
                   RemoteStartResultTypes.Reserved,
                   Sender,
                   Description ?? I18NString.Create("The EVSE or charging station is reserved!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );

        #endregion

        #region (static) Success                           (Session, Sender,                                            Runtime = null)

        /// <summary>
        /// The RemoteStart was successful and a charging session
        /// will be embedded within the response.
        /// </summary>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Success(ChargingSession  Session,
                                                System_Id        Sender,
                                                TimeSpan?        Runtime  = null)

            => new (
                   RemoteStartResultTypes.Success,
                   Sender,
                   Session: Session,
                   Runtime: Runtime
               );

        #endregion

        #region (static) AsyncOperation                    (Session, Sender, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The RemoteStart was successful.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult AsyncOperation(ChargingSession  Session,
                                                       System_Id        Sender,
                                                       I18NString?      Description      = null,
                                                       String?          AdditionalInfo   = null,
                                                       TimeSpan?        Runtime          = null)

            => new (
                   RemoteStartResultTypes.AsyncOperation,
                   Sender,
                   Description ?? I18NString.Create("An async RemoteStart was sent successfully!"),
                   AdditionalInfo,
                   Session,
                   Runtime: Runtime
               );

        #endregion

        #region (static) SuccessPlugInCableToStartCharging (Session, Sender, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The RemoteStart was successful. Please plug in the cable to start charging!
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult SuccessPlugInCableToStartCharging(ChargingSession  Session,
                                                                          System_Id        Sender,
                                                                          I18NString?      Description      = null,
                                                                          String?          AdditionalInfo   = null,
                                                                          TimeSpan?        Runtime          = null)

            => new (
                   RemoteStartResultTypes.SuccessPlugInCableToStartCharging,
                   Sender,
                   Description ?? I18NString.Create("The RemoteStart was successful. Please plug in the cable to start charging!"),
                   AdditionalInfo,
                   Session,
                   Runtime: Runtime
               );

        #endregion

        #region (static) Rejected                          (Sender,          Description = null,                        Runtime = null)

        /// <summary>
        /// The RemoteStart request was REJECTED.
        /// This is the same as the generic OCPP RequestStartStopStatus 'REJECTED',
        /// therefore we do not have any further information why the request was rejected.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="Description">An optional error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Rejected(System_Id    Sender,
                                                 I18NString?  Description   = null,
                                                 TimeSpan?    Runtime       = null)

            => new (
                   RemoteStartResultTypes.Rejected,
                   Sender,
                   Description ?? I18NString.Create("The remote start request was rejected!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) Timeout                           (Sender,          Description = null,                        Runtime = null)

        /// <summary>
        /// The RemoteStart request ran into a timeout.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="Description">An optional error message.</param>
        public static RemoteStartResult Timeout(System_Id    Sender,
                                                I18NString?  Description   = null,
                                                TimeSpan?    Runtime       = null)

            => new (
                   RemoteStartResultTypes.Timeout,
                   Sender,
                   Description ?? I18NString.Create("A timeout occured!"),
                   Runtime: Runtime
               );

        #endregion

        #region (static) CommunicationError                (Sender,          Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult CommunicationError(System_Id    Sender,
                                                           I18NString?  Description      = null,
                                                           String?      AdditionalInfo   = null,
                                                           TimeSpan?    Runtime          = null)

            => new (
                   RemoteStartResultTypes.CommunicationError,
                   Sender,
                   Description ?? I18NString.Create("A communication error occured!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );

        #endregion

        #region (static) Error                             (Sender,          Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The RemoteStart request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Error(System_Id  Sender,
                                              String?    Description      = null,
                                              String?    AdditionalInfo   = null,
                                              TimeSpan?  Runtime          = null)

            => new (
                   RemoteStartResultTypes.Error,
                   Sender,
                   Description?.ToI18NString() ?? I18NString.Create("An error occured!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );


        /// <summary>
        /// The RemoteStart request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Error(System_Id    Sender,
                                              I18NString?  Description,
                                              String?      AdditionalInfo   = null,
                                              TimeSpan?    Runtime          = null)

            => new (
                   RemoteStartResultTypes.Error,
                   Sender,
                   Description ?? I18NString.Create("An error occured!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );


        /// <summary>
        /// The RemoteStart request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Error(String     Description,
                                              System_Id  Sender,
                                              String?    AdditionalInfo   = null,
                                              TimeSpan?  Runtime          = null)

            => new (
                   RemoteStartResultTypes.Error,
                   Sender,
                   Description?.Trim().IsNotNullOrEmpty() == false
                       ? I18NString.Create(Description)
                       : I18NString.Create("An error occured!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );

        #endregion

        #region (static) Exception                         (Sender,          Exception,                                 Runtime = null)

        /// <summary>
        /// The RemoteStart request led to an exception.
        /// </summary>
        /// <param name="Exception">An exception.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Exception(System_Id  Sender,
                                                  Exception  Exception,
                                                  TimeSpan?  Runtime   = null)

            => new (
                   RemoteStartResultTypes.Error,
                   Sender,
                   I18NString.Create(Exception.Message),
                   Exception.StackTrace,
                   Runtime: Runtime
               );

        #endregion

        #region (static) NoOperation                       (Sender,          Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The RemoteStart request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult NoOperation(System_Id    Sender,
                                                    I18NString?  Description      = null,
                                                    String?      AdditionalInfo   = null,
                                                    TimeSpan?    Runtime          = null)

            => new (
                   RemoteStartResultTypes.Error,
                   Sender,
                   Description ?? I18NString.Create("An error occured!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );


        /// <summary>
        /// The RemoteStart request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the RemoteStart result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult NoOperation(String     Description,
                                                    System_Id  Sender,
                                                    String?    AdditionalInfo   = null,
                                                    TimeSpan?  Runtime          = null)

            => new (
                   RemoteStartResultTypes.Error,
                   Sender,
                   Description?.Trim().IsNotNullOrEmpty() == false
                       ? I18NString.Create(Description)
                       : I18NString.Create("An error occured!"),
                   AdditionalInfo,
                   Runtime: Runtime
               );

        #endregion


        #region ToJSON(ResponseMapper = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="ResponseMapper">An optional response mapper delegate.</param>
        public JObject ToJSON(Func<JObject, JObject>? ResponseMapper = null)
        {

            var JSON = JSONObject.Create(

                new JProperty("result",                 Result.     ToString()),

                Session is not null
                    ? new JProperty("session",          Session.    ToString())
                    : null,

                Description.IsNotNullOrEmpty()
                    ? new JProperty("description",      Description.ToJSON())
                    : null,

                AdditionalInfo.IsNotNullOrEmpty()
                    ? new JProperty("additionalInfo",   AdditionalInfo)
                    : null,

                      new JProperty("runtime",          Math.Round(Runtime.TotalMilliseconds, 0))

            );

            return ResponseMapper is not null
                       ? ResponseMapper(JSON)
                       : JSON;

        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Result.ToString() +
               (Description.IsNotNullOrEmpty() ? ": " + Description.FirstText() : "");

        #endregion

    }


    /// <summary>
    /// Extensions methods for RemoteStart result types.
    /// </summary>
    public static class RemoteStartResultTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a RemoteStart result type.
        /// </summary>
        /// <param name="Text">A text representation of a RemoteStart result type.</param>
        public static RemoteStartResultTypes Parse(String Text)
        {

            if (TryParse(Text, out var remoteStartResultType))
                return remoteStartResultType;

            throw new ArgumentException("Undefined RemoteStart result type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a RemoteStart result type.
        /// </summary>
        /// <param name="Text">A text representation of a RemoteStart result type.</param>
        public static RemoteStartResultTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var remoteStartResultType))
                return remoteStartResultType;

            return default;

        }

        #endregion

        #region TryParse(Text, out RemoteStartResultType)

        /// <summary>
        /// Parses the given text representation of a RemoteStart result type.
        /// </summary>
        /// <param name="Text">A text representation of a RemoteStart result type.</param>
        /// <param name="RemoteStartResultType">The parsed RemoteStart result type.</param>
        public static Boolean TryParse(String Text, out RemoteStartResultTypes RemoteStartResultType)
        {
            switch (Text?.Trim())
            {

                case "unknownOperator":
                    RemoteStartResultType = RemoteStartResultTypes.UnknownOperator;
                    return true;

                case "unknownLocation":
                    RemoteStartResultType = RemoteStartResultTypes.UnknownLocation;
                    return true;

                case "invalidSessionId":
                    RemoteStartResultType = RemoteStartResultTypes.InvalidSessionId;
                    return true;

                case "invalidCredentials":
                    RemoteStartResultType = RemoteStartResultTypes.InvalidCredentials;
                    return true;

                case "noEVConnectedToEVSE":
                    RemoteStartResultType = RemoteStartResultTypes.NoEVConnectedToEVSE;
                    return true;

                case "alreadyInUse":
                    RemoteStartResultType = RemoteStartResultTypes.AlreadyInUse;
                    return true;

                case "outOfService":
                    RemoteStartResultType = RemoteStartResultTypes.OutOfService;
                    return true;

                case "offline":
                    RemoteStartResultType = RemoteStartResultTypes.Offline;
                    return true;

                case "reserved":
                    RemoteStartResultType = RemoteStartResultTypes.Reserved;
                    return true;

                case "success":
                    RemoteStartResultType = RemoteStartResultTypes.Success;
                    return true;

                case "asyncOperation":
                    RemoteStartResultType = RemoteStartResultTypes.AsyncOperation;
                    return true;

                case "successPlugInCableToStartCharging":
                    RemoteStartResultType = RemoteStartResultTypes.SuccessPlugInCableToStartCharging;
                    return true;

                case "timeout":
                    RemoteStartResultType = RemoteStartResultTypes.Timeout;
                    return true;

                case "communicationError":
                    RemoteStartResultType = RemoteStartResultTypes.CommunicationError;
                    return true;

                case "error":
                    RemoteStartResultType = RemoteStartResultTypes.Error;
                    return true;

                case "noOperation":
                    RemoteStartResultType = RemoteStartResultTypes.NoOperation;
                    return true;

                default:
                    RemoteStartResultType = RemoteStartResultTypes.Unspecified;
                    return false;

            }
        }

        #endregion

        #region AsString(this RemoteStartResultType)

        /// <summary>
        /// Return a text representation of the given RemoteStart result type.
        /// </summary>
        /// <param name="RemoteStartResultType">An RemoteStart result type.</param>
        public static String AsString(this RemoteStartResultTypes RemoteStartResultType)

            => RemoteStartResultType switch {
                   RemoteStartResultTypes.UnknownOperator                    => "unknownOperator",
                   RemoteStartResultTypes.UnknownLocation                    => "unknownLocation",
                   RemoteStartResultTypes.InvalidSessionId                   => "invalidSessionId",
                   RemoteStartResultTypes.InvalidCredentials                 => "invalidCredentials",
                   RemoteStartResultTypes.NoEVConnectedToEVSE                => "noEVConnectedToEVSE",
                   RemoteStartResultTypes.AlreadyInUse                       => "alreadyInUse",
                   RemoteStartResultTypes.InternalUse                        => "internalUse",
                   RemoteStartResultTypes.OutOfService                       => "outOfService",
                   RemoteStartResultTypes.Offline                            => "offline",
                   RemoteStartResultTypes.Reserved                           => "reserved",
                   RemoteStartResultTypes.Success                            => "success",
                   RemoteStartResultTypes.AsyncOperation                     => "asyncOperation",
                   RemoteStartResultTypes.SuccessPlugInCableToStartCharging  => "successPlugInCableToStartCharging",
                   RemoteStartResultTypes.Timeout                            => "timeout",
                   RemoteStartResultTypes.CommunicationError                 => "communicationError",
                   RemoteStartResultTypes.Error                              => "error",
                   RemoteStartResultTypes.NoOperation                        => "noOperation",
                   _                                                         => "unspecified",
               };

        #endregion

    }


    /// <summary>
    /// The result types of a RemoteStart operation at an EVSE.
    /// </summary>
    public enum RemoteStartResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,


        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        UnknownOperator,

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,


        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Unauthorized RemoteStart or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// No electric vehicle connected to EVSE.
        /// </summary>
        NoEVConnectedToEVSE,

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        AlreadyInUse,

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        InternalUse,

        /// <summary>
        /// The EVSE is out-of-service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        Offline,

        /// <summary>
        /// The EVSE or charging station is reserved.
        /// </summary>
        Reserved,

        /// <summary>
        /// The RemoteStart was successful.
        /// </summary>
        Success,

        /// <summary>
        /// An async RemoteStart was sent successfully.
        /// </summary>
        AsyncOperation,

        /// <summary>
        /// The RemoteStart was successful. Please plug in the cable to start charging!
        /// </summary>
        SuccessPlugInCableToStartCharging,

        /// <summary>
        /// The RemoteStart request was REJECTED.
        /// This is the same as the generic OCPP RequestStartStopStatus 'REJECTED',
        /// therefore we do not have any further information why the request was rejected.
        /// </summary>
        Rejected,

        /// <summary>
        /// The RemoteStart request ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A bad request was sent.
        /// </summary>
        BadRequest,

        /// <summary>
        /// Unauthorized request.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The RemoteStart request led to an error.
        /// </summary>
        Error,

        /// <summary>
        /// No operation.
        /// </summary>
        NoOperation

    }

}
