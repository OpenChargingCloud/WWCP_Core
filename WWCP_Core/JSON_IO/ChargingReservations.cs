/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Net <https://github.com/OpenChargingCloud/WWCP_Net>
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
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this ChargingReservation)

        public static JObject ToJSON(this ChargingReservation  ChargingReservation)
        {

            #region Initial checks

            if (ChargingReservation == null)
                throw new ArgumentNullException(nameof(ChargingReservation), "The given charging reservation must not be null!");

            #endregion

            var TimeLeft = (UInt32) (ChargingReservation.StartTime + ChargingReservation.Duration - DateTime.Now).TotalSeconds;

            return JSONObject.Create(
                       new JProperty("ReservationId",            ChargingReservation.Id.               ToString()),
                       new JProperty("StartTime",                ChargingReservation.StartTime.        ToIso8601()),
                       new JProperty("Duration",        (UInt32) ChargingReservation.Duration.         TotalSeconds),
                       new JProperty("TimeLeft",                 TimeLeft > 0 ? TimeLeft : 0),
                       new JProperty("Level",                    ChargingReservation.ReservationLevel. ToString()),
                       ChargingReservation.ChargingPoolId    != null
                           ? new JProperty("ChargingPoolId",     ChargingReservation.ChargingPoolId.   ToString())
                           : null,
                       ChargingReservation.ChargingStationId != null
                           ? new JProperty("ChargingStationId",  ChargingReservation.ChargingStationId.ToString())
                           : null,
                       ChargingReservation.EVSEId            != null
                           ? new JProperty("EVSEId",             ChargingReservation.EVSEId.           ToString())
                           : null,

                       (ChargingReservation.AuthTokens.Any() ||
                        ChargingReservation.eMAIds. Any() ||
                        ChargingReservation.PINs.   Any())
                            ? new JProperty("AuthorizedIds", JSONObject.Create(

                                  ChargingReservation.AuthTokens.Any()
                                      ? new JProperty("AuthTokens", new JArray(ChargingReservation.AuthTokens.Select(v => v.ToString())))
                                      : null,

                                  ChargingReservation.eMAIds.Any()
                                      ? new JProperty("eMAIds",     new JArray(ChargingReservation.eMAIds. Select(v => v.ToString())))
                                      : null,

                                  ChargingReservation.PINs.Any()
                                      ? new JProperty("PINs",       new JArray(ChargingReservation.PINs.   Select(v => v.ToString())))
                                      : null

                                ))
                            : null

                      );

        }

        #endregion

        #region ToJSON(this ChargingReservation, JPropertyKey)

        public static JProperty ToJSON(this ChargingReservation ChargingReservation, String JPropertyKey)
        {

            #region Initial checks

            if (ChargingReservation == null)
                throw new ArgumentNullException(nameof(ChargingReservation),  "The given charging reservation must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),         "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargingReservation.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingReservations)

        public static JArray ToJSON(this IEnumerable<ChargingReservation>  ChargingReservations)
        {

            #region Initial checks

            if (ChargingReservations == null)
                return new JArray();

            #endregion

            return ChargingReservations != null && ChargingReservations.Any()
                       ? new JArray(ChargingReservations.SafeSelect(reservation => reservation.ToJSON()))
                       : new JArray();

        }

        #endregion

        #region ToJSON(this ChargingReservations, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingReservation> ChargingReservations, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingReservations != null
                       ? new JProperty(JPropertyKey, ChargingReservations.ToJSON())
                       : new JProperty(JPropertyKey, new JArray());

        }

        #endregion

    }

}
