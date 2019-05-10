import {setTokens} from 'sun'

export default function setUserData(state, data) {

  Object.assign(state, data);

  if (data.isPermanentLogin) {
    setTokens(data.tokens);
  }
}
