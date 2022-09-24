﻿/*
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

using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An abstract e-mobility entity.
    /// </summary>
    public abstract class AEMobilityEntity<TId> : AInternalData,
                                                  IEntity<TId>,
                                                  IHasId<TId>

        where TId : IId

    {

        #region Data

        /// <summary>
        /// A lookup for user-defined properties.
        /// </summary>
        protected readonly UserDefinedDictionary _UserDefined;

        #endregion

        #region Properties

        /// <summary>
        /// The global unique identification of this entity.
        /// </summary>
        [Mandatory]
        public TId               Id             { get; }

        public ulong Length => 0;

        public bool IsNullOrEmpty => false;

        /// <summary>
        /// A unique status identification of this entity.
        /// </summary>
        [Mandatory]
        public String?           ETag           { get; }

        /// <summary>
        /// The source of this information, e.g. the WWCP importer used.
        /// </summary>
        [Optional]
        public String?           DataSource     { get; set; }

        /// <summary>
        /// The timestamp of the last changes within this ChargingPool.
        /// Can be used as a HTTP ETag.
        /// </summary>
        [Mandatory]
        public DateTime          LastChange     { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event called whenever a property of this entity changed.
        /// </summary>
        public event OnPropertyChangedDelegate? OnPropertyChanged;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract entity.
        /// </summary>
        /// <param name="Id">The unique entity identification.</param>
        /// <param name="CustomData">An optional dictionary of customer-specific data.</param>
        public AEMobilityEntity(TId                                   Id,
                                IReadOnlyDictionary<String, Object>?  CustomData   = null)

            : base(null,
                   CustomData)

        {

            #region Initial checks

            if (Id.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(Id),  "The given Id must not be null or empty!");

            #endregion

            this.Id              = Id;
            this.DataSource      = String.Empty;
            this.LastChange      = Timestamp.Now;
            this._UserDefined    = new UserDefinedDictionary();

            this._UserDefined.OnPropertyChanged += (timestamp, eventtrackingid, sender, key, oldValue, newValue)
                => OnPropertyChanged?.Invoke(timestamp, eventtrackingid, sender, key, oldValue, newValue);

        }

        #endregion


        // Properties

        #region SetProperty<T>(ref FieldToChange, NewValue, EventTrackingId = null, [CallerMemberName])

        /// <summary>
        /// Change the given field and call the OnPropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the field to be changed.</typeparam>
        /// <param name="FieldToChange">A reference to the field to be changed.</param>
        /// <param name="NewValue">The new value of the field to be changed.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="PropertyName">The name of the property to be changed (set by the compiler!)</param>
        public void SetProperty<T>(ref                T                 FieldToChange,
                                                      T                 NewValue,
                                                      EventTracking_Id  EventTrackingId  = null,
                                   [CallerMemberName] String            PropertyName     = "")
        {

            if (!EqualityComparer<T>.Default.Equals(FieldToChange, NewValue))
            {

                var OldValue       = FieldToChange;
                    FieldToChange  = NewValue;

                PropertyChanged(PropertyName,
                                OldValue,
                                NewValue,
                                EventTrackingId ?? EventTracking_Id.New);

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

        #region PropertyChanged<T>(PropertyName, OldValue, NewValue, EventTrackingId)

        /// <summary>
        /// Notify subscribers that a property has changed.
        /// </summary>
        /// <typeparam name="T">The type of the changed property.</typeparam>
        /// <param name="PropertyName">The name of the changed property.</param>
        /// <param name="OldValue">The old value of the changed property.</param>
        /// <param name="NewValue">The new value of the changed property.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        public void PropertyChanged<T>(String            PropertyName,
                                       T                 OldValue,
                                       T                 NewValue,
                                       EventTracking_Id  EventTrackingId = null)
        {

            #region Initial checks

            if (PropertyName is null)
                throw new ArgumentNullException(nameof(PropertyName), "The given property name must not be null!");

            #endregion

            this.LastChange = Timestamp.Now;

            //DebugX.Log(String.Concat("Property '", PropertyName, "' changed from '", OldValue?.ToString() ?? "", "' to '", NewValue?.ToString() ?? "", "'!"));

            OnPropertyChanged?.Invoke(LastChange,
                                      EventTrackingId,
                                      this,
                                      PropertyName,
                                      OldValue,
                                      NewValue);

        }

        #endregion


        // User defined properties

        #region Set(Key, NewValue, OldValue = null)

        public SetPropertyResult Set(String  Key,
                                     Object  NewValue,
                                     Object  OldValue = null)

            => _UserDefined.Set(Key, NewValue, OldValue);

        #endregion

        #region ContainsKey(Key)

        public Boolean ContainsKey(String  Key)

            => _UserDefined.ContainsKey(Key);

        #endregion

        #region Contains(Key, Value)

        public Boolean Contains(String  Key,
                                Object  Value)

            => _UserDefined.Contains(Key, Value);

        #endregion

        #region Contains(KeyValuePair)

        public Boolean Contains(KeyValuePair<String, Object> KeyValuePair)

            => _UserDefined.Contains(KeyValuePair);

        #endregion

        #region Get(Key)

        public Object Get(String  Key)

            => _UserDefined.Get(Key);

        #endregion

        #region TryGet(Key, out Value)

        public Boolean TryGet(String      Key,
                              out Object  Value)

            => _UserDefined.TryGet(Key, out Value);

        #endregion

        #region AddJSON(Key)

        public JObject AddJSON(String Key)
        {
            lock (_UserDefined)
            {

                if (ContainsKey(Key))
                    _UserDefined.Remove(Key);

                var JSON = new JObject();
                _UserDefined.Set(Key, JSON);
                return JSON;

            }
        }

        #endregion

        #region SetJSON(GroupKey, Key, Value)

        public JObject SetJSON(String GroupKey, String Key, Object Value)
        {
            lock (_UserDefined)
            {

                if (_UserDefined.TryGet(GroupKey, out Object JValue))
                {

                    if (JValue is JObject JSON)
                    {
                        JSON.Add(new JProperty(Key, Value));
                        return JSON;
                    }

                    return null;

                }

                var JSON2 = new JObject();
                _UserDefined.Set(GroupKey, JSON2);
                JSON2.Add(new JProperty(Key, Value));
                return JSON2;

            }
        }

        #endregion



        public abstract int CompareTo(object obj);
        //{

        //    if (obj is ChargingSession_Id chargingSessionId)
        //        return CompareTo(chargingSessionId);

        //    return -1;

        //}

        //public int CompareTo(TId obj)
        //{

        //    if (obj is ChargingSession_Id chargingSessionId)
        //        return CompareTo(chargingSessionId);

        //    return -1;

        //}

        //public int CompareTo(ChargingSession_Id other)
        //    => Id.CompareTo(other);

        //public Boolean Equals(TId other)
        //    => Id.Equals(other);

    }

}
