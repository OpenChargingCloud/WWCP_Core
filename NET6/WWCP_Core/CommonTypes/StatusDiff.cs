﻿/*
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

        #region Timestamp

        private readonly DateTime _Timestamp;

        /// <summary>
        /// The timestamp of the status diff.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return _Timestamp;
            }
        }

        #endregion


        #region EVSEOperatorId

        private readonly ChargingStationOperator_Id _EVSEOperatorId;

        /// <summary>
        /// The unique identification of the Charging Station Operator.
        /// </summary>
        public ChargingStationOperator_Id EVSEOperatorId
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
        /// The optional internationalized name of the Charging Station Operator.
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
        /// Return a text representation of this object.
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
        /// Return a text representation of this object.
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

        #region StatusDiff(Timestamp, EVSEOperatorId, EVSEOperatorName = null)

        /// <summary>
        /// Create a new status diff.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the status diff.</param>
        /// <param name="EVSEOperatorId">The unique identification of the Charging Station Operator.</param>
        /// <param name="EVSEOperatorName">The optional internationalized name of the Charging Station Operator.</param>
        public StatusDiff(DateTime         Timestamp,
                          ChargingStationOperator_Id  EVSEOperatorId,
                          I18NString       EVSEOperatorName = null)
        {

            this._Timestamp         = Timestamp;
            this._EVSEOperatorId    = EVSEOperatorId;

            this._NewStatus         = NewStatus        != null ? new List<KeyValuePair<TId, TStatusType>>(NewStatus)     : new List<KeyValuePair<TId, TStatusType>>();
            this._ChangedStatus     = ChangedStatus    != null ? new List<KeyValuePair<TId, TStatusType>>(ChangedStatus) : new List<KeyValuePair<TId, TStatusType>>();
            this._RemovedIds        = RemovedIds       != null ? new List<TId>(RemovedIds)                               : new List<TId>();

            this._EVSEOperatorName  = EVSEOperatorName != null ? EVSEOperatorName                                        : new I18NString();

        }

        #endregion

        #region StatusDiff(Timestamp, EVSEOperatorId, NewStatus, ChangedStatus, RemovedIds, EVSEOperatorName = null)

        /// <summary>
        /// Create a new status diff.
        /// </summary>
        /// <param name="Timestamp">The timestamp of the status diff.</param>
        /// <param name="EVSEOperatorId">The unique identification of the Charging Station Operator.</param>
        /// <param name="NewStatus">All new status.</param>
        /// <param name="ChangedStatus">All changed status.</param>
        /// <param name="RemovedIds">All removed status.</param>
        /// <param name="EVSEOperatorName">The optional internationalized name of the Charging Station Operator.</param>
        public StatusDiff(DateTime                                     Timestamp,
                          ChargingStationOperator_Id                              EVSEOperatorId,
                          IEnumerable<KeyValuePair<TId, TStatusType>>  NewStatus,
                          IEnumerable<KeyValuePair<TId, TStatusType>>  ChangedStatus,
                          IEnumerable<TId>                             RemovedIds,
                          I18NString                                   EVSEOperatorName = null)
        {

            this._Timestamp         = Timestamp;
            this._EVSEOperatorId    = EVSEOperatorId;

            this._NewStatus         = NewStatus        != null ? new List<KeyValuePair<TId, TStatusType>>(NewStatus)     : new List<KeyValuePair<TId, TStatusType>>();
            this._ChangedStatus     = ChangedStatus    != null ? new List<KeyValuePair<TId, TStatusType>>(ChangedStatus) : new List<KeyValuePair<TId, TStatusType>>();
            this._RemovedIds        = RemovedIds       != null ? new List<TId>(RemovedIds)                               : new List<TId>();

            this._EVSEOperatorName  = EVSEOperatorName != null ? EVSEOperatorName                                        : new I18NString();

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


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "[" + Timestamp + "] Status diff: " + _NewStatus.Count + " new, " + _ChangedStatus.Count + " changed, " + _RemovedIds.Count + " removed";
        }

        #endregion

    }

}
