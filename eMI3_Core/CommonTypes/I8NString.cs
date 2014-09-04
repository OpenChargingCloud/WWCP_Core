/*
 * Copyright (c) 2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/GraphDefined/eMI3>
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

namespace com.graphdefined.eMI3
{

    public enum Languages
    {
        undef,
        de,
        en,
        fr
    }

    public struct I8NPair
    {

        private readonly Languages _Key;
        public Languages Language { get { return _Key;   } }

        private readonly String    _Value;
        public String    Value    { get { return _Value; } }

        public I8NPair(Languages key, String value)
        {
            _Key   = key;
            _Value = value;
        }

    }

    /// <summary>
    /// An internationalized string.
    /// </summary>
    public class I8NString : IEnumerable<I8NPair>
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

            else
                I8NStrings[Language] = Value;

            return this;

        }

        public String this[Languages Language]
        {

            get
            {
                return I8NStrings[Language];
            }

            set
            {
                I8NStrings[Language] = value;
            }

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


        public IEnumerator<I8NPair> GetEnumerator()
        {
            return I8NStrings.Select(kvp => new I8NPair(kvp.Key, kvp.Value)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return I8NStrings.Select(kvp => new I8NPair(kvp.Key, kvp.Value)).GetEnumerator();
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
