import buildPath from "services/buildPath"

export default ({ Vue }) => {
  Vue.prototype.$buildPath = buildPath;
}
