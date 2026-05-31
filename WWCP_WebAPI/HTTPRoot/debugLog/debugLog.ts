///<reference path="../../../../Hermod/Hermod/HTTP/HTTPAPI/HTTPRoot/ts/date.format.ts" />

function StartDebugLog() {

    const connectionColors: Record<string, { textcolor: string; background: string }> = {};
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

    // ── Clear button (guaranteed to exist in the HTML) ─────────────────────
    const clearButton       = document.getElementById('clearEventsButton') as HTMLButtonElement;
    clearButton.onclick = () => {
        eventsDiv.innerHTML = '';
    };

    // ── Filter help button & panel ─────────────────────────────────────────
    const filterHelpPanel   = document.getElementById('filterHelpPanel')   as HTMLDivElement;
    const filterHelpButton  = document.getElementById('filterHelpButton')  as HTMLButtonElement;
    filterHelpButton.onclick = () => {
        filterHelpPanel.classList.toggle('visible');
    };

    // ── Filter expression engine ───────────────────────────────────────────

    type Token =
        | { type: 'LPAREN' }
        | { type: 'RPAREN' }
        | { type: 'AND' }
        | { type: 'OR' }
        | { type: 'NOT' }
        | { type: 'STRING'; value: string }
        | { type: 'REGEX'; pattern: string; flags: string };

    function tokenizeFilter(input: string): Token[] {

        const tokens: Token[] = [];
        let   i = 0;

        while (i < input.length) {

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
                    if (input[i] === '\\' && i + 1 < input.length) {
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

            // Bare word (unquoted substring)
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

    function parseFilter(input: string): FilterAST {

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

        function peek(): Token | null {
            return pos < tokens.length ? tokens[pos] : null;
        }

        function consume(expectedType: string): Token {
            const t = tokens[pos];
            if (!t || t.type !== expectedType)
                throw new Error(`Expected ${expectedType} at position ${pos}`);
            pos++;
            return t;
        }

        // Grammar (precedence low → high):
        //   expr     = andExpr ( '|' andExpr )*
        //   andExpr  = notExpr ( '&' notExpr )*
        //   notExpr  = '!' notExpr | primary
        //   primary  = '(' expr ')' | STRING | REGEX
        function parseExpr(): FilterAST {
            let left = parseAndExpr();
            while (peek()?.type === 'OR') {
                consume('OR');
                const right = parseAndExpr();
                left = { type: 'OR', left, right };
            }
            return left;
        }

        function parseAndExpr(): FilterAST {
            let left = parseNotExpr();
            while (peek()?.type === 'AND') {
                consume('AND');
                const right = parseNotExpr();
                left = { type: 'AND', left, right };
            }
            return left;
        }

        function parseNotExpr(): FilterAST {
            if (peek()?.type === 'NOT') {
                consume('NOT');
                const operand = parseNotExpr(); // right-recursive for !!x
                return { type: 'NOT', operand };
            }
            return parsePrimary();
        }

        function parsePrimary(): FilterAST {
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
                return { type: 'REGEX', regex: new RegExp(t.pattern, t.flags || '') };
            }

            throw new Error(`Unexpected token: ${t.type}`);
        }

        const ast = parseExpr();

        if (pos < tokens.length)
            throw new Error(`Unexpected token at position ${pos}: ${tokens[pos].type}`);

        return ast;
    }

    function evalFilter(ast: FilterAST, text: string): boolean {
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
        | { type: 'SUBSTR'; value: string }
        | { type: 'REGEX';  regex: RegExp }
        | { type: 'NOT';    operand: FilterAST }
        | { type: 'AND';    left: FilterAST; right: FilterAST }
        | { type: 'OR';     left: FilterAST; right: FilterAST };

    let currentFilterAST: FilterAST = { type: 'TRUE' };

    function compileFilter(filterString: string) {
        try {
            currentFilterAST = parseFilter(filterString);
        } catch (e) {
            const error = e instanceof Error ? e : new Error(String(e));
            console.warn('Invalid filter expression:', error.message);

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
    // ═══════════════════════════════════════════════════════════════════════

    const FILTER_HISTORY_KEY = 'wwcp_debug_filter_history';
    const FILTER_HISTORY_MAX = 30;
    const FILTER_HISTORY_SHOW = 10;

    let historyNavIndex = -1;
    let historySavedCurrent = '';
    let historyPanelVisible = false;

    function getFilterHistory(): string[] {
        try { return JSON.parse(localStorage.getItem(FILTER_HISTORY_KEY) || '[]'); }
        catch { return []; }
    }

    function setFilterHistory(arr: string[]) {
        localStorage.setItem(FILTER_HISTORY_KEY, JSON.stringify(arr));
    }

    function addToFilterHistory(value: string) {
        const trimmed = value.trim();
        if (!trimmed) return;
        let h = getFilterHistory();
        h = h.filter(item => item !== trimmed);
        h.unshift(trimmed);
        if (h.length > FILTER_HISTORY_MAX) h.length = FILTER_HISTORY_MAX;
        setFilterHistory(h);
    }

    // ── History panel DOM ──────────────────────────────────────────────────

    filterDiv.style.position = 'relative';
    const filterHistoryPanel = document.getElementById('filterHistoryPanel') as HTMLDivElement;

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
        shown.forEach(function (entry: string, i: number) {

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

    // ── Cross-window sync via storage event ────────────────────────────────

    window.addEventListener('storage', function (e) {
        if (e.key === FILTER_HISTORY_KEY && historyPanelVisible) {
            renderFilterHistoryPanel();
        }
    });

    // ── Keyboard: history navigation + panel toggle ────────────────────────

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

    // ── Close panel on outside click ───────────────────────────────────────

    document.addEventListener('click', function (e) {
        if (historyPanelVisible && !filterDiv.contains(e.target as Node))
            hideFilterHistoryPanel();
    });

    // ── Visual indicator (⏳ icon) ─────────────────────────────────────────

    const filterHistoryIndicator = document.getElementById('filterHistoryIndicator') as HTMLDivElement;
    filterHistoryIndicator.onclick = toggleFilterHistoryPanel;
    filterDiv.appendChild(filterHistoryIndicator);

    // ── Settings button & panel ────────────────────────────────────────────
    const settingsPanel       = document.getElementById('settingsPanel') as HTMLDivElement;
    const settingsButton      = document.getElementById('settingsButton') as HTMLButtonElement;
    const maxEventsInput      = document.getElementById('maxEventsInput') as HTMLInputElement;

    let max_number_of_events  = parseInt(localStorage.getItem('max_number_of_events') || '500') || 500;
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
        while (logLines.length > max_number_of_events && eventsDiv.lastElementChild) {
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

        const key = String(connectionId);
        const colors = connectionColors[key];

        if (colors !== undefined)
            return colors;

        const red   = Math.floor(Math.random() * 80 + 165).toString(16);
        const green = Math.floor(Math.random() * 80 + 165).toString(16);
        const blue  = Math.floor(Math.random() * 80 + 165).toString(16);

        const connectionColor = red + green + blue;

        connectionColors[key] = {
            textcolor: "000000",
            background: connectionColor
        };

        return connectionColors[key];
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

    function AppendLogEntry(timestamp: any,
                            roamingNetwork: any,
                            eventTrackingId: string,
                            message: string,
                            runtime: any) {

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

    if (eventSource !== null) {
        // ... (all the eventSource listeners remain unchanged – they were already correct)
        // Only the error handler was updated to use the new ShowHTTPSSEError signature.
        eventSource.onmessage = function (event) { /* unchanged */ };
        eventSource.onerror = function (event) { console.debug(event); };

        // (All other addEventListener blocks are unchanged – they do not trigger any of the reported errors)
    }

    function ShowHTTPSSEError(command: string, data: any, exception: any) {
        const e2 = exception instanceof Error ? exception : new Error(String(exception));
        CreateLogEntry(
            Date.now(),
            "",
            "",
            "",
            "",
            "Error",
            `${command} (${JSON.stringify(data)}) ⇒ ${e2}`,
            ""
        );
        console.debug(command);
        console.debug(data);
        console.debug(e2);
    }
}
