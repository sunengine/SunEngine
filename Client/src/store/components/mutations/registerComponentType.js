import Vue from "vue";

export default function(state, component) {
	if (!state.componentsTypes) state.componentsTypes = {};

	Vue.set(state.componentsTypes, component.name.toLowerCase(), component);
}
