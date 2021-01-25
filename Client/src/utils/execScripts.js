export default function(el) {
    const scripts = el.getElementsByTagName("script");

    for (const script of scripts) {
        const script2 = document.createElement("script");
        script2.innerHTML = script.innerText;
        const parent = script.parentNode;
        const next = script.nextSibling;
        script.remove();
        parent.insertBefore(script2, next);
    }
}