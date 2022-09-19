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

using System;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The admin status schedule of a charging station.
    /// </summary>
    public class ChargingStationStatusSchedule : AInternalData
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charging station.
        /// </summary>
        public ChargingStation_Id                          Id               { get; }

        /// <summary>
        /// The timestamped status of the charging station.
        /// </summary>
        public StatusSchedule<ChargingStationStatusTypes>  StatusSchedule   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station admin status.
        /// </summary>
        /// <param name="Id">The unique identification of the charging station.</param>
        /// <param name="StatusSchedule">The timestamped admin status of the charging station.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public ChargingStationStatusSchedule(ChargingStation_Id                          Id,
                                             StatusSchedule<ChargingStationStatusTypes>  StatusSchedule,
                                             IReadOnlyDictionary<String, Object>         CustomData  = null)

            : base(null,
                   CustomData)

        {

            this.Id              = Id;
            this.StatusSchedule  = StatusSchedule;

        }

        #endregion

    }

}
