/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
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

using Newtonsoft.Json.Linq;

using NUnit.Framework;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// Signed data objects, and the signer roles of a node: which keys may
    /// sign as which role is written by signed role assignments, held to the
    /// owners of the node, to their versions, and to what a role of the node
    /// may do (white paper "Signed OCPP Messages and Signer Roles", sections
    /// 4.4, 6.1, 6.2 and 6.4).
    /// </summary>
    [TestFixture]
    public class SignerRoleTableTests
    {

        #region Data

        private static readonly DateTimeOffset  now  = new (2026, 11, 13, 12, 0, 0, TimeSpan.Zero);

        private ECCKeyPair  owner       = null!;
        private ECCKeyPair  owner2      = null!;
        private ECCKeyPair  technician  = null!;
        private ECCKeyPair  stranger    = null!;

        [SetUp]
        public void Keys()
        {
            owner       = ECCKeyPair.GenerateKeys()!;
            owner2      = ECCKeyPair.GenerateKeys(CryptoAlgorithm.Secp384r1)!;
            technician  = ECCKeyPair.GenerateKeys()!;
            stranger    = ECCKeyPair.GenerateKeys()!;
        }

        /// <summary>
        /// A node with the roles "systemadmin", who may do everything, and
        /// "support", who may look at its state and reset it.
        /// </summary>
        private static SignerRoleTable ATable()

            => new (role => role switch {
                                "systemadmin"  => [ "*:read", "*:edit", "*:run" ],
                                "support"      => [ "state:read", "reset:run" ],
                                _              => null
                            }) {
                   Now = () => now
               };

        private static JObject Assignment(String                Role,
                                          UInt64                Version,
                                          IEnumerable<String>?  Permissions,
                                          params ECCKeyPair[]   Keys)

            => SignerRoleAssignment.Create(
                   Role,
                   Version,
                   Keys.Select(key => (key.PublicKey, (String?) "a key", (DateTimeOffset?) null, (DateTimeOffset?) null)),
                   Permissions:  Permissions,
                   Issued:       now
               );

        private static JObject Signed(JObject Object, params ECCKeyPair[] By)
        {

            foreach (var key in By)
                SignedDataObject.Sign(Object, SignedDataObject.SignerRoleType, key, now);

            return Object;

        }

        /// <summary>
        /// A table whose owner is there already.
        /// </summary>
        private SignerRoleTable ATableWithItsOwner(params ECCKeyPair[] Owners)
        {

            var table = ATable();

            Assert.That(table.TryInstallFirstOwner(Assignment("systemadmin", 1, null, Owners.Length > 0 ? Owners : [ owner ]),
                                                   out _,
                                                   out var error),
                        Is.True,
                        error);

            return table;

        }

        #endregion


        #region ASignedObject_NamesItsSigners_AndNoChangedOneDoes()

        /// <summary>
        /// A signature holds for the object as it was signed: with its
        /// members in another order, and for nothing that says something else.
        /// </summary>
        [Test]
        public void ASignedObject_NamesItsSigners_AndNoChangedOneDoes()
        {

            var signed      = Signed(Assignment("support", 1, null, technician), owner, owner2);

            var reordered   = new JObject(signed.Properties().Reverse());

            var changed     = (JObject) signed.DeepClone();
            changed["version"] = 2;

            var broken      = (JObject) signed.DeepClone();
            broken["signatures"]![1]!["signature"] = broken["signatures"]![0]!["signature"]!.DeepClone();

            Assert.Multiple(() => {

                Assert.That(SignedDataObject.Verify(signed,    SignedDataObject.SignerRoleType, out var signers, out var e1, now), Is.True, e1);
                Assert.That(signers.Select(signer => signer.KeyId),
                            Is.EqualTo(new[] { JWS.Thumbprint(owner.PublicKey), JWS.Thumbprint(owner2.PublicKey) }));
                Assert.That(signers[0].PublicKeyBytes, Is.EqualTo(owner.PublicKeyBytes),
                            "The key of a signer is not the key a signature of a message names.");

                Assert.That(SignedDataObject.Verify(reordered, SignedDataObject.SignerRoleType, out var again,   out var e2, now), Is.True, e2);
                Assert.That(again.Count, Is.EqualTo(2));

                Assert.That(SignedDataObject.Verify(changed,   SignedDataObject.SignerRoleType, out _, out var e3, now), Is.False, "A changed object was verified!");
                Assert.That(e3, Does.Contain("does not hold"));

                Assert.That(SignedDataObject.Verify(broken,    SignedDataObject.SignerRoleType, out _, out _,     now), Is.False, "One signature that does not hold did not refuse the object!");

                Assert.That(SignedDataObject.Verify(Assignment("support", 1, null, technician), SignedDataObject.SignerRoleType, out var nobody, out var e4, now), Is.True, e4);
                Assert.That(nobody, Is.Empty, "An object without signatures has signers!");

            });

        }

        #endregion

        #region ASignature_IsForOneKindOfObject_AndForItsTime()

        /// <summary>
        /// A signature made for a policy is none for a role assignment; one
        /// made in the future or one that has expired is none - unless the
        /// time is left out, as for an object that is read again.
        /// </summary>
        [Test]
        public void ASignature_IsForOneKindOfObject_AndForItsTime()
        {

            var asAPolicy   = SignedDataObject.Sign(Assignment("support", 1, null, technician), SignedDataObject.SignaturePolicyType, owner, now);
            var tomorrow    = SignedDataObject.Sign(Assignment("support", 1, null, technician), SignedDataObject.SignerRoleType,      owner, now + TimeSpan.FromDays(1));
            var expired     = SignedDataObject.Sign(Assignment("support", 1, null, technician), SignedDataObject.SignerRoleType,      owner, now - TimeSpan.FromDays(2), now - TimeSpan.FromDays(1));
            var aYearOld    = SignedDataObject.Sign(Assignment("support", 1, null, technician), SignedDataObject.SignerRoleType,      owner, now - TimeSpan.FromDays(365));

            Assert.Multiple(() => {

                Assert.That(SignedDataObject.Verify(asAPolicy, SignedDataObject.SignerRoleType, out _, out var e1, now), Is.False, "A signature for a policy counted for a role assignment!");
                Assert.That(e1, Does.Contain(SignedDataObject.SignerRoleType));

                Assert.That(SignedDataObject.Verify(tomorrow,  SignedDataObject.SignerRoleType, out _, out var e2, now), Is.False, "A signature of tomorrow counted!");
                Assert.That(e2, Does.Contain("future"));

                Assert.That(SignedDataObject.Verify(expired,   SignedDataObject.SignerRoleType, out _, out var e3, now), Is.False, "An expired signature counted!");
                Assert.That(e3, Does.Contain("expired"));

                Assert.That(SignedDataObject.Verify(expired,   SignedDataObject.SignerRoleType, out _, out var e4, now, WithoutTimeWindow: true), Is.True, e4);

                // What is stored is verified later: a signature does not age by itself.
                Assert.That(SignedDataObject.Verify(aYearOld,  SignedDataObject.SignerRoleType, out _, out var e5, now), Is.True, e5);

            });

        }

        #endregion


        #region WithoutAnOwner_NothingIsAssigned_AndTheFirstOwnerComesLocally()

        /// <summary>
        /// A node without an owner takes no role assignment, whoever signed
        /// it. Its first owner is made at the node itself, once.
        /// </summary>
        [Test]
        public void WithoutAnOwner_NothingIsAssigned_AndTheFirstOwnerComesLocally()
        {

            var table    = ATable();
            var changes  = new List<SignerRoleTableChange>();
            table.OnChanged += changes.Add;

            Assert.Multiple(() => {

                Assert.That(table.HasOwner, Is.False);

                Assert.That(table.TryAdd(Signed(Assignment("support",     1, null, technician), stranger), out var r1, out _), Is.False, "A node without an owner took a role!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd(Signed(Assignment("systemadmin", 1, null, stranger),   stranger), out var r2, out _), Is.False, "The first owner came over the network, which was not allowed!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryInstallFirstOwner(Assignment("support",     1, null, technician), out var r3, out var e3), Is.False, "The support became the owner!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.NotAuthorized), e3);

                Assert.That(table.TryInstallFirstOwner(Assignment("systemadmin", 1, null, owner),      out _,      out var e4), Is.True,  e4);
                Assert.That(table.HasOwner, Is.True);
                Assert.That(table.RolesOf(owner.PublicKeyBytes), Is.EqualTo(new[] { "systemadmin" }));

                Assert.That(table.TryInstallFirstOwner(Assignment("systemadmin", 2, null, stranger),   out var r5, out _),      Is.False, "A second first owner was made!");
                Assert.That(r5, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(changes.Select(change => (change.Change, change.Role, change.Version, change.Locally)),
                            Is.EqualTo(new[] { (SignerRoleTableChanges.FirstOwnerInstalled, (String?) "systemadmin", 1UL, true) }));

            });

        }

        #endregion

        #region TheFirstOwner_ComesOverTheNetwork_OnlyWhereThatIsAllowed()

        /// <summary>
        /// Trust on first use, off by default: allowed, the first owner is
        /// whoever comes first with an assignment a key of its own signed.
        /// </summary>
        [Test]
        public void TheFirstOwner_ComesOverTheNetwork_OnlyWhereThatIsAllowed()
        {

            var table = ATable();
            table.AllowFirstOwnerByNetwork = true;

            Assert.Multiple(() => {

                Assert.That(table.TryAdd(Signed(Assignment("support",     1, null, technician), technician), out var r1, out _), Is.False, "The first one to come became the support!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd(Signed(Assignment("systemadmin", 1, null, owner),      stranger),   out var r2, out _), Is.False, "An owner was made by somebody else's signature!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd(Signed(Assignment("systemadmin", 1, null, owner),      owner),      out _,      out var e3), Is.True, e3);

                Assert.That(table.TryUpdate(Signed(Assignment("systemadmin", 2, null, stranger), stranger),  out var r4, out _), Is.False, "The first owner was replaced by the next one to come!");
                Assert.That(r4, Is.EqualTo(SignerRoleReasons.NotAuthorized));

            });

        }

        #endregion

        #region ARole_GetsItsKeysByItsOwners_AndByNobodyElse()

        /// <summary>
        /// Add, update, delete: by an assignment the owner signed, in a
        /// version that counts upwards.
        /// </summary>
        [Test]
        public void ARole_GetsItsKeysByItsOwners_AndByNobodyElse()
        {

            var table    = ATableWithItsOwner();
            var changes  = new List<SignerRoleTableChange>();
            table.OnChanged += changes.Add;

            Assert.Multiple(() => {

                Assert.That(table.TryAdd   (Signed(Assignment("support", 1, null, technician), stranger),   out var r1, out _), Is.False, "A stranger gave a role its keys!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd   (       Assignment("support", 1, null, technician),              out var r2, out _), Is.False, "A role got its keys without a signature!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryUpdate(Signed(Assignment("support", 1, null, technician), owner),      out var r3, out _), Is.False, "A role without keys was updated!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.UnknownRole));

                Assert.That(table.TryAdd   (Signed(Assignment("support", 1, null, technician), owner),      out _,      out var e4), Is.True, e4);
                Assert.That(table.RolesOf(technician.PublicKeyBytes), Is.EqualTo(new[] { "support" }));
                Assert.That(table.PermissionsOf("support"),           Is.EqualTo(new[] { "state:read", "reset:run" }));

                Assert.That(table.TryAdd   (Signed(Assignment("support", 2, null, technician), owner),      out var r5, out _), Is.False, "A role that has keys was added again!");
                Assert.That(r5, Is.EqualTo(SignerRoleReasons.RoleExists));

                Assert.That(table.TryUpdate(Signed(Assignment("support", 1, null, stranger),   owner),      out var r6, out _), Is.False, "The same version was taken twice!");
                Assert.That(r6, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

                Assert.That(table.TryUpdate(Signed(Assignment("support", 2, null, stranger),   owner),      out _,      out var e7), Is.True, e7);
                Assert.That(table.RolesOf(technician.PublicKeyBytes), Is.Empty, "A replaced key still signs as its role!");
                Assert.That(table.RolesOf(stranger.  PublicKeyBytes), Is.EqualTo(new[] { "support" }));

                Assert.That(table.TryDelete("support", 1, out var r8, out _), Is.False, "A delete of an older version was taken!");
                Assert.That(r8, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

                Assert.That(table.TryDelete("support", 2, out _, out var e9), Is.True, e9);
                Assert.That(table.RolesOf(stranger.PublicKeyBytes), Is.Empty);

                // What was deleted does not come back by an old assignment.
                Assert.That(table.TryAdd   (Signed(Assignment("support", 2, null, technician), owner),      out var r10, out _), Is.False, "An old assignment came back after its role was deleted!");
                Assert.That(r10, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

                Assert.That(table.TryAdd   (Signed(Assignment("support", 3, null, technician), owner),      out _,       out var e11), Is.True, e11);

                Assert.That(changes.Select(change => (change.Change, change.Role, change.Version)),
                            Is.EqualTo(new[] {
                                (SignerRoleTableChanges.RoleAdded,   (String?) "support", 1UL),
                                (SignerRoleTableChanges.RoleUpdated, (String?) "support", 2UL),
                                (SignerRoleTableChanges.RoleDeleted, (String?) "support", 2UL),
                                (SignerRoleTableChanges.RoleAdded,   (String?) "support", 3UL)
                            }));

            });

        }

        #endregion

        #region AnAssignment_SaysNoMoreThanTheNodeSaysOfTheRole()

        /// <summary>
        /// Keys are bound to the roles the node has. An assignment may say
        /// less than the node says of a role, and no more; nobody writes what
        /// the owner may do; and a role has no more keys than it may have.
        /// </summary>
        [Test]
        public void AnAssignment_SaysNoMoreThanTheNodeSaysOfTheRole()
        {

            var table = ATableWithItsOwner();
            table.MaxKeysPerRole = 2;

            Assert.Multiple(() => {

                Assert.That(table.TryAdd(Signed(Assignment("release-manager", 1, null,                              technician), owner), out var r1, out _), Is.False, "A role the node does not have got keys!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.UnknownRole));

                Assert.That(table.TryAdd(Signed(Assignment("release-manager", 1, [ "firmware:edit" ],               technician), owner), out var r2, out _), Is.False, "A role was defined over the network!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.UnknownRole));

                Assert.That(table.TryAdd(Signed(Assignment("support",         1, [ "state:read", "firmware:edit" ], technician), owner), out var r3, out var e3), Is.False, "A role was widened over the network!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.NotAuthorized));
                Assert.That(e3, Does.Contain("firmware:edit"));

                Assert.That(table.TryUpdate(Signed(Assignment("systemadmin",  2, [ "state:read" ],                  owner),      owner), out var r4, out _), Is.False, "What the owner may do was written!");
                Assert.That(r4, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd(Signed(Assignment("support",         1, null, technician, stranger, owner2),            owner), out var r5, out _), Is.False, "A role got more keys than it may have!");
                Assert.That(r5, Is.EqualTo(SignerRoleReasons.KeyLimitReached));

                Assert.That(table.TryAdd(Signed(Assignment("support",         1, [ "state:read" ],                  technician), owner), out _,      out var e6), Is.True, e6);
                Assert.That(table.PermissionsOf("support"), Is.EqualTo(new[] { "state:read" }), "A narrowed role may do what the node says of it!");

                table.AllowPermissionsByNetwork = true;

                Assert.That(table.TryAdd(Signed(Assignment("release-manager", 1, [ "firmware:edit" ],               stranger),   owner), out _,      out var e7), Is.True, e7);
                Assert.That(table.PermissionsOf("release-manager"), Is.EqualTo(new[] { "firmware:edit" }));

            });

        }

        #endregion

        #region AKey_IsAPublicKey_AndTheOneItsIdentifierNames()

        /// <summary>
        /// A private key is never part of an assignment, and a key is the
        /// one its identifier names.
        /// </summary>
        [Test]
        public void AKey_IsAPublicKey_AndTheOneItsIdentifierNames()
        {

            var table        = ATableWithItsOwner();

            var withPrivate  = Assignment("support", 1, null, technician);
            withPrivate["keys"]![0]!["jwk"]!["d"] = "AAAA";

            var wrongId      = Assignment("support", 1, null, technician);
            wrongId    ["keys"]![0]!["kid"] = JWS.Thumbprint(stranger.PublicKey);

            var offCurve     = Assignment("support", 1, null, technician);
            offCurve   ["keys"]![0]!["jwk"]!["x"] = offCurve["keys"]![0]!["jwk"]!["y"]!.DeepClone();

            var twice        = Assignment("support", 1, null, technician, technician);

            Assert.Multiple(() => {

                Assert.That(table.TryAdd(Signed(withPrivate, owner), out var r1, out var e1), Is.False, "A private key was taken!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.InvalidKey));
                Assert.That(e1, Does.Contain("private"));

                Assert.That(table.TryAdd(Signed(wrongId,     owner), out var r2, out _),      Is.False, "A key under another key's identifier was taken!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.InvalidKey));

                Assert.That(table.TryAdd(Signed(offCurve,    owner), out var r3, out _),      Is.False, "A point that is not on the curve was taken as a key!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.InvalidKey));

                Assert.That(table.TryAdd(Signed(twice,       owner), out var r4, out _),      Is.False, "A key was taken twice!");
                Assert.That(r4, Is.EqualTo(SignerRoleReasons.InvalidKey));

                Assert.That(table.Assignments.Select(assignment => assignment.Role), Is.EqualTo(new[] { "systemadmin" }));

            });

        }

        #endregion

        #region AKey_CountsFromItsFirstDayToItsLast()

        /// <summary>
        /// A key that is not valid yet, or no longer, signs as nobody.
        /// </summary>
        [Test]
        public void AKey_CountsFromItsFirstDayToItsLast()
        {

            var table  = ATableWithItsOwner();

            var json   = SignerRoleAssignment.Create(
                             "support",
                             1,
                             [
                                 (technician.PublicKey, "this year", now - TimeSpan.FromDays(30),  now + TimeSpan.FromDays(30)),
                                 (stranger.  PublicKey, "next year", now + TimeSpan.FromDays(300), null),
                                 (owner2.    PublicKey, "last year", null,                         now - TimeSpan.FromDays(300))
                             ],
                             Issued: now
                         );

            Assert.Multiple(() => {

                Assert.That(table.TryAdd(Signed(json, owner), out _, out var e1), Is.True, e1);

                Assert.That(table.RolesOf(technician.PublicKeyBytes), Is.EqualTo(new[] { "support" }));
                Assert.That(table.RolesOf(stranger.  PublicKeyBytes), Is.Empty, "A key of next year signs today!");
                Assert.That(table.RolesOf(owner2.    PublicKeyBytes), Is.Empty, "A key of last year signs today!");

                Assert.That(table.LastUsed("support"), Is.Null);
                table.RolesOf(technician.PublicKeyBytes, Used: true);
                Assert.That(table.LastUsed("support"), Is.EqualTo(now));

            });

        }

        #endregion

        #region NobodyIsLeftWithoutAnOwner()

        /// <summary>
        /// The last owner key is not taken away over the network: not by a
        /// delete, not by an assignment without a key that counts. Another
        /// owner replaces it.
        /// </summary>
        [Test]
        public void NobodyIsLeftWithoutAnOwner()
        {

            var table    = ATableWithItsOwner();

            var expired  = SignerRoleAssignment.Create("systemadmin", 2, [ (owner2.PublicKey, null, null, now - TimeSpan.FromDays(1)) ], Issued: now);

            Assert.Multiple(() => {

                Assert.That(table.TryDelete("systemadmin", 1, out var r1, out _), Is.False, "The last owner was deleted!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.WouldLockOut));

                Assert.That(table.TryUpdate(Signed(Assignment("systemadmin", 2, null),         owner), out var r2, out _), Is.False, "The owner was left without a key!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.WouldLockOut));

                Assert.That(table.TryUpdate(Signed(expired,                                    owner), out var r3, out _), Is.False, "The owner was left with a key that counts no more!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.WouldLockOut));

                Assert.That(table.TryUpdate(Signed(Assignment("systemadmin", 2, null, owner2), owner), out _,      out var e4), Is.True, e4);
                Assert.That(table.RolesOf(owner. PublicKeyBytes), Is.Empty,                           "The old owner is still one!");
                Assert.That(table.RolesOf(owner2.PublicKeyBytes), Is.EqualTo(new[] { "systemadmin" }));

                Assert.That(table.TryAdd(Signed(Assignment("support", 1, null, technician),    owner), out var r5, out _), Is.False, "The old owner still gives roles their keys!");
                Assert.That(r5, Is.EqualTo(SignerRoleReasons.NotAuthorized));

            });

        }

        #endregion

        #region AQuorum_TakesThatManyOwners()

        /// <summary>
        /// Where two owners have to sign, one is not enough, and the same
        /// one twice is one.
        /// </summary>
        [Test]
        public void AQuorum_TakesThatManyOwners()
        {

            var table = ATableWithItsOwner(owner, owner2);
            table.Quorum = 2;

            Assert.Multiple(() => {

                Assert.That(table.TryAdd(Signed(Assignment("support", 1, null, technician), owner),           out var r1, out var e1), Is.False, "One of two owners was enough!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.NotAuthorized));
                Assert.That(e1, Does.Contain("1 of the 2"));

                Assert.That(table.TryAdd(Signed(Assignment("support", 1, null, technician), owner, owner),    out var r2, out _),      Is.False, "One owner signing twice counted as two!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd(Signed(Assignment("support", 1, null, technician), owner, stranger), out var r3, out _),      Is.False, "A stranger counted as the second owner!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TryAdd(Signed(Assignment("support", 1, null, technician), owner, owner2),   out _,      out var e4), Is.True, e4);

            });

        }

        #endregion

        #region ThePolicy_IsSignedByTheOwners_AndCountsUpwards()

        /// <summary>
        /// The policy table is a signed data object as well: signed by the
        /// owners, of a version that counts upwards, and taken as a whole or
        /// not at all.
        /// </summary>
        [Test]
        public void ThePolicy_IsSignedByTheOwners_AndCountsUpwards()
        {

            var table    = ATableWithItsOwner();
            var applied  = new List<Int32>();

            JObject APolicy(UInt64 Version, Int32 Entries, ECCKeyPair By)
                => SignedDataObject.Sign(
                       new JObject(
                           new JProperty("version", Version),
                           new JProperty("entries", new JArray(Enumerable.Range(0, Entries).Select(number => new JObject(new JProperty("id", $"entry {number}")))))
                       ),
                       SignedDataObject.SignaturePolicyType,
                       By,
                       now
                   );

            String? Apply(JArray Entries)
            {

                if (Entries.Count > 2)
                    return "Too many entries!";

                applied.Add(Entries.Count);
                return null;

            }

            Assert.Multiple(() => {

                Assert.That(table.TrySetPolicy(APolicy(1, 1, stranger), Apply, out var r1, out _), Is.False, "A stranger set the policy!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.NotAuthorized));

                Assert.That(table.TrySetPolicy(APolicy(1, 3, owner),    Apply, out var r2, out var e2), Is.False, "A policy that cannot be applied was stored!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.InvalidPolicy));
                Assert.That(e2, Is.EqualTo("Too many entries!"));
                Assert.That(table.PolicyVersion, Is.Zero);

                Assert.That(table.TrySetPolicy(APolicy(1, 1, owner),    Apply, out _,      out var e3), Is.True, e3);
                Assert.That(table.PolicyVersion, Is.EqualTo(1));

                Assert.That(table.TrySetPolicy(APolicy(1, 2, owner),    Apply, out var r4, out _),      Is.False, "The same version of the policy was taken twice!");
                Assert.That(r4, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

                Assert.That(table.TrySetPolicy(APolicy(5, 2, owner),    Apply, out _,      out var e5), Is.True, e5);
                Assert.That(table.PolicyVersion,                 Is.EqualTo(5));
                Assert.That(table.Policy?["entries"]?.Count(),   Is.EqualTo(2));
                Assert.That(table.Policy?["signatures"]?.Count(), Is.EqualTo(1), "The policy is not stored as it was received.");

                Assert.That(applied, Is.EqualTo(new[] { 1, 2 }));

            });

        }

        #endregion

        #region WhatIsKept_IsReadBack_AsItWasReceived()

        /// <summary>
        /// The table is kept as the objects it was given, with their
        /// signatures, and reads itself back from them - a year later too.
        /// </summary>
        [Test]
        public void WhatIsKept_IsReadBack_AsItWasReceived()
        {

            var table    = ATableWithItsOwner();
            var support  = Signed(Assignment("support", 4, null, technician), owner);

            Assert.That(table.TryAdd(support, out _, out var error), Is.True, error);
            Assert.That(table.TryAdd(Signed(Assignment("release-manager", 1, null, stranger), owner), out _, out _), Is.False);

            var kept     = SignedDataObject.ParseAsReceived(table.ToJSON().ToString());

            // Newtonsoft's own way of reading makes dates of the texts that
            // look like one, and writes them back otherwise.
            var reread   = JObject.Parse(table.ToJSON().ToString());

            var later    = ATable();
            later.Now    = () => now + TimeSpan.FromDays(365);

            var tampered = (JObject) kept.DeepClone();
            tampered["roles"]![0]!["version"] = 9;

            Assert.Multiple(() => {

                Assert.That(kept["roles"]!.Select(role => role["role"]!.Value<String>()), Is.EqualTo(new[] { "support", "systemadmin" }));
                Assert.That(JToken.DeepEquals(kept["roles"]![0], support), Is.True, "The assignment is not kept as it was received.");

                Assert.That(later.TryLoad(kept, out var e1), Is.True, e1);
                Assert.That(later.HasOwner,                              Is.True);
                Assert.That(later.RolesOf(technician.PublicKeyBytes),    Is.EqualTo(new[] { "support" }));
                Assert.That(later.VersionOf("support"),                  Is.EqualTo(4));

                Assert.That(later.TryUpdate(Signed(Assignment("support", 4, null, stranger), owner), out var r2, out _), Is.False, "A version was taken again after the table was read back!");
                Assert.That(r2, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

                Assert.That(ATable().TryLoad(tampered, out var e3), Is.False, "A changed assignment was read back!");
                Assert.That(e3, Does.Contain("does not hold"));

                Assert.That(ATable().TryLoad(reread,   out var e4), Is.False, "An object whose times were read as dates still has its signatures!");
                Assert.That(e4, Does.Contain("does not hold"));

            });

        }

        #endregion

        #region AReset_ForgetsTheKeys_AndNotTheVersions()

        /// <summary>
        /// The way out for a node whose owners lost their keys: nobody signs
        /// as anybody afterwards, a first owner can be made again, and
        /// nothing old comes back.
        /// </summary>
        [Test]
        public void AReset_ForgetsTheKeys_AndNotTheVersions()
        {

            var table = ATableWithItsOwner();

            Assert.That(table.TryAdd(Signed(Assignment("support", 3, null, technician), owner), out _, out var error), Is.True, error);

            var changes = new List<SignerRoleTableChange>();
            table.OnChanged += changes.Add;

            table.Reset();

            Assert.Multiple(() => {

                Assert.That(table.HasOwner,                             Is.False);
                Assert.That(table.RolesOf(owner.     PublicKeyBytes),   Is.Empty);
                Assert.That(table.RolesOf(technician.PublicKeyBytes),   Is.Empty);
                Assert.That(changes.Single().Change,                    Is.EqualTo(SignerRoleTableChanges.TableReset));
                Assert.That(changes.Single().Locally,                   Is.True);

                Assert.That(table.TryInstallFirstOwner(Assignment("systemadmin", 1, null, owner2), out var r1, out _), Is.False, "The version of the owner before the reset was taken again!");
                Assert.That(r1, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

                Assert.That(table.TryInstallFirstOwner(Assignment("systemadmin", 2, null, owner2), out _,      out var e2), Is.True, e2);

                Assert.That(table.TryAdd(Signed(Assignment("support", 3, null, technician), owner2), out var r3, out _), Is.False, "An assignment of before the reset came back!");
                Assert.That(r3, Is.EqualTo(SignerRoleReasons.VersionNotNewer));

            });

        }

        #endregion

    }

}
