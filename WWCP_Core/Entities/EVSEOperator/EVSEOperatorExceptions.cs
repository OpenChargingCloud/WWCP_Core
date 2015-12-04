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

#region Usings

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// A EVSE operator exception.
    /// </summary>
    public class EVSEOperatorException : WWCPException
    {

        public EVSEOperatorException(String Message)
            : base(Message)
        { }

        public EVSEOperatorException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }


    #region ChargingPoolAlreadyExists

    /// <summary>
    /// An exception thrown whenever an EVS pool already exists within the given EVSE operator.
    /// </summary>
    public class ChargingPoolAlreadyExists : EVSEOperatorException
    {

        public ChargingPoolAlreadyExists(ChargingPool_Id       ChargingPool_Id,
                                    EVSEOperator_Id  EVSEOperator_Id)
            : base("The given ChargingPool identification '" + ChargingPool_Id + "' already exists within the given '" + EVSEOperator_Id + "' EVSE operator!")
        { }

    }

    #endregion

}
