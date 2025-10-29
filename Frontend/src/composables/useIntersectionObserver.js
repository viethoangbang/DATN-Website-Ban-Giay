import { ref, onMounted, onUnmounted } from 'vue'

export function useIntersectionObserver(options = {}) {
  const isVisible = ref(false)
  const target = ref(null)
  let observer = null
  
  onMounted(() => {
    observer = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
        isVisible.value = entry.isIntersecting
      })
    }, {
      threshold: options.threshold || 0.1,
      rootMargin: options.rootMargin || '0px'
    })
    
    if (target.value) {
      observer.observe(target.value)
    }
  })
  
  onUnmounted(() => {
    if (observer && target.value) {
      observer.unobserve(target.value)
    }
  })
  
  return {
    target,
    isVisible
  }
}

