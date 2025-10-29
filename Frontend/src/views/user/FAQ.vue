<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <!-- Hero Section -->
    <div class="bg-gradient-to-r from-primary-500 to-orange-600 text-white py-20 animate-fade-in-up">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <h1 class="text-4xl md:text-5xl font-bold mb-4">Câu hỏi thường gặp</h1>
        <p class="text-xl opacity-90">
          Tìm câu trả lời cho những thắc mắc của bạn
        </p>
      </div>
    </div>

    <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <!-- Search Box -->
      <div class="mb-12 animate-fade-in-up">
        <div class="relative">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Tìm kiếm câu hỏi..."
            class="w-full input-field pl-12"
          />
          <svg class="w-6 h-6 text-gray-400 absolute left-4 top-1/2 -translate-y-1/2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
        </div>
      </div>

      <!-- Categories -->
      <div class="mb-8 animate-fade-in-up delay-100">
        <div class="flex flex-wrap gap-3">
          <button
            v-for="cat in categories"
            :key="cat"
            @click="selectedCategory = cat"
            :class="[
              'px-6 py-2 rounded-full font-semibold transition-all',
              selectedCategory === cat
                ? 'bg-primary-500 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-100'
            ]"
          >
            {{ cat }}
          </button>
        </div>
      </div>

      <!-- FAQ List -->
      <div class="space-y-4">
        <div
          v-for="(faq, index) in filteredFaqs"
          :key="index"
          class="bg-white border border-gray-200 rounded-lg overflow-hidden animate-fade-in-up"
          :style="{ animationDelay: `${index * 50}ms` }"
        >
          <button
            @click="toggleFaq(index)"
            class="w-full flex items-start justify-between p-6 hover:bg-gray-50 transition-colors text-left"
          >
            <div class="flex-1 pr-4">
              <span class="inline-block px-3 py-1 bg-primary-100 text-primary-700 text-xs font-semibold rounded-full mb-2">
                {{ faq.category }}
              </span>
              <h3 class="font-semibold text-gray-900">{{ faq.question }}</h3>
            </div>
            <svg
              :class="{ 'rotate-180': openFaqIndex === index }"
              class="w-5 h-5 text-gray-500 transition-transform flex-shrink-0 mt-1"
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
            <div v-if="openFaqIndex === index" class="px-6 pb-6 text-gray-600 border-t">
              <div class="pt-4" v-html="faq.answer"></div>
            </div>
          </Transition>
        </div>
      </div>

      <!-- No Results -->
      <EmptyState
        v-if="filteredFaqs.length === 0"
        title="Không tìm thấy kết quả"
        description="Thử tìm kiếm với từ khóa khác hoặc liên hệ với chúng tôi"
        action-text="Liên hệ hỗ trợ"
        @action="$router.push('/contact')"
      />

      <!-- Contact CTA -->
      <div class="mt-12 bg-white rounded-xl shadow-md p-8 text-center animate-fade-in-up">
        <h2 class="text-2xl font-bold text-gray-900 mb-2">Không tìm thấy câu trả lời?</h2>
        <p class="text-gray-600 mb-6">Đội ngũ hỗ trợ của chúng tôi luôn sẵn sàng giúp đỡ bạn</p>
        <div class="flex flex-col sm:flex-row gap-4 justify-center">
          <router-link to="/contact" class="btn-primary">
            Liên hệ với chúng tôi
          </router-link>
          <a href="tel:19001234" class="btn-secondary">
            <svg class="w-5 h-5 mr-2 inline-block" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
            </svg>
            Gọi 1900 1234
          </a>
        </div>
      </div>
    </div>

    <Footer />
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import EmptyState from '../../components/common/EmptyState.vue'

const searchQuery = ref('')
const selectedCategory = ref('Tất cả')
const openFaqIndex = ref(null)

const categories = ['Tất cả', 'Sản phẩm', 'Đặt hàng', 'Thanh toán', 'Giao hàng', 'Đổi trả', 'Bảo hành']

const faqs = [
  {
    category: 'Sản phẩm',
    question: 'Làm thế nào để kiểm tra tính xác thực của sản phẩm?',
    answer: `<p class="mb-3">Mọi sản phẩm tại SneakerPoly đều được kiểm tra kỹ lưỡng trước khi giao hàng. Để kiểm tra tính xác thực:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Kiểm tra tem chống giả trên hộp và sản phẩm</li>
        <li>Xác minh mã sản phẩm trên website chính hãng của hãng</li>
        <li>Kiểm tra giấy bảo hành kèm theo</li>
        <li>So sánh với hình ảnh sản phẩm chính hãng</li>
      </ul>`
  },
  {
    category: 'Sản phẩm',
    question: 'Tôi có thể tìm size giày phù hợp như thế nào?',
    answer: `<p class="mb-3">Chúng tôi cung cấp bảng size chi tiết cho từng sản phẩm:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Xem bảng size trên trang chi tiết sản phẩm</li>
        <li>Đo chiều dài bàn chân từ gót đến ngón dài nhất</li>
        <li>Tham khảo đánh giá về size từ người mua trước</li>
        <li>Liên hệ hotline 1900 1234 để được tư vấn</li>
      </ul>`
  },
  {
    category: 'Sản phẩm',
    question: 'Sản phẩm có bảo hành không?',
    answer: `<p class="mb-3">Tất cả sản phẩm đều được bảo hành chính hãng:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Bảo hành từ 6-12 tháng tùy theo từng hãng</li>
        <li>Bảo hành các lỗi do nhà sản xuất</li>
        <li>Miễn phí sửa chữa trong thời gian bảo hành</li>
        <li>Đổi mới nếu lỗi không thể khắc phục</li>
      </ul>`
  },
  {
    category: 'Đặt hàng',
    question: 'Làm thế nào để đặt hàng?',
    answer: `<p class="mb-3">Đặt hàng rất đơn giản:</p>
      <ol class="list-decimal list-inside space-y-2 ml-4">
        <li>Chọn sản phẩm và thêm vào giỏ hàng</li>
        <li>Nhấn "Thanh toán" ở giỏ hàng</li>
        <li>Điền thông tin giao hàng</li>
        <li>Chọn phương thức thanh toán</li>
        <li>Xác nhận đơn hàng</li>
      </ol>
      <p class="mt-3">Bạn sẽ nhận được email xác nhận ngay sau khi đặt hàng thành công.</p>`
  },
  {
    category: 'Đặt hàng',
    question: 'Tôi có thể hủy hoặc thay đổi đơn hàng không?',
    answer: `<p class="mb-3">Bạn có thể hủy/thay đổi đơn hàng trước khi đơn hàng được xác nhận:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Liên hệ hotline 1900 1234 ngay sau khi đặt</li>
        <li>Gửi email đến support@sneakerpoly.com</li>
        <li>Thời gian xử lý: 1-2 giờ làm việc</li>
      </ul>
      <p class="mt-3">Sau khi đơn hàng đã được xác nhận và đóng gói, bạn không thể hủy nhưng có thể từ chối nhận hàng.</p>`
  },
  {
    category: 'Thanh toán',
    question: 'Có những phương thức thanh toán nào?',
    answer: `<p class="mb-3">Chúng tôi chấp nhận nhiều phương thức thanh toán:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Thanh toán khi nhận hàng (COD)</li>
        <li>Chuyển khoản ngân hàng</li>
        <li>Ví điện tử (MoMo, ZaloPay, VNPay)</li>
        <li>Thẻ tín dụng/ghi nợ quốc tế</li>
      </ul>`
  },
  {
    category: 'Thanh toán',
    question: 'Thanh toán có an toàn không?',
    answer: `<p class="mb-3">Hệ thống thanh toán của chúng tôi hoàn toàn an toàn:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Mã hóa SSL 256-bit</li>
        <li>Không lưu trữ thông tin thẻ</li>
        <li>Đối tác thanh toán uy tín</li>
        <li>Xác thực 2 lớp</li>
      </ul>`
  },
  {
    category: 'Giao hàng',
    question: 'Thời gian giao hàng là bao lâu?',
    answer: `<p class="mb-3">Thời gian giao hàng phụ thuộc vào địa chỉ:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Nội thành Hà Nội: 1-2 ngày</li>
        <li>Nội thành TP.HCM: 2-3 ngày</li>
        <li>Tỉnh thành khác: 3-5 ngày</li>
        <li>Vùng xa: 5-7 ngày</li>
      </ul>
      <p class="mt-3">Bạn có thể theo dõi đơn hàng real-time trong mục "Đơn hàng của tôi".</p>`
  },
  {
    category: 'Giao hàng',
    question: 'Phí vận chuyển là bao nhiêu?',
    answer: `<p class="mb-3">Chính sách phí vận chuyển:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Miễn phí cho đơn hàng trên 500,000đ</li>
        <li>30,000đ cho đơn hàng nội thành</li>
        <li>40,000đ - 60,000đ cho các tỉnh</li>
      </ul>`
  },
  {
    category: 'Giao hàng',
    question: 'Tôi có thể chỉ định thời gian giao hàng không?',
    answer: `<p>Hiện tại chúng tôi chưa hỗ trợ chỉ định giờ giao cụ thể. Tuy nhiên, bạn có thể:</p>
      <ul class="list-disc list-inside space-y-2 ml-4 mt-3">
        <li>Chọn buổi giao hàng (sáng/chiều)</li>
        <li>Ghi chú yêu cầu đặc biệt khi đặt hàng</li>
        <li>Liên hệ shipper khi họ gọi để hẹn lại</li>
      </ul>`
  },
  {
    category: 'Đổi trả',
    question: 'Chính sách đổi trả như thế nào?',
    answer: `<p class="mb-3">Chúng tôi chấp nhận đổi trả trong vòng 7 ngày kể từ ngày nhận hàng:</p>
      <p class="font-semibold mb-2">Điều kiện đổi trả:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Sản phẩm còn nguyên tem, nhãn mác</li>
        <li>Chưa qua sử dụng, không có dấu hiệu đã đi</li>
        <li>Còn đầy đủ hộp, phụ kiện đi kèm</li>
        <li>Có hóa đơn mua hàng</li>
      </ul>
      <p class="font-semibold mt-4 mb-2">Trường hợp được đổi trả:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Sản phẩm bị lỗi từ nhà sản xuất</li>
        <li>Giao sai size, sai mẫu</li>
        <li>Sản phẩm bị hư hỏng trong quá trình vận chuyển</li>
        <li>Không vừa size (đổi size khác nếu còn hàng)</li>
      </ul>`
  },
  {
    category: 'Đổi trả',
    question: 'Quy trình đổi trả như thế nào?',
    answer: `<ol class="list-decimal list-inside space-y-2 ml-4">
        <li>Liên hệ hotline 1900 1234 hoặc email</li>
        <li>Cung cấp thông tin đơn hàng và lý do đổi trả</li>
        <li>Chúng tôi sẽ xác nhận và hướng dẫn gửi hàng</li>
        <li>Đóng gói sản phẩm cẩn thận</li>
        <li>Shipper đến lấy hàng (miễn phí)</li>
        <li>Kiểm tra và xử lý đổi/trả trong 3-5 ngày</li>
      </ol>`
  },
  {
    category: 'Bảo hành',
    question: 'Bảo hành bao gồm những gì?',
    answer: `<p class="mb-3">Bảo hành chính hãng bao gồm:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Đế giày bị bong tróc, bung keo</li>
        <li>Đường chỉ bị tuột, rách</li>
        <li>Phụ kiện bị hỏng (khóa, móc, dây)</li>
        <li>Các lỗi về vật liệu, kỹ thuật</li>
      </ul>
      <p class="font-semibold mt-4 mb-2">Không bảo hành:</p>
      <ul class="list-disc list-inside space-y-2 ml-4">
        <li>Hao mòn tự nhiên do sử dụng</li>
        <li>Bị rách, thủng do va đập</li>
        <li>Bẩn, phai màu do sử dụng</li>
        <li>Tự ý sửa chữa</li>
      </ul>`
  },
  {
    category: 'Bảo hành',
    question: 'Làm thế nào để yêu cầu bảo hành?',
    answer: `<ol class="list-decimal list-inside space-y-2 ml-4">
        <li>Mang sản phẩm và phiếu bảo hành đến cửa hàng</li>
        <li>Hoặc liên hệ hotline để được hướng dẫn gửi hàng</li>
        <li>Nhân viên kiểm tra và xác định lỗi</li>
        <li>Xử lý bảo hành trong 7-14 ngày</li>
        <li>Thông báo khi hoàn thành</li>
      </ol>
      <p class="mt-3 text-primary-600 font-semibold">Miễn phí vận chuyển 2 chiều cho bảo hành!</p>`
  }
]

const filteredFaqs = computed(() => {
  let result = faqs

  // Filter by category
  if (selectedCategory.value !== 'Tất cả') {
    result = result.filter(faq => faq.category === selectedCategory.value)
  }

  // Filter by search query
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter(faq =>
      faq.question.toLowerCase().includes(query) ||
      faq.answer.toLowerCase().includes(query)
    )
  }

  return result
})

function toggleFaq(index) {
  openFaqIndex.value = openFaqIndex.value === index ? null : index
}
</script>

