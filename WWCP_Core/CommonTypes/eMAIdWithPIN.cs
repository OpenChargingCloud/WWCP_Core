/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
 * This file is part of WWCP OICP <https://github.com/GraphDefined/WWCP_OICP>
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP
{

    public enum PINCrypto
    {
        none,
        MD5,
        SHA1
    }


    public class eMAIdWithPIN2
    {

        #region Properties

        #region eMAId

        private readonly eMA_Id _eMAId;

        public eMA_Id eMAId
        {
            get
            {
                return _eMAId;
            }
        }

        #endregion

        #region PIN

        private readonly String _PIN;

        public String PIN
        {
            get
            {
                return _PIN;
            }
        }

        #endregion

        #region Function

        private readonly PINCrypto _Function;

        public PINCrypto Function
        {
            get
            {
                return _Function;
            }
        }

        #endregion

        #region Salt

        private readonly String _Salt;

        public String Salt
        {
            get
            {
                return _Salt;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region eMAIdWithPIN(eMAId, PIN)

        public eMAIdWithPIN2(eMA_Id  eMAId,
                            String  PIN)
        {

            this._eMAId     = eMAId;
            this._PIN       = PIN;
            this._Function  = PINCrypto.none;

        }

        #endregion

        #region eMAIdWithPIN(eMAId, PIN, Function, Salt = "")

        public eMAIdWithPIN2(eMA_Id     eMAId,
                            String     PIN,
                            PINCrypto  Function,
                            String     Salt = "")
        {

            this._eMAId     = eMAId;
            this._PIN       = PIN;
            this._Function  = Function;
            this._Salt      = Salt;

        }

        #endregion

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_eMAId.ToString(), " -", _Function != PINCrypto.none ? _Function.ToString(): "", "-> ", _PIN );
        }

        #endregion

    }

}
