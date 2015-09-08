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
    /// An EVSE status diff.
    /// </summary>
    public class EVSEStatusDiff
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

        #region NewEVSEStatus

        private List<KeyValuePair<EVSE_Id, EVSEStatusType>> _NewEVSEStatus;

        /// <summary>
        /// All new EVSE status.
        /// </summary>
        public IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> NewEVSEStatus
        {
            get
            {
                return _NewEVSEStatus;
            }
        }

        #endregion

        #region ChangedEVSEStatus

        private List<KeyValuePair<EVSE_Id, EVSEStatusType>> _ChangedEVSEStatus;

        /// <summary>
        /// All changed EVSE status.
        /// </summary>
        public IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>> ChangedEVSEStatus
        {
            get
            {
                return _ChangedEVSEStatus;
            }
        }

        #endregion

        #region RemovedEVSEIds

        private List<EVSE_Id> _RemovedEVSEIds;

        /// <summary>
        /// All removed EVSE status/Ids.
        /// </summary>
        public IEnumerable<EVSE_Id> RemovedEVSEIds
        {
            get
            {
                return _RemovedEVSEIds;
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
                return _NewEVSEStatus.Count + " / " + _ChangedEVSEStatus.Count + " / " + _RemovedEVSEIds.Count;
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
                return "New: "     + _NewEVSEStatus.    Select(kvp => kvp.Key.ToString() + " => " + kvp.Value.ToString()).AggregateWith(", ") + Environment.NewLine +
                       "Changed: " + _ChangedEVSEStatus.Select(kvp => kvp.Key.ToString() + " => " + kvp.Value.ToString()).AggregateWith(", ") + Environment.NewLine +
                       "Removed: " + _RemovedEVSEIds.   Select(EVSEId => EVSEId.ToString()).AggregateWith(", ") + Environment.NewLine;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSEStatusDiff(EVSEOperatorId, EVSEOperatorName = null)

        /// <summary>
        /// Create a new EVSE status diff.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the EVSE operator.</param>
        /// <param name="EVSEOperatorName">The optional internationalized name of the EVSE operator.</param>
        public EVSEStatusDiff(EVSEOperator_Id  EVSEOperatorId,
                              I18NString       EVSEOperatorName = null)
        {

            this._EVSEOperatorId     = EVSEOperatorId;
            this._EVSEOperatorName   = EVSEOperatorName != null ? EVSEOperatorName : new I18NString();

            this._NewEVSEStatus      = new List<KeyValuePair<EVSE_Id, EVSEStatusType>>();
            this._ChangedEVSEStatus  = new List<KeyValuePair<EVSE_Id, EVSEStatusType>>();
            this._RemovedEVSEIds     = new List<EVSE_Id>();

        }

        #endregion

        #region EVSEStatusDiff(EVSEOperatorId, NewEVSEStatus, ChangedEVSEStatus, RemovedEVSEIds, EVSEOperatorName = null)

        /// <summary>
        /// Create a new EVSE status diff.
        /// </summary>
        /// <param name="EVSEOperatorId">The unique identification of the EVSE operator.</param>
        /// <param name="NewEVSEStatus">All new EVSE status.</param>
        /// <param name="ChangedEVSEStatus">All changed EVSE status.</param>
        /// <param name="RemovedEVSEIds">All removed EVSE status.</param>
        /// <param name="EVSEOperatorName">The optional internationalized name of the EVSE operator.</param>
        public EVSEStatusDiff(EVSEOperator_Id                                     EVSEOperatorId,
                              IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  NewEVSEStatus,
                              IEnumerable<KeyValuePair<EVSE_Id, EVSEStatusType>>  ChangedEVSEStatus,
                              IEnumerable<EVSE_Id>                                RemovedEVSEIds,
                              I18NString                                          EVSEOperatorName = null)
        {

            this._EVSEOperatorId     = EVSEOperatorId;
            this._EVSEOperatorName   = EVSEOperatorName != null ? EVSEOperatorName : new I18NString();

            this._NewEVSEStatus      = new List<KeyValuePair<EVSE_Id, EVSEStatusType>>(NewEVSEStatus);
            this._ChangedEVSEStatus  = new List<KeyValuePair<EVSE_Id, EVSEStatusType>>(ChangedEVSEStatus);
            this._RemovedEVSEIds     = new List<EVSE_Id>(RemovedEVSEIds);

        }

        #endregion

        #endregion


        #region AddNewStatus(NewEVSEStatus)

        /// <summary>
        /// Add a new EVSE status.
        /// </summary>
        /// <param name="NewEVSEStatus">The new EVSE status</param>
        public EVSEStatusDiff AddNewStatus(KeyValuePair<EVSE_Id, EVSEStatusType> NewEVSEStatus)
        {

            this._NewEVSEStatus.Add(NewEVSEStatus);

            return this;

        }

        #endregion

        #region AddNewStatus(EVSEId, NewEVSEStatus)

        /// <summary>
        /// Add a new EVSE status.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="NewEVSEStatus">The new EVSE status</param>
        public EVSEStatusDiff AddNewStatus(EVSE_Id EVSEId, EVSEStatusType NewEVSEStatus)
        {

            this._NewEVSEStatus.Add(new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSEId, NewEVSEStatus));

            return this;

        }

        #endregion


        #region AddChangedStatus(ChangedEVSEStatus)

        /// <summary>
        /// Add a changed EVSE status.
        /// </summary>
        /// <param name="ChangedEVSEStatus">The changed EVSE status</param>
        public EVSEStatusDiff AddChangedStatus(KeyValuePair<EVSE_Id, EVSEStatusType> ChangedEVSEStatus)
        {

            this._ChangedEVSEStatus.Add(ChangedEVSEStatus);

            return this;

        }

        #endregion

        #region AddChangedStatus(EVSEId, ChangedEVSEStatus)

        /// <summary>
        /// Add a changed EVSE status.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification.</param>
        /// <param name="ChangedEVSEStatus">The changed EVSE status</param>
        public EVSEStatusDiff AddChangedStatus(EVSE_Id EVSEId, EVSEStatusType ChangedEVSEStatus)
        {

            this._ChangedEVSEStatus.Add(new KeyValuePair<EVSE_Id, EVSEStatusType>(EVSEId, ChangedEVSEStatus));

            return this;

        }

        #endregion


        #region AddRemovedEVSEId(RemovedEVSEId)

        /// <summary>
        /// Remove the EVSE status/Id.
        /// </summary>
        /// <param name="RemovedEVSEId">The removed EVSE Id.</param>
        public EVSEStatusDiff AddRemovedEVSEId(EVSE_Id RemovedEVSEId)
        {

            this._RemovedEVSEIds.Add(RemovedEVSEId);

            return this;

        }

        #endregion

        #region AddRemovedEVSEIds(RemovedEVSEIds)

        /// <summary>
        /// Remove the EVSE status/Ids.
        /// </summary>
        /// <param name="RemovedEVSEIds">The removed EVSE Ids.</param>
        public EVSEStatusDiff AddRemovedEVSEId(IEnumerable<EVSE_Id> RemovedEVSEIds)
        {

            this._RemovedEVSEIds.AddRange(RemovedEVSEIds);

            return this;

        }

        #endregion


        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return "EVSE status diff: " + _NewEVSEStatus.Count + " new, " + _ChangedEVSEStatus.Count + " changed, " + _RemovedEVSEIds.Count + " removed";
        }

        #endregion

    }

}
