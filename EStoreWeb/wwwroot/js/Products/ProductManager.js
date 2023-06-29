function getTokenFromCookie() {
    const cookies = document.cookie.split("; ");
    for (let i = 0; i < cookies.length; i++) {
        const cookie = cookies[i].split("=");
        const name = cookie[0];
        const value = cookie[1];
        if (name === "token") {
            return value;
        }
    }
    return null;
}
//$(document).ready(() => {
//    let token = getTokenFromCookie();
//    console.log(token);
//});

console.log(getTokenFromCookie());