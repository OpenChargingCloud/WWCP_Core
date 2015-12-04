/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP
{

    public enum ChargingFacilities
    {

        Unspecified,                 // Unspecified
        CF_100_120V_1Phase_less10A,  // 100 - 120V, 1-Phase ≤ 10A
        CF_100_120V_1Phase_less16A,  // 100 - 120V, 1-Phase ≤ 16A
        CF_100_120V_1Phase_less32A,  // 100 - 120V, 1-Phase ≤ 32A
        CF_200_240V_1Phase_less10A,  // 200 - 240V, 1-Phase ≤ 10A
        CF_200_240V_1Phase_less16A,  // 200 - 240V, 1-Phase ≤ 16A
        CF_200_240V_1Phase_less32A,  // 200 - 240V, 1-Phase ≤ 32A
        CF_200_240V_1Phase_over32A,  // 200 - 240V, 1-Phase > 32A
        CF_380_480V_3Phase_less16A,  // 380 - 480V, 3-Phase ≤ 16A
        CF_380_480V_3Phase_less32A,  // 380 - 480V, 3-Phase ≤ 32A
        CF_380_480V_3Phase_less63A,  // 380 - 480V, 3-Phase ≤ 63A
        Battery_exchange,            // Battery exchange
        DCCharging_less20kW,         // DC Charging ≤ 20kW
        DCCharging_less50kW,         // DC Charging ≤ 50kW
        DCCharging_over50kW          // DC Charging > 50kW

    }

}
