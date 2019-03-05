const TOKENS_KEY = "tokens";

const IdKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
const NameKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
const RolesKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";



export function hasTokens() {
  return localStorage.getItem(TOKENS_KEY) != null;
}

export function getTokens() {
  return JSON.parse(localStorage.getItem(TOKENS_KEY));
}

export function setTokens(tokens) {
  if(typeof tokens === 'object')
    localStorage.setItem(TOKENS_KEY, JSON.stringify(tokens));
  else if(typeof tokens === 'string')
    localStorage.setItem(TOKENS_KEY, tokens);
  else
    throw "Error saving tokens";
}

export function removeTokens() {
  localStorage.removeItem(TOKENS_KEY);
}

export function parseJwt(token) {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  return JSON.parse(atob(base64));
}

export function makeUserDataFromTokens(tokens) {

  const tokenParsed = parseJwt(tokens.shortToken);

  let roles = tokenParsed[RolesKey];

  if (!Array.isArray(roles)) {
    roles = [roles];
  }

  tokens.shortTokenExpiration = new Date(tokenParsed.exp * 1000);

  const data = {
    tokens: tokens,
    user: {
      id: tokenParsed[IdKey],
      name: tokenParsed[NameKey],
    },
    roles: roles,
  };

  return data;
}
