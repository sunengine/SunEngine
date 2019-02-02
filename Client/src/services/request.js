import axios from 'axios'
import buildPath from 'services/buildPath';
//import config from './config'

const apiAxios = axios.create({baseURL: config.API, withCredentials: true});

export default async function request(url, data, sendAsJson = false, token = null /* or it will be send as FormData */) {

  const headers = {};

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  headers['Accept'] = 'application/json';

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
      //withCredentials: true
    });


  return rez;

  /*return await axios({
    method: 'post',
    url: url,
    baseURL: config.API,
    data: body,
    headers: headers
  });*/
}

function ConvertObjectToFormData(obj) {
  var formData = new FormData();

  for (var key in obj) {
    formData.append(key, obj[key]);
  }

  return formData;
}
