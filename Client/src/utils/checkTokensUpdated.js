import {store} from 'sun'
import {getTokens} from 'sun'


export default function () {

  const tokens = getTokens();

  return store.state.auth.longToken != tokens?.longToken;

}
