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

export const orderApi = {
  // Get my orders
  getMyOrders: async () => {
    try {
      const api = createAuthAxios()
      const response = await api.get('/Order/my-orders')
      return response.data
    } catch (error) {
      console.error('Error fetching my orders:', error)
      throw error
    }
  },

  // Get order by ID
  getOrderById: async (id) => {
    try {
      const api = createAuthAxios()
      const response = await api.get(`/Order/${id}`)
      return response.data
    } catch (error) {
      console.error('Error fetching order:', error)
      throw error
    }
  },

  // Cancel order (customer endpoint)
  cancelOrder: async (id) => {
    try {
      const api = createAuthAxios()
      const response = await api.put(`/Order/${id}/cancel`)
      return response.data
    } catch (error) {
      console.error('Error cancelling order:', error)
      throw error
    }
  }
}

export default orderApi

