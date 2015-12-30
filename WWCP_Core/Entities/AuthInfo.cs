/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

    public class AuthInfo
    {

        #region Properties

        #region AuthToken

        private readonly Auth_Token _AuthToken;

        public Auth_Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
        }

        #endregion

        #region QRCodeIdentification

        private readonly eMAIdWithPIN2 _QRCodeIdentification;

        public eMAIdWithPIN2 QRCodeIdentification
        {
            get
            {
                return _QRCodeIdentification;
            }
        }

        #endregion

        #region PlugAndChargeIdentification

        private readonly eMA_Id _PlugAndChargeIdentification;

        public eMA_Id PlugAndChargeIdentification
        {
            get
            {
                return _PlugAndChargeIdentification;
            }
        }

        #endregion

        #region RemoteIdentification

        private readonly eMA_Id _RemoteIdentification;

        public eMA_Id RemoteIdentification
        {
            get
            {
                return _RemoteIdentification;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) AuthInfo(AuthToken)

        private AuthInfo(Auth_Token  AuthToken)
        {
            this._AuthToken                     = AuthToken;
        }

        #endregion

        #region (private) AuthInfo(QRCodeIdentification)

        private AuthInfo(eMAIdWithPIN2 QRCodeIdentification)
        {
            this._QRCodeIdentification = QRCodeIdentification;
        }

        #endregion

        #region (private) AuthInfo(PlugAndChargeIdentification, IsPnC)

        private AuthInfo(eMA_Id PlugAndChargeIdentification,
                                           Boolean IsPnC)
        {
            this._PlugAndChargeIdentification   = PlugAndChargeIdentification;
        }

        #endregion

        #region (private) AuthInfo(RemoteIdentification)

        private AuthInfo(eMA_Id RemoteIdentification)
        {
            this._RemoteIdentification          = RemoteIdentification;
        }

        #endregion

        #endregion


        #region (static) FromAuthToken(AuthToken)

        public static AuthInfo FromAuthToken(Auth_Token AuthToken)
        {
            return new AuthInfo(AuthToken);
        }

        #endregion

        #region (static) FromQRCodeIdentification(eMAId, PIN)

        public static AuthInfo FromQRCodeIdentification(eMA_Id  eMAId,
                                                                           String  PIN)
        {
            return new AuthInfo(new eMAIdWithPIN2(eMAId, PIN));
        }

        #endregion

        #region (static) FromQRCodeIdentification(QRCodeIdentification)

        public static AuthInfo FromQRCodeIdentification(eMAIdWithPIN2 QRCodeIdentification)
        {
            return new AuthInfo(QRCodeIdentification);
        }

        #endregion

        #region (static) FromPlugAndChargeIdentification(PlugAndChargeIdentification)

        public static AuthInfo FromPlugAndChargeIdentification(eMA_Id PlugAndChargeIdentification)
        {
            return new AuthInfo(PlugAndChargeIdentification);
        }

        #endregion

        #region (static) FromRemoteIdentification(RemoteIdentification)

        public static AuthInfo FromRemoteIdentification(eMA_Id RemoteIdentification)
        {
            return new AuthInfo(RemoteIdentification);
        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {

            if (_AuthToken != null)
                return _AuthToken.ToString();

            if (_QRCodeIdentification != null)
                return _QRCodeIdentification.ToString();

            if (_PlugAndChargeIdentification != null)
                return _PlugAndChargeIdentification.ToString();

            if (_RemoteIdentification != null)
                return _RemoteIdentification.ToString();

            return String.Empty;

        }

        #endregion

    }

}
