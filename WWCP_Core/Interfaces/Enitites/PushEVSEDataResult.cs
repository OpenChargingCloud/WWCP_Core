/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;
using System.Linq;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    // PushEVSEData
    // PushChargingStationData
    // PushChargingPoolData
    // ...

    public class PushSingleEVSEDataResult
    {

        public IEVSE                      EVSE        { get; }
        public PushSingleDataResultTypes  Result      { get; }
        public IEnumerable<Warning>       Warnings    { get; }

        public PushSingleEVSEDataResult(IEVSE                      EVSE,
                                        PushSingleDataResultTypes  Result,
                                        IEnumerable<Warning>?      Warnings   = null)
        {

            this.EVSE      = EVSE;
            this.Result    = Result;
            this.Warnings  = Warnings is not null
                                 ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                 : Array.Empty<Warning>();

        }

    }

    public class PushSingleChargingStationDataResult
    {

        public IChargingStation           ChargingStation    { get; }
        public PushSingleDataResultTypes  Result             { get; }
        public IEnumerable<Warning>       Warnings           { get; }

        public PushSingleChargingStationDataResult(IChargingStation           ChargingStation,
                                                   PushSingleDataResultTypes  Result,
                                                   IEnumerable<Warning>?      Warnings   = null)
        {

            this.ChargingStation  = ChargingStation;
            this.Result           = Result;
            this.Warnings         = Warnings is not null
                                        ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                        : Array.Empty<Warning>();

        }

    }

    public class PushSingleChargingPoolDataResult
    {

        public IChargingPool              ChargingPool    { get; }
        public PushSingleDataResultTypes  Result          { get; }
        public IEnumerable<Warning>       Warnings        { get; }

        public PushSingleChargingPoolDataResult(IChargingPool              ChargingPool,
                                                PushSingleDataResultTypes  Result,
                                                IEnumerable<Warning>?      Warnings   = null)
        {

            this.ChargingPool  = ChargingPool;
            this.Result        = Result;
            this.Warnings      = Warnings is not null
                                     ? Warnings.Where(warning => warning.IsNeitherNullNorEmpty())
                                     : Array.Empty<Warning>();

        }

    }

}
