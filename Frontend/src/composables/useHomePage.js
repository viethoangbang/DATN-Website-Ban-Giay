import { ref, computed } from 'vue'
import { productApi, categoryApi, productDetailApi, voucherApi, brandBannerApi, discountApi, handleApiError } from '@/services/api'

/**
 * Composable for Home Page data management
 * Senior approach: Separation of concerns, error handling, caching, performance optimization
 */
export function useHomePage() {
  // State
  const products = ref([])
  const categories = ref([])
  const productDetails = ref([])
  const vouchers = ref([])
  const discounts = ref([])
  const brandSections = ref([])
  const newProducts = ref([])
  const loading = ref(false)
  const error = ref(null)

  // Cache for transformed products (combine Product + ProductDetail)
  const transformedProducts = ref([])

  // Helper function to get active discount for a product
  const getProductDiscountInfo = (productId) => {
    if (!discounts.value || discounts.value.length === 0) return null
    const now = new Date()
    return discounts.value.find(d => 
      d.productId === productId &&
      d.status === 'Active' &&
      new Date(d.startDate) <= now &&
      new Date(d.endDate) >= now
    ) || null
  }

  /**
   * Transform Product + ProductDetails into display format
   * Each product can have multiple variants (colors/sizes)
   * We'll use the first available variant for display
   */
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
      // Discount info - lấy từ variant đầu tiên có discount
      discountInfo: discountInfo
      // No rating - removed because there's no review system
    }
  }

  /**
   * Fetch all data needed for home page
   */
  const fetchHomePageData = async () => {
    loading.value = true
    error.value = null
    
    try {
      // Fetch in parallel for better performance
      const [productsData, categoriesData, productDetailsData, vouchersData, discountsData, newProductsData, brandSectionsData] = await Promise.all([
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
        }),
        voucherApi.getAll().catch(err => {
          console.error('Error fetching vouchers:', err)
          return []
        }),
        discountApi.getAll().catch(err => {
          console.error('Error fetching discounts:', err)
          return []
        }),
        productApi.getNewProducts(10).catch(err => {
          console.error('Error fetching new products:', err)
          return []
        }),
        brandBannerApi.getBrandSections().catch(err => {
          console.error('Error fetching brand sections:', err)
          return []
        })
      ])

      products.value = productsData.filter(p => p.isActive)
      categories.value = categoriesData.filter(c => c.status === 'Active')
      productDetails.value = productDetailsData
      discounts.value = discountsData
      vouchers.value = vouchersData
      discounts.value = discountsData
      newProducts.value = newProductsData
      brandSections.value = brandSectionsData

      // Transform products for display
      transformedProducts.value = products.value
        .map(product => transformProductForDisplay(product, productDetails.value, categories.value))
        .filter(p => p !== null) // Remove products without variants
        .sort((a, b) => {
          // Sort by createDate if available, otherwise by id
          if (a.createDate && b.createDate) {
            return new Date(b.createDate) - new Date(a.createDate)
          }
          return b.id - a.id
        }) // Newest first

      // Transform new products for display
      // newProductsData is from API, but we need to transform it with ProductDetails
      // So we'll get the transformed products that match the new products IDs
      const newProductIds = new Set(newProductsData.map(p => p.id))
      newProducts.value = transformedProducts.value
        .filter(p => newProductIds.has(p.id))
        .slice(0, 10) // Limit to 10 products

    } catch (err) {
      error.value = handleApiError(err)
      console.error('Error in fetchHomePageData:', err)
    } finally {
      loading.value = false
    }
  }

  /**
   * Get products by category
   */
  const getProductsByCategory = (categoryName) => {
    if (!categoryName || categoryName === 'All') {
      return transformedProducts.value
    }
    return transformedProducts.value.filter(p => p.category === categoryName)
  }

  /**
   * Get hot deals - products with active vouchers or best sellers
   * Only show products that actually have vouchers applied
   */
  const getHotDeals = (limit = 3) => {
    const now = new Date()
    
    // Get active vouchers
    const activeVouchers = vouchers.value.filter(v => {
      if (v.status !== 'Active') return false
      if (!v.startDate || !v.endDate) return false
      if (v.quantity <= 0) return false
      
      const start = new Date(v.startDate)
      const end = new Date(v.endDate)
      return now >= start && now <= end
    })

    // If no active vouchers, return empty array (don't show fake deals)
    if (activeVouchers.length === 0) {
      return []
    }

    // Get products that have vouchers (by category or all products)
    const productsWithVouchers = transformedProducts.value
      .filter(product => {
        // Check if product category matches any voucher category
        // If voucher has no categoryId, it applies to all products
        return activeVouchers.some(v => 
          !v.categoryId || v.categoryId === product.categoryId
        )
      })
      .slice(0, limit)

    // Map products with their applicable vouchers
    return productsWithVouchers.map(product => {
      // Find best voucher for this product
      const applicableVoucher = activeVouchers.find(v => 
        !v.categoryId || v.categoryId === product.categoryId
      ) || activeVouchers[0]

      // Calculate discount
      let discountPercent = 0
      let salePrice = product.minPrice

      if (applicableVoucher) {
        if (applicableVoucher.type === 'Percentage') {
          discountPercent = applicableVoucher.discount || 0
          salePrice = product.minPrice * (1 - discountPercent / 100)
          // Apply max discount if exists
          if (applicableVoucher.maxDiscount && salePrice < product.minPrice - applicableVoucher.maxDiscount) {
            salePrice = product.minPrice - applicableVoucher.maxDiscount
            discountPercent = (applicableVoucher.maxDiscount / product.minPrice) * 100
          }
        } else if (applicableVoucher.type === 'FixedAmount') {
          salePrice = Math.max(0, product.minPrice - (applicableVoucher.discount || 0))
          discountPercent = ((product.minPrice - salePrice) / product.minPrice) * 100
        }
      }

      return {
        ...product,
        voucher: applicableVoucher,
        discount: Math.round(discountPercent),
        salePrice: Math.round(salePrice),
        hasDiscount: discountPercent > 0
      }
    })
  }

  /**
   * Get trending products (most variants = popular)
   */
  const getTrendingProducts = (limit = 8) => {
    return [...transformedProducts.value]
      .sort((a, b) => b.variants.length - a.variants.length)
      .slice(0, limit)
  }

  /**
   * Get new arrivals (recently created products)
   */
  const getNewArrivals = (limit = 4) => {
    return [...transformedProducts.value]
      .sort((a, b) => new Date(b.createDate) - new Date(a.createDate))
      .slice(0, limit)
  }

  /**
   * Get active categories for display
   */
  const getActiveCategories = computed(() => {
    return categories.value.filter(c => c.status === 'Active')
  })

  // Force recompute trigger for countdown
  const countdownTrigger = ref(0)

  /**
   * Get all active vouchers (for display in voucher section)
   */
  const getActiveVouchers = computed(() => {
    // Use countdownTrigger to force recompute
    const _ = countdownTrigger.value
    
    const now = new Date()
    
    return vouchers.value
      .filter(v => {
        if (v.status !== 'Active') return false
        if (!v.startDate || !v.endDate) return false
        if (v.quantity <= 0) return false
        
        const start = new Date(v.startDate)
        const end = new Date(v.endDate)
        return now >= start && now <= end
      })
      .map(v => {
        const endTime = new Date(v.endDate)
        const timeRemaining = endTime - now
        
        if (timeRemaining <= 0) return null

        const hours = Math.floor(timeRemaining / (1000 * 60 * 60))
        const minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60))
        const seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000)

        // Get category name if applicable
        const categoryName = v.categoryId 
          ? categories.value.find(c => c.id === v.categoryId)?.name || null
          : null

        return {
          ...v,
          timeRemaining: {
            hours: hours.toString().padStart(2, '0'),
            minutes: minutes.toString().padStart(2, '0'),
            seconds: seconds.toString().padStart(2, '0')
          },
          categoryName: categoryName
        }
      })
      .filter(v => v !== null)
      .sort((a, b) => {
        // Sort by: no category first (applies to all), then by discount amount
        if (!a.categoryId && b.categoryId) return -1
        if (a.categoryId && !b.categoryId) return 1
        // Compare discount value
        const discountA = a.type === 'Percentage' ? (a.discount || 0) : (a.discount || 0) / 1000
        const discountB = b.type === 'Percentage' ? (b.discount || 0) : (b.discount || 0) / 1000
        return discountB - discountA
      })
  })

  // Function to trigger countdown update
  const updateCountdown = () => {
    countdownTrigger.value++
  }

  return {
    // State
    products,
    categories,
    productDetails,
    vouchers,
    discounts,
    brandSections,
    newProducts,
    transformedProducts,
    loading,
    error,
    
    // Methods
    fetchHomePageData,
    getProductsByCategory,
    getHotDeals,
    getTrendingProducts,
    getNewArrivals,
    getActiveCategories,
    getActiveVouchers,
    updateCountdown
  }
}

