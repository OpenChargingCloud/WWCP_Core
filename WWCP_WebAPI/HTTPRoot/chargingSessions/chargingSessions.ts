/**
 * Charging Sessions — JSON viewer.
 *
 * This file provides:
 *   • The reusable `JsonPrettyPrinter` class (collapsible JSON viewer with
 *     search/highlight and path-based programmatic expand/collapse).
 *   • The charging-session bootstrapper that fetches JSON from the same URL
 *     (content negotiation via Accept + X-Portal headers) and wires up the
 *     toolbars in charging-sessions.html.
 *
 * Session toolbar behavior
 * ────────────────────────
 *   • The input field shows the session ID (last path segment of the URL)
 *     and is populated on page load.
 *   • Clicking "load" (or pressing Enter in the input) fetches the session:
 *       — if the input value differs from the currently loaded session, a
 *         fresh printer is built and the search UI is reset (folding /
 *         search / match index all cleared);
 *       — if the input value equals the currently loaded session, the JSON
 *         is reloaded but the printer's expansion and search state are
 *         preserved via `printer.setData()` — i.e. a true refresh.
 *   • The browser URL is kept in sync via `history.replaceState`, so a
 *     page reload (F5) still loads the session currently in view.
 */

export type PathSegment = string | number;
export type JsonPath    = readonly PathSegment[] | string;

export interface PrettyPrinterOptions {
    /** Indentation width in spaces. Default: 2. */
    indent?:        number;
    /** CSS class prefix for all generated elements. Default: "jpp". */
    classPrefix?:   string;
    /** If true, everything starts fully expanded. Default: false. */
    startExpanded?: boolean;
    /** If true, only the top-level container is open on initial render. Default: false. */
    expandRoot?:    boolean;
}

export interface SearchOptions {
    caseSensitive?: boolean;
    regex?:         boolean;
    searchKeys?:    boolean;
    searchValues?:  boolean;
}

export interface SearchMatch {
    path:           PathSegment[];
    matchedKey:     boolean;
    matchedValue:   boolean;
}


export class JsonPrettyPrinter {

    private readonly container: HTMLElement;
    private readonly options: Required<PrettyPrinterOptions>;
    private readonly expanded: Set<string> = new Set();
    private data: unknown;

    // Search state
    private searchQuery: string = "";
    private searchOpts: Required<SearchOptions> = {
        caseSensitive: false, regex: false, searchKeys: true, searchValues: true,
    };
    private searchRegexG: RegExp | null = null;
    private searchRegexTest: RegExp | null = null;
    private matches: SearchMatch[] = [];
    private renderMarkCount: number = 0;
    private currentMatchIdx: number = -1;

    private static readonly SEP = "\u0001";


    constructor(container: HTMLElement, data: unknown, options: PrettyPrinterOptions = {}) {

        this.container = container;
        this.data = data;
        this.options = {
            indent: options.indent ?? 2,
            classPrefix: options.classPrefix ?? "jpp",
            startExpanded: options.startExpanded ?? false,
            expandRoot: options.expandRoot ?? false,
        };

        if (this.options.startExpanded)
            this.collectAllPaths([], this.data, this.expanded);
        else if (this.options.expandRoot)
            this.expanded.add("");     // open only the root container

        this.render();

    }


    // ── expand / collapse ──────────────────────────────────────────────

    public setData(data: unknown): void {
        this.data = data;
        if (this.searchQuery) this.recomputeMatches();
        this.render();
    }

    public expand(path: JsonPath): void {
        const segs = this.normalize(path);
        for (let i = 0; i <= segs.length; i++)
            this.expanded.add(JsonPrettyPrinter.key(segs.slice(0, i)));
        this.render();
    }

    public expandRecursive(path: JsonPath): void {
        const segs = this.normalize(path);
        for (let i = 0; i <= segs.length; i++)
            this.expanded.add(JsonPrettyPrinter.key(segs.slice(0, i)));
        this.collectAllPaths(segs, this.resolve(segs), this.expanded);
        this.render();
    }

    public collapse(path: JsonPath): void {
        const segs = this.normalize(path);
        const k = JsonPrettyPrinter.key(segs);
        for (const p of Array.from(this.expanded))
            if (p === k || p.startsWith(k + JsonPrettyPrinter.SEP))
                this.expanded.delete(p);
        this.render();
    }

    public toggle(path: JsonPath): void {
        const segs = this.normalize(path);
        if (this.expanded.has(JsonPrettyPrinter.key(segs))) this.collapse(segs);
        else this.expand(segs);
    }

    public expandAll(): void { this.collectAllPaths([], this.data, this.expanded); this.render(); }
    public collapseAll(): void { this.expanded.clear(); this.render(); }
    public isExpanded(path: JsonPath): boolean {
        return this.expanded.has(JsonPrettyPrinter.key(this.normalize(path)));
    }


    // ── search ─────────────────────────────────────────────────────────

    public search(query: string, options: SearchOptions = {}): SearchMatch[] {
        this.searchQuery = query ?? "";
        this.searchOpts = {
            caseSensitive: options.caseSensitive ?? false,
            regex: options.regex ?? false,
            searchKeys: options.searchKeys ?? true,
            searchValues: options.searchValues ?? true,
        };
        this.currentMatchIdx = -1;
        this.recomputeMatches();
        this.render();
        if (this.renderMarkCount > 0) this.focusMatch(0);
        return this.matches;
    }

    public clearSearch(): void {
        this.searchQuery = "";
        this.searchRegexG = null;
        this.searchRegexTest = null;
        this.matches = [];
        this.currentMatchIdx = -1;
        this.render();
    }

    public getMatches(): SearchMatch[] { return this.matches.slice(); }
    public getMatchCount(): number { return this.renderMarkCount; }
    public getCurrentMatchIndex(): number { return this.currentMatchIdx; }

    public focusMatch(index: number): void {
        if (this.renderMarkCount === 0) return;
        const n = this.renderMarkCount;
        this.currentMatchIdx = ((index % n) + n) % n;
        this.render();
        const mark = this.container.querySelector(
            `[data-${this.options.classPrefix}-mark="${this.currentMatchIdx}"]`,
        ) as HTMLElement | null;
        if (mark) mark.scrollIntoView({ block: "center", behavior: "smooth" });
    }

    public nextMatch(): void { if (this.renderMarkCount > 0) this.focusMatch(this.currentMatchIdx + 1); }
    public prevMatch(): void { if (this.renderMarkCount > 0) this.focusMatch(this.currentMatchIdx - 1); }


    // ── render ─────────────────────────────────────────────────────────

    public render(): void {
        this.renderMarkCount = 0;
        const pre = document.createElement("pre");
        pre.className = `${this.options.classPrefix}-root`;
        this.renderValue(this.data, [], pre);
        this.container.replaceChildren(pre);
    }


    // ── internals ──────────────────────────────────────────────────────

    private static key(segments: readonly PathSegment[]): string {
        return segments.map(String).join(JsonPrettyPrinter.SEP);
    }

    private recomputeMatches(): void {

        this.matches = [];

        if (!this.searchQuery) {
            this.searchRegexG = null;
            this.searchRegexTest = null;
            return;
        }

        const pattern = this.searchOpts.regex
            ? this.searchQuery
            : this.searchQuery.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        const flags = this.searchOpts.caseSensitive ? "" : "i";

        try {
            this.searchRegexG = new RegExp(pattern, flags + "g");
            this.searchRegexTest = new RegExp(pattern, flags);
        } catch {
            this.searchRegexG = null;
            this.searchRegexTest = null;
            return;
        }

        this.findMatches([], this.data);

        for (const m of this.matches)
            for (let i = 0; i <= m.path.length; i++)
                this.expanded.add(JsonPrettyPrinter.key(m.path.slice(0, i)));

    }

    private findMatches(path: PathSegment[], value: unknown): void {

        const test = this.searchRegexTest;
        if (!test) return;

        if (value === null) {
            if (this.searchOpts.searchValues && test.test("null"))
                this.matches.push({ path: [...path], matchedKey: false, matchedValue: true });
        } else if (typeof value !== "object") {
            if (this.searchOpts.searchValues && test.test(String(value)))
                this.matches.push({ path: [...path], matchedKey: false, matchedValue: true });
        }

        if (Array.isArray(value)) {
            value.forEach((v, i) => this.findMatches([...path, i], v));
        } else if (value !== null && typeof value === "object") {
            for (const k of Object.keys(value)) {
                if (this.searchOpts.searchKeys && test.test(k))
                    this.matches.push({ path: [...path, k], matchedKey: true, matchedValue: false });
                this.findMatches([...path, k], (value as Record<string, unknown>)[k]);
            }
        }

    }

    private normalize(path: JsonPath): PathSegment[] {

        if (Array.isArray(path)) return [...path];

        const raw = path as string;
        if (!raw || raw === "$" || raw === "$.") return [];

        const cleaned = raw.replace(/^\$\.?/, "");
        const segments: PathSegment[] = [];
        const re = /([^.\[\]]+)|\[(\d+)\]/g;

        let m: RegExpExecArray | null;
        while ((m = re.exec(cleaned)) !== null) {
            if (m[2] !== undefined) {
                segments.push(Number(m[2]));
            } else {
                const token = m[1]!;
                const n = Number(token);
                segments.push(Number.isInteger(n) && token === String(n) ? n : token);
            }
        }

        return segments;

    }

    private resolve(segments: readonly PathSegment[]): unknown {
        let cur: unknown = this.data;
        for (const s of segments) {
            if (cur === null || typeof cur !== "object") return undefined;
            cur = (cur as Record<string, unknown>)[String(s)];
        }
        return cur;
    }

    private collectAllPaths(prefix: PathSegment[], value: unknown, target: Set<string>): void {

        if (value === null || typeof value !== "object") return;

        target.add(JsonPrettyPrinter.key(prefix));

        if (Array.isArray(value)) {
            value.forEach((v, i) => this.collectAllPaths([...prefix, i], v, target));
        } else {
            for (const k of Object.keys(value as object))
                this.collectAllPaths([...prefix, k], (value as Record<string, unknown>)[k], target);
        }

    }

    private renderValue(value: unknown, path: PathSegment[], parent: HTMLElement): void {

        const cp = this.options.classPrefix;

        if (value === null) {
            parent.appendChild(this.highlightSpan(`${cp}-null`, "null", "value"));
        } else if (typeof value === "boolean") {
            parent.appendChild(this.highlightSpan(`${cp}-bool`, String(value), "value"));
        } else if (typeof value === "number") {
            parent.appendChild(this.highlightSpan(`${cp}-number`, String(value), "value"));
        } else if (typeof value === "string") {
            parent.appendChild(this.highlightSpan(`${cp}-string`, JSON.stringify(value), "value"));
        } else if (Array.isArray(value)) {
            this.renderContainer(value, path, parent, "array");
        } else if (typeof value === "object") {
            this.renderContainer(value as Record<string, unknown>, path, parent, "object");
        }

    }

    private renderContainer(
        value: unknown[] | Record<string, unknown>,
        path: PathSegment[],
        parent: HTMLElement,
        kind: "object" | "array",
    ): void {

        const cp = this.options.classPrefix;
        const [open, close] = kind === "array" ? ["[", "]"] : ["{", "}"];
        const entries: Array<[PathSegment, unknown]> = Array.isArray(value)
            ? value.map((v, i) => [i, v])
            : Object.entries(value);

        if (entries.length === 0) {
            parent.appendChild(this.plainSpan(`${cp}-empty`, `${open}${close}`));
            return;
        }

        const expanded = this.expanded.has(JsonPrettyPrinter.key(path));

        const toggle = this.plainSpan(`${cp}-toggle`, expanded ? "▼" : "▶");
        toggle.addEventListener("click", e => { e.stopPropagation(); this.toggle(path); });
        parent.appendChild(toggle);
        parent.appendChild(this.plainSpan(`${cp}-brace`, open));

        if (!expanded) {
            const label = kind === "array" ? "item" : "key";
            const summary = this.plainSpan(
                `${cp}-summary`,
                ` ${entries.length} ${label}${entries.length === 1 ? "" : "s"} `,
            );
            summary.addEventListener("click", e => { e.stopPropagation(); this.toggle(path); });
            parent.appendChild(summary);
            parent.appendChild(this.plainSpan(`${cp}-brace`, close));
            return;
        }

        parent.appendChild(document.createTextNode("\n"));
        const childIndent = " ".repeat((path.length + 1) * this.options.indent);
        const closeIndent = " ".repeat(path.length * this.options.indent);

        entries.forEach(([k, v], idx) => {
            parent.appendChild(document.createTextNode(childIndent));
            if (kind === "object") {
                parent.appendChild(this.highlightSpan(`${cp}-key`, JSON.stringify(k as string), "key"));
                parent.appendChild(document.createTextNode(": "));
            }
            this.renderValue(v, [...path, k], parent);
            if (idx < entries.length - 1)
                parent.appendChild(document.createTextNode(","));
            parent.appendChild(document.createTextNode("\n"));
        });

        parent.appendChild(document.createTextNode(closeIndent));
        parent.appendChild(this.plainSpan(`${cp}-brace`, close));

    }

    private plainSpan(cls: string, text: string): HTMLSpanElement {
        const s = document.createElement("span");
        s.className = cls;
        s.textContent = text;
        return s;
    }

    private highlightSpan(cls: string, text: string, type: "key" | "value"): HTMLSpanElement {
        const s = document.createElement("span");
        s.className = cls;
        const active = this.searchRegexG !== null && (
            (type === "key" && this.searchOpts.searchKeys) ||
            (type === "value" && this.searchOpts.searchValues)
        );
        if (!active) { s.textContent = text; return s; }
        this.appendHighlighted(s, text);
        return s;
    }

    private appendHighlighted(parent: HTMLElement, text: string): void {

        const cp = this.options.classPrefix;
        const regex = this.searchRegexG!;
        regex.lastIndex = 0;

        let lastIdx = 0;
        let m: RegExpExecArray | null;
        while ((m = regex.exec(text)) !== null) {
            if (m[0].length === 0) { regex.lastIndex++; continue; }

            if (m.index > lastIdx)
                parent.appendChild(document.createTextNode(text.slice(lastIdx, m.index)));

            const mark = document.createElement("mark");
            mark.className = `${cp}-match`;
            mark.setAttribute(`data-${cp}-mark`, String(this.renderMarkCount));
            if (this.renderMarkCount === this.currentMatchIdx)
                mark.classList.add(`${cp}-match-current`);
            mark.textContent = m[0];
            parent.appendChild(mark);

            this.renderMarkCount++;
            lastIdx = m.index + m[0].length;
        }

        if (lastIdx < text.length)
            parent.appendChild(document.createTextNode(text.slice(lastIdx)));

    }

}


let printer:          JsonPrettyPrinter | null = null;
let currentSessionId: string            | null = null;


function showError(message: string): void {

    const viewer = document.getElementById("viewer");

    if (!viewer)
        return;

    viewer.textContent = "";

    const div = document.createElement("div");
    div.className   = "error-message";
    div.textContent = message;

    viewer.appendChild(div);

}


function sessionIdFromUrl(): string {
    const url  = new URL(window.location.href);
    const last = url.pathname.substring(url.pathname.lastIndexOf("/") + 1);
    return decodeURIComponent(last);
}


function urlForSessionId(sessionId: string): string {
    const url       = new URL(window.location.href);
    const lastSlash = url.pathname.lastIndexOf("/");
    url.pathname    = url.pathname.substring(0, lastSlash + 1) + encodeURIComponent(sessionId);
    return url.toString();
}


async function loadChargingSession(sessionId: string, preserveState: boolean): Promise<void> {

    if (!sessionId) {
        showError("No session id provided.");
        return;
    }

    const url = urlForSessionId(sessionId);

    const response = await fetch(url, {
        headers: {
            "Accept": "application/json",
            "X-Portal": "true",
        },
    }).catch((e: unknown) => {
        showError(`Network error: ${e instanceof Error ? e.message : String(e)}`);
        return null;
    });

    if (!response) return;

    if (!response.ok) {
        if (response.status === 404) {
            showError(`Charging session '${sessionId}' not found!`);
            return;
        }
        showError(`HTTP ${response.status} ${response.statusText}`);
        return;
    }

    const ct = response.headers.get("Content-Type") || "";
    if (!ct.includes("application/json")) {
        showError(`Expected JSON, got: ${ct || "(none)"}`);
        return;
    }

    try {

        const data = await response.json();
        const viewer = document.getElementById("viewer");
        if (!viewer) return;

        if (preserveState && printer !== null) {
            // Refresh: keep the printer's expansion + search state.
            // `setData` internally re-runs the search (if any) and re-renders.
            printer.setData(data);
        } else {
            // Fresh load: new printer instance, search UI reset.
            printer = new JsonPrettyPrinter(viewer, data, {
                indent: 2,
                expandRoot: true,
            });
            (window as unknown as { printer: JsonPrettyPrinter }).printer = printer;
            resetSearchUI();
        }

        currentSessionId = sessionId;
        history.replaceState(null, "", url);
        updateLoadButton();
        updateCounter();

    } catch (e: unknown) {
        showError(`Failed to render: ${e instanceof Error ? e.message : String(e)}`);
    }

}


function resetSearchUI(): void {
    searchInput.value  = "";
    caseSensitive      = false;
    useRegex           = false;
    btnCaseSens. classList.remove("toggled");
    btnRegex.    classList.remove("toggled");
    searchInput. classList.remove("invalid");
    matchCounter.classList.remove("invalid");
}


function updateLoadButton(): void {
    const typed = sessionIdInput.value.trim();
    btnLoadSession.textContent =
        (typed && typed === currentSessionId) ? "↻ refresh" : "load";
}


// ── Toolbar wiring ────────────────────────────────────────────────────

const $ = <T extends HTMLElement>(id: string): T =>
    document.getElementById(id) as T;

const sessionIdInput = $<HTMLInputElement> ("sessionIdInput");
const btnLoadSession = $<HTMLButtonElement>("btnLoadSession");
const pathInput      = $<HTMLInputElement> ("pathInput");
const searchInput    = $<HTMLInputElement> ("searchInput");
const matchCounter   = $<HTMLSpanElement>  ("matchCounter");
const btnCaseSens    = $<HTMLButtonElement>("btnCaseSensitive");
const btnRegex       = $<HTMLButtonElement>("btnRegex");
const btnPrev        = $<HTMLButtonElement>("btnPrevMatch");
const btnNext        = $<HTMLButtonElement>("btnNextMatch");
const btnClear       = $<HTMLButtonElement>("btnClearSearch");

let caseSensitive    = false;
let useRegex         = false;
let searchTimer:     ReturnType<typeof setTimeout> | null = null;


// ── Session controls ──────────────────────────────────────────────────

function triggerLoad(): void {

    const typed    = sessionIdInput.value.trim();

    if (!typed)
        return;

    const preserve = (typed === currentSessionId);

    loadChargingSession(typed, preserve);

}

btnLoadSession.addEventListener("click", triggerLoad);
sessionIdInput.addEventListener("keydown", e => {
    if (e.key === "Enter") {
        e.preventDefault();
        triggerLoad();
    }
});
sessionIdInput.addEventListener("input", updateLoadButton);


// ── Path controls ─────────────────────────────────────────────────────

const getPath = () => pathInput.value.trim();

$<HTMLButtonElement>("btnExpand").     addEventListener("click", () => printer?.expand(getPath()));
$<HTMLButtonElement>("btnExpandRec").  addEventListener("click", () => printer?.expandRecursive(getPath()));
$<HTMLButtonElement>("btnExpandAll").  addEventListener("click", () => printer?.expandAll());
$<HTMLButtonElement>("btnCollapseAll").addEventListener("click", () => printer?.collapseAll());

pathInput.addEventListener("keydown", e => {
    if (e.key === "Enter")
        printer?.expand(getPath());
});


// ── Search controls ───────────────────────────────────────────────────

function runSearch(): void {

    if (!printer)
        return;

    const q = searchInput.value;
    if (!q)
        printer.clearSearch();
    else
        printer.search(q, { caseSensitive, regex: useRegex });

    updateCounter();

}

function updateCounter(): void {

    if (!printer) {
        matchCounter.textContent = "—";
        matchCounter.classList.remove("invalid");
        searchInput. classList.remove("invalid");
        btnPrev.disabled = true;
        btnNext.disabled = true;
        return;
    }

    const q        = searchInput.value;
    const count    = printer.getMatchCount();
    const cur      = printer.getCurrentMatchIndex();

    const invalid  = !!q && useRegex && (() => {
        try {
            new RegExp(q);
            return false;
        } catch {
            return true;
        }
    })();

    searchInput. classList.toggle("invalid", invalid);
    matchCounter.classList.toggle("invalid", invalid);

    if (!q)
        matchCounter.textContent = "no search";
    else if (invalid)
        matchCounter.textContent = "invalid regex";
    else if (count === 0)
        matchCounter.textContent = "no matches";
    else
        matchCounter.textContent = `${cur + 1} of ${count}`;

    const hasMatches = count > 0;
    btnPrev.disabled = !hasMatches;
    btnNext.disabled = !hasMatches;

}


searchInput.addEventListener("input", () => {
    if (searchTimer) clearTimeout(searchTimer);
    searchTimer = setTimeout(runSearch, 120);
});

searchInput.addEventListener("keydown", e => {

    if (e.key === "Enter") {

        e.preventDefault();

        if (e.shiftKey)
            printer?.prevMatch();
        else
            printer?.nextMatch();

        updateCounter();

    }

    else if (e.key === "Escape") {
        e.preventDefault();
        searchInput.value = "";
        printer?.clearSearch();
        updateCounter();
    }

});

btnCaseSens.addEventListener("click", () => {
    caseSensitive = !caseSensitive;
    btnCaseSens.classList.toggle("toggled", caseSensitive);
    runSearch();
});

btnRegex.addEventListener("click", () => {
    useRegex = !useRegex;
    btnRegex.classList.toggle("toggled", useRegex);
    runSearch();
});

btnPrev.addEventListener("click", () => { printer?.prevMatch(); updateCounter(); });
btnNext.addEventListener("click", () => { printer?.nextMatch(); updateCounter(); });

btnClear.addEventListener("click", () => {
    searchInput.value = "";
    printer?.clearSearch();
    updateCounter();
    searchInput.focus();
});


// ── Init ───────────────────────────────────────────────────────────

updateCounter();
sessionIdInput.value = sessionIdFromUrl();
updateLoadButton();
loadChargingSession(sessionIdInput.value, false);
