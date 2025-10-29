<template>
  <div
    class="group relative bg-white rounded-xl shadow-md overflow-hidden transition-all duration-300 hover:shadow-2xl hover:-translate-y-2"
  >
    <!-- Quick Actions -->
    <div class="absolute top-4 right-4 z-10 flex flex-col space-y-2 opacity-0 group-hover:opacity-100 transition-all duration-300 transform translate-x-4 group-hover:translate-x-0">
      <button
        @click.prevent="toggleWishlist"
        :class="[
          'w-10 h-10 rounded-full backdrop-blur-sm flex items-center justify-center transition-all duration-300',
          isWishlisted ? 'bg-red-500 text-white' : 'bg-white/90 text-gray-700 hover:bg-red-500 hover:text-white'
        ]"
        title="Yêu thích"
      >
        <svg class="w-5 h-5" :fill="isWishlisted ? 'currentColor' : 'none'" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
        </svg>
      </button>
      
      <button
        @click.prevent="$emit('quick-view', product)"
        class="w-10 h-10 bg-white/90 backdrop-blur-sm rounded-full text-gray-700 hover:bg-primary-500 hover:text-white flex items-center justify-center transition-all duration-300"
        title="Xem nhanh"
      >
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
        </svg>
      </button>
    </div>
    
    <!-- Badges -->
    <div class="absolute top-4 left-4 z-10 flex flex-col space-y-2">
      <span v-if="product.isNew" class="bg-primary-500 text-white px-3 py-1 rounded-full text-xs font-bold shadow-lg">
        MỚI
      </span>
      <span v-if="product.discount" class="bg-red-500 text-white px-3 py-1 rounded-full text-xs font-bold shadow-lg">
        -{{ product.discount }}%
      </span>
      <span v-if="product.stock < 10" class="bg-yellow-500 text-white px-3 py-1 rounded-full text-xs font-bold shadow-lg">
        Sắp hết
      </span>
    </div>
    
    <!-- Image -->
    <router-link :to="`/products/${product.id}`">
      <div class="relative overflow-hidden bg-gray-100 aspect-square">
        <img
          :src="currentImage"
          :alt="product.name"
          class="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-500"
          @error="handleImageError"
        />
        <div class="absolute inset-0 bg-gradient-to-t from-black/20 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300"></div>
      </div>
    </router-link>
    
    <!-- Content -->
    <div class="p-6">
      <!-- Category & Rating -->
      <div class="flex items-center justify-between mb-2">
        <span class="text-sm text-primary-600 font-semibold">{{ product.category }}</span>
        <div class="flex items-center space-x-1">
          <svg class="w-4 h-4 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
            <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
          </svg>
          <span class="text-sm text-gray-600 font-medium">{{ product.rating }}</span>
        </div>
      </div>
      
      <!-- Title -->
      <router-link :to="`/products/${product.id}`">
        <h3 class="text-lg font-bold text-gray-900 mb-2 line-clamp-2 group-hover:text-primary-600 transition-colors">
          {{ product.name }}
        </h3>
      </router-link>
      
      <!-- Description -->
      <p class="text-gray-600 text-sm mb-4 line-clamp-2">
        {{ product.description }}
      </p>
      
      <!-- Colors -->
      <div v-if="product.colors && product.colors.length > 0" class="flex items-center space-x-2 mb-4">
        <span class="text-xs text-gray-500">Màu:</span>
        <div class="flex space-x-1">
          <button
            v-for="(color, index) in product.colors.slice(0, 4)"
            :key="index"
            @click.prevent="selectColor(color)"
            :class="[
              'w-5 h-5 rounded-full border-2 transition-all',
              selectedColor === color ? 'border-primary-500 scale-110' : 'border-gray-300'
            ]"
            :style="{ backgroundColor: getColorCode(color) }"
            :title="color"
          ></button>
        </div>
      </div>
      
      <!-- Price & Action -->
      <div class="flex items-center justify-between">
        <div>
          <span v-if="product.discount" class="text-lg text-gray-400 line-through mr-2">
            {{ formatPrice(product.price) }}
          </span>
          <span class="text-2xl font-bold text-primary-600">
            {{ formatPrice(discountedPrice) }}
          </span>
        </div>
        
        <button
          @click.prevent="addToCart"
          :disabled="adding"
          class="bg-primary-500 text-white p-3 rounded-lg hover:bg-primary-600 transition-all transform hover:scale-110 disabled:opacity-50 disabled:cursor-not-allowed"
          title="Thêm vào giỏ"
        >
          <svg v-if="!adding" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
          </svg>
          <svg v-else class="w-5 h-5 animate-spin" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useCartStore } from '../../stores/cart'

const props = defineProps({
  product: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['quick-view', 'wishlist-toggle'])

const cartStore = useCartStore()

const isWishlisted = ref(false)
const selectedColor = ref(props.product.colors?.[0] || null)
const currentImage = ref(props.product.image)
const adding = ref(false)

const discountedPrice = computed(() => {
  if (props.product.discount) {
    return props.product.price * (1 - props.product.discount / 100)
  }
  return props.product.price
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function toggleWishlist() {
  isWishlisted.value = !isWishlisted.value
  emit('wishlist-toggle', { product: props.product, wishlisted: isWishlisted.value })
}

function selectColor(color) {
  selectedColor.value = color
}

function getColorCode(colorName) {
  const colorMap = {
    'Black': '#000000',
    'White': '#FFFFFF',
    'Red': '#EF4444',
    'Blue': '#3B82F6',
    'Green': '#10B981',
    'Yellow': '#F59E0B',
    'Purple': '#8B5CF6',
    'Pink': '#EC4899',
    'Gray': '#6B7280',
    'Brown': '#92400E',
    'Navy': '#1E3A8A',
    'Orange': '#F97316'
  }
  return colorMap[colorName] || '#6B7280'
}

function handleImageError() {
  currentImage.value = 'https://via.placeholder.com/400x400?text=No+Image'
}

async function addToCart() {
  adding.value = true
  await new Promise(resolve => setTimeout(resolve, 500))
  
  const defaultSize = props.product.sizes?.[0] || 40
  cartStore.addToCart(props.product, defaultSize)
  
  adding.value = false
}
</script>

