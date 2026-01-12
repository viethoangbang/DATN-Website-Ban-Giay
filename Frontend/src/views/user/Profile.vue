<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="mb-8 animate-fade-in-up">
        <h1 class="text-3xl font-bold text-gray-900 mb-2">Tài khoản của tôi</h1>
        <p class="text-gray-600">Quản lý thông tin cá nhân</p>
      </div>

      <div v-if="loading" class="flex justify-center py-12">
        <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      </div>

      <div v-else class="grid grid-cols-1 lg:grid-cols-4 gap-8">
        <!-- Sidebar -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 animate-fade-in-up">
            <div class="flex flex-col items-center mb-6">
              <div class="w-24 h-24 rounded-full flex items-center justify-center mb-4 overflow-hidden border-2 border-gray-200">
                <img
                  v-if="userData?.avatarUrl"
                  :src="userData.avatarUrl"
                  :alt="userData?.fullName || userData?.email || 'User'"
                  class="w-full h-full object-cover"
                  @error="handleAvatarError"
                />
                <div
                  v-else
                  class="w-full h-full bg-primary-500 flex items-center justify-center"
                >
                  <span class="text-white text-3xl font-bold">{{ userInitial }}</span>
                </div>
              </div>
              <h3 class="font-bold text-gray-900 text-lg">{{ userData?.fullName || 'Người dùng' }}</h3>
              <p class="text-gray-600 text-sm">{{ userData?.email || '' }}</p>
            </div>

            <nav class="space-y-2">
              <button
                v-for="tab in tabs"
                :key="tab.id"
                @click="activeTab = tab.id"
                :class="[
                  'w-full flex items-center space-x-3 px-4 py-3 rounded-lg transition-all',
                  activeTab === tab.id
                    ? 'bg-primary-500 text-white'
                    : 'text-gray-700 hover:bg-gray-100'
                ]"
              >
                <component :is="tab.icon" class="w-5 h-5" />
                <span class="font-medium">{{ tab.name }}</span>
              </button>
            </nav>
          </div>
        </div>

        <!-- Content -->
        <div class="lg:col-span-3">
          <!-- Profile Info -->
          <div v-if="activeTab === 'profile'" class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up">
            <div class="flex justify-between items-center mb-6">
              <h2 class="text-xl font-bold text-gray-900">Thông tin cá nhân</h2>
              <button
                v-if="!editMode"
                @click="editMode = true"
                class="btn-primary"
              >
                Chỉnh sửa
              </button>
            </div>

            <form @submit.prevent="saveProfile" class="space-y-6">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Họ và tên *</label>
                  <input
                    v-model="profileForm.fullName"
                    type="text"
                    required
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
                    placeholder="Nhập họ và tên"
                  />
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Email</label>
                  <input
                    v-model="profileForm.email"
                    type="email"
                    disabled
                    class="input-field bg-gray-100"
                  />
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Số điện thoại</label>
                  <input
                    v-model="profileForm.phoneNumber"
                    type="tel"
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
                    placeholder="Nhập số điện thoại"
                  />
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Giới tính</label>
                  <select
                    v-model="profileForm.sex"
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
                  >
                    <option value="">-- Chọn giới tính --</option>
                    <option value="Male">Nam</option>
                    <option value="Female">Nữ</option>
                    <option value="Other">Khác</option>
                  </select>
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Năm sinh</label>
                  <input
                    v-model.number="profileForm.birthYear"
                    type="number"
                    min="1900"
                    max="2100"
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
                    placeholder="VD: 1990"
                  />
                </div>
                <div class="md:col-span-2">
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Ảnh đại diện</label>
                  <div class="flex flex-col sm:flex-row gap-4">
                    <!-- Avatar Preview -->
                    <div class="flex-shrink-0">
                      <div class="w-32 h-32 rounded-full overflow-hidden border-2 border-gray-200 flex items-center justify-center bg-gray-100">
                        <img
                          v-if="avatarPreview || profileForm.avatarUrl"
                          :src="avatarPreview || profileForm.avatarUrl"
                          alt="Avatar preview"
                          class="w-full h-full object-cover"
                          @error="handleAvatarPreviewError"
                        />
                        <div v-else class="w-full h-full bg-primary-500 flex items-center justify-center">
                          <span class="text-white text-4xl font-bold">{{ userInitial }}</span>
                        </div>
                      </div>
                    </div>
                    
                    <!-- Upload Options -->
                    <div class="flex-1 space-y-3">
                      <div v-if="editMode">
                        <label class="block text-sm font-medium text-gray-700 mb-2">Tải ảnh từ máy</label>
                        <div class="flex items-center space-x-3">
                          <input
                            ref="fileInput"
                            type="file"
                            accept="image/jpeg,image/jpg,image/png,image/gif,image/webp"
                            @change="handleFileSelect"
                            :disabled="uploadingAvatar"
                            class="hidden"
                          />
                          <button
                            type="button"
                            @click="$refs.fileInput.click()"
                            :disabled="uploadingAvatar"
                            class="btn-secondary text-sm"
                          >
                            {{ uploadingAvatar ? 'Đang tải...' : 'Chọn ảnh' }}
                          </button>
                          <button
                            v-if="selectedFile"
                            type="button"
                            @click="uploadAvatar"
                            :disabled="uploadingAvatar"
                            class="btn-primary text-sm"
                          >
                            {{ uploadingAvatar ? 'Đang upload...' : 'Upload' }}
                          </button>
                          <button
                            v-if="selectedFile"
                            type="button"
                            @click="clearSelectedFile"
                            class="text-sm text-gray-600 hover:text-gray-800"
                          >
                            Hủy
                          </button>
                        </div>
                        <p v-if="selectedFile" class="text-xs text-gray-500 mt-1">
                          {{ selectedFile.name }} ({{ formatFileSize(selectedFile.size) }})
                        </p>
                        <p v-if="avatarError" class="text-xs text-red-600 mt-1">{{ avatarError }}</p>
                        <p class="text-xs text-gray-500 mt-1">Định dạng: JPG, PNG, GIF, WEBP (Tối đa 5MB)</p>
                      </div>
                      <div v-else class="text-sm text-gray-500">
                        Nhấn "Chỉnh sửa" để thay đổi ảnh đại diện
                      </div>
                      
                      <div class="border-t pt-3">
                        <label class="block text-sm font-medium text-gray-700 mb-2">Hoặc nhập URL</label>
                        <input
                          v-model="profileForm.avatarUrl"
                          type="url"
                          :disabled="!editMode"
                          class="input-field disabled:bg-gray-100 text-sm"
                          placeholder="https://example.com/avatar.jpg"
                          @input="avatarPreview = null"
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div v-if="editMode" class="flex space-x-3">
                <button type="submit" :disabled="saving" class="btn-primary">
                  {{ saving ? 'Đang lưu...' : 'Lưu thay đổi' }}
                </button>
                <button type="button" @click="cancelEdit" class="btn-secondary">Hủy</button>
              </div>
            </form>
          </div>


          <!-- Address Book -->
          <div v-if="activeTab === 'addresses'" class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up">
            <div class="flex justify-between items-center mb-6">
              <h2 class="text-xl font-bold text-gray-900">Sổ địa chỉ</h2>
              <button @click="showAddressModal = true" class="btn-primary">
                Thêm địa chỉ mới
              </button>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div
                v-for="address in addresses"
                :key="address.id"
                class="border-2 border-gray-200 rounded-lg p-4 hover:border-primary-500 transition-colors"
              >
                <div class="flex justify-between items-start mb-2">
                  <h4 class="font-bold text-gray-900">{{ address.receiverName || 'Địa chỉ' }}</h4>
                </div>
                <p v-if="address.receiverPhone" class="text-gray-600 text-sm mb-1">{{ address.receiverPhone }}</p>
                <p v-if="address.receiverEmail" class="text-gray-600 text-sm mb-1">{{ address.receiverEmail }}</p>
                <p class="text-gray-600 text-sm">
                  {{ formatAddress(address) }}
                </p>
                <div class="mt-4 flex space-x-2">
                  <button @click="editAddress(address)" class="text-primary-600 hover:text-primary-700 text-sm font-semibold">Sửa</button>
                  <button @click="deleteAddress(address)" class="text-red-600 hover:text-red-700 text-sm font-semibold">Xóa</button>
                </div>
              </div>
            </div>

            <EmptyState
              v-if="addresses.length === 0"
              title="Chưa có địa chỉ"
              description="Thêm địa chỉ để dễ dàng đặt hàng"
              action-text="Thêm địa chỉ"
              @action="showAddressModal = true"
            />
          </div>
        </div>
      </div>
    </div>

    <Footer />

    <!-- Delete Address Confirmation Modal -->
    <Modal :show="showDeleteAddressModal" title="Xác nhận xóa địa chỉ" @close="showDeleteAddressModal = false">
      <div class="space-y-4">
        <p class="text-gray-700">Bạn có chắc chắn muốn xóa địa chỉ này?</p>
        <div v-if="addressToDelete" class="bg-gray-50 p-4 rounded-lg border border-gray-200">
          <p class="font-semibold text-gray-900">{{ addressToDelete.receiverName }}</p>
          <p class="text-sm text-gray-600">{{ addressToDelete.receiverPhone }}</p>
          <p class="text-sm text-gray-600">{{ formatAddress(addressToDelete) }}</p>
        </div>
        <p class="text-sm text-red-600 font-semibold">Hành động này không thể hoàn tác!</p>
      </div>
      <template #footer>
        <button @click="confirmDeleteAddress" :disabled="deletingAddress" class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed">
          {{ deletingAddress ? 'Đang xóa...' : 'Xóa địa chỉ' }}
        </button>
        <button @click="showDeleteAddressModal = false" :disabled="deletingAddress" class="btn-secondary">
          Hủy
        </button>
      </template>
    </Modal>

    <!-- Address Modal -->
    <Modal :show="showAddressModal" :title="editingAddress ? 'Chỉnh sửa địa chỉ' : 'Thêm địa chỉ mới'" @close="closeAddressModal">
      <form @submit.prevent="saveAddress" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Tên người nhận *</label>
          <input
            v-model="addressForm.receiverName"
            type="text"
            required
            class="input-field"
            placeholder="Nhập tên người nhận"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Số điện thoại *</label>
          <input
            v-model="addressForm.receiverPhone"
            type="tel"
            required
            class="input-field"
            placeholder="Nhập số điện thoại"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Email</label>
          <input
            v-model="addressForm.receiverEmail"
            type="email"
            class="input-field"
            placeholder="Nhập email (tùy chọn)"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Tỉnh/Thành phố *</label>
          <div class="relative" ref="provinceDropdownRef">
            <input
              v-model="provinceSearchQuery"
              @input="filterProvinces"
              @focus="showProvinceDropdown = true"
              type="text"
              :placeholder="addressForm.provinceName || 'Tìm kiếm hoặc chọn Tỉnh/Thành phố...'"
              class="input-field w-full"
              :disabled="loadingProvinces"
            />
            <div
              v-if="showProvinceDropdown && filteredProvinces.length > 0"
              class="absolute z-50 w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg max-h-60 overflow-y-auto"
            >
              <div
                v-for="province in filteredProvinces"
                :key="province.code"
                @click="selectProvince(province)"
                class="px-4 py-2 hover:bg-primary-50 cursor-pointer border-b border-gray-100 last:border-b-0"
                :class="{ 'bg-primary-100': addressForm.provinceCode === province.code }"
              >
                {{ province.name }}
              </div>
            </div>
            <div
              v-if="showProvinceDropdown && filteredProvinces.length === 0 && !loadingProvinces"
              class="absolute z-50 w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg p-4 text-center text-gray-500"
            >
              Không tìm thấy tỉnh/thành phố nào
            </div>
          </div>
          <input
            v-model="addressForm.provinceName"
            type="hidden"
          />
          <input
            v-model="addressForm.provinceCode"
            type="hidden"
          />
          <p v-if="loadingProvinces" class="text-xs text-gray-500 mt-1">Đang tải {{ provinces.length }} tỉnh/thành phố...</p>
          <p v-else-if="provinces.length > 0" class="text-xs text-gray-500 mt-1">
            Gõ để tìm kiếm.
          </p>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Quận/Huyện *</label>
          <div class="relative" ref="districtDropdownRef">
            <input
              v-model="districtSearchQuery"
              @input="filterDistricts"
              @focus="showDistrictDropdown = true"
              type="text"
              :placeholder="addressForm.districtName || 'Tìm kiếm hoặc chọn Quận/Huyện...'"
              class="input-field w-full"
              :disabled="!addressForm.provinceCode || loadingDistricts"
            />
            <div
              v-if="showDistrictDropdown && filteredDistricts.length > 0"
              class="absolute z-[60] w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg max-h-60 overflow-y-auto"
            >
              <div
                v-for="district in filteredDistricts"
                :key="district.code"
                @click="selectDistrict(district)"
                class="px-4 py-2 hover:bg-primary-50 cursor-pointer border-b border-gray-100 last:border-b-0"
                :class="{ 'bg-primary-100': addressForm.districtCode === district.code }"
              >
                {{ district.name }}
              </div>
            </div>
            <div
              v-if="showDistrictDropdown && filteredDistricts.length === 0 && !loadingDistricts"
              class="absolute z-[60] w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg p-4 text-center text-gray-500"
            >
              Không tìm thấy quận/huyện nào
            </div>
          </div>
          <input
            v-model="addressForm.districtName"
            type="hidden"
          />
          <input
            v-model="addressForm.districtCode"
            type="hidden"
          />
          <p v-if="loadingDistricts" class="text-xs text-gray-500 mt-1">Đang tải...</p>
          <p v-if="!addressForm.provinceCode" class="text-xs text-gray-500 mt-1">Vui lòng chọn Tỉnh/Thành phố trước</p>
        </div>

        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Phường/Xã *</label>
          <div class="relative" ref="wardDropdownRef">
            <input
              v-model="wardSearchQuery"
              @input="filterWards"
              @focus="showWardDropdown = true"
              type="text"
              :placeholder="addressForm.wardName || 'Tìm kiếm hoặc chọn Phường/Xã...'"
              class="input-field w-full"
              :disabled="!addressForm.districtCode || loadingWards"
            />
            <div
              v-if="showWardDropdown && filteredWards.length > 0"
              class="absolute z-50 w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg max-h-60 overflow-y-auto"
            >
              <div
                v-for="ward in filteredWards"
                :key="ward.code"
                @click="selectWard(ward)"
                class="px-4 py-2 hover:bg-primary-50 cursor-pointer border-b border-gray-100 last:border-b-0"
                :class="{ 'bg-primary-100': addressForm.wardCode === ward.code }"
              >
                {{ ward.name }}
              </div>
            </div>
            <div
              v-if="showWardDropdown && filteredWards.length === 0 && !loadingWards"
              class="absolute z-[60] w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg p-4 text-center text-gray-500"
            >
              Không tìm thấy phường/xã nào
            </div>
          </div>
          <input
            v-model="addressForm.wardName"
            type="hidden"
          />
          <input
            v-model="addressForm.wardCode"
            type="hidden"
          />
          <p v-if="loadingWards" class="text-xs text-gray-500 mt-1">Đang tải...</p>
          <p v-if="!addressForm.districtCode" class="text-xs text-gray-500 mt-1">Vui lòng chọn Quận/Huyện trước</p>
        </div>

      </form>
      <template #footer>
        <button @click="saveAddress" :disabled="savingAddress" class="btn-primary">
          {{ savingAddress ? 'Đang lưu...' : (editingAddress ? 'Cập nhật' : 'Thêm địa chỉ') }}
        </button>
        <button @click="closeAddressModal" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted, onUnmounted, watch, h } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { accountApi, orderApi, addressApi, imageApi } from '../../services/api'
import ghnLocationApi from '../../services/ghnLocationApi'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'
import EmptyState from '../../components/common/EmptyState.vue'
import Modal from '../../components/common/Modal.vue'

const router = useRouter()
const authStore = useAuthStore()

const loading = ref(true)
const activeTab = ref('profile')
const editMode = ref(false)
const showAddressModal = ref(false)
const saving = ref(false)
const savingAddress = ref(false)
const showDeleteAddressModal = ref(false)
const addressToDelete = ref(null)
const deletingAddress = ref(false)
const editingAddress = ref(null)
const uploadingAvatar = ref(false)
const selectedFile = ref(null)
const avatarPreview = ref(null)
const avatarError = ref('')
const fileInput = ref(null)

const userData = ref(null)
const orders = ref([])
const addresses = ref([])

const provinces = ref([])
const filteredProvinces = ref([])
const districts = ref([])
const filteredDistricts = ref([])
const wards = ref([])
const filteredWards = ref([])
const loadingProvinces = ref(false)
const loadingDistricts = ref(false)
const loadingWards = ref(false)
const provinceSearchQuery = ref('')
const districtSearchQuery = ref('')
const wardSearchQuery = ref('')
const showProvinceDropdown = ref(false)
const showDistrictDropdown = ref(false)
const showWardDropdown = ref(false)
const provinceDropdownRef = ref(null)
const districtDropdownRef = ref(null)
const wardDropdownRef = ref(null)

const addressForm = reactive({
  receiverName: '',
  receiverPhone: '',
  receiverEmail: '',
  provinceId: null,
  provinceCode: '',
  provinceName: '',
  districtId: null,
  districtCode: '',
  districtName: '',
  wardCode: '',
  wardName: ''
})

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

// Helper function to show toast
function showToast(message, type = 'success') {
  toast.message = message
  toast.type = type
  toast.show = true
}

// Icons
const UserIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z' })
])

const OrderIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z' })
])

const LocationIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z' })
])

const tabs = [
  { id: 'profile', name: 'Thông tin', icon: UserIcon },
  { id: 'addresses', name: 'Địa chỉ', icon: LocationIcon }
]

const profileForm = reactive({
  fullName: '',
  email: '',
  phoneNumber: '',
  sex: '',
  birthYear: null,
  avatarUrl: ''
})

const userInitial = computed(() => {
  if (userData.value?.fullName) {
    const names = userData.value.fullName.split(' ')
    return names.length > 1
      ? `${names[0][0]}${names[names.length - 1][0]}`.toUpperCase()
      : names[0][0].toUpperCase()
  }
  if (userData.value?.email) {
    return userData.value.email.substring(0, 2).toUpperCase()
  }
  return 'U'
})

function formatPrice(price) {
  if (!price && price !== 0) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function formatDate(date) {
  if (!date) return 'N/A'
  return new Date(date).toLocaleDateString('vi-VN', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

function formatAddress(address) {
  const parts = []
  if (address.wardName) parts.push(address.wardName)
  if (address.districtName) parts.push(address.districtName)
  if (address.provinceName) parts.push(address.provinceName)
  return parts.length > 0 ? parts.join(', ') : 'Chưa có địa chỉ'
}

function getStatusClass(status) {
  const classes = {
    'Pending': 'bg-yellow-100 text-yellow-800',
    'Processing': 'bg-blue-100 text-blue-800',
    'Shipping': 'bg-purple-100 text-purple-800',
    'Delivered': 'bg-green-100 text-green-800',
    'Cancelled': 'bg-red-100 text-red-800'
  }
  return classes[status] || 'bg-gray-100 text-gray-800'
}

function getStatusLabel(status) {
  const labels = {
    'Pending': 'Đang chờ',
    'Processing': 'Đang xử lý',
    'Shipping': 'Đang giao',
    'Delivered': 'Đã giao',
    'Cancelled': 'Đã hủy'
  }
  return labels[status] || status
}

function handleAvatarError(event) {
  event.target.style.display = 'none'
  const parent = event.target.parentElement
  if (parent) {
    const placeholder = parent.querySelector('.avatar-placeholder') || document.createElement('div')
    placeholder.className = 'avatar-placeholder w-full h-full bg-primary-500 flex items-center justify-center'
    placeholder.innerHTML = `<span class="text-white text-3xl font-bold">${userInitial.value}</span>`
    if (!parent.querySelector('.avatar-placeholder')) {
      parent.appendChild(placeholder)
    }
  }
}

function handleAvatarPreviewError(event) {
  event.target.style.display = 'none'
}

function handleFileSelect(event) {
  const file = event.target.files?.[0]
  if (!file) {
    selectedFile.value = null
    avatarPreview.value = null
    avatarError.value = ''
    return
  }

  avatarError.value = ''

  // Validate file type
  const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp']
  if (!allowedTypes.includes(file.type)) {
    avatarError.value = 'Định dạng file không hợp lệ. Chỉ chấp nhận: JPG, PNG, GIF, WEBP'
    selectedFile.value = null
    return
  }

  // Validate file size (5MB)
  const maxSize = 5 * 1024 * 1024
  if (file.size > maxSize) {
    avatarError.value = 'Kích thước file vượt quá 5MB'
    selectedFile.value = null
    return
  }

  selectedFile.value = file

  // Create preview
  const reader = new FileReader()
  reader.onload = (e) => {
    avatarPreview.value = e.target.result
  }
  reader.readAsDataURL(file)
}

function clearSelectedFile() {
  selectedFile.value = null
  avatarPreview.value = null
  avatarError.value = ''
  if (fileInput.value) {
    fileInput.value.value = ''
  }
}

function formatFileSize(bytes) {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i]
}

async function uploadAvatar() {
  if (!selectedFile.value) {
    avatarError.value = 'Vui lòng chọn file'
    return
  }

  uploadingAvatar.value = true
  avatarError.value = ''

  try {
    // Upload to Cloudinary via Image API
    const result = await imageApi.upload(selectedFile.value, 'Avatar', 'avatars')
    
    // Update avatarUrl in form
    profileForm.avatarUrl = result.url
    
    // Clear selected file and preview
    clearSelectedFile()
    
    toast.message = 'Đã upload ảnh đại diện thành công'
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    console.error('Error uploading avatar:', error)
    avatarError.value = error.response?.data?.message || 'Không thể upload ảnh'
    toast.message = 'Không thể upload ảnh đại diện'
    toast.type = 'error'
    toast.show = true
  } finally {
    uploadingAvatar.value = false
  }
}

async function loadUserData() {
  if (!authStore.user?.id) {
    router.push('/login')
    return
  }

  try {
    loading.value = true
    const [user, userOrders, userAddresses] = await Promise.all([
      accountApi.getMe(),
      orderApi.getMyOrders().catch(() => []),
      addressApi.getByAccountId().catch(() => [])
    ])

    userData.value = user
    orders.value = userOrders || []
    addresses.value = userAddresses || []

    // Populate form
    profileForm.fullName = user.fullName || ''
    profileForm.email = user.email || ''
    profileForm.phoneNumber = user.phoneNumber || ''
    profileForm.sex = user.sex || ''
    profileForm.birthYear = user.birthYear || null
    profileForm.avatarUrl = user.avatarUrl || ''
  } catch (error) {
    console.error('Error loading user data:', error)
    toast.message = 'Không thể tải thông tin tài khoản'
    toast.type = 'error'
    toast.show = true
  } finally {
    loading.value = false
  }
}

async function saveProfile() {
  if (!authStore.user?.id) return

  saving.value = true
  try {
    await accountApi.update(authStore.user.id, {
      fullName: profileForm.fullName,
      phoneNumber: profileForm.phoneNumber,
      sex: profileForm.sex || null,
      birthYear: profileForm.birthYear || null,
      avatarUrl: profileForm.avatarUrl || null
    })

    // Reload user data
    await loadUserData()

    editMode.value = false
    toast.message = 'Đã cập nhật thông tin thành công'
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    console.error('Error saving profile:', error)
    toast.message = error.response?.data?.message || 'Không thể cập nhật thông tin'
    toast.type = 'error'
    toast.show = true
  } finally {
    saving.value = false
  }
}

function cancelEdit() {
  editMode.value = false
  // Reset form to original data
  if (userData.value) {
    profileForm.fullName = userData.value.fullName || ''
    profileForm.phoneNumber = userData.value.phoneNumber || ''
    profileForm.sex = userData.value.sex || ''
    profileForm.birthYear = userData.value.birthYear || null
    profileForm.avatarUrl = userData.value.avatarUrl || ''
  }
}

async function loadProvinces() {
  if (provinces.value.length > 0) {
    filterProvinces() // Update filtered list
    return // Already loaded
  }
  
  loadingProvinces.value = true
  try {
    // Use GHN API instead of old Location API
    const data = await ghnLocationApi.getProvinces()
    
    // GHN API returns array with structure: { provinceID, provinceName }
    // Map to match expected structure: { id, code, name }
    provinces.value = (data || []).map(p => ({
      id: p.provinceID,
      code: p.provinceID.toString(),
      name: p.provinceName
    }))
    
    // Sort provinces by name
    provinces.value.sort((a, b) => {
      const nameA = (a.name || '').toLowerCase()
      const nameB = (b.name || '').toLowerCase()
      return nameA.localeCompare(nameB, 'vi')
    })
    
    // Initialize filtered list
    filteredProvinces.value = provinces.value
    
    if (provinces.value.length === 0) {
      toast.message = 'Không có dữ liệu tỉnh/thành phố'
      toast.type = 'warning'
      toast.show = true
    } else {
      console.log(`Loaded ${provinces.value.length} provinces`)
    }
  } catch (error) {
    console.error('Error loading provinces:', error)
    toast.message = 'Không thể tải danh sách tỉnh/thành phố: ' + (error.response?.data?.message || error.message || 'Lỗi không xác định')
    toast.type = 'error'
    toast.show = true
  } finally {
    loadingProvinces.value = false
  }
}

function filterProvinces() {
  const query = provinceSearchQuery.value.toLowerCase().trim()
  if (!query) {
    filteredProvinces.value = provinces.value
  } else {
    filteredProvinces.value = provinces.value.filter(p => {
      const name = (p.name || '').toLowerCase()
      const code = (p.code || '').toLowerCase()
      return name.includes(query) || code.includes(query)
    })
  }
}

function selectProvince(province) {
  addressForm.provinceId = province.id // GHN Province ID
  addressForm.provinceCode = province.code
  addressForm.provinceName = province.name
  provinceSearchQuery.value = province.name
  showProvinceDropdown.value = false
  onProvinceChange()
}

async function onProvinceChange() {
  // Reset districts and wards when province changes
  addressForm.districtId = null
  addressForm.districtCode = ''
  addressForm.districtName = ''
  addressForm.wardCode = ''
  addressForm.wardName = ''
  districts.value = []
  filteredDistricts.value = []
  wards.value = []
  filteredWards.value = []
  districtSearchQuery.value = ''
  wardSearchQuery.value = ''
  showDistrictDropdown.value = false
  showWardDropdown.value = false
  
  if (!addressForm.provinceId) {
    addressForm.provinceName = ''
    return
  }
  
  // Load districts using GHN API
  loadingDistricts.value = true
  try {
    const data = await ghnLocationApi.getDistricts(addressForm.provinceId)
    
    // Map GHN data to match existing structure
    districts.value = (data || []).map(d => ({
      id: d.districtID,
      code: d.districtID.toString(),
      name: d.districtName
    }))
    
    // Sort districts by name
    districts.value.sort((a, b) => {
      const nameA = (a.name || '').toLowerCase()
      const nameB = (b.name || '').toLowerCase()
      return nameA.localeCompare(nameB)
    })
    
    filteredDistricts.value = districts.value
    
    if (districts.value.length === 0) {
      showToast('Không có dữ liệu quận/huyện cho tỉnh/thành phố này', 'warning')
    }
  } catch (error) {
    console.error('Error loading districts:', error)
    showToast('Không thể tải danh sách quận/huyện', 'error')
  } finally {
    loadingDistricts.value = false
  }
}

function filterDistricts() {
  const query = districtSearchQuery.value.toLowerCase().trim()
  if (!query) {
    filteredDistricts.value = districts.value
  } else {
    filteredDistricts.value = districts.value.filter(d => {
      const name = (d.name || '').toLowerCase()
      const code = (d.code || '').toLowerCase()
      return name.includes(query) || code.includes(query)
    })
  }
}

function selectDistrict(district) {
  addressForm.districtId = district.id
  addressForm.districtCode = district.code
  addressForm.districtName = district.name
  districtSearchQuery.value = district.name
  showDistrictDropdown.value = false
  onDistrictChange()
}

async function onDistrictChange() {
  // Reset wards when district changes
  addressForm.wardCode = ''
  addressForm.wardName = ''
  wards.value = []
  filteredWards.value = []
  wardSearchQuery.value = ''
  showWardDropdown.value = false
  
  if (!addressForm.districtCode) {
    addressForm.districtName = ''
    return
  }
  
  // Find district info from selected code
  const selectedDistrict = districts.value.find(d => d.code === addressForm.districtCode)
  if (selectedDistrict) {
    addressForm.districtId = selectedDistrict.id
    addressForm.districtName = selectedDistrict.name
  }
  
  // Load wards using GHN API
  loadingWards.value = true
  try {
    const data = await ghnLocationApi.getWards(addressForm.districtId)
    
    // Map GHN data to match existing structure
    wards.value = (data || []).map(w => ({
      code: w.wardCode,
      name: w.wardName
    }))
    
    // Sort wards by name
    wards.value.sort((a, b) => {
      const nameA = (a.name || '').toLowerCase()
      const nameB = (b.name || '').toLowerCase()
      return nameA.localeCompare(nameB)
    })
    
    filteredWards.value = wards.value
    
    if (wards.value.length === 0) {
      showToast('Không có dữ liệu phường/xã cho quận/huyện này', 'warning')
    }
  } catch (error) {
    console.error('Error loading wards:', error)
    showToast('Không thể tải danh sách phường/xã', 'error')
  } finally {
    loadingWards.value = false
  }
}

function filterWards() {
  const query = wardSearchQuery.value.toLowerCase().trim()
  if (!query) {
    filteredWards.value = wards.value
  } else {
    filteredWards.value = wards.value.filter(w => {
      const name = (w.name || '').toLowerCase()
      const code = (w.code || '').toLowerCase()
      return name.includes(query) || code.includes(query)
    })
  }
}

function selectWard(ward) {
  addressForm.wardCode = ward.code
  addressForm.wardName = ward.name
  wardSearchQuery.value = ward.name
  showWardDropdown.value = false
}

async function editAddress(address) {
  editingAddress.value = address
  addressForm.receiverName = address.receiverName || ''
  addressForm.receiverPhone = address.receiverPhone || ''
  addressForm.receiverEmail = address.receiverEmail || ''
  addressForm.provinceName = address.provinceName || ''
  addressForm.districtName = address.districtName || ''
  addressForm.wardName = address.wardName || ''
  
  // Set GHN IDs if available
  addressForm.provinceId = address.provinceId || null
  addressForm.districtId = address.districtId || null
  addressForm.wardCode = address.wardCode || ''
  
  // Load provinces if not loaded
  await loadProvinces()
  
  // Try to find and set province from ID or name
  if (address.provinceId) {
    const province = provinces.value.find(p => p.id === address.provinceId)
    if (province) {
      addressForm.provinceId = province.id
      addressForm.provinceCode = province.code
      addressForm.provinceName = province.name
      provinceSearchQuery.value = province.name
      await onProvinceChange()
      
      // Try to find district
      if (address.districtName) {
        const district = districts.value.find(d => 
          d.name === address.districtName || 
          d.name.toLowerCase().includes(address.districtName.toLowerCase())
        )
        if (district) {
          addressForm.districtCode = district.code
          addressForm.districtName = district.name
          districtSearchQuery.value = district.name
          await onDistrictChange()
          
          // Try to find ward
          if (address.wardName) {
            const ward = wards.value.find(w => 
              w.name === address.wardName || 
              w.name.toLowerCase().includes(address.wardName.toLowerCase())
            )
            if (ward) {
              addressForm.wardCode = ward.code
              addressForm.wardName = ward.name
              wardSearchQuery.value = ward.name
            }
          }
        }
      }
    }
  }
  
  showAddressModal.value = true
}

function deleteAddress(address) {
  addressToDelete.value = address
  showDeleteAddressModal.value = true
}

async function confirmDeleteAddress() {
  if (!addressToDelete.value) return

  deletingAddress.value = true
  try {
    await addressApi.delete(addressToDelete.value.id)
    await loadUserData()
    toast.message = 'Đã xóa địa chỉ thành công'
    toast.type = 'success'
    toast.show = true
    showDeleteAddressModal.value = false
    addressToDelete.value = null
  } catch (error) {
    console.error('Error deleting address:', error)
    toast.message = error.response?.data?.message || 'Không thể xóa địa chỉ'
    toast.type = 'error'
    toast.show = true
  } finally {
    deletingAddress.value = false
  }
}


async function saveAddress() {
  if (!authStore.user?.id) return

  savingAddress.value = true
  try {
    const addressData = {
      receiverName: addressForm.receiverName,
      receiverPhone: addressForm.receiverPhone,
      receiverEmail: addressForm.receiverEmail || null,
      provinceName: addressForm.provinceName,
      districtName: addressForm.districtName,
      wardName: addressForm.wardName,
      provinceId: addressForm.provinceId,
      districtId: addressForm.districtId,
      wardCode: addressForm.wardCode,
      status: 'Active' // Mặc định là Active khi tạo mới
    }

    if (editingAddress.value) {
      // Update existing address
      await addressApi.update(editingAddress.value.id, addressData)
      showToast('Đã cập nhật địa chỉ thành công', 'success')
    } else {
      // Create new address
      addressData.accountId = authStore.user.id
      await addressApi.create(addressData)
      showToast('Đã thêm địa chỉ thành công', 'success')
    }

    await loadUserData()
    closeAddressModal()
  } catch (error) {
    console.error('Error saving address:', error)
    showToast(error.response?.data?.message || 'Không thể lưu địa chỉ', 'error')
  } finally {
    savingAddress.value = false
  }
}

function closeAddressModal() {
  showAddressModal.value = false
  editingAddress.value = null
  districts.value = []
  filteredDistricts.value = []
  wards.value = []
  filteredWards.value = []
  provinceSearchQuery.value = ''
  districtSearchQuery.value = ''
  wardSearchQuery.value = ''
  showProvinceDropdown.value = false
  showDistrictDropdown.value = false
  showWardDropdown.value = false
  Object.assign(addressForm, {
    receiverName: '',
    receiverPhone: '',
    receiverEmail: '',
    provinceId: null,
    provinceCode: '',
    provinceName: '',
    districtId: null,
    districtCode: '',
    districtName: '',
    wardCode: '',
    wardName: ''
  })
}

onMounted(async () => {
  await Promise.all([
    loadUserData(),
    loadProvinces()
  ])
})

// Watch for modal opening to load provinces
watch(showAddressModal, async (isOpen) => {
  if (isOpen) {
    await loadProvinces()
  } else {
    showProvinceDropdown.value = false
    showDistrictDropdown.value = false
    showWardDropdown.value = false
    provinceSearchQuery.value = ''
    districtSearchQuery.value = ''
    wardSearchQuery.value = ''
  }
})

// Close dropdown when clicking outside
function handleClickOutside(event) {
  if (provinceDropdownRef.value && !provinceDropdownRef.value.contains(event.target)) {
    showProvinceDropdown.value = false
  }
  if (districtDropdownRef.value && !districtDropdownRef.value.contains(event.target)) {
    showDistrictDropdown.value = false
  }
  if (wardDropdownRef.value && !wardDropdownRef.value.contains(event.target)) {
    showWardDropdown.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>
