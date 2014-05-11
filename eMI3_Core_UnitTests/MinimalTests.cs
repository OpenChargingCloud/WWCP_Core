/*
 * Copyright (c) 2013 Achim Friedland <achim.friedland@graphdefined.com>
 * This file is part of eMI3 Mockup <http://www.github.com/eMI3/Mockup>
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

using org.emi3group;

#endregion

namespace org.emi3group.UnitTests
{

    /// <summary>
    /// Minimal Unit tests.
    /// </summary>
    [TestFixture]
    public class MiniomalTests
    {

        #region Test1()

        [Test]
        public void Test1()
        {

            #region Create a new roaming network and an EVSE operator

            var RoamingNetwork = new RoamingNetwork(new RoamingNetwork_Id("eMI3"));

            Assert.IsNotNull(RoamingNetwork);
            Assert.AreEqual(new RoamingNetwork_Id("eMI3"), RoamingNetwork.Id);
            Assert.AreEqual("eMI3",                        RoamingNetwork.Id.ToString());
            Assert.AreEqual(0,                             RoamingNetwork.Count(),               "The number of entities within the roaming network must be 0!");
            Assert.AreEqual(0,                             RoamingNetwork.EVSEOperators.Count(), "The number of EVSE operators within the roaming network must be 0!");

            var BelectricDriveOperator = RoamingNetwork.CreateNewEVSEOperator(
                                                            new EVSEOperator_Id("822"),
                                                            Operator => {
                                                                Operator.Name.Add(Languages.de, "Belectric Drive");
                                                            }
                                                        );

            Assert.IsNotNull(BelectricDriveOperator);
            Assert.AreEqual(new EVSEOperator_Id("822"), BelectricDriveOperator.Id);
            Assert.AreEqual("822",                      BelectricDriveOperator.Id.ToString());
            Assert.AreEqual(0,                          BelectricDriveOperator.Count(),       "The number of EVSPools registered with the EVSE operator must be 0!");

            Assert.AreEqual(1,                          RoamingNetwork.Count(),               "The number of entities within the roaming network must be 1 now!");
            Assert.AreEqual(1,                          RoamingNetwork.EVSEOperators.Count(), "The number of EVSE operators within the roaming network must be 1 now!");

            #endregion

            #region Create an EVSE pool

            var BITSPool = BelectricDriveOperator.CreateNewEVSPool(
                                      new ChargingPool_Id("BITS"),
                                      pool => {

                                          pool.Name.       Add(Languages.en, "Belectric IT Solutions").
                                                           Add(Languages.de, "Belectric IT Solutions GmbH");

                                          pool.Description.Add(Languages.en, "Hello world!").
                                                           Add(Languages.de, "Hallo Welt!");

                                          pool.LocationLanguage         = Languages.de;

                                          pool.PoolLocation.Latitude    = 50.916887;
                                          pool.PoolLocation.Longitude   = 11.580169;

                                          pool.Address.Street           = "Leutragraben";
                                          pool.Address.HouseNumber      = "1";
                                          pool.Address.FloorLevel       = "12";
                                          pool.Address.PostalCode       = "07743";
                                          pool.Address.City             = "Jena";
                                          pool.Address.Country          = Country.Germany;

                                          //pool.OpeningTime

                                          #region Create a new charging station

                                          pool.CreateNewStation(new ChargingStation_Id("vStation01"),
                                                                station => {

                                                                    station.GeoLocation.Latitude   = 50.92;
                                                                    station.GeoLocation.Longitude  = 11.59;

                                                                    station.ServiceProviderComment.Add(Languages.en, "Hello World (1)!");

                                                                    station.CreateNewEVSE(
                                                                        new EVSE_Id("49*822*4201*1"),
                                                                        EVSE => {
                                                                            EVSE.CreateNewSocketOutlet(new SocketOutlet_Id("1"),
                                                                                socket => {
                                                                                    socket.GuranteedMinPower  = 3600;
                                                                                    socket.MaxPower           = 3600;
                                                                                    socket.Plug               = PlugType.SCHUKO;
                                                                                });
                                                                        });

                                                                    station.CreateNewEVSE(
                                                                        new EVSE_Id("49*822*4201*2"),
                                                                        EVSE => {
                                                                            EVSE.CreateNewSocketOutlet(new SocketOutlet_Id("1"),
                                                                                socket => {
                                                                                    socket.GuranteedMinPower  = 11000;
                                                                                    socket.MaxPower           = 11000;
                                                                                    socket.Plug               = PlugType.Mennekes_Type_2;
                                                                                });
                                                                        });

                                                                });

                                          #endregion

                                          #region Create a new charging station

                                          pool.CreateNewStation(new ChargingStation_Id("vStation02"),
                                                                station => {

                                                                    station.GeoLocation.Latitude   = 50.91;
                                                                    station.GeoLocation.Longitude  = 11.58;

                                                                    station.ServiceProviderComment.Add(Languages.en, "Hello World (2)!");

                                                                    station.CreateNewEVSE(
                                                                        new EVSE_Id("49*822*4202*1"),
                                                                        EVSE => {
                                                                            EVSE.CreateNewSocketOutlet(new SocketOutlet_Id("1"),
                                                                                socket => {
                                                                                    socket.GuranteedMinPower  = 3600;
                                                                                    socket.MaxPower           = 3600;
                                                                                    socket.Plug               = PlugType.SCHUKO;
                                                                                });
                                                                        });

                                                                    station.CreateNewEVSE(
                                                                        new EVSE_Id("49*822*4202*2"),
                                                                        EVSE => {
                                                                            EVSE.CreateNewSocketOutlet(new SocketOutlet_Id("1"),
                                                                                socket => {
                                                                                    socket.GuranteedMinPower  = 11000;
                                                                                    socket.MaxPower           = 11000;
                                                                                    socket.Plug               = PlugType.Mennekes_Type_2;
                                                                                });
                                                                        });

                                                                });

                                          #endregion

                                      });

            #endregion

        }

        #endregion

    }

}
