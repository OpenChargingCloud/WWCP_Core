/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using Newtonsoft.Json.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate called whenever the static data of the grid operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="GridOperator">The updated grid operator operator.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnGridOperatorDataChangedDelegate(DateTime          Timestamp,
                                                           EventTracking_Id  EventTrackingId,
                                                           IGridOperator     GridOperator,
                                                           String            PropertyName,
                                                           Object?           OldValue,
                                                           Object?           NewValue);

    /// <summary>
    /// A delegate called whenever the admin status of the grid operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="GridOperator">The updated grid operator.</param>
    /// <param name="OldStatus">The old timestamped status of the grid operator.</param>
    /// <param name="NewStatus">The new timestamped status of the grid operator.</param>
    public delegate Task OnGridOperatorAdminStatusChangedDelegate(DateTime                                   Timestamp,
                                                                  EventTracking_Id                           EventTrackingId,
                                                                  IGridOperator                              GridOperator,
                                                                  Timestamped<GridOperatorAdminStatusTypes>  OldStatus,
                                                                  Timestamped<GridOperatorAdminStatusTypes>  NewStatus);

    /// <summary>
    /// A delegate called whenever the dynamic status of the grid operator changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
    /// <param name="GridOperator">The updated grid operator.</param>
    /// <param name="OldStatus">The old timestamped status of the grid operator.</param>
    /// <param name="NewStatus">The new timestamped status of the grid operator.</param>
    public delegate Task OnGridOperatorStatusChangedDelegate(DateTime                              Timestamp,
                                                             EventTracking_Id                      EventTrackingId,
                                                             IGridOperator                         GridOperator,
                                                             Timestamped<GridOperatorStatusTypes>  OldStatus,
                                                             Timestamped<GridOperatorStatusTypes>  NewStatus);


    /// <summary>
    /// Extension methods for grid operators.
    /// </summary>
    public static class GridOperatorExtensions
    {

        #region ToJSON(this GridOperator, JPropertyKey)

        public static JProperty ToJSON(this IGridOperator GridOperator, String JPropertyKey)

            => new JProperty(JPropertyKey, GridOperator.ToJSON());

        #endregion

        #region ToJSON(this GridOperators, Skip = null, Take = null, Embedded = false, ExpandChargingRoamingNetworkId = false)

        /// <summary>
        /// Return a JSON representation for the given enumeration of Charging Station Operators.
        /// </summary>
        /// <param name="GridOperators">An enumeration of Charging Station Operators.</param>
        /// <param name="Skip">The optional number of Charging Station Operators to skip.</param>
        /// <param name="Take">The optional number of Charging Station Operators to return.</param>
        public static JArray ToJSON(this IEnumerable<IGridOperator>  GridOperators,
                                    UInt64?                          Skip                            = null,
                                    UInt64?                          Take                            = null,
                                    Boolean                          Embedded                        = false,
                                    Boolean                          ExpandChargingRoamingNetworkId  = false)

            => new (GridOperators.
                        Where     (gridOperator => gridOperator is not null).
                        OrderBy   (gridOperator => gridOperator.Id).
                        SkipTakeFilter(Skip, Take).
                        SafeSelect(gridOperator => gridOperator.ToJSON(Embedded,
                                                                       ExpandChargingRoamingNetworkId)));

        #endregion

        #region ToJSON(this GridOperators, JPropertyKey)

        public static JProperty ToJSON(this IEnumerable<IGridOperator> GridOperators, String JPropertyKey)
        {

            #region Initial checks

            if (JPropertyKey.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(JPropertyKey), "The json property key must not be null or empty!");

            #endregion

            return GridOperators != null
                       ? new JProperty(JPropertyKey, GridOperators.ToJSON())
                       : null;

        }

        #endregion

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<GridOperatorAdminStatusTypes>>  GridOperatorAdminStatus,
                                     UInt64?                                                      Skip         = null,
                                     UInt64?                                                      Take         = null,
                                     UInt64?                                                      HistorySize  = 1)

        {

            if (GridOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorAdminStatus.
                                       SkipTakeFilter(Skip, Take).

                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch
            {
                // e.g. when a Stack behind GridOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorAdminStatus, Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorAdminStatusTypes>>>>  GridOperatorAdminStatus,
                                     UInt64?                                                                                                  Skip         = null,
                                     UInt64?                                                                                                  Take         = null,
                                     UInt64?                                                                                                  HistorySize  = 1)

        {

            if (GridOperatorAdminStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorAdminStatus.
                                       SkipTakeFilter(Skip, Take).
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

                                                                                // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                                GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                                                                Select           (group => group.First()).

                                                                                OrderByDescending(tsv   => tsv.Timestamp).
                                                                                Take             (HistorySize).
                                                                                Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                                                         tsv.Value.    ToString())))

                                                          )));

            }
            catch
            {
                // e.g. when a Stack behind GridOperatorAdminStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<Timestamped<GridOperatorStatusTypes>>  GridOperatorStatus,
                                     UInt64?                                                 Skip         = null,
                                     UInt64?                                                 Take         = null,
                                     UInt64?                                                 HistorySize  = 1)

        {

            if (GridOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorStatus.
                                       SkipTakeFilter(Skip, Take).

                                       // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                       GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                       Select           (group => group.First()).

                                       OrderByDescending(tsv   => tsv.Timestamp).
                                       Take             (HistorySize).
                                       Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                tsv.Value.    ToString())));

            }
            catch
            {
                // e.g. when a Stack behind GridOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

        #region ToJSON(this GridOperatorStatus,      Skip = null, Take = null, HistorySize = 1)

        public static JObject ToJSON(this IEnumerable<KeyValuePair<GridOperator_Id, IEnumerable<Timestamped<GridOperatorStatusTypes>>>>  GridOperatorStatus,
                                     UInt64?                                                                                             Skip         = null,
                                     UInt64?                                                                                             Take         = null,
                                     UInt64?                                                                                             HistorySize  = 1)

        {

            if (GridOperatorStatus == null)
                return new JObject();

            try
            {

                return new JObject(GridOperatorStatus.
                                       SkipTakeFilter(Skip, Take).
                                       SafeSelect(statuslist => new JProperty(statuslist.Key.ToString(),
                                                                    new JObject(statuslist.Value.

                                                                                // Will filter multiple charging station status having the exact same ISO 8601 timestamp!
                                                                                GroupBy          (tsv   => tsv.  Timestamp.ToISO8601()).
                                                                                Select           (group => group.First()).

                                                                                OrderByDescending(tsv   => tsv.Timestamp).
                                                                                Take             (HistorySize).
                                                                                Select           (tsv   => new JProperty(tsv.Timestamp.ToISO8601(),
                                                                                                                         tsv.Value.    ToString())))

                                                                )));

            }
            catch
            {
                // e.g. when a Stack behind GridOperatorStatus is empty!
                return new JObject();
            }

        }

        #endregion

    }


    /// <summary>
    /// The common interface of all grid operators.
    /// </summary>
    public interface IGridOperator : IEntity<GridOperator_Id>,
                                     IAdminStatus<GridOperatorAdminStatusTypes>,
                                     IStatus<GridOperatorStatusTypes>,
                                     IEquatable<GridOperator>,
                                     IComparable<GridOperator>,
                                     IComparable,
                                     IRemoteGridOperator
    {

        #region Properties

        /// <summary>
        /// The roaming network of this grid operator.
        /// </summary>
        [InternalUseOnly]
        IRoamingNetwork?      RoamingNetwork        { get; }

        /// <summary>
        /// The remote grid operator.
        /// </summary>
        [InternalUseOnly]
        IRemoteGridOperator?  RemoteGridOperator    { get; }

        #endregion

        #region Events

        #region OnData/(Admin)StatusChanged

        /// <summary>
        /// An event fired whenever the static data changed.
        /// </summary>
        event OnGridOperatorDataChangedDelegate?         OnDataChanged;

        /// <summary>
        /// An event fired whenever the admin status changed.
        /// </summary>
        event OnGridOperatorAdminStatusChangedDelegate?  OnAdminStatusChanged;

        /// <summary>
        /// An event fired whenever the dynamic status changed.
        /// </summary>
        event OnGridOperatorStatusChangedDelegate?       OnStatusChanged;

        #endregion

        #endregion


        JObject ToJSON(Boolean  Embedded                         = false,
                       Boolean  ExpandChargingRoamingNetworkId   = false);


    }

}
