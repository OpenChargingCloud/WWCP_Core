/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3_Core>
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

namespace org.GraphDefined.eMI3
{

    /// <summary>
    /// An opening time.
    /// </summary>
    public class OpeningTime
    {

        #region Properties

        #region IsOpen24Hours

        private readonly Boolean _IsOpen24Hours;

        public Boolean IsOpen24Hours
        {
            get
            {
                return _IsOpen24Hours;
            }
        }

        #endregion

        #region Simple

        private readonly String _Simple;

        public String Simple
        {
            get
            {
                return _Simple;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region OpeningTime(IsOpen24Hours = true)

        public OpeningTime(Boolean IsOpen24Hours = true)
        {
            this._IsOpen24Hours  = IsOpen24Hours;
            this._Simple         = "Mon-Sun, 24 hours";
        }

        #endregion

        #region OpeningTime(Simple)

        public OpeningTime(String Simple)
        {
            this._IsOpen24Hours  = false;
            this._Simple         = Simple;
        }

        #endregion

        #endregion


        public static OpeningTime Is24Hours
        {
            get
            {
                return new OpeningTime(true);
            }
        }


    }

}
