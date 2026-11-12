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

    /// <summary>
    /// The decision chain of the white paper "Signed OCPP Messages and Signer
    /// Roles", chapter 5: once the signatures of a request are verified, who
    /// signed it is held against the policy table, before any handler runs.
    /// </summary>
    /// <remarks>
    /// Every incoming adapter verifies through this policy, so the chain runs
    /// here once for every action. A node says who a key belongs to
    /// (<see cref="SignerRolesOf"/>) and what a role may do
    /// (<see cref="PermissionsOfRole"/>); the roles are the node's own.
    /// The connection role and the peer's rights profile, the first step of the
    /// chain, are not part of it yet.
    /// </remarks>
    public partial class SignaturePolicy
    {

        #region Data

        private readonly List<MessagePolicyEntry>  policyEntries       = [];
        private readonly List<ActionPermission>    actionPermissions   = [];
        private readonly Object                    policyLock          = new();

        #endregion

        #region Properties

        /// <summary>
        /// What this node does with the policy table. "Off" by default: the node
        /// behaves as before it had one.
        /// </summary>
        public MessagePolicyModes                           Mode                  { get; set; }  = MessagePolicyModes.Off;

        /// <summary>
        /// The direction of the requests this node receives, "CSMS->CS" on a
        /// charging station and "CS->CSMS" on a CSMS. Without it, a policy
        /// entry for one direction matches nothing.
        /// </summary>
        public String?                                      IncomingDirection     { get; set; }

        /// <summary>
        /// Who must have signed a request no entry and no permission is about,
        /// in the mode "Enforce". Not set: "AnySigner" for what a CSMS sends a
        /// station, except Heartbeat, and "Unsigned" for everything else (Q5).
        /// </summary>
        public SignerRequirement?                           DefaultRequirement    { get; set; }

        /// <summary>
        /// The roles of the key that made a valid signature. Set by the node.
        /// </summary>
        public Func<Signature, IEnumerable<String>>?        SignerRolesOf         { get; set; }

        /// <summary>
        /// The permissions of a role, "resource:operation", as the node's role
        /// model defines it. Set by the node.
        /// </summary>
        public Func<String, IEnumerable<String>>?           PermissionsOfRole     { get; set; }

        /// <summary>
        /// The resource of the node's role model a device model component belongs
        /// to: "dns" for the DNSClientController. Unknown components are
        /// "configuration".
        /// </summary>
        public Func<String, String>                         ResourceOfComponent   { get; set; }  = DefaultResourceOfComponent;

        /// <summary>
        /// The device model requests whose entries are decided one by one
        /// (<see cref="DecideVariable"/>): for them neither the node's table
        /// nor the default decides the request as a whole.
        /// </summary>
        public HashSet<String>                              ActionsDecidedPerEntry   { get; }  = [];

        /// <summary>
        /// The policy table.
        /// </summary>
        public IEnumerable<MessagePolicyEntry> PolicyEntries
        {
            get
            {
                lock (policyLock)
                    return [.. policyEntries];
            }
        }

        /// <summary>
        /// What each action needs when no policy entry is about it.
        /// </summary>
        public IEnumerable<ActionPermission> ActionPermissions
        {
            get
            {
                lock (policyLock)
                    return [.. actionPermissions];
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// A request was not signed as the policy wants it: refused, or executed
        /// and recorded. Its signatures did not hold, or its signers may not do
        /// what it asks for. A response is not reported: whoever is told of a
        /// violation asks somebody, and the answer to that must not be the next
        /// violation.
        /// </summary>
        public event Action<PolicyViolation>? OnPolicyViolation;

        #endregion


        #region (static) DefaultResourceOfComponent(Component)

        /// <summary>
        /// The resources of the components every node has.
        /// </summary>
        public static String DefaultResourceOfComponent(String Component)

            => Component switch {
                   "DNSClientController"         => "dns",
                   "NTPClientController"         => "nts",
                   "NTSClientController"         => "nts",
                   "MessageSignatureController"  => "signatures",
                   "SignerRoleController"        => "signatures",
                   _                             => "configuration"
               };

        #endregion

        #region AddPolicyEntry / AddPolicyEntries / ClearPolicyEntries

        /// <summary>
        /// Add an entry to the policy table.
        /// </summary>
        /// <exception cref="ArgumentException">The table has an entry of that name, or one a message would match just as well.</exception>
        public SignaturePolicy AddPolicyEntry(MessagePolicyEntry Entry)
        {

            lock (policyLock)
            {

                if (policyEntries.Any(entry => entry.Id == Entry.Id))
                    throw new ArgumentException($"The policy table has an entry '{Entry.Id}' already!", nameof(Entry));

                if (policyEntries.FirstOrDefault(entry => entry.IsAsSpecificAs(Entry)) is MessagePolicyEntry other)
                    throw new ArgumentException($"The policy entry '{Entry.Id}' matches what '{other.Id}' matches, and is as specific: which of them wins?", nameof(Entry));

                policyEntries.Add(Entry);

            }

            return this;

        }

        public SignaturePolicy AddPolicyEntries(IEnumerable<MessagePolicyEntry> Entries)
        {

            foreach (var entry in Entries)
                AddPolicyEntry(entry);

            return this;

        }

        /// <summary>
        /// Add the entries of a policy table as JSON. Either all of them are
        /// taken, or none.
        /// </summary>
        public Boolean TryAddPolicyEntries(JArray                            JSON,
                                           [NotNullWhen(false)] out String?  ErrorResponse)
        {

            var entries = new List<MessagePolicyEntry>();

            foreach (var json in JSON)
            {

                if (json is not JObject entryJSON)
                {
                    ErrorResponse = "A policy entry is an object.";
                    return false;
                }

                if (!MessagePolicyEntry.TryParse(entryJSON, out var entry, out ErrorResponse))
                    return false;

                entries.Add(entry);

            }

            lock (policyLock)
            {

                var before = policyEntries.ToArray();

                try
                {
                    AddPolicyEntries(entries);
                }
                catch (ArgumentException e)
                {
                    policyEntries.Clear();
                    policyEntries.AddRange(before);
                    ErrorResponse = e.Message;
                    return false;
                }

            }

            ErrorResponse = null;
            return true;

        }

        public SignaturePolicy ClearPolicyEntries()
        {

            lock (policyLock)
                policyEntries.Clear();

            return this;

        }

        #endregion

        #region AddActionPermissions / ClearActionPermissions

        /// <summary>
        /// Say what actions need when no policy entry is about them: the table
        /// a kind of node ships.
        /// </summary>
        public SignaturePolicy AddActionPermissions(IEnumerable<ActionPermission> Permissions)
        {

            lock (policyLock)
                actionPermissions.AddRange(Permissions);

            return this;

        }

        public SignaturePolicy ClearActionPermissions()
        {

            lock (policyLock)
                actionPermissions.Clear();

            return this;

        }

        #endregion


        #region (private) Signers

        /// <summary>
        /// A key that made a valid signature, and the roles it holds.
        /// </summary>
        private sealed record Signer(String KeyId, IReadOnlyList<String> Roles);

        /// <summary>
        /// The distinct keys that made the valid signatures of a message.
        /// </summary>
        private List<Signer> SignersOf(ISignableMessage Message)
        {

            var signers = new List<Signer>();

            foreach (var signature in Message.Signatures)
            {

                if (signature.Status != VerificationStatus.ValidSignature)
                    continue;

                var keyId = Convert.ToBase64String(signature.KeyId);

                if (signers.Any(signer => signer.KeyId == keyId))
                    continue;

                signers.Add(new Signer(
                                keyId,
                                [.. (SignerRolesOf?.Invoke(signature) ?? []).Distinct(StringComparer.Ordinal)]
                            ));

            }

            return signers;

        }

        private static Boolean Satisfies(SignerRequirement                 Requirement,
                                         List<Signer>                      Signers,
                                         [NotNullWhen(false)] out String?  Missing)
        {

            Missing = null;

            switch (Requirement.Kind)
            {

                case SignerRequirement.Kinds.Unsigned:
                    return true;

                case SignerRequirement.Kinds.AnySigner:
                    if (Signers.Any(signer => signer.Roles.Count > 0))
                        return true;
                    Missing = Signers.Count == 0
                                  ? "it carries no valid signature"
                                  : "none of its signers holds a role";
                    return false;

                case SignerRequirement.Kinds.AllOf:
                    var absent = Requirement.Roles.Where(role => !Signers.Any(signer => signer.Roles.Contains(role, StringComparer.Ordinal))).ToArray();
                    if (absent.Length == 0)
                        return true;
                    Missing = $"it is not signed by {String.Join(" and ", absent.Select(role => $"'{role}'"))}";
                    return false;

                default:
                    var keys = Signers.Count(signer => signer.Roles.Intersect(Requirement.Roles, StringComparer.Ordinal).Any());
                    if (keys >= Requirement.Count)
                        return true;
                    Missing = $"it is signed by {keys} of the {Requirement.Count} key(s) it needs of {String.Join(" or ", Requirement.Roles.Select(role => $"'{role}'"))}";
                    return false;

            }

        }

        private Boolean Holds(List<Signer> Signers, String Permission)

            => PermissionsOfRole is not null &&
               Signers.Any(signer => signer.Roles.Any(role => PermissionsOfRole(role).Any(held => SignerPermissions.Covers(held, Permission))));

        #endregion

        #region (private) KindOf(Message)

        /// <summary>
        /// What is wrong with the signatures of a message that was refused, or
        /// would have been: the worst of what its signatures were found to be,
        /// and for signatures that hold, whether anybody knows their keys.
        /// </summary>
        private PolicyViolationKinds KindOf(ISignableMessage Message)
        {

            if (!Message.Signatures.Any())
                return PolicyViolationKinds.SignatureMissing;

            var signers = SignersOf(Message);

            if (signers.Count == 0)
            {

                if (Message.Signatures.Any(signature => signature.Status == VerificationStatus.Replayed))
                    return PolicyViolationKinds.Replayed;

                if (Message.Signatures.Any(signature => signature.Status == VerificationStatus.OutOfTime))
                    return PolicyViolationKinds.SignatureExpired;

                if (Message.Signatures.Any(signature => signature.Status == VerificationStatus.UnknownSigner))
                    return PolicyViolationKinds.SignerUnknown;

                return PolicyViolationKinds.SignatureInvalid;

            }

            return signers.All(signer => signer.Roles.Count == 0)
                       ? PolicyViolationKinds.SignerUnknown
                       : PolicyViolationKinds.SignerNotAuthorized;

        }

        #endregion

        #region (private) Report(Message, Action, Entry, Reason, Refused, Kind = null)

        private void Report(ISignableMessage       Message,
                            String                 Action,
                            MessagePolicyEntry?    Entry,
                            String                 Reason,
                            Boolean                Refused,
                            PolicyViolationKinds?  Kind   = null)
        {

            try
            {
                OnPolicyViolation?.Invoke(new PolicyViolation(Action, Entry, Reason, Refused, Kind ?? KindOf(Message), Message));
            }
            catch
            {
                // Whoever listens must not decide what happens to the message.
            }

        }

        #endregion

        #region (private) SignaturesDoNotHold(Message, Reason)

        /// <summary>
        /// Report a request whose signatures did not hold, or that carries none
        /// where a verification rule wants one: it is refused before the policy
        /// table is asked.
        /// </summary>
        private void SignaturesDoNotHold(ISignableMessage  Message,
                                         String            Reason)
        {

            var action = Message switch {
                             IResponse         => null,
                             IRequest request  => request.Action,
                             IMessage message  => message.Action,
                             _                 => null
                         };

            if (action is not null)
                Report(Message, action, null, Reason, Refused: true);

        }

        #endregion

        #region (private) Violated(Message, Action, Entry, Reason, OnViolation, out ErrorResponse, Kind = null)

        /// <summary>
        /// Report a violation, and say whether the message goes on.
        /// </summary>
        /// <returns>Whether the message is executed all the same.</returns>
        private Boolean Violated(ISignableMessage        Message,
                                 String                  Action,
                                 MessagePolicyEntry?     Entry,
                                 String                  Reason,
                                 PolicyViolationActions  OnViolation,
                                 out String?             ErrorResponse,
                                 PolicyViolationKinds?   Kind   = null)
        {

            var refused = Mode == MessagePolicyModes.Enforce &&
                          OnViolation == PolicyViolationActions.Refuse;

            Report(Message, Action, Entry, Reason, refused, Kind);

            ErrorResponse = refused
                                ? Entry is not null
                                      ? $"The policy entry '{Entry.Id}' refuses this '{Action}': {Reason}!"
                                      : $"The signature policy refuses this '{Action}': {Reason}!"
                                : null;

            return !refused;

        }

        #endregion

        #region (private) MostSpecific(Candidates, out Entry, out Ambiguous)

        private static Boolean MostSpecific(List<MessagePolicyEntry>  Candidates,
                                            out MessagePolicyEntry?   Entry,
                                            out String?               Ambiguous)
        {

            Entry      = null;
            Ambiguous  = null;

            if (Candidates.Count == 0)
                return false;

            var highest  = Candidates.Max(candidate => candidate.Specificity);
            var best     = Candidates.Where(candidate => candidate.Specificity == highest).ToArray();

            Entry = best[0];

            if (best.Length > 1)
                Ambiguous = $"the policy entries {String.Join(" and ", best.Select(entry => $"'{entry.Id}'"))} match it alike";

            return true;

        }

        #endregion


        #region Decide(Message, Payload, out ErrorResponse)

        /// <summary>
        /// Hold who signed a verified request against the policy table.
        /// </summary>
        /// <param name="Message">A request, or a message without a response, whose signatures are verified.</param>
        /// <param name="Payload">Its payload as received, for the conditions of the entries.</param>
        /// <param name="ErrorResponse">Why the message is refused.</param>
        /// <returns>Whether the message goes on to its handler.</returns>
        public Boolean Decide(ISignableMessage                  Message,
                              JObject?                          Payload,
                              [NotNullWhen(false)] out String?  ErrorResponse)
        {

            ErrorResponse = null;

            if (Mode == MessagePolicyModes.Off)
                return true;

            var action = Message switch {
                             IRequest request  => request.Action,
                             IMessage message  => message.Action,
                             _                 => null
                         };

            if (action is null)
                return true;

            var signers = SignersOf(Message);

            List<MessagePolicyEntry>  candidates;
            List<ActionPermission>    permissions;

            lock (policyLock)
            {

                candidates   = [.. policyEntries.Where(entry => !entry.IsAboutTheDeviceModel &&
                                                                 entry.AppliesTo(action, IncomingDirection, OCPPVersion) &&
                                                                 entry.Where.All(condition => condition.Matches(Payload)))];

                permissions  = [.. actionPermissions.Where(permission => permission.Action == action &&
                                                                         permission.Where.All(condition => condition.Matches(Payload)))];

            }

            #region A policy entry says who must have signed

            if (MostSpecific(candidates, out var entry, out var ambiguous))
            {

                if (ambiguous is not null)
                    return Violated(Message, action, entry, ambiguous, PolicyViolationActions.Refuse, out ErrorResponse, PolicyViolationKinds.SignerNotAuthorized);

                return Satisfies(entry!.Require, signers, out var missing) ||
                       Violated(Message, action, entry, missing, entry.OnViolation, out ErrorResponse);

            }

            #endregion

            // Its entries are decided one by one, by the handler.
            if (ActionsDecidedPerEntry.Contains(action))
                return true;

            #region The node's table says which permission the action needs

            if (permissions.Count > 0)
            {

                var needed = permissions.MaxBy(permission => permission.Where.Count)!;

                if (needed.RefusedBecause is not null)
                    return Violated(Message, action, null, needed.RefusedBecause, PolicyViolationActions.Refuse, out ErrorResponse, PolicyViolationKinds.SignerNotAuthorized);

                var lacking = needed.Permissions.
                                  Concat(needed.AdditionalPermissions?.Invoke() ?? []).
                                  Distinct(StringComparer.Ordinal).
                                  Where   (permission => !Holds(signers, permission)).
                                  ToArray ();

                return lacking.Length == 0 ||
                       Violated(Message,
                                action,
                                null,
                                signers.Count == 0
                                    ? $"it carries no valid signature, and needs {String.Join(" and ", lacking)}"
                                    : $"none of its signers may {String.Join(" and ", lacking)}",
                                PolicyViolationActions.Refuse,
                                out ErrorResponse);

            }

            #endregion

            #region The default

            var requirement = DefaultRequirement ??
                                  (IncomingDirection == "CSMS->CS" && action != "Heartbeat"
                                       ? SignerRequirement.AnySigner
                                       : SignerRequirement.Unsigned);

            return Satisfies(requirement, signers, out var missingByDefault) ||
                   Violated(Message, action, null, missingByDefault, PolicyViolationActions.Refuse, out ErrorResponse);

            #endregion

        }

        #endregion

        #region DecideVariable(Request, Component, Variable, Instance, EVSE, Operation, out Reason)

        /// <summary>
        /// Decide one entry of a device model request - SetVariables,
        /// GetVariables, SetVariableMonitoring, GetReport - by itself: a
        /// request with entries of different sensitivity is one request.
        /// </summary>
        /// <param name="Request">The verified request.</param>
        /// <param name="Component">The component of the entry.</param>
        /// <param name="Variable">Its variable.</param>
        /// <param name="Instance">The instance of the component, if any.</param>
        /// <param name="EVSE">The EVSE of the component, if any.</param>
        /// <param name="Operation">What is done to it: "read" or "edit".</param>
        /// <param name="Reason">Why the entry is refused.</param>
        /// <returns>Whether the entry is applied.</returns>
        public Boolean DecideVariable(IRequest                          Request,
                                      String                            Component,
                                      String?                           Variable,
                                      String?                           Instance,
                                      Int32?                            EVSE,
                                      String                            Operation,
                                      [NotNullWhen(false)] out String?  Reason)
        {

            Reason = null;

            if (Mode == MessagePolicyModes.Off)
                return true;

            var signers = SignersOf(Request);

            List<MessagePolicyEntry> candidates;

            lock (policyLock)
                candidates = [.. policyEntries.Where(entry => entry.IsAboutTheDeviceModel &&
                                                              entry.AppliesTo(Request.Action, IncomingDirection, OCPPVersion) &&
                                                             (entry.Component is null || entry.Component == Component) &&
                                                             (entry.Variable  is null || entry.Variable  == Variable)  &&
                                                             (entry.Instance  is null || entry.Instance  == Instance)  &&
                                                             (!entry.EVSE.HasValue    || entry.EVSE      == EVSE))];

            var target = $"{Component}{(Instance is not null ? $"[{Instance}]" : "")}{(Variable is not null ? $".{Variable}" : "")}";

            if (MostSpecific(candidates, out var entry, out var ambiguous))
            {

                if (ambiguous is not null)
                    return Violated(Request, Request.Action, entry, $"{target}: {ambiguous}", PolicyViolationActions.Refuse, out Reason, PolicyViolationKinds.SignerNotAuthorized);

                return Satisfies(entry!.Require, signers, out var missing) ||
                       Violated(Request, Request.Action, entry, $"{target}: {missing}", entry.OnViolation, out Reason);

            }

            var permission = $"{ResourceOfComponent(Component)}:{Operation}";

            return Holds(signers, permission) ||
                   Violated(Request,
                            Request.Action,
                            null,
                            signers.Count == 0
                                ? $"{target}: it carries no valid signature, and needs {permission}"
                                : $"{target}: none of its signers may {permission}",
                            PolicyViolationActions.Refuse,
                            out Reason);

        }

        #endregion

    }

}
