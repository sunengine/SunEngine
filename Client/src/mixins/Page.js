export default {
    data() {
        return {
            title: " ",
            mounted: false
            //  centered: false
        }
    },
    methods: {
        setTitle(title) {
            this.title = title;
        }
    },
    mounted() {
        this.mounted = true;
    },
    watch: {
        'mounted': function () {
            this.$store.state.mounted = this.mounted;
            this.$store.state.currentPage = this;
        }
    },
    beforeCreate() {
        this.$store.state.mounted = false;
        this.$store.state.currentCategory = null;
        this.$store.state.currentPage = null;
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
