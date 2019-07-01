import Vue from 'vue'


import {imagePath} from 'sun'

export default function (state, data) {

  data.photo = imagePath(data.photo);
  data.avatar = imagePath(data.avatar);

  Vue.set(state, 'roles' , data.roles);

  delete data.roles;

  Vue.set(state, 'user', data);
}
