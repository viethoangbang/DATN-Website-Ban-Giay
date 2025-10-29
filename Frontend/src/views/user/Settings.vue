<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="mb-8 animate-fade-in-up">
        <h1 class="text-3xl font-bold text-gray-900 mb-2">Cài đặt</h1>
        <p class="text-gray-600">Quản lý cài đặt tài khoản và ứng dụng</p>
      </div>

      <!-- Change Password -->
      <div class="bg-white rounded-xl shadow-md p-6 mb-6 animate-fade-in-up">
        <h2 class="text-xl font-bold text-gray-900 mb-6 flex items-center">
          <svg class="w-6 h-6 mr-2 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
          </svg>
          Đổi mật khẩu
        </h2>

        <form @submit.prevent="changePassword" class="space-y-4">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Mật khẩu hiện tại</label>
            <div class="relative">
              <input
                v-model="passwordForm.currentPassword"
                :type="showCurrentPassword ? 'text' : 'password'"
                class="input-field pr-12"
                placeholder="Nhập mật khẩu hiện tại"
              />
              <button
                type="button"
                @click="showCurrentPassword = !showCurrentPassword"
                class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700"
              >
                <svg v-if="showCurrentPassword" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
                <svg v-else class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              </button>
            </div>
          </div>

          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Mật khẩu mới</label>
            <div class="relative">
              <input
                v-model="passwordForm.newPassword"
                :type="showNewPassword ? 'text' : 'password'"
                class="input-field pr-12"
                placeholder="Nhập mật khẩu mới"
              />
              <button
                type="button"
                @click="showNewPassword = !showNewPassword"
                class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700"
              >
                <svg v-if="showNewPassword" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
                <svg v-else class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              </button>
            </div>
            <p class="text-xs text-gray-500 mt-1">Mật khẩu phải có ít nhất 8 ký tự</p>
          </div>

          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Xác nhận mật khẩu mới</label>
            <div class="relative">
              <input
                v-model="passwordForm.confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                class="input-field pr-12"
                placeholder="Nhập lại mật khẩu mới"
              />
              <button
                type="button"
                @click="showConfirmPassword = !showConfirmPassword"
                class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-500 hover:text-gray-700"
              >
                <svg v-if="showConfirmPassword" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
                <svg v-else class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              </button>
            </div>
          </div>

          <button type="submit" class="btn-primary">
            Cập nhật mật khẩu
          </button>
        </form>
      </div>

      <!-- Notification Settings -->
      <div class="bg-white rounded-xl shadow-md p-6 mb-6 animate-fade-in-up delay-100">
        <h2 class="text-xl font-bold text-gray-900 mb-6 flex items-center">
          <svg class="w-6 h-6 mr-2 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
          </svg>
          Thông báo
        </h2>

        <div class="space-y-4">
          <div class="flex items-center justify-between py-3 border-b">
            <div>
              <h4 class="font-semibold text-gray-900">Thông báo đơn hàng</h4>
              <p class="text-sm text-gray-600">Nhận thông báo về trạng thái đơn hàng</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="notificationSettings.orderUpdates" />
              <span class="slider"></span>
            </label>
          </div>

          <div class="flex items-center justify-between py-3 border-b">
            <div>
              <h4 class="font-semibold text-gray-900">Khuyến mãi</h4>
              <p class="text-sm text-gray-600">Nhận thông báo về các chương trình khuyến mãi</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="notificationSettings.promotions" />
              <span class="slider"></span>
            </label>
          </div>

          <div class="flex items-center justify-between py-3 border-b">
            <div>
              <h4 class="font-semibold text-gray-900">Sản phẩm mới</h4>
              <p class="text-sm text-gray-600">Nhận thông báo khi có sản phẩm mới</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="notificationSettings.newProducts" />
              <span class="slider"></span>
            </label>
          </div>

          <div class="flex items-center justify-between py-3">
            <div>
              <h4 class="font-semibold text-gray-900">Email marketing</h4>
              <p class="text-sm text-gray-600">Nhận email về tin tức và cập nhật</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="notificationSettings.newsletter" />
              <span class="slider"></span>
            </label>
          </div>
        </div>
      </div>

      <!-- Privacy Settings -->
      <div class="bg-white rounded-xl shadow-md p-6 mb-6 animate-fade-in-up delay-200">
        <h2 class="text-xl font-bold text-gray-900 mb-6 flex items-center">
          <svg class="w-6 h-6 mr-2 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
          </svg>
          Quyền riêng tư
        </h2>

        <div class="space-y-4">
          <div class="flex items-center justify-between py-3 border-b">
            <div>
              <h4 class="font-semibold text-gray-900">Hiển thị thông tin công khai</h4>
              <p class="text-sm text-gray-600">Cho phép người khác xem hồ sơ của bạn</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="privacySettings.publicProfile" />
              <span class="slider"></span>
            </label>
          </div>

          <div class="flex items-center justify-between py-3 border-b">
            <div>
              <h4 class="font-semibold text-gray-900">Lưu lịch sử tìm kiếm</h4>
              <p class="text-sm text-gray-600">Lưu các tìm kiếm gần đây của bạn</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="privacySettings.saveSearchHistory" />
              <span class="slider"></span>
            </label>
          </div>

          <div class="flex items-center justify-between py-3">
            <div>
              <h4 class="font-semibold text-gray-900">Chia sẻ dữ liệu phân tích</h4>
              <p class="text-sm text-gray-600">Giúp chúng tôi cải thiện trải nghiệm</p>
            </div>
            <label class="switch">
              <input type="checkbox" v-model="privacySettings.analytics" />
              <span class="slider"></span>
            </label>
          </div>
        </div>
      </div>

      <!-- Language & Currency -->
      <div class="bg-white rounded-xl shadow-md p-6 mb-6 animate-fade-in-up delay-300">
        <h2 class="text-xl font-bold text-gray-900 mb-6 flex items-center">
          <svg class="w-6 h-6 mr-2 text-primary-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5h12M9 3v2m1.048 9.5A18.022 18.022 0 016.412 9m6.088 9h7M11 21l5-10 5 10M12.751 5C11.783 10.77 8.07 15.61 3 18.129" />
          </svg>
          Ngôn ngữ & Tiền tệ
        </h2>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Ngôn ngữ</label>
            <select v-model="preferences.language" class="input-field">
              <option value="vi">Tiếng Việt</option>
              <option value="en">English</option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-semibold text-gray-700 mb-2">Đơn vị tiền tệ</label>
            <select v-model="preferences.currency" class="input-field">
              <option value="VND">VND (₫)</option>
              <option value="USD">USD ($)</option>
            </select>
          </div>
        </div>
      </div>

      <!-- Danger Zone -->
      <div class="bg-red-50 border-2 border-red-200 rounded-xl p-6 animate-fade-in-up delay-400">
        <h2 class="text-xl font-bold text-red-900 mb-4 flex items-center">
          <svg class="w-6 h-6 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
          Vùng nguy hiểm
        </h2>
        <p class="text-red-700 mb-4">Hành động không thể hoàn tác</p>
        
        <div class="space-y-3">
          <button
            @click="showDeleteModal = true"
            class="w-full md:w-auto px-6 py-3 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors font-semibold"
          >
            Xóa tài khoản
          </button>
        </div>
      </div>
    </div>

    <Footer />

    <!-- Delete Account Modal -->
    <Modal
      :show="showDeleteModal"
      @close="showDeleteModal = false"
      title="Xác nhận xóa tài khoản"
    >
      <p class="text-gray-700 mb-4">
        Bạn có chắc chắn muốn xóa tài khoản? Hành động này không thể hoàn tác và tất cả dữ liệu của bạn sẽ bị xóa vĩnh viễn.
      </p>
      <div class="bg-red-50 border border-red-200 rounded-lg p-4 mb-4">
        <p class="text-red-800 text-sm font-semibold">Lưu ý:</p>
        <ul class="text-red-700 text-sm mt-2 space-y-1 list-disc list-inside">
          <li>Tất cả đơn hàng sẽ bị hủy</li>
          <li>Dữ liệu cá nhân sẽ bị xóa</li>
          <li>Không thể khôi phục tài khoản</li>
        </ul>
      </div>

      <template #footer>
        <button @click="showDeleteModal = false" class="btn-secondary mr-3">
          Hủy
        </button>
        <button @click="deleteAccount" class="px-6 py-3 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors font-semibold">
          Xác nhận xóa
        </button>
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
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Modal from '../../components/common/Modal.vue'
import Toast from '../../components/common/Toast.vue'

const router = useRouter()

const showCurrentPassword = ref(false)
const showNewPassword = ref(false)
const showConfirmPassword = ref(false)
const showDeleteModal = ref(false)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const passwordForm = reactive({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})

const notificationSettings = reactive({
  orderUpdates: true,
  promotions: true,
  newProducts: false,
  newsletter: false
})

const privacySettings = reactive({
  publicProfile: false,
  saveSearchHistory: true,
  analytics: true
})

const preferences = reactive({
  language: 'vi',
  currency: 'VND'
})

function changePassword() {
  if (passwordForm.newPassword !== passwordForm.confirmPassword) {
    toast.message = 'Mật khẩu xác nhận không khớp'
    toast.type = 'error'
    toast.show = true
    return
  }

  if (passwordForm.newPassword.length < 8) {
    toast.message = 'Mật khẩu phải có ít nhất 8 ký tự'
    toast.type = 'error'
    toast.show = true
    return
  }

  // Change password logic
  toast.message = 'Đã cập nhật mật khẩu thành công'
  toast.type = 'success'
  toast.show = true
  
  // Reset form
  passwordForm.currentPassword = ''
  passwordForm.newPassword = ''
  passwordForm.confirmPassword = ''
}

function deleteAccount() {
  // Delete account logic
  showDeleteModal.value = false
  toast.message = 'Tài khoản đã được xóa'
  toast.type = 'success'
  toast.show = true
  
  setTimeout(() => {
    router.push('/login')
  }, 2000)
}
</script>

<style scoped>
/* Toggle Switch */
.switch {
  position: relative;
  display: inline-block;
  width: 48px;
  height: 24px;
}

.switch input {
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #cbd5e1;
  transition: 0.3s;
  border-radius: 24px;
}

.slider:before {
  position: absolute;
  content: "";
  height: 18px;
  width: 18px;
  left: 3px;
  bottom: 3px;
  background-color: white;
  transition: 0.3s;
  border-radius: 50%;
}

input:checked + .slider {
  background-color: #f97316;
}

input:checked + .slider:before {
  transform: translateX(24px);
}
</style>

