<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Quản lý Hero Banner</h1>
        <p class="text-gray-600 mt-1">
          Tự động hiển thị tất cả hình ảnh có Type = "Hero". 
          Để thêm Hero Image, vào 
          <router-link to="/admin/images" class="text-primary-600 hover:underline font-semibold">Quản lý Hình ảnh</router-link>
          và đặt Type = "Hero"
        </p>
      </div>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm Hero Image</span>
      </button>
    </div>

    <!-- Info Banner -->
    <div class="bg-blue-50 border border-blue-200 rounded-lg p-4 flex items-start space-x-3">
      <svg class="w-5 h-5 text-blue-600 flex-shrink-0 mt-0.5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
      <div class="flex-1">
        <p class="text-sm text-blue-800 font-semibold mb-1">Trang này tự động lọc và hiển thị tất cả hình ảnh có Type = "Hero"</p>
        <p class="text-xs text-blue-700">
          Bất kỳ hình ảnh nào có Type = "Hero" trong 
          <router-link to="/admin/images" class="underline font-medium">Quản lý Hình ảnh</router-link>
          sẽ tự động xuất hiện ở đây. Hiện có <strong>{{ heroImages.length }}</strong> Hero Image{{ heroImages.length !== 1 ? 's' : '' }}.
        </p>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="flex justify-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Empty State -->
    <div v-else-if="heroImages.length === 0" class="text-center py-12 bg-white rounded-xl shadow-md">
      <svg class="w-24 h-24 mx-auto text-gray-400 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
      </svg>
      <h3 class="text-xl font-bold text-gray-900 mb-2">Chưa có Hero Image</h3>
      <p class="text-gray-600 mb-2">Chưa có hình ảnh nào có Type = "Hero" trong hệ thống</p>
      <p class="text-sm text-gray-500 mb-4">
        Để thêm Hero Image, vào 
        <router-link to="/admin/images" class="text-primary-600 hover:underline font-semibold">Quản lý Hình ảnh</router-link>
        và đặt Type = "Hero" khi tạo hoặc chỉnh sửa hình ảnh
      </p>
      <div class="flex justify-center gap-3">
        <router-link to="/admin/images" class="btn-primary">
          Đi đến Quản lý Hình ảnh
        </router-link>
        <button @click="showAddModal = true" class="btn-secondary">
          Hoặc thêm từ đây
        </button>
      </div>
    </div>

    <!-- Grid View -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
      <div v-for="image in heroImages" :key="image.id" class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition-shadow">
        <div class="aspect-video bg-gray-100 relative">
          <img :src="image.url" :alt="`Hero Image ${image.id}`" class="w-full h-full object-cover" @error="handleImageError" />
          <div class="absolute top-2 right-2">
            <span class="bg-primary-500 text-white px-2 py-1 text-xs font-bold rounded-full">
              Hero
            </span>
          </div>
        </div>
        <div class="p-4">
          <p class="text-sm text-gray-600 mb-2">ID: #{{ image.id }}</p>
          <p class="text-xs text-gray-500 truncate" :title="image.url">{{ image.url }}</p>
          <div class="flex justify-end space-x-2 mt-4 pt-4 border-t">
            <button 
              @click="removeFromHero(image)" 
              class="p-2 text-orange-600 hover:bg-orange-50 rounded-lg"
              title="Gỡ khỏi Hero Banner"
            >
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
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

    <!-- Add Hero Image Modal -->
    <Modal :show="showAddModal" title="Thêm Hero Image" @close="closeAddModal">
      <div class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Chọn hình ảnh từ danh sách *</label>
          <select v-model="selectedImageId" class="input-field">
            <option value="">-- Chọn hình ảnh --</option>
            <option v-for="img in availableImages" :key="img.id" :value="img.id">
              {{ img.url || `Image #${img.id}` }} ({{ img.type || 'N/A' }})
            </option>
          </select>
          <p class="text-xs text-gray-500 mt-1">
            Chỉ hiển thị hình ảnh chưa có Type = "Hero". 
            <router-link to="/admin/images" class="text-primary-600 hover:underline">
              Thêm hình ảnh mới
            </router-link>
          </p>
        </div>

        <div v-if="selectedImagePreview" class="border rounded-lg p-4 bg-gray-50">
          <p class="text-sm font-semibold text-gray-700 mb-2">Xem trước:</p>
          <img :src="selectedImagePreview" alt="Preview" class="w-full h-48 object-cover rounded" @error="handleImageError" />
        </div>
      </div>
      <template #footer>
        <button @click="addToHero" :disabled="!selectedImageId || saving" class="btn-primary">
          {{ saving ? 'Đang thêm...' : 'Thêm vào Hero Banner' }}
        </button>
        <button @click="closeAddModal" class="btn-secondary">Hủy</button>
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
import { ref, reactive, computed, onMounted } from 'vue'
import { imageApi } from '../../services/api'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const heroImages = ref([])
const availableImages = ref([])
const loading = ref(false)
const showAddModal = ref(false)
const showDeleteModal = ref(false)
const imageToDelete = ref(null)
const saving = ref(false)
const selectedImageId = ref(null)

const toast = reactive({ show: false, message: '', type: 'success' })

const selectedImagePreview = computed(() => {
  if (!selectedImageId.value) return null
  const image = availableImages.value.find(img => img.id === selectedImageId.value)
  return image?.url || null
})

onMounted(async () => {
  await Promise.all([
    fetchHeroImages(),
    fetchAvailableImages()
  ])
})

const fetchHeroImages = async () => {
  loading.value = true
  try {
    heroImages.value = await imageApi.getHeroImages()
  } catch (error) {
    console.error('Error fetching hero images:', error)
    toast.message = 'Không thể tải danh sách hero images'
    toast.type = 'error'
    toast.show = true
  } finally {
    loading.value = false
  }
}

const fetchAvailableImages = async () => {
  try {
    const allImages = await imageApi.getAll()
    // Filter out images that are already Hero type
    availableImages.value = allImages.filter(img => img.type !== 'Hero' && img.status === 'Active')
  } catch (error) {
    console.error('Error fetching available images:', error)
  }
}

function handleImageError(e) {
  e.target.src = 'https://via.placeholder.com/400x400?text=Image+Not+Found'
}

async function addToHero() {
  if (!selectedImageId.value) {
    toast.message = 'Vui lòng chọn hình ảnh'
    toast.type = 'warning'
    toast.show = true
    return
  }

  saving.value = true
  try {
    await imageApi.update(selectedImageId.value, { type: 'Hero' })
    toast.message = 'Đã thêm vào Hero Banner thành công'
    toast.type = 'success'
    toast.show = true
    await Promise.all([
      fetchHeroImages(),
      fetchAvailableImages()
    ])
    closeAddModal()
  } catch (error) {
    toast.message = error.response?.data?.message || 'Không thể thêm vào Hero Banner'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

async function removeFromHero(image) {
  saving.value = true
  try {
    await imageApi.update(image.id, { type: 'Other' })
    toast.message = 'Đã gỡ khỏi Hero Banner thành công'
    toast.type = 'success'
    toast.show = true
    await Promise.all([
      fetchHeroImages(),
      fetchAvailableImages()
    ])
  } catch (error) {
    toast.message = error.response?.data?.message || 'Không thể gỡ khỏi Hero Banner'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

function deleteImage(image) {
  imageToDelete.value = image
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await imageApi.delete(imageToDelete.value.id)
    await fetchHeroImages()
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

function closeAddModal() {
  showAddModal.value = false
  selectedImageId.value = null
}
</script>

