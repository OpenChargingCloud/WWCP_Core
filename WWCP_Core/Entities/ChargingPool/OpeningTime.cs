/*
 * Copyright (c) 2014-2015 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/WorldWideCharging/WWCP_Core>
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
    /// An opening time.
    /// </summary>
    public class OpeningTime : IEquatable<OpeningTime>
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

        private String _Simple;

        public String Simple
        {

            get
            {
                return _Simple;
            }

            set
            {
                _Simple = value;
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


        #region (static) Is24Hours

        /// <summary>
        /// Is open for 24 hours a day.
        /// </summary>
        public static OpeningTime Is24Hours
        {
            get
            {
                return new OpeningTime(true);
            }
        }

        #endregion


        #region IEquatable<OpeningTime> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an OpeningTime.
            var OpenTime = Object as OpeningTime;
            if ((Object) OpenTime == null)
                return false;

            return this.Equals(OpenTime);

        }

        #endregion

        #region Equals(Operator)

        /// <summary>
        /// Compares two opening times for equality.
        /// </summary>
        /// <param name="Operator">An opening time to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OpeningTime OpenTime)
        {

            if ((Object) OpenTime == null)
                return false;

            if (IsOpen24Hours && OpenTime.IsOpen24Hours)
                return true;

            return Simple.Equals(OpenTime.Simple);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Simple.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return IsOpen24Hours ? "24 hours" : Simple;
        }

        #endregion

    }

}
