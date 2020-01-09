export default {
    data() {
        return {
            title: " ",
            mounted1: false
            //  category: null,
            //  centered: false
        }
    },
    methods: {
        setTitle(title) {
            this.title = title;
        }
    },
    mounted() {
        this.mounted1 = true;
    },
    watch: {
        'mounted1': function () {
            this.$store.state.mounted = this.mounted1;
        }
    },
    beforeCreate() {
        this.$store.state.mounted = false;
        this.$store.state.currentCategory = null;
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
