/* eslint-disable @typescript-eslint/no-empty-function */
export function getDeferred<T>() {
   let resolve: (value: T) => void = () => {};
   let reject: (error: Error) => void = () => {};
   const promise = new Promise<T>((res, rej) => {
      resolve = res;
      reject = rej;
   });
   return { promise, reject, resolve };
}
