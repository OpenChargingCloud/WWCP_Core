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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP.tests.RoamingNetwork.Ids
{

    /// <summary>
    /// Unit tests for charging station operator identifications.
    /// </summary>
    [TestFixture]
    public class ChargingStationOperatorIdTests
    {

        #region Parse_Test1()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void Parse_Test1()
        {
            var csoId = ChargingStationOperator_Id.Parse("DEGEF");
            Assert.That(csoId.ToString(), Is.EqualTo("DEGEF"));
            Assert.That(csoId.Length, Is.EqualTo(5));
        }

        #endregion

        #region Parse_Test2()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void Parse_Test2()
        {
            var csoId = ChargingStationOperator_Id.Parse("DE*GEF");
            Assert.That(csoId.ToString(), Is.EqualTo("DE*GEF"));
            Assert.That(csoId.Length, Is.EqualTo(6));
        }

        #endregion

        #region Parse_Test3()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void Parse_Test3()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.That(csoId.ToString(), Is.EqualTo("DE*GEF"));
            Assert.That(csoId.Length, Is.EqualTo(6));
        }

        #endregion


        #region TryParse_Test1()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParse_Test1()
        {

            var csoId = ChargingStationOperator_Id.TryParse("DEGEF");
            Assert.That(csoId, Is.Not.Null);

            if (csoId is not null)
            {
                Assert.That(csoId.Value.ToString(), Is.EqualTo("DEGEF"));
                Assert.That(csoId.Value.Length, Is.EqualTo(5));
            }

        }

        #endregion

        #region TryParse_Test2()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParse_Test2()
        {

            var csoId = ChargingStationOperator_Id.TryParse("DE*GEF");
            Assert.That(csoId, Is.Not.Null);

            if (csoId is not null)
            {
                Assert.That(csoId.Value.ToString(), Is.EqualTo("DE*GEF"));
                Assert.That(csoId.Value.Length, Is.EqualTo(6));
            }

        }

        #endregion

        #region TryParse_Test3()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParse_Test3()
        {

            var csoId = ChargingStationOperator_Id.TryParse(Country.Germany, "GEF");
            Assert.That(csoId, Is.Not.Null);

            if (csoId is not null)
            {
                Assert.That(csoId.Value.ToString(), Is.EqualTo("DE*GEF"));
                Assert.That(csoId.Value.Length, Is.EqualTo(6));
            }

        }

        #endregion


        #region TryParseOut_Test1()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test1()
        {
            Assert.That(ChargingStationOperator_Id.TryParse("DEGEF", out var csoId), Is.True);
            Assert.That(csoId.ToString(), Is.EqualTo("DEGEF"));
            Assert.That(csoId.Length, Is.EqualTo(5));
        }

        #endregion

        #region TryParseOut_Test2()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test2()
        {
            Assert.That(ChargingStationOperator_Id.TryParse("DE*GEF", out var csoId), Is.True);
            Assert.That(csoId.ToString(), Is.EqualTo("DE*GEF"));
            Assert.That(csoId.Length, Is.EqualTo(6));
        }

        #endregion

        #region TryParseOut_Test3()

        /// <summary>
        /// A test for parsing charging station operator identifications.
        /// </summary>
        [Test]
        public void TryParseOut_Test3()
        {
            Assert.That(ChargingStationOperator_Id.TryParse(Country.Germany, "GEF", out var csoId), Is.True);
            Assert.That(csoId.ToString(), Is.EqualTo("DE*GEF"));
            Assert.That(csoId.Length, Is.EqualTo(6));
        }

        #endregion


        #region Clone_Test()

        /// <summary>
        /// A test for cloning charging station operator identifications.
        /// </summary>
        [Test]
        public void ChargingStationOperator_IdChargingStationOperator_IdConstructorTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = csoId1.Clone();
            Assert.That(csoId2.ToString(), Is.EqualTo(csoId1.ToString()));
            Assert.That(csoId2.Length, Is.EqualTo(csoId1.Length));
            Assert.That(csoId2, Is.EqualTo(csoId1));
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.That(csoId == csoId, Is.True);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.That(csoId1 == csoId2, Is.True);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "234");
            Assert.That(csoId1 == csoId2, Is.False);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.That(csoId != csoId, Is.False);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.That(csoId1 != csoId2, Is.False);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1 != csoId2, Is.True);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            Assert.That(csoId1 != csoId2, Is.True);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.That(csoId < csoId, Is.False);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1 < csoId2, Is.False);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1 < csoId2, Is.True);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            Assert.That(csoId1 < csoId2, Is.True);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1 < csoId2, Is.False);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            Assert.That(csoId1 < csoId2, Is.False);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.That(csoId <= csoId, Is.True);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.That(csoId1 <= csoId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1 <= csoId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            Assert.That(csoId1 <= csoId2, Is.True);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1 <= csoId2, Is.False);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            Assert.That(csoId1 <= csoId2, Is.False);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.That(csoId > csoId, Is.False);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1 > csoId2, Is.False);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1 > csoId2, Is.False);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            Assert.That(csoId1 > csoId2, Is.False);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1 > csoId2, Is.True);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            Assert.That(csoId1 > csoId2, Is.True);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.That(csoId >= csoId, Is.True);
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
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.That(csoId1 >= csoId2, Is.True);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1 >= csoId2, Is.False);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            Assert.That(csoId1 >= csoId2, Is.False);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1 >= csoId2, Is.True);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            Assert.That(csoId1 >= csoId2, Is.True);
        }

        #endregion


        #region CompareToNonChargingStationOperator_IdTest()

        /// <summary>
        /// A test for CompareTo a non-ChargingStationOperator_Id.
        /// </summary>
        [Test]
        public void CompareToNonChargingStationOperator_IdTest()
        {

            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var text  = "DE*GEF";

            Assert.Throws<ArgumentException>(() => { var x = csoId.CompareTo(text); });

        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1.CompareTo(csoId2) < 0, Is.True);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "005");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "023");
            Assert.That(csoId1.CompareTo(csoId2) < 0, Is.True);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1.CompareTo(csoId2) == 0, Is.True);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1.CompareTo(csoId2) > 0, Is.True);
        }

        #endregion


        #region EqualsNonChargingStationOperator_IdTest()

        /// <summary>
        /// A test for equals a non-ChargingStationOperator_Id.
        /// </summary>
        [Test]
        public void EqualsNonChargingStationOperator_IdTest()
        {
            var csoId = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var text  = "DE*GEF";
            Assert.That(csoId.Equals(text), Is.False);
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            Assert.That(csoId1.Equals(csoId2), Is.True);
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var csoId1 = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var csoId2 = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            Assert.That(csoId1.Equals(csoId2), Is.False);
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var hashCode1 = ChargingStationOperator_Id.Parse(Country.Germany, "555").GetHashCode();
            var hashCode2 = ChargingStationOperator_Id.Parse(Country.Germany, "555").GetHashCode();
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
            var hashCode1 = ChargingStationOperator_Id.Parse(Country.Germany, "001").GetHashCode();
            var hashCode2 = ChargingStationOperator_Id.Parse(Country.Germany, "002").GetHashCode();
            Assert.That(hashCode2, Is.Not.EqualTo(hashCode1));
        }

        #endregion


        #region ChargingStationOperator_IdsAndNUnitTest()

        /// <summary>
        /// Tests ChargingStationOperator_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void ChargingStationOperator_IdsAndNUnitTest()
        {

            var a = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var b = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var c = ChargingStationOperator_Id.Parse(Country.Germany, "111");

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

        #region ChargingStationOperator_IdsInHashSetTest()

        /// <summary>
        /// Test ChargingStationOperator_Ids within a HashSet.
        /// </summary>
        [Test]
        public void ChargingStationOperator_IdsInHashSetTest()
        {

            var a = ChargingStationOperator_Id.Parse(Country.Germany, "111");
            var b = ChargingStationOperator_Id.Parse(Country.Germany, "222");
            var c = ChargingStationOperator_Id.Parse(Country.Germany, "111");

            var _HashSet = new HashSet<ChargingStationOperator_Id>();
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
