<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Danh mục</h1>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm danh mục</span>
      </button>
    </div>

    <!-- Loading -->
    <div v-if="productStore.loading" class="flex justify-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Table -->
    <div v-else class="bg-white rounded-xl shadow-md overflow-hidden">
      <table class="w-full">
        <thead class="bg-gray-50 border-b">
          <tr>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">ID</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Tên danh mục</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Mô tả</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Trạng thái</th>
            <th class="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Thao tác</th>
          </tr>
        </thead>
        <tbody class="divide-y">
          <tr v-for="cat in productStore.categories" :key="cat.id" class="hover:bg-gray-50">
            <td class="px-6 py-4 text-sm">#{{ cat.id }}</td>
            <td class="px-6 py-4 font-semibold">{{ cat.name }}</td>
            <td class="px-6 py-4 text-sm text-gray-600">{{ cat.description || '-' }}</td>
            <td class="px-6 py-4">
              <span :class="cat.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'" class="px-3 py-1 rounded-full text-sm font-medium">
                {{ cat.status === 'Active' ? 'Hoạt động' : 'Ngừng' }}
              </span>
            </td>
            <td class="px-6 py-4">
              <div class="flex justify-end space-x-2">
                <button @click="editItem(cat)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                  </svg>
                </button>
                <button @click="deleteItem(cat)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg">
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

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingItem ? 'Chỉnh sửa danh mục' : 'Thêm danh mục'" @close="closeModal">
      <form @submit.prevent="saveItem" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Tên danh mục *</label>
          <input v-model="formData.name" type="text" required class="input-field" />
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
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa danh mục <strong>{{ itemToDelete?.name }}</strong>?</p>
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

const formData = reactive({
  name: '',
  description: '',
  status: 'Active'
})

onMounted(() => {
  productStore.fetchCategories()
})

function editItem(item) {
  editingItem.value = item
  Object.assign(formData, item)
  showEditModal.value = true
}

function deleteItem(item) {
  itemToDelete.value = item
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await productStore.deleteCategory(itemToDelete.value.id)
    toast.message = 'Đã xóa danh mục thành công'
    toast.type = 'success'
    toast.show = true
    showDeleteModal.value = false
  } catch (error) {
    toast.message = 'Không thể xóa danh mục'
    toast.type = 'error'
    toast.show = true
  }
}

async function saveItem() {
  saving.value = true
  try {
    if (editingItem.value) {
      await productStore.updateCategory(editingItem.value.id, formData)
      toast.message = 'Đã cập nhật danh mục thành công'
    } else {
      await productStore.createCategory(formData)
      toast.message = 'Đã thêm danh mục thành công'
    }
    toast.type = 'success'
    toast.show = true
    closeModal()
  } catch (error) {
    toast.message = productStore.error || 'Không thể lưu danh mục'
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
  Object.assign(formData, { name: '', description: '', status: 'Active' })
}
</script>
