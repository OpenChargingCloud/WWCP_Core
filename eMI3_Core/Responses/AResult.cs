/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 HTTP <http://www.github.com/eMI3/HTTP>
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

namespace org.emi3group.LocalService
{

    public abstract class AResult
    {

        #region Properties

        #region AuthorizatorId

        private readonly AuthorizatorId _AuthorizatorId;

        public AuthorizatorId AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region PartnerSessionId

        public SessionId  PartnerSessionId         { get; set; }

        #endregion

        #region Description

        public String     Description              { get; set; }

        #endregion

        #endregion

        #region Constructor(s)

        public AResult(AuthorizatorId AuthorizatorId)
        {
            this._AuthorizatorId = AuthorizatorId;
        }

        #endregion

    }

}
