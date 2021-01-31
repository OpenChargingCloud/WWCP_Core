/*
 * Copyright (c) 2014-2021 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A delegate for mapping operator identifications.
    /// </summary>
    /// <param name="OperatorId">An operator identification to be mapped.</param>
    public delegate String  CustomOperatorIdMapperDelegate  (String              OperatorId);


    /// <summary>
    /// A delegate for filtering EVSE identifications.
    /// </summary>
    /// <param name="EVSEId">An EVSE identification to include.</param>
    public delegate Boolean IncludeEVSEIdDelegate           (EVSE_Id             EVSEId);

    /// <summary>
    /// A delegate for filtering EVSEs.
    /// </summary>
    /// <param name="EVSE">An EVSE to include.</param>
    public delegate Boolean IncludeEVSEDelegate             (EVSE                EVSE);

    /// <summary>
    /// A delegate for mapping EVSE Ids.
    /// </summary>
    /// <param name="EVSEId">An EVSE to be mapped.</param>
    public delegate String  CustomEVSEIdMapperDelegate      (String              EVSEId);


    /// <summary>
    /// A delegate for filtering charging station identifications.
    /// </summary>
    /// <param name="ChargingStationId">A charging station identification to include.</param>
    public delegate Boolean IncludeChargingStationIdDelegate(ChargingStation_Id  ChargingStationId);

    /// <summary>
    /// A delegate for filtering charging stations.
    /// </summary>
    /// <param name="ChargingStation">A charging station to include.</param>
    public delegate Boolean IncludeChargingStationDelegate  (ChargingStation     ChargingStation);


    /// <summary>
    /// A delegate for filtering charging pool identifications.
    /// </summary>
    /// <param name="ChargingPoolId">A charging pool identification to include.</param>
    public delegate Boolean IncludeChargingPoolIdDelegate   (ChargingPool_Id     ChargingPoolId);

    /// <summary>
    /// A delegate for filtering charging pools.
    /// </summary>
    /// <param name="ChargingPool">A charging pool to include.</param>
    public delegate Boolean IncludeChargingPoolDelegate     (ChargingPool        ChargingPool);

}
