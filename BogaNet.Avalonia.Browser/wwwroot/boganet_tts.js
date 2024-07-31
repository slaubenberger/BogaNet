"use strict";

if ('speechSynthesis' in window) {
    // Speech Synthesis supported 🎉
    console.log("Text to speech is supported!")
} else {
    // Speech Synthesis Not Supported 😣
    alert("Sorry, your browser doesn't support text to speech!");
}

const synth = window.speechSynthesis;
let voices = [];

populateVoiceList();
if (synth.onvoiceschanged !== undefined) {
    synth.onvoiceschanged = populateVoiceList;
}

function populateVoiceList() {
    voices = synth.getVoices();
}

export function getVoices() {
    let ii = 0;
    const voiceArray = Array(voices.length);

    for (const voice of voices) {
        //console.log(voice);
        voiceArray[ii] = voice.name + ";" + voice.lang;
        ii++;
    }

    return voiceArray;
}

export function speak(text, voice, rate, pitch, volume) {
    if (synth.speaking)
        synth.cancel();
    
    const utterance = new SpeechSynthesisUtterance(text);

    let usedVoice;

    for (const v of voices) {
        if (v.name === voice || (voice === "" && voice.default)) {
            usedVoice = v;
            break;
        }
    }

    if (usedVoice != null)
        utterance.voice = usedVoice;

    utterance.rate = rate;
    utterance.pitch = pitch;
    utterance.volume = volume;
    
    synth.speak(utterance);
}

export function silence() {
    synth.cancel();
}

console.log("+++ boganet_tts loaded!");