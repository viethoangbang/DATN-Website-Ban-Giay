<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 class="text-3xl font-bold text-gray-900 mb-8">Đơn hàng của tôi</h1>

      <div v-if="loading" class="text-center py-20">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary-600"></div>
        <p class="mt-4 text-gray-600">Đang tải đơn hàng...</p>
      </div>

      <div v-else-if="orders.length === 0" class="text-center py-20">
        <div class="text-6xl mb-4">📦</div>
        <p class="text-xl text-gray-600 mb-4">Bạn chưa có đơn hàng nào</p>
        <router-link
          to="/products"
          class="inline-block px-6 py-3 bg-primary-600 text-white rounded-lg hover:bg-primary-700"
        >
          Tiếp tục mua sắm
        </router-link>
      </div>

      <div v-else class="space-y-4">
        <div
          v-for="order in orders"
          :key="order.id"
          class="bg-white rounded-lg shadow hover:shadow-lg transition-shadow"
        >
          <div class="p-6">
            <!-- Order Header -->
            <div class="flex items-center justify-between mb-4">
              <div>
                <h3 class="text-lg font-semibold">{{ order.code }}</h3>
                <p class="text-sm text-gray-600">
                  {{ formatDate(order.createDate) }}
                </p>
              </div>
              <div class="text-right">
                <span
                  :class="[
                    'px-3 py-1 rounded-full text-sm font-semibold',
                    getStatusColor(order.status)
                  ]"
                >
                  {{ getStatusText(order.status) }}
                </span>
              </div>
            </div>

            <!-- Order Items Preview -->
            <div class="space-y-2 mb-4">
              <div
                v-for="item in order.items.slice(0, 2)"
                :key="item.id"
                class="flex items-center gap-3"
              >
                <img
                  :src="getItemImage(item)"
                  :alt="item.productName"
                  class="w-16 h-16 object-cover rounded"
                  @error="handleImageError"
                />
                <div class="flex-1 min-w-0">
                  <p class="text-xs font-semibold text-blue-600 mb-1">ID: {{ item.productDetailId }}</p>
                  <p class="font-medium truncate">{{ item.productName }}</p>
                  <p class="text-sm text-gray-600">Số lượng: {{ item.quantity }}</p>
                  <p v-if="item.size || item.color" class="text-sm text-gray-600">{{ item.size }} / {{ item.color }}</p>
                </div>
                <div class="text-right">
                  <p class="font-semibold">{{ formatPrice(item.finalPrice || item.price) }}</p>
                </div>
              </div>
              <p v-if="order.items.length > 2" class="text-sm text-gray-600">
                +{{ order.items.length - 2 }} sản phẩm khác
              </p>
            </div>

            <!-- Order Footer -->
            <div class="border-t pt-4 flex items-center justify-between">
              <div>
                <p class="text-sm text-gray-600">Tổng tiền</p>
                <p class="text-xl font-bold text-primary-600">{{ formatPrice(order.total) }}</p>
              </div>
              <div class="flex gap-2">
                <router-link
                  :to="`/orders/${order.id}`"
                  class="px-4 py-2 border border-primary-600 text-primary-600 rounded-lg hover:bg-primary-50"
                >
                  Xem chi tiết
                </router-link>
                <button
                  v-if="order.status === 'Pending' || order.status === 'Confirmed'"
                  @click="showCancelConfirm(order.id)"
                  class="px-4 py-2 border border-red-600 text-red-600 rounded-lg hover:bg-red-50"
                >
                  Hủy đơn
                </button>
              </div>
            </div>
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
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import Navbar from '@/components/user/Navbar.vue'
import Toast from '@/components/common/Toast.vue'
import Modal from '@/components/common/Modal.vue'
import orderApi from '@/services/orderApi'

const router = useRouter()
const authStore = useAuthStore()

const loading = ref(true)
const orders = ref([])
const showCancelModal = ref(false)
const orderToCancel = ref(null)
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
    const data = await orderApi.getMyOrders()
    orders.value = data.sort((a, b) => new Date(b.createDate) - new Date(a.createDate))
    // Debug: log first order item to check imageUrl
    if (orders.value.length > 0 && orders.value[0].items?.length > 0) {
      console.log('First order item:', orders.value[0].items[0])
    }
  } catch (error) {
    console.error('Error loading orders:', error)
    toast.value = {
      show: true,
      message: 'Không thể tải danh sách đơn hàng',
      type: 'error'
    }
  } finally {
    loading.value = false
  }
})

function showCancelConfirm(orderId) {
  orderToCancel.value = orderId
  showCancelModal.value = true
}

async function confirmCancelOrder() {
  if (!orderToCancel.value) return

  cancelling.value = true
  try {
    await orderApi.cancelOrder(orderToCancel.value)
    
    // Update local state
    const order = orders.value.find(o => o.id === orderToCancel.value)
    if (order) {
      order.status = 'Cancelled'
    }

    toast.value = {
      show: true,
      message: 'Đã hủy đơn hàng thành công',
      type: 'success'
    }

    showCancelModal.value = false
    orderToCancel.value = null
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

function getItemImage(item) {
  // Backend returns ImageUrl (PascalCase), but frontend might receive imageUrl (camelCase)
  // Try both cases
  if (item.imageUrl) return item.imageUrl
  if (item.ImageUrl) return item.ImageUrl
  
  if (item.image) {
    // If image is string, use it; if object, get url
    return typeof item.image === 'string' ? item.image : (item.image?.url || '')
  }
  if (item.Image) {
    return typeof item.Image === 'string' ? item.Image : (item.Image?.url || '')
  }
  
  if (item.images && Array.isArray(item.images) && item.images.length > 0) {
    const firstImg = item.images[0]
    return typeof firstImg === 'string' ? firstImg : (firstImg?.url || '')
  }
  if (item.Images && Array.isArray(item.Images) && item.Images.length > 0) {
    const firstImg = item.Images[0]
    return typeof firstImg === 'string' ? firstImg : (firstImg?.url || '')
  }
  
  // Debug: log if no image found
  console.warn('No image found for item:', item)
  // Return empty string to trigger error handler
  return ''
}

function handleImageError(event) {
  // Use SVG data URI as fallback instead of external file
  event.target.src = 'data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjQiIGhlaWdodD0iNjQiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHJlY3Qgd2lkdGg9IjY0IiBoZWlnaHQ9IjY0IiBmaWxsPSIjZjNmNGY2Ii8+PHRleHQgeD0iNTAlIiB5PSI1MCUiIGZvbnQtZmFtaWx5PSJBcmlhbCIgZm9udC1zaXplPSIxMCIgZmlsbD0iIzljYTNhZiIgdGV4dC1hbmNob3I9Im1pZGRsZSIgZHk9Ii4zZW0iPk5vIEltYWdlPC90ZXh0Pjwvc3ZnPg=='
}
</script>

