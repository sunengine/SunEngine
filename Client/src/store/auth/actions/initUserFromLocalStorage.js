import {getTokens} from 'sun'
import {makeUserDataFromTokens} from 'sun'
import {consoleInit} from 'sun'


export default function(context) {

  const tokens = getTokens();

  if (tokens) {
    const userData = makeUserDataFromTokens(tokens);
    userData.isPermanentLogin = true;

    context.commit('setUserData', userData);

    console.info('%cUser restored from localStorage', consoleInit, config.Log.InitExtended ? userData : '');
  }
}
