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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

namespace org.GraphDefined.WWCP
{

    public delegate void PropertyChanged_EventHandler(DateTime Timestamp, Object Sender, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// An abstract entity.
    /// </summary>
    public abstract class AEntity<TId> : IEntity<TId>
        where TId : IId
    {

        #region Properties

        #region Id

        private readonly TId _Id;

        /// <summary>
        /// The global unique identification of this entity.
        /// </summary>
        [Mandatory]
        public TId Id
        {
            get
            {
                return _Id;
            }
        }

        #endregion

        #region LastChange

        private DateTime _LastChange;

        /// <summary>
        /// The timestamp of the last changes within this ChargingPool.
        /// Can be used as a HTTP ETag.
        /// </summary>
        [Mandatory]
        public DateTime LastChange
        {
            get
            {
                return _LastChange;
            }
        }

        #endregion

        #endregion

        #region Events

        public event PropertyChanged_EventHandler OnPropertyChanged;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract entity.
        /// </summary>
        /// <param name="Id">The unique entity identification.</param>
        public AEntity(TId Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The given Id must not be null!");

            #endregion

            this._Id          = Id;
            this._LastChange  = DateTime.Now;

        }

        #endregion


        #region (protected) SetProperty<T>(ref FieldToChange, NewValue, [CallerMemberName])

        /// <summary>
        /// Change the given field and call the OnPropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the field to be changed.</typeparam>
        /// <param name="FieldToChange">A reference to the field to be changed.</param>
        /// <param name="NewValue">The new value of the field to be changed.</param>
        /// <param name="PropertyName">The name of the property to be changed (set by the compiler!)</param>
        protected void SetProperty<T>(ref                T       FieldToChange,
                                                         T       NewValue,
                                      [CallerMemberName] String  PropertyName = "")
        {

            if (!EqualityComparer<T>.Default.Equals(FieldToChange, NewValue))
            {

                var OldValue       = FieldToChange;
                    FieldToChange  = NewValue;

                PropertyChanged(PropertyName, OldValue, NewValue);

            }

        }

        #endregion

        #region (protected) PropertyChanged<T>(PropertyName, OldValue, NewValue)

        /// <summary>
        /// Notify subscribers that a property has changed.
        /// </summary>
        /// <typeparam name="T">The type of the changed property.</typeparam>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        protected void PropertyChanged<T>(String  PropertyName,
                                          T       OldValue,
                                          T       NewValue)
        {

            #region Initial checks

            if (PropertyName == null)
                throw new ArgumentNullException("PropertyName", "The given parameter must not be null!");

            #endregion

            this._LastChange = DateTime.Now;

            var OnPropertyChangedLocal = OnPropertyChanged;
            if (OnPropertyChangedLocal != null)
                OnPropertyChangedLocal(_LastChange, this, PropertyName, OldValue, NewValue);

        }

        #endregion

    }

}
