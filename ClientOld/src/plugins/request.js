import Vue from "vue"
import request from "request"

export default ({Vue}) => { Vue.prototype.$request = request; }


