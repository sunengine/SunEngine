
//import Vue from 'vue';

var exti = 1;


//         const name = vnode.context.$options.name;

export default ({ app, router, Vue }) => {

    Vue.prototype.$ext = function(name) {
        console.log("$ext "+name);

        return {
            inserted: function (el, binding, vnode) {

                var timerdiv = document.createElement("div");

                el.insertAdjacentElement('afterbegin', timerdiv);

                var myConstructor = Vue.extend({
                    categoryName: "ext",
                    extend: app,
                    data: function () {
                        return {
                            x: 1
                        }
                    },
                    methods: {
                        go() {
                            this.x++;
                        }
                    },
                    computed: {
                        userName() {
                            return app.store.state.auth.user?.categoryName;
                        }
                    },
                    render: function (createElement) {
                        return createElement(
                            'span',
                            [binding.arg, " - ",
                                this.x," - ",this.userName," - " ,name] // массив дочерних элементов
                        )
                    },
                    created() {

                        window.setInterval(this.go, 1000);
                    }
                });

                var myComp = new myConstructor();

                myComp.$mount(timerdiv);

                console.log("myComp", myComp);

            }
        }
    }


    // Vue.directive('exttest', {
    //     // Когда привязанный элемент вставлен в DOM...
    //     inserted: function (el,binding,vnode) {
    //
    //         //console.log("inserted",el,binding,vnode)
    //         var val = binding.value;
    //         var val1 = binding.value.myName;
    //         console.log("binding.value",binding.value)
    //
    //         var timerdiv = document.createElement("div");
    //
    //         //var id = "timer"+(exti++);
    //         //timerdiv.setAttribute("id",id);
    //
    //         el.insertAdjacentElement('afterbegin', timerdiv);
    //
    //         var myConstructor =  Vue.extend({
    //             //el: timerdiv,
    //             extend: app,
    //             data: function() {
    //                 return {
    //                     x: 1
    //                 }
    //             },
    //             methods: {
    //               go() {
    //                   this.x++;
    //               }
    //             },
    //             computed: {
    //                 userName() {
    //                     return app.store.state.auth.user?.name;
    //                 }
    //             },
    //             render: function (createElement) {
    //                 return createElement(
    //                     'span',
    //                     [binding.arg,
    //                     this.x] // массив дочерних элементов
    //                 )
    //             },
    //             created() {
    //                console.log("binding",binding);
    //                window.setInterval(this.go,1000);
    //             }
    //         });
    //
    //         new myConstructor().$mount(timerdiv);
    //
    //     }
    // });


}

export var ext = function (componentName,app) {
    return {
        inserted: function (el, binding, vnode) {


            var timerdiv = document.createElement("div");


            el.insertAdjacentElement('afterbegin', timerdiv);

            var myConstructor = Vue.extend({
                extend: app,
                data: function () {
                    return {
                        x: 1
                    }
                },
                methods: {
                    go() {
                        this.x++;
                    }
                },
                computed: {
                    userName() {
                        return app.store.state.auth.user?.categoryName;
                    }
                },
                render: function (createElement) {
                    return createElement(
                        'span',
                        [binding.arg,
                            this.x, myName] // массив дочерних элементов
                    )
                },
                created() {
                    console.log("binding", binding);
                    window.setInterval(this.go, 1000);
                }
            });

            new myConstructor().$mount(timerdiv);

        }
    }
}
