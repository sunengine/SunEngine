import imagePath from "../../../utils";

export function setUserInfo(state, data) {
  const userInfo = {
    photo: imagePath(data.photo),
    avatar: imagePath(data.avatar),
    link: data.link
  };

  state.userInfo = userInfo;
}
