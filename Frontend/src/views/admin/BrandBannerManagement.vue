<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Quản lý Banner Thương hiệu</h1>
        <p class="text-gray-600 mt-1">Quản lý banner images cho từng thương hiệu</p>
      </div>
      <button
        @click="showAddModal = true"
        class="btn-primary flex items-center space-x-2"
      >
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm banner</span>
      </button>
    </div>

    <!-- Filter by Brand -->
    <div class="flex flex-wrap gap-3">
      <select v-model="filterBrand" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
        <option value="">Tất cả thương hiệu</option>
        <option v-for="brand in distinctBrands" :key="brand" :value="brand">{{ brand }}</option>
      </select>
      <select v-model="filterStatus" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
        <option value="">Tất cả trạng thái</option>
        <option value="Active">Active</option>
        <option value="Inactive">Inactive</option>
      </select>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center items-center py-12">
      <svg class="animate-spin h-8 w-8 text-primary-500" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Banners Grid -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div
        v-for="banner in filteredBanners"
        :key="banner.id"
        class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-xl transition-shadow"
      >
        <!-- Banner Image -->
        <div class="relative h-48 bg-gray-100">
          <img
            :src="banner.imageUrl || '/placeholder-banner.jpg'"
            :alt="`${banner.brand} banner`"
            class="w-full h-full object-cover"
            @error="handleImageError"
          />
          <div class="absolute top-2 right-2">
            <span
              :class="[
                'px-2 py-1 rounded-full text-xs font-bold',
                banner.status === 'Active' ? 'bg-green-500 text-white' : 'bg-gray-500 text-white'
              ]"
            >
              {{ banner.status }}
            </span>
          </div>
        </div>

        <!-- Banner Info -->
        <div class="p-4">
          <h3 class="font-bold text-gray-900 mb-1">{{ banner.brand }}</h3>
          <p class="text-sm text-gray-600 mb-2">Display Order: {{ banner.displayOrder || 0 }}</p>
          <div class="flex items-center justify-between">
            <span class="text-xs text-gray-500">
              {{ new Date(banner.createDate).toLocaleDateString('vi-VN') }}
            </span>
            <div class="flex space-x-2">
              <button
                @click="editBanner(banner)"
                class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                title="Chỉnh sửa"
              >
                <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                </svg>
              </button>
              <button
                @click="deleteBanner(banner)"
                class="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                title="Xóa"
              >
                <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                </svg>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Empty State -->
    <div v-if="!loading && filteredBanners.length === 0" class="text-center py-12 bg-white rounded-xl shadow-md">
      <svg class="w-16 h-16 text-gray-400 mx-auto mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
      </svg>
      <p class="text-gray-600 mb-4">Chưa có banner nào</p>
      <button @click="showAddModal = true" class="btn-primary">
        Thêm banner đầu tiên
      </button>
    </div>

    <!-- Add/Edit Modal -->
    <Modal 
      :show="showAddModal || showEditModal" 
      :title="editingBanner ? 'Chỉnh sửa banner' : 'Thêm banner mới'" 
      @close="closeModal"
    >
      <form @submit.prevent="saveBanner" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Thương hiệu *</label>
          <input 
            v-model="formData.brand" 
            type="text" 
            required 
            class="input-field" 
            placeholder="VD: Nike, Adidas, Puma..."
            list="brands-list"
          />
          <datalist id="brands-list">
            <option v-for="brand in distinctBrands" :key="brand" :value="brand" />
          </datalist>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Hình ảnh *</label>
          <select 
            v-model="formData.imageId" 
            required 
            class="input-field"
          >
            <option value="">Chọn hình ảnh</option>
            <option v-for="image in availableImages" :key="image.id" :value="image.id">
              {{ image.url || `Image #${image.id}` }}
            </option>
          </select>
          <p class="text-xs text-gray-500 mt-1">
            Chưa có hình ảnh? 
            <router-link to="/admin/images" class="text-primary-600 hover:underline">
              Thêm hình ảnh mới
            </router-link>
          </p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Display Order</label>
            <input 
              v-model.number="formData.displayOrder" 
              type="number" 
              min="0"
              class="input-field" 
              placeholder="0"
            />
            <p class="text-xs text-gray-500 mt-1">Số nhỏ hơn sẽ hiển thị trước</p>
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái</label>
            <select v-model="formData.status" class="input-field">
              <option value="Active">Active</option>
              <option value="Inactive">Inactive</option>
            </select>
          </div>
        </div>

        <!-- Preview -->
        <div v-if="selectedImageUrl" class="mt-4">
          <label class="block text-sm font-semibold text-gray-700 mb-2">Preview</label>
          <div class="relative h-32 bg-gray-100 rounded-lg overflow-hidden">
            <img 
              :src="selectedImageUrl" 
              alt="Preview" 
              class="w-full h-full object-cover"
              @error="handleImageError"
            />
          </div>
        </div>
      </form>
      <template #footer>
        <button 
          @click="saveBanner" 
          :disabled="saving"
          class="btn-primary disabled:opacity-50"
        >
          {{ saving ? 'Đang lưu...' : 'Lưu' }}
        </button>
        <button @click="closeModal" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal
      :show="showDeleteModal"
      title="Xác nhận xóa"
      @close="showDeleteModal = false"
    >
      <p class="text-gray-600">
        Bạn có chắc chắn muốn xóa banner cho thương hiệu <strong>{{ bannerToDelete?.brand }}</strong>? 
        Hành động này không thể hoàn tác.
      </p>
      <template #footer>
        <button @click="confirmDelete" class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700">
          Xóa
        </button>
        <button @click="showDeleteModal = false" class="btn-secondary">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Toast -->
    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />
  </div>
</template>

<script setup>
import { ref, computed, reactive, onMounted } from 'vue'
import { brandBannerApi, imageApi, handleApiError } from '../../services/api'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const banners = ref([])
const availableImages = ref([])
const loading = ref(false)
const filterBrand = ref('')
const filterStatus = ref('')

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)

const bannerToDelete = ref(null)
const editingBanner = ref(null)
const saving = ref(false)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const formData = reactive({
  brand: '',
  imageId: null,
  displayOrder: 0,
  status: 'Active',
  createBy: 'admin'
})

// Initialize data
onMounted(async () => {
  await fetchData()
})

async function fetchData() {
  loading.value = true
  try {
    const [bannersData, imagesData] = await Promise.all([
      brandBannerApi.getAll(),
      imageApi.getAll()
    ])
    banners.value = bannersData
    availableImages.value = imagesData
  } catch (error) {
    toast.message = handleApiError(error)
    toast.type = 'error'
    toast.show = true
  } finally {
    loading.value = false
  }
}

const distinctBrands = computed(() => {
  const brands = new Set(banners.value.map(b => b.brand))
  return Array.from(brands).sort()
})

const filteredBanners = computed(() => {
  return banners.value.filter(banner => {
    const matchBrand = !filterBrand.value || banner.brand === filterBrand.value
    const matchStatus = !filterStatus.value || banner.status === filterStatus.value
    return matchBrand && matchStatus
  }).sort((a, b) => {
    // Sort by brand, then by display order
    if (a.brand !== b.brand) {
      return a.brand.localeCompare(b.brand)
    }
    return (a.displayOrder || 0) - (b.displayOrder || 0)
  })
})

const selectedImageUrl = computed(() => {
  if (!formData.imageId) return null
  const image = availableImages.value.find(img => img.id === formData.imageId)
  return image?.url || null
})

function editBanner(banner) {
  editingBanner.value = banner
  Object.assign(formData, {
    brand: banner.brand,
    imageId: banner.imageId,
    displayOrder: banner.displayOrder || 0,
    status: banner.status || 'Active',
    updateBy: 'admin'
  })
  showEditModal.value = true
}

function deleteBanner(banner) {
  bannerToDelete.value = banner
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await brandBannerApi.delete(bannerToDelete.value.id)
    toast.message = 'Đã xóa banner thành công'
    toast.type = 'success'
    toast.show = true
    await fetchData()
    showDeleteModal.value = false
  } catch (error) {
    toast.message = handleApiError(error)
    toast.type = 'error'
    toast.show = true
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingBanner.value = null
  Object.assign(formData, {
    brand: '',
    imageId: null,
    displayOrder: 0,
    status: 'Active',
    createBy: 'admin'
  })
}

async function saveBanner() {
  if (!formData.brand || !formData.imageId) {
    toast.message = 'Vui lòng điền đầy đủ thông tin'
    toast.type = 'error'
    toast.show = true
    return
  }

  saving.value = true
  try {
    if (editingBanner.value) {
      await brandBannerApi.update(editingBanner.value.id, formData)
      toast.message = 'Đã cập nhật banner thành công'
    } else {
      await brandBannerApi.create(formData)
      toast.message = 'Đã thêm banner thành công'
    }
    toast.type = 'success'
    toast.show = true
    await fetchData()
    closeModal()
  } catch (error) {
    toast.message = handleApiError(error)
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

function handleImageError(e) {
  e.target.src = '/placeholder-banner.jpg'
}
</script>

