const envBaseUrl = `${import.meta.env.VITE_VUE_APP_BASE_URL}`;
const basePath = envBaseUrl.endsWith('/') ? envBaseUrl : `${envBaseUrl}/`;
const currentMode = `${import.meta.env.VITE_NODE_ENV}`;

export { basePath, currentMode };
