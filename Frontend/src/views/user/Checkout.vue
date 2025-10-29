<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 class="text-3xl font-bold text-gray-900 mb-8 animate-fade-in-up">Thanh toán</h1>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Checkout Form -->
        <div class="lg:col-span-2 space-y-6">
          <!-- Shipping Information -->
          <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Thông tin giao hàng</h2>
            <form class="space-y-4">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Họ và tên *</label>
                  <input v-model="form.fullName" type="text" required class="input-field" placeholder="Nguyễn Văn A" />
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Số điện thoại *</label>
                  <input v-model="form.phone" type="tel" required class="input-field" placeholder="0912345678" />
                </div>
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">Email</label>
                <input v-model="form.email" type="email" class="input-field" placeholder="email@example.com" />
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">Địa chỉ *</label>
                <input v-model="form.address" type="text" required class="input-field" placeholder="Số nhà, tên đường" />
              </div>

              <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Tỉnh/Thành phố *</label>
                  <select v-model="form.city" required class="input-field">
                    <option value="">Chọn tỉnh/thành</option>
                    <option>Hà Nội</option>
                    <option>TP. Hồ Chí Minh</option>
                    <option>Đà Nẵng</option>
                  </select>
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Quận/Huyện *</label>
                  <select v-model="form.district" required class="input-field">
                    <option value="">Chọn quận/huyện</option>
                  </select>
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Phường/Xã *</label>
                  <select v-model="form.ward" required class="input-field">
                    <option value="">Chọn phường/xã</option>
                  </select>
                </div>
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">Ghi chú</label>
                <textarea v-model="form.note" rows="3" class="input-field" placeholder="Ghi chú về đơn hàng (tùy chọn)"></textarea>
              </div>
            </form>
          </div>

          <!-- Payment Method -->
          <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.1s;">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Phương thức thanh toán</h2>
            <div class="space-y-3">
              <label
                v-for="method in paymentMethods"
                :key="method.id"
                :class="[
                  'flex items-center p-4 border-2 rounded-lg cursor-pointer transition-all duration-300',
                  form.paymentMethod === method.id
                    ? 'border-primary-500 bg-primary-50'
                    : 'border-gray-200 hover:border-primary-300'
                ]"
              >
                <input
                  v-model="form.paymentMethod"
                  type="radio"
                  :value="method.id"
                  class="w-5 h-5 text-primary-600 focus:ring-primary-500"
                />
                <div class="ml-4 flex-1">
                  <div class="flex items-center justify-between">
                    <span class="font-semibold text-gray-900">{{ method.name }}</span>
                    <component :is="method.icon" class="w-8 h-8" />
                  </div>
                  <p class="text-sm text-gray-600 mt-1">{{ method.description }}</p>
                </div>
              </label>
            </div>
          </div>
        </div>

        <!-- Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 animate-fade-in-up" style="animation-delay: 0.2s;">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Đơn hàng của bạn</h2>

            <!-- Cart Items -->
            <div class="space-y-4 mb-6 max-h-80 overflow-y-auto">
              <div v-for="item in cartStore.items" :key="`${item.id}-${item.size}`" class="flex items-center space-x-4">
                <div class="relative">
                  <img :src="item.image" :alt="item.name" class="w-16 h-16 object-cover rounded-lg" />
                  <span class="absolute -top-2 -right-2 bg-primary-500 text-white text-xs font-bold rounded-full w-6 h-6 flex items-center justify-center">
                    {{ item.quantity }}
                  </span>
                </div>
                <div class="flex-1 min-w-0">
                  <h4 class="font-semibold text-gray-900 text-sm truncate">{{ item.name }}</h4>
                  <p class="text-xs text-gray-600">Size: {{ item.size }}</p>
                </div>
                <span class="font-semibold text-gray-900 text-sm">
                  {{ formatPrice(item.price * item.quantity) }}
                </span>
              </div>
            </div>

            <!-- Pricing -->
            <div class="space-y-3 mb-6 pb-6 border-b">
              <div class="flex justify-between text-gray-600">
                <span>Tạm tính</span>
                <span class="font-semibold">{{ formatPrice(cartStore.totalPrice) }}</span>
              </div>
              <div class="flex justify-between text-gray-600">
                <span>Phí vận chuyển</span>
                <span class="font-semibold">{{ shippingFee === 0 ? 'Miễn phí' : formatPrice(shippingFee) }}</span>
              </div>
              <div v-if="discount > 0" class="flex justify-between text-green-600">
                <span>Giảm giá</span>
                <span class="font-semibold">-{{ formatPrice(discount) }}</span>
              </div>
            </div>

            <div class="flex justify-between text-xl font-bold text-gray-900 mb-6">
              <span>Tổng cộng</span>
              <span class="text-primary-600">{{ formatPrice(total) }}</span>
            </div>

            <button
              @click="handleCheckout"
              :disabled="!isFormValid || isProcessing"
              class="w-full btn-primary py-4 text-lg disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span v-if="!isProcessing">Đặt hàng</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Đang xử lý...
              </span>
            </button>

            <!-- Security Notice -->
            <div class="mt-6 p-4 bg-gray-50 rounded-lg">
              <div class="flex items-start space-x-3">
                <svg class="w-5 h-5 text-green-500 mt-0.5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
                </svg>
                <div>
                  <h4 class="font-semibold text-gray-900 text-sm">Thanh toán an toàn</h4>
                  <p class="text-xs text-gray-600 mt-1">Thông tin của bạn được mã hóa và bảo mật</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <Footer />

    <!-- Success Modal -->
    <Modal
      :show="showSuccessModal"
      title="Đặt hàng thành công!"
      @close="handleSuccessModalClose"
    >
      <div class="text-center py-4">
        <div class="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
          <svg class="w-10 h-10 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
          </svg>
        </div>
        <h3 class="text-xl font-bold text-gray-900 mb-2">Cảm ơn bạn đã đặt hàng!</h3>
        <p class="text-gray-600 mb-4">Đơn hàng của bạn đã được xác nhận và đang được xử lý.</p>
        <p class="text-sm text-gray-600">Mã đơn hàng: <span class="font-bold text-primary-600">#{{ orderNumber }}</span></p>
      </div>
      <template #footer>
        <router-link to="/products" class="btn-secondary">
          Tiếp tục mua sắm
        </router-link>
        <router-link to="/" class="btn-primary">
          Về trang chủ
        </router-link>
      </template>
    </Modal>
  </div>
</template>

<script setup>
import { ref, computed, reactive, h } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '../../stores/cart'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Modal from '../../components/common/Modal.vue'

const router = useRouter()
const cartStore = useCartStore()

const form = reactive({
  fullName: '',
  phone: '',
  email: '',
  address: '',
  city: '',
  district: '',
  ward: '',
  note: '',
  paymentMethod: 'cod'
})

const isProcessing = ref(false)
const showSuccessModal = ref(false)
const discount = ref(0)
const orderNumber = ref('')

const CashIcon = () => h('svg', { class: 'text-green-600', fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M17 9V7a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2m2 4h10a2 2 0 002-2v-6a2 2 0 00-2-2H9a2 2 0 00-2 2v6a2 2 0 002 2zm7-5a2 2 0 11-4 0 2 2 0 014 0z' })
])

const CreditCardIcon = () => h('svg', { class: 'text-blue-600', fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z' })
])

const BankIcon = () => h('svg', { class: 'text-purple-600', fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M8 14v3m4-3v3m4-3v3M3 21h18M3 10h18M3 7l9-4 9 4M4 10h16v11H4V10z' })
])

const paymentMethods = [
  {
    id: 'cod',
    name: 'Thanh toán khi nhận hàng (COD)',
    description: 'Thanh toán bằng tiền mặt khi nhận hàng',
    icon: CashIcon
  },
  {
    id: 'card',
    name: 'Thẻ tín dụng/Ghi nợ',
    description: 'Visa, Mastercard, JCB',
    icon: CreditCardIcon
  },
  {
    id: 'bank',
    name: 'Chuyển khoản ngân hàng',
    description: 'Chuyển khoản qua Internet Banking',
    icon: BankIcon
  }
]

const shippingFee = computed(() => {
  return cartStore.totalPrice >= 500000 ? 0 : 30000
})

const total = computed(() => {
  return cartStore.totalPrice + shippingFee.value - discount.value
})

const isFormValid = computed(() => {
  return form.fullName && form.phone && form.address && form.city && form.paymentMethod
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

async function handleCheckout() {
  if (!isFormValid.value) return
  
  isProcessing.value = true
  
  // Simulate API call
  await new Promise(resolve => setTimeout(resolve, 2000))
  
  // Generate order number
  orderNumber.value = 'ORD' + Date.now().toString().slice(-8)
  
  // Clear cart
  cartStore.clearCart()
  
  isProcessing.value = false
  showSuccessModal.value = true
}

function handleSuccessModalClose() {
  showSuccessModal.value = false
  router.push('/')
}
</script>

