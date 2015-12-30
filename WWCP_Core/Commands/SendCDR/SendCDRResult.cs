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

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP
{

    public class SendCDRResult
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

        #region Status

        private SendCDRResultType _Status;

        public SendCDRResultType Status
        {
            get
            {
                return _Status;
            }
        }

        #endregion

        #region Description

        private String _Description;

        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #region AdditionalInfo

        private String _AdditionalInfo;

        public String AdditionalInfo
        {
            get
            {
                return _AdditionalInfo;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        private SendCDRResult(SendCDRResultType  Result,
                              Authorizator_Id    AuthorizatorId,
                              String             Description = null)
        {

            this._AuthorizatorId  = AuthorizatorId;
            this._Status          = Result;
            this._Description     = Description;

        }

        #endregion


        #region (static) Forwarded(AuthorizatorId)

        public static SendCDRResult Forwarded(Authorizator_Id AuthorizatorId)
        {
            return new SendCDRResult(SendCDRResultType.Forwarded, AuthorizatorId);
        }

        #endregion

        #region (static) False(AuthorizatorId, Description = null)

        public static SendCDRResult False(Authorizator_Id  AuthorizatorId,
                                          String           Description = null)
        {

            return new SendCDRResult(SendCDRResultType.False,
                                     AuthorizatorId,
                                     Description);

        }

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId, Description = null)

        public static SendCDRResult InvalidSessionId(Authorizator_Id  AuthorizatorId,
                                                     String           Description = null)
        {

            return new SendCDRResult(SendCDRResultType.InvalidSessionId,
                                     AuthorizatorId,
                                     Description != null ? Description : "Invalid session identification!");

        }

        #endregion

        #region (static) NotForwared(AuthorizatorId, Description = null)

        public static SendCDRResult NotForwared(Authorizator_Id  AuthorizatorId,
                                                String           Description = null)
        {

            return new SendCDRResult(SendCDRResultType.NotForwared,
                                     AuthorizatorId,
                                     Description);

        }

        #endregion

    }


    public enum SendCDRResultType
    {
        Forwarded,
        False,
        InvalidSessionId,
        NotForwared
    }

}
