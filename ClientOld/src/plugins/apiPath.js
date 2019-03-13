import apiPath from "services/apiPath"

export default ({ Vue }) => {
  Vue.prototype.$apiPath = apiPath;
}
