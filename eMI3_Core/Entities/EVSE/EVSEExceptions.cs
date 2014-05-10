/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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
using System.Collections.Generic;
using System.Collections.Concurrent;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// An EVSE exception.
    /// </summary>
    public class EVSEException : eMI3Exception
    {

        public EVSEException(String Message)
            : base(Message)
        { }

        public EVSEException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region SocketOutletAlreadyExists

    /// <summary>
    /// An exception thrown whenever a socket outlet already exists within the given EVSE.
    /// </summary>
    public class SocketOutletAlreadyExists : EVSEException
    {

        public SocketOutletAlreadyExists(SocketOutlet_Id   SocketOutlet_Id,
                                         EVSE_Id           EVSE_Id)
            : base("The given socket outlet identification '" + SocketOutlet_Id + "' already exists within the given '" + EVSE_Id + "' EVSE!")
        { }

    }

    #endregion

}
