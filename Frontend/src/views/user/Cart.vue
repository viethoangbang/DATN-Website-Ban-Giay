<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 class="text-3xl font-bold text-gray-900 mb-8 animate-fade-in-up">Giỏ hàng của bạn</h1>

      <div v-if="cartStore.items.length > 0" class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Cart Items -->
        <div class="lg:col-span-2 space-y-4">
          <div
            v-for="(item, index) in cartStore.items"
            :key="`${item.id}-${item.size}-${item.color}`"
            class="bg-white rounded-xl shadow-md p-6 flex flex-col sm:flex-row gap-6 animate-fade-in-up"
            :style="{ animationDelay: `${index * 0.1}s` }"
          >
            <img 
              :src="item.image || '/placeholder-shoe.jpg'" 
              :alt="item.name" 
              class="w-32 h-32 object-cover rounded-lg"
              @error="handleImageError"
            />
            
            <div class="flex-1">
              <div class="flex justify-between items-start mb-2">
                <div>
                  <h3 class="text-lg font-bold text-gray-900 mb-1">{{ item.name }}</h3>
                  <p class="text-sm font-semibold text-gray-600 mb-1">ID: {{ item.id }}</p>
                  <div class="flex flex-col space-y-1 text-sm text-gray-500 mb-2">
                    <p>Danh mục: {{ getCategoryName(item) }}</p>
                    <p>Hãng: {{ getBrandName(item) }}</p>
                  </div>
                  <div class="flex items-center space-x-3 text-sm">
                    <p class="text-gray-600">Size: {{ item.size }}</p>
                    <span v-if="item.color" class="flex items-center space-x-1">
                      <span class="text-gray-400">|</span>
                      <span class="text-gray-600">Màu:</span>
                      <span
                        class="inline-block w-4 h-4 rounded-full border border-gray-300"
                        :style="{ backgroundColor: item.color }"
                        :title="item.color"
                      ></span>
                    </span>
                  </div>
                </div>
                <button
                  @click="removeItem(item)"
                  class="text-red-600 hover:text-red-700 p-2 hover:bg-red-50 rounded-lg transition-colors"
                >
                  <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>

              <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mt-4">
                <div class="flex flex-col">
                  <div v-if="item.discountInfo && item.originalPrice && item.originalPrice > item.price" class="mb-1">
                    <span class="text-sm text-gray-400 line-through">
                      {{ formatPrice(item.originalPrice) }}
                    </span>
                  </div>
                  <div class="text-2xl font-bold text-primary-600">
                    {{ formatPrice(item.price) }}
                  </div>
                  <div v-if="item.discountInfo" class="mt-1">
                    <span class="inline-block px-2 py-1 bg-red-500 text-white rounded text-xs font-bold">
                      {{ item.discountInfo.discountType === 'Percentage' 
                        ? `-${item.discountInfo.discountValue}%` 
                        : `-${formatCurrency(item.discountInfo.discountValue)}` }}
                    </span>
                  </div>
                </div>

                <div class="flex flex-col items-end space-y-2">
                  <div class="flex items-center space-x-3">
                    <button
                      @click="updateQuantity(item, item.quantity - 1)"
                      :disabled="item.quantity <= 1"
                      class="w-8 h-8 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors flex items-center justify-center disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
                      </svg>
                    </button>
                    <span class="text-lg font-semibold w-8 text-center">{{ item.quantity }}</span>
                    <button
                      @click="updateQuantity(item, item.quantity + 1)"
                      :disabled="exceedsStock(item, item.quantity + 1) || isOutOfStock(item)"
                      class="w-8 h-8 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors flex items-center justify-center disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                      <svg class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                      </svg>
                    </button>
                  </div>
                  <div v-if="getItemStock(item) !== null" class="text-xs text-gray-500">
                    Còn {{ getItemStock(item) }} sản phẩm
                  </div>
                  <div v-if="isOutOfStock(item)" class="text-xs text-red-600 font-semibold">
                    Hết hàng
                  </div>
                </div>

                <div class="text-xl font-bold text-gray-900">
                  {{ formatPrice(item.price * item.quantity) }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Order Summary -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 animate-fade-in-up" style="animation-delay: 0.3s;">
            <h2 class="text-xl font-bold text-gray-900 mb-6">Tóm tắt đơn hàng</h2>

            <div class="space-y-4 mb-6">
              <div class="flex justify-between text-gray-600">
                <span>Tạm tính ({{ cartStore.totalItems }} sản phẩm)</span>
                <span class="font-semibold">{{ formatPrice(subtotal) }}</span>
              </div>
              
              <div class="flex justify-between text-gray-600">
                <span>Giảm giá sản phẩm</span>
                <span class="font-semibold text-dark-600">{{ formatPrice(productDiscount) }}</span>
              </div>
              
              <div class="border-t pt-4 flex justify-between text-lg font-bold text-gray-900">
                <span>Tổng cộng</span>
                <span class="text-primary-600">{{ formatPrice(finalTotal) }}</span>
              </div>
            </div>

            <router-link to="/checkout" class="block w-full btn-primary text-center py-4 text-lg mb-3">
              Tiến hành thanh toán
            </router-link>

            <router-link to="/products" class="block w-full btn-secondary text-center py-3">
              Tiếp tục mua sắm
            </router-link>
          </div>
        </div>
      </div>

      <!-- Empty Cart -->
      <div v-else class="text-center py-16 bg-white rounded-xl shadow-md animate-fade-in">
        <svg class="w-32 h-32 mx-auto text-gray-400 mb-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
        </svg>
        <h2 class="text-2xl font-bold text-gray-900 mb-4">Giỏ hàng trống</h2>
        <p class="text-gray-600 mb-8">Hãy thêm sản phẩm vào giỏ hàng để tiếp tục mua sắm</p>
        <router-link to="/products" class="btn-primary inline-block">
          Khám phá sản phẩm
        </router-link>
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
import { ref, computed, reactive, onMounted, watch } from 'vue'
import { useCartStore } from '../../stores/cart'
import { useAuthStore } from '../../stores/auth'
import { productApi, categoryApi, productDetailApi, discountApi, handleApiError } from '../../services/api'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'

const cartStore = useCartStore()
const authStore = useAuthStore()
const products = ref([]) // Store products to get categoryId
const categories = ref([]) // Store categories to get category name
const productDetails = ref([]) // Store product details to get stock for validation
const discounts = ref([]) // Store discounts for cart items
const loading = ref(false)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})


// Load products and categories to get categoryId
onMounted(async () => {
  loading.value = true
  try {
    const [productsData, categoriesData, discountsData] = await Promise.all([
      productApi.getAll(),
      categoryApi.getAll(),
      discountApi.getAll()
    ])
    products.value = productsData
    categories.value = categoriesData
    discounts.value = discountsData
    
    // Load product details for stock validation
    await loadProductDetailsForCart()
    
    // Fetch cart from backend if user is authenticated
    if (authStore.isAuthenticated) {
      await cartStore.fetchCart()
    }
  } catch (error) {
    console.error('Error loading data:', error)
  } finally {
    loading.value = false
  }
})

// Watch for auth changes to fetch cart
watch(() => authStore.isAuthenticated, async (isAuthenticated) => {
  if (isAuthenticated) {
    await cartStore.fetchCart()
  } else {
    // Clear cart if user logs out
    cartStore.items = []
  }
})

function formatPrice(price) {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price || 0)
}

// Calculate subtotal (original price before discounts)
const subtotal = computed(() => {
  return cartStore.items.reduce((total, item) => {
    const originalPrice = Number(item.originalPrice) || Number(item.price) || 0
    const quantity = Number(item.quantity) || 0
    return total + (originalPrice * quantity)
  }, 0)
})

// Calculate product discount (from discount table)
const productDiscount = computed(() => {
  return cartStore.items.reduce((total, item) => {
    const originalPrice = Number(item.originalPrice) || Number(item.price) || 0
    const finalPrice = Number(item.price) || 0
    const quantity = Number(item.quantity) || 0
    const itemDiscount = (originalPrice - finalPrice) * quantity
    return total + itemDiscount
  }, 0)
})

// Voucher discount (always 0 in cart, only applied at checkout)
const voucherDiscount = computed(() => {
  return 0
})

// Final total after all discounts
const finalTotal = computed(() => {
  return subtotal.value - productDiscount.value - voucherDiscount.value
})

function handleImageError(event) {
  event.target.src = '/placeholder-shoe.jpg'
}

// Get ProductDetailId for cart item
function getProductDetailId(item) {
  if (item.productDetailId) return item.productDetailId
  
  // Try to find from productDetails
  const productDetail = productDetails.value.find(pd => 
    pd.productId === item.id &&
    pd.sizeName === item.size &&
    pd.colorName === item.color
  )
  
  return productDetail?.id || null
}

// Get productId from ProductDetailId
function getProductIdFromDetailId(productDetailId) {
  const productDetail = productDetails.value.find(pd => pd.id === productDetailId)
  return productDetail?.productId || null
}

// Get stock quantity for item
const getItemStock = (item) => {
  if (item.stock !== null && item.stock !== undefined) return item.stock
  
  const productDetailId = getProductDetailId(item)
  if (!productDetailId) return null
  
  const productDetail = productDetails.value.find(pd => pd.id === productDetailId)
  return productDetail?.quantity || null
}

// Check if item is out of stock
const isOutOfStock = (item) => {
  const stock = getItemStock(item)
  return stock !== null && stock <= 0
}

// Check if quantity exceeds stock
const exceedsStock = (item, quantity) => {
  const stock = getItemStock(item)
  return stock !== null && quantity > stock
}

async function removeItem(item) {
  if (!authStore.isAuthenticated) {
    toast.message = 'Vui lòng đăng nhập để xóa sản phẩm'
    toast.type = 'error'
    toast.show = true
    return
  }
  
  if (!item.cartDetailId) {
    toast.message = 'Không tìm thấy sản phẩm trong giỏ hàng'
    toast.type = 'error'
    toast.show = true
    return
  }
  
  try {
    await cartStore.removeFromCart(item.cartDetailId)
    toast.message = 'Đã xóa sản phẩm khỏi giỏ hàng'
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    toast.message = handleApiError(error)
    toast.type = 'error'
    toast.show = true
  }
}

async function updateQuantity(item, newQuantity) {
  if (!authStore.isAuthenticated) {
    toast.message = 'Vui lòng đăng nhập để cập nhật giỏ hàng'
    toast.type = 'error'
    toast.show = true
    return
  }
  
  if (!item.cartDetailId) {
    toast.message = 'Không tìm thấy sản phẩm trong giỏ hàng'
    toast.type = 'error'
    toast.show = true
    return
  }
  
  if (newQuantity <= 0) {
    removeItem(item)
    return
  }
  
  // Validate stock
  const stock = getItemStock(item)
  if (stock !== null && newQuantity > stock) {
    toast.message = `Chỉ còn ${stock} sản phẩm trong kho`
    toast.type = 'error'
    toast.show = true
    return
  }
  
  try {
    await cartStore.updateQuantity(item.cartDetailId, newQuantity)
  } catch (error) {
    toast.message = handleApiError(error)
    toast.type = 'error'
    toast.show = true
  }
}


// Load product details for cart items
async function loadProductDetailsForCart() {
  try {
    const allProductDetails = await productDetailApi.getAll()
    productDetails.value = allProductDetails
  } catch (error) {
    console.error('Error loading product details:', error)
  }
}

// Get Category Name
function getCategoryName(item) {
  const product = products.value.find(p => p.id === item.id)
  if (!product) return 'Unknown'
  const category = categories.value.find(c => c.id === product.categoryId)
  return category ? category.name : 'Unknown'
}

// Get Brand Name
function getBrandName(item) {
  const product = products.value.find(p => p.id === item.id)
  return product?.brand || 'Unknown'
}

</script>

