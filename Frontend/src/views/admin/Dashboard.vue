<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Stats Cards -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <div
        v-for="(stat, index) in stats"
        :key="stat.title"
        class="bg-white rounded-xl shadow-md p-6 transition-all duration-300 hover:shadow-xl hover:-translate-y-1 animate-fade-in-up"
        :style="{ animationDelay: `${index * 0.1}s` }"
      >
        <div class="flex items-center justify-between">
          <div>
            <p class="text-gray-600 text-sm font-medium">{{ stat.title }}</p>
            <h3 class="text-3xl font-bold text-gray-900 mt-2">{{ stat.value }}</h3>
            <div class="flex items-center mt-2 space-x-1">
              <svg
                v-if="stat.trend === 'up'"
                class="w-4 h-4 text-green-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 10l7-7m0 0l7 7m-7-7v18" />
              </svg>
              <svg
                v-else
                class="w-4 h-4 text-red-500"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 14l-7 7m0 0l-7-7m7 7V3" />
              </svg>
              <span :class="stat.trend === 'up' ? 'text-green-600' : 'text-red-600'" class="text-sm font-semibold">
                {{ stat.change }}
              </span>
              <span class="text-gray-500 text-sm">so với tháng trước</span>
            </div>
          </div>
          <div :class="`w-16 h-16 ${stat.color} rounded-full flex items-center justify-center`">
            <component :is="stat.icon" class="w-8 h-8 text-white" />
          </div>
        </div>
      </div>
    </div>

    <!-- Charts Row -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Revenue Chart -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.4s;">
        <div class="flex items-center justify-between mb-6">
          <h3 class="text-lg font-bold text-gray-900">Doanh thu theo tháng</h3>
          <select class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
            <option>2024</option>
            <option>2023</option>
          </select>
        </div>
        <div class="h-80">
          <canvas ref="revenueChart"></canvas>
        </div>
      </div>

      <!-- Category Chart -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.5s;">
        <h3 class="text-lg font-bold text-gray-900 mb-6">Doanh thu theo danh mục</h3>
        <div class="h-80 flex items-center justify-center">
          <canvas ref="categoryChart"></canvas>
        </div>
      </div>
    </div>

    <!-- Recent Orders & Top Products -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Recent Orders -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.6s;">
        <div class="flex items-center justify-between mb-6">
          <h3 class="text-lg font-bold text-gray-900">Đơn hàng gần đây</h3>
          <router-link to="/admin/orders" class="text-primary-600 hover:text-primary-700 text-sm font-semibold">
            Xem tất cả →
          </router-link>
        </div>
        <div class="space-y-4">
          <div
            v-for="order in recentOrders"
            :key="order.id"
            class="flex items-center justify-between p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
          >
            <div class="flex items-center space-x-4">
              <div class="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center">
                <svg class="w-6 h-6 text-primary-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
                </svg>
              </div>
              <div>
                <p class="font-semibold text-gray-900">#{{ order.id }}</p>
                <p class="text-sm text-gray-600">{{ order.customer }}</p>
              </div>
            </div>
            <div class="text-right">
              <p class="font-bold text-gray-900">{{ formatPrice(order.amount) }}</p>
              <span :class="statusClasses[order.status]" class="text-xs px-2 py-1 rounded-full">
                {{ order.status }}
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- Top Products -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.7s;">
        <div class="flex items-center justify-between mb-6">
          <h3 class="text-lg font-bold text-gray-900">Sản phẩm bán chạy</h3>
          <router-link to="/admin/products" class="text-primary-600 hover:text-primary-700 text-sm font-semibold">
            Xem tất cả →
          </router-link>
        </div>
        <div class="space-y-4">
          <div
            v-for="(product, index) in topProducts"
            :key="product.id"
            class="flex items-center space-x-4 p-4 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
          >
            <div class="flex-shrink-0 w-8 h-8 bg-primary-500 text-white rounded-full flex items-center justify-center font-bold">
              {{ index + 1 }}
            </div>
            <img :src="product.image" :alt="product.name" class="w-12 h-12 object-cover rounded-lg" />
            <div class="flex-1 min-w-0">
              <p class="font-semibold text-gray-900 truncate">{{ product.name }}</p>
              <p class="text-sm text-gray-600">Đã bán: {{ product.sold }}</p>
            </div>
            <div class="text-right">
              <p class="font-bold text-primary-600">{{ formatPrice(product.revenue) }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Activity Timeline -->
    <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.8s;">
      <h3 class="text-lg font-bold text-gray-900 mb-6">Hoạt động gần đây</h3>
      <div class="space-y-4">
        <div
          v-for="activity in activities"
          :key="activity.id"
          class="flex items-start space-x-4 pb-4 border-b border-gray-100 last:border-0"
        >
          <div :class="`w-10 h-10 ${activity.color} rounded-full flex items-center justify-center flex-shrink-0`">
            <component :is="activity.icon" class="w-5 h-5 text-white" />
          </div>
          <div class="flex-1">
            <p class="text-gray-900 font-medium">{{ activity.title }}</p>
            <p class="text-sm text-gray-600">{{ activity.description }}</p>
            <p class="text-xs text-gray-500 mt-1">{{ activity.time }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, h } from 'vue'
import { Chart, registerables } from 'chart.js'

Chart.register(...registerables)

const revenueChart = ref(null)
const categoryChart = ref(null)

// Icons
const RevenueIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z' })
])

const OrderIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z' })
])

const CustomerIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z' })
])

const ProductIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4' })
])

const stats = [
  {
    title: 'Tổng doanh thu',
    value: '₫245M',
    change: '+12.5%',
    trend: 'up',
    color: 'bg-gradient-to-br from-blue-500 to-blue-600',
    icon: RevenueIcon
  },
  {
    title: 'Đơn hàng',
    value: '1,234',
    change: '+8.2%',
    trend: 'up',
    color: 'bg-gradient-to-br from-green-500 to-green-600',
    icon: OrderIcon
  },
  {
    title: 'Khách hàng',
    value: '892',
    change: '+15.3%',
    trend: 'up',
    color: 'bg-gradient-to-br from-purple-500 to-purple-600',
    icon: CustomerIcon
  },
  {
    title: 'Sản phẩm',
    value: '456',
    change: '-2.1%',
    trend: 'down',
    color: 'bg-gradient-to-br from-primary-500 to-primary-600',
    icon: ProductIcon
  }
]

const recentOrders = [
  { id: '10234', customer: 'Nguyễn Văn A', amount: 3500000, status: 'Hoàn thành' },
  { id: '10233', customer: 'Trần Thị B', amount: 2800000, status: 'Đang giao' },
  { id: '10232', customer: 'Lê Văn C', amount: 4200000, status: 'Đang xử lý' },
  { id: '10231', customer: 'Phạm Thị D', amount: 1800000, status: 'Hoàn thành' },
]

const statusClasses = {
  'Hoàn thành': 'bg-green-100 text-green-800',
  'Đang giao': 'bg-blue-100 text-blue-800',
  'Đang xử lý': 'bg-yellow-100 text-yellow-800',
  'Đã hủy': 'bg-red-100 text-red-800',
}

const topProducts = [
  { id: 1, name: 'Nike Air Max 2024', sold: 234, revenue: 819000000, image: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=100' },
  { id: 2, name: 'Adidas Ultraboost', sold: 198, revenue: 831600000, image: 'https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=100' },
  { id: 3, name: 'Jordan Retro High', sold: 156, revenue: 858000000, image: 'https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=100' },
  { id: 4, name: 'Puma RS-X', sold: 142, revenue: 397600000, image: 'https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=100' },
]

const NotificationIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9' })
])

const UserAddIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z' })
])

const activities = [
  {
    id: 1,
    title: 'Đơn hàng mới #10234',
    description: 'Nguyễn Văn A đã đặt đơn hàng trị giá ₫3,500,000',
    time: '5 phút trước',
    color: 'bg-blue-500',
    icon: OrderIcon
  },
  {
    id: 2,
    title: 'Khách hàng mới',
    description: 'Trần Thị B đã đăng ký tài khoản',
    time: '15 phút trước',
    color: 'bg-green-500',
    icon: UserAddIcon
  },
  {
    id: 3,
    title: 'Sản phẩm sắp hết hàng',
    description: 'Nike Air Max 2024 chỉ còn 5 sản phẩm',
    time: '1 giờ trước',
    color: 'bg-yellow-500',
    icon: NotificationIcon
  },
  {
    id: 4,
    title: 'Đơn hàng hoàn thành',
    description: 'Đơn hàng #10230 đã được giao thành công',
    time: '2 giờ trước',
    color: 'bg-primary-500',
    icon: OrderIcon
  },
]

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

onMounted(() => {
  // Revenue Chart
  if (revenueChart.value) {
    new Chart(revenueChart.value, {
      type: 'line',
      data: {
        labels: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6'],
        datasets: [{
          label: 'Doanh thu (triệu đồng)',
          data: [32, 38, 35, 45, 52, 48],
          borderColor: '#f97316',
          backgroundColor: 'rgba(249, 115, 22, 0.1)',
          tension: 0.4,
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              callback: (value) => value + 'M'
            }
          }
        }
      }
    })
  }

  // Category Chart
  if (categoryChart.value) {
    new Chart(categoryChart.value, {
      type: 'doughnut',
      data: {
        labels: ['Running', 'Basketball', 'Casual', 'Sports'],
        datasets: [{
          data: [35, 25, 30, 10],
          backgroundColor: [
            '#f97316',
            '#3b82f6',
            '#10b981',
            '#8b5cf6'
          ],
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom'
          }
        }
      }
    })
  }
})
</script>

