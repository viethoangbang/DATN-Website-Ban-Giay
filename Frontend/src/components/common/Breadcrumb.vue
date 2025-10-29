<template>
  <nav class="flex animate-fade-in" aria-label="Breadcrumb">
    <ol class="inline-flex items-center space-x-1 md:space-x-3">
      <li
        v-for="(item, index) in items"
        :key="index"
        class="inline-flex items-center"
      >
        <svg
          v-if="index > 0"
          class="w-3 h-3 text-gray-400 mx-1"
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path
            fill-rule="evenodd"
            d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
            clip-rule="evenodd"
          />
        </svg>
        <router-link
          v-if="item.to"
          :to="item.to"
          :class="[
            'text-sm font-medium transition-colors',
            index === items.length - 1
              ? 'text-primary-600'
              : 'text-gray-700 hover:text-primary-600'
          ]"
        >
          {{ item.label }}
        </router-link>
        <span
          v-else
          class="text-sm font-medium text-gray-500"
        >
          {{ item.label }}
        </span>
      </li>
    </ol>
  </nav>
</template>

<script setup>
defineProps({
  items: {
    type: Array,
    required: true
  }
})
</script>

