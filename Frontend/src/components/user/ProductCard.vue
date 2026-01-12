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
      <span v-if="product.discountInfo" class="bg-red-500 text-white px-3 py-1 rounded-full text-xs font-bold shadow-lg">
        {{ product.discountInfo.discountType === 'Percentage' 
          ? `-${product.discountInfo.discountValue}%` 
          : `-${formatCurrency(product.discountInfo.discountValue)}` }}
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
    
    <!-- Content - Fixed Layout -->
    <div class="p-4 flex flex-col h-[200px]">
      <!-- Category - 1 row, fixed height -->
      <div class="h-5 mb-2 flex items-center">
        <span class="text-xs text-primary-600 font-semibold line-clamp-1">{{ product.category || 'Uncategorized' }}</span>
      </div>
      
      <!-- Title - 1 row, fixed height -->
      <router-link :to="`/products/${product.id}`" class="h-6 mb-2 flex items-center">
        <h3 
          class="text-sm font-bold text-gray-900 line-clamp-1 group-hover:text-primary-600 transition-colors"
          :title="product.name"
        >
          {{ product.name }}
        </h3>
      </router-link>
      
      <!-- Description - 2 rows, fixed height -->
      <div class="h-10 mb-3 flex items-start">
        <p 
          class="text-xs text-gray-500 line-clamp-2 leading-tight"
          :title="product.description"
        >
          {{ product.description || 'Chưa có mô tả' }}
        </p>
      </div>
      
      <!-- Price - 2 rows, fixed height -->
      <div class="h-12 mt-auto flex flex-col justify-end space-y-1">
        <div v-if="product.discountInfo && discountedPrice < (product.price || product.minPrice || 0)" class="h-4 flex items-center">
          <span class="text-xs text-gray-400 line-through">
            {{ formatPrice(product.price || product.minPrice || 0) }}
          </span>
        </div>
        <div class="h-6 flex items-center">
          <span class="text-base font-bold text-primary-600">
            {{ formatPrice(discountedPrice) }}
          </span>
        </div>
      </div>
      
      <!-- Colors - below price -->
      <div v-if="productColors.length > 0" class="mt-2 flex items-center gap-1.5">
        <div class="flex items-center gap-1.5">
          <span
            v-for="(color, index) in productColors.slice(0, 6)"
            :key="index"
            class="w-4 h-4 rounded-full border border-gray-300 shadow-sm"
            :style="{ backgroundColor: getColorCode(color) }"
            :title="color"
          ></span>
          <span v-if="productColors.length > 6" class="text-xs text-gray-400 ml-1">+{{ productColors.length - 6 }}</span>
        </div>
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

// Helper function to extract image URL from product
function getImageUrl(product) {
  if (!product) return ''
  
  // If image is a string URL, use it directly
  if (typeof product.image === 'string' && product.image) {
    return product.image
  }
  
  // If image is an object with url property
  if (product.image && typeof product.image === 'object' && product.image.url) {
    return product.image.url
  }
  
  // If images array exists, use first image
  if (product.images && Array.isArray(product.images) && product.images.length > 0) {
    const firstImage = product.images[0]
    return typeof firstImage === 'string' ? firstImage : (firstImage?.url || '')
  }
  
  // If variant has imageUrl
  if (product.variants && product.variants.length > 0) {
    const firstVariant = product.variants[0]
    if (firstVariant.images && Array.isArray(firstVariant.images) && firstVariant.images.length > 0) {
      const firstImg = firstVariant.images[0]
      return typeof firstImg === 'string' ? firstImg : (firstImg?.url || '')
    }
    if (firstVariant.imageUrl) {
      return firstVariant.imageUrl
    }
  }
  
  // Fallback to empty string (will trigger error handler)
  return ''
}

const currentImage = ref(getImageUrl(props.product))
const adding = ref(false)

// Get unique colors from variants
const productColors = computed(() => {
  if (props.product.variants && props.product.variants.length > 0) {
    return [...new Set(props.product.variants.map(v => v.colorName).filter(Boolean))]
  }
  if (props.product.colors && props.product.colors.length > 0) {
    return props.product.colors
  }
  return []
})

// Sử dụng finalPrice từ backend (đã tính discount sẵn)
// Nếu không có finalPrice, dùng price
const discountedPrice = computed(() => {
  // Nếu có variant với finalPrice, dùng nó
  if (props.product.variants && props.product.variants.length > 0) {
    const firstVariant = props.product.variants.find(v => v.finalPrice) || props.product.variants[0]
    return firstVariant?.finalPrice || firstVariant?.price || props.product.price || props.product.minPrice || 0
  }
  return props.product.price || props.product.minPrice || 0
})

function formatPrice(price) {
  if (!price) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function formatCurrency(amount) {
  if (!amount) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
    maximumFractionDigits: 0
  }).format(amount)
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
  // Use SVG data URI as fallback instead of external URL
  currentImage.value = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNDAwIiBoZWlnaHQ9IjQwMCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cmVjdCB3aWR0aD0iNDAwIiBoZWlnaHQ9IjQwMCIgZmlsbD0iI2YzZjRmNiIvPjx0ZXh0IHg9IjUwJSIgeT0iNTAlIiBmb250LWZhbWlseT0iQXJpYWwiIGZvbnQtc2l6ZT0iMTgiIGZpbGw9IiM5Y2EzYWYiIHRleHQtYW5jaG9yPSJtaWRkbGUiIGR5PSIuM2VtIj5ObyBJbWFnZTwvdGV4dD48L3N2Zz4='
}

async function addToCart() {
  // ProductCard không có đủ thông tin variant để add to cart trực tiếp
  // Nên redirect đến product detail hoặc mở quick view
  // Tạm thời emit quick-view event để xử lý ở parent component
  emit('quick-view', props.product)
  
  // Hoặc nếu có variant đầu tiên, có thể add luôn
  if (props.product.variants && props.product.variants.length > 0) {
    const firstVariant = props.product.variants[0]
    if (firstVariant?.id) {
      adding.value = true
      try {
        await cartStore.addToCart(firstVariant.id, 1)
        adding.value = false
      } catch (error) {
        adding.value = false
        // Emit để parent component hiển thị error nếu cần
        console.error('Error adding to cart:', error)
      }
    } else {
      // Không có variant ID, emit quick-view
      emit('quick-view', props.product)
      adding.value = false
    }
  } else {
    // Không có variant, emit quick-view
    emit('quick-view', props.product)
    adding.value = false
  }
}
</script>

