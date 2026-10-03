/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

#region Usings

using NUnit.Framework;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Ids
{

    /// <summary>
    /// Unit tests for roaming network identifications.
    /// </summary>
    [TestFixture]
    public class RoamingNetworkIdTests
    {

        #region Parse_Test()

        /// <summary>
        /// A test for parsing roaming network identifications.
        /// </summary>
        [Test]
        public void Parse_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            Assert.That(roamingNetworkId.ToString(), Is.EqualTo("TEST"));
            Assert.That(roamingNetworkId.Length, Is.EqualTo(4));
        }

        #endregion

        #region TryParse_Test()

        /// <summary>
        /// A test for parsing roaming network identifications.
        /// </summary>
        [Test]
        public void TryParse_Test()
        {

            var roamingNetworkId = RoamingNetwork_Id.TryParse("TEST");
            Assert.That(roamingNetworkId, Is.Not.Null);

            if (roamingNetworkId is not null)
            {
                Assert.That(roamingNetworkId.Value.ToString(), Is.EqualTo("TEST"));
                Assert.That(roamingNetworkId.Value.Length, Is.EqualTo(4));
            }

        }

        #endregion

        #region TryParseOut_Test()

        /// <summary>
        /// A test for parsing roaming network identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test()
        {
            Assert.That(RoamingNetwork_Id.TryParse("TEST", out var roamingNetworkId), Is.True);
            Assert.That(roamingNetworkId.ToString(), Is.EqualTo("TEST"));
            Assert.That(roamingNetworkId.Length, Is.EqualTo(4));
        }

        #endregion


        #region Clone_Test()

        /// <summary>
        /// A test for cloning charging station operator identifications.
        /// </summary>
        [Test]
        public void Clone_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = roamingNetworkId1.Clone();
            Assert.That(roamingNetworkId2.ToString(), Is.EqualTo(roamingNetworkId1.ToString()));
            Assert.That(roamingNetworkId2.Length, Is.EqualTo(roamingNetworkId1.Length));
            Assert.That(roamingNetworkId2, Is.EqualTo(roamingNetworkId1));
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.That(roamingNetworkId == roamingNetworkId, Is.True);
            #pragma warning restore
        }

        #endregion

        #region op_Equality_Equals_Test()

        /// <summary>
        /// A test for the equality operator equals.
        /// </summary>
        [Test]
        public void op_Equality_Equals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.That(roamingNetworkId1 == roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("234");
            Assert.That(roamingNetworkId1 == roamingNetworkId2, Is.False);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.That(roamingNetworkId != roamingNetworkId, Is.False);
            #pragma warning restore
        }

        #endregion

        #region op_Inequality_Equals_Test()

        /// <summary>
        /// A test for the inequality operator equals.
        /// </summary>
        [Test]
        public void op_Inequality_Equals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.That(roamingNetworkId1 != roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1 != roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.That(roamingNetworkId1 != roamingNetworkId2, Is.True);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.That(roamingNetworkId < roamingNetworkId, Is.False);
            #pragma warning restore
        }

        #endregion

        #region op_Smaller_Equals_Test()

        /// <summary>
        /// A test for the smaller operator equals.
        /// </summary>
        [Test]
        public void op_Smaller_Equals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1 < roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1 < roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.That(roamingNetworkId1 < roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1 < roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.That(roamingNetworkId1 < roamingNetworkId2, Is.False);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.That(roamingNetworkId <= roamingNetworkId, Is.True);
            #pragma warning restore
        }

        #endregion

        #region op_SmallerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Equals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.That(roamingNetworkId1 <= roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1 <= roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.That(roamingNetworkId1 <= roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1 <= roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.That(roamingNetworkId1 <= roamingNetworkId2, Is.False);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.That(roamingNetworkId > roamingNetworkId, Is.False);
            #pragma warning restore
        }

        #endregion

        #region op_Bigger_Equals_Test()

        /// <summary>
        /// A test for the bigger operator equals.
        /// </summary>
        [Test]
        public void op_Bigger_Equals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1 > roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1 > roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.That(roamingNetworkId1 > roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1 > roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.That(roamingNetworkId1 > roamingNetworkId2, Is.True);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var roamingNetworkId = RoamingNetwork_Id.Parse("TEST");
            #pragma warning disable
            Assert.That(roamingNetworkId >= roamingNetworkId, Is.True);
            #pragma warning restore
        }

        #endregion

        #region op_BiggerOrEqual_Equals_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Equals_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("TEST");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("TEST");
            Assert.That(roamingNetworkId1 >= roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1 >= roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.That(roamingNetworkId1 >= roamingNetworkId2, Is.False);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1 >= roamingNetworkId2, Is.True);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("023");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("005");
            Assert.That(roamingNetworkId1 >= roamingNetworkId2, Is.True);
        }

        #endregion


        #region CompareToNonRoamingNetworkIdTest()

        /// <summary>
        /// A test for CompareTo a non-RoamingNetworkId.
        /// </summary>
        [Test]
        public void CompareToNonRoamingNetworkIdTest()
        {

            var roamingNetworkId  = RoamingNetwork_Id.Parse("TEST");
            var text              = "TEST";

            Assert.Throws<ArgumentException>(() => { var x = roamingNetworkId.CompareTo(text); });

        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1.CompareTo(roamingNetworkId2) < 0, Is.True);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("005");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("023");
            Assert.That(roamingNetworkId1.CompareTo(roamingNetworkId2) < 0, Is.True);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1.CompareTo(roamingNetworkId2) == 0, Is.True);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("222");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1.CompareTo(roamingNetworkId2) > 0, Is.True);
        }

        #endregion


        #region EqualsNonRoamingNetworkIdTest()

        /// <summary>
        /// A test for equals a non-RoamingNetworkId.
        /// </summary>
        [Test]
        public void EqualsNonRoamingNetworkIdTest()
        {
            var roamingNetworkId  = RoamingNetwork_Id.Parse("TEST");
            var text              = "TEST";
            Assert.That(roamingNetworkId.Equals(text), Is.False);
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("111");
            Assert.That(roamingNetworkId1.Equals(roamingNetworkId2), Is.True);
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var roamingNetworkId1 = RoamingNetwork_Id.Parse("111");
            var roamingNetworkId2 = RoamingNetwork_Id.Parse("222");
            Assert.That(roamingNetworkId1.Equals(roamingNetworkId2), Is.False);
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = RoamingNetwork_Id.Parse("TEST").GetHashCode();
            var hashCode2 = RoamingNetwork_Id.Parse("TEST").GetHashCode();
            Assert.That(hashCode2, Is.EqualTo(hashCode1));
        }

        #endregion

        #region GetHashCodeNotEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeNotEqualTest()
        {
            var hashCode1 = RoamingNetwork_Id.Parse("TEST1").GetHashCode();
            var hashCode2 = RoamingNetwork_Id.Parse("TEST2").GetHashCode();
            Assert.That(hashCode2, Is.Not.EqualTo(hashCode1));
        }

        #endregion


        #region RoamingNetworkIdsAndNUnitTest()

        /// <summary>
        /// Tests RoamingNetworkIds in combination with NUnit.
        /// </summary>
        [Test]
        public void RoamingNetworkIdsAndNUnitTest()
        {

            var a = RoamingNetwork_Id.Parse("111");
            var b = RoamingNetwork_Id.Parse("222");
            var c = RoamingNetwork_Id.Parse("111");

            // Each id equals itself: what Equals is asked here, on purpose.
#pragma warning disable NUnit2009
            Assert.That(a, Is.EqualTo(a));
            Assert.That(b, Is.EqualTo(b));
            Assert.That(c, Is.EqualTo(c));
#pragma warning restore NUnit2009

            Assert.That(c, Is.EqualTo(a));
            Assert.That(b, Is.Not.EqualTo(a));
            Assert.That(c, Is.Not.EqualTo(b));

        }

        #endregion

        #region RoamingNetworkIdsInHashSetTest()

        /// <summary>
        /// Test RoamingNetworkIds within a HashSet.
        /// </summary>
        [Test]
        public void RoamingNetworkIdsInHashSetTest()
        {

            var a = RoamingNetwork_Id.Parse("111");
            var b = RoamingNetwork_Id.Parse("222");
            var c = RoamingNetwork_Id.Parse("111");

            var _HashSet = new HashSet<RoamingNetwork_Id>();
            Assert.That(_HashSet.Count, Is.EqualTo(0));

            _HashSet.Add(a);
            Assert.That(_HashSet.Count, Is.EqualTo(1));

            _HashSet.Add(b);
            Assert.That(_HashSet.Count, Is.EqualTo(2));

            _HashSet.Add(c);
            Assert.That(_HashSet.Count, Is.EqualTo(2));

        }

        #endregion


    }

}
