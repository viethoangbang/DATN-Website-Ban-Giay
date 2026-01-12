<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Kích cỡ</h1>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm size</span>
      </button>
    </div>

    <!-- Loading -->
    <div v-if="productStore.loading" class="flex justify-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Grid View -->
    <div v-else class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 xl:grid-cols-8 gap-4">
      <div v-for="size in sortedSizes" :key="size.id" class="bg-white rounded-xl shadow-md p-6 hover:shadow-lg transition-shadow text-center">
        <div class="text-3xl font-bold text-primary-600 mb-2">{{ size.sizeName }}</div>
        <p class="text-sm text-gray-500 mb-4">ID: #{{ size.id }}</p>
        <div class="flex justify-center space-x-2 pt-4 border-t">
          <button @click="editItem(size)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg">
            <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </button>
          <button @click="deleteItem(size)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg">
            <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingItem ? 'Chỉnh sửa size' : 'Thêm size'" @close="closeModal">
      <form @submit.prevent="saveItem" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Kích cỡ *</label>
          <input v-model="formData.sizeName" type="text" required class="input-field" placeholder="VD: 38, 39, 40, XL, M" />
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
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa size <strong>{{ itemToDelete?.sizeName }}</strong>?</p>
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
import { useProductStore } from '../../stores/products'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const productStore = useProductStore()

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingItem = ref(null)
const itemToDelete = ref(null)
const saving = ref(false)

const toast = reactive({ show: false, message: '', type: 'success' })
const formData = reactive({ sizeName: '' })

const sortedSizes = computed(() => {
  return [...productStore.sizes].sort((a, b) => {
    const aNum = parseFloat(a.sizeName)
    const bNum = parseFloat(b.sizeName)
    if (!isNaN(aNum) && !isNaN(bNum)) return aNum - bNum
    return a.sizeName.localeCompare(b.sizeName)
  })
})

onMounted(() => {
  productStore.fetchSizes()
})

function editItem(item) {
  editingItem.value = item
  formData.sizeName = item.sizeName
  showEditModal.value = true
}

function deleteItem(item) {
  itemToDelete.value = item
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await productStore.deleteSize(itemToDelete.value.id)
    toast.message = 'Đã xóa size thành công'
    toast.type = 'success'
    toast.show = true
    showDeleteModal.value = false
  } catch (error) {
    toast.message = 'Không thể xóa size'
    toast.type = 'error'
    toast.show = true
  }
}

async function saveItem() {
  // Frontend validation
  if (!formData.sizeName || formData.sizeName.trim() === '') {
    toast.message = 'Vui lòng nhập tên size'
    toast.type = 'error'
    toast.show = true
    return
  }

  // Check for duplicate (case-insensitive)
  const trimmedSizeName = formData.sizeName.trim()
  const existing = productStore.sizes.find(s => 
    s.sizeName && s.sizeName.toLowerCase() === trimmedSizeName.toLowerCase() &&
    (!editingItem.value || s.id !== editingItem.value.id)
  )

  if (existing) {
    toast.message = `Size "${trimmedSizeName}" đã tồn tại`
    toast.type = 'error'
    toast.show = true
    return
  }

  saving.value = true
  try {
    if (editingItem.value) {
      await productStore.updateSize(editingItem.value.id, { ...formData, sizeName: trimmedSizeName })
      toast.message = 'Đã cập nhật size thành công'
    } else {
      await productStore.createSize({ ...formData, sizeName: trimmedSizeName })
      toast.message = 'Đã thêm size thành công'
    }
    toast.type = 'success'
    toast.show = true
    closeModal()
  } catch (error) {
    toast.message = productStore.error || error.message || 'Không thể lưu size'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingItem.value = null
  formData.sizeName = ''
}
</script>
