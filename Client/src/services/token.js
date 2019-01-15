
const TOKEN_KEY = "token";

export function hasToken() {
  return localStorage.getItem("TOKEN_KEY") != null;
}

export function getToken() {
    return localStorage.getItem("TOKEN_KEY");
}

export function setToken(token) {
    localStorage.setItem("TOKEN_KEY",token);
}

export function removeToken() {
    localStorage.removeItem("TOKEN_KEY");
}

export function parseJwt (token) {
  var base64Url = token.split('.')[1];
  var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  return JSON.parse(window.atob(base64));
};
