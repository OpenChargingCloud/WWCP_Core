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

    public enum AuthorizationResult
    {
        Authorized,
        NotAuthorized,
        Blocked
    }

    #region (abstract) AUTHResult

    public abstract class AUTHResult : AResult
    {

        #region Properties

        public AuthorizationResult  AuthorizationResult   { get; set; }
        public ChargingSession_Id   SessionId             { get; set; }
        public EVSP_Id              ProviderId            { get; set; }

        #endregion

        #region Constructor(s)

        public AUTHResult(Authorizator_Id AuthorizatorId)
            : base(AuthorizatorId)
        { }

        #endregion


        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (ProviderId != null)
                return String.Concat(AuthorizationResult.ToString(), ", ", ProviderId);

            return String.Concat(AuthorizationResult.ToString());

        }

        #endregion

    }

    #endregion

}
