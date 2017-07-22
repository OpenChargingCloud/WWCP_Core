/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An acknowledgement.
    /// </summary>
    public class PushDataResult
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public ResultTypes           Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String               Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>  Warnings        { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        public TimeSpan?            Runtime         { get;  }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        public PushDataResult(ResultTypes           Result,
                               String               Description  = null,
                               IEnumerable<String>  Warnings     = null,
                               TimeSpan?            Runtime      = null)
        {

            this.Result       = Result;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : null;

            this.Warnings     = Warnings != null
                                    ? Warnings.Where     (warning => warning != null).
                                               SafeSelect(warning => warning.Trim()).
                                               Where     (warning => warning.IsNotNullOrEmpty())
                                    : null;

            this.Runtime      = Runtime;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("Result: " + Result + "; " + Description);

        #endregion

    }

}
