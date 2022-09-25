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

using System.Runtime.CompilerServices;

using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// An abstract e-mobility entity.
    /// </summary>
    public abstract class AEMobilityEntity<TId,
                                           TAdminStatus,
                                           TStatus> : AInternalData,
                                                      IEntity<TId>,
                                                      IHasId<TId>,
                                                      IAdminStatus<TAdminStatus>,
                                                      IStatus<TStatus>

        where TId          : IId
        where TAdminStatus : IComparable
        where TStatus      : IComparable

    {

        #region Data

        /// <summary>
        /// The default max size of the admin status schedule/history.
        /// </summary>
        public const  UInt16  DefaultMaxAdminStatusScheduleSize   = 50;

        /// <summary>
        /// The default max size of the status schedule/history.
        /// </summary>
        public const  UInt16  DefaultMaxStatusScheduleSize        = 50;


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
        public TId                     Id                       { get; }

        /// <summary>
        /// The multi-language name of this entity.
        /// </summary>
        [Mandatory]
        public I18NString              Name                     { get; protected set; }

        /// <summary>
        /// The multi-language description of this entity.
        /// </summary>
        [Mandatory]
        public I18NString              Description              { get; protected set; }



        #region AdminStatus

        /// <summary>
        /// The current charging station admin status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<TAdminStatus> AdminStatus
        {

            get
            {
                return adminStatusSchedule.CurrentStatus;
            }

            set
            {
                if (!adminStatusSchedule.CurrentValue.Equals(value.Value))
                    SetAdminStatus(value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        protected readonly StatusSchedule<TAdminStatus> adminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        /// <param name="TimestampFilter">An optional admin status timestamp filter.</param>
        /// <param name="StatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries to skip.</param>
        /// <param name="Take">The number of admin status entries to return.</param>
        public IEnumerable<Timestamped<TAdminStatus>> AdminStatusSchedule(Func<DateTime,     Boolean>?  TimestampFilter   = null,
                                                                          Func<TAdminStatus, Boolean>?  StatusFilter      = null,
                                                                          UInt64?                       Skip              = null,
                                                                          UInt64?                       Take              = null)
        {

            TimestampFilter ??= timestamp => true;
            StatusFilter    ??= status    => true;

            var filteredStatusSchedule = adminStatusSchedule.
                                             Where(status => TimestampFilter(status.Timestamp)).
                                             Where(status => StatusFilter   (status.Value)).
                                             Skip (Skip ?? 0);

            return Take.HasValue
                       ? filteredStatusSchedule.Take(Take)
                       : filteredStatusSchedule;

        }

        #endregion


        #region Status

        /// <summary>
        /// The current charging station status.
        /// </summary>
        [InternalUseOnly]
        public Timestamped<TStatus> Status
        {

            get
            {
                return statusSchedule.CurrentStatus;
            }

            set
            {
                if (!statusSchedule.CurrentValue.Equals(value.Value))
                    SetStatus(value);
            }

        }

        #endregion

        #region StatusSchedule

        protected readonly StatusSchedule<TStatus> statusSchedule;

        /// <summary>
        /// The charging station status schedule.
        /// </summary>
        /// <param name="TimestampFilter">An optional status timestamp filter.</param>
        /// <param name="StatusFilter">An optional status value filter.</param>
        /// <param name="Skip">The number of status entries to skip.</param>
        /// <param name="Take">The number of status entries to return.</param>
        public IEnumerable<Timestamped<TStatus>> StatusSchedule(Func<DateTime, Boolean>?  TimestampFilter   = null,
                                                                Func<TStatus,  Boolean>?  StatusFilter      = null,
                                                                UInt64?                   Skip              = null,
                                                                UInt64?                   Take              = null)
        {

             TimestampFilter ??= timestamp => true;
             StatusFilter    ??= status    => true;

             var filteredStatusSchedule = statusSchedule.
                                              Where(status => TimestampFilter(status.Timestamp)).
                                              Where(status => StatusFilter   (status.Value)).
                                              Skip (Skip ?? 0);

             return Take.HasValue
                        ? filteredStatusSchedule.Take(Take)
                        : filteredStatusSchedule;

        }

        #endregion




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
        /// <param name="InternalData">An optional dictionary of customer-specific data.</param>
        public AEMobilityEntity(TId                         Id,
                                I18NString?                 Name                         = null,
                                I18NString?                 Description                  = null,

                                Timestamped<TAdminStatus>?  InitialAdminStatus           = null,
                                Timestamped<TStatus>?       InitialStatus                = null,
                                UInt16                      MaxAdminStatusScheduleSize   = DefaultMaxAdminStatusScheduleSize,
                                UInt16                      MaxStatusScheduleSize        = DefaultMaxStatusScheduleSize,

                                String?                     DataSource                   = null,
                                DateTime?                   LastChange                   = null,

                                JObject?                    CustomData                   = null,
                                UserDefinedDictionary?      InternalData                 = null)

            : base(CustomData,
                   InternalData)

        {

            #region Initial checks

            if (Id.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(Id),  "The given Id must not be null or empty!");

            #endregion

            this.Id                    = Id;
            this.Name                  = Name        ?? I18NString.Empty;
            this.Description           = Description ?? I18NString.Empty;

            this.adminStatusSchedule   = new StatusSchedule<TAdminStatus>(MaxAdminStatusScheduleSize);
            this.statusSchedule        = new StatusSchedule<TStatus>     (MaxStatusScheduleSize);

            if (InitialAdminStatus.HasValue)
                this.adminStatusSchedule.Insert(InitialAdminStatus.Value);

            if (InitialStatus.     HasValue)
                this.statusSchedule.     Insert(InitialStatus.Value);


            this.DataSource      = DataSource;
            this.LastChange      = LastChange ?? Timestamp.Now;

            //this._UserDefined    = new UserDefinedDictionary();

            //this._UserDefined.OnPropertyChanged += (timestamp, eventtrackingid, sender, key, oldValue, newValue)
            //    => OnPropertyChanged?.Invoke(timestamp, eventtrackingid, sender, key, oldValue, newValue);

        }

        #endregion


        #region (Admin-)Status management

        #region SetAdminStatus(NewAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(TAdminStatus  NewAdminStatus)
        {
            adminStatusSchedule.Insert(NewAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        public void SetAdminStatus(Timestamped<TAdminStatus>  NewTimestampedAdminStatus)
        {
            adminStatusSchedule.Insert(NewTimestampedAdminStatus);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatus, Timestamp)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewAdminStatus">A new admin status.</param>
        public void SetAdminStatus(TAdminStatus  NewAdminStatus,
                                   DateTime      Timestamp)
        {
            adminStatusSchedule.Insert(NewAdminStatus, Timestamp);
        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<TAdminStatus>>  NewAdminStatusList,
                                   ChangeMethods                           ChangeMethod = ChangeMethods.Replace)
        {
            adminStatusSchedule.Set(NewAdminStatusList, ChangeMethod);
        }

        #endregion


        #region SetStatus(NewStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        public void SetStatus(TStatus  NewStatus)
        {
            statusSchedule.Insert(NewStatus);
        }

        #endregion

        #region SetStatus(NewTimestampedStatus)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        public void SetStatus(Timestamped<TStatus>  NewTimestampedStatus)
        {
            statusSchedule.Insert(NewTimestampedStatus);
        }

        #endregion

        #region SetStatus(NewStatus, Timestamp)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        public void SetStatus(TStatus   NewStatus,
                              DateTime  Timestamp)
        {
            statusSchedule.Insert(NewStatus, Timestamp);
        }

        #endregion

        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace)

        /// <summary>
        /// Set the timestamped status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        public void SetStatus(IEnumerable<Timestamped<TStatus>>  NewStatusList,
                              ChangeMethods                      ChangeMethod = ChangeMethods.Replace)
        {
            statusSchedule.Set(NewStatusList, ChangeMethod);
        }

        #endregion

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

        //#region ContainsKey(Key)

        //public Boolean ContainsKey(String  Key)

        //    => _UserDefined.ContainsKey(Key);

        //#endregion

        //#region Contains(Key, Value)

        //public Boolean Contains(String  Key,
        //                        Object  Value)

        //    => _UserDefined.Contains(Key, Value);

        //#endregion

        //#region Contains(KeyValuePair)

        //public Boolean Contains(KeyValuePair<String, Object> KeyValuePair)

        //    => _UserDefined.Contains(KeyValuePair);

        //#endregion

        //#region Get(Key)

        //public Object Get(String  Key)

        //    => _UserDefined.Get(Key);

        //#endregion

        //#region TryGet(Key, out Value)

        //public Boolean TryGet(String      Key,
        //                      out Object  Value)

        //    => _UserDefined.TryGet(Key, out Value);

        //#endregion

        //#region AddJSON(Key)

        //public JObject AddJSON(String Key)
        //{
        //    lock (_UserDefined)
        //    {

        //        if (ContainsKey(Key))
        //            _UserDefined.Remove(Key);

        //        var JSON = new JObject();
        //        _UserDefined.Set(Key, JSON);
        //        return JSON;

        //    }
        //}

        //#endregion

        //#region SetJSON(GroupKey, Key, Value)

        //public JObject SetJSON(String GroupKey, String Key, Object Value)
        //{
        //    lock (_UserDefined)
        //    {

        //        if (_UserDefined.TryGet(GroupKey, out Object JValue))
        //        {

        //            if (JValue is JObject JSON)
        //            {
        //                JSON.Add(new JProperty(Key, Value));
        //                return JSON;
        //            }

        //            return null;

        //        }

        //        var JSON2 = new JObject();
        //        _UserDefined.Set(GroupKey, JSON2);
        //        JSON2.Add(new JProperty(Key, Value));
        //        return JSON2;

        //    }
        //}

        //#endregion

        public abstract Int32 CompareTo(Object? obj);


    }

}
