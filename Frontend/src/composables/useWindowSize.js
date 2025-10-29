import { ref, onMounted, onUnmounted } from 'vue'

export function useWindowSize() {
  const width = ref(window.innerWidth)
  const height = ref(window.innerHeight)
  
  function update() {
    width.value = window.innerWidth
    height.value = window.innerHeight
  }
  
  onMounted(() => {
    window.addEventListener('resize', update)
  })
  
  onUnmounted(() => {
    window.removeEventListener('resize', update)
  })
  
  return {
    width,
    height,
    isMobile: () => width.value < 768,
    isTablet: () => width.value >= 768 && width.value < 1024,
    isDesktop: () => width.value >= 1024
  }
}

