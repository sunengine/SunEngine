import axios from 'axios'

const apiAxios = axios.create({baseURL: config.API, withCredentials: process.env.DEV });


export default async function request(url, data, sendAsJson = false, tokens = null) {

  const headers = {};

  if (tokens) {

    if(tokens.shortTokenExpiration < new Date())
    {
      headers['LongToken1'] = tokens.longToken.token;
    }
    else {
      headers['Authorization'] = `Bearer ${tokens.shortToken}`;
    }
  }

  let body;

  if (data) {
    if ((typeof data === 'object')) {
      if (data instanceof FormData) {
        body = data;
      } else if (sendAsJson === false) {
        body = ConvertObjectToFormData(data);
      } else {
        headers['Content-Type'] = 'application/json';
        body = JSON.stringify(data);
      }
    } else {
      headers['Content-Type'] = 'application/x-www-form-urlencoded';
      body = data;
    }
  }


  const rez = await apiAxios.post(url, body,
    {
      headers: headers,
    });

/*  if(rez.headers.TOKENS) {

  }*/

  return rez;

}

function ConvertObjectToFormData(obj) {
  const formData = new FormData();

  for (const key in obj) {
    formData.append(key, obj[key]);
  }

  return formData;
}
