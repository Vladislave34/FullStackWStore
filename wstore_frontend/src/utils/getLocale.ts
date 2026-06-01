




export function getLocale() {
    if (typeof document === "undefined") return undefined;
    const locale = document.cookie
        .split("; ")
        .find(row => row.startsWith("locale="))
        ?.split("=")[1];
    return locale;
}

