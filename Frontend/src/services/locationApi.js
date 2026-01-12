import axiosInstance from '@/utils/axiosInstance'

export const locationApi = {
  // Lấy danh sách tỉnh/thành phố
  getProvinces: async () => {
    try {
      const response = await axiosInstance.get('/Location/provinces')
      // Handle different response formats
      const data = response.data
      if (Array.isArray(data)) {
        return data
      } else if (data?.data && Array.isArray(data.data)) {
        return data.data
      } else if (data?.provinces && Array.isArray(data.provinces)) {
        return data.provinces
      } else if (data?.results && Array.isArray(data.results)) {
        return data.results
      }
      return []
    } catch (error) {
      console.error('Error fetching provinces:', error)
      // Return empty array on error instead of throwing
      return []
    }
  },

  // Lấy danh sách quận/huyện theo tỉnh/thành phố
  getDistricts: async (provinceCode) => {
    try {
      const response = await axiosInstance.get(`/Location/provinces/${provinceCode}/districts`)
      // Handle different response formats
      const data = response.data
      if (Array.isArray(data)) {
        return data
      } else if (data?.data && Array.isArray(data.data)) {
        return data.data
      } else if (data?.districts && Array.isArray(data.districts)) {
        return data.districts
      } else if (data?.results && Array.isArray(data.results)) {
        return data.results
      }
      return []
    } catch (error) {
      console.error('Error fetching districts:', error)
      // Return empty array on error instead of throwing
      return []
    }
  },

  // Lấy danh sách phường/xã theo quận/huyện
  getWards: async (districtCode) => {
    try {
      const response = await axiosInstance.get(`/Location/districts/${districtCode}/wards`)
      // Handle different response formats
      const data = response.data
      if (Array.isArray(data)) {
        return data
      } else if (data?.data && Array.isArray(data.data)) {
        return data.data
      } else if (data?.wards && Array.isArray(data.wards)) {
        return data.wards
      } else if (data?.results && Array.isArray(data.results)) {
        return data.results
      }
      return []
    } catch (error) {
      console.error('Error fetching wards:', error)
      // Return empty array on error instead of throwing
      return []
    }
  }
}

