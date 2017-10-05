/*
 * Copyright (c) 2014-2017 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The common interface of an entity having one or multiple unique
    /// identification(s) and crypto methods.
    /// </summary>
    public interface ICryptoEntity : IEntity
    {

        PgpSecretKeyRing  SecretKeyRing     { get; }

        PgpPublicKeyRing  PublicKeyRing     { get; }

        Signature         Signature         { get; }


        Signature Sign();

    }

    /// <summary>
    /// The common generic interface of an entity having one or multiple unique
    /// identification(s) and crypto methods.
    /// </summary>
    /// <typeparam name="TId">THe type of the unique identificator.</typeparam>
    public interface ICryptoEntity<TId> : IEntity<TId>,
                                          ICryptoEntity

        where TId : IId

    { }

}
