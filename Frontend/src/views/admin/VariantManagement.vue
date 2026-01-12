<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Biến thể sản phẩm</h1>
        <p class="text-sm text-gray-600 mt-1">Quản lý các phiên bản sản phẩm theo màu sắc và kích cỡ</p>
      </div>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm biến thể</span>
      </button>
    </div>

    <!-- Filters -->
    <div class="flex flex-wrap gap-3">
      <select v-model="filterProduct" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500">
        <option value="">Tất cả sản phẩm</option>
        <option v-for="product in productStore.products" :key="product.id" :value="product.id">{{ product.name }}</option>
      </select>
      <select v-model="filterColor" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500">
        <option value="">Tất cả màu</option>
        <option v-for="color in productStore.colors" :key="color.id" :value="color.id">{{ color.name }}</option>
      </select>
      <select v-model="filterSize" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500">
        <option value="">Tất cả size</option>
        <option v-for="size in productStore.sizes" :key="size.id" :value="size.id">{{ size.sizeName }}</option>
      </select>
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
      <table class="w-full">
        <thead class="bg-gray-50 border-b">
          <tr>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Sản phẩm</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Hình ảnh</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Màu sắc</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Kích cỡ</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Giá</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Tồn kho</th>
            <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase">Trạng thái</th>
            <th class="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase">Thao tác</th>
          </tr>
        </thead>
        <tbody class="divide-y">
          <tr v-for="variant in filteredVariants" :key="variant.id" class="hover:bg-gray-50">
            <td class="px-6 py-4">
              <div>
                <p class="font-semibold text-gray-900">{{ getProductName(variant.productId) }}</p>
                <p class="text-sm text-gray-500">ID: #{{ variant.id }}</p>
              </div>
            </td>
            <td class="px-6 py-4">
              <div class="flex flex-wrap gap-1 max-w-[200px]">
                <img
                  v-for="(img, idx) in getVariantImages(variant)"
                  :key="img.id || idx"
                  :src="img.url"
                  :alt="`Image ${idx + 1}`"
                  class="w-10 h-10 object-cover rounded border border-gray-200 hover:border-primary-500 transition-colors cursor-pointer"
                  :title="img.url"
                  @error="handleImageError"
                />
                <div v-if="getVariantImages(variant).length === 0" class="w-10 h-10 bg-gray-100 rounded border border-gray-200 flex items-center justify-center">
                  <svg class="w-5 h-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                </div>
              </div>
            </td>
            <td class="px-6 py-4">
              <div class="flex items-center space-x-2">
                <div 
                  class="w-6 h-6 rounded-full border border-gray-300" 
                  :style="{ backgroundColor: getColorValue(variant.colorId) }"
                  :title="getColorName(variant.colorId)"
                ></div>
                <span>{{ getColorName(variant.colorId) }}</span>
              </div>
            </td>
            <td class="px-6 py-4">
              <span class="px-3 py-1 bg-gray-100 text-gray-800 rounded-full text-sm font-medium">
                {{ getSizeName(variant.sizeId) }}
              </span>
            </td>
            <td class="px-6 py-4 font-semibold text-gray-900">{{ formatPrice(variant.price) }}</td>
            <td class="px-6 py-4">
              <span :class="variant.quantity > 10 ? 'text-green-600' : 'text-red-600'" class="font-semibold">
                {{ variant.quantity }}
              </span>
            </td>
            <td class="px-6 py-4">
              <span :class="(variant.quantity ?? 0) > 0 ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'" class="px-3 py-1 rounded-full text-sm font-medium">
                {{ (variant.quantity ?? 0) > 0 ? 'Còn hàng' : 'Hết hàng' }}
              </span>
            </td>
            <td class="px-6 py-4">
              <div class="flex justify-end space-x-2">
                <button @click="editVariant(variant)" class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                  </svg>
                </button>
                <button @click="deleteVariant(variant)" class="p-2 text-red-600 hover:bg-red-50 rounded-lg">
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Empty State -->
      <div v-if="filteredVariants.length === 0" class="text-center py-12">
        <p class="text-gray-500">Chưa có biến thể nào</p>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingVariant ? 'Chỉnh sửa biến thể' : 'Thêm biến thể mới'" @close="closeModal">
      <form @submit.prevent="saveVariant" class="space-y-4">
        <!-- Step 1: Select Product (only for add mode) -->
        <div v-if="!editingVariant && !formData.productId">
          <label class="block text-sm font-semibold text-gray-700 mb-2">Bước 1: Chọn sản phẩm *</label>
          <select v-model.number="formData.productId" required class="input-field" @change="onProductSelected">
            <option :value="null">-- Chọn sản phẩm --</option>
            <option v-for="product in productStore.products" :key="product.id" :value="product.id">{{ product.name }}</option>
          </select>
          <p class="text-xs text-gray-500 mt-2">Sau khi chọn sản phẩm, bạn sẽ chọn màu sắc và kích cỡ</p>
        </div>

        <!-- Step 2: Select Variant Details (shown after product is selected or in edit mode) -->
        <template v-if="editingVariant || formData.productId">
          <div v-if="!editingVariant" class="mb-4 p-3 bg-blue-50 rounded-lg border border-blue-200">
            <div class="flex items-center justify-between">
              <div>
                <p class="text-sm font-semibold text-blue-900">Sản phẩm đã chọn:</p>
                <p class="text-sm text-blue-700">{{ getProductName(formData.productId) }}</p>
              </div>
              <button 
                type="button"
                @click="formData.productId = null"
                class="text-blue-600 hover:text-blue-800 text-sm"
              >
                Thay đổi
              </button>
            </div>
          </div>

          <div v-else>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Sản phẩm *</label>
            <select v-model.number="formData.productId" required class="input-field">
              <option :value="null">-- Chọn sản phẩm --</option>
              <option v-for="product in productStore.products" :key="product.id" :value="product.id">{{ product.name }}</option>
            </select>
          </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Màu sắc *</label>
            <select v-model.number="formData.colorId" required class="input-field">
              <option :value="null">-- Chọn màu --</option>
              <option v-for="color in productStore.colors" :key="color.id" :value="color.id">{{ color.name }}</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">
              Kích cỡ * 
              <span v-if="!editingVariant" class="text-xs text-gray-500 font-normal">(có thể chọn nhiều)</span>
            </label>
            <div v-if="editingVariant" class="space-y-2">
              <select v-model.number="formData.sizeId" required class="input-field">
                <option :value="null">-- Chọn size --</option>
                <option v-for="size in sortedSizes" :key="size.id" :value="size.id">{{ size.sizeName }}</option>
              </select>
              <p class="text-xs text-gray-500">Khi chỉnh sửa chỉ có thể chọn 1 size</p>
            </div>
            <div v-else class="border border-gray-300 rounded-lg p-3 max-h-48 overflow-y-auto bg-white">
              <div v-if="formData.sizeIds.length === 0" class="text-sm text-gray-500 text-center py-2">
                Chưa chọn size nào
              </div>
              <div v-else class="flex flex-wrap gap-2 mb-2">
                <span
                  v-for="sizeId in formData.sizeIds"
                  :key="sizeId"
                  class="inline-flex items-center px-3 py-1 bg-primary-100 text-primary-800 rounded-full text-sm font-medium"
                >
                  {{ getSizeName(sizeId) }}
                  <button
                    type="button"
                    @click="removeSize(sizeId)"
                    class="ml-2 text-primary-600 hover:text-primary-800"
                  >
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                    </svg>
                  </button>
                </span>
              </div>
              <div class="grid grid-cols-3 gap-2">
                <label
                  v-for="size in sortedSizes"
                  :key="size.id"
                  class="flex items-center space-x-2 p-2 border border-gray-300 rounded hover:bg-gray-50 cursor-pointer"
                  :class="{ 
                    'bg-primary-50 border-primary-500': formData.sizeIds.includes(size.id) && !isSizeDuplicate(size.id),
                    'bg-red-50 border-red-500': isSizeDuplicate(size.id)
                  }"
                >
                  <input
                    type="checkbox"
                    :value="size.id"
                    @change="handleSizeChange(size.id, $event)"
                    :checked="formData.sizeIds.includes(size.id)"
                    class="rounded border-gray-300 text-primary-600 focus:ring-primary-500"
                  />
                  <span class="text-sm font-medium">{{ size.sizeName }}</span>
                  <span v-if="isSizeDuplicate(size.id)" class="text-xs text-red-600">(trùng)</span>
                </label>
              </div>
              <p v-if="hasDuplicateSizes" class="text-xs text-red-600 mt-2">
                ⚠️ Có size bị trùng, vui lòng bỏ chọn size trùng trước khi lưu
              </p>
            </div>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Giá *</label>
            <input v-model.number="formData.price" type="number" required class="input-field" placeholder="3500000" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Tồn kho *</label>
            <input v-model.number="formData.quantity" type="number" required class="input-field" placeholder="100" />
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">
            Hình ảnh biến thể 
            <span class="text-xs text-gray-500 font-normal">(tối đa 4 ảnh)</span>
          </label>
          
          <!-- Upload from computer -->
          <div class="mb-3">
            <label class="block w-full">
              <input
                type="file"
                ref="fileInput"
                @change="handleFileUpload"
                multiple
                accept="image/*"
                :disabled="formData.imageIds.length >= 4 || uploadingImages"
                class="hidden"
              />
              <div class="flex items-center justify-center w-full h-32 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer hover:border-primary-500 transition-colors"
                   :class="{ 'opacity-50 cursor-not-allowed': formData.imageIds.length >= 4 || uploadingImages }"
                   @click="!uploadingImages && formData.imageIds.length < 4 && $refs.fileInput?.click()">
                <div class="text-center">
                  <svg class="mx-auto h-12 w-12 text-gray-400" stroke="currentColor" fill="none" viewBox="0 0 48 48">
                    <path d="M28 8H12a4 4 0 00-4 4v20m32-12v8m0 0v8a4 4 0 01-4 4H12a4 4 0 01-4-4v-4m32-4l-3.172-3.172a4 4 0 00-5.656 0L28 28M8 32l9.172-9.172a4 4 0 015.656 0L28 28m0 0l4 4m4-24h8m-4-4v8m-12 4h.02" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                  </svg>
                  <p class="mt-2 text-sm text-gray-600">
                    <span class="font-semibold">Click để tải ảnh</span> hoặc kéo thả
                  </p>
                  <p class="text-xs text-gray-500">PNG, JPG, GIF tối đa 5MB (tối đa 4 ảnh)</p>
                </div>
              </div>
            </label>
            <p v-if="uploadingImages" class="text-sm text-blue-600 mt-2">Đang tải ảnh lên...</p>
          </div>

          <!-- Select from existing images -->
          <div class="flex gap-2 mb-2">
            <select 
              v-model="selectedImageId" 
              class="input-field flex-1"
              @change="addImageToSelection"
              :disabled="formData.imageIds.length >= 4"
            >
              <option :value="null">-- Hoặc chọn hình ảnh có sẵn --</option>
              <option v-for="img in availableImages" :key="img.id" :value="img.id">
                #{{ img.id }} - {{ img.type }} ({{ img.url?.substring(0, 50) }}...)
              </option>
            </select>
          </div>
          
          <!-- Selected Images Preview -->
          <div v-if="formData.imageIds && formData.imageIds.length > 0" class="mt-3">
            <div class="flex flex-wrap gap-2">
              <div 
                v-for="(imageId, index) in formData.imageIds" 
                :key="imageId"
                class="relative group"
              >
                <img 
                  :src="getImageUrl(imageId)" 
                  alt="Preview" 
                  class="w-20 h-20 object-cover rounded border-2 border-gray-300 hover:border-primary-500 transition-colors" 
                />
                <button
                  type="button"
                  @click="removeImageFromSelection(index)"
                  class="absolute -top-2 -right-2 w-6 h-6 bg-red-500 text-white rounded-full flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity hover:bg-red-600"
                  title="Xóa hình ảnh"
                >
                  <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </button>
                <span class="absolute bottom-0 left-0 right-0 bg-black bg-opacity-50 text-white text-xs text-center py-0.5">
                  {{ index + 1 }}
                </span>
              </div>
            </div>
            <p class="text-xs mt-1" :class="formData.imageIds.length >= 4 ? 'text-green-600 font-semibold' : 'text-gray-500'">
              Đã chọn {{ formData.imageIds.length }}/4 hình ảnh
              <span v-if="formData.imageIds.length >= 4" class="ml-2">✓ Đã đủ</span>
            </p>
          </div>
          <p v-else class="text-sm text-gray-500 mt-1">Chưa có hình ảnh nào được chọn (0/4)</p>
          <p v-if="formData.imageIds.length >= 4" class="text-xs text-green-600 mt-1">
            ⚠️ Đã đạt giới hạn tối đa 4 ảnh cho mỗi biến thể
          </p>
        </div>
        </template>

      </form>
      <template #footer>
        <button @click="saveVariant" :disabled="saving" class="btn-primary">
          {{ saving ? 'Đang lưu...' : (editingVariant ? 'Cập nhật' : 'Lưu') }}
        </button>
        <button @click="closeModal" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Modal -->
    <Modal :show="showDeleteModal" title="Xác nhận xóa" @close="showDeleteModal = false">
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa biến thể này?</p>
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
import { productDetailApi, imageApi } from '../../services/api'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const productStore = useProductStore()

const variants = ref([])
const availableImages = ref([])
const loading = ref(false)
const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const editingVariant = ref(null)
const variantToDelete = ref(null)
const saving = ref(false)
const uploadingImages = ref(false)
const fileInput = ref(null)

const filterProduct = ref('')
const filterColor = ref('')
const filterSize = ref('')

const toast = reactive({ show: false, message: '', type: 'success' })

const selectedImageId = ref(null)

const formData = reactive({
  productId: null,
  colorId: null,
  sizeId: null, // For edit mode
  sizeIds: [], // For create mode - multiple sizes
  price: 0,
  quantity: 0,
  imageId: null, // Keep for backward compatibility
  imageIds: [] // New: array of image IDs
})


onMounted(async () => {
  await productStore.initialize()
  await fetchVariants()
  await fetchImages()
})

const fetchVariants = async () => {
  loading.value = true
  try {
    const data = await productDetailApi.getAll()
    variants.value = data
  } catch (error) {
    console.error('Error fetching variants:', error)
  } finally {
    loading.value = false
  }
}

const fetchImages = async () => {
  try {
    availableImages.value = await imageApi.getAll()
  } catch (error) {
    console.error('Error fetching images:', error)
  }
}

const filteredVariants = computed(() => {
  return variants.value.filter(v => {
    const matchProduct = !filterProduct.value || v.productId === filterProduct.value
    const matchColor = !filterColor.value || v.colorId === filterColor.value
    const matchSize = !filterSize.value || v.sizeId === filterSize.value
    return matchProduct && matchColor && matchSize
  })
})

// Sorted sizes: from high to low (or low to high based on numeric value)
const sortedSizes = computed(() => {
  return [...productStore.sizes].sort((a, b) => {
    // Try to parse as numbers
    const numA = parseFloat(a.sizeName) || 0
    const numB = parseFloat(b.sizeName) || 0
    
    // If both are numbers, sort descending (high to low: 45, 44, 43, ...)
    if (numA > 0 && numB > 0) {
      return numB - numA // Descending order
    }
    
    // If one is number and one is not, numbers first
    if (numA > 0 && numB === 0) return -1
    if (numA === 0 && numB > 0) return 1
    
    // Both are strings, sort alphabetically
    return (a.sizeName || '').localeCompare(b.sizeName || '')
  })
})

// Check if there are duplicate sizes in selected list
const hasDuplicateSizes = computed(() => {
  const uniqueSizes = new Set(formData.sizeIds)
  return formData.sizeIds.length !== uniqueSizes.size
})

// Check if a size is already selected (for visual feedback)
function isSizeDuplicate(sizeId) {
  // Count how many times this size appears in the array
  const count = formData.sizeIds.filter(id => id === sizeId).length
  return count > 1 // Return true if appears more than once
}

function getProductName(id) {
  return productStore.products.find(p => p.id === id)?.name || 'N/A'
}

function getColorName(id) {
  return productStore.colors.find(c => c.id === id)?.name || 'N/A'
}

function getColorValue(id) {
  const color = productStore.colors.find(c => c.id === id)
  if (!color?.name) return '#cccccc'
  
  // If name is a hex code (starts with #)
  if (color.name.startsWith('#')) {
    return color.name
  }
  
  // Try to parse as hex code
  if (/^#[0-9A-Fa-f]{6}$/.test(color.name)) {
    return color.name
  }
  
  // Map common color names to hex codes
  const colorMap = {
    'red': '#FF0000',
    'blue': '#0000FF',
    'green': '#008000',
    'yellow': '#FFFF00',
    'black': '#000000',
    'white': '#FFFFFF',
    'gray': '#808080',
    'grey': '#808080',
    'orange': '#FFA500',
    'purple': '#800080',
    'pink': '#FFC0CB',
    'brown': '#A52A2A',
    'đỏ': '#FF0000',
    'xanh': '#0000FF',
    'xanh lá': '#008000',
    'vàng': '#FFFF00',
    'đen': '#000000',
    'trắng': '#FFFFFF',
    'xám': '#808080',
    'cam': '#FFA500',
    'tím': '#800080',
    'hồng': '#FFC0CB',
    'nâu': '#A52A2A'
  }
  
  return colorMap[color.name.toLowerCase()] || '#cccccc'
}

function getSizeName(id) {
  return productStore.sizes.find(s => s.id === id)?.sizeName || 'N/A'
}

function handleSizeChange(sizeId, event) {
  if (event.target.checked) {
    // Prevent duplicate - only add if not already in array
    if (!formData.sizeIds.includes(sizeId)) {
      formData.sizeIds.push(sizeId)
    } else {
      // If already checked, prevent duplicate by unchecking
      event.target.checked = false
      toast.message = 'Size này đã được chọn rồi, không thể chọn trùng'
      toast.type = 'warning'
      toast.show = true
    }
  } else {
    // Remove if unchecked - remove all occurrences
    formData.sizeIds = formData.sizeIds.filter(id => id !== sizeId)
  }
}

function getVariantImages(variant) {
  // Return images array if exists, otherwise return empty array
  if (variant.images && Array.isArray(variant.images) && variant.images.length > 0) {
    return variant.images.filter(img => img && img.url) // Filter out invalid images
  }
  return []
}

function handleImageError(event) {
  // Replace broken image with placeholder
  event.target.src = 'https://via.placeholder.com/40x40?text=No+Img'
}

function removeSize(sizeId) {
  const index = formData.sizeIds.indexOf(sizeId)
  if (index > -1) {
    formData.sizeIds.splice(index, 1)
  }
}

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)
}

function addImageToSelection() {
  if (!selectedImageId.value) return
  
  // Check if already selected
  if (formData.imageIds.includes(selectedImageId.value)) {
    toast.message = 'Hình ảnh này đã được chọn rồi'
    toast.type = 'warning'
    toast.show = true
    selectedImageId.value = null
    return
  }
  
  // Check limit: maximum 4 images
  if (formData.imageIds.length >= 4) {
    toast.message = 'Mỗi biến thể chỉ được tối đa 4 hình ảnh'
    toast.type = 'error'
    toast.show = true
    selectedImageId.value = null
    return
  }
  
  formData.imageIds.push(selectedImageId.value)
  selectedImageId.value = null
}

function removeImageFromSelection(index) {
  formData.imageIds.splice(index, 1)
}

function editVariant(variant) {
  editingVariant.value = variant
  Object.assign(formData, {
    productId: variant.productId,
    colorId: variant.colorId,
    sizeId: variant.sizeId, // Single size for edit
    sizeIds: [], // Clear multiple sizes
    price: variant.price,
    quantity: variant.quantity,
    imageId: variant.imageId,
    imageIds: variant.images ? variant.images.map(img => img.id).filter(id => id) : []
  })
  selectedImageId.value = null
  showEditModal.value = true
}

function deleteVariant(variant) {
  variantToDelete.value = variant
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await productStore.deleteProductDetail(variantToDelete.value.id)
    await fetchVariants()
    toast.message = 'Đã xóa biến thể thành công'
    toast.type = 'success'
    toast.show = true
    showDeleteModal.value = false
  } catch (error) {
    toast.message = 'Không thể xóa biến thể'
    toast.type = 'error'
    toast.show = true
  }
}

async function saveVariant() {
  saving.value = true
  try {
    if (editingVariant.value) {
      // Edit mode: single variant
      if (!formData.sizeId) {
        toast.message = 'Vui lòng chọn size'
        toast.type = 'error'
        toast.show = true
        saving.value = false
        return
      }
      await productStore.updateProductDetail(editingVariant.value.id, formData)
      toast.message = 'Đã cập nhật biến thể thành công'
    } else {
      // Create mode: check if multiple sizes selected
      if (!formData.sizeIds || formData.sizeIds.length === 0) {
        toast.message = 'Vui lòng chọn ít nhất 1 size'
        toast.type = 'error'
        toast.show = true
        saving.value = false
        return
      }

      // Remove duplicates from sizeIds - CRITICAL: prevent duplicate sizes
      const uniqueSizeIds = [...new Set(formData.sizeIds)]
      if (uniqueSizeIds.length !== formData.sizeIds.length) {
        const duplicateCount = formData.sizeIds.length - uniqueSizeIds.length
        toast.message = `Đã loại bỏ ${duplicateCount} size trùng lặp. Vui lòng không chọn size trùng.`
        toast.type = 'error'
        toast.show = true
        formData.sizeIds = uniqueSizeIds
        saving.value = false
        return // Stop here to let user fix duplicates
      }

      // Check for existing variants before creating
      const existingVariants = []
      for (const sizeId of uniqueSizeIds) {
        const existing = variants.value.find(v => 
          v.productId === formData.productId && 
          v.colorId === formData.colorId && 
          v.sizeId === sizeId
        )
        if (existing) {
          existingVariants.push(sizeId)
        }
      }

      if (existingVariants.length > 0) {
        const sizeNames = existingVariants.map(id => getSizeName(id)).join(', ')
        toast.message = `Đã tồn tại variant với size: ${sizeNames}. Vui lòng chọn size khác.`
        toast.type = 'error'
        toast.show = true
        saving.value = false
        return
      }

      // Create multiple variants (one for each size)
      const createPromises = uniqueSizeIds.map(sizeId => {
        const variantData = {
          productId: formData.productId,
          colorId: formData.colorId,
          sizeId: sizeId,
          price: formData.price,
          quantity: formData.quantity,
          imageId: formData.imageId,
          imageIds: formData.imageIds
        }
        return productStore.createProductDetail(variantData)
      })

      try {
        await Promise.all(createPromises)
        toast.message = `Đã thêm ${uniqueSizeIds.length} biến thể thành công`
      } catch (error) {
        // Backend validation will catch duplicates
        throw error
      }
    }
    await fetchVariants()
    toast.type = 'success'
    toast.show = true
    closeModal()
  } catch (error) {
    console.error('Error saving variant:', error)
    toast.message = productStore.error || error.message || 'Không thể lưu biến thể'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingVariant.value = null
  selectedImageId.value = null
  if (fileInput.value) {
    fileInput.value.value = ''
  }
  Object.assign(formData, {
    productId: null,
    colorId: null,
    sizeId: null,
    sizeIds: [],
    price: 0,
    quantity: 0,
    imageId: null,
    imageIds: []
  })
}

function getImageUrl(imageId) {
  const image = availableImages.value.find(img => img.id === imageId)
  return image?.url || 'https://via.placeholder.com/400x400?text=No+Image'
}

async function handleFileUpload(event) {
  const files = Array.from(event.target.files || [])
  if (files.length === 0) return

  // Check total images limit (4 max)
  const remainingSlots = 4 - formData.imageIds.length
  if (files.length > remainingSlots) {
    toast.message = `Chỉ có thể tải thêm ${remainingSlots} ảnh nữa (tối đa 4 ảnh)`
    toast.type = 'error'
    toast.show = true
    event.target.value = '' // Reset input
    return
  }

  uploadingImages.value = true
  try {
    // Upload multiple images with type="Variant" and maxFiles=4
    const result = await imageApi.uploadMultiple(files, 'Variant', null, 4)
    
    if (result.images && result.images.length > 0) {
      // Add uploaded images to selection
      result.images.forEach(image => {
        if (!formData.imageIds.includes(image.id) && formData.imageIds.length < 4) {
          formData.imageIds.push(image.id)
        }
      })
      
      await fetchImages() // Refresh available images list
      
      toast.message = `Đã tải lên ${result.images.length} ảnh thành công`
      toast.type = 'success'
      toast.show = true
    } else {
      toast.message = 'Không thể tải ảnh lên'
      toast.type = 'error'
      toast.show = true
    }
    
    if (result.errors && result.errors.length > 0) {
      console.error('Upload errors:', result.errors)
      toast.message = `Tải lên thành công ${result.success} ảnh, ${result.failed} ảnh lỗi`
      toast.type = 'warning'
    }
  } catch (error) {
    console.error('Error uploading images:', error)
    toast.message = error.response?.data?.message || 'Không thể tải ảnh lên'
    toast.type = 'error'
    toast.show = true
  } finally {
    uploadingImages.value = false
    if (fileInput.value) {
      fileInput.value.value = '' // Reset input
    }
  }
}

function onProductSelected() {
  // Reset variant details when product changes
  formData.colorId = null
  formData.sizeIds = []
  formData.sizeId = null
}
</script>
