/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 <http://www.github.com/GraphDefined/eMI3>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using org.GraphDefined.WWCP;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.UnitTests
{

    /// <summary>
    /// Minimal Unit tests.
    /// </summary>
    [TestFixture]
    public class MinimalTests
    {


        public void Test0()
        {

            var _rns = new RoamingNetworks();
            var _rn  = _rns.CreateNewRoamingNetwork(RoamingNetwork_Id.New);
            var _op  = _rn.CreateNewEVSEOperator(EVSEOperator_Id.Parse("DE*822"));
            var _cp  = _op.CreateNewChargingPool();
            _cp.StatusAggregationDelegate = report => {
                                                          var max   = report.Max  (v => v.Value);
                                                          var max_n = report.Where(o => o.Value == max);
                                                          return (ChargingPoolStatusType) max_n.OrderBy(o => o.Key).First().Key;
                                                      };
            _cp.OnAggregatedStatusChanged += (ts, pool, os, ns) => { Console.WriteLine("New pool state: " + ns.Value); };

            var s1  = _cp.CreateNewStation(ChargingStation_Id.Random(_op.Id));
            s1.StatusAggregationDelegate = report => {
                                                          var max   = report.Max  (v => v.Value);
                                                          var max_n = report.Where(o => o.Value == max);
                                                          return (ChargingStationStatusType) max_n.OrderBy(o => o.Key).First().Key;
                                                      };
            s1.OnAggregatedStatusChanged += (ts, sta, os, ns) => { Console.WriteLine("New station #1 state: " + ns.Value); };

            var e1 = s1.CreateNewEVSE(EVSE_Id.Parse("DE*822*E1111*1"));
            var e2 = s1.CreateNewEVSE(EVSE_Id.Parse("DE*822*E1111*2"));
            var e3 = s1.CreateNewEVSE(EVSE_Id.Parse("DE*822*E1111*3"));
            var s2 = _cp.CreateNewStation(ChargingStation_Id.Random(_op.Id));
            s2.StatusAggregationDelegate = report => {
                                                          var max   = report.Max  (v => v.Value);
                                                          var max_n = report.Where(o => o.Value == max);
                                                          return (ChargingStationStatusType) max_n.OrderBy(o => o.Key).First().Key;
                                                      };
            s2.OnAggregatedStatusChanged += (ts, sta, os, ns) => { Console.WriteLine("New station #2 state: " + ns.Value); };

            var f1 = s2.CreateNewEVSE(EVSE_Id.Parse("DE*822*E2222*1"));
            var f2 = s2.CreateNewEVSE(EVSE_Id.Parse("DE*822*E2222*2"));
            var f3 = s2.CreateNewEVSE(EVSE_Id.Parse("DE*822*E2222*3"));


            e1.Status = EVSEStatusType.Available;
            e2.Status = EVSEStatusType.Available;
            e3.Status = EVSEStatusType.Available;

            f1.Status = EVSEStatusType.Available;
            f2.Status = EVSEStatusType.Available;
            f3.Status = EVSEStatusType.Available;

            e2.Status = EVSEStatusType.Charging;
            e3.Status = EVSEStatusType.Charging;

        }

        #region Test1()

        [Test]
        public void Test1()
        {

            //#region Create a new roaming network and an EVSE operator

            //var RoamingNetwork = new RoamingNetwork(RoamingNetwork_Id.Parse("eMI3"));

            //Assert.IsNotNull(RoamingNetwork);
            //Assert.AreEqual(RoamingNetwork_Id.Parse("eMI3"), RoamingNetwork.Id);
            //Assert.AreEqual("eMI3",                          RoamingNetwork.Id.ToString());
            //Assert.AreEqual(0,                               RoamingNetwork.Count(),               "The number of entities within the roaming network must be 0!");
            //Assert.AreEqual(0,                               RoamingNetwork.EVSEOperators.Count(), "The number of EVSE operators within the roaming network must be 0!");

            //var BelectricDriveOperator = RoamingNetwork.CreateNewEVSEOperator(
            //                                                EVSEOperator_Id.Parse(Country.Germany, "822"),
            //                                                Operator => {
            //                                                    Operator.Name.Add(Languages.de, "Belectric Drive");
            //                                                }
            //                                            );

            //Assert.IsNotNull(BelectricDriveOperator);
            //Assert.AreEqual(EVSEOperator_Id.Parse(Country.Germany, "822"), BelectricDriveOperator.Id);
            //Assert.AreEqual("822",                                         BelectricDriveOperator.Id.ToString());
            //Assert.AreEqual(0,                                             BelectricDriveOperator.Count(),       "The number of ChargingPools registered with the EVSE operator must be 0!");

            //Assert.AreEqual(1,                                             RoamingNetwork.Count(),               "The number of entities within the roaming network must be 1 now!");
            //Assert.AreEqual(1,                                             RoamingNetwork.EVSEOperators.Count(), "The number of EVSE operators within the roaming network must be 1 now!");

            //#endregion

            //var BelectricDriveEVSEOperatorId = EVSEOperator_Id.Parse(Country.Germany, "822");

            //#region Create an EVSE pool

            //var BITSPool = BelectricDriveOperator.CreateNewChargingPool(
            //                          ChargingPool_Id.Parse("BITS"),
            //                          pool => {

            //                              pool.Name.       Add(Languages.en, "Belectric IT Solutions").
            //                                               Add(Languages.de, "Belectric IT Solutions GmbH");

            //                              pool.Description.Add(Languages.en, "Hello world!").
            //                                               Add(Languages.de, "Hallo Welt!");

            //                              pool.LocationLanguage         = Languages.de;

            //                              pool.PoolLocation             = new GeoCoordinate(new Latitude (50.916887), new Longitude(11.580169));

            //                              pool.Address.Street           = "Leutragraben";
            //                              pool.Address.HouseNumber      = "1";
            //                              pool.Address.FloorLevel       = "12";
            //                              pool.Address.PostalCode       = "07743";
            //                              pool.Address.City             = "Jena";
            //                              pool.Address.Country          = Country.Germany;

            //                              //pool.OpeningTime

            //                              #region Create a new charging station

            //                              pool.CreateNewStation(ChargingStation_Id.Parse("vStation01"),
            //                                                    station => {

            //                                                        station.GeoLocation  = new GeoCoordinate(new Latitude(50.92), new Longitude(11.59));

            //                                                        station.ServiceProviderComment.Add(Languages.en, "Hello World (1)!");

            //                                                        station.CreateNewEVSE(
            //                                                            EVSE_Id.Parse(BelectricDriveEVSEOperatorId, "4201*1"),
            //                                                            EVSE => {
            //                                                                EVSE.CreateNewSocketOutlet(SocketOutlet_Id.Parse("1"),
            //                                                                    socket => {
            //                                                                        socket.GuranteedMinPower  = 3600;
            //                                                                        socket.MaxPower           = 3600;
            //                                                                        socket.Plug               = PlugType.SCHUKO;
            //                                                                    });
            //                                                            });

            //                                                        station.CreateNewEVSE(
            //                                                            EVSE_Id.Parse(BelectricDriveEVSEOperatorId, "4201*2"),
            //                                                            EVSE => {
            //                                                                EVSE.CreateNewSocketOutlet(SocketOutlet_Id.Parse("1"),
            //                                                                    socket => {
            //                                                                        socket.GuranteedMinPower  = 11000;
            //                                                                        socket.MaxPower           = 11000;
            //                                                                        socket.Plug               = PlugType.Mennekes_Type_2;
            //                                                                    });
            //                                                            });

            //                                                    });

            //                              #endregion

            //                              #region Create a new charging station

            //                              pool.CreateNewStation(ChargingStation_Id.Parse("vStation02"),
            //                                                    station => {

            //                                                        station.GeoLocation = new GeoCoordinate(new Latitude(50.91), new Longitude(11.58));

            //                                                        station.ServiceProviderComment.Add(Languages.en, "Hello World (2)!");

            //                                                        station.CreateNewEVSE(
            //                                                            EVSE_Id.Parse(BelectricDriveEVSEOperatorId, "4202*1"),
            //                                                            EVSE => {
            //                                                                EVSE.CreateNewSocketOutlet(SocketOutlet_Id.Parse("1"),
            //                                                                    socket => {
            //                                                                        socket.GuranteedMinPower  = 3600;
            //                                                                        socket.MaxPower           = 3600;
            //                                                                        socket.Plug               = PlugType.SCHUKO;
            //                                                                    });
            //                                                            });

            //                                                        station.CreateNewEVSE(
            //                                                            EVSE_Id.Parse(BelectricDriveEVSEOperatorId, "4202*2"),
            //                                                            EVSE => {
            //                                                                EVSE.CreateNewSocketOutlet(SocketOutlet_Id.Parse("1"),
            //                                                                    socket => {
            //                                                                        socket.GuranteedMinPower  = 11000;
            //                                                                        socket.MaxPower           = 11000;
            //                                                                        socket.Plug               = PlugType.Mennekes_Type_2;
            //                                                                    });
            //                                                            });

            //                                                    });

            //                              #endregion

            //                          });

            //#endregion

        }

        #endregion

    }

}
