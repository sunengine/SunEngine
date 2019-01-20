import {QChipsInput} from "quasar-framework/src/components/chips-input";

export default {
  categoryName: "MyChipsInput",
  extends: QChipsInput,
  methods: {
    addNoFocus(value = this.input) {
      clearTimeout(this.timer)

      if (this.isLoading || !this.editable || !value) {
        return
      }

      const val = this.lowerCase
        ? value.toLowerCase()
        : (
          this.upperCase
            ? value.toUpperCase()
            : value
        )

      if (this.model.includes(val)) {
        this.$emit('duplicate', val)
        return
      }

      this.$emit('add', {index: this.model.length, val})
      this.model.push(val)
      this.$emit('input', this.model)
      this.input = ''
    },
  },
  mounted() {
    const input = this.$el.querySelector("input");
    input.addEventListener("blur", this.f1 = function () {
      if(input.value)
        this.addNoFocus();
    }.bind(this));
    input.addEventListener("keyup", this.f2 = function (e) {
      if ((e.code == "Comma" || e.code == "Period") && input.value) {
        let text = input.value;
        text = text.slice(0, -1)
        this.addNoFocus(text);
        input.value = "";
      }
    }.bind(this));
  },
  beforeDestroy() {
    const input = this.$el.querySelector("input");
    input.removeEventListener("blur", this.f1);
    input.removeEventListener("keyup", this.f2);
  }
}
