"use strict";

export function setUrl(url) {
    console.log("getUrl: " + url);
    
    window.location.href = url;
}

export function getUrl() {
    const value = window.location.href;

    console.log("getUrl: " + value);

    return value;
}

console.log("+++ boganet_url loaded!");