import axiosInstance from '@/utils/axiosInstance'

// ==================== PRODUCTS ====================
export const productApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Product')
        return response.data
    },

    getNewProducts: async (limit = 10) => {
        const response = await axiosInstance.get(`/Product/new?limit=${limit}`)
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Product/${id}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Product', data)
        return response.data
    },

    update: async (id, data) => {
        const response = await axiosInstance.put(`/Product/${id}`, data)
        return response.data
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Product/${id}`)
    }
}

// ==================== CATEGORIES ====================
export const categoryApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Category')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Category/${id}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Category', data)
        return response.data
    },

    update: async (id, data) => {
        const response = await axiosInstance.put(`/Category/${id}`, data)
        return response.data
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Category/${id}`)
    }
}

// ==================== COLORS ====================
export const colorApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Color')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Color/${id}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Color', data)
        return response.data
    },

    update: async (id, data) => {
        const response = await axiosInstance.put(`/Color/${id}`, data)
        return response.data
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Color/${id}`)
    }
}

// ==================== SIZES ====================
export const sizeApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Size')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Size/${id}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Size', data)
        return response.data
    },

    update: async (id, data) => {
        const response = await axiosInstance.put(`/Size/${id}`, data)
        return response.data
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Size/${id}`)
    }
}

// ==================== PRODUCT DETAILS ====================
export const productDetailApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/ProductDetail')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/ProductDetail/${id}`)
        return response.data
    },

    getByProductId: async (productId) => {
        const response = await axiosInstance.get(`/ProductDetail/product/${productId}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/ProductDetail', data)
        return response.data
    },

    update: async (id, data) => {
        const response = await axiosInstance.put(`/ProductDetail/${id}`, data)
        return response.data
    },

    delete: async (id) => {
        await axiosInstance.delete(`/ProductDetail/${id}`)
    }
}

// ==================== IMAGES ====================
export const imageApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Image')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Image/${id}`)
        return response.data
    },

    getByType: async (type) => {
        const response = await axiosInstance.get(`/Image/type/${type}`)
        return response.data
    },

    getHeroImages: async () => {
        const response = await axiosInstance.get('/Image/hero')
        return response.data
    },

    getTypes: async () => {
        const response = await axiosInstance.get('/Image/types')
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Image', data)
        return response.data
    },

    update: async (id, data) => {
        const response = await axiosInstance.put(`/Image/${id}`, data)
        return response.data
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Image/${id}`)
    },

    upload: async (file, type = null, folder = null) => {
        const formData = new FormData()
        formData.append('file', file)
        
        // Build query parameters
        const params = new URLSearchParams()
        if (type && type.trim() !== '') {
            params.append('type', type.trim())
        }
        if (folder && folder.trim() !== '') {
            params.append('folder', folder.trim())
        }
        
        const queryString = params.toString()
        const url = queryString ? `/Image/upload?${queryString}` : '/Image/upload'
        
        const response = await axiosInstance.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        return response.data
    },

    uploadMultiple: async (files, type = null, folder = null, maxFiles = null) => {
        const formData = new FormData()
        files.forEach(file => {
            formData.append('files', file)
        })
        
        // Build query parameters
        const params = new URLSearchParams()
        if (type && type.trim() !== '') {
            params.append('type', type.trim())
        }
        if (folder && folder.trim() !== '') {
            params.append('folder', folder.trim())
        }
        if (maxFiles !== null && maxFiles !== undefined) {
            params.append('maxFiles', maxFiles.toString())
        }
        
        const queryString = params.toString()
        const url = queryString ? `/Image/upload-multiple?${queryString}` : '/Image/upload-multiple'
        
        const response = await axiosInstance.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        return response.data
    }
}

// ==================== DASHBOARD ====================
export const dashboardApi = {
    getStats: async () => {
        const response = await axiosInstance.get('/Dashboard/stats')
        return response.data
    },
    getReports: async (period = 'month') => {
        const response = await axiosInstance.get(`/Dashboard/reports?period=${period}`)
        return response.data
    },
    exportReports: async (period = 'month', format = 'csv') => {
        const response = await axiosInstance.get(`/Dashboard/reports/export?period=${period}&format=${format}`, {
            responseType: 'blob'
        })
        return response.data
    }
}

// ==================== ACCOUNTS ====================
export const accountApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Account')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Account/${id}`)
        return response.data
    },

    getMe: async () => {
        const response = await axiosInstance.get('/Account/me')
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Account', data)
        return response.data
    },

    update: async (id, data) => {
        await axiosInstance.put(`/Account/${id}`, data)
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Account/${id}`)
    },

    changePassword: async (id, data) => {
        await axiosInstance.put(`/Account/${id}/change-password`, data)
    }
}

// ==================== ADDRESSES ====================
export const addressApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Address')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Address/${id}`)
        return response.data
    },

    getByAccountId: async (accountId) => {
        const response = await axiosInstance.get(`/Address/my-addresses`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Address', data)
        return response.data
    },

    update: async (id, data) => {
        await axiosInstance.put(`/Address/${id}`, data)
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Address/${id}`)
    }
}

// ==================== ORDERS ====================
export const orderApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Order')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Order/${id}`)
        return response.data
    },

    getMyOrders: async () => {
        const response = await axiosInstance.get('/Order/my-orders')
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Order', data)
        return response.data
    },

    updateStatus: async (id, data) => {
        await axiosInstance.put(`/Order/${id}/status`, data)
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Order/${id}`)
    }
}

// ==================== VOUCHERS ====================
export const voucherApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Voucher')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Voucher/${id}`)
        return response.data
    },

    getByCode: async (code) => {
        const response = await axiosInstance.get(`/Voucher/code/${code}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Voucher', data)
        return response.data
    },

    update: async (id, data) => {
        await axiosInstance.put(`/Voucher/${id}`, data)
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Voucher/${id}`)
    }
}

// ==================== DISCOUNTS ====================
export const discountApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/Discount')
        return response.data
    },

    getByProductId: async (productId) => {
        const response = await axiosInstance.get(`/Discount/product/${productId}`)
        return response.data
    },

    getActiveByProductId: async (productId) => {
        const response = await axiosInstance.get(`/Discount/product/${productId}/active`)
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/Discount/${id}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/Discount', data)
        return response.data
    },

    update: async (id, data) => {
        await axiosInstance.put(`/Discount/${id}`, data)
    },

    delete: async (id) => {
        await axiosInstance.delete(`/Discount/${id}`)
    }
}

// ==================== CART ====================
export const cartApi = {
    getMyCart: async () => {
        const response = await axiosInstance.get('/Cart/my-cart')
        return response.data
    },

    addToCart: async (productDetailId, quantity) => {
        const response = await axiosInstance.post('/Cart/add', {
            productDetailId,
            quantity
        })
        return response.data
    },

    updateCartItem: async (cartDetailId, quantity) => {
        await axiosInstance.put(`/Cart/item/${cartDetailId}`, { quantity })
    },

    removeFromCart: async (cartDetailId) => {
        await axiosInstance.delete(`/Cart/item/${cartDetailId}`)
    },

    clearCart: async () => {
        await axiosInstance.delete('/Cart/clear')
    }
}

// ==================== BRAND BANNERS ====================
export const brandBannerApi = {
    getAll: async () => {
        const response = await axiosInstance.get('/BrandBanner')
        return response.data
    },

    getByBrand: async (brand) => {
        const response = await axiosInstance.get(`/BrandBanner/brand/${encodeURIComponent(brand)}`)
        return response.data
    },

    getActiveByBrand: async (brand) => {
        const response = await axiosInstance.get(`/BrandBanner/brand/${encodeURIComponent(brand)}/active`)
        return response.data
    },

    getBrandSections: async () => {
        const response = await axiosInstance.get('/BrandBanner/sections')
        return response.data
    },

    getById: async (id) => {
        const response = await axiosInstance.get(`/BrandBanner/${id}`)
        return response.data
    },

    create: async (data) => {
        const response = await axiosInstance.post('/BrandBanner', data)
        return response.data
    },

    update: async (id, data) => {
        await axiosInstance.put(`/BrandBanner/${id}`, data)
    },

    delete: async (id) => {
        await axiosInstance.delete(`/BrandBanner/${id}`)
    },

    getDistinctBrands: async () => {
        const response = await axiosInstance.get('/BrandBanner/brands')
        return response.data
    }
}

// ==================== HELPER FUNCTIONS ====================
export const handleApiError = (error) => {
    if (error.response) {
        // Backend returned error
        const { data } = error.response

        // Validation errors
        if (data.errors) {
            const errorMessages = Object.values(data.errors).flat().join(', ')
            return errorMessages
        }

        // Single error message
        if (data.message) {
            return data.message
        }

        // Default error
        return 'Đã xảy ra lỗi. Vui lòng thử lại.'
    }

    // Network error
    if (error.request) {
        return 'Không thể kết nối đến server. Vui lòng kiểm tra kết nối.'
    }

    return error.message || 'Đã xảy ra lỗi không xác định.'
}
