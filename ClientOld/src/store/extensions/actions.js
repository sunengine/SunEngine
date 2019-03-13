import Vue from 'vue'


var exttest1 = Vue.extend({
  name: "exttest1",
  data: function () {
    return {
      x: 1,
      interval: null
  }
  },
  methods: {
    go() {
      this.x++;
    }
  },
  computed: {
    userName() {
      //return app.store.state.auth.user?.name;
    }
  },
  render: function (createElement) {
    return createElement(
      'span',
      ["[Расширение с реактивностью: ", this.x, "]"]
    )
  },
  mounted() {
    this.interval = window.setInterval(this.go, 1000);
  },
  beforeDestroy() {
    window.clearInterval(this.interval);
  }
});

var exttest2 = Vue.extend({
  name: "exttest2",
  computed: {
    userName() {
      //return app.store.state.auth.user?.name;
    }
  },
  render: function (createElement) {
    return createElement(
      'span',
      ["[ExtTest2]"]
    )
  }
});

var exttest22 = Vue.extend({
  categoryName: "exttest22",
  computed: {
    userName() {
      return this.$store.state.auth.user?.name;
    }
  },
  render: function (createElement) {
    return createElement(
      'span',
      [`[Расширение получает данные из store: ${this.userName}]`]
    )
  }
});

var ExtTestData = Vue.extend({
  name: "ExtTestData",
  props: {
    component: {
      type: Object,
      required: true
    }
  },
  render: function (createElement) {
    return createElement(
      'span',
      ["[Расширение получает данные компонента: ", this.component.myData,"]"]
    )
  }
});

var ExtTestEnd = Vue.extend({
  name: "ExtTestEnd",
  render: function (createElement) {
    return createElement(
      'span',
      ["[ExtTestEnd]"]
    )
  }
});

export function getAndSetAllExtensions(context) {
  console.log("GetAndSetAllExtensions");
  context.state.all = {
    TestExt: {
      place1: {
        start: [exttest1],
        end: [ExtTestData]
      },
      place2: {
        start: [exttest2, exttest22],
        end: [ExtTestEnd]
      }
    }
  };
}
