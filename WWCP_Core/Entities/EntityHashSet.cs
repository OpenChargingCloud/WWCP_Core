/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Styx.Arrows;
using org.GraphDefined.Vanaheimr.Illias.Votes;

#endregion

namespace org.GraphDefined.WWCP
{

    public class EntityHashSet<THost, TId, T> : IEnumerable<T>

        where T   : IEntity<TId>
        where TId : IId

    {

        #region Data

        private readonly THost               _Host;

        private readonly Dictionary<TId, T>  _Dictionary;

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

        #region Consturctor(s)

        public EntityHashSet(THost Host)
        {

            this._Host        = Host;
            this._Dictionary  = new Dictionary<TId, T>();

            this._Addition    = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);
            this._Removal     = new VotingNotificator<DateTime, THost, T, Boolean>(() => new VetoVote(), true);

        }

        #endregion


        #region Ids

        public IEnumerable<TId> Ids
            => _Dictionary.Select(kvp => kvp.Key);

        #endregion

        #region Contains(...)


        public Boolean Contains(TId Id)
            => _Dictionary.ContainsKey(Id);


        public Boolean Contains(T Entity)
            => _Dictionary.ContainsValue(Entity);

        #endregion

        #region TryAdd(Entity, ...)

        public Boolean TryAdd(T Entity)
        {

            lock (_Dictionary)
            {

                if (_Addition.SendVoting(DateTime.Now, _Host, Entity))
                {

                    _Dictionary.Add(Entity.Id, Entity);

                    _Addition.SendNotification(DateTime.Now, _Host, Entity);

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(T          Entity,
                              Action<T>  OnSuccess)
        {

            lock (_Dictionary)
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

            lock (_Dictionary)
            {

                if (TryAdd(Entity))
                {

                    OnSuccess?.Invoke(DateTime.Now, Entity);

                    return true;

                }

                return false;

            }

        }

        public Boolean TryAdd(T                           Entity,
                              Action<DateTime, THost, T>  OnSuccess)
        {

            lock (_Dictionary)
            {

                if (TryAdd(Entity))
                {

                    OnSuccess?.Invoke(DateTime.Now, _Host, Entity);

                    return true;

                }

                return false;

            }

        }

        #endregion

        #region Get(Id)

        public T Get(TId Id)
        {

            lock (_Dictionary)
            {

                T _Entity;

                if (_Dictionary.TryGetValue(Id, out _Entity))
                    return _Entity;

                return default(T);

            }

        }

        #endregion

        #region TryGet(Id, out Entity)

        public Boolean TryGet(TId Id, out T Entity)
        {

            lock (_Dictionary)
            {

                if (_Dictionary.TryGetValue(Id, out Entity))
                    return true;

                Entity = default(T);

                return false;

            }

        }

        #endregion

        #region TryRemove(Id, out Entity)

        public Boolean TryRemove(TId Id, out T Entity)
            => _Dictionary.TryGetValue(Id, out Entity) && _Dictionary.Remove(Id);

        #endregion

        #region Remove(...)

        public Boolean Remove(TId Id)
            => _Dictionary.Remove(Id);

        public Boolean Remove(T Entity)
            => _Dictionary.Remove(Entity.Id);

        #endregion


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()

            => _Dictionary.Select(_ => _.Value).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()

            => _Dictionary.Select(_ => _.Value).GetEnumerator();

        #endregion

    }

}
