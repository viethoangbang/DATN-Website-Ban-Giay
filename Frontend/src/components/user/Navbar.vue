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
        <div class="hidden md:flex items-center space-x-6">
          <router-link
            v-for="link in navLinks"
            :key="link.path"
            :to="link.path"
            class="text-gray-700 hover:text-primary-500 font-medium transition-colors duration-200 relative group"
          >
            {{ link.name }}
            <span class="absolute bottom-0 left-0 w-0 h-0.5 bg-primary-500 group-hover:w-full transition-all duration-300"></span>
          </router-link>

          <!-- Categories Dropdown -->
          <div class="relative group" ref="categoriesDropdownRef">
            <button
              @click.stop="toggleCategoriesMenu"
              class="text-gray-700 hover:text-primary-500 font-medium transition-colors duration-200 flex items-center space-x-1"
            >
              <span>Danh mục</span>
              <svg class="w-4 h-4 transition-transform duration-200" :class="{ 'rotate-180': showCategoriesMenu }" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
              </svg>
            </button>
            <Transition
              enter-active-class="transition duration-200 ease-out"
              enter-from-class="transform scale-95 opacity-0"
              enter-to-class="transform scale-100 opacity-100"
              leave-active-class="transition duration-150 ease-in"
              leave-from-class="transform scale-100 opacity-100"
              leave-to-class="transform scale-95 opacity-0"
            >
              <div
                v-if="showCategoriesMenu"
                class="absolute top-full left-0 mt-2 w-56 bg-white rounded-lg shadow-xl border border-gray-100 py-2 z-50 max-h-96 overflow-y-auto"
              >
                <router-link
                  v-for="category in categories"
                  :key="category.id"
                  :to="`/products?category=${encodeURIComponent(category.name)}`"
                  @click="showCategoriesMenu = false"
                  class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 transition-colors"
                >
                  {{ category.name }}
                </router-link>
                <div v-if="categories.length === 0" class="px-4 py-2 text-gray-400 text-sm">
                  Chưa có danh mục
                </div>
              </div>
            </Transition>
          </div>

          <!-- Brands Dropdown -->
          <div class="relative group" ref="brandsDropdownRef">
            <button
              @click.stop="toggleBrandsMenu"
              class="text-gray-700 hover:text-primary-500 font-medium transition-colors duration-200 flex items-center space-x-1"
            >
              <span>Nhãn hàng</span>
              <svg class="w-4 h-4 transition-transform duration-200" :class="{ 'rotate-180': showBrandsMenu }" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
              </svg>
            </button>
            <Transition
              enter-active-class="transition duration-200 ease-out"
              enter-from-class="transform scale-95 opacity-0"
              enter-to-class="transform scale-100 opacity-100"
              leave-active-class="transition duration-150 ease-in"
              leave-from-class="transform scale-100 opacity-100"
              leave-to-class="transform scale-95 opacity-0"
            >
              <div
                v-if="showBrandsMenu"
                class="absolute top-full left-0 mt-2 w-56 bg-white rounded-lg shadow-xl border border-gray-100 py-2 z-50 max-h-96 overflow-y-auto"
              >
                <router-link
                  v-for="brand in brands"
                  :key="brand"
                  :to="`/products?brand=${encodeURIComponent(brand)}`"
                  @click="showBrandsMenu = false"
                  class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 transition-colors"
                >
                  {{ brand }}
                </router-link>
                <div v-if="brands.length === 0" class="px-4 py-2 text-gray-400 text-sm">
                  Chưa có nhãn hàng
                </div>
              </div>
            </Transition>
          </div>
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
              v-if="cartTotalItems > 0"
              class="absolute -top-1 -right-1 bg-primary-500 text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center group-hover:scale-110 transition-transform"
            >
              {{ cartTotalItems }}
            </span>
          </router-link>

          <!-- User Menu -->
          <div class="relative" v-if="authStore.isAuthenticated">
            <button
              @click="showUserMenu = !showUserMenu"
              class="flex items-center space-x-2 p-2 rounded-lg hover:bg-gray-100 transition-all duration-300"
            >
              <div class="w-8 h-8 rounded-full flex items-center justify-center overflow-hidden border-2 border-gray-200">
                <img
                  v-if="displayUser?.avatarUrl"
                  :src="displayUser.avatarUrl"
                  :alt="displayUser?.fullName || displayUser?.email || 'User'"
                  class="w-full h-full object-cover"
                  @error="handleAvatarError"
                />
                <div
                  v-else
                  class="w-full h-full bg-primary-500 flex items-center justify-center"
                >
                  <span class="text-white font-semibold text-sm">{{ userInitial }}</span>
                </div>
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
                  to="/orders"
                  class="block px-4 py-2 text-gray-700 hover:bg-primary-50 hover:text-primary-600 transition-colors"
                  @click="showUserMenu = false"
                >
                  Đơn hàng của tôi
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
          
          <!-- Mobile Categories -->
          <div class="border-t border-gray-200 pt-2 mt-2">
            <div class="px-4 py-2 font-semibold text-gray-900">Danh mục</div>
            <router-link
              v-for="category in categories"
              :key="category.id"
              :to="`/products?category=${encodeURIComponent(category.name)}`"
              @click="showMobileMenu = false"
              class="block px-6 py-2 text-gray-600 hover:bg-primary-50 hover:text-primary-600 rounded-lg transition-colors text-sm"
            >
              {{ category.name }}
            </router-link>
          </div>

          <!-- Mobile Brands -->
          <div class="border-t border-gray-200 pt-2 mt-2">
            <div class="px-4 py-2 font-semibold text-gray-900">Nhãn hàng</div>
            <router-link
              v-for="brand in brands"
              :key="brand"
              :to="`/products?brand=${encodeURIComponent(brand)}`"
              @click="showMobileMenu = false"
              class="block px-6 py-2 text-gray-600 hover:bg-primary-50 hover:text-primary-600 rounded-lg transition-colors text-sm"
            >
              {{ brand }}
            </router-link>
          </div>

          <router-link
            v-if="!authStore.isAuthenticated"
            to="/login"
            @click="showMobileMenu = false"
            class="block px-4 py-2 bg-primary-500 text-white text-center rounded-lg hover:bg-primary-600 transition-colors mt-4"
          >
            Đăng nhập
          </router-link>
        </div>
      </div>
    </Transition>
  </nav>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import { useCartStore } from '../../stores/cart'
import { categoryApi, brandBannerApi, accountApi } from '../../services/api'

const router = useRouter()
const authStore = useAuthStore()
const cartStore = useCartStore()

const showMobileMenu = ref(false)
const showUserMenu = ref(false)
const showCategoriesMenu = ref(false)
const showBrandsMenu = ref(false)

const categories = ref([])
const brands = ref([])
const categoriesDropdownRef = ref(null)
const brandsDropdownRef = ref(null)
const fullUserData = ref(null)

const navLinks = [
  { name: 'Trang chủ', path: '/' },
  { name: 'Sản phẩm', path: '/products' },
  { name: 'Liên hệ', path: '/contact' },
]

// Use full user data if available, otherwise use auth user
const displayUser = computed(() => fullUserData.value || authStore.user)

// Cart total items - computed to ensure reactivity
const cartTotalItems = computed(() => {
  return cartStore.totalItems
})

const userInitial = computed(() => {
  const currentUser = displayUser.value || authStore.user
  if (currentUser?.fullName) {
    const names = currentUser.fullName.split(' ')
    return names.length > 1
      ? `${names[0][0]}${names[names.length - 1][0]}`.toUpperCase()
      : names[0][0].toUpperCase()
  }
  if (currentUser?.email) {
    return currentUser.email.substring(0, 2).toUpperCase()
  }
  if (currentUser?.name) {
    return currentUser.name.charAt(0).toUpperCase()
  }
  return 'U'
})

function handleAvatarError(event) {
  // Hide the image and show placeholder
  event.target.style.display = 'none'
  const parent = event.target.parentElement
  if (parent) {
    const placeholder = parent.querySelector('.avatar-placeholder') || document.createElement('div')
    placeholder.className = 'avatar-placeholder w-full h-full bg-primary-500 flex items-center justify-center'
    placeholder.innerHTML = `<span class="text-white font-semibold text-sm">${userInitial.value}</span>`
    if (!parent.querySelector('.avatar-placeholder')) {
      parent.appendChild(placeholder)
    }
  }
}

function handleLogout() {
  authStore.logout()
  showUserMenu.value = false
  router.push('/login')
}

function toggleCategoriesMenu() {
  showCategoriesMenu.value = !showCategoriesMenu.value
  // Close brands menu when opening categories
  if (showCategoriesMenu.value) {
    showBrandsMenu.value = false
  }
}

function toggleBrandsMenu() {
  showBrandsMenu.value = !showBrandsMenu.value
  // Close categories menu when opening brands
  if (showBrandsMenu.value) {
    showCategoriesMenu.value = false
  }
}

// Handle click outside for dropdowns
function handleClickOutside(event) {
  // Close categories menu if click is outside
  if (showCategoriesMenu.value && categoriesDropdownRef.value) {
    if (!categoriesDropdownRef.value.contains(event.target)) {
      showCategoriesMenu.value = false
    }
  }
  
  // Close brands menu if click is outside
  if (showBrandsMenu.value && brandsDropdownRef.value) {
    if (!brandsDropdownRef.value.contains(event.target)) {
      showBrandsMenu.value = false
    }
  }
}

// Fetch categories and brands and setup click outside listener
onMounted(async () => {
  try {
    const promises = [
      categoryApi.getAll(),
      brandBannerApi.getDistinctBrands()
    ]
    
    // Fetch full user data if authenticated
    if (authStore.isAuthenticated) {
      promises.push(
        accountApi.getMe().catch(err => {
          console.error('Failed to fetch user data:', err)
          return null
        })
      )
    }
    
    const results = await Promise.all(promises)
    categories.value = results[0].filter(c => c.status === 'Active')
    brands.value = results[1] || []
    
    // Set full user data if fetched
    if (results[2]) {
      fullUserData.value = results[2]
    }
  } catch (error) {
    console.error('Error fetching data:', error)
  }
  
  // Add click outside listener
  document.addEventListener('click', handleClickOutside)
  
  // Fetch cart if user is authenticated
  if (authStore.isAuthenticated) {
    cartStore.fetchCart().catch(err => {
      console.error('Error fetching cart:', err)
    })
  }
})

// Watch auth state to fetch cart when user logs in
watch(() => authStore.isAuthenticated, async (isAuthenticated) => {
  if (isAuthenticated) {
    try {
      await cartStore.fetchCart()
    } catch (err) {
      console.error('Error fetching cart:', err)
    }
  } else {
    // Clear cart when user logs out
    cartStore.items = []
  }
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

