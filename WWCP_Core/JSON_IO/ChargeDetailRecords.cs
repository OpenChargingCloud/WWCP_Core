/*
 * Copyright (c) 2014-2018 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

        #region ToJSON(this ChargeDetailRecord)

        public static JObject ToJSON(this ChargeDetailRecord  ChargeDetailRecord)

            => JSONObject.Create(

                   new JProperty("SessionId",            ChargeDetailRecord.SessionId.ToString()),

                   ChargeDetailRecord.SessionTime.HasValue
                       ? new JProperty("SessionStart",           ChargeDetailRecord.SessionTime.Value.StartTime.ToIso8601())
                       : null,

                   ChargeDetailRecord.SessionTime.HasValue && ChargeDetailRecord.SessionTime.Value.EndTime.HasValue
                       ? new JProperty("SessionEnd",             ChargeDetailRecord.SessionTime.Value.EndTime.Value.ToIso8601())
                       : null,

                   ChargeDetailRecord.Reservation != null
                       ? new JProperty("Reservation", JSONObject.Create(
                                                          new JProperty("ReservationId",  ChargeDetailRecord.Reservation.Id.ToString()),
                                                          new JProperty("StartTime",      ChargeDetailRecord.Reservation.StartTime.ToIso8601()),
                                                          new JProperty("Duration",       ChargeDetailRecord.Reservation.ConsumedReservationTime.TotalSeconds)
                                                      ))
                       : ChargeDetailRecord.ReservationId != null
                             ? new JProperty("ReservationId",    ChargeDetailRecord.ReservationId.ToString())
                             : null,


                   ChargeDetailRecord.ProviderIdStart     != null
                       ? new JProperty("ProviderIdStart",        ChargeDetailRecord.ProviderIdStart.ToString())
                       : null,

                   ChargeDetailRecord.ProviderIdStop      != null
                       ? new JProperty("ProviderIdStop",         ChargeDetailRecord.ProviderIdStop.ToString())
                       : null,

                   //ChargeDetailRecord.EVSEOperatorId      != null
                   //    ? new JProperty("EVSEOperatorId",         ChargeDetailRecord.EVSEOperatorId.ToString())
                   //    : null,

                   //ChargeDetailRecord.ChargingPoolId      != null
                   //    ? new JProperty("ChargingPoolId",         ChargeDetailRecord.ChargingPoolId.ToString())
                   //    : null,

                   //ChargeDetailRecord.ChargingStationId   != null
                   //    ? new JProperty("ChargingStationId",      ChargeDetailRecord.ChargingStationId.ToString())
                   //    : null,

                   ChargeDetailRecord.EVSE                != null
                       ? new JProperty("EVSEId",                 ChargeDetailRecord.EVSE.Id.ToString())
                       : ChargeDetailRecord.EVSEId != null
                           ? new JProperty("EVSEId", ChargeDetailRecord.EVSEId.ToString())
                           : null,

                   ChargeDetailRecord.ChargingProduct != null
                       ? new JProperty("ChargingProduct",     JSONObject.Create(
                             new JProperty("Id",                              ChargeDetailRecord.ChargingProduct.Id.ToString()),
                             ChargeDetailRecord.ChargingProduct.MinDuration.HasValue
                                 ? new JProperty("MinDuration",               ChargeDetailRecord.ChargingProduct.MinDuration.Value.TotalSeconds)
                                 : null,
                             ChargeDetailRecord.ChargingProduct.StopChargingAfterTime.HasValue
                                 ? new JProperty("StopChargingAfterTime",     ChargeDetailRecord.ChargingProduct.StopChargingAfterTime.Value.TotalSeconds)
                                 : null,
                             ChargeDetailRecord.ChargingProduct.MinPower.HasValue
                                 ? new JProperty("MinPower",                  ChargeDetailRecord.ChargingProduct.MinPower.Value)
                                 : null,
                             ChargeDetailRecord.ChargingProduct.MaxPower.HasValue
                                 ? new JProperty("MaxPower",                  ChargeDetailRecord.ChargingProduct.MaxPower.Value)
                                 : null,
                             ChargeDetailRecord.ChargingProduct.MinEnergy.HasValue
                                 ? new JProperty("MinEnergy",                 ChargeDetailRecord.ChargingProduct.MinEnergy.Value)
                                 : null,
                             ChargeDetailRecord.ChargingProduct.StopChargingAfterKWh.HasValue
                                 ? new JProperty("StopChargingAfterKWh",      ChargeDetailRecord.ChargingProduct.StopChargingAfterKWh.Value)
                                 : null
                            ))
                       : null,

                   ChargeDetailRecord.EnergyMeterId       != null
                       ? new JProperty("EnergyMeterId",          ChargeDetailRecord.EnergyMeterId.ToString())
                       : null,

                   ChargeDetailRecord.EnergyMeteringValues.Any()
                       ? new JProperty("EnergyMeteringValues",   new JObject(
                                                                     ChargeDetailRecord.
                                                                         EnergyMeteringValues.
                                                                         Select(MeterValue => new JProperty(MeterValue.Timestamp.ToIso8601(),
                                                                                                            MeterValue.Value))
                                                                 ))
                       : null

            );

        #endregion

        #region ToJSON(this ChargeDetailRecord, JPropertyKey)

        public static JProperty ToJSON(this ChargeDetailRecord ChargeDetailRecord, String JPropertyKey)
        {

            #region Initial checks

            if (ChargeDetailRecord == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecord),  "The given charging session must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),        "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 ChargeDetailRecord.ToJSON());

        }

        #endregion

        #region ToJSON(this ChargeDetailRecords)

        public static JArray ToJSON(this IEnumerable<ChargeDetailRecord>  ChargeDetailRecords)
        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                return new JArray();

            #endregion

            return ChargeDetailRecords != null && ChargeDetailRecords.Any()
                       ? new JArray(ChargeDetailRecords.SafeSelect(session => session.ToJSON()))
                       : new JArray();

        }

        #endregion

        #region ToJSON(this ChargeDetailRecords, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<ChargeDetailRecord> ChargeDetailRecords, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return ChargeDetailRecords != null
                       ? new JProperty(JPropertyKey, ChargeDetailRecords.ToJSON())
                       : new JProperty(JPropertyKey, new JArray());

        }

        #endregion

    }

}
