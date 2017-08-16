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

#endregion

namespace org.GraphDefined.WWCP.Net.IO.JSON
{

    /// <summary>
    /// WWCP JSON I/O.
    /// </summary>
    public static partial class JSON_IO
    {

        #region ToJSON(this RoamingNetwork)

        /// <summary>
        /// Return a JSON representation of the given roaming network.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        public static JObject ToJSON(this RoamingNetwork RoamingNetwork)

            => RoamingNetwork == null
                   ? null

                   : JSONObject.Create(RoamingNetwork.Id.            ToJSON("RoamingNetworkId"),
                                       new JProperty("description",  RoamingNetwork.Description.ToJSON()));

        #endregion

        #region ToJSON(this RoamingNetwork, JPropertyKey)

        /// <summary>
        /// Return a JSON representation of the given roaming network
        /// using the given JSON property key.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// <param name="JPropertyKey">The name of the JSON property key to use.</param>
        public static JProperty ToJSON(this RoamingNetwork RoamingNetwork, String JPropertyKey)
        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),    "The given json property key must not be null or empty!");

            #endregion

            return new JProperty(JPropertyKey,
                                 RoamingNetwork.ToJSON());

        }

        #endregion

        #region ToJSON(this RoamingNetworks, Skip = 0, Take = 0)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        public static JArray ToJSON(this IEnumerable<RoamingNetwork>  RoamingNetworks,
                                    UInt64                            Skip  = 0,
                                    UInt64                            Take  = 0)
        {

            #region Initial checks

            if (RoamingNetworks == null)
                return new JArray();

            #endregion

            return new JArray(RoamingNetworks.
                                  Where     (roamingnetwork => roamingnetwork != null).
                                  OrderBy   (roamingnetwork => roamingnetwork.Id).
                                  SkipTakeFilter(Skip, Take).
                                  SafeSelect(roamingnetwork => roamingnetwork.ToJSON()));

        }

        #endregion

        #region ToJSON(this RoamingNetworks, JPropertyKey, Skip = 0, Take = 0)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks
        /// using the given JSON property key.
        /// </summary>
        /// <param name="RoamingNetworks">An enumeration of roaming networks.</param>
        /// <param name="JPropertyKey">The name of the JSON property key to use.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        public static JProperty ToJSON(this IEnumerable<RoamingNetwork>  RoamingNetworks,
                                       String                            JPropertyKey,
                                       UInt64                            Skip  = 0,
                                       UInt64                            Take  = 0)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey),  "The given json property key must not be null or empty!");

            #endregion

            return RoamingNetworks != null
                       ? new JProperty(JPropertyKey, RoamingNetworks.ToJSON(Skip, Take))
                       : null;

        }

        #endregion

        #region ToJSON(this RoamingNetworkAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>>  RoamingNetworkAdminStatus,
                                     UInt64                                                         Skip         = 0,
                                     UInt64                                                         Take         = 0,
                                     UInt64                                                         HistorySize  = 1)

        {

            if (RoamingNetworkAdminStatus == null)
                return new JObject();

            try
            {

                var _StatusHistory = Take == 0
                                         ? RoamingNetworkAdminStatus.Skip(Skip)
                                         : RoamingNetworkAdminStatus.Skip(Skip).Take(Take);

                return new JObject(_StatusHistory.
                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch (Exception)
            {
                // e.g. when a Stack behind RoamingNetworkAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this RoamingNetworkAdminStatus, Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>>>>  RoamingNetworkAdminStatus,
                                     UInt64                                                                                                       Skip         = 0,
                                     UInt64                                                                                                       Take         = 0,
                                     UInt64                                                                                                       HistorySize  = 1)

        {

            #region Initial checks

            if (RoamingNetworkAdminStatus == null || !RoamingNetworkAdminStatus.Any())
                return new JObject();

            var _RoamingNetworkAdminStatus = new Dictionary<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkAdminStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate RoamingNetwork identifications in the enumeration... take the newest one!

            foreach (var networkstatus in Take == 0 ? RoamingNetworkAdminStatus.Skip(Skip)
                                                : RoamingNetworkAdminStatus.Skip(Skip).Take(Take))
            {

                if (!_RoamingNetworkAdminStatus.ContainsKey(networkstatus.Key))
                    _RoamingNetworkAdminStatus.Add(networkstatus.Key, networkstatus.Value);

                else if (networkstatus.Value.FirstOrDefault().Timestamp > _RoamingNetworkAdminStatus[networkstatus.Key].FirstOrDefault().Timestamp)
                    _RoamingNetworkAdminStatus[networkstatus.Key] = networkstatus.Value;

            }

            #endregion

            return _RoamingNetworkAdminStatus.Count == 0

                   ? new JObject()

                   : new JObject(_RoamingNetworkAdminStatus.
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

                                                                                // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                                GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                                Select           (group => group.First()).

                                                                                OrderByDescending(tsv   => tsv.Timestamp).
                                                                                Take             (HistorySize).
                                                                                Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                         tsv.Value.    ToString())))

                                                          )));

        }

        #endregion

        #region ToJSON(this RoamingNetworkStatus,      Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<RoamingNetworkStatusTypes>>  RoamingNetworkStatus,
                                     UInt64                                                    Skip         = 0,
                                     UInt64                                                    Take         = 0,
                                     UInt64                                                    HistorySize  = 1)

        {

            if (RoamingNetworkStatus == null)
                return new JObject();

            try
            {

                var _StatusHistory = Take == 0
                                         ? RoamingNetworkStatus.Skip(Skip)
                                         : RoamingNetworkStatus.Skip(Skip).Take(Take);

                return new JObject(_StatusHistory.
                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch (Exception)
            {
                // e.g. when a Stack behind RoamingNetworkStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this RoamingNetworkStatus,      Skip = 0, Take = 0, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkStatusTypes>>>>  RoamingNetworkStatus,
                                     UInt64                                                                                                  Skip         = 0,
                                     UInt64                                                                                                  Take         = 0,
                                     UInt64                                                                                                  HistorySize  = 1)

        {

            #region Initial checks

            if (RoamingNetworkStatus == null || !RoamingNetworkStatus.Any())
                return new JObject();

            var _RoamingNetworkStatus = new Dictionary<RoamingNetwork_Id, IEnumerable<Timestamped<RoamingNetworkStatusTypes>>>();

            #endregion

            #region Maybe there are duplicate RoamingNetwork identifications in the enumeration... take the newest one!

            foreach (var networkstatus in Take == 0 ? RoamingNetworkStatus.Skip(Skip)
                                                : RoamingNetworkStatus.Skip(Skip).Take(Take))
            {

                if (!_RoamingNetworkStatus.ContainsKey(networkstatus.Key))
                    _RoamingNetworkStatus.Add(networkstatus.Key, networkstatus.Value);

                else if (networkstatus.Value.FirstOrDefault().Timestamp > _RoamingNetworkStatus[networkstatus.Key].FirstOrDefault().Timestamp)
                    _RoamingNetworkStatus[networkstatus.Key] = networkstatus.Value;

            }

            #endregion

            return _RoamingNetworkStatus.Count == 0

                   ? new JObject()

                   : new JObject(_RoamingNetworkStatus.
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

                                                                                // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                                GroupBy          (tsv   => tsv.  Timestamp.ToIso8601()).
                                                                                Select           (group => group.First()).

                                                                                OrderByDescending(tsv   => tsv.Timestamp).
                                                                                Take             (HistorySize).
                                                                                Select           (tsv   => new JProperty(tsv.Timestamp.ToIso8601(),
                                                                                                                         tsv.Value.    ToString())))

                                                                )));

        }

        #endregion

    }

}
