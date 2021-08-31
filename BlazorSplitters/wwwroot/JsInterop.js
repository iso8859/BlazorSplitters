// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function SPGetBoundingRect(elementId) {
    return JSON.stringify(document.getElementById(elementId).getBoundingClientRect());
}
