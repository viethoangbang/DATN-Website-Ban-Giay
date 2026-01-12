<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Giảm giá</h1>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm giảm giá</span>
      </button>
    </div>

    <!-- Filters -->
    <div class="bg-white rounded-xl shadow-md p-4">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Sản phẩm</label>
          <select v-model="filterProductId" class="input-field">
            <option :value="null">Tất cả sản phẩm</option>
            <option v-for="product in products" :key="product.id" :value="product.id">
              {{ product.name }}
            </option>
          </select>
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái</label>
          <select v-model="filterStatus" class="input-field">
            <option value="">Tất cả</option>
            <option value="Active">Hoạt động</option>
            <option value="Inactive">Ngừng</option>
          </select>
        </div>
        <div class="flex items-end">
          <button @click="resetFilters" class="btn-secondary w-full">Xóa bộ lọc</button>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="flex justify-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Table -->
    <div v-else class="bg-white rounded-xl shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-gray-50 border-b">
            <tr>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">ID</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Sản phẩm</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Loại</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Giá trị</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Ngày bắt đầu</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Ngày kết thúc</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Trạng thái</th>
              <th class="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y">
            <tr v-for="discount in filteredDiscounts" :key="discount.id" class="hover:bg-gray-50">
              <td class="px-6 py-4 text-sm">#{{ discount.id }}</td>
              <td class="px-6 py-4">
                <div class="font-semibold">{{ discount.productName || `Product #${discount.productId}` }}</div>
                <div class="text-xs text-gray-500">ID: {{ discount.productId }}</div>
              </td>
              <td class="px-6 py-4 text-sm">
                <span :class="discount.discountType === 'Percentage' ? 'bg-blue-100 text-blue-800' : 'bg-purple-100 text-purple-800'" class="px-2 py-1 rounded-full text-xs font-medium">
                  {{ discount.discountType === 'Percentage' ? 'Phần trăm' : 'Cố định' }}
                </span>
              </td>
              <td class="px-6 py-4 text-sm">
                <div class="font-semibold">
                  {{ discount.discountType === 'Percentage' ? `${discount.discountValue}%` : formatCurrency(discount.discountValue) }}
                </div>
              </td>
              <td class="px-6 py-4 text-sm text-gray-600">{{ formatDate(discount.startDate) }}</td>
              <td class="px-6 py-4 text-sm text-gray-600">{{ formatDate(discount.endDate) }}</td>
              <td class="px-6 py-4">
                <span :class="getStatusClass(discount)" class="px-3 py-1 rounded-full text-sm font-medium">
                  {{ getStatusLabel(discount) }}
                </span>
              </td>
              <td class="px-6 py-4">
                <div class="flex justify-end space-x-2">
                  <button @click="editItem(discount)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg">
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                    </svg>
                  </button>
                  <button @click="deleteItem(discount)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg">
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

    <!-- Empty State -->
    <div v-if="!loading && filteredDiscounts.length === 0" class="bg-white rounded-xl shadow-md p-12 text-center">
      <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
      <h3 class="mt-4 text-lg font-semibold text-gray-900">Chưa có giảm giá nào</h3>
      <p class="mt-2 text-sm text-gray-500">Bắt đầu bằng cách thêm giảm giá mới cho sản phẩm.</p>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingItem ? 'Chỉnh sửa giảm giá' : 'Thêm giảm giá'" @close="closeModal">
      <form @submit.prevent="saveItem" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Sản phẩm *</label>
          <select v-model.number="formData.productId" required class="input-field">
            <option :value="null">Chọn sản phẩm</option>
            <option v-for="product in products" :key="product.id" :value="product.id">
              {{ product.name }}
            </option>
          </select>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Loại *</label>
            <select v-model="formData.discountType" required class="input-field">
              <option value="Percentage">Phần trăm (%)</option>
              <option value="Fixed">Số tiền cố định (VNĐ)</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Giá trị *</label>
            <input 
              v-model.number="formData.discountValue" 
              type="number" 
              step="0.01" 
              min="0" 
              :max="formData.discountType === 'Percentage' ? 100 : undefined"
              required 
              class="input-field" 
            />
            <p class="text-xs text-gray-500 mt-1">
              {{ formData.discountType === 'Percentage' ? 'Từ 0 đến 100%' : 'Số tiền giảm (VNĐ)' }}
            </p>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Ngày bắt đầu *</label>
            <input v-model="formData.startDate" type="datetime-local" required class="input-field" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Ngày kết thúc *</label>
            <input v-model="formData.endDate" type="datetime-local" required class="input-field" />
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái</label>
          <select v-model="formData.status" class="input-field">
            <option value="Active">Hoạt động</option>
            <option value="Inactive">Ngừng</option>
          </select>
        </div>

        <div class="flex justify-end space-x-3 pt-4">
          <button type="button" @click="closeModal" class="btn-secondary">Hủy</button>
          <button type="submit" class="btn-primary" :disabled="saving">
            {{ saving ? 'Đang lưu...' : (editingItem ? 'Cập nhật' : 'Thêm') }}
          </button>
        </div>
      </form>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal :show="showDeleteModal" title="Xác nhận xóa" @close="showDeleteModal = false">
      <p class="text-gray-700 mb-4">
        Bạn có chắc chắn muốn xóa giảm giá này? Hành động này không thể hoàn tác.
      </p>
      <div class="flex justify-end space-x-3">
        <button @click="showDeleteModal = false" class="btn-secondary">Hủy</button>
        <button @click="confirmDelete" class="btn-danger" :disabled="saving">
          {{ saving ? 'Đang xóa...' : 'Xóa' }}
        </button>
      </div>
    </Modal>

    <!-- Toast -->
    <Toast :show="toast.show" :message="toast.message" :type="toast.type" @close="toast.show = false" />
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import { discountApi, productApi } from '@/services/api'
import Modal from '@/components/common/Modal.vue'
import Toast from '@/components/common/Toast.vue'

const loading = ref(false)
const saving = ref(false)
const discounts = ref([])
const products = ref([])
const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingItem = ref(null)
const itemToDelete = ref(null)

const filterProductId = ref(null)
const filterStatus = ref('')

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const formData = reactive({
  productId: null,
  discountType: 'Percentage',
  discountValue: 0,
  startDate: '',
  endDate: '',
  status: 'Active'
})

const filteredDiscounts = computed(() => {
  return discounts.value.filter(discount => {
    const matchProduct = !filterProductId.value || discount.productId === filterProductId.value
    const matchStatus = !filterStatus.value || discount.status === filterStatus.value
    return matchProduct && matchStatus
  })
})

onMounted(async () => {
  await Promise.all([fetchDiscounts(), fetchProducts()])
})

async function fetchDiscounts() {
  loading.value = true
  try {
    discounts.value = await discountApi.getAll()
  } catch (error) {
    showToast('Không thể tải danh sách giảm giá', 'error')
    console.error('Error fetching discounts:', error)
  } finally {
    loading.value = false
  }
}

async function fetchProducts() {
  try {
    products.value = await productApi.getAll()
  } catch (error) {
    console.error('Error fetching products:', error)
  }
}

function resetFilters() {
  filterProductId.value = null
  filterStatus.value = ''
}

function editItem(discount) {
  editingItem.value = discount
  Object.assign(formData, {
    productId: discount.productId,
    discountType: discount.discountType,
    discountValue: discount.discountValue,
    startDate: formatDateTimeLocal(discount.startDate),
    endDate: formatDateTimeLocal(discount.endDate),
    status: discount.status || 'Active'
  })
  showEditModal.value = true
}

function deleteItem(discount) {
  itemToDelete.value = discount
  showDeleteModal.value = true
}

async function saveItem() {
  saving.value = true
  try {
    const data = {
      productId: formData.productId,
      discountType: formData.discountType,
      discountValue: formData.discountValue,
      startDate: formData.startDate,
      endDate: formData.endDate,
      status: formData.status,
      createBy: 'admin',
      updateBy: 'admin'
    }

    if (editingItem.value) {
      await discountApi.update(editingItem.value.id, data)
      showToast('Đã cập nhật giảm giá thành công', 'success')
    } else {
      await discountApi.create(data)
      showToast('Đã thêm giảm giá thành công', 'success')
    }

    closeModal()
    await fetchDiscounts()
  } catch (error) {
    showToast(error.response?.data?.message || 'Không thể lưu giảm giá', 'error')
    console.error('Error saving discount:', error)
  } finally {
    saving.value = false
  }
}

async function confirmDelete() {
  saving.value = true
  try {
    await discountApi.delete(itemToDelete.value.id)
    showToast('Đã xóa giảm giá thành công', 'success')
    showDeleteModal.value = false
    itemToDelete.value = null
    await fetchDiscounts()
  } catch (error) {
    showToast('Không thể xóa giảm giá', 'error')
    console.error('Error deleting discount:', error)
  } finally {
    saving.value = false
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingItem.value = null
  Object.assign(formData, {
    productId: null,
    discountType: 'Percentage',
    discountValue: 0,
    startDate: '',
    endDate: '',
    status: 'Active'
  })
}

function getStatusClass(discount) {
  const now = new Date()
  const start = new Date(discount.startDate)
  const end = new Date(discount.endDate)

  if (discount.status === 'Inactive') {
    return 'bg-gray-100 text-gray-800'
  }

  if (now < start) {
    return 'bg-yellow-100 text-yellow-800'
  }

  if (now > end) {
    return 'bg-red-100 text-red-800'
  }

  return 'bg-green-100 text-green-800'
}

function getStatusLabel(discount) {
  const now = new Date()
  const start = new Date(discount.startDate)
  const end = new Date(discount.endDate)

  if (discount.status === 'Inactive') {
    return 'Ngừng'
  }

  if (now < start) {
    return 'Sắp bắt đầu'
  }

  if (now > end) {
    return 'Đã hết hạn'
  }

  return 'Đang áp dụng'
}

function formatDate(dateString) {
  if (!dateString) return '-'
  const date = new Date(dateString)
  return date.toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

function formatDateTimeLocal(dateString) {
  if (!dateString) return ''
  const date = new Date(dateString)
  const year = date.getFullYear()
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const day = String(date.getDate()).padStart(2, '0')
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  return `${year}-${month}-${day}T${hours}:${minutes}`
}

function formatCurrency(amount) {
  if (!amount) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(amount)
}

function showToast(message, type = 'success') {
  toast.message = message
  toast.type = type
  toast.show = true
}
</script>

