import { ref, watch } from 'vue'

export function useDebounce(value, delay = 300) {
  const debouncedValue = ref(value.value)
  let timeout = null
  
  watch(value, (newValue) => {
    if (timeout) clearTimeout(timeout)
    
    timeout = setTimeout(() => {
      debouncedValue.value = newValue
    }, delay)
  })
  
  return debouncedValue
}

export function debounce(fn, delay = 300) {
  let timeout = null
  
  return function (...args) {
    if (timeout) clearTimeout(timeout)
    
    timeout = setTimeout(() => {
      fn.apply(this, args)
    }, delay)
  }
}

