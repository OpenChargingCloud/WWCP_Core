﻿/*
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

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// A delegate for mapping operator identifications.
    /// </summary>
    /// <param name="OperatorId">An operator identification to be mapped.</param>
    public delegate String  CustomOperatorIdMapperDelegate  (String              OperatorId);

    /// <summary>
    /// A delegate for mapping EVSE Ids.
    /// </summary>
    /// <param name="EVSEId">An EVSE to be mapped.</param>
    public delegate String  CustomEVSEIdMapperDelegate      (String              EVSEId);

}
