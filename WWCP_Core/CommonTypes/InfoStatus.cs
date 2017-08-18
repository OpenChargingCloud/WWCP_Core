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

using org.GraphDefined.Vanaheimr.Illias;
using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.WWCP
{

    public enum InfoStatus
    {

        Expand,
        ShowIdOnly,
        Hidden

    }


    public static class InfoStatusExtentions
    {

        public static JProperty Switch(this InfoStatus  Status,
                                       Func<JProperty>  WhenShowIdOnly,
                                       Func<JProperty>  WhenExpand)
        {

            switch (Status)
            {

                case InfoStatus.ShowIdOnly:
                    return WhenShowIdOnly();

                case InfoStatus.Expand:
                    return WhenExpand();

                default:
                    return null;

            }

        }

    }

}
