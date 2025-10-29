<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <!-- Hero Section -->
    <div class="bg-gradient-to-r from-primary-500 to-orange-600 text-white py-20 animate-fade-in-up">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl md:text-5xl font-bold mb-4">Về Sneaker<span class="text-white">Poly</span></h1>
        <p class="text-xl opacity-90 max-w-3xl mx-auto">
          Chúng tôi mang đến những đôi giày chính hãng, chất lượng với giá tốt nhất
        </p>
      </div>
    </div>

    <!-- Story Section -->
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
        <div class="animate-fade-in-up">
          <h2 class="text-3xl font-bold text-gray-900 mb-6">Câu chuyện của chúng tôi</h2>
          <div class="space-y-4 text-gray-700">
            <p>
              SneakerPoly được thành lập vào năm 2020 với mục tiêu mang đến những đôi giày thể thao 
              chính hãng, chất lượng cao cho khách hàng Việt Nam.
            </p>
            <p>
              Chúng tôi hiểu rằng một đôi giày không chỉ đơn giản là phụ kiện thời trang, mà còn là 
              người bạn đồng hành trong mọi hoạt động hàng ngày của bạn. Vì vậy, chúng tôi luôn cam kết 
              mang đến những sản phẩm tốt nhất từ các thương hiệu hàng đầu thế giới.
            </p>
            <p>
              Với đội ngũ nhân viên giàu kinh nghiệm và đam mê, chúng tôi không ngừng nỗ lực để cải 
              thiện chất lượng dịch vụ và mang lại trải nghiệm mua sắm tuyệt vời nhất cho khách hàng.
            </p>
          </div>
        </div>
        <div class="animate-fade-in-up delay-100">
          <img
            src="https://images.unsplash.com/photo-1556906781-9a412961c28c?w=800"
            alt="About SneakerPoly"
            class="rounded-2xl shadow-xl"
          />
        </div>
      </div>
    </div>

    <!-- Values Section -->
    <div class="bg-white py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <h2 class="text-3xl font-bold text-gray-900 text-center mb-12 animate-fade-in-up">
          Giá trị cốt lõi
        </h2>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <div
            v-for="(value, index) in values"
            :key="index"
            class="text-center p-6 rounded-xl hover:shadow-lg transition-shadow animate-fade-in-up"
            :style="{ animationDelay: `${index * 100}ms` }"
          >
            <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <component :is="value.icon" class="w-8 h-8 text-primary-600" />
            </div>
            <h3 class="text-xl font-bold text-gray-900 mb-2">{{ value.title }}</h3>
            <p class="text-gray-600">{{ value.description }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Stats Section -->
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="grid grid-cols-2 md:grid-cols-4 gap-8">
        <div
          v-for="(stat, index) in stats"
          :key="index"
          class="text-center animate-fade-in-up"
          :style="{ animationDelay: `${index * 100}ms` }"
        >
          <div class="text-4xl font-bold text-primary-600 mb-2">{{ stat.value }}</div>
          <div class="text-gray-600">{{ stat.label }}</div>
        </div>
      </div>
    </div>

    <!-- Team Section -->
    <div class="bg-white py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <h2 class="text-3xl font-bold text-gray-900 text-center mb-12 animate-fade-in-up">
          Đội ngũ của chúng tôi
        </h2>
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
          <div
            v-for="(member, index) in team"
            :key="index"
            class="text-center group animate-fade-in-up"
            :style="{ animationDelay: `${index * 100}ms` }"
          >
            <div class="relative mb-4 overflow-hidden rounded-xl">
              <img
                :src="member.image"
                :alt="member.name"
                class="w-full aspect-square object-cover group-hover:scale-110 transition-transform duration-500"
              />
              <div class="absolute inset-0 bg-gradient-to-t from-black/50 to-transparent opacity-0 group-hover:opacity-100 transition-opacity" />
            </div>
            <h3 class="text-lg font-bold text-gray-900">{{ member.name }}</h3>
            <p class="text-primary-600 font-medium">{{ member.position }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- CTA Section -->
    <div class="bg-gradient-to-r from-primary-500 to-orange-600 text-white py-16">
      <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center animate-fade-in-up">
        <h2 class="text-3xl font-bold mb-4">Sẵn sàng bắt đầu?</h2>
        <p class="text-xl opacity-90 mb-8">
          Khám phá bộ sưu tập giày thể thao đa dạng của chúng tôi ngay hôm nay
        </p>
        <router-link to="/products" class="btn-primary bg-white text-primary-600 hover:bg-gray-100">
          Xem sản phẩm
        </router-link>
      </div>
    </div>

    <Footer />
  </div>
</template>

<script setup>
import { h } from 'vue'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'

// Icons
const QualityIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z' })
])

const CustomerIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z' })
])

const InnovationIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M13 10V3L4 14h7v7l9-11h-7z' })
])

const values = [
  {
    icon: QualityIcon,
    title: 'Chất lượng',
    description: '100% sản phẩm chính hãng, được kiểm tra kỹ lưỡng trước khi giao hàng'
  },
  {
    icon: CustomerIcon,
    title: 'Khách hàng là trung tâm',
    description: 'Luôn lắng nghe và đáp ứng nhu cầu của khách hàng một cách tốt nhất'
  },
  {
    icon: InnovationIcon,
    title: 'Đổi mới',
    description: 'Không ngừng cải tiến dịch vụ và trải nghiệm mua sắm'
  }
]

const stats = [
  { value: '50K+', label: 'Khách hàng' },
  { value: '100K+', label: 'Đơn hàng' },
  { value: '500+', label: 'Sản phẩm' },
  { value: '99%', label: 'Hài lòng' }
]

const team = [
  {
    name: 'Nguyễn Văn A',
    position: 'CEO & Founder',
    image: 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400'
  },
  {
    name: 'Trần Thị B',
    position: 'Marketing Director',
    image: 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400'
  },
  {
    name: 'Lê Văn C',
    position: 'Operations Manager',
    image: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400'
  },
  {
    name: 'Phạm Thị D',
    position: 'Customer Success',
    image: 'https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400'
  }
]
</script>

