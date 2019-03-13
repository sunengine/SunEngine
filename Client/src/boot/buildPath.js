import buildPath from "services/buildPath"

export default async ({ Vue }) => {
  Vue.prototype.$buildPath = buildPath;
}
