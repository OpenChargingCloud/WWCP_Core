﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

        public Authorizator_Id    AuthorizatorId    { get; }

        public SendCDRResultType  Status            { get; }

        public String             Description       { get; }

        public String             AdditionalInfo    { get; }

        #endregion

        #region Constructor(s)

        private SendCDRResult(SendCDRResultType  Result,
                              Authorizator_Id    AuthorizatorId,
                              String             Description = null)
        {

            this.AuthorizatorId  = AuthorizatorId;
            this.Status          = Result;
            this.Description     = Description;

        }

        #endregion


        #region (static) NotForwared(AuthorizatorId, Description = null)

        public static SendCDRResult NotForwared(Authorizator_Id  AuthorizatorId,
                                                String           Description = null)

            => new SendCDRResult(SendCDRResultType.NotForwared,
                                 AuthorizatorId,
                                 Description);

        #endregion

        #region (static) Enqueued(AuthorizatorId, Description = null)

        public static SendCDRResult Enqueued(Authorizator_Id  AuthorizatorId,
                                             String           Description = null)

            => new SendCDRResult(SendCDRResultType.Enqueued,
                                 AuthorizatorId,
                                 Description);

        #endregion

        #region (static) Forwarded(AuthorizatorId)

        public static SendCDRResult Forwarded(Authorizator_Id AuthorizatorId)

            => new SendCDRResult(SendCDRResultType.Forwarded, AuthorizatorId);

        #endregion

        #region (static) InvalidSessionId(AuthorizatorId, Description = null)

        public static SendCDRResult InvalidSessionId(Authorizator_Id  AuthorizatorId,
                                                     String           Description = null)

            => new SendCDRResult(SendCDRResultType.InvalidSessionId,
                                 AuthorizatorId,
                                 Description != null ? Description : "Invalid session identification!");

        #endregion

        #region (static) UnknownEVSE(AuthorizatorId, Description = null)

        public static SendCDRResult UnknownEVSE(Authorizator_Id  AuthorizatorId,
                                                String           Description = null)
            => new SendCDRResult(SendCDRResultType.UnknownEVSE,
                                 AuthorizatorId,
                                 Description);

        #endregion

        #region (static) OutOfService(AuthorizatorId, Description = null)

        public static SendCDRResult OutOfService(Authorizator_Id  AuthorizatorId,
                                                 String           Description = null)
            => new SendCDRResult(SendCDRResultType.OutOfService,
                                 AuthorizatorId,
                                 Description);

        #endregion

        #region (static) Error(AuthorizatorId, Description = null)

        public static SendCDRResult Error(Authorizator_Id  AuthorizatorId,
                                          String           Description = null)
            => new SendCDRResult(SendCDRResultType.Error,
                                 AuthorizatorId,
                                 Description);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Status + " via " + AuthorizatorId;

        #endregion

    }


    public enum SendCDRResultType
    {

        NotForwared,

        Enqueued,

        Forwarded,

        InvalidSessionId,

        UnknownEVSE,

        OutOfService,

        Error

    }

}
