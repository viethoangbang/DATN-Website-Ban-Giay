<template>
  <Transition
    enter-active-class="transition duration-300 ease-out"
    enter-from-class="opacity-0"
    enter-to-class="opacity-100"
    leave-active-class="transition duration-200 ease-in"
    leave-from-class="opacity-100"
    leave-to-class="opacity-0"
  >
    <div
      v-if="show && product"
      class="fixed inset-0 z-50 overflow-y-auto"
      @click.self="close"
    >
      <!-- Backdrop -->
      <div class="fixed inset-0 bg-black bg-opacity-50 backdrop-blur-sm"></div>

      <!-- Modal -->
      <div class="flex min-h-screen items-center justify-center p-4">
        <Transition
          enter-active-class="transition duration-300 ease-out"
          enter-from-class="transform scale-95 opacity-0"
          enter-to-class="transform scale-100 opacity-100"
          leave-active-class="transition duration-200 ease-in"
          leave-from-class="transform scale-100 opacity-100"
          leave-to-class="transform scale-95 opacity-0"
        >
          <div
            v-if="show"
            class="relative bg-white rounded-2xl shadow-2xl max-w-4xl w-full max-h-[90vh] overflow-hidden"
            @click.stop
          >
            <!-- Close Button -->
            <button
              @click="close"
              class="absolute top-4 right-4 z-10 w-10 h-10 bg-white rounded-full shadow-lg flex items-center justify-center hover:bg-gray-100 transition-colors"
            >
              <svg class="w-6 h-6 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>

            <div class="grid grid-cols-1 md:grid-cols-2 overflow-y-auto max-h-[90vh]">
              <!-- Product Image -->
              <div class="bg-gray-100 p-8 flex items-center justify-center">
                <img
                  :src="displayImage"
                  :alt="product.name"
                  class="w-full h-auto max-h-96 object-contain"
                />
              </div>

              <!-- Product Info -->
              <div class="p-8">
                <!-- Category & Badges -->
                <div class="flex items-center gap-2 mb-3">
                  <span class="inline-block px-3 py-1 bg-primary-100 text-primary-600 rounded-full text-sm font-semibold">
                    {{ product.category }}
                  </span>
                  <span v-if="product.isNew" class="inline-block px-3 py-1 bg-primary-500 text-white rounded-full text-sm font-bold">
                    MỚI
                  </span>
                  <span v-if="selectedVariant && selectedVariant.discountActive" class="inline-block px-3 py-1 bg-red-500 text-white rounded-full text-sm font-bold">
                    {{ selectedVariant.discountType === 'Percentage' 
                      ? `-${selectedVariant.discountValue}%` 
                      : `-${formatCurrency(selectedVariant.discountValue)}` }}
                  </span>
                </div>

                <!-- Product Name -->
                <h2 class="text-2xl md:text-3xl font-bold text-gray-900 mb-3">
                  {{ product.name }}
                </h2>

                <!-- Price -->
                <div class="mb-6">
                  <div v-if="selectedVariantPrice">
                    <div v-if="selectedVariant && selectedVariant.discountActive && selectedVariantDiscountedPrice < selectedVariantPrice" class="mb-2">
                      <span class="text-lg text-gray-400 line-through">
                        {{ formatPrice(selectedVariantPrice) }}
                      </span>
                    </div>
                    <div>
                      <span class="text-3xl font-bold text-primary-600">
                        {{ formatPrice(selectedVariantDiscountedPrice) }}
                      </span>
                    </div>
                  </div>
                  <div v-else>
                    <div>
                      <span class="text-3xl font-bold text-primary-600">
                        {{ formatPrice(productDiscountedPrice) }}
                      </span>
                    </div>
                    <span v-if="product.hasMultiplePrices" class="ml-2 text-sm text-gray-500">
                      (Từ {{ formatPrice(product.minPrice) }} - {{ formatPrice(product.maxPrice) }})
                    </span>
                  </div>
                </div>

                <!-- Description -->
                <p class="text-gray-600 mb-6 leading-relaxed">
                  {{ product.description }}
                </p>

                <!-- Colors (if available) -->
                <div v-if="availableColors.length > 0" class="mb-6">
                  <label class="block text-sm font-bold text-gray-900 mb-3">Màu sắc:</label>
                  <div class="flex flex-wrap gap-3">
                    <button
                      v-for="color in availableColors"
                      :key="color.name"
                      @click="selectedColor = color.name"
                      :class="[
                        'w-10 h-10 rounded-full border-2 transition-all',
                        selectedColor === color.name
                          ? 'border-primary-500 ring-4 ring-primary-100'
                          : 'border-gray-300 hover:border-gray-400'
                      ]"
                      :style="{ backgroundColor: color.value || '#ccc' }"
                      :title="color.name"
                    ></button>
                  </div>
                </div>

                <!-- Size Selection -->
                <div class="mb-6">
                  <label class="block text-sm font-bold text-gray-900 mb-3">Kích cỡ:</label>
                  <div v-if="availableSizes.length === 0" class="text-gray-500 text-sm">
                    {{ selectedColor ? 'Không có size cho màu này' : 'Vui lòng chọn màu sắc trước' }}
                  </div>
                  <div v-else class="grid grid-cols-5 gap-2">
                    <button
                      v-for="size in availableSizes"
                      :key="size"
                      @click="selectedSize = size"
                      :disabled="!isSizeAvailable(size)"
                      :class="[
                        'py-3 px-4 rounded-lg border-2 font-semibold transition-all',
                        selectedSize === size
                          ? 'border-primary-500 bg-primary-500 text-white'
                          : isSizeAvailable(size)
                            ? 'border-gray-300 text-gray-700 hover:border-primary-500'
                            : 'border-gray-200 text-gray-400 cursor-not-allowed opacity-50'
                      ]"
                    >
                      {{ size }}
                    </button>
                  </div>
                </div>

                <!-- Quantity -->
                <div class="mb-6">
                  <label class="block text-sm font-bold text-gray-900 mb-3">Số lượng:</label>
                  <div class="flex items-center space-x-4">
                    <button
                      @click="decreaseQuantity"
                      class="w-10 h-10 rounded-lg border-2 border-gray-300 flex items-center justify-center hover:border-primary-500 transition-colors"
                    >
                      <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
                      </svg>
                    </button>
                    <input
                      v-model.number="quantity"
                      type="number"
                      :min="1"
                      :max="selectedVariant ? (selectedVariant.quantity || 1) : 10"
                      @input="validateQuantity"
                      class="w-20 text-center py-2 border-2 border-gray-300 rounded-lg font-bold text-lg focus:outline-none focus:border-primary-500"
                    />
                    <button
                      @click="increaseQuantity"
                      class="w-10 h-10 rounded-lg border-2 border-gray-300 flex items-center justify-center hover:border-primary-500 transition-colors"
                    >
                      <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                      </svg>
                    </button>
                  </div>
                </div>

                <!-- Stock Status -->
                <div class="mb-6 flex items-center space-x-2">
                  <svg v-if="selectedSize && isSizeAvailable(selectedSize)" class="w-5 h-5 text-green-500" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                  </svg>
                  <svg v-else class="w-5 h-5 text-gray-400" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd" />
                  </svg>
                  <span :class="selectedSize && isSizeAvailable(selectedSize) ? 'text-green-600' : 'text-gray-500'" class="font-semibold">
                    {{ selectedSize && isSizeAvailable(selectedSize) ? 'Còn hàng' : (selectedSize ? 'Hết hàng' : 'Vui lòng chọn size') }}
                  </span>
                </div>

                <!-- Action Buttons -->
                <div class="flex flex-col sm:flex-row gap-3">
                  <button
                    @click="addToCart"
                    :disabled="!selectedSize"
                    class="flex-1 bg-primary-500 text-white py-4 rounded-lg font-bold hover:bg-primary-600 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center space-x-2"
                  >
                    <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                    <span>Thêm vào giỏ</span>
                  </button>
                  <button
                    @click="buyNow"
                    :disabled="!selectedSize"
                    class="flex-1 bg-orange-500 text-white py-4 rounded-lg font-bold hover:bg-orange-600 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                  >
                    Mua ngay
                  </button>
                </div>

                <!-- Additional Info -->
                <div class="mt-6 pt-6 border-t space-y-3">
                  <div class="flex items-center space-x-2 text-sm text-gray-600">
                    <svg class="w-5 h-5 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 8h14M5 8a2 2 0 110-4h14a2 2 0 110 4M5 8v10a2 2 0 002 2h10a2 2 0 002-2V8m-9 4h4" />
                    </svg>
                    <span>Miễn phí vận chuyển cho đơn hàng trên 500.000đ</span>
                  </div>
                  <div class="flex items-center space-x-2 text-sm text-gray-600">
                    <svg class="w-5 h-5 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                    </svg>
                    <span>Đổi trả trong 7 ngày nếu có lỗi</span>
                  </div>
                  <div class="flex items-center space-x-2 text-sm text-gray-600">
                    <svg class="w-5 h-5 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                    <span>100% hàng chính hãng</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </Transition>
      </div>
    </div>
  </Transition>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps({
  show: {
    type: Boolean,
    required: true
  },
  product: {
    type: Object,
    default: null
  }
})

const emit = defineEmits(['close', 'add-to-cart', 'error'])

const router = useRouter()
const selectedSize = ref('')
const selectedColor = ref('')
const quantity = ref(1)

// Extract colors and sizes from variants
const availableColors = computed(() => {
  if (!props.product?.variants || props.product.variants.length === 0) {
    return []
  }
  
  const colorMap = new Map()
  props.product.variants.forEach(variant => {
    if (variant.colorName && !colorMap.has(variant.colorName)) {
      // Try to get color value from color name
      const colorValue = getColorValue(variant.colorName)
      colorMap.set(variant.colorName, {
        name: variant.colorName,
        value: colorValue
      })
    }
  })
  
  return Array.from(colorMap.values())
})

const availableSizes = computed(() => {
  if (!props.product?.variants || props.product.variants.length === 0) {
    return []
  }
  
  // Filter variants by selected color (if color is selected)
  const filteredVariants = selectedColor.value
    ? props.product.variants.filter(v => v.colorName === selectedColor.value)
    : props.product.variants
  
  // Get unique sizes
  const sizeSet = new Set()
  filteredVariants.forEach(variant => {
    if (variant.sizeName) {
      sizeSet.add(variant.sizeName)
    }
  })
  
  // Sort sizes numerically if possible, otherwise alphabetically
  return Array.from(sizeSet).sort((a, b) => {
    const numA = parseFloat(a)
    const numB = parseFloat(b)
    if (!isNaN(numA) && !isNaN(numB)) {
      return numB - numA // Descending order
    }
    return a.localeCompare(b)
  })
})

// Check if size is available (has quantity > 0)
const isSizeAvailable = (size) => {
  if (!props.product?.variants) return false
  
  const variant = props.product.variants.find(v => 
    v.sizeName === size && 
    (!selectedColor.value || v.colorName === selectedColor.value)
  )
  
  return variant && (variant.quantity || 0) > 0
}

// Get selected variant
const selectedVariant = computed(() => {
  if (!props.product?.variants || !selectedSize.value) return null
  
  return props.product.variants.find(v => 
    v.sizeName === selectedSize.value && 
    (!selectedColor.value || v.colorName === selectedColor.value)
  ) || null
})

// Giá gốc (từ backend)
const selectedVariantPrice = computed(() => {
  return selectedVariant.value?.price || null
})

// Giá sau discount (từ backend - đã tính sẵn)
const selectedVariantDiscountedPrice = computed(() => {
  return selectedVariant.value?.finalPrice || selectedVariant.value?.price || null
})

// Calculate discounted price for product (when no variant selected)
const productDiscountedPrice = computed(() => {
  // Nếu có variant với finalPrice, dùng nó
  if (props.product?.variants && props.product.variants.length > 0) {
    const firstVariant = props.product.variants[0]
    return firstVariant.finalPrice || firstVariant.price || props.product.minPrice || props.product.price || 0
  }
  return props.product.minPrice || props.product.price || 0
})

// Get display image based on selected color
const displayImage = computed(() => {
  if (!props.product?.variants) return props.product.image || '/placeholder-shoe.jpg'
  
  // Try to find variant with selected color
  if (selectedColor.value) {
    const colorVariant = props.product.variants.find(v => 
      v.colorName === selectedColor.value && 
      (v.images?.length > 0 || v.imageUrl)
    )
    
    if (colorVariant) {
      return colorVariant.images?.[0]?.url || colorVariant.imageUrl || props.product.image
    }
  }
  
  // Fallback to product image or first variant image
  return props.product.image || 
         props.product.variants[0]?.images?.[0]?.url || 
         props.product.variants[0]?.imageUrl || 
         '/placeholder-shoe.jpg'
})

// Helper function to get color value from color name
function getColorValue(colorName) {
  if (!colorName) return '#ccc'
  
  const colorMap = {
    'red': '#ef4444', 'đỏ': '#ef4444',
    'blue': '#3b82f6', 'xanh dương': '#3b82f6', 'xanh': '#3b82f6',
    'green': '#10b981', 'xanh lá': '#10b981',
    'yellow': '#eab308', 'vàng': '#eab308',
    'orange': '#f97316', 'cam': '#f97316',
    'purple': '#a855f7', 'tím': '#a855f7',
    'pink': '#ec4899', 'hồng': '#ec4899',
    'black': '#000000', 'đen': '#000000',
    'white': '#ffffff', 'trắng': '#ffffff',
    'gray': '#6b7280', 'xám': '#6b7280', 'grey': '#6b7280',
    'brown': '#92400e', 'nâu': '#92400e'
  }
  
  const lowerName = colorName.toLowerCase()
  return colorMap[lowerName] || '#ccc'
}

// Reset when modal opens
watch(() => props.show, (newVal) => {
  if (newVal && props.product) {
    // Reset selections
    selectedColor.value = ''
    selectedSize.value = ''
    quantity.value = 1
    
    // Auto-select first color if available
    if (availableColors.value.length > 0) {
      selectedColor.value = availableColors.value[0].name
    }
    
    // Auto-select first available size
    if (availableSizes.value.length > 0) {
      selectedSize.value = availableSizes.value[0]
    }
  }
})

// Update size options when color changes
watch(selectedColor, () => {
  selectedSize.value = ''
  if (availableSizes.value.length > 0) {
    selectedSize.value = availableSizes.value[0]
  }
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function decreaseQuantity() {
  if (quantity.value > 1) {
    quantity.value--
  }
}

function increaseQuantity() {
  if (!selectedVariant.value) return
  const stock = selectedVariant.value.quantity || 0
  if (quantity.value < stock) {
    quantity.value++
  } else {
    emit('error', `Chỉ còn ${stock} sản phẩm trong kho`)
  }
}

function validateQuantity() {
  if (!selectedVariant.value) return
  const stock = selectedVariant.value.quantity || 0
  if (quantity.value > stock) {
    quantity.value = stock
    emit('error', `Chỉ còn ${stock} sản phẩm trong kho`)
  }
  if (quantity.value < 1) {
    quantity.value = 1
  }
}

function close() {
  emit('close')
}

function addToCart() {
  if (!selectedSize.value) {
    emit('error', 'Vui lòng chọn size')
    return
  }
  
  // Find the exact variant
  const variant = props.product.variants?.find(v => 
    v.sizeName === selectedSize.value && 
    (!selectedColor.value || v.colorName === selectedColor.value)
  )
  
  if (!variant) {
    emit('error', 'Không tìm thấy biến thể sản phẩm')
    return
  }

  // Validate stock
  const stock = variant.quantity || 0
  if (stock === 0) {
    emit('error', 'Sản phẩm đã hết hàng')
    return
  }

  if (quantity.value > stock) {
    emit('error', `Chỉ còn ${stock} sản phẩm trong kho`)
    return
  }

  emit('add-to-cart', {
    product: {
      ...props.product,
      variantId: variant.id,
      price: variant.price
    },
    size: selectedSize.value,
    color: selectedColor.value || variant.colorName,
    quantity: quantity.value
  })
  close()
}

function buyNow() {
  if (!selectedSize.value) return
  
  // Find the exact variant
  const variant = props.product.variants?.find(v => 
    v.sizeName === selectedSize.value && 
    (!selectedColor.value || v.colorName === selectedColor.value)
  )
  
  if (!variant) {
    alert('Không tìm thấy biến thể sản phẩm')
    return
  }

  emit('add-to-cart', {
    product: {
      ...props.product,
      variantId: variant.id,
      price: variant.price
    },
    size: selectedSize.value,
    color: selectedColor.value || variant.colorName,
    quantity: quantity.value
  })
  close()
  router.push('/cart')
}
</script>

