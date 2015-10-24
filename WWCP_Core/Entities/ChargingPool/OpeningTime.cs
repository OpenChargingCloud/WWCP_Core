/*
 * Copyright (c) 2014-2015 GraphDefined GmbH
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

        #region Text

        private String _Text;

        public String Text
        {

            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
            }

        }

        #endregion

        #endregion

        #region Constructor(s)

        #region OpeningTime(IsOpen24Hours = true)

        public OpeningTime(Boolean IsOpen24Hours = true)
        {
            this._IsOpen24Hours  = IsOpen24Hours;
            this._Text           = IsOpen24Hours ? "Mon-Sun, 24 hours" : "";
        }

        #endregion

        #region OpeningTime(Text)

        public OpeningTime(String Text)
        {
            this._IsOpen24Hours  = false;
            this._Text           = Text;
        }

        #endregion

        #endregion



        #region (static) Open24Hours

        /// <summary>
        /// Is open for 24 hours a day.
        /// </summary>
        public static OpeningTime Open24Hours
        {
            get
            {
                return new OpeningTime(true);
            }
        }

        #endregion


        #region Operator overloading

        #region Operator == (OpeningTime1, OpeningTime2)

        /// <summary>
        /// Compares two opening times for equality.
        /// </summary>
        /// <param name="OpeningTime1">An opening time.</param>
        /// <param name="OpeningTime2">Another opening time.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (OpeningTime OpeningTime1, OpeningTime OpeningTime2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(OpeningTime1, OpeningTime2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) OpeningTime1 == null) || ((Object) OpeningTime2 == null))
                return false;

            return OpeningTime1.Equals(OpeningTime2);

        }

        #endregion

        #region Operator != (OpeningTime1, OpeningTime2)

        /// <summary>
        /// Compares two opening times for inequality.
        /// </summary>
        /// <param name="OpeningTime1">An opening time.</param>
        /// <param name="OpeningTime2">Another opening time.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (OpeningTime OpeningTime1, OpeningTime OpeningTime2)
        {
            return !(OpeningTime1 == OpeningTime2);
        }

        #endregion

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

            return Text.Equals(OpenTime.Text);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()
        {
            return Text.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Get a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return IsOpen24Hours ? "24 hours" : Text;
        }

        #endregion

    }

}
