const TOKENS_KEY = "tokens";


export function hasTokens() {
  return localStorage.getItem(TOKENS_KEY) != null;
}

export function getTokens() {
  return JSON.parse(localStorage.getItem(TOKENS_KEY));
}

export function setTokens(tokens) {
  localStorage.setItem(TOKENS_KEY, JSON.stringify(tokens));
}

export function setTokensString(tokens) {
  localStorage.setItem(TOKENS_KEY, tokens);
}

export function removeTokens() {
  localStorage.removeItem(TOKENS_KEY);
}

export function parseJwt(token) {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  return JSON.parse(atob(base64));
}
