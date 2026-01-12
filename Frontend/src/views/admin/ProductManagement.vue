<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header Actions -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
      <div class="flex-1 max-w-md">
        <div class="relative">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Tìm kiếm sản phẩm..."
            class="input-field pl-10"
          />
          <svg class="w-5 h-5 text-gray-400 absolute left-3 top-1/2 transform -translate-y-1/2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
        </div>
      </div>
      <button
        @click="showAddModal = true"
        class="btn-primary flex items-center space-x-2"
      >
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm sản phẩm</span>
      </button>
    </div>

    <!-- Filters -->
    <div class="flex flex-wrap gap-3">
      <select v-model="filterCategory" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
        <option value="">Tất cả danh mục</option>
        <option v-for="cat in productStore.categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
      </select>
      <select v-model="filterStatus" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
        <option value="">Tất cả trạng thái</option>
        <option value="Active">Đang bán</option>
        <option value="Inactive">Ngừng bán</option>
      </select>
    </div>

    <!-- Loading State -->
    <div v-if="productStore.loading" class="flex justify-center items-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Products Table -->
    <div v-else class="bg-white rounded-xl shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-gray-50 border-b border-gray-200">
            <tr>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Sản phẩm
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Mã / Thương hiệu
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Danh mục
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Mô tả
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Hoạt động
              </th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Badge
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Mới
              </th>
              <th class="px-6 py-4 text-right text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Thao tác
              </th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200">
            <tr
              v-for="product in filteredProducts"
              :key="product.id"
              class="hover:bg-gray-50 transition-colors"
            >
              <td class="px-6 py-4">
                <div class="flex items-center space-x-4">
                  <div>
                    <p class="font-semibold text-gray-900">{{ product.name }}</p>
                    <p class="text-sm text-gray-600">ID: #{{ product.id }}</p>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4">
                <p class="text-sm font-medium text-gray-900">{{ product.code || 'N/A' }}</p>
                <p class="text-sm text-gray-600">{{ product.brand || '-' }}</p>
              </td>
              <td class="px-6 py-4">
                <span class="px-3 py-1 bg-primary-100 text-primary-800 rounded-full text-sm font-medium">
                  {{ getCategoryName(product.categoryId) }}
                </span>
              </td>
              <td class="px-6 py-4 text-sm text-gray-600">
                {{ product.description || 'Chưa có mô tả' }}
              </td>
              <td class="px-6 py-4">
                <label class="relative inline-flex items-center cursor-pointer">
                  <input 
                    type="checkbox" 
                    :checked="product.isActive" 
                    class="sr-only peer" 
                    @change="toggleIsActive(product)"
                  >
                  <div class="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-primary-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary-500"></div>
                </label>
              </td>
              <td class="px-6 py-4">
                <select 
                  :value="product.status || ''" 
                  @change="updateStatus(product, $event)"
                  class="px-3 py-1 text-sm border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500"
                >
                  <option value="">Không có</option>
                  <option value="New">Mới</option>
                  <option value="Hot">Hot</option>
                  <option value="Sell Well">Bán chạy</option>
                  <option value="Best Seller">Bán nhất</option>
                </select>
                <span v-if="product.status === 'New'" class="ml-2 text-xs text-gray-500">({{ newProductsCount }}/10)</span>
                <p v-if="product.status === 'New' && newProductsCount >= 10" class="text-xs text-red-500 mt-1">
                  Đã đủ 10 sản phẩm
                </p>
              </td>
              <td class="px-6 py-4">
                <div class="flex justify-end space-x-2">
                  <button
                    @click="addVariant(product)"
                    class="p-2 text-green-600 hover:bg-green-50 rounded-lg transition-colors"
                    title="Thêm biến thể"
                  >
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                    </svg>
                  </button>
                  <button
                    @click="editProduct(product)"
                    class="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                    title="Chỉnh sửa"
                  >
                    <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                    </svg>
                  </button>
                  <button
                    @click="deleteProduct(product)"
                    class="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                    title="Xóa"
                  >
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

      <!-- Pagination -->
      <div class="bg-gray-50 px-6 py-4 flex items-center justify-between border-t border-gray-200">
        <div class="text-sm text-gray-600">
          Hiển thị <span class="font-semibold">{{ filteredProducts.length }}</span> sản phẩm
          <span class="ml-4 text-green-600 font-semibold">
            ({{ newProductsCount }}/10 sản phẩm mới)
          </span>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingProduct ? 'Chỉnh sửa sản phẩm' : 'Thêm sản phẩm mới'" @close="closeModal">
      <form @submit.prevent="saveProduct" class="space-y-4">
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Tên sản phẩm *</label>
            <input v-model="formData.name" type="text" required class="input-field" placeholder="VD: Nike Air Max 2024" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Mã sản phẩm *</label>
            <input v-model="formData.code" type="text" required class="input-field" placeholder="VD: NIKE-AM-2024" />
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Thương hiệu</label>
            <input v-model="formData.brand" type="text" class="input-field" placeholder="VD: Nike" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Trọng lượng (gram) *</label>
            <input v-model.number="formData.weight" type="number" min="1" required class="input-field" placeholder="VD: 1000" />
            <p class="text-xs text-gray-500 mt-1">Trọng lượng sản phẩm tính bằng gram (mặc định: 1000g)</p>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Danh mục *</label>
            <div class="flex gap-2">
              <select v-model.number="formData.categoryId" required class="input-field flex-1">
                <option :value="null">-- Chọn danh mục --</option>
                <option v-for="cat in productStore.categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
              </select>
              <button 
                type="button"
                @click="showCategoryModal = true"
                class="px-4 py-2 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors"
                title="Thêm danh mục mới"
              >
                <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                </svg>
              </button>
            </div>
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mô tả</label>
          <textarea v-model="formData.description" rows="3" class="input-field" placeholder="Mô tả chi tiết về sản phẩm..."></textarea>
        </div>

        <!-- Status field removed - Backend will auto-set to "New" when creating new product -->
        <!-- IsActive checkbox is used for active/inactive status -->
      </form>
      <template #footer>
        <button type="submit" @click="saveProduct" :disabled="saving" class="btn-primary">
          <span v-if="!saving">{{ editingProduct ? 'Cập nhật' : 'Lưu sản phẩm' }}</span>
          <span v-else>Đang lưu...</span>
        </button>
        <button @click="closeModal" class="btn-secondary">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Category Modal -->
    <Modal :show="showCategoryModal" title="Thêm danh mục mới" @close="showCategoryModal = false">
      <form @submit.prevent="saveCategory" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Tên danh mục</label>
          <input v-model="categoryForm.name" type="text" required class="input-field" />
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mô tả</label>
          <textarea v-model="categoryForm.description" rows="2" class="input-field"></textarea>
        </div>
      </form>
      <template #footer>
        <button @click="saveCategory" class="btn-primary">Lưu danh mục</button>
        <button @click="showCategoryModal = false" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal
      :show="showDeleteModal"
      title="Xác nhận xóa"
      @close="showDeleteModal = false"
    >
      <p class="text-gray-600">
        Bạn có chắc chắn muốn xóa sản phẩm <strong>{{ productToDelete?.name }}</strong>? 
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

    <!-- Add Variant Modal -->
    <Modal :show="showVariantModal" title="Thêm biến thể mới" @close="closeVariantModal">
      <form @submit.prevent="saveVariant" class="space-y-4">
        <div class="mb-4 p-3 bg-blue-50 rounded-lg border border-blue-200">
          <p class="text-sm font-semibold text-blue-900">Sản phẩm:</p>
          <p class="text-sm text-blue-700">{{ selectedProduct?.name }}</p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Màu sắc *</label>
            <select v-model.number="variantForm.colorId" required class="input-field">
              <option :value="null">-- Chọn màu --</option>
              <option v-for="color in productStore.colors" :key="color.id" :value="color.id">{{ color.name }}</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">
              Kích cỡ * 
              <span class="text-xs text-gray-500 font-normal">(có thể chọn nhiều)</span>
            </label>
            <div class="border border-gray-300 rounded-lg p-3 max-h-48 overflow-y-auto bg-white">
              <div v-if="variantForm.sizeIds.length === 0" class="text-sm text-gray-500 text-center py-2">
                Chưa chọn size nào
              </div>
              <div v-else class="flex flex-wrap gap-2 mb-2">
                <span
                  v-for="sizeId in variantForm.sizeIds"
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
                    'bg-primary-50 border-primary-500': variantForm.sizeIds.includes(size.id)
                  }"
                >
                  <input
                    type="checkbox"
                    :value="size.id"
                    @change="handleSizeChange(size.id, $event)"
                    :checked="variantForm.sizeIds.includes(size.id)"
                    class="rounded border-gray-300 text-primary-600 focus:ring-primary-500"
                  />
                  <span class="text-sm font-medium">{{ size.sizeName }}</span>
                </label>
              </div>
            </div>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Giá *</label>
            <input v-model.number="variantForm.price" type="number" required class="input-field" placeholder="3500000" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Tồn kho *</label>
            <input v-model.number="variantForm.quantity" type="number" required class="input-field" placeholder="100" />
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
                :disabled="variantForm.imageIds.length >= 4 || uploadingImages"
                class="hidden"
              />
              <div class="flex items-center justify-center w-full h-32 border-2 border-dashed border-gray-300 rounded-lg cursor-pointer hover:border-primary-500 transition-colors"
                   :class="{ 'opacity-50 cursor-not-allowed': variantForm.imageIds.length >= 4 || uploadingImages }"
                   @click="!uploadingImages && variantForm.imageIds.length < 4 && $refs.fileInput?.click()">
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
              :disabled="variantForm.imageIds.length >= 4"
            >
              <option :value="null">-- Hoặc chọn hình ảnh có sẵn --</option>
              <option v-for="img in availableImages" :key="img.id" :value="img.id">
                #{{ img.id }} - {{ img.type }} ({{ img.url?.substring(0, 50) }}...)
              </option>
            </select>
          </div>
          
          <!-- Selected Images Preview -->
          <div v-if="variantForm.imageIds && variantForm.imageIds.length > 0" class="mt-3">
            <div class="flex flex-wrap gap-2">
              <div 
                v-for="(imageId, index) in variantForm.imageIds" 
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
            <p class="text-xs mt-1" :class="variantForm.imageIds.length >= 4 ? 'text-green-600 font-semibold' : 'text-gray-500'">
              Đã chọn {{ variantForm.imageIds.length }}/4 hình ảnh
              <span v-if="variantForm.imageIds.length >= 4" class="ml-2">✓ Đã đủ</span>
            </p>
          </div>
          <p v-else class="text-sm text-gray-500 mt-1">Chưa có hình ảnh nào được chọn (0/4)</p>
        </div>
      </form>
      <template #footer>
        <button @click="saveVariant" :disabled="savingVariant" class="btn-primary">
          {{ savingVariant ? 'Đang lưu...' : 'Lưu' }}
        </button>
        <button @click="closeVariantModal" class="btn-secondary">Hủy</button>
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
import { useProductStore } from '../../stores/products'
import { productDetailApi, imageApi } from '../../services/api'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const productStore = useProductStore()

const searchQuery = ref('')
const filterCategory = ref('')
const filterStatus = ref('')

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const showCategoryModal = ref(false)
const showVariantModal = ref(false)

const productToDelete = ref(null)
const editingProduct = ref(null)
const selectedProduct = ref(null)
const saving = ref(false)
const savingVariant = ref(false)
const uploadingImages = ref(false)
const fileInput = ref(null)
const availableImages = ref([])
const selectedImageId = ref(null)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const formData = reactive({
  name: '',
  code: '',
  brand: '',
  categoryId: null,
  description: '',
  isActive: true,
  status: null, // Will be set to "New" by backend if null
  weight: 1000, // Trọng lượng mặc định 1000 gram
  createBy: 'admin'
})

const categoryForm = reactive({
  name: '',
  description: '',
  status: 'Active'
})

const variantForm = reactive({
  productId: null,
  colorId: null,
  sizeIds: [],
  price: 0,
  quantity: 0,
  imageIds: []
})

// Initialize data
onMounted(async () => {
  await productStore.initialize()
  await fetchImages()
})

const fetchImages = async () => {
  try {
    availableImages.value = await imageApi.getAll()
  } catch (error) {
    console.error('Error fetching images:', error)
  }
}

const sortedSizes = computed(() => {
  return [...productStore.sizes].sort((a, b) => {
    const numA = parseFloat(a.sizeName) || 0
    const numB = parseFloat(b.sizeName) || 0
    if (numA > 0 && numB > 0) {
      return numB - numA
    }
    if (numA > 0 && numB === 0) return -1
    if (numA === 0 && numB > 0) return 1
    return (a.sizeName || '').localeCompare(b.sizeName || '')
  })
})

function getSizeName(id) {
  return productStore.sizes.find(s => s.id === id)?.sizeName || 'N/A'
}

function addVariant(product) {
  selectedProduct.value = product
  variantForm.productId = product.id
  variantForm.colorId = null
  variantForm.sizeIds = []
  variantForm.price = 0
  variantForm.quantity = 0
  variantForm.imageIds = []
  selectedImageId.value = null
  showVariantModal.value = true
}

function handleSizeChange(sizeId, event) {
  if (event.target.checked) {
    if (!variantForm.sizeIds.includes(sizeId)) {
      variantForm.sizeIds.push(sizeId)
    }
  } else {
    variantForm.sizeIds = variantForm.sizeIds.filter(id => id !== sizeId)
  }
}

function removeSize(sizeId) {
  variantForm.sizeIds = variantForm.sizeIds.filter(id => id !== sizeId)
}

async function handleFileUpload(event) {
  const files = Array.from(event.target.files || [])
  if (files.length === 0) return

  const remainingSlots = 4 - variantForm.imageIds.length
  if (files.length > remainingSlots) {
    toast.message = `Chỉ có thể tải thêm ${remainingSlots} ảnh nữa (tối đa 4 ảnh)`
    toast.type = 'error'
    toast.show = true
    event.target.value = ''
    return
  }

  uploadingImages.value = true
  try {
    const result = await imageApi.uploadMultiple(files, 'Variant', null, 4)
    
    if (result.images && result.images.length > 0) {
      result.images.forEach(image => {
        if (!variantForm.imageIds.includes(image.id) && variantForm.imageIds.length < 4) {
          variantForm.imageIds.push(image.id)
        }
      })
      
      await fetchImages()
      
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
      fileInput.value.value = ''
    }
  }
}

function addImageToSelection() {
  if (!selectedImageId.value) return
  
  if (variantForm.imageIds.includes(selectedImageId.value)) {
    toast.message = 'Hình ảnh này đã được chọn rồi'
    toast.type = 'warning'
    toast.show = true
    selectedImageId.value = null
    return
  }
  
  if (variantForm.imageIds.length >= 4) {
    toast.message = 'Mỗi biến thể chỉ được tối đa 4 hình ảnh'
    toast.type = 'error'
    toast.show = true
    selectedImageId.value = null
    return
  }
  
  variantForm.imageIds.push(selectedImageId.value)
  selectedImageId.value = null
}

function removeImageFromSelection(index) {
  variantForm.imageIds.splice(index, 1)
}

function getImageUrl(imageId) {
  const image = availableImages.value.find(img => img.id === imageId)
  return image?.url || 'https://via.placeholder.com/400x400?text=No+Image'
}

async function saveVariant() {
  if (!variantForm.sizeIds || variantForm.sizeIds.length === 0) {
    toast.message = 'Vui lòng chọn ít nhất 1 size'
    toast.type = 'error'
    toast.show = true
    return
  }

  const uniqueSizeIds = [...new Set(variantForm.sizeIds)]
  if (uniqueSizeIds.length !== variantForm.sizeIds.length) {
    toast.message = 'Có size trùng lặp, vui lòng kiểm tra lại'
    toast.type = 'error'
    toast.show = true
    return
  }

  savingVariant.value = true
  try {
    const createPromises = uniqueSizeIds.map(sizeId => {
      const variantData = {
        productId: variantForm.productId,
        colorId: variantForm.colorId,
        sizeId: sizeId,
        price: variantForm.price,
        quantity: variantForm.quantity,
        imageIds: variantForm.imageIds
      }
      return productStore.createProductDetail(variantData)
    })

    await Promise.all(createPromises)
    toast.message = `Đã thêm ${uniqueSizeIds.length} biến thể thành công`
    toast.type = 'success'
    toast.show = true
    closeVariantModal()
  } catch (error) {
    console.error('Error saving variant:', error)
    toast.message = productStore.error || error.message || 'Không thể lưu biến thể'
    toast.type = 'error'
    toast.show = true
  } finally {
    savingVariant.value = false
  }
}

function closeVariantModal() {
  showVariantModal.value = false
  selectedProduct.value = null
  selectedImageId.value = null
  if (fileInput.value) {
    fileInput.value.value = ''
  }
  Object.assign(variantForm, {
    productId: null,
    colorId: null,
    sizeIds: [],
    price: 0,
    quantity: 0,
    imageIds: []
  })
}

const filteredProducts = computed(() => {
  return productStore.products.filter(product => {
    const matchSearch = product.name.toLowerCase().includes(searchQuery.value.toLowerCase())
    const matchCategory = !filterCategory.value || product.categoryId === filterCategory.value
    const matchStatus = !filterStatus.value || product.status === filterStatus.value
    return matchSearch && matchCategory && matchStatus
  })
})

const newProductsCount = computed(() => {
  return productStore.products.filter(p => p.status === 'New').length
})

const canToggleNew = (product) => {
  const currentNewCount = newProductsCount.value
  if (product.status === 'New') {
    // Can always remove from New
    return true
  }
  // Can only add if less than 10
  return currentNewCount < 10
}

function getCategoryName(categoryId) {
  const category = productStore.categories.find(c => c.id === categoryId)
  return category?.name || 'Chưa phân loại'
}

async function toggleIsActive(product) {
  try {
    const newIsActive = !product.isActive
    await productStore.updateProduct(product.id, { ...product, isActive: newIsActive })
    toast.message = `Đã ${newIsActive ? 'kích hoạt' : 'vô hiệu hóa'} ${product.name}`
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    toast.message = 'Không thể cập nhật trạng thái'
    toast.type = 'error'
    toast.show = true
  }
}

async function updateStatus(product, event) {
  try {
    const newStatus = event?.target?.value || product.status || null
    const statusValue = newStatus === '' ? null : newStatus
    
    // Validate New status limit
    if (statusValue === 'New' && !canToggleNew(product)) {
      toast.message = 'Đã đạt giới hạn 10 sản phẩm mới. Vui lòng chọn badge khác.'
      toast.type = 'error'
      toast.show = true
      // Revert to previous status
      await productStore.fetchProducts()
      return
    }

    await productStore.updateProduct(product.id, { ...product, status: statusValue })
    toast.message = `Đã cập nhật badge cho ${product.name}`
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    toast.message = 'Không thể cập nhật badge'
    toast.type = 'error'
    toast.show = true
    // Revert on error
    await productStore.fetchProducts()
  }
}

function editProduct(product) {
  editingProduct.value = product
  Object.assign(formData, {
    name: product.name,
    code: product.code,
    brand: product.brand,
    categoryId: product.categoryId,
    description: product.description,
    isActive: product.isActive,
    status: product.status,
    weight: product.weight || 1000
  })
  showEditModal.value = true
}

function deleteProduct(product) {
  productToDelete.value = product
  showDeleteModal.value = true
}

async function confirmDelete() {
  try {
    await productStore.deleteProduct(productToDelete.value.id)
    toast.message = 'Đã xóa sản phẩm thành công'
    toast.type = 'success'
    toast.show = true
    showDeleteModal.value = false
    productToDelete.value = null
  } catch (error) {
    toast.message = 'Không thể xóa sản phẩm'
    toast.type = 'error'
    toast.show = true
  }
}

async function saveProduct() {
  saving.value = true
  try {
    if (editingProduct.value) {
      await productStore.updateProduct(editingProduct.value.id, formData)
      toast.message = 'Đã cập nhật sản phẩm thành công'
    } else {
      // When creating new product, don't send status - backend will auto-set to "New"
      const createData = { ...formData }
      createData.status = null // Let backend set to "New" automatically
      await productStore.createProduct(createData)
      toast.message = 'Đã thêm sản phẩm thành công'
    }
    toast.type = 'success'
    toast.show = true
    closeModal()
  } catch (error) {
    toast.message = productStore.error || 'Không thể lưu sản phẩm'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

async function saveCategory() {
  try {
    await productStore.createCategory(categoryForm)
    toast.message = 'Đã thêm danh mục thành công'
    toast.type = 'success'
    toast.show = true
    showCategoryModal.value = false
    categoryForm.name = ''
    categoryForm.description = ''
  } catch (error) {
    toast.message = productStore.error || 'Không thể thêm danh mục'
    toast.type = 'error'
    toast.show = true
  }
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingProduct.value = null
  Object.assign(formData, {
    name: '',
    code: '',
    brand: '',
    categoryId: null,
    description: '',
    isActive: true,
    status: null, // Backend will set to "New" automatically
    weight: 1000, // Reset to default weight
    createBy: 'admin'
  })
}
</script>
