<template>
  <div 
    class="flex h-screen flex-col bg-gray-900 transition-all duration-300 ease-in-out"
    :class="isCollapsed ? 'w-20' : 'w-64'"
  >
    <!-- Logo & Toggle -->
    <div 
      class="flex h-16 items-center justify-between border-b border-gray-800"
      :class="isCollapsed ? 'px-2' : 'px-4'"
    >
      <router-link 
        to="/admin"
        @click="emit('close-mobile')"
        class="flex items-center space-x-2 min-w-0"
        :class="{ 'hidden': isCollapsed }"
      >
        <img src="@/assets/logo.png" alt="Logo" class="h-10 w-10" />
        <span class="text-xl font-bold text-white">Admin</span>
      </router-link>
      <button
        @click="toggleSidebar"
        class="p-2 rounded-lg text-gray-400 hover:bg-gray-800 hover:text-white"
        :title="isCollapsed ? 'Mở rộng' : 'Thu gọn'"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-5 w-5 transition-transform"
          :class="{ 'rotate-180': isCollapsed }"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 19l-7-7 7-7m8 14l-7-7 7-7" />
        </svg>
      </button>
    </div>

    <!-- Navigation -->
    <nav 
      class="flex-1 overflow-y-auto py-6"
      :class="isCollapsed ? 'px-2' : 'px-4'"
    >
      <ul class="space-y-2">
        <li v-for="item in menuItems" :key="item.path">
          <router-link
            :to="item.path"
            @click="emit('close-mobile')"
            class="group relative flex items-center rounded-lg text-gray-300 transition-colors hover:bg-gray-800 hover:text-white"
            :class="{
              'bg-gray-800 text-white': isActive(item.path),
              'justify-center px-2 py-3': isCollapsed,
              'px-4 py-3': !isCollapsed
            }"
            :title="isCollapsed ? item.label : ''"
          >
            <component :is="item.icon" class="h-5 w-5 flex-shrink-0" />
            <span v-if="!isCollapsed" class="font-medium ml-3">
              {{ item.label }}
            </span>
            <!-- Tooltip when collapsed -->
            <div
              v-if="isCollapsed"
              class="absolute left-full ml-2 px-3 py-2 bg-gray-800 text-white text-sm rounded-lg opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-all whitespace-nowrap z-50"
            >
              {{ item.label }}
            </div>
          </router-link>
        </li>
      </ul>
    </nav>

    <!-- User Profile -->
    <div 
      class="border-t border-gray-800"
      :class="isCollapsed ? 'p-2' : 'p-4'"
    >
      <div class="flex items-center" :class="isCollapsed ? 'justify-center' : 'space-x-3'">
        <div class="flex-shrink-0">
          <div class="flex h-10 w-10 items-center justify-center rounded-full bg-indigo-600">
            <span class="text-sm font-semibold text-white">{{ userInitials }}</span>
          </div>
        </div>
        <div v-if="!isCollapsed" class="flex-1 min-w-0">
          <p class="text-sm font-medium text-white truncate">{{ user?.fullName || user?.email || 'Admin' }}</p>
          <div v-if="userRoles.length > 0" class="flex flex-wrap gap-1 mt-1">
            <span
              v-for="role in userRoles"
              :key="role"
              :class="getRoleBadgeClass(role)"
              class="text-xs px-1.5 py-0.5 rounded font-medium"
            >
              {{ role }}
            </span>
          </div>
        </div>
        <button
          @click="handleLogout"
          class="flex-shrink-0 rounded-lg p-2 text-gray-400 hover:bg-gray-800 hover:text-white"
          :title="isCollapsed ? 'Đăng xuất' : ''"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
          </svg>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, h } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const emit = defineEmits(['close-mobile'])

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const user = computed(() => authStore.user)
const isCollapsed = ref(false)

const userInitials = computed(() => {
  if (user.value?.fullName) {
    const names = user.value.fullName.split(' ')
    return names.length > 1
      ? `${names[0][0]}${names[names.length - 1][0]}`.toUpperCase()
      : names[0][0].toUpperCase()
  }
  if (user.value?.email) {
    return user.value.email.substring(0, 2).toUpperCase()
  }
  return 'AD'
})

const userRoles = computed(() => user.value?.roles || [])

const getRoleBadgeClass = (role) => {
  const classes = {
    'Admin': 'bg-red-500/20 text-red-300',
    'Manager': 'bg-blue-500/20 text-blue-300',
    'Staff': 'bg-green-500/20 text-green-300',
    'Customer': 'bg-gray-500/20 text-gray-300'
  }
  return classes[role] || 'bg-gray-500/20 text-gray-300'
}

// Icon Components
const DashboardIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6' })
])

const OrderIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z' })
])

const ProductIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4' })
])

const UserIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z' })
])

const VoucherIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M15 5v2m0 4v2m0 4v2M5 5a2 2 0 00-2 2v3a2 2 0 110 4v3a2 2 0 002 2h14a2 2 0 002-2v-3a2 2 0 110-4V7a2 2 0 00-2-2H5z' })
])

const ReportIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z' })
])

const menuItems = [
  {
    path: '/admin',
    label: 'Dashboard',
    icon: DashboardIcon
  },
  {
    path: '/admin/orders',
    label: 'Đơn hàng',
    icon: OrderIcon
  },
  {
    path: '/admin/products',
    label: 'Quản lý sản phẩm',
    icon: ProductIcon
  },
  {
    path: '/admin/users',
    label: 'Tài khoản',
    icon: UserIcon
  },
  {
    path: '/admin/vouchers',
    label: 'Voucher',
    icon: VoucherIcon
  },
  {
    path: '/admin/reports',
    label: 'Báo cáo',
    icon: ReportIcon
  }
]

const isActive = (path) => {
  if (path === '/admin') {
    return route.path === '/admin'
  }
  return route.path.startsWith(path)
}

const toggleSidebar = () => {
  isCollapsed.value = !isCollapsed.value
}

const handleLogout = async () => {
  await authStore.logout()
  router.push('/login')
}
</script>
