import {postsComponent} from 'sun'
// import {registerComponentsSite} from 'sun'
import {consoleInit} from 'sun'


export default function (context) {

  context.commit('registerComponentType', postsComponent);

  // registerComponentsSite(context);

  console.info('%cComponents registered', consoleInit, config.Log.InitExtended ? context.state.allComponents : '');
}
