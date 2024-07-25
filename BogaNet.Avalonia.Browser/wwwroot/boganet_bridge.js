import { exports } from "./main.js";

export async function setPreference(key, pref) {
    //console.log("setPreference: " + key + " - " + pref);

    window.localStorage.setItem(key, pref);
}

export async function getPreference(key) {
    const value = window.localStorage.getItem(key);

    //console.log("getPreference: " + key + " - " + value);

    exports.AvaloniaPreferencesContainer.Preference(value);
}



