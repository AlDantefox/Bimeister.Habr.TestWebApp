import { defineConfig, loadEnv } from 'vite';
import vue from '@vitejs/plugin-vue';
import eslintPlugin from 'vite-plugin-eslint';
import * as path from 'path';

export default ({ mode }) => {
   process.env = { ...process.env, ...loadEnv(mode, process.cwd()) };
   const basePath = process.env.VITE_VUE_APP_BASE_URL;
   return defineConfig({
      base: basePath, //for paths in dist files
      plugins: [vue(), eslintPlugin()],
      server: {
         port: 4444, //check port in ASP Core appsettings for debug
         strictPort: true,
      },
      resolve: {
         alias: [{ find: '@', replacement: path.resolve(__dirname, 'src') }],
      },
      optimizeDeps: {
         force: true,
      },
   });
};
