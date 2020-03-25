/*
 * Copyright (c) 2014-2020 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The common interface of an entity having one or multiple unique identification(s).
    /// </summary>
    public interface IEntity
    {

        /// <summary>
        /// The timestamp of the last change of this entity.
        /// </summary>
        DateTime                         LastChange  { get; }


        /// <summary>
        /// A delegate called whenever any (internal) state of this entity changed.
        /// </summary>
        event OnPropertyChangedDelegate  OnPropertyChanged;

    }

    /// <summary>
    /// The common generic interface of an entity having one or multiple unique identification(s).
    /// </summary>
    /// <typeparam name="TId">THe type of the unique identificator.</typeparam>
    public interface IEntity<TId> : IEntity, IId<TId>

        where TId : IId

    {

        ///// <summary>
        ///// The primary unique identification of this entity.
        ///// </summary>
        //TId              Id     { get; }

        /// <summary>
        /// Auxilary unique identifications of this entity.
        /// (Think of CNAMES in DNS, or brand names for companies)
        /// </summary>
        IEnumerable<TId> Ids    { get; }

    }

}
