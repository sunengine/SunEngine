import request from './request'
import {parseJwt} from './token'

const IdKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
const NameKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
const RolesKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

export async function loginRequest(nameOrEmail, password) {

  return await request("/Auth/Login", {nameOrEmail: nameOrEmail, password: password})
    .then(
      response => {
        debugger;
        return JSON.parse(response.headers.tokens);
      }
    );
}

export function getTokensFromResponse(response) {

}

export function makeUserDataFromToken(token) {

  const tokenParsed = parseJwt(token.shortToken);

  let roles = tokenParsed[RolesKey];
  let userGroups;
  let userGroup;

  if (Array.isArray(roles)) {
    userGroups = roles;
    if (roles.length == 1) {
      userGroup = roles[0];
    }
  } else if (roles) {
    userGroups = [roles];
    userGroup = roles;
  }

  token.shortTokenExpiration = new Date(tokenParsed.exp * 1000);

  let data = {
    tokens: token,
    user: {
      id: tokenParsed[IdKey],
      name: tokenParsed[NameKey],
    },
    userGroups: userGroups,
    userGroup: userGroup
  };

  return data;
}
