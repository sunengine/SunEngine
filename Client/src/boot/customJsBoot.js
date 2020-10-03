export default ({app, Vue}) => {
    try {
        if (customStartUp)
            customStartUp(app, Vue);
    } catch (_) { }
}
