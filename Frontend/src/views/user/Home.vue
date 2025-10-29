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
                <div class="text-4xl font-black text-gray-900">500+</div>
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
              
              <!-- Main Shoe - Container với size cố định -->
              <div class="absolute inset-0 flex items-center justify-center p-1">
                <img
                  src="../../assets/giaybanner.png"
                  alt="Featured Shoe"
                  class="w-full h-full object-contain transform hover:scale-110 transition-transform duration-700 drop-shadow-2xl animate-float"
                />
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
                    <div class="text-lg font-black bg-gradient-to-r from-red-600 to-orange-600 bg-clip-text text-transparent">-30%</div>
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
    <LoadingSpinner :show="productStore.loading" message="Đang tải sản phẩm..." />

    <!-- Categories Section -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="text-center mb-12 animate-fade-in-up">
        <h2 class="text-3xl md:text-4xl font-bold text-gray-900 mb-4">
          Danh Mục Sản Phẩm
        </h2>
        <p class="text-gray-600 text-lg">Tìm kiếm phong cách phù hợp với bạn</p>
      </div>

      <div class="grid grid-cols-2 md:grid-cols-4 gap-6">
        <div
          v-for="(category, index) in categories"
          :key="category"
          class="group cursor-pointer animate-fade-in-up"
          :style="{ animationDelay: `${index * 0.1}s` }"
          @click="filterByCategory(category)"
        >
          <div class="bg-white rounded-xl shadow-md p-6 text-center transition-all duration-300 hover:shadow-xl hover:scale-105 group-hover:bg-primary-500">
            <div class="w-16 h-16 bg-primary-100 group-hover:bg-white rounded-full flex items-center justify-center mx-auto mb-4 transition-colors">
              <svg class="w-8 h-8 text-primary-500 transition-transform group-hover:scale-110" fill="currentColor" viewBox="0 0 20 20">
                <path d="M3 1a1 1 0 000 2h1.22l.305 1.222a.997.997 0 00.01.042l1.358 5.43-.893.892C3.74 11.846 4.632 14 6.414 14H15a1 1 0 000-2H6.414l1-1H14a1 1 0 00.894-.553l3-6A1 1 0 0017 3H6.28l-.31-1.243A1 1 0 005 1H3zM16 16.5a1.5 1.5 0 11-3 0 1.5 1.5 0 013 0zM6.5 18a1.5 1.5 0 100-3 1.5 1.5 0 000 3z" />
              </svg>
            </div>
            <h3 class="font-bold text-gray-900 group-hover:text-white transition-colors">{{ category }}</h3>
          </div>
        </div>
      </div>
    </section>

    <!-- Flash Sale Banner -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="bg-gradient-to-r from-red-500 via-pink-500 to-purple-500 rounded-2xl overflow-hidden shadow-xl animate-fade-in-up">
        <div class="p-8 md:p-12 flex flex-col md:flex-row items-center justify-between text-white">
          <div class="mb-6 md:mb-0">
            <div class="flex items-center space-x-2 mb-2">
              <svg class="w-6 h-6 animate-pulse" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M12.395 2.553a1 1 0 00-1.45-.385c-.345.23-.614.558-.822.88-.214.33-.403.713-.57 1.116-.334.804-.614 1.768-.84 2.734a31.365 31.365 0 00-.613 3.58 2.64 2.64 0 01-.945-1.067c-.328-.68-.398-1.534-.398-2.654A1 1 0 005.05 6.05 6.981 6.981 0 003 11a7 7 0 1011.95-4.95c-.592-.591-.98-.985-1.348-1.467-.363-.476-.724-1.063-1.207-2.03zM12.12 15.12A3 3 0 017 13s.879.5 2.5.5c0-1 .5-4 1.25-4.5.5 1 .786 1.293 1.371 1.879A2.99 2.99 0 0113 13a2.99 2.99 0 01-.879 2.121z" clip-rule="evenodd" />
              </svg>
              <span class="text-sm font-bold uppercase">Flash Sale</span>
            </div>
            <h3 class="text-3xl md:text-4xl font-bold mb-2">Giảm đến 50%</h3>
            <p class="text-white/90 mb-4">Khuyến mãi có hạn - Nhanh tay đặt hàng!</p>
            <div class="flex items-center space-x-4">
              <div class="text-center">
                <div class="bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <span class="text-2xl font-bold">12</span>
                  <div class="text-xs">Giờ</div>
                </div>
              </div>
              <span class="text-2xl">:</span>
              <div class="text-center">
                <div class="bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <span class="text-2xl font-bold">34</span>
                  <div class="text-xs">Phút</div>
                </div>
              </div>
              <span class="text-2xl">:</span>
              <div class="text-center">
                <div class="bg-white/20 backdrop-blur-sm px-4 py-2 rounded-lg">
                  <span class="text-2xl font-bold">56</span>
                  <div class="text-xs">Giây</div>
                </div>
              </div>
            </div>
          </div>
          <div>
            <router-link to="/products" class="btn-primary bg-white text-purple-600 hover:bg-gray-100 px-8 py-4 text-lg">
              Mua ngay
            </router-link>
          </div>
        </div>
      </div>
    </section>

    <!-- Hot Deals Section -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="flex justify-between items-center mb-12">
        <div class="animate-fade-in-up">
          <div class="flex items-center space-x-2 mb-2">
            <svg class="w-8 h-8 text-red-500" fill="currentColor" viewBox="0 0 20 20">
              <path fill-rule="evenodd" d="M12.395 2.553a1 1 0 00-1.45-.385c-.345.23-.614.558-.822.88-.214.33-.403.713-.57 1.116-.334.804-.614 1.768-.84 2.734a31.365 31.365 0 00-.613 3.58 2.64 2.64 0 01-.945-1.067c-.328-.68-.398-1.534-.398-2.654A1 1 0 005.05 6.05 6.981 6.981 0 003 11a7 7 0 1011.95-4.95c-.592-.591-.98-.985-1.348-1.467-.363-.476-.724-1.063-1.207-2.03zM12.12 15.12A3 3 0 017 13s.879.5 2.5.5c0-1 .5-4 1.25-4.5.5 1 .786 1.293 1.371 1.879A2.99 2.99 0 0113 13a2.99 2.99 0 01-.879 2.121z" clip-rule="evenodd" />
            </svg>
            <h2 class="text-3xl md:text-4xl font-bold text-gray-900">
              Deals Hấp Dẫn
            </h2>
          </div>
          <p class="text-gray-600">Giá tốt nhất - Không thể bỏ lỡ!</p>
        </div>
      </div>

      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="(product, index) in hotDeals"
          :key="product.id"
          class="group animate-fade-in-up bg-gradient-to-br from-red-50 to-orange-50 rounded-xl overflow-hidden shadow-md hover:shadow-2xl transition-all duration-300 hover:-translate-y-2"
          :style="{ animationDelay: `${index * 0.1}s` }"
        >
          <div class="relative">
            <div class="aspect-square overflow-hidden">
              <img
                :src="product.image"
                :alt="product.name"
                class="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-500"
              />
            </div>
            <div class="absolute top-4 left-4 bg-red-500 text-white px-4 py-2 rounded-full text-sm font-bold shadow-lg animate-pulse">
              -{{ product.discount }}%
            </div>
            <button class="absolute top-4 right-4 w-10 h-10 bg-white rounded-full flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity shadow-lg hover:scale-110">
              <svg class="w-5 h-5 text-red-500" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" clip-rule="evenodd" />
              </svg>
            </button>
          </div>
          <div class="p-6">
            <span class="text-sm text-primary-600 font-semibold">{{ product.category }}</span>
            <h3 class="text-lg font-bold text-gray-900 mb-2 mt-1 group-hover:text-primary-600 transition-colors">
              {{ product.name }}
            </h3>
            <div class="flex items-center space-x-2 mb-4">
              <span class="text-2xl font-bold text-red-600">
                {{ formatPrice(product.salePrice) }}
              </span>
              <span class="text-gray-400 line-through text-sm">
                {{ formatPrice(product.price) }}
              </span>
            </div>
            <button
              @click="addToCart(product)"
              class="w-full bg-primary-500 text-white py-3 rounded-lg font-semibold hover:bg-primary-600 transition-colors flex items-center justify-center space-x-2"
            >
              <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
              </svg>
              <span>Thêm vào giỏ</span>
            </button>
          </div>
        </div>
      </div>
    </section>

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

        <div class="grid grid-cols-2 md:grid-cols-4 gap-4 md:gap-6">
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
                  class="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-500"
                />
                <div class="absolute top-2 right-2 bg-yellow-400 text-gray-900 px-2 py-1 rounded-full text-xs font-bold flex items-center space-x-1">
                  <svg class="w-3 h-3" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                  </svg>
                  <span>Hot</span>
                </div>
              </div>
              <div class="p-4">
                <h3 class="font-bold text-gray-900 text-sm mb-1 group-hover:text-primary-600 transition-colors line-clamp-1">
                  {{ product.name }}
                </h3>
                <div class="flex items-center justify-between">
                  <span class="text-lg font-bold text-primary-600">
                    {{ formatPrice(product.price) }}
                  </span>
                  <button
                    @click="addToCart(product)"
                    class="p-2 bg-primary-100 text-primary-600 rounded-lg hover:bg-primary-500 hover:text-white transition-colors"
                  >
                    <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                    </svg>
                  </button>
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

    <!-- New Arrivals -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="flex justify-between items-center mb-12">
        <div class="animate-fade-in-up">
          <div class="flex items-center space-x-2 mb-2">
            <span class="px-3 py-1 bg-green-100 text-green-700 rounded-full text-sm font-bold">Mới</span>
            <h2 class="text-3xl md:text-4xl font-bold text-gray-900">
              Hàng Mới Về
            </h2>
          </div>
          <p class="text-gray-600">Cập nhật bộ sưu tập mới nhất</p>
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

      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        <div
          v-for="(product, index) in newArrivals"
          :key="product.id"
          class="group animate-fade-in-up"
          :style="{ animationDelay: `${index * 0.1}s` }"
        >
          <div class="bg-white rounded-xl shadow-md overflow-hidden transition-all duration-300 hover:shadow-2xl hover:-translate-y-2">
            <div class="relative overflow-hidden bg-gradient-to-br from-green-50 to-blue-50 aspect-square">
              <img
                :src="product.image"
                :alt="product.name"
                class="w-full h-full object-cover transform group-hover:scale-110 group-hover:rotate-2 transition-all duration-500"
              />
              <div class="absolute top-4 right-4 bg-green-500 text-white px-3 py-1 rounded-full text-xs font-bold">
                New Arrival
              </div>
            </div>
            <div class="p-6">
              <div class="flex items-center justify-between mb-2">
                <span class="text-sm text-primary-600 font-semibold">{{ product.category }}</span>
                <div class="flex items-center space-x-1">
                  <svg class="w-4 h-4 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                  </svg>
                  <span class="text-sm text-gray-600">{{ product.rating }}</span>
                </div>
              </div>
              <h3 class="text-lg font-bold text-gray-900 mb-2 group-hover:text-primary-600 transition-colors line-clamp-2">
                {{ product.name }}
              </h3>
              <div class="flex items-center justify-between mt-4">
                <span class="text-2xl font-bold text-primary-600">
                  {{ formatPrice(product.price) }}
                </span>
                <button
                  @click="addToCart(product)"
                  class="bg-primary-500 text-white px-4 py-2 rounded-lg hover:bg-primary-600 transition-colors transform hover:scale-110 flex items-center space-x-1"
                >
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                  </svg>
                </button>
              </div>
            </div>
          </div>
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

    <!-- Testimonials -->
    <section class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
      <div class="text-center mb-12 animate-fade-in-up">
        <h2 class="text-3xl md:text-4xl font-bold text-gray-900 mb-2">Khách Hàng Nói Gì</h2>
        <p class="text-gray-600">Đánh giá từ khách hàng thực tế</p>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
        <div
          v-for="(review, index) in testimonials"
          :key="index"
          class="bg-white rounded-xl shadow-md p-6 hover:shadow-xl transition-all duration-300 animate-fade-in-up"
          :style="{ animationDelay: `${index * 0.15}s` }"
        >
          <div class="flex items-center mb-4">
            <div v-for="i in 5" :key="i" class="w-5 h-5">
              <svg class="w-5 h-5 text-yellow-400" fill="currentColor" viewBox="0 0 20 20">
                <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
              </svg>
            </div>
          </div>
          <p class="text-gray-700 mb-6 italic">"{{ review.content }}"</p>
          <div class="flex items-center space-x-3">
            <div class="w-12 h-12 bg-primary-500 rounded-full flex items-center justify-center text-white font-bold text-lg">
              {{ review.name.charAt(0) }}
            </div>
            <div>
              <div class="font-bold text-gray-900">{{ review.name }}</div>
              <div class="text-sm text-gray-600">{{ review.title }}</div>
            </div>
          </div>
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
import { ref, computed, onMounted, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useProductStore } from '../../stores/products'
import { useCartStore } from '../../stores/cart'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import LoadingSpinner from '../../components/common/LoadingSpinner.vue'
import Toast from '../../components/common/Toast.vue'
import QuickViewModal from '../../components/common/QuickViewModal.vue'

const router = useRouter()
const productStore = useProductStore()
const cartStore = useCartStore()

const newsletterEmail = ref('')
const showQuickView = ref(false)
const selectedProduct = ref(null)

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

const categories = computed(() => productStore.categories.filter(c => c !== 'All'))

// Hot Deals - 3 sản phẩm giảm giá
const hotDeals = computed(() => {
  return productStore.products.slice(0, 3).map((product, index) => ({
    ...product,
    discount: [30, 40, 50][index],
    salePrice: product.price * (1 - [0.3, 0.4, 0.5][index])
  }))
})

// Trending - 8 sản phẩm
const trendingProducts = computed(() => productStore.products.slice(3, 11))

// New Arrivals - 4 sản phẩm
const newArrivals = computed(() => productStore.products.slice(11, 15))

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price)
}

function filterByCategory(category) {
  router.push({ name: 'Products', query: { category } })
}

function addToCart(product) {
  // Mở Quick View Modal thay vì thêm trực tiếp
  selectedProduct.value = product
  showQuickView.value = true
}

function handleAddToCart({ product, size, color, quantity }) {
  // Thêm vào giỏ với thông tin đã chọn
  for (let i = 0; i < quantity; i++) {
    cartStore.addToCart(product, size, color)
  }
  
  toast.message = `Đã thêm ${quantity}x ${product.name} (Size ${size}) vào giỏ hàng`
  toast.type = 'success'
  toast.show = true
}

function subscribeNewsletter() {
  if (newsletterEmail.value) {
    toast.message = 'Đăng ký nhận tin thành công! Cảm ơn bạn đã quan tâm.'
    toast.type = 'success'
    toast.show = true
    newsletterEmail.value = ''
  }
}

onMounted(async () => {
  await productStore.fetchProducts()
  cartStore.loadFromLocalStorage()
})
</script>

