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
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP
{

    public class PriorityList<T>
    {

        #region Data

        private readonly ConcurrentDictionary<UInt32, T> _Dictionary;

        #endregion

        public PriorityList()
        {

            this._Dictionary = new ConcurrentDictionary<UInt32, T>();

        }


        public void Add(T iRemoteAuthorizeStartStop)
        {
            lock (_Dictionary)
            {

                _Dictionary.TryAdd(_Dictionary.Count > 0
                                       ? _Dictionary.Keys.Max() + 1
                                       : 1,
                                   iRemoteAuthorizeStartStop);

            }
        }


        public Task<T2[]> WhenAll<T2>(Func<T, Task<T2>> Work)
        {

            return Task.WhenAll(_Dictionary.
                                    OrderBy(kvp => kvp.Key).
                                    Select(kvp => Work(kvp.Value)));

        }


    }

}
