/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com> <achim.friedland@graphdefined.com>
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
    /// Defines if a reservation can be used for consecutive charging sessions.
    /// </summary>
    public readonly struct ReservationHandling
    {

        #region Properties

        /// <summary>
        /// The reservation should not end after the remote stop operation.
        /// </summary>
        public Boolean   IsKeepAlive
            => EndTime > Timestamp.Now;

        /// <summary>
        /// The time span in which the reservation can be used
        /// for additional charging sessions.
        /// </summary>
        public TimeSpan  KeepAliveTime    { get; }

        /// <summary>
        /// The timestamp when the reservation will expire.
        /// </summary>
        public DateTime  EndTime          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new reservation handling.
        /// </summary>
        /// <param name="KeepAliveTime">The timespan in which the reservation can be used for additional charging sessions (relative time, higher priority).</param>
        /// <param name="EndTime">The timestamp after which the reservation can no longer be used for additional charging sessions (absolute time).</param>
        private ReservationHandling(TimeSpan?  KeepAliveTime   = null,
                                    DateTime?  EndTime         = null)
        {

            var now = Timestamp.Now;

            if (KeepAliveTime.HasValue &&
                KeepAliveTime.Value.TotalSeconds > 0)
            {
                this.KeepAliveTime  = KeepAliveTime.Value;
                this.EndTime        = EndTime ?? (now + KeepAliveTime.Value);
            }

            else
            {
                this.EndTime        = EndTime ?? now;
                this.KeepAliveTime  = this.EndTime - now;
            }

        }

        #endregion


        #region (static) Parse   (JSON, CustomReservationHandlingParser = null)

        /// <summary>
        /// Parse the given JSON representation of a reservation handling.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="CustomReservationHandlingParser">A delegate to parse custom reservation handling JSON objects.</param>
        public static ReservationHandling Parse(JObject                                            JSON,
                                                CustomJObjectParserDelegate<ReservationHandling>?  CustomReservationHandlingParser   = null)
        {

            if (TryParse(JSON,
                         out var reservationHandling,
                         out var errorResponse,
                         CustomReservationHandlingParser) &&
                reservationHandling.HasValue)
            {
                return reservationHandling.Value;
            }

            throw new ArgumentException("The given JSON representation of a reservation handling is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out ReservationHandling, out ErrorResponse, CustomReservationHandlingParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a RemoteStop response.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ReservationHandling">The parsed RemoteStop response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                   JSON,
                                       out ReservationHandling?  ReservationHandling,
                                       out String?               ErrorResponse)

            => TryParse(JSON,
                        out ReservationHandling,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a RemoteStop response.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="ReservationHandling">The parsed RemoteStop response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomReservationHandlingParser">A delegate to parse custom RemoteStop response JSON objects.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       out ReservationHandling?                           ReservationHandling,
                                       out String?                                        ErrorResponse,
                                       CustomJObjectParserDelegate<ReservationHandling>?  CustomReservationHandlingParser   = null)
        {

            try
            {

                ReservationHandling = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse KeepAliveTime    [optional]

                if (JSON.ParseOptional("keepAliveTime",
                                       "keep alive time",
                                       out TimeSpan? KeepAliveTime,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Parse EndTime          [optional]

                if (JSON.ParseOptional("endTime",
                                       "end time",
                                       out DateTime? EndTime,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                var reservationHandling = new ReservationHandling(KeepAliveTime,
                                                                  EndTime);

                if (CustomReservationHandlingParser is not null)
                    reservationHandling = CustomReservationHandlingParser(JSON,
                                                                          reservationHandling);

                ReservationHandling = reservationHandling;

                return true;

            }
            catch (Exception e)
            {
                ReservationHandling  = default;
                ErrorResponse        = "The given JSON representation of a RemoteStop response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomReservationHandlingSerializer = null)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomReservationHandlingSerializer">A delegate to customize the serialization of ReservationHandling responses.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ReservationHandling>?  CustomReservationHandlingSerializer   = null)
        {

            var json = JSONObject.Create(
                           new JProperty("keepAliveTime",   KeepAliveTime.TotalSeconds),
                           new JProperty("endTime",         EndTime.ToIso8601())
                       );

            return CustomReservationHandlingSerializer is not null
                       ? CustomReservationHandlingSerializer(this, json)
                       : json;

        }

        #endregion


        #region Static methods

        #region (static) Close

        /// <summary>
        /// The reservation will end with the remote stop operation.
        /// </summary>
        public static ReservationHandling Close
            => new ();

        #endregion

        #region (static) KeepAlive(KeepAliveTime)

        /// <summary>
        /// The reservation can be used for additional charging sessions within the given time span.
        /// </summary>
        /// <param name="KeepAliveTime">The timespan in which the reservation can be used for additional charging sessions (relative time).</param>
        public static ReservationHandling KeepAlive(TimeSpan KeepAliveTime)
            => new (KeepAliveTime);

        #endregion

        #region (static) KeepAlive(EndTime)

        /// <summary>
        /// The reservation can be used for additional charging sessions within the given time span.
        /// </summary>
        /// <param name="EndTime">The timestamp after which the reservation can no longer be used for additional charging sessions (absolute time).</param>
        public static ReservationHandling KeepAlive(DateTime EndTime)
            => new (EndTime: EndTime);

        #endregion

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => EndTime > Timestamp.Now
                   ? $"Keep alive: {KeepAliveTime.TotalSeconds.ToString("0")} secs, end time: {EndTime.ToIso8601()}"
                   : "close";

        #endregion

    }

}
