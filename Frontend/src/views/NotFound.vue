<template>
  <div class="min-h-screen bg-gray-50 flex flex-col">
    <Navbar />
    
    <div class="flex-1 flex items-center justify-center px-4 sm:px-6 lg:px-8 py-16">
      <div class="max-w-3xl mx-auto text-center">
        <div class="animate-fade-in-up">
          <!-- 404 Illustration -->
          <div class="mb-8">
            <svg class="w-64 h-64 mx-auto text-primary-500" viewBox="0 0 200 200" fill="none">
              <circle cx="100" cy="100" r="90" stroke="currentColor" stroke-width="4" opacity="0.1"/>
              <!-- Left shoe -->
              <path d="M60 80 L70 90 L65 100 L55 95 Z" fill="currentColor" opacity="0.3" class="animate-wiggle"/>
              <!-- Right shoe -->
              <path d="M140 80 L130 90 L135 100 L145 95 Z" fill="currentColor" opacity="0.3" class="animate-wiggle delay-100"/>
              <!-- Sad face -->
              <circle cx="80" cy="90" r="4" fill="currentColor"/>
              <circle cx="120" cy="90" r="4" fill="currentColor"/>
              <path d="M75 115 Q100 105 125 115" stroke="currentColor" stroke-width="3" stroke-linecap="round" fill="none"/>
              <!-- 404 Text -->
              <text x="100" y="145" text-anchor="middle" font-size="40" font-weight="bold" fill="currentColor">404</text>
            </svg>
          </div>

          <!-- Error Message -->
          <h1 class="text-4xl md:text-6xl font-bold text-gray-900 mb-4">
            Oops! Không tìm thấy trang
          </h1>
          <p class="text-xl text-gray-600 mb-8">
            Trang bạn đang tìm kiếm không tồn tại hoặc đã được di chuyển.
          </p>

          <!-- Quick Links -->
          <div class="flex flex-col sm:flex-row gap-4 justify-center mb-12">
            <router-link to="/" class="btn-primary">
              <svg class="w-5 h-5 mr-2 inline-block" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" />
              </svg>
              Về trang chủ
            </router-link>
            <router-link to="/products" class="btn-secondary">
              <svg class="w-5 h-5 mr-2 inline-block" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
              </svg>
              Xem sản phẩm
            </router-link>
          </div>

          <!-- Search Box -->
          <div class="max-w-md mx-auto mb-12">
            <label class="block text-sm font-semibold text-gray-700 mb-2 text-left">
              Hoặc tìm kiếm sản phẩm
            </label>
            <div class="relative">
              <input
                v-model="searchQuery"
                @keyup.enter="handleSearch"
                type="text"
                placeholder="Nhập tên sản phẩm..."
                class="input-field pl-12"
              />
              <svg class="w-6 h-6 text-gray-400 absolute left-4 top-1/2 -translate-y-1/2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
              <button
                @click="handleSearch"
                class="absolute right-2 top-1/2 -translate-y-1/2 px-4 py-2 bg-primary-500 text-white rounded-lg hover:bg-primary-600 transition-colors"
              >
                Tìm
              </button>
            </div>
          </div>

          <!-- Popular Links -->
          <div class="bg-white rounded-xl shadow-md p-8">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Trang phổ biến</h2>
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4 text-left">
              <router-link
                v-for="link in popularLinks"
                :key="link.path"
                :to="link.path"
                class="flex items-center space-x-3 p-4 rounded-lg hover:bg-gray-50 transition-colors group"
              >
                <div class="w-10 h-10 bg-primary-100 rounded-lg flex items-center justify-center group-hover:bg-primary-500 transition-colors">
                  <component :is="link.icon" class="w-5 h-5 text-primary-600 group-hover:text-white" />
                </div>
                <div>
                  <div class="font-semibold text-gray-900 group-hover:text-primary-600">{{ link.name }}</div>
                  <div class="text-sm text-gray-600">{{ link.description }}</div>
                </div>
              </router-link>
            </div>
          </div>

          <!-- Contact Support -->
          <div class="mt-12 p-6 bg-primary-50 rounded-xl">
            <p class="text-gray-700 mb-4">
              Cần hỗ trợ? Đội ngũ của chúng tôi luôn sẵn sàng giúp đỡ bạn
            </p>
            <router-link to="/contact" class="text-primary-600 hover:text-primary-700 font-semibold">
              Liên hệ với chúng tôi →
            </router-link>
          </div>
        </div>
      </div>
    </div>

    <Footer />
  </div>
</template>

<script setup>
import { ref, h } from 'vue'
import { useRouter } from 'vue-router'
import Navbar from '../components/user/Navbar.vue'
import Footer from '../components/user/Footer.vue'

const router = useRouter()
const searchQuery = ref('')

// Icons
const HomeIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6' })
])

const ProductsIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z' })
])

const ContactIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z' })
])

const AboutIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z' })
])

const popularLinks = [
  {
    path: '/',
    name: 'Trang chủ',
    description: 'Khám phá bộ sưu tập mới',
    icon: HomeIcon
  },
  {
    path: '/products',
    name: 'Sản phẩm',
    description: 'Xem tất cả sản phẩm',
    icon: ProductsIcon
  },
  {
    path: '/about',
    name: 'Giới thiệu',
    description: 'Tìm hiểu về chúng tôi',
    icon: AboutIcon
  },
  {
    path: '/contact',
    name: 'Liên hệ',
    description: 'Nhận hỗ trợ nhanh chóng',
    icon: ContactIcon
  }
]

function handleSearch() {
  if (searchQuery.value.trim()) {
    router.push({ path: '/products', query: { search: searchQuery.value } })
  }
}
</script>

