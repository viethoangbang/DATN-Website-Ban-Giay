import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useCartStore = defineStore('cart', () => {
  const items = ref([])
  
  const totalItems = computed(() => {
    return items.value.reduce((total, item) => total + item.quantity, 0)
  })
  
  const totalPrice = computed(() => {
    return items.value.reduce((total, item) => {
      return total + (item.price * item.quantity)
    }, 0)
  })
  
  function addToCart(product, size, color = '') {
    const existingItem = items.value.find(
      item => item.id === product.id && item.size === size && item.color === color
    )
    
    if (existingItem) {
      existingItem.quantity++
    } else {
      items.value.push({
        id: product.id,
        name: product.name,
        price: product.salePrice || product.price,
        image: product.image,
        size: size,
        color: color,
        quantity: 1
      })
    }
    
    saveToLocalStorage()
  }
  
  function removeFromCart(productId, size, color = '') {
    items.value = items.value.filter(
      item => !(item.id === productId && item.size === size && item.color === color)
    )
    saveToLocalStorage()
  }
  
  function updateQuantity(productId, size, quantity, color = '') {
    const item = items.value.find(
      item => item.id === productId && item.size === size && item.color === color
    )
    
    if (item) {
      if (quantity <= 0) {
        removeFromCart(productId, size, color)
      } else {
        item.quantity = quantity
        saveToLocalStorage()
      }
    }
  }
  
  function clearCart() {
    items.value = []
    saveToLocalStorage()
  }
  
  function saveToLocalStorage() {
    localStorage.setItem('cart', JSON.stringify(items.value))
  }
  
  function loadFromLocalStorage() {
    const savedCart = localStorage.getItem('cart')
    if (savedCart) {
      items.value = JSON.parse(savedCart)
    }
  }
  
  return {
    items,
    totalItems,
    totalPrice,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    loadFromLocalStorage
  }
})

