import {imagePath} from 'sun'

export default function setUserInfo(state, data) {
  const userInfo = {
    photo: imagePath(data.photo),
    avatar: imagePath(data.avatar),
    link: data.link
  };

  state.userInfo = userInfo;
}
