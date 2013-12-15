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
using System.Linq;
using System.Collections.Generic;

#endregion

namespace de.eMI3
{

    public enum Languages
    {
        undef,
        de,
        en
    }

    /// <summary>
    /// An internationalized string.
    /// </summary>
    public class I8NString : IEnumerable<KeyValuePair<Languages, String>>
    {

        #region Data

        private Dictionary<Languages, String> I8NStrings;

        #endregion

        #region Properties

        public Boolean IsEmpty
        {
            get
            {
                return I8NStrings.Count == 0;
            }
        }

        #endregion

        #region Constructor(s)

        #region (private) I8NString()

        /// <summary>
        /// Generate a new internationalized string.
        /// </summary>
        private I8NString()
        {
            this.I8NStrings = new Dictionary<Languages, String>();
        }

        #endregion

        #region I8NString(Language, Value)

        /// <summary>
        /// Generate a new internationalized string.
        /// </summary>
        public I8NString(Languages Language, String Value)
            : this()
        {
            I8NStrings.Add(Language, Value);
        }

        #endregion

        #region I8NString(params Values)

        /// <summary>
        /// Generate a new internationalized string.
        /// </summary>
        public I8NString(params KeyValuePair<Languages, String>[] Values)
            : this()
        {

            foreach (var Value in Values)
                I8NStrings.Add(Value.Key, Value.Value);

        }

        #endregion

        #endregion


        public static I8NString Create(Languages Language, String Value)
        {
            return new I8NString().Add(Language, Value);
        }


        public I8NString Add(Languages Language, String Value)
        {

            if (!I8NStrings.ContainsKey(Language))
                I8NStrings.Add(Language, Value);

            return this;

        }

        public I8NString Remove(Languages Language)
        {

            if (!I8NStrings.ContainsKey(Language))
                I8NStrings.Remove(Language);

            return this;

        }

        public I8NString Clear()
        {

            I8NStrings.Clear();

            return this;

        }


        public IEnumerator<KeyValuePair<Languages, String>> GetEnumerator()
        {
            return I8NStrings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return I8NStrings.GetEnumerator();
        }



        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return I8NStrings.Select(i8n => i8n.Key.ToString() + ": " + i8n.Value).
                              Aggregate((a, b) => a + "; " + b);
        }

        #endregion

    }

}
