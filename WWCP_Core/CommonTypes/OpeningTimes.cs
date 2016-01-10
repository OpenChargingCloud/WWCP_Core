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
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Opening times.
    /// </summary>
    public class OpeningTimes : IEquatable<OpeningTimes>
    {

        #region Properties

        #region RegularOpenings

        private readonly RegularHours[] _RegularOpenings;

        /// <summary>
        /// The regular openings.
        /// </summary>
        public IEnumerable<RegularHours> RegularOpenings
        {
            get
            {
                return _RegularOpenings.Where(rh => !(rh.Weekday == DayOfWeek.Sunday && rh.Begin.Hour == 0 && rh.Begin.Minute == 0 && rh.End.Hour == 0 && rh.End.Minute == 0));
            }
        }

        #endregion

        private readonly List<ExceptionalPeriod> _ExceptionalOpenings;
        private readonly List<ExceptionalPeriod> _ExceptionalClosings;

        #region IsOpen24Hours

        /// <summary>
        /// 24/7 open...
        /// </summary>
        public Boolean IsOpen24Hours
        {
            get
            {
                return !RegularOpenings.Any();
            }
        }

        #endregion

        #region Text

        private String _Text;

        /// <summary>
        /// An additoonal free text.
        /// </summary>
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

        #region OpeningTime(Text = "")

        private OpeningTimes(String Text = "")
        {

            this._RegularOpenings      = new RegularHours[7];
            this._ExceptionalOpenings  = new List<ExceptionalPeriod>();
            this._ExceptionalClosings  = new List<ExceptionalPeriod>();
            this._Text                 = Text;

        }

        #endregion

        #region OpeningTime(FromWeekday, ToWeekday, Begin, End, Text = "")

        public OpeningTimes(DayOfWeek  FromWeekday,
                            DayOfWeek  ToWeekday,
                            HourMin    Begin,
                            HourMin    End,
                            String     Text = "")

            : this(Text)

        {

            SetRegularOpening(FromWeekday, ToWeekday, Begin, End);

        }

        #endregion

        #endregion


        public OpeningTimes SetRegularOpening(DayOfWeek  Weekday,
                                              HourMin    Begin,
                                              HourMin    End)
        {

            _RegularOpenings[(int) Weekday] = new RegularHours(Weekday, Begin, End);

            return this;

        }

        public OpeningTimes SetRegularOpening(DayOfWeek  FromWeekday,
                                              DayOfWeek  ToWeekday,
                                              HourMin    Begin,
                                              HourMin    End)
        {

            var _FromWeekday = (int) FromWeekday;
            var _ToWeekday   = (int) ToWeekday;

            if (_ToWeekday < _FromWeekday)
                _ToWeekday += 7;

            for (var weekday = _FromWeekday; weekday <= _ToWeekday; weekday++)
                _RegularOpenings[weekday % 7] = new RegularHours((DayOfWeek) (weekday % 7), Begin, End);

            return this;

        }

        public OpeningTimes AddExceptionalOpening(DateTime Start, DateTime End)
        {

            _ExceptionalOpenings.Add(new ExceptionalPeriod(Start, End));

            return this;

        }

        public OpeningTimes AddExceptionalClosing(DateTime Start, DateTime End)
        {

            _ExceptionalClosings.Add(new ExceptionalPeriod(Start, End));

            return this;

        }


        public static OpeningTimes FreeText(String Text)
        {

            return new OpeningTimes(Text);

        }


        public static OpeningTimes Parse(String Text)
        {

            OpeningTimes _OpeningTimes = null;

            if (TryParse(Text, out _OpeningTimes))
                return _OpeningTimes;

            return null;

        }

        public static Boolean TryParse(String Text, out OpeningTimes OpeningTimes)
        {

            OpeningTimes = null;

            var match = Regex.Match(Text, "([a-zA-Z]+) - ([a-zA-Z]+) ([0-9]{2}:[0-9]{2})h - ([0-9]{2}:[0-9]{2})h");
            if (!match.Success)
                return false;

            DayOfWeek FromWeekday;

            if (!Enum.TryParse<DayOfWeek>(match.Groups[1].Value, true, out FromWeekday))
                return false;

            DayOfWeek ToWeekday;

            if (!Enum.TryParse<DayOfWeek>(match.Groups[2].Value, true, out ToWeekday))
                return false;

            HourMin Begin;

            if (!HourMin.TryParse(match.Groups[3].Value, out Begin))
                return false;

            HourMin End;

            if (!HourMin.TryParse(match.Groups[4].Value, out End))
                return false;


            OpeningTimes = new OpeningTimes(FromWeekday, ToWeekday, Begin, End);
            return true;

        }


        #region (static) Open24Hours

        /// <summary>
        /// Is open for 24 hours a day.
        /// </summary>
        public static OpeningTimes Open24Hours
        {
            get
            {
                return new OpeningTimes();
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
        public static Boolean operator == (OpeningTimes OpeningTime1, OpeningTimes OpeningTime2)
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
        public static Boolean operator != (OpeningTimes OpeningTime1, OpeningTimes OpeningTime2)
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
            var OpenTime = Object as OpeningTimes;
            if ((Object) OpenTime == null)
                return false;

            return this.Equals(OpenTime);

        }

        #endregion

        #region Equals(OpeningTimes)

        /// <summary>
        /// Compares two opening times for equality.
        /// </summary>
        /// <param name="OpeningTimes">An opening time to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(OpeningTimes OpeningTimes)
        {

            if ((Object) OpeningTimes == null)
                return false;

            if (IsOpen24Hours && OpeningTimes.IsOpen24Hours)
                return true;

            return Text.Equals(OpeningTimes.Text);

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

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
        {
            return IsOpen24Hours ? "24 hours" : Text;
        }

        #endregion

    }

}
