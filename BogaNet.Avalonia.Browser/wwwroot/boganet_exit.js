"use strict";

import { dotnet } from './_framework/dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const dotnetRuntime = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

const config = dotnetRuntime.getConfig();

export const exports = await dotnetRuntime.getAssemblyExports(config.mainAssemblyName);

window.addEventListener('beforeunload', function (e) {
    e.preventDefault();
    e.returnValue = '';

    exports.Program.Exit();
});

console.log("+++ boganet_exit loaded!");