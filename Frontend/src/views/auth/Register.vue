<template>
  <div class="min-h-screen bg-gradient-to-br from-primary-50 via-white to-orange-50 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8 animate-fade-in-up">
      <!-- Logo -->
      <div class="text-center">
        <router-link to="/" class="inline-flex items-center justify-center space-x-2 group">
          <img 
            src="/src/assets/logo.png" 
            alt="SneakerPoly Logo" 
            class="w-16 h-16 transform group-hover:scale-110 transition-transform duration-300"
          />
          <span class="text-2xl font-bold text-gray-900">
            Sneaker<span class="text-primary-500">Poly</span>
          </span>
        </router-link>
        <h2 class="mt-6 text-3xl font-extrabold text-gray-900">
          Tạo tài khoản mới
        </h2>
        <p class="mt-2 text-sm text-gray-600">
          Đã có tài khoản?
          <router-link to="/login" class="font-medium text-primary-600 hover:text-primary-500 transition-colors">
            Đăng nhập ngay
          </router-link>
        </p>
      </div>

      <!-- Register Form -->
      <div class="bg-white rounded-2xl shadow-xl p-8">
        <form @submit.prevent="handleRegister" class="space-y-6">
          <!-- Full Name -->
          <div>
            <label for="fullName" class="block text-sm font-bold text-gray-700 mb-2">
              Họ và tên <span class="text-red-500">*</span>
            </label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                </svg>
              </div>
              <input
                id="fullName"
                v-model="form.fullName"
                type="text"
                required
                maxlength="255"
                class="input-field pl-10"
                :class="{ 'border-red-500': errors.fullName }"
                placeholder="Nguyễn Văn A"
                @blur="validateFullName"
              />
            </div>
            <p v-if="errors.fullName" class="mt-1 text-sm text-red-600">{{ errors.fullName }}</p>
          </div>

          <!-- Email -->
          <div>
            <label for="email" class="block text-sm font-bold text-gray-700 mb-2">
              Email <span class="text-red-500">*</span>
            </label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                </svg>
              </div>
              <input
                id="email"
                v-model="form.email"
                type="email"
                required
                maxlength="255"
                class="input-field pl-10"
                :class="{ 'border-red-500': errors.email }"
                placeholder="example@email.com"
                @blur="validateEmail"
              />
            </div>
            <p v-if="errors.email" class="mt-1 text-sm text-red-600">{{ errors.email }}</p>
          </div>

          <!-- Phone -->
          <div>
            <label for="phone" class="block text-sm font-bold text-gray-700 mb-2">
              Số điện thoại
            </label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                </svg>
              </div>
              <input
                id="phone"
                v-model="form.phone"
                type="tel"
                maxlength="20"
                class="input-field pl-10"
                :class="{ 'border-red-500': errors.phone }"
                placeholder="0912345678"
                @blur="validatePhone"
              />
            </div>
            <p v-if="errors.phone" class="mt-1 text-sm text-red-600">{{ errors.phone }}</p>
          </div>

          <!-- Password -->
          <div>
            <label for="password" class="block text-sm font-bold text-gray-700 mb-2">
              Mật khẩu <span class="text-red-500">*</span>
            </label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                </svg>
              </div>
              <input
                id="password"
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                required
                minlength="6"
                maxlength="255"
                class="input-field pl-10 pr-10"
                :class="{ 'border-red-500': errors.password }"
                placeholder="Tối thiểu 6 ký tự"
                @blur="validatePassword"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
              >
                <svg v-if="showPassword" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
                <svg v-else class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              </button>
            </div>
            <p v-if="errors.password" class="mt-1 text-sm text-red-600">{{ errors.password }}</p>
            <!-- Password Strength Indicator -->
            <div v-if="form.password" class="mt-2">
              <div class="flex space-x-1">
                <div
                  v-for="i in 4"
                  :key="i"
                  class="h-1 flex-1 rounded-full"
                  :class="i <= passwordStrength ? passwordStrengthColor : 'bg-gray-200'"
                ></div>
              </div>
              <p class="text-xs mt-1" :class="passwordStrengthColor.replace('bg-', 'text-')">
                {{ passwordStrengthText }}
              </p>
            </div>
          </div>

          <!-- Confirm Password -->
          <div>
            <label for="confirmPassword" class="block text-sm font-bold text-gray-700 mb-2">
              Xác nhận mật khẩu <span class="text-red-500">*</span>
            </label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
              <input
                id="confirmPassword"
                v-model="form.confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                required
                class="input-field pl-10 pr-10"
                :class="{ 'border-red-500': errors.confirmPassword }"
                placeholder="Nhập lại mật khẩu"
                @blur="validateConfirmPassword"
              />
              <button
                type="button"
                @click="showConfirmPassword = !showConfirmPassword"
                class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
              >
                <svg v-if="showConfirmPassword" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
                <svg v-else class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
              </button>
            </div>
            <p v-if="errors.confirmPassword" class="mt-1 text-sm text-red-600">{{ errors.confirmPassword }}</p>
          </div>

          <!-- Terms & Conditions -->
          <div class="flex items-start">
            <input
              id="terms"
              v-model="form.acceptTerms"
              type="checkbox"
              required
              class="h-4 w-4 text-primary-600 focus:ring-primary-500 border-gray-300 rounded mt-1"
            />
            <label for="terms" class="ml-2 block text-sm text-gray-700">
              Tôi đồng ý với
              <a href="#" class="text-primary-600 hover:text-primary-500 font-medium">Điều khoản dịch vụ</a>
              và
              <a href="#" class="text-primary-600 hover:text-primary-500 font-medium">Chính sách bảo mật</a>
            </label>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="loading || !isFormValid"
            class="w-full btn-primary py-4 text-lg font-bold disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center space-x-2"
          >
            <svg v-if="loading" class="animate-spin h-5 w-5 text-white" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            <span>{{ loading ? 'Đang xử lý...' : 'Đăng ký' }}</span>
          </button>
        </form>

        
      </div>

      <!-- Back to Home -->
      <div class="text-center">
        <router-link to="/" class="text-sm text-gray-600 hover:text-primary-600 transition-colors flex items-center justify-center space-x-1">
          <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
          </svg>
          <span>Quay lại trang chủ</span>
        </router-link>
      </div>
    </div>

    <!-- Toast Notification -->
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
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import Toast from '../../components/common/Toast.vue'

const router = useRouter()
const authStore = useAuthStore()

const loading = ref(false)
const showPassword = ref(false)
const showConfirmPassword = ref(false)

const form = reactive({
  fullName: '',
  email: '',
  phone: '',
  password: '',
  confirmPassword: '',
  acceptTerms: false
})

const errors = reactive({
  fullName: '',
  email: '',
  phone: '',
  password: '',
  confirmPassword: ''
})

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

// Validation functions
function validateFullName() {
  if (!form.fullName.trim()) {
    errors.fullName = 'Vui lòng nhập họ tên'
    return false
  }
  if (form.fullName.length < 2) {
    errors.fullName = 'Họ tên phải có ít nhất 2 ký tự'
    return false
  }
  errors.fullName = ''
  return true
}

function validateEmail() {
  if (!form.email.trim()) {
    errors.email = 'Vui lòng nhập email'
    return false
  }
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailRegex.test(form.email)) {
    errors.email = 'Email không hợp lệ'
    return false
  }
  errors.email = ''
  return true
}

function validatePhone() {
  if (form.phone && form.phone.trim()) {
    const phoneRegex = /^[0-9]{10,11}$/
    if (!phoneRegex.test(form.phone.replace(/\s/g, ''))) {
      errors.phone = 'Số điện thoại không hợp lệ (10-11 số)'
      return false
    }
  }
  errors.phone = ''
  return true
}

function validatePassword() {
  if (!form.password) {
    errors.password = 'Vui lòng nhập mật khẩu'
    return false
  }
  if (form.password.length < 6) {
    errors.password = 'Mật khẩu phải có ít nhất 6 ký tự'
    return false
  }
  errors.password = ''
  return true
}

function validateConfirmPassword() {
  if (!form.confirmPassword) {
    errors.confirmPassword = 'Vui lòng xác nhận mật khẩu'
    return false
  }
  if (form.password !== form.confirmPassword) {
    errors.confirmPassword = 'Mật khẩu không khớp'
    return false
  }
  errors.confirmPassword = ''
  return true
}

// Password strength
const passwordStrength = computed(() => {
  const password = form.password
  if (!password) return 0
  
  let strength = 0
  if (password.length >= 6) strength++
  if (password.length >= 8) strength++
  if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength++
  if (/[0-9]/.test(password) && /[^a-zA-Z0-9]/.test(password)) strength++
  
  return strength
})

const passwordStrengthColor = computed(() => {
  switch (passwordStrength.value) {
    case 1: return 'bg-red-500'
    case 2: return 'bg-yellow-500'
    case 3: return 'bg-blue-500'
    case 4: return 'bg-green-500'
    default: return 'bg-gray-200'
  }
})

const passwordStrengthText = computed(() => {
  switch (passwordStrength.value) {
    case 1: return 'Yếu'
    case 2: return 'Trung bình'
    case 3: return 'Mạnh'
    case 4: return 'Rất mạnh'
    default: return ''
  }
})

const isFormValid = computed(() => {
  return form.fullName && 
         form.email && 
         form.password && 
         form.confirmPassword && 
         form.acceptTerms &&
         !errors.fullName && 
         !errors.email && 
         !errors.phone &&
         !errors.password && 
         !errors.confirmPassword
})

async function handleRegister() {
  // Validate all fields
  const isValid = validateFullName() && 
                  validateEmail() && 
                  validatePhone() &&
                  validatePassword() && 
                  validateConfirmPassword()
  
  if (!isValid) {
    toast.message = 'Vui lòng kiểm tra lại thông tin'
    toast.type = 'error'
    toast.show = true
    return
  }

  if (!form.acceptTerms) {
    toast.message = 'Vui lòng đồng ý với điều khoản dịch vụ'
    toast.type = 'error'
    toast.show = true
    return
  }

  loading.value = true

  try {
    // TODO: Replace with actual API call
    await new Promise(resolve => setTimeout(resolve, 1500))

    const userData = {
      fullName: form.fullName.trim(),
      email: form.email.trim().toLowerCase(),
      phone: form.phone?.trim() || null,
      password: form.password,
      role: 'user'
    }

    // Simulate registration
    console.log('Registering user:', { ...userData, password: '***' })

    toast.message = 'Đăng ký thành công! Đang chuyển hướng...'
    toast.type = 'success'
    toast.show = true

    // Redirect to login after success
    setTimeout(() => {
      router.push('/login')
    }, 1500)

  } catch (error) {
    console.error('Registration error:', error)
    toast.message = error.response?.data?.message || 'Đăng ký thất bại. Vui lòng thử lại'
    toast.type = 'error'
    toast.show = true
  } finally {
    loading.value = false
  }
}
</script>

