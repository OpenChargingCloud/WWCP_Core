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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    // The policy table of the white paper "Signed OCPP Messages and Signer Roles"
    // (OpenChargingTechnology/Whitepapers, SignedMessages), sections 5.1 to 5.3:
    // who must have signed a message before its handler runs.

    #region MessagePolicyModes

    /// <summary>
    /// What a node does with the policy table.
    /// </summary>
    public enum MessagePolicyModes
    {

        /// <summary>
        /// The table is not looked at: the node behaves as before it had one.
        /// </summary>
        Off,

        /// <summary>
        /// Signatures are verified and the table is evaluated, but a violation
        /// is only reported: the message is executed.
        /// </summary>
        Observe,

        /// <summary>
        /// The table applies: what violates it is refused.
        /// </summary>
        Enforce

    }

    #endregion

    #region PolicyViolationActions

    /// <summary>
    /// What happens to a message that violates a policy entry.
    /// </summary>
    public enum PolicyViolationActions
    {

        /// <summary>
        /// The message is refused, and the violation reported.
        /// </summary>
        Refuse,

        /// <summary>
        /// The message is executed, and the violation reported. For a migration.
        /// </summary>
        Record

    }

    #endregion

    #region SignerRequirement

    /// <summary>
    /// Who must have signed: nobody, anybody with a role, or keys of named roles.
    /// </summary>
    public sealed class SignerRequirement
    {

        public enum Kinds { Unsigned, AnySigner, AnyOf, AllOf }

        /// <summary>
        /// No requirement.
        /// </summary>
        public static readonly SignerRequirement Unsigned   = new (Kinds.Unsigned,  [], 0);

        /// <summary>
        /// A valid signature by a key of any role.
        /// </summary>
        public static readonly SignerRequirement AnySigner  = new (Kinds.AnySigner, [], 1);

        public Kinds                  Kind     { get; }

        /// <summary>
        /// The role ids.
        /// </summary>
        public IReadOnlyList<String>  Roles    { get; }

        /// <summary>
        /// For "anyOf": how many distinct keys of these roles must have signed.
        /// </summary>
        public UInt32                 Count    { get; }

        private SignerRequirement(Kinds Kind, IReadOnlyList<String> Roles, UInt32 Count)
        {
            this.Kind   = Kind;
            this.Roles  = Roles;
            this.Count  = Count;
        }

        /// <summary>
        /// Valid signatures by the given number of distinct keys, each of one of the given roles.
        /// </summary>
        public static SignerRequirement AnyOf(IEnumerable<String> Roles, UInt32 Count = 1)
        {

            var roles = Roles.Distinct(StringComparer.Ordinal).ToArray();

            if (roles.Length == 0)
                throw new ArgumentException("\"anyOf\" names no role!", nameof(Roles));

            if (Count == 0)
                throw new ArgumentException("A quorum of 0 keys asks for nothing: write \"Unsigned\" instead!", nameof(Count));

            return new (Kinds.AnyOf, roles, Count);

        }

        /// <summary>
        /// A valid signature by a key of each of the given roles.
        /// </summary>
        public static SignerRequirement AllOf(IEnumerable<String> Roles)
        {

            var roles = Roles.Distinct(StringComparer.Ordinal).ToArray();

            if (roles.Length == 0)
                throw new ArgumentException("\"allOf\" names no role!", nameof(Roles));

            return new (Kinds.AllOf, roles, (UInt32) roles.Length);

        }

        /// <summary>
        /// "Unsigned", "AnySigner", { "anyOf": [...], "count": n } or { "allOf": [...] }.
        /// </summary>
        public static Boolean TryParse(JToken?                                      JSON,
                                       [NotNullWhen(true)]  out SignerRequirement?  Requirement,
                                       [NotNullWhen(false)] out String?             ErrorResponse)
        {

            Requirement    = null;
            ErrorResponse  = null;

            try
            {

                switch (JSON)
                {

                    case JValue { Type: JTokenType.String } text:
                        switch (text.Value<String>())
                        {
                            case "Unsigned":   Requirement = Unsigned;   return true;
                            case "AnySigner":  Requirement = AnySigner;  return true;
                        }
                        ErrorResponse = $"'{text.Value<String>()}' is not a requirement: \"Unsigned\", \"AnySigner\", or an object with \"anyOf\" or \"allOf\".";
                        return false;

                    case JObject json:

                        var anyOf = json["anyOf"] as JArray;
                        var allOf = json["allOf"] as JArray;

                        if ((anyOf is null) == (allOf is null))
                        {
                            ErrorResponse = "A requirement names its roles under \"anyOf\" or under \"allOf\", not both and not neither.";
                            return false;
                        }

                        if ((anyOf ?? allOf)!.Any(role => role.Type != JTokenType.String || role.Value<String>().IsNullOrEmpty()))
                        {
                            ErrorResponse = "A role is named by its id, a text.";
                            return false;
                        }

                        var roles = (anyOf ?? allOf)!.Select(role => role.Value<String>()!);

                        if (allOf is not null)
                        {

                            if (json["count"] is not null)
                            {
                                ErrorResponse = "\"allOf\" wants every role it names: it takes no \"count\".";
                                return false;
                            }

                            Requirement = AllOf(roles);
                            return true;

                        }

                        var count = 1U;

                        if (json["count"] is JToken countJSON)
                        {

                            if (countJSON.Type != JTokenType.Integer || countJSON.Value<Int64>() < 1 || countJSON.Value<Int64>() > UInt32.MaxValue)
                            {
                                ErrorResponse = "\"count\" is a number of keys, 1 or more.";
                                return false;
                            }

                            count = (UInt32) countJSON.Value<Int64>();

                        }

                        Requirement = AnyOf(roles, count);
                        return true;

                }

                ErrorResponse = "A requirement is \"Unsigned\", \"AnySigner\", or an object with \"anyOf\" or \"allOf\".";
                return false;

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        public JToken ToJSON()

            => Kind switch {
                   Kinds.Unsigned   => new JValue("Unsigned"),
                   Kinds.AnySigner  => new JValue("AnySigner"),
                   Kinds.AllOf      => new JObject(new JProperty("allOf", new JArray(Roles))),
                   _                => new JObject(new JProperty("anyOf", new JArray(Roles)),
                                                   new JProperty("count", Count))
               };

        public override String ToString()

            => Kind switch {
                   Kinds.Unsigned   => "Unsigned",
                   Kinds.AnySigner  => "AnySigner",
                   Kinds.AllOf      => $"all of {String.Join(", ", Roles)}",
                   _                => $"{Count} of {String.Join(", ", Roles)}"
               };

    }

    #endregion

    #region FieldCondition

    /// <summary>
    /// A condition on a field of the payload, by its dotted path, with one of a
    /// closed set of operators: "equals" or "in".
    /// </summary>
    public sealed class FieldCondition
    {

        /// <summary>
        /// The path of the field within the payload, e.g. "chargingProfile.chargingProfilePurpose".
        /// </summary>
        public String                 Path      { get; }

        /// <summary>
        /// The values the field may have; one for "equals".
        /// </summary>
        public IReadOnlyList<String>  Values    { get; }

        /// <summary>
        /// Whether this was written as "equals".
        /// </summary>
        public Boolean                IsEquals  { get; }

        private FieldCondition(String Path, IReadOnlyList<String> Values, Boolean IsEquals)
        {
            this.Path      = Path;
            this.Values    = Values;
            this.IsEquals  = IsEquals;
        }

        public static FieldCondition Equal(String Path, String Value)
            => new (Path, [ Value ], true);

        public static FieldCondition In(String Path, params String[] Values)
            => new (Path, [.. Values.Distinct(StringComparer.Ordinal)], false);

        /// <summary>
        /// The text a field is compared as: a text as it is, a number, a boolean
        /// as JSON writes them. An object, an array and null match nothing.
        /// </summary>
        private static String? TextOf(JToken? Token)

            => Token switch {
                   JValue { Type: JTokenType.String  } text     => text.Value<String>(),
                   JValue { Type: JTokenType.Integer } number   => number.Value<Int64>().ToString(System.Globalization.CultureInfo.InvariantCulture),
                   JValue { Type: JTokenType.Boolean } boolean  => boolean.Value<Boolean>() ? "true" : "false",
                   _                                            => null
               };

        /// <summary>
        /// Whether the payload has the field, with one of the values. A payload
        /// without the field does not match, and neither does no payload at all.
        /// </summary>
        public Boolean Matches(JObject? Payload)
        {

            JToken? token = Payload;

            foreach (var name in Path.Split('.'))
            {

                if (token is not JObject json)
                    return false;

                token = json[name];

            }

            return TextOf(token) is String text &&
                   Values.Contains(text, StringComparer.Ordinal);

        }

        /// <summary>
        /// { "type": { "in": [ "Immediate" ] }, "evseId": { "equals": 1 } }
        /// </summary>
        public static Boolean TryParse(JToken?                                               JSON,
                                       [NotNullWhen(true)]  out IReadOnlyList<FieldCondition>?  Conditions,
                                       [NotNullWhen(false)] out String?                      ErrorResponse)
        {

            Conditions     = null;
            ErrorResponse  = null;

            if (JSON is null || JSON.Type == JTokenType.Null)
            {
                Conditions = [];
                return true;
            }

            if (JSON is not JObject where)
            {
                ErrorResponse = "\"where\" is an object of fields and their conditions.";
                return false;
            }

            var conditions = new List<FieldCondition>();

            foreach (var field in where.Properties())
            {

                if (field.Name.IsNullOrEmpty() || field.Name.Split('.').Any(name => name.Length == 0))
                {
                    ErrorResponse = $"'{field.Name}' is not the path of a field.";
                    return false;
                }

                if (field.Value is not JObject condition || condition.Count != 1)
                {
                    ErrorResponse = $"The condition on '{field.Name}' is an object with one operator: \"equals\" or \"in\".";
                    return false;
                }

                var op = condition.Properties().First();

                switch (op.Name)
                {

                    case "equals":
                        if (TextOf(op.Value) is not String value)
                        {
                            ErrorResponse = $"\"equals\" on '{field.Name}' compares with a text, a whole number or a boolean.";
                            return false;
                        }
                        conditions.Add(Equal(field.Name, value));
                        break;

                    case "in":
                        if (op.Value is not JArray values || values.Count == 0 || values.Any(v => TextOf(v) is null))
                        {
                            ErrorResponse = $"\"in\" on '{field.Name}' compares with a list of texts, whole numbers or booleans, at least one.";
                            return false;
                        }
                        conditions.Add(In(field.Name, [.. values.Select(v => TextOf(v)!)]));
                        break;

                    default:
                        ErrorResponse = $"'{op.Name}' on '{field.Name}' is not an operator. Known operators: equals, in.";
                        return false;

                }

            }

            Conditions = conditions;
            return true;

        }

        public static JObject ToJSON(IEnumerable<FieldCondition> Conditions)

            => new (Conditions.Select(condition => new JProperty(
                                                       condition.Path,
                                                       condition.IsEquals
                                                           ? new JObject(new JProperty("equals", condition.Values[0]))
                                                           : new JObject(new JProperty("in",     new JArray(condition.Values)))
                                                   )));

        /// <summary>
        /// The same field, the same operator and the same values.
        /// </summary>
        public Boolean SameAs(FieldCondition Other)

            => Path == Other.Path &&
               Values.Count == Other.Values.Count &&
               Values.All(value => Other.Values.Contains(value, StringComparer.Ordinal));

        public override String ToString()
            => IsEquals
                   ? $"{Path} = {Values[0]}"
                   : $"{Path} in [{String.Join(", ", Values)}]";

    }

    #endregion

    #region MessagePolicyEntry

    /// <summary>
    /// One entry of the policy table: which messages it is about, who must
    /// have signed them, and what happens when they are not signed so.
    /// </summary>
    public sealed class MessagePolicyEntry
    {

        #region Properties

        public const String AnyAction = "*";

        /// <summary>
        /// The name of this entry, unique within its table.
        /// </summary>
        public String                         Id             { get; }

        /// <summary>
        /// The OCPP action as on the wire, or "*".
        /// </summary>
        public String                         Action         { get; }

        /// <summary>
        /// "CSMS->CS" or "CS->CSMS", or null for both.
        /// </summary>
        public String?                        Direction      { get; }

        /// <summary>
        /// The protocol versions, as WebSocket subprotocols ("ocpp2.1"), or none for all.
        /// </summary>
        public IReadOnlyList<String>          Versions       { get; }

        /// <summary>
        /// For the entries of a device model request: the component, or null.
        /// </summary>
        public String?                        Component      { get; }

        /// <summary>
        /// For the entries of a device model request: the variable, or null.
        /// </summary>
        public String?                        Variable       { get; }

        /// <summary>
        /// For the entries of a device model request: the instance of the component, or null.
        /// </summary>
        public String?                        Instance       { get; }

        /// <summary>
        /// For the entries of a device model request: the EVSE of the component, or null.
        /// </summary>
        public Int32?                         EVSE           { get; }

        /// <summary>
        /// Conditions on fields of the payload. All of them must hold.
        /// </summary>
        public IReadOnlyList<FieldCondition>  Where          { get; }

        /// <summary>
        /// Who must have signed.
        /// </summary>
        public SignerRequirement              Require        { get; }

        /// <summary>
        /// What happens to a message that is not signed so.
        /// </summary>
        public PolicyViolationActions         OnViolation    { get; }

        public String?                        Description    { get; }

        /// <summary>
        /// Whether this entry is about a target within the device model rather
        /// than about a whole message.
        /// </summary>
        public Boolean IsAboutTheDeviceModel
            => Component is not null || Variable is not null || Instance is not null || EVSE.HasValue;

        #endregion

        #region Constructor(s)

        public MessagePolicyEntry(String                        Id,
                                  String                        Action,
                                  SignerRequirement             Require,
                                  PolicyViolationActions        OnViolation   = PolicyViolationActions.Refuse,
                                  String?                       Direction     = null,
                                  IEnumerable<String>?          Versions      = null,
                                  IEnumerable<FieldCondition>?  Where         = null,
                                  String?                       Component     = null,
                                  String?                       Variable      = null,
                                  String?                       Instance      = null,
                                  Int32?                        EVSE          = null,
                                  String?                       Description   = null)
        {

            if (Id.IsNullOrEmpty())
                throw new ArgumentException("A policy entry has a name!", nameof(Id));

            if (Action.IsNullOrEmpty())
                throw new ArgumentException("A policy entry names an action, or \"*\"!", nameof(Action));

            if (Direction is not null && Direction != "CSMS->CS" && Direction != "CS->CSMS")
                throw new ArgumentException($"'{Direction}' is not a direction: \"CSMS->CS\" or \"CS->CSMS\"!", nameof(Direction));

            this.Id           = Id;
            this.Action       = Action;
            this.Require      = Require;
            this.OnViolation  = OnViolation;
            this.Direction    = Direction;
            this.Versions     = [.. (Versions ?? []).Distinct(StringComparer.Ordinal)];
            this.Where        = [.. Where ?? []];
            this.Component    = Component == AnyAction ? null : Component;
            this.Variable     = Variable  == AnyAction ? null : Variable;
            this.Instance     = Instance  == AnyAction ? null : Instance;
            this.EVSE         = EVSE;
            this.Description  = Description;

        }

        #endregion


        #region Specificity

        /// <summary>
        /// How specific this entry is: the most specific one that matches wins.
        /// Component and variable beat component, component beats the action,
        /// an action beats "*", and "where" beats no "where".
        /// </summary>
        public Int32 Specificity

            => (Variable  is not null    ? 32 : 0) +
               (Component is not null    ? 16 : 0) +
               (Instance  is not null    ?  8 : 0) +
               (EVSE.HasValue            ?  4 : 0) +
               (Action    != AnyAction   ?  2 : 0) +
               (Where.Count > 0          ?  1 : 0);

        #endregion

        #region AppliesTo(Action, Direction, Version)

        /// <summary>
        /// Whether this entry is about the given action in the given direction and version.
        /// </summary>
        /// <param name="Direction">The direction of the message, or null when the node has not said which side it is on: an entry for one direction then matches nothing.</param>
        public Boolean AppliesTo(String Action, String? Direction, String? Version)

            => (this.Action == AnyAction || this.Action == Action) &&
               (this.Direction is null   || this.Direction == Direction) &&
               (Versions.Count == 0      || (Version is not null && Versions.Contains(Version, StringComparer.Ordinal)));

        #endregion

        #region CouldBothMatch(Other)

        /// <summary>
        /// Whether a message could match this entry and the other one alike,
        /// with nothing to tell which of them is the more specific: a
        /// configuration error, refused when the entry is written.
        /// </summary>
        public Boolean IsAsSpecificAs(MessagePolicyEntry Other)

            => Action    == Other.Action    &&
               Component == Other.Component &&
               Variable  == Other.Variable  &&
               Instance  == Other.Instance  &&
               EVSE      == Other.EVSE      &&
               (Direction is null || Other.Direction is null || Direction == Other.Direction) &&
               (Versions.Count == 0 || Other.Versions.Count == 0 || Versions.Intersect(Other.Versions, StringComparer.Ordinal).Any()) &&
               Where.Count == Other.Where.Count &&
               Where.All(condition => Other.Where.Any(condition.SameAs));

        #endregion


        #region (static) TryParse(JSON, out Entry, out ErrorResponse)

        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out MessagePolicyEntry?  Entry,
                                       [NotNullWhen(false)] out String?              ErrorResponse)
        {

            Entry          = null;
            ErrorResponse  = null;

            try
            {

                static String? Text(JToken? token)
                    => token is JValue { Type: JTokenType.String } text ? text.Value<String>() : null;

                var id = Text(JSON["id"]);

                if (id.IsNullOrEmpty())
                {
                    ErrorResponse = "A policy entry has an \"id\", a text.";
                    return false;
                }

                if (JSON["match"] is not JObject match)
                {
                    ErrorResponse = $"The policy entry '{id}' has no \"match\".";
                    return false;
                }

                var action = Text(match["action"]);

                if (action.IsNullOrEmpty())
                {
                    ErrorResponse = $"The policy entry '{id}' names no \"action\": an OCPP action as on the wire, or \"*\".";
                    return false;
                }

                foreach (var name in new[] { "direction", "component", "variable", "instance" })
                    if (match[name] is JToken token && Text(token) is null)
                    {
                        ErrorResponse = $"\"{name}\" of the policy entry '{id}' is a text.";
                        return false;
                    }

                var versions = new List<String>();

                if (match["versions"] is JToken versionsJSON)
                {

                    if (versionsJSON is not JArray versionsArray || versionsArray.Any(version => Text(version).IsNullOrEmpty()))
                    {
                        ErrorResponse = $"\"versions\" of the policy entry '{id}' is a list of texts, as \"ocpp2.1\".";
                        return false;
                    }

                    versions.AddRange(versionsArray.Select(version => Text(version)!));

                }

                Int32? evse = null;

                if (match["evse"] is JToken evseJSON)
                {

                    if (evseJSON.Type != JTokenType.Integer || evseJSON.Value<Int64>() < 0 || evseJSON.Value<Int64>() > Int32.MaxValue)
                    {
                        ErrorResponse = $"\"evse\" of the policy entry '{id}' is the number of an EVSE.";
                        return false;
                    }

                    evse = (Int32) evseJSON.Value<Int64>();

                }

                if (!FieldCondition.TryParse(match["where"], out var where, out var whereError))
                {
                    ErrorResponse = $"The policy entry '{id}': {whereError}";
                    return false;
                }

                if (!SignerRequirement.TryParse(JSON["require"], out var require, out var requireError))
                {
                    ErrorResponse = $"The policy entry '{id}': {requireError}";
                    return false;
                }

                var onViolation = PolicyViolationActions.Refuse;

                if (JSON["onViolation"] is JToken onViolationJSON)
                {
                    switch (Text(onViolationJSON))
                    {
                        case "Refuse":  onViolation = PolicyViolationActions.Refuse;  break;
                        case "Record":  onViolation = PolicyViolationActions.Record;  break;
                        default:
                            ErrorResponse = $"\"onViolation\" of the policy entry '{id}' is \"Refuse\" or \"Record\".";
                            return false;
                    }
                }

                Entry = new MessagePolicyEntry(
                            id!,
                            action!,
                            require,
                            onViolation,
                            Text(match["direction"]),
                            versions,
                            where,
                            Text(match["component"]),
                            Text(match["variable"]),
                            Text(match["instance"]),
                            evse,
                            Text(JSON["description"])
                        );

                return true;

            }
            catch (Exception e)
            {
                ErrorResponse = e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON()

        public JObject ToJSON()
        {

            var match = new JObject(new JProperty("action", Action));

            if (Direction is not null)  match.Add("direction", Direction);
            if (Versions.Count > 0)     match.Add("versions",  new JArray(Versions));
            if (Component is not null)  match.Add("component", Component);
            if (Variable  is not null)  match.Add("variable",  Variable);
            if (Instance  is not null)  match.Add("instance",  Instance);
            if (EVSE.HasValue)          match.Add("evse",      EVSE.Value);
            if (Where.Count > 0)        match.Add("where",     FieldCondition.ToJSON(Where));

            var json = new JObject(
                           new JProperty("id",           Id),
                           new JProperty("match",        match),
                           new JProperty("require",      Require.ToJSON()),
                           new JProperty("onViolation",  OnViolation.ToString())
                       );

            if (Description is not null)
                json.Add("description", Description);

            return json;

        }

        #endregion

        public override String ToString()
            => $"'{Id}' ({Action}{(Where.Count > 0 ? $" where {String.Join(" and ", Where)}" : "")}: {Require})";

    }

    #endregion

    #region ActionPermission

    /// <summary>
    /// What an action needs when no policy entry says who must sign it: the
    /// permissions of the node's role model, "resource:operation". A table of
    /// these is what a kind of node ships (section 5.3).
    /// </summary>
    public sealed class ActionPermission
    {

        /// <summary>
        /// The OCPP action as on the wire.
        /// </summary>
        public String                           Action                   { get; }

        /// <summary>
        /// Conditions on fields of the payload. All of them must hold.
        /// </summary>
        public IReadOnlyList<FieldCondition>    Where                    { get; }

        /// <summary>
        /// The permissions a signer needs, all of them: "reset:run".
        /// </summary>
        public IReadOnlyList<String>            Permissions              { get; }

        /// <summary>
        /// When set: the action is refused whoever signed it, for this reason.
        /// </summary>
        public String?                          RefusedBecause           { get; }

        /// <summary>
        /// Further permissions that depend on the state of the node rather than on the
        /// message: what a metrologically relevant station asks for a firmware update.
        /// </summary>
        public Func<IEnumerable<String>>?       AdditionalPermissions    { get; }

        public ActionPermission(String                        Action,
                                IEnumerable<String>           Permissions,
                                IEnumerable<FieldCondition>?  Where                   = null,
                                Func<IEnumerable<String>>?    AdditionalPermissions   = null,
                                String?                       RefusedBecause          = null)
        {

            if (Action.IsNullOrEmpty() || Action == MessagePolicyEntry.AnyAction)
                throw new ArgumentException("A permission is asked for one action!", nameof(Action));

            this.Action                 = Action;
            this.Permissions            = [.. Permissions];
            this.Where                  = [.. Where ?? []];
            this.AdditionalPermissions  = AdditionalPermissions;
            this.RefusedBecause         = RefusedBecause;

            foreach (var permission in this.Permissions)
                if (!SignerPermissions.IsPermission(permission))
                    throw new ArgumentException($"'{permission}' is not a permission: a resource and an operation, as in \"reset:run\"!", nameof(Permissions));

        }

        /// <summary>
        /// The action needs the given permission.
        /// </summary>
        public static ActionPermission Needs(String Action, String Permission, params FieldCondition[] Where)
            => new (Action, [ Permission ], Where);

        /// <summary>
        /// The action is refused, whoever signed it.
        /// </summary>
        public static ActionPermission Refuses(String Action, String Because, params FieldCondition[] Where)
            => new (Action, [], Where, RefusedBecause: Because);

    }

    #endregion

    #region SignerPermissions

    /// <summary>
    /// Permissions as texts, "resource:operation", the way the node's role
    /// model writes them (WWCP_Node, Permission): the operations are read, edit
    /// and run, and "*" stands for every resource.
    /// </summary>
    public static class SignerPermissions
    {

        private static readonly String[] operations = [ "read", "edit", "run" ];

        /// <summary>
        /// Whether the text is a permission: a resource, or "*", and an operation.
        /// </summary>
        public static Boolean IsPermission(String? Text)
        {

            var parts = Text?.Split(':');

            return parts is { Length: 2 }     &&
                   parts[0].Length > 0        &&
                   parts[0] == parts[0].Trim().ToLowerInvariant() &&
                   operations.Contains(parts[1], StringComparer.Ordinal);

        }

        /// <summary>
        /// Whether a held permission lets somebody do what the needed one says.
        /// </summary>
        public static Boolean Covers(String Held, String Needed)
        {

            var held    = Held.  Trim().ToLowerInvariant().Split(':');
            var needed  = Needed.Trim().ToLowerInvariant().Split(':');

            return held.  Length == 2 &&
                   needed.Length == 2 &&
                   held[1] == needed[1] &&
                  (held[0] == "*" || held[0] == needed[0]);

        }

    }

    #endregion

    #region PolicyViolation

    /// <summary>
    /// A message that was not signed as the policy wants it.
    /// </summary>
    /// <param name="Action">The OCPP action.</param>
    /// <param name="Entry">The policy entry that was violated, or null when it was a permission of the node's table or the default.</param>
    /// <param name="Reason">What was missing.</param>
    /// <param name="Refused">Whether the message was refused; otherwise it was executed and the violation recorded.</param>
    public sealed record PolicyViolation(String               Action,
                                         MessagePolicyEntry?  Entry,
                                         String               Reason,
                                         Boolean              Refused);

    #endregion

}
