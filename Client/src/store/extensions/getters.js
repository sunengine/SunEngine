



export function getExtensions (state) {
    return function(name,place) {
        return state.all[name][place];
    }
}




