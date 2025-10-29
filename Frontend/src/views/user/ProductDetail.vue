<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div v-if="product" class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Breadcrumb -->
      <nav class="flex mb-8 animate-fade-in" aria-label="Breadcrumb">
        <ol class="inline-flex items-center space-x-1 md:space-x-3">
          <li><router-link to="/" class="text-gray-600 hover:text-primary-600">Trang chủ</router-link></li>
          <li><span class="text-gray-400 mx-2">/</span></li>
          <li><router-link to="/products" class="text-gray-600 hover:text-primary-600">Sản phẩm</router-link></li>
          <li><span class="text-gray-400 mx-2">/</span></li>
          <li class="text-primary-600 font-semibold">{{ product.name }}</li>
        </ol>
      </nav>

      <!-- Product Details -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-12 mb-12">
        <!-- Product Images -->
        <div class="animate-fade-in-up">
          <div class="bg-white rounded-2xl shadow-lg overflow-hidden mb-4">
            <img :src="product.image" :alt="product.name" class="w-full h-auto" />
          </div>
          <div class="grid grid-cols-4 gap-4">
            <div v-for="i in 4" :key="i" class="bg-white rounded-lg overflow-hidden shadow-md hover:shadow-lg transition-shadow cursor-pointer">
              <img :src="product.image" :alt="`${product.name} ${i}`" class="w-full h-full object-cover" />
            </div>
          </div>
        </div>

        <!-- Product Info -->
        <div class="animate-fade-in-up" style="animation-delay: 0.2s;">
          <div class="bg-white rounded-2xl shadow-lg p-8">
            <span class="inline-block px-4 py-1 bg-primary-100 text-primary-600 rounded-full text-sm font-semibold mb-4">
              {{ product.category }}
            </span>
            <h1 class="text-3xl md:text-4xl font-bold text-gray-900 mb-4">
              {{ product.name }}
            </h1>
            
            <!-- Rating -->
            <div class="flex items-center space-x-4 mb-6">
              <div class="flex items-center space-x-1">
                <svg v-for="i in 5" :key="i" class="w-5 h-5" :class="i <= product.rating ? 'text-yellow-400' : 'text-gray-300'" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                </svg>
              </div>
              <span class="text-gray-600">{{ product.rating }} ({{ product.reviews }} đánh giá)</span>
            </div>

            <!-- Price -->
            <div class="mb-6">
              <span class="text-4xl font-bold text-primary-600">{{ formatPrice(product.price) }}</span>
            </div>

            <!-- Description -->
            <p class="text-gray-600 mb-6 leading-relaxed">
              {{ product.description }}
            </p>

            <!-- Size Selection -->
            <div class="mb-6">
              <h3 class="font-bold text-gray-900 mb-3">Chọn size:</h3>
              <div class="grid grid-cols-6 gap-2">
                <button
                  v-for="size in product.sizes"
                  :key="size"
                  @click="selectedSize = size"
                  :class="[
                    'py-3 rounded-lg font-semibold transition-all duration-300',
                    selectedSize === size
                      ? 'bg-primary-500 text-white shadow-lg scale-105'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  ]"
                >
                  {{ size }}
                </button>
              </div>
            </div>

            <!-- Color Selection -->
            <div class="mb-6">
              <h3 class="font-bold text-gray-900 mb-3">Màu sắc:</h3>
              <div class="flex space-x-3">
                <button
                  v-for="color in product.colors"
                  :key="color"
                  @click="selectedColor = color"
                  :class="[
                    'px-6 py-2 rounded-lg font-medium transition-all duration-300',
                    selectedColor === color
                      ? 'bg-primary-500 text-white shadow-lg'
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  ]"
                >
                  {{ color }}
                </button>
              </div>
            </div>

            <!-- Quantity -->
            <div class="mb-8">
              <h3 class="font-bold text-gray-900 mb-3">Số lượng:</h3>
              <div class="flex items-center space-x-4">
                <button @click="quantity = Math.max(1, quantity - 1)" class="w-10 h-10 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">
                  <svg class="w-5 h-5 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
                  </svg>
                </button>
                <span class="text-xl font-bold w-12 text-center">{{ quantity }}</span>
                <button @click="quantity++" class="w-10 h-10 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors">
                  <svg class="w-5 h-5 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                  </svg>
                </button>
              </div>
            </div>

            <!-- Actions -->
            <div class="flex flex-col sm:flex-row gap-4">
              <button @click="handleAddToCart" class="flex-1 btn-primary py-4 text-lg">
                <svg class="w-6 h-6 inline-block mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                Thêm vào giỏ
              </button>
              <button class="px-8 py-4 border-2 border-primary-500 text-primary-500 rounded-lg font-bold hover:bg-primary-50 transition-all">
                <svg class="w-6 h-6 inline-block" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                </svg>
              </button>
            </div>

            <!-- Features -->
            <div class="mt-8 pt-8 border-t space-y-4">
              <div class="flex items-center space-x-3 text-gray-600">
                <svg class="w-6 h-6 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                </svg>
                <span>Miễn phí vận chuyển cho đơn hàng trên 500.000đ</span>
              </div>
              <div class="flex items-center space-x-3 text-gray-600">
                <svg class="w-6 h-6 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
                <span>Chính hãng 100%</span>
              </div>
              <div class="flex items-center space-x-3 text-gray-600">
                <svg class="w-6 h-6 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                </svg>
                <span>Đổi trả trong 7 ngày</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Related Products -->
      <div class="mt-16">
        <h2 class="text-2xl font-bold text-gray-900 mb-6">Sản phẩm liên quan</h2>
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <div v-for="relatedProduct in relatedProducts" :key="relatedProduct.id" class="group">
            <router-link :to="`/products/${relatedProduct.id}`">
              <div class="bg-white rounded-xl shadow-md overflow-hidden transition-all duration-300 hover:shadow-2xl hover:-translate-y-2">
                <div class="relative overflow-hidden bg-gray-100 aspect-square">
                  <img :src="relatedProduct.image" :alt="relatedProduct.name" class="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-500" />
                </div>
                <div class="p-4">
                  <h3 class="font-bold text-gray-900 mb-2 group-hover:text-primary-600 transition-colors">{{ relatedProduct.name }}</h3>
                  <span class="text-lg font-bold text-primary-600">{{ formatPrice(relatedProduct.price) }}</span>
                </div>
              </div>
            </router-link>
          </div>
        </div>
      </div>
    </div>

    <Footer />

    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, reactive } from 'vue'
import { useRoute } from 'vue-router'
import { useProductStore } from '../../stores/products'
import { useCartStore } from '../../stores/cart'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'

const route = useRoute()
const productStore = useProductStore()
const cartStore = useCartStore()

const selectedSize = ref(null)
const selectedColor = ref(null)
const quantity = ref(1)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const product = computed(() => {
  return productStore.getProductById(route.params.id)
})

const relatedProducts = computed(() => {
  if (!product.value) return []
  return productStore.products
    .filter(p => p.category === product.value.category && p.id !== product.value.id)
    .slice(0, 4)
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function handleAddToCart() {
  if (!selectedSize.value) {
    toast.message = 'Vui lòng chọn size'
    toast.type = 'warning'
    toast.show = true
    return
  }

  for (let i = 0; i < quantity.value; i++) {
    cartStore.addToCart(product.value, selectedSize.value)
  }

  toast.message = `Đã thêm ${quantity.value} sản phẩm vào giỏ hàng`
  toast.type = 'success'
  toast.show = true
}

onMounted(async () => {
  await productStore.fetchProducts()
  
  if (product.value) {
    selectedSize.value = product.value.sizes[0]
    selectedColor.value = product.value.colors[0]
  }
})
</script>

