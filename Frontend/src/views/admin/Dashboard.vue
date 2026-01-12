<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Loading State -->
    <div v-if="loading" class="flex items-center justify-center h-64">
      <div class="text-center">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
        <p class="mt-4 text-gray-600">Đang tải dữ liệu...</p>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="bg-red-50 border border-red-200 rounded-lg p-4">
      <p class="text-red-800">{{ error }}</p>
      <button @click="loadDashboardData" class="mt-2 text-red-600 hover:text-red-800 underline">
        Thử lại
      </button>
    </div>

    <!-- Dashboard Content -->
    <div v-else>
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
                <p class="font-semibold text-gray-900">#{{ order.code }}</p>
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
  </div>
</template>

<script setup>
import { ref, onMounted, h, reactive, computed } from 'vue'
import { Chart, registerables } from 'chart.js'
import { dashboardApi } from '@/services/api'
import { handleApiError } from '@/services/api'

Chart.register(...registerables)

const revenueChart = ref(null)
const categoryChart = ref(null)
const loading = ref(true)
const error = ref(null)

const dashboardData = reactive({
  stats: null,
  recentOrders: [],
  topProducts: [],
  monthlyRevenue: [],
  categoryRevenue: []
})

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

const stats = ref([
  {
    title: 'Tổng doanh thu',
    value: '₫0',
    change: '0%',
    trend: 'up',
    color: 'bg-gradient-to-br from-blue-500 to-blue-600',
    icon: RevenueIcon
  },
  {
    title: 'Đơn hàng',
    value: '0',
    change: '0%',
    trend: 'up',
    color: 'bg-gradient-to-br from-green-500 to-green-600',
    icon: OrderIcon
  },
  {
    title: 'Khách hàng',
    value: '0',
    change: '0%',
    trend: 'up',
    color: 'bg-gradient-to-br from-purple-500 to-purple-600',
    icon: CustomerIcon
  },
  {
    title: 'Sản phẩm',
    value: '0',
    change: '0%',
    trend: 'down',
    color: 'bg-gradient-to-br from-primary-500 to-primary-600',
    icon: ProductIcon
  }
])

const recentOrders = ref([])

const statusClasses = {
  'Hoàn thành': 'bg-green-100 text-green-800',
  'Đang giao': 'bg-blue-100 text-blue-800',
  'Đang xử lý': 'bg-yellow-100 text-yellow-800',
  'Đã hủy': 'bg-red-100 text-red-800',
}

const topProducts = ref([])

const NotificationIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9' })
])

const UserAddIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z' })
])

const activities = computed(() => {
  return recentOrders.value.slice(0, 4).map(order => ({
    id: order.id,
    title: `Đơn hàng mới #${order.code}`,
    description: `${order.customer} đã đặt đơn hàng trị giá ${formatPrice(order.amount)}`,
    time: getTimeAgo(order.createDate),
    color: order.status === 'Hoàn thành' ? 'bg-green-500' : 
            order.status === 'Đang giao' ? 'bg-blue-500' : 
            order.status === 'Đang xử lý' ? 'bg-yellow-500' : 'bg-gray-500',
    icon: OrderIcon
  }))
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function formatNumber(num) {
  if (num >= 1000000000) {
    return (num / 1000000000).toFixed(1) + 'B'
  }
  if (num >= 1000000) {
    return (num / 1000000).toFixed(1) + 'M'
  }
  if (num >= 1000) {
    return (num / 1000).toFixed(1) + 'K'
  }
  return num.toString()
}

function formatChange(percent) {
  const sign = percent >= 0 ? '+' : ''
  return `${sign}${percent.toFixed(1)}%`
}

function getTimeAgo(date) {
  const now = new Date()
  const diff = now - new Date(date)
  const minutes = Math.floor(diff / 60000)
  const hours = Math.floor(minutes / 60)
  const days = Math.floor(hours / 24)
  
  if (minutes < 1) return 'Vừa xong'
  if (minutes < 60) return `${minutes} phút trước`
  if (hours < 24) return `${hours} giờ trước`
  return `${days} ngày trước`
}

async function loadDashboardData() {
  try {
    loading.value = true
    error.value = null
    const data = await dashboardApi.getStats()
    
    // Update stats
    stats.value[0].value = formatPrice(data.stats.totalRevenue)
    stats.value[0].change = formatChange(data.stats.revenueChangePercent)
    stats.value[0].trend = data.stats.revenueChangePercent >= 0 ? 'up' : 'down'
    
    stats.value[1].value = data.stats.totalOrders.toLocaleString('vi-VN')
    stats.value[1].change = formatChange(data.stats.ordersChangePercent)
    stats.value[1].trend = data.stats.ordersChangePercent >= 0 ? 'up' : 'down'
    
    stats.value[2].value = data.stats.totalCustomers.toLocaleString('vi-VN')
    stats.value[2].change = formatChange(data.stats.customersChangePercent)
    stats.value[2].trend = data.stats.customersChangePercent >= 0 ? 'up' : 'down'
    
    stats.value[3].value = data.stats.totalProducts.toLocaleString('vi-VN')
    stats.value[3].change = formatChange(data.stats.productsChangePercent)
    stats.value[3].trend = data.stats.productsChangePercent >= 0 ? 'up' : 'down'
    
    // Update recent orders
    recentOrders.value = data.recentOrders.map(order => ({
      id: order.id,
      code: order.code,
      customer: order.customerName,
      amount: order.total,
      status: order.status,
      createDate: order.createDate
    }))
    
    // Update top products
    topProducts.value = data.topProducts.map(product => ({
      id: product.id,
      name: product.name,
      sold: product.sold,
      revenue: product.revenue,
      image: product.imageUrl || 'https://via.placeholder.com/100'
    }))
    
    // Update charts
    updateCharts(data.monthlyRevenue, data.categoryRevenue)
    
  } catch (err) {
    error.value = handleApiError(err)
    console.error('Error loading dashboard data:', err)
  } finally {
    loading.value = false
  }
}

function updateCharts(monthlyData, categoryData) {
  // Revenue Chart
  if (revenueChart.value) {
    new Chart(revenueChart.value, {
      type: 'line',
      data: {
        labels: monthlyData.map(m => m.month),
        datasets: [{
          label: 'Doanh thu',
          data: monthlyData.map(m => m.revenue / 1000000), // Convert to millions
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
    const colors = ['#f97316', '#3b82f6', '#10b981', '#8b5cf6', '#ec4899', '#f59e0b', '#6366f1']
    new Chart(categoryChart.value, {
      type: 'doughnut',
      data: {
        labels: categoryData.map(c => c.categoryName),
        datasets: [{
          data: categoryData.map(c => c.revenue),
          backgroundColor: colors.slice(0, categoryData.length),
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
}

onMounted(() => {
  loadDashboardData()
})
</script>

