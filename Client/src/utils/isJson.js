export default function (str) {
  try {
    JSON.parse(str);
  } catch {
    return false;
  }
  return true;
}
