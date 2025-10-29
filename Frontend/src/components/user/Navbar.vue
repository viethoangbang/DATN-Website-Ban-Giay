<template>
  <nav class="bg-white shadow-md sticky top-0 z-40 transition-all duration-300">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between items-center h-16">
        <!-- Logo -->
        <router-link to="/" class="flex items-center space-x-1 group">
          <img 
            src="/src/assets/logo.png" 
            alt="SneakerPoly Logo" 
            class="w-14 h-14 transform group-hover:scale-110 transition-transform duration-300"
          />
          <span class="text-xl font-bold text-gray-900 hidden sm:block">
            Sneaker<span class="text-primary-500">Poly</span>
          </span>
        </router-link>

        <!-- Desktop Navigation -->
        <div class="hidden md:flex items-center space-x-8">
          <router-link
            v-for="link in navLinks"
            :key="link.path"
            :to="link.path"
            class="text-gray-700 hover:text-primary-500 font-medium transition-colors duration-200 relative group"
          >
            {{ link.name }}
            <span class="absolute bottom-0 left-0 w-0 h-0.5 bg-primary-500 group-hover:w-full transition-all duration-300"></span>
          </router-link>
        </div>

        <!-- Right side icons -->
        <div class="flex items-center space-x-4">
          <!-- Search -->
          <button class="p-2 text-gray-700 hover:text-primary-500 hover:bg-primary-50 rounded-lg transition-all duration-300">
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </button>

          <!-- Cart -->
          <router-link
            to="/cart"
            class="relative p-2 text-gray-700 hover:text-primary-500 hover:bg-primary-50 rounded-lg transition-all duration-300 group"
          >
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
            <span
              v-if="cartStore.totalItems > 0"
              class="absolute -top-1 -right-1 bg-primary-500 text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center group-hover:scale-110 transition-transform"
            >
              {{ cartStore.totalItems }}
            </span>
          </router-link>

          <!-- User Menu -->
          <div class="relative" v-if="authStore.isAuthenticated">
            <button
              @click="showUserMenu = !showUserMenu"
              class="flex items-center space-x-2 p-2 rounded-lg hover:bg-gray-100 transition-all duration-300"
            >
              <div class="w-8 h-8 bg-primary-500 rounded-full flex items-center justify-center">
                <span class="text-white font-semibold text-sm">{{ userInitial }}</span>
              </div>
              <svg class="w-4 h-4 text-gray-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
              </svg>
            </button>

            <!-- Dropdown Menu -->
            <Transition
              enter-active-class="transition duration-200 ease-out"
              enter-from-class="transform scale-95 opacity-0"
              enter-to-class="transform scale-100 opacity-100"
              leave-active-class="transition duration-100 ease-in"
              leave-from-class="transform scale-100 opacity-100"
              leave-to-class="transform scale-95 opacity-0"
            >
              <div
                v-if="showUserMenu"
                class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg py-2 border border-gray-100"
              >
                <router-link
                  v-if="authStore.isAdmin"
                  to="/admin"
                  class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 transition-colors"
                  @click="showUserMenu = false"
                >
                  Admin Dashboard
                </router-link>
                <router-link
                  to="/profile"
                  class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 transition-colors"
                  @click="showUserMenu = false"
                >
                  Tài khoản của tôi
                </router-link>
                <router-link
                  to="/settings"
                  class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 transition-colors"
                  @click="showUserMenu = false"
                >
                  Cài đặt
                </router-link>
                <hr class="my-2">
                <button
                  @click="handleLogout"
                  class="block w-full text-left px-4 py-2 text-red-600 hover:bg-red-50 transition-colors"
                >
                  Đăng xuất
                </button>
              </div>
            </Transition>
          </div>

          <!-- Login Button -->
          <router-link
            v-else
            to="/login"
            class="btn-primary hidden sm:block"
          >
            Đăng nhập
          </router-link>

          <!-- Mobile Menu Button -->
          <button
            @click="showMobileMenu = !showMobileMenu"
            class="md:hidden p-2 text-gray-700 hover:text-primary-500 hover:bg-primary-50 rounded-lg transition-all duration-300"
          >
            <svg class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
        </div>
      </div>
    </div>

    <!-- Mobile Menu -->
    <Transition
      enter-active-class="transition duration-300 ease-out"
      enter-from-class="transform -translate-y-full opacity-0"
      enter-to-class="transform translate-y-0 opacity-100"
      leave-active-class="transition duration-200 ease-in"
      leave-from-class="transform translate-y-0 opacity-100"
      leave-to-class="transform -translate-y-full opacity-0"
    >
      <div v-if="showMobileMenu" class="md:hidden border-t border-gray-200">
        <div class="px-4 py-3 space-y-2">
          <router-link
            v-for="link in navLinks"
            :key="link.path"
            :to="link.path"
            @click="showMobileMenu = false"
            class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 rounded-lg transition-colors"
          >
            {{ link.name }}
          </router-link>
          <router-link
            v-if="!authStore.isAuthenticated"
            to="/login"
            @click="showMobileMenu = false"
            class="block px-4 py-2 bg-primary-500 text-white text-center rounded-lg hover:bg-primary-600 transition-colors"
          >
            Đăng nhập
          </router-link>
        </div>
      </div>
    </Transition>
  </nav>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { useCartStore } from '../../stores/cart'

const router = useRouter()
const authStore = useAuthStore()
const cartStore = useCartStore()

const showMobileMenu = ref(false)
const showUserMenu = ref(false)

const navLinks = [
  { name: 'Trang chủ', path: '/' },
  { name: 'Sản phẩm', path: '/products' },
  { name: 'Về chúng tôi', path: '/about' },
  { name: 'Liên hệ', path: '/contact' },
]

const userInitial = computed(() => {
  if (authStore.user?.name) {
    return authStore.user.name.charAt(0).toUpperCase()
  }
  return 'U'
})

function handleLogout() {
  authStore.logout()
  showUserMenu.value = false
  router.push('/login')
}
</script>

