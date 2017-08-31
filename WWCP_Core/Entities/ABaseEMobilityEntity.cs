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

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An abstract base e-mobility entity with crypto capabilities
    /// directly attached to a roaming network.
    /// </summary>
    public abstract class ABaseEMobilityEntity<TId> : ACryptoEMobilityEntity<TId>

        where TId : IId

    {

        #region Properties

        /// <summary>
        /// The parent roaming network.
        /// </summary>
        public RoamingNetwork  RoamingNetwork   { get; }

        #endregion

        #region Constructor(s)

        #region ABaseEMobilityEntity(Id,  RoamingNetwork, ...)

        /// <summary>
        /// Create a new abstract crypto entity.
        /// </summary>
        /// <param name="Id">The unique entity identification.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        protected ABaseEMobilityEntity(TId               Id,
                                       RoamingNetwork    RoamingNetwork,
                                       PgpPublicKeyRing  PublicKeyRing  = null,
                                       PgpSecretKeyRing  SecretKeyRing  = null)

            : base(Id,
                   PublicKeyRing,
                   SecretKeyRing)

        {

            this.RoamingNetwork = RoamingNetwork ?? throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

        }

        #endregion

        #region ABaseEMobilityEntity(Ids, RoamingNetwork, ...)

        /// <summary>
        /// Create a new abstract crypto entity.
        /// </summary>
        /// <param name="Ids">The unique entity identifications.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="PublicKeyRing">The public key ring of the entity.</param>
        /// <param name="SecretKeyRing">The secrect key ring of the entity.</param>
        protected ABaseEMobilityEntity(IEnumerable<TId>  Ids,
                                       RoamingNetwork    RoamingNetwork,
                                       PgpPublicKeyRing  PublicKeyRing  = null,
                                       PgpSecretKeyRing  SecretKeyRing  = null)

            : base(Ids,
                   PublicKeyRing,
                   SecretKeyRing)

        {

            this.RoamingNetwork = RoamingNetwork ?? throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

        }

        #endregion

        #endregion

    }

}
