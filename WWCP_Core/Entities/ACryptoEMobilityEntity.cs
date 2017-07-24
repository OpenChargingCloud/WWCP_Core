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

using System.Collections.Generic;

using Org.BouncyCastle.Bcpg.OpenPgp;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// An abstract e-mobility entity having crypto capabilities.
    /// </summary>
    public abstract class ACryptoEMobilityEntity<TId> : AEMobilityEntity<TId>,
                                                        ICryptoEntity<TId>

        where TId : IId

    {

        #region Properties

        public PgpPublicKeyRing  PublicKeyRing    { get; }

        public PgpSecretKeyRing  SecretKeyRing    { get; }

        public Signature         Signature        { get; protected set; }

        #endregion

        #region Constructor(s)

        #region ABaseEMobilityEntity(Id,  ...)

        /// <summary>
        /// Create a new abstract crypto entity.
        /// </summary>
        /// <param name="Id">The unique entity identification.</param>
        protected ACryptoEMobilityEntity(TId               Id,
                                         PgpPublicKeyRing  PublicKeyRing  = null,
                                         PgpSecretKeyRing  SecretKeyRing  = null)

            : base(Id)

        {


        }

        #endregion

        #region ABaseEMobilityEntity(Ids, ...)

        /// <summary>
        /// Create a new abstract crypto entity.
        /// </summary>
        /// <param name="Ids">The unique entity identifications.</param>
        protected ACryptoEMobilityEntity(IEnumerable<TId>  Ids,
                                         PgpPublicKeyRing  PublicKeyRing  = null,
                                         PgpSecretKeyRing  SecretKeyRing  = null)

            : base(Ids)

        {

        }

        #endregion

        #endregion


        #region Sign()

        public Signature Sign()

            => new Signature("");

        #endregion


    }

}
