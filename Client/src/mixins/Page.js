export default {
    data() {
        return {
            title: " ",
        }
    },
    methods: {
        setTitle(title) {
            this.title = title;
        }
    },
    meta() {
        return {
            title: this.title,
            titleTemplate(title) {
                if (title === " ")
                    return config.Global.SiteName;
                else
                    return config.Global.PageTitleTemplate
                        .replace("{siteName}", config.Global.SiteName)
                        .replace("{pageTitle}", title);
            }
        }
    }
}
