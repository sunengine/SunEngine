import {getTokens} from 'sun'
import {consoleInit} from 'sun'


export default function (context) {

  const tokens = getTokens();

  if (tokens) {
    context.state.longToken = tokens.longToken;

    console.info('%cTokens load from localStorage', consoleInit);
  }
}
