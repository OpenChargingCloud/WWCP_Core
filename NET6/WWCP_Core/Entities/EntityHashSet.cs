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

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Illias.Votes;
using org.GraphDefined.Vanaheimr.Styx.Arrows;

#endregion

namespace org.GraphDefined.WWCP
{

    public class EntityHashSet<THost, TId, T> : IEnumerable<T>

        where T   : IEntity<TId>, IHasIds<TId>
        where TId : IId

    {

        #region Data

        private readonly THost               _Host;

        private readonly Dictionary<TId, T>  _Lookup;

        private readonly Dictionary<TId, T>  _MultiIdLookup;

        private readonly Object              Lock = new Object();

        #endregion

        #region Events

        private readonly IVotingNotificator<DateTime, THost, T, Boolean> _Addition;

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, THost, T, Boolean> OnAddition
            => _Addition;


        private readonly IVotingNotificator<DateTime, THost, T, Boolean> _Removal;

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, THost, T, Boolean> OnRemoval
            => _Removal;

        #endregion

        #region Constructor(s)

        public EntityHashSet(THost Host)
        {

            this._Host           = Host;
            this._Lookup         = new Dictionary<TId, T>();
            this._MultiIdLookup  = new Dictionary<TId, T>();

            this._Addition       = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);
            this._Removal        = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region Ids

        public IEnumerable<TId> Ids
            => _MultiIdLookup.Select(kvp => kvp.Key);

        #endregion

        #region ContainsId(...)

        public Boolean ContainsId(TId Id)
            => _MultiIdLookup.ContainsKey(Id);

        #endregion

        #region Contains(...)

        public Boolean Contains(T Entity)
            => _Lookup.ContainsValue(Entity);

        #endregion

        #region TryAdd(Entity, ...)

        public Boolean TryAdd(T Entity)
        {

            lock (Lock)
            {

                if (_Addition.SendVoting(DateTime.UtcNow, _Host, Entity))
                {

                    _Lookup.Add(Entity.Id, Entity);

                    foreach (var Id in Entity.Ids)
                        _MultiIdLookup.Add(Id, Entity);

                    _Addition.SendNotification(DateTime.UtcNow, _Host, Entity);

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(T          Entity,
                              Action<T>  OnSuccess)
        {

            lock (Lock)
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

            lock (Lock)
            {

                if (TryAdd(Entity))
                {

                    OnSuccess?.Invoke(DateTime.UtcNow, Entity);

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(T                           Entity,
                              Action<DateTime, THost, T>  OnSuccess)
        {

            lock (Lock)
            {

                if (TryAdd(Entity))
                {

                    OnSuccess?.Invoke(DateTime.UtcNow, _Host, Entity);

                    return true;

                }

                return false;

            }

        }

        #endregion

        #region Get(Id)

        public T GetById(TId Id)
        {

            lock (Lock)
            {

                if (_MultiIdLookup.TryGetValue(Id, out T _Entity))
                    return _Entity;

                return default;

            }

        }

        #endregion

        #region TryGet(Id, out Entity)

        public Boolean TryGet(TId Id, out T Entity)
        {

            lock (Lock)
            {

                if (_MultiIdLookup.TryGetValue(Id, out Entity))
                    return true;

                Entity = default;

                return false;

            }

        }

        #endregion

        #region TryRemove(Id, out Entity)

        public Boolean TryRemove(TId Id, out T Entity)
        {

            if (_Lookup.TryGetValue(Id, out Entity))
            {

                _Lookup.Remove(Id);

                foreach (var _Id in Entity.Ids)
                    _MultiIdLookup.Remove(_Id);

                return true;

            }

            return false;

        }

        #endregion

        #region Remove(...)

        public T Remove(TId Id)
        {

            if (_Lookup.TryGetValue(Id, out T _Entity))
            {

                _Lookup.Remove(Id);

                foreach (var _Id in _Entity.Ids)
                    _MultiIdLookup.Remove(_Id);

                return _Entity;

            }

            return default;

        }

        public void Remove(T Entity)
        {

            if (_Lookup.TryGetValue(Entity.Id, out Entity))
            {

                _Lookup.Remove(Entity.Id);

                foreach (var _Id in Entity.Ids)
                    _MultiIdLookup.Remove(_Id);

            }

        }

        #endregion

        #region Clear()

        public void Clear()
        {
            _Lookup.Clear();
        }

        #endregion


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()

            => _Lookup.Select(_ => _.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()

            => _Lookup.Select(_ => _.Value).GetEnumerator();

        #endregion

    }

    public class SpecialHashSet<THost, TId, T> : IEnumerable<T>

        where T   : IEntity<TId>
        where TId : IId

    {

        #region Data

        private readonly THost               _Host;

        private readonly Dictionary<TId, T>  _Lookup;

        private readonly Object              Lock = new Object();

        #endregion

        #region Events

        private readonly IVotingNotificator<DateTime, THost, T, Boolean> _Addition;

        /// <summary>
        /// Called whenever a parking operator will be or was added.
        /// </summary>
        public IVotingSender<DateTime, THost, T, Boolean> OnAddition
            => _Addition;


        private readonly IVotingNotificator<DateTime, THost, T, Boolean> _Removal;

        /// <summary>
        /// Called whenever a parking operator will be or was removed.
        /// </summary>
        public IVotingSender<DateTime, THost, T, Boolean> OnRemoval
            => _Removal;

        #endregion

        #region Constructor(s)

        public SpecialHashSet(THost Host)
        {

            this._Host           = Host;
            this._Lookup         = new Dictionary<TId, T>();

            this._Addition       = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);
            this._Removal        = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region ContainsId(...)

        public Boolean ContainsId(TId Id)
            => _Lookup.ContainsKey(Id);

        #endregion

        #region Contains(...)

        public Boolean Contains(T Entity)
            => _Lookup.ContainsValue(Entity);

        #endregion

        #region TryAdd(Entity, ...)

        public Boolean TryAdd(T Entity)
        {

            lock (Lock)
            {

                if (_Addition.SendVoting(DateTime.UtcNow, _Host, Entity))
                {

                    _Lookup.Add(Entity.Id, Entity);

                    _Addition.SendNotification(DateTime.UtcNow, _Host, Entity);

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(T          Entity,
                              Action<T>  OnSuccess)
        {

            lock (Lock)
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

            lock (Lock)
            {

                if (TryAdd(Entity))
                {

                    OnSuccess?.Invoke(DateTime.UtcNow, Entity);

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(T                           Entity,
                              Action<DateTime, THost, T>  OnSuccess)
        {

            lock (Lock)
            {

                if (TryAdd(Entity))
                {

                    OnSuccess?.Invoke(DateTime.UtcNow, _Host, Entity);

                    return true;

                }

                return false;

            }

        }

        #endregion

        #region TryAdd(Entities, ...)

        public Boolean TryAdd(IEnumerable<T> Entities)
        {

            lock (Lock)
            {

                if (Entities.All(Entity => _Addition.SendVoting(DateTime.UtcNow, _Host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        _Lookup.Add(Entity.Id, Entity);
                        _Addition.SendNotification(DateTime.UtcNow, _Host, Entity);
                    }

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(IEnumerable<T>          Entities,
                              Action<IEnumerable<T>>  OnSuccess)
        {

            lock (Lock)
            {

                if (Entities.All(Entity => _Addition.SendVoting(DateTime.UtcNow, _Host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        _Lookup.Add(Entity.Id, Entity);
                        _Addition.SendNotification(DateTime.UtcNow, _Host, Entity);
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

            lock (Lock)
            {

                if (Entities.All(Entity => _Addition.SendVoting(DateTime.UtcNow, _Host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        _Lookup.Add(Entity.Id, Entity);
                        _Addition.SendNotification(DateTime.UtcNow, _Host, Entity);
                    }

                    OnSuccess?.Invoke(DateTime.UtcNow, Entities);
                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(IEnumerable<T>                           Entities,
                              Action<DateTime, THost, IEnumerable<T>>  OnSuccess)
        {

            lock (Lock)
            {

                if (Entities.All(Entity => _Addition.SendVoting(DateTime.UtcNow, _Host, Entity)))
                {

                    foreach (var Entity in Entities)
                    {
                        _Lookup.Add(Entity.Id, Entity);
                        _Addition.SendNotification(DateTime.UtcNow, _Host, Entity);
                    }

                    OnSuccess?.Invoke(DateTime.UtcNow, _Host, Entities);
                    return true;

                }

                return false;

            }

        }

        #endregion

        #region Get(Id)

        public T GetById(TId Id)
        {

            lock (Lock)
            {

                if (_Lookup.TryGetValue(Id, out T _Entity))
                    return _Entity;

                return default;

            }

        }

        #endregion

        #region TryGet(Id, out Entity)

        public Boolean TryGet(TId Id, out T Entity)
        {

            lock (Lock)
            {

                if (_Lookup.TryGetValue(Id, out Entity))
                    return true;

                Entity = default;

                return false;

            }

        }

        #endregion

        #region TryRemove(Id, out Entity)

        public Boolean TryRemove(TId Id, out T Entity)
        {

            if (_Lookup.TryGetValue(Id, out Entity))
            {
                _Lookup.Remove(Id);
                return true;
            }

            return false;

        }

        #endregion

        #region Remove(...)

        public T Remove(TId Id)
        {

            if (_Lookup.TryGetValue(Id, out T _Entity))
            {
                _Lookup.Remove(Id);
                return _Entity;
            }

            return default;

        }

        public void Remove(T Entity)
        {
            if (_Lookup.TryGetValue(Entity.Id, out Entity))
                _Lookup.Remove(Entity.Id);
        }

        #endregion

        #region Clear()

        public void Clear()
        {
            _Lookup.Clear();
        }

        #endregion


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()

            => _Lookup.Select(_ => _.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()

            => _Lookup.Select(_ => _.Value).GetEnumerator();

        #endregion

    }

}
