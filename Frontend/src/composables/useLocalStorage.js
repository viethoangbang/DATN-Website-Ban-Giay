import { ref, watch } from 'vue'

export function useLocalStorage(key, defaultValue = null) {
  const stored = localStorage.getItem(key)
  const data = ref(stored ? JSON.parse(stored) : defaultValue)
  
  watch(data, (newValue) => {
    if (newValue === null || newValue === undefined) {
      localStorage.removeItem(key)
    } else {
      localStorage.setItem(key, JSON.stringify(newValue))
    }
  }, { deep: true })
  
  function remove() {
    data.value = null
    localStorage.removeItem(key)
  }
  
  return {
    data,
    remove
  }
}

