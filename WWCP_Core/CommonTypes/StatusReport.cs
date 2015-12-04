/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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
    /// A status report.
    /// </summary>
    public class StatusReport<TEntity, TType> : IEnumerable<KeyValuePair<TType, Single>>
    {

        #region Data

        private readonly Dictionary<TType, Single> _Overview;

        #endregion

        #region Properties

        #region Entities

        private readonly IEnumerable<TEntity> _Entities;

        /// <summary>
        /// All aggregated entities.
        /// </summary>
        public IEnumerable<TEntity> Entities
        {
            get
            {
                return _Entities;
            }
        }

        #endregion

        #region Count

        private readonly UInt32 _Count;

        /// <summary>
        /// The number of aggregated entities.
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
        /// Create a new status report.
        /// </summary>
        /// <param name="Entities">An enumeration of entities.</param>
        /// <param name="GetStatusDelegate">A delegate to convert an entity into its status.</param>
        public StatusReport(IEnumerable<TEntity>  Entities,
                            Func<TEntity, TType>  GetStatusDelegate)
        {

            #region Initial checks

            if (GetStatusDelegate == null)
                throw new ArgumentNullException("GetStatusDelegate", "The given parameter must not be null!");

            #endregion

            _Entities  = Entities;
            _Count     = (UInt32) Entities.Count();

            var sum    = (Single) _Count;

            _Overview  = Entities.GroupBy(Entity => GetStatusDelegate(Entity)). // Entity.Status.Value).
                                          ToDictionary(gr => gr.Key,
                                                       gr => 100 * gr.Count() / sum );

        }

        #endregion


        #region this[Status]

        /// <summary>
        /// Return the percentage of entities having the given status.
        /// </summary>
        /// <param name="Status">An entity status.</param>
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

        #region (override) ToString()

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
