///<reference path="../../../../Hermod/Hermod/Hermod/HTTP/HTTPAPI/HTTPRoot/ts/date.format.ts" />
function StartDebugLog() {
    const connectionColors = {};
    const eventsDiv = document.getElementById('eventsDiv');
    const streamFilterInput = document.getElementById('eventsFilterDiv').getElementsByTagName('input')[0];
    streamFilterInput.onchange = () => {
        const allLogLines = eventsDiv.getElementsByClassName('logLine');
        for (let i = 0; i < allLogLines.length; i++) {
            if (allLogLines[i].innerHTML.indexOf(streamFilterInput.value) > -1)
                allLogLines[i].style.display = 'table-row';
            else
                allLogLines[i].style.display = 'none';
        }
    };
    function GetConnectionColors(connectionId) {
        const colors = connectionColors[connectionId];
        if (colors !== undefined)
            return colors;
        else {
            const red = Math.floor(Math.random() * 80 + 165).toString(16);
            const green = Math.floor(Math.random() * 80 + 165).toString(16);
            const blue = Math.floor(Math.random() * 80 + 165).toString(16);
            const connectionColor = red + green + blue;
            connectionColors[connectionId] = new Object();
            connectionColors[connectionId].textcolor = "000000";
            connectionColors[connectionId].background = connectionColor;
            return connectionColors[connectionId];
        }
    }
    function CreateLogEntry(timestamp, roamingNetwork, eventTrackingId, from, to, command, message, connectionColorKey) {
        const connectionColor = GetConnectionColors(connectionColorKey);
        if (typeof message === 'string') {
            message = [message];
        }
        const div = document.createElement('div');
        div.className = "logLine";
        div.style.color = "#" + connectionColor.textcolor;
        div.style.background = "#" + connectionColor.background;
        div.innerHTML = "<div class=\"timestamp\">" + new Date(timestamp).format('dd.mm.yyyy HH:MM:ss') + "</div>" +
            "<div class=\"roamingNetwork\">" + roamingNetwork + "</div>" +
            "<div class=\"eventTrackingId\">" + eventTrackingId + "</div>" +
            "<div class=\"from\">" + (from !== null && from !== void 0 ? from : "") + "</div>" +
            "<div class=\"to\">" + (to !== null && to !== void 0 ? to : "") + "</div>" +
            "<div class=\"command\">" + command + "</div>" +
            "<div class=\"message\">" + message.reduce(function (a, b) { return a + "<br />" + b; });
        +"</div>";
        if (div.innerHTML.indexOf(streamFilterInput.value) > -1)
            div.style.display = 'table-row';
        else
            div.style.display = 'none';
        eventsDiv.insertBefore(div, eventsDiv.firstChild);
    }
    function AppendLogEntry(timestamp, roamingNetwork, eventTrackingId, message, runtime) {
        const searchPattern = "\"eventTrackingId\">" + eventTrackingId;
        const allLogLines = eventsDiv.getElementsByClassName('logLine');
        for (let i = 0; i < allLogLines.length; i++) {
            if (allLogLines[i].innerHTML.indexOf(searchPattern) > -1) {
                allLogLines[i].getElementsByClassName("message")[0].innerHTML += `${message} [${runtime} ms]`;
                break;
            }
        }
    }
    const eventSource = window.EventSource !== undefined
        ? new EventSource('debugLog')
        : null;
    if (eventSource !== null) {
        // Will only be called for events without an event type!
        eventSource.onmessage = function (event) {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                const [key, value] = entries[0];
                const container = document.createElement('div');
                container.className = 'OnMessage';
                const keyDiv = document.createElement('div');
                keyDiv.className = 'key';
                keyDiv.textContent = String(key);
                const valueDiv = document.createElement('div');
                valueDiv.className = 'value';
                valueDiv.textContent = value == null ? '' : String(value);
                container.append(keyDiv, valueDiv);
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", "", "", "OnMessage", container.outerHTML, (_d = request.EVSEId) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                console.error(exception);
            }
        };
        eventSource.onerror = function (event) {
            console.debug(event);
        };
        eventSource.addEventListener('sub1', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                const [key, value] = entries[0];
                const container = document.createElement('div');
                container.className = 'sub1';
                const keyDiv = document.createElement('div');
                keyDiv.className = 'key';
                keyDiv.textContent = String(key);
                const valueDiv = document.createElement('div');
                valueDiv.className = 'value';
                valueDiv.textContent = value == null ? '' : String(value);
                container.append(keyDiv, valueDiv);
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", "", "", "sub1", container.outerHTML, (_d = request.EVSEId) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                console.error(exception);
            }
        }, false);
        eventSource.addEventListener('OnSetEVSEAdminStatusHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnSetEVSEAdminStatusHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnSetEVSEAdminStatusHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnSetEVSEAdminStatusHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnSetEVSEAdminStatusHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnSetEVSEStatusHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnSetEVSEStatusHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnSetEVSEStatusHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnSetEVSEStatusHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnSetEVSEStatusHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeStartRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnAuthorizeStartRequest", `${request.localAuthentication.authToken} @'${request.chargingLocation.evseId}'`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeStartRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeStartResponse', (event) => {
            var _a, _b;
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ ${response.result.result} (${(_a = response.result.sessionId) !== null && _a !== void 0 ? _a : "-"}) @'${(_b = response.result.providerId) !== null && _b !== void 0 ? _b : "-"} / ${response.result.authorizatorId}': ${response.result.description}`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeStartResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnAuthorizeHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('SendAuthorizeHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('SendAuthorizeHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeStartEVSEHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnAuthorizeStartEVSEHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeStartEVSEHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeStartEVSEHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeStartEVSEHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeStopEVSEHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnAuthorizeStopEVSEHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeStopEVSEHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnAuthorizeStopEVSEHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnAuthorizeStopEVSEHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnRemoteStartEVSEHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnRemoteStartEVSEHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnRemoteStartEVSEHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnRemoteStartEVSEHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnRemoteStartEVSEHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnRemoteStopEVSEHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnRemoteStopEVSEHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnRemoteStopEVSEHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnRemoteStopEVSEHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnRemoteStopEVSEHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnChargeDetailRecordsHTTPRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnChargeDetailRecordsHTTPRequest", `${request.evseId} (${JSON.stringify(request.patch)}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnChargeDetailRecordsHTTPRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnChargeDetailRecordsHTTPResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnChargeDetailRecordsHTTPResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPutSessionRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnPutSessionRequest", `${request.id} (${request.status}, ${request.kwh}, ${request.total_cost} ${request.currency})`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnPutSessionRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPutSessionResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnPutSessionResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPatchSessionRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnPatchSessionRequest", `${request.sessionId}`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnPatchSessionRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPatchSessionResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ !`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnPatchSessionResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPostCDRRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnPostCDRRequest", `${request.id} (${request.session_id}, ${request.cdr_token})`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnPostCDRRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPostCDRResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ ${response.cdrLocation}`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnPostCDRResponse', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPostTokenRequest', (event) => {
            var _a, _b, _c, _d;
            try {
                const request = JSON.parse(event.data);
                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;
                CreateLogEntry((_a = request.timestamp) !== null && _a !== void 0 ? _a : Date.now(), (_b = request.roamingNetworkId) !== null && _b !== void 0 ? _b : "", (_c = request.eventTrackingId) !== null && _c !== void 0 ? _c : "", request.from, request.to, "OnPostTokenRequest", `${request.tokenId} (${request.requestedTokenType})`, (_d = request.from) !== null && _d !== void 0 ? _d : "" // ConnectionColorKey
                );
            }
            catch (exception) {
                ShowHTTPSSEError('OnPostTokenRequest', event.data, exception);
            }
        }, false);
        eventSource.addEventListener('OnPostTokenResponse', (event) => {
            try {
                const response = JSON.parse(event.data);
                AppendLogEntry(response.timestamp, response.roamingNetwork, response.eventTrackingId, `⇒ ${response.result.allowed}`, response.runtime);
            }
            catch (exception) {
                ShowHTTPSSEError('OnPostTokenResponse', event.data, exception);
            }
        }, false);
    }
    function ShowHTTPSSEError(command, data, exception) {
        const e2 = exception instanceof Error
            ? exception
            : new Error(String(exception));
        CreateLogEntry(Date.now(), "", "", "", "", "Error", `${command} (${JSON.stringify(data)}) ⇒ ${e2}`, "" // ConnectionColorKey
        );
        console.debug(command);
        console.debug(data);
        console.debug(e2);
    }
}
//# sourceMappingURL=debugLog.js.map