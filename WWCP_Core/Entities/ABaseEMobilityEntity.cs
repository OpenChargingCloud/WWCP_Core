///*
// * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
// *
// * Licensed under the Affero GPL license, Version 3.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.gnu.org/licenses/agpl.html
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//#region Usings

//using System;
//using System.Collections.Generic;

//using Org.BouncyCastle.Crypto.Parameters;

//using org.GraphDefined.Vanaheimr.Illias;

//#endregion

//namespace cloud.charging.open.protocols.WWCP
//{

//    /// <summary>
//    /// An abstract base e-mobility entity with crypto capabilities
//    /// directly attached to a roaming network.
//    /// </summary>
//    public abstract class ABaseEMobilityEntity<TId> : ACryptoEMobilityEntity<TId>

//        where TId : IId

//    {

//        #region Properties

        

//        #endregion

//        #region Constructor(s)

//        #region ABaseEMobilityEntity(Id, Name, RoamingNetwork, ...)

//        /// <summary>
//        /// Create a new abstract crypto entity.
//        /// </summary>
//        /// <param name="Id">The unique entity identification.</param>
//        /// <param name="Name">The name of the entity.</param>
//        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
//        protected ABaseEMobilityEntity(TId                     Id,
//                                       I18NString              Name,
//                                       IRoamingNetwork         RoamingNetwork,
//                                       String                  EllipticCurve          = "P-256",
//                                       ECPrivateKeyParameters  PrivateKey             = null,
//                                       PublicKeyCertificates   PublicKeyCertificates  = null)

//            : base(Id,
//                   RoamingNetwork,
//                   EllipticCurve,
//                   PrivateKey,
//                   PublicKeyCertificates)

//        {

//            this.Name = Name;

//        }

//        #endregion

//        #region ABaseEMobilityEntity(Ids, RoamingNetwork, ...)

//        /// <summary>
//        /// Create a new abstract crypto entity.
//        /// </summary>
//        /// <param name="Ids">The unique entity identifications.</param>
//        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
//        protected ABaseEMobilityEntity(IEnumerable<TId>        Ids,
//                                       IRoamingNetwork         RoamingNetwork,
//                                       String                  EllipticCurve          = "P-256",
//                                       ECPrivateKeyParameters  PrivateKey             = null,
//                                       PublicKeyCertificates   PublicKeyCertificates  = null)

//            : base(Ids,
//                   RoamingNetwork,
//                   EllipticCurve,
//                   PrivateKey,
//                   PublicKeyCertificates)

//        {

//            this.Name = Name;

//        }

//        #endregion

//        #endregion

//    }

//}
