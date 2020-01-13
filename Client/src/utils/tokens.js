import { LocalStorage } from "quasar";
import { consoleInit } from "./consoleStyles";

const TOKENS_KEY = "tokens";

var longToken = null;

export function hasLongToken() {
	return !!longToken;
}

export function hasTokens() {
	return LocalStorage.has(TOKENS_KEY);
}

export function getTokens() {
	const rez = LocalStorage.getItem(TOKENS_KEY);
	if (!rez || !rez.shortTokenExpiration) return rez;
	rez.shortTokenExpiration = new Date(rez.shortTokenExpiration);
	return rez;
}

export function setTokens(tokens) {
	longToken = tokens.longToken;
	LocalStorage.set(TOKENS_KEY, tokens);
}

export function removeTokens() {
	LocalStorage.remove(TOKENS_KEY);
	longToken = null;
}

export function checkTokensUpdated() {
	return longToken != getTokens()?.longToken;
}

export function initLongTokenFromLocalStorage() {
	const tokens = getTokens();
	if (tokens) {
		longToken = tokens.longToken;
		console.info("%cUser credentials found in localStorage", consoleInit);
	}
}
