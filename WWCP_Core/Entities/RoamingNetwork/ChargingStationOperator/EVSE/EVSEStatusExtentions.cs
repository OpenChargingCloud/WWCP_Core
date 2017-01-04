/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extention emthods for the EVSE status.
    /// </summary>
    public static class EVSEStatusExtentions
    {

        #region Contains(this EVSEStatus, Id, Status)

        /// <summary>
        /// Check if the given enumeration of EVSEs and their current status
        /// contains the given pair of EVSE identification and status.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSEs and their current status.</param>
        /// <param name="Id">An EVSE identification.</param>
        /// <param name="Status">An EVSE status.</param>
        public static Boolean Contains(this IEnumerable<EVSEStatus>  EVSEStatus,
                                       EVSE_Id                       Id,
                                       EVSEStatusType                Status)
        {

            foreach (var status in EVSEStatus)
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
