/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// The result of a remote start operation.
    /// </summary>
    public class RemoteStartResult
    {

        #region Properties

        /// <summary>
        /// The result of a remote start operation.
        /// </summary>
        public RemoteStartResultTypes  Result            { get; }

        /// <summary>
        /// The charging session for the remote start operation.
        /// </summary>
        public ChargingSession?        Session           { get; }

        /// <summary>
        /// A optional description of the remote start result.
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
        /// Create a new remote start result.
        /// </summary>
        /// <param name="Result">The result of the remote start operation.</param>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStartResult(RemoteStartResultTypes  Result,
                                  I18NString?             Description      = null,
                                  String?                 AdditionalInfo   = null,
                                  ChargingSession?        Session          = null,
                                  TimeSpan?               Runtime          = null)
        {

            this.Result          = Result;
            this.Session         = Session;
            this.Description     = Description ?? I18NString.Empty;
            this.AdditionalInfo  = AdditionalInfo;
            this.Runtime         = Runtime     ?? TimeSpan.Zero;

        }

        #endregion


        #region (static) Unspecified

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        public static RemoteStartResult Unspecified

            => new RemoteStartResult(RemoteStartResultTypes.Unspecified);

        #endregion

        #region (static) UnknownOperator    (Runtime = null)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult UnknownOperator(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.UnknownOperator,
                                     I18NString.Create(Languages.en, "The EVSE or charging station operator is unknown!"), 
                                     Runtime: Runtime);

        #endregion

        #region (static) UnknownLocation    (Runtime = null)

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult UnknownLocation(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.UnknownLocation,
                                     I18NString.Create(Languages.en, "The charging location is unknown!"), 
                                     Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId   (Runtime = null)

        /// <summary>
        /// The given charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InvalidSessionId(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.InvalidSessionId,
                                     I18NString.Create(Languages.en, "The session identification is unknown or invalid!"), 
                                     Runtime: Runtime);

        #endregion

        #region (static) InvalidCredentials (Runtime = null)

        /// <summary>
        /// Unauthorized remote start or invalid credentials.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InvalidCredentials(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.InvalidCredentials,
                                     I18NString.Create(Languages.en, "Unauthorized remote start or invalid credentials!"),
                                     Runtime: Runtime);

        #endregion

        #region (static) NoEVConnectedToEVSE(Runtime = null)

        /// <summary>
        /// No electric vehicle connected to EVSE.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult NoEVConnectedToEVSE(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.NoEVConnectedToEVSE,
                                     I18NString.Create(Languages.en, "No electric vehicle connected to EVSE!"),
                                     Runtime: Runtime);

        #endregion

        #region (static) AlreadyInUse       (Runtime = null)

        /// <summary>
        /// The EVSE is already in use.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult AlreadyInUse(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.AlreadyInUse,
                                     I18NString.Create(Languages.en, "The EVSE is already in use!"),
                                     Runtime: Runtime);

        #endregion

        #region (static) InternalUse        (Runtime = null)

        /// <summary>
        /// The EVSE is reserved for internal use.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult InternalUse(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.InternalUse,
                                     I18NString.Create(Languages.en, "Reserved for internal use!"), 
                                     Runtime: Runtime);

        #endregion

        #region (static) OutOfService       (Runtime = null)

        /// <summary>
        /// The EVSE is out-of-service.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult OutOfService(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.OutOfService,
                                     I18NString.Create(Languages.en, "The EVSE or charging station is out of service!"),
                                     Runtime: Runtime);

        #endregion

        #region (static) Offline            (Runtime = null)

        /// <summary>
        /// The EVSE is offline.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Offline(TimeSpan? Runtime = null)

            => new RemoteStartResult(RemoteStartResultTypes.Offline,
                                     I18NString.Create(Languages.en, "The EVSE or charging station is offline!"),
                                     Runtime: Runtime);

        #endregion

        #region (static) Reserved                         (         Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The EVSE or charging station is reserved.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Reserved(I18NString  Description      = null,
                                                 String      AdditionalInfo   = null,
                                                 TimeSpan?   Runtime          = null)

            => new RemoteStartResult(RemoteStartResultTypes.Reserved,
                                     Description ?? I18NString.Create(Languages.en, "The EVSE or charging station is reserved!"),
                                     AdditionalInfo,
                                     Runtime: Runtime);

        #endregion

        #region (static) Success                          (Session,                                            Runtime = null)

        /// <summary>
        /// The remote start was successful and a charging session
        /// will be embedded within the response.
        /// </summary>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Success(ChargingSession  Session,
                                                TimeSpan?        Runtime  = null)

            => new RemoteStartResult(RemoteStartResultTypes.Success,
                                     Session: Session,
                                     Runtime: Runtime);

        #endregion

        #region (static) AsyncOperation                   (Session, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote start was successful.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult AsyncOperation(ChargingSession  Session,
                                                       I18NString?      Description      = null,
                                                       String?          AdditionalInfo   = null,
                                                       TimeSpan?        Runtime          = null)

            => new (RemoteStartResultTypes.AsyncOperation,
                    Description ?? I18NString.Create(Languages.en, "An async remote start was sent successfully!"),
                    AdditionalInfo,
                    Session,
                    Runtime: Runtime);

        #endregion

        #region (static) SuccessPlugInCableToStartCharging(Session, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote start was successful. Please plug in the cable to start charging!
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Session">The charging session.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult SuccessPlugInCableToStartCharging(ChargingSession  Session,
                                                                          I18NString?      Description      = null,
                                                                          String?          AdditionalInfo   = null,
                                                                          TimeSpan?        Runtime          = null)

            => new (RemoteStartResultTypes.SuccessPlugInCableToStartCharging,
                    Description ?? I18NString.Create(Languages.en, "The remote start was successful. Please plug in the cable to start charging!"),
                    AdditionalInfo,
                    Session,
                    Runtime: Runtime);

        #endregion

        #region (static) Timeout                          (         Description = null,                        Runtime = null)

        /// <summary>
        /// The remote start request ran into a timeout.
        /// </summary>
        /// <param name="Runtime">The runtime of the request.</param>
        /// <param name="Description">An optional error message.</param>
        public static RemoteStartResult Timeout(I18NString?  Description   = null,
                                                TimeSpan?    Runtime       = null)

            => new (RemoteStartResultTypes.Timeout,
                    Description ?? I18NString.Create(Languages.en, "A timeout occured!"),
                    Runtime: Runtime);

        #endregion

        #region (static) CommunicationError               (         Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// A communication error occured.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult CommunicationError(I18NString?  Description      = null,
                                                           String?      AdditionalInfo   = null,
                                                           TimeSpan?    Runtime          = null)

            => new (RemoteStartResultTypes.CommunicationError,
                    Description ?? I18NString.Create(Languages.en, "A communication error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);

        #endregion

        #region (static) Error                            (         Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Error(I18NString?  Description      = null,
                                              String?      AdditionalInfo   = null,
                                              TimeSpan?    Runtime          = null)

            => new (RemoteStartResultTypes.Error,
                    Description ?? I18NString.Create(Languages.en, "An error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);


        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult Error(String     Description,
                                              String?    AdditionalInfo   = null,
                                              TimeSpan?  Runtime          = null)

            => new (RemoteStartResultTypes.Error,
                    Description?.Trim().IsNotNullOrEmpty() == false
                        ? I18NString.Create(Languages.en, Description)
                        : I18NString.Create(Languages.en, "An error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);

        #endregion

        #region (static) NoOperation                      (         Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult NoOperation(I18NString?  Description      = null,
                                                    String?      AdditionalInfo   = null,
                                                    TimeSpan?    Runtime          = null)

            => new (RemoteStartResultTypes.Error,
                    Description ?? I18NString.Create(Languages.en, "An error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);


        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        /// <param name="Description">A optional description of the remote start result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStartResult NoOperation(String     Description,
                                                    String?    AdditionalInfo   = null,
                                                    TimeSpan?  Runtime          = null)

            => new (RemoteStartResultTypes.Error,
                    Description?.Trim().IsNotNullOrEmpty() == false
                        ? I18NString.Create(Languages.en, Description)
                        : I18NString.Create(Languages.en, "An error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);

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

                Description.IsNeitherNullNorEmpty()
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
               (Description.IsNeitherNullNorEmpty() ? ": " + Description.FirstText() : "");

        #endregion

    }


    /// <summary>
    /// The result types of a remote start operation at an EVSE.
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
        /// Unauthorized remote start or invalid credentials.
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
        /// The remote start was successful.
        /// </summary>
        Success,

        /// <summary>
        /// An async remote start was sent successfully.
        /// </summary>
        AsyncOperation,

        /// <summary>
        /// The remote start was successful. Please plug in the cable to start charging!
        /// </summary>
        SuccessPlugInCableToStartCharging,

        /// <summary>
        /// The remote start request ran into a timeout.
        /// </summary>
        Timeout,

        /// <summary>
        /// A communication error occured.
        /// </summary>
        CommunicationError,

        /// <summary>
        /// The remote start request led to an error.
        /// </summary>
        Error,

        NoOperation

    }

}
