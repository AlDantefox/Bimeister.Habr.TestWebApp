import { Product } from '@/models/product';
import { AxiosInstance } from 'axios';
import { FetchFactory, EnableStreamReader } from '@/API/fetchFactory';

export class ProductAPI {
   constructor(httpRequest: AxiosInstance, asyncEnumerableRequest: FetchFactory) {
      this.httpRequest = httpRequest;
      this.asyncEnumerableRequest = asyncEnumerableRequest;
   }

   private httpRequest: AxiosInstance;
   private asyncEnumerableRequest: FetchFactory;
   private abortController: AbortController;

   async getProducts(onlyEnabled: boolean): Promise<Product[]> {
      this.cancel();
      const result = await this.httpRequest.get<Product[]>('product/GetList', {
         params: {
            onlyEnabled: onlyEnabled,
         },
         signal: this.abortController.signal,
      });
      return result.data;
   }

   async getProductStream(onlyEnabled: boolean, onReceive: (elem: Product[]) => void) {
      this.cancel();
      const params = new URLSearchParams();
      params.append('onlyEnabled', onlyEnabled.toString());
      const response = await this.asyncEnumerableRequest.fetchGet(
         'product/GetAsyncStream',
         params,
         this.abortController.signal,
      );
      await EnableStreamReader<Product>(response, onReceive);
   }

   cancel(): void {
      if (this.abortController) {
         this.abortController.abort();
      }
      this.abortController = new AbortController();
   }
}
