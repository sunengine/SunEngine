export default {
    name: "Ext",
    props: {
        name: {
            type: String,
            required: true
        },
        component: {
            type: Object,
            required: false
        },
        tag: {
            type: String,
            required: false
        }
    },
    computed: {
        tagName() {
            if (this.tag) {
                return this.tag;
            }
            return "div";
        },
        componentDetect() {
            if (this.component) {
                return this.component;
            }
            else {
                let elCurrent = this.$parent;
                while (elCurrent) {
                    if (elCurrent.$options.ext) {
                        return elCurrent;
                    }
                    elCurrent = elCurrent.$parent;
                }
            }
        }
        ,
        extensions() {
            return this.$store.getters.getExtensions(this.componentDetect.$options.name, this.name);
        }
    },
    render(c) {
        if (!this.extensions) {
            return c(this.tagName, this.$slots.default);
        }

        var arr = [];

        if (this.extensions.start) {
            for (let cons of this.extensions.start) {
                arr.push(c(cons, {
                    props: {component: this.componentDetect}
                }));
            }
        }

        arr.push(this.$slots.default);

        if (this.extensions.end) {
            for (let cons of this.extensions.end) {
                arr.push(c(cons, {
                    props: {component: this.componentDetect}
                }));
            }
        }

        return c(this.tagName, arr);
    }
}
