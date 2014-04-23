/*
 * Copyright (c) 2013-2014 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Core <http://www.github.com/eMI3/Core>
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

namespace org.emi3group
{

    public enum PlugType
    {

        undefined               = 0,

        Standard_Type_A         = 1,
        Standard_Type_B         = 2,
        Standard_Type_C         = 3,
        Standard_Type_D         = 4,
        Standard_Type_E         = 5,

        /// <summary>
        /// Same as Type F
        /// </summary>
        SCHUKO                  = 6,

        /// <summary>
        /// Same as SCHUKO
        /// </summary>
        Standard_Type_F         = 6,

        Standard_Type_E_F       = 7,
        Standard_Type_G         = 8,
        Standard_Type_H         = 9,
        Standard_Type_I         = 10,
        Standard_Type_J         = 11,
        Standard_Type_K         = 12,
        Standard_Type_L         = 13,
        Standard_Type_M         = 14,

        /// <summary>
        /// (Same as IEC62196_Type_2!)
        /// </summary>
        Mennekes_Type_2         = 15,

        /// <summary>
        /// (Same as Mennekes_Type_2!)
        /// </summary>
        IEC62196_Type_2         = 15,

        Type_3C                 = 16,

        /// <summary>
        /// SAE J1772-2009/IEC 62196-2
        /// </summary>
        Type_1                  = 17,
        IEC309_2_single_phase   = 18,
        IEC309_2_three_phases   = 19,
        CHAdeMO                 = 20,

        /// <summary>
        /// Same as SCAME!
        /// </summary>
        Type_3A                 = 21,

        /// <summary>
        /// Same as Type_3A!
        /// </summary>
        SCAME                   = 21,

        NEMA_5_20               = 22,
        Tesla_Connector         = 23,
        AVCON_Connector         = 24,
        LargePaddle_Inductive   = 25,
        SmallPaddle_Inductive   = 26,
        ComboType_2_based       = 27,
        ComboType_1_based       = 28,
        ChinaGB_Part_2          = 29,
        ChinaGB_Part_3          = 30,
        BetterPlaceSocket       = 31,
        MarechalSocket          = 32,
        IEC309_2_DC_plug        = 33

    }



}
