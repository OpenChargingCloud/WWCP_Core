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

using System.Globalization;

using Newtonsoft.Json.Linq;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    #region SignerRoleReasons

    /// <summary>
    /// Why a change of the signer roles or of the signature policy of a node
    /// was refused: the reason codes of the messages of section 6.4 of the
    /// white paper "Signed OCPP Messages and Signer Roles".
    /// </summary>
    public enum SignerRoleReasons
    {

        /// <summary>
        /// The object is not signed by whom it takes, or says what it may not say.
        /// </summary>
        NotAuthorized,

        /// <summary>
        /// The version of the object is not greater than the one that is known.
        /// </summary>
        VersionNotNewer,

        /// <summary>
        /// The node has no such role, or no keys of it.
        /// </summary>
        UnknownRole,

        /// <summary>
        /// The role has keys already.
        /// </summary>
        RoleExists,

        /// <summary>
        /// A key of the object is none, is not the one its identifier names, or is a private key.
        /// </summary>
        InvalidKey,

        /// <summary>
        /// The object has more keys than a role may have.
        /// </summary>
        KeyLimitReached,

        /// <summary>
        /// The change would leave nobody who may change the roles.
        /// </summary>
        WouldLockOut,

        /// <summary>
        /// The policy cannot be the policy of a node: two of its entries are equally specific, or one is none.
        /// </summary>
        InvalidPolicy,

        /// <summary>
        /// The object is not a role assignment.
        /// </summary>
        InvalidRoleAssignment

    }

    #endregion

    #region SignerRoleKey

    /// <summary>
    /// A key of a signer role.
    /// </summary>
    /// <param name="KeyId">The RFC 7638 thumbprint of the key.</param>
    /// <param name="PublicKey">The public key.</param>
    /// <param name="PublicKeyBytes">The public key as its uncompressed point, as a signature of a message names it.</param>
    /// <param name="Label">Whose key it is, in the words of whoever assigned it.</param>
    /// <param name="NotBefore">From when on the key counts.</param>
    /// <param name="NotAfter">Until when the key counts.</param>
    public sealed record SignerRoleKey(String                 KeyId,
                                       ECPublicKeyParameters  PublicKey,
                                       Byte[]                 PublicKeyBytes,
                                       String?                Label,
                                       DateTimeOffset?        NotBefore,
                                       DateTimeOffset?        NotAfter)
    {

        /// <summary>
        /// Whether the key counts at the given time.
        /// </summary>
        public Boolean IsValidAt(DateTimeOffset Time)

            => (!NotBefore.HasValue || Time >= NotBefore.Value) &&
               (!NotAfter. HasValue || Time <= NotAfter. Value);

    }

    #endregion

    #region SignerRoleAssignment

    /// <summary>
    /// The keys of a signer role, as a signed data object (section 6.2): who
    /// may sign as the role, and since which version of the assignment.
    /// </summary>
    public sealed class SignerRoleAssignment
    {

        #region Properties

        /// <summary>
        /// The role the keys are assigned to.
        /// </summary>
        public String                         Role           { get; }

        /// <summary>
        /// What the role is for.
        /// </summary>
        public String?                        Description    { get; }

        /// <summary>
        /// What the role may do, where the assignment says so: null leaves it to the node's own definition of the role.
        /// </summary>
        public IReadOnlyList<String>?         Permissions    { get; }

        /// <summary>
        /// The keys of the role.
        /// </summary>
        public IReadOnlyList<SignerRoleKey>   Keys           { get; }

        /// <summary>
        /// The version of the assignment: a later one is greater.
        /// </summary>
        public UInt64                         Version        { get; }

        /// <summary>
        /// When the assignment was made.
        /// </summary>
        public DateTimeOffset?                Issued         { get; }

        /// <summary>
        /// The assignment as it was received, with its signatures.
        /// </summary>
        public JObject                        JSON           { get; }

        #endregion

        #region Constructor(s)

        private SignerRoleAssignment(String                        Role,
                                     String?                       Description,
                                     IReadOnlyList<String>?        Permissions,
                                     IReadOnlyList<SignerRoleKey>  Keys,
                                     UInt64                        Version,
                                     DateTimeOffset?               Issued,
                                     JObject                       JSON)
        {

            this.Role         = Role;
            this.Description  = Description;
            this.Permissions  = Permissions;
            this.Keys         = Keys;
            this.Version      = Version;
            this.Issued       = Issued;
            this.JSON         = JSON;

        }

        #endregion


        #region (static) Create(Role, Version, Keys, Description = null, Permissions = null, Issued = null)

        /// <summary>
        /// The JSON of a role assignment that is not signed yet.
        /// </summary>
        /// <param name="Role">The role.</param>
        /// <param name="Version">The version of the assignment.</param>
        /// <param name="Keys">The keys of the role, each with the label it is to have.</param>
        /// <param name="Description">What the role is for.</param>
        /// <param name="Permissions">What the role may do, where the assignment is to say so.</param>
        /// <param name="Issued">When the assignment is made: now, by default.</param>
        public static JObject Create(String                                                                                        Role,
                                     UInt64                                                                                        Version,
                                     IEnumerable<(ECPublicKeyParameters Key, String? Label, DateTimeOffset? NotBefore, DateTimeOffset? NotAfter)>  Keys,
                                     String?                                                                                       Description   = null,
                                     IEnumerable<String>?                                                                          Permissions   = null,
                                     DateTimeOffset?                                                                               Issued        = null)
        {

            var json = new JObject(new JProperty("role", Role));

            if (Description is not null)
                json.Add("description", Description);

            if (Permissions is not null)
                json.Add("permissions", new JArray(Permissions));

            json.Add("keys", new JArray(Keys.Select(key => {

                var jwk   = JWS.PublicKeyJWK(key.Key);
                var entry = new JObject(
                                new JProperty("kid",  JWS.Thumbprint(jwk)),
                                new JProperty("jwk",  jwk)
                            );

                if (key.Label is not null)
                    entry.Add("label",     key.Label);

                if (key.NotBefore.HasValue)
                    entry.Add("notBefore", key.NotBefore.Value.ToISO8601());

                if (key.NotAfter.HasValue)
                    entry.Add("notAfter",  key.NotAfter. Value.ToISO8601());

                return entry;

            })));

            json.Add("version", Version);
            json.Add("issued",  (Issued ?? Timestamp.Now).ToISO8601());

            return json;

        }

        #endregion

        #region (static) TryParse(JSON, out Assignment, out Reason, out ErrorResponse)

        /// <summary>
        /// Read a role assignment, as it was received. Its signatures are
        /// not looked at here.
        /// </summary>
        /// <param name="JSON">The assignment, as received.</param>
        /// <param name="Assignment">The assignment.</param>
        /// <param name="Reason">Why it is none.</param>
        /// <param name="ErrorResponse">Why it is none, in words.</param>
        public static Boolean TryParse(JObject                    JSON,
                                       out SignerRoleAssignment?  Assignment,
                                       out SignerRoleReasons      Reason,
                                       out String?                ErrorResponse)
        {

            Assignment     = null;
            Reason         = SignerRoleReasons.InvalidRoleAssignment;
            ErrorResponse  = null;

            #region Role

            if (JSON["role"]?.Type != JTokenType.String ||
                JSON["role"]!.Value<String>() is not String role ||
                role.Length == 0 ||
                role != role.Trim())
            {
                ErrorResponse = "The role assignment names no role!";
                return false;
            }

            #endregion

            #region Description

            String? description = null;

            if (JSON["description"] is JToken descriptionToken)
            {

                if (descriptionToken.Type != JTokenType.String)
                {
                    ErrorResponse = "The description of the role assignment is not a text!";
                    return false;
                }

                description = descriptionToken.Value<String>();

            }

            #endregion

            #region Permissions

            List<String>? permissions = null;

            if (JSON["permissions"] is JToken permissionsToken)
            {

                if (permissionsToken is not JArray permissionsArray)
                {
                    ErrorResponse = "The permissions of the role assignment are not an array!";
                    return false;
                }

                permissions = [];

                foreach (var permission in permissionsArray)
                {

                    if (permission.Type != JTokenType.String ||
                        !SignerPermissions.IsPermission(permission.Value<String>()))
                    {
                        ErrorResponse = $"'{permission}' is not a permission!";
                        return false;
                    }

                    permissions.Add(permission.Value<String>()!);

                }

            }

            #endregion

            #region Keys

            if (JSON["keys"] is not JArray keysArray)
            {
                ErrorResponse = "The role assignment has no keys!";
                return false;
            }

            var keys = new List<SignerRoleKey>();

            foreach (var keyToken in keysArray)
            {

                Reason = SignerRoleReasons.InvalidKey;

                if (keyToken is not JObject keyJSON ||
                    keyJSON["jwk"] is not JObject jwk)
                {
                    ErrorResponse = "A key of the role assignment is not a JSON Web Key!";
                    return false;
                }

                // A private key is never part of what is sent to a node.
                if (keyJSON["d"] is not null || keyJSON["private"] is not null ||
                    jwk    ["d"] is not null || jwk    ["private"] is not null)
                {
                    ErrorResponse = "A key of the role assignment is a private key!";
                    return false;
                }

                if (!JWS.TryParsePublicKey(jwk, out var publicKey) || publicKey is null)
                {
                    ErrorResponse = "A key of the role assignment is not a public key of a curve that is known!";
                    return false;
                }

                var keyId = JWS.Thumbprint(publicKey);

                if (keyJSON["kid"]?.Type != JTokenType.String ||
                    keyJSON["kid"]!.Value<String>() != keyId)
                {
                    ErrorResponse = $"A key of the role assignment is not the key its identifier names: it is '{keyId}'!";
                    return false;
                }

                if (keys.Any(key => key.KeyId == keyId))
                {
                    ErrorResponse = $"The key '{keyId}' is in the role assignment twice!";
                    return false;
                }

                String? label = null;

                if (keyJSON["label"] is JToken labelToken)
                {

                    if (labelToken.Type != JTokenType.String)
                    {
                        ErrorResponse = $"The label of the key '{keyId}' is not a text!";
                        return false;
                    }

                    label = labelToken.Value<String>();

                }

                if (!TryParseTime(keyJSON["notBefore"], out var notBefore) ||
                    !TryParseTime(keyJSON["notAfter"],  out var notAfter))
                {
                    ErrorResponse = $"The key '{keyId}' counts from or until no time there is!";
                    return false;
                }

                if (notBefore.HasValue && notAfter.HasValue && notAfter.Value < notBefore.Value)
                {
                    ErrorResponse = $"The key '{keyId}' ends to count before it begins to!";
                    return false;
                }

                keys.Add(new SignerRoleKey(
                             keyId,
                             publicKey,
                             publicKey.Q.GetEncoded(false),
                             label,
                             notBefore,
                             notAfter
                         ));

            }

            Reason = SignerRoleReasons.InvalidRoleAssignment;

            #endregion

            #region Version

            if (JSON["version"]?.Type != JTokenType.Integer)
            {
                ErrorResponse = "The role assignment has no version!";
                return false;
            }

            UInt64 version;

            try
            {
                version = JSON["version"]!.Value<UInt64>();
            }
            catch
            {
                ErrorResponse = "The version of the role assignment is not a number that counts upwards!";
                return false;
            }

            #endregion

            #region Issued

            if (!TryParseTime(JSON["issued"], out var issued))
            {
                ErrorResponse = "The role assignment was issued at no time there is!";
                return false;
            }

            #endregion

            Assignment = new SignerRoleAssignment(
                             role,
                             description,
                             permissions,
                             keys,
                             version,
                             issued,
                             (JObject) JSON.DeepClone()
                         );

            return true;

        }

        #endregion

        #region (internal static) TryParseTime(Token, out Time)

        /// <summary>
        /// A point in time as JSON has it, read in no culture of the machine's.
        /// A missing one is none, and no error.
        /// </summary>
        internal static Boolean TryParseTime(JToken? Token, out DateTimeOffset? Time)
        {

            Time = null;

            if (Token is null || Token.Type == JTokenType.Null)
                return true;

            // Read by a parser that makes dates of texts: the signatures of
            // such an object hold no longer, but its times are still times.
            if (Token.Type == JTokenType.Date)
            {

                Time = ((JValue) Token).Value switch {
                           DateTimeOffset withOffset  => withOffset.ToUniversalTime(),
                           DateTime       dateTime    => dateTime.Kind == DateTimeKind.Unspecified
                                                             ? new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc))
                                                             : new DateTimeOffset(dateTime.ToUniversalTime()),
                           _                          => null
                       };

                return Time.HasValue;

            }

            if (Token.Type == JTokenType.String &&
                DateTimeOffset.TryParse(Token.Value<String>(),
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                                        out var time))
            {
                Time = time;
                return true;
            }

            return false;

        }

        #endregion

    }

    #endregion

    #region SignerRoleTableChange

    /// <summary>
    /// What was changed about the signer roles of a node.
    /// </summary>
    public enum SignerRoleTableChanges
    {
        RoleAdded,
        RoleUpdated,
        RoleDeleted,
        FirstOwnerInstalled,
        TableReset,
        PolicySet
    }

    /// <summary>
    /// A change of the signer roles or of the signature policy of a node:
    /// what its security event "RoleTableChanged" tells of.
    /// </summary>
    /// <param name="Change">What was changed.</param>
    /// <param name="Role">The role, where one was changed.</param>
    /// <param name="Version">The version that is stored now.</param>
    /// <param name="Locally">Whether it was changed at the node itself, and not over the network.</param>
    public sealed record SignerRoleTableChange(SignerRoleTableChanges  Change,
                                               String?                 Role,
                                               UInt64                  Version,
                                               Boolean                 Locally);

    #endregion


    /// <summary>
    /// The signer roles of a node: which keys may sign as which of its roles,
    /// as the signed role assignments it was given, and the policy table it
    /// was given with them (sections 6.1, 6.2 and 6.4).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Whatever writes a role comes through here - a message, a file read at
    /// the node, a page - so that an assignment is held to the same rules
    /// wherever it comes from. The first owner key is the one exception:
    /// there is nothing yet to hold it to.
    /// </para>
    /// <para>
    /// The owner is whoever holds the permission to edit the signatures, by
    /// default the node's role "systemadmin". Once there is an owner, only an
    /// assignment the owners signed changes a role, and no change leaves the
    /// node without one.
    /// </para>
    /// </remarks>
    public sealed class SignerRoleTable
    {

        #region Data

        private readonly Lock                                      sync       = new();

        private readonly Dictionary<String, SignerRoleAssignment>  roles      = new (StringComparer.Ordinal);

        /// <summary>
        /// The greatest version every role ever had, of the deleted ones as well.
        /// </summary>
        private readonly Dictionary<String, UInt64>                versions   = new (StringComparer.Ordinal);

        private readonly Dictionary<String, DateTimeOffset>        lastUsed   = new (StringComparer.Ordinal);

        private          JObject?                                  policy;

        #endregion

        #region Properties

        /// <summary>
        /// What a role may do by the node's own definition of it, or null for a role the node does not have.
        /// </summary>
        public Func<String, IEnumerable<String>?>   PermissionsOfRole           { get; }

        /// <summary>
        /// The role of the owner.
        /// </summary>
        public String                               OwnerRole                   { get; init; } = "systemadmin";

        /// <summary>
        /// The permission that makes an owner.
        /// </summary>
        public String                               OwnerPermission             { get; init; } = "signatures:edit";

        /// <summary>
        /// How many owners have to sign a change.
        /// </summary>
        public UInt32                               Quorum                      { get; set; }  = 1;

        /// <summary>
        /// How many keys a role may have.
        /// </summary>
        public UInt32                               MaxKeysPerRole              { get; set; }  = 16;

        /// <summary>
        /// Whether the first owner may come over the network, while there is none (section 6.1, option 3).
        /// </summary>
        public Boolean                              AllowFirstOwnerByNetwork    { get; set; }

        /// <summary>
        /// Whether an assignment may say what a role may do beyond what the node says of it (Q4).
        /// </summary>
        public Boolean                              AllowPermissionsByNetwork   { get; set; }

        /// <summary>
        /// The time.
        /// </summary>
        public Func<DateTimeOffset>                 Now                         { get; set; }  = () => Timestamp.Now;

        /// <summary>
        /// How far in the future a signature may say it was made.
        /// </summary>
        public TimeSpan                             MaxClockSkew                { get; set; }  = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The version of the policy table, 0 while none was given.
        /// </summary>
        public UInt64                               PolicyVersion               { get; private set; }

        /// <summary>
        /// The policy table as it was received, or null.
        /// </summary>
        public JObject?                             Policy
        {
            get
            {
                lock (sync)
                    return (JObject?) policy?.DeepClone();
            }
        }

        /// <summary>
        /// The role assignments that are stored.
        /// </summary>
        public IReadOnlyList<SignerRoleAssignment>  Assignments
        {
            get
            {
                lock (sync)
                    return [.. roles.Values.OrderBy(assignment => assignment.Role, StringComparer.Ordinal)];
            }
        }

        /// <summary>
        /// Whether somebody may change the roles.
        /// </summary>
        public Boolean                              HasOwner
        {
            get
            {
                lock (sync)
                    return OwnerKeysOf(roles).Count > 0;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Something about the roles or the policy was changed.
        /// </summary>
        public event Action<SignerRoleTableChange>? OnChanged;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new, empty table of signer roles.
        /// </summary>
        /// <param name="PermissionsOfRole">What a role may do by the node's own definition of it, or null for a role the node does not have.</param>
        public SignerRoleTable(Func<String, IEnumerable<String>?> PermissionsOfRole)
        {
            this.PermissionsOfRole = PermissionsOfRole;
        }

        #endregion


        #region (private) PermissionsOf(Assignment) / OwnerKeysOf(Roles)

        private IEnumerable<String> PermissionsOf(SignerRoleAssignment Assignment)

            => Assignment.Role == OwnerRole
                   ? PermissionsOfRole(Assignment.Role) ?? [ "*:read", "*:edit", "*:run" ]
                   : Assignment.Permissions ?? PermissionsOfRole(Assignment.Role) ?? [];

        private Boolean IsOwner(SignerRoleAssignment Assignment)

            => PermissionsOf(Assignment).Any(permission => SignerPermissions.Covers(permission, OwnerPermission));

        /// <summary>
        /// The keys that may change the roles: those that count now, or will.
        /// </summary>
        private List<SignerRoleKey> OwnerKeysOf(IReadOnlyDictionary<String, SignerRoleAssignment> Roles)
        {

            var now = Now();

            return [.. Roles.Values.
                             Where     (IsOwner).
                             SelectMany(assignment => assignment.Keys).
                             Where     (key => !key.NotAfter.HasValue || key.NotAfter.Value >= now)];

        }

        #endregion

        #region PermissionsOf(Role)

        /// <summary>
        /// What a role may do: what its assignment says, where it says so,
        /// and the node's own definition of the role otherwise.
        /// </summary>
        /// <param name="Role">A role.</param>
        public IEnumerable<String> PermissionsOf(String Role)
        {

            lock (sync)
                return roles.TryGetValue(Role, out var assignment)
                           ? [.. PermissionsOf(assignment)]
                           : [.. PermissionsOfRole(Role) ?? []];

        }

        #endregion

        #region RolesOf(PublicKeyBytes, Used = false)

        /// <summary>
        /// The roles a key may sign as, at this moment.
        /// </summary>
        /// <param name="PublicKeyBytes">A public key as its uncompressed point, as a signature of a message names it.</param>
        /// <param name="Used">Whether this is asked for a signature that holds, so that the roles count as used now.</param>
        public IReadOnlyList<String> RolesOf(Byte[] PublicKeyBytes, Boolean Used = false)
        {

            if (PublicKeyBytes.Length == 0)
                return [];

            var now = Now();

            lock (sync)
            {

                var found = roles.Values.
                                  Where  (assignment => assignment.Keys.Any(key => key.IsValidAt(now) &&
                                                                                   key.PublicKeyBytes.AsSpan().SequenceEqual(PublicKeyBytes))).
                                  Select (assignment => assignment.Role).
                                  OrderBy(role => role, StringComparer.Ordinal).
                                  ToList ();

                if (Used)
                    foreach (var role in found)
                        lastUsed[role] = now;

                return found;

            }

        }

        #endregion

        #region LastUsed(Role)

        /// <summary>
        /// When a signature of a role was last asked for, or null.
        /// </summary>
        public DateTimeOffset? LastUsed(String Role)
        {
            lock (sync)
                return lastUsed.TryGetValue(Role, out var time) ? time : null;
        }

        #endregion

        #region VersionOf(Role)

        /// <summary>
        /// The greatest version a role ever had, 0 for one that never had keys.
        /// </summary>
        public UInt64 VersionOf(String Role)
        {
            lock (sync)
                return versions.TryGetValue(Role, out var version) ? version : 0;
        }

        #endregion


        #region (private) IsSignedByTheOwners(Object, Type, Assignment, out ErrorResponse)

        /// <summary>
        /// Whether enough owners signed the given object - or, while there
        /// is no owner and the first one may come over the network, whether
        /// the object makes one and is signed by a key of its own.
        /// </summary>
        private Boolean IsSignedByTheOwners(JObject                Object,
                                            String                 Type,
                                            SignerRoleAssignment?  Assignment,
                                            out String?            ErrorResponse)
        {

            if (!SignedDataObject.Verify(Object,
                                         Type,
                                         out var signers,
                                         out ErrorResponse,
                                         Now(),
                                         MaxClockSkew))
            {
                return false;
            }

            var ownerKeys = OwnerKeysOf(roles);

            if (ownerKeys.Count == 0)
            {

                if (AllowFirstOwnerByNetwork &&
                    Assignment is not null   &&
                    IsOwner(Assignment)      &&
                    signers.Any(signer => Assignment.Keys.Any(key => key.KeyId == signer.KeyId)))
                {
                    return true;
                }

                ErrorResponse = "This node has no owner yet, and its first owner does not come this way!";
                return false;

            }

            var now       = Now();
            var owners    = signers.Count(signer => ownerKeys.Any(key => key.KeyId == signer.KeyId && key.IsValidAt(now)));

            if (owners < Quorum)
            {
                ErrorResponse = owners == 0
                                    ? "The object is not signed by an owner of this node!"
                                    : $"The object is signed by {owners} of the {Quorum} owners it takes!";
                return false;
            }

            return true;

        }

        #endregion

        #region (private) MayBeAssigned(Assignment, out Reason, out ErrorResponse)

        /// <summary>
        /// What an assignment may say, whoever signed it.
        /// </summary>
        private Boolean MayBeAssigned(SignerRoleAssignment   Assignment,
                                      out SignerRoleReasons  Reason,
                                      out String?            ErrorResponse)
        {

            Reason         = SignerRoleReasons.NotAuthorized;
            ErrorResponse  = null;

            var ofTheNode  = PermissionsOfRole(Assignment.Role)?.ToArray();

            if (Assignment.Role == OwnerRole)
            {

                // What the owner may do is everything, and nobody writes it.
                if (Assignment.Permissions is not null)
                {
                    ErrorResponse = $"What the role '{OwnerRole}' may do cannot be written!";
                    return false;
                }

            }

            else if (ofTheNode is null)
            {

                if (Assignment.Permissions is null || !AllowPermissionsByNetwork)
                {
                    Reason         = SignerRoleReasons.UnknownRole;
                    ErrorResponse  = $"This node has no role '{Assignment.Role}'!";
                    return false;
                }

            }

            else if (Assignment.Permissions is not null && !AllowPermissionsByNetwork)
            {

                // Less than the node says of the role may be said, and no more.
                var beyond = Assignment.Permissions.
                                 Where(permission => !ofTheNode.Any(held => SignerPermissions.Covers(held, permission))).
                                 ToArray();

                if (beyond.Length > 0)
                {
                    ErrorResponse = $"The role '{Assignment.Role}' of this node may not {String.Join(", ", beyond)}!";
                    return false;
                }

            }

            if (Assignment.Keys.Count > MaxKeysPerRole)
            {
                Reason         = SignerRoleReasons.KeyLimitReached;
                ErrorResponse  = $"A role has no more than {MaxKeysPerRole} keys!";
                return false;
            }

            return true;

        }

        #endregion

        #region (private) Store(Assignment, Change, Locally)

        private void Store(SignerRoleAssignment Assignment)
        {
            roles   [Assignment.Role] = Assignment;
            versions[Assignment.Role] = Assignment.Version;
        }

        private void Tell(SignerRoleTableChanges Change, String? Role, UInt64 Version, Boolean Locally)
        {
            try
            {
                OnChanged?.Invoke(new SignerRoleTableChange(Change, Role, Version, Locally));
            }
            catch
            {
                // Whoever listens does not undo what was stored.
            }
        }

        #endregion

        #region (private) TryWrite(JSON, Adding, Locally, out Assignment, out Reason, out ErrorResponse)

        private Boolean TryWrite(JObject                    JSON,
                                 Boolean?                   Adding,
                                 Boolean                    Locally,
                                 out SignerRoleAssignment?  Assignment,
                                 out SignerRoleReasons      Reason,
                                 out String?                ErrorResponse)
        {

            if (!SignerRoleAssignment.TryParse(JSON, out Assignment, out Reason, out ErrorResponse) ||
                Assignment is null)
            {
                return false;
            }

            SignerRoleTableChanges change;

            lock (sync)
            {

                var hadAnOwner  = OwnerKeysOf(roles).Count > 0;
                var exists      = roles.ContainsKey(Assignment.Role);

                if (!MayBeAssigned(Assignment, out Reason, out ErrorResponse))
                    return false;

                Reason = SignerRoleReasons.NotAuthorized;

                if (!IsSignedByTheOwners(JSON, SignedDataObject.SignerRoleType, Assignment, out ErrorResponse))
                    return false;

                if (Adding == true && exists)
                {
                    Reason         = SignerRoleReasons.RoleExists;
                    ErrorResponse  = $"The role '{Assignment.Role}' has keys already!";
                    return false;
                }

                if (Adding == false && !exists)
                {
                    Reason         = SignerRoleReasons.UnknownRole;
                    ErrorResponse  = $"The role '{Assignment.Role}' has no keys that could be replaced!";
                    return false;
                }

                if (versions.TryGetValue(Assignment.Role, out var known) && Assignment.Version <= known)
                {
                    Reason         = SignerRoleReasons.VersionNotNewer;
                    ErrorResponse  = $"The role '{Assignment.Role}' is known in version {known}, and this is version {Assignment.Version}!";
                    return false;
                }

                if (Assignment.Version == 0)
                {
                    Reason         = SignerRoleReasons.VersionNotNewer;
                    ErrorResponse  = "The first version of a role assignment is 1!";
                    return false;
                }

                // Nobody is left without an owner by a change of the roles.
                if (hadAnOwner)
                {

                    var afterwards = new Dictionary<String, SignerRoleAssignment>(roles, StringComparer.Ordinal) {
                                         [Assignment.Role] = Assignment
                                     };

                    if (OwnerKeysOf(afterwards).Count == 0)
                    {
                        Reason         = SignerRoleReasons.WouldLockOut;
                        ErrorResponse  = "This would leave nobody who may change the roles of this node!";
                        return false;
                    }

                }

                Store(Assignment);

                change = !hadAnOwner
                             ? SignerRoleTableChanges.FirstOwnerInstalled
                             : exists
                                   ? SignerRoleTableChanges.RoleUpdated
                                   : SignerRoleTableChanges.RoleAdded;

            }

            Tell(change, Assignment.Role, Assignment.Version, Locally);

            return true;

        }

        #endregion

        #region TryAdd   (JSON, out Reason, out ErrorResponse, Locally = false)

        /// <summary>
        /// Give keys to a role that has none yet.
        /// </summary>
        /// <param name="JSON">A signed role assignment, as received.</param>
        /// <param name="Reason">Why it was refused.</param>
        /// <param name="ErrorResponse">Why it was refused, in words.</param>
        /// <param name="Locally">Whether this happens at the node itself.</param>
        public Boolean TryAdd(JObject                JSON,
                              out SignerRoleReasons  Reason,
                              out String?            ErrorResponse,
                              Boolean                Locally   = false)

            => TryWrite(JSON, true, Locally, out _, out Reason, out ErrorResponse);

        #endregion

        #region TryUpdate(JSON, out Reason, out ErrorResponse, Locally = false)

        /// <summary>
        /// Replace the keys of a role.
        /// </summary>
        /// <param name="JSON">A signed role assignment of a greater version, as received.</param>
        /// <param name="Reason">Why it was refused.</param>
        /// <param name="ErrorResponse">Why it was refused, in words.</param>
        /// <param name="Locally">Whether this happens at the node itself.</param>
        public Boolean TryUpdate(JObject                JSON,
                                 out SignerRoleReasons  Reason,
                                 out String?            ErrorResponse,
                                 Boolean                Locally   = false)

            => TryWrite(JSON, false, Locally, out _, out Reason, out ErrorResponse);

        #endregion

        #region TryImport(JSON, out Reason, out ErrorResponse, Locally = true)

        /// <summary>
        /// Take a role assignment as it is: the keys of a role that has
        /// none, or other keys for one that has.
        /// </summary>
        /// <param name="JSON">A signed role assignment, as read from a file.</param>
        /// <param name="Reason">Why it was refused.</param>
        /// <param name="ErrorResponse">Why it was refused, in words.</param>
        /// <param name="Locally">Whether this happens at the node itself.</param>
        public Boolean TryImport(JObject                JSON,
                                 out SignerRoleReasons  Reason,
                                 out String?            ErrorResponse,
                                 Boolean                Locally   = true)

            => TryWrite(JSON, null, Locally, out _, out Reason, out ErrorResponse);

        #endregion

        #region TryDelete(Role, Version, out Reason, out ErrorResponse, Locally = false)

        /// <summary>
        /// Take the keys of a role away. Who asks for it is not looked at
        /// here: a request to delete carries no object of its own, and is
        /// held to the policy as the request it is.
        /// </summary>
        /// <param name="Role">The role.</param>
        /// <param name="Version">The version that is to be deleted: a delete of another one came too late.</param>
        /// <param name="Reason">Why it was refused.</param>
        /// <param name="ErrorResponse">Why it was refused, in words.</param>
        /// <param name="Locally">Whether this happens at the node itself.</param>
        public Boolean TryDelete(String                 Role,
                                 UInt64                 Version,
                                 out SignerRoleReasons  Reason,
                                 out String?            ErrorResponse,
                                 Boolean                Locally   = false)
        {

            Reason         = SignerRoleReasons.UnknownRole;
            ErrorResponse  = null;

            lock (sync)
            {

                if (!roles.TryGetValue(Role, out var assignment))
                {
                    ErrorResponse = $"The role '{Role}' has no keys!";
                    return false;
                }

                if (assignment.Version != Version)
                {
                    Reason         = SignerRoleReasons.VersionNotNewer;
                    ErrorResponse  = $"The role '{Role}' is stored in version {assignment.Version}, not in version {Version}!";
                    return false;
                }

                var afterwards = new Dictionary<String, SignerRoleAssignment>(roles, StringComparer.Ordinal);
                afterwards.Remove(Role);

                if (OwnerKeysOf(roles).Count > 0 && OwnerKeysOf(afterwards).Count == 0)
                {
                    Reason         = SignerRoleReasons.WouldLockOut;
                    ErrorResponse  = "This would leave nobody who may change the roles of this node!";
                    return false;
                }

                roles.   Remove(Role);
                lastUsed.Remove(Role);

            }

            Tell(SignerRoleTableChanges.RoleDeleted, Role, Version, Locally);

            return true;

        }

        #endregion

        #region TryInstallFirstOwner(JSON, out Reason, out ErrorResponse)

        /// <summary>
        /// Make the first owner of this node, at the node itself: a role
        /// assignment for the owner's role, held to nothing but its form,
        /// since there is nobody yet who could have signed it (section 6.1,
        /// option 2). Not while there is an owner.
        /// </summary>
        /// <param name="JSON">A role assignment for the role of the owner, signed or not.</param>
        /// <param name="Reason">Why it was refused.</param>
        /// <param name="ErrorResponse">Why it was refused, in words.</param>
        public Boolean TryInstallFirstOwner(JObject                JSON,
                                            out SignerRoleReasons  Reason,
                                            out String?            ErrorResponse)
        {

            if (!SignerRoleAssignment.TryParse(JSON, out var assignment, out Reason, out ErrorResponse) ||
                assignment is null)
            {
                return false;
            }

            lock (sync)
            {

                Reason = SignerRoleReasons.NotAuthorized;

                if (OwnerKeysOf(roles).Count > 0)
                {
                    ErrorResponse = "This node has an owner, and only its owners make another!";
                    return false;
                }

                if (!MayBeAssigned(assignment, out Reason, out ErrorResponse))
                    return false;

                Reason = SignerRoleReasons.NotAuthorized;

                if (!IsOwner(assignment))
                {
                    ErrorResponse = $"The role '{assignment.Role}' is no owner of this node: it may not {OwnerPermission}!";
                    return false;
                }

                var now = Now();

                if (!assignment.Keys.Any(key => !key.NotAfter.HasValue || key.NotAfter.Value >= now))
                {
                    Reason         = SignerRoleReasons.InvalidKey;
                    ErrorResponse  = "The role assignment has no key that counts!";
                    return false;
                }

                // Its signatures have to hold where it has any, whoever made them.
                if (!SignedDataObject.Verify(JSON, SignedDataObject.SignerRoleType, out _, out ErrorResponse, now, MaxClockSkew))
                    return false;

                if (assignment.Version == 0 ||
                   (versions.TryGetValue(assignment.Role, out var known) && assignment.Version <= known))
                {
                    Reason         = SignerRoleReasons.VersionNotNewer;
                    ErrorResponse  = $"The role '{assignment.Role}' was known in version {(versions.TryGetValue(assignment.Role, out var was) ? was : 0)} already!";
                    return false;
                }

                Store(assignment);

            }

            Tell(SignerRoleTableChanges.FirstOwnerInstalled, assignment.Role, assignment.Version, true);

            return true;

        }

        #endregion

        #region Reset()

        /// <summary>
        /// Forget every key of every role, and the policy: the way out for
        /// a node whose owners lost their keys, at the node itself. The
        /// versions that were known stay known, so that nothing old comes
        /// back.
        /// </summary>
        public void Reset()
        {

            lock (sync)
            {
                roles.   Clear();
                lastUsed.Clear();
                policy = null;
            }

            Tell(SignerRoleTableChanges.TableReset, null, 0, true);

        }

        #endregion


        #region TrySetPolicy(JSON, Apply, out Reason, out ErrorResponse, Locally = false)

        /// <summary>
        /// Take a policy table: a signed data object with a "version" and
        /// its "entries", signed by the owners.
        /// </summary>
        /// <param name="JSON">The policy, as received.</param>
        /// <param name="Apply">Makes the entries the ones of the node, all of them or none, and says why not.</param>
        /// <param name="Reason">Why it was refused.</param>
        /// <param name="ErrorResponse">Why it was refused, in words.</param>
        /// <param name="Locally">Whether this happens at the node itself.</param>
        public Boolean TrySetPolicy(JObject                        JSON,
                                    Func<JArray, String?>          Apply,
                                    out SignerRoleReasons          Reason,
                                    out String?                    ErrorResponse,
                                    Boolean                        Locally   = false)
        {

            Reason         = SignerRoleReasons.InvalidPolicy;
            ErrorResponse  = null;

            if (JSON["version"]?.Type != JTokenType.Integer ||
                JSON["entries"] is not JArray entries)
            {
                ErrorResponse = "The policy has no version or no entries!";
                return false;
            }

            UInt64 version;

            try
            {
                version = JSON["version"]!.Value<UInt64>();
            }
            catch
            {
                ErrorResponse = "The version of the policy is not a number that counts upwards!";
                return false;
            }

            lock (sync)
            {

                Reason = SignerRoleReasons.NotAuthorized;

                if (!IsSignedByTheOwners(JSON, SignedDataObject.SignaturePolicyType, null, out ErrorResponse))
                    return false;

                if (version <= PolicyVersion)
                {
                    Reason         = SignerRoleReasons.VersionNotNewer;
                    ErrorResponse  = $"The policy is known in version {PolicyVersion}, and this is version {version}!";
                    return false;
                }

                if (Apply(entries) is String whyNot)
                {
                    Reason         = SignerRoleReasons.InvalidPolicy;
                    ErrorResponse  = whyNot;
                    return false;
                }

                policy         = (JObject) JSON.DeepClone();
                PolicyVersion  = version;

            }

            Tell(SignerRoleTableChanges.PolicySet, null, version, Locally);

            return true;

        }

        #endregion


        #region ToJSON()

        /// <summary>
        /// What is to be kept of this table: the role assignments and the
        /// policy as they were received, and the versions that were known.
        /// </summary>
        public JObject ToJSON()
        {

            lock (sync)
            {

                var json = new JObject(

                               new JProperty("roles",     new JArray(roles.Values.
                                                                           OrderBy(assignment => assignment.Role, StringComparer.Ordinal).
                                                                           Select (assignment => assignment.JSON.DeepClone()))),

                               new JProperty("versions",  new JObject(versions.
                                                                          OrderBy(version => version.Key, StringComparer.Ordinal).
                                                                          Select (version => new JProperty(version.Key, version.Value)))),

                               new JProperty("policyVersion", PolicyVersion)

                           );

                if (policy is not null)
                    json.Add("policy", policy.DeepClone());

                return json;

            }

        }

        #endregion

        #region TryLoad(JSON, out ErrorResponse, Apply = null)

        /// <summary>
        /// Read back what ToJSON() wrote. What is read was accepted when it
        /// was received, and is not held to its signers or to the time again;
        /// its signatures still have to hold.
        /// </summary>
        /// <param name="JSON">What was kept of a table.</param>
        /// <param name="ErrorResponse">Why it cannot be read. Nothing of it is taken then.</param>
        /// <param name="Apply">Makes the entries of the policy the ones of the node, and says why not.</param>
        public Boolean TryLoad(JObject                 JSON,
                               out String?             ErrorResponse,
                               Func<JArray, String?>?  Apply   = null)
        {

            ErrorResponse = null;

            var readRoles     = new Dictionary<String, SignerRoleAssignment>(StringComparer.Ordinal);
            var readVersions  = new Dictionary<String, UInt64>              (StringComparer.Ordinal);

            if (JSON["roles"] is not JArray rolesArray)
            {
                ErrorResponse = "No roles!";
                return false;
            }

            foreach (var role in rolesArray)
            {

                if (role is not JObject roleJSON ||
                    !SignerRoleAssignment.TryParse(roleJSON, out var assignment, out _, out ErrorResponse) ||
                    assignment is null)
                {
                    ErrorResponse ??= "A role assignment is not an object!";
                    return false;
                }

                if (!SignedDataObject.Verify(roleJSON, SignedDataObject.SignerRoleType, out _, out ErrorResponse, WithoutTimeWindow: true))
                    return false;

                readRoles   [assignment.Role] = assignment;
                readVersions[assignment.Role] = assignment.Version;

            }

            if (JSON["versions"] is JObject versionsJSON)
                foreach (var version in versionsJSON.Properties())
                {

                    if (version.Value.Type != JTokenType.Integer)
                    {
                        ErrorResponse = $"The version of the role '{version.Name}' is not a number!";
                        return false;
                    }

                    var known = version.Value.Value<UInt64>();

                    if (!readVersions.TryGetValue(version.Name, out var stored) || known > stored)
                        readVersions[version.Name] = known;

                }

            var     readPolicy         = JSON["policy"] as JObject;
            UInt64  readPolicyVersion  = JSON["policyVersion"]?.Type == JTokenType.Integer
                                             ? JSON["policyVersion"]!.Value<UInt64>()
                                             : 0;

            if (readPolicy is not null)
            {

                if (readPolicy["entries"] is not JArray entries ||
                    !SignedDataObject.Verify(readPolicy, SignedDataObject.SignaturePolicyType, out _, out ErrorResponse, WithoutTimeWindow: true))
                {
                    ErrorResponse ??= "The policy has no entries!";
                    return false;
                }

                if (Apply?.Invoke(entries) is String whyNot)
                {
                    ErrorResponse = whyNot;
                    return false;
                }

            }

            lock (sync)
            {

                roles.   Clear();
                versions.Clear();
                lastUsed.Clear();

                foreach (var role    in readRoles)
                    roles.   Add(role.Key,    role.Value);

                foreach (var version in readVersions)
                    versions.Add(version.Key, version.Value);

                policy         = (JObject?) readPolicy?.DeepClone();
                PolicyVersion  = readPolicyVersion;

            }

            return true;

        }

        #endregion

    }

}
