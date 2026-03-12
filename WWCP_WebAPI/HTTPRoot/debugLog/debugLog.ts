///<reference path="../../../../Hermod/Hermod/Hermod/HTTP/HTTPAPI/HTTPRoot/ts/date.format.ts" />

function StartDebugLog() {

    const connectionColors   = {};
    const eventsDiv          = document.getElementById('eventsDiv');
    const streamFilterInput  = document.getElementById('eventsFilterDiv').getElementsByTagName('input')[0] as HTMLInputElement;

    // Live filtering as you type...
    streamFilterInput.oninput = () => {

        compileFilter(streamFilterInput.value);

        const allLogLines = Array.from(eventsDiv.getElementsByClassName('logLine') as HTMLCollectionOf<HTMLDivElement>);

        for (let i = 0; i < allLogLines.length; i++) {
            allLogLines[i].style.display =
                matchesFilter(allLogLines[i].innerHTML)
                    ? 'table-row'
                    : 'none';
        }

    };


    const clearButton       = document.getElementById('clearEventsButton');
    clearButton.onclick = () => {
        eventsDiv.innerHTML = '';
    };

    // ── Filter help button & panel ──────────────────────────────────

    const filterHelpPanel   = document.getElementById('filterHelpPanel');
    const filterHelpButton  = document.getElementById('filterHelpButton');
    filterHelpButton.onclick = () => {
        filterHelpPanel.classList.toggle('visible');
    };



    // ── Filter expression engine ──────────────────────────────────────
    function tokenizeFilter(input: string | any[])
    {

        const tokens = [];
        let   i      = 0;

        while (i < input.length)
        {

            const ch = input[i];

            // Skip whitespace
            if (ch === ' ' || ch === '\t') { i++; continue; }

            // Single-character operators
            if (ch === '(') { tokens.push({ type: 'LPAREN' }); i++; continue; }
            if (ch === ')') { tokens.push({ type: 'RPAREN' }); i++; continue; }
            if (ch === '&') { tokens.push({ type: 'AND'    }); i++; continue; }
            if (ch === '|') { tokens.push({ type: 'OR'     }); i++; continue; }
            if (ch === '!') { tokens.push({ type: 'NOT'    }); i++; continue; }

            // Quoted string: "..."
            if (ch === '"') {
                let str = '';
                i++; // skip opening quote
                while (i < input.length && input[i] !== '"') {
                    str += input[i];
                    i++;
                }
                i++; // skip closing quote
                tokens.push({ type: 'STRING', value: str });
                continue;
            }

            // Regex literal: /pattern/flags
            if (ch === '/') {
                let pattern = '';
                i++; // skip opening /
                while (i < input.length && input[i] !== '/') {
                    if (input[i] === '\\\\' && i + 1 < input.length) {
                        pattern += input[i] + input[i + 1];
                        i += 2;
                    } else {
                        pattern += input[i];
                        i++;
                    }
                }
                i++; // skip closing /
                let flags = '';
                while (i < input.length && /[gimsuy]/.test(input[i])) {
                    flags += input[i];
                    i++;
                }
                tokens.push({ type: 'REGEX', pattern, flags });
                continue;
            }

            // Bare word (unquoted substring) — everything until a special char or whitespace
            let word = '';
            while (i < input.length && !'(&|)! \t"'.includes(input[i])) {
                word += input[i];
                i++;
            }
            if (word.length > 0) {
                tokens.push({ type: 'STRING', value: word });
            }

        }

        return tokens;

    }

    function parseFilter(input: string): FilterAST
    {

        // An empty filter matches everything
        const trimmed = input.trim();
        if (trimmed === '')
            return { type: 'TRUE' };

        let   pos    = 0;
        const tokens = tokenizeFilter(trimmed);

        // Strip trailing binary operators (& / |) that have no right operand
        while (tokens.length > 0 &&
              (tokens[tokens.length - 1].type === 'AND' ||
               tokens[tokens.length - 1].type === 'OR')) {
            tokens.pop();
        }

        // Match everything, when stripping left us with no tokens
        if (tokens.length === 0)
            return { type: 'TRUE' };


        function peek() {

            return pos < tokens.length
                       ? tokens[pos]
                       : null;

        }

        function consume(expectedType: string)
        {

            const t = tokens[pos];

            if (expectedType && (!t || t.type !== expectedType))
                throw new Error(`Expected ${expectedType} at position ${pos}`);

            pos++;
            return t;

        }

        // Grammar (precedence low → high):
        //   expr     = andExpr ( '|' andExpr )*
        //   andExpr  = notExpr ( '&' notExpr )*
        //   notExpr  = '!' notExpr | primary
        //   primary  = '(' expr ')' | STRING | REGEX
        function parseExpr()
        {
            let left = parseAndExpr();
            while (peek() && peek().type === 'OR') {
                consume('OR');
                const right = parseAndExpr();
                left = { type: 'OR', left, right };
            }
            return left;
        }

        function parseAndExpr()
        {

            let left = parseNotExpr();

            while (peek() && peek().type === 'AND') {
                consume('AND');
                const right = parseNotExpr();
                left = { type: 'AND', left, right };
            }

            return left;

        }

        function parseNotExpr()
        {

            if (peek() && peek().type === 'NOT') {
                consume('NOT');
                const operand = parseNotExpr(); // right-recursive for !!x
                return { type: 'NOT', operand };
            }

            return parsePrimary();

        }

        function parsePrimary()
        {

            const t = peek();

            if (!t)
                throw new Error('Unexpected end of filter expression');

            if (t.type === 'LPAREN') {
                consume('LPAREN');
                const expr = parseExpr();
                consume('RPAREN');
                return expr;
            }

            if (t.type === 'STRING') {
                consume('STRING');
                return { type: 'SUBSTR', value: t.value.toLowerCase() };
            }

            if (t.type === 'REGEX') {
                consume('REGEX');
                return { type: 'REGEX', regex: new RegExp(t.pattern, t.flags) };
            }

            throw new Error(`Unexpected token: ${t.type}`);

        }

        const ast = parseExpr();

        if (pos < tokens.length)
            throw new Error(`Unexpected token at position ${pos}: ${tokens[pos].type}`);

        return ast;

    }

    function evalFilter(ast: FilterAST, text: string): boolean
    {
        switch (ast.type) {
            case 'TRUE':   return true;
            case 'SUBSTR': return text.toLowerCase().includes(ast.value);
            case 'REGEX':  return ast.regex.test(text);
            case 'NOT':    return !evalFilter(ast.operand, text);
            case 'AND':    return  evalFilter(ast.left, text) && evalFilter(ast.right, text);
            case 'OR':     return  evalFilter(ast.left, text) || evalFilter(ast.right, text);
            default:       return true;
        }
    }

    // Compile once, evaluate many times
    type FilterAST =
        | { type: 'TRUE' }
        | { type: 'SUBSTR'; value:   string    }
        | { type: 'REGEX';  regex:   RegExp    }
        | { type: 'NOT';    operand: FilterAST }
        | { type: 'AND';    left:    FilterAST; right: FilterAST }
        | { type: 'OR';     left:    FilterAST; right: FilterAST };

    let currentFilterAST: FilterAST = { type: 'TRUE' };

    function compileFilter(filterString: string)
    {
        try
        {

            currentFilterAST = parseFilter(filterString);

        } catch (e) {

            console.warn('Invalid filter expression:', e.message);

            // On syntax error, fall back to simple substring match
            const val = filterString.trim().toLowerCase();
            currentFilterAST = val === ''
                ? { type: 'TRUE' }
                : { type: 'SUBSTR', value: val };

        }
    }

    function matchesFilter(innerHTML: string) {
        return evalFilter(currentFilterAST, innerHTML);
    }







    function GetConnectionColors(connectionId: string | number) {

        const colors = connectionColors[connectionId];

        if (colors !== undefined)
            return colors;

        else
        {

            const red   = Math.floor(Math.random() * 80 + 165).toString(16);
            const green = Math.floor(Math.random() * 80 + 165).toString(16);
            const blue  = Math.floor(Math.random() * 80 + 165).toString(16);

            const connectionColor = red + green + blue;

            connectionColors[connectionId]             = new Object();
            connectionColors[connectionId].textcolor   = "000000";
            connectionColors[connectionId].background  = connectionColor;

            return connectionColors[connectionId];

        }

    }

    function CreateLogEntry(timestamp: string | number | Date, roamingNetwork: string, eventTrackingId: string, from: string, to: string, command: string, message: string | any[], connectionColorKey: string) {

        const connectionColor = GetConnectionColors(connectionColorKey);

        if (typeof message === 'string') {
            message = [message];
        }

        const div = document.createElement('div');
        div.className         = "logLine";
        div.style.color       = "#" + connectionColor.textcolor;
        div.style.background  = "#" + connectionColor.background;
        div.innerHTML         = "<div class=\"timestamp\">"       + new Date(timestamp).format('dd.mm.yyyy HH:MM:ss') + "</div>" +
                                "<div class=\"roamingNetwork\">"  + roamingNetwork  + "</div>" +
                                "<div class=\"eventTrackingId\">" + eventTrackingId + "</div>" +
                                "<div class=\"from\">"            + (from ?? "")    + "</div>" +
                                "<div class=\"to\">"              + (to   ?? "")    + "</div>" +
                                "<div class=\"command\">"         + command + "</div>" +
                                "<div class=\"message\">"         + message.reduce(function (a: string, b: string) { return a + "<br />" + b; }); + "</div>";

        //if (div.innerHTML.indexOf(streamFilterInput.value) > -1)
        //    div.style.display = 'table-row';
        //else
        //    div.style.display = 'none';

        div.style.display = matchesFilter(div.innerHTML) ? 'table-row' : 'none';


        eventsDiv.insertBefore(div, eventsDiv.firstChild);

    }

    function AppendLogEntry(timestamp: any, roamingNetwork: any, eventTrackingId: string, message: string, runtime: any) {

        const searchPattern  = "\"eventTrackingId\">" + eventTrackingId;
        const allLogLines    = eventsDiv.getElementsByClassName('logLine');

        for (let i = 0; i < allLogLines.length; i++) {
            if (allLogLines[i].innerHTML.indexOf(searchPattern) > -1) {
                allLogLines[i].getElementsByClassName("message")[0].innerHTML += `${message} [${runtime} ms]`;
                break;
            }
        }

    }



    const randomBytes  = new Uint8Array(16);
    window.crypto.getRandomValues(randomBytes);
    const streamId     = btoa(String.fromCharCode(...randomBytes)).
                         replace(/\+/g, '-').
                         replace(/\//g, '_').
                         replace(/=+$/, '');

    const eventSource  = window.EventSource !== undefined
                             ? new EventSource(`debugLog?streamId=${streamId}`)
                             : null;

    if (eventSource !== null)
    {

        // Will only be called for events without an event type!
        eventSource.onmessage = function (event) {

            try
            {

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


                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    "",
                    "",
                    "OnMessage",
                    container.outerHTML,
                    request.EVSEId ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                console.error(exception);
            }

        };

        eventSource.onerror = function (event) {
            console.debug(event);
        };


        // -- SetEVSE(Admin)StatusRequest/-Response ----------------------------------------------------------------

        eventSource.addEventListener('OnSetEVSEAdminStatusRequest', (event: MessageEvent<string>) => {

            try {
                function printEVSEStatus(evseStatus: any) {
                    return `${evseStatus.id}: ${evseStatus.status}`
                }

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnSetEVSEAdminStatusRequest",
                    `${request.evseStatusList.map(printEVSEStatus).join("; ")}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEAdminStatusRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnSetEVSEAdminStatusResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.result}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEAdminStatusResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnSetEVSEAdminStatusHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnSetEVSEAdminStatusHTTPRequest",
                    `${request.evseId} (${JSON.stringify(request.patch)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEAdminStatusHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnSetEVSEAdminStatusHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEAdminStatusHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);


        eventSource.addEventListener('OnSetEVSEStatusRequest', (event: MessageEvent<string>) => {

            try
            {
                function printEVSEStatus(evseStatus: any) {
                    return `${evseStatus.id}: ${evseStatus.status}`
                }

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp        ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId  ?? "",
                    request.from,
                    request.to,
                    "OnSetEVSEStatusRequest",
                    `${request.evseStatusList.map(printEVSEStatus).join("; ") }`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEStatusRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnSetEVSEStatusResponse', (event: MessageEvent<string>) => {

            try
            {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.result}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEStatusResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnSetEVSEStatusHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnSetEVSEStatusHTTPRequest",
                    `${request.evseId} (${JSON.stringify(request.patch)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEStatusHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnSetEVSEStatusHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnSetEVSEStatusHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);



        // -- TokenRequest/-Response ---------------------------------------------------------------------------------

        eventSource.addEventListener('OnPostTokenRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnPostTokenRequest",
                    `${request.tokenId} (${request.requestedTokenType})`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnPostTokenRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnPostTokenResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.allowed}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnPostTokenResponse',
                    event.data,
                    exception
                );
            }

        }, false);



        // -- AuthorizeStart/-Stop ---------------------------------------------------------------------------------

        eventSource.addEventListener('OnAuthorizeStartRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp        ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId  ?? "",
                    request.from,
                    request.to,
                    "OnAuthorizeStartRequest",
                    `${request.localAuthentication.authToken} @'${request.chargingLocation.evseId}'`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStartRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnAuthorizeStartResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.result} @'${response.result.providerId ?? "-"} / ${response.result.authorizatorId}' (${response.result.sessionId ?? "-"}): ${response.result.description.en}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStartResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnAuthorizeStartEVSEHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp        ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId  ?? "",
                    request.from,
                    request.to,
                    "OnAuthorizeStartEVSEHTTPRequest",
                    `${request.evseId} (${JSON.stringify(request.patch)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStartEVSEHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnAuthorizeStartEVSEHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStartEVSEHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);


        eventSource.addEventListener('OnAuthorizeStopRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnAuthorizeStopRequest",
                    `${request.localAuthentication.authToken} @'${request.chargingLocation.evseId}'`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStopRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnAuthorizeStopResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.result} @'${response.result.providerId ?? "-"} / ${response.result.authorizatorId}' (${response.result.sessionId ?? "-"}): ${response.result.description.en}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStopResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnAuthorizeStopEVSEHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnAuthorizeStopEVSEHTTPRequest",
                    `${request.evseId} (${JSON.stringify(request.patch)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStopEVSEHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnAuthorizeStopEVSEHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnAuthorizeStopEVSEHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);



        // -- RemoteStart/-Stop ---------------------------------------------------------------------------------

        eventSource.addEventListener('OnRemoteStartRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnRemoteStartRequest",
                    `${request.remoteAuthentication.remoteIdentification} (${request.providerId}) @'${request.chargingLocation.evseId}' (${request.csoRoamingProviderId ?? "-"} / ${request.sessionId ?? "-"})`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStartRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnRemoteStartResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.result}: ${response.result.description.en}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStartResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnRemoteStartEVSEHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnRemoteStartEVSEHTTPRequest",
                    `${request.evseId} (${JSON.stringify(request.patch)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStartEVSEHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnRemoteStartEVSEHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStartEVSEHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);


        eventSource.addEventListener('OnRemoteStopRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnRemoteStopRequest",
                    `${request.remoteAuthentication.remoteIdentification ?? "-"} (${request.providerId}) @'${request.chargingLocation.evseId}' (${request.csoRoamingProviderId ?? "-"} / ${request.sessionId})`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStopRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnRemoteStopResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${response.result.result}: ${response.result.description.en}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStopResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnRemoteStopEVSEHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnRemoteStopEVSEHTTPRequest",
                    `${request.evseId} (${JSON.stringify(request.patch)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStopEVSEHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnRemoteStopEVSEHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnRemoteStopEVSEHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);



        // -- Put/PatchSessionRequest/-Response -------------------------------------------------------------------

        eventSource.addEventListener('OnPutOCPISessionHTTPRequest', (event: MessageEvent<string>) => {

            try
            {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnPutOCPISessionHTTPRequest",
                    `${JSON.stringify(request)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnPutOCPISessionHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnPutOCPISessionHTTPResponse', (event: MessageEvent<string>) => {

            try
            {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnPutOCPISessionHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);



        // -- ChargeDetailRecordsRequest/-Response ----------------------------------------------------------------

        eventSource.addEventListener('OnChargeDetailRecordsRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                const chargeDetailRecord = request.chargeDetailRecords.firstOrDefault();

                CreateLogEntry(
                    request.timestamp        ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId  ?? "",
                    request.from,
                    request.to,
                    "OnChargeDetailRecordsRequest",
                    `${chargeDetailRecord["@id"]} (${request.sessionId}, ${request.evseId}, ${request.providerIdStart}, ${request.authenticationStart})`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnChargeDetailRecordsRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnChargeDetailRecordsResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                const result = response.results.firstOrDefault();

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ ${result.result}: ${result.description.en}`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnChargeDetailRecordsResponse',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnChargeDetailRecordsHTTPRequest', (event: MessageEvent<string>) => {

            try {

                const request = JSON.parse(event.data);

                const entries = Object.entries(request);
                if (entries.length === 0)
                    return;

                CreateLogEntry(
                    request.timestamp ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId ?? "",
                    request.from,
                    request.to,
                    "OnChargeDetailRecordsHTTPRequest",
                    `${JSON.stringify(request)}`,
                    request.from ?? "" // ConnectionColorKey
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnChargeDetailRecordsHTTPRequest',
                    event.data,
                    exception
                );
            }

        }, false);

        eventSource.addEventListener('OnChargeDetailRecordsHTTPResponse', (event: MessageEvent<string>) => {

            try {

                const response = JSON.parse(event.data);

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    `⇒ !`,
                    response.runtime
                );

            }
            catch (exception) {
                ShowHTTPSSEError(
                    'OnChargeDetailRecordsHTTPResponse',
                    event.data,
                    exception
                );
            }

        }, false);


    }


    function ShowHTTPSSEError(command:    string,
                              data:       any,
                              exception:  any) {

        const e2 = exception instanceof Error
                       ? exception
                       : new Error(String(exception));

        CreateLogEntry(
            Date.now(),
            "",
            "",
            "",
            "",
            "Error",
            `${command} (${JSON.stringify(data)}) ⇒ ${e2}`,
            "" // ConnectionColorKey
        );

        console.debug(command);
        console.debug(data);
        console.debug(e2);

    }

}
