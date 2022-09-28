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

using System.Collections;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    public class EntityHashSet<THost, TId, T> : IEnumerable<T>

        where T   : IEntity<TId>
        where TId : IId

    {

        #region Data

        private readonly THost               host;

        private readonly Dictionary<TId, T>  lookup;

        private readonly Object              lockObject = new ();

        #endregion

        #region Events

        private readonly IVotingNotificator<DateTime, THost, T, Boolean> addition;

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, THost, T, Boolean> OnAddition
            => addition;


        private readonly IVotingNotificator<DateTime, THost, T, Boolean> removal;

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, THost, T, Boolean> OnRemoval
            => removal;

        #endregion

        #region Constructor(s)

        public EntityHashSet(THost Host)
        {

            this.host           = Host;
            this.lookup         = new Dictionary<TId, T>();

            this.addition       = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);
            this.removal        = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region ContainsId(...)

        public Boolean ContainsId(TId Id)
            => lookup.ContainsKey(Id);

        #endregion

        #region Contains(...)

        public Boolean Contains(T Entity)
            => lookup.ContainsValue(Entity);

        #endregion

        #region TryAdd(Entity, ...)

        public Boolean TryAdd(T Entity)
        {
            lock (lockObject)
            {

                if (addition.SendVoting(Timestamp.Now, host, Entity))
                {
                    lookup.Add(Entity.Id, Entity);
                    addition.SendNotification(Timestamp.Now, host, Entity);
                    return true;
                }

                return false;

            }
        }

        public Boolean TryAdd(T          Entity,
                              Action<T>  OnSuccess)
        {
            lock (lockObject)
            {

                if (TryAdd(Entity))
                {
                    OnSuccess?.Invoke(Entity);
                    return true;
                }

                return false;

            }
        }

        public Boolean TryAdd(T                    Entity,
                              Action<DateTime, T>  OnSuccess)
        {
            lock (lockObject)
            {

                if (TryAdd(Entity))
                {
                    OnSuccess?.Invoke(Timestamp.Now, Entity);
                    return true;
                }

                return false;

            }
        }

        public Boolean TryAdd(T                           Entity,
                              Action<DateTime, THost, T>  OnSuccess)
        {
            lock (lockObject)
            {

                if (TryAdd(Entity))
                {
                    OnSuccess?.Invoke(Timestamp.Now, host, Entity);
                    return true;
                }

                return false;

            }
        }

        #endregion

        #region TryAdd(Entities, ...)

        public Boolean TryAdd(IEnumerable<T> Entities)
        {
            lock (lockObject)
            {

                if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        lookup.Add(Entity.Id, Entity);
                        addition.SendNotification(Timestamp.Now, host, Entity);
                    }

                    return true;

                }

                return false;

            }
        }

        public Boolean TryAdd(IEnumerable<T>          Entities,
                              Action<IEnumerable<T>>  OnSuccess)
        {
            lock (lockObject)
            {

                if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        lookup.Add(Entity.Id, Entity);
                        addition.SendNotification(Timestamp.Now, host, Entity);
                    }

                    OnSuccess?.Invoke(Entities);
                    return true;

                }

                return false;

            }
        }

        public Boolean TryAdd(IEnumerable<T>                    Entities,
                              Action<DateTime, IEnumerable<T>>  OnSuccess)
        {
            lock (lockObject)
            {

                if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        lookup.Add(Entity.Id, Entity);
                        addition.SendNotification(Timestamp.Now, host, Entity);
                    }

                    OnSuccess?.Invoke(Timestamp.Now, Entities);
                    return true;

                }

                return false;

            }
        }

        public Boolean TryAdd(IEnumerable<T>                           Entities,
                              Action<DateTime, THost, IEnumerable<T>>  OnSuccess)
        {
            lock (lockObject)
            {

                if (Entities.All(Entity => addition.SendVoting(Timestamp.Now, host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        lookup.Add(Entity.Id, Entity);
                        addition.SendNotification(Timestamp.Now, host, Entity);
                    }

                    OnSuccess?.Invoke(Timestamp.Now, host, Entities);
                    return true;

                }

                return false;

            }
        }

        #endregion

        #region GetById(Id)

        public T? GetById(TId Id)
        {
            lock (lockObject)
            {

                if (lookup.TryGetValue(Id, out T? entity))
                    return entity;

                return default;

            }
        }

        #endregion

        #region TryGet(Id, out Entity)

        public Boolean TryGet(TId Id, out T? Entity)
        {
            lock (lockObject)
            {

                if (lookup.TryGetValue(Id, out Entity))
                    return true;

                Entity = default;
                return false;

            }
        }

        #endregion

        #region TryRemove(Id, out Entity)

        public Boolean TryRemove(TId Id, out T? Entity)
        {

            if (lookup.TryGetValue(Id, out Entity))
            {
                lookup.Remove(Id);
                return true;
            }

            return false;

        }

        #endregion

        #region Remove(...)

        public T? Remove(TId Id)
        {

            if (lookup.TryGetValue(Id, out T? entity))
            {
                lookup.Remove(Id);
                return entity;
            }

            return default;

        }

        public Boolean Remove(T Entity)

            => lookup.Remove(Entity.Id);

        #endregion

        #region Clear()

        public void Clear()
        {
            lookup.Clear();
        }

        #endregion


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
            => lookup.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => lookup.Values.GetEnumerator();

        #endregion

    }

}
