/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A charging station status report.
    /// </summary>
    public class StatusReport<TEntity, TType> : IEnumerable<KeyValuePair<TType, Single>>
        where TEntity : IStatus<TType>
    {

        #region Data

        private readonly Dictionary<TType, Single> _Overview;

        #endregion

        #region Properties

        #region ChargingStations

        private readonly IEnumerable<TEntity> _ChargingStations;

        /// <summary>
        /// All aggregated charging stations.
        /// </summary>
        public IEnumerable<TEntity> ChargingStations
        {
            get
            {
                return _ChargingStations;
            }
        }

        #endregion

        #region Count

        private readonly UInt32 _Count;

        /// <summary>
        /// The number of aggregated charging stations.
        /// </summary>
        public UInt32 Count
        {
            get
            {
                return _Count;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station status report.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        public StatusReport(IEnumerable<TEntity> ChargingStations)
        {

            _ChargingStations  = ChargingStations;
            _Count             = (UInt32) ChargingStations.Count();

            var sum            = (Single) _Count;

            _Overview          = ChargingStations.GroupBy(_ChargingStation => _ChargingStation.Status.Value).
                                                  ToDictionary(gr => gr.Key,
                                                               gr => 100 * gr.Count() / sum );

        }

        #endregion


        #region this[Status]

        /// <summary>
        /// Return the percentage of charging stations having the given status.
        /// </summary>
        /// <param name="Status">A charging station status.</param>
        public Single this[TType Status]
        {
            get
            {

                if (!_Overview.ContainsKey(Status))
                    return 0;

                return _Overview[Status];

            }
        }

        #endregion


        #region IEnumerable Members

        /// <summary>
        /// Get an enumeration of all values.
        /// </summary>
        public IEnumerator<KeyValuePair<TType, Single>> GetEnumerator()
        {
            return _Overview.GetEnumerator();
        }

        /// <summary>
        /// Get an enumeration of all values.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Overview.GetEnumerator();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            return _Overview.
                       Select(v => v.Key + ": " + v.Value.ToString("{0:0.##}")).
                       AggregateWith(", ");

        }

        #endregion

    }

}
