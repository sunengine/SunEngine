import {parseJwt} from './token'
import {store} from 'store'

const IdKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
const NameKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
const RolesKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";



export async function loginRequest(nameOrEmail, password) {

  return await request("/Account/Login", {nameOrEmail: nameOrEmail, password: password})
    .then(
      response => {
        return store.state.auth.tokens;
      }
    );
}


