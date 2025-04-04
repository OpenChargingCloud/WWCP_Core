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

#region Usings

using System;
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class POIDataPull<T>
    {

        public IEnumerable<T>        POIData     { get; }
        public IEnumerable<Warning>  Warnings    { get; }

        public POIDataPull(IEnumerable<T>        POIData,
                           IEnumerable<Warning>  Warnings = null)
        {

            this.POIData   = POIData;
            this.Warnings  = Warnings != null
                                 ? Warnings.Where(warning => warning != null)
                                 : new Warning[0];

        }

        public POIDataPull(IEnumerable<T>    POIData,
                           params Warning[]  Warnings)
        {

            this.POIData   = POIData;
            this.Warnings  = Warnings ?? new Warning[0];

        }

    }

}
