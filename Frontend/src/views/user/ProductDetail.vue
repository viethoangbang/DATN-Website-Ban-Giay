<template>
  <div class="min-h-screen bg-gray-50">
    <Navbar />

    <div v-if="loading" class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="text-center py-20">
        <div class="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-primary-600"></div>
        <p class="mt-4 text-gray-600">Đang tải sản phẩm...</p>
      </div>
    </div>

    <div v-else-if="product" class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Breadcrumb -->
      <nav class="flex mb-8 animate-fade-in" aria-label="Breadcrumb">
        <ol class="inline-flex items-center space-x-1 md:space-x-3">
          <li><router-link to="/" class="text-gray-600 hover:text-primary-600">Trang chủ</router-link></li>
          <li><span class="text-gray-400 mx-2">/</span></li>
          <li><router-link to="/products" class="text-gray-600 hover:text-primary-600">Sản phẩm</router-link></li>
          <li><span class="text-gray-400 mx-2">/</span></li>
          <li class="text-primary-600 font-semibold line-clamp-1">{{ product.name }}</li>
        </ol>
      </nav>

      <!-- Product Details -->
      <div class="grid grid-cols-1 lg:grid-cols-2 gap-12 mb-12">
        <!-- Product Images -->
        <div class="animate-fade-in-up">
          <div class="bg-white rounded-2xl shadow-lg overflow-hidden mb-4">
            <img 
              :src="currentMainImage" 
              :alt="product.name" 
              class="w-full h-auto object-cover"
              @error="handleImageError"
            />
          </div>
          <div v-if="productImages.length > 1" class="grid grid-cols-4 gap-4">
            <button
              v-for="(img, index) in productImages.slice(0, 4)"
              :key="index"
              @click="currentMainImage = img.url || img"
              :class="[
                'bg-white rounded-lg overflow-hidden shadow-md hover:shadow-lg transition-shadow cursor-pointer border-2',
                currentMainImage === (img.url || img) ? 'border-primary-500' : 'border-transparent'
              ]"
            >
              <img 
                :src="img.url || img" 
                :alt="`${product.name} ${index + 1}`" 
                class="w-full h-full object-cover aspect-square"
                @error="handleImageError"
              />
            </button>
          </div>
        </div>

        <!-- Product Info -->
        <div class="animate-fade-in-up" style="animation-delay: 0.2s;">
          <div class="bg-white rounded-2xl shadow-lg p-8">
            <!-- Category & Status Badges -->
            <div class="flex items-center gap-2 mb-4">
              <span class="inline-block px-4 py-1 bg-primary-100 text-primary-600 rounded-full text-sm font-semibold">
                {{ product.category }}
              </span>
              <span v-if="product.isNew" class="inline-block px-4 py-1 bg-primary-500 text-white rounded-full text-sm font-bold">
                MỚI
              </span>
              <span v-if="selectedVariant && selectedVariant.discountActive" class="inline-block px-4 py-1 bg-red-500 text-white rounded-full text-sm font-bold">
                {{ selectedVariant.discountType === 'Percentage' 
                  ? `-${selectedVariant.discountValue}%` 
                  : `-${formatCurrency(selectedVariant.discountValue)}` }}
              </span>
            </div>
            
            <h1 class="text-3xl md:text-4xl font-bold text-gray-900 mb-4">
              {{ product.name }}
            </h1>
            
            <!-- Price -->
            <div class="mb-6">
              <div v-if="selectedVariant && selectedVariant.discountActive && selectedVariantPrice && selectedVariantDiscountedPrice < selectedVariantPrice" class="mb-2">
                <span class="text-xl text-gray-400 line-through">
                  {{ formatPrice(selectedVariantPrice) }}
                </span>
              </div>
              <div>
                <span class="text-4xl font-bold text-primary-600">
                  {{ formatPrice(selectedVariantDiscountedPrice) }}
                </span>
              </div>
            </div>

            <!-- Description -->
            <p class="text-gray-600 mb-6 leading-relaxed">
              {{ product.description || 'Chưa có mô tả' }}
            </p>

            <!-- Size Selection -->
            <div class="mb-6">
              <h3 class="font-bold text-gray-900 mb-3">Chọn size:</h3>
              <div class="grid grid-cols-6 gap-2">
                <button
                  v-for="size in availableSizes"
                  :key="size"
                  @click="selectedSize = size"
                  :disabled="!isSizeAvailable(size)"
                  :class="[
                    'py-3 rounded-lg font-semibold transition-all duration-300',
                    selectedSize === size
                      ? 'bg-primary-500 text-white shadow-lg scale-105'
                      : isSizeAvailable(size)
                      ? 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                      : 'bg-gray-50 text-gray-400 cursor-not-allowed opacity-50'
                  ]"
                >
                  {{ size }}
                </button>
              </div>
            </div>

            <!-- Color Selection -->
            <div class="mb-6">
              <h3 class="font-bold text-gray-900 mb-3">Màu sắc:</h3>
              <div class="flex flex-wrap gap-3">
                <button
                  v-for="color in availableColors"
                  :key="color"
                  @click="selectedColor = color"
                  :class="[
                    'w-10 h-10 rounded-full border-2 transition-all duration-300 shadow-sm',
                    selectedColor === color
                      ? 'border-primary-500 scale-110 ring-2 ring-primary-200'
                      : 'border-gray-300 hover:border-gray-400 hover:scale-105'
                  ]"
                  :style="{ backgroundColor: getColorCode(color) }"
                  :title="color"
                ></button>
              </div>
            </div>

            <!-- Quantity -->
            <div class="mb-8">
              <h3 class="font-bold text-gray-900 mb-3">Số lượng:</h3>
              <div class="flex items-center space-x-4">
                <button 
                  @click="quantity = Math.max(1, quantity - 1)" 
                  class="w-10 h-10 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors"
                >
                  <svg class="w-5 h-5 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
                  </svg>
                </button>
                <span class="text-xl font-bold w-12 text-center">{{ quantity }}</span>
                <button 
                  @click="quantity++" 
                  class="w-10 h-10 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors"
                >
                  <svg class="w-5 h-5 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
                  </svg>
                </button>
                <span v-if="selectedVariant" class="text-sm text-gray-500">
                  (Còn {{ selectedVariant.quantity || 0 }} sản phẩm)
                </span>
              </div>
            </div>

            <!-- Actions -->
            <div class="flex flex-col sm:flex-row gap-4">
              <button 
                @click="handleAddToCart" 
                :disabled="!selectedVariant || (selectedVariant.quantity || 0) === 0"
                :class="[
                  'flex-1 btn-primary py-4 text-lg transition-all',
                  (!selectedVariant || (selectedVariant.quantity || 0) === 0) 
                    ? 'opacity-50 cursor-not-allowed' 
                    : ''
                ]"
              >
                <svg class="w-6 h-6 inline-block mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                {{ (!selectedVariant || (selectedVariant.quantity || 0) === 0) ? 'Hết hàng' : 'Thêm vào giỏ' }}
              </button>
              <button 
                @click="toggleWishlist"
                :class="[
                  'px-8 py-4 border-2 rounded-lg font-bold transition-all',
                  isWishlisted 
                    ? 'border-red-500 text-red-500 bg-red-50' 
                    : 'border-primary-500 text-primary-500 hover:bg-primary-50'
                ]"
              >
                <svg 
                  class="w-6 h-6 inline-block" 
                  :fill="isWishlisted ? 'currentColor' : 'none'" 
                  viewBox="0 0 24 24" 
                  stroke="currentColor"
                >
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                </svg>
              </button>
            </div>

          </div>
        </div>
      </div>

      <!-- Related Products -->
      <div v-if="relatedProducts.length > 0" class="mt-16">
        <h2 class="text-2xl font-bold text-gray-900 mb-6">Sản phẩm liên quan</h2>
        <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          <ProductCard
            v-for="relatedProduct in relatedProducts"
            :key="relatedProduct.id"
            :product="relatedProduct"
            @quick-view="$emit('quick-view', $event)"
          />
        </div>
      </div>
    </div>

    <div v-else class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="text-center py-20">
        <p class="text-gray-600 text-lg">Không tìm thấy sản phẩm</p>
        <router-link to="/products" class="mt-4 inline-block text-primary-600 hover:underline">
          Quay lại danh sách sản phẩm
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
import { ref, computed, onMounted, reactive, watch } from 'vue'
import { useRoute } from 'vue-router'
import { productApi, productDetailApi, categoryApi, discountApi } from '@/services/api'
import { useCartStore } from '../../stores/cart'
import { useProductDiscount } from '@/composables/useProductDiscount'
import Navbar from '../../components/user/Navbar.vue'
import Footer from '../../components/user/Footer.vue'
import Toast from '../../components/common/Toast.vue'
import ProductCard from '../../components/user/ProductCard.vue'

const route = useRoute()
const cartStore = useCartStore()
const { enrichProductWithDiscount } = useProductDiscount()

const product = ref(null)
const productDetails = ref([])
const categories = ref([])
const loading = ref(true)
const selectedSize = ref(null)
const selectedColor = ref(null)
const quantity = ref(1)
const currentMainImage = ref('')
const isWishlisted = ref(false)

const toast = reactive({
  show: false,
  message: '',
  type: 'success'
})

// Get available colors and sizes from variants
const availableColors = computed(() => {
  if (!product.value || !product.value.variants) return []
  return [...new Set(product.value.variants.map(v => v.colorName).filter(Boolean))]
})

const availableSizes = computed(() => {
  if (!product.value || !product.value.variants) return []
  return [...new Set(product.value.variants.map(v => v.sizeName).filter(Boolean))].sort((a, b) => {
    // Sort sizes numerically if they're numbers
    const aNum = parseInt(a)
    const bNum = parseInt(b)
    if (!isNaN(aNum) && !isNaN(bNum)) return aNum - bNum
    return a.localeCompare(b)
  })
})

// Filter sizes based on selected color
const availableSizesForColor = computed(() => {
  if (!selectedColor.value || !product.value?.variants) return availableSizes.value
  
  return [...new Set(
    product.value.variants
      .filter(v => v.colorName === selectedColor.value && (v.quantity || 0) > 0)
      .map(v => v.sizeName)
      .filter(Boolean)
  )].sort((a, b) => {
    const aNum = parseInt(a)
    const bNum = parseInt(b)
    if (!isNaN(aNum) && !isNaN(bNum)) return aNum - bNum
    return a.localeCompare(b)
  })
})

const isSizeAvailable = (size) => {
  if (!selectedColor.value) return true
  return availableSizesForColor.value.includes(size)
}

// Get selected variant based on color and size
const selectedVariant = computed(() => {
  if (!product.value?.variants || !selectedSize.value) return null
  
  return product.value.variants.find(v => 
    v.sizeName === selectedSize.value && 
    (!selectedColor.value || v.colorName === selectedColor.value)
  ) || product.value.variants.find(v => v.sizeName === selectedSize.value)
})

const selectedVariantPrice = computed(() => {
  return selectedVariant.value?.price || product.value?.price || product.value?.minPrice || 0
})

// Check if quantity exceeds stock
const quantityExceedsStock = computed(() => {
  if (!selectedVariant.value || !selectedVariant.value.quantity) return false
  return quantity.value > selectedVariant.value.quantity
})

// Check if can increase quantity
const canIncreaseQuantity = computed(() => {
  if (!selectedVariant.value) return false
  const stock = selectedVariant.value.quantity || 0
  return quantity.value < stock
})

// Increase quantity with validation
function increaseQuantity() {
  if (!selectedVariant.value) return
  const stock = selectedVariant.value.quantity || 0
  if (quantity.value < stock) {
    quantity.value++
  } else {
    toast.message = `Chỉ còn ${stock} sản phẩm trong kho`
    toast.type = 'error'
    toast.show = true
  }
}

// Giá sau discount từ backend (finalPrice từ ProductDetail - backend đã tính sẵn)
const selectedVariantDiscountedPrice = computed(() => {
  // Dùng finalPrice từ backend (đã tính discount sẵn)
  return selectedVariant.value?.finalPrice || selectedVariant.value?.price || product.value?.price || product.value?.minPrice || 0
})

// Get all images for the product, filtered by selected color
const productImages = computed(() => {
  if (!product.value) return []
  
  // If a color is selected, only show images from variants of that color
  const variantsToUse = selectedColor.value 
    ? product.value.variants?.filter(v => 
        v.colorName && v.colorName.trim().toLowerCase() === selectedColor.value.trim().toLowerCase()
      ) || []
    : product.value.variants || []
  
  // Collect all images from filtered variants
  const allImages = []
  variantsToUse.forEach(variant => {
    if (variant.images && variant.images.length > 0) {
      allImages.push(...variant.images)
    } else if (variant.imageUrl) {
      allImages.push({ url: variant.imageUrl })
    }
  })
  
  // Remove duplicates
  const uniqueImages = []
  const seen = new Set()
  allImages.forEach(img => {
    const url = img.url || img
    if (!seen.has(url)) {
      seen.add(url)
      uniqueImages.push(img)
    }
  })
  
  // If no images found for selected color, fallback to all product images
  if (uniqueImages.length === 0 && product.value.variants) {
    product.value.variants.forEach(variant => {
      if (variant.images && variant.images.length > 0) {
        allImages.push(...variant.images)
      } else if (variant.imageUrl) {
        allImages.push({ url: variant.imageUrl })
      }
    })
    
    const seen2 = new Set()
    allImages.forEach(img => {
      const url = img.url || img
      if (!seen2.has(url)) {
        seen2.add(url)
        uniqueImages.push(img)
      }
    })
  }
  
  return uniqueImages.length > 0 ? uniqueImages : [{ url: product.value.image || '/placeholder-shoe.jpg' }]
})

// Watch for color change to update image and available sizes
watch(selectedColor, (newColor) => {
  if (!product.value?.variants || !newColor) return
  
  // Update selected size if current size is not available for new color
  if (selectedSize.value && !isSizeAvailable(selectedSize.value)) {
    selectedSize.value = availableSizesForColor.value[0] || null
  }
  
  // productImages computed will automatically update based on selectedColor
  // Then we'll update currentMainImage via watch on productImages
}, { immediate: false })

// Watch productImages to update currentMainImage when color changes
watch(productImages, (newImages) => {
  if (newImages && newImages.length > 0) {
    const firstImage = newImages[0]
    if (firstImage) {
      const imageUrl = firstImage.url || firstImage
      if (imageUrl && imageUrl !== currentMainImage.value) {
        currentMainImage.value = imageUrl
      }
    }
  }
}, { immediate: true })

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

function handleImageError(event) {
  event.target.src = '/placeholder-shoe.jpg'
}

function getColorCode(colorName) {
  const colorMap = {
    'Black': '#000000',
    'White': '#FFFFFF',
    'Red': '#EF4444',
    'Blue': '#3B82F6',
    'Green': '#10B981',
    'Yellow': '#F59E0B',
    'Purple': '#8B5CF6',
    'Pink': '#EC4899',
    'Gray': '#6B7280',
    'Grey': '#6B7280',
    'Brown': '#92400E',
    'Navy': '#1E3A8A',
    'Orange': '#F97316',
    'Beige': '#F5F5DC',
    'Khaki': '#C3B091',
    'Olive': '#808000',
    'Maroon': '#800000',
    'Burgundy': '#800020',
    'Cream': '#FFFDD0',
    'Tan': '#D2B48C',
    'Gold': '#FFD700',
    'Silver': '#C0C0C0'
  }
  return colorMap[colorName] || '#6B7280'
}

function toggleWishlist() {
  isWishlisted.value = !isWishlisted.value
}

async function handleAddToCart() {
  if (!selectedSize.value) {
    toast.message = 'Vui lòng chọn size'
    toast.type = 'warning'
    toast.show = true
    return
  }

  if (!selectedVariant.value || (selectedVariant.value.quantity || 0) === 0) {
    toast.message = 'Sản phẩm đã hết hàng'
    toast.type = 'warning'
    toast.show = true
    return
  }

  // Validate quantity
  const stock = selectedVariant.value.quantity || 0
  if (quantity.value > stock) {
    toast.message = `Chỉ còn ${stock} sản phẩm trong kho`
    toast.type = 'error'
    toast.show = true
    return
  }

  // Check if adding this quantity would exceed stock
  const existingCartItem = cartStore.items.find(item => 
    item.id === product.value.id && 
    item.size === selectedSize.value && 
    item.color === (selectedColor.value || '')
  )
  const totalQuantity = (existingCartItem?.quantity || 0) + quantity.value
  if (totalQuantity > stock) {
    toast.message = `Chỉ còn ${stock} sản phẩm trong kho. Bạn đã có ${existingCartItem?.quantity || 0} sản phẩm trong giỏ hàng.`
    toast.type = 'error'
    toast.show = true
    return
  }

  if (!selectedVariant.value?.id) {
    toast.message = 'Vui lòng chọn size và màu sắc'
    toast.type = 'error'
    toast.show = true
    return
  }

  try {
    await cartStore.addToCart(selectedVariant.value.id, quantity.value)
    
    // Format message with product name, size, and color (like QuickView)
    const productName = product.value?.name || 'sản phẩm'
    const size = selectedSize.value || ''
    const color = selectedColor.value || ''
    const colorText = color ? `${color}` : ''
    const sizeText = size ? `Size ${size}` : ''
    const detailText = [sizeText, colorText].filter(Boolean).join(', ')
    
    toast.message = `Đã thêm ${quantity.value}x ${productName}${detailText ? ` (${detailText})` : ''} vào giỏ hàng`
    toast.type = 'success'
    toast.show = true
  } catch (error) {
    toast.message = error.message || 'Không thể thêm sản phẩm vào giỏ hàng'
    toast.type = 'error'
    toast.show = true
  }
}

// Transform product data
function transformProductForDisplay(productData, allProductDetails, allCategories, discounts) {
  const variants = allProductDetails.filter(pd => pd.productId === productData.id)
  
  if (variants.length === 0) {
    return null
  }

  const displayVariant = variants.find(v => 
    (v.images && v.images.length > 0) || v.imageUrl
  ) || variants[0]
  
  const category = allCategories.find(c => c.id === productData.categoryId)
  
  // Lấy giá từ finalPrice (đã tính discount từ backend) hoặc price (nếu không có discount)
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
    id: productData.id,
    name: productData.name,
    description: productData.description,
    brand: productData.brand,
    code: productData.code,
    category: category?.name || 'Uncategorized',
    categoryId: productData.categoryId,
    status: productData.status,
    isActive: productData.isActive,
    price: displayVariant.finalPrice || displayVariant.price || minPrice || 0,
    image: displayVariant.images?.[0]?.url || displayVariant.imageUrl || '/placeholder-shoe.jpg',
    images: displayVariant.images || (displayVariant.imageUrl ? [{ url: displayVariant.imageUrl }] : []),
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
    minPrice: minPrice,
    maxPrice: maxPrice,
    hasMultiplePrices: new Set(variants.map(v => v.finalPrice || v.price)).size > 1,
    totalQuantity: variants.reduce((sum, v) => sum + (v.quantity || 0), 0),
    createDate: productData.createDate,
    isNew: productData.status === 'New',
    discountInfo: discountInfo
  }
}

// Related products
const relatedProducts = ref([])

// Function to load product data
async function loadProduct() {
  loading.value = true
  
  // Reset state
  product.value = null
  selectedSize.value = null
  selectedColor.value = null
  quantity.value = 1
  currentMainImage.value = ''
  relatedProducts.value = []
  
  try {
    const productId = parseInt(route.params.id)
    
    if (!productId || isNaN(productId)) {
      loading.value = false
      return
    }
    
    const [productData, productDetailsData, categoriesData, discountsData, allProductsData, allProductDetailsData] = await Promise.all([
      productApi.getById(productId).catch(() => null),
      productDetailApi.getByProductId(productId).catch(() => []),
      categoryApi.getAll().catch(() => []),
      discountApi.getAll().catch(() => []),
      productApi.getAll().catch(() => []),
      productDetailApi.getAll().catch(() => [])
    ])

    if (!productData) {
      loading.value = false
      return
    }

    const transformed = transformProductForDisplay(
      productData,
      productDetailsData,
      categoriesData,
      discountsData
    )

    if (!transformed) {
      loading.value = false
      return
    }

    product.value = enrichProductWithDiscount(transformed)
    
    // Set initial values
    if (product.value.variants && product.value.variants.length > 0) {
      selectedColor.value = availableColors.value[0] || null
      selectedSize.value = availableSizes.value[0] || null
      currentMainImage.value = productImages.value[0]?.url || product.value.image || '/placeholder-shoe.jpg'
    }

    // Fetch related products (same category, different product)
    const relatedProductsData = allProductsData
      .filter(p => p.categoryId === product.value.categoryId && p.id !== product.value.id && p.isActive)
      .slice(0, 4)
    
    relatedProducts.value = relatedProductsData
      .map(p => {
        const transformed = transformProductForDisplay(p, allProductDetailsData, categoriesData, discountsData)
        return transformed ? enrichProductWithDiscount(transformed) : null
      })
      .filter(p => p !== null)
  } catch (error) {
    console.error('Error loading product:', error)
    toast.message = 'Không thể tải thông tin sản phẩm'
    toast.type = 'error'
    toast.show = true
  } finally {
    loading.value = false
  }
}

// Watch for route params changes
watch(() => route.params.id, (newId, oldId) => {
  if (newId && newId !== oldId) {
    loadProduct()
  }
}, { immediate: false })

// Load product on mount
onMounted(() => {
  loadProduct()
})
</script>
