const TOKENS_KEY = "tokens";

export function hasToken() {
  return localStorage.getItem(TOKENS_KEY) != null;
}

export function getToken() {
  return localStorage.getItem(TOKENS_KEY);
}

export function setToken(tokens) {
  localStorage.setItem(TOKENS_KEY, tokens);
}

export function removeToken() {
  localStorage.removeItem(TOKENS_KEY);
}

export function parseJwt(token) {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  return JSON.parse(atob(base64));
};
