//import { exports } from "./main.js";

console.log("boganet_bridge loaded!");

export async function setPreference(key, pref) {
    //console.log("setPreference: " + key + " - " + pref);

    window.localStorage.setItem(key, pref);
}

export async function getPreference(key) {
    const value = window.localStorage.getItem(key);

    //console.log("getPreference: " + key + " - " + value);

    return value;
    /*
    return await new Promise((resolve) => {
        setTimeout(() => {
            resolve(data);
        }, 2000);
    });
    
    exports.WebPreferencesContainer.Preference(key, value);
*/
}
 


