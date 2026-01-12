<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Đơn hàng</h1>
      <div class="flex flex-wrap gap-3">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Tìm kiếm theo mã đơn hàng..."
          class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent"
        />
      </div>
    </div>

    <!-- Filters -->
    <div class="flex flex-wrap gap-3">
      <button
        v-for="status in statusFilters"
        :key="status.value"
        @click="filterStatus = status.value"
        :class="filterStatus === status.value ? 'bg-primary-600 text-white' : 'bg-white text-gray-700 hover:bg-gray-50'"
        class="px-4 py-2 rounded-lg border border-gray-300 font-medium transition-colors"
      >
        {{ status.label }}
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="flex justify-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Orders Table -->
    <div v-else class="bg-white rounded-xl shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-gray-50 border-b">
            <tr>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Mã đơn</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Khách hàng</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Người nhận</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Tổng tiền</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Trạng thái</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Ngày tạo</th>
              <th class="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y">
            <tr v-for="order in filteredOrders" :key="order.id" class="hover:bg-gray-50">
              <td class="px-6 py-4">
                <div class="font-mono font-semibold text-primary-600">#{{ order.code || order.id }}</div>
              </td>
              <td class="px-6 py-4 text-sm">
                <div>ID: {{ order.customerId || '-' }}</div>
              </td>
              <td class="px-6 py-4 text-sm">
                <div class="font-medium">{{ order.receiverName || '-' }}</div>
                <div class="text-gray-500 text-xs">{{ order.receiverPhone || '-' }}</div>
              </td>
              <td class="px-6 py-4">
                <div class="font-semibold text-gray-900">{{ formatCurrency(order.total) }}</div>
                <div v-if="order.discount" class="text-xs text-gray-500">Giảm: {{ formatCurrency(order.discount) }}</div>
              </td>
              <td class="px-6 py-4">
                <span :class="getStatusClass(order.status)" class="px-3 py-1 rounded-full text-sm font-medium">
                  {{ getStatusLabel(order.status) }}
                </span>
              </td>
              <td class="px-6 py-4 text-sm text-gray-600">
                {{ formatDate(order.createDate) }}
              </td>
              <td class="px-6 py-4">
                <div class="flex justify-end space-x-2">
                  <button @click="viewOrder(order)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg" title="Xem chi tiết">
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                    </svg>
                  </button>
                  <button @click="showStatusModal(order)" class="p-2 text-purple-600 hover:bg-purple-50 rounded-lg" title="Cập nhật trạng thái">
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                    </svg>
                  </button>
                  <button @click="deleteOrder(order)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg" title="Xóa">
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                    </svg>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Order Detail Modal -->
    <Modal :show="showDetailModal" :title="`Chi tiết đơn hàng #${selectedOrder?.code || selectedOrder?.id}`" @close="showDetailModal = false">
      <div v-if="selectedOrder" class="space-y-6">
        <!-- Order Info -->
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="text-sm font-semibold text-gray-600">Mã đơn hàng</label>
            <p class="font-mono font-semibold text-primary-600">#{{ selectedOrder.code || selectedOrder.id }}</p>
          </div>
          <div>
            <label class="text-sm font-semibold text-gray-600">Trạng thái</label>
            <p>
              <span :class="getStatusClass(selectedOrder.status)" class="px-3 py-1 rounded-full text-sm font-medium">
                {{ getStatusLabel(selectedOrder.status) }}
              </span>
            </p>
          </div>
          <div>
            <label class="text-sm font-semibold text-gray-600">Ngày tạo</label>
            <p class="text-gray-900">{{ formatDate(selectedOrder.createDate) }}</p>
          </div>
          <div v-if="selectedOrder.deliveryDate">
            <label class="text-sm font-semibold text-gray-600">Ngày giao</label>
            <p class="text-gray-900">{{ formatDate(selectedOrder.deliveryDate) }}</p>
          </div>
        </div>

        <!-- Receiver Info -->
        <div class="border-t pt-4">
          <h4 class="font-semibold text-gray-900 mb-3">Thông tin người nhận</h4>
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="text-sm font-semibold text-gray-600">Tên người nhận</label>
              <p class="text-gray-900">{{ selectedOrder.receiverName || '-' }}</p>
            </div>
            <div>
              <label class="text-sm font-semibold text-gray-600">Số điện thoại</label>
              <p class="text-gray-900">{{ selectedOrder.receiverPhone || '-' }}</p>
            </div>
            <div class="col-span-2">
              <label class="text-sm font-semibold text-gray-600">Địa chỉ</label>
              <p class="text-gray-900">{{ selectedOrder.receiverAddress || '-' }}</p>
            </div>
          </div>
        </div>

        <!-- Order Items -->
        <div class="border-t pt-4">
          <h4 class="font-semibold text-gray-900 mb-3">Sản phẩm</h4>
          <div class="space-y-2">
            <div
              v-for="item in selectedOrder.items"
              :key="item.id"
              class="flex justify-between items-center p-3 bg-gray-50 rounded-lg"
            >
              <div class="flex-1">
                <p class="font-medium text-gray-900">{{ item.productName }}</p>
                <p class="text-sm text-gray-600">Số lượng: {{ item.quantity }}</p>
              </div>
              <div class="text-right">
                <p class="font-semibold text-gray-900">{{ formatCurrency(item.price) }}</p>
                <p class="text-sm text-gray-600">Tổng: {{ formatCurrency((item.price || 0) * (item.quantity || 0)) }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Order Summary -->
        <div class="border-t pt-4">
          <div class="space-y-2">
            <div class="flex justify-between text-sm">
              <span class="text-gray-600">Tạm tính:</span>
              <span class="text-gray-900">{{ formatCurrency(selectedOrder.subTotal) }}</span>
            </div>
            <div v-if="selectedOrder.discount" class="flex justify-between text-sm">
              <span class="text-gray-600">Giảm giá:</span>
              <span class="text-red-600">-{{ formatCurrency(selectedOrder.discount) }}</span>
            </div>
            <div class="flex justify-between text-lg font-bold border-t pt-2">
              <span class="text-gray-900">Tổng cộng:</span>
              <span class="text-primary-600">{{ formatCurrency(selectedOrder.total) }}</span>
            </div>
          </div>
        </div>

        <div v-if="selectedOrder.description" class="border-t pt-4">
          <label class="text-sm font-semibold text-gray-600">Ghi chú</label>
          <p class="text-gray-900">{{ selectedOrder.description }}</p>
        </div>

        <!-- Order Timeline -->
        <div v-if="selectedOrder.statusHistories && selectedOrder.statusHistories.length > 0" class="border-t pt-4">
          <h4 class="font-semibold text-gray-900 mb-4">Lịch sử đơn hàng</h4>
          <div class="relative pl-4 border-l-2 border-gray-200 space-y-6">
            <div v-for="history in selectedOrder.statusHistories" :key="history.id" class="relative">
              <div class="absolute -left-[21px] top-1 w-4 h-4 rounded-full border-2 border-white" :class="getStatusColorClass(history.toStatus)"></div>
              <div>
                <p class="font-medium text-gray-900">
                  {{ getStatusLabel(history.toStatus) }}
                  <span v-if="history.fromStatus" class="text-gray-500 font-normal text-sm">
                    (từ {{ getStatusLabel(history.fromStatus) }})
                  </span>
                </p>
                <p class="text-xs text-gray-500">{{ formatDate(history.createDate) }} - bởi {{ history.createdBy || 'System' }}</p>
                <p v-if="history.note" class="text-sm text-gray-600 mt-1 italic">"{{ history.note }}"</p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <template #footer>
        <button @click="showDetailModal = false" class="btn-secondary">Đóng</button>
      </template>
    </Modal>

    <!-- Update Status Modal -->
    <Modal :show="showStatusUpdateModal" title="Cập nhật trạng thái đơn hàng" @close="showStatusUpdateModal = false">
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái *</label>
          <select v-model="statusForm.status" class="input-field">
            <option v-for="status in availableStatuses" :key="status.value" :value="status.value">
              {{ status.label }}
            </option>
          </select>
        </div>
      </div>
      <template #footer>
        <button @click="updateStatus" :disabled="saving" class="btn-primary">
          {{ saving ? 'Đang cập nhật...' : 'Cập nhật' }}
        </button>
        <button @click="showStatusUpdateModal = false" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Modal -->
    <Modal :show="showDeleteModal" title="Xác nhận xóa" @close="showDeleteModal = false">
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa đơn hàng <strong>#{{ orderToDelete?.code || orderToDelete?.id }}</strong>?</p>
      <template #footer>
        <button @click="confirmDelete" class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700">Xóa</button>
        <button @click="showDeleteModal = false" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Toast -->
    <Toast :show="toast.show" :message="toast.message" :type="toast.type" @close="toast.show = false" />
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import { orderApi, handleApiError } from '@/services/api'
import Modal from '@/components/common/Modal.vue'
import Toast from '@/components/common/Toast.vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

const loading = ref(false)
const saving = ref(false)
const orders = ref([])
const searchQuery = ref('')
const filterStatus = ref(null)

const showDetailModal = ref(false)
const showStatusUpdateModal = ref(false)
const showDeleteModal = ref(false)
const selectedOrder = ref(null)
const orderToDelete = ref(null)
const availableStatuses = ref([])

const statusForm = reactive({
  status: 'Pending',
  note: ''
})

const toast = reactive({ show: false, message: '', type: 'success' })

const statusFilters = [
  { value: null, label: 'Tất cả' },
  { value: 'Pending', label: 'Chờ xử lý' },
  { value: 'Processing', label: 'Đang xử lý' },
  { value: 'Shipping', label: 'Đang giao hàng' },
  { value: 'Delivered', label: 'Đã giao hàng' },
  { value: 'Cancelled', label: 'Đã hủy' }
]

const filteredOrders = computed(() => {
  let result = orders.value

  // Filter by status
  if (filterStatus.value) {
    result = result.filter(order => order.status === filterStatus.value)
  }

  // Filter by search query
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter(order => 
      (order.code && order.code.toLowerCase().includes(query)) ||
      (order.id && order.id.toString().includes(query))
    )
  }

  // Sort by create date (newest first)
  return result.sort((a, b) => {
    const dateA = a.createDate ? new Date(a.createDate) : new Date(0)
    const dateB = b.createDate ? new Date(b.createDate) : new Date(0)
    return dateB - dateA
  })
})

onMounted(() => {
  loadOrders()
})

async function loadOrders() {
  try {
    loading.value = true
    orders.value = await orderApi.getAll()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    loading.value = false
  }
}

function viewOrder(order) {
  selectedOrder.value = order
  showDetailModal.value = true
}

function showStatusModal(order) {
  selectedOrder.value = order
  // Determine available next statuses
  availableStatuses.value = getAvailableStatuses(order.status)
  
  if (availableStatuses.value.length > 0) {
    statusForm.status = availableStatuses.value[0].value
  } else {
    statusForm.status = order.status
  }
  statusForm.note = ''
  
  showStatusUpdateModal.value = true
}

function getAvailableStatuses(currentStatus) {
  const allStatuses = [
    { value: 'Pending', label: 'Chờ xử lý' },
    { value: 'Processing', label: 'Đang xử lý' },
    { value: 'Shipping', label: 'Đang giao hàng' },
    { value: 'Delivered', label: 'Đã giao hàng' },
    { value: 'Cancelled', label: 'Đã hủy' }
  ]
  
  // Strict flow logic matches Backend
  switch (currentStatus) {
    case 'Pending':
      return allStatuses.filter(s => ['Processing', 'Cancelled'].includes(s.value))
    case 'Processing':
      return allStatuses.filter(s => ['Shipping', 'Cancelled'].includes(s.value))
    case 'Shipping':
      return allStatuses.filter(s => ['Delivered', 'Cancelled'].includes(s.value))
    case 'Delivered':
      return [] // No further updates
    case 'Cancelled':
      return [] // No further updates
    default:
      return allStatuses // Fallback
  }
}

function getStatusColorClass(status) {
  const classes = {
    'Pending': 'bg-yellow-500',
    'Processing': 'bg-blue-500',
    'Shipping': 'bg-purple-500',
    'Delivered': 'bg-green-500',
    'Cancelled': 'bg-red-500'
  }
  return classes[status] || 'bg-gray-500'
}

async function updateStatus() {
  if (!selectedOrder.value) return

  try {
    saving.value = true
    await orderApi.updateStatus(selectedOrder.value.id, statusForm)
    showToast('Đã cập nhật trạng thái đơn hàng thành công', 'success')
    showStatusUpdateModal.value = false
    await loadOrders()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    saving.value = false
  }
}

function deleteOrder(order) {
  orderToDelete.value = order
  showDeleteModal.value = true
}

async function confirmDelete() {
  if (!orderToDelete.value) return

  try {
    await orderApi.delete(orderToDelete.value.id)
    showToast('Đã xóa đơn hàng thành công', 'success')
    showDeleteModal.value = false
    await loadOrders()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  }
}

function formatCurrency(amount) {
  if (!amount) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount)
}

function formatDate(date) {
  if (!date) return '-'
  return new Date(date).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

function getStatusClass(status) {
  const classes = {
    'Pending': 'bg-yellow-100 text-yellow-800',
    'Processing': 'bg-blue-100 text-blue-800',
    'Shipping': 'bg-purple-100 text-purple-800',
    'Delivered': 'bg-green-100 text-green-800',
    'Cancelled': 'bg-red-100 text-red-800'
  }
  return classes[status] || 'bg-gray-100 text-gray-800'
}

function getStatusLabel(status) {
  const labels = {
    'Pending': 'Chờ xử lý',
    'Processing': 'Đang xử lý',
    'Shipping': 'Đang giao hàng',
    'Delivered': 'Đã giao hàng',
    'Cancelled': 'Đã hủy'
  }
  return labels[status] || status || 'Chưa xác định'
}

function showToast(message, type = 'success') {
  toast.message = message
  toast.type = type
  toast.show = true
}
</script>
