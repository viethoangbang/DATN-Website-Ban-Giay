<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Page Header -->
      <div class="mb-8 animate-fade-in-up">
        <h1 class="text-3xl md:text-4xl font-bold text-gray-900 mb-2">
          Tất cả sản phẩm
        </h1>
        <p class="text-gray-600">Khám phá bộ sưu tập giày thể thao đa dạng</p>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-4 gap-8">
        <!-- Filters Sidebar -->
        <aside class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 space-y-6 animate-fade-in-up">
            <div>
              <h3 class="font-bold text-gray-900 mb-4">Danh mục</h3>
              <div class="space-y-2">
                <button
                  v-for="category in categories"
                  :key="category"
                  @click="selectedCategory = category"
                  :class="[
                    'w-full text-left px-4 py-2 rounded-lg transition-all duration-300',
                    selectedCategory === category
                      ? 'bg-primary-500 text-white'
                      : 'hover:bg-gray-100 text-gray-700'
                  ]"
                >
                  {{ category }}
                </button>
              </div>
            </div>

            <div class="pt-6 border-t">
              <h3 class="font-bold text-gray-900 mb-4">Giá</h3>
              <div class="space-y-3">
                <div>
                  <input
                    v-model="priceRange.min"
                    type="number"
                    placeholder="Từ"
                    class="input-field"
                  />
                </div>
                <div>
                  <input
                    v-model="priceRange.max"
                    type="number"
                    placeholder="Đến"
                    class="input-field"
                  />
                </div>
              </div>
            </div>

            <div class="pt-6 border-t">
              <h3 class="font-bold text-gray-900 mb-4">Đánh giá</h3>
              <div class="space-y-2">
                <button
                  v-for="rating in [5, 4, 3]"
                  :key="rating"
                  @click="selectedRating = rating"
                  :class="[
                    'w-full flex items-center space-x-2 px-4 py-2 rounded-lg transition-all',
                    selectedRating === rating ? 'bg-primary-50 text-primary-600' : 'hover:bg-gray-100'
                  ]"
                >
                  <div class="flex">
                    <svg v-for="i in rating" :key="i" class="w-4 h-4 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                    </svg>
                  </div>
                  <span class="text-sm">& trở lên</span>
                </button>
              </div>
            </div>

            <button
              @click="resetFilters"
              class="w-full btn-secondary"
            >
              Xóa bộ lọc
            </button>
          </div>
        </aside>

        <!-- Products Grid -->
        <div class="lg:col-span-3">
          <!-- Sort & View Options -->
          <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-6 animate-fade-in-up">
            <p class="text-gray-600">
              Hiển thị <span class="font-semibold">{{ filteredProducts.length }}</span> sản phẩm
            </p>
            <select v-model="sortBy" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
              <option value="default">Mặc định</option>
              <option value="price-asc">Giá: Thấp đến cao</option>
              <option value="price-desc">Giá: Cao đến thấp</option>
              <option value="rating">Đánh giá cao nhất</option>
              <option value="newest">Mới nhất</option>
            </select>
          </div>

          <!-- Loading Spinner -->
          <LoadingSpinner :show="productStore.loading" message="Đang tải sản phẩm..." />

          <!-- Products Grid -->
          <div v-if="!productStore.loading" class="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-6">
            <div
              v-for="(product, index) in sortedProducts"
              :key="product.id"
              class="group animate-fade-in-up"
              :style="{ animationDelay: `${index * 0.05}s` }"
            >
              <router-link :to="`/products/${product.id}`">
                <div class="bg-white rounded-xl shadow-md overflow-hidden transition-all duration-300 hover:shadow-2xl hover:-translate-y-2">
                  <div class="relative overflow-hidden bg-gray-100 aspect-square">
                    <img
                      :src="product.image"
                      :alt="product.name"
                      class="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-500"
                    />
                    <div class="absolute top-4 right-4 bg-primary-500 text-white px-3 py-1 rounded-full text-sm font-bold">
                      New
                    </div>
                  </div>
                  <div class="p-6">
                    <div class="flex items-center justify-between mb-2">
                      <span class="text-sm text-primary-600 font-semibold">{{ product.category }}</span>
                      <div class="flex items-center space-x-1">
                        <svg class="w-4 h-4 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
                          <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                        </svg>
                        <span class="text-sm text-gray-600">{{ product.rating }}</span>
                      </div>
                    </div>
                    <h3 class="text-lg font-bold text-gray-900 mb-2 group-hover:text-primary-600 transition-colors">
                      {{ product.name }}
                    </h3>
                    <p class="text-gray-600 text-sm mb-4 line-clamp-2">{{ product.description }}</p>
                    <div class="flex items-center justify-between">
                      <span class="text-2xl font-bold text-primary-600">
                        {{ formatPrice(product.price) }}
                      </span>
                      <button
                        @click.prevent="addToCart(product)"
                        class="bg-primary-500 text-white p-2 rounded-lg hover:bg-primary-600 transition-colors transform hover:scale-110"
                      >
                        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                        </svg>
                      </button>
                    </div>
                  </div>
                </div>
              </router-link>
            </div>
          </div>

          <!-- Empty State -->
          <div v-if="!productStore.loading && filteredProducts.length === 0" class="text-center py-12">
            <svg class="w-24 h-24 mx-auto text-gray-400 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
            </svg>
            <h3 class="text-xl font-bold text-gray-900 mb-2">Không tìm thấy sản phẩm</h3>
            <p class="text-gray-600 mb-4">Thử điều chỉnh bộ lọc của bạn</p>
            <button @click="resetFilters" class="btn-primary">
              Xóa bộ lọc
            </button>
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
import LoadingSpinner from '../../components/common/LoadingSpinner.vue'
import Toast from '../../components/common/Toast.vue'

const route = useRoute()
const productStore = useProductStore()
const cartStore = useCartStore()

const selectedCategory = ref('All')
const selectedRating = ref(0)
const sortBy = ref('default')
const priceRange = reactive({ min: 0, max: 10000000 })

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const categories = computed(() => productStore.categories)

const filteredProducts = computed(() => {
  return productStore.products.filter(product => {
    const matchCategory = selectedCategory.value === 'All' || product.category === selectedCategory.value
    const matchRating = selectedRating.value === 0 || product.rating >= selectedRating.value
    const matchPrice = product.price >= priceRange.min && (priceRange.max === 0 || product.price <= priceRange.max)
    return matchCategory && matchRating && matchPrice
  })
})

const sortedProducts = computed(() => {
  const products = [...filteredProducts.value]
  
  switch (sortBy.value) {
    case 'price-asc':
      return products.sort((a, b) => a.price - b.price)
    case 'price-desc':
      return products.sort((a, b) => b.price - a.price)
    case 'rating':
      return products.sort((a, b) => b.rating - a.rating)
    case 'newest':
      return products.reverse()
    default:
      return products
  }
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function resetFilters() {
  selectedCategory.value = 'All'
  selectedRating.value = 0
  sortBy.value = 'default'
  priceRange.min = 0
  priceRange.max = 10000000
}

function addToCart(product) {
  const defaultSize = product.sizes[0]
  cartStore.addToCart(product, defaultSize)
  
  toast.message = `Đã thêm ${product.name} vào giỏ hàng`
  toast.type = 'success'
  toast.show = true
}

onMounted(async () => {
  await productStore.fetchProducts()
  
  // Check for category from query params
  if (route.query.category) {
    selectedCategory.value = route.query.category
  }
})
</script>

