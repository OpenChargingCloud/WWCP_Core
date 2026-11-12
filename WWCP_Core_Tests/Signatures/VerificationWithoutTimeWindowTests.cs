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

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.WWCP.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.WWCP.UnitTests.Signatures
{

    /// <summary>
    /// A verification rule without time window: the signatures of its messages are
    /// verified without "iat" and "exp" being held to the clock, and with everything
    /// else checked as before.
    /// </summary>
    /// <remarks>
    /// White paper "Key Pins and TLS 1.3 Raw Public Keys for Local Controllers",
    /// section 5.7: a signed pin set carries a version, and a station without a
    /// synchronized clock takes it by that version, not by a time window it cannot
    /// apply. Each test signs a request as an adapter does, takes the signatures as
    /// they come over the wire, and verifies them with a policy of the receiver
    /// whose clock is far from the signer's. Nothing is sent over a network.
    /// </remarks>
    [TestFixture]
    public class VerificationWithoutTimeWindowTests
    {

        #region (class) ARequest

        /// <summary>
        /// A request of a JSON-LD context of its own, with a pin set as its payload.
        /// </summary>
        private sealed class ARequest : ACustomSignableData,
                                        IRequest
        {

            public JSONLDContext      Context              { get; }
            public SourceRouting      Destination          { get; }
            public NetworkPath        NetworkPath          { get; } = NetworkPath.Empty;
            public Request_Id         RequestId            { get; }
            public DateTimeOffset     RequestTimestamp     { get; } = DateTimeOffset.UtcNow;
            public TimeSpan           RequestTimeout       { get; } = TimeSpan.FromSeconds(30);
            public EventTracking_Id   EventTrackingId      { get; } = EventTracking_Id.New;
            public String             Action               { get; }
            public CancellationToken  CancellationToken    { get; } = default;

            public String             Pins                 { get; }
            public UInt64             Version              { get; }

            public ARequest(JSONLDContext            Context,
                            String                   Action,
                            SourceRouting            Destination,
                            Request_Id               RequestId,
                            String                   Pins,
                            UInt64                   Version,
                            IEnumerable<Signature>?  Signatures   = null)

                : base(Signatures: Signatures)

            {

                this.Context      = Context;
                this.Action       = Action;
                this.Destination  = Destination;
                this.RequestId    = RequestId;
                this.Pins         = Pins;
                this.Version      = Version;

            }

            public JObject ToJSON()

                => new (
                       new JProperty("serverKeyPins",         Pins),
                       new JProperty("serverKeyPinsVersion",  Version)
                   );

            /// <summary>
            /// This request as its receiver has it: the same payload and frame, and the
            /// signatures as they came over the wire - or another frame, where one is given.
            /// </summary>
            public ARequest Received(String?         Action        = null,
                                     SourceRouting?  Destination   = null,
                                     Request_Id?     RequestId     = null)

                => new (Context,
                        Action      ?? this.Action,
                        Destination ?? this.Destination,
                        RequestId   ?? this.RequestId,
                        Pins,
                        Version,
                        [.. Signatures.Select(signature => Signature.TryParse(signature.ToJSON(), out var received, out var errorResponse)
                                                               ? received
                                                               : throw new ArgumentException(errorResponse))]);

        }

        #endregion

        #region Data

        private static readonly JSONLDContext   pinSetContext   = JSONLDContext.Parse("https://open.charging.cloud/context/test/pinSetRequest");
        private static readonly JSONLDContext   resetContext    = JSONLDContext.Parse("https://open.charging.cloud/context/test/resetRequest");

        private static readonly SourceRouting   toTheStation    = SourceRouting.To(NetworkingNode_Id.Parse("cs01"));
        private static readonly SourceRouting   toAnother       = SourceRouting.To(NetworkingNode_Id.Parse("cs02"));
        private static readonly Request_Id      theRequestId    = Request_Id.Parse("4711");

        /// <summary>
        /// The station's clock: a day behind, or a day ahead, of the signer's.
        /// </summary>
        private static readonly DateTimeOffset  signedAt        = DateTimeOffset.UtcNow;

        #endregion

        #region (private) Helpers

        /// <summary>
        /// A pin set request, signed at the given time by the given key.
        /// </summary>
        private static ARequest Signed(ECCKeyPair      Signer,
                                       JSONLDContext?  Context                   = null,
                                       DateTimeOffset? At                        = null,
                                       Boolean         NonceInsteadOfMessageId   = false,
                                       String          Action                    = "SetVariables")
        {

            var request = new ARequest(Context ?? pinSetContext,
                                       Action,
                                       toTheStation,
                                       theRequestId,
                                       "6a1f0c" + new String('0', 58),
                                       3);

            // Signed as the CSMS's adapter signs: bound to its node and its protocol.
            var policy  = new SignaturePolicy() { NonceInsteadOfMessageId = NonceInsteadOfMessageId }.
                              BindTo(NetworkingNode_Id.Parse("csms01"), "ocpp2.1");

            Assert.That(policy.SignRequestMessage(request, request.ToJSON(), out var errorResponse, Signer.ToSignInfo1(Timestamp: At ?? signedAt)),
                        Is.True,
                        errorResponse);

            Assert.That(request.Signatures.Count(), Is.EqualTo(1));

            return request;

        }

        /// <summary>
        /// A station's policy with one rule for pin sets, by the given key, and one
        /// for everything else of the reset context, by the same key; its clock
        /// is the given offset away from the signer's.
        /// </summary>
        private static SignaturePolicy AStation(KeyPair                   PinSetSigner,
                                                Boolean                   WithoutTimeWindow,
                                                TimeSpan                  ClockOffset,
                                                VerificationRuleActions   Action   = VerificationRuleActions.VerifyAny)

            => new (VerificationRules: [
                       new VerificationRule(pinSetContext, 1, Action, PinSetSigner, WithoutTimeWindow),
                       new VerificationRule(resetContext,  1, Action, PinSetSigner)
                   ]) {
                       Now = () => signedAt + ClockOffset
                   };

        /// <summary>
        /// Verify as the receiving adapter does. The error response is "" where there is none, so that
        /// an assertion on it fails rather than throws, and every assertion of a test is told.
        /// </summary>
        private static Boolean Verify(SignaturePolicy  Station,
                                      ARequest         Received,
                                      out String       ErrorResponse)
        {

            var verified   = Station.VerifyRequestMessage(Received, Received.ToJSON(), out var errorResponse);

            ErrorResponse  = errorResponse ?? "";

            return verified;

        }

        #endregion


        #region AnOldSignature_IsTakenWithoutTimeWindow_AndRefusedWithIt(Days)

        /// <summary>
        /// A pin set signed a day before, or a day after, the station's clock says:
        /// out of time with the time window, taken without it.
        /// </summary>
        [TestCase(-1, "expired")]
        [TestCase(+1, "further in the future")]
        public void AnOldSignature_IsTakenWithoutTimeWindow_AndRefusedWithIt(Int32 Days, String Why)
        {

            var signer  = ECCKeyPair.GenerateKeys()!;
            var request = Signed(signer);

            // The station's clock is a day ahead of the signer's (the signature is a
            // day old), or a day behind it (the signature lies a day in the future).
            var clock   = TimeSpan.FromDays(-Days);

            var withoutWindow = Verify(AStation(signer, WithoutTimeWindow: true,  clock), request.Received(), out var errorWithout);
            var withWindow    = Verify(AStation(signer, WithoutTimeWindow: false, clock), request.Received(), out var errorWith);

            Assert.Multiple(() => {

                Assert.That(withoutWindow,  Is.True,                errorWithout);

                Assert.That(withWindow,     Is.False,               "A signature out of time was taken with the time window!");
                Assert.That(errorWith,      Does.Contain("OutOfTime"));
                Assert.That(errorWith,      Does.Contain(Why));

            });

        }

        #endregion

        #region TheTimeWindow_StillGuardsEveryOtherRule()

        /// <summary>
        /// The opt-in belongs to the rule of the pin set's context: a reset signed
        /// a day before is still out of time at the same station.
        /// </summary>
        [Test]
        public void TheTimeWindow_StillGuardsEveryOtherRule()
        {

            var signer   = ECCKeyPair.GenerateKeys()!;
            var station  = AStation(signer, WithoutTimeWindow: true, TimeSpan.FromDays(1));

            var pinSet   = Verify(station, Signed(signer).                       Received(), out var pinSetError);
            var reset    = Verify(station, Signed(signer, Context: resetContext).Received(), out var resetError);

            Assert.Multiple(() => {
                Assert.That(pinSet,      Is.True,  pinSetError);
                Assert.That(reset,       Is.False, "A reset a day old was taken because the pin set's rule has no time window!");
                Assert.That(resetError,  Does.Contain("OutOfTime"));
            });

        }

        #endregion

        #region AReplay_IsStillRefused(NonceInsteadOfMessageId, Action)

        /// <summary>
        /// The same signature a second time is a replay, without time window as
        /// with it - also where its "iat" lies further back than MaxSignatureAge,
        /// so that a replay cache entry kept until "iat" + MaxSignatureAge would be
        /// gone before it was written.
        /// </summary>
        [TestCase(false, VerificationRuleActions.VerifyAny)]
        [TestCase(true,  VerificationRuleActions.VerifyAny)]
        [TestCase(false, VerificationRuleActions.VerifyAll)]
        [TestCase(true,  VerificationRuleActions.VerifyAll)]
        public void AReplay_IsStillRefused(Boolean NonceInsteadOfMessageId, VerificationRuleActions Action)
        {

            var signer   = ECCKeyPair.GenerateKeys()!;
            var request  = Signed(signer, NonceInsteadOfMessageId: NonceInsteadOfMessageId);
            var station  = AStation(signer, WithoutTimeWindow: true, TimeSpan.FromDays(1), Action);

            var first    = Verify(station, request.Received(), out var firstError);
            var replay   = Verify(station, request.Received(), out var replayError);

            Assert.Multiple(() => {
                Assert.That(first,        Is.True,  firstError);
                Assert.That(replay,       Is.False, "The same signed pin set was taken twice!");
                Assert.That(replayError,  Does.Contain("Replayed"));
            });

        }

        #endregion

        #region TheOtherChecks_StillBite()

        /// <summary>
        /// Without time window, a signature by another key, for another
        /// destination, for another action or for another message id is refused
        /// as before.
        /// </summary>
        [Test]
        public void TheOtherChecks_StillBite()
        {

            var signer   = ECCKeyPair.GenerateKeys()!;
            var another  = ECCKeyPair.GenerateKeys()!;
            var clock    = TimeSpan.FromDays(1);

            var byAnotherKey       = Verify(AStation(another, WithoutTimeWindow: true, clock), Signed(signer).Received(),                          out var anotherKeyError);
            var toAnotherStation   = Verify(AStation(signer,  WithoutTimeWindow: true, clock), Signed(signer).Received(Destination: toAnother),    out var anotherStationError);
            var forAnotherAction   = Verify(AStation(signer,  WithoutTimeWindow: true, clock), Signed(signer).Received(Action: "Reset"),           out var anotherActionError);
            var forAnotherMessage  = Verify(AStation(signer,  WithoutTimeWindow: true, clock), Signed(signer).Received(RequestId: Request_Id.Parse("4712")),
                                                                                                                                                  out var anotherMessageError);

            // ...and a station bound to its own node id refuses a pin set for another one,
            // and takes one for itself.
            var atAnotherNode      = Verify(AStation(signer, WithoutTimeWindow: true, clock).BindTo(NetworkingNode_Id.Parse("cs02"), "ocpp2.1"),
                                            Signed(signer).Received(Destination: toAnother),                                                      out var anotherNodeError);

            var atItsNode          = Verify(AStation(signer, WithoutTimeWindow: true, clock).BindTo(NetworkingNode_Id.Parse("cs01"), "ocpp2.1"),
                                            Signed(signer).Received(),                                                                            out var itsNodeError);

            Assert.Multiple(() => {

                Assert.That(atItsNode,            Is.True,  itsNodeError);

                Assert.That(byAnotherKey,         Is.False, "A pin set signed by another key was taken!");
                Assert.That(anotherKeyError,      Does.Contain("UnknownSigner"));

                Assert.That(toAnotherStation,     Is.False, "A pin set signed for another station was taken!");
                Assert.That(anotherStationError,  Does.Contain("WrongFrame"));
                Assert.That(anotherStationError,  Does.Contain("destination"));

                Assert.That(forAnotherAction,     Is.False, "A pin set signed for another action was taken!");
                Assert.That(anotherActionError,   Does.Contain("WrongFrame"));
                Assert.That(anotherActionError,   Does.Contain("action"));

                Assert.That(forAnotherMessage,    Is.False, "A pin set signed for another message id was taken!");
                Assert.That(anotherMessageError,  Does.Contain("WrongFrame"));
                Assert.That(anotherMessageError,  Does.Contain("message id"));

                Assert.That(atAnotherNode,        Is.False, "A pin set signed for cs01 was taken by the node cs02!");
                Assert.That(anotherNodeError,     Does.Contain("WrongFrame"));
                Assert.That(anotherNodeError,     Does.Contain("destination"));

            });

        }

        #endregion

        #region TheRule_IsWrittenAndReadBack()

        /// <summary>
        /// A rule without time window keeps it through its JSON form, and a rule
        /// with it writes nothing new.
        /// </summary>
        [Test]
        public void TheRule_IsWrittenAndReadBack()
        {

            var without  = new VerificationRule(pinSetContext, 7, VerificationRuleActions.VerifyAny, WithoutTimeWindow: true);
            var with     = new VerificationRule(pinSetContext, 7, VerificationRuleActions.VerifyAny);

            Assert.Multiple(() => {

                Assert.That(VerificationRule.TryParse(without.ToJSON(), out var readBack, out var errorResponse), Is.True, errorResponse);
                Assert.That(readBack!.WithoutTimeWindow,                         Is.True);

                Assert.That(with.ToJSON().ContainsKey("withoutTimeWindow"),      Is.False);
                Assert.That(VerificationRule.TryParse(with.ToJSON(), out var readBackWith, out errorResponse), Is.True, errorResponse);
                Assert.That(readBackWith!.WithoutTimeWindow,                     Is.False);

                Assert.That(without,                                             Is.Not.EqualTo(with));

            });

        }

        #endregion

    }

}
