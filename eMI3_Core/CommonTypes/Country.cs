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

#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace org.emi3group
{

    /// <summary>
    /// A country.
    /// </summary>
    public class Country : IEquatable<Country>, IComparable<Country>, IComparable
    {

        #region Data

        private static readonly Dictionary<String, Country> CountryNames = new Dictionary<String, Country>();
        private static readonly Dictionary<String, Country> Alpha2Codes  = new Dictionary<String, Country>();
        private static readonly Dictionary<String, Country> Alpha3Codes  = new Dictionary<String, Country>();
        private static readonly Dictionary<UInt16, Country> NumericCodes = new Dictionary<UInt16, Country>();

        #endregion

        #region Properties

        #region CountryName

        private readonly String _CountryName;

        /// <summary>
        /// The name of the country.
        /// </summary>
        public String CountryName
        {
            get
            {
                return _CountryName;
            }
        }

        #endregion

        #region Alpha2Code

        private readonly String _Alpha2Code;

        /// <summary>
        /// The ISO Alpha-2 Code of the country.
        /// </summary>
        public String Alpha2Code
        {
            get
            {
                return _Alpha2Code;
            }
        }

        #endregion

        #region Alpha3Code

        private readonly String _Alpha3Code;

        /// <summary>
        /// The ISO Alpha-3 Code of the country.
        /// </summary>
        public String Alpha3Code
        {
            get
            {
                return _Alpha3Code;
            }
        }

        #endregion

        #region NumericCode

        private readonly UInt16 _NumericCode;

        /// <summary>
        /// The ISO numeric code UN M49 Numerical Code of the country.
        /// </summary>
        public UInt16 NumericCode
        {
            get
            {
                return _NumericCode;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region Country(CountryName, Alpha2Code, Alpha3Code, NumericCode)

        /// <summary>
        /// Creates a new country based on a country name and its codings.
        /// </summary>
        /// <param name="CountryName">The name of the country.</param>
        /// <param name="Alpha2Code">The ISO Alpha-2 Code of the country.</param>
        /// <param name="Alpha3Code">The ISO Alpha-3 Code of the country.</param>
        /// <param name="NumericCode">The ISO numeric code UN M49 Numerical Code of the country.</param>
        public Country(String  CountryName,
                       String  Alpha2Code,
                       String  Alpha3Code,
                       UInt16  NumericCode)
        {

            this._CountryName  = CountryName;
            this._Alpha2Code   = Alpha2Code;
            this._Alpha3Code   = Alpha3Code;
            this._NumericCode  = NumericCode;

        }

        #endregion

        #endregion


        /// <summary>
        /// An unkown country.
        /// </summary>
        public static readonly Country unknown = new Country("unknown", "", "", 000);

        #region List of countries

        public static readonly Country Afghanistan                              = new Country("Afghanistan",                                    "AF", "AFG", 004);
        public static readonly Country AlandIslands                             = new Country("Aland Islands",                                  "AX", "ALA", 248);
        public static readonly Country Albania                                  = new Country("Albania",                                        "AL", "ALB", 008);
        public static readonly Country Algeria                                  = new Country("Algeria",                                        "DZ", "DZA", 012);
        public static readonly Country AmericanSamoa                            = new Country("American Samoa",                                 "AS", "ASM", 016);
        public static readonly Country Andorra                                  = new Country("Andorra",                                        "AD", "AND", 020);
        public static readonly Country Angola                                   = new Country("Angola",                                         "AO", "AGO", 024);
        public static readonly Country Anguilla                                 = new Country("Anguilla",                                       "AI", "AIA", 660);
        public static readonly Country Antarctica                               = new Country("Antarctica",                                     "AQ", "ATA", 010);
        public static readonly Country AntiguaAndBarbuda                        = new Country("Antigua and Barbuda",                            "AG", "ATG", 028);
        public static readonly Country Argentina                                = new Country("Argentina",                                      "AR", "ARG", 032);
        public static readonly Country Armenia                                  = new Country("Armenia",                                        "AM", "ARM", 051);
        public static readonly Country Aruba                                    = new Country("Aruba",                                          "AW", "ABW", 533);
        public static readonly Country Australia                                = new Country("Australia",                                      "AU", "AUS", 036);
        public static readonly Country Austria                                  = new Country("Austria",                                        "AT", "AUT", 040);
        public static readonly Country Azerbaijan                               = new Country("Azerbaijan",                                     "AZ", "AZE", 031);
        public static readonly Country Bahamas                                  = new Country("Bahamas",                                        "BS", "BHS", 044);
        public static readonly Country Bahrain                                  = new Country("Bahrain",                                        "BH", "BHR", 048);
        public static readonly Country Bangladesh                               = new Country("Bangladesh",                                     "BD", "BGD", 050);
        public static readonly Country Barbados                                 = new Country("Barbados",                                       "BB", "BRB", 052);
        public static readonly Country Belarus                                  = new Country("Belarus",                                        "BY", "BLR", 112);
        public static readonly Country Belgium                                  = new Country("Belgium",                                        "BE", "BEL", 056);
        public static readonly Country Belize                                   = new Country("Belize",                                         "BZ", "BLZ", 084);
        public static readonly Country Benin                                    = new Country("Benin",                                          "BJ", "BEN", 204);
        public static readonly Country Bermuda                                  = new Country("Bermuda",                                        "BM", "BMU", 060);
        public static readonly Country Bhutan                                   = new Country("Bhutan",                                         "BT", "BTN", 064);
        public static readonly Country Bolivia                                  = new Country("Bolivia",                                        "BO", "BOL", 068);
        public static readonly Country BosniaAndHerzegovina                     = new Country("Bosnia and Herzegovina",                         "BA", "BIH", 070);
        public static readonly Country Botswana                                 = new Country("Botswana",                                       "BW", "BWA", 072);
        public static readonly Country BouvetIsland                             = new Country("Bouvet Island",                                  "BV", "BVT", 074);
        public static readonly Country Brazil                                   = new Country("Brazil",                                         "BR", "BRA", 076);
        public static readonly Country BritishVirginIslands                     = new Country("British Virgin Islands",                         "VG", "VGB", 092);
        public static readonly Country BritishIndianOceanTerritory              = new Country("British Indian Ocean Territory",                 "IO", "IOT", 086);
        public static readonly Country BruneiDarussalam                         = new Country("Brunei Darussalam",                              "BN", "BRN", 096);
        public static readonly Country Bulgaria                                 = new Country("Bulgaria",                                       "BG", "BGR", 100);
        public static readonly Country BurkinaFaso                              = new Country("Burkina Faso",                                   "BF", "BFA", 854);
        public static readonly Country Burundi                                  = new Country("Burundi",                                        "BI", "BDI", 108);
        public static readonly Country Cambodia                                 = new Country("Cambodia",                                       "KH", "KHM", 116);
        public static readonly Country Cameroon                                 = new Country("Cameroon",                                       "CM", "CMR", 120);
        public static readonly Country Canada                                   = new Country("Canada",                                         "CA", "CAN", 124);
        public static readonly Country CapeVerde                                = new Country("Cape Verde",                                     "CV", "CPV", 132);
        public static readonly Country CaymanIslands                            = new Country("Cayman Islands",                                 "KY", "CYM", 136);
        public static readonly Country CentralAfricanRepublic                   = new Country("Central African Republic",                       "CF", "CAF", 140);
        public static readonly Country Chad                                     = new Country("Chad",                                           "TD", "TCD", 148);
        public static readonly Country Chile                                    = new Country("Chile",                                          "CL", "CHL", 152);
        public static readonly Country China                                    = new Country("China",                                          "CN", "CHN", 156);
        public static readonly Country HongKong                                 = new Country("Hong Kong",                                      "HK", "HKG", 344);
        public static readonly Country Macao                                    = new Country("Macao",                                          "MO", "MAC", 446);
        public static readonly Country ChristmasIsland                          = new Country("Christmas Island",                               "CX", "CXR", 162);
        public static readonly Country Cocos_Keeling_Islands                    = new Country("Cocos (Keeling) Islands",                        "CC", "CCK", 166);
        public static readonly Country Colombia                                 = new Country("Colombia",                                       "CO", "COL", 170);
        public static readonly Country Comoros                                  = new Country("Comoros",                                        "KM", "COM", 174);
        public static readonly Country Congo_Brazzaville                        = new Country("Congo (Brazzaville)",                            "CG", "COG", 178);
        public static readonly Country DemocraticRepublicOfTheCongo             = new Country("Democratic Republic of the Congo",               "CD", "COD", 180);
        public static readonly Country CookIslands                              = new Country("Cook Islands",                                   "CK", "COK", 184);
        public static readonly Country CostaRica                                = new Country("Costa Rica",                                     "CR", "CRI", 188);
        public static readonly Country CoteDIvoire                              = new Country("Côte d'Ivoire",                                  "CI", "CIV", 384);
        public static readonly Country Croatia                                  = new Country("Croatia",                                        "HR", "HRV", 191);
        public static readonly Country Cuba                                     = new Country("Cuba",                                           "CU", "CUB", 192);
        public static readonly Country Cyprus                                   = new Country("Cyprus",                                         "CY", "CYP", 196);
        public static readonly Country CzechRepublic                            = new Country("Czech Republic",                                 "CZ", "CZE", 203);
        public static readonly Country Denmark                                  = new Country("Denmark",                                        "DK", "DNK", 208);
        public static readonly Country Djibouti                                 = new Country("Djibouti",                                       "DJ", "DJI", 262);
        public static readonly Country Dominica                                 = new Country("Dominica",                                       "DM", "DMA", 212);
        public static readonly Country DominicanRepublic                        = new Country("Dominican Republic",                             "DO", "DOM", 214);
        public static readonly Country Ecuador                                  = new Country("Ecuador",                                        "EC", "ECU", 218);
        public static readonly Country Egypt                                    = new Country("Egypt",                                          "EG", "EGY", 818);
        public static readonly Country ElSalvador                               = new Country("El Salvador",                                    "SV", "SLV", 222);
        public static readonly Country EquatorialGuinea                         = new Country("Equatorial Guinea",                              "GQ", "GNQ", 226);
        public static readonly Country Eritrea                                  = new Country("Eritrea",                                        "ER", "ERI", 232);
        public static readonly Country Estonia                                  = new Country("Estonia",                                        "EE", "EST", 233);
        public static readonly Country Ethiopia                                 = new Country("Ethiopia",                                       "ET", "ETH", 231);
        public static readonly Country FalklandIslands_Malvinas                 = new Country("Falkland Islands (Malvinas)",                    "FK", "FLK", 238);
        public static readonly Country FaroeIslands                             = new Country("Faroe Islands",                                  "FO", "FRO", 234);
        public static readonly Country Fiji                                     = new Country("Fiji",                                           "FJ", "FJI", 242);
        public static readonly Country Finland                                  = new Country("Finland",                                        "FI", "FIN", 246);
        public static readonly Country France                                   = new Country("France",                                         "FR", "FRA", 250);
        public static readonly Country FrenchGuiana                             = new Country("French Guiana",                                  "GF", "GUF", 254);
        public static readonly Country FrenchPolynesia                          = new Country("French Polynesia",                               "PF", "PYF", 258);
        public static readonly Country FrenchSouthernTerritories                = new Country("French Southern Territories",                    "TF", "ATF", 260);
        public static readonly Country Gabon                                    = new Country("Gabon",                                          "GA", "GAB", 266);
        public static readonly Country Gambia                                   = new Country("Gambia",                                         "GM", "GMB", 270);
        public static readonly Country Georgia                                  = new Country("Georgia",                                        "GE", "GEO", 268);
        public static readonly Country Germany                                  = new Country("Germany",                                        "DE", "DEU", 276);
        public static readonly Country Ghana                                    = new Country("Ghana",                                          "GH", "GHA", 288);
        public static readonly Country Gibraltar                                = new Country("Gibraltar",                                      "GI", "GIB", 292);
        public static readonly Country Greece                                   = new Country("Greece",                                         "GR", "GRC", 300);
        public static readonly Country Greenland                                = new Country("Greenland",                                      "GL", "GRL", 304);
        public static readonly Country Grenada                                  = new Country("Grenada",                                        "GD", "GRD", 308);
        public static readonly Country Guadeloupe                               = new Country("Guadeloupe",                                     "GP", "GLP", 312);
        public static readonly Country Guam                                     = new Country("Guam",                                           "GU", "GUM", 316);
        public static readonly Country Guatemala                                = new Country("Guatemala",                                      "GT", "GTM", 320);
        public static readonly Country Guernsey                                 = new Country("Guernsey",                                       "GG", "GGY", 831);
        public static readonly Country Guinea                                   = new Country("Guinea",                                         "GN", "GIN", 324);
        public static readonly Country GuineaBissau                             = new Country("Guinea-Bissau",                                  "GW", "GNB", 624);
        public static readonly Country Guyana                                   = new Country("Guyana",                                         "GY", "GUY", 328);
        public static readonly Country Haiti                                    = new Country("Haiti",                                          "HT", "HTI", 332);
        public static readonly Country HeardIslandAndMcdonaldIslands            = new Country("Heard Island and Mcdonald Islands",              "HM", "HMD", 334);
        public static readonly Country VaticanState                             = new Country("Holy See (Vatican City State)",                  "VA", "VAT", 336);
        public static readonly Country Honduras                                 = new Country("Honduras",                                       "HN", "HND", 340);
        public static readonly Country Hungary                                  = new Country("Hungary",                                        "HU", "HUN", 348);
        public static readonly Country Iceland                                  = new Country("Iceland",                                        "IS", "ISL", 352);
        public static readonly Country India                                    = new Country("India",                                          "IN", "IND", 356);
        public static readonly Country Indonesia                                = new Country("Indonesia",                                      "ID", "IDN", 360);
        public static readonly Country IslamicRepublicOfIran                    = new Country("Islamic Republic of Iran",                       "IR", "IRN", 364);
        public static readonly Country Iraq                                     = new Country("Iraq",                                           "IQ", "IRQ", 368);
        public static readonly Country Ireland                                  = new Country("Ireland",                                        "IE", "IRL", 372);
        public static readonly Country IsleOfMan                                = new Country("Isle of Man",                                    "IM", "IMN", 833);
        public static readonly Country Israel                                   = new Country("Israel",                                         "IL", "ISR", 376);
        public static readonly Country Italy                                    = new Country("Italy",                                          "IT", "ITA", 380);
        public static readonly Country Jamaica                                  = new Country("Jamaica",                                        "JM", "JAM", 388);
        public static readonly Country Japan                                    = new Country("Japan",                                          "JP", "JPN", 392);
        public static readonly Country Jersey                                   = new Country("Jersey",                                         "JE", "JEY", 832);
        public static readonly Country Jordan                                   = new Country("Jordan",                                         "JO", "JOR", 400);
        public static readonly Country Kazakhstan                               = new Country("Kazakhstan",                                     "KZ", "KAZ", 398);
        public static readonly Country Kenya                                    = new Country("Kenya",                                          "KE", "KEN", 404);
        public static readonly Country Kiribati                                 = new Country("Kiribati",                                       "KI", "KIR", 296);
        public static readonly Country DemocraticPeoplesRepublicOfKorea         = new Country("Democratic People's Republic of Korea",          "KP", "PRK", 408);
        public static readonly Country RepublicOfKorea                          = new Country("Republic of Korea",                              "KR", "KOR", 410);
        public static readonly Country Kuwait                                   = new Country("Kuwait",                                         "KW", "KWT", 414);
        public static readonly Country Kyrgyzstan                               = new Country("Kyrgyzstan",                                     "KG", "KGZ", 417);
        public static readonly Country LaoPDR                                   = new Country("Lao PDR",                                        "LA", "LAO", 418);
        public static readonly Country Latvia                                   = new Country("Latvia",                                         "LV", "LVA", 428);
        public static readonly Country Lebanon                                  = new Country("Lebanon",                                        "LB", "LBN", 422);
        public static readonly Country Lesotho                                  = new Country("Lesotho",                                        "LS", "LSO", 426);
        public static readonly Country Liberia                                  = new Country("Liberia",                                        "LR", "LBR", 430);
        public static readonly Country Libya                                    = new Country("Libya",                                          "LY", "LBY", 434);
        public static readonly Country Liechtenstein                            = new Country("Liechtenstein",                                  "LI", "LIE", 438);
        public static readonly Country Lithuania                                = new Country("Lithuania",                                      "LT", "LTU", 440);
        public static readonly Country Luxembourg                               = new Country("Luxembourg",                                     "LU", "LUX", 442);
        public static readonly Country RepublicOfMacedonia                      = new Country("Republic of Macedonia",                          "MK", "MKD", 807);
        public static readonly Country Madagascar                               = new Country("Madagascar",                                     "MG", "MDG", 450);
        public static readonly Country Malawi                                   = new Country("Malawi",                                         "MW", "MWI", 454);
        public static readonly Country Malaysia                                 = new Country("Malaysia",                                       "MY", "MYS", 458);
        public static readonly Country Maldives                                 = new Country("Maldives",                                       "MV", "MDV", 462);
        public static readonly Country Mali                                     = new Country("Mali",                                           "ML", "MLI", 466);
        public static readonly Country Malta                                    = new Country("Malta",                                          "MT", "MLT", 470);
        public static readonly Country MarshallIslands                          = new Country("Marshall Islands",                               "MH", "MHL", 584);
        public static readonly Country Martinique                               = new Country("Martinique",                                     "MQ", "MTQ", 474);
        public static readonly Country Mauritania                               = new Country("Mauritania",                                     "MR", "MRT", 478);
        public static readonly Country Mauritius                                = new Country("Mauritius",                                      "MU", "MUS", 480);
        public static readonly Country Mayotte                                  = new Country("Mayotte",                                        "YT", "MYT", 175);
        public static readonly Country Mexico                                   = new Country("Mexico",                                         "MX", "MEX", 484);
        public static readonly Country FederatedStatesOfMicronesia              = new Country("Federated States of Micronesia",                 "FM", "FSM", 583);
        public static readonly Country Moldova                                  = new Country("Moldova",                                        "MD", "MDA", 498);
        public static readonly Country Monaco                                   = new Country("Monaco",                                         "MC", "MCO", 492);
        public static readonly Country Mongolia                                 = new Country("Mongolia",                                       "MN", "MNG", 496);
        public static readonly Country Montenegro                               = new Country("Montenegro",                                     "ME", "MNE", 499);
        public static readonly Country Montserrat                               = new Country("Montserrat",                                     "MS", "MSR", 500);
        public static readonly Country Morocco                                  = new Country("Morocco",                                        "MA", "MAR", 504);
        public static readonly Country Mozambique                               = new Country("Mozambique",                                     "MZ", "MOZ", 508);
        public static readonly Country Myanmar                                  = new Country("Myanmar",                                        "MM", "MMR", 104);
        public static readonly Country Namibia                                  = new Country("Namibia",                                        "NA", "NAM", 516);
        public static readonly Country Nauru                                    = new Country("Nauru",                                          "NR", "NRU", 520);
        public static readonly Country Nepal                                    = new Country("Nepal",                                          "NP", "NPL", 524);
        public static readonly Country Netherlands                              = new Country("Netherlands",                                    "NL", "NLD", 528);
        public static readonly Country NetherlandsAntilles                      = new Country("Netherlands Antilles",                           "AN", "ANT", 530);
        public static readonly Country NewCaledonia                             = new Country("New Caledonia",                                  "NC", "NCL", 540);
        public static readonly Country NewZealand                               = new Country("New Zealand",                                    "NZ", "NZL", 554);
        public static readonly Country Nicaragua                                = new Country("Nicaragua",                                      "NI", "NIC", 558);
        public static readonly Country Niger                                    = new Country("Niger",                                          "NE", "NER", 562);
        public static readonly Country Nigeria                                  = new Country("Nigeria",                                        "NG", "NGA", 566);
        public static readonly Country Niue                                     = new Country("Niue",                                           "NU", "NIU", 570);
        public static readonly Country NorfolkIsland                            = new Country("Norfolk Island",                                 "NF", "NFK", 574);
        public static readonly Country NorthernMarianaIslands                   = new Country("Northern Mariana Islands",                       "MP", "MNP", 580);
        public static readonly Country Norway                                   = new Country("Norway",                                         "NO", "NOR", 578);
        public static readonly Country Oman                                     = new Country("Oman",                                           "OM", "OMN", 512);
        public static readonly Country Pakistan                                 = new Country("Pakistan",                                       "PK", "PAK", 586);
        public static readonly Country Palau                                    = new Country("Palau",                                          "PW", "PLW", 585);
        public static readonly Country PalestinianTerritory                     = new Country("Palestinian Territory",                          "PS", "PSE", 275);
        public static readonly Country Panama                                   = new Country("Panama",                                         "PA", "PAN", 591);
        public static readonly Country PapuaNewGuinea                           = new Country("Papua New Guinea",                               "PG", "PNG", 598);
        public static readonly Country Paraguay                                 = new Country("Paraguay",                                       "PY", "PRY", 600);
        public static readonly Country Peru                                     = new Country("Peru",                                           "PE", "PER", 604);
        public static readonly Country Philippines                              = new Country("Philippines",                                    "PH", "PHL", 608);
        public static readonly Country Pitcairn                                 = new Country("Pitcairn",                                       "PN", "PCN", 612);
        public static readonly Country Poland                                   = new Country("Poland",                                         "PL", "POL", 616);
        public static readonly Country Portugal                                 = new Country("Portugal",                                       "PT", "PRT", 620);
        public static readonly Country PuertoRico                               = new Country("Puerto Rico",                                    "PR", "PRI", 630);
        public static readonly Country Qatar                                    = new Country("Qatar",                                          "QA", "QAT", 634);
        public static readonly Country Réunion                                  = new Country("Réunion",                                        "RE", "REU", 638);
        public static readonly Country Romania                                  = new Country("Romania",                                        "RO", "ROU", 642);
        public static readonly Country RussianFederation                        = new Country("Russian Federation",                             "RU", "RUS", 643);
        public static readonly Country Rwanda                                   = new Country("Rwanda",                                         "RW", "RWA", 646);
        public static readonly Country SaintBarthelemy                          = new Country("Saint-Barthélemy",                               "BL", "BLM", 652);
        public static readonly Country SaintHelena                              = new Country("Saint Helena",                                   "SH", "SHN", 654);
        public static readonly Country SaintKittsAndNevis                       = new Country("Saint Kitts and Nevis",                          "KN", "KNA", 659);
        public static readonly Country SaintLucia                               = new Country("Saint Lucia",                                    "LC", "LCA", 662);
        public static readonly Country SaintMartin                              = new Country("Saint-Martin (French part)",                     "MF", "MAF", 663);
        public static readonly Country SaintPierreAndMiquelon                   = new Country("Saint Pierre and Miquelon",                      "PM", "SPM", 666);
        public static readonly Country SaintVincentAndGrenadines                = new Country("Saint Vincent and Grenadines",                   "VC", "VCT", 670);
        public static readonly Country Samoa                                    = new Country("Samoa",                                          "WS", "WSM", 882);
        public static readonly Country SanMarino                                = new Country("San Marino",                                     "SM", "SMR", 674);
        public static readonly Country SaoTomeAndPrincipe                       = new Country("Sao Tome and Principe",                          "ST", "STP", 678);
        public static readonly Country SaudiArabia                              = new Country("Saudi Arabia",                                   "SA", "SAU", 682);
        public static readonly Country Senegal                                  = new Country("Senegal",                                        "SN", "SEN", 686);
        public static readonly Country Serbia                                   = new Country("Serbia",                                         "RS", "SRB", 688);
        public static readonly Country Seychelles                               = new Country("Seychelles",                                     "SC", "SYC", 690);
        public static readonly Country SierraLeone                              = new Country("Sierra Leone",                                   "SL", "SLE", 694);
        public static readonly Country Singapore                                = new Country("Singapore",                                      "SG", "SGP", 702);
        public static readonly Country Slovakia                                 = new Country("Slovakia",                                       "SK", "SVK", 703);
        public static readonly Country Slovenia                                 = new Country("Slovenia",                                       "SI", "SVN", 705);
        public static readonly Country SolomonIslands                           = new Country("Solomon Islands",                                "SB", "SLB", 090);
        public static readonly Country Somalia                                  = new Country("Somalia",                                        "SO", "SOM", 706);
        public static readonly Country SouthAfrica                              = new Country("South Africa",                                   "ZA", "ZAF", 710);
        public static readonly Country SouthGeorgiaAndTheSouthSandwichIslands   = new Country("South Georgia and the South Sandwich Islands",   "GS", "SGS", 239);
        public static readonly Country SouthSudan                               = new Country("South Sudan",                                    "SS", "SSD", 728);
        public static readonly Country Spain                                    = new Country("Spain",                                          "ES", "ESP", 724);
        public static readonly Country SriLanka                                 = new Country("Sri Lanka",                                      "LK", "LKA", 144);
        public static readonly Country Sudan                                    = new Country("Sudan",                                          "SD", "SDN", 736);
        public static readonly Country Suriname                                 = new Country("Suriname",                                       "SR", "SUR", 740);
        public static readonly Country SvalbardAndJanMayenIslands               = new Country("Svalbard and Jan Mayen Islands",                 "SJ", "SJM", 744);
        public static readonly Country Swaziland                                = new Country("Swaziland",                                      "SZ", "SWZ", 748);
        public static readonly Country Sweden                                   = new Country("Sweden",                                         "SE", "SWE", 752);
        public static readonly Country Switzerland                              = new Country("Switzerland",                                    "CH", "CHE", 756);
        public static readonly Country SyrianArabRepublic                       = new Country("Syrian Arab Republic",                           "SY", "SYR", 760);
        public static readonly Country Taiwan                                   = new Country("Taiwan",                                         "TW", "TWN", 158);
        public static readonly Country Tajikistan                               = new Country("Tajikistan",                                     "TJ", "TJK", 762);
        public static readonly Country UnitedRepublicOfTanzania                 = new Country("United Republic of Tanzania",                    "TZ", "TZA", 834);
        public static readonly Country Thailand                                 = new Country("Thailand",                                       "TH", "THA", 764);
        public static readonly Country TimorLeste                               = new Country("Timor-Leste",                                    "TL", "TLS", 626);
        public static readonly Country Togo                                     = new Country("Togo",                                           "TG", "TGO", 768);
        public static readonly Country Tokelau                                  = new Country("Tokelau",                                        "TK", "TKL", 772);
        public static readonly Country Tonga                                    = new Country("Tonga",                                          "TO", "TON", 776);
        public static readonly Country TrinidadAndTobago                        = new Country("Trinidad and Tobago",                            "TT", "TTO", 780);
        public static readonly Country Tunisia                                  = new Country("Tunisia",                                        "TN", "TUN", 788);
        public static readonly Country Turkey                                   = new Country("Turkey",                                         "TR", "TUR", 792);
        public static readonly Country Turkmenistan                             = new Country("Turkmenistan",                                   "TM", "TKM", 795);
        public static readonly Country TurksAndCaicosIslands                    = new Country("Turks and Caicos Islands",                       "TC", "TCA", 796);
        public static readonly Country Tuvalu                                   = new Country("Tuvalu",                                         "TV", "TUV", 798);
        public static readonly Country Uganda                                   = new Country("Uganda",                                         "UG", "UGA", 800);
        public static readonly Country Ukraine                                  = new Country("Ukraine",                                        "UA", "UKR", 804);
        public static readonly Country UnitedArabEmirates                       = new Country("United Arab Emirates",                           "AE", "ARE", 784);
        public static readonly Country UnitedKingdom                            = new Country("United Kingdom",                                 "GB", "GBR", 826);
        public static readonly Country UnitedStatesOfAmerica                    = new Country("United States of America",                       "US", "USA", 840);
        public static readonly Country UnitedStatesMinorOutlyingIslands         = new Country("United States Minor Outlying Islands",           "UM", "UMI", 581);
        public static readonly Country Uruguay                                  = new Country("Uruguay",                                        "UY", "URY", 858);
        public static readonly Country Uzbekistan                               = new Country("Uzbekistan",                                     "UZ", "UZB", 860);
        public static readonly Country Vanuatu                                  = new Country("Vanuatu",                                        "VU", "VUT", 548);
        public static readonly Country Venezuela                                = new Country("Bolivarian Republic of Venezuela",               "VE", "VEN", 862);
        public static readonly Country VietNam                                  = new Country("Viet Nam",                                       "VN", "VNM", 704);
        public static readonly Country VirginIslands                            = new Country("Virgin Islands",                                 "VI", "VIR", 850);
        public static readonly Country WallisAndFutunaIslands                   = new Country("Wallis and Futuna Islands",                      "WF", "WLF", 876);
        public static readonly Country WesternSahara                            = new Country("Western Sahara",                                 "EH", "ESH", 732);
        public static readonly Country Yemen                                    = new Country("Yemen",                                          "YE", "YEM", 887);
        public static readonly Country Zambia                                   = new Country("Zambia",                                         "ZM", "ZMB", 894);
        public static readonly Country Zimbabwe                                 = new Country("Zimbabwe",                                       "ZW", "ZWE", 716);

        #endregion


        #region Tools

        #region (private) ReflectData()

        private static void ReflectData()
        {

            lock (CountryNames)
            {

                if (CountryNames.Count == 0)
                {

                    Country _Country;

                    foreach (var _FieldInfo in typeof(Country).GetFields())
                    {

                        _Country = _FieldInfo.GetValue(null) as Country;

                        if (_Country.CountryName == unknown.CountryName)
                            continue;

                        CountryNames.Add(_Country.CountryName, _Country);
                        Alpha2Codes. Add(_Country.Alpha2Code,  _Country);
                        Alpha3Codes. Add(_Country.Alpha3Code,  _Country);
                        NumericCodes.Add(_Country.NumericCode, _Country);

                    }
                }

            }

        }

        #endregion


        #region Parse(AnyString)

        /// <summary>
        /// Tries to find the apropriate country for the given string.
        /// </summary>
        /// <param name="AnyString">Any string.</param>
        public static Country Parse(String AnyString)
        {

            Country _Country;

            if (AnyString.Length == 2)
                return ParseAlpha2Code(AnyString);

            else if (AnyString.Length == 3)
                return ParseAlpha3Code(AnyString);

            else if (TryParseNumericCode(AnyString, out _Country))
                return _Country;

            return ParseCountryName(AnyString);

        }

        #endregion


        #region ParseCountryName(CountryName)

        /// <summary>
        /// Tries to find the apropriate country for the given country name.
        /// </summary>
        /// <param name="CountryName">A country name.</param>
        public static Country ParseCountryName(String CountryName)
        {

            ReflectData();

            Country _Country;

            if (CountryNames.TryGetValue(CountryName, out _Country))
                return _Country;

            return null;

        }

        #endregion

        #region TryParseCountryName(CountryName, out Country)

        /// <summary>
        /// Tries to find the apropriate country for the given country name.
        /// </summary>
        /// <param name="CountryName">A country name.</param>
        /// <param name="Country">The corresponding country.</param>
        /// <returns>true, if successful; false otherwise.</returns>
        public static Boolean TryParseCountryName(String CountryName, out Country Country)
        {
            ReflectData();
            return CountryNames.TryGetValue(CountryName, out Country);
        }

        #endregion


        #region ParseAlpha2Code(Alpha2Code)

        /// <summary>
        /// Tries to find the apropriate country for the given alpha2code.
        /// </summary>
        /// <param name="Alpha2Code">An alpha2code for a country, e.g. "DE" for Germany.</param>
        public static Country ParseAlpha2Code(String Alpha2Code)
        {

            ReflectData();

            Country _Country;

            if (Alpha2Codes.TryGetValue(Alpha2Code, out _Country))
                return _Country;

            return null;

        }

        #endregion

        #region TryParseAlpha2Code(Alpha2Code, out Country)

        /// <summary>
        /// Tries to find the apropriate country for the given alpha2code.
        /// </summary>
        /// <param name="Alpha2Code">An alpha2code for a country, e.g. "DE" for Germany.</param>
        /// <param name="Country">The corresponding country.</param>
        /// <returns>true, if successful; false otherwise.</returns>
        public static Boolean TryParseAlpha2Code(String Alpha2Code, out Country Country)
        {
            ReflectData();
            return Alpha2Codes.TryGetValue(Alpha2Code, out Country);
        }

        #endregion


        #region ParseAlpha3Code(Alpha3Code)

        /// <summary>
        /// Tries to find the apropriate country for the given alpha3code.
        /// </summary>
        /// <param name="Alpha3Code">An alpha3code for a country, e.g. "DEU" for Germany.</param>
        public static Country ParseAlpha3Code(String Alpha3Code)
        {

            ReflectData();

            Country _Country;

            if (Alpha3Codes.TryGetValue(Alpha3Code, out _Country))
                return _Country;

            return null;

        }

        #endregion

        #region TryParseAlpha3Code(Alpha3Code, out Country)

        /// <summary>
        /// Tries to find the apropriate country for the given alpha3code.
        /// </summary>
        /// <param name="Alpha3Code">An alpha3code for a country, e.g. "DEU" for Germany.</param>
        /// <param name="Country">The corresponding country.</param>
        /// <returns>true, if successful; false otherwise.</returns>
        public static Boolean TryParseAlpha3Code(String Alpha3Code, out Country Country)
        {
            ReflectData();
            return Alpha3Codes.TryGetValue(Alpha3Code, out Country);
        }

        #endregion


        #region ParseNumericCode(NumericCode)

        /// <summary>
        /// Tries to find the apropriate country for the given numeric code.
        /// </summary>
        /// <param name="NumericCode">A numeric code for a country, e.g. "276" for Germany.</param>
        public static Country ParseNumericCode(UInt16 NumericCode)
        {

            ReflectData();

            Country _Country;

            if (NumericCodes.TryGetValue(NumericCode, out _Country))
                return _Country;

            return null;

        }

        /// <summary>
        /// Tries to find the apropriate country for the given numeric code.
        /// </summary>
        /// <param name="NumericCode">A numeric code for a country, e.g. "276" for Germany.</param>
        public static Country ParseNumericCode(String NumericCode)
        {
            ReflectData();
            return NumericCodes[UInt16.Parse(NumericCode)];
        }

        #endregion

        #region TryParseNumericCode(NumericCode, out Country)

        /// <summary>
        /// Tries to find the apropriate country for the given numeric code.
        /// </summary>
        /// <param name="NumericCode">A numeric code for a country, e.g. "276" for Germany.</param>
        /// <param name="Country">The corresponding country.</param>
        /// <returns>true, if successful; false otherwise.</returns>
        public static Boolean TryParseNumericCode(UInt16 NumericCode, out Country Country)
        {
            ReflectData();
            return NumericCodes.TryGetValue(NumericCode, out Country);
        }

        /// <summary>
        /// Tries to find the apropriate country for the given numeric code.
        /// </summary>
        /// <param name="NumericCode">A numeric code for a country, e.g. "276" for Germany.</param>
        /// <param name="Country">The corresponding country.</param>
        /// <returns>true, if successful; false otherwise.</returns>
        public static Boolean TryParseNumericCode(String NumericCode, out Country Country)
        {

            ReflectData();

            UInt16 _NumericCode;

            if (UInt16.TryParse(NumericCode, out _NumericCode))
                return NumericCodes.TryGetValue(_NumericCode, out Country);

            Country = unknown;
            return false;

        }

        #endregion

        #endregion


        #region Operator overloading

        #region Operator == (myCountry1, myCountry2)

        public static Boolean operator == (Country myCountry1, Country myCountry2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(myCountry1, myCountry2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) myCountry1 == null) || ((Object) myCountry2 == null))
                return false;

            return myCountry1.Equals(myCountry2);

        }

        #endregion

        #region Operator != (myCountry1, myCountry2)

        public static Boolean operator != (Country myCountry1, Country myCountry2)
        {
            return !(myCountry1 == myCountry2);
        }

        #endregion

        #region Operator <  (myCountry1, myCountry2)

        public static Boolean operator < (Country myCountry1, Country myCountry2)
        {

            // Check if myCountry1 is null
            if ((Object) myCountry1 == null)
                throw new ArgumentNullException("Parameter myCountry1 must not be null!");

            // Check if myCountry2 is null
            if ((Object) myCountry2 == null)
                throw new ArgumentNullException("Parameter myCountry2 must not be null!");

            return myCountry1.CompareTo(myCountry2) < 0;

        }

        #endregion

        #region Operator >  (myCountry1, myCountry2)

        public static Boolean operator > (Country myCountry1, Country myCountry2)
        {

            // Check if myCountry1 is null
            if ((Object) myCountry1 == null)
                throw new ArgumentNullException("Parameter myCountry1 must not be null!");

            // Check if myCountry2 is null
            if ((Object) myCountry2 == null)
                throw new ArgumentNullException("Parameter myCountry2 must not be null!");

            return myCountry1.CompareTo(myCountry2) > 0;

        }

        #endregion

        #region Operator <= (myCountry1, myCountry2)

        public static Boolean operator <= (Country myCountry1, Country myCountry2)
        {
            return !(myCountry1 > myCountry2);
        }

        #endregion

        #region Operator >= (myCountry1, myCountry2)

        public static Boolean operator >= (Country myCountry1, Country myCountry2)
        {
            return !(myCountry1 < myCountry2);
        }

        #endregion

        #endregion

        #region IComparable<Country> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException("The given object must not be null!");

            // Check if the given object is an Country.
            var Country = Object as Country;
            if ((Object) Country == null)
                throw new ArgumentException("The given object is not a Country!");

            return CompareTo(Country);

        }

        #endregion

        #region CompareTo(Country)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Country">An object to compare with.</param>
        public Int32 CompareTo(Country Country)
        {

            if ((Object) Country == null)
                throw new ArgumentNullException("The given Country must not be null!");

            return CountryName.CompareTo(Country.CountryName);

        }

        #endregion

        #endregion

        #region IEquatable<Country> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an Country.
            var Country = Object as Country;
            if ((Object) Country == null)
                return false;

            return this.Equals(Country);

        }

        #endregion

        #region Equals(Country)

        /// <summary>
        /// Compares two Countrys for equality.
        /// </summary>
        /// <param name="Country">A Country to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Country Country)
        {

            if ((Object) Country == null)
                return false;

            return CountryName == Country.CountryName;

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return CountryName.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Return a string represtentation of this object.
        /// </summary>
        public override String ToString()
        {
            return String.Concat(_CountryName, ", ", _Alpha2Code, ", ", _Alpha3Code, ", ", _NumericCode);
        }

        #endregion

    }

}
