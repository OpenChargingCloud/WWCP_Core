///<reference path="../../../../Hermod/Hermod/Hermod/HTTP/HTTPAPI/HTTPRoot/ts/date.format.ts" />

function StartDebugLog() {

    const connectionColors   = {};
    const eventsDiv          = document.getElementById('eventsDiv')       as HTMLDivElement;
    const filterDiv          = document.getElementById('eventsFilterDiv') as HTMLDivElement;
    const streamFilterInput  = filterDiv.getElementsByTagName('input')[0] as HTMLInputElement;

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






    // ═══════════════════════════════════════════════════════════════════════
    //  Filter History  (cross-window safe)
    //  ─────────────────────────────────────
    //  Arrow Up/Down : navigate previous filter expressions
    //  Ctrl+H        : toggle history panel (last 10 entries)
    //  Enter         : save current filter to history
    //  Escape        : close panel / reset navigation
    //  ⏳ icon       : click to toggle panel
    //
    //  Uses read-before-write on every localStorage mutation and
    //  listens to the `storage` event so an open panel in another
    //  browser window auto-refreshes.
    // ═══════════════════════════════════════════════════════════════════════

    const FILTER_HISTORY_KEY = 'wwcp_debug_filter_history';
    const FILTER_HISTORY_MAX = 30;
    const FILTER_HISTORY_SHOW = 10;

    let historyNavIndex = -1;
    let historySavedCurrent = '';
    let historyPanelVisible = false;

    // ── Read-through: always fresh from localStorage ─────────────────────

    function getFilterHistory() {
        try { return JSON.parse(localStorage.getItem(FILTER_HISTORY_KEY) || '[]'); }
        catch { return []; }
    }

    function setFilterHistory(arr) {
        localStorage.setItem(FILTER_HISTORY_KEY, JSON.stringify(arr));
    }

    function addToFilterHistory(value) {
        const trimmed = value.trim();
        if (!trimmed) return;
        let h = getFilterHistory();
        h = h.filter(item => item !== trimmed);
        h.unshift(trimmed);
        if (h.length > FILTER_HISTORY_MAX) h.length = FILTER_HISTORY_MAX;
        setFilterHistory(h);
    }

    // ── History panel DOM ────────────────────────────────────────────────

    filterDiv.style.position = 'relative';

    const filterHistoryPanel = document.createElement('div');
    filterHistoryPanel.id = 'filterHistoryPanel';
    filterHistoryPanel.style.cssText = [
        'display:none',
        'position:absolute',
        'top:100%',
        'left:0',
        'right:0',
        'background:#1e1e2e',
        'border:1px solid #444',
        'border-top:none',
        'border-radius:0 0 6px 6px',
        'box-shadow:0 4px 12px rgba(0,0,0,0.3)',
        'z-index:9999',
        'max-height:320px',
        'overflow-y:auto',
        "font-family:'Consolas','Fira Code',monospace",
        'font-size:13px'
    ].join(';');
    filterDiv.appendChild(filterHistoryPanel);

    function renderFilterHistoryPanel() {

        const history = getFilterHistory();
        filterHistoryPanel.innerHTML = '';
        const shown = history.slice(0, FILTER_HISTORY_SHOW);

        if (shown.length === 0) {
            filterHistoryPanel.innerHTML =
                '<div style="padding:8px 12px;color:#888;font-style:italic;">' +
                'No filter history yet</div>';
            return;
        }

        // Header
        const header = document.createElement('div');
        header.style.cssText =
            'padding:6px 12px;color:#888;font-size:11px;border-bottom:1px solid #333;' +
            'display:flex;justify-content:space-between;align-items:center;';
        header.innerHTML =
            '<span>Filter History (Ctrl+H)</span>' +
            '<span id="fh_clear" style="cursor:pointer;color:#666;font-size:10px;">CLEAR ALL</span>';
        filterHistoryPanel.appendChild(header);

        // Entries
        shown.forEach(function (entry, i) {

            const row = document.createElement('div');
            row.style.cssText =
                'padding:6px 12px;cursor:pointer;color:#ccc;border-bottom:1px solid #2a2a3a;' +
                'transition:background 0.1s;display:flex;align-items:center;gap:8px;';
            row.onmouseenter = function () { row.style.background = '#2a2a4a'; };
            row.onmouseleave = function () { row.style.background = 'transparent'; };

            const num = document.createElement('span');
            num.style.cssText = 'color:#555;font-size:11px;min-width:18px;text-align:right;';
            num.textContent = String(i + 1);

            const text = document.createElement('span');
            text.style.cssText = 'flex:1;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;';
            text.textContent = entry;

            const del = document.createElement('span');
            del.style.cssText = 'color:#555;font-size:14px;padding:0 4px;cursor:pointer;';
            del.textContent = '\u00D7';
            del.title = 'Remove';
            del.onclick = function (e) {
                e.stopPropagation();
                let h = getFilterHistory();
                h = h.filter(item => item !== entry);
                setFilterHistory(h);
                renderFilterHistoryPanel();
            };

            row.appendChild(num);
            row.appendChild(text);
            row.appendChild(del);

            row.onclick = function () {
                streamFilterInput.value = entry;
                streamFilterInput.dispatchEvent(new Event('input'));
                hideFilterHistoryPanel();
                streamFilterInput.focus();
            };

            filterHistoryPanel.appendChild(row);
        });

        // Wire clear button
        const clearBtn = document.getElementById('fh_clear');
        if (clearBtn) {
            clearBtn.onclick = function () {
                setFilterHistory([]);
                renderFilterHistoryPanel();
            };
        }

    }

    function showFilterHistoryPanel() { renderFilterHistoryPanel(); filterHistoryPanel.style.display = 'block'; historyPanelVisible = true; }
    function hideFilterHistoryPanel() { filterHistoryPanel.style.display = 'none'; historyPanelVisible = false; }
    function toggleFilterHistoryPanel() { historyPanelVisible ? hideFilterHistoryPanel() : showFilterHistoryPanel(); }

    // ── Cross-window sync via storage event ──────────────────────────────

    window.addEventListener('storage', function (e) {
        if (e.key === FILTER_HISTORY_KEY && historyPanelVisible) {
            renderFilterHistoryPanel();
        }
    });

    // ── Keyboard: history navigation + panel toggle ──────────────────────

    streamFilterInput.addEventListener('keydown', function (e) {

        // Ctrl+H → toggle history panel
        if (e.ctrlKey && e.key === 'h') {
            e.preventDefault();
            toggleFilterHistoryPanel();
            return;
        }

        // Escape → close panel / reset navigation
        if (e.key === 'Escape') {
            if (historyPanelVisible) {
                hideFilterHistoryPanel();
                e.preventDefault();
                return;
            }
            if (historyNavIndex !== -1) {
                streamFilterInput.value = historySavedCurrent;
                streamFilterInput.dispatchEvent(new Event('input'));
                historyNavIndex = -1;
                e.preventDefault();
                return;
            }
        }

        // Enter → commit current filter to history
        if (e.key === 'Enter') {
            addToFilterHistory(streamFilterInput.value);
            historyNavIndex = -1;
            if (historyPanelVisible) hideFilterHistoryPanel();
            return;
        }

        // Arrow Up → navigate history backwards
        if (e.key === 'ArrowUp') {
            const history = getFilterHistory();
            if (history.length === 0) return;
            e.preventDefault();
            if (historyNavIndex === -1) historySavedCurrent = streamFilterInput.value;
            historyNavIndex = Math.min(historyNavIndex + 1, history.length - 1);
            streamFilterInput.value = history[historyNavIndex];
            streamFilterInput.dispatchEvent(new Event('input'));
            streamFilterInput.setSelectionRange(streamFilterInput.value.length, streamFilterInput.value.length);
            return;
        }

        // Arrow Down → navigate history forwards
        if (e.key === 'ArrowDown') {
            if (historyNavIndex === -1) return;
            e.preventDefault();
            historyNavIndex--;
            if (historyNavIndex < 0) {
                historyNavIndex = -1;
                streamFilterInput.value = historySavedCurrent;
            } else {
                streamFilterInput.value = getFilterHistory()[historyNavIndex];
            }
            streamFilterInput.dispatchEvent(new Event('input'));
            streamFilterInput.setSelectionRange(streamFilterInput.value.length, streamFilterInput.value.length);
            return;
        }

        // Any printable key resets navigation index
        if (!e.ctrlKey && !e.altKey && !e.metaKey && e.key.length === 1) {
            historyNavIndex = -1;
        }

    });

    // ── Close panel on outside click ─────────────────────────────────────

    document.addEventListener('click', function (e) {
        if (historyPanelVisible && !filterDiv.contains(e.target as Node))
            hideFilterHistoryPanel();
    });

    // ── Visual indicator (⏳ icon) ───────────────────────────────────────

    const filterHistoryIndicator = document.createElement('span');
    filterHistoryIndicator.style.cssText =
        'position:absolute;right:8px;top:50%;transform:translateY(-50%);' +
        'color:#666;font-size:12px;cursor:pointer;z-index:10;';
    filterHistoryIndicator.title = 'Filter History (Ctrl+H)';
    filterHistoryIndicator.textContent = '\u29D6';
    filterHistoryIndicator.onclick = toggleFilterHistoryPanel;
    filterDiv.appendChild(filterHistoryIndicator);
















    // ── Settings button & panel ─────────────────────────────────────
    const settingsPanel       = document.getElementById('settingsPanel');
    const settingsButton      = document.getElementById('settingsButton');
    const maxEventsInput      = document.getElementById('maxEventsInput') as HTMLInputElement;

    let max_number_of_events  = parseInt(localStorage.getItem('max_number_of_events')) || 500;
    maxEventsInput.value      = max_number_of_events.toString();

    settingsButton.onclick    = () => {
        settingsPanel.classList.toggle('visible');
    };

    maxEventsInput.oninput = () => {

        const val = parseInt(maxEventsInput.value);

        if (!isNaN(val) && val >= 10) {
            max_number_of_events = val;
            localStorage.setItem('max_number_of_events', val.toString());
            trimOldEvents();
        }

    };


    function trimOldEvents() {

        const logLines = eventsDiv.getElementsByClassName('logLine') as HTMLCollectionOf<HTMLDivElement>;

        // First pass: remove oldest events that are currently hidden by the filter
        if (logLines.length > max_number_of_events) {
            for (let i = logLines.length - 1; i >= 0 && logLines.length > max_number_of_events; i--) {
                if (logLines[i].style.display === 'none')
                    eventsDiv.removeChild(logLines[i]);
            }
        }

        // Second pass: if still over the limit, remove the oldest visible events
        while (logLines.length > max_number_of_events) {
            // New events are inserted at the top,
            // so the oldest events are at the bottom
            eventsDiv.removeChild(eventsDiv.lastElementChild);
        }

    }

    const eventsObserver = new MutationObserver((mutations) => {
        for (const mutation of mutations) {
            if (mutation.type === 'childList' && mutation.addedNodes.length > 0)
                trimOldEvents();
        }
    });

    eventsObserver.observe(
        eventsDiv,
        { childList: true }
    );




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
                                "<div class=\"message\">"         + message.reduce(function (a: string, b: string) { return a + "<br />" + b; }) + "</div>" +
                                "<div class=\"runtime\"></div>";

        div.style.display = matchesFilter(div.innerHTML)
            ? 'table-row'
            : 'none';

        eventsDiv.insertBefore(
            div,
            eventsDiv.firstChild
        );

    }

    function AppendLogEntry(timestamp:        any,
                            roamingNetwork:   any,
                            eventTrackingId:  string,
                            message:          string,
                            runtime:          any)
    {

        const searchPattern  = "\"eventTrackingId\">" + eventTrackingId;
        const allLogLines    = eventsDiv.getElementsByClassName('logLine');

        for (let i = 0; i < allLogLines.length; i++) {
            if (allLogLines[i].innerHTML.indexOf(searchPattern) > -1) {
                allLogLines[i].getElementsByClassName("message")[0].innerHTML += `${message}`;
                allLogLines[i].getElementsByClassName("runtime")[0].innerHTML += `${runtime} ms`;
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
                    ` ⇒ ${response.result.result}`,
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
                    ` ⇒ !`,
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
                    ` ⇒ ${response.result.result}`,
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
                    ` ⇒ !`,
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
                    ` ⇒ ${response.result.allowed}`,
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
                    `${request.localAuthentication.authToken} @ '${request.chargingLocation.evseId}'`,
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
                    ` ⇒ ${response.result.result} @'${response.result.providerId ?? "-"} / ${response.result.authorizatorId}' (${response.result.sessionId ?? "-"}): ${response.result.description?.en ?? "-"}`,
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
                    ` ⇒ !`,
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
                    ` ⇒ ${response.result.result} @'${response.result.providerId ?? "-"} / ${response.result.authorizatorId}' (${response.result.sessionId ?? "-"}): ${response.result.description?.en ?? "-"}`,
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
                    ` ⇒ !`,
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
                    ` ⇒ ${response.result.result}: ${response.result.description?.en ?? "-"}`,
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
                    ` ⇒ !`,
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
                    `${request.remoteAuthentication?.remoteIdentification ?? "-"} (${request.providerId}) @'${request.chargingLocation?.evseId ?? "-"}' (${request.csoRoamingProviderId ?? "-"} / ${request.sessionId})`,
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
                    ` ⇒ ${response.result.result}: ${response.result.description?.en ?? "-"}`,
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
                    ` ⇒ !`,
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
                    ` ⇒ !`,
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

                const chargeDetailRecord = request.chargeDetailRecords?.length > 0
                    ? request.chargeDetailRecords[0]
                    : null;

                CreateLogEntry(
                    request.timestamp        ?? Date.now(),
                    request.roamingNetworkId ?? "",
                    request.eventTrackingId  ?? "",
                    request.from,
                    request.to,
                    "OnChargeDetailRecordsRequest",
                    `Id: ${chargeDetailRecord["@id"]} (${chargeDetailRecord.providerIdStart}, ${chargeDetailRecord.sessionId}, EVSE Id: ${chargeDetailRecord.evseId}, auth: ${chargeDetailRecord.authenticationStart})`,
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

                const result = response.results?.length > 0
                    ? response.results[0]
                    : null;

                AppendLogEntry(
                    response.timestamp,
                    response.roamingNetwork,
                    response.eventTrackingId,
                    ` ⇒ ${result.result}: ${result.description?.en ?? "-"}`,
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
                    ` ⇒ !`,
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
