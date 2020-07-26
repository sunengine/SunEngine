export default ({app, Vue}) => {
	if (!config.Admin.StartUpScripts || !config.Admin.AllowCustomJavaScript)
		return;

	const scriptsToStart = config.Admin.StartUpScripts.split(",");

	for (const script of scriptsToStart) {
     try {
         window[script](app, Vue);
     } catch (e) {
         console.error(e);
     }
    }
}
