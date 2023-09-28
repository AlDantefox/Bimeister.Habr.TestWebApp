import { getDeferred } from '@/utils/deferred';

export class FetchFactory {
   private baseUrl: string;
   private requestHeaders: HeadersInit;

   constructor(baseUrl: string) {
      this.baseUrl = baseUrl;
      this.requestHeaders = new Headers();
      this.requestHeaders.set('Content-Type', 'application/json');
      this.requestHeaders.set('Access-Control-Allow-Credentials', 'true');
   }

   public fetchGet(url: string, params?: URLSearchParams, signal?: AbortSignal): Promise<Response> {
      let checkedUrl = url.startsWith('/') ? `${this.baseUrl}${url}` : `${this.baseUrl}/${url}`;
      checkedUrl = params === undefined ? checkedUrl : `${checkedUrl}?${params}`;
      return fetch(checkedUrl, {
         method: 'GET',
         headers: this.requestHeaders,
         credentials: 'include',
         signal: signal,
      });
   }
}

export async function EnableStreamReader<T>(
   apiResponse: Response,
   resultIterator: (newElems: T[]) => void,
): Promise<void> {
   const { promise, resolve, reject } = getDeferred<void>();
   const decoder = new TextDecoder();
   const reader = apiResponse.body?.getReader();
   if (reader != null) {
      let streamNotEnded = true;
      while (streamNotEnded) {
         const { done, value } = await reader.read(); //reading new batch
         streamNotEnded = !done;
         if (streamNotEnded) {
            const result = decoder.decode(value).replace(/^\s*\[|]\s*$/g, '').replace(/^\s*,/, ''); //convert bytes to text
            if (result != null) {
               const parsed = JSON.parse(`[${result}]`); //parse to array
               if (parsed != null) {
                  const casted = parsed as T[]; //cast to array of T
                  if (casted != null) {
                     resultIterator(parsed); //iterate on result
                  } else reject(new Error('Readed value is not a JSON of expected Type'));
               } else reject(new Error('Readed value is not a JSON object'));
            } else reject(new Error('Readed value is not a string'));
         }
      }
      reader.releaseLock();
      resolve();
   } else reject(new Error('Reader not ready'));

   return promise;
}
