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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

namespace org.GraphDefined.WWCP
{

    public delegate void PropertyChanged_EventHandler(DateTime Timestamp, Object Sender, String PropertyName, Object OldValue, Object NewValue);

    /// <summary>
    /// An abstract e-mobility entity.
    /// </summary>
    public abstract class AEMobilityEntity<TId> : IEntity<TId>
        where TId : IId
    {

        #region Properties

        #region Id

        protected readonly TId _Id;

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

        #region DataSource

        private String _DataSource;

        /// <summary>
        /// The source of this information, e.g. the WWCP importer used.
        /// </summary>
        [Optional]
        public String DataSource
        {

            get
            {
                return _DataSource;
            }

            set
            {
                _DataSource = value;
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

        #region Unstructured

        private ConcurrentDictionary<String, Object> _UserDefined;

        /// <summary>
        /// A lookup for user-defined properties.
        /// </summary>
        public ConcurrentDictionary<String, Object> UserDefined
        {
            get
            {
                return _UserDefined;
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
        public AEMobilityEntity(TId Id)
        {

            #region Initial checks

            if (Id == null)
                throw new ArgumentNullException("Id", "The given Id must not be null!");

            #endregion

            this._Id           = Id;
            this._DataSource   = String.Empty;
            this._LastChange   = DateTime.Now;
            this._UserDefined  = new ConcurrentDictionary<String, Object>();

        }

        #endregion


        #region SetProperty<T>(ref FieldToChange, NewValue, [CallerMemberName])

        /// <summary>
        /// Change the given field and call the OnPropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the field to be changed.</typeparam>
        /// <param name="FieldToChange">A reference to the field to be changed.</param>
        /// <param name="NewValue">The new value of the field to be changed.</param>
        /// <param name="PropertyName">The name of the property to be changed (set by the compiler!)</param>
        public void SetProperty<T>(ref                T       FieldToChange,
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

        #region DeleteProperty<T>(ref FieldToChange, [CallerMemberName])

        /// <summary>
        /// Delete the given field and call the OnPropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the field to be deleted.</typeparam>
        /// <param name="FieldToChange">A reference to the field to be deleted.</param>
        /// <param name="PropertyName">The name of the property to be deleted (set by the compiler!)</param>
        public void DeleteProperty<T>(ref                T       FieldToChange,
                                      [CallerMemberName] String  PropertyName = "")
        {

            if (FieldToChange != null)
            {

                var OldValue       = FieldToChange;
                    FieldToChange  = default(T);

                PropertyChanged(PropertyName, OldValue, default(T));

            }

        }

        #endregion

        #region PropertyChanged<T>(PropertyName, OldValue, NewValue)

        /// <summary>
        /// Notify subscribers that a property has changed.
        /// </summary>
        /// <typeparam name="T">The type of the changed property.</typeparam>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        public void PropertyChanged<T>(String  PropertyName,
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


        #region this[PropertyName]

        /// <summary>
        /// Return the user-defined property for the given property name.
        /// </summary>
        /// <param name="PropertyName">The name of the user-defined property.</param>
        public Object this[String PropertyName]
        {

            get
            {
                return _UserDefined[PropertyName];
            }

            set
            {
                _UserDefined[PropertyName] = value;
            }

        }

        #endregion

        #region RemoveUserDefinedProperty()

        /// <summary>
        /// Try to remove a user-defined property.
        /// </summary>
        /// <param name="PropertyName"></param>
        public void RemoveUserDefinedProperty(String PropertyName)
        {

            Object Value;

            _UserDefined.TryRemove(PropertyName, out Value);

        }

        #endregion


    }

}
