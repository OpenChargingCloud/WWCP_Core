/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Net <https://github.com/GraphDefined/WWCP_Net>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.Net
{

    /// <summary>
    /// WWCP HTTP API extention methods.
    /// </summary>
    public static class ExtentionMethods
    {

        #region SkipTakeFilter(this Enumeration, Skip = 0, Take = 0)

        /// <summary>
        /// Return a JSON representation for the given enumeration of roaming networks.
        /// </summary>
        /// <param name="Enumeration">An enumeration of roaming networks.</param>
        /// <param name="Skip">The optional number of roaming networks to skip.</param>
        /// <param name="Take">The optional number of roaming networks to return.</param>
        public static IEnumerable<T> SkipTakeFilter<T>(this IEnumerable<T>  Enumeration,
                                                       UInt64               Skip  = 0,
                                                       UInt64               Take  = 0)
        {

            #region Initial checks

            if (Enumeration == null)
                return new T[0];

            #endregion

            if (Take == 0)
                return Enumeration.Skip(Skip);

            return Enumeration.Skip(Skip).Take(Take);

        }

        #endregion

    }

}
