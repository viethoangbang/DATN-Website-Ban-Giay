<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">

      <h1 class="text-3xl font-bold text-gray-900 mb-8">Thanh toán</h1>

      <div v-if="loading" class="text-center py-20">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary-600"></div>
        <p class="mt-4 text-gray-600">Đang tải...</p>
      </div>

      <div v-else-if="cartStore.items.length === 0" class="text-center py-20">
        <p class="text-gray-600 mb-4">Giỏ hàng của bạn đang trống</p>
        <router-link to="/products" class="text-primary-600 hover:text-primary-700">
          Tiếp tục mua sắm
        </router-link>
      </div>

      <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Left: Delivery & Payment Info -->
        <div class="lg:col-span-2 space-y-6">
          <!-- Delivery Address -->
          <div class="bg-white rounded-lg shadow p-6">
            <h2 class="text-xl font-bold mb-4">Địa chỉ giao hàng</h2>
            
            <div v-if="addresses.length === 0" class="text-center py-8">
              <p class="text-gray-600 mb-4">Bạn chưa có địa chỉ giao hàng</p>
              <router-link to="/profile" class="text-primary-600 hover:text-primary-700">
                Thêm địa chỉ mới
              </router-link>
            </div>

            <div v-else class="space-y-3">
              <div
                v-for="address in addresses"
                :key="address.id"
                @click="selectAddress(address)"
                :class="[
                  'border-2 rounded-lg p-4 cursor-pointer transition-all',
                  selectedAddress?.id === address.id
                    ? 'border-primary-500 bg-primary-50'
                    : 'border-gray-200 hover:border-primary-300'
                ]"
              >
                <div class="flex items-start justify-between">
                  <div class="flex-1">
                    <p class="font-semibold">{{ address.receiverName }}</p>
                    <p class="text-gray-600">{{ address.receiverPhone }}</p>
                    <p class="text-gray-600 text-sm mt-1">
                      {{ address.wardName }}, {{ address.districtName }}, {{ address.provinceName }}
                    </p>
                  </div>
                  <div v-if="selectedAddress?.id === address.id" class="text-primary-600">
                    <svg class="w-6 h-6" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                    </svg>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Voucher -->
          <div class="bg-white rounded-lg shadow p-6">
            <h2 class="text-xl font-bold mb-4">Mã giảm giá</h2>
            <div class="flex gap-2">
              <input
                v-model="voucherCode"
                type="text"
                placeholder="Nhập mã giảm giá"
                class="flex-1 border border-gray-300 rounded-lg px-4 py-2"
              />
              <button
                @click="applyVoucher"
                class="px-6 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
              >
                Áp dụng
              </button>
            </div>
            <p v-if="voucherError" class="text-red-500 text-sm mt-2">{{ voucherError }}</p>
            <div v-if="voucherApplied && voucherDiscount > 0" class="mt-2 p-3 bg-green-50 rounded-lg">
              <p class="text-green-700 font-semibold">
                ✓ Đã áp dụng: {{ voucherName || voucherCode }}
              </p>
              <p class="text-green-600 text-sm mt-1">
                Giảm {{ formatPrice(voucherDiscount) }}
              </p>
            </div>
          </div>

          <!-- Payment Method -->
          <div class="bg-white rounded-lg shadow p-6">
            <h2 class="text-xl font-bold mb-4">Phương thức thanh toán</h2>
            <div class="space-y-3">
              <div
                @click="paymentMethod = 'COD'"
                :class="[
                  'border-2 rounded-lg p-4 cursor-pointer transition-all',
                  paymentMethod === 'COD'
                    ? 'border-primary-500 bg-primary-50'
                    : 'border-gray-200 hover:border-primary-300'
                ]"
              >
                <div class="flex items-center justify-between">
                  <div class="flex items-center gap-3">
                    <div class="w-12 h-12 bg-gray-100 rounded-lg flex items-center justify-center">
                      💵
                    </div>
                    <div>
                      <p class="font-semibold">Thanh toán khi nhận hàng (COD)</p>
                      <p class="text-sm text-gray-600">Thanh toán bằng tiền mặt</p>
                    </div>
                  </div>
                  <div v-if="paymentMethod === 'COD'" class="text-primary-600">
                    <svg class="w-6 h-6" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                    </svg>
                  </div>
                </div>
              </div>

              <div
                @click="paymentMethod = 'VNPay'"
                :class="[
                  'border-2 rounded-lg p-4 cursor-pointer transition-all',
                  paymentMethod === 'VNPay'
                    ? 'border-primary-500 bg-primary-50'
                    : 'border-gray-200 hover:border-primary-300'
                ]"
              >
                <div class="flex items-center justify-between">
                  <div class="flex items-center gap-3">
                    <div class="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                      <img src="https://vnpay.vn/s1/statics.vnpay.vn/2023/9/06ncktiwd6dc1694418196384.png" alt="VNPay" class="w-8 h-8 object-contain" />
                    </div>
                    <div>
                      <p class="font-semibold">Thanh toán VNPay</p>
                      <p class="text-sm text-gray-600">Thanh toán qua VNPay QR</p>
                    </div>
                  </div>
                  <div v-if="paymentMethod === 'VNPay'" class="text-primary-600">
                    <svg class="w-6 h-6" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                    </svg>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Note -->
          <div class="bg-white rounded-lg shadow p-6">
            <h2 class="text-xl font-bold mb-4">Ghi chú đơn hàng</h2>
            <textarea
              v-model="note"
              rows="3"
              placeholder="Ghi chú cho người bán..."
              class="w-full border border-gray-300 rounded-lg px-4 py-2"
            ></textarea>
          </div>
        </div>

        <!-- Right: Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-lg shadow p-6 sticky top-4">
            <h2 class="text-xl font-bold mb-4">Đơn hàng</h2>

            <!-- Cart Items -->
            <div class="space-y-3 mb-4 max-h-64 overflow-y-auto">
              <div v-for="item in cartStore.items" :key="item.productDetailId" class="flex gap-3">
                <img :src="item.image" alt="" class="w-16 h-16 object-cover rounded" />
                <div class="flex-1 min-w-0">
                  <p class="text-xs font-semibold text-blue-600 mb-1">ID: {{ item.productDetailId }}</p>
                  <p class="text-sm font-medium truncate">{{ item.name }}</p>
                  <p class="text-xs text-gray-500">{{ getCategoryName(item) }} - {{ getBrandName(item) }}</p>
                  <p class="text-xs text-gray-600">{{ item.size }} / {{ item.color }}</p>
                  <p class="text-sm">{{ formatPrice(item.price) }} x {{ item.quantity }}</p>
                </div>
              </div>
            </div>

            <div class="border-t pt-4 space-y-2">
              <div class="flex justify-between text-gray-600">
                <span>Tạm tính ({{ cartStore.totalItems }} sản phẩm)</span>
                <span class="font-semibold">{{ formatPrice(subtotal) }}</span>
              </div>
              
              <div class="flex justify-between text-gray-600">
                <span>Giảm giá sản phẩm</span>
                <span class="font-semibold text-dark-600">{{ formatPrice(productDiscount) }}</span>
              </div>
              
              <div class="flex justify-between text-gray-600">
                <span>Giảm giá voucher</span>
                <span class="font-semibold text-dark-600">{{ formatPrice(voucherDiscount) }}</span>
              </div>
              
              <div v-if="shippingFee > 0" class="flex justify-between text-gray-600">
                <span>Phí vận chuyển</span>
                <span>{{ formatPrice(shippingFee) }}</span>
              </div>

              <div class="border-t pt-2 flex justify-between text-xl font-bold">
                <span>Tổng cộng</span>
                <span class="text-primary-600">{{ formatPrice(total) }}</span>
              </div>
            </div>

            <button
              @click="placeOrder"
              :disabled="!selectedAddress || placing"
              class="w-full mt-6 bg-primary-600 text-white py-3 rounded-lg font-semibold hover:bg-primary-700 disabled:bg-gray-300 disabled:cursor-not-allowed"
            >
              {{ placing ? 'Đang xử lý...' : 'Đặt hàng' }}
            </button>

            <p v-if="!selectedAddress" class="text-red-500 text-sm text-center mt-2">
              Vui lòng chọn địa chỉ giao hàng
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- Toast -->
    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />

    <!-- Confetti Canvas -->
    <canvas ref="confettiCanvas" class="fixed top-0 left-0 w-full h-full pointer-events-none z-50" style="display: none;"></canvas>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '@/stores/cart'
import { useAuthStore } from '@/stores/auth'
import Navbar from '@/components/user/Navbar.vue'
import Toast from '@/components/common/Toast.vue'
import { addressApi, productApi, categoryApi } from '@/services/api'
import checkoutApi from '@/services/checkoutApi'

const router = useRouter()
const cartStore = useCartStore()
const authStore = useAuthStore()

const loading = ref(true)
const placing = ref(false)
const addresses = ref([])
const products = ref([])
const categories = ref([])
const selectedAddress = ref(null)
const voucherCode = ref('')
const voucherApplied = ref(false)
const voucherError = ref('')
const voucherDiscount = ref(0)
const voucherName = ref('')
const paymentMethod = ref('COD')
const note = ref('')
const shippingFee = ref(0)
const showComplete = ref(false)
const confettiCanvas = ref(null)

const toast = ref({
  show: false,
  message: '',
  type: 'success'
})

// Calculate subtotal (original price before discounts)
const subtotal = computed(() => {
  return cartStore.items.reduce((total, item) => {
    const originalPrice = Number(item.originalPrice) || Number(item.price) || 0
    const quantity = Number(item.quantity) || 0
    return total + (originalPrice * quantity)
  }, 0)
})

// Calculate product discount (from discount table)
const productDiscount = computed(() => {
  return cartStore.items.reduce((total, item) => {
    const originalPrice = Number(item.originalPrice) || Number(item.price) || 0
    const finalPrice = Number(item.price) || 0
    const quantity = Number(item.quantity) || 0
    const itemDiscount = (originalPrice - finalPrice) * quantity
    return total + itemDiscount
  }, 0)
})

const total = computed(() => {
  return subtotal.value - productDiscount.value - voucherDiscount.value + shippingFee.value
})

onMounted(async () => {
  if (!authStore.isAuthenticated) {
    router.push('/login')
    return
  }

  try {
    const [addressesData, productsData, categoriesData] = await Promise.all([
      addressApi.getByAccountId(),
      productApi.getAll(),
      categoryApi.getAll()
    ])
    addresses.value = addressesData.filter(a => a.status !== 'Deleted')
    products.value = productsData
    categories.value = categoriesData
    
    // Auto select first address
    if (addresses.value.length > 0) {
      await selectAddress(addresses.value[0])
    }
  } catch (error) {
    console.error('Error loading addresses:', error)
    toast.value = {
      show: true,
      message: 'Không thể tải địa chỉ giao hàng',
      type: 'error'
    }
  } finally {
    loading.value = false
  }
})

async function selectAddress(address) {
  selectedAddress.value = address
  
  // Check if address has GHN location IDs
  if (!address.districtId || !address.wardCode) {
    console.warn('Address missing GHN location IDs. Using default shipping fee.')
    toast.value = {
      show: true,
      message: 'Địa chỉ này chưa có thông tin vận chuyển đầy đủ. Vui lòng cập nhật địa chỉ trong phần Profile.',
      type: 'warning'
    }
    shippingFee.value = 30000
    return
  }
  
  // Calculate shipping fee
  try {
    const result = await checkoutApi.calculateShippingFee(address.id)
    console.log('Shipping fee response:', result)
    // Backend returns Fee (capital F) but JSON serializer converts to camelCase
    const fee = result.fee || result.Fee || 0
    
    // Check if result has message indicating default fee was used
    if (result.message && result.message.includes('default shipping fee')) {
      // GHN API error or shop info issue - use default but don't show error toast
      shippingFee.value = 30000
      console.warn('GHN API returned default fee:', result.message)
    } else if (fee > 0) {
      shippingFee.value = fee
    } else {
      console.warn('Shipping fee is 0 or invalid, using default 30000')
      shippingFee.value = 30000
    }
  } catch (error) {
    console.error('Error calculating shipping fee:', error)
    console.error('Error details:', error.response?.data)
    shippingFee.value = 30000 // Default
    // Don't show error toast for shipping fee calculation errors - it's expected in sandbox
  }
}

async function applyVoucher() {
  if (!voucherCode.value.trim()) {
    voucherError.value = 'Vui lòng nhập mã giảm giá'
    voucherApplied.value = false
    voucherDiscount.value = 0
    voucherName.value = ''
    return
  }

  try {
    const result = await checkoutApi.calculateVoucherDiscount(voucherCode.value, subtotal.value)
    
    if (result.isValid) {
      voucherApplied.value = true
      voucherError.value = ''
      voucherDiscount.value = result.discount || 0
      voucherName.value = result.voucherName || ''
    } else {
      voucherApplied.value = false
      voucherError.value = result.message || 'Mã giảm giá không hợp lệ'
      voucherDiscount.value = 0
      voucherName.value = ''
    }
  } catch (error) {
    console.error('Error applying voucher:', error)
    voucherApplied.value = false
    voucherError.value = error.response?.data?.message || 'Không thể áp dụng mã giảm giá'
    voucherDiscount.value = 0
    voucherName.value = ''
  }
}

// Recalculate voucher discount when subtotal changes
watch(subtotal, async () => {
  if (voucherApplied.value && voucherCode.value.trim()) {
    try {
      const result = await checkoutApi.calculateVoucherDiscount(voucherCode.value, subtotal.value)
      if (result.isValid) {
        voucherDiscount.value = result.discount || 0
      } else {
        // Voucher became invalid (e.g., expired), reset
        voucherApplied.value = false
        voucherDiscount.value = 0
        voucherName.value = ''
        voucherError.value = result.message || 'Mã giảm giá không còn hợp lệ'
      }
    } catch (error) {
      console.error('Error recalculating voucher discount:', error)
    }
  }
})

async function placeOrder() {
  if (!selectedAddress.value) {
    toast.value = {
      show: true,
      message: 'Vui lòng chọn địa chỉ giao hàng',
      type: 'error'
    }
    return
  }

  placing.value = true

  try {
    const orderData = {
      addressDeliveryId: selectedAddress.value.id,
      paymentMethod: paymentMethod.value,
      voucherCode: voucherApplied.value ? voucherCode.value : null,
      shippingFee: shippingFee.value,
      note: note.value
    }

    const result = await checkoutApi.createOrder(orderData)

    if (result.requiresPayment && result.paymentUrl) {
      // Redirect to VNPay
      window.location.href = result.paymentUrl
    } else {
      // Order placed successfully (COD)
      toast.value = {
        show: true,
        message: 'Đặt hàng thành công!',
        type: 'success'
      }

      // Animate step to complete
      setTimeout(() => {
        showComplete.value = true
        triggerConfetti()
      }, 300)

      // Redirect after animation completes
      setTimeout(() => {
        router.push(`/orders/${result.order.id}`)
      }, 3500)
    }
  } catch (error) {
    console.error('Error placing order:', error)
    toast.value = {
      show: true,
      message: error.response?.data?.message || 'Không thể đặt hàng. Vui lòng thử lại.',
      type: 'error'
    }
  } finally {
    placing.value = false
  }
}

function formatPrice(price) {
  if (!price && price !== 0) return '0đ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

// Confetti effect
function triggerConfetti() {
  if (!confettiCanvas.value) return
  
  const canvas = confettiCanvas.value
  const ctx = canvas.getContext('2d')
  canvas.style.display = 'block'
  canvas.width = window.innerWidth
  canvas.height = window.innerHeight

  const particles = []
  const particleCount = 150
  const colors = ['#ea580c', '#f97316', '#fb923c', '#fdba74', '#fef3c7', '#34d399', '#10b981', '#3b82f6', '#8b5cf6', '#ec4899']

  // Get position of "Complete" step element
  const completeStepElement = document.querySelector('.step-complete')
  let centerX = window.innerWidth / 2
  let startY = 150
  
  if (completeStepElement) {
    const rect = completeStepElement.getBoundingClientRect()
    centerX = rect.left + rect.width / 2
    startY = rect.top + rect.height / 2
  }

  // Create particles
  for (let i = 0; i < particleCount; i++) {
    particles.push({
      x: centerX + (Math.random() - 0.5) * 100,
      y: startY,
      vx: (Math.random() - 0.5) * 6,
      vy: Math.random() * 3 + 2,
      size: Math.random() * 8 + 4,
      color: colors[Math.floor(Math.random() * colors.length)],
      rotation: Math.random() * 360,
      rotationSpeed: (Math.random() - 0.5) * 10,
      gravity: 0.2,
      life: 1
    })
  }

  let animationId
  function animate() {
    ctx.clearRect(0, 0, canvas.width, canvas.height)
    
    particles.forEach((particle, index) => {
      // Update position
      particle.x += particle.vx
      particle.y += particle.vy
      particle.vy += particle.gravity
      particle.rotation += particle.rotationSpeed
      particle.life -= 0.01

      // Draw particle
      ctx.save()
      ctx.globalAlpha = particle.life
      ctx.translate(particle.x, particle.y)
      ctx.rotate((particle.rotation * Math.PI) / 180)
      ctx.fillStyle = particle.color
      ctx.fillRect(-particle.size / 2, -particle.size / 2, particle.size, particle.size)
      ctx.restore()

      // Remove dead particles
      if (particle.life <= 0 || particle.y > canvas.height) {
        particles.splice(index, 1)
      }
    })

    if (particles.length > 0) {
      animationId = requestAnimationFrame(animate)
    } else {
      canvas.style.display = 'none'
    }
  }

  animate()

  // Clean up after 3 seconds
  setTimeout(() => {
    if (animationId) {
      cancelAnimationFrame(animationId)
    }
    canvas.style.display = 'none'
  }, 3000)
}

// Get Category Name
function getCategoryName(item) {
  const product = products.value.find(p => p.id === item.id)
  if (!product) return 'Unknown'
  const category = categories.value.find(c => c.id === product.categoryId)
  return category ? category.name : 'Unknown'
}

// Get Brand Name
function getBrandName(item) {
  const product = products.value.find(p => p.id === item.id)
  return product?.brand || 'Unknown'
}
</script>
