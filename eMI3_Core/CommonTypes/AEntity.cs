/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
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
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace org.emi3group
{

    public delegate void PropertyChanged_EventHandler   (Object Sender, String PropertyName, Object OldValue, Object NewValue);
    public delegate void PropertyChanged_EventHandler<T>(Object Sender, String PropertyName, T      OldValue, T      NewValue);

    /// <summary>
    /// An abstract ev entity.
    /// </summary>
    public abstract class AEntity<TId> : IEntity<TId>
        where TId : IId
    {

        #region Data

        private readonly ConcurrentDictionary<String, PropertyChanged_EventHandler>  Events;

        #endregion

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

        /// <summary>
        /// The timestamp of the last changes within this EVSPool.
        /// Can be used as a HTTP ETag.
        /// </summary>
        [Mandatory]
        public DateTime LastChange { get; private set; }

        #endregion

        #endregion

        #region Events

        public event PropertyChanged_EventHandler           OnPropertyChanged;
        public event PropertyChanged_EventHandler<String>   OnStringPropertyChanged;

        #endregion

        #region Constructor(s)

        #region AEntity(Id)

        /// <summary>
        /// Create a new abstract entity.
        /// </summary>
        /// <param name="Id">The entity Id.</param>
        public AEntity(TId Id)
        {

            if (Id == null)
                throw new ArgumentNullException("Id", "The given Id must not be null!");

            this._Id            = Id;
            this.LastChange     = DateTime.Now;

        }

        #endregion

        #endregion



        //public PropertyChanged_EventHandler<T> OnChanged<T>(Func<EVSPool, T>                 PropertySelector,
        //                                                    PropertyChanged_EventHandler<T>  EventAction,
        //                                                    [CallerMemberName] String Name = null)
        //{

        //    return null;

        //}


        protected void SetProperty<T>(ref T Field, T NewValue, [CallerMemberName] String PropertyName = "")
        {

            if (!EqualityComparer<T>.Default.Equals(Field, NewValue))
            {

                var OldValue  = Field;
                    Field     = NewValue;

                this.LastChange = DateTime.Now;

                var OnPropertyChangedHandler = OnPropertyChanged;
                if (OnPropertyChangedHandler != null)
                    OnPropertyChangedHandler(this, PropertyName, OldValue, NewValue);

            }

        }


    }

}
