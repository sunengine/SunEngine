export default function buildPath(...args) {
	const parts = args.map(x => x.toString().trim());

	let ret = parts
		.map((part, i) => {
			if (i === 0) {
				return part.replace(/[\/]*$/g, "");
			} else {
				return part.replace(/(^[\/]*|[\/]*$)/g, "");
			}
		})
		.filter(x => x.length)
		.join("/");

	if (parts[0] === "/") return "/" + ret;

	return ret;
}
