<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <!-- Hero Section -->
    <div class="bg-gradient-to-r from-primary-500 to-orange-600 text-white py-20 animate-fade-in-up">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl md:text-5xl font-bold mb-4">Liên hệ với chúng tôi</h1>
        <p class="text-xl opacity-90">
          Chúng tôi luôn sẵn sàng lắng nghe và hỗ trợ bạn
        </p>
      </div>
    </div>

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-12">
        <!-- Contact Form -->
        <div class="animate-fade-in-up">
          <div class="bg-white rounded-2xl shadow-lg p-8">
            <h2 class="text-2xl font-bold text-gray-900 mb-6">Gửi tin nhắn</h2>
            
            <form @submit.prevent="submitForm" class="space-y-6">
              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">
                  Họ và tên <span class="text-red-500">*</span>
                </label>
                <input
                  v-model="form.name"
                  type="text"
                  required
                  class="input-field"
                  placeholder="Nhập họ và tên của bạn"
                />
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">
                  Email <span class="text-red-500">*</span>
                </label>
                <input
                  v-model="form.email"
                  type="email"
                  required
                  class="input-field"
                  placeholder="example@email.com"
                />
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">
                  Số điện thoại
                </label>
                <input
                  v-model="form.phone"
                  type="tel"
                  class="input-field"
                  placeholder="0912345678"
                />
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">
                  Chủ đề <span class="text-red-500">*</span>
                </label>
                <select v-model="form.subject" required class="input-field">
                  <option value="">Chọn chủ đề</option>
                  <option value="product">Thông tin sản phẩm</option>
                  <option value="order">Đơn hàng</option>
                  <option value="return">Đổi trả hàng</option>
                  <option value="warranty">Bảo hành</option>
                  <option value="feedback">Góp ý</option>
                  <option value="other">Khác</option>
                </select>
              </div>

              <div>
                <label class="block text-sm font-semibold text-gray-700 mb-2">
                  Nội dung <span class="text-red-500">*</span>
                </label>
                <textarea
                  v-model="form.message"
                  required
                  rows="5"
                  class="input-field"
                  placeholder="Nhập nội dung tin nhắn của bạn..."
                ></textarea>
              </div>

              <button type="submit" class="w-full btn-primary">
                <svg class="w-5 h-5 mr-2 inline-block" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                </svg>
                Gửi tin nhắn
              </button>
            </form>
          </div>
        </div>

        <!-- Contact Info -->
        <div class="space-y-8 animate-fade-in-up delay-100">
          <!-- Contact Cards -->
          <div class="grid grid-cols-1 gap-6">
            <div
              v-for="(contact, index) in contacts"
              :key="index"
              class="bg-white rounded-xl shadow-md p-6 hover:shadow-lg transition-shadow"
            >
              <div class="flex items-start space-x-4">
                <div class="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center flex-shrink-0">
                  <component :is="contact.icon" class="w-6 h-6 text-primary-600" />
                </div>
                <div>
                  <h3 class="font-bold text-gray-900 mb-1">{{ contact.title }}</h3>
                  <p class="text-gray-600">{{ contact.value }}</p>
                  <p v-if="contact.subtitle" class="text-gray-500 text-sm mt-1">{{ contact.subtitle }}</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Map -->
          <div class="bg-white rounded-xl shadow-md overflow-hidden">
            <div class="h-64 bg-gray-200 relative">
              <iframe
                src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3723.863981044104!2d105.74459731533157!3d21.038132792832767!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x313455e940879933%3A0xcf10b34e9f1a03df!2zVHLGsOG7nW5nIENhbyDEkeG6s25nIEZQVCBQb2x5dGVjaG5pYw!5e0!3m2!1svi!2s!4v1635000000000!5m2!1svi!2s"
                width="100%"
                height="100%"
                style="border:0;"
                allowfullscreen=""
                loading="lazy"
                class="absolute inset-0"
              ></iframe>
            </div>
            <div class="p-6">
              <h3 class="font-bold text-gray-900 mb-2">Trụ sở chính</h3>
              <p class="text-gray-600">Đường Trịnh Văn Bô, Nam Từ Liêm, Hà Nội</p>
            </div>
          </div>

          <!-- Social Media -->
          <div class="bg-white rounded-xl shadow-md p-6">
            <h3 class="font-bold text-gray-900 mb-4">Theo dõi chúng tôi</h3>
            <div class="flex space-x-4">
              <a
                v-for="social in socials"
                :key="social.name"
                :href="social.url"
                target="_blank"
                class="w-12 h-12 bg-gray-100 rounded-lg flex items-center justify-center hover:bg-primary-500 hover:text-white transition-all duration-300 group"
              >
                <component :is="social.icon" class="w-6 h-6" />
              </a>
            </div>
          </div>

          <!-- Working Hours -->
          <div class="bg-white rounded-xl shadow-md p-6">
            <h3 class="font-bold text-gray-900 mb-4">Giờ làm việc</h3>
            <div class="space-y-2 text-gray-600">
              <div class="flex justify-between">
                <span>Thứ 2 - Thứ 6:</span>
                <span class="font-semibold">8:00 - 21:00</span>
              </div>
              <div class="flex justify-between">
                <span>Thứ 7 - Chủ nhật:</span>
                <span class="font-semibold">9:00 - 20:00</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- FAQ Section -->
    <div class="bg-white py-16">
      <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
        <h2 class="text-3xl font-bold text-gray-900 text-center mb-12 animate-fade-in-up">
          Câu hỏi thường gặp
        </h2>
        <div class="space-y-4">
          <div
            v-for="(faq, index) in faqs"
            :key="index"
            class="border border-gray-200 rounded-lg overflow-hidden animate-fade-in-up"
            :style="{ animationDelay: `${index * 100}ms` }"
          >
            <button
              @click="toggleFaq(index)"
              class="w-full flex items-center justify-between p-6 hover:bg-gray-50 transition-colors text-left"
            >
              <span class="font-semibold text-gray-900">{{ faq.question }}</span>
              <svg
                :class="{ 'rotate-180': openFaqIndex === index }"
                class="w-5 h-5 text-gray-500 transition-transform"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
              </svg>
            </button>
            <Transition
              enter-active-class="transition duration-200 ease-out"
              enter-from-class="opacity-0 -translate-y-2"
              enter-to-class="opacity-100 translate-y-0"
              leave-active-class="transition duration-150 ease-in"
              leave-from-class="opacity-100 translate-y-0"
              leave-to-class="opacity-0 -translate-y-2"
            >
              <div v-if="openFaqIndex === index" class="px-6 pb-6 text-gray-600">
                {{ faq.answer }}
              </div>
            </Transition>
          </div>
        </div>
        <div class="text-center mt-8">
          <router-link to="/faq" class="text-primary-600 hover:text-primary-700 font-semibold">
            Xem thêm câu hỏi →
          </router-link>
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
import { ref, reactive, h } from 'vue'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'

const openFaqIndex = ref(null)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const form = reactive({
  name: '',
  email: '',
  phone: '',
  subject: '',
  message: ''
})

// Icons
const PhoneIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z' })
])

const EmailIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z' })
])

const LocationIcon = () => h('svg', { fill: 'none', viewBox: '0 0 24 24', stroke: 'currentColor' }, [
  h('path', { 'stroke-linecap': 'round', 'stroke-linejoin': 'round', 'stroke-width': '2', d: 'M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z' })
])

const FacebookIcon = () => h('svg', { fill: 'currentColor', viewBox: '0 0 24 24' }, [
  h('path', { d: 'M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z' })
])

const InstagramIcon = () => h('svg', { fill: 'currentColor', viewBox: '0 0 24 24' }, [
  h('path', { d: 'M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069zm0-2.163c-3.259 0-3.667.014-4.947.072-4.358.2-6.78 2.618-6.98 6.98-.059 1.281-.073 1.689-.073 4.948 0 3.259.014 3.668.072 4.948.2 4.358 2.618 6.78 6.98 6.98 1.281.058 1.689.072 4.948.072 3.259 0 3.668-.014 4.948-.072 4.354-.2 6.782-2.618 6.979-6.98.059-1.28.073-1.689.073-4.948 0-3.259-.014-3.667-.072-4.947-.196-4.354-2.617-6.78-6.979-6.98-1.281-.059-1.69-.073-4.949-.073zm0 5.838c-3.403 0-6.162 2.759-6.162 6.162s2.759 6.163 6.162 6.163 6.162-2.759 6.162-6.163c0-3.403-2.759-6.162-6.162-6.162zm0 10.162c-2.209 0-4-1.79-4-4 0-2.209 1.791-4 4-4s4 1.791 4 4c0 2.21-1.791 4-4 4zm6.406-11.845c-.796 0-1.441.645-1.441 1.44s.645 1.44 1.441 1.44c.795 0 1.439-.645 1.439-1.44s-.644-1.44-1.439-1.44z' })
])

const TiktokIcon = () => h('svg', { fill: 'currentColor', viewBox: '0 0 24 24' }, [
  h('path', { d: 'M19.59 6.69a4.83 4.83 0 0 1-3.77-4.25V2h-3.45v13.67a2.89 2.89 0 0 1-5.2 1.74 2.89 2.89 0 0 1 2.31-4.64 2.93 2.93 0 0 1 .88.13V9.4a6.84 6.84 0 0 0-1-.05A6.33 6.33 0 0 0 5 20.1a6.34 6.34 0 0 0 10.86-4.43v-7a8.16 8.16 0 0 0 4.77 1.52v-3.4a4.85 4.85 0 0 1-1-.1z' })
])

const contacts = [
  {
    icon: PhoneIcon,
    title: 'Hotline',
    value: '1900 1234',
    subtitle: '(8:00 - 21:00 hàng ngày)'
  },
  {
    icon: EmailIcon,
    title: 'Email',
    value: 'support@sneakerpoly.com',
    subtitle: 'Phản hồi trong vòng 24h'
  },
  {
    icon: LocationIcon,
    title: 'Địa chỉ',
    value: 'Đường Trịnh Văn Bô, Nam Từ Liêm, Hà Nội'
  }
]

const socials = [
  { name: 'Facebook', icon: FacebookIcon, url: '#' },
  { name: 'Instagram', icon: InstagramIcon, url: '#' },
  { name: 'Tiktok', icon: TiktokIcon, url: '#' }
]

const faqs = [
  {
    question: 'Làm thế nào để kiểm tra tính xác thực của sản phẩm?',
    answer: 'Mọi sản phẩm tại SneakerPoly đều có tem chống giả và giấy bảo hành chính hãng. Bạn có thể kiểm tra mã sản phẩm trên website của hãng.'
  },
  {
    question: 'Chính sách đổi trả như thế nào?',
    answer: 'Chúng tôi chấp nhận đổi trả trong vòng 7 ngày kể từ ngày nhận hàng nếu sản phẩm còn nguyên tem, chưa qua sử dụng và có đầy đủ hóa đơn.'
  },
  {
    question: 'Thời gian giao hàng bao lâu?',
    answer: 'Đơn hàng nội thành Hà Nội thường được giao trong vòng 1-2 ngày. Các tỉnh thành khác từ 3-5 ngày làm việc.'
  }
]

function toggleFaq(index) {
  openFaqIndex.value = openFaqIndex.value === index ? null : index
}

function submitForm() {
  // Submit form logic
  toast.message = 'Đã gửi tin nhắn thành công! Chúng tôi sẽ phản hồi sớm nhất.'
  toast.type = 'success'
  toast.show = true
  
  // Reset form
  form.name = ''
  form.email = ''
  form.phone = ''
  form.subject = ''
  form.message = ''
}
</script>

