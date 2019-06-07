import {getTokens} from 'sun'
import {consoleInit} from 'sun'


export default function(context) {

  const tokens = getTokens();

  if (tokens) {
    context.state.isPermanentLogin = true;
    context.state.tokens = tokens;

    console.info('%cTokens load from localStorage', consoleInit);
  }
}
