export default {
    initializeState: 'none', // 'none', 'running', 'error', 'done'
    initializedPromise: null,
    currentCategory: null,
    mounted: false
};

export const InitializeState = {
    None: 'none',
    Running: 'running',
    Error: 'error',
    Done: 'done'
};
