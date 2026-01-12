import axios from 'axios'

const API_URL = 'http://localhost:5166/api'

// Get auth token from localStorage
const getAuthToken = () => {
  return localStorage.getItem('token')
}

// Create axios instance with auth
const createAuthAxios = () => {
  const token = getAuthToken()
  return axios.create({
    baseURL: API_URL,
    headers: {
      'Authorization': token ? `Bearer ${token}` : ''
    }
  })
}

export const checkoutApi = {
  // Calculate shipping fee
  calculateShippingFee: async (addressDeliveryId) => {
    try {
      const api = createAuthAxios()
      const response = await api.post('/Checkout/calculate-shipping-fee', {
        addressDeliveryId
      })
      return response.data
    } catch (error) {
      console.error('Error calculating shipping fee:', error)
      throw error
    }
  },

  // Calculate voucher discount
  calculateVoucherDiscount: async (voucherCode, subtotal) => {
    try {
      const api = createAuthAxios()
      const response = await api.post('/Checkout/calculate-voucher-discount', {
        voucherCode,
        subtotal
      })
      return response.data
    } catch (error) {
      console.error('Error calculating voucher discount:', error)
      throw error
    }
  },

  // Create order
  createOrder: async (orderData) => {
    try {
      const api = createAuthAxios()
      const response = await api.post('/Checkout/create-order', orderData)
      return response.data
    } catch (error) {
      console.error('Error creating order:', error)
      throw error
    }
  }
}

export default checkoutApi

