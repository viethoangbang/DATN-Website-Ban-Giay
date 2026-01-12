import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { cartApi, productDetailApi } from '@/services/api'
import { useAuthStore } from './auth'

export const useCartStore = defineStore('cart', () => {
  const items = ref([])
  const loading = ref(false)
  const authStore = useAuthStore()
  
  const totalItems = computed(() => {
    // Count distinct items (number of different products/variants), not total quantity
    return items.value.length
  })
  
  const totalPrice = computed(() => {
    return items.value.reduce((total, item) => {
      const price = Number(item.price) || 0
      const quantity = Number(item.quantity) || 0
      return total + (price * quantity)
    }, 0)
  })
  
  /**
   * Fetch cart from backend
   */
  async function fetchCart() {
    if (!authStore.isAuthenticated) {
      items.value = []
      return
    }
    
    loading.value = true
    try {
      const backendCart = await cartApi.getMyCart()
      
      if (backendCart && backendCart.items && backendCart.items.length > 0) {
        // Load product details to get additional info (images, stock, etc.)
        const productDetailIds = backendCart.items.map(item => item.productDetailId).filter(Boolean)
        const productDetailsMap = new Map()
        
        // Fetch product details in parallel
        if (productDetailIds.length > 0) {
          const productDetailsPromises = productDetailIds.map(id => 
            productDetailApi.getById(id).catch(() => null)
          )
          const productDetailsList = await Promise.all(productDetailsPromises)
          
          productDetailsList.forEach(pd => {
            if (pd && pd.id) {
              productDetailsMap.set(pd.id, pd)
            }
          })
        }
        
        // Transform backend items to frontend format
        // Create new array to ensure reactivity
        const transformedItems = backendCart.items.map(item => {
          const productDetail = productDetailsMap.get(item.productDetailId)
          
          // Get image from backend item or product detail
          let itemImage = item.imageUrl || ''
          if (!itemImage && productDetail) {
            itemImage = productDetail.images?.[0]?.url || productDetail.imageUrl || ''
          }
          if (!itemImage) {
            itemImage = '/placeholder-shoe.jpg'
          }
          
          // Backend đã tính discount và trả về finalPrice
          const originalPrice = item.price || 0 // Giá gốc từ backend
          const finalPrice = item.finalPrice || item.price || 0 // finalPrice từ backend (đã tính discount)
          
          // Lấy discount info từ backend response
          let discountInfo = null
          if (item.discountActive) {
            discountInfo = {
              discountType: item.discountType,
              discountValue: item.discountValue,
              discountActive: true
            }
          }
          
          return {
            id: productDetail?.productId || item.productDetailId, // Product ID
            name: item.productName || '',
            price: finalPrice, // finalPrice từ backend (đã tính discount)
            originalPrice: originalPrice, // Giá gốc từ backend
            image: itemImage,
            size: item.sizeName || '',
            color: item.colorName || '',
            quantity: Number(item.quantity) || 1,
            productDetailId: item.productDetailId,
            cartDetailId: item.id, // Backend cart detail ID (required for update/remove)
            stock: productDetail?.quantity || null,
            discountInfo: discountInfo
          }
        })
        
        // Replace items array completely to ensure reactivity
        items.value = transformedItems
      } else {
        items.value = []
      }
    } catch (error) {
      console.error('Error fetching cart:', error)
      items.value = []
      throw error
    } finally {
      loading.value = false
    }
  }
  
  /**
   * Add item to cart via backend API
   * @param {number} productDetailId - Product detail ID (required)
   * @param {number} quantity - Quantity to add (default: 1)
   */
  async function addToCart(productDetailId, quantity = 1) {
    if (!authStore.isAuthenticated) {
      throw new Error('Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng')
    }
    
    try {
      // Call backend API
      await cartApi.addToCart(productDetailId, quantity)
      // Refresh cart from backend
      await fetchCart()
    } catch (error) {
      console.error('Error adding to cart:', error)
      throw error
    }
  }
  
  /**
   * Update cart item quantity via backend API
   * @param {number} cartDetailId - Cart detail ID from backend (required)
   * @param {number} quantity - New quantity
   */
  async function updateQuantity(cartDetailId, quantity) {
    if (!authStore.isAuthenticated) {
      throw new Error('Vui lòng đăng nhập để cập nhật giỏ hàng')
    }
    
    if (quantity <= 0) {
      // Remove item instead
      await removeFromCart(cartDetailId)
      return
    }
    
    try {
      // Call backend API
      await cartApi.updateCartItem(cartDetailId, quantity)
      // Refresh cart from backend
      await fetchCart()
    } catch (error) {
      console.error('Error updating cart item:', error)
      throw error
    }
  }
  
  /**
   * Remove item from cart via backend API
   * @param {number} cartDetailId - Cart detail ID from backend (required)
   */
  async function removeFromCart(cartDetailId) {
    if (!authStore.isAuthenticated) {
      throw new Error('Vui lòng đăng nhập để xóa sản phẩm khỏi giỏ hàng')
    }
    
    try {
      // Call backend API
      await cartApi.removeFromCart(cartDetailId)
      // Refresh cart from backend
      await fetchCart()
    } catch (error) {
      console.error('Error removing from cart:', error)
      throw error
    }
  }
  
  /**
   * Clear entire cart via backend API
   */
  async function clearCart() {
    if (!authStore.isAuthenticated) {
      items.value = []
      return
    }
    
    try {
      // Call backend API
      await cartApi.clearCart()
      // Refresh cart from backend
      await fetchCart()
    } catch (error) {
      console.error('Error clearing cart:', error)
      throw error
    }
  }
  
  /**
   * Set items directly (for backward compatibility, but should use fetchCart instead)
   * @deprecated Use fetchCart() instead
   */
  function setItemsFromBackend(backendItems, productDetailsList = [], discounts = []) {
    items.value = backendItems.map(item => {
      const productDetail = productDetailsList.find(pd => pd.id === item.productDetailId)
      const productId = productDetail?.productId || null
      
      let itemImage = item.imageUrl || ''
      if (!itemImage && productDetail) {
        itemImage = productDetail.images?.[0]?.url || productDetail.imageUrl || ''
      }
      if (!itemImage) {
        itemImage = '/placeholder-shoe.jpg'
      }
      
      const originalPrice = item.price || 0
      const finalPrice = item.finalPrice || item.price || 0
      
      let discountInfo = null
      if (item.discountActive) {
        discountInfo = {
          discountType: item.discountType,
          discountValue: item.discountValue,
          discountActive: true
        }
      }
      
      return {
        id: productId || item.productDetailId,
        name: item.productName || '',
        price: finalPrice,
        originalPrice: originalPrice,
        image: itemImage,
        size: item.sizeName || '',
        color: item.colorName || '',
        quantity: item.quantity || 1,
        productDetailId: item.productDetailId,
        cartDetailId: item.id,
        categoryId: item.categoryId || null,
        stock: productDetail?.quantity || null,
        discountInfo: discountInfo
      }
    })
  }
  
  return {
    items,
    loading,
    totalItems,
    totalPrice,
    fetchCart,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    setItemsFromBackend // Keep for backward compatibility
  }
})
