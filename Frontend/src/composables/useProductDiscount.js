import { ref } from 'vue'
import { discountApi } from '@/services/api'

/**
 * Composable for managing product discounts
 * Provides centralized discount calculation and caching
 */
export function useProductDiscount() {
  const discounts = ref([])
  const loading = ref(false)

  /**
   * Fetch all discounts from API
   */
  const fetchDiscounts = async () => {
    if (discounts.value.length > 0) return // Already loaded
    
    loading.value = true
    try {
      discounts.value = await discountApi.getAll()
    } catch (error) {
      console.error('Error fetching discounts:', error)
      discounts.value = []
    } finally {
      loading.value = false
    }
  }

  /**
   * Get active discount for a product
   */
  const getProductDiscount = (productId) => {
    if (!productId || !discounts.value || discounts.value.length === 0) return null
    
    const now = new Date()
    return discounts.value.find(d => 
      d.productId === productId &&
      d.status === 'Active' &&
      new Date(d.startDate) <= now &&
      new Date(d.endDate) >= now
    ) || null
  }

  /**
   * Calculate discounted price for a product
   */
  const calculateDiscountedPrice = (productPrice, discountInfo) => {
    if (!discountInfo || !productPrice) return productPrice
    
    if (discountInfo.discountType === 'Percentage') {
      const discountAmount = productPrice * (discountInfo.discountValue / 100)
      return Math.max(0, productPrice - discountAmount)
    } else if (discountInfo.discountType === 'Fixed') {
      return Math.max(0, productPrice - discountInfo.discountValue)
    }
    
    return productPrice
  }

  /**
   * Get discount percentage for display
   */
  const getDiscountPercentage = (productPrice, discountInfo) => {
    if (!discountInfo || !productPrice) return null
    
    if (discountInfo.discountType === 'Percentage') {
      return discountInfo.discountValue
    } else if (discountInfo.discountType === 'Fixed') {
      return Math.round((discountInfo.discountValue / productPrice) * 100)
    }
    
    return null
  }

  /**
   * Add discount info to product object
   */
  const enrichProductWithDiscount = (product) => {
    if (!product || !product.id) return product
    
    const discountInfo = getProductDiscount(product.id)
    const discountedPrice = discountInfo 
      ? calculateDiscountedPrice(product.price || product.minPrice || 0, discountInfo)
      : (product.price || product.minPrice || 0)
    
    return {
      ...product,
      discountInfo,
      discountedPrice,
      discountPercent: getDiscountPercentage(product.price || product.minPrice || 0, discountInfo)
    }
  }

  return {
    discounts,
    loading,
    fetchDiscounts,
    getProductDiscount,
    calculateDiscountedPrice,
    getDiscountPercentage,
    enrichProductWithDiscount
  }
}

