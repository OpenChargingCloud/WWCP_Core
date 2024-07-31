/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod;

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
        public const  UInt16  DefaultMaxAdminStatusScheduleSize    = 50;

        /// <summary>
        /// The default max size of the status schedule/history.
        /// </summary>
        public const  UInt16  DefaultMaxStatusScheduleSize         = 50;

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
        [Optional]
        public I18NString              Name                     { get; }

        /// <summary>
        /// The multi-language description of this entity.
        /// </summary>
        [Optional]
        public I18NString              Description              { get; }


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
                    adminStatusSchedule.Insert(value);
            }

        }

        #endregion

        #region AdminStatusSchedule

        protected readonly StatusSchedule<TAdminStatus> adminStatusSchedule;

        /// <summary>
        /// The charging station admin status schedule.
        /// </summary>
        /// <param name="TimestampFilter">An optional admin status timestamp filter.</param>
        /// <param name="AdminStatusFilter">An optional admin status value filter.</param>
        /// <param name="Skip">The number of admin status entries to skip.</param>
        /// <param name="Take">The number of admin status entries to return.</param>
        public IEnumerable<Timestamped<TAdminStatus>> AdminStatusSchedule(Func<DateTime,     Boolean>?  TimestampFilter     = null,
                                                                          Func<TAdminStatus, Boolean>?  AdminStatusFilter   = null,
                                                                          UInt64?                       Skip                = null,
                                                                          UInt64?                       Take                = null)
        {

            TimestampFilter   ??= timestamp => true;
            AdminStatusFilter ??= status    => true;

            var filteredAdminStatusSchedule = adminStatusSchedule.
                                                  Where(adminStatus => TimestampFilter  (adminStatus.Timestamp)).
                                                  Where(adminStatus => AdminStatusFilter(adminStatus.Value)).
                                                  Skip (Skip ?? 0);

            return Take.HasValue
                       ? filteredAdminStatusSchedule.Take(Take)
                       : filteredAdminStatusSchedule;

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
                    statusSchedule.Insert(value);
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


        #region ETag

        private String? eTag;

        /// <summary>
        /// A unique status identification of this entity.
        /// </summary>
        [Mandatory]
        public String? ETag
        {

            get
            {
                return eTag;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref eTag,
                                value);

                else
                    DeleteProperty(ref eTag);

            }

        }

        #endregion

        #region DataSource

        private String? dataSource;

        /// <summary>
        /// The source of this information, e.g. the WWCP importer used.
        /// </summary>
        [Optional]
        public String? DataSource
        {

            get
            {
                return dataSource;
            }

            set
            {

                if (value is not null)
                    SetProperty(ref dataSource,
                                value);

                else
                    DeleteProperty(ref dataSource);

            }

        }

        #endregion

        public UInt64   Length
            => 0;

        public Boolean  IsNullOrEmpty
            => false;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract entity.
        /// </summary>
        /// <param name="Id">The unique entity identification.</param>
        /// 
        /// <param name="CustomData">Optional customer specific data, e.g. in combination with custom parsers and serializers.</param>
        /// <param name="InternalData">Optional internal data.</param>
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
                   InternalData,
                   LastChange ?? Timestamp.Now)

        {

            #region Initial checks

            if (Id.IsNullOrEmpty)
                throw new ArgumentNullException(nameof(Id),  "The given identification must not be null or empty!");

            #endregion

            this.Id          = Id;
            this.eTag        = null;
            this.DataSource  = DataSource;


            #region Name

            this.Name                  = Name        ?? I18NString.Empty;

            this.Name.OnPropertyChanged += (timestamp,
                                            eventTrackingId,
                                            sender,
                                            propertyName,
                                            newValue,
                                            oldValue,
                                            dataSource) =>
            {

                PropertyChanged("name",
                                newValue,
                                oldValue,
                                dataSource,
                                eventTrackingId);

                return Task.CompletedTask;

            };

            #endregion

            #region Description

            this.Description           = Description ?? I18NString.Empty;

            this.Description.OnPropertyChanged += (timestamp,
                                                   eventTrackingId,
                                                   sender,
                                                   propertyName,
                                                   newValue,
                                                   oldValue,
                                                   dataSource) =>
            {

                PropertyChanged("description",
                                newValue,
                                oldValue,
                                dataSource,
                                eventTrackingId);

                return Task.CompletedTask;

            };

            #endregion


            #region AdminStatusSchedule

            this.adminStatusSchedule   = new StatusSchedule<TAdminStatus>(MaxAdminStatusScheduleSize);

            if (InitialAdminStatus.HasValue)
                this.adminStatusSchedule.Insert(InitialAdminStatus.Value);

            #endregion

            #region StatusSchedule

            this.statusSchedule = new StatusSchedule<TStatus>(MaxStatusScheduleSize);

            if (InitialStatus.     HasValue)
                this.statusSchedule.     Insert(InitialStatus.Value);

            #endregion


        }

        #endregion


        #region (Admin-)Status management

        #region SetAdminStatus(NewAdminStatus, DataSource = null)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewAdminStatus">A new timestamped admin status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetAdminStatus(TAdminStatus  NewAdminStatus,
                                   Context?      DataSource   = null)
        {

            adminStatusSchedule.Insert(NewAdminStatus,
                                       DataSource);

        }

        #endregion

        #region SetAdminStatus(NewTimestampedAdminStatus, DataSource = null)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="NewTimestampedAdminStatus">A new timestamped admin status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetAdminStatus(Timestamped<TAdminStatus>  NewTimestampedAdminStatus,
                                   Context?                   DataSource   = null)
        {

            adminStatusSchedule.Insert(NewTimestampedAdminStatus,
                                       DataSource);

        }

        #endregion

        #region SetAdminStatus(NewAdminStatus, Timestamp, DataSource = null)

        /// <summary>
        /// Set the admin status.
        /// </summary>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="NewAdminStatus">A new admin status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetAdminStatus(TAdminStatus  NewAdminStatus,
                                   DateTime      Timestamp,
                                   Context?      DataSource   = null)
        {

            adminStatusSchedule.Insert(NewAdminStatus,
                                       Timestamp,
                                       DataSource);

        }

        #endregion

        #region SetAdminStatus(NewAdminStatusList, ChangeMethod = ChangeMethods.Replace, DataSource = null)

        /// <summary>
        /// Set the timestamped admin status.
        /// </summary>
        /// <param name="NewAdminStatusList">A list of new timestamped admin status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetAdminStatus(IEnumerable<Timestamped<TAdminStatus>>  NewAdminStatusList,
                                   ChangeMethods                           ChangeMethod   = ChangeMethods.Replace,
                                   Context?                                DataSource     = null)
        {

            adminStatusSchedule.Set(NewAdminStatusList,
                                    ChangeMethod,
                                    DataSource);

        }

        #endregion


        #region SetStatus(NewStatus, DataSource = null)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetStatus(TStatus   NewStatus,
                              Context?  DataSource   = null)
        {

            statusSchedule.Insert(NewStatus,
                                  DataSource);

        }

        #endregion

        #region SetStatus(NewTimestampedStatus, DataSource = null)

        /// <summary>
        /// Set the current status.
        /// </summary>
        /// <param name="NewTimestampedStatus">A new timestamped status.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetStatus(Timestamped<TStatus>  NewTimestampedStatus,
                              Context?              DataSource   = null)
        {

            statusSchedule.Insert(NewTimestampedStatus,
                                  DataSource);

        }

        #endregion

        #region SetStatus(NewStatus, Timestamp, DataSource = null)

        /// <summary>
        /// Set the status.
        /// </summary>
        /// <param name="NewStatus">A new status.</param>
        /// <param name="Timestamp">The timestamp when this change was detected.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetStatus(TStatus   NewStatus,
                              DateTime  Timestamp,
                              Context?  DataSource   = null)
        {

            statusSchedule.Insert(NewStatus,
                                  Timestamp,
                                  DataSource);

        }

        #endregion

        #region SetStatus(NewStatusList, ChangeMethod = ChangeMethods.Replace, DataSource = null)

        /// <summary>
        /// Set the timestamped status.
        /// </summary>
        /// <param name="NewStatusList">A list of new timestamped status.</param>
        /// <param name="ChangeMethod">The change mode.</param>
        /// <param name="DataSource">An optional data source or context for the status update.</param>
        public void SetStatus(IEnumerable<Timestamped<TStatus>>  NewStatusList,
                              ChangeMethods                      ChangeMethod   = ChangeMethods.Replace,
                              Context?                           DataSource     = null)
        {

            statusSchedule.Set(NewStatusList,
                               ChangeMethod,
                               DataSource);

        }

        #endregion

        #endregion


        #region (protected) LogEvent(Logger, LogHandler, ...)

        protected async Task LogEvent<TDelegate>(String                                             WWCPIO,
                                                 TDelegate?                                         Logger,
                                                 Func<TDelegate, Task>                              LogHandler,
                                                 [CallerArgumentExpression(nameof(Logger))] String  EventName   = "",
                                                 [CallerMemberName()]                       String  Command     = "")

            where TDelegate : Delegate

        {
            if (Logger is not null)
            {
                try
                {

                    await Task.WhenAll(
                              Logger.GetInvocationList().
                                     OfType<TDelegate>().
                                     Select(LogHandler)
                          );

                }
                catch (Exception e)
                {
                    await HandleErrors(WWCPIO, $"{Command}.{EventName}", e);
                }
            }
        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ErrorResponse)

        public virtual Task HandleErrors(String  Module,
                                         String  Caller,
                                         String  ErrorResponse)
        {

            return Task.CompletedTask;

        }

        #endregion

        #region (virtual) HandleErrors(Module, Caller, ExceptionOccured)

        public virtual Task HandleErrors(String     Module,
                                         String     Caller,
                                         Exception  ExceptionOccured)
        {

            return Task.CompletedTask;

        }

        #endregion


        public abstract Int32 CompareTo(Object? obj);


    }

}
