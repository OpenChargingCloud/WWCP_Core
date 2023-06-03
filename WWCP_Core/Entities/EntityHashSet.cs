/*
 * Copyright (c) 2014-2023 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Collections;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;
using social.OpenData.UsersAPI;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class EntityHashSet<TParentDataStructure, TId, TEntity> : IEnumerable<TEntity>

        where TEntity : IEntity<TId>
        where TId     : IId

    {

        #region Data

        private readonly TParentDataStructure                parentDataStructure;

        private readonly ConcurrentDictionary<TId, TEntity>  lookup;

        #endregion

        #region Events

        private readonly IVotingNotificator<DateTime, EventTracking_Id, User_Id, TParentDataStructure, TEntity, Boolean> addition;

        /// <summary>
        /// Called whenever an entity will be or was added.
        /// </summary>
        public IVotingSender<DateTime, EventTracking_Id, User_Id, TParentDataStructure, TEntity, Boolean> OnAddition
            => addition;


        private readonly IVotingNotificator<DateTime, TParentDataStructure, TEntity, TEntity, Boolean> update;

        /// <summary>
        /// Called whenever an entity will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, TParentDataStructure, TEntity, TEntity, Boolean> OnUpdate
            => update;


        private readonly IVotingNotificator<DateTime, TParentDataStructure, TEntity, Boolean> removal;

        /// <summary>
        /// Called whenever an entity will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, TParentDataStructure, TEntity, Boolean> OnRemoval
            => removal;

        #endregion

        #region Constructor(s)

        public EntityHashSet(TParentDataStructure                                                            ParentDataStructure,

                             IVotingNotificator<DateTime, EventTracking_Id, User_Id, TParentDataStructure, TEntity,          Boolean>?  Addition   = null,
                             IVotingNotificator<DateTime, TParentDataStructure, TEntity, TEntity, Boolean>?  Update     = null,
                             IVotingNotificator<DateTime, TParentDataStructure, TEntity,          Boolean>?  Removal    = null)
        {

            this.lookup               = new ConcurrentDictionary<TId, TEntity>();

            this.parentDataStructure  = ParentDataStructure;

            this.addition             = Addition ?? new VotingNotificator<DateTime, EventTracking_Id, User_Id, TParentDataStructure, TEntity,          Boolean>(() => new VetoVote(), true);
            this.update               = Update   ?? new VotingNotificator<DateTime, TParentDataStructure, TEntity, TEntity, Boolean>(() => new VetoVote(), true);
            this.removal              = Removal  ?? new VotingNotificator<DateTime, TParentDataStructure, TEntity,          Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region TryAdd(Entity, ...)

        public Boolean TryAdd(TEntity           Entity,
                              EventTracking_Id  EventTrackingId,
                              User_Id?          CurrentUserId)
        {

            if (addition.SendVoting(Timestamp.Now,
                                    EventTrackingId,
                                    CurrentUserId ?? User_Id.Anonymous,
                                    parentDataStructure,
                                    Entity) &&

                lookup.TryAdd(Entity.Id, Entity))

            {

                addition.SendNotification(Timestamp.Now,
                                          EventTrackingId,
                                          CurrentUserId ?? User_Id.Anonymous,
                                          parentDataStructure,
                                          Entity);

                return true;

            }

            return false;

        }


        /// <summary>
        /// Try to add the given entity to the hashset.
        /// </summary>
        /// <param name="Entity">An entity.</param>
        /// <param name="OnSuccess">A delegate called after adding the entity, but before the notifications are send.</param>
        /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        /// <param name="CurrentUserId">An optional user identification initiating this command/request.</param>
        public Boolean TryAdd(TEntity           Entity,
                              Action<TEntity>   OnSuccess,
                              EventTracking_Id  EventTrackingId,
                              User_Id?          CurrentUserId)
        {

            if (addition.SendVoting(Timestamp.Now,
                                    EventTrackingId,
                                    CurrentUserId ?? User_Id.Anonymous,
                                    parentDataStructure,
                                    Entity) &&

                lookup.TryAdd(Entity.Id, Entity))

            {

                OnSuccess?.Invoke(Entity);

                addition.SendNotification(Timestamp.Now,
                                          EventTrackingId,
                                          CurrentUserId ?? User_Id.Anonymous,
                                          parentDataStructure,
                                          Entity);

                return true;

            }

            return false;

        }

        public Boolean TryAdd(TEntity                    Entity,
                              Action<DateTime, TEntity>  OnSuccess,
                              EventTracking_Id           EventTrackingId,
                              User_Id?                   CurrentUserId)
        {

            if (addition.SendVoting(Timestamp.Now,
                                    EventTrackingId,
                                    CurrentUserId ?? User_Id.Anonymous,
                                    parentDataStructure,
                                    Entity) &&

                lookup.TryAdd(Entity.Id, Entity))

            {

                OnSuccess?.Invoke(Timestamp.Now, Entity);

                addition.SendNotification(Timestamp.Now,
                                          EventTrackingId,
                                          CurrentUserId ?? User_Id.Anonymous,
                                          parentDataStructure,
                                          Entity);

                return true;

            }

            return false;

        }

        public Boolean TryAdd(TEntity                                                                     Entity,
                              Action<DateTime, EventTracking_Id, User_Id, TParentDataStructure, TEntity>  OnSuccess,
                              EventTracking_Id                                                            EventTrackingId,
                              User_Id?                                                                    CurrentUserId)
        {

            var userId = CurrentUserId ?? User_Id.Anonymous;

            if (addition.SendVoting(Timestamp.Now,
                                    EventTrackingId,
                                    userId,
                                    parentDataStructure,
                                    Entity) &&

                lookup.TryAdd(Entity.Id, Entity))

            {

                OnSuccess?.Invoke(Timestamp.Now,
                                  EventTrackingId,
                                  userId,
                                  parentDataStructure,
                                  Entity);

                addition.SendNotification(Timestamp.Now,
                                          EventTrackingId,
                                          CurrentUserId ?? User_Id.Anonymous,
                                          parentDataStructure,
                                          Entity);

                return true;

            }

            return false;

        }

        #endregion

        #region TryAdd(Entities, ...)

        public Boolean TryAdd(IEnumerable<TEntity>  Entities,
                              EventTracking_Id      EventTrackingId,
                              User_Id?              CurrentUserId)
        {

            var currentUserId = CurrentUserId ?? User_Id.Anonymous;

            // Only when all are allowed we will go on!
            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now,
                                                           EventTrackingId,
                                                           currentUserId,
                                                           parentDataStructure,
                                                           Entity)))
            {

                foreach (var entity in Entities)
                {

                    lookup.TryAdd(entity.Id, entity);

                    addition.SendNotification(Timestamp.Now,
                                              EventTrackingId,
                                              currentUserId,
                                              parentDataStructure,
                                              entity);

                }

                return true;

            }

            return false;

        }

        public Boolean TryAdd(IEnumerable<TEntity>          Entities,
                              Action<IEnumerable<TEntity>>  OnSuccess,
                              EventTracking_Id              EventTrackingId,
                              User_Id?                      CurrentUserId)
        {

            var currentUserId = CurrentUserId ?? User_Id.Anonymous;

            // Only when all are allowed we will go on!
            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now,
                                                           EventTrackingId,
                                                           currentUserId,
                                                           parentDataStructure,
                                                           Entity)))
            {

                foreach (var entity in Entities)
                {

                    lookup.TryAdd(entity.Id, entity);

                    addition.SendNotification(Timestamp.Now,
                                              EventTrackingId,
                                              currentUserId,
                                              parentDataStructure,
                                              entity);

                }

                OnSuccess?.Invoke(Entities);
                return true;

            }

            return false;

        }

        public Boolean TryAdd(IEnumerable<TEntity>                    Entities,
                              Action<DateTime, IEnumerable<TEntity>>  OnSuccess,
                              EventTracking_Id                        EventTrackingId,
                              User_Id?                                CurrentUserId)
        {

            var currentUserId = CurrentUserId ?? User_Id.Anonymous;

            // Only when all are allowed we will go on!
            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now,
                                                           EventTrackingId,
                                                           currentUserId,
                                                           parentDataStructure,
                                                           Entity)))
            {

                foreach (var entity in Entities)
                {

                    lookup.TryAdd(entity.Id, entity);

                    addition.SendNotification(Timestamp.Now,
                                              EventTrackingId,
                                              currentUserId,
                                              parentDataStructure,
                                              entity);

                }

                OnSuccess?.Invoke(Timestamp.Now, Entities);
                return true;

            }

            return false;

        }

        public Boolean TryAdd(IEnumerable<TEntity>                                          Entities,
                              Action<DateTime, TParentDataStructure, IEnumerable<TEntity>>  OnSuccess,
                              EventTracking_Id                                              EventTrackingId,
                              User_Id?                                                      CurrentUserId)
        {

            var currentUserId = CurrentUserId ?? User_Id.Anonymous;

            // Only when all are allowed we will go on!
            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now,
                                                           EventTrackingId,
                                                           currentUserId,
                                                           parentDataStructure,
                                                           Entity)))
            {

                foreach (var entity in Entities)
                {

                    lookup.TryAdd(entity.Id, entity);

                    addition.SendNotification(Timestamp.Now,
                                              EventTrackingId,
                                              currentUserId,
                                              parentDataStructure,
                                              entity);

                }

                OnSuccess?.Invoke(Timestamp.Now, parentDataStructure, Entities);
                return true;

            }

            return false;

        }

        #endregion


        #region ContainsId(...)

        public Boolean ContainsId(TId Id)

            => lookup.ContainsKey(Id);

        #endregion

        #region Contains  (...)

        public Boolean Contains(TEntity Entity)
        {

            foreach (var entity in lookup)
            {
                if (entity.Equals(Entity))
                    return true;
            }

            return false;

        }

        #endregion

        #region GetById   (Id)

        public TEntity? GetById(TId Id)
        {

            if (lookup.TryGetValue(Id, out var entity))
                return entity;

            return default;

        }

        #endregion

        #region TryGet    (Id, out Entity)

        public Boolean TryGet(TId Id, out TEntity? Entity)
        {

            if (lookup.TryGetValue(Id, out Entity))
                return true;

            Entity = default;
            return false;

        }

        #endregion

        #region TryUpdate (Id, NewEntity, OldEntity)

        public Boolean TryUpdate(TId               Id,
                                 TEntity           NewEntity,
                                 TEntity           OldEntity,
                                 EventTracking_Id  EventTrackingId,
                                 User_Id?          CurrentUserId)

            => lookup.TryUpdate(Id, NewEntity, OldEntity);

        #endregion


        #region Remove   (Id)

        public TEntity? Remove(TId               Id,
                               EventTracking_Id  EventTrackingId,
                               User_Id?          CurrentUserId)
        {

            if (lookup.TryRemove(Id, out var entity))
                return entity;

            return default;

        }

        #endregion

        #region TryRemove(Id, out Entity)

        public Boolean TryRemove(TId               Id,
                                 out TEntity?      Entity,
                                 EventTracking_Id  EventTrackingId,
                                 User_Id?          CurrentUserId)

            => lookup.TryRemove(Id, out Entity);


        #endregion

        #region TryRemove(Entity)

        public Boolean TryRemove(TEntity           Entity,
                                 EventTracking_Id  EventTrackingId,
                                 User_Id?          CurrentUserId)

            => lookup.TryRemove(Entity.Id, out _);

        #endregion

        #region Clear()

        public void Clear(EventTracking_Id  EventTrackingId,
                          User_Id?          CurrentUserId)
        {
            lookup.Clear();
        }

        #endregion


        #region IEnumerable<T> Members

        public IEnumerator<TEntity> GetEnumerator()

            => lookup.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()

            => lookup.Values.GetEnumerator();

        #endregion


    }

}
