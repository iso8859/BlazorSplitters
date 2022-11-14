// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function SPGetBoundingRect(elementId) {
    return JSON.stringify(document.getElementById(elementId).getBoundingClientRect());
}

export function SPCapturePointer(id, p) {
    var el = document.getElementById(id);
    if (el !== null) {
        el.setPointerCapture(p);
    }
}

export function SPReleasePointer(id, p) {
    var el = document.getElementById(id);
    if (el !== null) {
        el.releasePointerCapture(p);
    }
}