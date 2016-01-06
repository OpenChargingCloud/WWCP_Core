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

    /// <summary>
    /// An acknowledgement.
    /// </summary>
    public class Acknowledgement
    {

        #region Properties

        #region Result

        private readonly Boolean _Result;

        /// <summary>
        /// The result of the operation.
        /// </summary>
        public Boolean Result
        {
            get
            {
                return _Result;
            }
        }

        #endregion

        #region StatusCode

        private readonly String _Description;

        /// <summary>
        /// An optional description.
        /// </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new acknowledgement.
        /// </summary>
        /// <param name="Result">The result of the operation.</param>
        /// <param name="Description">The description of the result code.</param>
        public Acknowledgement(Boolean  Result,
                               String   Description  = null)
        {

            this._Result       = Result;
            this._Description  = Description;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat("Result: " + _Result + "; " + _Description);
        }

        #endregion

    }

}
