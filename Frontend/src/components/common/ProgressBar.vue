<template>
  <div class="w-full">
    <div v-if="label" class="flex justify-between mb-2">
      <span class="text-sm font-medium text-gray-700">{{ label }}</span>
      <span class="text-sm font-semibold text-gray-900">{{ percentage }}%</span>
    </div>
    <div class="w-full bg-gray-200 rounded-full h-2.5 overflow-hidden">
      <div
        :class="[
          'h-2.5 rounded-full transition-all duration-500 ease-out',
          colorClass
        ]"
        :style="{ width: `${percentage}%` }"
      ></div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  value: {
    type: Number,
    required: true
  },
  max: {
    type: Number,
    default: 100
  },
  label: {
    type: String,
    default: ''
  },
  color: {
    type: String,
    default: 'primary',
    validator: (value) => ['primary', 'success', 'warning', 'danger'].includes(value)
  }
})

const percentage = computed(() => {
  return Math.min(Math.round((props.value / props.max) * 100), 100)
})

const colorClass = computed(() => {
  const colors = {
    primary: 'bg-primary-500',
    success: 'bg-green-500',
    warning: 'bg-yellow-500',
    danger: 'bg-red-500'
  }
  return colors[props.color]
})
</script>

