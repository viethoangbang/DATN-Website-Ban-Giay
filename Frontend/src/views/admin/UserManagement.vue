<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-gray-900">Quản lý Người dùng & Nhân viên</h1>
      <button @click="showAddModal = true" class="btn-primary flex items-center space-x-2">
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        <span>Thêm người dùng</span>
      </button>
    </div>

    <!-- Filter Tabs -->
    <div class="flex space-x-2 border-b">
      <button
        @click="filterRole = null"
        :class="filterRole === null ? 'border-b-2 border-primary-600 text-primary-600' : 'text-gray-600'"
        class="px-4 py-2 font-medium"
      >
        Tất cả
      </button>
      <button
        @click="filterRole = 'customer'"
        :class="filterRole === 'customer' ? 'border-b-2 border-primary-600 text-primary-600' : 'text-gray-600'"
        class="px-4 py-2 font-medium"
      >
        Khách hàng
      </button>
      <button
        @click="filterRole = 'employee'"
        :class="filterRole === 'employee' ? 'border-b-2 border-primary-600 text-primary-600' : 'text-gray-600'"
        class="px-4 py-2 font-medium"
      >
        Nhân viên
      </button>
      <button
        @click="filterRole = 'admin'"
        :class="filterRole === 'admin' ? 'border-b-2 border-primary-600 text-primary-600' : 'text-gray-600'"
        class="px-4 py-2 font-medium"
      >
        Quản trị viên
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
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Avatar</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Họ tên</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Số điện thoại</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vai trò</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Trạng thái</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Ngày tạo</th>
            <th class="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Thao tác</th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="user in filteredUsers" :key="user.id" class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap">
              <div class="flex-shrink-0 h-10 w-10">
                <img
                  v-if="user.avatarUrl"
                  :src="user.avatarUrl"
                  :alt="user.fullName || user.email"
                  class="h-10 w-10 rounded-full object-cover"
                  @error="handleImageError"
                />
                <div
                  v-else
                  class="h-10 w-10 rounded-full bg-gray-300 flex items-center justify-center text-gray-600 font-semibold text-sm"
                >
                  {{ getUserInitials(user) }}
                </div>
              </div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">#{{ user.id }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ user.email }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ user.fullName || '-' }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{{ user.phoneNumber || '-' }}</td>
            <td class="px-6 py-4 whitespace-nowrap">
              <span
                v-for="role in user.roles"
                :key="role"
                :class="getRoleBadgeClass(role)"
                class="px-2 py-1 text-xs font-semibold rounded-full mr-1"
              >
                {{ getRoleLabel(role) }}
              </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
              <span
                :class="getStatusClass(user.status)"
                class="px-2 py-1 text-xs font-semibold rounded-full"
              >
                {{ getStatusLabel(user.status) }}
              </span>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
              {{ formatDate(user.createDate) }}
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
              <button @click="viewAddresses(user)" class="text-green-600 hover:text-green-900 mr-4" title="Xem địa chỉ">Địa chỉ</button>
              <button @click="editUser(user)" class="text-blue-600 hover:text-blue-900 mr-4">Sửa</button>
              <button @click="showChangePasswordModal(user)" class="text-purple-600 hover:text-purple-900 mr-4">Đổi MK</button>
              <button @click="deleteUser(user)" class="text-red-600 hover:text-red-900">Xóa</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Add/Edit Modal -->
    <Modal :show="showAddModal || showEditModal" :title="editingUser ? 'Chỉnh sửa người dùng' : 'Thêm người dùng'" @close="closeModal">
      <form @submit.prevent="saveUser" class="space-y-4">
        <div v-if="!editingUser">
          <label class="block text-sm font-semibold text-gray-700 mb-2">Email *</label>
          <input v-model="formData.email" type="email" required class="input-field" placeholder="user@example.com" />
        </div>
        <div v-if="!editingUser">
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mật khẩu *</label>
          <input v-model="formData.password" type="password" required :minlength="6" class="input-field" placeholder="Tối thiểu 6 ký tự" />
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Họ tên</label>
          <input v-model="formData.fullName" type="text" class="input-field" placeholder="Nguyễn Văn A" />
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Số điện thoại</label>
          <input v-model="formData.phoneNumber" type="tel" class="input-field" placeholder="0123456789" />
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Giới tính</label>
          <select v-model="formData.sex" class="input-field">
            <option value="">Chọn giới tính</option>
            <option value="Male">Nam</option>
            <option value="Female">Nữ</option>
            <option value="Other">Khác</option>
          </select>
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Năm sinh</label>
          <input v-model.number="formData.birthYear" type="number" min="1900" max="2100" class="input-field" placeholder="1990" />
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Avatar URL</label>
          <input v-model="formData.avatarUrl" type="url" class="input-field" placeholder="https://example.com/avatar.jpg" />
          <p class="text-xs text-gray-500 mt-1">Nhập URL hình ảnh avatar</p>
          <div v-if="formData.avatarUrl" class="mt-2">
            <img :src="formData.avatarUrl" alt="Avatar preview" class="h-20 w-20 rounded-full object-cover border-2 border-gray-200" @error="handleImageError" />
          </div>
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Vai trò *</label>
          <div class="space-y-2">
            <label class="flex items-center">
              <input
                v-model="formData.roles"
                type="checkbox"
                value="customer"
                class="w-4 h-4 text-primary-600 border-gray-300 rounded focus:ring-primary-500"
              />
              <span class="ml-2 text-sm text-gray-700">Khách hàng</span>
            </label>
            <label class="flex items-center">
              <input
                v-model="formData.roles"
                type="checkbox"
                value="employee"
                class="w-4 h-4 text-primary-600 border-gray-300 rounded focus:ring-primary-500"
              />
              <span class="ml-2 text-sm text-gray-700">Nhân viên</span>
            </label>
            <label class="flex items-center">
              <input
                v-model="formData.roles"
                type="checkbox"
                value="admin"
                class="w-4 h-4 text-primary-600 border-gray-300 rounded focus:ring-primary-500"
              />
              <span class="ml-2 text-sm text-gray-700">Quản trị viên</span>
            </label>
          </div>
          <p v-if="formData.roles.length === 0" class="text-xs text-red-500 mt-1">Vui lòng chọn ít nhất một vai trò</p>
        </div>
        <div v-if="editingUser">
          <label class="block text-sm font-semibold text-gray-700 mb-2">Trạng thái</label>
          <select v-model="formData.status" class="input-field">
            <option value="Active">Hoạt động</option>
            <option value="Inactive">Không hoạt động</option>
            <option value="Banned">Bị khóa</option>
          </select>
          <p class="text-xs text-gray-500 mt-1">
            <span v-if="formData.status === 'Active'">Tài khoản sẽ được kích hoạt và có thể sử dụng</span>
            <span v-else>Tài khoản sẽ bị vô hiệu hóa và không thể đăng nhập</span>
          </p>
        </div>
      </form>
      <template #footer>
        <button @click="saveUser" :disabled="saving" class="btn-primary">
          {{ saving ? 'Đang lưu...' : (editingUser ? 'Cập nhật' : 'Lưu') }}
        </button>
        <button @click="closeModal" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Change Password Modal -->
    <Modal :show="showPasswordModal" title="Đổi mật khẩu" @close="showPasswordModal = false">
      <form @submit.prevent="changePassword" class="space-y-4">
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mật khẩu hiện tại *</label>
          <input v-model="passwordData.currentPassword" type="password" required class="input-field" />
        </div>
        <div>
          <label class="block text-sm font-semibold text-gray-700 mb-2">Mật khẩu mới *</label>
          <input v-model="passwordData.newPassword" type="password" required :minlength="6" class="input-field" placeholder="Tối thiểu 6 ký tự" />
        </div>
      </form>
      <template #footer>
        <button @click="changePassword" :disabled="saving" class="btn-primary">
          {{ saving ? 'Đang đổi...' : 'Đổi mật khẩu' }}
        </button>
        <button @click="showPasswordModal = false" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Delete Modal -->
    <Modal :show="showDeleteModal" title="Xác nhận xóa" @close="showDeleteModal = false">
      <p class="text-gray-600">Bạn có chắc chắn muốn xóa người dùng <strong>{{ userToDelete?.email }}</strong>?</p>
      <template #footer>
        <button @click="confirmDelete" class="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700">Xóa</button>
        <button @click="showDeleteModal = false" class="btn-secondary">Hủy</button>
      </template>
    </Modal>

    <!-- Addresses Modal -->
    <Modal :show="showAddressModal" :title="`Địa chỉ của ${selectedUser?.fullName || selectedUser?.email || 'Người dùng'}`" @close="showAddressModal = false">
      <div v-if="loadingAddresses" class="flex justify-center py-8">
        <svg class="animate-spin h-8 w-8 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
          <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
          <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
        </svg>
      </div>
      <div v-else-if="addresses.length === 0" class="text-center py-8 text-gray-500">
        <p>Người dùng này chưa có địa chỉ nào</p>
      </div>
      <div v-else class="space-y-4">
        <div
          v-for="address in addresses"
          :key="address.id"
          class="border border-gray-200 rounded-lg p-4 hover:bg-gray-50"
        >
          <div class="flex justify-between items-start">
            <div class="flex-1">
              <h4 class="font-semibold text-gray-900 mb-2">{{ address.receiverName || 'Chưa có tên' }}</h4>
              <p class="text-sm text-gray-600 mb-1">
                <span class="font-medium">Địa chỉ:</span> 
                {{ [address.wardName, address.districtName, address.provinceName].filter(Boolean).join(', ') || 'Chưa có địa chỉ' }}
              </p>
              <p class="text-sm text-gray-600 mb-1">
                <span class="font-medium">SĐT:</span> {{ address.receiverPhone || '-' }}
              </p>
              <p class="text-sm text-gray-600">
                <span class="font-medium">Email:</span> {{ address.receiverEmail || '-' }}
              </p>
              <span
                :class="address.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'"
                class="inline-block mt-2 px-2 py-1 text-xs font-semibold rounded-full"
              >
                {{ address.status || 'Chưa xác định' }}
              </span>
            </div>
          </div>
        </div>
      </div>
      <template #footer>
        <button @click="showAddressModal = false" class="btn-secondary">Đóng</button>
      </template>
    </Modal>

    <!-- Toast -->
    <Toast :show="toast.show" :message="toast.message" :type="toast.type" @close="toast.show = false" />
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import { accountApi, addressApi, handleApiError } from '@/services/api'
import Modal from '@/components/common/Modal.vue'
import Toast from '@/components/common/Toast.vue'

const loading = ref(false)
const saving = ref(false)
const loadingAddresses = ref(false)
const users = ref([])
const addresses = ref([])
const filterRole = ref(null)

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const showPasswordModal = ref(false)
const showAddressModal = ref(false)
const editingUser = ref(null)
const userToDelete = ref(null)
const selectedUser = ref(null)

const formData = reactive({
  email: '',
  password: '',
  fullName: '',
  phoneNumber: '',
  sex: '',
  birthYear: null,
  avatarUrl: '',
  roles: ['customer'],
  status: 'Active'
})

const passwordData = reactive({
  currentPassword: '',
  newPassword: ''
})

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const filteredUsers = computed(() => {
  if (!filterRole.value) return users.value
  return users.value.filter(user => user.roles.includes(filterRole.value))
})

const getRoleLabel = (role) => {
  const labels = {
    admin: 'Quản trị viên',
    employee: 'Nhân viên',
    customer: 'Khách hàng'
  }
  return labels[role] || role
}

const getRoleBadgeClass = (role) => {
  const classes = {
    admin: 'bg-red-100 text-red-800',
    employee: 'bg-blue-100 text-blue-800',
    customer: 'bg-green-100 text-green-800'
  }
  return classes[role] || 'bg-gray-100 text-gray-800'
}

const getStatusClass = (status) => {
  const classes = {
    'Active': 'bg-green-100 text-green-800',
    'Inactive': 'bg-yellow-100 text-yellow-800',
    'Banned': 'bg-red-100 text-red-800'
  }
  return classes[status] || 'bg-gray-100 text-gray-800'
}

const getStatusLabel = (status) => {
  const labels = {
    'Active': 'Hoạt động',
    'Inactive': 'Không hoạt động',
    'Banned': 'Bị khóa'
  }
  return labels[status] || status || 'Chưa xác định'
}

const formatDate = (date) => {
  if (!date) return '-'
  return new Date(date).toLocaleDateString('vi-VN')
}

const getUserInitials = (user) => {
  if (user.fullName) {
    const names = user.fullName.split(' ')
    if (names.length > 1) {
      return `${names[0][0]}${names[names.length - 1][0]}`.toUpperCase()
    }
    return names[0][0].toUpperCase()
  }
  if (user.email) {
    return user.email[0].toUpperCase()
  }
  return 'U'
}

const handleImageError = (event) => {
  event.target.style.display = 'none'
  const parent = event.target.parentElement
  if (parent && !parent.querySelector('.avatar-placeholder')) {
    const placeholder = document.createElement('div')
    placeholder.className = 'avatar-placeholder h-10 w-10 rounded-full bg-gray-300 flex items-center justify-center text-gray-600 font-semibold text-sm'
    placeholder.textContent = '?'
    parent.appendChild(placeholder)
  }
}

const loadUsers = async () => {
  try {
    loading.value = true
    users.value = await accountApi.getAll()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    loading.value = false
  }
}

const editUser = (user) => {
  editingUser.value = user
  formData.email = user.email || ''
  formData.fullName = user.fullName || ''
  formData.phoneNumber = user.phoneNumber || ''
  formData.sex = user.sex || ''
  formData.birthYear = user.birthYear || null
  formData.avatarUrl = user.avatarUrl || ''
  formData.roles = user.roles && user.roles.length > 0 ? [...user.roles] : ['customer']
  formData.status = user.status || 'Active'
  showEditModal.value = true
}

const deleteUser = (user) => {
  userToDelete.value = user
  showDeleteModal.value = true
}

const viewAddresses = async (user) => {
  try {
    selectedUser.value = user
    showAddressModal.value = true
    loadingAddresses.value = true
    addresses.value = []
    
    // Get all addresses and filter by accountId
    const allAddresses = await addressApi.getAll()
    addresses.value = allAddresses.filter(addr => addr.accountId === user.id)
  } catch (error) {
    console.error('Error loading addresses:', error)
    showToast(handleApiError(error), 'error')
    addresses.value = []
  } finally {
    loadingAddresses.value = false
  }
}

const showChangePasswordModal = (user) => {
  editingUser.value = user
  passwordData.currentPassword = ''
  passwordData.newPassword = ''
  showPasswordModal.value = true
}

const saveUser = async () => {
  // Validate roles
  if (!formData.roles || formData.roles.length === 0) {
    showToast('Vui lòng chọn ít nhất một vai trò', 'error')
    return
  }

  try {
    saving.value = true
    
    if (editingUser.value) {
      await accountApi.update(editingUser.value.id, {
        fullName: formData.fullName,
        phoneNumber: formData.phoneNumber,
        sex: formData.sex || null,
        birthYear: formData.birthYear || null,
        avatarUrl: formData.avatarUrl || null,
        roles: formData.roles,
        status: formData.status
      })
      showToast('Cập nhật người dùng thành công!', 'success')
    } else {
      await accountApi.create({
        email: formData.email,
        password: formData.password,
        fullName: formData.fullName,
        phoneNumber: formData.phoneNumber,
        sex: formData.sex || null,
        birthYear: formData.birthYear || null,
        avatarUrl: formData.avatarUrl || null,
        roles: formData.roles,
        status: formData.status
      })
      showToast('Thêm người dùng thành công!', 'success')
    }
    
    closeModal()
    await loadUsers()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    saving.value = false
  }
}

const changePassword = async () => {
  try {
    saving.value = true
    await accountApi.changePassword(editingUser.value.id, {
      currentPassword: passwordData.currentPassword,
      newPassword: passwordData.newPassword
    })
    showToast('Đổi mật khẩu thành công!', 'success')
    showPasswordModal.value = false
    passwordData.currentPassword = ''
    passwordData.newPassword = ''
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = async () => {
  try {
    await accountApi.delete(userToDelete.value.id)
    showToast('Xóa người dùng thành công!', 'success')
    showDeleteModal.value = false
    await loadUsers()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  }
}

const closeModal = () => {
  showAddModal.value = false
  showEditModal.value = false
  editingUser.value = null
  formData.email = ''
  formData.password = ''
  formData.fullName = ''
  formData.phoneNumber = ''
  formData.sex = ''
  formData.birthYear = null
  formData.avatarUrl = ''
  formData.roles = ['customer']
  formData.status = 'Active'
}

const showToast = (message, type = 'success') => {
  toast.message = message
  toast.type = type
  toast.show = true
  setTimeout(() => {
    toast.show = false
  }, 3000)
}

onMounted(() => {
  loadUsers()
})
</script>
