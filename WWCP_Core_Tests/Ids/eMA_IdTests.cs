﻿/*
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

using System;
using System.Collections.Generic;

using NUnit.Framework;

using cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests
{

    /// <summary>
    /// Unit tests for the eMobilityAccount_Id class.
    /// </summary>
    [TestFixture]
    public class eMobilityAccount_IdTests
    {

        #region eMobilityAccount_IdEmptyConstructorTest()

        /// <summary>
        /// A test for an empty eMobilityAccount_Id constructor.
        /// </summary>
        [Test]
        public void eMobilityAccount_IdEmptyConstructorTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("");
            Assert.IsTrue(_eMobilityAccount_Id1.Length > 0);
            Assert.IsTrue(_eMobilityAccount_Id2.Length > 0);
            Assert.AreNotEqual(_eMobilityAccount_Id1, _eMobilityAccount_Id2);
        }

        #endregion

        #region eMobilityAccount_IdStringConstructorTest()

        /// <summary>
        /// A test for the eMobilityAccount_Id string constructor.
        /// </summary>
        [Test]
        public void eMobilityAccount_IdStringConstructorTest()
        {
            var _eMobilityAccount_Id = eMobilityAccount_Id.Parse("123");
            Assert.AreEqual("123", _eMobilityAccount_Id.ToString());
            Assert.AreEqual(3,     _eMobilityAccount_Id.Length);
        }

        #endregion

        #region eMobilityAccount_IdeMobilityAccount_IdConstructorTest()

        /// <summary>
        /// A test for the eMobilityAccount_Id eMobilityAccount_Id constructor.
        /// </summary>
        [Test]
        public void eMobilityAccount_IdeMobilityAccount_IdConstructorTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            var _eMobilityAccount_Id2 = _eMobilityAccount_Id1.Clone;
            Assert.AreEqual(_eMobilityAccount_Id1.ToString(), _eMobilityAccount_Id2.ToString());
            Assert.AreEqual(_eMobilityAccount_Id1.Length,     _eMobilityAccount_Id2.Length);
            Assert.AreEqual(_eMobilityAccount_Id1,            _eMobilityAccount_Id2);
        }

        #endregion


        #region NeweMobilityAccount_IdMethodTest()

        /// <summary>
        /// A test for the static neweMobilityAccount_Id method.
        /// </summary>
        [Test]
        public void NeweMobilityAccount_IdMethodTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("");
            Assert.AreNotEqual(_eMobilityAccount_Id1, _eMobilityAccount_Id2);
        }

        #endregion


        #region op_Equality_SameReference_Test()

        /// <summary>
        /// A test for the equality operator same reference.
        /// </summary>
        [Test]
        
        public void op_Equality_SameReference_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            #pragma warning disable
            Assert.IsTrue(_eMobilityAccount_Id1 == _eMobilityAccount_Id1);
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
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1 == _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Equality_NotEquals_Test()

        /// <summary>
        /// A test for the equality operator not-equals.
        /// </summary>
        [Test]
        public void op_Equality_NotEquals_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsFalse(_eMobilityAccount_Id1 == _eMobilityAccount_Id2);
        }

        #endregion


        #region op_Inequality_SameReference_Test()

        /// <summary>
        /// A test for the inequality operator same reference.
        /// </summary>
        [Test]
        public void op_Inequality_SameReference_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            #pragma warning disable
            Assert.IsFalse(_eMobilityAccount_Id1 != _eMobilityAccount_Id1);
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
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsFalse(_eMobilityAccount_Id1 != _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Inequality_NotEquals1_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsTrue(_eMobilityAccount_Id1 != _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Inequality_NotEquals2_Test()

        /// <summary>
        /// A test for the inequality operator not-equals.
        /// </summary>
        [Test]
        public void op_Inequality_NotEquals2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("5");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("23");
            Assert.IsTrue(_eMobilityAccount_Id1 != _eMobilityAccount_Id2);
        }

        #endregion


        #region op_Smaller_SameReference_Test()

        /// <summary>
        /// A test for the smaller operator same reference.
        /// </summary>
        [Test]
        public void op_Smaller_SameReference_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            #pragma warning disable
            Assert.IsFalse(_eMobilityAccount_Id1 < _eMobilityAccount_Id1);
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
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsFalse(_eMobilityAccount_Id1 < _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Smaller_Smaller1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsTrue(_eMobilityAccount_Id1 < _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Smaller_Smaller2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Smaller2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("5");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("23");
            Assert.IsTrue(_eMobilityAccount_Id1 < _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Smaller_Bigger1_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("2");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsFalse(_eMobilityAccount_Id1 < _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Smaller_Bigger2_Test()

        /// <summary>
        /// A test for the smaller operator not-equals.
        /// </summary>
        [Test]
        public void op_Smaller_Bigger2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("23");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("5");
            Assert.IsFalse(_eMobilityAccount_Id1 < _eMobilityAccount_Id2);
        }

        #endregion


        #region op_SmallerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SameReference_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            #pragma warning disable
            Assert.IsTrue(_eMobilityAccount_Id1 <= _eMobilityAccount_Id1);
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
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1 <= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsTrue(_eMobilityAccount_Id1 <= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_SmallerThan2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("5");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("23");
            Assert.IsTrue(_eMobilityAccount_Id1 <= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("2");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsFalse(_eMobilityAccount_Id1 <= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_SmallerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the smallerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_SmallerOrEqual_Bigger2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("23");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("5");
            Assert.IsFalse(_eMobilityAccount_Id1 <= _eMobilityAccount_Id2);
        }

        #endregion


        #region op_Bigger_SameReference_Test()

        /// <summary>
        /// A test for the bigger operator same reference.
        /// </summary>
        [Test]
        public void op_Bigger_SameReference_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            #pragma warning disable
            Assert.IsFalse(_eMobilityAccount_Id1 > _eMobilityAccount_Id1);
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
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsFalse(_eMobilityAccount_Id1 > _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Bigger_Smaller1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsFalse(_eMobilityAccount_Id1 > _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Bigger_Smaller2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Smaller2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("5");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("23");
            Assert.IsFalse(_eMobilityAccount_Id1 > _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Bigger_Bigger1_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("2");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1 > _eMobilityAccount_Id2);
        }

        #endregion

        #region op_Bigger_Bigger2_Test()

        /// <summary>
        /// A test for the bigger operator not-equals.
        /// </summary>
        [Test]
        public void op_Bigger_Bigger2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("23");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("5");
            Assert.IsTrue(_eMobilityAccount_Id1 > _eMobilityAccount_Id2);
        }

        #endregion


        #region op_BiggerOrEqual_SameReference_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator same reference.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SameReference_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("");
            #pragma warning disable
            Assert.IsTrue(_eMobilityAccount_Id1 >= _eMobilityAccount_Id1);
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
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1 >= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsFalse(_eMobilityAccount_Id1 >= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_SmallerThan2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_SmallerThan2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("5");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("23");
            Assert.IsFalse(_eMobilityAccount_Id1 >= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger1_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger1_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("2");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1 >= _eMobilityAccount_Id2);
        }

        #endregion

        #region op_BiggerOrEqual_Bigger2_Test()

        /// <summary>
        /// A test for the biggerOrEqual operator not-equals.
        /// </summary>
        [Test]
        public void op_BiggerOrEqual_Bigger2_Test()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("23");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("5");
            Assert.IsTrue(_eMobilityAccount_Id1 >= _eMobilityAccount_Id2);
        }

        #endregion


        #region CompareToNoneMobilityAccount_IdTest()

        /// <summary>
        /// A test for CompareTo a non-eMobilityAccount_Id.
        /// </summary>
        [Test]
        public void CompareToNoneMobilityAccount_IdTest()
        {
            var _eMobilityAccount_Id = eMobilityAccount_Id.Parse("");
            var _Object   = "123";
            Assert.Throws<ArgumentNullException>(() => { var x = _eMobilityAccount_Id.CompareTo(_Object); });
        }

        #endregion

        #region CompareToSmallerTest1()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest1()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsTrue(_eMobilityAccount_Id1.CompareTo(_eMobilityAccount_Id2) < 0);
        }

        #endregion

        #region CompareToSmallerTest2()

        /// <summary>
        /// A test for CompareTo smaller.
        /// </summary>
        [Test]
        public void CompareToSmallerTest2()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("5");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("23");
            Assert.IsTrue(_eMobilityAccount_Id1.CompareTo(_eMobilityAccount_Id2) < 0);
        }

        #endregion

        #region CompareToEqualsTest()

        /// <summary>
        /// A test for CompareTo equals.
        /// </summary>
        [Test]
        public void CompareToEqualsTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1.CompareTo(_eMobilityAccount_Id2) == 0);
        }

        #endregion

        #region CompareToBiggerTest()

        /// <summary>
        /// A test for CompareTo bigger.
        /// </summary>
        [Test]
        public void CompareToBiggerTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("2");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1.CompareTo(_eMobilityAccount_Id2) > 0);
        }

        #endregion


        #region EqualsNoneMobilityAccount_IdTest()

        /// <summary>
        /// A test for equals a non-eMobilityAccount_Id.
        /// </summary>
        [Test]
        public void EqualsNoneMobilityAccount_IdTest()
        {
            var _eMobilityAccount_Id = eMobilityAccount_Id.Parse("");
            var _Object   = "123";
            Assert.IsFalse(_eMobilityAccount_Id.Equals(_Object));
        }

        #endregion

        #region EqualsEqualsTest()

        /// <summary>
        /// A test for equals.
        /// </summary>
        [Test]
        public void EqualsEqualsTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("1");
            Assert.IsTrue(_eMobilityAccount_Id1.Equals(_eMobilityAccount_Id2));
        }

        #endregion

        #region EqualsNotEqualsTest()

        /// <summary>
        /// A test for not-equals.
        /// </summary>
        [Test]
        public void EqualsNotEqualsTest()
        {
            var _eMobilityAccount_Id1 = eMobilityAccount_Id.Parse("1");
            var _eMobilityAccount_Id2 = eMobilityAccount_Id.Parse("2");
            Assert.IsFalse(_eMobilityAccount_Id1.Equals(_eMobilityAccount_Id2));
        }

        #endregion


        #region GetHashCodeEqualTest()

        /// <summary>
        /// A test for GetHashCode
        /// </summary>
        [Test]
        public void GetHashCodeEqualTest()
        {
            var _SensorHashCode1 = eMobilityAccount_Id.Parse("5").GetHashCode();
            var _SensorHashCode2 = eMobilityAccount_Id.Parse("5").GetHashCode();
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
            var _SensorHashCode1 = eMobilityAccount_Id.Parse("1").GetHashCode();
            var _SensorHashCode2 = eMobilityAccount_Id.Parse("2").GetHashCode();
            Assert.AreNotEqual(_SensorHashCode1, _SensorHashCode2);
        }

        #endregion


        #region eMobilityAccount_IdsAndNUnitTest()

        /// <summary>
        /// Tests eMobilityAccount_Ids in combination with NUnit.
        /// </summary>
        [Test]
        public void eMobilityAccount_IdsAndNUnitTest()
        {

            var a = eMobilityAccount_Id.Parse("1");
            var b = eMobilityAccount_Id.Parse("2");
            var c = eMobilityAccount_Id.Parse("1");

            Assert.AreEqual(a, a);
            Assert.AreEqual(b, b);
            Assert.AreEqual(c, c);

            Assert.AreEqual(a, c);
            Assert.AreNotEqual(a, b);
            Assert.AreNotEqual(b, c);

        }

        #endregion

        #region eMobilityAccount_IdsInHashSetTest()

        /// <summary>
        /// Test eMobilityAccount_Ids within a HashSet.
        /// </summary>
        [Test]
        public void eMobilityAccount_IdsInHashSetTest()
        {

            var a = eMobilityAccount_Id.Parse("1");
            var b = eMobilityAccount_Id.Parse("2");
            var c = eMobilityAccount_Id.Parse("1");

            var _HashSet = new HashSet<eMobilityAccount_Id>();
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
