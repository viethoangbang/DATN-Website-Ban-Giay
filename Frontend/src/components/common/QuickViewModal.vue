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
                  :src="product.image"
                  :alt="product.name"
                  class="w-full h-auto max-h-96 object-contain"
                />
              </div>

              <!-- Product Info -->
              <div class="p-8">
                <!-- Category -->
                <span class="inline-block px-3 py-1 bg-primary-100 text-primary-600 rounded-full text-sm font-semibold mb-3">
                  {{ product.category }}
                </span>

                <!-- Product Name -->
                <h2 class="text-2xl md:text-3xl font-bold text-gray-900 mb-3">
                  {{ product.name }}
                </h2>

                <!-- Rating -->
                <div class="flex items-center space-x-2 mb-4">
                  <div class="flex items-center">
                    <svg v-for="i in 5" :key="i" class="w-5 h-5 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
                      <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                    </svg>
                  </div>
                  <span class="text-gray-600 text-sm">({{ product.rating }} đánh giá)</span>
                </div>

                <!-- Price -->
                <div class="mb-6">
                  <div v-if="product.salePrice" class="flex items-center space-x-3">
                    <span class="text-3xl font-bold text-red-600">
                      {{ formatPrice(product.salePrice) }}
                    </span>
                    <span class="text-xl text-gray-400 line-through">
                      {{ formatPrice(product.price) }}
                    </span>
                    <span class="px-3 py-1 bg-red-100 text-red-600 rounded-full text-sm font-bold">
                      -{{ product.discount }}%
                    </span>
                  </div>
                  <div v-else>
                    <span class="text-3xl font-bold text-primary-600">
                      {{ formatPrice(product.price) }}
                    </span>
                  </div>
                </div>

                <!-- Description -->
                <p class="text-gray-600 mb-6 leading-relaxed">
                  {{ product.description }}
                </p>

                <!-- Colors (if available) -->
                <div v-if="product.colors && product.colors.length > 0" class="mb-6">
                  <label class="block text-sm font-bold text-gray-900 mb-3">Màu sắc:</label>
                  <div class="flex flex-wrap gap-3">
                    <button
                      v-for="color in product.colors"
                      :key="color"
                      @click="selectedColor = color"
                      :class="[
                        'w-10 h-10 rounded-full border-2 transition-all',
                        selectedColor === color
                          ? 'border-primary-500 ring-4 ring-primary-100'
                          : 'border-gray-300 hover:border-gray-400'
                      ]"
                      :style="{ backgroundColor: color }"
                      :title="color"
                    ></button>
                  </div>
                </div>

                <!-- Size Selection -->
                <div class="mb-6">
                  <label class="block text-sm font-bold text-gray-900 mb-3">Kích cỡ:</label>
                  <div class="grid grid-cols-5 gap-2">
                    <button
                      v-for="size in product.sizes"
                      :key="size"
                      @click="selectedSize = size"
                      :class="[
                        'py-3 px-4 rounded-lg border-2 font-semibold transition-all',
                        selectedSize === size
                          ? 'border-primary-500 bg-primary-500 text-white'
                          : 'border-gray-300 text-gray-700 hover:border-primary-500'
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
                      min="1"
                      max="10"
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
                  <svg class="w-5 h-5 text-green-500" fill="currentColor" viewBox="0 0 20 20">
                    <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                  </svg>
                  <span class="text-green-600 font-semibold">Còn hàng</span>
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
import { ref, watch } from 'vue'
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

const emit = defineEmits(['close', 'add-to-cart'])

const router = useRouter()
const selectedSize = ref('')
const selectedColor = ref('')
const quantity = ref(1)

// Reset when modal opens
watch(() => props.show, (newVal) => {
  if (newVal && props.product) {
    selectedSize.value = props.product.sizes?.[0] || ''
    selectedColor.value = props.product.colors?.[0] || ''
    quantity.value = 1
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
  if (quantity.value < 10) {
    quantity.value++
  }
}

function close() {
  emit('close')
}

function addToCart() {
  if (!selectedSize.value) return

  emit('add-to-cart', {
    product: props.product,
    size: selectedSize.value,
    color: selectedColor.value,
    quantity: quantity.value
  })
  close()
}

function buyNow() {
  if (!selectedSize.value) return

  emit('add-to-cart', {
    product: props.product,
    size: selectedSize.value,
    color: selectedColor.value,
    quantity: quantity.value
  })
  close()
  router.push('/cart')
}
</script>

