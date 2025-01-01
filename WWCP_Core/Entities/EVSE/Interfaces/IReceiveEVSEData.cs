/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// The common interface of all WWCP point-of-interest data management.
    /// </summary>
    public interface IReceiveEVSEData
    {

        #region AddEVSE           (EVSE,  ...)

        /// <summary>
        /// Add the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddEVSEResult>

            AddEVSE(IEVSE              EVSE,

                    DateTime?          Timestamp           = null,
                    EventTracking_Id?  EventTrackingId     = null,
                    TimeSpan?          RequestTimeout      = null,
                    CancellationToken  CancellationToken   = default);

        #endregion

        #region AddEVSEIfNotExists(EVSE,  ...)

        /// <summary>
        /// Add the given EVSE, if it does not already exist.
        /// </summary>
        /// <param name="EVSE">An EVSE to add, if it does not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddEVSEResult>

            AddEVSEIfNotExists(IEVSE              EVSE,

                               DateTime?          Timestamp           = null,
                               EventTracking_Id?  EventTrackingId     = null,
                               TimeSpan?          RequestTimeout      = null,
                               CancellationToken  CancellationToken   = default);

        #endregion

        #region AddOrUpdateEVSE   (EVSE,  ...)

        /// <summary>
        /// Add or update the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateEVSEResult>

            AddOrUpdateEVSE(IEVSE              EVSE,

                            DateTime?          Timestamp           = null,
                            EventTracking_Id?  EventTrackingId     = null,
                            TimeSpan?          RequestTimeout      = null,
                            CancellationToken  CancellationToken   = default);

        #endregion

        #region UpdateEVSE        (EVSE,  PropertyName, NewValue, OldValue = null, DataSource = null, ...)

        /// <summary>
        /// Update the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="OldValue">The optional old value of the EVSE property to update.</param>
        /// <param name="DataSource">An optional data source or context for the data change.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateEVSEResult>

            UpdateEVSE(IEVSE              EVSE,
                       String             PropertyName,
                       Object?            NewValue,
                       Object?            OldValue            = null,
                       Context?           DataSource          = null,

                       DateTime?          Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       CancellationToken  CancellationToken   = default);

        #endregion

        #region DeleteEVSE        (EVSE,  ...)

        /// <summary>
        /// Delete the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteEVSEResult>

            DeleteEVSE(IEVSE              EVSE,

                       DateTime?          Timestamp           = null,
                       EventTracking_Id?  EventTrackingId     = null,
                       TimeSpan?          RequestTimeout      = null,
                       CancellationToken  CancellationToken   = default);

        #endregion


        #region AddEVSEs          (EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddEVSEsResult>

            AddEVSEs(IEnumerable<IEVSE>  EVSEs,

                     DateTime?           Timestamp           = null,
                     EventTracking_Id?   EventTrackingId     = null,
                     TimeSpan?           RequestTimeout      = null,
                     CancellationToken   CancellationToken   = default);

        #endregion

        #region AddEVSEsIfNotExist(EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs, if they do not already exist.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add, if they do not already exist.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<AddEVSEsResult>

            AddEVSEsIfNotExist(IEnumerable<IEVSE>  EVSEs,

                               DateTime?           Timestamp           = null,
                               EventTracking_Id?   EventTrackingId     = null,
                               TimeSpan?           RequestTimeout      = null,
                               CancellationToken   CancellationToken   = default);

        #endregion

        #region AddOrUpdateEVSEs  (EVSEs, ...)

        /// <summary>
        /// Add or update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to add or update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<AddOrUpdateEVSEsResult>

            AddOrUpdateEVSEs(IEnumerable<IEVSE>  EVSEs,

                             DateTime?           Timestamp           = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null,
                             CancellationToken   CancellationToken   = default);

        #endregion

        #region UpdateEVSEs       (EVSEs, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<UpdateEVSEsResult>

            UpdateEVSEs(IEnumerable<IEVSE>  EVSEs,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default);

        #endregion

        #region DeleteEVSEs       (EVSEs, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs to delete.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        Task<DeleteEVSEsResult>

            DeleteEVSEs(IEnumerable<IEVSE>  EVSEs,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default);

        #endregion


    }

}
