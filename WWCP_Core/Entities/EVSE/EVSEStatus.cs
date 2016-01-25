/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System;

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The current status of an EVSE.
    /// </summary>
    public class EVSEStatus
    {

        #region Properties

        #region EVSE

        private readonly EVSE _EVSE;

        /// <summary>
        /// The related the Electric Vehicle Supply Equipment (EVSE).
        /// </summary>
        public EVSE EVSE
        {
            get
            {
                return _EVSE;
            }
        }

        #endregion

        #region Id

        private readonly EVSE_Id _Id;

        /// <summary>
        /// The unique identification of an EVSE.
        /// </summary>
        public EVSE_Id Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region Status

        private readonly EVSEStatusType _Status;

        /// <summary>
        /// The current status of an EVSE.
        /// </summary>
        public EVSEStatusType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region EVSEStatusRecord(EVSE)

        /// <summary>
        /// Create a new EVSE status and store
        /// a reference to the given EVSE.
        /// </summary>
        /// <param name="EVSE">The current status of an EVSE.</param>
        public EVSEStatus(EVSE EVSE)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException("EVSE", "The given EVSE must not be null!");

            #endregion

            this._EVSE    = EVSE;
            this._Id      = EVSE.Id;
            this._Status  = EVSE.Status.Value;

        }

        #endregion

        #region EVSEStatusRecord(Id, Status)

        /// <summary>
        /// Create a new EVSE status.
        /// </summary>
        /// <param name="Id">The unique identification of an EVSE.</param>
        /// <param name="Status">The current status of an EVSE.</param>
        public EVSEStatus(EVSE_Id         Id,
                          EVSEStatusType  Status)

        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The given unique identification of an EVSE must not be null!");

            #endregion

            this._Id      = Id;
            this._Status  = Status;

        }

        #endregion

        #endregion


    }

}
