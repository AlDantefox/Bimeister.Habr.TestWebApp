import axios from 'axios';
import { basePath, currentMode } from '@/utils/baseUrl';
import { FetchFactory } from '@/API/fetchFactory';
import { ProductAPI } from '@/API/productApi';

const basePathApi = `${basePath}api`;
if (currentMode !== 'PRD') {
   console.log(`Current API path: ${basePathApi}`);
}
const httpRequest = axios.create({
   baseURL: basePathApi,
   withCredentials: true,
   headers: {
      Accept: 'application/json',
      'Content-Type': 'application/json',
      'Access-Control-Allow-Credentials': true,
   },
});
const asyncEnumerableRequest = new FetchFactory(basePathApi);
const productApi = new ProductAPI(httpRequest, asyncEnumerableRequest);

class API {
   product = productApi;
}

const APIStatement = new API();
export { APIStatement as API };
