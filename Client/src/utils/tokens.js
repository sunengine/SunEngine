import {LocalStorage} from 'quasar'


const TOKENS_KEY = "tokens";


export function hasTokens() {
  return LocalStorage.has(TOKENS_KEY);
}

export function getTokens() {
  const rez = LocalStorage.getItem(TOKENS_KEY);
  if(!rez || !rez.shortTokenExpiration)
    return rez;
  rez.shortTokenExpiration = new Date(rez.shortTokenExpiration);
  return rez;
}

export function setTokens(tokens) {
  LocalStorage.set(TOKENS_KEY, tokens);
}

export function removeTokens() {
  LocalStorage.remove(TOKENS_KEY);
}
