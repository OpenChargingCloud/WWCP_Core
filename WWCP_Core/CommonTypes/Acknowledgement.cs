/*
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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    public enum ResultType
    {
        True,
        NoOperation,
        False
    }


    /// <summary>
    /// An acknowledgement.
    /// </summary>
    public class Acknowledgement
    {

        #region Properties

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public ResultType           Result          { get; }

        /// <summary>
        /// An optional description of the result code.
        /// </summary>
        public String               Description     { get; }

        /// <summary>
        /// Warnings or additional information.
        /// </summary>
        public IEnumerable<String>  Warnings        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">An optional description of the result code.</param>
        /// <param name="Warnings">Warnings or additional information.</param>
        public Acknowledgement(ResultType           Result,
                               String               Description  = null,
                               IEnumerable<String>  Warnings     = null)
        {

            this.Result       = Result;
            this.Description  = Description.IsNotNullOrEmpty() ? Description.Trim() : null;
            this.Warnings     = Warnings.Select(warning => warning.Trim()).
                                         Where (warning => warning.IsNotNullOrEmpty());

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
