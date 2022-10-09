/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using org.GraphDefined.Vanaheimr.Illias;
using System.Collections;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Results of the UserDefinedDictionary SET method.
    /// </summary>
    public enum SetPropertyResult
    {

        /// <summary>
        /// A new property was added.
        /// </summary>
        Added    = 0,

        /// <summary>
        /// An existing property value was updated.
        /// </summary>
        Changed  = 1,

        /// <summary>
        /// The property could not be updated.
        /// </summary>
        Conflict = 2,

        /// <summary>
        /// The property was removed.
        /// </summary>
        Removed  = 3

    }

    public class UserDefinedDictionary : IEnumerable<KeyValuePair<String, Object>>
    {

        #region Data

        private Dictionary<String, Object> _Dictionary;

        #endregion

        #region Events

        /// <summary>
        /// An event called whenever a property of this entity changed.
        /// </summary>
        public event OnPropertyChangedDelegate OnPropertyChanged;

        #endregion

        #region Constructor(s)

        public UserDefinedDictionary()
        {
            _Dictionary = new Dictionary<String, Object>();
        }

        #endregion


        #region Set(Key, NewValue, OldValue = null, EventTrackingId = null)

        public SetPropertyResult Set(String             Key,
                                     Object             NewValue,
                                     Object?            OldValue          = null,
                                     EventTracking_Id?  EventTrackingId   = null)
        {

            // Locks are shit, but ConcurrentDictionary does not compare values correctly!
            lock (_Dictionary)
            {

                Object _CurrentValue;

                if (!_Dictionary.TryGetValue(Key, out _CurrentValue))
                {

                    _Dictionary.Add(Key, NewValue);

                    OnPropertyChanged?.Invoke(Timestamp.Now,
                                              EventTrackingId,
                                              this,
                                              Key,
                                              OldValue,
                                              NewValue);

                    return SetPropertyResult.Added;

                }

                if (_CurrentValue.ToString() != OldValue.ToString())
                    return SetPropertyResult.Conflict;

                if (NewValue != null)
                {

                    _Dictionary[Key] = NewValue;

                    OnPropertyChanged?.Invoke(Timestamp.Now,
                                              EventTrackingId,
                                              this,
                                              Key,
                                              OldValue,
                                              NewValue);

                    return SetPropertyResult.Changed;

                }


                _Dictionary.Remove(Key);

                OnPropertyChanged?.Invoke(Timestamp.Now,
                                          EventTrackingId,
                                          this,
                                          Key,
                                          OldValue,
                                          null);

                return SetPropertyResult.Removed;

            }

        }

        #endregion

        #region ContainsKey(Key)

        public Boolean ContainsKey(String Key)

            => _Dictionary.ContainsKey(Key);

        #endregion

        #region Contains(Key, Value)

        public Boolean Contains(String  Key,
                                Object  Value)
        {

            Object CurrentValue;

            if (!_Dictionary.TryGetValue(Key, out CurrentValue))
                return false;

            return CurrentValue.ToString() == Value.ToString();

        }

        #endregion

        #region Contains(KeyValuePair)

        public Boolean Contains(KeyValuePair<String, Object>  KeyValuePair)

            => Contains(KeyValuePair.Key, KeyValuePair.Value);

        #endregion

        #region Get(Key)

        public Object Get(String Key)
        {

            lock (_Dictionary)
            {

                Object _CurrentValue;

                if (_Dictionary.TryGetValue(Key, out _CurrentValue))
                    return _CurrentValue;

                return null;

            }

        }

        #endregion

        #region TryGet(Key, out Value)

        public Boolean TryGet(String Key, out Object Value)

            => _Dictionary.TryGetValue(Key, out Value);

        #endregion

        #region Remove(Key)

        public Object Remove(String Key)
        {

            lock (_Dictionary)
            {

                if (_Dictionary.TryGetValue(Key, out Object currentValue))
                {
                    _Dictionary.Remove(Key);
                    return currentValue;
                }

                return null;

            }

        }

        #endregion


        #region GetEnumerator()

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
            => _Dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _Dictionary.GetEnumerator();

        #endregion

    }

}
