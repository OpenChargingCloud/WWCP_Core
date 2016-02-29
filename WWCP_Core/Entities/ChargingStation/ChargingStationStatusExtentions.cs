/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extention emthods for the charging station status.
    /// </summary>
    public static class ChargingStationStatusExtentions
    {

        #region Contains(this ChargingStationStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of charging stations and their current status
        /// contains the given pair of charging station identification and status.
        /// </summary>
        /// <param name="ChargingStationStatus">An enumeration of charging stations and their current status.</param>
        /// <param name="Id">An charging station identification.</param>
        /// <param name="Status">An charging station status.</param>
        public static Boolean Contains(this IEnumerable<ChargingStationStatus>  ChargingStationStatus,
                                       ChargingStation_Id                       Id,
                                       ChargingStationStatusType                Status)
        {

            foreach (var status in ChargingStationStatus)
            {

                if (status.Id     == Id &&
                    status.Status == Status)
                    return true;

            }

            return false;

        }

        #endregion

    }

}
