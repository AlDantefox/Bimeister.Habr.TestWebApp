<script setup lang="ts">
import { ref } from 'vue';
import { API } from '@/API/apiService';
import { Product } from '@/models/product';

const list = ref<Product[]>([]);
const onlyEnabled = ref<boolean>(true);

async function getList() {
   list.value = [];
   const newValues = await API.product.getProducts(onlyEnabled.value);
   list.value = newValues;
}
async function getListByStream() {
   list.value = [];
   await API.product.getProductStream(onlyEnabled.value, (prodArr) => {
      if (prodArr != undefined && prodArr.length > 0) {
         list.value.push(...prodArr);
      }
   });
}
function cancelLoading() {
   API.product.cancel();
}
</script>
<template>
   <h3>Classic load - wait all items from backend before render it</h3>
   <h3>Stream load - render items when they are received from backend</h3>
   <h3>Cancel to stop</h3>
   <div class="panel">
      <input id="onlyEnabledChb" v-model="onlyEnabled" type="checkbox" />
      <label for="onlyEnabledChb"> only enabled? </label>
   </div>
   <div class="panel">
      <button @click="getList">Classic load</button>
      <button @click="getListByStream">Stream load</button>
      <button @click="cancelLoading">Cancel loading</button>
   </div>
   <div v-for="item in list" :key="item.id" class="product">
      <b>Name:</b> '{{ item.name }}' <b>Code:</b> '{{ item.code }}' <b>SortOrder:</b> '{{ item.sortOrder }}'
      <b>Enabled:</b> '{{ item.enabled }}'
   </div>
</template>
<style lang="scss" scoped>
.product {
   margin: 10px;
   text-align: center;
}

.panel {
   display: flex;
   flex-direction: row;
   flex-wrap: nowrap;
   justify-content: center;
   align-items: baseline;
}
.panel button {
   margin-left: 10px;
}
</style>
