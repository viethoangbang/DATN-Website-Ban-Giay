<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <!-- Hero Banner -->
    <section class="relative bg-white overflow-hidden">
      <!-- Decorative Background -->
      <div class="absolute inset-0">
        <!-- Orange Gradient Circle -->
        <div class="absolute top-0 right-0 w-[600px] h-[600px] bg-gradient-to-br from-primary-400 to-orange-600 rounded-full blur-3xl opacity-20 -translate-y-1/3 translate-x-1/3"></div>
        <!-- Yellow Accent -->
        <div class="absolute bottom-0 left-0 w-[400px] h-[400px] bg-gradient-to-tr from-yellow-400 to-orange-400 rounded-full blur-3xl opacity-10 translate-y-1/3 -translate-x-1/3"></div>
      </div>

      <div class="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16 lg:py-24">
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
          <!-- Left Content -->
          <div class="space-y-8 animate-fade-in-up">
            <!-- Small Badge -->
            <div class="inline-flex items-center space-x-2 bg-primary-50 text-primary-600 px-4 py-2 rounded-full">
              <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
              </svg>
              <span class="text-sm font-semibold">Thương hiệu #1 về giày thể thao</span>
            </div>

            <h1 class="text-5xl md:text-6xl lg:text-7xl font-black leading-tight text-gray-900">
              Bước Đi
              <span class="block bg-gradient-to-r from-primary-600 to-orange-600 bg-clip-text text-transparent">
                Phong Cách
              </span>
            </h1>
            
            <p class="text-xl text-gray-600 max-w-xl leading-relaxed">
              Khám phá bộ sưu tập giày thể thao chính hãng với đa dạng kiểu dáng từ các thương hiệu hàng đầu thế giới
            </p>

            <div class="flex flex-col sm:flex-row gap-4">
              <router-link
                to="/products"
                class="group bg-gradient-to-r from-primary-600 to-orange-600 text-white px-8 py-4 rounded-xl font-bold text-lg hover:shadow-xl transition-all duration-300 transform hover:scale-105 text-center flex items-center justify-center space-x-2"
              >
                <span>Mua sắm ngay</span>
                <svg class="w-5 h-5 group-hover:translate-x-1 transition-transform" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 7l5 5m0 0l-5 5m5-5H6" />
                </svg>
              </router-link>
              <router-link
                to="/products"
                class="border-2 border-gray-900 text-gray-900 px-8 py-4 rounded-xl font-bold text-lg hover:bg-gray-900 hover:text-white transition-all duration-300 text-center"
              >
                Xem bộ sưu tập
              </router-link>
            </div>

            <!-- Stats -->
            <div class="grid grid-cols-3 gap-6 pt-4">
              <div class="animate-fade-in-up" style="animation-delay: 0.2s;">
                <div class="text-4xl font-black text-gray-900">{{ transformedProducts.length }}+</div>
                <div class="text-gray-600 text-sm font-medium">Sản phẩm</div>
              </div>
              <div class="animate-fade-in-up" style="animation-delay: 0.3s;">
                <div class="text-4xl font-black text-gray-900">10k+</div>
                <div class="text-gray-600 text-sm font-medium">Khách hàng</div>
              </div>
              <div class="animate-fade-in-up" style="animation-delay: 0.4s;">
                <div class="text-4xl font-black bg-gradient-to-r from-primary-600 to-orange-600 bg-clip-text text-transparent">4.9★</div>
                <div class="text-gray-600 text-sm font-medium">Đánh giá</div>
              </div>
            </div>
          </div>

          <!-- Right - Shoe Image -->
          <div class="relative animate-fade-in-up hidden lg:flex items-center justify-center" style="animation-delay: 0.2s;">
            <div class="relative w-full max-w-[500px] aspect-square">
              <!-- Background Circle -->
              <div class="absolute inset-0 flex items-center justify-center">
                <div class="w-full h-full bg-gradient-to-br from-orange-100 to-primary-100 rounded-full opacity-60"></div>
              </div>
              
              <!-- Hero Image Carousel -->
              <div class="absolute inset-0 flex items-center justify-center p-1 overflow-hidden">
                <!-- Fallback image if no hero images -->
                <img
                  v-if="heroImages.length === 0"
                  src="../../assets/giaybanner.png"
                  alt="Featured Shoe"
                  class="w-full h-full object-contain transform hover:scale-110 transition-transform duration-700 drop-shadow-2xl animate-float"
                  @error="handleHeroImageError"
                />
                <!-- Carousel with multiple images -->
                <TransitionGroup v-else name="hero-slide" tag="div" class="relative w-full h-full">
                  <img
                    v-for="(heroImage, index) in heroImages"
                    :key="heroImage.id"
                    v-show="currentHeroIndex === index"
                    :src="heroImage.url"
                    :alt="`Hero Image ${index + 1}`"
                    class="absolute inset-0 w-full h-full object-contain transform hover:scale-110 transition-transform duration-700 drop-shadow-2xl animate-float"
                    @error="handleHeroImageError"
                  />
                </TransitionGroup>
                <!-- Navigation Dots -->
                <div v-if="heroImages.length > 1" class="absolute bottom-4 left-1/2 transform -translate-x-1/2 flex space-x-2 z-10">
                  <button
                    v-for="(img, index) in heroImages"
                    :key="img.id"
                    @click="currentHeroIndex = index"
                    :class="[
                      'w-2 h-2 rounded-full transition-all duration-300',
                      currentHeroIndex === index ? 'bg-white w-6' : 'bg-white/50 hover:bg-white/75'
                    ]"
                    :title="`Slide ${index + 1}`"
                  ></button>
                </div>
              </div>
              
              <!-- Floating Elements -->
              <div class="absolute top-8 right-8 bg-white rounded-2xl shadow-xl p-3 animate-float" style="animation-delay: 0.5s;">
                <div class="flex items-center space-x-2">
                  <div class="w-10 h-10 bg-gradient-to-br from-primary-500 to-orange-500 rounded-full flex items-center justify-center flex-shrink-0">
                    <svg class="w-5 h-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
                    </svg>
                  </div>
                  <div>
                    <div class="text-xs text-gray-600 whitespace-nowrap">Chính hãng</div>
                    <div class="text-sm font-bold text-gray-900">100%</div>
                  </div>
                </div>
              </div>
              
              <div class="absolute bottom-8 left-8 bg-white rounded-2xl shadow-xl p-3 animate-float" style="animation-delay: 1s;">
                <div class="flex items-center space-x-2">
                  <div class="text-2xl">🔥</div>
                  <div>
                    <div class="text-xs text-gray-600 whitespace-nowrap">Hot Sale</div>
                    <!-- <div class="text-lg font-black bg-gradient-to-r from-red-600 to-orange-600 bg-clip-text text-transparent">-30%</div> -->
                  </div>
                </div>
              </div>
              
              <!-- Decorative Elements -->
              <div class="absolute top-16 left-8 w-3 h-3 bg-primary-500 rounded-full animate-ping"></div>
              <div class="absolute bottom-24 right-16 w-2 h-2 bg-orange-500 rounded-full animate-ping" style="animation-delay: 1s;"></div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Loading Spinner -->
    <LoadingSpinner :show="loading" message="Đang tải sản phẩm..." />

    
    

    <!-- Trending Products -->
    <section class="bg-gradient-to-b from-gray-50 to-white py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="text-center mb-12 animate-fade-in-up">
          <div class="inline-flex items-center space-x-2 mb-4">
            <svg class="w-8 h-8 text-primary-500 animate-bounce" fill="currentColor" viewBox="0 0 20 20">
              <path d="M2 11a1 1 0 011-1h2a1 1 0 011 1v5a1 1 0 01-1 1H3a1 1 0 01-1-1v-5zM8 7a1 1 0 011-1h2a1 1 0 011 1v9a1 1 0 01-1 1H9a1 1 0 01-1-1V7zM14 4a1 1 0 011-1h2a1 1 0 011 1v12a1 1 0 01-1 1h-2a1 1 0 01-1-1V4z" />
            </svg>
            <h2 class="text-3xl md:text-4xl font-bold text-gray-900">Xu Hướng Hiện Nay</h2>
          </div>
          <p class="text-gray-600 text-lg">Những mẫu giày được săn đón nhiều nhất</p>
        </div>

        <div v-if="loading" class="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-6">
          <div v-for="i in 8" :key="i" class="bg-white rounded-xl shadow-md overflow-hidden animate-pulse">
            <div class="aspect-square bg-gray-200"></div>
            <div class="p-4 space-y-2">
              <div class="h-4 bg-gray-200 rounded w-3/4"></div>
              <div class="h-6 bg-gray-200 rounded w-1/2"></div>
            </div>
          </div>
        </div>
        <div v-else-if="trendingProducts.length === 0" class="text-center py-12 text-gray-500">
          <p>Chưa có sản phẩm xu hướng</p>
        </div>
        <div v-else class="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-6">
          <div
            v-for="(product, index) in trendingProducts"
            :key="product.id"
            class="group animate-fade-in-up"
            :style="{ animationDelay: `${index * 0.08}s` }"
          >
            <div class="bg-white rounded-xl shadow-md overflow-hidden transition-all duration-300 hover:shadow-2xl hover:-translate-y-2">
              <div class="relative overflow-hidden aspect-square bg-gray-100">
                <img
                  :src="product.image"
                  :alt="product.name"
                  @error="handleImageError"
                  class="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-500"
                  loading="lazy"
                />
                <div class="absolute top-2 left-2 z-10 flex flex-col space-y-1">
                  <span v-if="product.isNew" class="bg-primary-500 text-white px-2 py-0.5 rounded-full text-xs font-bold shadow-lg">
                    MỚI
                  </span>
                  <span v-if="product.discountInfo" class="bg-red-500 text-white px-2 py-0.5 rounded-full text-xs font-bold shadow-lg">
                    {{ product.discountInfo.discountType === 'Percentage' 
                      ? `-${product.discountInfo.discountValue}%` 
                      : `-${formatCurrency(product.discountInfo.discountValue)}` }}
                  </span>
                </div>
              </div>
              <div class="p-4">
                <h3 class="font-bold text-gray-900 text-sm mb-1 group-hover:text-primary-600 transition-colors line-clamp-1">
                  {{ product.name }}
                </h3>
                <div class="space-y-1">
                  <div v-if="product.discountInfo && product.discountedPrice < product.price" class="flex items-center space-x-2">
                    <span class="text-sm text-gray-400 line-through">
                      {{ formatPrice(product.price) }}
                    </span>
                  </div>
                  <div class="flex items-baseline space-x-2">
                    <span class="text-lg font-bold text-primary-600">
                      {{ formatPrice(product.discountedPrice || product.price) }}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="text-center mt-12">
          <router-link to="/products" class="btn-primary inline-flex items-center space-x-2">
            <span>Khám phá thêm</span>
            <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 7l5 5m0 0l-5 5m5-5H6" />
            </svg>
          </router-link>
        </div>
      </div>
    </section>

    <!-- New Products Section -->
    <section v-if="transformedNewProducts.length > 0" class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="flex justify-between items-center mb-12">
        <div class="animate-fade-in-up">
          <div class="flex items-center space-x-2 mb-2">
            <span class="px-3 py-1 bg-primary-500 text-white rounded-full text-sm font-bold">MỚI</span>
            <h2 class="text-3xl md:text-4xl font-bold text-gray-900">
              Sản Phẩm Mới
            </h2>
          </div>
          <p class="text-gray-600">Bộ sưu tập mới nhất được quản lý bởi admin</p>
        </div>
        <router-link
          to="/products"
          class="hidden md:flex items-center space-x-2 text-primary-600 hover:text-primary-700 font-semibold group"
        >
          <span>Xem tất cả</span>
          <svg class="w-5 h-5 transform group-hover:translate-x-2 transition-transform" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
          </svg>
        </router-link>
      </div>

      <div v-if="loading" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <div v-for="i in 4" :key="i" class="bg-white rounded-xl shadow-md overflow-hidden animate-pulse">
          <div class="aspect-square bg-gray-200"></div>
          <div class="p-6 space-y-3">
            <div class="h-4 bg-gray-200 rounded w-1/2"></div>
            <div class="h-6 bg-gray-200 rounded w-3/4"></div>
            <div class="h-8 bg-gray-200 rounded w-1/3"></div>
          </div>
        </div>
      </div>
      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <ProductCard
          v-for="(product, index) in transformedNewProducts"
          :key="product.id"
          :product="product"
          @quick-view="addToCart"
          class="animate-fade-in-up"
          :style="{ animationDelay: `${index * 0.1}s` }"
        />
      </div>
    </section>

    <!-- Brand Sections -->
    <section v-if="brandSections && brandSections.length > 0" class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16 space-y-16">
      <div
        v-for="(section, sectionIndex) in brandSections"
        :key="section.brand"
        class="animate-fade-in-up"
        :style="{ animationDelay: `${sectionIndex * 0.1}s` }"
      >
        <!-- Brand Header -->
        <div class="text-center mb-8">
          <h2 class="text-3xl md:text-4xl font-bold text-gray-900 mb-2">{{ section.brand }}</h2>
          <p class="text-gray-600">Khám phá bộ sưu tập {{ section.brand }}</p>
        </div>

        <!-- Banner Carousel -->
        <div v-if="section.banners && section.banners.length > 0" class="mb-12">
          <BrandCarousel :banners="section.banners" :brand="section.brand" />
        </div>

        <!-- Products Grid -->
        <div v-if="section.products && section.products.length > 0" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <ProductCard
            v-for="(product, index) in section.products"
            :key="product.id"
            :product="transformBrandProduct(product)"
            @quick-view="addToCart"
            class="animate-fade-in-up"
            :style="{ animationDelay: `${index * 0.05}s` }"
          />
        </div>
        <div v-else class="text-center py-12 text-gray-500">
          <p>Chưa có sản phẩm cho thương hiệu {{ section.brand }}</p>
        </div>
      </div>
    </section>

    <!-- Top Brands - Infinite Scroll -->
    <section class="bg-gray-100 py-16 overflow-hidden">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="text-center mb-12 animate-fade-in-up">
          <h2 class="text-3xl md:text-4xl font-bold text-gray-900 mb-2">Thương Hiệu Nổi Tiếng</h2>
          <p class="text-gray-600">Đối tác chính thức của các thương hiệu hàng đầu</p>
        </div>

        <!-- Infinite Scroll Container -->
        <div class="relative">
          <div class="flex overflow-hidden">
            <!-- First Set -->
            <div class="flex animate-scroll">
              <div
                v-for="brand in brandLogos"
                :key="brand.name"
                class="flex-shrink-0 mx-8 bg-white rounded-xl p-8 shadow-md hover:shadow-xl transition-all duration-300 group"
              >
                <div class="flex items-center justify-center w-32 h-20">
                  <div :class="brand.class" class="transition-colors">
                    {{ brand.name }}
                  </div>
                </div>
              </div>
            </div>
            <!-- Duplicate Set for Seamless Loop -->
            <div class="flex animate-scroll" aria-hidden="true">
              <div
                v-for="brand in brandLogos"
                :key="`duplicate-${brand.name}`"
                class="flex-shrink-0 mx-8 bg-white rounded-xl p-8 shadow-md hover:shadow-xl transition-all duration-300 group"
              >
                <div class="flex items-center justify-center w-32 h-20">
                  <div :class="brand.class" class="transition-colors">
                    {{ brand.name }}
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Gradient Overlays -->
          <div class="absolute left-0 top-0 bottom-0 w-32 bg-gradient-to-r from-gray-100 to-transparent pointer-events-none"></div>
          <div class="absolute right-0 top-0 bottom-0 w-32 bg-gradient-to-l from-gray-100 to-transparent pointer-events-none"></div>
        </div>
      </div>
    </section>

    


    <!-- Features Section -->
    <section class="bg-white py-16">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-8">
          <div class="text-center p-6 animate-fade-in-up">
            <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg class="w-8 h-8 text-primary-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 8h14M5 8a2 2 0 110-4h14a2 2 0 110 4M5 8v10a2 2 0 002 2h10a2 2 0 002-2V8m-9 4h4" />
              </svg>
            </div>
            <h3 class="text-xl font-bold text-gray-900 mb-2">Miễn phí vận chuyển</h3>
            <p class="text-gray-600">Cho đơn hàng trên 500.000đ</p>
          </div>

          <div class="text-center p-6 animate-fade-in-up" style="animation-delay: 0.1s;">
            <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg class="w-8 h-8 text-primary-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <h3 class="text-xl font-bold text-gray-900 mb-2">Hàng chính hãng 100%</h3>
            <p class="text-gray-600">Cam kết chất lượng</p>
          </div>

          <div class="text-center p-6 animate-fade-in-up" style="animation-delay: 0.2s;">
            <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg class="w-8 h-8 text-primary-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
            </div>
            <h3 class="text-xl font-bold text-gray-900 mb-2">Đổi trả trong 7 ngày</h3>
            <p class="text-gray-600">Nếu có lỗi từ nhà sản xuất</p>
          </div>

          <div class="text-center p-6 animate-fade-in-up" style="animation-delay: 0.3s;">
            <div class="w-16 h-16 bg-primary-100 rounded-full flex items-center justify-center mx-auto mb-4">
              <svg class="w-8 h-8 text-primary-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.364 5.636l-3.536 3.536m0 5.656l3.536 3.536M9.172 9.172L5.636 5.636m3.536 9.192l-3.536 3.536M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-5 0a4 4 0 11-8 0 4 4 0 018 0z" />
              </svg>
            </div>
            <h3 class="text-xl font-bold text-gray-900 mb-2">Hỗ trợ 24/7</h3>
            <p class="text-gray-600">Hotline: 1900 xxxx</p>
          </div>
        </div>
      </div>
    </section>

    <Footer />

    <!-- Quick View Modal -->
    <QuickViewModal
      :show="showQuickView"
      :product="selectedProduct"
      @close="showQuickView = false"
      @add-to-cart="handleAddToCart"
      @error="(msg) => { toast.message = msg; toast.type = 'error'; toast.show = true; }"
    />

    <!-- Toast Notification -->
    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted, reactive, TransitionGroup } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '../../stores/cart'
import { useHomePage } from '../../composables/useHomePage'
import { useProductDiscount } from '../../composables/useProductDiscount'
import { imageApi } from '../../services/api'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import LoadingSpinner from '../../components/common/LoadingSpinner.vue'
import Toast from '../../components/common/Toast.vue'
import QuickViewModal from '../../components/common/QuickViewModal.vue'
import ProductCard from '../../components/user/ProductCard.vue'
import BrandCarousel from '../../components/user/BrandCarousel.vue'

const router = useRouter()
const cartStore = useCartStore()

// Use discount composable
const { fetchDiscounts, enrichProductWithDiscount } = useProductDiscount()

// Use composable for home page data
const {
  loading,
  error,
  transformedProducts,
  newProducts,
  brandSections,
  fetchHomePageData,
  getHotDeals,
  getTrendingProducts,
  getNewArrivals,
  getActiveCategories,
  getActiveVouchers,
  updateCountdown
} = useHomePage()

// Computed properties
const hotDeals = computed(() => getHotDeals(3))
const trendingProducts = computed(() => {
  const products = getTrendingProducts(8)
  return products.map(product => enrichProductWithDiscount(product))
})
const newArrivals = computed(() => getNewArrivals(4))
const categories = computed(() => getActiveCategories.value.map(c => c.name || 'Uncategorized'))
const activeVouchers = computed(() => getActiveVouchers.value)
const transformedNewProducts = computed(() => {
  // newProducts is already transformed, just ensure isNew flag is set
  return newProducts.value.map(product => ({
    ...product,
    isNew: true // All products in newProducts are marked as New
  }))
})

const newsletterEmail = ref('')
const showQuickView = ref(false)
const selectedProduct = ref(null)
const heroImages = ref([])
const currentHeroIndex = ref(0)
let heroCarouselInterval = null

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

const brandLogos = ref([
  { name: 'NIKE', class: 'text-3xl font-black text-gray-800 group-hover:text-black' },
  { name: 'adidas', class: 'text-3xl font-bold text-gray-700 group-hover:text-black italic' },
  { name: 'PUMA', class: 'text-3xl font-black text-gray-800 group-hover:text-black' },
  { name: 'Reebok', class: 'text-2xl font-bold text-gray-700 group-hover:text-red-600' },
  { name: 'VANS', class: 'text-3xl font-black text-gray-800 group-hover:text-black' },
  { name: 'Converse', class: 'text-2xl font-bold text-gray-700 group-hover:text-black italic' },
  { name: 'New Balance', class: 'text-xl font-bold text-gray-700 group-hover:text-red-600' },
  { name: 'ASICS', class: 'text-3xl font-black text-gray-800 group-hover:text-blue-600' }
])

const testimonials = ref([
  {
    name: 'Nguyễn Văn A',
    title: 'Khách hàng thân thiết',
    content: 'Giày chất lượng tuyệt vời, đúng như mô tả. Giao hàng nhanh, đóng gói cẩn thận. Tôi rất hài lòng với dịch vụ!'
  },
  {
    name: 'Trần Thị B',
    title: 'Đã mua 5 lần',
    content: 'Mình đã mua nhiều đôi giày ở đây, toàn hàng auth, giá cả hợp lý. Nhân viên tư vấn nhiệt tình, chu đáo!'
  },
  {
    name: 'Lê Văn C',
    title: 'Khách hàng mới',
    content: 'Lần đầu mua online nhưng rất ưng ý. Giày đẹp, êm chân, ship nhanh. Chắc chắn sẽ quay lại mua tiếp!'
  }
])

function formatPrice(price) {
  if (!price && price !== 0) return 'Liên hệ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function formatCurrency(amount) {
  if (!amount) return '0 VNĐ'
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
    maximumFractionDigits: 0
  }).format(amount)
}

function filterByCategory(categoryName) {
  router.push({ name: 'Products', query: { category: categoryName } })
}

function addToCart(product) {
  // Mở Quick View Modal thay vì thêm trực tiếp
  selectedProduct.value = product
  showQuickView.value = true
}

async function handleAddToCart({ product, size, color, quantity }) {
  // Find the matching variant
  const variant = product.variants?.find(v => 
    v.sizeName === size && v.colorName === color
  )
  
  if (!variant) {
    toast.message = 'Không tìm thấy biến thể sản phẩm'
    toast.type = 'error'
    toast.show = true
    return
  }

  // Validate stock
  const stock = variant.quantity || 0
  if (stock === 0) {
    toast.message = 'Sản phẩm đã hết hàng'
    toast.type = 'error'
    toast.show = true
    return
  }

  if (quantity > stock) {
    toast.message = `Chỉ còn ${stock} sản phẩm trong kho`
    toast.type = 'error'
    toast.show = true
    return
  }

  // Check if adding this quantity would exceed stock
  const existingCartItem = cartStore.items.find(item => 
    item.id === product.id && 
    item.size === size && 
    item.color === color
  )
  const totalQuantity = (existingCartItem?.quantity || 0) + quantity
  if (totalQuantity > stock) {
    toast.message = `Chỉ còn ${stock} sản phẩm trong kho. Bạn đã có ${existingCartItem?.quantity || 0} sản phẩm trong giỏ hàng.`
    toast.type = 'error'
    toast.show = true
    return
  }

  if (!variant?.id) {
    toast.message = 'Không tìm thấy thông tin sản phẩm'
    toast.type = 'error'
    toast.show = true
    return
  }

  try {
    await cartStore.addToCart(variant.id, quantity)
    toast.message = `Đã thêm ${quantity}x ${product.name} (Size ${size}, ${color}) vào giỏ hàng`
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    toast.message = error.message || 'Không thể thêm sản phẩm vào giỏ hàng'
    toast.type = 'error'
    toast.show = true
  }
}

function subscribeNewsletter() {
  if (newsletterEmail.value) {
    toast.message = 'Đăng ký nhận tin thành công! Cảm ơn bạn đã quan tâm.'
    toast.type = 'success'
    toast.show = true
    newsletterEmail.value = ''
  }
}

// Handle image errors
function handleImageError(event) {
  event.target.src = '/placeholder-shoe.jpg'
}

function handleHeroImageError(event) {
  event.target.src = '../../assets/giaybanner.png'
}

// Fetch hero images
async function fetchHeroImages() {
  try {
    heroImages.value = await imageApi.getHeroImages()
    if (heroImages.value.length > 0) {
      startHeroCarousel()
    }
  } catch (error) {
    console.error('Error fetching hero images:', error)
    // Fallback to default image
    heroImages.value = []
  }
}

// Auto-rotate hero images
function startHeroCarousel() {
  if (heroImages.value.length <= 1) return
  
  heroCarouselInterval = setInterval(() => {
    currentHeroIndex.value = (currentHeroIndex.value + 1) % heroImages.value.length
  }, 5000) // Change every 5 seconds
}

function stopHeroCarousel() {
  if (heroCarouselInterval) {
    clearInterval(heroCarouselInterval)
    heroCarouselInterval = null
  }
}

function transformBrandProduct(product) {
  // Find product in transformedProducts to get full details
  const fullProduct = transformedProducts.value.find(p => p.id === product.id)
  let transformed = null
  
  if (fullProduct) {
    transformed = {
      ...fullProduct,
      isNew: product.status === 'New' || fullProduct.isNew
    }
  } else {
    // If not found, return basic product with isNew flag
    transformed = {
      ...product,
      isNew: product.status === 'New',
      image: product.image || '/placeholder-shoe.jpg',
      price: product.price || 0,
      category: 'Uncategorized',
      variants: [],
      colors: [],
      sizes: []
    }
  }
  
  // Enrich with discount info
  return enrichProductWithDiscount(transformed)
}

// Update voucher countdown every second
let voucherInterval = null

onMounted(async () => {
  await Promise.all([
    fetchHomePageData(),
    fetchDiscounts(),
    fetchHeroImages()
  ])
  
  // Update voucher countdowns every second
  voucherInterval = setInterval(() => {
    updateCountdown()
  }, 1000)
  
  // Show error toast if data loading failed
  if (error.value) {
    toast.message = `Không thể tải dữ liệu: ${error.value}`
    toast.type = 'error'
    toast.show = true
  }
})

onUnmounted(() => {
  if (voucherInterval) {
    clearInterval(voucherInterval)
  }
  stopHeroCarousel()
})
</script>

<style scoped>
/* Hero Carousel Slide Animation */
.hero-slide-enter-active,
.hero-slide-leave-active {
  transition: all 0.8s ease-in-out;
}

.hero-slide-enter-from {
  opacity: 0;
  transform: translateY(30px) scale(0.95);
}

.hero-slide-enter-to {
  opacity: 1;
  transform: translateY(0) scale(1);
}

.hero-slide-leave-from {
  opacity: 1;
  transform: translateY(0) scale(1);
}

.hero-slide-leave-to {
  opacity: 0;
  transform: translateY(-30px) scale(0.95);
}
</style>

