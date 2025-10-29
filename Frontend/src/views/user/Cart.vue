<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 class="text-3xl font-bold text-gray-900 mb-8 animate-fade-in-up">Giỏ hàng của bạn</h1>

      <div v-if="cartStore.items.length > 0" class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Cart Items -->
        <div class="lg:col-span-2 space-y-4">
          <div
            v-for="(item, index) in cartStore.items"
            :key="`${item.id}-${item.size}-${item.color}`"
            class="bg-white rounded-xl shadow-md p-6 flex flex-col sm:flex-row gap-6 animate-fade-in-up"
            :style="{ animationDelay: `${index * 0.1}s` }"
          >
            <img :src="item.image" :alt="item.name" class="w-32 h-32 object-cover rounded-lg" />
            
            <div class="flex-1">
              <div class="flex justify-between items-start mb-2">
                <div>
                  <h3 class="text-lg font-bold text-gray-900 mb-1">{{ item.name }}</h3>
                  <div class="flex items-center space-x-3 text-sm">
                    <p class="text-gray-600">Size: {{ item.size }}</p>
                    <span v-if="item.color" class="flex items-center space-x-1">
                      <span class="text-gray-400">|</span>
                      <span class="text-gray-600">Màu:</span>
                      <span
                        class="inline-block w-4 h-4 rounded-full border border-gray-300"
                        :style="{ backgroundColor: item.color }"
                        :title="item.color"
                      ></span>
                    </span>
                  </div>
                </div>
                <button
                  @click="removeItem(item)"
                  class="text-red-600 hover:text-red-700 p-2 hover:bg-red-50 rounded-lg transition-colors"
                >
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>

              <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mt-4">
                <div class="text-2xl font-bold text-primary-600">
                  {{ formatPrice(item.price) }}
                </div>

                <div class="flex items-center space-x-3">
                  <button
                    @click="updateQuantity(item, item.quantity - 1)"
                    class="w-8 h-8 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors flex items-center justify-center"
                  >
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
                    </svg>
                  </button>
                  <span class="text-lg font-semibold w-8 text-center">{{ item.quantity }}</span>
                  <button
                    @click="updateQuantity(item, item.quantity + 1)"
                    class="w-8 h-8 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors flex items-center justify-center"
                  >
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                    </svg>
                  </button>
                </div>

                <div class="text-xl font-bold text-gray-900">
                  {{ formatPrice(item.price * item.quantity) }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 animate-fade-in-up" style="animation-delay: 0.3s;">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Tóm tắt đơn hàng</h2>

            <div class="space-y-4 mb-6">
              <div class="flex justify-between text-gray-600">
                <span>Tạm tính ({{ cartStore.totalItems }} sản phẩm)</span>
                <span class="font-semibold">{{ formatPrice(cartStore.totalPrice) }}</span>
              </div>
              <div class="flex justify-between text-gray-600">
                <span>Phí vận chuyển</span>
                <span class="font-semibold">{{ shippingFee === 0 ? 'Miễn phí' : formatPrice(shippingFee) }}</span>
              </div>
              <div class="border-t pt-4 flex justify-between text-lg font-bold text-gray-900">
                <span>Tổng cộng</span>
                <span class="text-primary-600">{{ formatPrice(cartStore.totalPrice + shippingFee) }}</span>
              </div>
            </div>

            <router-link to="/checkout" class="block w-full btn-primary text-center py-4 text-lg mb-3">
              Tiến hành thanh toán
            </router-link>

            <router-link to="/products" class="block w-full btn-secondary text-center py-3">
              Tiếp tục mua sắm
            </router-link>

            <!-- Promo Code -->
            <div class="mt-6 pt-6 border-t">
              <h3 class="font-semibold text-gray-900 mb-3">Mã giảm giá</h3>
              <div class="flex gap-2">
                <input
                  v-model="promoCode"
                  type="text"
                  placeholder="Nhập mã giảm giá"
                  class="flex-1 input-field"
                />
                <button class="btn-secondary whitespace-nowrap">Áp dụng</button>
              </div>
            </div>

            <!-- Free Shipping Notice -->
            <div v-if="cartStore.totalPrice < 500000" class="mt-6 p-4 bg-primary-50 rounded-lg">
              <p class="text-sm text-primary-800">
                <svg class="w-5 h-5 inline mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
                Mua thêm {{ formatPrice(500000 - cartStore.totalPrice) }} để được miễn phí vận chuyển
              </p>
              <div class="mt-2 bg-primary-200 rounded-full h-2">
                <div class="bg-primary-500 h-2 rounded-full transition-all duration-500" :style="{ width: `${(cartStore.totalPrice / 500000) * 100}%` }"></div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Empty Cart -->
      <div v-else class="text-center py-16 bg-white rounded-xl shadow-md animate-fade-in">
        <svg class="w-32 h-32 mx-auto text-gray-400 mb-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
        </svg>
        <h2 class="text-2xl font-bold text-gray-900 mb-4">Giỏ hàng trống</h2>
        <p class="text-gray-600 mb-8">Hãy thêm sản phẩm vào giỏ hàng để tiếp tục mua sắm</p>
        <router-link to="/products" class="btn-primary inline-block">
          Khám phá sản phẩm
        </router-link>
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
import { ref, computed, reactive } from 'vue'
import { useCartStore } from '../../stores/cart'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'

const cartStore = useCartStore()
const promoCode = ref('')

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const shippingFee = computed(() => {
  return cartStore.totalPrice >= 500000 ? 0 : 30000
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function removeItem(item) {
  cartStore.removeFromCart(item.id, item.size, item.color)
  toast.message = 'Đã xóa sản phẩm khỏi giỏ hàng'
  toast.type = 'success'
  toast.show = true
}

function updateQuantity(item, newQuantity) {
  if (newQuantity <= 0) {
    removeItem(item)
  } else {
    cartStore.updateQuantity(item.id, item.size, newQuantity, item.color)
  }
}
</script>

