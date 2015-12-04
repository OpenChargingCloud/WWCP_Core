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

namespace org.GraphDefined.WWCP.LocalService
{

    public class RemoteStartResult
    {

        #region Properties

        #region Result

        private readonly RemoteStartResultType _Result;

        public RemoteStartResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region SessionId

        private readonly ChargingSession_Id _SessionId;

        public ChargingSession_Id SessionId
        {
            get
            {
                return _SessionId;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public RemoteStartResult(RemoteStartResultType  Result,
                                 ChargingSession_Id     SessionId = null)
        {
            this._Result     = Result;
            this._SessionId  = SessionId;
        }

        #endregion


        public static RemoteStartResult Success
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.Success);
            }
        }

        public static RemoteStartResult Error
        {
            get
            {
                return new RemoteStartResult(RemoteStartResultType.Error);
            }
        }

    }


    public enum RemoteStartResultType
    {
        Error,
        Success,
        EVSE_NotReachable,
        SessionId_AlreadyInUse,
        EVSE_AlreadyInUse,
        UnknownEVSE,
        EVSEReserved,
        EVSEOutOfService,
        Start_Timeout
    }

}
