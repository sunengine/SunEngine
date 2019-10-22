import {router} from 'sun'
import {removeTokens} from 'sun'
import {Api} from 'sun'
import {request} from 'sun'


export default function() {
  request(
    Api.Auth.Logout
  ).finally(
    () => {
      removeTokens();
      router.push({name: 'Home'});
    }
  );
}
