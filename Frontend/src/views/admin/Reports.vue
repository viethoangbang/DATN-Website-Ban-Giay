<template>
  <div class="space-y-6 animate-fade-in">
    <!-- Time Period Selector -->
    <div class="bg-white rounded-xl shadow-md p-6">
      <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <h2 class="text-xl font-bold text-gray-900">Báo cáo thống kê</h2>
          <p class="text-gray-600 text-sm">Phân tích chi tiết về doanh thu và sản phẩm</p>
        </div>
        <div class="flex flex-wrap gap-3">
          <button
            v-for="period in periods"
            :key="period.value"
            @click="selectedPeriod = period.value"
            :class="[
              'px-4 py-2 rounded-lg font-medium transition-all duration-300',
              selectedPeriod === period.value
                ? 'bg-primary-500 text-white shadow-lg'
                : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
            ]"
          >
            {{ period.label }}
          </button>
        </div>
      </div>
    </div>

    <!-- Summary Cards -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
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
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
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
    <div class="bg-white rounded-xl shadow-md p-6 animate-fade-in-up" style="animation-delay: 0.8s;">
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
                  <img :src="product.image" :alt="product.name" class="w-10 h-10 object-cover rounded-lg" />
                  <div>
                    <p class="font-semibold text-gray-900">{{ product.name }}</p>
                    <p class="text-sm text-gray-600">{{ product.category }}</p>
                  </div>
                </div>
              </td>
              <td class="px-6 py-4 font-semibold text-gray-900">
                {{ product.sold }} units
              </td>
              <td class="px-6 py-4 font-semibold text-gray-900">
                {{ formatPrice(product.revenue) }}
              </td>
              <td class="px-6 py-4">
                <span :class="product.growth > 0 ? 'text-green-600 bg-green-100' : 'text-red-600 bg-red-100'" class="px-3 py-1 rounded-full text-sm font-semibold">
                  {{ product.growth > 0 ? '+' : '' }}{{ product.growth }}%
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
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
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
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { Chart } from 'chart.js'

const salesTrendChart = ref(null)
const productPerformanceChart = ref(null)
const customerChart = ref(null)
const regionChart = ref(null)
const revenueComparisonChart = ref(null)
const sparklineRefs = ref([])

const selectedPeriod = ref('month')

const periods = [
  { label: '7 ngày', value: 'week' },
  { label: 'Tháng này', value: 'month' },
  { label: 'Quý này', value: 'quarter' },
  { label: 'Năm nay', value: 'year' }
]

const summaryCards = [
  {
    title: 'Tổng doanh thu',
    value: '₫486M',
    change: '+18.2%',
    color: '#f97316',
    bgColor: 'bg-gradient-to-br from-primary-500 to-primary-600',
    badgeClass: 'bg-green-100 text-green-800',
    icon: 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z'
  },
  {
    title: 'Đơn hàng',
    value: '2,543',
    change: '+12.4%',
    color: '#3b82f6',
    bgColor: 'bg-gradient-to-br from-blue-500 to-blue-600',
    badgeClass: 'bg-green-100 text-green-800',
    icon: 'M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z'
  },
  {
    title: 'Khách hàng mới',
    value: '1,245',
    change: '+24.8%',
    color: '#10b981',
    bgColor: 'bg-gradient-to-br from-green-500 to-green-600',
    badgeClass: 'bg-green-100 text-green-800',
    icon: 'M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z'
  },
  {
    title: 'Tỷ lệ hoàn đơn',
    value: '2.4%',
    change: '-1.2%',
    color: '#8b5cf6',
    bgColor: 'bg-gradient-to-br from-purple-500 to-purple-600',
    badgeClass: 'bg-red-100 text-red-800',
    icon: 'M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15'
  }
]

const topProducts = [
  { id: 1, name: 'Nike Air Max 2024', category: 'Running', sold: 456, revenue: 1596000000, growth: 15.3, image: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=100' },
  { id: 2, name: 'Adidas Ultraboost', category: 'Running', sold: 389, revenue: 1633800000, growth: 22.1, image: 'https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=100' },
  { id: 3, name: 'Jordan Retro High', category: 'Basketball', sold: 312, revenue: 1716000000, growth: 8.7, image: 'https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=100' },
  { id: 4, name: 'New Balance 574', category: 'Casual', sold: 287, revenue: 918400000, growth: -3.2, image: 'https://images.unsplash.com/photo-1539185441755-769473a23570?w=100' },
  { id: 5, name: 'Puma RS-X', category: 'Casual', sold: 234, revenue: 655200000, growth: 11.5, image: 'https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=100' }
]

const quickStats = [
  { label: 'Giá trị đơn trung bình', value: '₫1.9M', change: '+5.2%', changeClass: 'bg-green-100 text-green-800', percentage: 75, barColor: 'bg-green-500' },
  { label: 'Tỷ lệ chuyển đổi', value: '3.24%', change: '+0.8%', changeClass: 'bg-green-100 text-green-800', percentage: 60, barColor: 'bg-blue-500' },
  { label: 'Khách hàng quay lại', value: '42%', change: '+3.1%', changeClass: 'bg-green-100 text-green-800', percentage: 42, barColor: 'bg-purple-500' },
  { label: 'Đánh giá trung bình', value: '4.7/5', change: '+0.2', changeClass: 'bg-green-100 text-green-800', percentage: 94, barColor: 'bg-yellow-500' }
]

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

onMounted(() => {
  // Sales Trend Chart
  if (salesTrendChart.value) {
    new Chart(salesTrendChart.value, {
      type: 'line',
      data: {
        labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'T8', 'T9', 'T10', 'T11', 'T12'],
        datasets: [
          {
            label: '2024',
            data: [32, 38, 35, 45, 52, 48, 56, 62, 58, 65, 70, 68],
            borderColor: '#f97316',
            backgroundColor: 'rgba(249, 115, 22, 0.1)',
            tension: 0.4,
            fill: true
          },
          {
            label: '2023',
            data: [28, 32, 30, 38, 42, 40, 45, 48, 46, 52, 55, 53],
            borderColor: '#3b82f6',
            backgroundColor: 'rgba(59, 130, 246, 0.1)',
            tension: 0.4,
            fill: true
          }
        ]
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
  }

  // Product Performance Chart (Radar)
  if (productPerformanceChart.value) {
    new Chart(productPerformanceChart.value, {
      type: 'radar',
      data: {
        labels: ['Running', 'Basketball', 'Casual', 'Sports', 'Training'],
        datasets: [{
          label: 'Doanh số',
          data: [85, 72, 90, 65, 78],
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
  }

  // Customer Chart (Pie)
  if (customerChart.value) {
    new Chart(customerChart.value, {
      type: 'pie',
      data: {
        labels: ['Khách mới', 'Khách cũ', 'Khách VIP'],
        datasets: [{
          data: [45, 35, 20],
          backgroundColor: ['#f97316', '#3b82f6', '#10b981'],
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
  }

  // Region Chart (Bar)
  if (regionChart.value) {
    new Chart(regionChart.value, {
      type: 'bar',
      data: {
        labels: ['Hà Nội', 'TP.HCM', 'Đà Nẵng', 'Hải Phòng', 'Cần Thơ'],
        datasets: [{
          label: 'Doanh thu (triệu)',
          data: [125, 142, 78, 65, 52],
          backgroundColor: '#f97316'
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: { legend: { display: false } }
      }
    })
  }

  // Revenue Comparison Chart
  if (revenueComparisonChart.value) {
    new Chart(revenueComparisonChart.value, {
      type: 'bar',
      data: {
        labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6'],
        datasets: [
          {
            label: 'Doanh thu',
            data: [45, 52, 48, 56, 62, 58],
            backgroundColor: '#f97316'
          },
          {
            label: 'Chi phí',
            data: [28, 32, 30, 35, 38, 36],
            backgroundColor: '#3b82f6'
          },
          {
            label: 'Lợi nhuận',
            data: [17, 20, 18, 21, 24, 22],
            backgroundColor: '#10b981'
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false
      }
    })
  }

  // Sparklines
  sparklineRefs.value.forEach((canvas, index) => {
    if (canvas) {
      const data = [12, 19, 15, 25, 22, 30, 28, 35]
      new Chart(canvas, {
        type: 'line',
        data: {
          labels: Array(8).fill(''),
          datasets: [{
            data: data,
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
    }
  })
})
</script>

