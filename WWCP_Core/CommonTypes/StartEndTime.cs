/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
using System;

#endregion

namespace org.GraphDefined.WWCP
{

    public struct StartEndTime
    {

        #region Properties

        #region StartTime

        private DateTime _StartTime;

        public DateTime StartTime
        {
            get
            {
                return _StartTime;
            }
        }

        public DateTime? StartTimeOpt
        {
            get
            {
                return _StartTime;
            }
        }

        #endregion

        #region EndTime

        private DateTime _EndTime;

        public DateTime EndTime
        {
            get
            {
                return _EndTime;
            }
        }

        public DateTime? EndTimeOpt
        {
            get
            {
                return _EndTime;
            }
        }

        #endregion

        #region Duration

        public TimeSpan Duration
        {
            get
            {
                return _EndTime - _StartTime;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public StartEndTime(DateTime  StartTime,
                            DateTime  EndTime)
        {

            _StartTime  = StartTime;
            _EndTime    = EndTime;

        }

        #endregion


        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return _StartTime.GetHashCode() ^ _EndTime.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return _StartTime.ToString() + " -> " + _EndTime.ToString();
        }

        #endregion

    }

}
