/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@belectric.com>
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

namespace de.eMI3
{

    /// <summary>
    /// A EVSEOperator exception.
    /// </summary>
    public class EVSEOperatorException : eMI3Exception
    {

        public EVSEOperatorException(String Message)
            : base(Message)
        { }

        public EVSEOperatorException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region EVSPoolAlreadyExists

    /// <summary>
    /// An exception thrown whenever an EVS pool already exists within the given EVSE operator.
    /// </summary>
    public class EVSPoolAlreadyExists : RoamingNetworkException
    {

        public EVSPoolAlreadyExists(EVSPool_Id       EVSPool_Id,
                                    EVSEOperator_Id  EVSEOperator_Id)
            : base("The given EVSPool identification '" + EVSPool_Id + "' already exists within the '" + EVSEOperator_Id + "' EVSE operator!")
        { }

    }

    #endregion

}
