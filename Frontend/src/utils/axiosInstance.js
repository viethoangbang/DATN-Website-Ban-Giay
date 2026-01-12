import axios from 'axios'
import { useAuthStore } from '@/stores/auth'
import router from '@/router'

const axiosInstance = axios.create({
  baseURL: 'http://localhost:5166/api',
  timeout: 10000,
  withCredentials: true, // Important for cookies
  headers: {
    'Content-Type': 'application/json',
  }
})

// Request interceptor
axiosInstance.interceptors.request.use(
  (config) => {
    const authStore = useAuthStore()

    // Add token to header if exists
    if (authStore.token) {
      config.headers.Authorization = `Bearer ${authStore.token}`
    }

    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor
axiosInstance.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    if (error.response) {
      // Handle 401 Unauthorized
      if (error.response.status === 401) {
        const authStore = useAuthStore()
        authStore.logout()
        router.push('/login')
      }

      // Handle 403 Forbidden
      if (error.response.status === 403) {
        router.push('/403')
      }
    }

    return Promise.reject(error)
  }
)

export default axiosInstance

