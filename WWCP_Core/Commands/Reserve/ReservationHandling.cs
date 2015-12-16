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

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP
{

    public class ReservationHandling
    {

        #region Properties

        #region KeepAlive

        public Boolean KeepAlive
        {
            get
            {
                return _KeepAliveTime.HasValue;
            }
        }

        #endregion

        #region KeepAliveTime

        private readonly TimeSpan? _KeepAliveTime;

        public TimeSpan? KeepAliveTime
        {
            get
            {
                return _KeepAliveTime;
            }
        }

        #endregion

        #region Timeout

        private readonly DateTime? _Timeout;

        public DateTime? Timeout
        {
            get
            {
                return _Timeout;
            }
        }

        #endregion

        #region (static) Close

        public static ReservationHandling Close
        {
            get
            {
                return new ReservationHandling(TimeSpan.Zero);
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public ReservationHandling(TimeSpan KeepAliveTime)
        {

            this._KeepAliveTime  = KeepAliveTime;
            this._Timeout        = DateTime.Now + KeepAliveTime;

        }

        #endregion

    }

}
