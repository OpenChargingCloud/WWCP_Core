/*
 * Copyright (c) 2014-2022 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.WWCP.UnitTests
{

    /// <summary>
    /// Unit tests for the EVSE operator identifications.
    /// </summary>
    [TestFixture]
    public class EVSEOperator_IdTests
    {

        #region EVSEOperator_IdStringConstructorTest()

        /// <summary>
        /// A test for the EVSEOperator_Id string constructor.
        /// </summary>
        [Test]
        public void EVSEOperator_IdStringConstructorTest()
        {
            var evseOperator_Id = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.AreEqual("GEF", evseOperator_Id.ToString());
            Assert.AreEqual(3,     evseOperator_Id.Length);
        }

        #endregion

        #region EVSEOperator_IdEVSEOperator_IdConstructorTest()

        /// <summary>
        /// A test for the EVSEOperator_Id EVSEOperator_Id constructor.
        /// </summary>
        [Test]
        public void EVSEOperator_IdEVSEOperator_IdConstructorTest()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var evseOperator_Id2 = evseOperator_Id1.Clone;
            Assert.AreEqual(evseOperator_Id1.ToString(), evseOperator_Id2.ToString());
            Assert.AreEqual(evseOperator_Id1.Length,     evseOperator_Id2.Length);
            Assert.AreEqual(evseOperator_Id1,            evseOperator_Id2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]

        public void op_Equality_SameReference_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.IsTrue(evseOperator_Id1 == evseOperator_Id1);
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
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.IsTrue(evseOperator_Id1 == evseOperator_Id2);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "234");
            Assert.IsFalse(evseOperator_Id1 == evseOperator_Id2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.IsFalse(evseOperator_Id1 != evseOperator_Id1);
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
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.IsFalse(evseOperator_Id1 != evseOperator_Id2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsTrue(evseOperator_Id1 != evseOperator_Id2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            Assert.IsTrue(evseOperator_Id1 != evseOperator_Id2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.IsFalse(evseOperator_Id1 < evseOperator_Id1);
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
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsFalse(evseOperator_Id1 < evseOperator_Id2);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsTrue(evseOperator_Id1 < evseOperator_Id2);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            Assert.IsTrue(evseOperator_Id1 < evseOperator_Id2);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsFalse(evseOperator_Id1 < evseOperator_Id2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            Assert.IsFalse(evseOperator_Id1 < evseOperator_Id2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.IsTrue(evseOperator_Id1 <= evseOperator_Id1);
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
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.IsTrue(evseOperator_Id1 <= evseOperator_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsTrue(evseOperator_Id1 <= evseOperator_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            Assert.IsTrue(evseOperator_Id1 <= evseOperator_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsFalse(evseOperator_Id1 <= evseOperator_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            Assert.IsFalse(evseOperator_Id1 <= evseOperator_Id2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.IsFalse(evseOperator_Id1 > evseOperator_Id1);
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
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsFalse(evseOperator_Id1 > evseOperator_Id2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsFalse(evseOperator_Id1 > evseOperator_Id2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            Assert.IsFalse(evseOperator_Id1 > evseOperator_Id2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsTrue(evseOperator_Id1 > evseOperator_Id2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            Assert.IsTrue(evseOperator_Id1 > evseOperator_Id2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            #pragma warning disable
            Assert.IsTrue(evseOperator_Id1 >= evseOperator_Id1);
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
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            Assert.IsTrue(evseOperator_Id1 >= evseOperator_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsFalse(evseOperator_Id1 >= evseOperator_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            Assert.IsFalse(evseOperator_Id1 >= evseOperator_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsTrue(evseOperator_Id1 >= evseOperator_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            Assert.IsTrue(evseOperator_Id1 >= evseOperator_Id2);
        }

        #endregion


        #region CompareToNonEVSEOperator_IdTest()

        /// <summary>
        /// A test for CompareTo a non-EVSEOperator_Id.
        /// </summary>
        [Test]
        public void CompareToNonEVSEOperator_IdTest()
        {
            var evseOperator_Id = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var _Object   = "GEF";
            Assert.Throws<ArgumentNullException>(() => { var x = evseOperator_Id.CompareTo(_Object); });
        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsTrue(evseOperator_Id1.CompareTo(evseOperator_Id2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "5");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "23");
            Assert.IsTrue(evseOperator_Id1.CompareTo(evseOperator_Id2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsTrue(evseOperator_Id1.CompareTo(evseOperator_Id2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsTrue(evseOperator_Id1.CompareTo(evseOperator_Id2) > 0);
        }

        #endregion


        #region EqualsNonEVSEOperator_IdTest()

        /// <summary>
        /// A test for equals a non-EVSEOperator_Id.
        /// </summary>
        [Test]
        public void EqualsNonEVSEOperator_IdTest()
        {
            var evseOperator_Id = ChargingStationOperator_Id.Parse(Country.Germany, "GEF");
            var _Object          = "DE*123";
            Assert.IsFalse(evseOperator_Id.Equals(_Object));
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            Assert.IsTrue(evseOperator_Id1.Equals(evseOperator_Id2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var evseOperator_Id1 = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var evseOperator_Id2 = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            Assert.IsFalse(evseOperator_Id1.Equals(evseOperator_Id2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var _SensorHashCode1 = ChargingStationOperator_Id.Parse(Country.Germany, "5").GetHashCode();
            var _SensorHashCode2 = ChargingStationOperator_Id.Parse(Country.Germany, "5").GetHashCode();
            Assert.AreEqual(_SensorHashCode1, _SensorHashCode2);
        }

        #endregion

        #region GetHashCodeNotEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeNotEqualTest()
        {
            var _SensorHashCode1 = ChargingStationOperator_Id.Parse(Country.Germany, "1").GetHashCode();
            var _SensorHashCode2 = ChargingStationOperator_Id.Parse(Country.Germany, "2").GetHashCode();
            Assert.AreNotEqual(_SensorHashCode1, _SensorHashCode2);
        }

        #endregion


        #region EVSEOperator_IdsAndNUnitTest()

        /// <summary>
        /// Tests EVSEOperator_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void EVSEOperator_IdsAndNUnitTest()
        {

            var a = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var b = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var c = ChargingStationOperator_Id.Parse(Country.Germany, "1");

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);
            Assert.AreEqual(c, c);

            Assert.AreEqual(a, c);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);

        }

        #endregion

        #region EVSEOperator_IdsInHashSetTest()

        /// <summary>
        /// Test EVSEOperator_Ids within a HashSet.
        /// </summary>
        [Test]
        public void EVSEOperator_IdsInHashSetTest()
        {

            var a = ChargingStationOperator_Id.Parse(Country.Germany, "1");
            var b = ChargingStationOperator_Id.Parse(Country.Germany, "2");
            var c = ChargingStationOperator_Id.Parse(Country.Germany, "1");

            var _HashSet = new HashSet<ChargingStationOperator_Id>();
            Assert.AreEqual(0, _HashSet.Count);

            _HashSet.Add(a);
            Assert.AreEqual(1, _HashSet.Count);

            _HashSet.Add(b);
            Assert.AreEqual(2, _HashSet.Count);

            _HashSet.Add(c);
            Assert.AreEqual(2, _HashSet.Count);

        }

        #endregion

    }

}
