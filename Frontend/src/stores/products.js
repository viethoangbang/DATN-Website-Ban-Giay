import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useProductStore = defineStore('products', () => {
  const products = ref([])
  const loading = ref(false)
  const categories = ref(['All', 'Running', 'Basketball', 'Casual', 'Sports'])
  
  // Mock data cho demo
  const mockProducts = [
    {
      id: 1,
      name: 'Nike Air Max 2024',
      price: 3500000,
      image: 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500',
      category: 'Running',
      description: 'Giày chạy bộ cao cấp với công nghệ đệm khí tiên tiến',
      sizes: [38, 39, 40, 41, 42, 43],
      colors: ['Black', 'White', 'Red'],
      rating: 4.8,
      reviews: 245
    },
    {
      id: 2,
      name: 'Adidas Ultraboost',
      price: 4200000,
      image: 'https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=500',
      category: 'Running',
      description: 'Đế boost êm ái, phù hợp chạy marathon',
      sizes: [38, 39, 40, 41, 42, 43],
      colors: ['White', 'Black', 'Blue'],
      rating: 4.9,
      reviews: 312
    },
    {
      id: 3,
      name: 'Jordan Retro High',
      price: 5500000,
      image: 'https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=500',
      category: 'Basketball',
      description: 'Phong cách cổ điển, chất lượng vượt trội',
      sizes: [39, 40, 41, 42, 43, 44],
      colors: ['Red', 'Black', 'White'],
      rating: 4.7,
      reviews: 189
    },
    {
      id: 4,
      name: 'Puma RS-X',
      price: 2800000,
      image: 'https://images.unsplash.com/photo-1551107696-a4b0c5a0d9a2?w=500',
      category: 'Casual',
      description: 'Thiết kế retro, thoải mái cả ngày',
      sizes: [38, 39, 40, 41, 42],
      colors: ['White', 'Blue', 'Yellow'],
      rating: 4.5,
      reviews: 156
    },
    {
      id: 5,
      name: 'New Balance 574',
      price: 3200000,
      image: 'https://images.unsplash.com/photo-1539185441755-769473a23570?w=500',
      category: 'Casual',
      description: 'Classic style với sự thoải mái tuyệt đối',
      sizes: [38, 39, 40, 41, 42, 43],
      colors: ['Grey', 'Navy', 'Brown'],
      rating: 4.6,
      reviews: 203
    },
    {
      id: 6,
      name: 'Converse Chuck 70',
      price: 1800000,
      image: 'https://images.unsplash.com/photo-1607522370275-f14206abe5d3?w=500',
      category: 'Casual',
      description: 'Biểu tượng thời trang đường phố',
      sizes: [36, 37, 38, 39, 40, 41, 42],
      colors: ['Black', 'White', 'Red'],
      rating: 4.4,
      reviews: 421
    },
    {
      id: 7,
      name: 'Vans Old Skool',
      price: 1650000,
      image: 'https://images.unsplash.com/photo-1525966222134-fcfa99b8ae77?w=500',
      category: 'Casual',
      description: 'Phong cách skate đích thực',
      sizes: [37, 38, 39, 40, 41, 42],
      colors: ['Black/White', 'Navy', 'Red'],
      rating: 4.5,
      reviews: 367
    },
    {
      id: 8,
      name: 'Reebok Classic Leather',
      price: 2100000,
      image: 'https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=500',
      category: 'Casual',
      description: 'Chất liệu da cao cấp, bền bỉ theo thời gian',
      sizes: [38, 39, 40, 41, 42, 43],
      colors: ['White', 'Black', 'Brown'],
      rating: 4.3,
      reviews: 178
    }
  ]
  
  async function fetchProducts() {
    loading.value = true
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000))
    products.value = mockProducts
    loading.value = false
  }
  
  function getProductById(id) {
    return products.value.find(p => p.id === parseInt(id))
  }
  
  function getProductsByCategory(category) {
    if (category === 'All') return products.value
    return products.value.filter(p => p.category === category)
  }
  
  return {
    products,
    loading,
    categories,
    fetchProducts,
    getProductById,
    getProductsByCategory
  }
})

