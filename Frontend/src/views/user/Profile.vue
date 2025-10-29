<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="mb-8 animate-fade-in-up">
        <h1 class="text-3xl font-bold text-gray-900 mb-2">Tài khoản của tôi</h1>
        <p class="text-gray-600">Quản lý thông tin cá nhân và đơn hàng</p>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-4 gap-8">
        <!-- Sidebar -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 animate-fade-in-up">
            <div class="flex flex-col items-center mb-6">
              <div class="w-24 h-24 bg-primary-500 rounded-full flex items-center justify-center mb-4">
                <span class="text-white text-3xl font-bold">{{ userInitial }}</span>
              </div>
              <h3 class="font-bold text-gray-900 text-lg">{{ authStore.user?.name }}</h3>
              <p class="text-gray-600 text-sm">{{ authStore.user?.email }}</p>
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
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Họ và tên</label>
                  <input
                    v-model="profileForm.name"
                    type="text"
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
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
                    v-model="profileForm.phone"
                    type="tel"
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
                  />
                </div>
                <div>
                  <label class="block text-sm font-semibold text-gray-700 mb-2">Ngày sinh</label>
                  <input
                    v-model="profileForm.birthday"
                    type="date"
                    :disabled="!editMode"
                    class="input-field disabled:bg-gray-100"
                  />
                </div>
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">Địa chỉ</label>
                <textarea
                  v-model="profileForm.address"
                  rows="3"
                  :disabled="!editMode"
                  class="input-field disabled:bg-gray-100"
                ></textarea>
              </div>

              <div v-if="editMode" class="flex space-x-3">
                <button type="submit" class="btn-primary">Lưu thay đổi</button>
                <button type="button" @click="cancelEdit" class="btn-secondary">Hủy</button>
              </div>
            </form>
          </div>

          <!-- Orders -->
          <div v-if="activeTab === 'orders'" class="space-y-4 animate-fade-in-up">
            <h2 class="text-xl font-bold text-gray-900 mb-4">Đơn hàng của tôi</h2>
            
            <div
              v-for="order in orders"
              :key="order.id"
              class="bg-white rounded-xl shadow-md p-6 hover:shadow-lg transition-shadow"
            >
              <div class="flex justify-between items-start mb-4">
                <div>
                  <h3 class="font-bold text-gray-900">Đơn hàng #{{ order.id }}</h3>
                  <p class="text-sm text-gray-600">{{ order.date }}</p>
                </div>
                <span :class="statusClasses[order.status]" class="px-4 py-2 rounded-full text-sm font-semibold">
                  {{ order.status }}
                </span>
              </div>

              <div class="space-y-3 mb-4">
                <div v-for="item in order.items" :key="item.id" class="flex items-center space-x-4">
                  <img :src="item.image" :alt="item.name" class="w-16 h-16 object-cover rounded-lg" />
                  <div class="flex-1">
                    <h4 class="font-semibold text-gray-900">{{ item.name }}</h4>
                    <p class="text-sm text-gray-600">Size: {{ item.size }} x {{ item.quantity }}</p>
                  </div>
                  <span class="font-bold text-gray-900">{{ formatPrice(item.price * item.quantity) }}</span>
                </div>
              </div>

              <div class="border-t pt-4 flex justify-between items-center">
                <span class="text-gray-700">Tổng cộng:</span>
                <span class="text-2xl font-bold text-primary-600">{{ formatPrice(order.total) }}</span>
              </div>

              <div class="mt-4 flex space-x-3">
                <button class="btn-primary">Xem chi tiết</button>
                <button v-if="order.status === 'Đang giao'" class="btn-secondary">Theo dõi đơn hàng</button>
              </div>
            </div>

            <EmptyState
              v-if="orders.length === 0"
              title="Chưa có đơn hàng"
              description="Bạn chưa có đơn hàng nào. Hãy bắt đầu mua sắm!"
              action-text="Mua sắm ngay"
              @action="$router.push('/products')"
            />
          </div>

          <!-- Wishlist -->
          <div v-if="activeTab === 'wishlist'" class="animate-fade-in-up">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Sản phẩm yêu thích</h2>
            
            <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
              <div
                v-for="product in wishlist"
                :key="product.id"
                class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition-shadow"
              >
                <div class="relative aspect-square">
                  <img :src="product.image" :alt="product.name" class="w-full h-full object-cover" />
                  <button
                    @click="removeFromWishlist(product.id)"
                    class="absolute top-4 right-4 w-10 h-10 bg-white rounded-full flex items-center justify-center hover:bg-red-500 hover:text-white transition-colors"
                  >
                    <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                      <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                    </svg>
                  </button>
                </div>
                <div class="p-4">
                  <h3 class="font-bold text-gray-900 mb-2">{{ product.name }}</h3>
                  <p class="text-xl font-bold text-primary-600 mb-3">{{ formatPrice(product.price) }}</p>
                  <button class="w-full btn-primary">Thêm vào giỏ</button>
                </div>
              </div>
            </div>

            <EmptyState
              v-if="wishlist.length === 0"
              title="Danh sách yêu thích trống"
              description="Bạn chưa có sản phẩm yêu thích nào"
              action-text="Khám phá sản phẩm"
              @action="$router.push('/products')"
            />
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
                class="border-2 rounded-lg p-4 hover:border-primary-500 transition-colors"
                :class="{ 'border-primary-500 bg-primary-50': address.isDefault }"
              >
                <div class="flex justify-between items-start mb-2">
                  <h4 class="font-bold text-gray-900">{{ address.name }}</h4>
                  <span v-if="address.isDefault" class="badge-primary">Mặc định</span>
                </div>
                <p class="text-gray-600 text-sm mb-1">{{ address.phone }}</p>
                <p class="text-gray-600 text-sm">{{ address.fullAddress }}</p>
                <div class="mt-4 flex space-x-2">
                  <button class="text-primary-600 hover:text-primary-700 text-sm font-semibold">Sửa</button>
                  <button v-if="!address.isDefault" class="text-red-600 hover:text-red-700 text-sm font-semibold">Xóa</button>
                  <button v-if="!address.isDefault" class="text-gray-600 hover:text-gray-700 text-sm font-semibold">Đặt mặc định</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <Footer />

    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />
  </div>
</template>

<script setup>
import { ref, reactive, computed, h } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/auth'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'
import EmptyState from '../../components/common/EmptyState.vue'

const router = useRouter()
const authStore = useAuthStore()

const activeTab = ref('profile')
const editMode = ref(false)
const showAddressModal = ref(false)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

// Icons
const UserIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z' })
])

const OrderIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z' })
])

const HeartIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z' })
])

const LocationIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z' })
])

const tabs = [
  { id: 'profile', name: 'Thông tin', icon: UserIcon },
  { id: 'orders', name: 'Đơn hàng', icon: OrderIcon },
  { id: 'wishlist', name: 'Yêu thích', icon: HeartIcon },
  { id: 'addresses', name: 'Địa chỉ', icon: LocationIcon }
]

const profileForm = reactive({
  name: 'Nguyễn Văn A',
  email: 'user@sneakerpoly.com',
  phone: '0912345678',
  birthday: '1990-01-01',
  address: 'Đường Trịnh Văn Bô, Nam Từ Liêm, Hà Nội'
})

const orders = ref([
  {
    id: '10234',
    date: '15/10/2024',
    status: 'Đang giao',
    total: 3500000,
    items: [
      { id: 1, name: 'Nike Air Max 2024', size: 42, quantity: 1, price: 3500000, image: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=100' }
    ]
  },
  {
    id: '10233',
    date: '10/10/2024',
    status: 'Hoàn thành',
    total: 4200000,
    items: [
      { id: 2, name: 'Adidas Ultraboost', size: 41, quantity: 1, price: 4200000, image: 'https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=100' }
    ]
  }
])

const wishlist = ref([
  { id: 1, name: 'Jordan Retro High', price: 5500000, image: 'https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=400' },
  { id: 2, name: 'Puma RS-X', price: 2800000, image: 'https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=400' }
])

const addresses = ref([
  {
    id: 1,
    name: 'Nhà riêng',
    phone: '0912345678',
    fullAddress: 'Đường Trịnh Văn Bô, Nam Từ Liêm, Hà Nội',
    isDefault: true
  },
  {
    id: 2,
    name: 'Văn phòng',
    phone: '0987654321',
    fullAddress: 'Số 1 Đại Cồ Việt, Hai Bà Trưng, Hà Nội',
    isDefault: false
  }
])

const statusClasses = {
  'Đang xử lý': 'bg-yellow-100 text-yellow-800',
  'Đang giao': 'bg-blue-100 text-blue-800',
  'Hoàn thành': 'bg-green-100 text-green-800',
  'Đã hủy': 'bg-red-100 text-red-800'
}

const userInitial = computed(() => {
  return authStore.user?.name?.charAt(0).toUpperCase() || 'U'
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function saveProfile() {
  // Save profile logic
  editMode.value = false
  toast.message = 'Đã cập nhật thông tin thành công'
  toast.type = 'success'
  toast.show = true
}

function cancelEdit() {
  editMode.value = false
  // Reset form
}

function removeFromWishlist(id) {
  wishlist.value = wishlist.value.filter(item => item.id !== id)
  toast.message = 'Đã xóa khỏi danh sách yêu thích'
  toast.type = 'success'
  toast.show = true
}
</script>

