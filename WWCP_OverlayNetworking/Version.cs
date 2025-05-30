﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP OverlayNetworking <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace cloud.charging.open.protocols.WWCP.OverlayNetworking
{

    /// <summary>
    /// The current EEBus version.
    /// </summary>
    public static class Version
    {

        /// <summary>
        /// This EEBus version 0.1 as text "v0.1".
        /// </summary>
        public const            String      String   = "v0.1";

        /// <summary>
        /// This EEBus version "0.1" as version identification.
        /// </summary>
        public readonly static  Version_Id  Id       = Version_Id.Parse(String[1..]);

    }

}
