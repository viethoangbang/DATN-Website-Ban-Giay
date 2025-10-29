import { ref } from 'vue'

const toasts = ref([])
let toastId = 0

export function useToast() {
  function show(message, type = 'info', duration = 3000) {
    const id = ++toastId
    const toast = {
      id,
      message,
      type,
      visible: true
    }
    
    toasts.value.push(toast)
    
    if (duration > 0) {
      setTimeout(() => {
        remove(id)
      }, duration)
    }
    
    return id
  }
  
  function remove(id) {
    const index = toasts.value.findIndex(t => t.id === id)
    if (index > -1) {
      toasts.value.splice(index, 1)
    }
  }
  
  function success(message, duration) {
    return show(message, 'success', duration)
  }
  
  function error(message, duration) {
    return show(message, 'error', duration)
  }
  
  function warning(message, duration) {
    return show(message, 'warning', duration)
  }
  
  function info(message, duration) {
    return show(message, 'info', duration)
  }
  
  function clear() {
    toasts.value = []
  }
  
  return {
    toasts,
    show,
    remove,
    success,
    error,
    warning,
    info,
    clear
  }
}

