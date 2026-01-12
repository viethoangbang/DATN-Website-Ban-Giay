<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Time Period Selector -->
    <div class="bg-white rounded-xl shadow-md p-6">
      <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h2 class="text-xl font-bold text-gray-900">Báo cáo thống kê</h2>
          <p class="text-gray-600 text-sm">Phân tích chi tiết về doanh thu và sản phẩm</p>
        </div>
        <div class="flex flex-wrap gap-3 items-center">
          <button
            v-for="period in periods"
            :key="period.value"
            @click="changePeriod(period.value)"
            :class="[
              'px-4 py-2 rounded-lg font-medium transition-all duration-300',
              selectedPeriod === period.value
                ? 'bg-primary-500 text-white shadow-lg'
                : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
            ]"
          >
            {{ period.label }}
          </button>
          <button
            @click="exportReport"
            :disabled="loading"
            class="px-4 py-2 rounded-lg font-medium bg-green-500 text-white hover:bg-green-600 transition-all duration-300 flex items-center space-x-2 disabled:opacity-50"
          >
            <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
            <span>Xuất file</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center items-center py-12">
      <svg class="animate-spin h-12 w-12 text-primary-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
      </svg>
    </div>

    <!-- Summary Cards -->
    <div v-if="!loading" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <div
        v-for="(summary, index) in summaryCards"
        :key="summary.title"
        class="bg-gradient-to-br from-white to-gray-50 rounded-xl shadow-md p-6 border-l-4 hover:shadow-xl transition-all duration-300 animate-fade-in-up"
        :style="{ borderColor: summary.color, animationDelay: `${index * 0.1}s` }"
      >
        <div class="flex items-center justify-between mb-4">
          <div :class="`w-12 h-12 ${summary.bgColor} rounded-lg flex items-center justify-center`">
            <svg class="w-6 h-6 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="summary.icon" />
            </svg>
          </div>
          <span :class="`text-sm font-semibold px-3 py-1 rounded-full ${summary.badgeClass}`">
            {{ summary.change }}
          </span>
        </div>
        <h3 class="text-gray-600 text-sm font-medium mb-1">{{ summary.title }}</h3>
        <p class="text-2xl font-bold text-gray-900">{{ summary.value }}</p>
      </div>
    </div>

    <!-- Charts Grid -->
    <div v-if="!loading" class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Sales Trend -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.4s;">
        <div class="flex items-center justify-between mb-6">
          <div>
            <h3 class="text-lg font-bold text-gray-900">Xu hướng doanh thu</h3>
            <p class="text-sm text-gray-600">So sánh theo thời gian</p>
          </div>
          <div class="flex items-center space-x-2">
            <span class="w-3 h-3 bg-primary-500 rounded-full"></span>
            <span class="text-sm text-gray-600">2024</span>
            <span class="w-3 h-3 bg-blue-500 rounded-full ml-4"></span>
            <span class="text-sm text-gray-600">2023</span>
          </div>
        </div>
        <div class="h-80">
          <canvas ref="salesTrendChart"></canvas>
        </div>
      </div>

      <!-- Product Performance -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.5s;">
        <h3 class="text-lg font-bold text-gray-900 mb-6">Hiệu suất theo danh mục</h3>
        <div class="h-80">
          <canvas ref="productPerformanceChart"></canvas>
        </div>
      </div>

      <!-- Customer Analytics -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.6s;">
        <h3 class="text-lg font-bold text-gray-900 mb-6">Phân tích khách hàng</h3>
        <div class="h-80">
          <canvas ref="customerChart"></canvas>
        </div>
      </div>

      <!-- Sales by Region -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.7s;">
        <h3 class="text-lg font-bold text-gray-900 mb-6">Doanh thu theo khu vực</h3>
        <div class="h-80">
          <canvas ref="regionChart"></canvas>
        </div>
      </div>
    </div>

    <!-- Top Selling Products Table -->
    <div v-if="!loading" class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.8s;">
      <div class="flex items-center justify-between mb-6">
        <h3 class="text-lg font-bold text-gray-900">Top sản phẩm bán chạy</h3>
        <button class="text-primary-600 hover:text-primary-700 text-sm font-semibold">
          Xem chi tiết →
        </button>
      </div>
      <div class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase">Xếp hạng</th>
              <th class="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase">Sản phẩm</th>
              <th class="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase">Đã bán</th>
              <th class="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase">Doanh thu</th>
              <th class="px-6 py-3 text-left text-xs font-semibold text-gray-600 uppercase">Tăng trưởng</th>
              <th class="px-6 py-3 text-right text-xs font-semibold text-gray-600 uppercase">Biểu đồ</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200">
            <tr v-for="(product, index) in topProducts" :key="product.id" class="hover:bg-gray-50 transition-colors">
              <td class="px-6 py-4">
                <div :class="`w-8 h-8 rounded-full flex items-center justify-center font-bold text-white ${getRankColor(index)}`">
                  {{ index + 1 }}
                </div>
              </td>
              <td class="px-6 py-4">
                <div class="flex items-center space-x-3">
                  <img :src="product.image" :alt="product.name" class="w-10 h-10 object-cover rounded-lg" @error="handleImageError" />
                  <div>
                    <p class="font-semibold text-gray-900">{{ product.name }}</p>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4 font-semibold text-gray-900">
                {{ product.sold }}
              </td>
              <td class="px-6 py-4 font-semibold text-gray-900">
                {{ formatPrice(product.revenue) }}
              </td>
              <td class="px-6 py-4">
                <span class="text-gray-600 bg-gray-100 px-3 py-1 rounded-full text-sm font-semibold">
                  N/A
                </span>
              </td>
              <td class="px-6 py-4">
                <div class="flex justify-end">
                  <div class="w-24 h-12">
                    <canvas :ref="el => { if (el) sparklineRefs[index] = el }"></canvas>
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Revenue Comparison -->
    <div v-if="!loading" class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <div class="lg:col-span-2 bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.9s;">
        <h3 class="text-lg font-bold text-gray-900 mb-6">So sánh doanh thu chi tiết</h3>
        <div class="h-96">
          <canvas ref="revenueComparisonChart"></canvas>
        </div>
      </div>

      <!-- Quick Stats -->
      <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 1s;">
        <h3 class="text-lg font-bold text-gray-900 mb-6">Thống kê nhanh</h3>
        <div class="space-y-4">
          <div v-for="stat in quickStats" :key="stat.label" class="p-4 bg-gray-50 rounded-lg">
            <div class="flex items-center justify-between mb-2">
              <span class="text-sm font-medium text-gray-600">{{ stat.label }}</span>
              <span :class="`text-xs font-semibold px-2 py-1 rounded ${stat.changeClass}`">
                {{ stat.change }}
              </span>
            </div>
            <p class="text-2xl font-bold text-gray-900">{{ stat.value }}</p>
            <div class="mt-2 bg-gray-200 rounded-full h-2">
              <div :class="`h-2 rounded-full ${stat.barColor}`" :style="{ width: `${stat.percentage}%` }"></div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Toast -->
    <Toast :show="toast.show" :message="toast.message" :type="toast.type" @close="toast.show = false" />
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, watch, computed, nextTick } from 'vue'
import { Chart, registerables } from 'chart.js'
import { dashboardApi, handleApiError } from '@/services/api'
import Toast from '@/components/common/Toast.vue'

Chart.register(...registerables)

const salesTrendChart = ref(null)
const productPerformanceChart = ref(null)
const customerChart = ref(null)
const regionChart = ref(null)
const revenueComparisonChart = ref(null)
const sparklineRefs = ref([])
const chartInstances = ref([])

const selectedPeriod = ref('month')
const loading = ref(false)
const reportsData = ref(null)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const periods = [
  { label: '7 ngày', value: 'week' },
  { label: 'Tháng này', value: 'month' },
  { label: 'Quý này', value: 'quarter' },
  { label: 'Năm nay', value: 'year' }
]

const summaryCards = computed(() => {
  if (!reportsData.value?.stats) return []
  
  const stats = reportsData.value.stats
  return [
    {
      title: 'Tổng doanh thu',
      value: formatPrice(stats.totalRevenue),
      change: `${stats.revenueChangePercent >= 0 ? '+' : ''}${stats.revenueChangePercent}%`,
      color: '#f97316',
      bgColor: 'bg-gradient-to-br from-primary-500 to-primary-600',
      badgeClass: stats.revenueChangePercent >= 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800',
      icon: 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z'
    },
    {
      title: 'Đơn hàng',
      value: stats.totalOrders.toLocaleString('vi-VN'),
      change: `${stats.ordersChangePercent >= 0 ? '+' : ''}${stats.ordersChangePercent}%`,
      color: '#3b82f6',
      bgColor: 'bg-gradient-to-br from-blue-500 to-blue-600',
      badgeClass: stats.ordersChangePercent >= 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800',
      icon: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z'
    },
    {
      title: 'Khách hàng mới',
      value: stats.totalCustomers.toLocaleString('vi-VN'),
      change: `${stats.customersChangePercent >= 0 ? '+' : ''}${stats.customersChangePercent}%`,
      color: '#10b981',
      bgColor: 'bg-gradient-to-br from-green-500 to-green-600',
      badgeClass: stats.customersChangePercent >= 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800',
      icon: 'M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z'
    },
    {
      title: 'Tổng sản phẩm',
      value: stats.totalProducts.toLocaleString('vi-VN'),
      change: `${stats.productsChangePercent >= 0 ? '+' : ''}${stats.productsChangePercent}%`,
      color: '#8b5cf6',
      bgColor: 'bg-gradient-to-br from-purple-500 to-purple-600',
      badgeClass: stats.productsChangePercent >= 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800',
      icon: 'M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4'
    }
  ]
})

const topProducts = computed(() => {
  if (!reportsData.value?.topProducts) return []
  return reportsData.value.topProducts.map((p, index) => ({
    ...p,
    growth: 0, // Can be calculated if needed
    image: p.imageUrl || 'https://via.placeholder.com/100'
  }))
})

const quickStats = computed(() => {
  if (!reportsData.value?.stats) return []
  
  const stats = reportsData.value.stats
  const avgOrderValue = stats.totalOrders > 0 ? stats.totalRevenue / stats.totalOrders : 0
  
  return [
    { 
      label: 'Giá trị đơn trung bình', 
      value: formatPrice(avgOrderValue), 
      change: '+0%', 
      changeClass: 'bg-green-100 text-green-800', 
      percentage: Math.min(100, (avgOrderValue / 2000000) * 100), 
      barColor: 'bg-green-500' 
    },
    { 
      label: 'Tỷ lệ chuyển đổi', 
      value: 'N/A', 
      change: 'N/A', 
      changeClass: 'bg-gray-100 text-gray-800', 
      percentage: 0, 
      barColor: 'bg-blue-500' 
    },
    { 
      label: 'Tổng khách hàng', 
      value: stats.totalCustomers.toLocaleString('vi-VN'), 
      change: `${stats.customersChangePercent >= 0 ? '+' : ''}${stats.customersChangePercent}%`, 
      changeClass: stats.customersChangePercent >= 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800', 
      percentage: Math.min(100, (stats.totalCustomers / 10000) * 100), 
      barColor: 'bg-purple-500' 
    },
    { 
      label: 'Tổng sản phẩm', 
      value: stats.totalProducts.toLocaleString('vi-VN'), 
      change: `${stats.productsChangePercent >= 0 ? '+' : ''}${stats.productsChangePercent}%`, 
      changeClass: stats.productsChangePercent >= 0 ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800', 
      percentage: Math.min(100, (stats.totalProducts / 1000) * 100), 
      barColor: 'bg-yellow-500' 
    }
  ]
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
    notation: 'compact'
  }).format(price)
}

function getRankColor(index) {
  const colors = ['bg-yellow-500', 'bg-gray-400', 'bg-orange-600', 'bg-primary-500', 'bg-blue-500']
  return colors[index] || 'bg-gray-500'
}

const handleImageError = (event) => {
  event.target.src = 'https://via.placeholder.com/100'
}

const loadReportsData = async () => {
  try {
    loading.value = true
    reportsData.value = await dashboardApi.getReports(selectedPeriod.value)
    await nextTick()
    updateCharts()
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    loading.value = false
  }
}

const changePeriod = async (period) => {
  selectedPeriod.value = period
  await loadReportsData()
}

const exportReport = async () => {
  try {
    loading.value = true
    const blob = await dashboardApi.exportReports(selectedPeriod.value, 'csv')
    
    // Create download link
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `BaoCao_${selectedPeriod.value}_${new Date().toISOString().slice(0, 10)}.csv`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    
    showToast('Xuất file thành công!', 'success')
  } catch (error) {
    showToast(handleApiError(error), 'error')
  } finally {
    loading.value = false
  }
}

const updateCharts = () => {
  if (!reportsData.value) return
  
  // Destroy existing charts
  chartInstances.value.forEach(chart => {
    if (chart) chart.destroy()
  })
  chartInstances.value = []
  
  // Sales Trend Chart
  if (salesTrendChart.value && reportsData.value.monthlyRevenue) {
    const chart = new Chart(salesTrendChart.value, {
      type: 'line',
      data: {
        labels: reportsData.value.monthlyRevenue.map(m => m.month),
        datasets: [{
          label: 'Doanh thu',
          data: reportsData.value.monthlyRevenue.map(m => m.revenue / 1000000), // Convert to millions
          borderColor: '#f97316',
          backgroundColor: 'rgba(249, 115, 22, 0.1)',
          tension: 0.4,
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } },
        scales: {
          y: {
            beginAtZero: true,
            ticks: { callback: (value) => value + 'M' }
          }
        }
      }
    })
    chartInstances.value.push(chart)
  }

  // Product Performance Chart (Category Revenue as Radar)
  if (productPerformanceChart.value && reportsData.value.categoryRevenue) {
    const chart = new Chart(productPerformanceChart.value, {
      type: 'radar',
      data: {
        labels: reportsData.value.categoryRevenue.slice(0, 5).map(c => c.categoryName),
        datasets: [{
          label: 'Doanh số',
          data: reportsData.value.categoryRevenue.slice(0, 5).map(c => Number(c.percentage)),
          backgroundColor: 'rgba(249, 115, 22, 0.2)',
          borderColor: '#f97316',
          borderWidth: 2
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          r: {
            beginAtZero: true,
            max: 100
          }
        }
      }
    })
    chartInstances.value.push(chart)
  }

  // Category Revenue Chart (Pie)
  if (customerChart.value && reportsData.value.categoryRevenue) {
    const chart = new Chart(customerChart.value, {
      type: 'pie',
      data: {
        labels: reportsData.value.categoryRevenue.slice(0, 5).map(c => c.categoryName),
        datasets: [{
          data: reportsData.value.categoryRevenue.slice(0, 5).map(c => Number(c.revenue)),
          backgroundColor: ['#f97316', '#3b82f6', '#10b981', '#8b5cf6', '#f59e0b'],
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom' }
        }
      }
    })
    chartInstances.value.push(chart)
  }

  // Category Revenue Bar Chart
  if (regionChart.value && reportsData.value.categoryRevenue) {
    const chart = new Chart(regionChart.value, {
      type: 'bar',
      data: {
        labels: reportsData.value.categoryRevenue.slice(0, 5).map(c => c.categoryName),
        datasets: [{
          label: 'Doanh thu (triệu)',
          data: reportsData.value.categoryRevenue.slice(0, 5).map(c => c.revenue / 1000000),
          backgroundColor: '#f97316'
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } }
      }
    })
    chartInstances.value.push(chart)
  }

  // Revenue Comparison Chart
  if (revenueComparisonChart.value && reportsData.value.monthlyRevenue) {
    const chart = new Chart(revenueComparisonChart.value, {
      type: 'bar',
      data: {
        labels: reportsData.value.monthlyRevenue.map(m => m.month),
        datasets: [{
          label: 'Doanh thu',
          data: reportsData.value.monthlyRevenue.map(m => m.revenue / 1000000),
          backgroundColor: '#f97316'
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false
      }
    })
    chartInstances.value.push(chart)
  }

  // Sparklines for top products
  if (topProducts.value.length > 0) {
    topProducts.value.forEach((product, index) => {
      if (sparklineRefs.value[index]) {
        const chart = new Chart(sparklineRefs.value[index], {
          type: 'line',
          data: {
            labels: Array(8).fill(''),
            datasets: [{
              data: Array(8).fill(0).map(() => Math.random() * 100),
              borderColor: '#f97316',
              borderWidth: 2,
              tension: 0.4,
              pointRadius: 0
            }]
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { display: false } },
            scales: {
              x: { display: false },
              y: { display: false }
            }
          }
        })
        chartInstances.value.push(chart)
      }
    })
  }
}

const showToast = (message, type = 'success') => {
  toast.message = message
  toast.type = type
  toast.show = true
  setTimeout(() => {
    toast.show = false
  }, 3000)
}

watch(selectedPeriod, () => {
  loadReportsData()
})

watch(selectedPeriod, () => {
  loadReportsData()
})

onMounted(async () => {
  await loadReportsData()
})
</script>

