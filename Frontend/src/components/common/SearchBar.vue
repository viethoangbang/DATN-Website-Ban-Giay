<template>
  <div class="relative">
    <div class="relative">
      <input
        v-model="searchQuery"
        @input="handleInput"
        @focus="showSuggestions = true"
        @blur="handleBlur"
        type="text"
        :placeholder="placeholder"
        class="w-full pl-10 pr-10 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent outline-none transition-all"
      />
      <svg
        class="w-5 h-5 text-gray-400 absolute left-3 top-1/2 transform -translate-y-1/2"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
        />
      </svg>
      <button
        v-if="searchQuery"
        @click="clear"
        class="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600 transition-colors"
      >
        <svg class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>
    
    <!-- Suggestions -->
    <Transition
      enter-active-class="transition duration-200 ease-out"
      enter-from-class="transform scale-95 opacity-0"
      enter-to-class="transform scale-100 opacity-100"
      leave-active-class="transition duration-100 ease-in"
      leave-from-class="transform scale-100 opacity-100"
      leave-to-class="transform scale-95 opacity-0"
    >
      <div
        v-if="showSuggestions && filteredSuggestions.length > 0"
        class="absolute z-50 w-full mt-2 bg-white rounded-lg shadow-lg border border-gray-200 max-h-60 overflow-y-auto"
      >
        <button
          v-for="(suggestion, index) in filteredSuggestions"
          :key="index"
          @mousedown.prevent="selectSuggestion(suggestion)"
          class="w-full px-4 py-3 text-left hover:bg-gray-50 transition-colors flex items-center space-x-3"
        >
          <svg class="w-4 h-4 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
          <span class="text-gray-700">{{ suggestion }}</span>
        </button>
      </div>
    </Transition>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  modelValue: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: 'Tìm kiếm...'
  },
  suggestions: {
    type: Array,
    default: () => []
  }
})

const emit = defineEmits(['update:modelValue', 'search'])

const searchQuery = ref(props.modelValue)
const showSuggestions = ref(false)

const filteredSuggestions = computed(() => {
  if (!searchQuery.value) return []
  return props.suggestions.filter(s =>
    s.toLowerCase().includes(searchQuery.value.toLowerCase())
  ).slice(0, 5)
})

function handleInput() {
  emit('update:modelValue', searchQuery.value)
}

function handleBlur() {
  setTimeout(() => {
    showSuggestions.value = false
  }, 200)
}

function clear() {
  searchQuery.value = ''
  emit('update:modelValue', '')
}

function selectSuggestion(suggestion) {
  searchQuery.value = suggestion
  emit('update:modelValue', suggestion)
  emit('search', suggestion)
  showSuggestions.value = false
}
</script>

