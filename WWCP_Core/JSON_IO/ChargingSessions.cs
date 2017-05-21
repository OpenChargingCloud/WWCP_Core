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
using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Hermod;
using System.Globalization;

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this ChargingSession)

        public static JObject ToJSON(this ChargingSession  ChargingSession)

            => JSONObject.Create(

                   new JProperty("SessionId",            ChargingSession.Id.ToString()),

                   ChargingSession.SessionTime.HasValue
                       ? new JProperty("SessionStart",           ChargingSession.SessionTime.Value.StartTime.ToIso8601())
                       : null,

                   ChargingSession.SessionTime.HasValue && ChargingSession.SessionTime.Value.EndTime.HasValue
                       ? new JProperty("SessionEnd",             ChargingSession.SessionTime.Value.EndTime.Value.ToIso8601())
                       : null,

                   ChargingSession.Reservation != null

                       ? new JProperty("Reservation", new JObject(
                                                          new JProperty("ReservationId",  ChargingSession.Reservation.Id.ToString()),
                                                          new JProperty("Start",          ChargingSession.Reservation.StartTime.ToIso8601()),
                                                          new JProperty("Duration",       ChargingSession.Reservation.Duration.TotalSeconds)
                                                          )
                                                      )

                       : ChargingSession.ReservationId != null
                             ? new JProperty("ReservationId",    ChargingSession.ReservationId.ToString())
                             : null,

                   ChargingSession.OperatorId.HasValue
                       ? new JProperty("EVSEOperatorId",         ChargingSession.OperatorId.ToString())
                       : null,

                   ChargingSession.ChargingPoolId.HasValue
                       ? new JProperty("ChargingPoolId",         ChargingSession.ChargingPoolId.ToString())
                       : null,

                   ChargingSession.ChargingStationId.HasValue
                       ? new JProperty("ChargingStationId",      ChargingSession.ChargingStationId.ToString())
                       : null,

                   ChargingSession.EVSEId.HasValue
                       ? new JProperty("EVSEId",                 ChargingSession.EVSEId.ToString())
                       : null,

                   ChargingSession.ChargingProduct != null
                       ? new JProperty("ChargingProduct",     JSONObject.Create(
                             new JProperty("Id",                              ChargingSession.ChargingProduct.Id.ToString()),
                             ChargingSession.ChargingProduct.MinDuration.HasValue
                                 ? new JProperty("MinDuration",               ChargingSession.ChargingProduct.MinDuration.Value.TotalSeconds)
                                 : null,
                             ChargingSession.ChargingProduct.StopChargingAfterTime.HasValue
                                 ? new JProperty("StopChargingAfterTime",     ChargingSession.ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                 : null,
                             ChargingSession.ChargingProduct.MinPower.HasValue
                                 ? new JProperty("MinPower",                  ChargingSession.ChargingProduct.MinPower.Value)
                                 : null,
                             ChargingSession.ChargingProduct.MaxPower.HasValue
                                 ? new JProperty("MaxPower",                  ChargingSession.ChargingProduct.MaxPower.Value)
                                 : null,
                             ChargingSession.ChargingProduct.MinEnergy.HasValue
                                 ? new JProperty("MinEnergy",                 ChargingSession.ChargingProduct.MinEnergy.Value)
                                 : null,
                             ChargingSession.ChargingProduct.StopChargingAfterKWh.HasValue
                                 ? new JProperty("StopChargingAfterKWh",      ChargingSession.ChargingProduct.StopChargingAfterKWh.Value)
                                 : null
                            ))
                       : null,

                   ChargingSession.ProviderIdStart          != null
                       ? new JProperty("ProviderId",             ChargingSession.ProviderIdStart.ToString())
                       : null,

                   ChargingSession.EnergyMeterId.HasValue
                       ? new JProperty("EnergyMeterId",          ChargingSession.EnergyMeterId.ToString())
                       : null,

                   ChargingSession.EnergyMeteringValues.Count > 0
                       ? new JProperty("EnergyMeterValues",      new JObject(
                                                                     ChargingSession.
                                                                         EnergyMeteringValues.
                                                                         Select(MeterValue => new JProperty(MeterValue.Timestamp.ToIso8601(),
                                                                                                            MeterValue.Value))
                                                                 ))
                       : null

            );

        #endregion

        #region ToJSON(this ChargingSession, JPropertyKey)

        public static JProperty ToJSON(this ChargingSession ChargingSession, String JPropertyKey)
        {

            #region Initial checks

            if (ChargingSession == null)
                throw new ArgumentNullException(nameof(ChargingSession),  "The given charging session must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),     "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargingSession.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargingSessions)

        public static JArray ToJSON(this IEnumerable<ChargingSession>  ChargingSessions)
        {

            #region Initial checks

            if (ChargingSessions == null)
                return new JArray();

            #endregion

            return ChargingSessions != null && ChargingSessions.Any()
                       ? new JArray(ChargingSessions.SafeSelect(session => session.ToJSON()))
                       : new JArray();

        }

        #endregion

        #region ToJSON(this ChargingSessions, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargingSession> ChargingSessions, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargingSessions != null
                       ? new JProperty(JPropertyKey, ChargingSessions.ToJSON())
                       : new JProperty(JPropertyKey, new JArray());

        }

        #endregion

    }

}
