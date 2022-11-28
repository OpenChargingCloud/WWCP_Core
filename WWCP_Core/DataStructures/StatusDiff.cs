/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A generic status diff.
    /// </summary>
    public class StatusDiff<TParentId, TId, TStatusType>

        where TParentId : IId
        where TId       : IId

    {

        #region Data

        private readonly HashSet<KeyValuePair<TId, TStatusType>>  newStatus;

        private readonly HashSet<KeyValuePair<TId, TStatusType>>  changedStatus;

        private readonly HashSet<TId>                             removedIds;

        #endregion

        #region Properties

        /// <summary>
        /// The timestamp of the status diff.
        /// </summary>
        public DateTime                                     Timestamp     { get; }

        /// <summary>
        /// The unique identification of the charging station operator.
        /// </summary>
        public TParentId                                    ParentId      { get; }

        /// <summary>
        /// The optional internationalized name of the charging station operator.
        /// </summary>
        public I18NString                                   ParentName    { get; }


        /// <summary>
        /// All new status.
        /// </summary>
        public IEnumerable<KeyValuePair<TId, TStatusType>>  NewStatus
            => NewStatus;

        /// <summary>
        /// All changed status.
        /// </summary>
        public IEnumerable<KeyValuePair<TId, TStatusType>>  ChangedStatus
            => ChangedStatus;

        /// <summary>
        /// All removed Ids (status).
        /// </summary>
        public IEnumerable<TId>                             RemovedIds
            => RemovedIds;


        #region ShortInfo

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public String ShortInfo

            => String.Concat(newStatus.    Count, " / ",
                             changedStatus.Count, " / ",
                             removedIds.   Count);

        #endregion

        #region ExtendedInfo

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public String ExtendedInfo
            => "New: "     + newStatus.    Select(kvp    => kvp.Key.ToString() + " => " + kvp.Value.ToString()).AggregateWith(", ") + Environment.NewLine +
               "Changed: " + changedStatus.Select(kvp    => kvp.Key.ToString() + " => " + kvp.Value.ToString()).AggregateWith(", ") + Environment.NewLine +
               "Removed: " + removedIds.   Select(evseId => evseId. ToString()).AggregateWith(", ") + Environment.NewLine;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic status diff.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the generic status diff.</param>
        /// <param name="ParentId">The unique identification of the parent data structure.</param>
        /// <param name="ParentName">The optional multi-language name of the parent data structure.</param>
        /// <param name="NewStatus">An optional enumeration of all new generic status.</param>
        /// <param name="ChangedStatus">An optional enumeration of all generic status.</param>
        /// <param name="RemovedIds">An optional enumeration of all removed generic status.</param>
        public StatusDiff(DateTime                                      Timestamp,
                          TParentId                                     ParentId,
                          I18NString?                                   ParentName      = null,
                          IEnumerable<KeyValuePair<TId, TStatusType>>?  NewStatus       = null,
                          IEnumerable<KeyValuePair<TId, TStatusType>>?  ChangedStatus   = null,
                          IEnumerable<TId>?                             RemovedIds      = null)
        {

            this.Timestamp      = Timestamp;
            this.ParentId       = ParentId;
            this.ParentName     = ParentName ?? new I18NString();

            this.newStatus      = NewStatus     is not null && NewStatus.    Any() ? new HashSet<KeyValuePair<TId, TStatusType>>(NewStatus)     : new HashSet<KeyValuePair<TId, TStatusType>>();
            this.changedStatus  = ChangedStatus is not null && ChangedStatus.Any() ? new HashSet<KeyValuePair<TId, TStatusType>>(ChangedStatus) : new HashSet<KeyValuePair<TId, TStatusType>>();
            this.removedIds     = RemovedIds    is not null && RemovedIds   .Any() ? new HashSet<TId>(RemovedIds)                               : new HashSet<TId>();

        }

        #endregion


        #region AddNewStatus(NewStatus)

        /// <summary>
        /// Add a new status.
        /// </summary>
        /// <param name="NewStatus">The new status</param>
        public StatusDiff<TParentId, TId, TStatusType> AddNewStatus(KeyValuePair<TId, TStatusType> NewStatus)
        {

            newStatus.Add(NewStatus);

            return this;

        }

        #endregion

        #region AddNewStatus(Id, NewStatus)

        /// <summary>
        /// Add a new status.
        /// </summary>
        /// <param name="Id">The identification.</param>
        /// <param name="NewStatus">The new status</param>
        public StatusDiff<TParentId, TId, TStatusType> AddNewStatus(TId Id, TStatusType NewStatus)
        {

            newStatus.Add(new KeyValuePair<TId, TStatusType>(Id, NewStatus));

            return this;

        }

        #endregion


        #region AddChangedStatus(ChangedStatus)

        /// <summary>
        /// Add a changed status.
        /// </summary>
        /// <param name="ChangedStatus">The changed status</param>
        public StatusDiff<TParentId, TId, TStatusType> AddChangedStatus(KeyValuePair<TId, TStatusType> ChangedStatus)
        {

            changedStatus.Add(ChangedStatus);

            return this;

        }

        #endregion

        #region AddChangedStatus(Id, ChangedStatus)

        /// <summary>
        /// Add a changed status.
        /// </summary>
        /// <param name="Id">The identification.</param>
        /// <param name="ChangedStatus">The changed status</param>
        public StatusDiff<TParentId, TId, TStatusType> AddChangedStatus(TId Id, TStatusType ChangedStatus)
        {

            changedStatus.Add(new KeyValuePair<TId, TStatusType>(Id, ChangedStatus));

            return this;

        }

        #endregion


        #region AddRemovedId(RemovedId)

        /// <summary>
        /// Remove the status/Id.
        /// </summary>
        /// <param name="RemovedId">The removed Id.</param>
        public StatusDiff<TParentId, TId, TStatusType> AddRemovedId(TId RemovedId)
        {

            removedIds.Add(RemovedId);

            return this;

        }

        #endregion

        #region AddRemovedIds(RemovedIds)

        /// <summary>
        /// Remove the status/Ids.
        /// </summary>
        /// <param name="RemovedIds">The removed Ids.</param>
        public StatusDiff<TParentId, TId, TStatusType> AddRemovedId(IEnumerable<TId> RemovedIds)
        {

            foreach (var removedId in removedIds)
                removedIds.Add(removedId);

            return this;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("[", Timestamp,
                             "] Status diff: ",
                             NewStatus.    Count(), " new, ",
                             ChangedStatus.Count(), " changed, ",
                             RemovedIds.   Count(), " removed");

        #endregion

    }

}
