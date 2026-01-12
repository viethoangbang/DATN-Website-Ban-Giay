import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { jwtDecode } from 'jwt-decode'
import axiosInstance from '@/utils/axiosInstance'

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token') || null)
  const user = ref(JSON.parse(localStorage.getItem('user') || 'null'))

  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.roles?.includes('Admin') || false)
  const isEmployee = computed(() => user.value?.roles?.includes('Manager') || user.value?.roles?.includes('Staff') || false)
  const isCustomer = computed(() => user.value?.roles?.includes('Customer') || false)
  const isAdminOrEmployee = computed(() => isAdmin.value || isEmployee.value)

  // Decode JWT token
  const decodeToken = (tokenString) => {
    try {
      return jwtDecode(tokenString)
    } catch (error) {
      console.error('Error decoding token:', error)
      return null
    }
  }

  // Check if token is expired
  const isTokenExpired = () => {
    if (!token.value) return true

    const decoded = decodeToken(token.value)
    if (!decoded || !decoded.exp) return true

    const currentTime = Date.now() / 1000
    return decoded.exp < currentTime
  }

  // Set auth data
  const setAuth = (tokenString, userData) => {
    token.value = tokenString
    user.value = userData

    localStorage.setItem('token', tokenString)
    localStorage.setItem('user', JSON.stringify(userData))
  }

  // Clear auth data
  const clearAuth = () => {
    token.value = null
    user.value = null

    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  // Login
  const login = async (credentials) => {
    try {
      const response = await axiosInstance.post('/Auth/login', {
        email: credentials.email,
        password: credentials.password
      })

      // Backend returns: { success, message, token, user: { id, email, fullName, phoneNumber, roles } }
      const { success, token: tokenString, user: userData } = response.data

      if (!success || !tokenString || !userData) {
        return {
          success: false,
          message: response.data.message || 'Đăng nhập thất bại'
        }
      }

      setAuth(tokenString, userData)
      return { success: true, data: { token: tokenString, user: userData } }
    } catch (error) {
      console.error('Login error:', error)
      return {
        success: false,
        message: error.response?.data?.message || 'Đã xảy ra lỗi khi đăng nhập'
      }
    }
  }

  // Register
  const register = async (userData) => {
    try {
      const response = await axiosInstance.post('/Auth/register', {
        email: userData.email,
        password: userData.password,
        confirmPassword: userData.confirmPassword
      })

      // Backend now returns: { success, message, token, user }
      if (response.data.success) {
        // Save token and user info using setAuth function
        const { token: authToken, user: authUser } = response.data
        setAuth(authToken, authUser)
        return { success: true, message: response.data.message }
      }

      return { success: false, message: response.data.message }
    } catch (error) {
      console.error('Register error:', error)
      console.error('Error response:', error.response?.data)

      // Handle validation errors
      if (error.response?.data?.errors) {
        const errors = error.response.data.errors
        const errorMessages = Object.values(errors).flat().join(', ')
        return {
          success: false,
          message: errorMessages || 'Dữ liệu không hợp lệ',
          errors: errors
        }
      }

      // Handle single error message
      const errorMsg = error.response?.data?.message || 'Đã xảy ra lỗi khi đăng ký'
      return {
        success: false,
        message: errorMsg
      }
    }
  }

  // Logout
  const logout = () => {
    clearAuth()
  }

  // Get current user from token
  const getCurrentUser = () => {
    if (!token.value || isTokenExpired()) {
      clearAuth()
      return null
    }

    return user.value
  }

  // Initialize auth state
  const initAuth = () => {
    if (token.value && !isTokenExpired()) {
      // Token is valid, user data already in localStorage
      return true
    } else {
      clearAuth()
      return false
    }
  }

  return {
    token,
    user,
    isAuthenticated,
    isAdmin,
    isEmployee,
    isCustomer,
    isAdminOrEmployee,
    login,
    register,
    logout,
    getCurrentUser,
    initAuth,
    isTokenExpired,
    decodeToken
  }
})
