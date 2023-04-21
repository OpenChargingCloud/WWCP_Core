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

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class EntityHashSet<TParentDataStructure, TId, TEntity> : IEnumerable<TEntity>

        where TEntity : IEntity<TId>
        where TId     : IId

    {

        #region Data

        private readonly TParentDataStructure          parentDataStructure;

        private readonly ConcurrentDictionary<TId, TEntity>  lookup;

        #endregion

        #region Events

        private readonly IVotingNotificator<DateTime, TParentDataStructure, TEntity, Boolean> addition;

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, TParentDataStructure, TEntity, Boolean> OnAddition
            => addition;


        private readonly IVotingNotificator<DateTime, TParentDataStructure, TEntity, Boolean> removal;

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, TParentDataStructure, TEntity, Boolean> OnRemoval
            => removal;

        #endregion

        #region Constructor(s)

        public EntityHashSet(TParentDataStructure ParentDataStructure)
        {

            this.parentDataStructure  = ParentDataStructure;
            this.lookup               = new ConcurrentDictionary<TId, TEntity>();

            this.addition             = new VotingNotificator<DateTime, TParentDataStructure, TEntity, Boolean>(() => new VetoVote(), true);
            this.removal              = new VotingNotificator<DateTime, TParentDataStructure, TEntity, Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region TryAdd(Entity, ...)

        public Boolean TryAdd(TEntity Entity)
        {

            if (addition.SendVoting(Timestamp.Now, parentDataStructure, Entity))
            {
                lookup.TryAdd(Entity.Id, Entity);
                addition.SendNotification(Timestamp.Now, parentDataStructure, Entity);
                return true;
            }

            return false;

        }

        public Boolean TryAdd(TEntity          Entity,
                              Action<TEntity>  OnSuccess)
        {

            if (TryAdd(Entity))
            {
                OnSuccess?.Invoke(Entity);
                return true;
            }

            return false;

        }

        public Boolean TryAdd(TEntity                    Entity,
                              Action<DateTime, TEntity>  OnSuccess)
        {

            if (TryAdd(Entity))
            {
                OnSuccess?.Invoke(Timestamp.Now, Entity);
                return true;
            }

            return false;

        }

        public Boolean TryAdd(TEntity                           Entity,
                              Action<DateTime, TParentDataStructure, TEntity>  OnSuccess)
        {

            if (TryAdd(Entity))
            {
                OnSuccess?.Invoke(Timestamp.Now, parentDataStructure, Entity);
                return true;
            }

            return false;

        }

        #endregion

        #region TryAdd(Entities, ...)

        public Boolean TryAdd(IEnumerable<TEntity> Entities)
        {

            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, parentDataStructure, Entity)))
            {

                foreach (var Entity in Entities)
                {
                    lookup.TryAdd(Entity.Id, Entity);
                    addition.SendNotification(Timestamp.Now, parentDataStructure, Entity);
                }

                return true;

            }

            return false;

        }

        public Boolean TryAdd(IEnumerable<TEntity>          Entities,
                              Action<IEnumerable<TEntity>>  OnSuccess)
        {

            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, parentDataStructure, Entity)))
            {

                foreach (var Entity in Entities)
                {
                    lookup.TryAdd(Entity.Id, Entity);
                    addition.SendNotification(Timestamp.Now, parentDataStructure, Entity);
                }

                OnSuccess?.Invoke(Entities);
                return true;

            }

            return false;

        }

        public Boolean TryAdd(IEnumerable<TEntity>                    Entities,
                              Action<DateTime, IEnumerable<TEntity>>  OnSuccess)
        {

            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, parentDataStructure, Entity)))
            {

                foreach (var Entity in Entities)
                {
                    lookup.TryAdd(Entity.Id, Entity);
                    addition.SendNotification(Timestamp.Now, parentDataStructure, Entity);
                }

                OnSuccess?.Invoke(Timestamp.Now, Entities);
                return true;

            }

            return false;

        }

        public Boolean TryAdd(IEnumerable<TEntity>                           Entities,
                              Action<DateTime, TParentDataStructure, IEnumerable<TEntity>>  OnSuccess)
        {

            if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, parentDataStructure, Entity)))
            {

                foreach (var Entity in Entities)
                {
                    lookup.TryAdd(Entity.Id, Entity);
                    addition.SendNotification(Timestamp.Now, parentDataStructure, Entity);
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

        #region Contains(...)

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

        #region GetById(Id)

        public TEntity? GetById(TId Id)
        {

            if (lookup.TryGetValue(Id, out var entity))
                return entity;

            return default;

        }

        #endregion

        #region TryGet(Id, out Entity)

        public Boolean TryGet(TId Id, out TEntity? Entity)
        {

            if (lookup.TryGetValue(Id, out Entity))
                return true;

            Entity = default;
            return false;

        }

        #endregion


        #region Remove   (Id)

        public TEntity? Remove(TId Id)
        {

            if (lookup.TryRemove(Id, out var entity))
                return entity;

            return default;

        }

        #endregion

        #region TryRemove(Id, out Entity)

        public Boolean TryRemove(TId Id, out TEntity? Entity)

            => lookup.TryRemove(Id, out Entity);


        #endregion

        #region TryRemove(Entity)

        public Boolean TryRemove(TEntity Entity)

            => lookup.TryRemove(Entity.Id, out _);

        #endregion

        #region Clear()

        public void Clear()
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
