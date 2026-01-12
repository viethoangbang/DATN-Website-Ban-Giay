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

export const ghnLocationApi = {
  // Get provinces from GHN
  getProvinces: async () => {
    try {
      const api = createAuthAxios()
      const response = await api.get('/GHNLocation/provinces')
      return response.data
    } catch (error) {
      console.error('Error fetching GHN provinces:', error)
      throw error
    }
  },

  // Get districts by province ID
  getDistricts: async (provinceId) => {
    try {
      const api = createAuthAxios()
      const response = await api.get(`/GHNLocation/districts/${provinceId}`)
      return response.data
    } catch (error) {
      console.error('Error fetching GHN districts:', error)
      throw error
    }
  },

  // Get wards by district ID
  getWards: async (districtId) => {
    try {
      const api = createAuthAxios()
      const response = await api.get(`/GHNLocation/wards/${districtId}`)
      return response.data
    } catch (error) {
      console.error('Error fetching GHN wards:', error)
      throw error
    }
  }
}

export default ghnLocationApi

