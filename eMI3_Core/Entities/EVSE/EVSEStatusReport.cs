/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3_Core>
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

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// An EVSE status report.
    /// </summary>
    public class EVSEStatusReport : IEnumerable<KeyValuePair<EVSEStatusType, Single>>
    {

        #region Data

        private readonly Dictionary<EVSEStatusType, Single> _Overview;

        #endregion

        #region Properties

        #region EVSEs

        private readonly IEnumerable<EVSE> _EVSEs;

        /// <summary>
        /// All aggregated EVSEs.
        /// </summary>
        public IEnumerable<EVSE> EVSEs
        {
            get
            {
                return _EVSEs;
            }
        }

        #endregion

        #region Count

        private readonly UInt32 _Count;

        /// <summary>
        /// The number of aggregated EVSEs.
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
        /// Create a new EVSE status report.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        public EVSEStatusReport(IEnumerable<EVSE>  EVSEs)

        {

            _EVSEs     = EVSEs;
            _Count     = (UInt32) EVSEs.Count();

            var sum    = (Single) _Count;

            _Overview  = EVSEs.GroupBy(_EVSE => _EVSE.Status.Value).
                               ToDictionary(gr => gr.Key,
                                            gr => 100 * gr.Count() / sum );

        }

        #endregion


        #region this[Status]

        /// <summary>
        /// Return the percentage of EVSEs having the given status.
        /// </summary>
        /// <param name="Status">An EVSE status.</param>
        public Single this[EVSEStatusType Status]
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
        public IEnumerator<KeyValuePair<EVSEStatusType, Single>> GetEnumerator()
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
