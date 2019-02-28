import buildPath from "services/buildPath"

export default function (token) {
  return buildPath(config.API, token);
}
