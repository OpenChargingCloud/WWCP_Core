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

using org.GraphDefined.Vanaheimr.Illias;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP
{

    public delegate IRemoteEMobilityProvider RemoteEMobilityProviderCreatorDelegate(eMobilityProviderStub EMobilityProvider);

    public delegate String eMobilityProviderNameSelectorDelegate(I18NString I18NText);




    /// <summary>
    /// A delegate called whenever the static data of the e-mobility station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="eMobilityStation">The updated e-mobility station.</param>
    /// <param name="PropertyName">The name of the changed property.</param>
    /// <param name="OldValue">The old value of the changed property.</param>
    /// <param name="NewValue">The new value of the changed property.</param>
    public delegate Task OnEMobilityProviderDataChangedDelegate(DateTime          Timestamp,
                                                                eMobilityStation  eMobilityStation,
                                                                String            PropertyName,
                                                                Object            OldValue,
                                                                Object            NewValue);

    /// <summary>
    /// A delegate called whenever the admin status of the e-mobility station changed.
    /// </summary>
    /// <param name="Timestamp">The timestamp when this change was detected.</param>
    /// <param name="eMobilityStation">The updated e-mobility station.</param>
    /// <param name="OldStatus">The old timestamped status of the charging station.</param>
    /// <param name="NewStatus">The new timestamped status of the charging station.</param>
    public delegate Task OnEMobilityProviderAdminStatusChangedDelegate(DateTime                                      Timestamp,
                                                                       eMobilityStation                              eMobilityStation,
                                                                       Timestamped<eMobilityStationAdminStatusType>  OldStatus,
                                                                       Timestamped<eMobilityStationAdminStatusType>  NewStatus);

}
