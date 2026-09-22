/*
 * Compile every entry of compilerconfig.json to CSS, plain and minified.
 *
 * This is what the build runs, and what compileSASS.sh now calls, so that the
 * one arrangement produces the bytes in both places.
 *
 * It replaces a shell script that needed 'sass' and 'jq' installed globally.
 * The stylesheets it produces are embedded as resources and are gitignored,
 * so a machine that had never run that script could not build this project at
 * all - it stopped with
 *
 *   error CS1566: Fehler beim Lesen der Ressource
 *                 "...HTTPRoot.css.chargingSessions.chargingSessions.css"
 *
 * which names a missing file rather than the missing step that produces it.
 * The compiler now comes with the repository, in package.json beside the
 * solution, and the build runs it.
 *
 * Node rather than shell: the build already needs Node for TypeScript, this
 * runs the same on Windows and Linux, and reading a JSON file needs no jq.
 */

import { readFile, mkdir, writeFile } from "node:fs/promises";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";

import * as sass from "sass";

const here   = dirname(fileURLToPath(import.meta.url));
const config = await readFile(join(here, "compilerconfig.json"), "utf8");

// Without the byte order mark, which Visual Studio wrote and JSON.parse
// refuses. jq swallowed it, so the shell script this replaces never had to
// know - and a build that stops at "Unexpected token" over an invisible
// character is a poor way to find that out.
const entries = JSON.parse(config.replace(/^\uFEFF/, ""));

for (const { inputFile, outputFile } of entries) {

    const input  = resolve(here, inputFile);
    const output = resolve(here, outputFile);

    await mkdir(dirname(output), { recursive: true });

    for (const [path, style] of [[output,                             "expanded"],
                                 [output.replace(/\.css$/, ".min.css"), "compressed"]]) {

        const { css } = sass.compile(input, { style, sourceMap: false });

        // Written only when it would change, so that a build which compiles
        // nothing new leaves the timestamps - and any incremental check that
        // hangs off them - alone.
        let existing = null;
        try { existing = await readFile(path, "utf8"); } catch { /* not there yet */ }

        if (existing !== css)
            await writeFile(path, css, "utf8");

    }

}
