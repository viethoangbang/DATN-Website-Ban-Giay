<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Hình ảnh</h1>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm hình ảnh</span>
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="flex justify-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Grid View -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
      <div v-for="image in images" :key="image.id" class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition-shadow">
        <div class="aspect-square bg-gray-100 relative">
          <img :src="image.url" :alt="image.type" class="w-full h-full object-cover" @error="handleImageError" />
          <div class="absolute top-2 right-2">
            <span :class="image.status === 'Active' ? 'bg-green-500' : 'bg-gray-500'" class="px-2 py-1 text-xs text-white rounded-full">
              {{ image.status === 'Active' ? 'Active' : 'Inactive' }}
            </span>
          </div>
        </div>
        <div class="p-4">
          <p class="text-sm text-gray-600 mb-2">ID: #{{ image.id }}</p>
          <p class="text-sm text-gray-600 mb-2">Type: {{ image.type || 'N/A' }}</p>
          <p class="text-xs text-gray-500 truncate" :title="image.url">{{ image.url }}</p>
          <div class="flex justify-end space-x-2 mt-4 pt-4 border-t">
            <button @click="editImage(image)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg">
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
              </svg>
            </button>
            <button @click="deleteImage(image)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg">
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
              </svg>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingImage ? 'Chỉnh sửa hình ảnh' : 'Thêm hình ảnh'" @close="closeModal">
      <form @submit.prevent="saveImage" class="space-y-4">
        <!-- Upload Method Tabs (only show when adding new image) -->
        <div v-if="!editingImage" class="flex space-x-2 border-b">
          <button
            type="button"
            @click="uploadMethod = 'file'"
            :class="[
              'px-4 py-2 font-medium text-sm transition-colors',
              uploadMethod === 'file'
                ? 'border-b-2 border-primary-500 text-primary-600'
                : 'text-gray-500 hover:text-gray-700'
            ]"
          >
            Upload từ máy
          </button>
          <button
            type="button"
            @click="uploadMethod = 'url'"
            :class="[
              'px-4 py-2 font-medium text-sm transition-colors',
              uploadMethod === 'url'
                ? 'border-b-2 border-primary-500 text-primary-600'
                : 'text-gray-500 hover:text-gray-700'
            ]"
          >
            Nhập URL
          </button>
        </div>

        <!-- File Upload -->
        <div v-if="!editingImage && uploadMethod === 'file'">
          <label class="block text-sm font-semibold text-gray-700 mb-2">Chọn hình ảnh * (có thể chọn nhiều)</label>
          <div class="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-primary-500 transition-colors">
            <input
              ref="fileInput"
              type="file"
              accept="image/jpeg,image/jpg,image/png,image/gif,image/webp"
              multiple
              @change="handleFileSelect"
              class="hidden"
            />
            <div v-if="selectedFiles.length === 0" class="space-y-2">
              <svg class="w-12 h-12 mx-auto text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12" />
              </svg>
              <p class="text-sm text-gray-600">Kéo thả hình ảnh vào đây hoặc</p>
              <button
                type="button"
                @click="$refs.fileInput.click()"
                class="btn-primary text-sm"
              >
                Chọn file (nhiều file)
              </button>
              <p class="text-xs text-gray-500 mt-2">
                Định dạng: JPG, PNG, GIF, WEBP (Tối đa 5MB/file, tối đa 10 file/lần)
              </p>
            </div>
            <div v-else class="space-y-4">
              <div class="grid grid-cols-2 gap-3 max-h-64 overflow-y-auto">
                <div v-for="(file, index) in selectedFiles" :key="index" class="relative border rounded-lg p-2 bg-gray-50">
                  <img :src="filePreviews[index]" alt="Preview" class="w-full h-32 object-cover rounded" />
                  <p class="text-xs text-gray-600 mt-1 truncate" :title="file.name">{{ file.name }}</p>
                  <p class="text-xs text-gray-500">{{ formatFileSize(file.size) }}</p>
                  <button
                    type="button"
                    @click="removeFile(index)"
                    class="absolute top-1 right-1 w-6 h-6 bg-red-500 text-white rounded-full flex items-center justify-center text-xs hover:bg-red-600"
                    title="Xóa"
                  >
                    ×
                  </button>
                </div>
              </div>
              <button
                type="button"
                @click="clearAllFiles"
                class="text-sm text-red-600 hover:text-red-700"
              >
                Xóa tất cả và chọn lại
              </button>
              <p class="text-xs text-gray-500">
                Đã chọn {{ selectedFiles.length }} file (tối đa 10 file)
              </p>
            </div>
          </div>
          <p v-if="fileError" class="text-xs text-red-600 mt-1">{{ fileError }}</p>
        </div>

        <!-- URL Input -->
        <div v-if="editingImage || uploadMethod === 'url'">
          <label class="block text-sm font-semibold text-gray-700 mb-2">URL hình ảnh *</label>
          <input v-model="formData.url" type="url" required class="input-field" placeholder="https://example.com/image.jpg" />
          <p class="text-xs text-gray-500 mt-1">Hỗ trợ URL tối đa 1000 ký tự</p>
        </div>

        <!-- Preview (only for URL) -->
        <div v-if="(formData.url || filePreview) && uploadMethod === 'url'" class="border rounded-lg p-4 bg-gray-50">
          <p class="text-sm font-semibold text-gray-700 mb-2">Xem trước:</p>
          <img :src="filePreview || formData.url" alt="Preview" class="w-full h-48 object-cover rounded" @error="handleImageError" />
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Loại hình ảnh</label>
          <div class="flex gap-2">
            <select v-model="formData.type" class="input-field flex-1">
              <option value="">-- Chọn hoặc nhập mới --</option>
              <option v-for="type in availableTypes" :key="type" :value="type">{{ type }}</option>
            </select>
            <input
              v-if="formData.type === '' || !availableTypes.includes(formData.type)"
              v-model="formData.type"
              type="text"
              class="input-field flex-1"
              placeholder="Nhập loại mới (ví dụ: Hero, Banner, Product...)"
            />
          </div>
          <p class="text-xs text-gray-500 mt-1">
            Chọn từ danh sách hoặc nhập loại mới. Ví dụ: Hero, Product, Variant, Banner
          </p>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái</label>
          <select v-model="formData.status" class="input-field">
            <option value="Active">Active</option>
            <option value="Inactive">Inactive</option>
          </select>
        </div>
      </form>
      <template #footer>
        <button @click="saveImage" :disabled="saving || uploading" class="btn-primary">
          {{ uploading ? 'Đang upload...' : (saving ? 'Đang lưu...' : (editingImage ? 'Cập nhật' : 'Lưu')) }}
        </button>
        <button @click="closeModal" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Modal -->
    <Modal :show="showDeleteModal" title="Xác nhận xóa" @close="showDeleteModal = false">
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa hình ảnh này?</p>
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
import { imageApi } from '../../services/api'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const images = ref([])
const loading = ref(false)
const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingImage = ref(null)
const imageToDelete = ref(null)
const saving = ref(false)
const uploading = ref(false)
const uploadMethod = ref('file') // 'file' or 'url'
const selectedFile = ref(null) // Keep for backward compatibility
const selectedFiles = ref([]) // Array for multiple files
const filePreview = ref(null) // Keep for backward compatibility
const filePreviews = ref([]) // Array for multiple file previews
const fileError = ref('')
const fileInput = ref(null)
const uploadProgress = ref({})
const availableTypes = ref([])

const toast = reactive({ show: false, message: '', type: 'success' })

const formData = reactive({
  url: '',
  type: '',
  status: 'Active'
})

onMounted(async () => {
  await Promise.all([
    fetchImages(),
    fetchTypes()
  ])
})

const fetchImages = async () => {
  loading.value = true
  try {
    images.value = await imageApi.getAll()
  } catch (error) {
    console.error('Error fetching images:', error)
  } finally {
    loading.value = false
  }
}

const fetchTypes = async () => {
  try {
    availableTypes.value = await imageApi.getTypes()
  } catch (error) {
    console.error('Error fetching types:', error)
    // Default types if API fails
    availableTypes.value = ['Product', 'Variant', 'Banner', 'Hero', 'Other']
  }
}

function handleImageError(e) {
  e.target.src = 'https://via.placeholder.com/400x400?text=Image+Not+Found'
}

function editImage(image) {
  editingImage.value = image
  Object.assign(formData, image)
  showEditModal.value = true
}

function deleteImage(image) {
  imageToDelete.value = image
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await imageApi.delete(imageToDelete.value.id)
    await fetchImages()
    toast.message = 'Đã xóa hình ảnh thành công'
    toast.type = 'success'
    toast.show = true
    showDeleteModal.value = false
  } catch (error) {
    toast.message = 'Không thể xóa hình ảnh'
    toast.type = 'error'
    toast.show = true
  }
}

function handleFileSelect(event) {
  const files = Array.from(event.target.files || [])
  if (files.length === 0) return

  fileError.value = ''

  // Validate file count (max 10 files)
  if (files.length > 10) {
    fileError.value = 'Tối đa 10 file mỗi lần upload'
    return
  }

  const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp']
  const maxSize = 5 * 1024 * 1024 // 5MB
  const validFiles = []
  const errors = []

  files.forEach((file, index) => {
    // Validate file type
    if (!allowedTypes.includes(file.type)) {
      errors.push(`${file.name}: Định dạng không hợp lệ`)
      return
    }

    // Validate file size
    if (file.size > maxSize) {
      errors.push(`${file.name}: Kích thước vượt quá 5MB`)
      return
    }

    validFiles.push(file)
  })

  if (errors.length > 0) {
    fileError.value = errors.join(', ')
  }

  if (validFiles.length > 0) {
    selectedFiles.value = [...selectedFiles.value, ...validFiles]
    
    // Create previews for new files
    validFiles.forEach((file) => {
      const reader = new FileReader()
      reader.onload = (e) => {
        filePreviews.value.push(e.target.result)
      }
      reader.readAsDataURL(file)
    })

    // Keep backward compatibility
    if (validFiles.length === 1) {
      selectedFile.value = validFiles[0]
      filePreview.value = filePreviews.value[filePreviews.value.length - 1]
    }
  }

  // Reset input to allow selecting same files again
  if (fileInput.value) {
    fileInput.value.value = ''
  }
}

function removeFile(index) {
  selectedFiles.value.splice(index, 1)
  filePreviews.value.splice(index, 1)
  
  // Update backward compatibility
  if (selectedFiles.value.length === 0) {
    selectedFile.value = null
    filePreview.value = null
  } else if (selectedFiles.value.length === 1) {
    selectedFile.value = selectedFiles.value[0]
    filePreview.value = filePreviews.value[0]
  }
}

function clearAllFiles() {
  selectedFiles.value = []
  filePreviews.value = []
  selectedFile.value = null
  filePreview.value = null
  fileError.value = ''
  if (fileInput.value) {
    fileInput.value.value = ''
  }
}

function removeSelectedFile() {
  clearAllFiles()
}

function formatFileSize(bytes) {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i]
}

async function saveImage() {
  // If uploading multiple files
  if (!editingImage.value && uploadMethod.value === 'file' && selectedFiles.value.length > 0) {
    uploading.value = true
    uploadProgress.value = {}
    
    try {
      // If only 1 file, use single upload endpoint
      if (selectedFiles.value.length === 1) {
        // Ensure type is sent even if empty string (will be trimmed)
        const typeToSend = formData.type && formData.type.trim() ? formData.type.trim() : null
        const result = await imageApi.upload(selectedFiles.value[0], typeToSend, null)
        toast.message = 'Đã upload và lưu hình ảnh thành công'
        toast.type = 'success'
        toast.show = true
        await fetchImages()
        closeModal()
      } else {
        // Multiple files - use multiple upload endpoint
        // Ensure type is sent even if empty string (will be trimmed)
        const typeToSend = formData.type && formData.type.trim() ? formData.type.trim() : null
        const result = await imageApi.uploadMultiple(selectedFiles.value, typeToSend, null)
        
        if (result.success > 0) {
          toast.message = `Đã upload thành công ${result.success} hình ảnh${result.failed > 0 ? `, ${result.failed} hình ảnh thất bại` : ''}`
          toast.type = result.failed > 0 ? 'warning' : 'success'
          toast.show = true
          
          if (result.errors && result.errors.length > 0) {
            console.error('Upload errors:', result.errors)
          }
          
          await fetchImages()
          closeModal()
        } else {
          toast.message = 'Tất cả hình ảnh upload thất bại'
          toast.type = 'error'
          toast.show = true
        }
      }
    } catch (error) {
      toast.message = error.response?.data?.message || 'Không thể upload hình ảnh'
      toast.type = 'error'
      toast.show = true
    } finally {
      uploading.value = false
      uploadProgress.value = {}
    }
    return
  }

  // If using URL or editing
  if (!formData.url && !editingImage.value) {
    toast.message = 'Vui lòng chọn file hoặc nhập URL'
    toast.type = 'warning'
    toast.show = true
    return
  }

  saving.value = true
  try {
    if (editingImage.value) {
      await imageApi.update(editingImage.value.id, formData)
      toast.message = 'Đã cập nhật hình ảnh thành công'
    } else {
      await imageApi.create(formData)
      toast.message = 'Đã thêm hình ảnh thành công'
    }
    await fetchImages()
    toast.type = 'success'
    toast.show = true
    closeModal()
  } catch (error) {
    toast.message = error.response?.data?.message || 'Không thể lưu hình ảnh'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingImage.value = null
  uploadMethod.value = 'file'
  clearAllFiles()
  Object.assign(formData, {
    url: '',
    type: '',
    status: 'Active'
  })
}
</script>
