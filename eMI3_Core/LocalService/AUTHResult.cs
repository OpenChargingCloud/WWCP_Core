/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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

        public AuthorizationResult   AuthorizationResult   { get; set; }
        public SessionId             SessionId             { get; set; }
        public EVServiceProvider_Id  ProviderId            { get; set; }

        #endregion

        #region Constructor(s)

        public AUTHResult(String AuthorizatorId)
            : base(AuthorizatorId)
        { }

        #endregion

    }

    #endregion

}
