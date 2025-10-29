<template>
  <nav class="flex items-center justify-between border-t border-gray-200 px-4 py-3 sm:px-6" aria-label="Pagination">
    <div class="hidden sm:block">
      <p class="text-sm text-gray-700">
        Hiển thị
        <span class="font-medium">{{ startItem }}</span>
        đến
        <span class="font-medium">{{ endItem }}</span>
        trong
        <span class="font-medium">{{ totalItems }}</span>
        kết quả
      </p>
    </div>
    <div class="flex flex-1 justify-between sm:justify-end">
      <button
        @click="$emit('prev')"
        :disabled="currentPage === 1"
        class="relative inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
      >
        Trước
      </button>
      
      <div class="hidden md:flex items-center space-x-2 mx-4">
        <button
          v-for="page in pages"
          :key="page"
          @click="$emit('goto', page)"
          :class="[
            'relative inline-flex items-center px-4 py-2 text-sm font-semibold rounded-md transition-all',
            page === currentPage
              ? 'bg-primary-500 text-white shadow-lg'
              : 'bg-white text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50'
          ]"
        >
          {{ page }}
        </button>
      </div>
      
      <button
        @click="$emit('next')"
        :disabled="currentPage === totalPages"
        class="relative ml-3 inline-flex items-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-all"
      >
        Sau
      </button>
    </div>
  </nav>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  currentPage: {
    type: Number,
    required: true
  },
  totalPages: {
    type: Number,
    required: true
  },
  totalItems: {
    type: Number,
    required: true
  },
  itemsPerPage: {
    type: Number,
    default: 10
  }
})

defineEmits(['prev', 'next', 'goto'])

const startItem = computed(() => {
  return (props.currentPage - 1) * props.itemsPerPage + 1
})

const endItem = computed(() => {
  return Math.min(props.currentPage * props.itemsPerPage, props.totalItems)
})

const pages = computed(() => {
  const pages = []
  const maxPages = 5
  let start = Math.max(1, props.currentPage - Math.floor(maxPages / 2))
  let end = Math.min(props.totalPages, start + maxPages - 1)
  
  if (end - start < maxPages - 1) {
    start = Math.max(1, end - maxPages + 1)
  }
  
  for (let i = start; i <= end; i++) {
    pages.push(i)
  }
  
  return pages
})
</script>

