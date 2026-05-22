/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Extension methods for the IStatus interface.
    /// </summary>
    public static class IStatusExtensions
    {

        #region ToJSON              (this StatusList, Skip = null, Take = null)

        public static JObject ToJSON<TId, TData>(this IEnumerable<IStatus<TId, TData>>  StatusList,
                                                 UInt64?                                Skip   = null,
                                                 UInt64?                                Take   = null)

            where TId : notnull

        {

            #region Initial checks

            if (StatusList is null || !StatusList.Any())
                return [];

            #endregion

            #region Maybe there are duplicate EVSE identifications in the enumeration... take the newest one!

            var filteredStatus = new Dictionary<TId, IStatus<TId, TData>>();

            foreach (var status in StatusList)
            {

                if (!filteredStatus.ContainsKey(status.Id))
                    filteredStatus.Add(status.Id, status);

                else if (filteredStatus[status.Id].Timestamp >= status.Timestamp)
                    filteredStatus[status.Id] = status;

            }

            #endregion


            return new JObject(

                       (Take.HasValue
                            ? filteredStatus.OrderBy(status => status.Key).Skip(Skip).Take(Take)
                            : filteredStatus.OrderBy(status => status.Key).Skip(Skip)).

                       Select(kvp => new JProperty(
                                         kvp.Key?.ToString() ?? "-",
                                         new JArray(
                                             kvp.Value.Timestamp.ToISO8601(),
                                             kvp.Value.Status?.  ToString() ?? "-"
                                         )
                                     ))

                   );

        }

        #endregion

        #region Contains            (this StatusList, Id, Status)

        /// <summary>
        /// Check if the given enumeration of entity statuses
        /// contains the given pair of entity identification and status.
        /// </summary>
        /// <param name="StatusList">An enumeration of entity status.</param>
        /// <param name="Id">An entity identification.</param>
        /// <param name="Status">An entity status.</param>
        public static Boolean Contains<TId, TData>(this IEnumerable<IStatus<TId, TData>>  StatusList,
                                                   TId                                    Id,
                                                   TData                                  Status)

            where TId : notnull

        {

            foreach (var status in StatusList)
            {
                if (status.Id.    Equals(Id)  &&
                    status.Status is not null &&
                    status.Status.Equals(Status))
                {
                    return true;
                }
            }

            return false;

        }

        #endregion

        #region ToJSON              (this Status)

        /// <summary>
        /// Return a JSON representation of the given status.
        /// </summary>
        /// <typeparam name="TId">The type of the status identification.</typeparam>
        /// <typeparam name="TData">The type of the status data.</typeparam>
        /// <param name="Status">The status</param>
        public static JObject ToJSON<TId, TData>(this IStatus<TId, TData> Status)
            where TId : notnull

            => JSONObject.Create(
                   new JProperty("id",         Status.Id.       ToString()),
                   new JProperty("status",     Status.Status?.  ToString()),
                   new JProperty("timestamp",  Status.Timestamp.ToISO8601())
               );

        #endregion

        #region AsTimestampedStatus (this Status)

        /// <summary>
        /// Return a timestamped status of the given status.
        /// </summary>
        /// <typeparam name="TId">The type of the status identification.</typeparam>
        /// <typeparam name="TData">The type of the status data.</typeparam>
        /// <param name="Status">The status</param>
        public static Timestamped<TData> AsTimestampedStatus<TId, TData>(this IStatus<TId, TData> Status)
            where TId : notnull

            => new (
                   Status.Timestamp,
                   Status.Status
               );

        #endregion


        public static JObject ToStatusList<TId, TData>(this IEnumerable<IStatus<TId, TData>> StatusList)
            where TId : notnull

            => new (
                   StatusList.Select(
                       status => new JProperty(
                                     status.Id?.ToString() ?? "-",
                                     new JArray(
                                         status.Timestamp.ToISO8601(),
                                         status.Status?.  ToString() ?? "-"
                                     )
                                 )
                   )
               );

    }

    public interface IStatus<TId, TData>

        where TId: notnull

    {

        TId             Id           { get; }

        TData           Status       { get; }

        DateTimeOffset  Timestamp    { get; }

        Context?        Context      { get; }

    }

}
