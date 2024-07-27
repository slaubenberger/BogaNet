"use strict";

export function setPreference(key, pref) {
    window.localStorage.setItem(key, pref);

    //console.log("setPreference: " + key + " - " + pref);
}

export function getPreference(key) {
    const value = window.localStorage.getItem(key);

    //console.log("getPreference: " + key + " - " + value);

    return value;
}

export function deletePreference(key) {
    window.localStorage.removeItem(key);

    //console.log("deletePreference: " + key);
}

console.log("+++ boganet_prefs loaded!");