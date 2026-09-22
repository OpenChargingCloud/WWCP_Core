/*
 * Compile every entry of a project's compilerconfig.json to CSS, plain and
 * minified.
 *
 * Run from the project directory - the build does that, and so does
 * WWCP_WebAPI/compileSASS.sh, so that the one arrangement produces the bytes
 * in both places. At the repository root rather than beside the one project
 * that needs it, so that this repository and WWCP_OCPP - which has eleven of
 * them - are arranged the same way.
 *
 * It replaces a shell script that needed 'sass' and 'jq' installed globally.
 * The stylesheets it produces are embedded as resources and are gitignored,
 * so a machine that had never run that script could not build WWCP_WebAPI at
 * all - it stopped with
 *
 *   error CS1566: Fehler beim Lesen der Ressource
 *                 "...HTTPRoot.css.chargingSessions.chargingSessions.css"
 *
 * which names a missing file rather than the missing step that produces it.
 *
 * Node rather than shell: the build already needs Node for TypeScript, this
 * runs the same on Windows and Linux, and reading a JSON file needs no jq.
 */

import { readFile, mkdir, writeFile } from "node:fs/promises";
import { dirname, join, resolve } from "node:path";

import * as sass from "sass";

const project = process.cwd();
const config  = await readFile(join(project, "compilerconfig.json"), "utf8");

// Without the byte order mark, which Visual Studio wrote and JSON.parse
// refuses. jq swallowed it, so the shell script this replaces never had to
// know - and a build that stops at "Unexpected token" over an invisible
// character is a poor way to find that out.
const entries = JSON.parse(config.replace(/^﻿/, ""));

for (const { inputFile, outputFile } of entries) {

    const input  = resolve(project, inputFile);
    const output = resolve(project, outputFile);

    await mkdir(dirname(output), { recursive: true });

    for (const [path, style] of [[output,                               "expanded"],
                                 [output.replace(/\.css$/, ".min.css"), "compressed"]]) {

        // The stylesheets use @import, which Dart Sass deprecated and will
        // drop in 3.0. Silenced rather than migrated: @use is not a rename,
        // it namespaces what it pulls in, so converting these is a change to
        // the stylesheets and a decision of its own. Left loud it would be
        // four warnings per project on every build, which is how a build
        // teaches people to stop reading its output.
        const { css } = sass.compile(input, {
                            style,
                            sourceMap:           false,
                            silenceDeprecations: [ "import" ]
                        });

        // Written only when it would change, so that a build which compiles
        // nothing new leaves the timestamps - and any incremental check that
        // hangs off them - alone.
        let existing = null;
        try { existing = await readFile(path, "utf8"); } catch { /* not there yet */ }

        if (existing !== css)
            await writeFile(path, css, "utf8");

    }

}
