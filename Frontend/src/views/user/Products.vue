<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Page Header -->
      <div class="mb-8 animate-fade-in-up">
        <h1 class="text-3xl md:text-4xl font-bold text-gray-900 mb-2">
          Tất cả sản phẩm
        </h1>
        <p class="text-gray-600">Khám phá bộ sưu tập giày thể thao đa dạng</p>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-4 gap-8">
        <!-- Filters Sidebar -->
        <aside class="lg:col-span-1">
          <div class="bg-white rounded-xl shadow-md p-6 sticky top-20 space-y-6 animate-fade-in-up">
            <div>
              <h3 class="font-bold text-gray-900 mb-4">Danh mục</h3>
              <div class="space-y-2">
                <button
                  @click="selectedCategory = 'All'"
                  :class="[
                    'w-full text-left px-4 py-2 rounded-lg transition-all duration-300',
                    selectedCategory === 'All'
                      ? 'bg-primary-500 text-white'
                      : 'hover:bg-gray-100 text-gray-700'
                  ]"
                >
                  Tất cả
                </button>
                <button
                  v-for="category in categories"
                  :key="category.id"
                  @click="selectedCategory = category.name"
                  :class="[
                    'w-full text-left px-4 py-2 rounded-lg transition-all duration-300',
                    selectedCategory === category.name
                      ? 'bg-primary-500 text-white'
                      : 'hover:bg-gray-100 text-gray-700'
                  ]"
                >
                  {{ category.name }}
                </button>
              </div>
            </div>

            <div class="pt-6 border-t">
              <h3 class="font-bold text-gray-900 mb-4">Giá</h3>
              <div class="space-y-3">
                <div>
                  <input
                    v-model="priceRange.min"
                    type="number"
                    placeholder="Từ"
                    class="input-field"
                  />
                </div>
                <div>
                  <input
                    v-model="priceRange.max"
                    type="number"
                    placeholder="Đến"
                    class="input-field"
                  />
                </div>
              </div>
            </div>


            <button
              @click="resetFilters"
              class="w-full btn-secondary"
            >
              Xóa bộ lọc
            </button>
          </div>
        </aside>

        <!-- Products Grid -->
        <div class="lg:col-span-3">
          <!-- Sort & View Options -->
          <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-6 animate-fade-in-up">
            <p class="text-gray-600">
              Hiển thị <span class="font-semibold">{{ filteredProducts.length }}</span> sản phẩm
            </p>
            <select v-model="sortBy" class="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent">
              <option value="default">Mặc định</option>
              <option value="price-asc">Giá: Thấp đến cao</option>
              <option value="price-desc">Giá: Cao đến thấp</option>
              <option value="newest">Mới nhất</option>
            </select>
          </div>

          <!-- Loading Spinner -->
          <LoadingSpinner :show="loading" message="Đang tải sản phẩm..." />

          <!-- Products Grid -->
          <div v-if="!loading" class="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-6">
            <ProductCard
              v-for="(product, index) in sortedProducts"
              :key="product.id"
              :product="product"
              @quick-view="addToCart"
              class="animate-fade-in-up"
              :style="{ animationDelay: `${index * 0.05}s` }"
            />
          </div>

          <!-- Empty State -->
          <div v-if="!loading && filteredProducts.length === 0" class="text-center py-12">
            <svg class="w-24 h-24 mx-auto text-gray-400 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
            </svg>
            <h3 class="text-xl font-bold text-gray-900 mb-2">Không tìm thấy sản phẩm</h3>
            <p class="text-gray-600 mb-4">Thử điều chỉnh bộ lọc của bạn</p>
            <button @click="resetFilters" class="btn-primary">
              Xóa bộ lọc
            </button>
          </div>
        </div>
      </div>
    </div>

    <Footer />

    <!-- Quick View Modal -->
    <QuickViewModal
      :show="showQuickView"
      :product="selectedProduct"
      @close="showQuickView = false"
      @add-to-cart="handleAddToCart"
      @error="(msg) => { toast.message = msg; toast.type = 'error'; toast.show = true; }"
    />

    <Toast
      :show="toast.show"
      :message="toast.message"
      :type="toast.type"
      @close="toast.show = false"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted, reactive, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useProductStore } from '../../stores/products'
import { useCartStore } from '../../stores/cart'
import { useProductDiscount } from '../../composables/useProductDiscount'
import { productApi, categoryApi, productDetailApi, handleApiError } from '../../services/api'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import LoadingSpinner from '../../components/common/LoadingSpinner.vue'
import Toast from '../../components/common/Toast.vue'
import ProductCard from '../../components/user/ProductCard.vue'
import QuickViewModal from '../../components/common/QuickViewModal.vue'

const route = useRoute()
const productStore = useProductStore()
const cartStore = useCartStore()
const { fetchDiscounts, enrichProductWithDiscount } = useProductDiscount()

const selectedCategory = ref('All')
const selectedBrand = ref('')
const sortBy = ref('default')
const priceRange = reactive({ min: 0, max: 10000000 })

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

// Data
const products = ref([])
const categories = ref([])
const productDetails = ref([])
const loading = ref(false)
const transformedProducts = ref([])
const selectedProduct = ref(null)
const showQuickView = ref(false)

// Transform Product + ProductDetails into display format
const transformProductForDisplay = (product, allProductDetails, allCategories) => {
  // Get all variants for this product
  const variants = allProductDetails.filter(pd => pd.productId === product.id)
  
  if (variants.length === 0) {
    return null // Skip products without variants
  }

  // Get the first variant with images for display
  const displayVariant = variants.find(v => 
    (v.images && v.images.length > 0) || v.imageUrl
  ) || variants[0]
  
  // Get category name
  const category = allCategories.find(c => c.id === product.categoryId)
  
  // Get price range - sử dụng finalPrice từ backend (đã tính discount)
  const prices = variants.map(v => v.finalPrice || v.price || 0).filter(p => p > 0)
  const minPrice = prices.length > 0 ? Math.min(...prices) : 0
  const maxPrice = prices.length > 0 ? Math.max(...prices) : 0
  
  // Lấy discount từ variant đầu tiên có discount (backend đã tính sẵn)
  const variantWithDiscount = variants.find(v => v.discountActive)
  const discountInfo = variantWithDiscount ? {
    discountType: variantWithDiscount.discountType,
    discountValue: variantWithDiscount.discountValue,
    discountActive: variantWithDiscount.discountActive
  } : null
  
  return {
    id: product.id,
    name: product.name,
    description: product.description,
    brand: product.brand,
    code: product.code,
    category: category?.name || 'Uncategorized',
    categoryId: product.categoryId,
    status: product.status,
    isActive: product.isActive,
    // From variant - sử dụng finalPrice từ backend
    price: displayVariant.finalPrice || displayVariant.price || minPrice || 0,
    image: displayVariant.images?.[0]?.url || displayVariant.imageUrl || '/placeholder-shoe.jpg',
    images: displayVariant.images || (displayVariant.imageUrl ? [{ url: displayVariant.imageUrl }] : []),
    // All variants for this product
    variants: variants.map(v => ({
      id: v.id,
      price: v.price, // Giá gốc
      finalPrice: v.finalPrice || v.price, // Giá sau discount (từ backend)
      quantity: v.quantity,
      colorName: v.colorName,
      sizeName: v.sizeName,
      colorId: v.colorId,
      sizeId: v.sizeId,
      imageUrl: v.imageUrl,
      images: v.images || [],
      discountType: v.discountType, // Từ backend
      discountValue: v.discountValue, // Từ backend
      discountActive: v.discountActive || false // Từ backend
    })),
    // Additional info
    minPrice: minPrice,
    maxPrice: maxPrice,
    hasMultiplePrices: new Set(variants.map(v => v.finalPrice || v.price)).size > 1,
    totalQuantity: variants.reduce((sum, v) => sum + (v.quantity || 0), 0),
    // For sorting/filtering
    createDate: product.createDate,
    // New product flag - based on Status="New"
    isNew: product.status === 'New',
    discountInfo: discountInfo
  }
}

const filteredProducts = computed(() => {
  return transformedProducts.value
    .filter(product => product.isActive) // Only show active products
    .filter(product => {
      const matchCategory = selectedCategory.value === 'All' || product.category === selectedCategory.value
      const matchBrand = !selectedBrand.value || product.brand === selectedBrand.value
      const productPrice = product.price || product.minPrice || 0
      const matchPrice = productPrice >= priceRange.min && (priceRange.max === 0 || productPrice <= priceRange.max)
      return matchCategory && matchBrand && matchPrice
    })
    .map(product => enrichProductWithDiscount(product)) // Add discount info
})

const sortedProducts = computed(() => {
  const products = [...filteredProducts.value]
  
  switch (sortBy.value) {
    case 'price-asc':
      return products.sort((a, b) => (a.discountedPrice || a.price || a.minPrice || 0) - (b.discountedPrice || b.price || b.minPrice || 0))
    case 'price-desc':
      return products.sort((a, b) => (b.discountedPrice || b.price || b.minPrice || 0) - (a.discountedPrice || a.price || a.minPrice || 0))
    case 'newest':
      return products.sort((a, b) => new Date(b.createDate || 0) - new Date(a.createDate || 0))
    default:
      return products
  }
})

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

function resetFilters() {
  selectedCategory.value = 'All'
  selectedBrand.value = ''
  sortBy.value = 'default'
  priceRange.min = 0
  priceRange.max = 10000000
}

function addToCart(product) {
  // Mở Quick View Modal
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

async function fetchProductsData() {
  loading.value = true
  try {
    const [productsData, categoriesData, productDetailsData] = await Promise.all([
      productApi.getAll().catch(err => {
        console.error('Error fetching products:', err)
        return []
      }),
      categoryApi.getAll().catch(err => {
        console.error('Error fetching categories:', err)
        return []
      }),
      productDetailApi.getAll().catch(err => {
        console.error('Error fetching product details:', err)
        return []
      })
    ])

    products.value = productsData.filter(p => p.isActive)
    categories.value = categoriesData.filter(c => c.status === 'Active')
    productDetails.value = productDetailsData

    // Transform products
    transformedProducts.value = products.value
      .map(product => transformProductForDisplay(product, productDetails.value, categories.value))
      .filter(p => p !== null) // Remove products without variants
  } catch (error) {
    console.error('Error fetching products data:', error)
    toast.message = 'Không thể tải dữ liệu sản phẩm'
    toast.type = 'error'
    toast.show = true
  } finally {
    loading.value = false
  }
}

// Function to update filters from route query
function updateFiltersFromRoute() {
  if (route.query.category) {
    selectedCategory.value = route.query.category
  } else {
    selectedCategory.value = 'All'
  }
  
  if (route.query.brand) {
    selectedBrand.value = route.query.brand
  } else {
    selectedBrand.value = ''
  }
}

onMounted(async () => {
  await Promise.all([
    fetchProductsData(),
    fetchDiscounts()
  ])
  
  // Check for category and brand from query params
  updateFiltersFromRoute()
})

// Watch route query params for changes
watch(() => route.query, () => {
  updateFiltersFromRoute()
}, { deep: true })
</script>

