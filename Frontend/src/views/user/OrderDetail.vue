<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div v-if="loading" class="text-center py-20">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary-600"></div>
        <p class="mt-4 text-gray-600">Đang tải thông tin đơn hàng...</p>
      </div>

      <div v-else-if="!order" class="text-center py-20">
        <p class="text-gray-600">Không tìm thấy đơn hàng</p>
        <router-link to="/orders" class="text-primary-600 hover:text-primary-700 mt-4 inline-block">
          Quay lại danh sách đơn hàng
        </router-link>
      </div>

      <div v-else class="space-y-6">
        <!-- Header -->
        <div class="flex items-center justify-between">
          <div>
            <h1 class="text-3xl font-bold text-gray-900">{{ order.code }}</h1>
            <p class="text-gray-600 mt-1">{{ formatDate(order.createDate) }}</p>
          </div>
          <span
            :class="[
              'px-4 py-2 rounded-full text-sm font-semibold',
              getStatusColor(order.status)
            ]"
          >
            {{ getStatusText(order.status) }}
          </span>
        </div>

        <!-- Order Items -->
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-xl font-bold mb-4">Sản phẩm</h2>
          <div class="space-y-4">
            <div
              v-for="item in order.items"
              :key="item.id"
              class="flex items-center gap-4 pb-4 border-b last:border-b-0"
            >
              <img
                :src="item.imageUrl || '/placeholder-shoe.jpg'"
                :alt="item.productName"
                class="w-20 h-20 object-cover rounded"
                @error="handleImageError"
              />
              <div class="flex-1">
                <p class="text-sm font-semibold text-blue-600 mb-1">ID biến thể: {{ item.productDetailId }}</p>
                <p class="font-semibold">{{ item.productName }}</p>
                <p class="text-sm text-gray-600">Số lượng: {{ item.quantity }}</p>
                <p v-if="item.size || item.color" class="text-sm text-gray-600">{{ item.size }} / {{ item.color }}</p>
                <div v-if="item.originalPrice && item.originalPrice > (item.finalPrice || item.price)" class="flex items-center gap-2">
                  <span class="text-sm text-gray-400 line-through">{{ formatPrice(item.originalPrice) }}</span>
                  <span class="text-sm font-semibold text-red-600">{{ formatPrice(item.finalPrice || item.price) }}</span>
                </div>
                <p v-else class="text-sm font-semibold">{{ formatPrice(item.finalPrice || item.price) }}</p>
              </div>
              <div class="text-right">
                <p class="font-bold text-lg">{{ formatPrice((item.finalPrice || item.price) * item.quantity) }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Delivery Info -->
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-xl font-bold mb-4">Thông tin giao hàng</h2>
          <div class="space-y-2">
            <div class="flex">
              <span class="w-32 text-gray-600">Người nhận:</span>
              <span class="font-semibold">{{ order.receiverName }}</span>
            </div>
            <div class="flex">
              <span class="w-32 text-gray-600">Số điện thoại:</span>
              <span class="font-semibold">{{ order.receiverPhone }}</span>
            </div>
            <div class="flex">
              <span class="w-32 text-gray-600">Địa chỉ:</span>
              <span class="font-semibold">{{ order.receiverAddress }}</span>
            </div>
            <div v-if="order.note" class="flex">
              <span class="w-32 text-gray-600">Ghi chú:</span>
              <span class="italic text-gray-700">{{ order.note }}</span>
            </div>
          </div>
        </div>

        <!-- Payment Info -->
        <div class="bg-white rounded-lg shadow p-6">
          <h2 class="text-xl font-bold mb-4">Thanh toán</h2>
          <div class="space-y-2">
            <div class="flex justify-between">
              <span class="text-gray-600">Tạm tính</span>
              <span>{{ formatPrice(order.productTotal) }}</span>
            </div>
            <div v-if="order.productDiscount && order.productDiscount > 0" class="flex justify-between text-green-600">
              <span>Giảm giá sản phẩm</span>
              <span>-{{ formatPrice(order.productDiscount) }}</span>
            </div>
            <div v-if="order.voucherDiscount && order.voucherDiscount > 0" class="flex justify-between text-green-600">
              <span>Mã giảm giá ({{ order.voucherCode }})</span>
              <span>-{{ formatPrice(order.voucherDiscount) }}</span>
            </div>
            <div v-if="order.shippingFee" class="flex justify-between">
              <span class="text-gray-600">Phí vận chuyển</span>
              <span>{{ formatPrice(order.shippingFee) }}</span>
            </div>
            <div class="border-t pt-2 flex justify-between text-xl font-bold">
              <span>Tổng cộng</span>
              <span class="text-primary-600">{{ formatPrice(order.total) }}</span>
            </div>
            <div class="mt-4 pt-4 border-t">
              <div class="flex items-center gap-2">
                <span class="text-gray-600">Phương thức thanh toán:</span>
                <span class="font-semibold">{{ getPaymentMethodText(order.paymentMethod) }}</span>
                <span
                  :class="[
                    'px-2 py-1 rounded text-xs font-semibold',
                    order.paymentStatus === 'Paid' ? 'bg-green-100 text-green-800' : 'bg-yellow-100 text-yellow-800'
                  ]"
                >
                  {{ getPaymentStatusText(order.paymentStatus) }}
                </span>
              </div>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="flex gap-3">
          <router-link
            to="/orders"
            class="px-6 py-3 border border-gray-300 rounded-lg hover:bg-gray-50"
          >
            Quay lại
          </router-link>
          <button
            v-if="order.status === 'Pending' || order.status === 'Confirmed'"
            @click="showCancelConfirm"
            class="px-6 py-3 bg-red-600 text-white rounded-lg hover:bg-red-700"
          >
            Hủy đơn hàng
          </button>
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

    <!-- Cancel Order Confirmation Modal -->
    <Modal
      :show="showCancelModal"
      title="Xác nhận hủy đơn hàng"
      @close="showCancelModal = false"
    >
      <template #footer>
        <button
          @click="showCancelModal = false"
          class="px-4 py-2 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50"
        >
          Quay lại
        </button>
        <button
          @click="confirmCancelOrder"
          :disabled="cancelling"
          class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {{ cancelling ? 'Đang hủy...' : 'Xác nhận hủy' }}
        </button>
      </template>
      
      <div class="py-4">
        <p class="text-gray-700 mb-4">
          Bạn có chắc chắn muốn hủy đơn hàng này không?
        </p>
        <p class="text-sm text-gray-500">
          Sau khi hủy, đơn hàng sẽ không thể khôi phục.
        </p>
      </div>
    </Modal>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import Navbar from '@/components/user/Navbar.vue'
import Toast from '@/components/common/Toast.vue'
import Modal from '@/components/common/Modal.vue'
import orderApi from '@/services/orderApi'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const loading = ref(true)
const order = ref(null)
const showCancelModal = ref(false)
const cancelling = ref(false)

const toast = ref({
  show: false,
  message: '',
  type: 'success'
})

onMounted(async () => {
  if (!authStore.isAuthenticated) {
    router.push('/login')
    return
  }

  try {
    const orderId = parseInt(route.params.id)
    order.value = await orderApi.getOrderById(orderId)
  } catch (error) {
    console.error('Error loading order:', error)
    toast.value = {
      show: true,
      message: 'Không thể tải thông tin đơn hàng',
      type: 'error'
    }
  } finally {
    loading.value = false
  }
})

function showCancelConfirm() {
  showCancelModal.value = true
}

async function confirmCancelOrder() {
  if (!order.value) return

  cancelling.value = true
  try {
    await orderApi.cancelOrder(order.value.id)
    order.value.status = 'Cancelled'
    showCancelModal.value = false

    toast.value = {
      show: true,
      message: 'Đã hủy đơn hàng thành công',
      type: 'success'
    }
  } catch (error) {
    console.error('Error cancelling order:', error)
    toast.value = {
      show: true,
      message: error.response?.data?.message || 'Không thể hủy đơn hàng',
      type: 'error'
    }
  } finally {
    cancelling.value = false
  }
}

function getStatusColor(status) {
  const colors = {
    'Pending': 'bg-yellow-100 text-yellow-800',
    'Confirmed': 'bg-blue-100 text-blue-800',
    'Processing': 'bg-purple-100 text-purple-800',
    'Shipping': 'bg-indigo-100 text-indigo-800',
    'Delivered': 'bg-green-100 text-green-800',
    'Cancelled': 'bg-red-100 text-red-800'
  }
  return colors[status] || 'bg-gray-100 text-gray-800'
}

function getStatusText(status) {
  const texts = {
    'Pending': 'Chờ xác nhận',
    'Confirmed': 'Đã xác nhận',
    'Processing': 'Đang chuẩn bị',
    'Shipping': 'Đang giao',
    'Delivered': 'Đã giao',
    'Cancelled': 'Đã hủy'
  }
  return texts[status] || status
}

function getPaymentMethodText(method) {
  const methods = {
    'COD': 'Thanh toán khi nhận hàng',
    'VNPay': 'VNPay'
  }
  return methods[method] || method
}

function getPaymentStatusText(status) {
  const statuses = {
    'Pending': 'Chưa thanh toán',
    'Paid': 'Đã thanh toán',
    'Failed': 'Thanh toán thất bại',
    'Refunded': 'Đã hoàn tiền'
  }
  return statuses[status] || status
}

function formatPrice(price) {
  if (!price && price !== 0) return '0đ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function formatDate(dateString) {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

function handleImageError(event) {
  event.target.src = '/placeholder-shoe.jpg'
}
</script>

