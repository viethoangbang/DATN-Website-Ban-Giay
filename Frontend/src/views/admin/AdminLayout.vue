<template>
  <div class="flex h-screen overflow-hidden bg-gray-100">
    <!-- Mobile Overlay -->
    <div
      v-if="isMobileMenuOpen"
      class="fixed inset-0 bg-black bg-opacity-50 z-40 lg:hidden"
      @click="isMobileMenuOpen = false"
    ></div>

    <!-- Sidebar -->
    <aside
      class="fixed lg:static inset-y-0 left-0 z-50 flex-shrink-0 transition-transform duration-300 ease-in-out"
      :class="isMobileMenuOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'"
    >
      <AdminSidebar @close-mobile="isMobileMenuOpen = false" />
    </aside>

    <!-- Main Content -->
    <div class="flex flex-1 flex-col overflow-hidden lg:ml-0">
      <!-- Header -->
      <header class="bg-white shadow-sm">
        <div class="flex h-16 items-center justify-between px-4 lg:px-6">
          <!-- Mobile Menu Button -->
          <button
            @click="isMobileMenuOpen = !isMobileMenuOpen"
            class="lg:hidden p-2 rounded-lg text-gray-600 hover:bg-gray-100"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-6 w-6"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M4 6h16M4 12h16M4 18h16"
              />
            </svg>
          </button>
          <h1 class="text-xl lg:text-2xl font-semibold text-gray-800 truncate">{{ pageTitle }}</h1>
          <div class="flex items-center space-x-4">
            <!-- Notifications -->
            <button
              class="rounded-full p-2 text-gray-600 hover:bg-gray-100 hover:text-gray-800"
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-6 w-6"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9"
                />
              </svg>
            </button>
          </div>
        </div>
      </header>

      <!-- Content Area -->
      <main class="flex-1 overflow-y-auto p-4 lg:p-6">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, onMounted, onUnmounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import AdminSidebar from '@/components/admin/AdminSidebar.vue'

const route = useRoute()
const isMobileMenuOpen = ref(false)

// Close mobile menu on route change
watch(() => route.path, () => {
  isMobileMenuOpen.value = false
})

// Close mobile menu on window resize (when switching to desktop)
const handleResize = () => {
  if (window.innerWidth >= 1024) {
    isMobileMenuOpen.value = false
  }
}

onMounted(() => {
  window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
})

const pageTitle = computed(() => {
  const titles = {
    '/admin': 'Dashboard',
    '/admin/products': 'Quản lý sản phẩm',
    '/admin/categories': 'Quản lý danh mục',
    '/admin/colors': 'Quản lý màu sắc',
    '/admin/sizes': 'Quản lý kích cỡ',
    '/admin/variants': 'Quản lý biến thể sản phẩm',
    '/admin/images': 'Quản lý hình ảnh',
    '/admin/hero-images': 'Quản lý Hero Banner',
    '/admin/orders': 'Quản lý đơn hàng',
    '/admin/users': 'Quản lý người dùng',
    '/admin/vouchers': 'Quản lý voucher',
    '/admin/reports': 'Báo cáo'
  }
  
  for (const [path, title] of Object.entries(titles)) {
    if (route.path.startsWith(path)) {
      return title
    }
  }
  
  return 'Admin Panel'
})
</script>
