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
    /// The result of a remote stop operation.
    /// </summary>
    public class RemoteStopResult
    {

        #region Properties

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopResultTypes    Result                   { get; }

        /// <summary>
        /// The charging session identification, e.g. in case of an unknown/invalid remote stop request.
        /// </summary>
        public ChargingSession_Id       SessionId                { get; }

        /// <summary>
        /// The charging session identification for an invalid remote stop operation.
        /// </summary>
        public ChargingSession?         ChargingSession        { get; }

        /// <summary>
        /// A optional description of the authorize stop result.
        /// </summary>
        public I18NString               Description              { get; }

        /// <summary>
        /// An optional additional message.
        /// </summary>
        public String?                  AdditionalInfo           { get; }

        /// <summary>
        /// The charging reservation identification.
        /// </summary>
        public ChargingReservation_Id?  ReservationId            { get; }

        /// <summary>
        /// The handling of the charging reservation after the charging session stopped.
        /// </summary>
        public ReservationHandling      ReservationHandling      { get; }

        /// <summary>
        /// The charge detail record for a successfully stopped charging process.
        /// </summary>
        public ChargeDetailRecord?      ChargeDetailRecord       { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan                 Runtime                  { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new remote stop result.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Result">The result of the remote stop request.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="ChargeDetailRecord">An optional charge detail record for a successfully stopped charging process.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        private RemoteStopResult(ChargingSession_Id       SessionId,
                                 RemoteStopResultTypes    Result,
                                 I18NString?              Description           = null,
                                 String?                  AdditionalInfo        = null,
                                 ChargingSession?         ChargingSession       = null,
                                 ChargingReservation_Id?  ReservationId         = null,
                                 ReservationHandling?     ReservationHandling   = null,
                                 ChargeDetailRecord?      ChargeDetailRecord    = null,
                                 TimeSpan?                Runtime               = null)
        {

            this.SessionId            = SessionId;
            this.Result               = Result;
            this.Description          = Description         ?? I18NString.Empty;
            this.AdditionalInfo       = AdditionalInfo;

            this.ChargingSession      = ChargingSession;
            this.ReservationId        = ReservationId;
            this.ReservationHandling  = ReservationHandling ?? WWCP.ReservationHandling.Close;
            this.ChargeDetailRecord   = ChargeDetailRecord;
            this.Runtime              = Runtime             ?? TimeSpan.Zero;

        }

        #endregion


        #region (static) Unspecified       (SessionId)

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult Unspecified(ChargingSession_Id SessionId)

            => new (SessionId,
                    RemoteStopResultTypes.Unspecified);

        #endregion

        #region (static) UnknownOperator   (SessionId, Runtime = null)

        /// <summary>
        /// The charging station operator is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult UnknownOperator(ChargingSession_Id  SessionId,
                                                       TimeSpan?           Runtime = null)

            => new (SessionId,
                    RemoteStopResultTypes.UnknownOperator,
                    I18NString.Create("The EVSE or charging station operator is unknown!"),
                    Runtime: Runtime);

        #endregion

        #region (static) UnknownLocation   (SessionId, Runtime = null)

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult UnknownLocation(ChargingSession_Id  SessionId,
                                                        TimeSpan?          Runtime = null)

            => new (SessionId,
                    RemoteStopResultTypes.UnknownLocation,
                    I18NString.Create("The charging location is unknown!"),
                    Runtime: Runtime);

        #endregion

        #region (static) InvalidSessionId  (SessionId, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult InvalidSessionId(ChargingSession_Id  SessionId,
                                                        String              AdditionalInfo   = null,
                                                        TimeSpan?           Runtime          = null)

            => new (SessionId:       SessionId,
                    Result:          RemoteStopResultTypes.InvalidSessionId,
                    Description:     I18NString.Create("The session identification is unknown or invalid!"),
                    AdditionalInfo:  AdditionalInfo,
                    Runtime:         Runtime);

        #endregion

        #region (static) InvalidCredentials(SessionId, Runtime = null)

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult InvalidCredentials(ChargingSession_Id  SessionId,
                                                          TimeSpan?           Runtime  = null)

            => new (SessionId,
                    RemoteStopResultTypes.InvalidCredentials,
                    I18NString.Create("Unauthorized remote stop or invalid credentials!"),
                    Runtime: Runtime);

        #endregion

        #region (static) InternalUse       (SessionId, Runtime = null)

        /// <summary>
        /// Reserved for internal use!
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult InternalUse(ChargingSession_Id  SessionId,
                                                   TimeSpan?           Runtime  = null)

            => new (SessionId,
                    RemoteStopResultTypes.InternalUse,
                    I18NString.Create("Reserved for internal use!"),
                    Runtime: Runtime);

        #endregion

        #region (static) OutOfService      (SessionId, Runtime = null)

        /// <summary>
        /// The  is out of service.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult OutOfService(ChargingSession_Id  SessionId,
                                                    TimeSpan?           Runtime  = null)

            => new (SessionId,
                    RemoteStopResultTypes.OutOfService,
                    I18NString.Create("The EVSE or charging station is out of service!"),
                    Runtime: Runtime);

        #endregion

        #region (static) Offline           (SessionId, Runtime = null)

        /// <summary>
        /// The  is offline.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Offline(ChargingSession_Id  SessionId,
                                               TimeSpan?           Runtime  = null)

            => new (SessionId,
                    RemoteStopResultTypes.Offline,
                    I18NString.Create("The EVSE or charging station is offline!"),
                    Runtime: Runtime);

        #endregion

        #region (static) AlreadyStopped    (SessionId, ..., Runtime = null)

        /// <summary>
        /// A previous remote stop was alredy successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult AlreadyStopped(ChargingSession_Id  SessionId,
                                                      String              AdditionalInfo   = null,
                                                      TimeSpan?           Runtime          = null)

            => new (SessionId,
                    RemoteStopResultTypes.AlreadyStopped,
                    I18NString.Create("The charging process was already stopped!"),
                    AdditionalInfo,
                    Runtime: Runtime);

        #endregion

        #region (static) Success           (SessionId, ..., ChargeDetailRecord, Runtime = null)

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="ChargeDetailRecord">The charge detail record for a successfully stopped charging process.</param>
        /// <param name="ReservationId">The optional charging reservation identification of the charging session.</param>
        /// <param name="ReservationHandling">The handling of the charging reservation after the charging session stopped.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Success(ChargingSession_Id       SessionId,
                                               ChargingSession?         Session               = null,
                                               I18NString?              Description           = null,
                                               String?                  AdditionalInfo        = null,
                                               ChargingReservation_Id?  ReservationId         = null,
                                               ReservationHandling?     ReservationHandling   = null,
                                               ChargeDetailRecord?      ChargeDetailRecord    = null,
                                               TimeSpan?                Runtime               = null)

            => new (SessionId,
                    RemoteStopResultTypes.Success,
                    Description,
                    AdditionalInfo,
                    Session,
                    ReservationId,
                    ReservationHandling,
                    ChargeDetailRecord,
                    Runtime);

        #endregion

        #region (static) AsyncOperation    (SessionId, Description = null, ..., Runtime = null)

        /// <summary>
        /// An async remote stop was sent successfully.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult AsyncOperation(ChargingSession_Id  SessionId,
                                                      I18NString          Description      = null,
                                                      String              AdditionalInfo   = null,
                                                      TimeSpan?           Runtime          = null)

            => new (SessionId,
                    RemoteStopResultTypes.AsyncOperation,
                    Description ?? I18NString.Create("An async remote stop was sent successfully!"),
                    AdditionalInfo,
                    Runtime: Runtime);

        #endregion

        #region (static) Timeout           (SessionId, Description = null,                        Runtime = null)

        /// <summary>
        /// The remote stop ran into a timeout.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">An optional error message.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Timeout(ChargingSession_Id  SessionId,
                                               I18NString          Description   = null,
                                               TimeSpan?           Runtime       = null)

            => new (SessionId,
                    RemoteStopResultTypes.Timeout,
                    Description ?? I18NString.Create("A timeout occured!"),
                    Runtime: Runtime);

        #endregion

        #region (static) CommunicationError(SessionId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop led to a communication error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">An optional error message.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult CommunicationError(ChargingSession_Id  SessionId,
                                                          I18NString          Description      = null,
                                                          String              AdditionalInfo   = null,
                                                          TimeSpan?           Runtime          = null)

            => new (SessionId,
                    RemoteStopResultTypes.CommunicationError,
                    Description ?? I18NString.Create("A communication error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);

        #endregion

        #region (static) Error             (SessionId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Error(ChargingSession_Id  SessionId,
                                             I18NString?         Description      = null,
                                             String?             AdditionalInfo   = null,
                                             TimeSpan?           Runtime          = null)

            => new (SessionId,
                    RemoteStopResultTypes.Error,
                    Description ?? I18NString.Create("An error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);


        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        /// <param name="Description">A optional description of the remote stop result.</param>
        /// <param name="AdditionalInfo">An optional additional information on this error, e.g. the HTTP error response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public static RemoteStopResult Error(ChargingSession_Id  SessionId,
                                             String?             Description,
                                             String?             AdditionalInfo   = null,
                                             TimeSpan?           Runtime          = null)

            => new (SessionId,
                    RemoteStopResultTypes.Error,
                    Description?.Trim().IsNotNullOrEmpty() == false
                         ? I18NString.Create(Description)
                         : I18NString.Create("An error occured!"),
                    AdditionalInfo,
                    Runtime: Runtime);

        #endregion

        #region (static) NoOperation             (SessionId, Description = null, AdditionalInfo = null, Runtime = null)

        /// <summary>
        /// The remote stop led to an error.
        /// </summary>
        /// <param name="SessionId">The unique charging session identification.</param>
        public static RemoteStopResult NoOperation(ChargingSession_Id  SessionId)

            => new (SessionId,
                    RemoteStopResultTypes.NoOperation);

        #endregion


        #region ToJSON(ResponseMapper = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="ResponseMapper">An optional response mapper delegate.</param>
        public JObject ToJSON(Boolean                                              Embedded                             = false,
                              CustomJObjectSerializerDelegate<ChargeDetailRecord>  CustomChargeDetailRecordSerializer   = null,
                              Func<JObject, JObject>                               ResponseMapper                       = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("sessionId",            SessionId.          ToString()),

                                 new JProperty("result",               Result.             ToString()),

                           Description.IsNotNullOrEmpty()
                               ? new JProperty("description",          Description.        ToJSON())
                               : null,

                           AdditionalInfo.IsNotNullOrEmpty()
                               ? new JProperty("additionalInfo",       AdditionalInfo)
                               : null,

                           ReservationId.HasValue
                               ? new JProperty("reservationId",        ReservationId.      ToString())
                               : null,

                           new JProperty("reservationHandling",        ReservationHandling.ToString()),

                           ChargeDetailRecord is not null
                               ? new JProperty("chargeDetailRecord",   ChargeDetailRecord.ToJSON(Embedded: false,
                                                                                                 CustomChargeDetailRecordSerializer))
                               : null,

                                 new JProperty("runtime",              Math.Round(Runtime.TotalMilliseconds, 0))

                       );

            return ResponseMapper is not null
                       ? ResponseMapper(json)
                       : json;

        }

        #endregion

        public static RemoteStopResult Parse(JObject JSON)
        {

            return new RemoteStopResult(ChargingSession_Id.Parse(JSON["sessionId"]?.Value<String>()),
                                        (RemoteStopResultTypes) Enum.Parse(typeof(RemoteStopResultTypes), JSON["result"]?.Value<String>(), true),
                                        JSON["description"] is JObject descriptionJSON ? I18NString.Parse(descriptionJSON) : null,
                                        JSON["additionalInfo"] != null ? JSON["additionalInfo"].Value<String>() : null,
                                        null,
                                        JSON["reservationId"] != null ? ChargingReservation_Id.Parse(JSON["reservationId"]?.Value<String>()) : new ChargingReservation_Id?(),
                                        null,
                                        null, //JSON["chargeDetailRecord"] != null ? ChargeDetailRecord.Parse(JSON["chargeDetailRecord"]) : null,
                                        JSON["runtime"] != null ? TimeSpan.FromMilliseconds(JSON["runtime"].Value<Double>()) : new TimeSpan?());

        }


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
    /// Extensions methods for remote stop result types.
    /// </summary>
    public static class RemoteStopResultTypesExtensions
    {

        #region Parse   (Text)

        /// <summary>
        /// Parses the given text representation of a remote stop result type.
        /// </summary>
        /// <param name="Text">A text representation of a remote stop result type.</param>
        public static RemoteStopResultTypes Parse(String Text)
        {

            if (TryParse(Text, out var remoteStartResultType))
                return remoteStartResultType;

            throw new ArgumentException("Undefined remote stop result type '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parses the given text representation of a remote stop result type.
        /// </summary>
        /// <param name="Text">A text representation of a remote stop result type.</param>
        public static RemoteStopResultTypes? TryParse(String Text)
        {

            if (TryParse(Text, out var remoteStartResultType))
                return remoteStartResultType;

            return default;

        }

        #endregion

        #region TryParse(Text, out RemoteStartResultType)

        /// <summary>
        /// Parses the given text representation of a remote stop result type.
        /// </summary>
        /// <param name="Text">A text representation of a remote stop result type.</param>
        /// <param name="RemoteStartResultType">The parsed remote stop result type.</param>
        public static Boolean TryParse(String Text, out RemoteStopResultTypes RemoteStartResultType)
        {
            switch (Text?.Trim())
            {

                case "unknownOperator":
                    RemoteStartResultType = RemoteStopResultTypes.UnknownOperator;
                    return true;

                case "unknownLocation":
                    RemoteStartResultType = RemoteStopResultTypes.UnknownLocation;
                    return true;

                case "invalidSessionId":
                    RemoteStartResultType = RemoteStopResultTypes.InvalidSessionId;
                    return true;

                case "invalidCredentials":
                    RemoteStartResultType = RemoteStopResultTypes.InvalidCredentials;
                    return true;

                case "outOfService":
                    RemoteStartResultType = RemoteStopResultTypes.OutOfService;
                    return true;

                case "offline":
                    RemoteStartResultType = RemoteStopResultTypes.Offline;
                    return true;

                case "success":
                    RemoteStartResultType = RemoteStopResultTypes.Success;
                    return true;

                case "asyncOperation":
                    RemoteStartResultType = RemoteStopResultTypes.AsyncOperation;
                    return true;

                case "timeout":
                    RemoteStartResultType = RemoteStopResultTypes.Timeout;
                    return true;

                case "communicationError":
                    RemoteStartResultType = RemoteStopResultTypes.CommunicationError;
                    return true;

                case "error":
                    RemoteStartResultType = RemoteStopResultTypes.Error;
                    return true;

                case "noOperation":
                    RemoteStartResultType = RemoteStopResultTypes.NoOperation;
                    return true;

                default:
                    RemoteStartResultType = RemoteStopResultTypes.Unspecified;
                    return false;

            }
        }

        #endregion

        #region AsString(this RemoteStartResultType)

        /// <summary>
        /// Return a text representation of the given remote stop result type.
        /// </summary>
        /// <param name="RemoteStartResultType">An remote stop result type.</param>
        public static String AsString(this RemoteStopResultTypes RemoteStartResultType)

            => RemoteStartResultType switch {
                   RemoteStopResultTypes.UnknownOperator     => "unknownOperator",
                   RemoteStopResultTypes.UnknownLocation     => "unknownLocation",
                   RemoteStopResultTypes.InvalidSessionId    => "invalidSessionId",
                   RemoteStopResultTypes.InvalidCredentials  => "invalidCredentials",
                   RemoteStopResultTypes.InternalUse         => "internalUse",
                   RemoteStopResultTypes.OutOfService        => "outOfService",
                   RemoteStopResultTypes.Offline             => "offline",
                   RemoteStopResultTypes.Success             => "success",
                   RemoteStopResultTypes.AsyncOperation      => "asyncOperation",
                   RemoteStopResultTypes.Timeout             => "timeout",
                   RemoteStopResultTypes.CommunicationError  => "communicationError",
                   RemoteStopResultTypes.Error               => "error",
                   RemoteStopResultTypes.NoOperation         => "noOperation",
                   _                                         => "unspecified",
               };

        #endregion

    }


    /// <summary>
    /// The result types of a remote stop operation.
    /// </summary>
    public enum RemoteStopResultTypes
    {

        /// <summary>
        /// The result is unknown and/or should be ignored.
        /// </summary>
        Unspecified,


        /// <summary>
        /// The EVSE or charging station operator is unknown.
        /// </summary>
        UnknownOperator,

        /// <summary>
        /// The charging location is unknown.
        /// </summary>
        UnknownLocation,

        /// <summary>
        /// The charging session identification is unknown or invalid.
        /// </summary>
        InvalidSessionId,

        /// <summary>
        /// Unauthorized remote stop or invalid credentials.
        /// </summary>
        InvalidCredentials,

        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        InternalUse,

        /// <summary>
        /// The EVSE or charging station is out of service.
        /// </summary>
        OutOfService,

        /// <summary>
        /// The EVSE or charging station is offline.
        /// </summary>
        Offline,

        /// <summary>
        /// A previous remote stop was alredy successful.
        /// </summary>
        AlreadyStopped,

        /// <summary>
        /// The remote stop was successful.
        /// </summary>
        Success,

        /// <summary>
        /// An async remote stop was sent successfully.
        /// </summary>
        AsyncOperation,

        /// <summary>
        /// The remote stop ran into a timeout.
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
        /// The remote stop led to an error.
        /// </summary>
        Error,

        NoOperation

    }

}
