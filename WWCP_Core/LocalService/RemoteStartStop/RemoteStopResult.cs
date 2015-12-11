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

    /// <summary>
    /// The result of a remote stop operation.
    /// </summary>
    public class RemoteStopResult
    {

        #region Properties

        #region Result

        private readonly RemoteStopResultType _Result;

        /// <summary>
        /// The result of a remote stop operation.
        /// </summary>
        public RemoteStopResultType Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        public RemoteStopResult(RemoteStopResultType  Result)
        {

            this._Result  = Result;

        }

        #endregion


        #region (static) Unknown

        /// <summary>
        /// Create a new remote stop 'Unknown' result.
        /// </summary>
        public static RemoteStopResult Unknown
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Unknown);
            }
        }

        #endregion

        #region (static) Success

        /// <summary>
        /// Create a new remote stop 'Success' result.
        /// </summary>
        public static RemoteStopResult Success
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Success);
            }
        }

        #endregion

        #region (static) Error

        /// <summary>
        /// Create a new remote stop 'Error' result.
        /// </summary>
        public static RemoteStopResult Error
        {
            get
            {
                return new RemoteStopResult(RemoteStopResultType.Error);
            }
        }

        #endregion

    }


    /// <summary>
    /// The result types of a remote stop operation.
    /// </summary>
    public enum RemoteStopResultType
    {

        /// <summary>
        /// The result is unknown or should be ignored.
        /// </summary>
        Unknown,

        Success,

        Error,

        EVSE_NotReachable,
        Stop_Timeout,
        UnknownEVSE,
        EVSEOutOfService,
        SessionIsInvalid

    }

}
