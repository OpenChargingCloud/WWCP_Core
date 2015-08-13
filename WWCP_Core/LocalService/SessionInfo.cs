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

using System;

#endregion

namespace org.GraphDefined.WWCP.LocalService
{

    /// <summary>
    /// E-Mobility Charging Session Information.
    /// </summary>
    public class SessionInfo
    {

        #region Properties

        #region Created

        private readonly DateTime _Created;

        public DateTime Created
        {
            get
            {
                return _Created;
            }
        }

        #endregion

        #region Token

        private readonly Auth_Token _Token;

        public Auth_Token Token
        {
            get
            {
                return _Token;
            }
        }

        #endregion

        public Boolean Finished     { get; set; }

        #endregion

        #region Constructor(s)

        public SessionInfo(Auth_Token Token)
        {
            this._Created   = DateTime.Now;
            this._Token     = Token;
        }

        #endregion

    }

}
