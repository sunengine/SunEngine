import {setTokens} from 'sun'
import {extend} from "quasar";

export default function setUserData(state, data) {

  extend(true, state, data);

  if (data.isPermanentLogin) {
    setTokens(data.tokens);
  }
}
