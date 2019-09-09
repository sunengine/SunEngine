import {request} from 'sun'

export default async function (context, data) {
  data.data = data;
  return await request(data.url, data);
}
