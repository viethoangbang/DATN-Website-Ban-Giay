<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Voucher</h1>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm voucher</span>
      </button>
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
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Mã</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Tên</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Loại</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Giảm giá</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Số lượng</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Ngày bắt đầu</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Ngày kết thúc</th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Trạng thái</th>
              <th class="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Thao tác</th>
            </tr>
          </thead>
          <tbody class="divide-y">
            <tr v-for="voucher in vouchers" :key="voucher.id" class="hover:bg-gray-50">
              <td class="px-6 py-4 text-sm">#{{ voucher.id }}</td>
              <td class="px-6 py-4 font-mono text-sm font-semibold text-primary-600">{{ voucher.code }}</td>
              <td class="px-6 py-4 font-semibold">{{ voucher.name }}</td>
              <td class="px-6 py-4 text-sm">
                <span :class="voucher.type === 'Percentage' ? 'bg-blue-100 text-blue-800' : 'bg-purple-100 text-purple-800'" class="px-2 py-1 rounded-full text-xs font-medium">
                  {{ voucher.type === 'Percentage' ? '%' : 'VNĐ' }}
                </span>
              </td>
              <td class="px-6 py-4 text-sm">
                <div>
                  <div class="font-semibold">{{ formatDiscount(voucher) }}</div>
                  <div v-if="voucher.maxDiscount" class="text-xs text-gray-500">Tối đa: {{ formatCurrency(voucher.maxDiscount) }}</div>
                </div>
              </td>
              <td class="px-6 py-4 text-sm">{{ voucher.quantity || '∞' }}</td>
              <td class="px-6 py-4 text-sm text-gray-600">{{ formatDate(voucher.startDate) }}</td>
              <td class="px-6 py-4 text-sm text-gray-600">{{ formatDate(voucher.endDate) }}</td>
              <td class="px-6 py-4">
                <span :class="getStatusClass(voucher)" class="px-3 py-1 rounded-full text-sm font-medium">
                  {{ getStatusLabel(voucher) }}
                </span>
              </td>
              <td class="px-6 py-4">
                <div class="flex justify-end space-x-2">
                  <button @click="editItem(voucher)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg">
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                    </svg>
                  </button>
                  <button @click="deleteItem(voucher)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg">
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

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingItem ? 'Chỉnh sửa voucher' : 'Thêm voucher'" @close="closeModal">
      <form @submit.prevent="saveItem" class="space-y-4">
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Mã voucher *</label>
            <input v-model="formData.code" type="text" required :readonly="editingItem" class="input-field" :class="{ 'bg-gray-100': editingItem }" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Tên voucher *</label>
            <input v-model="formData.name" type="text" required class="input-field" />
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Loại *</label>
            <select v-model="formData.type" required class="input-field">
              <option value="Percentage">Phần trăm (%)</option>
              <option value="Fixed">Số tiền cố định (VNĐ)</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Giảm giá *</label>
            <input v-model.number="formData.discount" type="number" step="0.01" min="0" required class="input-field" />
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Giảm giá tối đa (VNĐ)</label>
          <input v-model.number="formData.maxDiscount" type="number" step="0.01" min="0" class="input-field" />
          <p class="text-xs text-gray-500 mt-1">Chỉ áp dụng cho loại phần trăm. Để trống nếu không giới hạn.</p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Số lượng</label>
            <input v-model.number="formData.quantity" type="number" min="0" class="input-field" />
            <p class="text-xs text-gray-500 mt-1">Để trống nếu không giới hạn</p>
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Danh mục</label>
            <select v-model.number="formData.categoryId" class="input-field">
              <option :value="null">Tất cả danh mục</option>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
            </select>
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
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mô tả</label>
          <textarea v-model="formData.description" rows="3" class="input-field"></textarea>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái</label>
          <select v-model="formData.status" class="input-field">
            <option value="Active">Hoạt động</option>
            <option value="Inactive">Ngừng</option>
          </select>
        </div>
      </form>
      <template #footer>
        <button @click="saveItem" :disabled="saving" class="btn-primary">
          {{ saving ? 'Đang lưu...' : (editingItem ? 'Cập nhật' : 'Lưu') }}
        </button>
        <button @click="closeModal" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Modal -->
    <Modal :show="showDeleteModal" title="Xác nhận xóa" @close="showDeleteModal = false">
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa voucher <strong>{{ itemToDelete?.name }}</strong>?</p>
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
import { ref, reactive, onMounted } from 'vue'
import { voucherApi, handleApiError } from '@/services/api'
import { useProductStore } from '@/stores/products'
import Modal from '@/components/common/Modal.vue'
import Toast from '@/components/common/Toast.vue'
import { useAuthStore } from '@/stores/auth'

const productStore = useProductStore()
const authStore = useAuthStore()

const loading = ref(false)
const saving = ref(false)
const vouchers = ref([])
const categories = ref([])

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingItem = ref(null)
const itemToDelete = ref(null)

const toast = reactive({ show: false, message: '', type: 'success' })

const formData = reactive({
  code: '',
  name: '',
  type: 'Percentage',
  discount: 0,
  maxDiscount: null,
  quantity: null,
  description: '',
  status: 'Active',
  startDate: '',
  endDate: '',
  categoryId: null
})

onMounted(async () => {
  await loadVouchers()
  await loadCategories()
})

async function loadVouchers() {
  try {
    loading.value = true
    vouchers.value = await voucherApi.getAll()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    loading.value = false
  }
}

async function loadCategories() {
  try {
    await productStore.fetchCategories()
    categories.value = productStore.categories
  } catch (error) {
    console.error('Failed to load categories:', error)
  }
}

function editItem(item) {
  editingItem.value = item
  formData.code = item.code || ''
  formData.name = item.name || ''
  formData.type = item.type || 'Percentage'
  formData.discount = item.discount || 0
  formData.maxDiscount = item.maxDiscount || null
  formData.quantity = item.quantity || null
  formData.description = item.description || ''
  formData.status = item.status || 'Active'
  formData.startDate = item.startDate ? formatDateForInput(item.startDate) : ''
  formData.endDate = item.endDate ? formatDateForInput(item.endDate) : ''
  formData.categoryId = item.categoryId || null
  showEditModal.value = true
}

function deleteItem(item) {
  itemToDelete.value = item
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await voucherApi.delete(itemToDelete.value.id)
    showToast('Đã xóa voucher thành công', 'success')
    showDeleteModal.value = false
    await loadVouchers()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  }
}

async function saveItem() {
  saving.value = true
  try {
    const payload = {
      ...formData,
      createBy: authStore.user?.email || 'admin',
      updateBy: authStore.user?.email || 'admin'
    }

    if (editingItem.value) {
      await voucherApi.update(editingItem.value.id, payload)
      showToast('Đã cập nhật voucher thành công', 'success')
    } else {
      await voucherApi.create(payload)
      showToast('Đã thêm voucher thành công', 'success')
    }
    closeModal()
    await loadVouchers()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    saving.value = false
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingItem.value = null
  Object.assign(formData, {
    code: '',
    name: '',
    type: 'Percentage',
    discount: 0,
    maxDiscount: null,
    quantity: null,
    description: '',
    status: 'Active',
    startDate: '',
    endDate: '',
    categoryId: null
  })
}

function formatDiscount(voucher) {
  if (voucher.type === 'Percentage') {
    return `${voucher.discount}%`
  }
  return formatCurrency(voucher.discount)
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

function formatDateForInput(dateString) {
  if (!dateString) return ''
  const date = new Date(dateString)
  const year = date.getFullYear()
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const day = String(date.getDate()).padStart(2, '0')
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  return `${year}-${month}-${day}T${hours}:${minutes}`
}

function getStatusClass(voucher) {
  const now = new Date()
  const startDate = voucher.startDate ? new Date(voucher.startDate) : null
  const endDate = voucher.endDate ? new Date(voucher.endDate) : null

  if (voucher.status === 'Inactive') {
    return 'bg-gray-100 text-gray-800'
  }

  if (startDate && now < startDate) {
    return 'bg-yellow-100 text-yellow-800'
  }

  if (endDate && now > endDate) {
    return 'bg-red-100 text-red-800'
  }

  if (voucher.quantity !== null && voucher.quantity <= 0) {
    return 'bg-red-100 text-red-800'
  }

  return 'bg-green-100 text-green-800'
}

function getStatusLabel(voucher) {
  const now = new Date()
  const startDate = voucher.startDate ? new Date(voucher.startDate) : null
  const endDate = voucher.endDate ? new Date(voucher.endDate) : null

  if (voucher.status === 'Inactive') {
    return 'Ngừng'
  }

  if (startDate && now < startDate) {
    return 'Chưa bắt đầu'
  }

  if (endDate && now > endDate) {
    return 'Hết hạn'
  }

  if (voucher.quantity !== null && voucher.quantity <= 0) {
    return 'Hết số lượng'
  }

  return 'Hoạt động'
}

function showToast(message, type = 'success') {
  toast.message = message
  toast.type = type
  toast.show = true
}
</script>

