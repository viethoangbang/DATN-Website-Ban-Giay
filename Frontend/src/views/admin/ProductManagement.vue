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
        <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
      </select>
      <select v-model="filterStatus" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
        <option value="">Tất cả trạng thái</option>
        <option value="active">Đang bán</option>
        <option value="inactive">Ngừng bán</option>
      </select>
    </div>

    <!-- Products Table -->
    <div class="bg-white rounded-xl shadow-md overflow-hidden">
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-gray-50 border-b border-gray-200">
            <tr>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Sản phẩm
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Danh mục
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Giá
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Tồn kho
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Đánh giá
              </th>
              <th class="px-6 py-4 text-left text-xs font-semibold text-gray-600 uppercase tracking-wider">
                Trạng thái
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
                  <img :src="product.image" :alt="product.name" class="w-12 h-12 object-cover rounded-lg" />
                  <div>
                    <p class="font-semibold text-gray-900">{{ product.name }}</p>
                    <p class="text-sm text-gray-600">ID: #{{ product.id }}</p>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4">
                <span class="px-3 py-1 bg-primary-100 text-primary-800 rounded-full text-sm font-medium">
                  {{ product.category }}
                </span>
              </td>
              <td class="px-6 py-4 font-semibold text-gray-900">
                {{ formatPrice(product.price) }}
              </td>
              <td class="px-6 py-4">
                <span :class="product.stock > 20 ? 'text-green-600' : 'text-red-600'" class="font-semibold">
                  {{ product.stock }}
                </span>
              </td>
              <td class="px-6 py-4">
                <div class="flex items-center space-x-1">
                  <svg class="w-5 h-5 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                  </svg>
                  <span class="text-sm font-semibold text-gray-700">{{ product.rating }}</span>
                  <span class="text-sm text-gray-500">({{ product.reviews }})</span>
                </div>
              </td>
              <td class="px-6 py-4">
                <label class="relative inline-flex items-center cursor-pointer">
                  <input type="checkbox" :checked="product.active" class="sr-only peer" @change="toggleStatus(product)">
                  <div class="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-primary-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-primary-500"></div>
                </label>
              </td>
              <td class="px-6 py-4">
                <div class="flex justify-end space-x-2">
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
        </div>
        <div class="flex space-x-2">
          <button class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-white transition-colors">
            Trước
          </button>
          <button class="px-4 py-2 bg-primary-500 text-white rounded-lg">
            1
          </button>
          <button class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-white transition-colors">
            2
          </button>
          <button class="px-4 py-2 border border-gray-300 rounded-lg hover:bg-white transition-colors">
            Sau
          </button>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" title="Thêm sản phẩm mới" @close="closeModal">
      <form @submit.prevent="saveProduct" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Tên sản phẩm</label>
          <input v-model="formData.name" type="text" required class="input-field" />
        </div>
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Giá</label>
            <input v-model="formData.price" type="number" required class="input-field" />
          </div>
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Danh mục</label>
            <select v-model="formData.category" required class="input-field">
              <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
            </select>
          </div>
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mô tả</label>
          <textarea v-model="formData.description" rows="3" class="input-field"></textarea>
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">URL hình ảnh</label>
          <input v-model="formData.image" type="url" required class="input-field" />
        </div>
      </form>
      <template #footer>
        <button type="submit" @click="saveProduct" class="btn-primary">
          Lưu sản phẩm
        </button>
        <button @click="closeModal" class="btn-secondary">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Delete Confirmation Modal -->
    <Modal
      :show="showDeleteModal"
      title="Xác nhận xóa"
      confirm-text="Xóa"
      @close="showDeleteModal = false"
      @confirm="confirmDelete"
    >
      <p class="text-gray-600">
        Bạn có chắc chắn muốn xóa sản phẩm <strong>{{ productToDelete?.name }}</strong>? 
        Hành động này không thể hoàn tác.
      </p>
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
import { ref, computed, reactive } from 'vue'
import { useProductStore } from '../../stores/products'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const productStore = useProductStore()

const searchQuery = ref('')
const filterCategory = ref('')
const filterStatus = ref('')

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)

const productToDelete = ref(null)
const editingProduct = ref(null)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const formData = reactive({
  name: '',
  price: 0,
  category: 'Running',
  description: '',
  image: '',
  stock: 100,
  active: true
})

const categories = ['Running', 'Basketball', 'Casual', 'Sports']

// Mock products with additional fields
const products = ref([
  { id: 1, name: 'Nike Air Max 2024', price: 3500000, category: 'Running', stock: 45, rating: 4.8, reviews: 245, active: true, image: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=100', description: 'Great shoes' },
  { id: 2, name: 'Adidas Ultraboost', price: 4200000, category: 'Running', stock: 32, rating: 4.9, reviews: 312, active: true, image: 'https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=100', description: 'Comfortable' },
  { id: 3, name: 'Jordan Retro High', price: 5500000, category: 'Basketball', stock: 18, rating: 4.7, reviews: 189, active: true, image: 'https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=100', description: 'Classic style' },
  { id: 4, name: 'Puma RS-X', price: 2800000, category: 'Casual', stock: 8, rating: 4.5, reviews: 156, active: false, image: 'https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=100', description: 'Retro design' },
  { id: 5, name: 'New Balance 574', price: 3200000, category: 'Casual', stock: 56, rating: 4.6, reviews: 203, active: true, image: 'https://images.unsplash.com/photo-1539185441755-769473a23570?w=100', description: 'Classic comfort' },
])

const filteredProducts = computed(() => {
  return products.value.filter(product => {
    const matchSearch = product.name.toLowerCase().includes(searchQuery.value.toLowerCase())
    const matchCategory = !filterCategory.value || product.category === filterCategory.value
    const matchStatus = !filterStatus.value || (filterStatus.value === 'active' ? product.active : !product.active)
    return matchSearch && matchCategory && matchStatus
  })
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function toggleStatus(product) {
  product.active = !product.active
  toast.message = `Đã ${product.active ? 'kích hoạt' : 'vô hiệu hóa'} ${product.name}`
  toast.type = 'success'
  toast.show = true
}

function editProduct(product) {
  editingProduct.value = product
  Object.assign(formData, product)
  showEditModal.value = true
}

function deleteProduct(product) {
  productToDelete.value = product
  showDeleteModal.value = true
}

function confirmDelete() {
  const index = products.value.findIndex(p => p.id === productToDelete.value.id)
  if (index !== -1) {
    products.value.splice(index, 1)
    toast.message = 'Đã xóa sản phẩm thành công'
    toast.type = 'success'
    toast.show = true
  }
  showDeleteModal.value = false
  productToDelete.value = null
}

function saveProduct() {
  if (editingProduct.value) {
    Object.assign(editingProduct.value, formData)
    toast.message = 'Đã cập nhật sản phẩm thành công'
  } else {
    products.value.push({
      id: products.value.length + 1,
      ...formData,
      rating: 0,
      reviews: 0
    })
    toast.message = 'Đã thêm sản phẩm thành công'
  }
  toast.type = 'success'
  toast.show = true
  closeModal()
}

function closeModal() {
  showAddModal.value = false
  showEditModal.value = false
  editingProduct.value = null
  Object.assign(formData, {
    name: '',
    price: 0,
    category: 'Running',
    description: '',
    image: '',
    stock: 100,
    active: true
  })
}
</script>

