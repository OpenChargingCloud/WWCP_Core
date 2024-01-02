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
    /// A status report.
    /// </summary>
    public class StatusReport<TEntity, TStatus> : IEnumerable<KeyValuePair<TStatus, Tuple<UInt64, Single>>>

        where TEntity : notnull
        where TStatus : notnull

    {

        #region Data

        private readonly Dictionary<TStatus, Tuple<UInt64, Single>> overview;

        #endregion

        #region Properties

        /// <summary>
        /// The timestamp of the status report generation.
        /// </summary>
        public DateTime              Timestamp      { get; }

        /// <summary>
        /// All aggregated entities.
        /// </summary>
        public IEnumerable<TEntity>  Entities       { get; }

        /// <summary>
        /// The number of aggregated entities.
        /// </summary>
        public UInt32                Count          { get; }

        /// <summary>
        /// The JSON context of this status report.
        /// </summary>
        public String                JSONContext    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new status report.
        /// </summary>
        /// <param name="Entities">An enumeration of entities.</param>
        /// <param name="GetStatusDelegate">A delegate to convert an entity into its status.</param>
        /// <param name="Timestamp">The optional timestamp of the status report generation.</param>
        /// <param name="JSONContext">The JSON context of this status report.</param>
        public StatusReport(IEnumerable<TEntity>    Entities,
                            Func<TEntity, TStatus>  GetStatusDelegate,
                            DateTime?               Timestamp        = null,
                            String?                 JSONContext      = null)
        {

            #region Initial checks

            if (GetStatusDelegate is null)
                throw new ArgumentNullException(nameof(GetStatusDelegate), "The given get status delegate must not be null!");

            #endregion

            this.Entities        = Entities;
            this.Timestamp       = Timestamp   ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.JSONContext     = JSONContext ?? "https://open.charging.cloud/contexts/wwcp+json/statusReport";

            this.Count           = (UInt32) Entities.Count();
            var sum              = (Single) this.Count;

            this.overview        = Entities.GroupBy(Entity => GetStatusDelegate(Entity)).
                                                    ToDictionary(group => group.Key,
                                                                 group => new Tuple<UInt64, Single>(
                                                                              (UInt64) group.Count(),
                                                                                 100 * group.Count() / sum
                                                                          ));

        }

        #endregion


        #region CountOf     (Status)

        /// <summary>
        /// Return the count of entities having the given status.
        /// </summary>
        /// <param name="Status">An entity status.</param>
        public UInt64 CountOf(TStatus Status)

            => overview.ContainsKey(Status)
                   ? overview[Status].Item1
                   : 0;

        #endregion

        #region PercentageOf(Status)

        /// <summary>
        /// Return the percentage of entities having the given status.
        /// </summary>
        /// <param name="Status">An entity status.</param>
        public Single PercentageOf(TStatus Status)

            => overview.ContainsKey(Status)
                   ? overview[Status].Item2
                   : 0;

        #endregion


        #region ToJSON(Embedded = false)

        /// <summary>
        /// Return a JSON representation of the given status report.
        /// </summary>
        /// <param name="Embedded">Whether this data structure is embedded into another data structure.</param>
        public JObject ToJSON(Boolean Embedded = false)
        {

            var reportJSON = new JObject();

            foreach (var item in overview)
            {

                var key = item.Key.ToString();

                if (key is not null)
                    reportJSON.Add(new JProperty(key, new JObject(
                                                          new JProperty("count",      item.Value.Item1),
                                                          new JProperty("percentage", item.Value.Item2)
                                                      )));

            }

            var json = JSONObject.Create(

                           !Embedded
                               ? new JProperty("@context",   JSONContext)
                               : null,

                           new JProperty("timestamp",  Timestamp.ToIso8601()),
                           new JProperty("count",      Count),
                           new JProperty("report",     reportJSON)

                       );

            return json;

        }

        #endregion


        #region IEnumerable Members

        /// <summary>
        /// Get an enumeration of all values.
        /// </summary>
        public IEnumerator<KeyValuePair<TStatus, Tuple<UInt64, Single>>> GetEnumerator()
            => overview.GetEnumerator();

        /// <summary>
        /// Get an enumeration of all values.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => overview.GetEnumerator();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   Count, " entities; ",

                   overview.
                       Select(v => String.Concat(v.Key, ": ", v.Value.Item1, " (", v.Value.Item2.ToString("0.00"), ")")).
                       AggregateWith(", ")

               );

        #endregion

    }

}
