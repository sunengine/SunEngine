export default function buildPath(...args) {
  var parts = args.map(x=>x.toString().trim());


  var ret =  parts.map((part, i) => {
    if (i === 0){
      return part.replace(/[\/]*$/g, '')
    } else {
      return part.replace(/(^[\/]*|[\/]*$)/g, '')
    }
  }).filter(x=>x.length).join('/');

  if(parts[0]==='/')
    return '/' + ret;

  return ret;
}
