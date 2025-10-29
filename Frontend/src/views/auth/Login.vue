<template>
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-primary-50 via-white to-primary-100 px-4 py-12">
    <div class="max-w-6xl w-full grid md:grid-cols-2 gap-8 items-center">
      <!-- Left side - Illustration -->
      <div class="hidden md:block animate-fade-in">
        <div class="relative">
          <div class="absolute -top-10 -left-10 w-72 h-72 bg-primary-200 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-pulse-slow"></div>
          <div class="absolute -bottom-10 -right-10 w-72 h-72 bg-primary-300 rounded-full mix-blend-multiply filter blur-xl opacity-70 animate-pulse-slow" style="animation-delay: 1s;"></div>
          <div class="relative">
            <img 
              src="https://images.unsplash.com/photo-1460353581641-37baddab0fa2?w=600" 
              alt="Sneakers" 
              class="w-full h-auto rounded-2xl shadow-2xl transform hover:scale-105 transition-transform duration-500"
            />
            <div class="absolute inset-0 bg-gradient-to-t from-black/50 to-transparent rounded-2xl"></div>
            <div class="absolute bottom-8 left-8 text-white">
              <h2 class="text-3xl font-bold mb-2 animate-fade-in-up">SneakerPoly</h2>
              <p class="text-lg opacity-90 animate-fade-in-up" style="animation-delay: 0.2s;">Bước đi phong cách, tự tin mỗi ngày</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Right side - Login Form -->
      <div class="bg-white rounded-2xl shadow-2xl p-8 md:p-12 animate-fade-in-up">
        <div class="text-center mb-8">
          <h1 class="text-3xl md:text-4xl font-bold text-gray-900 mb-2">
            Chào mừng trở lại
          </h1>
          <p class="text-gray-600">Đăng nhập để tiếp tục mua sắm</p>
        </div>

        <form @submit.prevent="handleLogin" class="space-y-6">
          <!-- Email Input -->
          <div class="transform transition-all duration-300 hover:scale-[1.02]">
            <label for="email" class="block text-sm font-semibold text-gray-700 mb-2">
              Email
            </label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207" />
                </svg>
              </div>
              <input
                id="email"
                v-model="form.email"
                type="email"
                required
                class="input-field pl-10"
                placeholder="your.email@example.com"
              />
            </div>
          </div>

          <!-- Password Input -->
          <div class="transform transition-all duration-300 hover:scale-[1.02]">
            <label for="password" class="block text-sm font-semibold text-gray-700 mb-2">
              Mật khẩu
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
                class="input-field pl-10 pr-10"
                placeholder="••••••••"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600"
              >
                <svg v-if="!showPassword" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
                <svg v-else class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />
                </svg>
              </button>
            </div>
          </div>

          <!-- Remember & Forgot -->
          <div class="flex items-center justify-between">
            <label class="flex items-center">
              <input
                type="checkbox"
                v-model="form.remember"
                class="w-4 h-4 text-primary-600 border-gray-300 rounded focus:ring-primary-500"
              />
              <span class="ml-2 text-sm text-gray-600">Ghi nhớ đăng nhập</span>
            </label>
            <a href="#" class="text-sm text-primary-600 hover:text-primary-700 font-medium">
              Quên mật khẩu?
            </a>
          </div>

          <!-- Login Buttons -->
          <div class="space-y-3">
            <button
              type="submit"
              :disabled="loading"
              class="w-full btn-primary py-3 text-lg font-bold disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <span v-if="!loading">Đăng nhập</span>
              <span v-else class="flex items-center justify-center">
                <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Đang đăng nhập...
              </span>
            </button>

            <!-- Quick Login Buttons -->
            <div class="relative">
              <div class="absolute inset-0 flex items-center">
                <div class="w-full border-t border-gray-300"></div>
              </div>
              <div class="relative flex justify-center text-sm">
                <span class="px-2 bg-white text-gray-500">Hoặc đăng nhập nhanh</span>
              </div>
            </div>

            <div class="grid grid-cols-2 gap-3">
              <button
                type="button"
                @click="quickLogin('user')"
                class="flex items-center justify-center px-4 py-2 border-2 border-blue-500 text-blue-500 rounded-lg hover:bg-blue-50 transition-all duration-300 transform hover:scale-105"
              >
                <svg class="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clip-rule="evenodd" />
                </svg>
                User Demo
              </button>
              <button
                type="button"
                @click="quickLogin('admin')"
                class="flex items-center justify-center px-4 py-2 border-2 border-primary-500 text-primary-500 rounded-lg hover:bg-primary-50 transition-all duration-300 transform hover:scale-105"
              >
                <svg class="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-6-3a2 2 0 11-4 0 2 2 0 014 0zm-2 4a5 5 0 00-4.546 2.916A5.986 5.986 0 0010 16a5.986 5.986 0 004.546-2.084A5 5 0 0010 11z" clip-rule="evenodd" />
                </svg>
                Admin Demo
              </button>
            </div>
          </div>

          <!-- Register Link -->
          <div class="text-center">
            <p class="text-sm text-gray-600">
              Chưa có tài khoản?
              <a href="#" class="text-primary-600 hover:text-primary-700 font-semibold">
                Đăng ký ngay
              </a>
            </p>
          </div>
        </form>
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
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import Toast from '../../components/common/Toast.vue'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  email: '',
  password: '',
  remember: false
})

const showPassword = ref(false)
const loading = ref(false)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

function showToast(message, type = 'success') {
  toast.message = message
  toast.type = type
  toast.show = true
}

async function handleLogin() {
  loading.value = true
  
  // Simulate API call
  await new Promise(resolve => setTimeout(resolve, 1500))
  
  try {
    // Mock authentication
    const userData = {
      id: 1,
      name: 'User Demo',
      email: form.email,
      role: form.email.includes('admin') ? 'admin' : 'user'
    }
    
    authStore.login(userData, 'mock-token-' + Date.now())
    
    showToast('Đăng nhập thành công!', 'success')
    
    setTimeout(() => {
      if (userData.role === 'admin') {
        router.push('/admin')
      } else {
        router.push('/')
      }
    }, 1000)
  } catch (error) {
    showToast('Đăng nhập thất bại. Vui lòng thử lại!', 'error')
  } finally {
    loading.value = false
  }
}

function quickLogin(role) {
  if (role === 'admin') {
    form.email = 'admin@sneakerpoly.com'
    form.password = 'admin123'
  } else {
    form.email = 'user@sneakerpoly.com'
    form.password = 'user123'
  }
  handleLogin()
}
</script>

