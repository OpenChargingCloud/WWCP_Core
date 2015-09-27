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
    /// A generic status diff.
    /// </summary>
    public class StatusDiff<TId, TStatusType>
    {

        #region Properties

        #region EVSEOperatorId

        private readonly EVSEOperator_Id _EVSEOperatorId;

        /// <summary>
        /// The unique identification of the EVSE operator.
        /// </summary>
        public EVSEOperator_Id EVSEOperatorId
        {
            get
            {
                return _EVSEOperatorId;
            }
        }

        #endregion

        #region EVSEOperatorName

        private readonly I18NString _EVSEOperatorName;

        /// <summary>
        /// The optional internationalized name of the EVSE operator.
        /// </summary>
        public I18NString EVSEOperatorName
        {
            get
            {
                return _EVSEOperatorName;
            }
        }

        #endregion


        #region NewStatus

        private List<KeyValuePair<TId, TStatusType>> _NewStatus;

        /// <summary>
        /// All new status.
        /// </summary>
        public IEnumerable<KeyValuePair<TId, TStatusType>> NewStatus
        {
            get
            {
                return _NewStatus;
            }
        }

        #endregion

        #region ChangedStatus

        private List<KeyValuePair<TId, TStatusType>> _ChangedStatus;

        /// <summary>
        /// All changed status.
        /// </summary>
        public IEnumerable<KeyValuePair<TId, TStatusType>> ChangedStatus
        {
            get
            {
                return _ChangedStatus;
            }
        }

        #endregion

        #region RemovedEVSEIds

        private List<TId> _RemovedIds;

        /// <summary>
        /// All removed Ids (status).
        /// </summary>
        public IEnumerable<TId> RemovedIds
        {
            get
            {
                return _RemovedIds;
            }
        }

        #endregion


        #region ShortInfo

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public String ShortInfo
        {
            get
            {
                return _NewStatus.Count + " / " + _ChangedStatus.Count + " / " + _RemovedIds.Count;
            }
        }

        #endregion

        #region ExtendedInfo

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public String ExtendedInfo
        {
            get
            {
                return "New: "     + _NewStatus.    Select(kvp => kvp.Key.ToString() + " => " + kvp.Value.ToString()).AggregateWith(", ") + Environment.NewLine +
                       "Changed: " + _ChangedStatus.Select(kvp => kvp.Key.ToString() + " => " + kvp.Value.ToString()).AggregateWith(", ") + Environment.NewLine +
                       "Removed: " + _RemovedIds.   Select(EVSEId => EVSEId.ToString()).AggregateWith(", ") + Environment.NewLine;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region StatusDiff(EVSEOperatorId, EVSEOperatorName = null)

        /// <summary>
        /// Create a new status diff.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the EVSE operator.</param>
        /// <param name="EVSEOperatorName">The optional internationalized name of the EVSE operator.</param>
        public StatusDiff(EVSEOperator_Id  EVSEOperatorId,
                          I18NString       EVSEOperatorName = null)
        {

            this._EVSEOperatorId     = EVSEOperatorId;
            this._EVSEOperatorName   = EVSEOperatorName != null ? EVSEOperatorName : new I18NString();

            this._NewStatus      = new List<KeyValuePair<TId, TStatusType>>();
            this._ChangedStatus  = new List<KeyValuePair<TId, TStatusType>>();
            this._RemovedIds     = new List<TId>();

        }

        #endregion

        #region StatusDiff(EVSEOperatorId, NewStatus, ChangedStatus, RemovedIds, EVSEOperatorName = null)

        /// <summary>
        /// Create a new status diff.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the EVSE operator.</param>
        /// <param name="NewStatus">All new status.</param>
        /// <param name="ChangedStatus">All changed status.</param>
        /// <param name="RemovedIds">All removed status.</param>
        /// <param name="EVSEOperatorName">The optional internationalized name of the EVSE operator.</param>
        public StatusDiff(EVSEOperator_Id                              EVSEOperatorId,
                          IEnumerable<KeyValuePair<TId, TStatusType>>  NewStatus,
                          IEnumerable<KeyValuePair<TId, TStatusType>>  ChangedStatus,
                          IEnumerable<TId>                             RemovedIds,
                          I18NString                                   EVSEOperatorName = null)
        {

            this._EVSEOperatorId     = EVSEOperatorId;
            this._EVSEOperatorName   = EVSEOperatorName != null ? EVSEOperatorName : new I18NString();

            this._NewStatus      = new List<KeyValuePair<TId, TStatusType>>(NewStatus);
            this._ChangedStatus  = new List<KeyValuePair<TId, TStatusType>>(ChangedStatus);
            this._RemovedIds     = new List<TId>(RemovedIds);

        }

        #endregion

        #endregion


        #region AddNewStatus(NewStatus)

        /// <summary>
        /// Add a new status.
        /// </summary>
        /// <param name="NewStatus">The new status</param>
        public StatusDiff<TId, TStatusType> AddNewStatus(KeyValuePair<TId, TStatusType> NewStatus)
        {

            this._NewStatus.Add(NewStatus);

            return this;

        }

        #endregion

        #region AddNewStatus(Id, NewStatus)

        /// <summary>
        /// Add a new status.
        /// </summary>
        /// <param name="Id">The identification.</param>
        /// <param name="NewStatus">The new status</param>
        public StatusDiff<TId, TStatusType> AddNewStatus(TId Id, TStatusType NewStatus)
        {

            this._NewStatus.Add(new KeyValuePair<TId, TStatusType>(Id, NewStatus));

            return this;

        }

        #endregion


        #region AddChangedStatus(ChangedStatus)

        /// <summary>
        /// Add a changed status.
        /// </summary>
        /// <param name="ChangedStatus">The changed status</param>
        public StatusDiff<TId, TStatusType> AddChangedStatus(KeyValuePair<TId, TStatusType> ChangedStatus)
        {

            this._ChangedStatus.Add(ChangedStatus);

            return this;

        }

        #endregion

        #region AddChangedStatus(Id, ChangedStatus)

        /// <summary>
        /// Add a changed status.
        /// </summary>
        /// <param name="Id">The identification.</param>
        /// <param name="ChangedStatus">The changed status</param>
        public StatusDiff<TId, TStatusType> AddChangedStatus(TId Id, TStatusType ChangedStatus)
        {

            this._ChangedStatus.Add(new KeyValuePair<TId, TStatusType>(Id, ChangedStatus));

            return this;

        }

        #endregion


        #region AddRemovedId(RemovedId)

        /// <summary>
        /// Remove the status/Id.
        /// </summary>
        /// <param name="RemovedId">The removed Id.</param>
        public StatusDiff<TId, TStatusType> AddRemovedId(TId RemovedId)
        {

            this._RemovedIds.Add(RemovedId);

            return this;

        }

        #endregion

        #region AddRemovedIds(RemovedIds)

        /// <summary>
        /// Remove the status/Ids.
        /// </summary>
        /// <param name="RemovedIds">The removed Ids.</param>
        public StatusDiff<TId, TStatusType> AddRemovedId(IEnumerable<TId> RemovedIds)
        {

            this._RemovedIds.AddRange(RemovedIds);

            return this;

        }

        #endregion


        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return " status diff: " + _NewStatus.Count + " new, " + _ChangedStatus.Count + " changed, " + _RemovedIds.Count + " removed";
        }

        #endregion

    }

}
