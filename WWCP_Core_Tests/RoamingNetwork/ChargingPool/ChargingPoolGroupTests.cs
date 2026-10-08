/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using NUnit.Framework;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork
{

    /// <summary>
    /// Unit tests for charging pool groups: DE*GEF has the charging pool
    /// P0001 with the charging station AAAA and its EVSE 1, and P0002 and
    /// P0003 without any charging station.
    /// </summary>
    [TestFixture]
    public class ChargingPoolGroupTests : AEVSETests
    {

        #region Data

        private IChargingPool? DE_GEF_P0002;
        private IChargingPool? DE_GEF_P0003;

        private IChargingStationOperator  Operator  => DE_GEF       ?? throw new InvalidOperationException("No charging station operator!");
        private IChargingPool             P1        => DE_GEF_P0001 ?? throw new InvalidOperationException("No charging pool P0001!");
        private IChargingPool             P2        => DE_GEF_P0002 ?? throw new InvalidOperationException("No charging pool P0002!");
        private IChargingPool             P3        => DE_GEF_P0003 ?? throw new InvalidOperationException("No charging pool P0003!");

        #endregion

        #region SetupEachTest()

        [SetUp]
        public override void SetupEachTest()
        {

            base.SetupEachTest();

            DE_GEF_P0002 = AddPool("0002");
            DE_GEF_P0003 = AddPool("0003");

            Assert.That(DE_GEF_P0002, Is.Not.Null);
            Assert.That(DE_GEF_P0003, Is.Not.Null);

        }

        private IChargingPool? AddPool(String Suffix)

            => DE_GEF?.AddChargingPool(
                   Id:                  ChargingPool_Id.Parse(DE_GEF.Id, Suffix),
                   Name:                I18NString.Create(Languages.de, "GraphDefined Charging Pool #" + Suffix),
                   InitialAdminStatus:  ChargingPoolAdminStatusType.OutOfService,
                   InitialStatus:       ChargingPoolStatusType.Offline
               ).Result.ChargingPool;

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public override void ShutdownEachTest()
        {

            base.ShutdownEachTest();

            DE_GEF_P0002 = null;
            DE_GEF_P0003 = null;

        }

        #endregion


        #region (private) NewGroup(Suffix, ...)

        private ChargingPoolGroup NewGroup(String                         Suffix,
                                           IEnumerable<IChargingPool>?    Members            = null,
                                           IEnumerable<ChargingPool_Id>?  MemberIds          = null,
                                           Func<IChargingPool, Boolean>?  AutoIncludePools   = null)

            => new (ChargingPoolGroup_Id.Parse(Operator.Id, Suffix),
                    Operator,
                    I18NString.Create(Languages.de, "Group " + Suffix),
                    Members:           Members,
                    MemberIds:         MemberIds,
                    AutoIncludePools:  AutoIncludePools);

        #endregion


        #region AGroupIdIsWrittenWithGPAndReadBack()

        [Test]
        public void AGroupIdIsWrittenWithGPAndReadBack()
        {

            var groupId = ChargingPoolGroup_Id.Parse(Operator.Id, "city");

            Assert.That(groupId.ToString(),                                            Is.EqualTo("DE*GEF*GPcity"));
            Assert.That(ChargingPoolGroup_Id.Parse(groupId.ToString()),                Is.EqualTo(groupId));
            Assert.That(ChargingPoolGroup_Id.TryParse("DE*GEF*GScity", out _),         Is.False, "a charging station group's identification");
            Assert.That(ChargingStationGroup_Id.TryParse(groupId.ToString(), out _),   Is.False, "read as a charging station group's identification");

        }

        #endregion

        #region AGroupWithoutMemberIdsTakesEveryPool()

        [Test]
        public void AGroupWithoutMemberIdsTakesEveryPool()
        {

            var group = NewGroup("all");

            Assert.That(group.ChargingPools,       Is.Empty);
            Assert.That(group.Add(P1),             Is.True);
            Assert.That(group.Add(P2),             Is.True);

            Assert.That(group.ChargingPoolIds,     Is.EquivalentTo(new[] { P1.Id, P2.Id }));
            Assert.That(group,                     Is.EquivalentTo(new[] { P1,    P2    }));
            Assert.That(group.ChargingStationIds,  Is.EqualTo(new[] { DE_GEF_S0001_AAAA!.Id }));
            Assert.That(group.EVSEIds,             Is.EqualTo(new[] { DE_GEF_E0001_AAAA_1!.Id }));

        }

        #endregion

        #region AGroupWithMemberIdsTakesThoseOnly()

        [Test]
        public void AGroupWithMemberIdsTakesThoseOnly()
        {

            var group = NewGroup("some", MemberIds: [ P1.Id ]);

            Assert.That(group.AllowedMemberIds,    Is.EqualTo(new[] { P1.Id }));
            Assert.That(group.Add(P2),             Is.False);
            Assert.That(group.Add(P1),             Is.True);
            Assert.That(group.ChargingPoolIds,     Is.EqualTo(new[] { P1.Id }));

            group.Add(P2.Id);

            Assert.That(group.Add(P2),             Is.True);
            Assert.That(group.ChargingPoolIds,     Is.EquivalentTo(new[] { P1.Id, P2.Id }));

        }

        #endregion

        #region MembersAreInTheGroupFromItsStart()

        [Test]
        public void MembersAreInTheGroupFromItsStart()
        {

            var group = NewGroup("start", Members: [ P1 ]);

            Assert.That(group.Contains(P1.Id),     Is.True);
            Assert.That(group.AllowedMemberIds,    Is.EqualTo(new[] { P1.Id }));
            Assert.That(group.Add(P2),             Is.False, "no other pool comes in by itself");

        }

        #endregion

        #region AutoIncludePoolsDecidesForPoolsNotNamed()

        [Test]
        public void AutoIncludePoolsDecidesForPoolsNotNamed()
        {

            var p2    = P2;
            var group = NewGroup("auto",
                                 MemberIds:         [ P1.Id ],
                                 AutoIncludePools:  pool => pool.Id == p2.Id);

            Assert.That(group.Add(P3),             Is.False);
            Assert.That(group.Add(P2),             Is.True);
            Assert.That(group.Add(P1),             Is.True, "named in MemberIds");

        }

        #endregion

        #region RemovedPoolsAreOutAndStayOut()

        [Test]
        public void RemovedPoolsAreOutAndStayOut()
        {

            var group = NewGroup("remove", Members: [ P1 ]);

            Assert.That(group.Remove(P1.Id),       Is.True);
            Assert.That(group.Remove(P1.Id),       Is.False);
            Assert.That(group.Contains(P1.Id),     Is.False);
            Assert.That(group.AllowedMemberIds,    Is.Empty);
            Assert.That(group.Add(P1),             Is.False);

        }

        #endregion

        #region ItsJSONNamesItsPoolsAndEVSEs()

        [Test]
        public void ItsJSONNamesItsPoolsAndEVSEs()
        {

            var json      = NewGroup("json", Members: [ P2, P1 ]).ToJSON();
            var expanded  = NewGroup("json", Members: [ P2, P1 ]).ToJSON(ExpandChargingPoolIds: InfoStatus.Expanded);

            Assert.That(json,                                                 Is.Not.Null);
            Assert.That(json?["@id"]?.Value<String>(),                        Is.EqualTo("DE*GEF*GPjson"));
            Assert.That(json?["chargingStationOperatorId"]?.Value<String>(),  Is.EqualTo(Operator.Id.ToString()));
            Assert.That(json?["roamingNetworkId"]?.Value<String>(),           Is.EqualTo(roamingNetwork!.Id.ToString()));
            Assert.That(json?["chargingPoolIds"]?.Values<String>(),           Is.EqualTo(new[] { "DE*GEF*P0001", "DE*GEF*P0002" }));
            Assert.That(json?["EVSEIds"]?.Values<String>(),                   Is.EqualTo(new[] { DE_GEF_E0001_AAAA_1!.Id.ToString() }));

            Assert.That(expanded?["chargingPoolIds"],                         Is.Null);
            Assert.That(expanded?["chargingPools"]?.Count(),                  Is.EqualTo(2));

            Assert.That(NewGroup("empty").ToJSON()?["chargingPoolIds"],       Is.Null, "no pools, no list");

        }

        #endregion

        #region GroupsAreEqualAndOrderedByTheirIds()

        [Test]
        public void GroupsAreEqualAndOrderedByTheirIds()
        {

            var a1 = NewGroup("a");
            var a2 = NewGroup("a", Members: [ P1 ]);
            var b  = NewGroup("b");

            Assert.That(a1 == a2,                   Is.True);
            Assert.That(a1.Equals(a2),              Is.True);
            Assert.That(a1.Equals((Object) a2),     Is.True);
            Assert.That(a1.GetHashCode(),           Is.EqualTo(a2.GetHashCode()));
            Assert.That(a1 != b,                    Is.True);
            Assert.That(a1 <  b,                    Is.True);
            Assert.That(b  >  a2,                   Is.True);
            Assert.That(a1.CompareTo(a2),           Is.Zero);
            Assert.That(() => a1.CompareTo("a"),    Throws.ArgumentException);
            Assert.That(a1.Equals(null),            Is.False);

        }

        #endregion

    }

}
