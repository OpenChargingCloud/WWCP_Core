/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/GraphDefined/WWCP_Core>
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

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// The data licenses within the electric vehicle domain.
    /// </summary>
    public enum DataLicenses
    {

        /// <summary>
        /// No license, ask the data source for more details.
        /// </summary>
        None                                =  0,



        // Open Data licenses

        /// <summary>
        /// Open Data Commons: Public Domain Dedication and License (PDDL)
        /// </summary>
        /// <seealso cref="http://opendatacommons.org/licenses/pddl/"/>
        PublicDomainDedicationAndLicense    =  1,

        /// <summary>
        /// Open Data Commons: Attribution License (ODC-By)
        /// </summary>
        /// <seealso cref="http://opendatacommons.org/licenses/by/"/>
        AttributionLicense                  =  2,

        /// <summary>
        /// Open Data Commons: Open Data Commons Open Database License (ODbL)
        /// Attribution and Share-Alike for Data/Databases
        /// </summary>
        /// <seealso cref="http://opendatacommons.org/licenses/odbl/"/>
        /// <seealso cref="http://opendatacommons.org/licenses/odbl/summary/"/>
        /// <seealso cref="http://opendatacommons.org/licenses/odbl/1.0/"/>
        OpenDatabaseLicense                 =  3,




        // Special German licenses

        /// <summary>
        /// Datenlizenz Deutschland – Namensnennung – Version 2.0
        /// </summary>
        /// <seealso cref="https://www.govdata.de/dl-de/by-2-0"/>
        DatenlizenzDeutschland_BY_2         = 10,

        /// <summary>
        /// Datenlizenz Deutschland – Namensnennung – Version 2.0
        /// </summary>
        /// <seealso cref="https://www.govdata.de/dl-de/zero-2-0"/>
        DatenlizenzDeutschland_Zero_2       = 11,

        /// <summary>
        /// GeoLizenz V1.3 – Open
        /// </summary>
        /// <seealso cref="https://www.geolizenz.org/index/page.php?p=GL/opendata"/>
        /// <seealso cref="https://www.geolizenz.org/modules/geolizenz/docs/1.3.1/GeoLizenz_V1.3_Open_050615_V1.pdf"/>
        /// <seealso cref="https://www.geolizenz.org/modules/geolizenz/docs/1.3.1/Erl%C3%A4uterungen_GeoLizenzV1.3_Open_06.06.2015_V1.pdf"/>
        GeoLizenz_OpenData_1_3_1            = 12,




        // Creative Commons licenses

        /// <summary>
        /// Creative Commons Attribution 4.0 International (CC BY 4.0)
        /// </summary>
        /// <seealso cref="http://creativecommons.org/licenses/by/4.0/"/>
        /// <seealso cref="http://creativecommons.org/licenses/by/4.0/legalcode"/>
        CreativeCommons_BY_4                = 20,

        /// <summary>
        /// Creative Commons Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)
        /// </summary>
        /// <seealso cref="http://creativecommons.org/licenses/by-sa/4.0/"/>
        /// <seealso cref="http://creativecommons.org/licenses/by-sa/4.0/legalcode"/>
        CreativeCommons_BY_SA_4             = 21,

        /// <summary>
        /// Creative Commons Attribution-NoDerivs 4.0 International (CC BY-ND 4.0)
        /// </summary>
        /// <seealso cref="http://creativecommons.org/licenses/by-nd/4.0/"/>
        /// <seealso cref="http://creativecommons.org/licenses/by-nd/4.0/legalcode"/>
        CreativeCommons_BY_ND_4             = 22,

        /// <summary>
        /// Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)
        /// </summary>
        /// <seealso cref="http://creativecommons.org/licenses/by-nc/4.0/"/>
        /// <seealso cref="http://creativecommons.org/licenses/by-nc/4.0/legalcode"/>
        CreativeCommons_BY_NC_4             = 23,

        /// <summary>
        /// Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0)
        /// </summary>
        /// <seealso cref="http://creativecommons.org/licenses/by-nc-sa/4.0/"/>
        /// <seealso cref="http://creativecommons.org/licenses/by-nc-sa/4.0/legalcode"/>
        CreativeCommons_BY_NC_SA_4          = 24,

        /// <summary>
        /// Creative Commons Attribution-NonCommercial-NoDerivs 4.0 International (CC BY-NC-ND 4.0)
        /// </summary>
        /// <seealso cref="http://creativecommons.org/licenses/by-nc-nd/4.0/"/>
        /// <seealso cref="http://creativecommons.org/licenses/by-nc-nd/4.0/legalcode"/>
        CreativeCommons_BY_NC_ND_4          = 25,


    }

}
