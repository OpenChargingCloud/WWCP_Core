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

    public abstract class AResult
    {

        #region Properties

        #region AuthorizatorId

        private readonly Authorizator_Id _AuthorizatorId;

        public Authorizator_Id AuthorizatorId
        {
            get
            {
                return _AuthorizatorId;
            }
        }

        #endregion

        #region PartnerSessionId

        public ChargingSession_Id  PartnerSessionId         { get; set; }

        #endregion

        #region Description

        public String     Description              { get; set; }

        #endregion

        #region AdditionalInfo

        public String     AdditionalInfo           { get; set; }

        #endregion

        #endregion

        #region Constructor(s)

        public AResult(Authorizator_Id AuthorizatorId)
        {
            this._AuthorizatorId = AuthorizatorId;
        }

        #endregion

    }

}
