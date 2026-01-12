import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { productApi, categoryApi, colorApi, sizeApi, productDetailApi, imageApi, handleApiError } from '@/services/api'

export const useProductStore = defineStore('products', () => {
  // State
  const products = ref([])
  const categories = ref([])
  const colors = ref([])
  const sizes = ref([])
  const loading = ref(false)
  const error = ref(null)

  // Computed
  const productCount = computed(() => products.value.length)
  const activeProducts = computed(() => products.value.filter(p => p.isActive))

  // ==================== PRODUCTS ====================
  const fetchProducts = async () => {
    loading.value = true
    error.value = null
    try {
      products.value = await productApi.getAll()
    } catch (err) {
      error.value = handleApiError(err)
      console.error('Error fetching products:', err)
    } finally {
      loading.value = false
    }
  }

  const getProduct = async (id) => {
    try {
      return await productApi.getById(id)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const createProduct = async (productData) => {
    loading.value = true
    error.value = null
    try {
      const newProduct = await productApi.create(productData)
      products.value.push(newProduct)
      return newProduct
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateProduct = async (id, productData) => {
    loading.value = true
    error.value = null
    try {
      await productApi.update(id, productData)
      const index = products.value.findIndex(p => p.id === id)
      if (index !== -1) {
        products.value[index] = { ...products.value[index], ...productData }
      }
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const deleteProduct = async (id) => {
    loading.value = true
    error.value = null
    try {
      await productApi.delete(id)
      products.value = products.value.filter(p => p.id !== id)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  // ==================== CATEGORIES ====================
  const fetchCategories = async () => {
    try {
      categories.value = await categoryApi.getAll()
    } catch (err) {
      console.error('Error fetching categories:', err)
    }
  }

  const createCategory = async (categoryData) => {
    try {
      const newCategory = await categoryApi.create(categoryData)
      categories.value.push(newCategory)
      return newCategory
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const updateCategory = async (id, categoryData) => {
    try {
      await categoryApi.update(id, categoryData)
      const index = categories.value.findIndex(c => c.id === id)
      if (index !== -1) {
        categories.value[index] = { ...categories.value[index], ...categoryData }
      }
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const deleteCategory = async (id) => {
    try {
      await categoryApi.delete(id)
      categories.value = categories.value.filter(c => c.id !== id)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  // ==================== COLORS ====================
  const fetchColors = async () => {
    try {
      colors.value = await colorApi.getAll()
    } catch (err) {
      console.error('Error fetching colors:', err)
    }
  }

  const createColor = async (colorData) => {
    try {
      const newColor = await colorApi.create(colorData)
      colors.value.push(newColor)
      return newColor
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const updateColor = async (id, colorData) => {
    try {
      await colorApi.update(id, colorData)
      const index = colors.value.findIndex(c => c.id === id)
      if (index !== -1) {
        colors.value[index] = { ...colors.value[index], ...colorData }
      }
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const deleteColor = async (id) => {
    try {
      await colorApi.delete(id)
      colors.value = colors.value.filter(c => c.id !== id)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  // ==================== SIZES ====================
  const fetchSizes = async () => {
    try {
      sizes.value = await sizeApi.getAll()
    } catch (err) {
      console.error('Error fetching sizes:', err)
    }
  }

  const createSize = async (sizeData) => {
    try {
      const newSize = await sizeApi.create(sizeData)
      sizes.value.push(newSize)
      return newSize
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const updateSize = async (id, sizeData) => {
    try {
      await sizeApi.update(id, sizeData)
      const index = sizes.value.findIndex(s => s.id === id)
      if (index !== -1) {
        sizes.value[index] = { ...sizes.value[index], ...sizeData }
      }
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const deleteSize = async (id) => {
    try {
      await sizeApi.delete(id)
      sizes.value = sizes.value.filter(s => s.id !== id)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  // ==================== IMAGES ====================
  const createImage = async (imageData) => {
    try {
      return await imageApi.create(imageData)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  // ==================== PRODUCT DETAILS ====================
  const getProductDetails = async (productId) => {
    try {
      return await productDetailApi.getByProductId(productId)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const createProductDetail = async (detailData) => {
    try {
      return await productDetailApi.create(detailData)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const updateProductDetail = async (id, detailData) => {
    try {
      return await productDetailApi.update(id, detailData)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  const deleteProductDetail = async (id) => {
    try {
      await productDetailApi.delete(id)
    } catch (err) {
      error.value = handleApiError(err)
      throw err
    }
  }

  // Initialize
  const initialize = async () => {
    await Promise.all([
      fetchProducts(),
      fetchCategories(),
      fetchColors(),
      fetchSizes()
    ])
  }

  return {
    // State
    products,
    categories,
    colors,
    sizes,
    loading,
    error,

    // Computed
    productCount,
    activeProducts,

    // Products
    fetchProducts,
    getProduct,
    createProduct,
    updateProduct,
    deleteProduct,

    // Categories
    fetchCategories,
    createCategory,
    updateCategory,
    deleteCategory,

    // Colors
    fetchColors,
    createColor,
    updateColor,
    deleteColor,

    // Sizes
    fetchSizes,
    createSize,
    updateSize,
    deleteSize,

    // Images
    createImage,

    // Product Details
    getProductDetails,
    createProductDetail,
    updateProductDetail,
    deleteProductDetail,

    // Initialize
    initialize
  }
})
