import {getTokens} from 'sun'
import {makeUserDataFromTokens} from 'sun'
import {consoleInit} from 'sun'


export default function initUser(store) {

  const tokens = getTokens();

  store.state.auth.tokens = tokens;

  if (tokens) {
    const userData = makeUserDataFromTokens(tokens);
    userData.isPermanentLogin = true;

    store.commit('setUserData', userData);

    console.info('%cUser restored from localStorage', consoleInit, config.Log.InitExtended ? userData : '');
  }
}
